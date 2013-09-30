<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CollectionPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.CollectionPropertyEditor" %>

<div class="control-group">
  <asp:Label AssociatedControlID="ListContainer" runat="server" ID="LabelText" CssClass="control-label" />
  <asp:Panel ID="ListContainer" CssClass="controls" runat="server">

    <ul ID="Items" class="sortable-collection unstyled wide-input" runat="server">
    </ul>

    <button runat="server" ID="AddNewButton" class="btn"><i class="icon-plus-sign"></i> Add...</button>
    <asp:TextBox ID="CollectionValue" CssClass="collection-value hide" runat="server" />
  </asp:Panel>
</div>