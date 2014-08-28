<%@ Page Title="Create new page" Language="C#" AutoEventWireup="true" CodeBehind="SelectPagetypeDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectPagetypeDialog" MasterPageFile="Dialog.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="ScriptArea">
  <script type="text/javascript">
    function selectPageType(pageTypeId) {
      top.createNewPage(pageTypeId);
    }
  </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainArea" runat="server">
  <p>Select page type:</p>
  <dl class="dl-horizontal">
    <asp:Literal ID="PageTypeList" runat="server" />
  </dl>
</asp:Content>
