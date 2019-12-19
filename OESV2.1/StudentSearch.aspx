<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Student Search"
    CodeBehind="StudentSearch.aspx.vb" Inherits="Unirecruite.StudentSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function addTitleAttributes()
        {
            numOptions = document.getElementById('ddlcenter').options.length;
            for (i = 0; i < numOptions; i++)
                document.getElementById('ddlcenter').options[i].title = document.getElementById('ddlcenter').options[i].text;
        }
        function numbersonly(myfield, e, str)
        {
            var key;
            var keychar;
            var strValidate;
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
            if ((key==null) || (key==0) || (key==8) || (key==9) || (key==13) || (key==27) )
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
    </script>
<%--<form id="form1" runat="server">--%>
    <%--Created by Pranit Chimurkar on 2019/11/11--%>
        <!-- Content Header (Page header) -->
    <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">            
              <div class="row">
                    <div class="col-sm-12">
                            <h1><%=Resources.Resource.admin_StudList%></h1>
                    </div>
              </div>
              <div class="row">
                   <div class="col-sm-12">
                      <asp:Label runat="server" ID="errorMsg" Visible="False" ForeColor="Red"></asp:Label>
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
                          <h1 class="card-title"><%=Resources.Resource.StudentSearch_StdDts%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentSearch_CndNm %></label>
                                       <asp:TextBox runat="server" ID="txtfn" MaxLength="50" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.StudentSearch_ClsNm %></label>
                                       <asp:DropDownList ID="ddlcenter" runat="server" AutoPostBack="False" onmouseover="addTitleAttributes();" class="form-control">
                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4 button_aligned" style="text-align:center">
                                        <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources:Resource, Common_BtnSrch %>" class="btn btn-primary"></asp:Button>&nbsp;
                                        <asp:Button runat="server" ID="btnclear" CausesValidation="False" TabIndex="7" Text="<%$Resources:Resource, Common_btnClr %>" class="btn btn-primary"></asp:Button>&nbsp;
                                        <%--<asp:Button ID="btnBack" runat="server" CausesValidation="False" TabIndex="6" Text="<%$Resources:Resource, Common_btnBck %>" class="btn btn-primary"></asp:Button>--%>
                                </div>
                           </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
         </section>

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
                                       <strong><%=Resources.Resource.AdminList_TotRecord%>:<asp:Label runat="server" ID="lblrecords"></asp:Label></strong>
                                     </div>
                                </div>
                            <div class="row">
                            <div class="col-sm-12">
                            <div class="table-responsive SpecifyHeight">
                                    <asp:DataGrid ID="DGData" runat="server" AutoGenerateColumns="False" AllowPaging="True" ItemStyle-Wrap="true" CssClass="table table-bordered table-striped table-hover">
                                    <SelectedItemStyle></SelectedItemStyle>
                                    <PagerStyle HorizontalAlign="Left" Visible="false" Mode="NumericPages" />
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="SrNo" ItemStyle-Width="10%" HeaderText="<%$Resources:Resource, Common_SrNo%>">
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Userid" HeaderText="User Id" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" HeaderText="<%$Resources:Resource, StudentSearch_CndNm%>" ItemStyle-Width="19%"></asp:BoundColumn>
                                        <%--  <asp:BoundColumn DataField="SurName" HeaderText="Last name" ItemStyle-Width="15%"
                                                        ItemStyle-CssClass="gItemStyle"></asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="Center_Name" HeaderText="<%$Resources:Resource, StudentSearch_CenterNm%>" ItemStyle-Width="10%"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Email" HeaderText="<%$Resources:Resource, StudentSearch_Email%>" ItemStyle-Width="20%"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="RollNo" HeaderText="<%$Resources:Resource, StudentSearch_RNo%>" ItemStyle-Width="10%"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Birthdate" HeaderText="<%$Resources:Resource, StudentSearch_DOB%>" ItemStyle-Width="12%"></asp:BoundColumn>
                                        <%--<asp:BoundColumn DataField="LoginName" HeaderText="Login name" ItemStyle-Width="13%"                                         ItemStyle-CssClass="gItemStyle">
                                                        </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Pwd" HeaderText="Password" ItemStyle-Width="14%" ItemStyle-CssClass="gItemStyle">
                                                    </asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="Delete_Flg" HeaderText="delflag" Visible="false"></asp:BoundColumn>
                                        <asp:ButtonColumn HeaderText="<%$Resources:Resource,admin_Edit%>" CommandName="Details" Text="<%$Resources:Resource,admin_Edit%>" ItemStyle-Width="8%"></asp:ButtonColumn>
                                        <asp:TemplateColumn HeaderText="Select / Deselect">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource, Common_Sltall%>" AutoPostBack="true"
                                                    OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRemove" runat="server" Text="" AutoPostBack="True"/>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <HeaderStyle BackColor="#189599" ForeColor="#ffffff" HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                </asp:DataGrid>
                               </div>
                              </div>
                            </div>
                            
                             <%--Pagination--%>
                             <div class="row">
                            <div class="col-sm-12 SpecifyMargin">
                                            <%--<div class="row">
                                            <span><%=Resources.Resource.Common_GoToPage%></span>&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="60px" runat="server" class="form-control">
                                            </asp:DropDownList>
                                           </div>--%>
                             <%--End of Pagination--%>
                            
                                  <asp:Button ID="imgbtnenable" runat="server" Text="<%$Resources:Resource,Common_Enble %>" TabIndex="5" Visible="False" class="btn btn-primary"></asp:Button>&nbsp;
                                  <asp:Button runat="server" ID="imgbtndisable" Text="<%$Resources:Resource,Common_Disable %>" CausesValidation="false" TabIndex="6" Visible="False" class="btn btn-primary"></asp:Button>&nbsp;
                                  <asp:Button runat="server" ID="imgbtndelete" Text="<%$Resources:Resource,Common_Delete %>" CausesValidation="false" TabIndex="7" Visible="False" class="btn btn-primary"></asp:Button>
                                  <table id="tblPagebuttons" runat="server" visible="false">
                                            <tr>
                                            </tr>
                                        </table>
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
    </div>
  </div>
    <script>
             $(function () {
                 var result1 = [];
                 var labeldata1 = [];
                 $.ajax({
                     type: "POST",
                     url: "StudentSearch.aspx/GetStudentList",
                     //data: JSON.stringify({ Classid1: Classid1 }),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     error: function (XMLHttpRequest, textStatus, errorThrown) {
                         alert("Request: " + XMLHttpRequest.toString() + "\n\nStatus: " + textStatus + "\n\nError: " + errorThrown);
                     },
                     success: function (result) {
                         result1 = result.d;
                          for (var i = 0; i < result1.length; i++) {
                              labeldata1[i] = result1[i];                           
                          }
                     }
                 });   

                  $("_ctl0_ContentPlaceHolder1_txtfn").autocomplete({
                    source: labeldata1      
                 });
                
              });
  </script>   
    <%-- </form>--%>
    <%--Ended by Pranit Chimurkar--%>
<%--</form>--%>
</asp:Content>