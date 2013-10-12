<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.LinkPropertyEditor" %>

<div class="control-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <asp:HiddenField runat="server" ID="Url" />
        <asp:HiddenField runat="server" ID="Type" />
        <div class="input-append">
            <asp:TextBox runat="server" CssClass="medium-input" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="btn" runat="server"><i class="icon-file-alt"></i></asp:HyperLink>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
