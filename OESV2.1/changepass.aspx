<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.changepass"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Change Password"
    CodeBehind="changepass.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/JavaScript">
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);



//-->
    </script>

    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS">
    <%--<style type="text/css">
        .entryHeader
        {
            font-size: 50px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
        }
        .entryLabel
        {
            font-names: Bookman Old Style;
            forecolor: Navy;
            font-bold: True;
            font-size: 18px;
        }
        .style1
        {
            width: 100%;
            height: 316px;
        }
    </style>--%>
    <%--<form id="frm_login" name="frm_login" action="" method="post" runat="server">--%>

    <%--Created by Pranit Chimurkar on 2019/10/25--%>
        <!-- Content Header (Page header) -->

    <script type="text/javascript">
        //$("body").css("overflow", "hidden");
        //function userValid() {
        //    var OldPwd, NewPwd, ConfPwd;
        //    OldPwd = document.getElementById("txt_oldpass").value;
        //    NewPwd = document.getElementById("txt_newpass").value;
        //    ConfPwd = document.getElementById("txt_confpassword").value;
        //    LblVal = document.getElementById("lbl_passchange").value;

        //    if (OldPwd == '' && NewPwd == '' && ConfPwd == '') {
        //        document.getElementById('lbl_passchange').style.display = 'inherit';
        //        LblVal.innerHTML = "Please Enter all fields"
        //        return false;
        //    }  
        //    if (OldPwd == '') {
        //        alert("Please Enter Login ID");  
        //        return false;  
        //    }  
        //    if (NewPwd == 0) {  
        //       alert("Please Select gender");  
        //       return false;  
        //    }
        //      return true;  
        //    } 
    </script>
    <body>
      <div class="container">
        <div class="row">
            <div class="col-sm-12"><br /><br /></div>
        </div>
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-12" style="text-align:center">
                  <asp:Label ID="lbl_passchange" runat="server" Visible="false" ForeColor="Red"></asp:Label>
              </div>
            </div>
          </div>
        </section>

        <!-- main content -->
        <section class="content">
             <div class="container-fluid">
               <div class="row">
                  <div class="col-md-3"></div>
                    <div class="col-md-6">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.ChangePass_ChPwdHe%></h1>
                        </div>
                         <div class="card-body">
                             <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-4">
                                       <label><%=Resources.Resource.ChangePass_OlPwd %></label>
                                    </div>
                                    <div class="col-sm-8">
                                       <asp:TextBox ID="txt_oldpass" TextMode="password" MaxLength="16" runat="server" class="form-control"/>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-4">
                                       <label><%=Resources.Resource.ChangePass_NPwd %></label>
                                    </div>
                                    <div class="col-sm-8">
                                       <asp:TextBox ID="txt_newpass" TextMode="password" MaxLength="16" runat="server" class="form-control"/>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <label><%=Resources.Resource.ChangePass_CPwd %></label>
                                    </div>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txt_confpassword" TextMode="password" MaxLength="16" runat="server" class="form-control"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12" style="text-align:center">
                                    <div class="form-group">
                                        <asp:Button ID="btn_update" runat="server" Text="<%$Resources:Resource, Common_Update%>" class="btn btn-primary" Width="100px"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="imgbtn_cancel" runat="server" CausesValidation="False" Text="<%$Resources:Resource, ChangePass_Cancel %>" class="btn btn-primary" Width="100px"></asp:Button>
                                    </div>
                                </div>
                            </div>                            
                        </div>
                      </div>
                          <%--<div class="row">
                                <div class="col-sm-12">
                                    <div class="form-group">
                                        <asp:RequiredFieldValidator ID="validreqdoldpass" runat="server" ErrorMessage="Please fill the Old Password" ControlToValidate="txt_oldpass" Display="None" Width="84px"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="validreqdnewpass" runat="server" ErrorMessage="Please fill the New Password" ControlToValidate="txt_newpass" Display="None"></asp:RequiredFieldValidator>
                                        <%--<asp:CustomValidator ID="validoldpass" runat="server" ErrorMessage="The Old Password Seems to be wrong" ControlToValidate="txt_oldpass" Display="Dynamic"></asp:CustomValidator>--%>
                                        <%--<asp:RequiredFieldValidator ID="validreqdconfpass" runat="server" ErrorMessage="Please fill the confirm password" ControlToValidate="txt_confpassword" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="validcompnewconf" runat="server" ErrorMessage="New password and confirm password do not match" ControlToValidate="txt_newpass" Display="None" ControlToCompare="txt_confpassword"></asp:CompareValidator>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
                                    </div>
                                </div>
                          </div>--%>
                      </div>
                     <div class="col-md-3"></div>
                  </div>
              </div>
         </section>
      </div>
     </div>
   </div>
  </body>
    <%--</form>--%>
</asp:Content>
