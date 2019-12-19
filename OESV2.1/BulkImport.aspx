<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.WebForm3"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Bulk Data Import"
    CodeBehind="BulkImport.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function Validate()
        {
            var objUpload=eval("document.getElementById('MyFile')");
            var sUpload=objUpload.value;
            if(document.getElementById('MyFile').value == "")
            {
                //  var error= document.getElementById("lblcampusid");
                // error.innerHTML="Please Select A File";
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

    <style type="text/css">
        /*.entryHeader
        {
            font-size: 50px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
        }
        .cssLable
        {
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: #000000;
        }
        .cssTable
        {
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
        }
        .WrapperDiv
        {
            width: 800px;
            border: 1px solid black;
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
            position: relative;
        }*/
    </style>
    <style type="text/css">
            .button_aligned {
                margin-top: 30px;
            }
            @media (max-width: 576px){
                .button_aligned {
                margin-top:0px;
                }                
            }
    </style>
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
                  Please <a href="Excel Import/Upload Bulk User Data.zip">click here</a> to download
                user's bulk data import template file
              </div>
            </div>
              <div class="row">
                    <div class="col-sm-12">
                            <h1><%=Resources.Resource.BulkImport_BulkUserData%></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.BulkImport_UpUserDetail%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-6">
                                       <label><%=Resources.Resource.BulkImport_ChooseFile %></label>
                                        <asp:FileUpload ID="MyFile" runat="Server" contentEditable="false" lang="en-US" name="MyFile" type="file" class="form-control"/>
                                </div>
                                <div class="col-sm-6 button_aligned">
                                        <asp:Button ID="imgBtnImpData" runat="server" Text="<%$Resources:Resource, BulkImport_Import_Data %>" class="btn btn-primary"/>&nbsp;
                                        <asp:Button ID="imgBtnDetails" Enabled="false" runat="server" Text="<%$Resources:Resource, BulkImport_Import_Detail %>" class="btn btn-primary"/>&nbsp;
                                        <asp:Button ID="imgBtnBack" runat="server" Text="<%$Resources:Resource, Common_btnBck%>" class="btn btn-primary"/>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
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
                                    <asp:Label ID="lblSummary" runat="server" Font-Bold="True" Visible="False"><%=Resources.Resource.BulkImport_ImpSummary%></asp:Label><br />
                                    <asp:Label ID="lblTotal" runat="server" Visible="False"></asp:Label><br />
                                    <asp:Label ID="lblInserted" runat="server" Visible="False"></asp:Label><br />
                                    <asp:Label ID="lblDuplicate" runat="server" Visible="False"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <asp:DataGrid ID="DataGrid1" runat="server" AllowPaging="True" AutoGenerateColumns="False" Visible="False" Height="300px" CssClass="table table-responsive table-bordered table-striped table-hover">
                                            <HeaderStyle/>
                                            <SelectedItemStyle/>
                                            <ItemStyle/>
                                            <PagerStyle HorizontalAlign="Left" Visible="false" Mode="NumericPages" />
                                            <%-- <FooterStyle BackColor="White" />--%>
                                            <Columns>
                                                <asp:BoundColumn HeaderStyle-Width="20cm" DataField="name" HeaderText="Name"></asp:BoundColumn>
                                                <asp:TemplateColumn>
                                                    <HeaderStyle Width="20cm" BackColor="#189599" ForeColor="#ffffff"/>
                                                    <HeaderTemplate>
                                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataValueField="criteria"
                                                            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" SelectedIndex='<% #Session("value") %>'>
                                                            <asp:ListItem Value="0">All Records</asp:ListItem>
                                                            <asp:ListItem Value="1">Inserted Records</asp:ListItem>
                                                            <asp:ListItem Value="2">Duplicate Records</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRecords" runat="server" Text='<%# Container.DataItem("criteria") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                             <%--<table id="tblPagebuttons" runat="server" visible="false">
                                  <tr></tr>
                             </table>--%>
                            
                             <%--Pagination--%>
                             <div class="row">
                                    <div class="col-sm-6">
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