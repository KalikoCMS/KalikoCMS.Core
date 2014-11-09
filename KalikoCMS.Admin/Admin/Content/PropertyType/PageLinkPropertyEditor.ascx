<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageLinkPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.PageLinkPropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" Text="Page link" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-6">
        <asp:HiddenField runat="server" ID="LanguageId" />
        <asp:HiddenField runat="server" ID="PageId" />
        <div class="input-group">
            <asp:TextBox runat="server" CssClass="form-control" ID="DisplayField" ReadOnly="True" /><span class="input-group-btn add-on"><asp:HyperLink ID="SelectButton" CssClass="btn btn-default" runat="server"><i class="icon icon-file-o"></i></asp:HyperLink></span>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
