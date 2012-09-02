<%@ Page Title="Select page" Language="C#" AutoEventWireup="true" CodeBehind="SelectPageDialog.aspx.cs" Inherits="KalikoCMS.Admin.Content.Dialogs.SelectPageDialog" MasterPageFile="Dialog.Master" %>

<asp:Content ContentPlaceHolderID="MainArea" runat="server">
    <div id="pagetree"></div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ButtonArea" runat="server">
    <a id="select-button"class="btn btn-primary disabled" href="javascript:selectPage();">Select page</a>
    <a id="deselect-button" href="javascript:noPage();" class="btn">No page</a>
    <a data-dismiss="modal" class="btn" href="javascript:close();">Close</a>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptArea" runat="server">

    <script src="/Admin/Assets/Scripts/jquery.hotkeys.js"></script>
    <script src="/Admin/Assets/Scripts/jquery.jstree.js"></script>
    <script type="text/javascript">
        var currentPageId = "";
        var pageName = "";
        var callback;
        

        function init(initValues) {
            alert(initValues+"!!!!!!");
        }

        function selectPage() {
            callback(currentPageId, pageName);
            close();
        }

        function noPage() {
            callback('', '');
            close();
        }


        $(document).ready(function () {
            initTreeView();
        });

        function refreshTreeNode(node) {
            //$("#pagetree").jstree("refresh", $(".jstree-clicked"))
            $("#pagetree").jstree("refresh", "#node_" + node);
        }

        function initTreeView() {
            var plugins = ["themes", "json_data", "ui", "crrm", "search", "types", "hotkeys"];

            $("#pagetree").jstree({
                "plugins": plugins,
                "themes": { "theme": "classic", "url": "/Admin/Assets/Styles/classic/style.css" },
                "json_data": {
                    "ajax": {
                        "url": "/Admin/Content/PageTree/JQueryTreeContent.ashx",
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
                            "icon": { "image": "/Admin/file.png" }
                        },
                        "folder": {
                            "valid_children": ["default", "folder"],
                            "icon": { "image": "/Admin/folder.png" }
                        },
                        "root": {
                            "valid_children": ["default", "folder"],
                            "icon": { "image": "/Admin/root.png" },
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
                // `data.rslt.obj` is the jquery extended node that was clicked
                var _url = data.rslt.obj.attr("url");
                var pageId = data.rslt.obj.attr("id").replace("node_", "");
                
                pageName = data.rslt.obj.text();


                currentPageId = pageId;
                $('#select-button').removeClass('disabled');
            });

        }

    </script>
</asp:Content>