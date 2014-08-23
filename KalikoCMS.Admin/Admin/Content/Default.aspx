<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KalikoCMS.Admin.Content.Default" MasterPageFile="../Templates/MasterPages/Admin.Master" %>
<%@ Register TagPrefix="custom" tagName="PageTreeControl" src="PageTree/PageTreeControl.ascx" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
    <div class="well sidebar-nav full-height no-scroll">
        <custom:PageTreeControl runat="server"/>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
    <div id="maincontent-wrapper">
        <iframe id="maincontent" src="about:blank"></iframe>
    </div>
    
    <script type="text/javascript">
        $(document).ready(function () { $(".left-navigation a").click(loadIframe); });

        function loadIframe(e) {
            $("#maincontent").attr("src", $(this).attr("href"));
            return false;
        }
    </script>
</asp:Content>
