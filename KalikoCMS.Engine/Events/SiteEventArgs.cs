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

namespace KalikoCMS.Events {
    using System;
    using Core;

    public delegate void SiteEventHandler(object sender, SiteEventArgs e);

    public class SiteEventArgs : EventArgs {
        private readonly Guid _siteId;
        private readonly int _languageId;
        private CmsSite _site;

        public SiteEventArgs(Guid siteId, int languageId) {
            _siteId = siteId;
            _languageId = languageId;
        }

        public int LanguageId {
            get { return _languageId; }
        }

        public Guid SiteId {
            get { return _siteId; }
        }

        public CmsSite Site {
            get { return _site ?? (_site = SiteFactory.Get(_siteId)); }
        }
    }
}