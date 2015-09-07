<%@ Page Title="Edit image" Language="C#" MasterPageFile="Dialog.Master" AutoEventWireup="true" CodeBehind="EditImageDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.EditImageDialog" %>

<%@ Register TagPrefix="cms" Namespace="KalikoCMS.Admin.WebControls" Assembly="KalikoCMS.Admin" %>

<asp:Content ContentPlaceHolderID="MainArea" runat="server">
  <asp:Literal runat="server" ID="PostbackResult" />

  <div class="modal-toolbar">
    <button id="select-image-button" class="btn btn-small" type="button"><i class="icon-folder-open"></i>Select image..</button>
  </div>

  <div class="image-container">
    <div class="background-checkered">
      <asp:Image ID="CropImage" runat="server" />
    </div>
    <div id="image-description-field" class="form-inline">
      <asp:Label AssociatedControlID="DescriptionField" runat="server" Text="Description" CssClass="span2" />
      <asp:TextBox runat="server" CssClass="span7" ID="DescriptionField" />
    </div>
  </div>

  <asp:HiddenField ID="CropX" runat="server" />
  <asp:HiddenField ID="CropY" runat="server" />
  <asp:HiddenField ID="CropW" runat="server" />
  <asp:HiddenField ID="CropH" runat="server" />
  <asp:HiddenField ID="Width" runat="server" />
  <asp:HiddenField ID="Height" runat="server" />
  <asp:HiddenField ID="ImageSource" runat="server" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ButtonArea" runat="server">
  <cms:bootstrapbutton id="SaveButton" enabled="false" icon="icon-thumbs-up icon-white" mode="Primary" text="Save image" runat="server" />
  <cms:bootstrapbutton id="RemoveButton" icon="icon-remove" mode="Danger" text="No image" runat="server" />
  <button type="button" id="close-button" data-dismiss="modal" class="btn btn-default">Cancel</button>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptArea" runat="server">
  <link rel="stylesheet" href="assets/vendors/jquery/css/jquery.jcrop.min.css" />
  <script src="assets/js/kalikocms.admin.imageeditor.min.js"></script>
  <script>
    (function (iife) {
      iife(jQuery, window);
    }(function ($, window) {
      var jcropApi;
      var cropImage = $("#<%=CropImage.ClientID %>");
      var image = new Image();
      var localImagePath = "<%=string.IsNullOrEmpty(OriginalPath) ? "data:image/gif;base64,R0lGODlhAQABAAD/ACwAAAAAAQABAAACADs%3D" : OriginalPath %>";
      var trueSize = [0, 0];

      image.onload = function () {
        trueSize = [this.width, this.height];

        if (typeof (jcropApi) != "undefined") {
          jcropApi.setOptions({ trueSize: trueSize });
        }
      };

      $(window).load(function () {
        cropImage.Jcrop({
          <%=PostedParameters %>
          boxWidth: 678,
          boxHeight: 280,
          onChange: storeCoords,
          onSelect: storeCoords,
          trueSize: trueSize
        }, function () { jcropApi = this; });
      });

      cropImage.load(function () {
        image.src = $(this).attr('src');

        if (image.src.substr(0, 5) == "data:") {
            return;
        }

        enableSaveButton();
        if (typeof (jcropApi) != "undefined") {
            jcropApi.setSelect(<%=CropValues%>);
        }
        setTimeout(function () { jcropApi.setSelect(<%=CropValues%>); }, 200); // Call with slight delay
      }).attr("src", localImagePath);

      function enableSaveButton() {
        $("#<%=SaveButton.ClientID %>")
          .removeClass("disabled")
          .removeAttr("disabled");
      }

      function storeCoords(c) {
        $("#<%=CropX.ClientID%>").val(parseInt(c.x, 10));
        $("#<%=CropY.ClientID%>").val(parseInt(c.y, 10));
        $("#<%=CropW.ClientID%>").val(parseInt(c.w, 10));
        $("#<%=CropH.ClientID%>").val(parseInt(c.h, 10));
      }

      function changeImage(filePath) {
        image.src = filePath;
        $("#<%=ImageSource.ClientID %>").val(filePath);
        localImagePath = filePath;
        jcropApi.setImage(filePath, onImageLoaded);
        enableSaveButton();
      }

      function onImageLoaded() {
        jcropApi.setSelect(<%=CropValues%>);
        setTimeout(function () { jcropApi.setSelect([0, 0, 9999, 9999]); }, 200); // Call with slight delay
      }

      $(function () {
        $("#close-button").click(abort);
        $("#select-image-button").click(function () {
          top.registerCallback(changeImage);
          parent.openModal("Content/Dialogs/SelectFileDialog.aspx?filePath=" + "", 700, 500);
        });
      });
    }));
  </script>
</asp:Content>
