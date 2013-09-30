<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NumericPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.NumericPropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <asp:TextBox runat="server" CssClass="medium-input" ID="ValueField" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
