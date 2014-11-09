<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinkPropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.LinkPropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" Text="Link" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-6">
        <asp:HiddenField runat="server" ID="Url" />
        <asp:HiddenField runat="server" ID="Type" />
        <div class="input-group">
            <asp:TextBox runat="server" CssClass="form-control" ID="DisplayField" ReadOnly="True" /><span class="input-group-btn add-on"><asp:HyperLink ID="SelectButton" CssClass="btn btn-default" runat="server"><i class="icon icon-file-o"></i></asp:HyperLink></span>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>
