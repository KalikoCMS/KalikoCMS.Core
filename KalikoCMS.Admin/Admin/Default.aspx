<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KalikoCMS.Admin.Default" MasterPageFile="~/Admin/Admin.Master" %>

<asp:Content ContentPlaceHolderID="RightRegion" runat="server">
    <div class="hero-unit">
        <h1>Welcome to KalikoCMS</h1>
    </div>
    
    <button class='btn primary ajax-modal' data-backdrop='true' data-controls-modal='modal-create' data-keyboard='true' url='/Empty.htm'>New ISAT</button>
<div class='modal hide fade' id='modal-create'></div>

<script>

    $(".ajax-modal").live('click', function () {
        var url = $(this).attr('url');
        var modal_id = $(this).attr('data-controls-modal');
        //$("#" + modal_id).load(url);
        //alert(modal_id);
        //alert(url);
        $('#modal-create').css("width","400px").html("<iframe src=\"" + url + "\" style=\"width:400px;height:300px;\"></iframe>");
        $('#modal-create').modal();
        return false;
    });



</script>

</asp:Content>

