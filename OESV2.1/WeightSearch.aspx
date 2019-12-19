<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WeightSearch.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Weightage Search"
    Inherits="Unirecruite.WeightSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%--Created by Pranit Chimurkar on 2019/10/23--%>
   <%-- <form id="form1" runat="server">--%>    
    <%--Search Condition--%>
  <div class="container">
    <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">            
            <div class="row">
              <div class="col-sm-12">
                  <h1><%=Resources.Resource.WeightSearch_WgtSrh%></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.WeightSearch_CrsDts%></h1>
                        </div>
                         <div class="card-body">                             
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightSearch_CrsNm %></label>
                                       <asp:TextBox runat="server" ID="txtcn" Visible="false" MaxLength="50"></asp:TextBox>
                                       <asp:DropDownList ID="ddlCourses" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightSearch_SubNm %></label>
                                       <asp:TextBox runat="server" ID="txtsn" Visible="false" MaxLength="50"></asp:TextBox>
                                       <asp:DropDownList ID="ddlSubjects" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4 button_aligned" style="text-align:center">
                                        <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources:Resource,Common_BtnSrch %>" class="btn btn-primary"></asp:Button>&nbsp;
                                        <asp:Button runat="server" ID="btnclear" CausesValidation="false" Text="<%$Resources:Resource,Common_btnClr %>" class="btn btn-primary"></asp:Button>&nbsp;
                                        <asp:Button ID="btnBack" runat="server" Visible="false" CausesValidation="false" Text="<%$Resources:Resource,Common_btnBck %>" class="btn btn-primary"></asp:Button>&nbsp;
                                        <asp:Button ID="btnNewLink" runat="server" CausesValidation="false" Text="<%$Resources:Resource,Common_Nlnk %>" class="btn btn-primary"></asp:Button>
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
                                         <strong><asp:Label ID="Label2" runat="server" Visible="true"></asp:Label></strong>
                                         <strong><asp:Label runat="server" ID="lblrecords"></asp:Label></strong>
                                     </div>
                                </div>
                            <div class="row">
                                <div class="col-md-12 table-responsive" style="height:258px">
                                            <asp:DataGrid ID="DGData" runat="server" AutoGenerateColumns="False" AllowPaging="True" ItemStyle-Wrap="true" Width="100%" CssClass="table  table-bordered table-striped table-hover">
                                                <SelectedItemStyle></SelectedItemStyle>
                                                <PagerStyle HorizontalAlign="Left" Visible="false" Mode="NumericPages" />
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                <Columns>
                                                    <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="wid" HeaderText="<%$Resources:Resource, WeightSearch_WeightID %>" Visible="False" SortExpression="wid">
                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <%-- <asp:BoundColumn DataField="Name" HeaderText="Course name" ItemStyle-Width="15%" ItemStyle-CssClass="gItemStyle">
                                                                </asp:BoundColumn>--%>
                                                    <asp:BoundColumn DataField="Course_name" HeaderText="<%$Resources:Resource, WeightSearch_CrsNm %>" ItemStyle-Width="15%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Subject_name" HeaderText="<%$Resources:Resource, WeightSearch_SubNm %>" ItemStyle-Width="15%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="sub_Weight" HeaderText="<%$Resources:Resource, WeightSearch_Weightage %>" ItemStyle-Width="13%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="single" HeaderText="<%$Resources:Resource, WeightSearch_TF %>" ItemStyle-Width="10%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="multi_choice" HeaderText="<%$Resources:Resource, WeightSearch_Mtlplec %>" ItemStyle-Width="14%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="blanks" HeaderText="<%$Resources:Resource, WeightSearch_Blank %>" ItemStyle-Width="10%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="basic" Visible="false" HeaderText="<%$Resources:Resource, WeightSearch_BasicQues %>" ItemStyle-Width="12%"></asp:BoundColumn>
                                                    <asp:BoundColumn DataField="intermed" Visible="false" HeaderText="<%$Resources:Resource, WeightSearch_InterQues %>" ItemStyle-Width="12%"></asp:BoundColumn>
                                                    <%--<asp:BoundColumn DataField="LoginName" HeaderText="Login name" ItemStyle-Width="13%"                                         ItemStyle-CssClass="gItemStyle">
                                                                    </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Pwd" HeaderText="Password" ItemStyle-Width="14%" ItemStyle-CssClass="gItemStyle">
                                                                </asp:BoundColumn>--%>
                                                    <asp:BoundColumn DataField="del_flag" HeaderText="delflag" Visible="false"></asp:BoundColumn>
                                                    <asp:ButtonColumn HeaderText="<%$Resources:Resource, CourseMaintenance_Update %>" CommandName="Edit" Text="Edit" ItemStyle-Width="8%"></asp:ButtonColumn>
                                                    <asp:ButtonColumn HeaderText="<%$Resources:Resource,WeightSearch_Rmv%>" CommandName="Remove" Text="Remove" ItemStyle-Width="8%"></asp:ButtonColumn>
                                                    <asp:BoundColumn DataField="df" ItemStyle-Width="0%" HeaderText="df" Visible="false">
                                                    </asp:BoundColumn>
                                                    <%-- <asp:TemplateColumn HeaderText="Select / Deselect">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="SelectAll" AutoPostBack="true"
                                                                            OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkRemove" runat="server" Text="" AutoPostBack="True" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateColumn>--%>
                                                </Columns>
                                                <HeaderStyle BackColor="#189599" ForeColor="#ffffff" HorizontalAlign="Center"></HeaderStyle>
                                            </asp:DataGrid>
                                </div>
                            </div>

                             <%--Pagination--%>
                             <div class="row">
                                         <div class="col-sm-12" style="margin-top:10px">
                                             <asp:Button ID="imgbtnenable" runat="server" Visible="False"></asp:Button>
                                             <asp:Button runat="server" ID="imgbtndisable" CausesValidation="false" Visible="False"></asp:Button>
                                            <%--<div class="row">
                                            <span style="color: #0071B7"><%=Resources.Resource.CourseMaintenance_GotoPageNo%></span>&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="60px" Height="35px" runat="server" class="form-control">
                                            </asp:DropDownList>
                                           </div>--%>
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
  <%--  </form>--%>
</asp:Content>