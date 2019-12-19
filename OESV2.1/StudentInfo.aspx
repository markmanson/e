<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StudentInfo.aspx.vb" Inherits="Unirecruite.StudentInfo" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%-- Added by Pragnesha for Bug ID 970(Web browser Icon) Date: 14-05-19--%>
    <link rel="shortcut icon" type="image/png" href="images/OES_ICON.png" />
    <%-- ---------Ended by Pragnesha-------------------------------------------%>
    <title>Online Examinations solution------Examination Information</title>
     <link href="images/css.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="f1" runat="server">
        <table cellpadding="0" cellspacing="0" style="width: 80%; height: 80%;" align="center"
            border="1">
            <tr>
                <td style="width: 100%;">
                    <img src="images/Commonheader.JPG" width="100%" />
                </td>
            </tr>
            <tr>
                <td  valign="top"  style="height: 338px; 
                    background-color: transparent; background-repeat: no-repeat;">
                    
                    <table align="center">
                        <tr>
                            <td>
                                               <b> <center><h4 style="text-align: left; color: Black ; font-size:large  ; font-family: Calibri;"> Candidate Details </h4>
                    </center></b>
                            </td>
                        </tr>
                    </table>

                    
                    <table align="center" width="30%" border="1" style="text-align: left; color: Black ; font-size:medium ; font-family: Calibri;">
                        <tr>
                            <td  style="width:40%" >
                                Candidate's Name </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" Text=" "></asp:Label>
                                <%--<asp:Label runat="server" ID="LblExamTime" Text="<%=GetTime()%> Minutes."></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Class Name </td>
                            <td>
                                <asp:Label runat="server" ID="lblCenterName" 
                                    Text=" "></asp:Label>
                                 <%--<font><%=GetQuePerPage()%> .</font>--%>
                                  <%-- <asp:Label runat="server" ID="lblTotalQtnPerPage" Text="<%=GetQuePerPage()%> ."></asp:Label>     --%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Date of Birth </td>
                            <td>
                                <asp:Label runat="server" ID="lblDOB" 
                                    Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Photo </td>
                            <td>
                                <asp:Image ID="StudentImage"  Height="100" ImageUrl="" Width="100" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan ="2" align="center">
                              
                        <asp:LinkButton ID="lnkbtnNext"   runat="server">Next</asp:LinkButton>          
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
                                    <span class="copyright">Copyright &#174 2010-11 Unitech Systems, All Rights Reserved.</span>
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
