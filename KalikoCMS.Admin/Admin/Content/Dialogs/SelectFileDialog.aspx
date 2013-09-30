<%@ Page Title="Select file" Language="C#" MasterPageFile="Dialog.Master" AutoEventWireup="true" CodeBehind="SelectFileDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectFileDialog" %>
<%@ Import Namespace="KalikoCMS.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainArea" runat="server">
  <link rel="stylesheet" href="assets/css/jquery.fileupload-ui.css" />
  <style>
    .file-list
    {
      width: 100%;
    }
    .file-list tr
    {
      cursor: pointer;
    }
    .file-list td
    {
      padding: 2px 15px;
    }
    .table-striped.file-list tr.selected td
    {
      background: #0088CC;
      color: #ffffff;
    }
    
    .breadcrumb { margin-bottom: 10px;}
    
    #file-queue-container,
    #file-list-container {
      left: 15px;
      overflow: auto;
      right: 15px;
    }
    
    .fileupload-buttonbar .btn, .fileupload-buttonbar .toggle { margin: 0; }
    .fileupload-progress { margin-top: 3px; }
    .progress { margin-bottom: 0; }
    
    
    .template-upload .progress { width: 100px; }
    
  </style>
  <div id="filelist" style="position: absolute;width:100%;">
    <div class="modal-toolbar">
      <button type="button" data-original-title="Create a new folder in current path." class="btn btn-small" id="create-folder-button"><i class="icon-folder-open-alt"></i> Create folder</button>
      <button type="button" data-original-title="Upload files into current path." class="btn btn-small" id="upload-file-button"><i class="icon-upload-alt"></i> Upload file(s)</button>
    </div>
    
    <ul class="breadcrumb"><li>&nbsp;</li></ul>
    <table><tr><th>File</th><th>Size</th></tr></table>
    <div id="file-list-container">
      <table id="file-list" class="table-striped table-hover file-list"></table>
    </div>
  </div>
  <div id="upload">
  <div id="upload-container">
    <div id="fileupload">
      <div class="modal-toolbar fileupload-buttonbar">
        <span data-original-title="Add files to upload." class="btn btn-small fileinput-button" id="Button1">
          <i class="icon-plus"></i> Add files... 
          <input type="file" name="files[]" multiple="multiple" />
        </span>
        <button type="submit" class="btn btn-small start">
          <i class="icon-upload-alt"></i> Start upload
        </button>
        <button type="reset" class="btn btn-small cancel">
          <i class="icon-ban-circle"></i> Cancel upload
        </button>
        
        <!-- The global progress information -->
        <div class="pull-right span3 fileupload-progress fade">
          <!-- The global progress bar -->
          <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100">
            <div class="bar" style="width: 0%;">
            </div>
          </div>
        </div>
      </div>
      <div id="file-queue-container">
        <table role="presentation" class="table table-hover">
          <tbody class="files">
            <tr>
              <th colspan="2" style="width: 60%;">
                File
              </th>
              <th style="width:20%;">
                Size
              </th>
              <th style="width:20%;">
                State
              </th>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
  </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ButtonArea" runat="server">
  <span id="filelist-buttons">
  <a id="select-button" class="btn btn-primary disabled"><i class="icon-ok"></i> Select file</a>
  <a id="deselect-button" class="btn btn-danger"><i class="icon-trash icon-white"></i> No file</a>
  </span>
  
  <span id="upload-buttons" class="hide">
  <a class="btn btn-primary" href="javascript:switchToFileListMode();"><i class="icon-arrow-left"></i> Return to file list</a>
  </span>

  <a data-dismiss="modal" class="btn" href="javascript:close();">Cancel</a>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptArea" runat="server">
  <script src="assets/js/tmpl.min.js" type="text/javascript"></script>
  <script src="assets/js/jquery.ui.widget.js" type="text/javascript"></script>
  <script src="assets/js/jquery.iframe-transport.js" type="text/javascript"></script>
  <script src="assets/js/jquery.fileupload.js" type="text/javascript"></script>
  <script src="assets/js/jquery.fileupload-process.js" type="text/javascript"></script>
  <script src="assets/js/jquery.fileupload-validate.js" type="text/javascript"></script>
  <script src="assets/js/jquery.fileupload-ui.js" type="text/javascript"></script>
  <script src="assets/js/bootbox.min.js" type="text/javascript"></script>

  <script type="text/javascript">
    var path = "<%=SiteSettings.Instance.FilePath %>";
    var mode = "fileList";
    var selectedFile = "";
    var buttonEnabled = false;

    $(document).ready(function () {
      fillHeight("#file-list-container", ".modal-footer");
      fillHeight("#file-queue-container", ".modal-footer");
      $("#upload").hide();

      $("#select-button").click(selectFile);
      $("#deselect-button").click(noFile);


      $("#create-folder-button")
        .click(function () {
          bootbox.prompt("Enter name for new folder", function (result) {
            if (result !== null && result.length > 0) {
              createFolder(result);
            }
          });
        })
        .tooltip({ placement: "bottom" });


      $("#upload-file-button")
        .click(switchToUploadMode).
        tooltip({ placement: "bottom" });

      loadFileList();
    });

    function fillHeight(selector, bottom) {
      var element = $(selector);
      element.css({ position: 'fixed', top: element.offset().top, bottom: $(bottom).outerHeight() });
    }

    function createFolder(folderName) {
      callWebMethod('CreateFolder', '{ folderName: "' + folderName + '", path: "' + path + '" }', loadFileList, displayError);
    }

    function switchToFileListMode() {
      mode = "fileList";
      $("#filelist").show();
      $("#upload").hide();
      $("#filelist-buttons").show();
      $("#upload-buttons").hide();
      loadFileList();
    }

    function switchToUploadMode() {
      mode = "upload";
      $("#filelist").hide();
      $("#upload").show();
      $("#filelist-buttons").hide();
      $("#upload-buttons").show();
    }

    $(function() {
      'use strict';

      // Initialize the jQuery File Upload widget:
      $('#fileupload')
        .fileupload({
            url: 'Handlers/FileHandler.ashx',
            sequentialUploads: true,
            formData: function(form) {
              return [{ name: 'path', value: path }];
            },
            maxFileSize: 5000000 //,
            //acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
          }
        )
        .bind('fileuploaddrop', function(e, data) {
          if (mode == "fileList") {
            switchToUploadMode();
          }
        });


      //}

    });

    function changePath(newPath) {
      path = newPath;
      loadFileList();
    }

    function disableSelectButton() {
      $('#select-button').addClass('disabled');
    }
    
    function callWebMethod(webmethod, data, onSuccess, onError) {
      $.ajax({
        type: 'POST',
        url: 'Content/Dialogs/SelectFileDialog.aspx/' + webmethod,
        data: data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(msg) {
          if (msg.d.status == 200) {
            onSuccess(msg.d.data);
          } else {
            onError(msg.d.data);
          }
        }
      });
    }

    function displayError(data) {
      bootbox.alert(data);
      return false;
    }

    function displayFileList(data) {
      disableSelectButton();
      
      var fileList = $('.file-list');
      fileList.html('');

      var folders = data.folders;
      for (var i in folders) {
        var folder = folders[i];
        fileList.append('<tr data-path="' + folder.name + '" class="folder"><td colspan="2"><i class="icon-folder-close"></i> ' + folder.name + '</td></tr>');
      }

      var files = data.files;
      for (i in files) {
        var file = files[i];
        fileList.append('<tr data-path="' + file.name + '" class="file"><td colspan="2"><i class="icon-file-alt"></i> ' + file.name + '</td></tr>');
      }

      $('.file-list tr.file').click(function () {
        $('.file-list tr').removeClass('selected');
        $(this).addClass('selected');

        selectedFile = $(this).attr('data-path');
        $('#select-button').removeClass('disabled');
        buttonEnabled = true;
      });
      
      $('.file-list tr.folder').click(function () {
        path += $(this).attr('data-path') + '/';
        loadFileList();
      });

      var breadcrumbs = $(".breadcrumb");
      var pathArray = path.replace(/(^\/*)|(\/*$)/g, '').split('/');
      var newPath = '/';

      breadcrumbs.empty();
      $.each(pathArray, function (index, value) {
        newPath += value + "/";
        breadcrumbs.append('<li><a href="javascript:changePath(\'' + newPath + '\');">' + value + '</a>' + ((index + 1) < pathArray.length ? ' <span class="divider">/</span>' : '') + '</li>');
      });
    }

    function loadFileList() {
      callWebMethod('GetFileList', '{ path: "' + path + '" }', displayFileList, displayError);
    }
    
    function selectFile() {
      if (!buttonEnabled)
        return;

      top.executeCallback(path + selectedFile);
      close();
    }

    function noFile() {
      top.executeCallback('');
      close();
    }
  </script>
  
  

    <!-- The template to display files available for upload -->
  <script id="template-upload" type="text/x-tmpl">
{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-upload fade {% if (file.error) { %}error{% } %}">
        <td>
            {%=file.name%}
            {% if (file.error) { %}
                <span class="label label-important">Error</span> {%=file.error%}
            {% } %}
        </td>
        <td>
            {% if (!o.files.error) { %}
                <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><div class="bar bar-success" style="width:0%;"></div></div>
            {% } %}
        </td>
        <td>
            <p class="xxsize">{%=o.formatFileSize(file.size)%}</p>
        </td>
        <td>
            {% if (!o.files.error && !i && !o.options.autoUpload) { %}
                <button class="btn btn-icon-normal btn-primary start">
                    <i class="icon-upload-alt icon-white"></i>
                    <span>Start</span>
                </button>
            {% } %}
            {% if (!i) { %}
                <button class="btn btn-icon-normal btn-warning cancel">
                    <i class="icon-ban-circle icon-white"></i>
                </button>
            {% } %}
        </td>
    </tr>
{% } %}
  
  </script>
  <!-- The template to display files available for download -->
  <script id="template-download" type="text/x-tmpl">
{% for (var i=0, file; file=o.files[i]; i++) { %}
    <tr class="template-download fade {% if (file.error) { %}error{% } else { %}success{% } %}">
        <td colspan="2">
            {%=file.name%}
            
            {% if (file.error) { %}
                <span class="label label-important">Error</span> {%=file.error%}
            {% } %}
        </td>
        <td>
            <span class="size">{%=o.formatFileSize(file.size)%}</span>
        </td>
        <td>
        </td>
    </tr>
{% } %}
  </script>
</asp:Content>
