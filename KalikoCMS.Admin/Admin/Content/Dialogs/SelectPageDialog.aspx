<%@ Page Title="Select page" Language="C#" AutoEventWireup="true" CodeBehind="SelectPageDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectPageDialog" MasterPageFile="Dialog.Master" %>

<asp:Content ContentPlaceHolderID="MainArea" runat="server">
    <div id="pagetree"></div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ButtonArea" runat="server">
    <button type="button" id="select-button"class="btn btn-primary disabled"><i class="icon-ok"></i> Select page</button>
    <button type="button" id="deselect-button" class="btn btn-danger"><i class="icon-trash icon-white"></i> No page</button>
    <button type="button" id="close-button" data-dismiss="modal" class="btn">Close</button>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptArea" runat="server">

    <script src="assets/js/jquery.hotkeys.js"></script>
    <script src="assets/js/jquery.jstree.js"></script>
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
            $("#pagetree").jstree("refresh", "#node_" + node);
        }

        function initTreeView() {
            var plugins = ["themes", "json_data", "ui", "crrm", "search", "types", "hotkeys"];

            $("#pagetree").jstree({
              "plugins": plugins,
              "themes": { "theme": "classic", "url": "assets/css/jstree.classic.css" },
              "json_data": {
                "ajax": {
                  "url": "Content/PageTree/JQueryTreeContent.ashx",
                  "type": 'POST',
                  "data": function (n) {
                    return {
                      "operation": "get_children",
                      "id": n.attr ? n.attr("id").replace("node_", "") : "<%=Guid.Empty %>"
                    };
                  }
                },
                "data": [
                        {
                          "data": "Root",
                          "attr": { "id": "<%=Guid.Empty %>", "rel": "root" },
                          "state": "closed"
                        }
                    ]
              },
              "types": {
                "max_depth": -2,
                "max_children": -2,
                "types": {
                  "default": {
                    "icon": { "image": "file.png" }
                  },
                  "folder": {
                    "valid_children": ["default", "folder"],
                    "icon": { "image": "folder.png" }
                  },
                  "root": {
                    "valid_children": ["default", "folder"],
                    "icon": { "image": "root.png" },
                    "start_drag": false,
                    "move_node": false,
                    "delete_node": false,
                    "remove": false
                  }
                }
              },
              "ui": { "disable_selecting_children": true },
              "core": {
                "initially_open": ["<%=CurrentPage %>"]
              }
            }).bind("select_node.jstree", function (event, data) {
              var pageId = data.rslt.obj.attr("id").replace("node_", "");

              pageName = data.rslt.obj.text();


              currentPageId = pageId;
              $('#select-button').removeClass('disabled');
              buttonEnabled = true;
            });

        }

    </script>
</asp:Content>