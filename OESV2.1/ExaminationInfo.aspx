<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ExaminationInfo.aspx.vb"
    Inherits="Unirecruite.ExaminationInfo1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%-- Added by Pragnesha for Bug ID 970(Web browser Icon) Date: 14-05-19--%>
    <link rel="shortcut icon" type="image/png" href="images/OES_ICON.png" />
    <%-- ---------Ended by Pragnesha-------------------------------------------%>
    <title>Online Examinations solution------Examination Information</title>
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
                <td valign="top" style="height: 338px;
                    background-color: transparent; background-repeat: no-repeat;">
                    <br />
                    <br />
                    <br />
                    <table align="center" width="40%" style="text-align: left; color: navy; font-family: Calibri;">
                        <tr>
                            <td>
                                1.</td>
                            <td>
                                <asp:Label runat="server" ID="LblTime" Text="The duration of the examination will be "></asp:Label>
                                <font><%=GetTime()%> minutes.</font>
                                <%--<asp:Label runat="server" ID="LblExamTime" Text="<%=GetTime()%> Minutes."></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                2.</td>
                            <td>
                                <asp:Label runat="server" ID="lblPageNo" Text="Number of questions per page is :"></asp:Label>
                                 <font><%=GetQuePerPage()%> .</font>
                                  <%-- <asp:Label runat="server" ID="lblTotalQtnPerPage" Text="<%=GetQuePerPage()%> ."></asp:Label>     --%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                3.</td>
                            <td>
                                <asp:Label runat="server" ID="Label2" Text="Please do not use browser's Previous / Back button during examination."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                4.</td>
                            <td>
                                <asp:Label runat="server" ID="Label3" Text="All questions will be of objective type."></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                5.</td>
                            <td>
                                <asp:Label runat="server" ID="Label4" Text="Click Start button to appear for the examination."></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                6.
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label1" 
                                    Text="During the exam, if any system error occurs before clicking on 'End Exam' button, please re-login using the same user id and Password." 
                                    Height='15px'></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; background-color: #526B94;">
                    <br />
                    <center>
                        <asp:ImageButton ID="imgBtnStartExamination" runat="server" ImageUrl="images/Exam_Info_Btn_Online_Examinaation.png" /></center>
                    <br />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
