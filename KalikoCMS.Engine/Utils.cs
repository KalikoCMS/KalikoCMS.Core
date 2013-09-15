#region License and copyright notice
/* 
 * Kaliko Content Management System
 * 
 * Copyright (c) Fredrik Schultz and Contributors
 * 
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 * 
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
 * EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
 */
#endregion

namespace KalikoCMS {
    using System;
    using System.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using KalikoCMS.Configuration;
    using KalikoCMS.Extensions;

    // TODO: Move to more specific classes
    public static class Utils {
        private static string _version;

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
            return string.IsNullOrEmpty(setting) ? defaultValue : setting;
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
    }


}
