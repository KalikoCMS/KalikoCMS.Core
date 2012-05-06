<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StringPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.StringPropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <asp:TextBox runat="server" CssClass="span5" ID="ValueField" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
