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
    using System.Text.RegularExpressions;
    using System.Web;
    using Collections;

    public class PageNameBuilder {
        private static List<KeyValuePair<string, string>> _letterTranslation;

        private static IEnumerable<KeyValuePair<string, string>> LetterTranslation {
            get {
                return _letterTranslation ?? (_letterTranslation = SetupLetterTranslation());
            }
        }

        public static string PageNameToUrl(string pageName, Guid parentId) {
            var baseUrl = pageName.ToLower();

            baseUrl = HandleWhiteSpaces(baseUrl);
            baseUrl = HandleNonAsciiLetters(baseUrl);
            baseUrl = RemoveNonValidCharacters(baseUrl);

            var finalUrl = GetUniqueUrl(parentId, baseUrl);

            return finalUrl;
        }

        internal static string GetUniqueUrl(Guid parentId, string baseUrl) {
            PageCollection siblings = PageFactory.GetChildrenForPage(parentId, PublishState.All);
            var siblingNames = new List<string>();

            foreach (CmsPage child in siblings) {
                siblingNames.Add(child.UrlSegment);
            }

            string suggestedUrl = baseUrl;
            int counter = 1;

            while (siblingNames.Contains(suggestedUrl)) {
                counter++;
                suggestedUrl = string.Format("{0}~{1}", baseUrl, counter);
            }

            return suggestedUrl;
        }

        private static string RemoveNonValidCharacters(string urlSuggestion) {
            return HttpUtility.UrlEncode(Regex.Replace(urlSuggestion, "[^a-zA-Z0-9._-]", ""));
        }

        private static string HandleNonAsciiLetters(string urlSuggestion) {
            foreach (var translation in LetterTranslation) {
                urlSuggestion = urlSuggestion.Replace(translation.Key, translation.Value);
            }

            return urlSuggestion;
        }

        private static string HandleWhiteSpaces(string urlSuggestion) {
            return urlSuggestion.Replace(" ", "-");
        }

        private static List<KeyValuePair<string, string>> SetupLetterTranslation() {
            var letterTranslation = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("á", "a"),
                new KeyValuePair<string, string>("à", "a"),
                new KeyValuePair<string, string>("å", "a"),
                new KeyValuePair<string, string>("ã", "a"),
                new KeyValuePair<string, string>("ä", "a"),
                new KeyValuePair<string, string>("ç", "c"),
                new KeyValuePair<string, string>("è", "e"),
                new KeyValuePair<string, string>("é", "e"),
                new KeyValuePair<string, string>("ü", "u"),
                new KeyValuePair<string, string>("ö", "o"),
                new KeyValuePair<string, string>("ß", "ss")
            };

            return letterTranslation;
        }
    }
}
