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

namespace KalikoCMS.Core {
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web;
    using Configuration;
    using Data.Entities;

    public partial class Language {
        private static readonly List<Language> LanguageList;
        private static readonly string DefaultLanguageValue;

        public int LanguageId { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; }

        static Language() {
            LanguageList = Data.SiteLanguageData.GetLanguages();

            if (LanguageList.Count == 0) {
                throw new Exception("No languages defined in database.");
            }

            DefaultLanguageValue = ReadDefaultLanguage();
        }
        

        public static Collection<Language> Languages {
            get { return new Collection<Language>(LanguageList); }
        }


        public static string CurrentLanguage {
            get { return Utils.GetItem("cms_lang"); }
            set {
                Utils.SetCookie("cms_lang", value);
                Utils.StoreItem("cms_lang", value);
            }
        }


        public static string DefaultLanguage {
            get { return DefaultLanguageValue; }
        }


        public static int CurrentLanguageId {
            get {
                Language language = LanguageList.Find(l => l.ShortName == CurrentLanguage);

                if (language != null) {
                    return language.LanguageId;
                }
                else {
                    return LanguageList[0].LanguageId;
                }
            }
        }


        public static int GetLanguageId(string shortName) {
            Language language = LanguageList.Find(l => l.ShortName == shortName);

            return language != null ? language.LanguageId : 0;
        }


        public static bool IsValidLanguage(string shortName) {
            return LanguageList.Exists(l => l.ShortName == shortName);
        }


        private static string ReadDefaultLanguage() {
            string language = GetDefaultLanguageFromConfig();

            if (string.IsNullOrEmpty(language)) {
                language = GetFirstLanguageAsDefault();
            }

            return language;
        }


        private static string GetFirstLanguageAsDefault() {
            if (LanguageList.Count > 0) {
                return LanguageList[0].ShortName;
            }
            else {
                return null;
            }
        }


        private static string GetDefaultLanguageFromConfig() {
            SiteHostConfiguration siteHostConfiguration = SiteHostConfiguration.ConfigurationSection;

            if (siteHostConfiguration != null) {
                return SiteHostConfiguration.GetLanguage("*");
            }
            else {
                return null;
            }
        }


        public static string ReadLanguageFromHostAddress() {
            string host = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
            int indexOfLastDot = host.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);

            if (indexOfLastDot != -1) {
                host = host.Substring(indexOfLastDot).ToLowerInvariant();
            }
            else {
                host = "*";
            }

            return SiteHostConfiguration.GetLanguage(host);
        }


        public static void AttachLanguageToHttpContext() {
            string languageFromCookie = Utils.GetCookie("cms_lang");

            Utils.StoreItem("cms_lang", languageFromCookie ?? DefaultLanguage);
        }
    }
}
