<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageLinkPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.PageLinkPropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-6">
        <asp:HiddenField runat="server" ID="LanguageId" />
        <asp:HiddenField runat="server" ID="PageId" />
        <div class="input-group">
            <asp:TextBox runat="server" CssClass="form-control" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="input-group-addon" runat="server"><i class="icon-file-alt"></i></asp:HyperLink>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
