<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VideoList.aspx.vb" Inherits="Unirecruite.VideoList"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Video List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jstree/3.2.1/themes/default/style.min.css" />
   <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.12.1/jquery.min.js"></script>
   <script src="https://cdnjs.cloudflare.com/ajax/libs/jstree/3.2.1/jstree.min.js"></script>
    
   <%-- <script src="build/js/jquery.min.js"></script>--%>

  

    <script type="text/javascript" src="jwplayer/jwplayer.js"></script>

    <script type="text/javascript" src="swfobject.js"></script>

    <script language="javascript" type="text/javascript">
    function Validate()
    {
     var objUpload=eval("document.getElementById('MyFile')");
     var sUpload=objUpload.value;    
     if(document.getElementById('MyFile').value == "")
     {
       alert("You must enter a file to process.");
                            event.returnValue=false;                           
                            return ; 
     }
     if(objUpload!="")
        {
             var iExt=sUpload.indexOf("\\");
             var iDot=sUpload.indexOf(".");            
             if((iExt < 0 ) || (iDot < 0))
             {
                    alert("Invalid file path to proceed.");
                                objUpload.focus();
                                event.returnValue=false;                                
                                return; 
              }
       if(iDot > 0)
       {
        var aUpload=sUpload.split(".");
        if((aUpload[aUpload.length-1]!="csv") && (aUpload[aUpload.length-1]!="CSV"))
        {
         alert("Only CSV file are allowed to proceed.");
         objUpload.focus();
         event.returnValue=false;            
         return; 
        }
       }
     }
    }
    </script>   
    <script type="text/javascript">
        $(function () {
            var data = [];
            var arr1 = [];
            var arr2 = [];
            var arr3 = [];
            $.ajax({
                     type: "POST",
                     url: "VideoList.aspx/GetVideoList",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     error: function (XMLHttpRequest, textStatus, errorThrown) {
                         alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                     },
                success: function (result) {
                    jsondata = result.d;
                          for (var i = 0; i < jsondata.length; i++) {
                             arr1[i] = jsondata[i].split("_")[0];
                             arr2[i] = jsondata[i].split("_")[1];                        
                              arr3[i] = jsondata[i].split("_")[2];                                 
                               data[i] = { "id": arr1[i], "parent": arr2[i], "text": arr3[i]};
                    }
                     createJSTree(data);
                }
            });         
        });
        function createJSTree(data) {            
            $('#SimpleJSTree').jstree({
                "core": {
                    "check_callback": true,
                    'data':data
                },
                "plugins": ["dnd"]
            });
        }
    </script>


    <script>
        document.addEventListener("click", function (evt) {
            var temp = (evt.target.id);
            var CategoryID = temp.slice(0, -7);
            //alert(CategoryID);
            if (CategoryID != "") {
                $.ajax({
                    type: "POST",
                    url: "VideoList.aspx/CallVideoList",
                    data: JSON.stringify({ CategoryID: CategoryID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {

                    }
                });
            }
        });
    </script>








    <%--Created by Pranit on 2019/12/13--%>
    <%--<link href="plugins/jsgrid/jsgrid-theme.css" rel="stylesheet" />
    <link href="plugins/jsgrid/jsgrid-theme.min.css" rel="stylesheet" />
    <link href="plugins/jsgrid/jsgrid.css" rel="stylesheet" />
    <script src="plugins/jsgrid/jsgrid.js"></script>
    <link href="plugins/jsgrid/jsgrid.min.css" rel="stylesheet" />
    <script>
        document.addEventListener("click", function (evt) {
            var temp = (evt.target.id);
            var CategoryID = temp.slice(0, -7);
            //alert(CategoryID);
            if (CategoryID != "") {
                var data = [];
                var arr1 = [];
                var arr2 = [];
                var arr3 = [];
                var arr4 = [];
                $.ajax({
                    type: "POST",
                    url: "VideoList.aspx/GetVideoListData",
                    data: JSON.stringify({ CategoryID: CategoryID }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                    },
                    success: function (result) {
                        jsonvideodata = result.d;
                        alert(jsonvideodata);
                        for (var i = 0; i < jsonvideodata.length; i++) {
                             arr1[i] = jsonvideodata[i].split("*#")[0];
                             arr2[i] = jsonvideodata[i].split("*#")[1];
                             arr3[i] = jsonvideodata[i].split("*#")[2];                               
                             arr4[i] = jsonvideodata[i].split("*#")[3];                               
                             data[i] = {"SrNo": arr1[i], "Title": arr2[i], "Category": arr3[i], "FileName": arr4[i]};
                        }                        
                        alert(data);
                        ShowJS(data);
                    }                    
                });
            }
        });

        function ShowJS(data) {
            $("#jsGrid1").jsGrid({
                height: "100%",
                width: "100%",

                sorting: true,
                paging: true,

                var db = {

                    loadData: function(filter) {
                        return $.grep(this.data, function(client) {
                            return (!filter.SrNo || client.SrNo.indexOf(filter.SrNo) > -1)
                                && (!filter.Title || client.Title.indexOf(filter.Title) > -1)
                                && (!filter.Category || client.Category.indexOf(filter.Category) > -1)
                                && (!filter.FileName || client.FileName.indexOf(filter.FileName) > -1);
                        });
                    },

                    insertItem: function(insertingClient) {
                        this.data.push(insertingClient);
                    },

                    updateItem: function(updatingClient) { },

                    deleteItem: function(deletingClient) {
                        var clientIndex = $.inArray(deletingClient, this.data);
                        this.data.splice(clientIndex, 1);
                    }

                };

                window.db = db;

                'data': db.data,

                fields: [
                    { name: "SrNo", type: "text", width: 20 },
                    { name: "Title", type: "text", width: 100 },
                    { name: "Category", type: "text", width: 70 },
                    { name: "FileName", type: "text", width: 80 }
                ]
            });
        }
    </script>--%>

    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR"/>
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE"/>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS"/>
   <%-- <form id="Form1" method="post" runat="server">--%>
    <asp:ScriptManager runat="server" ID="Script1">
    </asp:ScriptManager>

    <%--Created by Pranit Chimurkar on 07/11/2019--%>
    <br />
    <div class="container-fluid">
       <div class="row">
         <div class="col-sm-12">
             <section class="content">
                <div class="container-fluid">
                  <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                         <div class="card-body">                             
                            <div class="row">
                                <div class="col-sm-3 border border-info rounded" style="background-color:beige">
                                    <div class="row">
                                        <asp:UpdatePanel runat="server" ID="Up1">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="trvwCategory" EventName="trvwCategory_TreeNodePopulate" />
                                                <asp:AsyncPostBackTrigger ControlID="trvwCategory" EventName="trvwCategory_SelectedNodeChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <div style="vertical-align: top; height: 420px; overflow: auto;">
                                                    <asp:TreeView ID="trvwCategory" runat="server" Font-Size="15px" Font-Names="Arial"
                                                        ExpandDepth="0" NodeWrap="True" ShowLines="True">
                                                        <ParentNodeStyle ForeColor="#526B94" />
                                                        <SelectedNodeStyle/>
                                                        <RootNodeStyle ForeColor="#526B94" />
                                                        <Nodes>
                                                        </Nodes>
                                                    </asp:TreeView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="row">
                                        <div id="SimpleJSTree"></div>
                                    </div>

                                </div>
                                <div class="col-sm-9 border border-info rounded">
                                    <div class="row">
                                              <div class="col-sm-12" style="text-align:center">
                                                  <asp:Label runat="server" ID="errorMsg" Visible="false"></asp:Label>
                                              </div>
                                    </div>
                                    <div id="gridDiv" visible="true" runat="Server">
                                       <div class="container upOnGrid" style="margin-top:20px">
                                        <div class="row">
                                             <strong><%=Resources.Resource.Common_Show%></strong>&nbsp;
                                             <asp:DropDownList ID="PageSizeList" Autopostback="true" Width="44px" OnSelectedIndexChanged="Selection_Change" runat="server" class="select2">
                                                 <asp:ListItem>10</asp:ListItem>
                                                 <asp:ListItem>15</asp:ListItem>
                                                 <asp:ListItem>25</asp:ListItem>
                                                 <asp:ListItem>50</asp:ListItem>
                                             </asp:DropDownList>&nbsp;<strong><%=Resources.Resource.Common_record%></strong>
                                        </div>
                                     <div style="display:flex; flex-wrap: nowrap">
                                         <nav aria-label="Page navigation example">
                                                                     <ul class="pagination">
                                                                         <li class="page-item"><span class="page-link">
                                                                             <asp:LinkButton ID="imgfirst" runat="server" CommandArgument="0" OnClick="PagerButtonClick"><%=Resources.Resource.Common_First%></asp:LinkButton></span></li>
                                                                         <li class="page-item"><span class="page-link">
                                                                             <asp:LinkButton ID="imgprev" runat="server" CommandArgument="prev" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Prev%></asp:LinkButton></span></li>                                             
                                                                         <li class="page-item"><span class="page-link">
                                                                             <asp:LinkButton ID="imgnext" runat="server" CommandArgument="next" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Next%></asp:LinkButton></span></li>
                                                                         <li class="page-item"><span class="page-link">
                                                                             <asp:LinkButton ID="imglast" runat="server" CommandArgument="last" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Last%></asp:LinkButton></span></li>
                                                                     </ul>
                                                                 </nav>
                                     </div>
                                     <div>
                                       <strong><%=Resources.Resource.AdminList_TotRecord%>:<asp:Label runat="server" ID="lblrecords"></asp:Label></strong>
                                     </div>
                                </div>
                                            <div class="row">                                          
                                                    <div class="col-sm-12 table-responsive" style="height:350px">                                                    
                                                            <h2><%=Resources.Resource.CourseMaintenance_SrhResults%></h2>
                                                            <asp:DataGrid ID="DGData" runat="server"  AutoGenerateColumns="False" AllowPaging="True"
                                                                DataKeyField="Vedio_ID" CssClass="table table-bordered table-striped table-hover">
                                                                <SelectedItemStyle></SelectedItemStyle>
                                                                <PagerStyle HorizontalAlign="Left" ForeColor="#000066" Visible="false" BackColor="Transparent"
                                                                    Mode="NumericPages"></PagerStyle>
                                                                <ItemStyle></ItemStyle>
                                                                <Columns>
                                                                    <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource,CourseMaintenance_SrNo%>">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle/>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Vedio_ID" HeaderStyle-Wrap="true" Visible="false">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Title" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,VideoList_Title%>">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="CategoryName" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,VideoList_Category%>" SortExpression="CategoryName">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="FileNames" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,VideoList_FileNm%>" SortExpression="FileNames">
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                    <asp:TemplateColumn HeaderText="<%$Resources:Resource,VideoList_Delete%>">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="LnkDelete" runat="server" CommandName="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
                                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Vedio_ID")%>' OnClick="LnkDelete_Click">Delete</asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#189599" ForeColor="#ffffff" HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                                            </asp:DataGrid>

                                                        <%--Pagination--%>
                                                         <div class="row">
                                                                <div class="col-sm-6">
                                                                 
                                                                </div>
                                                                          <%--<div class="row">
                                                                                <span><%=Resources.Resource.Common_GoToPage%></span>&nbsp;&nbsp;
                                                                                <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="60px" runat="server" class="form-control">
                                                                                </asp:DropDownList>
                                                                           </div>--%>
                                                                </div>
                                                        <table id="tblPagebuttons" runat="server" visible="false">
                                                                            <tr>
                                                                            </tr>
                                                                        </table>
                                                         <%--End of Pagination--%>
                                                </div>
                                           </div>
                                       </div>
                                    </div>
                                </div>
                             </div>
                          </div>
                       </div>
                   </div>
                </div>
            </section>
          </div>
       </div>
    </div>
    <%--</form>--%>
</asp:Content>