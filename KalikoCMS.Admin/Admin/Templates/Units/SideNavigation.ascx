<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SideNavigation.ascx.cs" Inherits="KalikoCMS.Admin.Templates.Units.SideNavigation" %>

<div id="sidebar">
  <div id="logo"></div>
  <ul class="list-unstyled">
    <li <%=ActiveArea == "Content" ? "class=\"active\"" : "" %>><a href="Content"><i class="icon-file"></i>Pages</a></li>
    <%--<li <%=ActiveArea == "Search" ? "class=\"active\"" : "" %>><a href="Search"><i class="icon-search"></i>Search</a></li>--%>
    <% RenderDashboardAreas(); %>
  </ul>
  <div id="version">Powered by KalikoCMS version <%=KalikoCMS.Utils.Version %></div>
</div>
