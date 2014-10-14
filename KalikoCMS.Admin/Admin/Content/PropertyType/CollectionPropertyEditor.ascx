<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CollectionPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.CollectionPropertyEditor" %>

<div class="form-group">
  <asp:Label AssociatedControlID="ListContainer" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
  <asp:Panel ID="ListContainer" CssClass="controls col-xs-10" runat="server">

    <ul ID="Items" class="sortable-collection unstyled" runat="server">
    </ul>

    <button runat="server" ID="AddNewButton" class="btn btn-primary"><i class="icon icon-plus"></i> Add...</button>
    <asp:TextBox ID="CollectionValue" CssClass="collection-value hide" runat="server" />
  </asp:Panel>
</div>