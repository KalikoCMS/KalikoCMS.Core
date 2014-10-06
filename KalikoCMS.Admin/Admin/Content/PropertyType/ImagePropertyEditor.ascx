<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImagePropertyEditor.ascx.cs" Inherits="KalikoCMS.Admin.Content.PropertyType.ImagePropertyEditor" %>

<div class="form-group">
    <asp:Label AssociatedControlID="SelectButton" runat="server" ID="LabelText" Text="Image" CssClass="control-label col-xs-2" />
    <div class="controls col-xs-6">
        <div class="image-property-control">
            <asp:HyperLink ID="SelectButton" class="btn btn-primary btn-icon" runat="server"><i class="icon icon-edit icon-light"></i></asp:HyperLink>
            <asp:Image ID="ImagePreview" Width="128" Height="128" runat="server"/>
        </div>

        <asp:Literal ID="ErrorText" runat="server" />
    </div>
</div> 

<asp:HiddenField runat="server" ID="ImagePath" />
<asp:HiddenField runat="server" ID="WidthValue" />
<asp:HiddenField runat="server" ID="HeightValue" />
<asp:HiddenField runat="server" ID="AltText" />
<asp:HiddenField runat="server" ID="OriginalImagePath" />
<asp:HiddenField runat="server" ID="CropX" />
<asp:HiddenField runat="server" ID="CropY" />
<asp:HiddenField runat="server" ID="CropW" />
<asp:HiddenField runat="server" ID="CropH" />

