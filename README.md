# Kaliko CMS

Kaliko CMS is an open source content management system (CMS) for the ASP.NET platform that supports both ASP.NET MVC and traditional WebForms. It provides you with the possibilities to build your website the way you want using the tools you choose. Whether you prefer MVC over WebForms or SQL Server over SQLite, it gives you a great framework to build on.

The framework approach also doesn't set any boundaries on how you structure your website or content. It's also very easy to extend the framework with content types if the ones included in the core doesn't quite match the requirements.

You find a couple of articles on [how to get started using Kaliko CMS here](http://kaliko.com/cms/get-started/).

Demo project using ASP.NET MVC and SQLite:
https://github.com/KalikoCMS/DemoSite.Mvc
 and the accompanying article:
http://www.codeproject.com/Articles/1021598/Build-a-website-with-Kaliko-CMS-using-ASP-NET-MVC

Demo project using WebForms and SQLite:
https://github.com/KalikoCMS/DemoSite.WebForms
 and the accompanying article:
http://www.codeproject.com/Articles/845483/Build-a-website-with-Kaliko-CMS-using-WebForms

Sign up to the Kaliko CMS Developer Newsletter to get updates about the project: http://eepurl.com/dJzMY

## Install via NuGet

The far easiest way to install Kaliko CMS is through NuGet:

* Install [Kaliko CMS Core](https://www.nuget.org/packages/KalikoCMS.Core/)-package
* Install either [Kaliko CMS WebForms Provider](https://www.nuget.org/packages/KalikoCMS.WebForms/)-package or [Kaliko CMS MVC Provider](https://www.nuget.org/packages/KalikoCMS.Mvc/)-package
* Install either [Kaliko CMS SQLite Provider](https://www.nuget.org/packages/KalikoCMS.Data.SQLite/)-package or [Kaliko CMS Sql Server Provider](https://www.nuget.org/packages/KalikoCMS.Data.SQLite/)-package *

Optionally install [Kaliko CMS Identity](https://www.nuget.org/packages/KalikoCMS.Identity/)-package to enable ASP.NET Identity based user authentication.

Optionally install [Kaliko CMS Search](https://www.nuget.org/packages/KalikoCMS.Search/)-package to enable search functionality.

*) For other databases see [supported databases](http://kaliko.com/cms/knowledge-base/supported-databases/)

## Requirements

* ASP.NET 4.5 or later
* Your choice of database ([list of supported databases](http://kaliko.com/cms/knowledge-base/supported-databases/))
* ASP.NET MVC or WebForms depending of choice

## Branching strategy

The master branch will always include the latest release while new development will be done in separate branches. All releases are also tagged.

## Licenses

Kaliko CMS is licensed under *GNU Lesser General Public License version 3 (LGPL)*. However, this project also contains other libraries with their individual licenses.

**KalikoCMS.Core makes use of:**

* AutoMapper ([MIT](https://github.com/AutoMapper/AutoMapper/blob/develop/LICENSE.txt))
* Bootbox.js ([MIT](http://opensource.org/licenses/mit-license.php))
* Bootstrap Modal ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
* Bootstrap-notify ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
* Bootstrap Tags Input ([MIT](http://opensource.org/licenses/mit-license.php))
* Datepicker for Bootstrap ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
* Font Awesome ([SIL OFL/MIT](http://fortawesome.github.io/Font-Awesome/license/))
* JavaScript Templates ([MIT](http://opensource.org/licenses/mit-license.php))
* Jcrop ([MIT](http://opensource.org/licenses/mit-license.php))
* jQuery ([MIT](http://jquery.org/license) / [GPLv2](http://jquery.org/license))
* jQuery Color Animations ([MIT](http://opensource.org/licenses/mit-license.php))
* jQuery Cookie plugin ([MIT](http://opensource.org/licenses/mit-license.php))
* jQuery File Upload Plugin ([MIT](http://opensource.org/licenses/mit-license.php))
* jQuery Selectric ([MIT](http://opensource.org/licenses/mit-license.php))
* Json.NET ([MIT](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md))
* jsTree ([MIT](http://opensource.org/licenses/mit-license.php))
* Logger ([Ms-PL](http://dotnetlogger.codeplex.com/license))
* MarkdownSharp ([MIT](http://opensource.org/licenses/mit-license.php))
* Moment.js ([MIT](http://opensource.org/licenses/mit-license.php))
* .NET Image Library ([Ms-PL](http://dotnetlogger.codeplex.com/license))
* Telerik Data Access ([License](http://www.telerik.com/purchase/license-agreement/data-access))
* TinyMCE ([LGPL 2.1](http://www.tinymce.com/js/tinymce4/js/tinymce/license.txt))
* Twitter Bootstrap ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
* Typeahead.js ([MIT](http://opensource.org/licenses/mit-license.php))

**KalikoCMS.Data-providers makes use of:**

* MySQL Connector ([GPLv2](http://www.gnu.org/licenses/old-licenses/gpl-2.0.html))
* System.Data.SQLite ([Public Domain](http://www.sqlite.org/copyright.html))

**KalikoCMS.Search makes use of:**

* Lucene.net ([Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.html))
* Html Agility Pack ([Ms-PL](https://htmlagilitypack.codeplex.com/license))

**KalikoCMS.Identity makes use of:**

* ASP.NET Identity provider for Telerik Data Access  ([Ms-PL](http://opensource.org/licenses/MS-PL))

## Thanks to

Thanks to Alvaro Muñoz (@pwntester) and Alexandr Mirosh from Hewlett-Packard Enterprise Security for great help with identifying and solving vulnerability related to JSON deserialization.

This project has been developed using:
* [BrowserStack](https://www.browserstack.com/)
* [Document! X](http://www.innovasys.com/product/dx/overview)
* [ReSharper](http://www.jetbrains.com/resharper/features/index.html?linklogos)
