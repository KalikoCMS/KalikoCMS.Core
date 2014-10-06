<%@ Page Title="Edit item" Language="C#" AutoEventWireup="true" MasterPageFile="Dialog.Master" CodeBehind="EditCollectionPropertyDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.EditCollectionPropertyDialog" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainArea" runat="server">
  <div class="form-horizontal fill-area">
  <asp:PlaceHolder runat="server" ID="PropertyEditor" />
  <asp:Literal runat="server" ID="PostbackResult" />  
  </div>
</asp:Content>


<asp:Content ID="ButtonContent" ContentPlaceHolderID="ButtonArea" runat="server">
    <button id="SaveButton" type="button" runat="server" class="btn btn-primary"><i class="icon-thumbs-up icon-white"></i> Save</button>
    <button id="deselect-button" type="button" class="btn btn-danger"><i class="icon-trash icon-white"></i> Remove item</button>
    <button id="close-button" type="button" data-dismiss="modal" class="btn btn-default">Cancel</button>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ScriptArea" runat="server">
  <script type="text/javascript">

    (function (iife) {
      iife(jQuery, window, document);
    } (function ($, window, document) {

      $(function () {
        $("#deselect-button").click(removeItem);
        $("#close-button").click(abort);
      });

      function removeItem() {
        top.executeCallback(null, null);
        top.closeModal();
      }
    }));
  </script>
</asp:Content>