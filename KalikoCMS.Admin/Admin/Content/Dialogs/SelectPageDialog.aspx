<%@ Page Title="Select page" Language="C#" AutoEventWireup="true" CodeBehind="SelectPageDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectPageDialog" MasterPageFile="Dialog.Master" %>

<asp:Content ContentPlaceHolderID="MainArea" runat="server">
    <div id="pagetree"></div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ButtonArea" runat="server">
    <button type="button" id="select-button"class="btn btn-primary disabled"><i class="icon-thumbs-up icon-white"></i> Select page</button>
    <button type="button" id="deselect-button" class="btn btn-danger"><i class="icon-trash icon-white"></i> No page</button>
    <button type="button" id="close-button" data-dismiss="modal" class="btn btn-default">Close</button>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptArea" runat="server">
    <script type="text/javascript">
        var currentPageId = "";
        var pageName = "";
        var buttonEnabled = false;

        function selectPage() {
          if (!buttonEnabled)
            return;

          top.executeCallback(currentPageId, pageName);
          close();
        }

        function noPage() {
            top.executeCallback('', '');
            close();
        }

        $(document).ready(function () {
          initTreeView();

          $('#select-button').click(selectPage);
          $('#deselect-button').click(noPage);
          $('#close-button').click(abort);
        });

        function refreshTreeNode(node) {
            $("#pagetree").jstree("load_node", node);
        }

        function initTreeView() {
            $('#pagetree').jstree({
                'plugins': ['state'],
                'core': {
                    'data': {
                        'url': 'Content/PageTree/JQueryTreeContent.ashx',
                        'type': 'POST',
                        'dataType': 'JSON',
                        'data': function(n) {
                            return {
                                'operation': 'get_children',
                                'id': n.id
                            };
                        }
                    }
                },
                'state': { 'key': 'dialog' }
            }).bind("select_node.jstree", function (event, data) {
              var pageId = data.node.id;
              pageName = data.node.text;
              currentPageId = pageId;
              $('#select-button').removeClass('disabled');
              buttonEnabled = true;
            });

        }

    </script>
</asp:Content>