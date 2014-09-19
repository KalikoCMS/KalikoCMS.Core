<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.TextPropertyEditor" %>
<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" Text="Text" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-10">
        <asp:TextBox runat="server" CssClass="full-width form-control" ID="ValueField" TextMode="MultiLine" Height="80" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>