<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopNavigation.ascx.cs" Inherits="KalikoCMS.Admin.Templates.Units.TopNavigation" %>

<nav id="topbar" class="navbar navbar-static-top" role="navigation">
  <div class="navbar-header">
    <span class="navbar-brand" href=""><%=ActiveArea %></span>
  </div>
  <ul class="nav navbar-nav navbar-right">
    <li class="dropdown">
      <a class="dropdown-toggle" data-toggle="dropdown" href="#">
        <i class="icon icon-user"></i>&nbsp; <%=CurrentUser %> <span class="caret"></span>
      </a>
      <ul class="dropdown-menu">
        <li><a href="/Logout.aspx">Sign Out</a></li>
      </ul>
    </li>
  </ul>

</nav>
