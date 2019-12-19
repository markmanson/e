<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CentreRegistration.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Class Registration"
    Inherits="Unirecruite.CenterRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Code written by Rajat Argade on 22/10/2019 -->
    <script type="text/javascript">
	function numbersonly(myfield, e, str)
    {

        var key;

        var keychar;

        var strValidate;        

        //If str is tru means Textbox DataType is integer

        //Else decimal

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

            if ((key==null) || (key==0) || (key==8) 

                    || (key==9) || (key==13) || (key==27) )

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
//     function emailvalidater()
//	{	
//	with(document.form1)
//		{	
//            var strE=txt_email.value;           
//             apos=strE.indexOf("@"); dotpos=strE.lastIndexOf("."); 
//			if(strE!="")
//			{	if (apos<1||dotpos-apos<2)
//			{	alert("Please, Enter valid Email Address."); txt_email.focus(); txt_email.select(); return false;	}	}
//		}	
//		return true;
//	}
    </script>
    <style type="text/css">
        textarea.form-control {
            height: 102px;
        }
    </style>
<%--<form id="form1" runat="server">--%>

    <div class="container" >
        <div class="row">
      <div class="col-sm-12">
               <section class="content-header">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-12">
                            <h1>&nbsp;<%=Resources.Resource.CenterRegistration_ClsRegDts%></h1>
                                </div>
                           </div>
                        <div class="row">
                           <div class="col-sm-12">
                                 <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="red"></asp:Label>
                               </div>
                            </div>
                        <asp:Label runat="server" ID="legendLabel" Visible="false"></asp:Label>
                       </div><!-- /.container-fluid -->
               </section>
                  <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.CenterRegistration_ClassRegis%></h1>
                        </div>
                          <asp:Label runat="server" ID="lblhead" Visible="false"></asp:Label>
                         <div class="card-body">
                            <div class="row">
                                 <div class="col-sm-3">
                                    <div class="form-group">
                                      <label><%=Resources.Resource.CenterRegistration_clsNm%></label>
                                      <asp:TextBox runat="server" class="form-control" id="txtCenterName" MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_clsNm %>"></asp:TextBox>
                                   </div>
                                </div>
                                <div class="col-sm-3">
                                 <div class="form-group" >
                                      <label><%=Resources.Resource.CenterRegistration_clsCode%></label>
                                      <asp:TextBox runat="server" class="form-control" id="txtCentreCode" MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_clsCode %>"></asp:TextBox>
                                 </div>
                                </div>
                                    <div class="col-sm-6">
                                        <div class="form-group">
                                          <label><%=Resources.Resource.CenterRegistration_OwNm%></label>
                                            <div class="input-group">
                                              <asp:TextBox runat="server" class="form-control" id="txtFName"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_FN %>"></asp:TextBox>
                                              <asp:TextBox runat="server" class="form-control" id="txtMName"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_MN %>"></asp:TextBox>
                                              <asp:TextBox runat="server" class="form-control" id="txtLName"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_LN %>"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                             <div  class="row">
                              <div class="col-sm-6">
                                    <div class="form-group" >
                                  <label><%=Resources.Resource.CenterRegistration_Adrss%></label>
                                  <asp:TextBox runat="server" Height="50px" TextMode="MultiLine" class="form-control" id="TxtCentreAddress"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_Adrss %>"></asp:TextBox>
                               </div>
                              </div>
                                <div class="col-sm-3">
                                    <div class="form-group" >
                                  <label><%=Resources.Resource.CenterRegistration_Email%></label>
                                  <asp:TextBox runat="server" class="form-control" id="txtEmail"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_Email %>"></asp:TextBox>
                                </div>
                              </div>
                                  <div class="col-sm-3">
                                 <div class="form-group" >
                                  <label><%=Resources.Resource.CenterRegistration_CntNo%></label>
                                  <asp:TextBox runat="server" class="form-control" id="txtContactNo"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_CntNo %>"></asp:TextBox>
                                </div>
                                      </div>
                                 </div>
                              <div class="row">
                                  <div class="col-sm-3">
                                    <div class="form-group" >
                                  <label><%=Resources.Resource.CenterRegistration_City%></label>
                                  <asp:TextBox runat="server" class="form-control" id="txtCity"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_City %>"></asp:TextBox>
                             </div>
                            </div>
                                  <div class="col-sm-3">
                                    <div class="form-group" >
                                  <label><%=Resources.Resource.CenterRegistration_State%></label>
                                  <asp:TextBox runat="server" class="form-control"  id="txtstate"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_State %>"></asp:TextBox>
                               </div>
                                </div>
                            <div class="col-sm-3">
                                 <div class="form-group" >
                                      <label><%=Resources.Resource.CenterRegistration_Country%></label>
                                      <asp:TextBox runat="server" class="form-control"  id="txtCountry"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_Country %>"></asp:TextBox>
                                 </div> 
                            </div>
                            <div class="col-sm-3">
                                <div class="form-group" >
                                  <label><%=Resources.Resource.CenterRegistration_Pincd%></label>
                                  <asp:TextBox runat="server" class="form-control"  id="txtPinCode"  MaxLength="255" placeholder="<%$Resources:Resource, CenterRegistration_Pincd %>"></asp:TextBox>
                                </div>
                            </div>
                          </div>
                                <div class="row">
                                <div class="col-sm-12" style="text-align:center">
                                   <asp:Button ID="imgBtnSubmit" class="btn btn-primary" runat="server" Text="<%$Resources:Resource, CenterRegistration_btnRegis %>"/>&nbsp;
                                   <asp:Button ID="imgBtnClear" class="btn btn-primary" runat="server" Text="<%$Resources:Resource, CenterRegistration_btnClear %>"/>&nbsp;
                                   <asp:Button ID="imgBtnUpdate" class="btn btn-primary" runat="server" Text="<%$Resources:Resource, Common_Update %>" Visible="False" />&nbsp;
                                   <asp:Button ID="ImgBtnBack" class="btn btn-primary" Text="<%$Resources:Resource, Common_btnBck %>" runat="server"/>&nbsp;
                                   <asp:Button ID="imgBtnReset" class="btn btn-primary" Text="<%$Resources:Resource, Common_Reset %>" runat="server" Visible="False" />
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
    <!-- Code Ended by Rajat Argade on 23/10/2019 -->
</asp:Content>
