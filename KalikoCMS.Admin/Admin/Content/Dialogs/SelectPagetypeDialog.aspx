<%@ Page Title="Create new page" Language="C#" AutoEventWireup="true" CodeBehind="SelectPagetypeDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectPagetypeDialog" MasterPageFile="Dialog.Master" %>

<asp:Content runat="server" ContentPlaceHolderID="ScriptArea">
  <script type="text/javascript">
    function selectPageType(pageTypeId) {
      top.createNewPage(pageTypeId);
    }
  </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainArea" runat="server">
  <div class="fill-area">
    <p>Select page type:</p>
    <div class="row">
      <asp:Literal ID="PageTypeList" runat="server" />
    </div>
  </div>
</asp:Content>
