<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KalikoCMS.Admin.Content.Default" MasterPageFile="../Templates/MasterPages/Admin.Master" %>
<%@ Register TagPrefix="custom" tagName="PageTreeControl" src="PageTree/PageTreeControl.ascx" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
    <div class="sidebar-nav">
        <custom:PageTreeControl runat="server"/>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
    <div class="main-area">
        <iframe id="maincontent" src="about:blank"></iframe>
    </div>
    
    <script type="text/javascript">
        function loadInMainContent(url) {
            $("#maincontent").attr("src", url);
        }
    </script>
</asp:Content>
