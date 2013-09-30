<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BooleanPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.BooleanPropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <asp:CheckBox runat="server" ID="ValueField" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>