<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPage.aspx.cs" Inherits="KalikoCMS.Admin.Content.EditPage" ValidateRequest="false" MasterPageFile="../Templates/MasterPages/Base.Master" %>
<%@ Register tagPrefix="cms" tagName="StringPropertyEditor" src="PropertyType/StringPropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="DateTimePropertyEditor" src="PropertyType/DateTimePropertyEditor.ascx" %>
<%@ Register tagPrefix="cms" tagName="BooleanPropertyEditor" src="PropertyType/BooleanPropertyEditor.ascx" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
  <form id="MainForm" runat="server">
  <h1><i class="icon-edit"></i> <asp:Literal ID="PageHeader" runat="server" /></h1>
  <div style="position: fixed; top: 60px; bottom: 93px; left: 0px; right: 0px; overflow: auto; padding: 10px;">
    <div class="form-horizontal">
      <fieldset>
        <cms:StringPropertyEditor ID="PageName" runat="server" />
        <cms:DateTimePropertyEditor ID="StartPublishDate" runat="server" />
        <cms:DateTimePropertyEditor ID="StopPublishDate" runat="server" />
        <cms:BooleanPropertyEditor ID="VisibleInMenu" runat="server" />
        <asp:Panel ID="EditControls" runat="server" />
        <asp:Literal ID="ErrorMessage" runat="server" />
        <asp:Literal ID="MessageBox" Visible="False" runat="server" />
        <div class="form-actions" style="position: fixed; bottom: 10px; width: 100%; margin: 0;">
          <asp:LinkButton runat="server" ID="SaveButton" CssClass="btn btn-large btn-primary"><i class="icon-ok icon-white"></i> Save page</asp:LinkButton>
        </div>
      </fieldset>
    </div>
  </div>
  </form>
</asp:Content>


<asp:Content ContentPlaceHolderID="AdditionalScripts" runat="server">
    <script src="assets/js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="assets/js/bootstrap-datetimepicker.min.js?d=<%=DateTime.Now %>" type="text/javascript"></script>
    <script src="assets/js/jquery.wysiwyg.js" type="text/javascript"></script>
    
    <script type="text/javascript">
      $(document).ready(function () {
        $('textarea.html-editor').editHtml();
      });
    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="AdditionalStyles" runat="server">
    <link href="assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet">

    <style type="text/css">
        #ui-datepicker-div { font-size: 12px; }
        
        .html-toolbar {
            display: block;
            margin: 10px 0 0 0;
            padding: 0;
            list-style: disc outside none;
            background: #eaeaea;
            padding: 4px; 
            height: 30px; 
            border: 1px solid rgb(216, 216, 216); 
            border-bottom: none; 
            border-radius: 3px 3px 0 0; 
            box-shadow: 0 1px 3px rgba(255, 255, 255, 0.075) inset;
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
            -moz-border-radius: 3px;
            -webkit-border-radius: 3px;
            border-radius: 3px;
            color: #555555;
            font-size: 13px;
            height: 200px;
            line-height: 18px;
            margin-bottom: 9px;
            padding: 4px;
            width: 500px;
        }
    </style>
</asp:Content>
