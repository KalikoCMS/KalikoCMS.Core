#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace KalikoCMS {
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using Configuration;
    using Extensions;
    using Kaliko;

    // TODO: Move to more specific classes
    public static class Utils {
        private static string _version;
        private static string _applicationPath;
        private static string _domainApplicationPath;
        private static string _serverDomain;

        public static bool IsNullableType(Type type) {
            bool result = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
            return result;
        }



        public static string GetAppSetting(string key) {
            return ConfigurationManager.AppSettings.Get(key);
        }

        public static string GetAppSetting(string key, string defaultValue) {
            return ConfigurationManager.AppSettings.Get(key) ?? defaultValue;
        }

        public static string GetCookie(string key) {
            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[key];
            return httpCookie != null ? httpCookie.Value : null;
        }

        public static Guid GetCurrentPageId() {
            string value = GetFormOrQueryString("id");
            Guid pageId;

            if(value.TryParseGuid(out pageId)) {
                return pageId;
            }
            else {
                return SiteSettings.Instance.StartPageId;
            }
        }

        public static string GetFormOrQueryString(string value) {
            HttpRequest httpRequest = HttpContext.Current.Request;
            return httpRequest.QueryString["id"] ?? httpRequest.Form["id"];
        }

        public static string GetItem(string key) {
            return (string)HttpContext.Current.Items[key];
        }

        public static Stream GetResourceStream(string resourceName) {
            Assembly assembly = Assembly.GetCallingAssembly();

            return assembly.GetManifestResourceStream(resourceName);
        }

        public static Stream GetResourceStream(Assembly assembly, string resourceName) {
            return assembly.GetManifestResourceStream(resourceName);
        }

        public static string GetResourceText(string resourceName) {
            Assembly assembly = Assembly.GetCallingAssembly();
            Stream resourceStream = GetResourceStream(assembly, resourceName); //assembly.GetName().Name + "." +
            StreamReader streamReader = new StreamReader(resourceStream);
            string value = streamReader.ReadToEnd();
            streamReader.Close();

            return value;
        }

        public static T LazyInit<T>(T field, Func<T> getMethod) {
            if (field == null) {
                field = getMethod();
            }

            return field;
        }


        public static String ReadSetting(String key, String defaultValue) {
            string setting = ConfigurationManager.AppSettings[key];
            return String.IsNullOrEmpty(setting) ? defaultValue : setting;
        }

        public static void SetCookie(string key, string value) {
            HttpCookie httpCookie = new HttpCookie(key, value) {Expires = DateTime.Now.AddYears(2)};
            HttpContext.Current.Response.Cookies.Add(httpCookie);
        }

        public static void StoreItem(string key, string value) {
            HttpContext.Current.Items[key] = value;
        }

        public static void StoreItem(string key, bool value) {
            HttpContext.Current.Items[key] = value;
        }

        public static string Version {
            get { return _version ?? (_version = Assembly.GetExecutingAssembly().GetName().Version.ToString()); }
        }

        public static string ApplicationPath {
            get { return _applicationPath ?? (_applicationPath = HttpContext.Current.Request.ApplicationPath); }
        }

        public static string DomainApplicationPath {
            get { return _domainApplicationPath ?? (_domainApplicationPath = GetDomainApplicationPath()); }
        }

        public static string ServerDomain {
            get { return _serverDomain ?? (_serverDomain = HttpContext.Current.Request.Url.Host); }
        }

        private static string GetDomainApplicationPath() {
            var protocol = HttpContext.Current.Request.IsSecureConnection ? "https" : "http";

            if (ApplicationPath == "/") {
                return string.Format("{0}://{1}", protocol, HttpContext.Current.Request.Url.Host);
            }
            else {
                return string.Format("{0}://{1}{2}", protocol, HttpContext.Current.Request.Url.Host, ApplicationPath);
            }
        }

        public static void RenderSimplePage(HttpResponse httpResponse, string header, string text, int statusCode = 200) {
            httpResponse.StatusCode = statusCode;
            httpResponse.Write("<html><head><style>body{background:#444444 url(data:image/jpeg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAABaAAD/7gAOQWRvYmUAZMAAAAAB/9sAhAABAQEBAQEBAQEBAgEBAQICAgEBAgICAgICAgICAwIDAwMDAgMDBAQEBAQDBQUFBQUFBwcHBwcICAgICAgICAgIAQEBAQICAgUDAwUHBQQFBwgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAj/wAARCABRAFEDAREAAhEBAxEB/8QAfAAAAwEBAQEAAAAAAAAAAAAAAQIDAAQFCQEBAAAAAAAAAAAAAAAAAAAAABAAAgEDAgMFAgkKBwEAAAAAAQIRIRIDADFBURNhcZEiMrEE8IGhwfFC0iNT0eFykrIzc5OzFFJiosKjNFRkEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwD47C5Z6FrZEuGTywrE1iIUzIGgq4jGoAWwz95tIB4GDvuDEeGg5um2MZIYstzUAgSBJHea/m0DDCVQOpbGGAAJbcMwBBtDcfo4aDohkVlsAuYKXNQxIEUAJnjWmglkdiSfKGqRlkSQTWSY3tHHt5aAEqAzAgLCHIIMFyOFu8n6NAKMIFoGNTaxkAG2GMqRABk/RoCCCCXNrIVABg2zvsF+Ks6BySGIRCcXlFpgQSKybeylY0EP7TJ+Hi/l5Ps6CitOVclUZlVWq0sAADUbbUM/mDMGAyKhOM3C4mgkN5TyFTG+guIQZDJa7jJMuQSa1kihr2xoOZHxrkPWx2mZLi2JRQRBMUM18dAwLvjLuGWCKm55BYAKdyJJp27c9BUOuRVLrfcCbiVYEEQ0ma7D4DQTgDIw/cwSCZEgQOIkECOXDv0AlS3ncMONzQoAEKBSteMT3aBkCs/VLBJUdTIST6juAJ37CNvEJ5MhudoVWZmJyABgxJpQTAgkzzPKdAbP/vw+DaB0a705rVUefYAwK+kDao2+XQMGx4cZx3gsGY5DuJBIDEQO+dBJ6KwgvcokqditxI9vs0D5UuRASH6s+UiQokmII7e7v0BhgYLFGcABRQhronaZNY7R36BS5xJleqipuaoESLaciPy9oHIq1x+twYUEhgwKxAW/bbhEnQZMtoYMZCMRIQksSxWSTsYgbz8ddAgVajLACzVQSQwgAggjkRvoGXpkBjkaSrBcZYBSopdsTwnfjoE/tvdvxG/XX7Ogr1Q6hFa0JUwALgCTAiePCK+GgQrczBFN8gK/lANnxEVO23joHZ3BJUgDH6huIgAz2xoN0wqWtVFg5CBFxJ4RUHv7O3QBizF+iynG8sr+a2kJAJApufm0ADNdF9p85ADqsNM0gCgB8NBhjzEhmXq2LclS01YxQ02gQD8k6BDGSw5FLAWEGPUxgAkGO3bQBEIcoMgJMlMhUwblmpIB3A5cPjCxaEZyQiWizMG9IAI3ZoqIoR3c9A/Xy/hYvFPtaBBjEOpI84IbKVFskWtQUiPZoFVjGImVz1vKy4kzEhpoIIPs0BytjGMZOj1bjcFA4q0EQprvMDuOgANzi0KMjBUC0hwRWvM9op8egZXB85UFT+8GQNaAvmoZHDeR46BMpRklC4yAMAWhQVZbRwIqSSfm30D5lbGoPTbKcikAlmuBUgSK7nt56CcISSpVC9nTrMTbWpPP4cQZmZr8SmZMjKsC1Q8FvKQYI7OO+g15S0ZVVHc1Hmja0FiDvDHhoB0M344/V0Bl0dkjpIpMASVY2R5pExB7qaB28gCEWlZgVBCrwkA0kExBPPQB0uZUQichIZiI4EiIjgaH26CYDPevTKAKFbGNzAkG4AkyOztpoGCqqnE9ESQpkEgFuLcIDCKmfDQKzvjDwwDqyikFBAImpoJNa8K6Ak2qy48vVxkQCQLaG2AHECTyPxRGgAVccjIxdchoJIFxCwIkmYr36BHYqzQli2C1VHqqCZtHpPH20Gg6Jm4Q1xBuxzFCIaRzrv3aCf8Ace6/i/8AEPs6BPvkJUZCoUvJDbSDyAMTueHjoLLkt6ljXmIKrBgAyRavDY6ABw6tZ9cE4yQSbYtAgU3P0aCYW25nrAIqYEhwAACABFNBS3LarkguGmpGxFswvKk/CQKouVSJVv8APLE1BXcxBPI/QE2DjKpY3sVgEgiCwNADUCOZ+aQc5FVsjbYT5Vx28mkR5lpaOegLGQBcXVjLvEVEUMQfq/mrQE6bIrwD1W9AJ73gXA7bfCoS6nvn/oHgftaByxdrWa1klS1LoMDgDsSBO/ZzB3YMoyOoCkgWi4MwAmZW7jPKeWgKrkIwu2S1nNwkkxIBEiBBNOB8Y0GQBFKMSVYgpIBHmIJilaCkcfkBSXQAlbypKXXG0LSOMkGeVPDQUCOFAVTkBX0mTUksPhTnoJ4lbEWKBigUWlrletJptvGgKsuJUGV7r74FfLUnmT9UGn5dAWbJk+7yteUoSZasgmgkjfcdldBQpiYl63kqSQZD+WgGxg7U47aCf3/4n+k/a0D9TGeT2tGUMxYiDJk1qJ4H59BL7zHKsVcIGIm4kTBAEgRx28N9A7NaSwzXPkYFgwESzRFGApxPZoMu8M4zKSBjMAglZpItHCOwaCb5OmwGWEusFQoB2JNYNCsbcNAVL3OxEgWhATcIiTImux8NBgqIDkZBeAGyp/jDR5hI4kcp2poKkOVxdPKVxk2tlbpxQ2eWtd9zTQaApxgtKwtrMSakEqRM8Rz0AfpUxkVxSWyQogldlI4VHyRz0CW+7/8AmxfzMn5dBsaqqXYhaBAmWB3ExFdogceGw0C5AWXIypIFQqBpBWQCJClgD3eFNAjkBma0YgBAIZ7SErcdge7w4aCrRk8+NlDKplmHq8sUJmsDvn5QwUKqs69JoJNQQQtLjBApAk+HYACPGNlWjVUgQYlpEGDEmmgORiqhDkPTRZOJVExtPm49+8cNBj5encVSCbjUC3Yi3mS0T2aDYziEQgdk8syDcwlSDSoHHunsIMIYUZcha0Y4DA2qZmCYoe7tnfQc9vvXI/rjQdf1W/iH2YtBPL6sX6J/p+76B/edve+9vZoOXJ9X9L/adA7/ALt+8/1V0HWfVi/SP7A0HJm9Xuf8R/a2gY/9v48H7TaBvffTj/hZP2smgUf9fH/EH9Q6D0dB/9k=);font-family:'Tahoma',arial,sans-serif;color:#ffffff;text-align:center;}h1{font-weight:400;font-size:56px;margin-top:150px;text-shadow:1px 1px 4px rgba(0,0,0,0.8);}p{font-weight:400;font-size:20px;margin-top:40px;text-shadow:1px 1px 1px rgba(0,0,0,0.6);}</style></head><body><h1>");
            httpResponse.Write(header);
            httpResponse.Write("</h1><p>");
            httpResponse.Write(text);
            httpResponse.Write("</p></body></html>");

            try {
                httpResponse.End();
            }
            catch (System.Threading.ThreadAbortException) {
                // No problem
            }
        }

        public static void Throw<T>(string message, Logger.Severity severity = Logger.Severity.Major) where T : Exception {
            var exception = (T)Activator.CreateInstance(typeof(T), message);
            Logger.Write(exception, severity);
            throw exception;
        }
    }


}
