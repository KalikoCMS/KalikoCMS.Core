<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.FilePropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="FilePath" runat="server" ID="LabelText" CssClass="control-label col-xs-2" />
    <div class="controls  col-xs-6">
        <div class="input-group">
            <asp:HiddenField ID="FilePath" runat="server" />
            <asp:TextBox runat="server" CssClass="form-control" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="input-group-addon" runat="server"><i class="icon-file-alt"></i></asp:HyperLink>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>    
