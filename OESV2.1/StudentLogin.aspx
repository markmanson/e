<%@ Page Language="vb" AutoEventWireup="false" Codebehind="StudentLogin.aspx.vb"
    Inherits="Unirecruite.StudentLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%-- Added by Pragnesha  for Bug ID 970(Web browser Icon) Date: 13-12-18--%>
 <link rel="shortcut icon" type="image/png" href="images/OES_ICON.png" />
<%-- ---------Ended by Pragnesha-------------------------------------------%>
    <title>Online Examinations solution------Student Login</title>
</head>

<script language="JavaScript" type="text/JavaScript">
	
	
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

function validation(){
	loginmsg = "Please Enter Username"
	pwd	= "Pleae Enter Password"
	msg = "Please Enter Username and Password"
	
	if(document.frm_login.txt_login.value==""
	  && document.frm_login.txt_pwd.value=="")
	{
		alert(msg);
	}	
	else if(document.frm_login.txt_login.value==""){
		alert (loginmsg);		
	}
	else if(document.frm_login.txt_pwd.value==""){
		alert(pwd)		
	}
}

//-->
</script>

<body>
    <form id="frm_login" runat="server">
        <table cellpadding="0" cellspacing="0" style="width: 80%; height: 80%;" align="center" >
            <tr>
                <td style="width: 100%;">
                    <img src="images/Commonheader.JPG" width="100%" />
                   <center>
                  <span style="font-family:Times New Roman;font-size:30px;font-weight:bold;color:#526B94">ONLINE  EXAMINATIONS  PORTAL</span>
                  </center>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" style="height: 338px; 
                    background-color: transparent; background-repeat: no-repeat;">
                    <b><%--<asp:HyperLink ID="HyperLink2" Visible="false" NavigateUrl="~/login.aspx" runat="server" ForeColor="#3D297E">Login for Administrator</asp:HyperLink>--%></b>
                    <br />
                    <center>
                    <br />
                    <br />
                        <table cellspacing="7">
                            <tr>
                                <td valign="top" style="height: 40px" colspan="2">
                                    <asp:Label runat="server" ID="lbl_saved" Visible="false" Font-Bold="true" Font-Size="18px"
                                        ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td valign="top" style="height: 40px">
                                    <b>Stream (Course)</b>
                                </td>
                                <td valign="middle" style="height: 40px">
                                    <asp:TextBox ID="txtStreamCourse" runat="server" Height="30px" Width="280px" ReadOnly="True" MaxLength="255"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td valign="middle">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <b>Exam Roll No.</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                               
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<pre><h2> Exam Roll No.</h2>(As give on Hall Ticket)</pre>--%>
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txt_login" runat="server" Height="30px" Width="280px" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <b>Password</b>
                                </td>
                                <td valign="middle">
                                    <asp:TextBox ID="txt_pwd" runat="server" Height="30px" Width="280px" TextMode="Password" MaxLength="20"></asp:TextBox><br />
                                  
                                </td>
                                
                            </tr>
                            <tr>
                            <td colspan="2" align="left">
                             Incase of any issue or bugs, kindly report it to following mentioned URL.
                                            <a href="http://internals.usindia.com:8091/uksbugtracker/my_view_page.php">http://internals.usindia.com:8091/uksbugtracker/</a>
                            </td>
                            </tr>
                        </table>
                    </center>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; background-color: #526B94;">
                    <br />
                    <center>
                        <asp:ImageButton ID="imgBtnSubmit" runat="server" ImageUrl="images/btnBigSubmit.jpg" />
                        <asp:ImageButton ID="imgBtnBack" runat="server" ImageUrl="images/btnBigBack.jpg" Visible="false" />
                        </center>
                    <br />

                </td>
            </tr>

        </table>
      
        &nbsp;<br />
        <br />
        <table width="100%" >
                            <tr >
                                <td align="right" colspan="2" style="width: 65%">
                               <font size = "2">
                                    <span class="copyright">Copyright &#174 2019-20 Unitech Systems, All Rights Reserved.</span>
                                    </font>
                                </td>
                                <td align="center" style="width: 35%">
                                <font size = "2">
                                    <span class="copyright">Site Powered by: <a href="http://www.usindia.com" target="_blank"
                                        style="color: black">Unitech Systems</a></span>
                                        </font>
                                </td>
                            </tr>
                        </table>
                        
               

                        
                
                 
                        
                        


           

        
        <script language="javascript" type="text/javascript">
					
		if(""=="err")
		{
			alert("Incorrect login id or password")
			
		}
		document.frm_login.txt_login.focus()
        </script>

        <script language="javascript" type="text/javascript">
		
		if("<% Response.Write(Session.Item("userid"))%>"=="err")
		{
			alert("Incorrect login id or password")
		    
		}
	document.frm_login.txt_login.focus()		
        </script>

    </form>
    <table style="width: 89%" >
                            <tr >
                                <td align="right" style="width: 65%">
                       <font size="2px">  This site is best viewed in Internet Explorer 11, Mozilla 3.6, Chrome 67.0 
                                    browsers or above and screen resolution 1204 X 768  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Version 1.2            </font>
                                </td>
                            </tr>
                        </table>
                        
               

                        
                
                 
                        
                        


           

        
        </body>
</html>
