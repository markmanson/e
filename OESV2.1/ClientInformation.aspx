<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ClientInformation.aspx.vb"
    Inherits="Unirecruite.unirecruite.ClientInformation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Examinations solution------Client Information</title>
    <link href="images/css.css" type="text/css" rel="stylesheet" />

    <script type="text/javascript">
        
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

</head>
<body>
    <body>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" align="center" width="80%">
            <tr>
                <%--Header Here--%>
                <td colspan="3" valign="bottom" style="width: 100%; background-image: url(images/Commonheader.jpg);
                    height: 135px; background-repeat: no-repeat;">
                    <table width="100%" style="height: 100%" cellspacing="0">
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td valign="bottom" align="right">
                                <a href="admin.aspx">
                                    <img src="images/btnhome.jpg" border="0"></a> <a href="login.aspx">
                                        <img src="images/btnlogoff.jpg" border="0" id="IMG2" alt="" />
                                    </a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <%--Content Here--%>
                <td colspan="2" class="pageContentTD" valign="top" align="left">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 100%">
                                <asp:Label runat="server" ID="lblMsg" Visible="False" Height="22px" CssClass="errorMsg"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <fieldset>
                                    <%-- <fieldset style="height: 150px">--%>
                                    <legend class="outerframe">Client Search</legend>
                                    <table class="content" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="tblhead" colspan="2" height="2%">
                                                Client Details
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdcontent_label" style="height: 23px">
                                                Client ID 
                                            </td>
                                            <td class="tdcontent_data" align="left" style="height: 23px; width: 207px;">
                                                <asp:TextBox ID="txtClientName" runat="server" MaxLength="255" Width="200px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%-- <tr>
                                            <td class="tdcontent_label" style="height: 23px">
                                                Address</td>
                                            <td class="tdcontent_data" align="left" style="height: 23px; width: 207px;">
                                                <asp:TextBox ID="txtCenterCode" runat="server" MaxLength="255" Width="200px"></asp:TextBox>
                                              
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdcontent_label">
                                                Mobile Number
                                            </td>
                                            <td class="tdcontent_data" align="left" style="width: 207px">
                                                <asp:TextBox ID="txtOwnerName" runat="server" MaxLength="255" Width="200px"></asp:TextBox>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdcontent_label" style="height: 25px">
                                                Email ID</td>
                                            <td class="tdcontent_data" align="left" style="height: 25px; width: 207px;">
                                                <asp:TextBox ID="txtCity" runat="server" Width="199px" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%">
                                <table>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="images/btnsearch.jpg" />
                                &nbsp;<asp:ImageButton ID="imgBtnClear" runat="server" ImageUrl="images/btnclear.jpg" />
                                &nbsp;<asp:ImageButton ID="ImgBtnBack" runat="server" ImageUrl="images/BtnBack.jpg" />
                                &nbsp;<asp:ImageButton ID="imgBtnNewCentreRegistration" runat="server" ImageUrl="images/btnRegistrattion1.jpg" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <div id="DivGrid" runat="server" visible="false">
                                    <fieldset>
                                        <legend class="outerframe">Search Result </legend>
                                        <div style="text-align: right">
                                            <asp:Label runat="server" CssClass="genMsg" ID="lblrecords"></asp:Label>
                                        </div>
                                        <asp:DataGrid runat="server" ID="DG_ClientDetail" AutoGenerateColumns="False" Visible="False"
                                            DataKeyField="client_id" CssClass="gridClass " AllowSorting="True" AllowPaging="True"
                                            Width="1029px">
                                            <FooterStyle BackColor="White" ForeColor="#526B94" />
                                            <SelectedItemStyle CssClass="gSelectedItem" />
                                            <PagerStyle CssClass="gPagerStyle" Visible="false" HorizontalAlign="Left" Mode="NumericPages" />
                                            <ItemStyle CssClass="gItemStyle" HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Sr.No.">
                                                    <ItemTemplate>
                                                        <%#Container.DataSetIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="gItemStyleNum" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="client_id" HeaderText="Client Id">
                                                    <ItemStyle CssClass="gItemStyleNum" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="client_name" HeaderText="Client Nme">
                                                    <ItemStyle CssClass="gItemStyle" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="client_address" HeaderText="Address">
                                                    <ItemStyle CssClass="gItemStyle" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="email" HeaderText="Email">
                                                    <ItemStyle CssClass="gItemStyle" />
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="mob_number" HeaderText="Contact No">
                                                    <ItemStyle CssClass="gItemStyle" />
                                                </asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Update">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" CssClass="gButtonStyle" runat="server" Font-Underline="True"
                                                            CommandName="lnkEdit" Width="58px" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.client_id")%>'>Edit</asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle CssClass="gButtonStyle" />
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Select / Deselect">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="Select All"
                                                            AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkRemove" runat="server" Text="" AutoPostBack="true" OnCheckedChanged="chkRemove_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="Del_Flag" Visible="False">
                                                    <ItemStyle CssClass="gItemStyleNum" />
                                                </asp:BoundColumn>
                                            </Columns>
                                            <HeaderStyle BackColor="#526B94" Font-Bold="True" ForeColor="White" CssClass="ghead" />
                                        </asp:DataGrid>
                                        <table align="left">
                                            <tr>
                                                <%-- Display the First Page/Previous Page buttons --%>
                                                <td>
                                                    <%--<asp:LinkButton ID="Firstbutton" Text="First" CommandArgument="0" runat="server" OnClick="PagerButtonClick" />--%>
                                                    <asp:ImageButton ID="imgfirst" runat="server" Height="15px" ImageUrl="images/first1.gif"
                                                        Width="10px" CommandArgument="0" ToolTip="First" OnClick="PagerButtonClick" />
                                                </td>
                                                <td>
                                                    <%--<asp:LinkButton ID="Prevbutton" Text="Prev" CommandArgument="prev" runat="server" OnClick="PagerButtonClick" />--%>
                                                    <asp:ImageButton ID="imgprev" runat="server" Height="15px" ImageUrl="images/prev1.gif"
                                                        Width="10px" CommandArgument="prev" ToolTip="Previous" OnClick="PagerButtonClick" />
                                                </td>
                                                <%-- Display the Page No. buttons --%>
                                                <td>
                                                    <table id="tblPagebuttons" runat="server">
                                                        <tr>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <%-- Display the Next Page/Last Page buttons --%>
                                                <td>
                                                    <asp:ImageButton ID="imgnext" runat="server" Height="15px" ImageUrl="images/next1.gif"
                                                        Width="10px" CommandArgument="next" ToolTip="Next" OnClick="PagerButtonClick" /><%--<asp:LinkButton ID="Nextbutton" Text="Next" CommandArgument="next" runat="server" OnClick="PagerButtonClick" />--%>
                                                </td>
                                                <td>
                                                    <%--<asp:LinkButton ID="Lastbutton" Text="Last" CommandArgument="last" runat="server"
                                        OnClick="PagerButtonClick" />--%><asp:ImageButton ID="imglast" runat="server" Height="15px"
                                            ImageUrl="images/last1.gif" Width="10px" CommandArgument="last" ToolTip="Last"
                                            OnClick="PagerButtonClick" />
                                                </td>
                                                <td>
                                                    &nbsp;&nbsp;<span style="color: #526B94"> Go to page no.</span>
                                                    <asp:DropDownList ID="ddlPages" AutoPostBack="true" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                    <table>
                                        <tr>
                                            <td align="left" style="height: 29px">
                                                <asp:ImageButton ID="imgBtnEnable" runat="server" ImageUrl="images/btnEnable.jpg"
                                                    Visible="true" />&nbsp;<asp:ImageButton ID="imgBtnDisable" runat="server" ImageUrl="images/btnDisable.jpg"
                                                        Visible="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="background-color: #526B94;">
                    <table width="100%">
                        <tr>
                            <td align="right" colspan="2" style="width: 65%">
                                <span class="copyright">Copyright &#174 2010-11 Unikaihatsu Software Pvt.Ltd., All Rights
                                    Reserved.</span>
                            </td>
                            <td align="right" style="width: 35%">
                                <span class="copyright">Site Powered by: <a href="http://www.usindia.com" target="_blank"
                                    style="color: White">Unitech Systems</a></span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </form>
    </body>
</body>
</html>
