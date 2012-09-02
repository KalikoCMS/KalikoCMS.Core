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
        
        .html-toolbar {
            display: block;
            margin: 10px 0 0 0;
            padding: 0;
            list-style: disc outside none;
            background: #eaeaea;
            padding: 4px; height: 30px; width: 500px; border-left: 1px solid rgb(216, 216, 216); border-top: 1px solid rgb(216, 216, 216); border-right: 1px solid rgb(216, 216, 216); border-radius: 3px 3px 0px 0px; box-shadow: 0pt 1px 3px rgba(255, 255, 255, 0.075) inset;
        }
        
        .html-toolbar > li {
            display: list-item;
            float: left;
            list-style: none outside none;
            margin: 0 5px 0 0;
        }
        
        .html-toolbar:after {
            clear: both;
            content: "";
            display: table;
        }

        iframe {
            background-color: #FFFFFF;
            border: 1px solid #CCCCCC;
            border-radius: 3px;
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            color: #555555;
            font-size: 13px;
            height: 200px;
            line-height: 18px;
            margin-bottom: 9px;
            padding: 4px;
            width: 500px;
        }
    </style>
    <!-- IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="/Admin/Assets/Scripts/html5shiv.min.js"></script>
    <![endif]-->
    <script src="/Admin/Assets/Scripts/jquery-1.7.2.min.js"></script>
    <script src="/Admin/Assets/Scripts/jquery.wysiwyg.js"></script>

    <script>
        var propertyEditor = {
            dialogs: {
                openSelectPageDialog: function (pageId, languageId, callback) {
                    parent.openModal("/Admin/Content/Dialogs/SelectPageDialog.aspx?pageId=" + pageId, 500, 400, callback);
                    return false;
                },
                openSelectFileDialog: function (filePath, callback) {
                    parent.openModal("/Admin/Content/Dialogs/SelectFileDialog.aspx?filePath=" + filePath, 500, 400, callback);
                    return false;
                }
            }
        };
    </script>

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
    
    <script>
        $().ready(function () {
            $('textarea.html-editor').editHtml();
        });
    </script>
</body>
</html>
