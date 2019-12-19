<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AppearedExam.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Exam Report"
    Inherits="Unirecruite.AppearedExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style type="text/css">
        .aspNetDisabled{
            display: block;
            width: 100%;
            height: calc(1.75rem + 2px);
            padding: 1.6px 6.4px;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: grey;
            /*background-color: #fff;*/
            background-color: #dee2e6;
            background-clip: padding-box;
            border: 1px solid #ced4da;
            border-radius: .25rem;
            box-shadow: inset 0 0 0 transparent;
            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }
    </style>
    <script type="text/javascript">      
     function addTitleAttributes(field)
     {
        numOptions = document.getElementById(field.id).options.length;
        for (i = 0; i < numOptions; i++)
        document.getElementById(field.id).options[i].title = document.getElementById(field.id).options[i].text; 
     }
    </script>
    <%--Created by Pranit Chimurkar on 2019/10/21--%>
     <!-- Content Header (Page header) -->
      <div class="container">
        <div class="row">
         <div class="col-sm-12">
            <section class="content-header">
              <div class="container-fluid">
                <div class="row">
                  <div class="col-sm-12">
                      <h1><%=Resources.Resource.AppearedExam_Head%></h1>
                  </div>
                </div>
                <div class="row">
                  <div class="col-sm-12">
                      <asp:Label runat="server" ID="errorMsg" ForeColor="Red"></asp:Label>
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
                          <h1 class="card-title"><%=Resources.Resource.StudentTimeInfo_SrchExam%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentTimeInfo_ClsNm1%></label>
                                       <asp:DropDownList ID="sel_subjectid" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes(sel_subjectid);" class="form-control">
                                                <asp:ListItem Text="<%$Resources: Resource, Search_Select %>" Value="0"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.AppearedExam_CrsNm%></label>
                                       <asp:DropDownList ID="ddlcourse" runat="server" Enabled="False" onmouseover="addTitleAttributes(ddlcourse);" AutoPostBack="true" class="form-control">
                                                <asp:ListItem Text="<%$Resources: Resource, Search_Select %>" Value="0"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.AppearedExam_CndNm%></label>
                                       <asp:DropDownList runat="server" ID="ddlcandidates" Enabled="false" onmouseover="addTitleAttributes(ddlcandidates);" class="form-control">
                                                <asp:ListItem Text="<%$Resources: Resource, Search_Select %>" Value="0"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                             </div>
                             <div class="row">
                                <div class="col-sm-12" style="text-align:center">
                                      <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>" class="btn btn-primary"/>&nbsp;&nbsp;
                                      <asp:Button runat="server" ID="btnclear" CausesValidation="false" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary"/>&nbsp;&nbsp;
                                      <%--<asp:Button ID="btnBack" runat="server" CausesValidation="False" Text="<%$Resources: Resource, Common_btnBck %>" class="btn btn-primary"/>&nbsp;&nbsp;--%>
                                      <asp:Button ID="ExportAppearedExamDetails" runat="server" CausesValidation="false" Visible="true" Text="<%$Resources: Resource, Common_Export %>" class="btn btn-primary"/>
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

        <%--Search Result--%>
        <div class="row" id="gridDiv" visible="false" runat="Server">
         <div class="col-sm-12">
        <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.Common_SrchResult%></h1>
                        </div>
                         <div class="card-body">
                                 <div class="container upOnGrid">
                                        <div class="row">
                                             <strong><%=Resources.Resource.Common_Show%></strong>&nbsp;
                                             <asp:DropDownList ID="PageSizeList" Autopostback="true" Width="44px" OnSelectedIndexChanged="Selection_Change" runat="server" class="select2">
                                                 <asp:ListItem>10</asp:ListItem>
                                                 <asp:ListItem>15</asp:ListItem>
                                                 <asp:ListItem>25</asp:ListItem>
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
                                       <strong><%=Resources.Resource.AdminList_TotRecord%><asp:Label runat="server" ID="lblrecords"></asp:Label></strong>
                                     </div>
                                </div>
                            <div class="row">
                                <div class="col-sm-12">                                        
                                    <div class="table-responsive SpecifyHeight">
                                         <asp:DataGrid ID="DGData" runat="server" AutoGenerateColumns="False" AllowPaging="True" CssClass="table table-bordered table-striped table-hover">
                                            <SelectedItemStyle></SelectedItemStyle>
                                            <PagerStyle HorizontalAlign="Left" Visible="false" ForeColor="#000066" BackColor="Transparent" Mode="NumericPages"></PagerStyle>
                                            <ItemStyle></ItemStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle/>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Userid" HeaderText="<%$Resources:Resource, AppearedExam_UserID %>" Visible="False">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="test_type" HeaderText="<%$Resources:Resource, AppearedExam_testTyp %>" Visible="False">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="question" HeaderText="<%$Resources:Resource, AppearedExam_Ques %>">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="total_marks" HeaderText="<%$Resources:Resource, WeightMgt_ttlMrks %>">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="obtained_marks" HeaderText="<%$Resources:Resource, CandStatus_MrkObt %>">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="View" Text="<%$Resources:Resource, AppearedExam_View %>" HeaderText="<%$Resources:Resource, AppearedExam_dts %>">
                                                </asp:ButtonColumn>
                                                <asp:BoundColumn DataField="qno" HeaderText="<%$Resources:Resource, AppearedExam_qno %>" Visible="False">
                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                            </Columns>
                                            <HeaderStyle BackColor="#189599" ForeColor="#ffffff"></HeaderStyle>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>

                            <%--Pagination--%>
                                            <%--<div class="row">
                                            <span style="color: #0071B7"><%=Resources.Resource.Common_GoToPage%></span>&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="60px" Height="35px" runat="server" class="form-control">
                                            </asp:DropDownList>
                                           </div>--%>

                             <%--End of Pagination--%>
                             <table id="tblPagebuttons" runat="server">
                                        </table>
                           </div>
                      </div>
                    </div>
                    </div>
                  </div>
         </section>
        </div>
        </div>
     </div>
 <%--Ended By Pranit Chimurkar on 2019/10/21--%>
</asp:Content>