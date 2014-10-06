<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageTreeControl.ascx.cs" Inherits="KalikoCMS.Admin.Content.PageTree.PageTreeControl" %>
    
    <div class="btn-group pagetree-controls">
        <a href="#" id="new-page-button" class="btn btn-sm btn-default" rel="tooltip" data-original-title="Add a new page under the selected page below." data-container="body"><i class="icon icon-plus"></i> Add page</a>        
        <a href="#" id="remove-page-button" class="btn btn-sm btn-default" rel="tooltip" data-original-title="Remove the selected page." data-container="body"><i class="icon icon-trash"></i> Remove page</a>
    </div>

    <div id="pagetree"></div>



    <script type="text/javascript">
      // TODO: Clean up code

      var currentPageId = "";

      $(document).ready(function () {
        initTreeView();
        $("#new-page-button").click(clickCreateNewPage);
        $("#remove-page-button").click(clickDeletePage);

        $("#new-page-button").tooltip({ placement: "bottom" });
        $("#remove-page-button").tooltip({ placement: "bottom" });
      });

      function clickCreateNewPage() {
        top.openModal("Content/Dialogs/SelectPagetypeDialog.aspx?pageId=" + currentPageId, 500, 400);
        return false;
      }

      function clickDeletePage() {
        if (confirm("Really delete page?")) {
          deletePage(currentPageId);
        }
        return false;
      }

      function deletePage() {
        $.ajax({
          async: false,
          type: 'POST',
          url: "Content/PageTree/JQueryTreeContent.ashx",
          data: {
            "operation": "remove_node",
            "id": currentPageId
          },
          success: function(r) {
            if (!r.status) {
              refreshTreeNode(); // TODO: Reference parent
            }
          }
        });
      }

      function createNewPage(pageTypeId) {
        top.closeModal();

        $("#maincontent").attr("src", "Content/EditPage.aspx?id=&parentId=" + currentPageId + "&pageTypeId=" + pageTypeId);
      }

      function refreshTreeNode(node) {
        $("#pagetree").jstree("refresh", "#node_" + node);
      }

      function initTreeView() {
        $("#pagetree")
          .jstree({
            "plugins": ["state", "dnd", "types"],
<%--            "json_data": {
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
            },--%>
            "types": {
              "max_depth": -2,
              "max_children": -2,
              "types": {
                "default": {
                  "icon": { "image": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAANkE3LLaAgAAAT9JREFUeJyNkSFvAkEUhOft3RVIoAaBrjl+AuJ+R9Okrg6B5AcQbAUC339Rh606GlSbYAmChDaQ1HBw+6aGuxA4lhs5+963M7uCo1qt1kO3231U1cyCqkJVEcfx12QyeYdLURQ9s0C73Y6r1YqDwWDsBHQ6nafz5cPhwPV6zdlsRpIcjUZvALzTPeOCep6HSqUCQLDdbtHr9V76/f5r6QQkmaYpN5sNp9NPxvGU8/mcYRhG2Z7v7HVM0Wg00G6HSJIEQRCg2Wy2AXyUAmSQer2OarUKYwxEJP+qUoB82PdzYCbnI5aCnhskQbJwWEQgIrDWXgKstVBVWGudgKxGYYL9fg9VdQKcFdI0vQm4WkFEvFqtdhMQBAF83/cuAMvl8nc4HH4DAK8QRESMMbJYLH5y7+T8DsB94dWX+gOQAMA/YTjF6dr/m9QAAAAASUVORK5CYII=" }
                },
                "folder": {
                  "valid_children": ["default", "folder"],
                  "icon": { "image": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAANkE3LLaAgAAAf5JREFUeJylk09rU1EQxX/vvZs2sa1FK7ZSNy7UhVhFEL+GFEFQkIK40A/gxp3uBDeCn0Fxp1B0J925E0sXtoYm8TV5SdskbZLm/cm9d1zkJZaWasEDw53FnDNnZrjwn3DS9/zNG3PX0QaUR7PRaOeLG0vHVrl35/bzKOxKrRrIZq0q5fIve2vuyl1gEjh1RHgACsBzXW80myUbhQCM5bLO5y9L79rtVst1Hedgw4mJk86jhQcv3n9YfKmAs5nTM9PfVn6gdS8tERzHxXU4uX9WSfPtTsyZC5evweKMml94/OzV6zcPrZGUClYGMv0k0bCnZbgw13G4/+TpfLAbN1Qhv1ppRZZMxsXYfoGVvpDYPqUZCh0tDIbxHAe/bc3PtbWK8tfXC34zZHJ8DG0EEUEETCqSiFBrm6F9ANeF7S6ZoFQoqe1K0c9XGsm52RMjSU9jLWiRoYt619JNLPtX6SqoNCNT3yiWFRAsl7ba0fjsVDfSWNsnW4Hd2NCKhYN38DIu5VozBl1RQH0579fj6atT4V6CtmBECLXQ6R0mA6iRDGW/ugPUFdBeWS3U9i7aS2EnwaQOelYOMwcOcjmCQmkT2FEA1WIhMOEoJhobnvJv8CTHTuDXAKsA+Pp2cSvujKNj8w9uOsOox/dPH+HPZ8oCE8doPoADdIDwmPVH4zez+Qeo9wGi9wAAAABJRU5ErkJggg==" }
                },
                "root": {
                  "valid_children": ["default", "folder"],
                  "icon": { "image": "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABGdBTUEAANkE3LLaAgAAAitJREFUeJx9kz9rFEEYxn/zZ29vd2bvvLvEKIJYSWI8QmwkRSqxC/glUuYDpUoUOSFgLcaAWPkVJHZWEVEOkvM2O7s7Y7GXuKTIAw/MDLw/nnfmHcF/PVZKPUnTtAJQSi2ONVo3e6UU0+lU53n+DTgFEC3Ap9XVtZfj8djFcYIxBmMsxhisNRiTsrS0xPHxx87h4cFn4EWDbxQBT3u9EXGcdYyxWJuRZRm9niXLGi8vj1hbGwOsAykwvwKkEKXQIc8rpKzQukLrEq1LlCpRyjGb5cRxAkQGygSYywXAQjcJQVMUAedoOeCcx7maPC9JkgxIDXAHoAVIOt5risIvigJl6RfFjS8vS6zNsHZJAPfagKFSMXUtcY5FitBaN4CiqImiiH5/BDBqAx5qnVJV4rqFomhclg2kSVZTVQFrBwDD1ivIu0J0KIpAFAU6HW44LCANwJg+IAfgo0WCMAKJc/X1hd3s37mKoqgpiooss0AYAokGop2dV8P9/X2899cTp5RCCEEIgRA83ntC8Cgl2d5eZ2/v5+jk5IOVQHdz89lwNOrR7WriWKI1SFkjRIWUNVJ6pPQIESAEHtxfYWvr+SDLMqsB41zRPz39znw+p60QAgBCNBMvheRsdoaMJb4OgziOrQb+TqfTPxsbG8xmM26TFpoJE3bZ5cv7r7/Pz88vNHBxdDR5o3X0SCkVQvDhVoiMec1Ev313MHHO/bj6jQJYAdQttW154BdQ/wPpmwtvrx3CnQAAAABJRU5ErkJggg==" },
                  "start_drag": false,
                  "move_node": false,
                  "delete_node": false,
                  "remove": false
                }
              }
            },
            "core": {
              "data":
                  {
                    'url': 'Content/PageTree/JQueryTreeContent.ashx',
                      'type': 'POST',
                      'dataType': 'JSON',
                      'data': function(n) {
                          return {
                              "operation": "get_children",
                              "id": n.id
                          };
                      }
                  }
            }
          })
          .bind("before.jstree", function (e, data) {
            if (data.func === "remove" && !confirm("Are you sure you want to delete the page?")) {
              e.stopImmediatePropagation();
              return false;
            }
          })
          .bind("select_node.jstree", function (event, data) {
            var pageId = data.selected[0];

            currentPageId = pageId;

            $("#maincontent").attr("src", "Content/EditPage.aspx?id=" + pageId);
          })
          .bind("move_node.jstree", function (e, data) {
            //data.rslt.o.each(function (i) {
            //  $.ajax({
            //    async: false,
            //    type: 'POST',
            //    url: "Content/PageTree/JQueryTreeContent.ashx",
            //    data: {
            //      "operation": "move_node",
            //      "id": $(this).attr("id").replace("node_", ""),
            //      "ref": data.rslt.cr === -1 ? 1 : data.rslt.np.attr("id").replace("node_", ""),
            //      "position": data.rslt.cp + i,
            //      "title": data.rslt.name,
            //      "copy": data.rslt.cy ? 1 : 0
            //    },
            //    success: function (r) {
            //      if (!r.success) {
            //        $.jstree.rollback(data.rlbk);
            //        alert("Could not move " + data.rslt.name);
            //      } else {
            //        $(data.rslt.oc).attr("id", "node_" + r.id);
            //        if (data.rslt.cy && $(data.rslt.oc).children("UL").length) {
            //          data.inst.refresh(data.inst._get_parent(data.rslt.oc));
            //        } else {
            //          data.inst.refresh(data.rslt.np);
            //        }
            //      }
            //    }
            //  });
            //});
          });
      }

    </script>
