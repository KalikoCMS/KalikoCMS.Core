   <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HtmlPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.HtmlPropertyEditor" %>
<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-10">
        <asp:TextBox runat="server" ID="ValueField" CssClass="full-width html-editor hide" TextMode="MultiLine" />
    </div>
</div>
