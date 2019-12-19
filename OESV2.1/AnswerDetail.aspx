<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AnswerDetail.aspx.vb"
    Inherits="Unirecruite.AnswerDetail" MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Answer Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<style type="text/css">
        .entryHeader
        {
            font-size: 50px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
        }
        .cssTable
        {
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
        }
        .cssLable
        {
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
        }
        .cssLable
        {
            font-size: 15px;
        }
        .WrapperDiv
        {
            width: 800px;
            border: 1px solid black;
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: #ffffff;
            position: relative;
        }
    </style>--%>

    <script language="javascript" type="text/javascript">
   function Validate()
    {
     
     var objUpload=eval("document.getElementById('MyFile')");
     var sUpload=objUpload.value;    
         
     if(document.getElementById('MyFile').value == "")
     {
       alert("You must enter a file to process.");
                            event.returnValue=false;                           
                            return ; 
     }
     if(objUpload!="")
        {
             var iExt=sUpload.indexOf("\\");
             var iDot=sUpload.indexOf(".");
             
             
            
       if((iExt < 0 ) || (iDot < 0))
             {
        alert("Invalid file path to proceed.");
                                objUpload.focus();
                                event.returnValue=false;                                
                                return; 
       }
       if(iDot > 0)
       {
        var aUpload=sUpload.split(".");
        if((aUpload[aUpload.length-1]!="csv") && (aUpload[aUpload.length-1]!="CSV"))
        {
         alert("Only CSV file are allowed to proceed.");
         objUpload.focus();
         event.returnValue=false;            
         return; 
        }
       }
     }
    }
function Layer2_onclick() {

}

    </script>
    <%--<form id="form1" runat="server">--%>
    <div class="container">
        <div class="row">
         <div class="col-sm-12">
    <section class="content" style="margin-top:10px">
                  <div class="container-fluid">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.AnswerDetail_ans%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-12" id="tblDetail" runat="server">
                                            <div class="row" id="tdQues" runat="server"></div>
                                            <div class="row">
                                                <div class="col-sm-6" id="tdOptions" runat="server">
                                                    <strong><asp:Label ID="text1" Text="<%$Resources:Resource, Ans_option%>" runat="server"></asp:Label></strong>
                                                </div>
                                                <div class="col-sm-3" id="tdCorrectOption" runat="server">
                                                    <strong><asp:Label ID="text2" Text="<%$Resources:Resource, Ans_CorOption%>" runat="server"></asp:Label></strong>
                                                </div>
                                                <div class="col-sm-3" id="tdGivenAnswer" runat="server">
                                                    <strong><asp:Label ID="text3" Text="<%$Resources:Resource, Ans_AnsOption%>" runat="server"></asp:Label></strong>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <asp:button ID="btnBack" runat="server" Text="<%$Resources:Resource, Common_btnBck%>" CausesValidation="False" class="btn btn-primary"></asp:button>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label ID="lblTotalMarks" runat="server" Text="Label"></asp:Label>
                                                </div>
                                                <div class="col-sm-3">
                                                    <asp:Label ID="lblMarksObtained" runat="server" Text="Label"></asp:Label>
                                                </div>
                                            </div>
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
