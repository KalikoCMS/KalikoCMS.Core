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

namespace KalikoCMS.WebControls {
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Foundation that adds the capability to do autobinding to all web controls which inherits this class.
    /// </summary>
    public abstract class AutoBindableBase : CustomWebControl {
        private bool _databound;

        [Bindable(true),
         Category("Data"),
         DefaultValue(null)]
        public bool AutoBind { get; set; }

        public override void DataBind() {
            _databound = true;

            base.DataBind();
        }

        // TODO: Flytta senare i eventordningen?
        protected override void OnLoad(EventArgs e) {
            if(AutoBind && !_databound) {
                DataBind();
            }

            base.OnLoad(e);
        }
    }
}