### New in 1.1.0
* Added composite property
* Added page preview
* Added options to set class or additional attributes on ImageProperty.ToHtml()
* Added tags and summary field to search hits
* Extended FindSimular to support additional meta data
* Fixed wrong path for DateTimeProperty
* Fixed sort order in page type dialog
* Fixed problem for systems that have duplicate tag context
* Fixed issues related to not using singleton OWIN user manager
* Fixed problem with preauthorized page requests

### New in 1.0.1
* Added Html helper for menu trees
* Added Html helper for breadcrumbs
* Improved MVC routing
* Fixed problems with image editor

### New in 1.0.0
* Added Markdown property type
* Added option to set URL segment when editing a page
* Added option to set visibility in sitemaps when editing a page
* Added page versioning
* Added page published event
* Added child sort order to pages
* Added default child sort order to pagetypes
* Added function to drag-and-drop sort order in page tree
* Added function to limit available pagetypes for each pagetype
* Get children now returns sorted list based on parent page settings
* Divided startup code in pre and post CMS initialization
* Improved custom routing in MVC provider
* Improved pagetype selector
* Added function to fetch all pages based on a predicate filter
* Added original status for use on working copies
* Fixed support for application paths in page URLs
* Fixed support for abstract (inehrited) MVC controllers
* Fixed property HTML output for Razor views
* Fixed default log path to use the |DataDirectory| alias
* Fixed bug overwriting image preview in editor
* Fixed log level for datacontext
* Fixed assembly resolver for SQLite
* Fixed bug in collection properties when sub-property included an apostrophe
* Cleaned up NuGet package removing bundle source files

### New in 0.9.9
* Fixed broken upload component

### New in 0.9.8
* Fixed bug in NuGet packages that replaced custom connectionstrings during update
* Improved simular page search

### New in 0.9.8-beta
* Added redirect support for old page links when moving pages or changing URLs
* Improved image picker
* Fixed broken button icons for link property editors

### New in 0.9.7-beta
* Added ASP.NET Identity integration
* Added sort order to startup sequence
* Added administration of roles and users
* Added a warning if leaving editor page with unsaved changes
* Fixed bug in page list caching
* Updated third party references to latest versions

### New in 0.9.6-beta
* Added new UniversalDateTimeProperty for timezone independent dates
* Publishing dates made timezone independent
* Username stored in page author field
* Security fix: Prevent loading unpublished page through template
* Minor update in default theme

### New in 0.9.5-beta
* Major update of the administration interface
* Restructured includes of third party styles and scripts in admin
* Fixed bug moving pages under the root
* Added confirm box when moving pages
* Updated JsTree to version 3
* Fixed admin path references
* Added configurable theme setting

### New in 0.9.4-beta
* Fixes problem with instability at AppDomain restart
* Minor code clean-up

### New in 0.9.3-beta
* Replaced data layer by Telerik Data Access
* Automatic database creation, updates and versioning
* Fixes UI for links property editor
* Fixes UI for editing collection items
* Overall code cleanup

### New in 0.9.2-beta
* Fixes round-robin bug when creating the first page
* Improved TinyMCE editor settings
* Added predicate filtering for getting page trees