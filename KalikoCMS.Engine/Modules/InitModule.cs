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

namespace KalikoCMS.Modules {
    using System;
    using System.Web;
    using Core;

    public class InitModule : IHttpModule {
        private static bool _isRunning;
        private static bool _firstRun = true;
        private static readonly object Padlock = new object();

        private InitModule() { }

        public void Init(HttpApplication context) {
            BreakIfAlreadyRunning();

            lock (Padlock) {
                if (_firstRun) {
                    InitializeApplication();
                }
            }
        }

        private static void InitializeApplication() {
            _isRunning = true;

            try {
                RunInitializingSteps();
            }
            catch (Exception) {
                _firstRun = true;
                _isRunning = false;
                throw;
            }

            _firstRun = false;
            _isRunning = false;
        }

        private static void RunInitializingSteps() {
            PageType.LoadPageTypes();
            PageFactory.IndexSite();
            RunStartupSequence();
        }

        private static void RunStartupSequence() {
            var types = InterfaceReader.GetTypesWithInterface(typeof(IStartupSequence));
            
            foreach (Type type in types) {
                if(type.IsInterface) {
                    continue;
                }

                var startupSequence = Activator.CreateInstance(type) as IStartupSequence;
                if (startupSequence != null) {
                    startupSequence.Startup();
                }
            }
        }

        private static void BreakIfAlreadyRunning() {
            if (_isRunning) {
                HttpResponse httpResponse = HttpContext.Current.Response;

                httpResponse.StatusCode = 503;
                httpResponse.Write("Starting up..");
                httpResponse.End();
            }
        }

        public void Dispose() {
        }
    }
}
