<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="KalikoCMS.Identity.Admin.Identity.Users" MasterPageFile="../Templates/MasterPages/Admin.Master" %>

<%@ Register Src="IdentityMenu.ascx" TagPrefix="admin" TagName="Menu" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
  <admin:menu activemenuitem="Users" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
  <div class="main-area admin-page">
    <div class="page-head">
      <h1>Manage users</h1>
    </div>

    <div class="main-content">
      <div class="inner-container">
        <table class="table table-striped table-hover">
          <thead>
            <tr>
              <th style="width:20%;">Username</th>
              <th style="width:25%;">Real name</th>
              <th style="width:25%;">Email</th>
              <th style="width:15%;">Created</th>
              <th style="width:15%;">Updated</th>
            </tr>
          </thead>
          <tbody>
            <asp:Literal ID="UserList" runat="server" />
          </tbody>
        </table>
        <a href="Identity/CreateUser.aspx" class="btn btn-primary pull-right"><i class="icon-plus"></i> Add user</a>
      </div>
    </div>
  </div>
</asp:Content>
