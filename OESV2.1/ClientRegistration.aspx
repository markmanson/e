<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ClientRegistration.aspx.vb"
    Inherits="Unirecruite.unirecruite.ClientRegistration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Online Examinations solution------New Client Registration</title>
    <link href="images/css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" align="center" width="80%" border="0" id="TABLE1">
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
                                    <img src="images/btnlogoff.jpg" border="0" id="IMG2"></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <%--Content Here--%>
            <td class="pageContentTD" valign="top" align="left" style="width: 100%; height: 390px;">
                <table style="width: 100%">
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Label runat="server" ID="lblMsg" CssClass="errorMsg" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 100%">
                            <fieldset>
                                <legend class="outerframe">
                                    <asp:Label runat="server" ID="legendLabel" Text="New Client Registration"></asp:Label>
                                </legend>
                                <table>
                                    <tr>
                                        <td style="width: 70%;">
                                            <table cellspacing="0" style="width: 70%">
                                                <tr>
                                                    <td class="tblhead" width="100%" colspan="2" style="height: 2%">
                                                        <asp:Label runat="server" ID="lblhead" Text="New Client Registration"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdcontent_label" style="width: 613%">
                                                        <span class="staticText">Client Id <a class="mand">*</a></span>
                                                    </td>
                                                    <td class="tdcontent_data" colspan="2" style="width: 100%">
                                                        <asp:TextBox runat="server" ID="txtClientId" MaxLength="5"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdcontent_label" style="width: 613%">
                                                        <span class="staticText">Client Name <a class="mand">*</a></span>
                                                    </td>
                                                    <td class="tdcontent_data" colspan="2" style="width: 100%">
                                                        <asp:TextBox runat="server" ID="txtClientName" MaxLength="255"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdcontent_label" style="height: 25px; width: 613%;">
                                                        <span class="staticText">Email ID<a class="mand">*</a></span>
                                                    </td>
                                                    <td class="tdcontent_data" colspan="2" style="height: 25px; width: 100%;">
                                                        <asp:TextBox runat="server" ID="txtEmailAddress" MaxLength="50"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdcontent_label" style="height: 19px; width: 613%;">
                                                        <span class="staticText">Mobile Number <a class="mand">*</a></span>
                                                    </td>
                                                    <td colspan="2" class="tdcontent_data" style="height: 19px; width: 100%;">
                                                        <asp:TextBox runat="server" ID="txtMobileNo" MaxLength="20"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdcontent_label" style="width: 613%">
                                                        Address<span class="staticText"><a class="mand">*</a></span>
                                                    </td>
                                                    <td class="tdcontent_data" style="width: 381px">
                                                        <asp:TextBox runat="server" ID="txtAddress" MaxLength="500" Columns="72" Rows="3"
                                                            TextMode="MultiLine" Width="475px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" colspan="2">
                                                        <asp:ImageButton ID="btnSubmit" runat="server" ImageUrl="images/btnsubmit.jpg"></asp:ImageButton>
                                                        <asp:ImageButton ID="btnUpdate" runat="server" ImageUrl="images/btnupdate.jpg" Visible="false">
                                                        </asp:ImageButton>
                                                      
                                                        &nbsp;<asp:ImageButton ID="btnClear" runat="server" ImageUrl="images/btnclear.jpg">
                                                        </asp:ImageButton>&nbsp;
                                                        <asp:ImageButton ID="btnBack" runat="server" ImageUrl="images/BtnBack.jpg">
                                                        </asp:ImageButton>  &nbsp;
                                                        <asp:ImageButton ID="btnReset" runat="server" ImageUrl="images/btnreset.jpg" Visible="false">
                                                        </asp:ImageButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
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
</html>
