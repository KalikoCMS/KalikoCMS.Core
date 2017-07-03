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
    using Kaliko;
    using Newtonsoft.Json;

    public class JsonSerialization {
        #region Private members

        private static readonly JsonSerializerSettings AjaxSerializerSettings;
        private static readonly JsonSerializerSettings StandardSerializerSettings;
        private static readonly JsonSerializerSettings TypedJsonSerializerSettings;
        private const int RandomlyChosenPrimeNumber1 = 17;
        private const int RandomlyChosenPrimeNumber2 = 31;

        #endregion

        #region Constructors

        static JsonSerialization() {
            AjaxSerializerSettings = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new WritablePropertiesOnlyResolver()
            };

            StandardSerializerSettings = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new WritablePropertiesOnlyResolver()
            };

            TypedJsonSerializerSettings = new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.Objects,
                SerializationBinder = new PropertyTypeBinder(),
                ContractResolver = new WritablePropertiesOnlyResolver()
            };
        }

        #endregion

        #region Public functions

        #region Deserialization

        public static T DeserializeJson<T>(string json) {
            if (string.IsNullOrEmpty(json)) {
                return default(T);
            }

            try {
                return JsonConvert.DeserializeObject<T>(json, StandardSerializerSettings);
            }
            catch (Exception exception) {
                Logger.Write($"Could not deserialize {json}: {exception.Message}", Logger.Severity.Major);
            }

            return default(T);
        }

        public static object DeserializeTypedJson(string json) {
            if (string.IsNullOrEmpty(json)) {
                return default(object);
            }

            if (json.StartsWith("{\"$types\":")) {
                json = LegacyJsonConverter.Convert(json);
            }

            try {
                return JsonConvert.DeserializeObject(json, TypedJsonSerializerSettings);
            }
            catch (Exception e) {
                Logger.Write($"Could not deserialize {json}: {e.Message}", Logger.Severity.Major);
            }

            return default(object);
        }

        #endregion

        #region Serialization

        public static string SerializeJson(object instance) {
            return JsonConvert.SerializeObject(instance, StandardSerializerSettings);
        }

        public static string SerializeJsonForAjax(object instance) {
            return JsonConvert.SerializeObject(instance, AjaxSerializerSettings);
        }

        public static string SerializeTypedJson(object instance) {
            return JsonConvert.SerializeObject(instance, TypedJsonSerializerSettings);
        }

        #endregion

        #region Hashing

        public static int GetNewHash() {
            return RandomlyChosenPrimeNumber1;
        }

        public static int CombineHashCode(int hash, object o) {
            hash *= RandomlyChosenPrimeNumber2;

            if (o != null) {
                hash += o.GetHashCode();
            }

            return hash;
        }

        #endregion

        #endregion
    }
}
