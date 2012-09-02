<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.FilePropertyEditor" %>

<div class="control-group">
    <asp:Label AssociatedControlID="FilePath" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <div class="input-append">
            <asp:HiddenField ID="FilePath" runat="server" />
            <asp:TextBox runat="server" CssClass="span3" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="btn" runat="server"><i class="icon-file"></i></asp:HyperLink>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>    
