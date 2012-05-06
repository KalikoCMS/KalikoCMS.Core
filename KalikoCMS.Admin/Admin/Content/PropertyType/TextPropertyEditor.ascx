<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.TextPropertyEditor" %>
<div class="control-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <asp:TextBox runat="server" CssClass="span5" ID="ValueField" TextMode="MultiLine" Height="80" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>