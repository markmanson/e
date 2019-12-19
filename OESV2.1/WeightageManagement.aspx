<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="WeightageManagement.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2------Weight Management"
    Inherits="Unirecruite.WeightageManagement" ClientTarget="downlevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<script type="text/javascript" src="JQuerys/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="JQuerys/jquery-ui-1.8.custom.min.js"></script>

    <script type="text/javascript" src="JQuerys/ui.multiselect.js"></script>--%>

    <%--<script type="text/javascript">
	$(function(){
		$(".multiselect").multiselect();
	});
	



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


	
	
	  function addTitleAttributes()

{

   numOptions = document.getElementById('ddlCourse').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('ddlCourse').options[i].title = document.getElementById('ddlCourse').options[i].text;

   

}

  

    </script>

    <script type="text/javascript" language="javascript">
	$(function(){
		//$.localise('ui-multiselect', {/*language: 'en',*/ path: 'js/locale/'});
		$(".multiselect").multiselect();
	});



       
    </script>--%>
    <!--Added by Pranit Chimurkar on 06/12/2019-->
    <script>
        function isNumberKey(evt)
          {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

             return true;
          }
        function ChangeInterMediate() {
            var basic1val = document.getElementById("<%=txtBasicq.ClientID%>").value;            
            if (basic1val > 100) {
                basic1val = document.getElementById("<%=txtBasicq.ClientID%>").value = 100;
                document.getElementById("<%=txtIntermediateq.ClientID%>").value = 0;
            }
            else {
                var intermediate1val = 100 - basic1val;
                document.getElementById("<%=txtIntermediateq.ClientID%>").value = intermediate1val;
            }
        }
        function ChangeBasic() {
            var intermediate2val = document.getElementById("<%=txtIntermediateq.ClientID%>").value;
            if (intermediate2val > 100) {
                intermediate2val = document.getElementById("<%=txtIntermediateq.ClientID%>").value = 100;
                document.getElementById("<%=txtBasicq.ClientID%>").value = 0;
            }
            else {
                var basic2val = 100 - intermediate2val;
                document.getElementById("<%=txtBasicq.ClientID%>").value = basic2val;
            }            
        }




        //For True/False
        function ChangeBlankMchoice() {
            var tf1val = document.getElementById("<%=txtTF.ClientID%>").value;
            if (document.getElementById("<%=txtBlank.ClientID%>").value == "" && document.getElementById("<%=txtMChoice.ClientID%>").value == "") {
                if (isNaN(tf1val) == true) {
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                }
                else if (tf1val >= 100) {
                    tf1val = document.getElementById("<%=txtTF.ClientID%>").value = 100;
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else {
                }
            }
            else if (document.getElementById("<%=txtBlank.ClientID%>").value != "" && document.getElementById("<%=txtMChoice.ClientID%>").value == "") {
                var blank1val = document.getElementById("<%=txtBlank.ClientID%>").value;
                if (isNaN(tf1val) == true) {
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                }
                else if (tf1val >= 100 - blank1val) {
                    tf1val = document.getElementById("<%=txtTF.ClientID%>").value = 100 - blank1val;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else {
                    var mChoice1val = 100 - tf1val - blank1val;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = mChoice1val;
                }
            }
            else if (document.getElementById("<%=txtBlank.ClientID%>").value == "" && document.getElementById("<%=txtMChoice.ClientID%>").value != "") {
                var mchoice1val = document.getElementById("<%=txtMChoice.ClientID%>").value;
                if (isNaN(tf1val) == true) {
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                }
                else if (tf1val >= 100 - mchoice1val) {
                    tf1val = document.getElementById("<%=txtTF.ClientID%>").value = 100 - mchoice1val;
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                else {
                    var blank1val = 100 - tf1val - mchoice1val;
                    document.getElementById("<%=txtBlank.ClientID%>").value = blank1val;
                }
            }
            else if (document.getElementById("<%=txtBlank.ClientID%>").value != "" && document.getElementById("<%=txtMChoice.ClientID%>").value != ""){
                    if (isNaN(tf1val) == true) {
                        document.getElementById("<%=txtTF.ClientID%>").value = 0;
                    }
                    document.getElementById("<%=txtBlank.ClientID%>").value = "";
                    document.getElementById("<%=txtMChoice.ClientID%>").value = "";
            }
        }




        //For Blank
        function ChangeTFMchoice() {
            var blank2val = document.getElementById("<%=txtBlank.ClientID%>").value;
            if (document.getElementById("<%=txtTF.ClientID%>").value == "" && document.getElementById("<%=txtMChoice.ClientID%>").value == "") {
                if (isNaN(blank2val) == true) {
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                else if (blank2val >= 100) {
                    blank2val = document.getElementById("<%=txtBlank.ClientID%>").value = 100;
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else {
                }
            }
            else if (document.getElementById("<%=txtTF.ClientID%>").value != "" && document.getElementById("<%=txtMChoice.ClientID%>").value == "") {
                var tf2val = document.getElementById("<%=txtTF.ClientID%>").value;
                if (isNaN(blank2val) == true) {
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                else if (blank2val >= 100 - tf2val) {
                    blank2val = document.getElementById("<%=txtBlank.ClientID%>").value = 100 - tf2val;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else {
                    var mChoice2val = 100 - tf2val - blank2val;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = mChoice2val;
                }
            }
            else if (document.getElementById("<%=txtTF.ClientID%>").value == "" && document.getElementById("<%=txtMChoice.ClientID%>").value != "") {
                var mChoice2val = document.getElementById("<%=txtMChoice.ClientID%>").value;
                if (isNaN(blank2val) == true) {
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                else if (blank2val >= 100 - mChoice2val) {
                    blank2val = document.getElementById("<%=txtBlank.ClientID%>").value = 100 - mChoice2val;
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                }
                else {
                    let tf2val = 100 - blank2val - mChoice2val;
                    document.getElementById("<%=txtTF.ClientID%>").value = tf2val;
                }
            }
            else if (document.getElementById("<%=txtTF.ClientID%>").value != "" && document.getElementById("<%=txtMChoice.ClientID%>").value != "") {
                if (isNaN(blank2val) == true) {
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                document.getElementById("<%=txtTF.ClientID%>").value = "";
                document.getElementById("<%=txtMChoice.ClientID%>").value = "";
            }
        }




        //For Multiple Choice
        function ChangeTFBlank() {
            var mChoice3val = document.getElementById("<%=txtMChoice.ClientID%>").value;
            if (document.getElementById("<%=txtTF.ClientID%>").value == "" && document.getElementById("<%=txtBlank.ClientID%>").value == "") {
                if (isNaN(mChoice3val) == true) {
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else if (mChoice3val >= 100) {
                    mChoice3val = document.getElementById("<%=txtMChoice.ClientID%>").value = 100;
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                else {
                }
            }
            else if (document.getElementById("<%=txtTF.ClientID%>").value != "" && document.getElementById("<%=txtBlank.ClientID%>").value == "") {
                var tf3val = document.getElementById("<%=txtTF.ClientID%>").value;
                if (isNaN(mChoice3val) == true) {
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else if (mChoice3val >= 100 - tf3val) {
                    mChoice3val = document.getElementById("<%=txtMChoice.ClientID%>").value = 100 - tf3val;
                    document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                }
                else {
                    var blank3val = 100 - tf3val - mChoice3val;
                    document.getElementById("<%=txtBlank.ClientID%>").value = blank3val;
                }
            }
            else if (document.getElementById("<%=txtTF.ClientID%>").value == "" && document.getElementById("<%=txtBlank.ClientID%>").value != "") {
                blank3val = document.getElementById("<%=txtBlank.ClientID%>").value;
                if (isNaN(mChoice3val) == true) {
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                else if (mChoice3val >= 100 - blank3val) {
                    mChoice3val = document.getElementById("<%=txtMChoice.ClientID%>").value = 100 - blank3val;
                    document.getElementById("<%=txtTF.ClientID%>").value = 0;
                }
                else {
                    var tf3val = 100 - blank3val - mChoice3val;
                    document.getElementById("<%=txtTF.ClientID%>").value = tf3val;
                }
            }
            else if (document.getElementById("<%=txtTF.ClientID%>").value != "" && document.getElementById("<%=txtBlank.ClientID%>").value != ""){
                if (isNaN(mChoice3val) == true) {
                    document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
                }
                document.getElementById("<%=txtTF.ClientID%>").value = "";
                document.getElementById("<%=txtBlank.ClientID%>").value = "";
            }
        }

        <%--function ChangeTFMchoice() {
            var blank2val = document.getElementById("<%=txtBlank.ClientID%>").value;
            if (blank2val > 100) {
                blank2val = document.getElementById("<%=txtBlank.ClientID%>").value = 100;                
                document.getElementById("<%=txtTF.ClientID%>").value = 0;
                document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
            }
            else {
                    var tf2val = Math.floor((100 - blank2val) / 2);
                    var mChoice2val = 100 - blank2val - tf2val;
                    document.getElementById("<%=txtTF.ClientID%>").value = tf2val;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = mChoice2val;
            }            
        }
        function ChangeTFBlank() {
            var mChoice3val = document.getElementById("<%=txtMChoice.ClientID%>").value;
            if (mChoice3val > 100) {
                mChoice3val = document.getElementById("<%=txtMChoice.ClientID%>").value = 100;
                document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                document.getElementById("<%=txtTF.ClientID%>").value = 0;
            }
            else {
                    var tf3val = Math.floor((100 - mChoice3val) / 2);
                    var blank3val = 100 - mChoice3val - tf3val;
                    document.getElementById("<%=txtTF.ClientID%>").value = tf3val;
                    document.getElementById("<%=txtBlank.ClientID%>").value = blank3val;
            }            
        }--%>
        <%--function ChangeBlankMchoice() {
            var tf1val = document.getElementById("<%=txtTF.ClientID%>").value;
            if (tf1val > 100) {
                tf1val = document.getElementById("<%=txtTF.ClientID%>").value = 100;
                document.getElementById("<%=txtBlank.ClientID%>").value = 0;
                document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
            }
            else {
                    var blank1val = Math.floor((100 - tf1val) / 2);
                    var mChoice1val = 100 - tf1val - blank1val;
                    document.getElementById("<%=txtBlank.ClientID%>").value = blank1val;
                    document.getElementById("<%=txtMChoice.ClientID%>").value = mChoice1val;
            }            
        }
        function ChangeTFMchoice() {
            var blank2val = document.getElementById("<%=txtBlank.ClientID%>").value;
            var tf2val = document.getElementById("<%=txtTF.ClientID%>").value;
            if (blank2val > 100 - tf2val) {
                blank2val = document.getElementById("<%=txtBlank.ClientID%>").value = 100 - tf2val;
                document.getElementById("<%=txtMChoice.ClientID%>").value = 0;
            }
            else {
                var mChoice2val = 100 - blank2val - tf2val;
                document.getElementById("<%=txtMChoice.ClientID%>").value = mChoice2val;
            }            
        }--%>
    </script>
   
    <%--<form id="form1" runat="server">--%>
    <%--Created by Pranit Chimurkar on 2019/10/24--%>
        <!-- Content Header (Page header) -->
      <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">            
            <div class="row">
              <div class="col-sm-12">
                  <h1><%=Resources.Resource.WeightMgt_WeightageDetails%></h1>
              </div>
            </div>
            <div class="row">
              <div class="col-sm-12">
                  <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="Red"></asp:Label>
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
                          <h1 class="card-title"><%=Resources.Resource.WeightMgt_WeightageDetails%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group">
                                               <label><%=Resources.Resource.WeightSearch_CrsNm%><span style="color:red">*</span></label>
                                               <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="true" onmouseover="addTitleAttributes();" class="form-control"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <label><%=Resources.Resource.WeightMgt_ttlMrks %></label>
                                            <asp:Label ID="lblMarks" class="staticText" ForeColor="Green" runat="server" Text="0"></asp:Label>
                                            <br />
                                            <label><%=Resources.Resource.WeightMgt_VctSpceSub%></label>
                                            <asp:Label ID="lblVacant" ForeColor="Green" runat="server" Text="0"></asp:Label>
                                        </div>
                                    </div>                                    
                                 </div>
                                 <div class="col-sm-3">
                                     <div class="row">
                                         <div class="col-sm-12">
                                             <div class="form-group">
                                               <label><%=Resources.Resource.WeightSearch_SubNm%><span style="color:red">*</span></label>
                                               <asp:DropDownList ID="ddlSubject" runat="server" AutoPostBack="true" onmouseover="addTitleAttributes();" class="form-control"></asp:DropDownList>
                                            </div>
                                         </div>
                                     </div>
                                     <div class="row">
                                         <div class="col-sm-12">
                                             <label><%=Resources.Resource.WeightMgt_ttlQue %></label>
                                            <asp:Label ID="lblTotalQuestion" ForeColor="Green" runat="server" Text="0"></asp:Label>
                                            <br />
                                            <label><%=Resources.Resource.WeightMgt_ttlMrks %></label>
                                            <asp:Label ID="lblTotalMarks" ForeColor="Green"  runat="server" Text="0"></asp:Label>
                                            <br />
                                            <label><%=Resources.Resource.WeightMgt_bscQue %></label>
                                            <asp:Label ID="lblBasicQuestion" ForeColor="Green" runat="server" Text="0"></asp:Label>
                                            <br />
                                            <label><%=Resources.Resource.WeightMgt_Interque %></label>
                                            <asp:Label ID="lblInterQuestion" ForeColor="Green" runat="server" Text="0"></asp:Label>
                                         </div>
                                     </div>                                    
                                 </div>
                                 <div class="col-sm-6">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            <div class="form-group">  
                                               <label><%=Resources.Resource.WeightMgt_SubWeight%><span style="color:red">*</span></label>
                                               <asp:TextBox ID="txtWeight" MaxLength="3" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="form-group">
                                                <label><%=Resources.Resource.WeightMana_DifLevel%>&nbsp;(%)</label>
                                                <div class="input-group">
                                                     <asp:TextBox ID="txtBasicq" MaxLength="3" runat="server" class="form-control" onkeypress="return isNumberKey(event)" placeholder="<%$Resources:Resource, Common_Basic%>" onchange="ChangeInterMediate()"></asp:TextBox>                                        
                                                     <asp:TextBox ID="txtIntermediateq" MaxLength="3" runat="server" class="form-control" onkeypress="return isNumberKey(event)" placeholder="<%$Resources:Resource, Common_InterMediate%>" onchange="ChangeBasic()"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-12">
                                        <div class="form-group">                                        
                                        <label><%=Resources.Resource.WeightMgmt_QuesTypWiseWeight%>&nbsp;(%)</label>
                                        <div class="input-group">
                                             <asp:TextBox ID="txtTF" MaxLength="3" runat="server" class="form-control" onkeypress="return isNumberKey(event)" placeholder="<%$Resources:Resource, WeightMgmt_Trfa%>" onchange="ChangeBlankMchoice()"></asp:TextBox>
                                             <asp:TextBox ID="txtBlank" MaxLength="3" runat="server" class="form-control" onkeypress="return isNumberKey(event)" placeholder="<%$Resources:Resource, WeightMgmt_Blnk%>" onchange="ChangeTFMchoice()"></asp:TextBox>
                                             <asp:TextBox ID="txtMChoice" MaxLength="3" runat="server" class="form-control" onkeypress="return isNumberKey(event)" placeholder="<%$Resources:Resource, WeightMgmt_Mchoic%>" onchange="ChangeTFBlank()"></asp:TextBox>
                                        </div>
                                    </div>
                                    </div>                                    
                                </div>
                            </div>
                        </div>
                            
                            
                            <%--<div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightMgt_TF%><span style="color:red">*</span></label>
                                       <asp:TextBox ID="txtTF" MaxLength="3" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightMgt_MultipleChoice%><span style="color:red">*</span></label>
                                       <asp:TextBox ID="txtMChoice" MaxLength="3" runat="server" class="form-control"></asp:TextBox>                                     
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightMgt_Blank%><span style="color:red">*</span></label>
                                      <%-- <asp:TextBox ID="txtBlank" MaxLength="3" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                           <%-- <div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightMgt_BasicQue%><span style="color:red">*</span></label>
                                      <%-- <asp:TextBox ID="txtBasicq" MaxLength="3" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>--%>
                            <%--<div class="row">
                                <div class="col-sm-6">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.WeightMgt_Interques%><span style="color:red">*</span></label>
                                       <asp:TextBox ID="txtIntermediateq" MaxLength="3" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>--%>
                            <div class="row">
                                <div class="col-sm-12" style="text-align:center">
                                        <asp:Button ID="imgBtnSubmit" runat="server" class="btn btn-primary" Text="<%$Resources:Resource, Common_Regis %>" />&nbsp;&nbsp;
                                        <asp:Button ID="imgBtnClear" runat="server" class="btn btn-primary" Text="<%$Resources:Resource, Common_btnClr %>"/>&nbsp;&nbsp;
                                        <asp:Button ID="imgBtnUpdate" runat="server" Visible="False" class="btn btn-primary" Text="<%$Resources:Resource, Common_Update %>"/>
                                        <asp:Button ID="ImgBtnBack" runat="server" class="btn btn-primary" Text="<%$Resources:Resource, Common_btnBck %>"/>&nbsp;&nbsp;
                                        <asp:Button ID="imgBtnReset" runat="server" Visible="False" class="btn btn-primary" Text="<%$Resources:Resource, Common_Reset %>"/>
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
  
    <%--</form>--%>
</asp:Content>