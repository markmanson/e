<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.BulkQuestionsInsert"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Bulk Question Import"
    CodeBehind="BulkQuestionsImport.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    $('form').live("submit", function () {
        ShowProgress();
    });
</script>
    <script runat="server">

    Protected Sub btnDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    </script>

     <style type="text/css">
            #ContentPlaceHolder1_MyFile{
                display: block;
                width: 100%;
                height: calc(1.75rem + 2px);
                padding: .0rem .75rem;
                font-size: 1rem;
                font-weight: 400;
                line-height: 1.5;
                color: #495057;
                background-color: #fff;
                background-clip: padding-box;
                border: 1px solid #ced4da;
                border-radius: .25rem;
                box-shadow: inset 0 0 0 transparent;
                transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            }
            .btn-default {
                background-color: #f8f9fa;
                border-color: #ddd;
                color: #444;
                width: 300px;
            }
            .aspNetDisabled{
                display: inline-block;
                font-weight: 400;
                color: #fff;
                background-color: #007bff;
                border-color: #007bff;
                text-align: center;
                vertical-align: middle;
                -webkit-user-select: none;
                -moz-user-select: none;
                -ms-user-select: none;
                user-select: none;
                border: 1px solid;
                padding: .375rem .75rem;
                font-size: 1rem;
                line-height: 1.5;
                border-radius: .25rem;
                opacity: .65;
                box-shadow: none;
                transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out;
            }
    </style>

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
<%--<form id="form1" runat="server">--%>
    <%--Created by Pranit Chimurkar on 2019/11/12--%>
        <!-- Content Header (Page header) -->
    <%--<form id="form1" runat="server">--%>
    <div class="container" id="tbl_feedback" runat="server" onblur="javascript:fnc_limit_txt_desc()">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-12">
                  <strong><%=Resources.Resource.BulQImpo_Download%></strong><br />
                  <a href="Excel Import/Bulk Question Import Format.xlt" class="badge badge-info"><%=Resources.Resource.BulQImpo_TFMul%></a>
                  <a href="Excel Import/Bulk Question Import Format_Blanks.xlt" class="badge badge-info"><%=Resources.Resource.BulQImpo_FillBlank%></a>
              </div>
            </div>
          </div><!-- /.container-fluid -->
        </section>

        <!-- main content -->
        <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.BulQImpo_UpQDet%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-6">
                                       <label><%=Resources.Resource.BulkImport_ChooseFile %></label><br />
                                    <div class="btn btn-default btn-file">
                                    <i class="fas fa-paperclip"></i><%=Resources.Resource.Common_ChoosePhoto%>
                                    <asp:FileUpload lang="en-US" ID="MyFile" contentEditable="false" type="file" name="MyFile" runat="Server" class="form-control"/>
                                </div>
                                       
                                </div>
                                <div class="col-sm-6 button_aligned" style="text-align:right">
                                        <asp:Button ID="imgBtnImpData" Text="<%$Resources:Resource, BulkImport_Import_Data %>" class="btn btn-primary" runat="server"/>&nbsp;
                                        <asp:Button ID="imgBtnDetails" Text="<%$Resources:Resource, BulkImport_Import_Detail %>" class="btn btn-primary" runat="server" Enabled="false"/>&nbsp;
                                        <asp:Button ID="imgBtnBack" runat="server" Text="<%$Resources:Resource, Common_btnBck%>" class="btn btn-primary"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblCsvFormat" runat="server" ForeColor="Red"></asp:Label>
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
        
        <div class="row" id="gridDiv" visible="false" runat="server">
         <div class="col-sm-12">
             <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.BulkImport_Import_Detail%></h1>
                        </div>
                         <div class="card-body" id="tblrow2" runat="server">
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblSummary" runat="server" Visible="False" Font-Bold="True"><%=Resources.Resource.BulkImport_ImpSummary%></asp:Label>
                                    <asp:Label ID="lblTotal" runat="server" Visible="False"></asp:Label><br />
                                    <asp:Label ID="lblInserted" runat="server" Visible="False"></asp:Label><br />
                                    <asp:Label ID="lblDuplicate" runat="server" Visible="False"></asp:Label><br />
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <asp:DataGrid ID="DataGrid1" runat="server"  Visible="False" AutoGenerateColumns="False" AllowPaging="True" Height="300px" CssClass="table table-responsive table-bordered table-striped table-hover">
                                            <SelectedItemStyle></SelectedItemStyle>
                                            <ItemStyle></ItemStyle>
                                            <HeaderStyle BackColor="#189599" ForeColor="#ffffff"></HeaderStyle>
                                            <FooterStyle BackColor="White"></FooterStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="name" HeaderText="Questions Name"></asp:BoundColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderTemplate>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" Width="153px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                                                            DataValueField="criteria" AutoPostBack="True" SelectedIndex='<% #Session("value") %>'>
                                                            <asp:ListItem Value="0">All Records</asp:ListItem>
                                                            <asp:ListItem Value="1">Inserted Records</asp:ListItem>
                                                            <asp:ListItem Value="2">Duplicate Records</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRecords" runat="server" Text='<%# Container.DataItem("criteria") %>'
                                                            Width="145px">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Left" Visible="false" ForeColor="#000066" BackColor="White"
                                                Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                             <%--<table id="tblPagebuttons" runat="server" visible="false">
                                  <tr></tr>
                             </table>--%>
                            
                             <%--Pagination--%>
                             <div class="row">
                                    <div class="col-sm-6" runat="server" id="tblpages">
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
                                         <div class="col-sm-6">
                                            <div class="row">
                                                <span style="color: #0071B7"><%=Resources.Resource.Common_GoToPage%></span>&nbsp;&nbsp;&nbsp;
                                                <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="60px" Height="35px" runat="server" class="form-control">
                                                </asp:DropDownList>
                                           </div>
                                         </div>
                             </div>
                             <div class="loading" align="center">
                                Loading. Please wait.<br />
                                <br />
                                <img src="images/loader.gif" alt="" />
                            </div>
                             <%--End of Pagination--%>
                        </div>
                      </div>
                    </div>
                  </div>
               </div>
         </section>
        </div>
       </div>
     </div>
   <%-- </form>--%>
    <%--Ended by Pranit Chimurkar--%>
<%--</form>--%>
</asp:Content>