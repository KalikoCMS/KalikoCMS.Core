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

namespace KalikoCMS.Headless.Attributes {
    using System;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Web.Http.Controllers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class JsonConfigurationAttribute : Attribute, IControllerConfiguration {
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor) {
            var formatter = controllerSettings.Formatters.OfType<JsonMediaTypeFormatter>().Single();
            controllerSettings.Formatters.Remove(formatter);

            formatter = new JsonMediaTypeFormatter {
                SerializerSettings = {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    PreserveReferencesHandling = PreserveReferencesHandling.None
                }
            };

            formatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            controllerSettings.Formatters.Add(formatter);
        }
    }
}