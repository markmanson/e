<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SubjectRegistration.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Subject Registration"
    Inherits="Unirecruite.SubjectRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Code is written by Rajat Argade on 5/11/2019 -->
    <script language="javascript" type="text/javascript">
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
        function IsValid(val)
        {
            var flag = true;
            if(val != "")
            {
                if(!isNaN(val) )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        function addTitleAttributes(field)
        {
            numOptions = document.getElementById(field.id).options.length;
            for (i = 0; i < numOptions; i++)
            document.getElementById(field.id).options[i].title = document.getElementById(field.id).options[i].text; 
        }
    </script>
   
    <script type="text/javascript">
        $(function(){
            $(".multiselect").multiselect();
        });
    </script>
    <script type="text/javascript">
        $(function(){
            $(".multiselect").multiselect();
        });
    </script>
    <style type="text/css">
        input[type=text]{
    display: block;
    width: 100%;
    height: calc(1.5rem + 2px);
    padding: .1rem .5rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #495057;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid #ced4da;
    border-radius: .25rem;
    box-shadow: inset 0 0 0 transparent;
    transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
}
    </style>
    <div class="container">
        <div class="row">
      <div class="col-sm-12">
               <section class="content-header">
                    <div class="container-fluid">                            
                         <div class="row">
                            <div class="col-sm-12">
                            <h1><%=Resources.Resource.SubjectRegistration_SubReg %></h1>
                                </div>
                           </div>
                           <div class="row">
                              <div class="col-sm-12">
                                <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
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
                                                 <h1 class="card-title"><%=Resources.Resource.SubjectRegistration_Subdet%></h1>
                        </div>  
                           <div class="card-body">
                                <div class="row">
                                     <div class="col-sm-4">
                                          <label><%=Resources.Resource.SubjectRegistration_SubNm%></label> 
                                          <asp:TextBox runat="server" class="form-control" id="txtSubjectName" MaxLength="255" placeholder="<%$Resources:Resource, SubjectRegistration_SubNm %>"></asp:TextBox>    
                                     </div>
                                     <div class="col-sm-4">
                                         <label><%=Resources.Resource.SubjectRegistration_SubCode%></label> 
                                         <asp:TextBox runat="server" class="form-control" id="txtSubCode" MaxLength="255" placeholder="<%$Resources:Resource, SubjectRegistration_SubCode %>"></asp:TextBox>
                                     </div>
                                     <div class="col-sm-4 button_aligned" style="text-align:center">
                                         <asp:Button ID="imgBtnSearch" class="btn btn-primary" Text="<%$Resources: Resource, Common_BtnSrch%>" runat="server"/>&nbsp;
                                         <asp:Button ID="imgBtnClear" class="btn btn-primary" Text="<%$Resources: Resource, Common_btnClr%>" runat="server"/>&nbsp;
                                         <asp:Button ID="imgBtnBack" class="btn btn-primary"  Visible="false" runat="server" />&nbsp;
                                         <asp:Button ID="imgBtnSubmit" class="btn btn-primary" Text="<%$Resources: Resource, Common_Regis %>" runat="server" />&nbsp;
                                        <%-- <asp:Button ID="imgBtnUpdate" class="btn btn-primary" runat="server" Text="<%$Resources: Resource, Common_Update %>"  Visible="false" />--%>
                                     </div>
                                </div>
                                                <tr runat="server" id="chkrow" visible="false" align="left">
                                                    <td align="left" class="tdcontent_label">
                                                        <span class="genText">To register subject as a course?</span>
                                                    </td>
                                                    <td class="tdcontent_data" align="left" colspan="2">
                                                        <asp:CheckBox runat="server" ID="chkcourse" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <%--End: Jatin Gangajaliya, 2011/03/23--%>


                                        <td style="width: 60%" align="left" valign="top" class="tdcontent_main">
                                            <table cellspacing="0" width="100%" class="content" border="thin">
                                                <tr runat="server" id="maincourserow" visible="false">
                                                    <td style="width: 18%">
                                                        <span class="staticText">Main Course <a class="mand">*</a></span>
                                                    </td>
                                                    <td class="tdcontent_data" align="left" colspan="2">
                                                        <asp:DropDownList ID="ddlMainCourse" runat="server" Width="180px" onmouseover="addTitleAttributes(ddlMainCourse);">
                                                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <%--  <tr runat="server" id="centrerow" visible="false">
                                                    <td  class="staticText " >
                                                        Centre Name <span class="mand" id="updatespan" runat="server">*</span>
                                                    </td>
                                                    <td colspan="2" class="tdcontent_data" align="left">
                                                        <asp:ListBox runat="server" ID="multiBoxIdcenter" CssClass="multiselect" SelectionMode="Multiple"
                                                            Width="460px"></asp:ListBox>
                                                    </td>
                                                </tr>--%>
                                                <tr runat="server" id="textboxcoderow" visible="false">
                                                    <td>
                                                        <span class="staticText">Course Code <a class="mand">*</a></span>
                                                    </td>
                                                    <td colspan="2" align="left">
                                                        <asp:TextBox ID="txtcoursecode" runat="server" MaxLength="4" Width="175px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="textboxrow" visible="false">
                                                    <td>
                                                        <span class="staticText">Total Alloted Exam Time(Minute) <a class="mand">*</a></span>
                                                    </td>
                                                    <td colspan="2" class="style3" align="left">
                                                        <asp:TextBox ID="txtTotalTime" runat="server" MaxLength="3" Width="175px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="textMarksrow" visible="false">
                                                    <td>
                                                        <span class="staticText">Total Marks <a class="mand">*</a></span>
                                                    </td>
                                                    <td colspan="2" class="style7" align="left">
                                                        <asp:TextBox ID="txtTotalMarks" runat="server" MaxLength="3" Width="175px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="textPassmarksrow" visible="false">
                                                    <td>
                                                        <span class="staticText">Pasing Marks(%) <a class="mand">*</a></span>
                                                    </td>
                                                    <td colspan="2" class="tdcontent_data" align="left">
                                                        <asp:TextBox ID="txtPassMarks" runat="server" MaxLength="3" Width="175px"></asp:TextBox>
                                                    </td>
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
        
                <div id="gridDiv" class="row" style="overflow: auto" visible="false" runat="server">
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
                               <div class="container upOnGrid">
                                        <div class="row">
                                                <strong><%=Resources.Resource.Common_NoOfRecords%></strong>&nbsp;&nbsp;
                                                <asp:DropDownList ID="PageSizeList" Autopostback="true" Width="44px" OnSelectedIndexChanged="Selection_Change" runat="server" class="select2">
                                                    <asp:ListItem>10</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                </asp:DropDownList>
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
                                      <strong><asp:Label runat="server"  ID="LblTotalRecCnt"></asp:Label></strong>
                                     </div>
                                </div>
                            
                           <div class="row">
                            <div class="col-md-12">
                            <div class="form-group">
                                 <div class="table-responsive" style="height:263px">
                            <asp:DataGrid runat="server" ID="DataGridSubjectDetails" DataKeyField="test_type" AutoGenerateColumns="false" 
                                Visible="False" CssClass="table table-bordered table-striped table-hover" AllowPaging="True">
                                <HeaderStyle />
                                <%--<FooterStyle BackColor="White" ForeColor="#0071B7" />--%>
                                <SelectedItemStyle />
                                <PagerStyle  HorizontalAlign="Left" Visible="false" Mode="NumericPages" />
                                <ItemStyle />
                                <Columns>
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderText="<%$Resources:Resource, centerMaintenance_SrNo %>">
                                        <ItemTemplate>
                                        <%#Container.DataSetIndex + 1 %>
                                    </ItemTemplate>
                                    </asp:TemplateColumn>                                    
                                    <asp:TemplateColumn HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:Resource, SubjectRegistration_SubCode %>" Visible="true" >
                                        <ItemTemplate>
                                            <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "Sub_Code") %>' runat="server"/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox id="txtsubcodegrid" runat="server" 
                                           text='<%# DataBinder.Eval(Container.DataItem, _
                                                                                 "Sub_Code") %>' />
                                        </EditItemTemplate>
                                        <ItemStyle ></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn  ItemStyle-HorizontalAlign="Center" HeaderText="<%$Resources:Resource, SubjectRegistration_SubNm %>">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Label Text='<%# DataBinder.Eval(Container.DataItem, "test_name") %>' runat="server"/>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox id="txttestnamegrid" runat="server" 
                                           text='<%# DataBinder.Eval(Container.DataItem, _
                                                                                 "test_name") %>' />
                                        </EditItemTemplate>
                                        <ItemStyle ></ItemStyle>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn  DataField="del_flag" ReadOnly="true" HeaderText="del_flag"
                                        Visible="false" >
                                        <ItemStyle ></ItemStyle>
                                    </asp:BoundColumn>
                                     <asp:EditCommandColumn  ButtonType="LinkButton" HeaderText="Edit"
                                                         EditText="Edit"
                                                         CancelText="Cancel"
                                                         UpdateText="Update" ItemStyle-HorizontalAlign="Center"/>
                                   


                                    <%--<asp:TemplateColumn HeaderText="<%$Resources:Resource,admin_Edit%>"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" >
                                        <ItemTemplate>

                                            <asp:LinkButton ID="lnkEdit" runat="server"  CommandName="lnkEdit"
                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.test_type")%>'>Edit</asp:LinkButton>
                                        </ItemTemplate>
                                        
                                        <ItemStyle />
                                    </asp:TemplateColumn>--%>
                                    <asp:BoundColumn DataField="test_type" Visible="false" />
                                    <%--<asp:TemplateColumn Visible="false">
                                        <ItemTemplate>
                                            <asp:HiddenField id="hfid" runat="server" 
                                           value='<%# DataBinder.Eval(Container.DataItem, _
                                                                                             "test_type") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateColumn>--%>
                                   
                                     
                                    <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_Sltall%>" >
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource,Common_Sltall%>" AutoPostBack="true"
                                                OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRemove" runat="server" Text="" OnCheckedChanged="chkRemove_CheckedChanged" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateColumn>                                    
                                </Columns>
                                <HeaderStyle  BackColor="#189599" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                            </asp:DataGrid>
                                     </div>
                                </div>
                                </div>
                               </div>
                   <div class="container bottomOfGrid">
                                    <div>
                                      <asp:Button ID="imgBtnEnable" runat="server" Text="<%$Resources:Resource,Common_Enble %>"  class="btn btn-primary"></asp:Button>&nbsp;
                                      <asp:Button runat="server" ID="imgBtnDisable" Text="<%$Resources:Resource,Common_Disable %>" CausesValidation="false"  class="btn btn-primary"></asp:Button>&nbsp;
                                      <asp:Button runat="server" ID="imgBtnDelete" Text="<%$Resources:Resource,Common_Delete %>" CausesValidation="false" class="btn btn-primary"></asp:Button>
                                    </div>
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
                 
                    </section>
                    </div>
       </div>
    </div>
  <!-- Code is Ended by Rajat Argade on 6/11/2019 -->
</asp:Content>
