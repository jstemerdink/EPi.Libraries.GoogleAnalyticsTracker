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
using System.Web.Http.Controllers;

using EPi.Libraries.GoogleAnalyticsTracker.Core;

using GoogleAnalyticsTracker.WebApi;

namespace EPi.Libraries.GoogleAnalyticsTracker.WebApi
{
    /// <summary>
    ///     Class EPiServerActionTrackingAttribute. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class EPiServerActionTrackingAttribute : ActionTrackingAttribute
    {
        /// <summary>
        ///     The tracking account
        /// </summary>
        private readonly string trackingAccount;

        /// <summary>
        ///     The tracking domain
        /// </summary>
        private readonly string trackingDomain;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        public EPiServerActionTrackingAttribute()
            : this(Helpers.GetTrackingAccount(), Helpers.GetTrackingDomain(), null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="trackingAccount">The tracking account.</param>
        /// <param name="trackingDomain">The tracking domain.</param>
        public EPiServerActionTrackingAttribute(string trackingAccount, string trackingDomain)
            : this(trackingAccount, trackingDomain, null, null)
        {
            this.trackingAccount = trackingAccount;
            this.trackingDomain = trackingDomain;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="trackingAccount">The tracking account.</param>
        public EPiServerActionTrackingAttribute(string trackingAccount)
            : this(trackingAccount, null, null, null)
        {
            this.trackingAccount = trackingAccount;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="trackingAccount">The tracking account.</param>
        /// <param name="trackingDomain">The tracking domain.</param>
        /// <param name="actionDescription">The action description.</param>
        /// <param name="actionUrl">The action URL.</param>
        public EPiServerActionTrackingAttribute(
            string trackingAccount,
            string trackingDomain,
            string actionDescription,
            string actionUrl)
        {
            this.trackingAccount = trackingAccount;

            if (string.IsNullOrEmpty(trackingDomain))
            {
                this.trackingDomain = trackingDomain = Helpers.GetTrackingDomain();
            }

            this.Tracker = new Tracker(
                trackingAccount,
                trackingDomain,
                new CookieBasedAnalyticsSession(),
                new AspNetWebApiTrackerEnvironment());
            this.ActionDescription = actionDescription;
            this.ActionUrl = actionUrl;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="tracker">The tracker.</param>
        public EPiServerActionTrackingAttribute(Tracker tracker)
            : this(tracker, action => true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="tracker">The tracker.</param>
        /// <param name="isTrackableAction">The is trackable action.</param>
        public EPiServerActionTrackingAttribute(Tracker tracker, Func<HttpActionContext, bool> isTrackableAction)
        {
            this.Tracker = tracker;
            this.IsTrackableAction = isTrackableAction;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="isTrackableAction">The is trackable action.</param>
        public EPiServerActionTrackingAttribute(Func<HttpActionContext, bool> isTrackableAction)
            : this(Helpers.GetTrackingAccount(), Helpers.GetTrackingDomain(), isTrackableAction)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="trackingAccount">The tracking account.</param>
        /// <param name="trackingDomain">The tracking domain.</param>
        /// <param name="isTrackableAction">The is trackable action.</param>
        public EPiServerActionTrackingAttribute(
            string trackingAccount,
            string trackingDomain,
            Func<HttpActionContext, bool> isTrackableAction)
        {
            this.Tracker = new Tracker(
                trackingAccount,
                trackingDomain,
                new CookieBasedAnalyticsSession(),
                new AspNetWebApiTrackerEnvironment());
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
    }
}
