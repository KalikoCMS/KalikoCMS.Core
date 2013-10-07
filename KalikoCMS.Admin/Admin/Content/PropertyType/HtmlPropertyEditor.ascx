<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HtmlPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.HtmlPropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls wide-input">
        <asp:TextBox runat="server" ID="ValueField" CssClass="full-width html-editor hide" TextMode="MultiLine" />
    </div>
</div>
