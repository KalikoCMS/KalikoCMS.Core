<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopNavigation.ascx.cs" Inherits="KalikoCMS.Admin.Templates.Units.TopNavigation" %>

<nav class="navbar navbar-default navbar-fixed-top" role="navigation">
        <div class="container-fluid">
            <div class="navbar-header">
                <a class="navbar-brand" href="">Admin</a>
            </div>
            <ul class="nav navbar-nav">
                <li class="active"><a href="Content/"><i class="icon-edit icon-white"></i> Pages</a></li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li class="dropdown">
                <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                    <i class="icon-user"></i> <%=CurrentUser %> <span class="caret"></span>
                </a>
                <ul class="dropdown-menu">
                    <li><a href="Login.aspx?cmd=logout">Sign Out</a></li>
                </ul>
                </li>
            </ul>
        </div>
</nav>
