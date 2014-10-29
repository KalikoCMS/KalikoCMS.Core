<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="KalikoCMS.Identity.Admin.Identity.Roles" MasterPageFile="../Templates/MasterPages/Admin.Master" %>

<%@ Register Src="IdentityMenu.ascx" TagPrefix="admin" TagName="Menu" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
  <admin:menu activemenuitem="Roles" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
  <div class="main-area admin-page">
    <div class="page-head">
      <h1>Manage roles</h1>
    </div>

    <div class="main-content">
      <div class="inner-container">
        <table class="table table-striped table-hover">
          <thead>
            <tr>
              <th>Role</th>
            </tr>
          </thead>
          <tbody>
            <asp:Literal ID="RoleList" runat="server" />
          </tbody>
        </table>
        <a href="Identity/CreateRole.aspx" class="btn btn-primary pull-right"><i class="icon-plus"></i> Add role</a>
      </div>
    </div>
  </div>
</asp:Content>
