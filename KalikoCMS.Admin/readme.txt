
Thanks for installing Kaliko CMS version 1.2.2!

Notice if you're upgrading Kaliko CMS (this doesn't apply to new projects):
============================================================================
 If you're upgrading from a previous version prior to version 1.2.1 and are 
 using either composite or collection properties it's recommended to backup 
 your existing database. In version 1.2.1 the third party JSON library was 
 replaced for security reasons. Although the replacement has been fully 
 tested we recommend as an extra precaution to backup the database.
============================================================================

New in 1.2.2
* Fixed problem with custom property types only used in site definition not registering

New in 1.2.1:
* Replaced JSON component
* Method for setting default values for pages
* Fixed problem where collection property editor breaks on larger property value
* Fixed problem with non-responsive dialog when moving pages
* Fixed problem with image editor in collection properties
* Fixed problem with using composite properties in collections
* Fixed problem with failing page saves on PageLink properties
* Fixed problem with property type scripts when using collection properties
* Fixed problem with preview when site is set up as a subsite
* Fixed problem with short url in editor when site is set up as a subsite
* Fixed problem with editor allowing start publish date after stop publish date
* Added IoC support for MVC controllers
* Added option to select what fields to search in
* Added PageMoved event
* Updated core project to ASP.NET 4.5
* Updated AutoMapper

---

For documentation:
http://kaliko.com/cms/get-started/

For feature requests and bug reports:
https://github.com/KalikoCMS/KalikoCMS.Core/issues

For general support:
http://kaliko.com/cms/forum
