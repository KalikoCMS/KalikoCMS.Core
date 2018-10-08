<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="KalikoCMS.Admin.Content.EditPage" ValidateRequest="false" MasterPageFile="../Templates/MasterPages/Base.Master" %>
<%@ Import Namespace="KalikoCMS" %>
<%@ Register tagPrefix="cms" tagName="StringPropertyEditor" src="PropertyType/StringPropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="UniversalDateTimePropertyEditor" src="PropertyType/UniversalDateTimePropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="BooleanPropertyEditor" src="PropertyType/BooleanPropertyEditor.ascx" %>

<asp:Content ContentPlaceHolderID="HeaderScripts" runat="server">
    <script type="text/javascript">
        // Reset property editors
        top.propertyEditor = null;
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <form id="MainForm" class="page-editor admin-page" role="form" runat="server">
    <div class="page-head">
      <h1><asp:Literal ID="PageHeader" runat="server" /></h1>
    </div>
    <div id="editor-panel">
      <div class="form-horizontal">
        <asp:Literal ID="Feedback" runat="server" />
        <fieldset>
          <legend>Standard information</legend>
          <cms:StringPropertyEditor ID="PageName" runat="server" />
          <cms:UniversalDateTimePropertyEditor ID="StartPublishDate" runat="server" />
          <cms:UniversalDateTimePropertyEditor ID="StopPublishDate" runat="server" />
          <cms:BooleanPropertyEditor ID="VisibleInMenu" runat="server" />
          <asp:Panel ID="AdvancedOptionButton" CssClass="row" runat="server">
            <span id="advanced-options" class="col-xs-10 col-xs-offset-2"><i class="icon-plus-square text-primary"></i> Show advanced options</span>
          </asp:Panel>
          <div id="advanced-panel" style="display:none;">
            <cms:BooleanPropertyEditor ID="VisibleInSitemap" runat="server" />
            <asp:HiddenField ID="OldPageUrlSegment" runat="server" />
            <div class="form-group">
              <asp:Label AssociatedControlID="PageUrlSegment" runat="server" Text="URL segment" CssClass="control-label col-xs-2" />
              <div class="controls col-xs-4">
                <asp:TextBox runat="server" ID="PageUrlSegment" CssClass="form-control" />
              </div>
              <div class="col-xs-6">
                <i class="form-comment">Leave blank to let the CMS handle the segment creation.</i>
              </div>
            </div>
            <div class="form-group">
              <asp:Label AssociatedControlID="ChildSortOrder" runat="server" Text="Sort children by" CssClass="control-label col-xs-2" />
              <div class="controls col-xs-2">
                <asp:DropDownList ID="ChildSortOrder" CssClass="form-control" runat="server" />
              </div>
              <div class="controls col-xs-2">
                <asp:DropDownList ID="ChildSortDirection" CssClass="form-control" runat="server" />
              </div>
            </div>
          </div>
        </fieldset>
                
        <fieldset>
          <asp:Panel ID="EditControls" runat="server" />
        </fieldset>
        
        <fieldset>
          <legend>Information</legend>
          <div class="form-group">
            <label class="control-label col-xs-2" for="PageId">Page Id</label>
            <div class="controls col-xs-10">
              <span class="static-text"><asp:Literal ID="PageId" runat="server" /></span>
            </div>
          </div>
          <div class="form-group">
            <label class="control-label col-xs-2" for="PageTypeName">Pagetype</label>
            <div class="controls col-xs-10">
              <span class="static-text"><asp:Literal ID="PageTypeName" runat="server" /></span>
            </div>
          </div>
          <div class="form-group">
            <label class="control-label col-xs-2" for="PublishedUrl">Published Url</label>
            <div class="controls col-xs-10">
              <span class="static-text"><asp:Literal ID="PublishedUrl" runat="server" /></span>
            </div>
          </div>
          <div class="form-group">
            <label class="control-label col-xs-2" for="ShortUrl">Short Url</label>
            <div class="controls col-xs-10">
              <span class="static-text"><asp:Literal ID="ShortUrl" runat="server" /></span>
            </div>
          </div>
        </fieldset>
      </div>
    </div>
    <div class="form-actions">
        <button id="versionbutton" type="button" class="btn btn-default pull-right"><i class="icon-code-fork"></i> Show versions</button>
        <asp:LinkButton runat="server" ID="PublishButton" CssClass="btn btn-lg btn-primary"><i class="icon-thumbs-up"></i> Publish page</asp:LinkButton>
        <asp:LinkButton runat="server" ID="SaveButton" CssClass="btn btn-lg btn-default"><i class="icon-pencil"></i> Save working copy</asp:LinkButton>
    </div>
  </form>
</asp:Content>


<asp:Content ContentPlaceHolderID="AdditionalScripts" runat="server">
    <script src="assets/js/kalikocms.admin.editor.min.js?v=<%=Utils.VersionHash %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            initHtmlEditor('../assets/');
            initMarkdownEditor();
            initDropDowns();
        
            <asp:Literal Id="ScriptArea" runat="server" />

            warnBeforeLeavingIfChangesBeenMade();

            $('#versionbutton').click(function() {
                parent.openModal("Content/Dialogs/PageVersionDialog.aspx?id=<%=CurrentPageId%>", 700, 500);
            });

            $('#advanced-options').click(function() {
                $('#<%=AdvancedOptionButton.ClientID%>').hide();
                $('#advanced-panel').slideDown();
            });
        });
    </script>
</asp:Content>
