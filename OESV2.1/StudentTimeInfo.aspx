<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StudentTimeInfo.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution-----Student Time Info Details" Inherits="Unirecruite.StudentTimeInfo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <%--<form id="form1" runat="server">--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Content Header (Page header) -->
      <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-12">
                  <asp:Label runat="server" ID="errorMsg" ForeColor="Red"></asp:Label>
              </div>
            </div>
            <div class="row">
              <div class="col-sm-12">
                  <h1><%=Resources.Resource.StudentTimeInfo_Head%></h1>
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
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentTimeInfo_ClsNm%></label>
                                       <asp:DropDownList ID="sel_subjectid" runat="server" onmouseover="addTitleAttributes(sel_subjectid);" class="form-control">
                                               <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentTimeInfo_ApperExam%></label>
                                       <input id="TextFromApp" runat="server" class="form-control" onkeydown="return false"/>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentTimeInfo_CrsNm%></label>
                                       <asp:DropDownList ID="ddlcourse" runat="server" Enabled="True" onmouseover="addTitleAttributes(ddlcourse);" class="form-control">
                                               <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentTimeInfo_AppearExamTo%></label>
                                       <input id="TxtToApp" runat="server" class="form-control" onkeydown="return false"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentTimeInfo_CndNm%></label>
                                       <asp:TextBox runat="server" ID="txtCandidateName" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-6 button_aligned" style="text-align:center">
                                      <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>" class="btn btn-primary"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:Button ID="btnclear" runat="server" CausesValidation="false" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary"/>
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
                          <h1 class="card-title"><%=Resources.Resource.CourseMaintenance_SrhResults%></h1>
                        </div>
                         <div class="card-body">
                             <div class="container upOnGrid">
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
                                                    <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource,CourseMaintenance_SrNo%>">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle/>
                                                    </asp:BoundColumn>
                                                    <%--<asp:BoundColumn DataField="User_Id" HeaderText="User Id" Visible="False">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                    </asp:BoundColumn>--%>
                                                    <asp:BoundColumn DataField="Course_id" HeaderText="Course Id" Visible="False">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="attempt" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_noOFLogin%> ">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Total_Time" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_AssignExam%>">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Summ" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_ConsTme%>">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="User_Name" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_CndiNm%>">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="CenterName" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_ClsNm%>">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Course_Name" HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_CrsNm%>">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="AppearedDate" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}"
                                                        HeaderStyle-Wrap="true" HeaderText="<%$Resources:Resource,StudentTimeInfo_Written%> ">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <%-- <asp:BoundColumn DataField="Start_time" HeaderStyle-Wrap="true" HeaderText="Start Exam Date-Time">
                                                        <HeaderStyle HorizontalAlign="Center" Width="4%"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="End_time" HeaderStyle-Wrap="true" HeaderText="End Exam Date-Time">
                                                        <HeaderStyle HorizontalAlign="Center" Width="4%"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                    </asp:BoundColumn>--%>
                                                    <%--  <asp:BoundColumn DataField="Differences" HeaderStyle-Wrap="true" HeaderText="Total Duration(Minute)">
                                                        <HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                    </asp:BoundColumn>--%>
                                                    <%-- <asp:ButtonColumn ButtonType="LinkButton" CommandName="View" Text="View" HeaderText="Detail">
                                            </asp:ButtonColumn>
                                            <asp:BoundColumn DataField="qno" HeaderText="qno" Visible="False">
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                            </asp:BoundColumn>--%>
                                                    <%--  <asp:BoundColumn DataField="Result" HeaderText="Result">
                                                    <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                </asp:BoundColumn>   --%>
                                                </Columns>
                                                <HeaderStyle BackColor="#189599" ForeColor="#ffffff" HorizontalAlign="Center"></HeaderStyle>
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
                             <%--<table id="tblPagebuttons" runat="server">
                                        </table>--%>
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
  </ContentTemplate>
 </asp:UpdatePanel>
 <%--</form>--%>
</asp:Content>