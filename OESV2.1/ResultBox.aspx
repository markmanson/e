<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ResultBox.aspx.vb" Inherits="Unirecruite.WebForm3" 
    title="Result Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<%--<form id="form1" runat="server">--%>
      <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">            
                   <div class="row">
                            <div class="col-sm-12">
                            <h1><%=Resources.Resource.ResultBox_Result%></h1>
                                </div>
                           </div>
              </div>
            </section>

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
                            <label> <%=Resources.Resource.Search_ClsNm%></label>
                                             <asp:DropDownList ID="ddlClass" runat="server" class="form-control" AutoPostBack="True"> 
                                   
                                </asp:DropDownList>
                           </div>
                                        </div>
                           
                            <div class="col-sm-3">
                                        <div class="form-group">
                            <label> <%=Resources.Resource.Search_CrsNm%></label>
                                             <asp:DropDownList ID="ddlCourse" runat="server" class="form-control" AutoPostBack="True"> 
                                   
                                </asp:DropDownList>
                           </div>
                                        </div>
                     
                            <div class="col-sm-3">
                                        <div class="form-group">
                              <label> <%=Resources.Resource.ResultBox_Examapdat%>(<%=Resources.Resource.CandStatus_From%>):</label>
                            <input id="txtAppFromDate" runat="server" class="form-control" onkeydown="return false"/>
                                            </div>
                                </div>
                                 <div class="col-sm-3">
                                     <label> <%=Resources.Resource.CandStatus_To%> :</label>
                                <div class="form-group">
                                    <input id="txtAppToDate" runat="server" class="form-control" onkeydown="return false"/>
                                    </div>
                                     </div>
                                </div>
                             
                               <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                               
                                <asp:ImageButton ID="imgBtnFrom" Height="20" Width="20" ImageUrl="images/calander.gif" runat="server" />
                                <cc1:CalendarExtender ID="CalExFrom" PopupPosition="Right" TargetControlID="txtAppFromDate"
                                    Format="dd/MM/yyyy" PopupButtonID="imgBtnFrom" runat="server">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>--%>
                        <%--<tr>
                            <td align="left" width="12%">
                                Course Name : 
                            </td>
                            <td width="5%">
                                &nbsp;
                            </td>
                            <td align="left" width="20%">
                               <%-- <asp:DropDownList ID="" runat="server" Width="150px" >
                                </asp:DropDownList>
                            </td>--%>
                            <%--<td width="10%">
                                &nbsp;
                            </td>
                            <td align="right" width="20%">
                                (To)
                            </td>
                            <td width="5%">
                                &nbsp;
                            </td>
                            <td align="left" width="20%">
                                <asp:TextBox ID="txtAppToDate" MaxLength="10" runat="server" Width="140px"></asp:TextBox>
                                <asp:ImageButton ID="imgBtnTo" Height="20" Width="20" ImageUrl="images/calander.gif" runat="server" />
                                <cc1:CalendarExtender ID="CalExTo" PopupPosition="Right" TargetControlID="txtAppToDate"
                                    Format="dd/MM/yyyy" PopupButtonID="imgBtnTo" runat="server">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>--%>
                    
       <div class="row">
           <div class="col-sm-12" style="text-align:center">
                <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>" class="btn btn-primary">
                </asp:Button>
                &nbsp;
                <asp:Button runat="server" ID="btnclear" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary"  CausesValidation="false" />
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





          <div class="row" id="trGrid" runat="server" visible="false" align="left">
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
                                          <div class="container" style="margin-bottom:10px">
                                        <div class="row">
                                             <strong><%=Resources.Resource.Common_Show%></strong>&nbsp;
                                             <asp:DropDownList ID="PageSizeList" Autopostback="true" Width="44px" OnSelectedIndexChanged="Selection_Change" runat="server" class="select2">
                                                 <asp:ListItem>10</asp:ListItem>
                                                 <asp:ListItem>15</asp:ListItem>
                                                 <asp:ListItem>25</asp:ListItem>
                                             </asp:DropDownList>&nbsp;<strong><%=Resources.Resource.Common_record%></strong>
                                        </div>
                                    </div>
                                       <asp:Label runat="server" ID="lblMsg" ForeColor="Red"></asp:Label>
                                      <div class="row">
                                        <div class="col-sm-12">
                                        <div class="table-responsive SpecifyHeight">


        <%--<tr id="trGrid" runat="server" visible="false">
            <td align="left">
                <fieldset>
                    <legend class="outerframe">Search Result</legend>
                    <asp:Label ID="lblMsg" runat="server" style="color:Red; text-align:center;" Visible="false"></asp:Label>--%>
                    <asp:GridView ID="GVResult" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-hover" AllowPaging="True">
                        <SelectedRowStyle  />
                        <PagerStyle HorizontalAlign="Center" ForeColor="#000066" BackColor="Transparent" ></PagerStyle>
                        <HeaderStyle BackColor="#189599" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <Columns>
                            <%--<asp:BoundField DataField="Sr. No." HeaderText="Sr. No."  />--%>
                            <asp:BoundField DataField="Center_Name" HeaderText="<%$Resources: Resource, AssignedExam_centrnm %>" SortExpression="Center_Name" />
                            <asp:BoundField DataField="Course_Name" HeaderText="<%$Resources: Resource, AssignedExam_crsnm %>" SortExpression="Course_Name" />
                            <%--<asp:BoundField DataField="ExamID" HeaderText="Exam ID" SortExpression="ExamID" />
                            <asp:BoundField DataField="ExamPassword" HeaderText="Exam Password" SortExpression="ExamPassword" />
                            <asp:BoundField DataField="writtentestdate" HeaderText="Exam Assigned Date" SortExpression="writtentestdate" />--%>
                            <asp:BoundField DataField="appearancedate" HeaderText="<%$Resources: Resource, CandStatus_ExamAppearedDate %>" SortExpression="appearancedate" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="total_marks" HeaderText="<%$Resources: Resource, CourseMaintenance_TtlMrks %>" SortExpression="total_marks" />
                            <asp:BoundField DataField="obtained_marks" HeaderText="<%$Resources: Resource, ResultBox_obtainedmarks %>" SortExpression="obtained_marks" />
                            <asp:BoundField DataField="Grad" HeaderText="<%$Resources: Resource, CandStatus_Grade %>" SortExpression="Grad" />
                            <asp:BoundField DataField="Status" HeaderText="<%$Resources: Resource, CandStatus_Sts %>" SortExpression="Status" />
                        </Columns>
                    </asp:GridView>
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
<%--    </form>--%>
</asp:Content>
