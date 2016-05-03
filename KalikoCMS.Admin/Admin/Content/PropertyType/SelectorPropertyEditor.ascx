<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectorPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.SelectorPropertyEditor" %>
<div class="form-group">
    <asp:Label AssociatedControlID="Items" runat="server" ID="LabelText" Text="Text" CssClass="control-label col-xs-2" />
    <div class="col-xs-10">
        <asp:Literal runat="server" ID="Items" />
        <asp:DropDownList ID="Value" runat="server"/>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
