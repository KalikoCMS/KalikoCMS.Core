<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopNavigation.ascx.cs" Inherits="KalikoCMS.Admin.TopNavigation" %>

    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container-fluid">
                <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span
                    class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span>
                </a><a class="brand" href="/Admin/">kalikocms</a>
                <div class="btn-group pull-right">
                    <a class="btn dropdown-toggle" data-toggle="dropdown" href="#">
                        <i class="icon-user"></i> <%=CurrentUser %> <span class="caret"></span>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a href="/Admin/Login.aspx?cmd=logout">Sign Out</a></li>
                    </ul>
                </div>
                <div class="nav-collapse">
                    <ul class="nav">
                        <li class="active"><a href="/Admin/Content/"><i class="icon-pencil icon-white"></i> Content</a></li>
                    </ul>
                </div>
                <!--/.nav-collapse -->
            </div>
        </div>
    </div>
