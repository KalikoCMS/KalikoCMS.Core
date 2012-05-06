<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectPagetypeDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectPagetypeDialog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Admin/Assets/Styles/bootstrap.css" rel="stylesheet">
    <!-- IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="/Admin/Assets/Scripts/html5shiv.min.js"></script>
    <![endif]-->
    <script src="/Admin/Assets/Scripts/jquery-1.7.2.min.js"></script>
    <script src="/Admin/Assets/Scripts/bootstrap.min.js"></script>

    <script>
        function selectPageType(pageTypeId) {
            parent.createNewPage(pageTypeId);
        }

        function close() {
            parent.closeModal();
        }
    </script>
    <style>
        .modal-body { bottom: 58px; left: 0; position: fixed; right: 0; top: 47px; }        
        .modal-footer { position: fixed;left: 0;right: 0;bottom: 0;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="modal-header">
            <h3>Create new page</h3>
        </div>
        <div class="modal-body">
            <p>Select page type:</p>
            <ul class="unstyled">
                <asp:Literal ID="PageTypeList" runat="server" />
            </ul>
        </div>
        <div class="modal-footer">
            <a data-dismiss="modal" class="btn" href="javascript:close();">Close</a>
        </div>
    </form>
</body>
</html>
