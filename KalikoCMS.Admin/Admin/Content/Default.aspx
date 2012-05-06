<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KalikoCMS.Admin.Content.Default" MasterPageFile="~/Admin/Admin.Master" %>
<%@ Register TagPrefix="custom" tagName="PageTreeControl" src="PageTree/PageTreeControl.ascx" %>

<asp:Content ContentPlaceHolderID="LeftRegion" runat="server">
    <div class="well sidebar-nav full-height no-scroll" style="width:280px;">
        <custom:PageTreeControl runat="server"/>
    </div>
</asp:Content>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
    <div style="left:320px;right:20px;top:60px;bottom:20px;position:absolute;min-width:600px;">
        <iframe id="maincontent" src="about:blank" style="width:100%;height:100%;"></iframe>
    </div>
    
    <script type="text/javascript">
        $(document).ready(function () { $(".left-navigation a").click(loadIframe); });

        function loadIframe(e) {
            $("#maincontent").attr("src", $(this).attr("href"));
            return false;
        }
    </script>

</asp:Content>
