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

        public static int GetRequestedVersion() {
            var httpContext = HttpContext.Current;

            var value = httpContext.Request.QueryString["version"];
            if (string.IsNullOrEmpty(value)) {
                return -1;
            }

            if ((string)httpContext.Session["CmsAdminMode"] != "yes") {
                return -1;
            }

            int version;
            if (int.TryParse(value, out version)) {
                return version;
            }

            return -1;
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

        public static string VersionHash {
            get { return Version.GetHashCode().ToString(); }
        }

        public static string ApplicationPath {
            get { return _applicationPath ?? (_applicationPath = GetApplicationPath()); }
        }

        private static string GetApplicationPath() {
            var applicationPath = HttpContext.Current.Request.ApplicationPath;

            if (applicationPath.EndsWith("/")) {
                return applicationPath;
            }
            
            return string.Format("{0}/", applicationPath);
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
            httpResponse.Clear();
            httpResponse.StatusCode = statusCode;
            httpResponse.Write("<html><head><style>body{background:#008ED6;font-family:'Open Sans',sans-serif;color:#ffffff;text-align:center;}h1{font-weight:300;font-size:40px;margin-top:150px;}p{font-weight:400;font-size:16px;margin-top:30px;}div{max-width:600px;display:inline-block;text-align:left;}h1:before{background: #ffffff;border-radius: 0.5em;color: #008ED6;content: \"i\";display: block;font-family: serif;font-style: italic;font-weight: bold;height: 1em;line-height: 1em;margin-left: -1.4em;margin-top: 8px;position: absolute;text-align: center;width: 1em;}</style><link href='http://fonts.googleapis.com/css?family=Open+Sans:400,300' rel='stylesheet' type='text/css'></head><body><div><h1>");
            httpResponse.Write(header);
            httpResponse.Write("</h1><p>");
            httpResponse.Write(text);
            httpResponse.Write("</p></div></body></html>");

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
