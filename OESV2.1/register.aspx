<%@ Page ClientTarget="UpLevel" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.register" MasterPageFile="~/MasterPage.Master" Language="vb" Title="OESV2---Registration" CodeBehind="register.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">

    <script type="text/javascript">
        function isNumberKey(evt)
          {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

             return true;
          }
    </script>
   
  <script language="javascript" type="text/javascript">
	
 var browser_type=navigator.appName;
 var browser_version=parseInt(navigator.appVersion);
 var wd = screen.width;
 var ht = screen.height;
 var i;
   function fnc_limitperadd(source, arguments)
   {
		var str = document.frm_register.txt_peradd.value;
		if (str.length>100)
		{
   			alert("The length of Description has exceeded 100 characters");
   			document.frm_register.txt_peradd.focus();
		}
   }
   
 function checkResolution(url)
 {
  if (wd == 800 && ht == 600)
  {
   window.open(url,"","fullscreen, scrollbars=no, width="+wd+",height="+ht)
        }
  if (wd > 800 && ht > 600)
  {
   window.open(url,"","fullscreen, scrollbars=no, width="+wd+",height="+ht)
        }
  if (wd < 800 && ht < 600)
  {
   window.open(url,"","fullscreen, scrollbars=no, width="+wd+",height="+ht)
        }
    }
 function openWindow(url) 
 {
    if (browser_type=="Microsoft Internet Explorer" && browser_version>=4) 
    {
   checkResolution(url);
    }
    else
    if (browser_type=="Netscape" && browser_version>=4) 
       {
            windowFeatures='width='+wd+',height='+ht+' toolbar=0,location=0,directories=0,status=0,menuBar=0,titlebar=0,scrollBars=no,resizable=0,screenx=0,screeny=0';
            return window.open( url, "win", windowFeatures)
    }
 }

 function Validation()
 {
       var CityId=document.getElementById("cmb_city").selectedIndex;
       var CityText=document .getElementById ("cmb_city")[CityId].text;
        if(CityText =="Others")
        {                
            if(document.getElementById("txt_others").value=="")
            {
                alert("Please, Enter Other City Name.");
                document.getElementById("txt_others").focus();
                return false;
            } 
        }
 }
function addTitleAttributes(field)
{
//debugger;
   numOptions = document.getElementById(field.id).options.length;
   for (i = 0; i < numOptions; i++)
      document.getElementById(field.id).options[i].title = document.getElementById(field.id).options[i].text;
}
    function NotNullField(field)
    {
        if(field.value=="")
        {   return false;   }
        return true;
    }
	function ChkNumber(field)
	{
		var filter =  /[^0-9]/ ;
		if(filter.test(field.value))
		{
			
			//else
			//{
			    alert("Please, Enter Digits only.");
			    field.focus();
			    return false;
			//}
		}
		else if(field.value.length<10)
			{   alert("Please, Enter 10 digit Mobile Number."); field.focus(); return false;   }
		else
		{
		    return true;
		}
	}
	
    function textvalidater(field)
    {
        var patt1=/[0-9!@#$%^&*,.?:;()-=+:;`~_{}\/\\\|\"<>\[\]\{\}]/g;
		if(patt1.test(field.value))
		{	alert("Please, Enter Alphabets only.");
//		field.focus(); field.select();
		  return false; 	}
		return true;
    }
function textrequiredvalidater(field)
	{	
//with(frmDetail)
//		{
//			var strF=txtFirst.value; var strM=txtIni.value; var strL=txtLast.value;
//			if(field.value=="")
//            {   alert("Please, Enter value for currently focused field.",field); return false;    }
			var patt1=/[0-9!@#$%^&*,.?:;()-=+:;`~_{}\/\\\|\"<>\[\]\{\}]/g;
			if(patt1.test(field.value))
			{	alert("Please, Enter Alphabets Only."); 
//			field.focus(); field.select();
			 return false; 	}
//			if(patt1.test(strM))
//			{	alert("Enter Valid Initial"); txtIni.focus(); txtIni.select(); return false;	}
//			if(patt1.test(strL))
//			{	alert("Enter Valid Last Name"); txtLast.focus(); txtLast.select(); return false;	}
//		}
			return true;
	}
	function emailvalidater()
	{	
	with(document.frm_register)
		{
            var strE=txt_email.value;
            if(strE == "")
            {	alert("Please, Enter Email Address."); txt_email.focus(); return false;	}
             apos=strE.indexOf("@"); dotpos=strE.lastIndexOf("."); 
			if(strE!="")
			{	if (apos<1||dotpos-apos<2)
			{	alert("Please, Enter valid Email Address."); txt_email.focus(); txt_email.select(); return false;	}	}
		}
		return true;
	}
	function success()
	{

	    with(document.frm_register)
	    {
	        if(txt_firstname.value=="")
	        {    alert("Please enter the value in First name."); txt_firstname.focus(); return false;  }
	        if(!textvalidater(txt_firstname))
	        {    txt_firstname.focus(); return false;   }
	        
	        if(!textvalidater(txt_middlename))
	        {    txt_middlename.focus(); return false;   }
	        
	        if(txt_surname.value=="")
	        {    alert("Please enter the value in Last Name."); txt_surname.focus(); return false;  }
	        if(!textrequiredvalidater(txt_surname))
	        {    txt_surname.focus(); return false;   }
	        
	          if(!ValidateDate())
	        {   return false;   }
	        
	         if(!emailvalidater())
	        {   return false;    }
	        
	        if(!ChkNumber(txt_phone))
	        {   return false;    }
	        
	        if(!NotNullField(txt_login))
	        {   alert("Please enter the value in Login ID."); txt_login.focus(); return false;    }
	        if(!NotNullField(txt_password))
	        {   alert("Please enter the value in Password."); txt_password.focus(); return false; }
	        if(!NotNullField(txt_confpassword))
	        {   alert("Please enter the value in Confirm Password."); txt_confpassword.focus(); return false; }
	        if(txt_password.value != txt_confpassword.value)
	        {   alert("Password & Confirm Password are Different."); txt_confpassword.focus(); return false;    }
	        
//	        if(!textvalidater(txt_indlang))
//	        {    txt_indlang.focus(); return false;  }
//            if(!textvalidater(txt_frnlang))
//            {    txt_frnlang.focus(); return false;  }
        }
        return true;
	}
/**
 * DHTML date validation script. Courtesy of SmartWebby.com (http://www.smartwebby.com/dhtml/)
 */
// Declaring valid date character, minimum year and maximum year
var dtCh= "/";
var minYear=1900;
var d = new Date();
var maxYear=d.getFullYear();
var D_Entry;
var Today;
function isInteger(s)
{
	var i;
    for (i = 0; i < s.length; i++)
    {   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

function stripCharsInBag(s, bag)
{
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++)
    {   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

function daysInFebruary (year)
{
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}
function DaysArray(n) 
{
	for (var i = 1; i <= n; i++) 
	{
		this[i] = 31;
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30;}
		if (i==2) {this[i] = 29;}
    } 
   return this
}

function isDate(dtStr)
{
//    var nameOfControl = dtStr.toString();
	var daysInMonth = DaysArray(12);
	var pos1=dtStr.indexOf(dtCh);
	var pos2=dtStr.indexOf(dtCh,pos1+1);
	var strYear=dtStr.substring(0,pos1);
	var strMonth=dtStr.substring(pos1+1,pos2);
	var strDay=dtStr.substring(pos2+1);
	var strYr=strYear;
	if (strDay.charAt(0)=="0" && strDay.length>1) 
	{   strDay=strDay.substring(1); }
	if (strMonth.charAt(0)=="0" && strMonth.length>1)
	{   strMonth=strMonth.substring(1); }
	for (var i = 1; i <= 3; i++)
	{
		if (strYr.charAt(0)=="0" && strYr.length>1) 
		{ strYr=strYr.substring(1);}
	}
	month=parseInt(strMonth);
	day=parseInt(strDay);
	year=parseInt(strYr);
	if (pos1==-1 || pos2==-1 || pos1!=4){
		alert("The date format should be : yyyy/mm/dd");
		return false;
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Please enter a valid month");
		return false;
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
		alert("Please enter a valid day");
		return false;
	}
	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
		alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear);
		return false;
	}
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
		alert("Please enter a valid date");
		return false;
	}
return true;
}

function ValidateDate(){
	var dt=document.frm_register.txt_dob;
	if(dt.value!="")
	{
	    if (isDate(dt.value)==false)
	    {
		    dt.focus();
		    return false;
	    }
	    else
	    {
	        var dateEntered = dt.value;
//	        var today_temp = new Date();
	        var y_t = d.getFullYear();
	        var m_t = d.getMonth();
	        var d_t = d.getDate();
	        var pos1=dateEntered.indexOf(dtCh);
	        var pos2=dateEntered.indexOf(dtCh,pos1+1);
	        var Year=parseInt(dateEntered.substring(0,pos1),10);
	        var Month=parseInt(dateEntered.substring(pos1+1,pos2),10);
	        var Day=parseInt(dateEntered.substring(pos2+1),10);
	        var D_Entry = new Date(Year,Month,Day);
	        var Today = new Date(y_t,m_t+1,d_t);
	        if(D_Entry > Today)
	        {   alert("Please, Enter Birth Date less than Today's date.");
	            dt.focus();
	            return false;
	        }
	    }
	}
	else
	{
	    alert("Please, Enter Date of Birth.");
	    dt.focus();
	   return false;
	}
    return true;
 }


    </script>

    

    <script type="text/javascript">

$(function(){

$(".multiselect").multiselect();

});


 debugger;
 function valid()
 {
    if(document.getElementById("txt_firstname").value == "")
    {
        document.getElementById("lblMsg").innerText = "Please Enter FirstName.";
        document.getElementById("txt_firstname").focus();
        return false;
    }
    if(document.getElementById("txt_surname").value == "")
    {
        document.getElementById("lblMsg").innerText = "Please Enter LastName.";
        document.getElementById("txt_surname").focus();
        return false;
    }
    if(document.getElementById("txt_dob").value == "")
    {
        document.getElementById("lblMsg").innerText = "Please Enter Date of Birth.";
        document.getElementById("txt_dob").focus();
        return false;
    }
    if(document.getElementById("txt_email").value == "")
    {
        document.getElementById("lblMsg").innerText = "Please Enter Email.";
        document.getElementById("txt_email").focus();
        return false;
    }
    else
    {
        var reg= /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
        var email_value=document.getElementById("txt_email").value;
        if(reg.test(email_value)==false)
        {
            document.getElementById("lblMsg").innerText = "Please Enter Valid Email address.";
            document.getElementById("txt_email").focus();
            return false;
        }
    }
    if(document.getElementById("txtrollnumber").value == "")
    {
        document.getElementById("lblMsg").innerText = "Please Enter Roll Number.";
        document.getElementById("txtrollnumber").focus();
        return false;
    }
    if(document.getElementById("ddlCenters").selectedIndex == 0)
    {
        document.getElementById("lblMsg").innerText = "Please Select any Class Name.";
        document.getElementById("ddlCenters").focus();
        return false;
    }
    else
    {
        if($('.count').text()=="0 items selected")
        {
            document.getElementById("lblMsg").innerText = "Please Select 1 or More Course Name.";
            return false;
        }
    }
 }
 
    </script>

    <script type="text/javascript">
        $(function(){

        $(".multiselect").multiselect();

        }); 
    </script>



    <style type="text/css">
        .aspNetDisabled{
            display: block;
            width: 100%;
            height: calc(1.75rem + 2px);
            padding: 1.6px 6.4px;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.5;
            color: grey;
            /*background-color: #fff;*/
            background-color: #dee2e6;
            background-clip: padding-box;
            border: 1px solid #ced4da;
            border-radius: .25rem;
            box-shadow: inset 0 0 0 transparent;
            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
        }
        .manda{
            color:red;
            font-size:small;
        }
    </style>

    <%--<form class="content" id="frm_register" action="" method="post" runat="server">--%>
        <!-- Contains page content -->
    <!-- Content Header (Page header) -->
  <div class="container">
    <section class="content-header">
        <div class="row">
          <div class="col-sm-12">
              <h4><asp:Label ID="lblLegend" runat="server" Text="<%$Resources:Resource,register_UserRegis%>"></asp:Label></h4>
          </div>
        </div>
        <div class="row">
          <div class="col-sm-12">
              <asp:Label ID="lblMsg" runat="server" class="errorMsg" ForeColor="Red"></asp:Label>
              <asp:Label ID="LblMsg1" runat="server" Visible="False" ForeColor="Red"></asp:Label>
          </div>
        </div>
    </section>

     <section class="content">
        <div class="row">
          <div class="col-md-12">
            <div class="card card-info">
              <div class="card-header">
                <h3 class="card-title"><%=Resources.Resource.register_UsDts%></h3>
             </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="txt_firstname"> <%=Resources.Resource.register_UserTy%></label>
                                <asp:DropDownList class="form-control" ID="cmb_usertype" runat="server" AutoPostBack="false" onmouseover="addTitleAttributes(cmb_usertype)" onchange="toggle()">
                                    <asp:ListItem Value="0" Text="<%$Resources:Resource,register_Student%>"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="<%$Resources:Resource,register_Admin%>"></asp:ListItem>
                                </asp:DropDownList>
                           </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="txt_firstname"> <%=Resources.Resource.register_FNm%></label><span class="manda">*</span>
                                <asp:TextBox ID="txt_firstname" runat="server" MaxLength="50" placeholder="<%$Resources:Resource,register_FNm%>" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="txt_middlename"><%=Resources.Resource.register_MidNm%></label>
                                <asp:TextBox ID="txt_middlename" runat="server" MaxLength="50" placeholder="<%$Resources:Resource,register_MidNm%>" class="form-control"></asp:TextBox>                    
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="txt_surname"><%=Resources.Resource.register_LstNm%></label><span class="manda">*</span>
                                <asp:TextBox ID="txt_surname" runat="server" MaxLength="50" placeholder=" <%$Resources:Resource,register_LstNm%>" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-3">
                            <div class="form-group">                    
                                    <label for="rblist_sex"><%=Resources.Resource.register_Gen%></label><span class="manda">*</span>
                                    <asp:RadioButtonList ID="rblist_sex" runat="server" RepeatDirection="Horizontal" TabIndex="5" Font-Size="Small">
                                                <asp:ListItem Selected="True" Value="M" Text="<%$Resources:Resource,register_M%>"></asp:ListItem>
                                                <asp:ListItem Value="F" Text="<%$Resources:Resource,register_F%>"></asp:ListItem>
                                    </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                  <label for="txt_dob"> <%=Resources.Resource.register_DOB%></label>
                                  <input id="txt_dob" runat="server" class="form-control" onkeydown="return false"/>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="txt_email"><%=Resources.Resource.register_EmAdd%></label><span class="manda">*</span>
                                <asp:TextBox ID="txt_email" runat="server" MaxLength="50" onkeyup="this.value = this.value.toLowerCase();" placeholder="<%$Resources:Resource,register_EmAdd%>" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <label for="txt_phone"><%=Resources.Resource.register_MobNo%></label>
                                <asp:TextBox ID="txt_phone" runat="server" MaxLength="10" placeholder="<%$Resources:Resource,register_MobNo%>" class="form-control" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="txtperadd"><%=Resources.Resource.register_AddDts%></label>
                                <asp:TextBox ID="txtperadd" runat="server" TextMode="MultiLine" MaxLength="100" Rows="2" Style="resize:none;" placeholder="<%$Resources:Resource,register_AddDts%>" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label for="Uploader"><%=Resources.Resource.register_Photo%></label><span style="font-size:12px; color:black; background-color:bisque">(<%=Resources.Resource.Register_Inst%>)</span><br />
                                <div class="btn btn-default btn-file">
                                    <i class="fas fa-paperclip"></i><%=Resources.Resource.Common_ChoosePhoto%>
                                    <asp:FileUpload ID="Uploader" runat="server"/>
                                </div>
                                <div><asp:Image ID="StudentImage" runat="server" Height="70" ImageUrl="" Width="60" visible="false"/></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
          </div>
        </div>
    </section>

      <div class="row">
          <div class="col-md-6">
               <section class="content">
                <div class="card card-info">
                      <div class="card-header" runat="server" id="headrow">
                        <h3 class="card-title"><%=Resources.Resource.register_LgnDts%></h3>
                      </div>
                      <div class="card-body" runat="server" id="contentrow">
                          <div class="row">
                              <div class="col-sm-4">
                                  <div class="form-group">
                                    <label for="txt_login"><%=Resources.Resource.register_logID%> </label><span class="manda">*</span>
                                    <asp:TextBox ID="txt_login" runat="server" MaxLength="20" placeholder=" <%$Resources:Resource,register_logID%>" class="form-control"></asp:TextBox>
                                </div>
                              </div>
                              <div class="col-sm-4">
                                  <div class="form-group">
                                        <label><asp:Label ID="lbl_password" runat="server" Text="<%$Resources:Resource,register_pwd%>"></asp:Label></label><span class="manda">*</span>
                                        <asp:TextBox ID="txt_password" runat="server" MaxLength="20" TextMode="Password" placeholder="<%$Resources:Resource,register_pwd%>" class="form-control"></asp:TextBox>
                                        <div runat="server" id="trChangePass">
                                            <asp:LinkButton ID="lnk_chpass" runat="server" CausesValidation="False" ToolTip="Change Password">ChangePassword</asp:LinkButton>
                                        </div>
                                  </div>
                              </div>
                              <div class="col-sm-4">
                                  <div class="form-group" id="trpwd" runat="server">
                                        <label><asp:Label ID="lbl_confpassword" runat="server" Text="<%$Resources:Resource,register_ConPwd%>"></asp:Label></label><span id="star" class="manda" runat="server">*</span>
                                        <asp:TextBox ID="txt_confpassword" runat="server" MaxLength="20" TextMode="Password" placeholder="<%$Resources:Resource,register_ConPwd%>" class="form-control"></asp:TextBox>
                                  </div>
                              </div>
                          </div>
                        </div>
                    </div>
          </section>
         </div>
          <div class="col-md-6">
            <section class="content">
            <div class="card card-info" runat="server" id="div1" style="display:block">
              <div class="card-header" runat="server" id="trClassHead" visible="true">
                <h3 class="card-title"><%=Resources.Resource.centerMaintenance_ClsDts%></h3>
              </div>
              <div class="card-body" runat="server" id="div2" visible="true">
                  <div class="row">
                    <div class="col-sm-6">
                    <div class="form-group" runat="server" id="rollnumber" visible="true">
                        <label for="txtrollnumber"><%=Resources.Resource.register_RNo%></label><span class="manda">*</span>
                        <asp:TextBox ID="txtrollnumber" runat="server" MaxLength="50" placeholder="<%$Resources:Resource,register_RNo%>" class="form-control"></asp:TextBox>
                    </div>
                    </div>
                    <div class="col-sm-6">
                       <div class="form-group" id="centerrow" runat="server" visible="true">
                        <label><asp:Label runat="server" ID="Labelcentre" Text="<%$Resources:Resource,register_ClsNm%>"></asp:Label></label><span class="manda">*</span>
                        <asp:DropDownList ID="ddlCenters" runat="server" Font-Names="Times New Roman" onmouseover="addTitleAttributes(ddlCenters)" class="form-control">
                                                                <asp:ListItem Text="----Select----"></asp:ListItem>
                        </asp:DropDownList>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </section>
          </div>          
        </div>
              <div class="card card-info" id="tablediv" runat="server" visible="false">
                  <div class="card-body">
                      <table id="tblResult" style="height: 20px" runat="server" cellspacing="0"> 

                      </table>
                  </div>
              </div>
            <div class="row" style="text-align:center">
              <div class="col-sm-12">
                  <asp:Button ID="img_saveexit" runat="server" Text="<%$Resources:Resource,Common_Save%>" class="btn btn-primary"/>&nbsp;
                  <asp:Button ID="img_update" runat="server" Text="<%$Resources:Resource,Common_Update%>" class="btn btn-primary"/>&nbsp;
                  <asp:Button ID="imgClear" runat="server" CausesValidation="False" Text="<%$Resources:Resource,Common_btnClr%>" class="btn btn-primary"/>&nbsp;
                  <asp:Button ID="btnBack" runat="server" CausesValidation="False" Text="<%$Resources:Resource,Common_btnBck%>" class="btn btn-primary"/>&nbsp;
                  <asp:Button ID="imgReset" runat="server" CausesValidation="False" Text="<%$Resources:Resource,Common_Reset%>" class="btn btn-primary" Visible="False"/>
              </div>
            </div>
      </div>

    <script>
        function toggle() {
            var cont = document.getElementById('_ctl0_ContentPlaceHolder1_div1');
            if (cont.style.display == 'block') {
                cont.style.display = 'none';

            }
            else {
                cont.style.display = 'block';
            }
        }
    </script>

<script type="text/javascript">
        $(document).ready(function ()
		{
            $("#Uploader").change(function () 
			{
                // Get uploaded file extension
                var extension = $(this).val().split('.').pop().toLowerCase();
                // Create array with the files extensions that we wish to upload

 
                var validFileExtensions = ['jpeg', 'jpg', 'png'];
         //Check file extension in the array.if -1 that means the file extension is not in the list. 
                if ($.inArray(extension, validFileExtensions) == -1) 
				{
                    $('#lblMsg').text("Sorry!! Upload only jpg, jpeg, png file").show();
                    // Clear fileuload control selected file
                    $(this).replaceWith($(this).val('').clone(true));
                }
               
            });
        });
    </script>

    <script language="javascript" type="text/javascript">
			//<!--
			/* if(document.frm_register.txt_login.disabled==false){
				document.frm_register.txt_firstname.focus()
	
    }else{
				document.frm_register.txt_firstname.focus()
			}  Commented by Kamal on 2006/01/28 */
			
//			document.frm_register.txt_firstname.focus() // Coded by Kamal on 2006/01/28
			
			/*if(document.frm_register.txt_other_pgrad.value=="")
				document.frm_register.txt_other_pgrad.disabled=true
			else
				document.frm_register.txt_other_pgrad.disabled=false
			*/	
			function enableOther()
			{
				if(document.frm_register.cmb_pgrad.value=="OT")
				{
					document.frm_register.txt_other_pgrad.disabled=false
					document.frm_register.txt_other_pgrad.focus()
				}else{
					document.frm_register.txt_other_pgrad.disabled=true
				}
			}
						//-->
    </script>
</asp:Content>