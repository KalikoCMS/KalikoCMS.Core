<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageLinkPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.PageLinkPropertyEditor" %>

<div class="control-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <asp:HiddenField runat="server" ID="LanguageId" />
        <asp:HiddenField runat="server" ID="PageId" />
        <div class="input-append">
            <asp:TextBox runat="server" CssClass="medium-input" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="btn" runat="server"><i class="icon-file-alt"></i></asp:HyperLink>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
