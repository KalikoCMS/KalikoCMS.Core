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

namespace KalikoCMS.Extensions {
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtension {
        private static readonly Regex GuidMatchPattern = new Regex(
                "^[A-Fa-f0-9]{32}$|" + 
                "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" +
                "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");


        /* Based on code from http://geekswithblogs.net/colinbo/archive/2006/01/18/66307.aspx */
        public static bool TryParseGuid(this string value, out Guid result) {
            if (!string.IsNullOrEmpty(value) && GuidMatchPattern.IsMatch(value)) {
                result = new Guid(value);
                return true;
            }
            else {
                result = Guid.Empty;
                return false;
            }
        }

        public static string LimitCharacters(this string text, int length) {
            if (string.IsNullOrEmpty(text)) {
                return string.Empty;
            }

            if (text.Length <= length) {
                return text;
            }

            var delimiters = new[] {' ', '.', ',', ':', ';'};
            int index = text.LastIndexOfAny(delimiters, length - 3);

            if (index > (length/2)) {
                return text.Substring(0, index) + "...";
            }
            else {
                return text.Substring(0, length - 3) + "...";
            }
        }

        public static string StripHtml(this string htmlString) {
            const string pattern = @"<(.|\n)*?>";

            return Regex.Replace(htmlString, pattern, string.Empty);
        }
    }
}
