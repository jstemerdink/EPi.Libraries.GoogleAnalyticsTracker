using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using EPiServer;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace EPi.Libraries.GoogleAnalyticsTracker.Core
{
    public static class Helpers
    {
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(Helpers));

        private static Injected<IContentLoader> ContentLoader { get; set; }

        public static string GetTrackingDomain()
        {
            string trackingDomain = string.Empty;

            if (HttpContext.Current == null)
            {
                return trackingDomain;
            }

            try
            {
                trackingDomain = HttpContext.Current.Request.Url.Host;
            }
            catch (HttpException httpException)
            {
                Logger.Error("[GoogleAnalyticsTracker] Error getting http request.", httpException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                Logger.Error("[GoogleAnalyticsTracker] Error getting host from Url.", invalidOperationException);
            }

            return trackingDomain;
        }

        public static string GetTrackingAccount()
        {
            PageData startPage;
            ContentLoader.Service.TryGet(ContentReference.StartPage, out startPage);

            PropertyInfo trackingAccountProperty =
                startPage.GetType().GetProperties().Where(HasAttribute<GoogleAnalyticsAccountAttribute>).FirstOrDefault();

            if (trackingAccountProperty == null)
            {
                return string.Empty;
            }

            if (trackingAccountProperty.PropertyType != typeof(string))
            {
                return string.Empty;
            }

            try
            {
                return startPage[trackingAccountProperty.Name] as string;
            }
            catch (EPiServerException epiServerException)
            {
                Logger.Error("[GoogleAnalyticsTracker] Error getting tracking account value.", epiServerException);
            }

            return string.Empty;
        }

        private static bool HasAttribute<T>(PropertyInfo propertyInfo) where T : Attribute
        {
            T attr = default(T);

            try
            {
                attr = (T)Attribute.GetCustomAttribute(propertyInfo, typeof(T));
            }
            catch (Exception exception)
            {
                Logger.Error("[GoogleAnalyticsTracker] Error getting custom attribute.", exception);
            }

            return attr != null;
        }
    }
}