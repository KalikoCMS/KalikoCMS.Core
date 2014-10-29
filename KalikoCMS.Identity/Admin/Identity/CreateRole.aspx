<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateRole.aspx.cs" Inherits="KalikoCMS.Identity.Admin.Identity.CreateRole" MasterPageFile="../Templates/MasterPages/Admin.Master" %>

<%@ Register Src="IdentityMenu.ascx" TagPrefix="admin" TagName="Menu" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
  <admin:menu activemenuitem="Roles" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
  <div class="main-area admin-page">
    <div class="page-head">
      <h1>Edit role</h1>
    </div>

    <div class="main-content">
      <div class="inner-container form-horizontal">
        <asp:Literal ID="Feedback" runat="server" />

        <fieldset>
          <asp:Panel ID="FormFields" runat="server">
            <legend>Information</legend>
            <asp:HiddenField ID="RoleId" runat="server" />
            <div class="form-group">
              <asp:Label AssociatedControlID="RoleName" CssClass="control-label col-xs-2" runat="server">Role name</asp:Label>
              <div class="col-xs-10">
                <asp:TextBox ID="RoleName" CssClass="form-control" runat="server" />
              </div>
            </div>
          </asp:Panel>
          <div class="form-buttons">
            <a href="Identity/Roles.aspx" class="btn btn-default">Cancel</a>
            <asp:LinkButton ID="SaveButton" CssClass="btn btn-primary" runat="server"><i class="icon-thumbs-up icon-white"></i> Create role</asp:LinkButton>
          </div>
        </fieldset>
      </div>
    </div>
  </div>
</asp:Content>
