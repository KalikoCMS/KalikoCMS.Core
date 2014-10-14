In this folder all generated, resized and/or cropped images will be stored.

If you wish to store them elsewhere you can change the imageCachePath 
attribute in the SiteSettings element of your web.config, for example:

  <siteSettings imageCachePath="/MyGeneratedImagesGoesHere/" />

Make sure that the folder ends with a slash and that the folder path exists 
with the proper access rights (no folders will be created by the system).