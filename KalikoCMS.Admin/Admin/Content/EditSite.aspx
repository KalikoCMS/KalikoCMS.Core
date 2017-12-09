<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditSite.aspx.cs" Inherits="KalikoCMS.Admin.Content.EditSite" ValidateRequest="false" MasterPageFile="../Templates/MasterPages/Base.Master" %>
<%@ Import Namespace="KalikoCMS" %>
<%@ Register tagPrefix="cms" tagName="StringPropertyEditor" src="PropertyType/StringPropertyEditor.ascx" %>

<asp:Content ContentPlaceHolderID="HeaderScripts" runat="server">
    <script type="text/javascript">
        // Reset property editors
        top.propertyEditor = null;
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <form id="MainForm" class="page-editor admin-page" role="form" runat="server">
    <div class="page-head">
      <h1>
        <asp:Literal ID="PageHeader" runat="server" /></h1>
    </div>
    <div id="editor-panel">
      <div class="form-horizontal">
        <asp:Literal ID="Feedback" runat="server" />
        <div role="alert" class="alert alert-warning alert-dismissible"><button data-dismiss="alert" class="close" type="button"><span aria-hidden="true">×</span><span class="sr-only">Close</span></button>You are editing the site settings.</div>
        <fieldset>
          <legend>Standard information</legend>
          <cms:StringPropertyEditor ID="SiteName" runat="server" />
          <div class="form-group">
            <asp:Label AssociatedControlID="ChildSortOrder" runat="server" Text="Sort children by" CssClass="control-label col-xs-2" />
            <div class="controls col-xs-2">
              <asp:DropDownList ID="ChildSortOrder" CssClass="form-control" runat="server" />
            </div>
            <div class="controls col-xs-2">
              <asp:DropDownList ID="ChildSortDirection" CssClass="form-control" runat="server" />
            </div>
          </div>
        </fieldset>

        <fieldset>
          <asp:Panel ID="EditControls" runat="server" />
        </fieldset>

        <div class="form-actions">
          <asp:LinkButton runat="server" ID="PublishButton" CssClass="btn btn-lg btn-primary"><i class="icon-thumbs-up"></i> Publish changes</asp:LinkButton>
        </div>
      </div>
    </div>

  </form>
</asp:Content>

<asp:Content ContentPlaceHolderID="AdditionalScripts" runat="server">
    <script src="assets/js/kalikocms.admin.editor.min.js?v=<%=Utils.VersionHash %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        $(document).ready(function() {
            initHtmlEditor('../assets/');
            initMarkdownEditor();
            initDropDowns();

            <asp:Literal Id="ScriptArea" runat="server" />

            warnBeforeLeavingIfChangesBeenMade();
        });
    </script>
</asp:Content>
