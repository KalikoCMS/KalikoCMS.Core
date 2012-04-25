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

namespace KalikoCMS.Core {
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Xml;
    using Kaliko;

    public partial class Language {
        private static readonly Hashtable TranslationData = new Hashtable();

        public static string Translate(string key) {
            if (string.IsNullOrEmpty(key)) {
                ArgumentException argumentException = new ArgumentException("Argument to Translate(string key) should not be null or empty.");
                Logger.Write(argumentException);
                throw argumentException;
            }

            Hashtable lookupTable = GetLocalizedData();

            if(!lookupTable.Contains(key.ToUpperInvariant())) {
                Logger.Write(string.Format("Could not locate the text for the key: {0}. The table contained {1} elements", key, lookupTable.Count), Logger.Severity.Minor);
                return string.Empty;
            }

            return lookupTable[key].ToString();
        }

        private static Hashtable GetLocalizedData() {
            string currentLanguage = CurrentLanguage.ToUpperInvariant();
            
            if(TranslationData.Count == 0) {
                Logger.Write("TranslationData was empty, trying to re-read the xml.", Logger.Severity.Info);
                ReadLanguages();
            }
            if(TranslationData.Contains(currentLanguage)) {
                return (Hashtable)TranslationData[currentLanguage];
            }
            if(TranslationData.Contains(DefaultLanguageValue)) {
                return (Hashtable)TranslationData[DefaultLanguageValue];
            }

            return new Hashtable(0);
        }

        private static void ReadLanguages() {
            string folder = HttpContext.Current.Server.MapPath("/lang/");

            if(!Directory.Exists(folder)) {
                Logger.Write("Could not find the lang xml folder: " + folder, Logger.Severity.Major);
                return;
            }

            string[] xmlFilenames = Directory.GetFiles(folder, "*.xml");

            foreach(string file in xmlFilenames) {
                try {
                    ParseXmlFile(file);
                }
                catch (Exception ex) {
                    Logger.Write(string.Format("Error parsing xml-file '{0}'! {1}", file, ex.Message), Logger.Severity.Critical);
                }
            }

        }

        private static void ParseXmlFile(string file) {
            string languageCode;
            var lookupTable = GetLookupTableFromFile(file, out languageCode);

            if (!string.IsNullOrEmpty(languageCode)) {
                languageCode = languageCode.ToUpperInvariant();

                if (TranslationData.ContainsKey(languageCode)) {
                    AddValuesToExistingLookupTable(lookupTable, languageCode);
                }
                else {
                    TranslationData.Add(languageCode, lookupTable);
                }
            }
            else {
                Logger.Write(string.Format("Error parsing xml-file '{0}'!  Couldn't find language id in file.", file), Logger.Severity.Critical);
            }
        }

        private static Hashtable GetLookupTableFromFile(string file, out string languageCode) {
            XmlTextReader xmlTextReader = new XmlTextReader(file);
            Hashtable lookupTable = new Hashtable();
            string path = string.Empty;
            languageCode = string.Empty;

            while (xmlTextReader.Read()) {
                switch (xmlTextReader.NodeType) {
                    case XmlNodeType.Element: // Start element
                        if ((xmlTextReader.Name == "language"))
                            languageCode = xmlTextReader.GetAttribute("id");
                        else
                            path += string.Format(CultureInfo.InvariantCulture, "{0}/", xmlTextReader.Name.ToUpperInvariant());
                        break;
                    case XmlNodeType.EndElement: //End element.
                        path = path.Substring(0, path.LastIndexOf('/', path.Length - 2) + 1);
                        break;
                    case XmlNodeType.Text: // Content
                        lookupTable.Add(path.TrimEnd('/'), xmlTextReader.Value);
                        break;
                }
            }

            xmlTextReader.Close();
            return lookupTable;
        }

        private static void AddValuesToExistingLookupTable(Hashtable lookupTable, string languageCode) {
            Hashtable existingTable = (Hashtable)TranslationData[languageCode];

            foreach (DictionaryEntry dictionaryEntry in lookupTable) {
                existingTable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
            }
        }
    }
}
