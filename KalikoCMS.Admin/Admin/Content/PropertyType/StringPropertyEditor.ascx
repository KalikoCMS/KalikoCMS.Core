<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StringPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.StringPropertyEditor" %>
<div class="form-group">
    <asp:Label AssociatedControlID="ValueField" runat="server" ID="LabelText" Text="Text" CssClass="control-label col-xs-2" />
    <div class="col-xs-10">
        <asp:TextBox runat="server" CssClass="full-width form-control" ID="ValueField" />
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
