<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ManageCourse.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Manage Course"
    Inherits="Unirecruite.ManageCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Code written by Rajat Argade on 23/10/2019 -->
    <script type="text/javascript">
    function numbersonly(myfield, e, str)
    {

        var key;

        var keychar;

        var strValidate;        

        //If str is tru means Textbox DataType is integer

        //Else decimal

        if(str == 'true')
        {
            strValidate = "0123456789";
        }

        else
        {
            strValidate = "0123456789";

        }     

         if (window.event)

            key = window.event.keyCode;

            else if (e)

            key = e.which;

            else
                return true;

            keychar = String.fromCharCode(key);            

            // control keys allowed

            if ((key==null) || (key==0) || (key==8) 

                    || (key==9) || (key==13) || (key==27) )

                return true; 

            // numbers allowed

            else if (((strValidate).indexOf(keychar) > -1))
            {
                if(keychar == ".")
                {               

                    var valEntered=document.getElementById(myfield.id).value; 

                    for(var i=0;i<valEntered.length;i++)
                    {
                        var oneChar = valEntered.charAt(i);

                        if(oneChar == ".")

                        return false;

                    }
                }

                return true;
            }

           else

                return false;
     }
     
      function addTitleAttributes()

{

   numOptions = document.getElementById('ddlCenters').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('ddlCenters').options[i].title = document.getElementById('ddlCenters').options[i].text;

   

}

      function addTitleAttributes1()

{

   numOptions = document.getElementById('ddlCourses').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('ddlCourses').options[i].title = document.getElementById('ddlCourses').options[i].text;

   

}


    </script>
    <div class="container">
        <div class="row">
      <div class="col-sm-12">
               <section class="content-header">
                    <div class="container-fluid">                        
                        <div class="row">
                            <div class="col-sm-12">
                            <h1><%=Resources.Resource.ManageCourse_Managecrs %></h1>
                                </div>
                           </div>
                        <div class="row">
                           <div class="col-sm-12">
                                 <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="Red"></asp:Label>
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
                          <h1 class="card-title"><%=Resources.Resource.ManageCourse_CntrDts%></h1>
                        </div>
                          <div class="card-body">
                            <div class="row">
                                 <div class="col-sm-4">
                                    <div class="form-group" >
                                        <label><%=Resources.Resource.ManageCourse_clsNm%></label>
                                        <asp:DropDownList ID="ddlCenters" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes();" class="form-control">
                                              <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                 </div>
                                 <div class="col-sm-4">
                                        <asp:TextBox ID="txtCenterName" Visible="false" runat="server" MaxLength="50" Columns="50"></asp:TextBox>
                                        <div class="form-group" >
                                            <label><%=Resources.Resource.ManageCourse_CrsNm%></label>
                                            <asp:DropDownList ID="ddlCourses" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes();" class="form-control">
                                                   <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <asp:TextBox ID="txtCourseName" Visible="false" runat="server" MaxLength="50" Columns="50"></asp:TextBox>
                                </div>
                                <div class="col-sm-4 button_aligned" style="text-align:center">
                                    <asp:Button ID="imgBtnSearch" ToolTip="Search" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_BtnSrch %>" />&nbsp;
                                    <asp:Button ID="imgBtnClear" ToolTip="Clear" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_btnClr %>" />&nbsp;
                                    <asp:Button ID="ImgBtnBack" runat="server" ToolTip="Back" class="btn btn-primary"  Visible="false" />
                                    <asp:Button ID="imgBtnNewCourse" runat="server" class="btn btn-primary" ToolTip="Add New Course" Text="<%$Resources: Resource, ManageCourse_btnAddNewCourse %>"/>
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
                <div id="DivGrid" class="row" runat="server" visible="false" align="left">
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
                                                        <asp:DataGrid runat="server" ID="DataGridCourseDetails" AutoGenerateColumns="False" Visible="False" DataKeyField="Course_ID" CssClass="table table-bordered table-striped table-hover" AllowPaging="True">
                                                            <FooterStyle BackColor="White" ForeColor="#189599" />
                                                            <SelectedItemStyle  />
                                                            <PagerStyle  Visible="false" HorizontalAlign="Left" Mode="NumericPages" />
                                                            <ItemStyle  HorizontalAlign="Center"/>
                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataSetIndex + 1 %>
                                                                    </ItemTemplate>
                                                                    <ItemStyle />
                                                                </asp:TemplateColumn>
                                                                <asp:BoundColumn DataField="Center_ID" Visible="False">
                                                                    <ItemStyle/>
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Course_ID" Visible="False">
                                                                    <ItemStyle/>
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Center_Name"  HeaderText="<%$Resources:Resource, ManageCourse_clsNm %>">
                                                                    <ItemStyle  HorizontalAlign="Center"/>
                                                                </asp:BoundColumn>
                                                                <asp:BoundColumn DataField="Course_Name" HeaderText="<%$Resources:Resource, ManageCourse_CrsNm %>">
                                                                    <ItemStyle HorizontalAlign="Center"/>
                                                                </asp:BoundColumn>
                                                                <%--<asp:BoundColumn DataField="Total_Marks" HeaderText="Total Marks">
                                                                                    <ItemStyle CssClass="gItemStyleNum" />
                                                                                </asp:BoundColumn>
                                                                                 <asp:BoundColumn DataField="Total_passmarks_Per" HeaderText="Passing Marks(%)">
                                                                                    <ItemStyle CssClass="gItemStyleNum" />
                                                                    </asp:BoundColumn>--%>
                                                                <asp:BoundColumn DataField="del_flag" HeaderText="del_flag" Visible="false">
                                                                    <ItemStyle/>
                                                                </asp:BoundColumn>
                                                                <%--<asp:TemplateColumn HeaderText="Update">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkEdit" CssClass="gButtonStyle" runat="server" Font-Underline="True"
                                                                                            CommandName="lnkEdit" Width="58px" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.Course_ID")%>'>Edit</asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle CssClass="gButtonStyle" />
                                                                                </asp:TemplateColumn>--%>
                                                                <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_SltDeslt%>">
                                                                    <HeaderTemplate>
                                                                        <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource,Common_Sltall%>" AutoPostBack="true"
                                                                            OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkRemove" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center"/>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#189599" HorizontalAlign="Center" Font-Bold="True" ForeColor="White"/>
                                                        </asp:DataGrid>
                                                 </div>
                                            </div>
                                       </div>

                          <%--Pagination--%>

                                        <div class="row" style="margin-top:10px">
                                            <div class="col-sm-12">
                                              <asp:Button ID="imgBtnEnable" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_Enble %>" Visible="true"/>&nbsp;
                                              <asp:Button ID="imgBtnDisable" runat="server" class="btn btn-primary" Visible="true" Text="<%$Resources: Resource, Common_Disable %>"/>
                                            </div>
                                            <%--<div class="row">
                                                  <strong><%=Resources.Resource.Common_GoToPage%></strong>&nbsp;&nbsp;&nbsp;
                                                  <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="44px" runat="server" class="select2">
                                                  </asp:DropDownList>
                                            </div>--%>                                
                                        </div>
                                    <table id="tblPagebuttons" runat="server" visible="false">
                                        <tr>
                                        </tr>
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
<!-- Code Ended By Rajat Argade on 23/10/2019 --> 
</asp:Content>