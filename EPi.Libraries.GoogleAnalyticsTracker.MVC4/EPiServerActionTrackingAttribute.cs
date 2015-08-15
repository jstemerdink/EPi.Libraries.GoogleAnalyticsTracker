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
using System.Web.Mvc;

using EPi.Libraries.GoogleAnalyticsTracker.Core;

using GoogleAnalyticsTracker.Mvc4;

namespace EPi.Libraries.GoogleAnalyticsTracker.Mvc4
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class EPiServerActionTrackingAttribute : ActionTrackingAttribute
    {
        private readonly string trackingAccount;

        private readonly string trackingDomain;

        public EPiServerActionTrackingAttribute()
            : this(null, null, null, null)
        {
        }

        public EPiServerActionTrackingAttribute(string trackingAccount, string trackingDomain)
            : this(trackingAccount, trackingDomain, null, null)
        {
            this.trackingAccount = trackingAccount;
            this.trackingDomain = trackingDomain;
        }

        public EPiServerActionTrackingAttribute(string trackingAccount)
            : this(trackingAccount, null, null, null)
        {
            this.trackingAccount = trackingAccount;
        }

        public EPiServerActionTrackingAttribute(
            string trackingAccount,
            string trackingDomain,
            string actionDescription,
            string actionUrl)
        {
            if (string.IsNullOrWhiteSpace(trackingAccount))
            {
                this.trackingAccount = trackingAccount = Helpers.GetTrackingAccount();
            }

            if (string.IsNullOrEmpty(trackingDomain))
            {
                this.trackingDomain = trackingDomain = Helpers.GetTrackingDomain();
            }

            this.Tracker = new Tracker(
                trackingAccount,
                trackingDomain,
                new CookieBasedAnalyticsSession(),
                new AspNetMvc4TrackerEnvironment());
            this.ActionDescription = actionDescription;
            this.ActionUrl = actionUrl;
        }

        public EPiServerActionTrackingAttribute(Tracker tracker)
            : this(tracker, action => true)
        {
        }

        public EPiServerActionTrackingAttribute(Tracker tracker, Func<ActionDescriptor, bool> isTrackableAction)
        {
            this.Tracker = tracker;
            this.IsTrackableAction = isTrackableAction;
        }

        public EPiServerActionTrackingAttribute(Func<ActionDescriptor, bool> isTrackableAction)
        {
            this.trackingAccount = Helpers.GetTrackingAccount();
            this.trackingDomain = Helpers.GetTrackingDomain();

            this.Tracker = new Tracker(
                this.trackingAccount,
                this.trackingDomain,
                new CookieBasedAnalyticsSession(),
                new AspNetMvc4TrackerEnvironment());
            this.IsTrackableAction = isTrackableAction;
        }

        /// <summary>
        ///     Gets the tracking account.
        /// </summary>
        /// <value>The tracking account.</value>
        public string TrackingAccount
        {
            get
            {
                return this.trackingAccount;
            }
        }

        /// <summary>
        ///     Gets the tracking domain.
        /// </summary>
        /// <value>The tracking domain.</value>
        public string TrackingDomain
        {
            get
            {
                return this.trackingDomain;
            }
        }

        public static void RegisterGlobalFilter(string trackingDomain)
        {
            string trackingAccount = Helpers.GetTrackingAccount();
            GlobalFilters.Filters.Add(new ActionTrackingAttribute(trackingAccount, trackingDomain));
        }

        public static void RegisterGlobalFilter()
        {
            string trackingAccount = Helpers.GetTrackingAccount();
            string trackingDomain = Helpers.GetTrackingDomain();

            GlobalFilters.Filters.Add(new ActionTrackingAttribute(trackingAccount, trackingDomain));
        }
    }
}
