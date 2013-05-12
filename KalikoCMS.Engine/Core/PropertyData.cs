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
    using KalikoCMS.Serialization;

    public abstract class PropertyData {

        protected abstract string StringValue { get; }

        protected abstract PropertyData DeserializeFromJson(string data);

        internal PropertyData Deserialize(string data) {
            PropertyData property = DeserializeFromJson(data);

            if (property == null) {
                property = (PropertyData)Activator.CreateInstance(GetType());
            }

            return property;
        }

        internal string Serialize() {
            return JsonSerialization.SerializeJson(this);
        }

        public override string ToString() {
            return StringValue;
        }

        public override abstract int GetHashCode();
    }
}
