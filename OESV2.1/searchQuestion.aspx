<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.searchQuestion"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Question Maintenance"
    CodeBehind="searchQuestion.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS" />
    <style type="text/css">
        /*.entryHeader
        {
            font-size: 50px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
        }
        .genLbl
        {
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: Black;
        }
        .WrapperDiv
        {
            width: 100%;
            border: 1px solid black;
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            position:relative;
        }
        .HardBreak
        {
            width: 100%;
            break-word: break-word;
        }*/
    </style>

    <script language="javascript" type="text/javascript">
// <!CDATA[

  function addTitleAttributes()

{
   numOptions = document.getElementById('sel_test_type').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('sel_test_type').options[i].title = document.getElementById('sel_test_type').options[i].text;

   

}
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you really want to Delete this?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }

// ]]>
    </script>

   
    <div class="container">
        <div class="row">
      <div class="col-sm-12">
             <section class="content-header">
                  <div class="container-fluid">
                        <div class="row">
                           <div class="col-sm-12">
                                 <asp:Label runat="server" ID="lblMsg" Visible="False"></asp:Label>
                           </div>
                        </div>
             <div class="row">
                            <div class="col-sm-12">
                                <h1><%=Resources.Resource.SearchQues_QtnMain %></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.searchQuestion_QueSrch%></h1>
                        </div>
                          <div class="card-body">
                            <div class="row">
                                 <div class="col-sm-6">
                                <div class="form-group" >
                                    <label for="exampleInputClassName1"><%=Resources.Resource.searchQuestion_SubNm%></label> 

                                    <asp:DropDownList ID="sel_test_type" runat="server" onmouseover="addTitleAttributes();" class="form-control" >
                                    </asp:DropDownList>
                               </div>
                                     </div>
                                <div class="col-sm-6">
                            <%--							<tr>
								<td style="HEIGHT: 10px">&nbsp;</td>
								<td style="HEIGHT: 10px">Display Type</td>
								<td style="HEIGHT: 10px">&nbsp;</td>
								<td style="HEIGHT: 10px">&nbsp;<asp:RadioButton ID="rdBtnNormal" Text="Normal" runat="server" Checked="True" />&nbsp;<asp:RadioButton ID="rdBtnParagraph" Text="Paragraph" runat="server" />&nbsp;<asp:RadioButton ID="rdBtnAll" Text="All" runat="server" /></td>
							</tr>--%>
                                     <div class="form-group" >
                                    <label for="exampleInputClassName1"><%=Resources.Resource.searchQuestion_QuesAnyKey%></label> 
                                    <asp:TextBox ID="txt_question"  class="form-control" runat="server" MaxLength="4000" ></asp:TextBox>
                                    <asp:CheckBox ID="ChkExactSearch" runat="server" Text="<%$Resources: Resource, searchques_Exactm %>" ForeColor="Black" />
                                         </div>
                               
                                </div>
                            </div>
                        <div class="row">
                            <div class="col-sm-12" style="text-align:center">
                                <asp:Button ID="imgBtnSearch" runat="server" Text="<%$Resources: Resource, Common_BtnSrch %>" class="btn btn-primary"></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="imgBtnClear" runat="server" Visible="false" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary"></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnBack" runat="server" Text="<%$Resources: Resource, Common_btnBck %>" Visible="false"></asp:Button>
                                <asp:Button ID="btnNewQuestion" runat="server" Text="<%$Resources: Resource, SearchQues_NewQues %>" class="btn btn-primary"></asp:Button>&nbsp;&nbsp;
                                <asp:Button ID="btnQuestion" runat="server" Text="<%$Resources: Resource, SearchQues_BlkInstQues %>" class="btn btn-primary"></asp:Button>&nbsp;&nbsp;                  
                                <asp:Button ID="BtnRemove" runat="server" Text="<%$Resources: Resource, Common_Revm %>" CausesValidation="False" Visible="False"></asp:Button>
                                <asp:Button ID="ExportQueAnsTF" runat="server" Text="<%$Resources: Resource, SearchQues_ExportQues %>"  class="btn btn-primary"/>&nbsp;&nbsp;
                                <asp:Button ID="ExportQA_Match" runat="server" Text="<%$Resources: Resource, SearchQues_Export %>" class="btn btn-primary"/>
                             </div>
                        </div>
                   </div>
                </div>
             </div>
           </section>
        </div>
     </div>

        


            <%--<tr>
                                <td colspan="2" align="left" style="width: 100%; ">
                                    <asp:Label ID="lblMsg" runat="server"   CssClass="errorMsg"></asp:Label></td>
                            </tr>--%>
             <div class="row" id="gridDiv" runat="server" visible="false" align="left">
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
                                       <strong><asp:Label runat="server" ID="LblRecCnt"></asp:Label></strong>
                                     </div>
                                </div>
                            <%--   <asp:Label ID="LblRecCnt" runat="server" Width="150px" CssClass="genLbl"></asp:Label>--%>

                                  <div class="row">
                            <div class="col-sm-12">
                            <div class="table-responsive SpecifyHeight">

                            <asp:DataGrid ID="DGData" runat="server" OnPageIndexChanged="DgData_PageIndexChanged"
                                AutoGenerateColumns="False" AllowPaging="True" CssClass="table table-bordered table-striped table-hover ">
                               <FooterStyle BackColor="White" ForeColor="#526B94" />
                                <HeaderStyle  />
                                <SelectedItemStyle  />
                                <ItemStyle />
                                <PagerStyle  HorizontalAlign="Left" Mode="NumericPages" BackColor="Transparent" />
                                <%-- <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>--%>
                                <Columns>
                                    <asp:BoundColumn DataField="qnid" HeaderText="<%$Resources: Resource, searchQuestion_QueID %>" >
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Serial_no"
                                        HeaderText="<%$Resources:Resource, Common_SrNo %>" ItemStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" Width="5%"/>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="Test_Type" HeaderText="<%$Resources: Resource, searchQuestion_TstTyp %>">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="question" 
                                        HeaderText="<%$Resources:Resource, searchQuestion_Ques %>">
                                        <%--<HeaderStyle HorizontalAlign="Center" Width="84%"></HeaderStyle>--%>
                                        <%--<ItemStyle HorizontalAlign="Left" CssClass="gItemStyle"></ItemStyle>--%>
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="dflg" 
                                        HeaderText="dflg" Visible="false">
                                        <%--<HeaderStyle HorizontalAlign="Center" Width="84%"></HeaderStyle>--%>
                                        <%--<ItemStyle HorizontalAlign="Left" CssClass="gItemStyle"></ItemStyle>--%>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="<%$Resources:Resource, searchQuestion_Viw %>">
                                        <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center"/>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="qnid" runat="server" Text="<%$Resources:Resource, searchQuestion_Viw%>" NavigateUrl='<%#"question_ans.aspx?qid=" & _&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;Container.DataItem("qnid") & _&#13;&#10;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;&#9;"&test=" & sEncodeString(Container.DataItem("test_type")) %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_Sltall%>">
                                        <HeaderTemplate >
                                            <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource, searchQuestion_SelectAll %>" Font-Bold AutoPostBack="true"
                                                OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                        </HeaderTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkRemove" runat="server" Text=""/>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="CourseName" HeaderText="<%$Resources:Resource,searchQuestion_SubNm%>"  >
                                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle VerticalAlign="Bottom" Visible="false" HorizontalAlign="Left" ForeColor="#000066"
                                    BackColor="Transparent" PageButtonCount="20" Mode="NumericPages"></PagerStyle>
                                <HeaderStyle BackColor="#189599" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                            </asp:DataGrid>
                                </div>
                                </div>
                                      </div>

                            
                                        </div>
                                <div class="row">
                                <div class="col-sm-12 SpecifyMargin">
<%--                                        &nbsp;&nbsp;<span style="color: #526B94"><%=Resources.Resource.Common_GoToPage%>   </span>--%>
                                        <asp:DropDownList ID="ddlPages" AutoPostBack="true" class="form-control" Width="55px" runat="server" Visible="false">
                                        </asp:DropDownList>
                                    <div class="form-group">
                        <asp:Button ID="imgBtnEnable" Visible="false"  class="btn btn-primary" runat="server" >
                        </asp:Button>&nbsp;
                        <asp:Button ID="imgBtnDisable" runat="server"  class="btn btn-primary" Text="<%$Resources:Resource,StudentSearch_btnDel%>"  OnClientClick = "Confirm()">
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
                 </div>
    
</asp:Content>
