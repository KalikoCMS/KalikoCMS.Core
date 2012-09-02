<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.ImagePropertyEditor" %>

<div class="control-group">
    <asp:Label AssociatedControlID="DisplayField" runat="server" ID="LabelText" CssClass="control-label" />
    <div class="controls">
        <div class="input-append">
            <asp:TextBox runat="server" CssClass="span3" ID="DisplayField" ReadOnly="True" /><asp:HyperLink ID="SelectButton" CssClass="btn" runat="server"><i class="icon-picture"></i></asp:HyperLink>
        </div>

        <asp:TextBox runat="server" ID="AltText" />

        
        <div>
            <asp:Image ID="ImagePreview" Width="120" CssClass="thumbnail" runat="server"/>
        </div>

        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div>    

<asp:HiddenField runat="server" ID="ImagePath" />
<asp:HiddenField runat="server" ID="WidthValue" />
<asp:HiddenField runat="server" ID="HeightValue" />
