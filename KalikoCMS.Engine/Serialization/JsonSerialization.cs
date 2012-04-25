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
    using Kaliko;
    using fastJSON;

    public class JsonSerialization {
        private const int RandomlyChosenPrimeNumber = 17;
        private const int RandomlyChosenPrimeNumber2 = 31;

        private static readonly JSON JsonSerializer = GetSerializer();

        private static JSON GetSerializer() {
            JSON instance = JSON.Instance;
            return instance;
        }

        public static T DeserializeJson<T>(string json) {
            if (String.IsNullOrEmpty(json)) {
                return default(T);
            }

            try {
                T instance = JsonSerializer.ToObject<T>(json);
                //T instance = (T)JsonSerializer.Parse(json);
                return instance;
            }
            catch (Exception e) {
                Logger.Write("Could not deserialize " + json + ": " + e.Message, Logger.Severity.Major);
            }

            return default(T);
        }

        public static string SerializeJson(object instance) {
            string json = JsonSerializer.ToJSON(instance, false);

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
