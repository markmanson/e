<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdminList.aspx.vb" Inherits="Unirecruite.AdminList"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Administrator Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Registration</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <script type="text/javascript">
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
       <%--Created by Pranit Chimurkar on 2019/10/16--%>
        <!-- Content Header (Page header) -->
      <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
              <div class="row">
                    <div class="col-sm-12">
                        <h1><%=Resources.Resource.AdministratorList_AdminLst %></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.AdminList_AdminSrch%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-6">
                                       <asp:TextBox runat="server" ID="txtfn" class="form-control"></asp:TextBox>                                       
                                </div>
                                <div class="col-sm-6">
                                      <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources:Resource, Common_BtnSrch %>" class="btn btn-primary"></asp:Button>&nbsp;
                                      <%--<asp:Button runat="server" ID="btnclear" Text="<%$Resources:Resource, Common_btnClr %>" CausesValidation="false" class="btn btn-primary"></asp:Button>--%>
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

        <div class="row" id="gridDiv" visible="false" runat="Server">
         <div class="col-sm-12">
             <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.AdminList_AdminSrchRes%></h1>
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
                                       <strong><%=Resources.Resource.AdminList_TotRecord%>:<asp:Label runat="server" ID="lblRecords"></asp:Label></strong>
                                     </div>
                                </div>
                            <div class="row">
                            <div class="col-sm-12">
                                <div class="table-responsive SpecifyHeight">
                                <asp:DataGrid ID="DGData" runat="server" AutoGenerateColumns="False" AllowPaging="True" CssClass="table table-bordered table-striped table-hover">
                                    <SelectedItemStyle></SelectedItemStyle>
                                    <PagerStyle HorizontalAlign="Left" Visible="false" ForeColor="#000066" BackColor="Transparent"
                                        Mode="NumericPages" ></PagerStyle>
                                    <ItemStyle></ItemStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Userid" HeaderText="<%$Resources:Resource, AdminList_UserID %>" Visible="False">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" HeaderText="<%$Resources:Resource,AdminList_AdminNm %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <%-- <asp:BoundColumn DataField="SurName" HeaderText="Last name">
                                                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                        </asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="Email" HeaderText="<%$Resources:Resource,AdminList_Email %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Birthdate" HeaderText="<%$Resources:Resource,AdminList_DOB %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="LoginName" HeaderText="<%$Resources:Resource,AdminList_LoginNm %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                                 <%-- 
                                                        <asp:BoundColumn DataField="Pwd" HeaderText="Password">
                                                            <HeaderStyle HorizontalAlign="Center" Width="14%"></HeaderStyle>
                                                            <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                        </asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="Delete_Flg" HeaderText="delflag" Visible="false"></asp:BoundColumn>                                        
                                        <asp:ButtonColumn HeaderText="<%$Resources:Resource,admin_Edit%>" HeaderStyle-Font-Bold="true" CommandName="Details" Text="<%$Resources:Resource,admin_Edit%>">
                                            <HeaderStyle HorizontalAlign="Center"/>
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:ButtonColumn>
                                        <asp:TemplateColumn HeaderText="Select / Deselect">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource,Common_Sltall%>" AutoPostBack="true"
                                                    OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRemove" runat="server" Text="" OnCheckedChanged="chkRemove_CheckedChanged1"
                                                    AutoPostBack="True" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <HeaderStyle BackColor="#189599" ForeColor="#ffffff"></HeaderStyle>
                                </asp:DataGrid>
                                </div>
                            </div>
                            </div>
                             <%--<table id="tblPagebuttons" runat="server" visible="false">
                                  <tr></tr>
                             </table>--%>
                            
                             <%--Pagination--%>

                             <%--End of Pagination--%>

                             <div class="row SpecifyMargin">
                                 <div class="col-sm-12">
                                      <asp:Button ID="imgbtnenable" runat="server" Text="<%$Resources:Resource,Common_Enble %>" Visible="False" class="btn btn-primary"></asp:Button>&nbsp;
                                      <asp:Button runat="server" ID="imgbtndisable" Text="<%$Resources:Resource,Common_Disable %>" CausesValidation="false" Visible="False" class="btn btn-primary"></asp:Button>&nbsp;
                                      <asp:Button runat="server" ID="imgbtndelete" Text="<%$Resources:Resource,Common_Delete %>" CausesValidation="false" Visible="False" class="btn btn-primary"></asp:Button>
                                    <%--<div class="row">
                                          <strong><%=Resources.Resource.Common_GoToPage%></strong>&nbsp;&nbsp;&nbsp;
                                          <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="44px" runat="server" class="select2">
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
         <script>
             $(function () {
                 var result1 = [];
                 var labeldata1 = [];
                 $.ajax({
                     type: "POST",
                     url: "AdminList.aspx/GetAdminList",
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
                         alert(labeldata1);
                     }
                 });   

                  $("_ctl0_ContentPlaceHolder1_txtfn").autocomplete({
                    source: labeldata1      
                 });
                
              });
  </script>
   <%-- </form>--%>
    <%--Ended by Pranit Chimurkar--%>
</asp:Content>