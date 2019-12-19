<%@ Page Language="vb" ValidateRequest="false" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.question_ans"
    CodeBehind="question_ans.aspx.vb" MasterPageFile="~/MasterPage.Master" Title="OESV2------Question Answer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="JQuerys/jquery-1.7.2.min.js"></script>

<script src="JQuerys/jquery-1.8.9-ui.js" type="text/javascript"></script>
<link href="JQuerys/jquery-1.8.9-ui.css" rel="stylesheet" type="text/css" />
                                   <script language="javascript" type="text/javascript">
    
     function ConDel() {
         var answer = confirm("Do you want to delete the question?");
         if (!answer) {
             window.event.returnValue = false;
         }

     }

     function addTitleAttributesSub() {

         numOptions = document.getElementById('cmb_subject').options.length;

         for (i = 0; i < numOptions; i++)
               
             document.getElementById('cmb_subject')
                 .options[i].title = document.getElementById('cmb_subject').options[i].text;
     }

     function checktxtreadonly() {
         var txt = document.getElementById('TxtReadOnly');
         var txttext = txt.value;
         txttext = txttext + '';
         var hidden = document.getElementById('hdn');
         hidden.value = txttext.toString();            
     }


     /*function addTitleAttributesOp()
   
   {
   
      numOptions = document.getElementById('_ctl30').options.length;
   
      for (i = 0; i < numOptions; i++)
   
         document.getElementById('_ctl30').options[i].title = document.getElementById('_ct130').options[i].text;
   
   }
   
     function addTitleAttributesLev()
   
   {
   
      numOptions = document.getElementById('_ctl34').options.length;
   
      for (i = 0; i < numOptions; i++)
   
         document.getElementById('_ctl34').options[i].title = document.getElementById('_ctl34').options[i].text;
   
      
   
   }*/
        <%   ' --------------------------------------------
'Code added by Pragnesha 
'Purpose: Display choose image 
'---------------------------------------------%>
     function previewFile() { 
        var test = document.getElementsByTagName("img")[3]
        if(test != null)
        {

        document.getElementsByTagName("img")[3].removeAttribute("src");
        document.getElementsByTagName("img")[3].removeAttribute("alt");
        document.getElementsByTagName("img")[3].removeAttribute("width");
        }
 
         var preview = document.querySelector('#<%=Img.ClientID %>');
            var file = document.querySelector('#<%=fileuldque.ClientID%>').files[0];
            var reader = new FileReader();

            reader.onloadend = function () {
                preview.src = reader.result;
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }
        
        //  Preview Question Code added by Pragnesha[2019-06-04]
        $(document).ready(function () {
     
  var test = document.getElementsByTagName("img")[3]
        if(test.id != 'modal-img')
        {
        var tmpimg= document.getElementsByTagName("img")[3].src;
                    $('#modal-img').attr('src', tmpimg);
         }
        else 
           {
             
              $('#_ctl0_ContentPlaceHolder1_fileuldque').change(function (event) {
               var tmpimgpath = URL.createObjectURL(event.target.files[0]);
       getimg(tmpimgpath);
        });
        }
          });

       function getimg(tmpimg)
        {
          
         $('#modal-img').attr('src', tmpimg);
        }
  $(document).on("click", "[id*=imgbtnprev]", function () {
        
             $('#modal-text').val($('#_ctl0_ContentPlaceHolder1_txt_Question').val());
            $('#modal-opt1').val($('#_ctl0_ContentPlaceHolder1_txt_option1').val());  
              $('#modal-opt2').val($('#_ctl0_ContentPlaceHolder1_txt_option2').val());  
                $('#modal-opt3').val($('#_ctl0_ContentPlaceHolder1_txt_option3').val());  
                  $('#modal-opt4').val($('#_ctl0_ContentPlaceHolder1_txt_option4').val());  
                 
                  var tmpaudio=$('#sound').attr('src');
                    $('#modal-audio').attr('src', tmpaudio);

                 

 if ($("#modal-audio").attr('src') != "") 
 {
 $("#audiorow").show();
 }
else 
{
  $("#audiorow").hide();
 
}

 if ($("#modal-img").attr('src') != "")
 {
  $("#modal-img").show();
 }
else 
{
  $("#modal-img").hide();
}

if( $('#modal-text').val().length <= '50')
{
$("#rowQue").show();
$("#rowPara").hide();
}
else{

 $('#Textarea1').val($('#_ctl0_ContentPlaceHolder1_txt_Question').val());
$("#rowPara").show();
$("#rowQue").hide();
}

 if (!$("#modal-opt1").val())
 {
  $("#rowopt1").hide();
 }
else 
{
  $("#rowopt1").show();
}

 if (!$("#modal-opt2").val())
 {
 $("#rowopt2").hide();
 }
else 
{ $("#rowopt2").show();
  
}
  if (!$("#modal-opt3").val())
 {
  $("#rowopt3").hide();
 }
else 
{
  $("#rowopt3").show();
}
if (!$("#modal-opt4").val())
 {
  $("#rowopt4").hide();
 }
else 
{
  $("#rowopt4").show();
}

                 $("#dialog").dialog({
 width: '70%',
  
                title: "Question Preview",
                buttons: {
                    Ok: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
            return false;
        });  
     <%------------Ended by Pragnesha----------%>
    
			
                                   </script>

    <style type="text/css">
        input[type=text] {
       display: block;
    width: 95%;
    height: calc(1.5rem + 2px);
    padding: .1rem .5rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #495057;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid #ced4da;
    border-radius: .25rem;
    box-shadow: inset 0 0 0 transparent;
    transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
}
        textarea {
 display: block;
    width: 95%;
    height: calc(1.5rem + 2px);
    padding: .1rem .5rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #495057;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid #ced4da;
    border-radius: .25rem;
    box-shadow: inset 0 0 0 transparent;
    transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
    height:50px;
    resize:none;
}
        select {
            display: block;
    width: 95%;
    height: calc(1.5rem + 2px);
    padding: .1rem .5rem;
    font-size: 1rem;
    font-weight: 400;
    line-height: 1.5;
    color: #495057;
    background-color: #fff;
    background-clip: padding-box;
    border: 1px solid #ced4da;
    border-radius: .25rem;
    box-shadow: inset 0 0 0 transparent;
    transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
    
        }
        /*select#Listopt{
            width:100px;
            height:100px;
        }*/
        table{
           
            
            
        }
        label  {
            
            margin-left:20px;
        }
        
       
        
        table tr{
            width:1000px;
            align-items:flex-end;
        }
        table#_ctl0_ContentPlaceHolder1_TABLE4 td{
            padding: 5px;
            text-align: center;
            white-space : inherit;
        }

        table#_ctl0_ContentPlaceHolder1_TABLE3 td{ 
            text-align: center;
            white-space : inherit;
        }

    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS" />
    
   <%-- <form id="frm_quesans" name="frm_quesans" method="post" runat="server">--%>
    <div class="container" id="frm_quesans" runat="server">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-12">
                  <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
              </div>
            </div>
              <div class="row">
                    <div class="col-sm-12">
                            <h1><asp:Label runat="server" ID="lbl"></asp:Label></h1>
                    </div>
              </div>
          </div><!-- /.container-fluid -->
        </section>

        <!-- main content -->
        <section class="content">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.QuestionAns_QuesDts%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-12">
                                
                                <table>
        
        <tr>
            <%--Content Here--%>
            <td class="pageContentTD">
                    <table>
                        <tr>
                            <td>
                                <%-- <fieldset>
                                         <legend class="outerframe">
                                    <asp:Label runat="server" ID="lbl"></asp:Label>
                                </legend>--%>
                                <table id="TABLE1" cellspacing="0" cellpadding="0"  runat="server" visible="true">
                                    <tr>
                                        <td colspan="2" align="left" class="tdcontent_label">
                                            <asp:CheckBox runat="server" ID="chknormal" Text="<%$Resources: Resource, QuestionAns_TFSA %>"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left" class="tdcontent_label">
                                            <asp:CheckBox runat="server" ID="chkblanks" Text="<%$Resources: Resource, QuestionAns_FIBMTF %>"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                      <tr>
                                        <td colspan="2" align="left" class="tdcontent_label">
                                            <asp:CheckBox runat="server" ID="chkcho" Text="<%$Resources: Resource, QuestionAns_Listing %>"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="rowsubject"  visible="false" >
                                        <td>
                                         <label ><%=Resources.Resource.CourseMain_SectionDesc%></label>
                                         <asp:DropDownList  ID="ddlSectionDes" runat="server"  AutoPostBack="True"   onmouseover="addTitleAttributes();" class="form-control">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                         </asp:DropDownList>
                                          </td>
                                        <td class="tdcontent_label " style="width:200px">
                                         <label style="width:100px;margin-left:20px"><%=Resources.Resource.Questionans_Subnam%></label>  
                                       
                                            <asp:DropDownList style="margin-left:20px" ID="cmb_subject" AutoPostBack="True" runat="server" onmouseover="addTitleAttributesSub()"  class="form-control">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="TxtSubject" runat="server" MaxLength="50" Enabled="False" Width="200px"></asp:TextBox>
                                        </td> <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td ><label > Difficulty Level</label>
                                            <asp:DropDownList style="margin-left:40px"  Width="190px" ID="ddldl" AutoPostBack="false" runat="server" class="form-control"  > 
                                               <asp:ListItem Text="Basic"  Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Intermediate" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <%--<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
                                        <td ><label style="width:100px;margin-left:20px">Total Marks *</label> 
                                            <input type="text" id="totalmarks" class="form-control"  style="width:170px; height:26px;margin-left:20px" />
                                            
                                            </td>
                                        <%--<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>--%>
                                    
                                        </tr>
                                    
                                       <tr runat="server" class="table-responsive" id="ImgSection" >
                                           <td><br /><br /><br /><br /><br /></td>
                                        <td class="tdcontent_label" style="width:70px" ><br/><%=Resources.Resource.Questionans_coimf%>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
                                            <asp:Image ID="Img" runat="server" Height="32px" Width="250px" />
                                            <asp:FileUpload ID="fileuldque" runat="server" onchange="previewFile()" />
                                            <asp:Literal ID="quesimage" runat="server">
                                            </asp:Literal>
                                            <asp:LinkButton runat="server" ID="lnkquestion" Text="Clear" Visible="false">
                                            </asp:LinkButton>
                                        </td>
                                           <td>&nbsp;</td>
                                           <td class="tdcontent_label" style="width: 10%"><%=Resources.Resource.QuestionAns_Que%>
                                            <asp:TextBox  ID="txt_Question" runat="server" Columns="54" Rows="5"  TextMode="MultiLine" MaxLength="4000" Width="700px">
                                            </asp:TextBox>
                                        </td>


                                    </tr>
                                     
    <%--' --------------------------------------------
        'Code added by Pragnesha 
        'Purpose: Choose Audio And play using control 
        '---------------------------------------------
    --%>
                                    <tr runat="server" id="rowAudio" visible="false">
                                        
                                        <td class=""  >
                                            <%=Resources.Resource.Questionans_Coaf%>
                                      
                                            <asp:FileUpload ID="fileUpldaudio" runat="server" onClientClick=" document.getElementById('HiddenField1').value = document.getElementById('fileUpldaudio').value"/>
             <asp:RequiredFieldValidator 
 id="RequiredFieldValidator1" runat="server" 
 ErrorMessage="<%$Resources: Resource, Questionans_errmsg %>" 
 ControlToValidate="fileUpldaudio"></asp:RequiredFieldValidator>
                                            <audio id="sound" controls ="controls" style="display:none" src="<%= HiddenField1.Value%>" >
                                            </audio>
                                            <script>

                                                var fuData = document.getElementById('<%= fileUpldaudio.ClientID%>');

                                                fuData.onchange = function (e) {
                                                    document.getElementById("sound").style.display = 'block';
                                                    var sound = document.getElementById('sound');
                                                    sound.src = URL.createObjectURL(this.files[0]);

                                                    sound.onend = function (e) {
                                                        URL.revokeObjectURL(this.src);
                                                    }
                                                }
                                            </script>
                                            <asp:HiddenField ID="HiddenField1" runat="server" />
<%--'-----------Purpose:Play saved audio file ---------------------%>

                           
                                                        
                                            <script>


                                                var HiddData = document.getElementById('<%= HiddenField1.ClientID%>');
                                                //alert(HiddData.value);

                                                if (HiddData != null && HiddData != undefined && HiddData.value != null && HiddData.value != "")
                                                    document.getElementById("sound").style.display = 'block';
                                                else
                                                    document.getElementById("sound").style.display = 'none';
                                                HiddData.onchange = function (e) {
                                                    var sound1 = document.getElementById('sound');
                                                    sound1.src = URL.createObjectURL(this.files[0]);

                                                    sound1.onend = function (e) {
                                                        URL.revokeObjectURL(this.src);
                                                    }
                                                }

                                            </script>
                                           <%-- <script>
                                                function ValidateRadUpload1() {

                                                    var Upload_Image = document.getElementById('<%=fileUpldaudio.ClientID %>');
                                                    var myfile = Upload_Image.value;
                                                    var format = new Array();
                                                    if (myfile.indexOf("mp3") > 0 || myfile.indexOf("wav") > 0)
                                                     {
                                                        alert('Valid Format');
                                                    }
                                                    else {
                                                        alert('Invalid Format');
                                                    }

                                                }
                                            </script>--%>
                                        </td>
                                    </tr>
    <%------------Ended by Pragnesha ----------%>

                                    <%------------Added by Nisha ----------%> 
                               <tr>         
                        <td><asp:Literal ID="queAudio" runat="server">
                                            </asp:Literal></td>
                                    </tr>
                                    <%-------------Ended by Nisha-----------%>
                                    <tr runat="server" class="table-responsive" id="rowquestion" visible="false">
                                        
                                    </tr>
                                    
                                   
                                   
                                   <tr>
                                       
                                        <td class="content" colspan="2" align="left">
                                        </td>
                                    </tr>
                                </table>
                                <%--</fieldset>--%>
                                <%--<table id="TABLE2" class="pageContentTD" cellspacing="0" cellpadding="0" width="100%" align="left" runat="server">
                                    </table>
                                    <table id="TABLE3" class="pageContentTD" cellspacing="0" cellpadding="0" align="left" runat="server" border="1">
                                    </table>--%>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="TABLE2" class="pageContentTD" cellspacing="10" cellpadding="0" width="1000px"
                                    align="left" runat="server">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="TABLE3" class="pageContentTD" cellspacing="10" cellpadding="0" align="left" width="1000px"
                                    runat="server" border="0">
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="TABLE4" cellspacing="10" class="pageContentTD" cellpadding="0" align="left"
                                    runat="server" border="0" width="100%">
                                </table>
                            </td>
                        </tr>
                    </table>
               
                <asp:Label ID="lblValidmessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" style="width: 113px;">
                <asp:Panel runat="server" ID="pnl" Width="762px">
                    <asp:Button ID="img_update" runat="server" Text="<%$Resources: Resource, Common_Update %>" class="btn btn-primary"></asp:Button>&nbsp;
                    <asp:Button ID="img_saveexit" runat="server" ToolTip="Save the Question" Text="<%$Resources: Resource, Common_Save %>" class="btn btn-primary"
                         OnClientClick="checktxtreadonly();" ValidationGroup="validate"  >
                    </asp:Button>&nbsp;
                    <asp:Button ID="img_savecont" runat="server" Text="<%$Resources: Resource, Common_Next %>" class="btn btn-primary"></asp:Button>&nbsp;
                    <asp:Button ID="img_addmore" runat="server" Text="<%$Resources: Resource, Common_AddMo %>" class="btn btn-primary"></asp:Button>&nbsp;
                    <asp:Button ID="btnBack" runat="server" Text="<%$Resources: Resource, Common_btnBck %>" class="btn btn-primary"
                       CausesValidation="False"></asp:Button>&nbsp;
                    <asp:Button ID="btnDel" runat="server" Text="<%$Resources: Resource, Common_Delete %>" CausesValidation="False" Visible="False" class="btn btn-primary"></asp:Button>&nbsp;
                    <asp:Button ID="imgbtnclr" runat="server" Text="<%$Resources: Resource, Common_btnClr %>" class="btn btn-primary"
                         CausesValidation="False" Visible="False"></asp:Button>&nbsp;
                    <asp:Button ID="imgbtnreset" runat="server" Text="<%$Resources: Resource, Common_Reset %>" CausesValidation="False" Visible="False" class="btn btn-primary"></asp:Button>
                    &nbsp;
                    <asp:Button ID="imgbtnprev" runat="server" Text="<%$Resources: Resource, Questionans_preview %>" 
                        CausesValidation="False"  class="btn btn-primary"
                        ToolTip="Preview the Question." BorderStyle="None" 
                        />
                </asp:Panel>
            </td>
        </tr>
         
    </table>
                                    
                                    </div>
                                <%--<div class="col-sm-6">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.AdminList_AdminNm %></label>
                                       <asp:TextBox runat="server" ID="txtfn" MaxLength="50" placeholder="<%$Resources:Resource, AdminList_AdminNm %>" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-6">
                                    <div class="form-group button_aligned">
                                      <asp:Button ID="BtnSearch" runat="server" Text="<%$Resources:Resource, Common_BtnSrch %>" class="btn btn-primary"></asp:Button>&nbsp;
                                      <asp:Button runat="server" ID="btnclear" Text="<%$Resources:Resource, Common_btnClr %>" CausesValidation="false" class="btn btn-primary"></asp:Button>
                                    </div>
                                </div>--%>
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
    
        
    <asp:HiddenField runat="server" ID="hdn" />

     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

     <%--' --------------------------------------------
        'Feature ID :1096
        'Date: 2019-06-04
        'Purpose: Preview Question 
        'Code added by Pragnesha
        '---------------------------------------------
    --%>
    <div id="dialog" style="display: none">  
   
        <table style="background-color: #FFFEE4; border: none; width:900px" >
           <tr id="audiorow"><td>
            <audio controls id="modal-audio">
            <source src="" type="audio/mpeg">
            </audio>
           </td></tr>
            <tr id="rowQue">
                <td>
                  <%=Resources.Resource.QuestionAns_Que%>  <input id="modal-text" type="text"  style="width:800px;background-color: #FFFEE4 ;
                        border-color: transparent; border: none;font-family:‚l‚r ‚oƒSƒVƒbƒN" disabled="disabled"  />
                </td>
            </tr>
                <tr id="rowPara">
                <td>
                  <%=Resources.Resource.QuestionAns_Que%>  <textarea rows='15' cols='40' id="Textarea1"   style="width:800px;background-color: #FFFEE4 ;
                        border-color: transparent; border: none;font-family:‚l‚r ‚oƒSƒVƒbƒN" disabled="disabled"  ></textarea>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="" id="modal-img" width="300" height="150" />
                </td>
            </tr>
            <tr id="rowopt1">
                <td>
                    <input type="radio" />
                    <input id="modal-opt1" type="text" style="width:500px;background-color: #FFFEE4 ; border-color: transparent;border: none;font-family:‚l‚r ‚oƒSƒVƒbƒN" width="500" height="200" disabled="disabled" />
                </td>
            </tr>
            <tr id="rowopt2">
                <td>
                    <input type="radio" />
                    <input id="modal-opt2" type="text"  style=" width:500px;background-color: #FFFEE4 ; border-color: transparent;border: none;font-family:‚l‚r ‚oƒSƒVƒbƒN" disabled="disabled" />
                </td>
            </tr>
            <tr id="rowopt3">
                <td>
                    <input type="radio" />
                    <input id="modal-opt3" type="text" style="width:500px; background-color: #FFFEE4 ; border-color: transparent;border: none;font-family:‚l‚r ‚oƒSƒVƒbƒN" disabled="disabled" />
                </td>
            </tr>
            <tr id="rowopt4">
                <td>
                    <input type="radio" />
                    <input id="modal-opt4" type="text" style="width:500px; background-color: #FFFEE4 ; border-color: transparent;border: none;font-family:‚l‚r ‚oƒSƒVƒbƒN" disabled="disabled"  />
                </td>
            </tr>
        </table>
    </div>
    <%--------------Ended by Pragnesha-------------------%>
    <%--</form>--%>
  
    </asp:Content>