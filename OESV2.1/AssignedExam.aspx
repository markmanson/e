<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="AssignedExam.aspx.vb" Inherits="Unirecruite.WebForm2" Title="Assigned Exam Search" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <form id="form1" runat="server">--%>
     <div class="container">
        <div class="row">
      <div class="col-sm-12">
             <section class="content-header">
                  <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-12">
                                <h1><%=Resources.Resource.AssignedExam_AssignedExam %></h1>
                            </div>
                        </div>
                      </div><!-- /.container-fluid -->               
            </section> 

                  <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.AssignedExam_AssignedExam%></h1>
                        </div>
                           <div class="card-body">
                            <div class="row">
                                 <div class="col-sm-3">
              <%--  <fieldset>--%>
                 <%--   <legend class="outerframe">Search Condition</legend>--%>
                   <div class="form-group" >
                                  <label for="exampleInputClassName1"><%=Resources.Resource.CenterRegistration_clsNm%> </label>
                              
                       <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="True"  class="form-control">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                           </asp:DropDownList>
                       </div>
                                     </div>
                                <div class="col-sm-3">
                                <div class="form-group" >
                                  <label for="exampleInputClassName1"><%=Resources.Resource.CandStatus_CrsNm%> </label>
                                     <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="True"  class="form-control">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                           </asp:DropDownList>
                                    </div>
                                     </div>
                            <%--</td>
                            <td width="5%">
                                &nbsp;
                            </td>
                            <td align="left" width="20%">
                                <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="True" 
                                    Width="150px">
                                </asp:DropDownList>--%>
                            
                           
                                <div class="col-sm-3">
                           
                                                         <div class="form-group">
                                                        <label><%=Resources.Resource.CandStatus_ExmAssgDte%>(<%=Resources.Resource.CandStatus_From%>):</label>
                                                             <input id="txtAppFromDate" runat="server" class="form-control" onkeydown="return false"/>
                                                             </div>
                           </div>
                           <%-- <td align="left" width="20%">
                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                             <%--   <asp:TextBox ID="txtAppFromDate" class="form-control" MaxLength="10" runat="server" Width="140px"></asp:TextBox>--%>
                               <%-- <asp:ImageButton ID="imgBtnFrom" Height="20" Width="20" ImageUrl="images/calander.gif" runat="server" />
                                <cc1:CalendarExtender ID="CalExFrom" PopupPosition="Right" TargetControlID="txtAppFromDate"
                                    Format="dd/MM/yyyy" PopupButtonID="imgBtnFrom" runat="server">
                                </cc1:CalendarExtender>
                            </td>--%>
                        <%--</tr>--%>
                                <div class="col-sm-3">
                                    <label><%=Resources.Resource.CandStatus_To%>:</label>
                                    <div class="form-group ">
                                      <%--  <input id="TxtTo" runat="server" class="form-control" onkeydown="return false"/>--%>
                                         <input id="txtAppToDate" runat="server" class="form-control" onkeydown="return false"/>
                                    </div>
                                                    
                               </div>
                            </div>
                       <%-- <tr>
                            <td align="left" width="12%">
                                Course Name : 
                            </td>
                            <td width="5%">
                                &nbsp;
                            </td>
                            <td align="left" width="20%">
                               <%-- <asp:DropDownList ID="ddlCourse" runat="server" Width="150px" >
                                </asp:DropDownList>--%>
                           <%-- </td>
                            <td width="10%">
                                &nbsp;
                            </td>
                            <td align="right" width="20%">
                                (To)
                            </td>
                            <td width="5%">
                                &nbsp;
                            </td>
                            <td align="left" width="20%">
                               <%-- <asp:TextBox ID="txtAppToDate" MaxLength="10" runat="server" Width="140px"></asp:TextBox>--%>
                               <%-- <asp:ImageButton ID="imgBtnTo" Height="20" Width="20" ImageUrl="images/calander.gif" runat="server" />
                                <cc1:CalendarExtender ID="CalExTo" PopupPosition="Right" TargetControlID="txtAppToDate"
                                    Format="dd/MM/yyyy" PopupButtonID="imgBtnTo" runat="server">
                                </cc1:CalendarExtender>
                            </td>
                        </tr>
                    </table>    
                </fieldset>--%>
            <%--</td>
        </tr>--%>




<div class="row">
       <div class="col-sm-12" style="text-align:center">
             <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>" class="btn btn-primary">
                </asp:Button>
                &nbsp;
                <asp:Button runat="server" ID="btnclear" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary" CausesValidation="false">
                </asp:Button>
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

        <%--<tr id="trGrid" runat="server" visible="false">
            <td align="left">
                <fieldset>
                    <legend class="outerframe">Assigned Exam</legend>--%>

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
                                             <div class="col-sm-12">
                                        <div class="table-responsive">

                    <asp:Label ID="lblMsg" runat="server" style="color:Red; text-align:center;" Visible="false"></asp:Label>
                    <asp:GridView ID="GVExam" runat="server" AutoGenerateColumns="False"  CssClass="table table-bordered table-striped table-hover" AllowPaging="True">
                        <SelectedRowStyle  />
                        <PagerStyle HorizontalAlign="Left" ForeColor="#526B94" BackColor="Transparent" ></PagerStyle>
                        <HeaderStyle BackColor="#189599" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundField DataField="RollNo" HeaderText="<%$Resources: Resource, AssignedExam_Rollno %>" SortExpression="RollNo" />
                            <asp:BoundField DataField="ExamID" HeaderText="<%$Resources: Resource, AssignedExam_examid %>" SortExpression="ExamID" />
                            <asp:BoundField DataField="ExamPassword" HeaderText="<%$Resources: Resource, AssignedExam_Exampass %>" SortExpression="ExamPassword" />
                            <asp:BoundField DataField="ExaminationDate" HeaderText="<%$Resources: Resource, AssignedExam_Exmdate %>" SortExpression="ExaminationDate" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="LastDate" HeaderText="<%$Resources: Resource, AssignedExam_LastDate %>" SortExpression="LastDate" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="Course_Name" HeaderText="<%$Resources: Resource, AssignedExam_crsnm %>" SortExpression="CourseName" />
                            <asp:BoundField DataField="CenterName" HeaderText="<%$Resources: Resource, AssignedExam_centrnm %>" SortExpression="CenterName" />
                            <asp:TemplateField HeaderText="<%$Resources: Resource, AssignedExam_link %>">
                                <ItemTemplate >
                                    <asp:LinkButton ID="lnkBtn" runat="server" PostBackUrl="~/StudentLogin.aspx"><%=Resources.Resource.BulkImport_clickhere%></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                
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
             
  <%--  </form>--%>
</asp:Content>
