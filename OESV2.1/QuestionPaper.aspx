<%@ Page Language="vb" AutoEventWireup="false" Codebehind="QuestionPaper.aspx.vb"
    Inherits="Unirecruite.QuestionPaper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="google" content="notranslate">
<style type="text/css">
        body
        {
            font-family: arial;
            font-size: 10pt;
        }
        .draggable
        {
            width: 350px;
            overflow: visible;
            background-color: #ccc;
            border: 2px solid #666;
            margin-bottom: 1em;
            padding: 4px;
            cursor: default;
        }
        .active
        {
            border: 2px solid #000;
            background-color: Red;
        }
        #droppable
        {
            font-size: 14pt;
            width: 400px;
            background-color: #7F38EC;
        }
        #droppableHolder
        {
            margin-top: 5em;
            background-color: #7F38EC;
        }
     
          .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
        
    </style>
    <%-- Added by Pragnesha for Bug ID 970(Web browser Icon) Date: 14-05-19--%>
    <link rel="shortcut icon" type="image/png" href="images/OES_ICON.png" />
    <%-- ---------Ended by Pragnesha-------------------------------------------%>
    <title>Online Examinations solution------Question Paper</title>
    <link  rel="Stylesheet" type="text/css" href="images/css.css" />     
    <link href="JQuerys/StyleSheet1.css" type="text/css" rel="stylesheet"/>
   

    <script src="JQuerys/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="JQuerys/jquery-1.4.2.min.js"></script>

     <script type="text/javascript" src="JQuerys/jquery-ui.min-1.8.12.js"></script>

    <script src="JQuerys/progress.js" type="text/javascript"></script>
    <%--<script type="text/javascript" src="JQuerys/JScript2.js"></script>--%>
</head>

<%--for image zoom--%>  
<%--end image zoom--%>


<script type="text/javascript" language="javascript" id="hi">
var fvals="";
var value="";
	$(function () {              
            $(".draggable").draggable({
               // revert: true,
                helper: 'clone',
                start: function (event, ui) {
                    //$(this).fadeTo('fast', 0.5);
                },
                stop: function (event, ui) {
                   // $(this).fadeTo(0, 1);
                }
            });
           
             var total='<%=Session("rf")%>';
             var spl=total.split(',');
              var jso="";
              var incr=0;
        
       
        
         var split="";
         var hash= new Array() ;
               
             for (its=0;its<spl.length;its++)
             {
                jso=jso+"#droppable"+spl[its]+",";
                
             }
             jso=jso.substring(0, jso.length-1);           
         
                 
            $(jso).droppable({
             
                hoverClass: 'active',
                drop: function (event, ui) {
               
                    this.value = ui.draggable.text();   
                      var  id = $(this).attr("id");
                      value= $(ui.draggable).attr("id")+",";
                      split=jso.split(",");
                      id="#"+id;
			  for(incr=0;incr<split.length;incr++)
			  {
			  
			  if(id==split[incr]){
			  var number=split[incr].substring(split[incr].length -1,split[incr].length);
			  hash[number-1]=value ;
			  break;
			  }
                   }   
                    var FinalValue="";
			for(incr=0;incr<hash.length;incr++)
			{FinalValue+=hash[incr];
			}
			
			 fvals=FinalValue;
                   //  fvals=fvals + $(ui.draggable).attr("id")+",";
                }
            });
           
         
          
          
        });
		    function getCheckedRadioButton() {
			 
		        var hiddenAllRdb = document.getElementById("rdbAllId").value;		        
		        var rdbIds = hiddenAllRdb.split(",");
		        var size = rdbIds.length - 1;

		        var hiddenCheckedRdb = document.getElementById("rdbCheckedId");
		        var hiddenNotAttempt = document.getElementById("queNotAttempt");
		        var strCheckedRdb = "";
		        var strNotAttemptRdb = "";
		        var iLoop = 1;
		        var iCnt = 0;
		        var fvalt;
		       		       
		        if(fvals!="")
		        {
		        fvals =fvals.split(',') ;//Chang
		        }
		      
		       
		        for(iLoop = 1; iLoop <= size; iLoop++) {
		        
		             var control = document.getElementById(rdbIds[iLoop - 1]);
		             var ddCheckQuestionId=document.getElementById("ddCheckQuestionId");
		             //************Chang**********
                     if (control!=null && control.tagName=="DIV")  
                     {
                         
                         fvalt = document.getElementById(fvals[iLoop - 1]);
                       if (fvalt != null )               
                       {
                          var fva;
                          fva=fvals[iLoop-1].split('_');  
                          fva =fva[fva.length-1] ;
                          strCheckedRdb += rdbIds[iLoop - 1]  + "_" + fva +","; 
                         }
                         else
                         {
                        
                           ddCheckQuestionId.value=hiddenAllRdb;
                         }
                           //************Chang**********
                     }else if(control != null && control.type == "text")
                     { 
                         if( control.value != "")
                         {
                            if ( control.value != " ")
                            {
                                 if ( control.value != "  ")
                                 {
                                    strCheckedRdb += rdbIds[iLoop - 1] + "_" + control.value +","; 
                                    ddCheckQuestionId.value="";
                                 }
                            }
                         
                         }
                        
                     }else 
                     {
                        var rdbValue = document.getElementById(rdbIds[iLoop - 1]).checked;
		                if(rdbValue == true) {
		                    strCheckedRdb += rdbIds[iLoop - 1] + ",";
		                     ddCheckQuestionId.value="";
    		                
		                } else {		                
		                    iCnt++;
		                }		            
		                if(iLoop % 4 == 0 && iCnt == 4) {		                
		                    strNotAttemptRdb += rdbIds[iLoop - 1] + ",";
		                    iCnt = 0;
		                     ddCheckQuestionId.value="";
		                }
                     }
		       
		            
		        }
		        hiddenCheckedRdb.value = strCheckedRdb;
		        hiddenNotAttempt.value = strNotAttemptRdb;

		        debugger;
		        // putting current timer value in the hidden
		        document.getElementById("currTime").value = document.getElementById("TmrExam").value;

		      
		    }
		    
//		    function getCheckedValues()
//		    {
//		        var total=""
//              //  for(var i=0; i < document.form1.chkbx.; i++){
//             //   if(document.form1.scripts[i].checked)
//             //   total +=document.form1.scripts[i].value + "\n"
//            }


		    debugger;
		    function confirmFinish() {
		        var objTime = document.getElementById("TmrExam");
		        if (objTime.value=="00:02")
                return yes;
                else
                {
		       //var total = '<%=Session("TotalQueCnt") %>';
                    debugger;
		        var total='<%=Session("val")%>';
		        var current = '<%=Session("EndQues") %>';
		        var totalAnswerd='<%=Session("val1") %>';
		        var message = 'You have appeared for ' + totalAnswerd + '/' + total +' Questions.\nAre you sure you want to finish the test.';
		        var yes = confirm(message);
		        if(yes){
		            getCheckedRadioButton();
		        }		        
		        return yes;
                }
		    }
            
    
// Validate fill in the blanks

function formatInt(input){

    var num = input.value.replace(/\,/g,'');

    if(!isNaN(num)){

    if(num.indexOf('.') > -1) {

    alert("You must not enter any decimals in the blanks!");

   // input.value = input.value.substring(0,input.value.length-1);
   input.value="";
   input.focus();


    }

    } else {

    alert('You must enter only option numbers in the blanks!');

//    input.value = input.value.substring(0,input.value.length-1);
input.value="";
input.focus();

    }

}

//Loading image Pragnesha

    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }
    $('form').live("submit", function () {
        ShowProgress();
    });

</script>

<%--<body>
    <form id="form1" runat="server">
        <div align="left">
            <table id="MainTable" style="position: fixed; border-right: black 2px inset; table-layout:auto;
                border-top: black 2px inset; border-left: black 2px inset; border-bottom: black 2px inset;
                border-collapse: collapse; width: 1000px; height: 562px;">
                <tr style="background: #dcdcdc;">
                    <td style="width: 1000px; height: 110px; border-bottom: black 2px inset;" valign="baseline"
                        align="left" colspan="2">
                        <table id="TblHeader" style="width: 1000px; background-color: #dcdcdc; table-layout: fixed;
                            position: static; border-collapse: collapse;">
                            <tr style="background-color: #dcdcdc;" align="center">
                                <td style="padding-left: 10px; padding-right: 10px; padding-bottom: 10px; width: 112px;
                                    position: fixed;" valign="baseline">
                                    <asp:Image runat="server" ID="imgStudent" BorderColor="Black" BorderStyle="Inset"
                                        BorderWidth="1px" Height="85px" Width="85px" />
                                    <br />
                                    <asp:Label ID="lblStudentId" runat="server" Font-Bold="True" Height="20px" Width="30px"
                                        Font-Size="12pt" BorderColor="Black" BorderStyle="Inset" BorderWidth="1px"></asp:Label></td>
                                <td style="padding-top: 10px; padding-right: 10px; width: 980px;" align="center"
                                    valign="top">
                                    <asp:Image runat="server" ID="imgHeader" ImageUrl="~/images/HeaderName.JPG" Width="290px"
                                        ImageAlign="AbsMiddle" /><br />
                                    <br />
                                    <asp:Label runat="server" ID="lblExamNameandCode" Font-Size="12pt" Width="350px"
                                        BorderColor="Black" BorderStyle="Inset" BorderWidth="1px" Font-Bold="True"></asp:Label>
                                </td>
                                <td style="padding-top: 10px; padding-right: 10px; width: 180px;" align="center"
                                    valign="middle">
                                      <form id="FormQA" name="count">
                                        &nbsp;</form>
                                    <input class="timer" id="TmrExam" style="font-size: 16px; font-weight: bold; width: 55px;
                                        background-color: #dcdcdc;" type="text" value='<%=Session("TestXX")%>' name="count2" /><%--/form><br />
                                    <br />
                                    <asp:Label runat="server" ID="lblTotalMarks" Font-Bold="True" Font-Size="16px" Width="130px"></asp:Label><br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" align="left" style="width: 1000px; height: 230px; border-collapse: collapse;
                        border-left: black 1px inset; border-right: black 2px inset; border-bottom: black 2px inset;">
                        <table id="tblQuestion" runat="server" style="width: 1000px; border-collapse: collapse; 
                            height: 380px;">
                        </table>
                    </td>
                    <td valign="baseline" style="margin-left: 1000px; margin-right: 0px; border-bottom: black 2px inset;">
                        <div style="overflow: scroll; height: 380px; width: 300px">
                            <table id="TblAllquestions" runat="server" style="width: 280px; height: 820px;
                                border-collapse: collapse; background-color: #dcdcdc;position: static;">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr style="background: #dcdcdc;">
                    <td style="width: 1000px; height: 56px; border-collapse: collapse; position: fixed;"
                        colspan="2" align="left">
                        <table id="TblAllNavigation" style="width: 1300px; background-color: #dcdcdc; position:static;
                            table-layout: fixed; border-collapse: collapse;">
                            <tr>
                                <td align="center" style="height: 50px">
                                    <asp:ImageButton runat="server" ID="imgBtnFirst" ImageUrl="~/images/ImgBtnFirst.JPG"
                                        Enabled="False" Height="27px" />
                                </td>
                                <td align="center" style="height: 50px">
                                    <asp:ImageButton runat="server" ID="imgBtnPrevious" Visible="True" ImageUrl="~/images/imgBtnPrevious.JPG"
                                        Enabled="False" Height="32px" />
                                </td>
                                <td align="center" style="height: 50px">
                                    <asp:ImageButton runat="server" ID="imgBtnNext" ImageUrl="~/images/imgBtnNext.JPG"
                                        Height="34px" />
                                </td>
                                <td align="center" style="height: 50px">
                                    <asp:ImageButton runat="server" ID="imgBtnLast" ImageUrl="~/images/imgBtnLast.JPG"
                                        Height="29px" />
                                </td>
                                <td align="center" style="height: 50px">
                                    <asp:ImageButton runat="server" ID="imgBtnUnAnswer" ImageUrl="~/images/imgBtnUnAnswer.JPG"
                                        Height="34px" /></td>
                                <td align="center" style="height: 50px">
                                    <asp:ImageButton runat="server" ID="imgBtnEndExam" ImageUrl="~/images/imgBtnEndExam.JPG"
                                        Height="34px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="rdbAllId" runat="server" />
            <input type="hidden" id="rdbCheckedId" runat="server" />
            <input type="hidden" id="queNotAttempt" runat="server" />
            <input type="hidden" id="currTime" runat="server" />
            <asp:Button ID="hiddButton" runat="server" UseSubmitBehavior="false" />
        </div>
    </form>

    <script language="JavaScript" type="text/javascript">
        
			var dsec = 60;
			var objTime = document.getElementById("TmrExam"); 
			
			var dmin = 0;
			if(objTime.value.indexOf(':') != -1){
			    var timeArr = objTime.value.split(":");
			    dmin = timeArr[0];
			    dsec = timeArr[1];
			}else {		
			    dmin = objTime.value - 1;
			}
			
			var milsec=10;
			function countdown(){
				//document.all.moveme.style.top  = document.body.scrollTop;
				
				//document.forms.count.count2.value= dmin + ":" + dsec; 
				objTime.value = dmin + ":" + dsec;
				
				if(dmin==0 && dsec==0)
				{				    
					ForcePostBack();
				}
				
				setTimeout("calcTime()",1000);
			}
			
			function calcTime()
			{			
				countdown();
				
				if(dsec==0)
				{
					dsec=60;
					dmin=dmin-1;
				}
				
				dsec = dsec-1;
			}
			
			countdown();
			
			function ForcePostBack()
		    {
				alert("Time Over");
				getCheckedRadioButton();
				
				if (document.getElementById("hiddButton").dispatchEvent) {				    
				    var e = document.createEvent("MouseEvents");

                    e.initEvent("click", clickBtnEv(), true);
                    document.getElementById("hiddButton").dispatchEvent(e);
				} else {
				    document.getElementById("hiddButton").click();
				}
				//frm_quespaper.submit();
			}
			
			function clickBtnEv() {
                //do click process
                document.getElementById("hiddButton").click();
            } 

    </script>

</body>--%>
<body>
    
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="Up1" runat="server">
    <ContentTemplate>
    
        <table id="MainTable" runat="server" style="table-layout: fixed; width: 100%;">
            <tr>
                <td colspan="3" style="width: 100%; background-image: url(images/Bh_Head111.jpg); background-repeat: no-repeat;">
                    <table width="100%">
                        <tr>
                            <td style="width: 15%; height: 100%" align="center">
                                <asp:Image runat="server" ID="imgStudent" BorderColor="Black" BorderStyle="Inset"
                                    BorderWidth="1px" Width="125px" Height="125px" /><br />
                                <asp:Label runat="server" ID="lblStudentId" ForeColor="#F9FFF9"></asp:Label><br />
                                <br />
                                <asp:HiddenField ID="HiddenField1" runat="server" /> <%--Added by Pragnesha for fetch category id of question--%>

                                <br />
                            </td>
                            <td style="width: 70%">
                                <table width="100%">
                                    <tr>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%; height: 21px;">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                        <td style="width: 10%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="10" align="center" valign="middle">
                                            <asp:Label  runat="server" ID="lblExamNameandCode" Font-Size="12pt" Width="199%"
                                                ForeColor="#F9FFF9" Font-Bold="True" Height="69%"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 15%">
                                <table width="100%" align="center">
                                    <tr>
                                    
                                        <td colspan="2" align="center">
                                            <input class="timer" id="TmrExam" style="font-size: 16px; color: White; font-weight: bold;
                                                width: 29%; background-repeat: no-repeat; background-image: url(images/simple1.JPG);
                                                table-layout: fixed; border-top-style: none; border-right-style: none; border-left-style: none;
                                                border-collapse: collapse; border-bottom-style: none;" value='<%=Session("TestXX")%>'
                                                name="count2" readonly="readOnly" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            &nbsp; &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label runat="server" ID="lblTotalMarks" Font-Bold="True" Font-Size="16px" Width="10%"
                                                ForeColor="White"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>        
            <tr style="background-color: #FFFEE4; position: static; width: 100%">
                <td align="left" style="background-color: #FFFDE4; border-collapse: collapse; position: static;
                    height: 46%; border-left: black 1px inset; background-repeat: no-repeat; border-right: black 2px inset;
                    background-position: center 50%; border-bottom: black 2px inset; "
                    valign="top" colspan="2">
                    <div class=" " style=" overflow:scroll ; height:400px"><%--313px">commented and changed by Pragnesha Kulkarni on 2018/06/12 [BugID: 775, Desc: Increase height of tblQuestions]--%>
                  <%--   Added by Pragnesha Kulkarni on 2018/06/14 [Desc:Display Audio Control at starting of Question paper]--%>
                     <table id="tblAudio"  class="thumb" runat="server" style="border-collapse: collapse; height: 5%;
                        width: 100%">
                    </table>
                 <%--   Ended by Pragnesha Kulkarni on 2018/06/14--%>
                    <table id="tblQuestion"  class="thumb" runat="server" style="border-collapse: collapse; height: 46%;
                        width: 100%">
                    </table>
                    </div>
                </td>
                <td style="border-bottom: black 2px inset; background-color: #FFFDE4; height: 46%;"
                    valign="baseline">
                    <div style="overflow: scroll; height:400px"><%--313px">commented and changed by Pragnesha Kulkarni on 2018/06/12 [BugID: 775, Desc: Increase height of tblAllQuestions]--%>
                        <table id="TblAllquestions" runat="server" style="border-collapse: collapse; background-color: #FFFDE4;
                            position: static;width:92%;">
                        </table>
                    </div>
                </td>
                <%--<td valign="baseline" style="margin-left: 1000px; margin-right: 0px; border-bottom: black 2px inset;">
                        <div style="overflow: scroll; height: 380px; width: 300px">
                            <table id="TblAllquestions" runat="server" style="width: 280px; height: 820px;
                                border-collapse: collapse; background-color: #dcdcdc;position: static;">
                            </table>
                        </div>
                    </td>--%>
            </tr>
            <tr style="background-color: #526B94">
                <td style="width: 100%; height: 8%; border-top: black 2px inset; border-collapse: collapse;
                    position: static;" colspan="3" valign="baseline" align="left">
                    <table id="TblAllNavigation" runat="server" style="width: 97.6%; position: static;
                        table-layout: fixed; border-collapse: collapse;">
                        <tr>
                            <td align="center" style="height: 10%">
                                
                                <asp:ImageButton runat="server" ID="imgBtnFirst" ImageUrl="images/btnfirst.jpg"
                                    Enabled="False" Height="30px" />
                            </td>
                            <td align="center" style="height: 10%">
                                <asp:ImageButton runat="server" ID="imgBtnPrevious" Visible="True" ImageUrl="images/btnprev.jpg"
                                    Enabled="False" Height="30px" />
                            </td>
                            <td align="center" style="height: 10%">
                                <asp:ImageButton runat="server" ID="imgBtnNext" ImageUrl="images/btnnext.jpg" Height="30px" />
                            </td>
                            <td align="center" style="height: 10%">
                                <asp:ImageButton runat="server" ID="imgBtnLast" ImageUrl="images/btnlast.jpg" Height="30px" />
                            </td>
                            <td align="center" style="height: 10%">
                                <asp:ImageButton runat="server" ID="imgBtnUnAnswer" ImageUrl="images/btnunans.jpg"
                                    Height="30px" /></td>
                            <td align="center" style="height: 10%">
                                <asp:ImageButton runat="server" ID="imgBtnEndExam" ImageUrl="images/EndExambutton.JPG"
                                    Height="30px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
        <input type="hidden" id="rdbAllId" runat="server" />
        <input type="hidden" id="rdbCheckedId" runat="server" />
        <input type="hidden" id="queNotAttempt" runat="server" />
        <input type="hidden" id="currTime" runat="server" />
        <input type="hidden" id="ddCheckQuestionId" runat="server" />
        <asp:Button ID="hiddButton" runat="server" UseSubmitBehavior="false" />
        
       <%--comented by saraswati patel--%>
        <script language="JavaScript" type="text/javascript">
 
			var  dsec ="60";
			var objTime = document.getElementById("TmrExam"); 
			
			var dmin = "0";
			if(objTime.value.indexOf(':') != -1){
			    var timeArr = objTime.value.split(":");
			    dmin = timeArr[0];
			    dsec = timeArr[1];
	
			}else {		
			    dmin = objTime.value - 1;
			}
			//--------------------------------------------
			//Code added by Pragnesha 
			//Purpose: play Audio Question  using Audio control 
			//---------------------------------------------
			var value1 = document.getElementById('<%=HiddenField1.ClientID%>').value

            if (value1 == 2) {
               var tblrow = tblAudio.insertRow(0);
                      //var tblrow = tblQuestion.insertRow(0);
		            // Insert new cells (<td> elements) at the 1st and 2nd position of the "new" <tr> element:
                var cell1 = tblrow.insertCell(0);
		            // Add some control to the new cells:
                cell1.innerHTML = "<audio id='sound1' controls src='<%= HiddenField2.Value%>' ></audio>";
                                                       
                       }
			//----------------Ended on 26-10-2017-----------

                       debugger;
			var milsec=10;
			function countdown(){
				//document.all.moveme.style.top  = document.body.scrollTop;
				//debugger;
				//document.forms.count.count2.value= dmin + ":" + dsec; 
                // objTime.value =  dmin + ":" + dsec; Commented by rajesh Nagvanshi 
				objTime.value = ('0' + dmin).slice(-2) + ":" + ('0' + dsec).slice(-2);

			    var minus = objTime.value ;
			    PageMethods.GetContactName(objTime.value);
			     
				if(dmin==0 && dsec==02)
				{
				    clickBtnEv();
				//objTime.value ="00:00";
				//On_TimeOver();				   
					//ForcePostBack();
				}else if  (minus.indexOf("-",0) !=-1) {
			    On_TimeOver();
				}

				setTimeout("calcTime()",1000);
			}
			
			function calcTime()
			{			
				countdown();
				
				if(dsec==0)
				{
					dsec=60;
					dmin=dmin-1;
		
					
				}
				
				dsec = dsec-1;
//				if(dsec.toString().length==1)
//				{
//				
//				}
			}
			
			countdown();
			
			function ForcePostBack()
		    {
				alert("Time Over");
				//debugger;
				getCheckedRadioButton();
				
				if (document.getElementById("hiddButton").dispatchEvent) {				    
				    var e = document.createEvent("MouseEvents");

                    e.initEvent("click", clickBtnEv(), true);
                    document.getElementById("hiddButton").dispatchEvent(e);
				} else {
				    document.getElementById("hiddButton").click();
				}
    			//window.location="StudentLogin.aspx";
    			//Commmented by rajesh 
    			window.location="QuestionPaper.aspx?Status=TimeOver";
    			//window.location="ExamResult.aspx";
    			
				//frm_quespaper.submit();
			}
			debugger;
			function clickBtnEv() {
			    //do click process
			    document.getElementById("<%=imgBtnEndExam.ClientID %>").click();
                //document.getElementById("hiddButton").click(); commented for check end exam click
            } 

        function On_TimeOver()
        {
           var hiddenAllRdb = document.getElementById("rdbAllId").value;		        
		                var rdbIds = hiddenAllRdb.split(",");
		                var size = rdbIds.length - 1;
		                var hiddenCheckedRdb = document.getElementById("rdbCheckedId");
		                var hiddenNotAttempt = document.getElementById("queNotAttempt");
		                var strCheckedRdb = "";
		                var strNotAttemptRdb = "";
		                var iLoop = 1;
		                var iCnt = 0;
        		        
		                for(iLoop = 1; iLoop <= size; iLoop++) {
        		        
		                     var control = document.getElementById(rdbIds[iLoop - 1]);

                             if(control != null && control.type == "text")
                             { 
                                 if(control.value != "")
                                 {
                                    strCheckedRdb += rdbIds[iLoop - 1] + "_" + control.value +","; 
                                 }
                                
                             }else
                             {
                                var rdbValue = document.getElementById(rdbIds[iLoop - 1]).checked;
		                        if(rdbValue == true) {
		                            strCheckedRdb += rdbIds[iLoop - 1] + ",";
            		                
		                        } else {		                
		                            iCnt++;
		                        }		            
		                        if(iLoop % 4 == 0 && iCnt == 4) {		                
		                            strNotAttemptRdb += rdbIds[iLoop - 1] + ",";
		                            iCnt = 0;
		                        }
                             }
        		       
        		            
		                }
		                hiddenCheckedRdb.value = strCheckedRdb;
		                hiddenNotAttempt.value = strNotAttemptRdb;
				
				        PageMethods.ForceTimeout(hiddenCheckedRdb.value,hiddenNotAttempt.value);//hiddenAllRdb,hiddenCheckedRdb,hiddenNotAttempt); //ForceTimeout
				        alert("Time Over");
				        //window.location="StudentLogin.aspx";
				        //window.location="ExamResult.aspx?Status=TimeOver"; 
				        //changed by rajesh
				        window.location="QuestionPaper.aspx?Status=TimeOver"; 
				        
				        
        }

//        window.onbeforeunload = function () {
//            return "Do you really want to close?";
//        };
        </script>
      <%--      ' --------------------------------------------
    'Code added by Pragnesha 
    'Purpose: play Audio Question  using Audio control 
    '-----------------------------------------------%>
        <asp:HiddenField ID="HiddenField2" runat="server" />
                                        <script>
                                                HiddData = "";
                                                var HiddData = document.getElementById('<%= HiddenField2.ClientID%>');
                                                //alert(HiddData.value);
                                                HiddData.onchange = function (e) {
                                                    var sound1 = document.getElementById('sound1');
                                                    sound1.src = URL.createObjectURL(this.files[0]);

                                                    sound1.onend = function (e) {
                                                        URL.revokeObjectURL(this.src);
                                                    }
                                                }
                                            </script>
        <%--'--------------------Ended by pragnesha-------------------------%>

<%--//load image--%>
<div class="loading" align="center">
    Loading. Please wait.<br />
    <br />
    <img src="images/loader.gif" alt="" />
</div>
    </form>
</body>
</html>
