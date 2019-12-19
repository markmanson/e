<%@ Page AutoEventWireup="false" Inherits="Unirecruite.unirecruite.login" Language="vb"
    Codebehind="login.aspx.vb" %>

<!DOCTYPE HTML>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <!-- Bootstrap CSS-->
    <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <title>E Exam Tool | Log in</title>
  <!-- Tell the browser to be responsive to screen width -->
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <!-- Font Awesome -->
  <link rel="stylesheet" href="plugins/fontawesome-free/css/all.min.css">
  <!-- Ionicons -->
  <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
  <!-- icheck bootstrap -->
  <link rel="stylesheet" href="plugins/icheck-bootstrap/icheck-bootstrap.min.css">
  <!-- Theme style -->
  <link rel="stylesheet" href="dist/css/adminlte.min.css">
  <!-- Google Font: Source Sans Pro -->
  <link href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700" rel="stylesheet">

<%-- Added by Pragnesha for Bug ID 970(Web browser Icon) Date: 13-12-18--%>
 <link rel="shortcut icon" type="image/png" href="images/OES_ICON.png" />
<%-- ---------Ended by Pragnesha-------------------------------------------%>

   <%-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>--%>

    <!--StartFragment -->

    <%--prevent go back--%>
    <%--<script type="text/javascript" language="javascript">
         function DisableBackButton() {
           window.history.forward()
          }
         DisableBackButton();
         window.onload = DisableBackButton;
         window.onpageshow = function(evt) { if (evt.persisted) DisableBackButton() }
         window.onunload = function() { void (0) }
    </script>--%>

    <style type="text/css">
        .display-4 {
            font-size: 3rem;
            font-weight: 300;
            line-height: 1.2;
        }
        .login-logo{
                font-size: 1.6rem;
                font-weight: 450;
        }
        img {
        display: block;
        margin: 0 auto;
        max-width: 100%;
      }
    </style>

    <script language="JavaScript" type="text/JavaScript">
	
	function ClearTextboxes()
        {
            document.getElementById('txt_login').value = '';
            document.getElementById('txt_pwd').value = '';
            document.getElementById('txt_login').focus();
            return false;
        }

function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

function validation(){
	loginmsg = "Please Enter Username"
	pwd	= "Please Enter Password"
	msg = "Please Enter Username and Password"
	
	if(document.frm_login.txt_login.value==""
	  && document.frm_login.txt_pwd.value=="")
	{
		alert(msg);
		document.frm_login.txt_login.focus();
		return false;
	}
	else if(document.frm_login.txt_login.value==""){
		alert (loginmsg);
		document.frm_login.txt_login.focus();
        return false;
	}
	else if(document.frm_login.txt_pwd.value==""){
		alert(pwd)
		document.frm_login.txt_pwd.focus();
		return false;
	}
	else
	{
	    return true;
	}
}
    </script>
    <script>
        function white_space(evt){
            document.getElementById('whit').value;
        }
    </script>
</head>
    <body class="hold-transition login-page">
        
<div class="login-box">
  
  <!-- /.login-logo -->
  <div class="card">
    <div class="card-body login-card-body" style="align-self:center" >
        <div>
    <img  src="images/oesus.png" alt="uks-oes">
  </div>
  <div class="login-logo">
  <span class="login100-form-title p-b-32" > <h1 class="display-4"><b style="color:#2e004d;"> E-exam</b><b style="color:red"> Tool </b></h1></span>
  </div>
        <div class="mb-3" > <span class="login-logo" >Login Details</span></div>
        <form id="Form1" name="frm_login" action="" method="post" runat="server">
        <div class="input-group mb-3">
          <input type="text" id="txt_login" class="form-control" placeholder="Username" runat="server"/>
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-user"></span>
            </div>
          </div>
        </div>
        <div class="input-group mb-3">
          <input type="password" id="txt_pwd" class="form-control" placeholder="Password" runat="server" maxlength="20" />
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>        

      <div class="row">
          <div class="col-12">
            <div class="align-self-auto">
              <a href="http://internals.usindia.com:8091/uksbugtracker/my_view_page.php" class="text-dark">
              Report a Bug
              </a>
            </div>
          </div>
        </div>
        <div class="row">        
            <div class="col-12" style="text-align:center">
              <input type="submit"  id="btn_login" runat="server" Value="Login" class="btn btn-outline-primary" style="width:150px" OnServerClick="btn_login_Click" OnClientClick="return validation()"/>              
            </div>        
        </div>
        </form>
          <p class="txt3 text-center" >
              <a href="http://www.usindia.com">
                  Unikaihatsu Software Private Limited
              </a>
              <br />
              OES Version 2.0.1
          </p>
      
    </div>
    <!-- /.login-card-body -->
  </div>
</div>
<!-- /.login-box -->

<!-- jQuery -->
<script src="plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- AdminLTE App -->
<script src="dist/js/adminlte.min.js"></script>

    <%--Added by Pragnesha on 2018/07/03 [Decs:To highlight link, assign link color=white]--%>
    <%--<form id="frm_login" name="frm_login" action="" method="post" runat="server">--%>
        <%--	<body leftMargin="0" background="images/regbg.gif" topMargin="0" marginwidth="0" marginheight="0">
--%>
        <%--Response.Write("<br>" & Session.Item("userid") & "<br>")--%>
        <%--<table cellpadding="0" cellspacing="0" align="center" width="80%">
            <tr>
                <td style="height: 135px">
                    <table id="content" cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="background-image: url(images/Commonheader.jpg); width:1128px; height: 135px; background-repeat: no-repeat;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="middle" style="height: 390px" align="center">
                    <table width="100%" cellpadding="2" cellspacing="4">
                        <tr>
                            <td style="width: 100%" align="center">
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; " align="center" valign="bottom">
                                <fieldset style="width: 388px; height: 280px; border: none; background-color:#526B94">                                    
                                    <br />
                                    <table cellpadding="2" cellspacing="5">
                                        <tr>
                                            <td colspan="2" style="color:white;font-size:24px;">
                                                Login Details</td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 28%;font-size:18px;color:White;">
                                                Username</td>
                                            <td style="width: 70%">
                                                <asp:TextBox ID="" runat="server" MaxLength="20" Width="160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 28%;font-size:18px;color:White;">
                                                Password</td>
                                             <td style="width: 70%">
                                                <asp:TextBox ID="txt_pwd" runat="server" MaxLength="20" TextMode="Password" Width="160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                        
                                            <td colspan="2" style="width: 100%" align="center">
                                            <br />
                                  --%>
                                                <%--<asp:ImageButton ID="btn_login" runat="server" OnClientClick="return validation()"
                                                    ImageUrl="images/btnlogin.jpg" AlternateText="Login"></asp:ImageButton>&nbsp;<asp:ImageButton
                                                        class="iconstyle" OnClientClick="return ClearTextboxes()" ID="Clear" Height="25"
                                                        alt="Clear The Username and Password" Width="74" border="0" runat="server" ImageUrl="images/btnloginclear.jpg">
                                                    </asp:ImageButton>&nbsp;<asp:ImageButton class="iconstyle" ID="ImageButton1" Height="25"
                                                        alt="Clear The Username and Password" Width="74" border="0" runat="server" ImageUrl="images/BtnloginBack.jpg"
                                                        PostBackUrl="StudentLogin.aspx" Visible="false"></asp:ImageButton>
                                                        <br />
                                                        <br />
                                            </td>
                                        </tr>
                                    
                                       
                                            
                                    </table>--%>
                                    <%--<br />--%>
                                    <%--<table>
                                      <tr>
                                        
                                            <td align="left" colspan="2" style="width: 100%" align="center">
                                            Incase of any issue or bugs, kindly report it to following mentioned URL.
                                            <font size="2px"><a href="http://internals.usindia.com:8091/uksbugtracker/my_view_page.php">http://internals.usindia.com:8091/uksbugtracker/</a></font>
                                            </td>
                                            </tr>
                                            </table>
                                            
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 19px; width: 80%; background-color: #526B94;position:absolute;bottom:20">
                 <table width="100%">
                            <tr>--%>
                                <%--<td align="right" colspan="2" style="width: 65%">
                                    <span class="copyright">Copyright &#174 2010-11 Unitech Systems, All Rights Reserved.</span>
                                </td>
                                <td align="right" style="width: 35%">
                                    <span class="copyright">Site Powered by: <a href="http://www.usindia.com" target="_blank"
                                        style="color: White">Unitech Systems</a></span>
                                </td>
                            </tr>
                        </table>--%>

                    <script language="javascript" type="text/javascript">
					
		                    if(""=="err")
		                    {
			                    alert("The username or password you entered is incorrect.")
		                    }
		                    document.frm_login.txt_login.focus()
                    </script>

                <%--</td>
            </tr>--%>

              <%-- Added by Pragnesha Kulkarni on 2018/07/03
                        BugID: 786 
                        Desc: For user to identification, mentioned correct browser and resolution details .  --%>
                   <%--<tr>
                    <td align="center" style="height: 19px; width: 80%; background-color: #526B94; position: fixed;
                      color: White; bottom: 5">
                       <font size="2px">  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;This site is best viewed in Internet Explorer 11, Mozilla 3.6, Chrome
                    67.0 browsers or above and screen resolution 1204 X 768  &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;  Version 1.2            </font>
                      </tr>--%>
              <%--  Ended by Pragnesha Kulkarni on 2018/07/03--%>

        <%--</table>--%>
        <!-- Jquery JS-->
</body>
</html>