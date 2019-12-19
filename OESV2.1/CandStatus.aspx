<%@ Page ClientTarget="UpLevel" Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.CandStatus"
    MasterPageFile="~/MasterPage.Master" Title="OESV2------Candidate Status"
    CodeBehind="CandStatus.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

    <script language="JavaScript" type="text/JavaScript">

        
        function checkAll() {
            var chkId = "DGReport__ctl";
              var i = 3;
            var isSelected = false;
            for (; ; ) {
                if (!document.getElementById(chkId + i + "_ChkSel")) {
                    break;
                }
                else {
                    if (document.getElementById(chkId + i + "_ChkSel").checked) {
                        isSelected = true;
                        break;
                    }
                }
                ++i;
            }
            //return isSelected;
            if (!isSelected) {
                alert("Please select at least one element");
            }
            else {

                var answer = confirm("Do you want to delete selected Test Record?");
                if (!answer) {
                    window.event.returnValue = false;
                }
            }

        }
   
//    'Added By: Pragnesha Kulkarni
//        'Date      : 20/07/18
//        'Desc      : This is event for checking and unchecking all checkboxes.
//--------------------------------------------------------------------------------
	  function HeaderCheckBoxClick(checkbox)
       {
          //First Access the GridView Control
        var gridview = document.getElementById('<%=DGReport.ClientID %>');
        debugger;
        //Now get the all the Input type elements in the GridView
        var AllInputsElements = gridview.getElementsByTagName('input');
        var TotalInputs = AllInputsElements.length;
        //Now we have to find the checkboxes in the rows.
        for(var i=0;i< TotalInputs ; i++ )
        {
            if(AllInputsElements[i].type =='checkbox')
            {
                AllInputsElements[i].checked = checkbox.checked;
            }
        }
      }

    function ChildCheckBoxClick(checkbox) {
        var atleastOneCheckBoxUnchecked = false;
        var gridview = document.getElementById('<%=DGReport.ClientID %>');

        //Now get the all the Input type elements in the GridView
        var AllInputsElements = gridview.getElementsByTagName('input');
        var TotalInputs = AllInputsElements.length;
        //Now we have to find the checkboxes in the rows.
        for (var i = 0; i < TotalInputs; i++) 
        {
            if (AllInputsElements[i].type == 'checkbox')
                if (AllInputsElements[i].checked == false)
                 {
                    atleastOneCheckBoxUnchecked = true;
                    break;
                }
        }
            for (var i = 0; i < 1; i++) 
        {
            if (AllInputsElements[i].type == 'checkbox') 
            {
                AllInputsElements[i].checked = !atleastOneCheckBoxUnchecked;
            }
        }
    }
       //-----------------------------------------------------------------------------------  

		function ConDel()
			{debugger;
				var answer = confirm("Do you want to delete the candidate?");
				if(!answer)
				{
					window.event.returnValue = false;					
				}				
			}
		

		
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
        .modalbackground1
        {
            background-color: #000;
            filter: alpha(opacity=40);
            opacity: 0.3;
        }
        /*.modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=0);
            color: black;
            opacity: 1;
        }*/
        .modalPopup
        {
            background-color: #b9eef0;
            border-width: 4px;
            border-style: solid;
            border-color: #1ed3d9;
            padding: 10px;
            text-align: center;
        }
    </style>
<%--<form id="form1" runat="server" defaultbutton="BtnSearch">--%>
     <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">            
            <div class="row">
              <div class="col-sm-12">
                               <h1><%=Resources.Resource.CandStatus_SrchCondition%></h1>
                   </div>
            </div>
            <div class="row">
              <div class="col-sm-12">
                            <asp:Label ID="LblMsg" runat="server" Visible="False"
                                ForeColor="Red"></asp:Label>
                       </div>
            </div>
          </div><!-- /.container-fluid -->
        </section>


                               <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                            <h1 class="card-title"><%=Resources.Resource.CandStatus_CndDts%></h1>
                        </div>
                                      <div class="card-body">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="form-group">
                                                       <label  ><%=Resources.Resource.CandStatus_ClsNm%> </label>
                                                   
                                                        <asp:DropDownList ID="dblCenter" class="form-control" runat="server" AutoPostBack="false"
                                                            ToolTip="Centre Name" onmouseover="addTitleAttributes(dblCenter);">
                                                        </asp:DropDownList>
                                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                                        <label><%=Resources.Resource.CandStatus_CrsNm%> </label>
                                                    
                                                        <asp:DropDownList ID="dblCourse" class="form-control" runat="server" ToolTip="Course Name"
                                                            onmouseover="addTitleAttributes(dblCourse);">
                                                        </asp:DropDownList>
                                                    </div>
                               </div>
                                             <div class="col-sm-3">
                                    <div class="form-group">
                                                        <label><%=Resources.Resource.CandStatus_CndNm%></label>
                                                  
                                                        <asp:TextBox ID="TxtUserName" class="form-control" runat="server" MaxLength="50"></asp:TextBox>
                                                    </div>
                                                  </div> 
                              
                               <div class="col-sm-3">
                                    <div class="form-group">
                                                        <label><%=Resources.Resource.CandStatus_ExamSts%></label>  
                                                    
                                                        <asp:DropDownList ID="ddlStatus" class="form-control" runat="server"  ToolTip="Exam Status"
                                                            onmouseover="addTitleAttributes(ddlStatus);">
                                                        </asp:DropDownList>
                                                    </div>
                                                    </div>
                                 </div>
                                               
                                          <div class="row">
                                               <div class="col-sm-2">
                                    <div class="form-group">
                                                      <label><%=Resources.Resource.CandStatus_Grade%></label>
                                                    
                                                        <asp:DropDownList ID="ddlgrade" class="form-control" runat="server"  ToolTip="Grade" onmouseover="addTitleAttributes(ddlgrade);">
                                                            <asp:ListItem Text="--Select--"></asp:ListItem>
                                                            <asp:ListItem Text="A+"></asp:ListItem>
                                                            <asp:ListItem Text="A"></asp:ListItem>
                                                            <asp:ListItem Text="B"></asp:ListItem>
                                                            <asp:ListItem Text="C"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                   </div>
                               
                                              
                                                    <div class="col-sm-10">
                                                        <div class="row">
                                                            <div class="col-sm-3">
                                                                <div class="form-group">
                                                        <label><%=Resources.Resource.CandStatus_ExmAssgDte%>(From)</label>
                                                        <input id="TxtFrom" runat="server" class="form-control" onkeydown="return false"/>
                                                                    </div>                                                        
                                                      </div>
                                                <div class="col-sm-3">
                                    <div class="form-group">                                                
                                                   <label>(To)</label>
                                                        <input id="TxtTo" runat="server" class="form-control" onkeydown="return false"/>
                                                    </div>
                                                    </div>
                                                <div class="col-sm-3">
                                    <div class="form-group">
                                                        <label><%=Resources.Resource.CandStatus_ExamAppearedDate%>(From)</label>
                                                        <input id="txtAppFromDate" runat="server" class="form-control" onkeydown="return false"/>
                                                    </div>
                                                    </div>
                                                <div class="col-sm-3">
                                    <div class="form-group  ">
                                                       <label> (To) </label>
                                                        <input id="TxtAppToDate" runat="server" class="form-control" onkeydown="return false"/>
                                                    </div>
                                                    </div>
                                                   </div>
                                                        </div>
                                                        </div>
                                                         
                                            
                                          
                          
                                          
                    <div class="row">
                        <div class="col-sm-12" style="text-align:center">
                            <asp:Button ID="BtnSearch" runat="server"  class="btn btn-primary" Text="<%$Resources: Resource, Common_BtnSrch %>" AlternateText="Search the candidate">
                            </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button runat="server" Text="<%$Resources: Resource, Common_btnClr %>" ID="btnclear" 
                                AlternateText="Clear" ToolTip="Clear" CausesValidation="false" class="btn btn-primary" ></asp:Button>&nbsp;
                            <asp:ImageButton ID="btnBack" runat="server" ImageUrl="images/BtnBack.jpg" AlternateText="Back"
                                ToolTip="Back" Visible="false" CausesValidation="False"></asp:ImageButton>
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
                         
                            <%--s--%>
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                    </asp:ScriptManager>
                                    <asp:Panel ID="Panel1" Style="display: none" runat="server">
                                        <div class="modalBackground">
                                            <%-- <div class="modalPopup " id="PopupHeader">
                                                            </div>--%>
                                            <div class="modalPopup">
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-sm-4">
                                                            <label><b>Old Date</b></label>
                                                            </div>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lbldate" runat="server" Text="Label" Font-Size="Larger"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4">
                                                            <label><b>New Date</b></label>
                                                            </div>
                                                        <div class="col-sm-8">
                                                            <input id="txtnewdate" runat="server" class="form-control" onkeydown="return false"/>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12" style="margin-top:5px">
                                                            <asp:Button runat="server" ID="btnOkay" type="button" Text="Save" value="Done" class="btn btn-outline-info" />
                                                            <asp:Button runat="server" ID="btnPopUpCancel" type="button" Text="Cancel" value="Done" class="btn btn-outline-info"/>
                                                            <input id="btnCancel" runat="server" type="button" style="display: none" value="Cancel" class="btn btn-outline-info"/>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <%--<cc1:CalendarExtender ID="CalendarExtender1" PopupPosition="Right" TargetControlID="txtnewdate"
                                        Format="dd/MM/yyyy" PopupButtonID="btnCalander" runat="server">
                                    </cc1:CalendarExtender>--%>
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender1" RepositionMode="RepositionOnWindowResize"
                                        runat="server" CancelControlID="btnCancel" TargetControlID="lbldate" PopupControlID="Panel1"
                                        PopupDragHandleControlID="PopupHeader" X="550" Y="300" BackgroundCssClass="modalBackground1">
                                    </cc1:ModalPopupExtender>
                                    <asp:Panel ID="Panel2" style="display:none" runat="server">
                                        <div class="modalBackground">
                                            <div class="modalPopup">
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-sm-4">
                                                            <label><b>Old Date</b></label>
                                                            </div>
                                                        <div class="col-sm-8">
                                                            <asp:Label ID="lbldate2" runat="server" Text="Label" Font-Size="Larger"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-4">
                                                            <label><b>New Date</b></label>
                                                            </div>
                                                        <div class="col-sm-8">
                                                            <input id="txtReassign" runat="server" class="form-control" onkeydown="return false"/>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-sm-12" style="margin-top:5px">
                                                            <asp:Button runat="server" ID="btnOkay2" type="button" Text="Save" value="Done" class="btn btn-outline-info"/>
                                                            <asp:Button runat="server" ID="btnPopUpCancel2" type="button" Text="Cancel" value="Done" class="btn btn-outline-info"/>
                                                            <input id="btnCancel2" runat="server" type="button" style="display: none" value="Cancel" class="btn btn-outline-info"/>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <%--<cc1:CalendarExtender ID="CalendarExtender6" PopupPosition="Right" TargetControlID="txtReassign"
                                        Format="dd/MM/yyyy" PopupButtonID="imgBtnReassign" runat="server">
                                    </cc1:CalendarExtender>--%>
                                    <cc1:ModalPopupExtender ID="ModalPopupExtender2" RepositionMode="RepositionOnWindowResize"
                                        runat="server" CancelControlID="btnCancel2" PopupControlID="Panel2" TargetControlID="lbldate2"
                                        PopupDragHandleControlID="PopupHeader" X="550" Y="300" BackgroundCssClass="modalBackground1">
                                    </cc1:ModalPopupExtender>




                                    <div id="gridDiv" runat="server" visible="false">
                                         <div class="col-sm-12">
                               <section class="content">
                                   <div class="container-fluid">
                                        <div class="row">
                                          <div class="col-md-12">
                                             <div class="card card-info">
                                                 <div class="card-header">
                                                      <h1 class="card-title"><%=Resources.Resource.Common_SrchResult%></h1>
                                                 </div>
                                             <div class="card-body">
                                                 <div class="container upOnGrid">
                                                        <div class="row">
                                                             <strong><%=Resources.Resource.Common_Show%></strong>&nbsp;
                                                             <asp:DropDownList ID="PageSizeList" Autopostback="true" Width="44px" OnSelectedIndexChanged="Selection_Change" runat="server" class="select2">
                                                                 <asp:ListItem>10</asp:ListItem>
                                                                 <asp:ListItem>15</asp:ListItem>
                                                                 <asp:ListItem>25</asp:ListItem>
                                                                 <asp:ListItem>50</asp:ListItem>
                                                             </asp:DropDownList>&nbsp;<strong><%=Resources.Resource.Common_record%></strong>
                                                        </div>
                                                     <div style="display:flex; flex-wrap: nowrap">
                                                        <nav aria-label="Page navigation example">
                                                         <ul class="pagination">
                                                             <li class="page-item"><span class="page-link">
                                                                 <asp:LinkButton ID="imgfirst" runat="server" CommandArgument="0" OnClick="PagerButtonClick"><%=Resources.Resource.Common_First%></asp:LinkButton></span></li>
                                                             <li class="page-item"><span class="page-link">
                                                                 <asp:LinkButton ID="imgprev" runat="server" CommandArgument="prev" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Prev%></asp:LinkButton></span></li>                                             
                                                             <li class="page-item"><span class="page-link">
                                                                 <asp:LinkButton ID="imgnext" runat="server" CommandArgument="next" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Next%></asp:LinkButton></span></li>
                                                             <li class="page-item"><span class="page-link">
                                                                 <asp:LinkButton ID="imglast" runat="server" CommandArgument="last" OnClick="PagerButtonClick"><%=Resources.Resource.Common_Last%></asp:LinkButton></span></li>
                                                         </ul>
                                                        </nav>
                                                     </div>
                                                     <div>
                                                       <strong><%=Resources.Resource.AdminList_TotRecord%><asp:Label runat="server" ID="LblRecCnt"></asp:Label><asp:Label runat="server"  ID="lblRecordCount"></asp:Label></strong>
                                                     </div>
                                                </div>

                                             <div class="row">
                                                 <div class="col-sm-12">
                                                    <div class="table-responsive SpecifyHeight">
                                              <asp:DataGrid ID="DGReport" runat="server" OnPageIndexChanged="DgReport_PageIndexChanged"
                                                AutoGenerateColumns="False" AllowPaging="True" Visible="false" CssClass="table table-bordered table-striped table-hover" DataKeyField="userid">
                                              
                                                     <SelectedItemStyle  />
                                                          <PagerStyle HorizontalAlign="Left" Visible="false"  BackColor="Transparent"
                                                              Mode="NumericPages" ></PagerStyle>
                                                     <Columns>
                                                               <asp:TemplateColumn Visible="False">
                                                                  <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                                      <ItemTemplate>
                                                                       <asp:CheckBox ID="ChkSel" runat="server" Checked='false' />
                                                                  </ItemTemplate>
                                                                   </asp:TemplateColumn>
                                                                       <asp:BoundColumn Visible="False" DataField="userid" HeaderText="userid">
                                                                    <ItemStyle  />
                                                                            </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="Sr.No." HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                                                   <ItemStyle HorizontalAlign="Center"/>
                                                                    </asp:BoundColumn>
                                                                    <asp:BoundColumn DataField="UserName" HeaderText="<%$Resources:Resource, CandStatus_CndNm %>">
                                                                     <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                      </asp:BoundColumn>
                                                                      <asp:BoundColumn DataField="Center_Name" HeaderText="<%$Resources:Resource, CandStatus_Class %>">
                                                                      <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                    </asp:BoundColumn>
                                                                     <asp:BoundColumn DataField="course_name" HeaderText="<%$Resources:Resource, CandStatus_Course %>">
                                                                  <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                     </asp:BoundColumn>
                                                                         <asp:BoundColumn DataField="LoginName" HeaderText="<%$Resources:Resource, CandStatus_ID %>" >
                                                                            <HeaderStyle  />
                                                                        <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                       </asp:BoundColumn>
                                                                       <asp:BoundColumn DataField="Password" HeaderText="Password">
                                                                          <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                                       </asp:BoundColumn>
                                                                           <asp:BoundColumn DataField="WrittenTestDate" HeaderText="<%$Resources:Resource, CandStatus_AssgnDate %>" DataFormatString="{0:dd/MM/yyyy}">
                                                                         <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                                     </asp:BoundColumn>
                                                                     
                                                    <asp:BoundColumn DataField="AppearanceDate" HeaderText="<%$Resources:Resource, CandStatus_Appeared %>"
                                                        DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="total_marks" HeaderText="<%$Resources:Resource, CandStatus_MaxMrks %>">
                                                        <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <%--<asp:BoundColumn DataField="Attempted" HeaderText="Attempted">
                                                                    <ItemStyle HorizontalAlign="Center" CssClass="gItemStyleNum"></ItemStyle>
                                                                </asp:BoundColumn>--%>
                                                    <asp:BoundColumn DataField="obtained_marks" HeaderText="<%$Resources:Resource, CandStatus_MrkObt %>">
                                                        <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                    </asp:BoundColumn>
                                                        
                                                    <asp:BoundColumn DataField="Status" HeaderText="<%$Resources:Resource, CandStatus_Sts %>">
                                                        <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="Grad" HeaderText="<%$Resources:Resource, CandStatus_Grade %>">
                                                        <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:ButtonColumn HeaderText="<%$Resources:Resource, CandStat_ProMark %>" CommandName="MarkSheet" >
                                                        <ItemStyle CssClass="gButtonStyle" />
                                                    </asp:ButtonColumn>

                                                    <asp:BoundColumn DataField="course_id" HeaderText="couse_id" Visible="False">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:BoundColumn DataField="appearedflag" HeaderText="appearedflag" Visible="False">
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    </asp:BoundColumn>
                                                    <asp:ButtonColumn HeaderText="<%$Resources:Resource, CandStatus_Remder %>" CommandName="Remind"  Text="Remind">
                                                        <ItemStyle  />
                                                    </asp:ButtonColumn>
                                                <%--<asp:ButtonColumn HeaderText="Re-assign"  CommandName="Reassign" ItemStyle-Width="0px" 
                                                        Text="Re-Assign" OnClientClick="return confirm('Are you sure you want to Reassign this Exam?');" >
                                                        <ItemStyle CssClass="gButtonStyle" />
                                                    </asp:ButtonColumn>--%>
                                                     <asp:TemplateColumn  HeaderText="<%$Resources:Resource, CandStat_Reassign %>" >
                                                         
                                                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                        <asp:LinkButton runat="server" Text="Re-Assign" CommandName="Reassign" 
                                                         OnClientClick="return confirm('The older history will be lost. Are you sure you want to Re-Assign Exam ?');" > 
                                                        </asp:LinkButton>
                                                        </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                          <asp:ButtonColumn HeaderText="<%$Resources:Resource, admin_Edit %>" CommandName="EditDate" Text="<%$Resources:Resource, admin_Edit %>" ItemStyle-Width="0px" >
                                                                      <ItemStyle  />
                                                                    </asp:ButtonColumn>
                                                                        <asp:ButtonColumn HeaderText="<%$Resources:Resource, CandStatus_DisableExam %>"  CommandName="Disable" Text="Disable">
                                                        <ItemStyle  />
                                                    </asp:ButtonColumn>
                                                         
                                                  <%-- *********Nisha 2017/09/25*******--%>
                                                    <asp:ButtonColumn HeaderText="<%$Resources:Resource, CandStatus_ExamDts %>" CommandName="Excellsheet" Text="Excellsheet">
                                                        <ItemStyle  />
                                                    </asp:ButtonColumn>
                                                    <%-- *********Nisha 2017/09/25*******--%>
                                                  
                                                    <%--   'Added By: Pragnesha Kulkarni 
                                                              'Date: 20/07/18
                                                              'Description: For displaying select all column.--%>
                                          
                                                     <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_SltDeslt%>">
                                                     <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="SelectAll" onclick="HeaderCheckBoxClick(this);" />
                                                   <%--  AutoPostBack="true"
                                              OnCheckedChanged="chkSelectAll_CheckedChanged" />--%>
                                                       </HeaderTemplate>                                                       
                                                   <HeaderStyle HorizontalAlign="Center"  ></HeaderStyle>
                                                    <ItemTemplate>
                                                    <asp:CheckBox ID="chkRemove" runat="server" Text="" onclick="ChildCheckBoxClick(this);" />
                                                    </ItemTemplate>
                                                     <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateColumn>
                                          
                                                      <%-- *********Ended by Pragnesha 2018/07/20*******--%>

                                                </Columns>
                                                
                                                <HeaderStyle Font-Bold="True" BackColor="#189599" ForeColor="#ffffff"/>
                                            </asp:DataGrid>
                                            </div>
                                              </div>
                                              </div>

                                               
                                             
                                <%--<div class="row">
                                    &nbsp;&nbsp;<span style="color: #526B94"><%=Resources.Resource.Common_GoToPage%></span>
                                    <asp:DropDownList ID="ddlPages" AutoPostBack="true" runat="server" width="60px"  class="form-control">
                                    </asp:DropDownList>
                              </div>--%>
                        
                                                 </div>
                                                 </div>
                                              </div>
                                            </div>
                                       </div>
                                   </section>
                                             </div>
                                        </div>

  </ContentTemplate>
                            </asp:UpdatePanel>

                                                        <table id="tblPagebuttons" runat="server"  visible="false"> <tr></tr>
                                                        </table>  
                                        
                                             <%--   'Added By: Pragnesha Kulkarni 
                                                       'Date: 20/07/18
                                                       'Description:For deleting student records.--%>
                                        <asp:ImageButton ID="imgBtnDelete" runat="server" ImageUrl="images/btnDelete.jpg"  Visible="false" />
                                         
                                        <br />


         </div>
    

                                   
                                
   
</asp:Content>
