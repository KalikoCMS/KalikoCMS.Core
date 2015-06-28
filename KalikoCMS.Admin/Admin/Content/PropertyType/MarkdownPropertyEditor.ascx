   <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarkdownPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.MarkdownPropertyEditor" %>
<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-10">
        <asp:TextBox runat="server" ID="ValueField" CssClass="full-width markdown-editor" TextMode="MultiLine" />
    </div>
</div>
