<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CentreMaintenance.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Class Maintenance"
    Inherits="Unirecruite.CenterMaintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Code Written By Rajat Argade on 22/10/2019 -->
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

   numOptions = document.getElementById('ddlMainCourse').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('ddlMainCourse').options[i].title = document.getElementById('ddlMainCourse').options[i].text;

   

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
                                <h1><%=Resources.Resource.CentreMaintainance_ClsMain %></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.CenterRegistration_ClsRegDts%></h1>
                        </div>
                         <div class="card-body">
                                <div class="row">
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                              <label for="exampleInputClassName1"><%=Resources.Resource.CenterRegistration_clsNm%></label>
                                              <asp:TextBox runat="server" class="form-control" id="txtCenterName" MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_clsNm %>"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label for="exampleInputClassCode1"><%=Resources.Resource.CenterRegistration_clsCode%></label>
                                                <asp:TextBox runat="server" class="form-control" id="txtCenterCode" MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_clsCode %>"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label for="exampleInputOwnerName1"><%=Resources.Resource.CenterRegistration_OwNm%></label>
                                                <asp:TextBox runat="server" class="form-control" id="txtOwnerName" MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_OwNm %>"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-2">
                                            <div class="form-group">
                                                <label for="exampleInputCity1"><%=Resources.Resource.CenterRegistration_City%></label>
                                                <asp:TextBox runat="server" class="form-control" id="txtCity" MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_City %>"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-4 button_aligned">
                                             <asp:Button ID="imgBtnSearch" class="btn btn-primary" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>"/>&nbsp;
                                             <asp:Button ID="imgBtnClear" class="btn btn-primary" runat="server" Text="<%$Resources: Resource, Common_btnClr %>"/>&nbsp;   
                                             <asp:Button Text="<%$Resources: Resource, centerMaintenance_btnNewclass %>" class="btn btn-primary" ID="imgBtnNewCentreRegistration" runat="server" />
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
        <div id="DivGrid" class ="row" runat="server" visible="false">
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
                                <strong><asp:Label runat="server"  ID="lblrecords"></asp:Label></strong>
                            </div>
                            </div>
                           <div class="row">
                            <div class="col-sm-12">
                            <div class="table-responsive SpecifyHeight">
                        <asp:DataGrid runat="server" ID="DataGridCenterDetails" AutoGenerateColumns="False" Visible="False" DataKeyField="Center_ID" CssClass="table table-bordered table-striped table-hover" AllowPaging="True">
                            <SelectedItemStyle />
                            <PagerStyle Visible="false" HorizontalAlign="Left" BackColor="Transparent" ForeColor="#189599" Mode="NumericPages" />
                            <ItemStyle  HorizontalAlign="Center"/>
                            <Columns>
                                <asp:TemplateColumn HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                    <ItemTemplate>
                                        <%#Container.DataSetIndex + 1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center"/>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="del_flg" Visible="False">
                                    <ItemStyle/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Center_ID" Visible="False">
                                    <ItemStyle/>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Center_Name" HeaderText="<%$Resources:Resource, CenterRegistration_clsNm %>">
                                     <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Center_Code" HeaderText="<%$Resources:Resource, CenterRegistration_clsCode %>">
                                  <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Owner_Name" HeaderText="<%$Resources:Resource, CenterRegistration_OwNm %>">
                                  <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="City_Name" HeaderText="<%$Resources:Resource, CenterRegistration_City %>">
                                  <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="State_Name"  HeaderText="<%$Resources:Resource, CenterRegistration_State %>">
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Country_Name" HeaderText="<%$Resources:Resource, CenterRegistration_Country %>">
                                   <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Contact_No" HeaderText="<%$Resources:Resource, CenterRegistration_CntNo %> ">
                                  <HeaderStyle HorizontalAlign="Center"  Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="<%$Resources:Resource,admin_Edit%>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Font-Underline="True" 
                                            CommandName="lnkEdit"  CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Center_ID")%>'><%=Resources.Resource.admin_Edit%></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle/>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_SltDeslt%>">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource,Common_Sltall%>" AutoPostBack="true"
                                            OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRemove" runat="server" Text="" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateColumn>
                            </Columns>
                            <HeaderStyle BackColor="#189599" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                        </asp:DataGrid>
                        </div>
                         </div>
                        </div>                                
                            <table id="tblPagebuttons" runat="server" visible="false">
                                        <tr>
                                        </tr>
                                    </table>
                                <div class="row SpecifyMargin">
                                    <div class="col-sm-12">
                                        <asp:Button ID="imgBtnEnable" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_Enble %>" Visible="true" />&nbsp;
                                        <asp:Button ID="imgBtnDisable" runat="server" class="btn btn-primary" Visible="true" Text="<%$Resources: Resource, Common_Disable %>"/>&nbsp;
                                        <asp:Button ID="imgBtnDelete" runat="server" class="btn btn-primary" Text="<%$Resources: Resource, Common_Delete %>" />
                                    </div>
                                    <%--<div class="row"><%=Resources.Resource.Common_GoToPage%>&nbsp;&nbsp;
                                        <asp:DropDownList ID="ddlPages" AutoPostBack="true" runat="server" class="select2">
                                        </asp:DropDownList>
                                    </div>--%>
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
       <!-- Code Ended By Rajat Argade on 23/10/2019 -->
</asp:Content>
