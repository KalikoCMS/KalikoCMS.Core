<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.FilePropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="FilePath" runat="server" ID="LabelText" Text="File" CssClass="control-label col-xs-2" />
    <div class="controls  col-xs-6">
        <div class="input-group">
            <asp:HiddenField ID="FilePath" runat="server" />
            <asp:TextBox runat="server" CssClass="form-control" ID="DisplayField" ReadOnly="True" /><span class="input-group-btn add-on"><asp:HyperLink ID="SelectButton" CssClass="btn btn-default" runat="server"><i class="icon icon-file-o"></i></asp:HyperLink></span>
        </div>
        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>    
