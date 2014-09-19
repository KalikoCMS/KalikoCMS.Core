<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.LinkPropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" Text="Link" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-6">
        <asp:HiddenField runat="server" ID="Url" />
        <asp:HiddenField runat="server" ID="Type" />
        <div class="input-group">
            <asp:TextBox runat="server" CssClass="form-control" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="input-group-btn" runat="server"><i class="btn btn-default icon-file-alt"></i></asp:HyperLink>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
