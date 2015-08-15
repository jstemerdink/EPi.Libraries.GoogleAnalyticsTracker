// Copyright© 2015 Jeroen Stemerdink. 
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;
using System.Reflection;
using System.Web;

using EPiServer;
using EPiServer.Core;
using EPiServer.Logging;
using EPiServer.ServiceLocation;

namespace EPi.Libraries.GoogleAnalyticsTracker.Core
{
    /// <summary>
    ///     Class Helpers.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger(typeof(Helpers));

        /// <summary>
        ///     Gets or sets the content loader.
        /// </summary>
        /// <value>The content loader.</value>
        private static Injected<IContentLoader> ContentLoader { get; set; }

        /// <summary>
        ///     Gets the tracking domain.
        /// </summary>
        /// <returns>System.String.</returns>
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

        /// <summary>
        ///     Gets the tracking account.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetTrackingAccount()
        {
            PageData startPage;
            ContentLoader.Service.TryGet(ContentReference.StartPage, out startPage);

            PropertyInfo trackingAccountProperty =
                Enumerable.FirstOrDefault(
                    startPage.GetType().GetProperties().Where(HasAttribute<GoogleAnalyticsAccountAttribute>));

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

        /// <summary>
        ///     Determines whether the specified property information has attribute.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns><c>true</c> if the specified property information has attribute; otherwise, <c>false</c>.</returns>
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
