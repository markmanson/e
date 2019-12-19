<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReassignExam.aspx.vb" Inherits="Unirecruite.ReassignExam" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat ="server" id ="h1">
    <title>Online Examinations solution------Candidate Status</title>
    <!--StartFragment -->

    <script language="JavaScript" type="text/JavaScript">		
		function checkAll(){		
                   var chkId = "DGReport__ctl";
                             var i =3;
                             var isSelected = false;
                            for(;;){
                               if(!document.getElementById(chkId+i+"_ChkSel")){
                                          break;
                               }else {
                                          if(document.getElementById(chkId+i+"_ChkSel").checked){
                                                        isSelected = true;
                                                        break;
                                          }
                               }
                               ++i;
                            }
                            //return isSelected;
                            if(!isSelected){
                                          alert("Please select at least one element");
                            }else{
                                    var answer = confirm("Do you want to delete selected Test Record?");
				                    if(!answer)
				                    {
					                    window.event.returnValue = false;					
				                    }			
                            }
              }

		
//		function ConDel()
//			{
//				var answer = confirm("Do you want to delete the candidate?");
//				if(!answer)
//				{
//					window.event.returnValue = false;					
//				}				
//			}
		

		
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

//-->
////Created By : Aalok Parikh
////Descrioption : Date Validation
//var dtCh= "/";
//var minYear=1900;
//var d = new Date();
//var maxYear=d.getFullYear();
//var D_Entry;
//var Today;
//function isInteger(s)
//{
//	var i;
//    for (i = 0; i < s.length; i++)
//    {   
//        // Check that current character is number.
//        var c = s.charAt(i);
//        if (((c < "0") || (c > "9"))) return false;
//    }
//    // All characters are numbers.
//    return true;
//}

//function stripCharsInBag(s, bag)
//{
//	var i;
//    var returnString = "";
//    // Search through string's characters one by one.
//    // If character is not in bag, append to returnString.
//    for (i = 0; i < s.length; i++)
//    {   
//        var c = s.charAt(i);
//        if (bag.indexOf(c) == -1) returnString += c;
//    }
//    return returnString;
//}

//function daysInFebruary (year)
//{
//	// February has 29 days in any year evenly divisible by four,
//    // EXCEPT for centurial years which are not also divisible by 400.
//    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
//}
//function DaysArray(n) 
//{
//	for (var i = 1; i <= n; i++) 
//	{
//		this[i] = 31;
//		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30;}
//		if (i==2) {this[i] = 29;}
//    } 
//   return this
//}

//function isDate(dtStr)
//{
////    var nameOfControl = dtStr.toString();
//	var daysInMonth = DaysArray(12);
//	var pos1=dtStr.indexOf(dtCh);
//	var pos2=dtStr.indexOf(dtCh,pos1+1);
//	var strYear=dtStr.substring(0,pos1);
//	var strMonth=dtStr.substring(pos1+1,pos2);
//	var strDay=dtStr.substring(pos2+1);
//	var strYr=strYear;
//	if (strDay.charAt(0)=="0" && strDay.length>1) 
//	{   strDay=strDay.substring(1); }
//	if (strMonth.charAt(0)=="0" && strMonth.length>1)
//	{   strMonth=strMonth.substring(1); }
//	for (var i = 1; i <= 3; i++)
//	{
//		if (strYr.charAt(0)=="0" && strYr.length>1) 
//		{ strYr=strYr.substring(1);}
//	}
//	month=parseInt(strMonth);
//	day=parseInt(strDay);
//	year=parseInt(strYr);
//	if (pos1==-1 || pos2==-1){
//		alert("The date format should be : yyyy/mm/dd");
//		return false;
//	}
//	if (strMonth.length<1 || month<1 || month>12){
//		alert("Please enter a valid month");
//		return false;
//	}
//	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>daysInFebruary(year)) || day > daysInMonth[month]){
//		alert("Please enter a valid day");
//		return false;
//	}
//	if (strYear.length != 4 || year==0 || year<minYear || year>maxYear){
//		alert("Please enter a valid 4 digit year between "+minYear+" and "+maxYear);
//		return false;
//	}
//	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || isInteger(stripCharsInBag(dtStr, dtCh))==false){
//		alert("Please enter a valid date");
//		return false;
//	}
//return true;
//}

//function ValidateDate(field){
//	var dt=field;
//	if(dt.value!="")
//	{
//	    if (isDate(dt.value)==false)
//	    {
//		    dt.focus();
//		    return false;
//	    }
////	    else
////	    {
////	        var dateEntered = dt.value;
//////	        var today_temp = new Date();
//////	        var y_t = d.getFullYear();
//////	        var m_t = d.getMonth();
//////	        var d_t = d.getDate();

////	        var pos1=dateEntered.indexOf(dtCh);
////	        var pos2=dateEntered.indexOf(dtCh,pos1+1);
////	        var Year=parseInt(dateEntered.substring(0,pos1),10);
////	        var Month=parseInt(dateEntered.substring(pos1+1,pos2),10);
////	        var Day=parseInt(dateEntered.substring(pos2+1),10);
////	        var D_Entry = new Date(Year,Month,Day);
////	        var Today = new Date(y_t,m_t+1,d_t);
////	        if(D_Entry > Today)
////	        {   alert("Please, Enter To Date gratter then From Date.");
////	            dt.focus();
////	            return false;
////	        }
////	    }
//	}
////	else
////	{
////	    alert("Please, Enter Birth Date.");
////	    dt.focus();
////	    return false;
////	}
//    return true;
// }
//function compareDate(field1,field2)
//{
//    if (field1.value == "")
//    {
//        alert("Please, Enter From Date."); return false;
//    }    
//    else if (field2.value <> "")
//    {
//        dt1 = field1.value;
//        dt2 = field2.value;
//        var pos1=dt1.indexOf(dtCh);
//	    var pos2=dt1.indexOf(dtCh,pos1+1);
//	    var Year=parseInt(dt1.substring(0,pos1),10);
//	    var Month=parseInt(dt1.substring(pos1+1,pos2),10);
//	    var Day=parseInt(dt1.substring(pos2+1),10);
//	    var D_Entry = new Date(Year,Month,Day);
//	    var pos1=dt2.indexOf(dtCh);
//	    var pos2=dt2.indexOf(dtCh,pos1+1);
//	    var y_t=parseInt(dt2.substring(0,pos1),10);
//	    var m_t=parseInt(dt2.substring(pos1+1,pos2),10);
//	    var d_t=parseInt(dt2.substring(pos2+1),10);
//	    var Today = new Date(y_t,m_t,d_t);
//	    if(D_Entry < Today)
//	    {
//	        alert("Please, Enter To date greater then From date."); 
//	        field1.focus();
//	        return false;
//	    }
//    }
//    return true;
//}
function addTitleAttributes(field)

{
//debugger;
   numOptions = document.getElementById(field.id).options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById(field.id).options[i].title = document.getElementById(field.id).options[i].text;

   

}


    </script>

    <style type="text/css">
        
        .modalBackground1 { 
    
        background-color: #fff;
    filter:alpha(opacity=60); 
    opacity:0.7px; 
    } 
        .modalBackground
        {

        background-color: Gray; 
        filter: alpha(opacity=70);

        opacity: 0.7; 
        }

        .modalPopup {

        background-color: #FFFFFF; 
        border-width: 3px;

        border-style: solid; 
        border-color: #0071B7  ;

        padding: 10px; 
        width: 180px;

        text-align: center; 
        }

        .entryHeader
        {
            font-size: 50px;
            font-family: Arial, Helvetica, sans-serif;
            color: =="#ffffff";}
        .genHeader
        {
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: =="Black";}
        .lblFromTo
        {
            font-size: 13px;
            font-family: Arial, Helvetica, sans-serif;
            color: =="Black";}
        .WrapperDiv
        {
            width: 800px;
            border: 1px solid black;
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: =="#ffffff";position:relative;}</style>
    <!-- InstanceBegin template="/loginlayouts/newhtm/Templates/template.dwt" codeOutsideHTMLIsLocked="false" -->
    <!-- InstanceBeginEditable name="doctitle" -->
    <!-- InstanceEndEditable -->
    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS">
    <link href="images/css.css" type="text/css" rel="stylesheet">
    <!-- InstanceBeginEditable name="head" -->
    <!-- InstanceEndEditable -->
    <!-- InstanceParam name="body" type="boolean" value="true" -->
</head>
<body <%--leftmargin="0" topmargin="0"--%> >
    <form id="FrmCStatus" name="frm_login" action="" method="post" runat="server">
    <table cellpadding="0" cellspacing="0" align="center" width="80%" >
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
                                <img src="images/btnhome.jpg" border="0"></a> <a href="login.aspx" >
                                    <img src="images/btnlogoff.jpg" border="0" id="IMG2"></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <%--Content Here--%>
            <td class="pageContentTD" width="100%" align="center" valign="top" height="390px">
                <table cellspacing="0" cellpadding="0" width="100%" align="center" border="0">
                    <tr>
                        <td align="center">
                            <table width="100%" border="0" cellspacing="0">
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:Label ID="LblMsg" runat="server" Visible="False" CssClass="genHeader" Font-Bold="True"
                                                ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%">
                                            <fieldset >
                                                <legend class="outerframe">Search Conditions</legend>
                                                <table class="content" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2" class="tblhead" style="height: 18px">
                                                            <center>
                                                                Candidate Details</center>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 420px; height: 100%;" align="left" valign="top">
                                                            <table width="100%" cellspacing="0">
                                                                <tr style="color: #000000; margin-top: 0px">
                                                                    <td class="tdcontent_label" style="height: 27px">
                                                                        <asp:Label ID="LblUserName" CssClass="genText" Text="Centre Name" runat="server" />
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 27px">
                                                                        <asp:DropDownList ID="dblCenter" runat="server" Width="320px" AutoPostBack="false"
                                                                            ToolTip="Centre Name" onmouseover="addTitleAttributes(dblCenter);">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdcontent_label" style="height: 28px">
                                                                        <asp:Label ID="Label10" CssClass="genText" Text="Course Name" runat="server" />
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 28px">
                                                                        <asp:DropDownList ID="ddlcourse" runat="server" Width="320px" ToolTip="Course Name"
                                                                            onmouseover="addTitleAttributes(ddlcourse);">
                                                                        </asp:DropDownList>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                        <td class="tdcontent_label">
                                                                            &nbsp;<asp:Label ID="LblTestName" Text="Subject Name" runat="server" CssClass="genText"></asp:Label></td>
                                                                        <td class="tdcontent_data">
                                                                            <asp:DropDownList ID="ddlTestName" runat="server" Width="180px" ToolTip="Subject Name" onmouseover="addTitleAttributes(ddlTestName);">
                                                                            </asp:DropDownList></td>
                                                                    </tr>--%>
                                                                <tr>
                                                                    <td class="tdcontent_label" valign="top" style="height: 28px">
                                                                        <asp:Label ID="Label11" CssClass="genText" Text="Candidate Name" runat="server" />
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 28px">
                                                                        <asp:TextBox ID="TxtUserName" runat="server" MaxLength="50" Width="180px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr style="margin: 0px; height: 27px">
                                                                    <td class="tdcontent_label" style="height: 28px">
                                                                        <%--<asp:Label ID="LblStatus" CssClass="genText" Text="Exam Status" runat="server" />--%>
                                                                    </td>
                                                                    <td class="tdcontent_data" style="margin: 0px; height: 28px">
                                                                        <%--<asp:DropDownList ID="ddlStatus" runat="server" Width="180px" ToolTip="Exam Status"
                                                                            onmouseover="addTitleAttributes(ddlStatus);">
                                                                        </asp:DropDownList>--%>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                        <td class="tdcontent_label" valign="top" style="height: 27px">
                                                                        <asp:Label ID="lblgrade" CssClass="genText" Text="Grade" runat="server" />
                                                                        </td>
                                                                        <td class="tdcontent_data" style="height: 27px">
                                                                        <asp:DropDownList ID="ddlgrade" runat="server" Width="140px" ToolTip="Grade"
                                                                                onmouseover="addTitleAttributes(ddlgrade);">
                                                                                <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                                <asp:ListItem Text="A+"></asp:ListItem>
                                                                                <asp:ListItem Text="A"></asp:ListItem>
                                                                                <asp:ListItem Text="B"></asp:ListItem>
                                                                                <asp:ListItem Text="C"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>--%>
                                                                <tr>
                                                                    <td class="tdcontent_label" valign="top" style="height: 28px">
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 28px">
                                                                    </td>
                                                                </tr>
                                                                <%-- <tr>
                                                            <td align="right">
                                                                <asp:Label ID="LblResult" CssClass="genHeader" Text="Result" runat="server" /></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlResult" runat="server" Width="150px">
                                                                </asp:DropDownList></td>
                                                        </tr>--%>
                                                            </table>
                                                        </td>
                                                        <td valign="top" style="width: 480px; height: 100%;">
                                                            <table width="100%" cellspacing="0">
                                                                <%--<tr style="margin: 0px">
                                                                        <td class="tdcontent_label">
                                                                            <asp:Label ID="LblStatus" CssClass="genText" Text="Exam Status" runat="server" /></td>
                                                                        <td class="tdcontent_data">
                                                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="140px" ToolTip="Exam Status" onmouseover="addTitleAttributes(ddlStatus);">
                                                                            </asp:DropDownList></td>
                                                                    </tr>--%>
                                                                <tr>
                                                                    <%--<td class="tdcontent_label" valign="top" style="height: 27px">
                                                                        <asp:Label ID="lblgrade" CssClass="genText" Text="Grade" runat="server" />
                                                                    </td>--%>
                                                                    <%--<td class="tdcontent_data" style="height: 27px">
                                                                        <asp:DropDownList ID="ddlgrade" runat="server" Width="140px" ToolTip="Grade" onmouseover="addTitleAttributes(ddlgrade);">
                                                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                                                            <asp:ListItem Text="A+"></asp:ListItem>
                                                                            <asp:ListItem Text="A"></asp:ListItem>
                                                                            <asp:ListItem Text="B"></asp:ListItem>
                                                                            <asp:ListItem Text="C"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdcontent_label" style="height: 27px">
                                                                        <asp:Label ID="LblExamDate" CssClass="genText" Text="Exam Reassigned  Date" runat="server" />
                                                                        (<asp:Label ID="Label1" CssClass="genText" Text="From" runat="server" />)
                                                                        <%----%>
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 27px">
                                                                        <asp:TextBox ID="TxtFrom" runat="server" MaxLength="10" Width="140px"></asp:TextBox>
                                                                          <asp:ImageButton ID ="imgbtnfrom" Height="20" Width="20" ImageUrl="images/calander.gif"  runat="server" />
                                                           
                                                             <cc1:CalendarExtender ID="CalendarExtender2" PopupPosition="Right"         TargetControlID="TxtFrom"
                                                        Format="dd/MM/yyyy" PopupButtonID="imgbtnfrom" runat="server">
                                                    </cc1:CalendarExtender>
                                                                        <%--<span
                                                                            class="tdlabel">(<asp:Label ID="Label3" runat="server" Text="YYYY/MM/DD" CssClass="genText"></asp:Label>)</span>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdcontent_label" style="height: 27px">
                                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; (<asp:Label
                                                                            ID="Label2" runat="server" CssClass="genText">To</asp:Label>)
                                                                        <%--(<asp:Label ID="Label4" runat="server" Text="yyyy/mm/dd" CssClass="lblFromTo"></asp:Label>)--%>
                                                                    </td>
                                                                    <td class="tdcontent_data">
                                                                        <asp:TextBox ID="TxtTo" runat="server" MaxLength="10" Width="140px"></asp:TextBox>
                                                                         <asp:ImageButton ID ="imgBtnTo" Height="20" Width="20" ImageUrl="images/calander.gif"  runat="server" />
                                                           
                                                             <cc1:CalendarExtender ID="CalendarExtender3" PopupPosition="Right"         TargetControlID="TxtTo"
                                                        Format="dd/MM/yyyy" PopupButtonID="imgBtnTo" runat="server">
                                                    </cc1:CalendarExtender>
                                                                        
                                                                        <%--<span
                                                                            class="tdlabel">(<asp:Label ID="Label4" runat="server" Text="YYYY/MM/DD" CssClass="genText"></asp:Label>)</span>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdcontent_label" style="height: 27px">
                                                                        <%--<asp:Label ID="Label5" CssClass="genText" Text="Exam Appeared Date" runat="server" />--%>
                                                                       <%-- (<asp:Label ID="Label6" CssClass="genText" Text="From" runat="server" />)--%>
                                                                        <%--(<asp:Label ID="Label7" runat="server" Text="yyyy/mm/dd" CssClass="lblFromTo"></asp:Label>)--%>
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 27px">
                                                                       <%-- <asp:TextBox ID="txtAppFromDate" MaxLength="10" runat="server" Width="140px"></asp:TextBox>--%>
                                                                        <%-- <asp:ImageButton ID ="ImageButton1" Height="20" Width="20" ImageUrl="images/calander.gif"  runat="server" />--%>
                                                           <%--
                                                             <cc1:CalendarExtender ID="CalendarExtender4" PopupPosition="Right"         TargetControlID="txtAppFromDate"
                                                        Format="dd/MM/yyyy" PopupButtonID="ImageButton1" runat="server">
                                                    </cc1:CalendarExtender>--%>
                                                                        <%--<span
                                                                            class="tdlabel">(<asp:Label ID="Label7" runat="server" Text="YYYY/MM/DD" CssClass="genText"></asp:Label>)</span>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="tdcontent_label" style="height: 27px">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; 
                                                                        <%--(<asp:Label ID="Label9" runat="server" Text="yyyy/mm/dd" CssClass="lblFromTo"></asp:Label>)--%>
                                                                    </td>
                                                                    <td class="tdcontent_data" style="height: 27px">
                                                                        <%--<asp:TextBox ID="TxtAppToDate" MaxLength="10" runat="server" Width="140px"></asp:TextBox>--%>
                                                                        <%--    <asp:ImageButton ID ="ImageButton2" Height="20" Width="20" ImageUrl="images/calander.gif"  runat="server" />
                                                           
                                                             <cc1:CalendarExtender ID="CalendarExtender5" PopupPosition="Right"         TargetControlID="TxtAppToDate"
                                                        Format="dd/MM/yyyy" PopupButtonID="ImageButton2" runat="server">
                                                    </cc1:CalendarExtender>--%>
                                                                        <%--<span
                                                                            class="tdlabel">(<asp:Label ID="Label9" runat="server" Text="YYYY/MM/DD" CssClass="genText"></asp:Label>)</span>--%>
                                                                    </td>
                                                                </tr>
                                                                <%-- <tr>
                                                            <td colspan="2">
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                 
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td colspan="2" align="left" style="height: 31px">
                                            <asp:ImageButton ID="BtnSearch" runat="server" ImageUrl="images/btnsearch.jpg"
                                                AlternateText="Search the candidate"></asp:ImageButton>&nbsp;<asp:ImageButton runat="server"
                                                    ID="btnclear" ImageUrl="images/btnclear.jpg" AlternateText="Clear" ToolTip="Clear"
                                                    CausesValidation="false"></asp:ImageButton>&nbsp;
                                            <asp:ImageButton ID="btnBack" runat="server" ImageUrl="images/BtnBack.jpg" AlternateText="Back"
                                                ToolTip="Back" CausesValidation="False"></asp:ImageButton>&nbsp;
                                              <%--  <asp:ImageButton
                                                    ID="btnPrint" runat="server" ImageUrl="images/btnprint.jpg" AlternateText="New Question"
                                                    ToolTip="print the report"></asp:ImageButton>&nbsp;
                                            <asp:ImageButton
                                                    ID="imgbtnExport" runat="server" ImageUrl="images/btnExport.jpg" AlternateText="Export
                                                    ToolTip="Export to Excel" Enabled="False" Visible="False"></asp:ImageButton>
                                                    &nbsp;
                                                     <asp:ImageButton
                                                    ID="imgbtnReport" runat="server" ImageUrl="images/imgbtnReport.jpg" AlternateText="Export
                                                    ToolTip="Report" Visible="false"    ></asp:ImageButton>  --%>
                                                  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" valign="top">
                                            <%--s--%>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                    </asp:ScriptManager>
                                                    <asp:Panel ID="Panel1" Width="210px"   Style="display: none"
                                                        runat="server">
                                                        <div class="modalBackground">
                                                           <%-- <div class="modalPopup " id="PopupHeader">
                                                            </div>--%>
                                                            <div class="modalPopup">
                                                                <table style="width:100%">
                                                                    <tr>
                                                                        <td class="tdcontent_labelPopup">
                                                                            <%--Old Date--%>
                                                                        </td>
                                                                        <td class="tdcontent_dataPopup">
                                                                            <asp:Label ID="lbldate" runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdcontent_labelPopup">
                                                                            New Date
                                                                        </td>
                                                                        <td class="tdcontent_dataPopup"  >
                                                                            <asp:TextBox ID="txtnewdate" Columns="8" runat="server"> &nbsp;</asp:TextBox><asp:ImageButton ID ="btnCalander" Height="20" Width="20" ImageUrl="images/calander.gif"  runat="server" />
                                                                           <%-- <asp:Button ID="Button2"  runat="server" />--%>
                                                                            
                                                                        </td> 
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" align="center">
                                                                            <asp:Button runat="server" ID="btnOkay" type="button" Text="Save" value="Done" />
                                                                             <asp:Button runat="server" ID="btnPopUpCancel" type="button" Text="Cancel" value="Done" />
                                                                            <input id="btnCancel" runat="server" type="button" style="display:none" value="Cancel" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <cc1:CalendarExtender ID="CalendarExtender1" PopupPosition="Right"    TargetControlID="txtnewdate"
                                                        Format="dd/MM/yyyy" PopupButtonID="btnCalander" runat="server">
                                                    </cc1:CalendarExtender>
                                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1"  RepositionMode="RepositionOnWindowResize"
                                                        runat="server" CancelControlID="btnCancel" TargetControlID="lbldate" PopupControlID="Panel1"
                                                        PopupDragHandleControlID="PopupHeader" X="550" Y="300"   BackgroundCssClass="modalBackground1"   >
                                                    </cc1:ModalPopupExtender>
                                               
                                            <div id="gridDiv" runat="server" visible="false">
                                                <fieldset>
                                                    <legend class="outerframe">Search Result </legend>
                                                    <label id="lblRecordCount" class="genMsg">
                                                    </label>
                                                    <div id="label" runat="server" style="text-align: right">
                                                        <asp:Label ID="LblRecCnt" runat="server" Font-Bold="True" CssClass="genHeader" Visible="False"></asp:Label></div>
                                                    <asp:DataGrid ID="DGReport" runat="server" OnPageIndexChanged="DgReport_PageIndexChanged"
                                                        AutoGenerateColumns="False" AllowPaging="True" CssClass="gridClass" DataKeyField="userid"
                                                        Width="100%">
                                                        <SelectedItemStyle CssClass="gSelectedItem" />
                                                        <PagerStyle HorizontalAlign="Left" ForeColor="#000066" Visible="false" BackColor="Transparent" Mode="NumericPages"
                                                            CssClass="gPagerStyle"></PagerStyle>
                                                        <Columns>
                                                            <asp:TemplateColumn Visible="False">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="ChkSel" runat="server" Checked='false' />
                                                                </ItemTemplate>
                                                            </asp:TemplateColumn>
                                                            <asp:BoundColumn Visible="False" DataField="userid" HeaderText="userid">
                                                                <ItemStyle CssClass="gItemStyleNum" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Sr.No." HeaderText="Sr.No.">
                                                                <ItemStyle CssClass="gItemStyleNum" />
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="UserName" HeaderText="Candidate Name">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Center_Name" HeaderText="Center Name">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="course_name" HeaderText="Course Name">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="LoginName" HeaderText="Login Name" HeaderStyle-Width="70px">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Password" HeaderText="Password">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="WrittenTestDate" HeaderText="Exam Asssigned Date" DataFormatString="{0:dd/MM/yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:ButtonColumn HeaderText=" " CommandName="EditDate" ItemStyle-Width="0px" Text="Edit">
                                                                <ItemStyle CssClass="gButtonStyle" />
                                                            </asp:ButtonColumn>
                                                            <asp:ButtonColumn HeaderText="Disable Exam" CommandName="Disable" ItemStyle-Width="0px" Text="Disable">
                                                                <ItemStyle CssClass="gButtonStyle" />
                                                            </asp:ButtonColumn>
                                                            <asp:BoundColumn DataField="AppearanceDate" HeaderText="Exam Appeared Date-Time"
                                                                DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="total_marks" HeaderText="Maximum Marks">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <%--<asp:BoundColumn DataField="Attempted" HeaderText="Attempted">
                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                                </asp:BoundColumn>--%>
                                                            <asp:BoundColumn DataField="obtained_marks" HeaderText="Marks Obtained">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Status" HeaderText="Status">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="Grad" HeaderText="Grade">
                                                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:ButtonColumn HeaderText="Provision MarkSheet"  CommandName="MarkSheet" Text="MarkSheet">
                                                                <ItemStyle CssClass="gButtonStyle" />
                                                            </asp:ButtonColumn>
                                                            <asp:BoundColumn DataField="course_id" HeaderText="couse_id" Visible="False">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:BoundColumn>
                                                            <asp:BoundColumn DataField="appearedflag" HeaderText="appearedflag" Visible="False">
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:BoundColumn>
                                                        </Columns>
                                                        <HeaderStyle CssClass="ghead" />
                                                    </asp:DataGrid>
                                                    
                                                    
                                       <table align="left" > <tr> 
                                    <%-- Display the First Page/Previous Page buttons --%>
                                    
                                    <td><%--<asp:LinkButton ID="Firstbutton" Text="First" CommandArgument="0" runat="server" OnClick="PagerButtonClick" />--%>
                                    <asp:ImageButton ID="imgfirst" runat="server" Height="15px" 
                                            ImageUrl="images/first1.gif" Width="10px" CommandArgument="0" ToolTip="First"  OnClick="PagerButtonClick" /></td>
                                    <td><%--<asp:LinkButton ID="Prevbutton" Text="Prev" CommandArgument="prev" runat="server" OnClick="PagerButtonClick" />--%>
                                    <asp:ImageButton ID="imgprev" runat="server" Height="15px" 
                                            ImageUrl="images/prev1.gif" Width="10px" CommandArgument="prev" ToolTip="Previous" OnClick="PagerButtonClick"/>
                                    </td>
                                    <%-- Display the Page No. buttons --%>
                                    <td><table id="tblPagebuttons" runat="server" >
                                    </table></td>
                                    <%-- Display the Next Page/Last Page buttons --%>
                                    <td>
                                        <asp:ImageButton ID="imgnext" runat="server" Height="15px" 
                                            ImageUrl="images/next1.gif" Width="10px" CommandArgument="next" ToolTip="Next" OnClick="PagerButtonClick" /><%--<asp:LinkButton ID="Nextbutton" Text="Next" CommandArgument="next" runat="server" OnClick="PagerButtonClick" />--%></td>
                                    <td><%--<asp:LinkButton ID="Lastbutton" Text="Last" CommandArgument="last" runat="server"
                                        OnClick="PagerButtonClick" />--%><asp:ImageButton ID="imglast" runat="server" Height="15px" 
                                            ImageUrl="images/last1.gif" Width="10px" CommandArgument="last" ToolTip="Last" OnClick="PagerButtonClick"/></td>
                                        <td> &nbsp;&nbsp;<span style="color:#526B94"> Go to page no.</span> <asp:DropDownList ID="ddlPages" AutoPostBack="true" runat="server"></asp:DropDownList> </td>
                                        
                                        </tr> </table>
                                                    
                                                    
                                                </fieldset>
                                            </div>
                                             </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <%--  e--%>
                                        </td>
                                    </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       <tr>
                <td style="background-color: #526B94; " >
                <table width="100%" >
                    <tr>
                        
                        <td align="right" colspan="2" style="width:65%"><span class="copyright"  >Copyright &#174 2010-11 Unikaihatsu Software Pvt.Ltd., All Rights Reserved.Unikaihatsu Software Pvt.Ltd., All Rights Reserved.</td>
                        <td align="right" style="width:35%" > <span class="copyright"> Site Powered by: <a href="http://www.usindia.com" target="_blank" style="color:White">Unitech Systems</a></span></td>
                    </tr>
                </table>
           
               </td>
            </tr>
    </table>
    </form>
    <!-- InstanceEnd -->
    <%--</FONT></TR></TBODY></TABLE>--%>
</body>
</html>
