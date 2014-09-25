# Kaliko CMS

Kaliko CMS is an open source content management system (CMS) for the ASP.NET platform that supports both ASP.NET MVC and traditional WebForms. It provides you with the possibilities to build your website the way you want using the tools you choose. Whether you prefer MVC over WebForms or SQL Server over SQLite, it gives you a great framework to build on.

The framework approach also doesn't set any boundaries on how you structure your website or content. It's also very easy to extend the framework with content types if the ones included in the core doesn't quite match the requirements.

You find a couple of articles on [how to get started using Kaliko CMS here](http://kaliko.com/cms/get-started/).

## Install via NuGet

The far easiest way to install Kaliko CMS is through NuGet:

* Install [Kaliko CMS Core](https://www.nuget.org/packages/KalikoCMS.Core/)-package
* Install either [Kaliko CMS WebForms Provider](https://www.nuget.org/packages/KalikoCMS.WebForms/)-package or [Kaliko CMS MVC Provider](https://www.nuget.org/packages/KalikoCMS.Mvc/)-package
* Install either [Kaliko CMS SQLite Provider](https://www.nuget.org/packages/KalikoCMS.Data.SQLite/)-package or [Kaliko CMS Sql Server Provider](https://www.nuget.org/packages/KalikoCMS.Data.SQLite/)-package *

Optionally install [Kaliko CMS Search](https://www.nuget.org/packages/KalikoCMS.Search/)-package to enable search functionality.

*) For other databases see [supported databases](http://kaliko.com/cms/knowledge-base/supported-databases/)

## Requirements

* ASP.NET 4.0 or later
* Your choice of database ([list of supported databases](http://kaliko.com/cms/knowledge-base/supported-databases/))
* ASP.NET MVC or WebForms depending of choice

## Branching strategy

The master branch will always include the latest release while new development will be done in separate branches. All releases are also tagged.

## Licenses

Kaliko CMS is licensed under *GNU Lesser General Public License version 3 (LGPL)*. However, this project also contains other libraries with their individual licenses.

**KalikoCMS.Core makes use of:**
- fastJSON ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
- Font Awesome ([SIL OFL/MIT](http://fortawesome.github.io/Font-Awesome/license/))
- Telerik Data Access ([License](http://www.telerik.com/purchase/license-agreement/data-access))
- jQuery ([MIT](http://jquery.org/license) / [GPLv2](http://jquery.org/license))
- Logger ([Ms-PL](http://dotnetlogger.codeplex.com/license))
- .NET Image Library ([Ms-PL](http://dotnetlogger.codeplex.com/license))
- Twitter Bootstrap ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
