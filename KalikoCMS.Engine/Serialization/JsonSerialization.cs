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

namespace KalikoCMS.Serialization {
    using System;
    using System.Text.RegularExpressions;
    using Kaliko;
    using fastJSON;

    public class JsonSerialization {
        private const int RandomlyChosenPrimeNumber = 17;
        private const int RandomlyChosenPrimeNumber2 = 31;

        private static readonly JSONParameters GenericParameters = new JSONParameters {UseExtensions = false, UseFastGuid = false, EnableAnonymousTypes = true };
        private static readonly JSONParameters TypedParameters = new JSONParameters { UseExtensions = true, UsingGlobalTypes = true, UseFastGuid = false };

        public static T DeserializeJson<T>(string json) {
            if (string.IsNullOrEmpty(json)) {
                return default(T);
            }

            try {
                var instance = JSON.ToObject<T>(json);
                return instance;
            }
            catch (Exception e) {
                Logger.Write("Could not deserialize " + json + ": " + e.Message, Logger.Severity.Major);
            }

            return default(T);
        }

        public static object DeserializeTypedJson(string json) {
            if (string.IsNullOrEmpty(json)) {
                return default(object);
            }

            try {
                var instance = JSON.ToObject(json, TypedParameters);
                return instance;
            }
            catch (Exception e) {
                Logger.Write("Could not deserialize " + json + ": " + e.Message, Logger.Severity.Major);
            }

            return default(object);
        }

        public static string SerializeJson(object instance) {
            return JSON.ToJSON(instance, GenericParameters);
        }

        public static string SerializeJson(object instance, JSONParameters parameters) {
            return JSON.ToJSON(instance, parameters);
        }

        public static string SerializeTypedJson(object instance) {
            var json = JSON.ToJSON(instance, TypedParameters);

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
