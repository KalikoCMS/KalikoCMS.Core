<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.TagPropertyEditor" %>
<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-4">
        <asp:TextBox runat="server" CssClass="form-control" ID="ValueField" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
