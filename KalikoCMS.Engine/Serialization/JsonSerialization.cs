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

namespace KalikoCMS.Serialization {
    using System;
    using System.Text.RegularExpressions;
    using Kaliko;
    using fastJSON;

    public class JsonSerialization {
        private const int RandomlyChosenPrimeNumber = 17;
        private const int RandomlyChosenPrimeNumber2 = 31;

        private static readonly JSON JsonSerializer = GetSerializer();
        private static readonly JSONParameters GenericParameters = new JSONParameters {UseExtensions = false};
        private static readonly JSONParameters TypedParameters = new JSONParameters { UseExtensions = true, UsingGlobalTypes = true };

        private static JSON GetSerializer() {
            return JSON.Instance;
        }

        public static T DeserializeJson<T>(string json) {
            if (String.IsNullOrEmpty(json)) {
                return default(T);
            }

            try {
                var instance = JsonSerializer.ToObject<T>(json);
                return instance;
            }
            catch (Exception e) {
                Logger.Write("Could not deserialize " + json + ": " + e.Message, Logger.Severity.Major);
            }

            return default(T);
        }

        public static object DeserializeTypedJson(string json) {
            if (String.IsNullOrEmpty(json)) {
                return default(object);
            }

            try {
                var instance = JsonSerializer.ToObject(json);
                return instance;
            }
            catch (Exception e) {
                Logger.Write("Could not deserialize " + json + ": " + e.Message, Logger.Severity.Major);
            }

            return default(object);
        }

        public static string SerializeJson(object instance) {
            return JsonSerializer.ToJSON(instance, GenericParameters);
        }

        public static string SerializeTypedJson(object instance) {
            var json = JsonSerializer.ToJSON(instance, TypedParameters);

            json = OptimizeJsonTypes(json);

            return json;
        }

        private static string OptimizeJsonTypes(string json) {
            var match = Regex.Match(json, ".*?\\\"\\$types\\\":\\{.*?\\}");
            if (match.Success) {
                var types = match.Groups[0].Value;
                var optimizedTypes = Regex.Replace(types, @", Version=\d+.\d+.\d+.\d+", string.Empty);
                optimizedTypes = Regex.Replace(optimizedTypes, @", Culture=\w+", string.Empty);
                optimizedTypes = Regex.Replace(optimizedTypes, @", PublicKeyToken=\w+", string.Empty);
                json = json.Replace(types, optimizedTypes);
            }
            return json;
        }

        public static int GetNewHash() {
            return RandomlyChosenPrimeNumber;
        }

        public static int CombineHashCode(int hash, object o) {
            hash *= RandomlyChosenPrimeNumber2;

            if (o != null) {
                hash += o.GetHashCode();
            }

            return hash;
        }
    }
}
