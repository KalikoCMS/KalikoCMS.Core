<%@ Page Title="Edit image" Language="C#" MasterPageFile="Dialog.Master" AutoEventWireup="true" CodeBehind="EditImageDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.EditImageDialog" %>
<%@ Register TagPrefix="cms" Namespace="KalikoCMS.Admin.WebControls" Assembly="KalikoCMS.Admin" %>

<asp:Content ContentPlaceHolderID="MainArea" runat="server">
  <asp:Literal runat="server" ID="PostbackResult" />   

  <div class="modal-toolbar">
    <button id="select-image-button" class="btn btn-small" type="button"><i class="icon-folder-open-alt"></i> Select image..</button>
  </div>
      
  <div class="image-container">
    <div class="background-checkered">
        <asp:Image id="CropImage" runat="server" />
    </div>
    <div id="image-description-field" class="form-inline">
      <div class="row" style="margin-top:5px;">
        <asp:Label AssociatedControlID="DescriptionField" runat="server" Text="Description" CssClass="span2" />
        <asp:TextBox runat="server" CssClass="span7" ID="DescriptionField" />
      </div>
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
  <cms:BootstrapButton id="SaveButton" Enabled="false" Icon="icon-ok" Mode="Primary" Text="Save image" runat="server"/>
  <cms:BootstrapButton id="RemoveButton" Icon="icon-delete" Mode="Danger" Text="No image" runat="server"/>
  <button type="button" id="close-button" data-dismiss="modal" class="btn">Cancel</button>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptArea" runat="server">
  <link rel="stylesheet" href="assets/css/jquery.jcrop.min.css" />
  <script src="assets/js/jquery.color.js"></script>
  <script src="assets/js/jquery.jcrop.min.js"></script>
  <script>
    (function(iife) {
      iife(jQuery, window);
    }(function($, window) {
      var jcropApi;
      var cropImage = $("#<%=CropImage.ClientID %>");
      var image = new Image();
      var localImagePath = "<%=OriginalPath %>";
      var trueSize = [0, 0];

      image.onload = function() {
        trueSize = [this.width, this.height];

        if (typeof(jcropApi) != "undefined") {
          jcropApi.setOptions({ trueSize: trueSize });
        }
      };
      
      $(window).load(function() {
        cropImage.Jcrop({
          <%=PostedParameters %>
          boxWidth: 500,
          boxHeight: 300,
          onChange: storeCoords,
          onSelect: storeCoords,
          trueSize: trueSize
        }, function() { jcropApi = this; });
      });

      cropImage.load(function() {
        enableSaveButton();
        image.src = $(this).attr('src');
        jcropApi.setSelect([0, 0, 999, 999]);
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
            jcropApi.setSelect([0, 0, 999, 999]);
        }

      $(function() {
        $("#close-button").click(abort);
        $("#select-image-button").click(function() {
          top.registerCallback(changeImage);
          parent.openModal("Content/Dialogs/SelectFileDialog.aspx?filePath=" + "", 700, 500);
        });
      });
    }));
  </script>
</asp:Content>