<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IdentityMenu.ascx.cs" Inherits="KalikoCMS.Identity.Admin.Identity.IdentityMenu" %>

<div class="sidebar-nav">
  <ul class="nav nav-pills nav-stacked">
    <li <%=(ActiveMenuItem == "Users" ? " class=\"active\"" : "") %>><a href="Identity/Users.aspx"><i class="icon-user"></i>Users</a></li>
    <li <%=(ActiveMenuItem == "Roles" ? " class=\"active\"" : "") %>><a href="Identity/Roles.aspx"><i class="icon-users"></i>Roles</a></li>
  </ul>
</div>
