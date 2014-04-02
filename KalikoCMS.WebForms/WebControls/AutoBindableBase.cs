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

namespace KalikoCMS.WebForms.WebControls {
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

        protected override void OnLoad(EventArgs e) {
            if(AutoBind && !_databound) {
                DataBind();
            }

            base.OnLoad(e);
        }
    }
}