<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CourseMaintenance.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Course Maintenance"
    Inherits="Unirecruite.CourseMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Code Written By Rajat Argade on 22/10/2019 -->
    <style type="text/css">
        .WrapperDiv
        {
            width: 800px;
            border: 1px solid black;
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
        }
    </style>

    <script type="text/javascript">
    function numbersonly(myfield, e, str)
    {

        var key;

        var keychar;

        var strvalidate;        

        //if str is tru means textbox datatype is integer

        //else decimal

        if(str == 'true')
        {
            strvalidate = "0123456789";
        }

        else
        {
            strvalidate = "0123456789";

        }     

         if (window.event)

            key = window.event.keycode;

            else if (e)

            key = e.which;

            else
                return true;

            keychar = string.fromcharcode(key);            

            // control keys allowed

            if ((key==null) || (key==0) || (key==8) 

                    || (key==9) || (key==13) || (key==27) )

                return true; 

            // numbers allowed

            else if (((strvalidate).indexof(keychar) > -1))
            {
                if(keychar == ".")
                {               

                    var valentered=document.getelementbyid(myfield.id).value; 

                    for(var i=0;i<valentered.length;i++)
                    {
                        var onechar = valentered.charat(i);

                        if(onechar == ".")

                        return false;

                    }
                }

                return true;
            }

           else

                return false;
     }
     
      function addtitleattributes()

{

   numoptions = document.getelementbyid('ddlmaincourse').options.length;

   for (i = 0; i < numoptions; i++)

      document.getelementbyid('ddlmaincourse').options[i].title = document.getelementbyid('ddlmaincourse').options[i].text;

   

        }

        function addtitleattributesSection()

{

   numoptions = document.getelementbyid('ddlSectionDes').options.length;

   for (i = 0; i < numoptions; i++)

      document.getelementbyid('ddlSectionDes').options[i].title = document.getelementbyid('ddlSectionDes').options[i].text;

   

}

      function addtitleattributes1()

{

   numoptions = document.getelementbyid('ddlcourses').options.length;

   for (i = 0; i < numoptions; i++)

      document.getelementbyid('ddlcourses').options[i].title = document.getelementbyid('ddlcourses').options[i].text;

   

}


    </script>
<%--<form id="form1" runat="server">--%>
    

    <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">            
                   <div class="row">
                            <div class="col-sm-12">
                            <h1><%=Resources.Resource.CourseMaintenance_crsMain %></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.StudentTimeInfo_SrchExam%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                           <label><%=Resources.Resource.CourseMaintenance_MainCrs%></label>
                                           <asp:DropDownList ID="ddlMainCourse" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes();" class="form-control">
                                           </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                            <label><%=Resources.Resource.CourseMain_SectionDesc%></label>
                                            <asp:DropDownList ID="ddlSectionDes" runat="server" AutoPostBack="True" onmouseover="addtitleattributesSection();" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                           <label><%=Resources.Resource.CourseMaintenance_CrNm%></label>
                                           <asp:DropDownList ID="ddlCourses" runat="server"  onmouseover="addTitleAttributes1();" AutoPostBack="true" class="form-control">
                                                    <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                           </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-sm-3">
                                        <div class="form-group">
                                           <label><%=Resources.Resource.CourseMaintenance_TtlTime%></label>
                                           <asp:TextBox ID="txtTotalTime" runat="server" class="form-control" MaxLength="4"></asp:TextBox>
                                        </div>
                                    </div>
                             </div>
                             <div class="row">
                                  <div class="col-sm-12" style="text-align:center">
                                      <asp:Button ID="imgBtnSearch" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>" class="btn btn-primary"/>
                                      <asp:Button runat="server" ID="imgBtnClear" CausesValidation="false" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary"/>
                                      <asp:Button ID="ImgBtnBack" Visible="false" runat="server" CausesValidation="False" Text="<%$Resources: Resource, Common_btnBck %>" class="btn btn-primary"/>
                                      <asp:Button ID="imgBtnNewCourseRegistration" runat="server" Text="<%$Resources: Resource, CourseMaintainance_NCR %>" class="btn btn-primary"/>
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
           
                <div class="row" id="DivGrid" runat="server" visible="false" align="left">
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
                                                             <asp:ListItem>50</asp:ListItem>
                                                         </asp:DropDownList>&nbsp;<strong><%=Resources.Resource.Common_record%></strong>
                                                    </div>
                                                 <div style="display:flex; flex-wrap: nowrap">
                                                    <nav aria-label="Page navigation example">
                                                        <%-- Display the First Page/Previous Page buttons --%>
                                                        <ul class="pagination">
                                                          <li class="page-item"><span class="page-link">
                                                          <asp:LinkButton ID="imgfirst" runat="server" CommandArgument="0" OnClick="PagerButtonClick"><%=Resources.Resource.Common_First%></asp:LinkButton></span></li>
                              
                                                          <li class="page-item"><span class="page-link">
                                                                         <asp:LinkButton ID="imgprev" runat="server" CommandArgument="prev" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Prev%></asp:LinkButton></span></li>        
                                                        <%-- Display the Page No. buttons --%>
                               
                                                            <table id="tblPagebuttons" runat="server" visible="false">
                                                                <tr>
                                                                </tr>
                                                            </table>
                                
                                                        <%-- Display the Next Page/Last Page buttons --%>
                              
                                                            <li class="page-item"><span class="page-link">
                                                            <asp:LinkButton ID="imgnext" runat="server" CommandArgument="next" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Next%></asp:LinkButton></span></li>
                                                           <li class="page-item"><span class="page-link">
                                                                         <asp:LinkButton ID="imglast" runat="server" CommandArgument="last" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Last%></asp:LinkButton></span></li>
                                                            </ul>
                                                            </nav>
                                                                         </div>
                                                             <div>
                                                               <strong><asp:Label runat="server" ID="lblrecords"></asp:Label></strong>
                                                             </div>
                                                        </div>
                                      <div class="row">
                                        <div class="col-md-12 table-responsive SpecifyHeight">
                                    <asp:DataGrid runat="server" ID="DataGridCourseDetails" AutoGenerateColumns="False"
                                        Visible="False" DataKeyField="Course_ID" CssClass="table table-bordered table-striped table-hover" AllowPaging="True">
                                        <FooterStyle BackColor="White" ForeColor="#526B94" />
                                        <SelectedItemStyle  />
                                        <PagerStyle BackColor="Transparent" ForeColor="#000066" Visible="false" HorizontalAlign="Left" Mode="NumericPages" />
                                        <ItemStyle HorizontalAlign="Center" />
                                        <Columns>   
                                            <asp:TemplateColumn HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                                <ItemTemplate>
                                                    <%#Container.DataSetIndex + 1 %>
                                                </ItemTemplate>
                                                <ItemStyle  />
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="Course_ID" HeaderText="<%$Resources:Resource, CourseMaintenance_MainCrs %>">
                                                 <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Course_Name" HeaderText="<%$Resources:Resource, CourseMaintenance_CrNm %>">
                                                 <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Total_Time" HeaderText="<%$Resources:Resource, CourseRegistration_ttlAllotExam %>">
                                                 <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Total_Marks" HeaderText="<%$Resources:Resource,CourseRegistration_ttlMrks %>">
                                                 <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="Total_passmarks_Per" HeaderText="<%$Resources:Resource,CourseRegistration_PsingMrks %>">
                                                <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:BoundColumn>
                                            <asp:BoundColumn DataField="del_flag" HeaderText="del_flag" Visible="false">
                                                <ItemStyle  />
                                            </asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_Update%>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" Font-Underline="True"
                                                        CommandName="lnkEdit"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Course_ID")%>'>Edit</asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle/>
                                            </asp:TemplateColumn>

                                            <asp:TemplateColumn HeaderText="<%$Resources:Resource,searchQuestion_SltDselt%>">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="SelectAll" AutoPostBack="true"
                                                        OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                </HeaderTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemove" runat="server" Text="" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateColumn>
                                        </Columns>
                                        <HeaderStyle BackColor="#189599" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                    </asp:DataGrid>
                                </div>
                              </div>

                            <!-- Pagination -->
                         <div class="row SpecifyMargin">
                            <div class="col-sm-12">
                                <asp:Button ID="imgBtnEnable" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_Enble %>" Visible="true" />&nbsp;
                                <asp:Button ID="imgBtnDisable" runat="server" class="btn btn-primary" Visible="true" Text="<%$Resources: Resource, Common_Disable %>"/>&nbsp;
                                <asp:Button ID="imgBtnDelete" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_Delete %>" />
                                <%--<div class="row">
                                    &nbsp;&nbsp;<span style="color: #526B94"><%=Resources.Resource.Common_GoToPage%></span>
                                    <asp:DropDownList ID="ddlPages" AutoPostBack="true" runat="server" class="form-control">
                                    </asp:DropDownList>
                              </div>--%>
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
    
<!-- Code Ended By Rajat Argade on 22/10/2019 -->
</asp:Content>