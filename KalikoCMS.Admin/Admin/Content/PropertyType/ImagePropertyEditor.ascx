<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.ImagePropertyEditor" %>
<div>

<asp:Label AssociatedControlID="ImagePath"  runat="server" ID="LabelText" /><br/>

Imagepath:
<asp:TextBox runat="server" ID="ImagePath" /><br/>
Width:
<asp:TextBox runat="server" ID="WidthValue" /><br/>
Height:
<asp:TextBox runat="server" ID="HeightValue" /><br/>
Alt:
<asp:TextBox runat="server" ID="AltText" /><br/>
</div>
<hr/>