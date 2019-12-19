<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ExamCount.aspx.vb" Inherits="Unirecruite.ExamCount"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Exam Count" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS" />

    <script language="javascript" type="text/javascript">
//      function addTitleAttributes()
//      {

//   numOptions = document.getElementById('ddlcenter').options.length;

//   for (i = 0; i < numOptions; i++)

//      document.getElementById('ddlcenter').options[i].title = document.getElementById('ddlcenter').options[i].text;
//}

  function addTitleAttributes()

{

   numOptions = document.getElementById('ddlcourse').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('ddlcourse').options[i].title = document.getElementById('ddlcourse').options[i].text;

   

}
  function addTitleAttributes1()

{

   numOptions = document.getElementById('sel_subjectid').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('sel_subjectid').options[i].title = document.getElementById('sel_subjectid').options[i].text;

   

}

</script>
    <%--Created by Pranit Chimurkar on 2019/10/18--%>
    <%--Search Condition--%>
    <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-12">
                  <h1><%=Resources.Resource.ExamCount_Head%></h1>
              </div>
            </div>
            <div class="row">
              <div class="col-sm-12">
                    <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
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
                          <h1 class="card-title"><%=Resources.Resource.ExamCount_SrchCon%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.ExamCount_CrsNm %></label>
                                       <asp:DropDownList runat="server" ID="ddlcourse" AutoPostBack="true" onmouseover="addTitleAttributes();" class="form-control">
                                            <asp:ListItem Text="<%$Resources: Resource, Search_Select %>"></asp:ListItem><%--<%$Resources:Resource, Search_Select %>">--%>
                                       </asp:DropDownList>                                       
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.ExamCount_SubNm %></label>
                                       <asp:DropDownList ID="sel_subjectid" runat="server" AutoPostBack="false" Enabled="False" onmouseover="addTitleAttributes1();" class="form-control">
                                            <asp:ListItem Text="<%$Resources: Resource, Search_Select %>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                              </div>
                            <div class="row">
                                <div class="col-sm-12" style="text-align:center">
                                        <asp:Button ID="searchbtn" runat="server" Text="<%$Resources:Resource,Common_BtnSrch %>" class="btn btn-primary"></asp:Button>&nbsp;&nbsp;
                                        <asp:Button ID="btnClear" runat="server" Text="<%$Resources:Resource,Common_btnClr %>" class="btn btn-primary"/>
                                        <%--<asp:Button ID="btnBack" runat="server" CausesValidation="False" Text="<%$Resources:Resource,Common_btnBck %>" class="btn btn-primary"/>--%>
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
        <div class="row" id="gridDiv" runat="server" visible="false">
         <div class="col-sm-12">
        <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.ExamCount_SrchRes%></h1>
                        </div>
                         <div class="card-body">
                             <div class="row">
                                 <div class="col-sm-6">
                                    <div class="d-flex flex-row">
                                          <strong><asp:Label ID="lblcount" runat="server" Visible="true"></asp:Label></strong>
                                    </div>
                                 </div>
                                 <div class="col-sm-6">
                                    <div class="d-flex flex-row-reverse">
                                          <strong><asp:Label ID="lblrecords" runat="server" Text="<%$Resources:Resource,ExamCount_TtlRcrds %>"></asp:Label></strong>
                                    </div>
                                 </div>
                             </div>
                            <div class="row">
                                <div class="col-sm-12">                                        
                                    <div class="form-group">
                                            <asp:DataGrid ID="DataGrid2" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="100%" UseAccessibleHeader="True" CssClass="table table-responsive table-bordered table-striped table-hover">
                                                <SelectedItemStyle></SelectedItemStyle>
                                                <ItemStyle></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" BackColor="#189599" ForeColor="#ffffff"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource,Common_SrNo %>" ItemStyle-Width="10%">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <%--<asp:BoundColumn DataField="test_name" ItemStyle-Width="10%" HeaderText="Subject Name">
                                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                </asp:BoundColumn>--%>
                                                <asp:BoundColumn DataField="question" ItemStyle-Width="75%" HeaderText="<%$Resources:Resource,ExamCount_Question %>">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="fre_count" ItemStyle-Width="10%" HeaderText="<%$Resources:Resource,ExamCount_Asked %>">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="qnid" ItemStyle-Width="5%" HeaderText="<%$Resources:Resource,ExamCount_test %>" Visible="false">
                                                    <ItemStyle/>
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="test_type" ItemStyle-Width="5%" HeaderText="<%$Resources:Resource,ExamCount_test %>" Visible="false">
                                                    <ItemStyle/>
                                                </asp:BoundColumn>
                                                <asp:ButtonColumn ButtonType="LinkButton" CommandName="view" ItemStyle-Width="20%" Text="<%$Resources:Resource,ExamCount_View %>" HeaderText="<%$Resources:Resource,ExamCount_View %>">
                                                </asp:ButtonColumn>
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Left" Visible="false" ForeColor="#000066" BackColor="Transparent" Mode="NumericPages"></PagerStyle>
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>

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
             <%--<table id="tblPagebuttons" runat="server">
                                            <tr>
                                            </tr>
                                        </table>--%>
            </div>
        </div>
</asp:Content>