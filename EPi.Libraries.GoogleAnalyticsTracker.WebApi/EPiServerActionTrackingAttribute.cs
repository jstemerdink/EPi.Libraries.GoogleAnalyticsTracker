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

using EPi.Libraries.GoogleAnalyticsTracker.Core;

using GoogleAnalyticsTracker.WebApi;

namespace EPi.Libraries.GoogleAnalyticsTracker.WebApi
{
    /// <summary>
    ///     Class EPiServerActionTrackingAttribute. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
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
            : this(null, null, null, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        public EPiServerActionTrackingAttribute(string actionDescription)
            : this(null, null, actionDescription, null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EPiServerActionTrackingAttribute" /> class.
        /// </summary>
        /// <param name="trackingAccount">The tracking account.</param>
        /// <param name="trackingDomain">The tracking domain.</param>
        /// <param name="actionDescription">The action description.</param>
        /// <param name="actionUrl">The action URL.</param>
        private EPiServerActionTrackingAttribute(
            string trackingAccount,
            string trackingDomain,
            string actionDescription,
            string actionUrl)
        {
            if (string.IsNullOrEmpty(trackingAccount))
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
                new AspNetWebApiTrackerEnvironment());
            this.ActionDescription = actionDescription;
            this.ActionUrl = actionUrl;
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
