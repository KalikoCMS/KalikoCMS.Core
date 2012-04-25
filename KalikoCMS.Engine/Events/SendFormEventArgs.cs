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

namespace KalikoCMS.Events {
    using System;
    using System.Linq;
    using System.Web.UI;

    public class SendFormEventArgs : EventArgs {
        private bool _formWasSentCorrectly = true;
        private readonly ControlCollection _formContainer;

        public SendFormEventArgs(ControlCollection formContainer) {
            _formContainer = formContainer;
        }

        public bool FormWasSentCorrectly {
            get {
                return _formWasSentCorrectly;
            }
            set {
                _formWasSentCorrectly = value;
            }
        }

        public Control this[string name] {
            get { return _formContainer.Cast<Control>().FirstOrDefault(c => c.ID == name); }
        }
    }
}