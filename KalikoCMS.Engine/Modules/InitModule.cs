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
