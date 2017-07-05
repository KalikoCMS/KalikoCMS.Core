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
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class LegacyJsonConverter {

        internal static string Convert(string json) {
            var jsonObject = JObject.Parse(json);

            var types = ExtractTypeArray(jsonObject);
            UpdateTypeParameters(jsonObject, types);

            return jsonObject.ToString(Formatting.None);
        }

        private static void UpdateTypeParameters(JObject jObject, IReadOnlyDictionary<string, string> types) {
            var tokens = jObject.FindTokens("$type");
            foreach (JProperty property in tokens) {
                var typeId = property.Value.Value<string>();
                var type = types[typeId];
                property.Value = type;
            }
        }

        private static Dictionary<string, string> ExtractTypeArray(JObject jsonObject) {
            var types = jsonObject.First.First.ToObject<Dictionary<string, string>>();
            jsonObject.Remove("$types");

            return types.ToDictionary(x => x.Value, x => x.Key);
        }
    }
}
