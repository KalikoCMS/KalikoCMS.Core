<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="KalikoCMS.Admin.Content.EditPage" ValidateRequest="false" %>
<%@ Register tagPrefix="cms" tagName="StringPropertyEditor" src="PropertyType/StringPropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="DateTimePropertyEditor" src="PropertyType/DateTimePropertyEditor.ascx" %>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>KalikoCMS</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Le styles -->
    <link href="/Admin/Assets/Styles/bootstrap.css" rel="stylesheet">
    <link href="/Admin/Assets/Styles/jquery-ui.custom.css" rel="stylesheet" />
    <style type="text/css">
        #ui-datepicker-div { font-size: 12px; }
    </style>
    <!-- IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="/Admin/Assets/Scripts/html5shiv.min.js"></script>
    <![endif]-->
    <script src="/Admin/Assets/Scripts/jquery-1.7.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <h1><asp:Literal ID="PageHeader" runat="server" /></h1>
        <div style="position: fixed;top:40px;bottom:92px;left:0px;right:0px;overflow: auto;padding:10px;">
            <div class="form-horizontal">
                <fieldset>
                    <cms:StringPropertyEditor ID="PageName" runat="server" />
                    <cms:DateTimePropertyEditor ID="StartPublishDate" runat="server" />
                    <cms:DateTimePropertyEditor ID="StopPublishDate" runat="server" />
        
                    <asp:Panel ID="EditControls" runat="server" />
                    <asp:Literal ID="ErrorMessage" runat="server" />
                    <asp:Literal ID="MessageBox" Visible="False" runat="server" />                    

                    <div class="form-actions" style="position: fixed;bottom:0px;width: 100%;">
                    
                    <asp:LinkButton runat="server" ID="SaveButton" CssClass="btn btn-large btn-primary">
                        <i class="icon-ok icon-white"></i> Save page
                    </asp:LinkButton>
<%--                    <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-large">
                        <i class="icon-trash"></i> Delete page
                    </asp:LinkButton>--%>
                    </div>
                </fieldset>
            </div>
        </div>
    </form>
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="/Admin/Assets/Scripts/bootstrap.min.js"></script>
    
    <script src="/Admin/Assets/Scripts/jquery-ui-1.8.20.custom.js"></script>
    <script src="/Admin/Assets/Scripts/jquery.datetimeentry.js"></script>
</body>
</html>
