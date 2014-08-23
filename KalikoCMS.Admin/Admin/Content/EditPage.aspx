<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="KalikoCMS.Admin.Content.EditPage" ValidateRequest="false" MasterPageFile="../Templates/MasterPages/Base.Master" %>
<%@ Register tagPrefix="cms" tagName="StringPropertyEditor" src="PropertyType/StringPropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="DateTimePropertyEditor" src="PropertyType/DateTimePropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="BooleanPropertyEditor" src="PropertyType/BooleanPropertyEditor.ascx" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <form id="MainForm" class="page-editor" role="form" runat="server">
    <h1><i class="icon-file-text"></i> <asp:Literal ID="PageHeader" runat="server" /></h1>
    <div id="editor-panel">
      <div class="form-horizontal">
        <fieldset>
          <cms:StringPropertyEditor ID="PageName" runat="server" />
          <cms:DateTimePropertyEditor ID="StartPublishDate" runat="server" />
          <cms:DateTimePropertyEditor ID="StopPublishDate" runat="server" />
          <cms:BooleanPropertyEditor ID="VisibleInMenu" runat="server" />
          <asp:Panel ID="EditControls" runat="server" />
          <div class="form-group">
            <label class="control-label col-xs-2" for="PageId">Page Id</label>
            <div class="controls col-xs-10">
              <span class="static-text">
                <asp:Literal ID="PageId" runat="server" /></span>
            </div>
          </div>
          <asp:Literal ID="ErrorMessage" runat="server" />
          <asp:Literal ID="MessageBox" Visible="False" runat="server" />
          <div class="form-actions">
            <asp:LinkButton runat="server" ID="SaveButton" CssClass="btn btn-lg btn-primary"><i class="icon-ok icon-white"></i> Save page</asp:LinkButton>
          </div>
        </fieldset>
      </div>
    </div>
  </form>
</asp:Content>


<asp:Content ContentPlaceHolderID="AdditionalScripts" runat="server">
    <script src="assets/js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="assets/js/bootstrap-datetimepicker.min.js" type="text/javascript"></script>
    <script src="assets/js/tinymce.min.js" type="text/javascript"></script>
    
    <script type="text/javascript">
      $(document).ready(function () {
        // TODO: Get editor options/toolbar from property attribute and web.config
        tinymce.init({
          selector: "textarea.html-editor",
          plugins: [
              "advlist autolink lists link image charmap anchor",
              "searchreplace visualblocks code",
              "insertdatetime media table contextmenu paste"
          ],
          resize: true,
          height: 300,
          menubar: false,
          toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | code",
          file_picker_callback: function (callback, value, meta) {
            if (meta.filetype == 'file') {
              top.registerCallback(function (newUrl, newType) { callback(newUrl); });
              top.propertyEditor.dialogs.openSelectLinkDialog(value, 0);
            }
            if (meta.filetype == 'image') {
              top.registerCallback(function (imagePath, cropX, cropY, cropW, cropH, originalPath, description) { callback(imagePath, { alt: description }); });
              top.propertyEditor.dialogs.openEditImageDialog(value, value, '', '', '', '', '', '', '');
            }
          }
        });
      });
    </script>
</asp:Content>
