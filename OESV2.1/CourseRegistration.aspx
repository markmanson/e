<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CourseRegistration.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Course Registration"
    Inherits="Unirecruite.CourseRegistration" ClientTarget="downlevel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Code written by Rajat Argade on 22/10/2019 -->
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <!-- Tell the browser to be responsive to screen width -->
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    
    <script type="text/javascript">
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

   numOptions = document.getElementById('ddlMainCourse').options.length;

   for (i = 0; i < numOptions; i++)

      document.getElementById('ddlMainCourse').options[i].title = document.getElementById('ddlMainCourse').options[i].text;

   

}

  

    </script>

    <script type="text/javascript" language="javascript">
	$(function(){
		//$.localise('ui-multiselect', {/*language: 'en',*/ path: 'js/locale/'});
		$(".multiselect").multiselect();
        });

        function addtitleattributesSection()

{

   numoptions = document.getelementbyid('ddlSectionDes').options.length;

   for (i = 0; i < numoptions; i++)

      document.getelementbyid('ddlSectionDes').options[i].title = document.getelementbyid('ddlSectionDes').options[i].text;

   

}



    </script>
    <!-- Content Header (Page header) -->
      <div class="container">
        <div class="row">
         <div class="col-sm-12">
            <section class="content-header">           
                <div class="row">
                  <div class="col-sm-12">
                      <h1><asp:Label runat="server" ID="legendLabel"></asp:Label></h1>
                  </div>
                </div>
                  <div class="row">
                  <div class="col-sm-12">
                      <asp:Label runat="server" ID="lblMsg" Visible="False" ForeColor="Red"></asp:Label>
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
                          <h1 class="card-title"><asp:Label runat="server" ID="lblhead"></asp:Label></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseRegistration_MainCrs%></label>
                                       <asp:DropDownList ID="ddlMainCourse" runat="server" AutoPostBack="True" class="form-control" onmouseover="addTitleAttributes();">
                                            </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseMain_SectionDesc%></label>
                                       <asp:DropDownList ID="ddlSectionDes" runat="server" AutoPostBack="True" onmouseover="addtitleattributesSection();" class="form-control">
                                            </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseRegistration_CourseCode%></label>
                                       <asp:TextBox runat="server" class="form-control" id="TxtCourseCode"   MaxLength="255" placeholder="<%$Resources:Resource, CourseRegistration_CourseCode %>"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseRegistration_CrsNm%></label>
                                       <asp:TextBox runat="server" class="form-control" id="TxtCourseName" MaxLength="255" placeholder="<%$Resources:Resource, CourseRegistration_CrsNm %>"></asp:TextBox>
                                    </div>
                                </div>
                             </div>
                             <div class="row">
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseMaintenance_TtlTime%></label>
                                       <asp:TextBox runat="server" class="form-control" id="txtTotalTime" MaxLength="255" placeholder="<%$Resources:Resource, CourseMaintenance_TtlTime %>"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseRegistration_ttlMrks%></label>
                                       <asp:TextBox runat="server" class="form-control" id="txtTotalMarks"  Width=""  MaxLength="255" placeholder="<%$Resources:Resource, CourseRegistration_ttlMrks %>"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3">
                                    <div class="form-group">
                                       <label><%=Resources.Resource.CourseRegistration_PsingMrks%></label>
                                       <asp:TextBox runat="server" class="form-control" id="txtPassingMarks"  MaxLength="255" placeholder="<%$Resources:Resource, CourseRegistration_PsingMrks %>"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-sm-3 button_aligned" style="text-align:center">
                                        <asp:Button ID="imgBtnSubmit" class="btn btn-primary" runat="server" Text="<%$Resources:Resource, CourseRegistration_btnReg %>"  />&nbsp;
                                        <asp:Button ID="imgBtnClear" class="btn btn-primary" runat="server" Text="<%$Resources:Resource, CourseRegistration_btnClr %>" />&nbsp;
                                        <asp:Button ID="imgBtnUpdate" class="btn btn-primary" Text="<%$Resources:Resource, Common_Update %>" runat="server" Visible="False" />&nbsp;
                                        <asp:Button ID="ImgBtnBack" class="btn btn-primary" Text="<%$Resources:Resource, Common_btnBck %>" runat="server"/>&nbsp;
                                        <asp:Button ID="imgBtnReset" class="btn btn-primary" Text="<%$Resources:Resource, Common_Reset %>" runat="server"  Visible="False" />
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
    <!-- Code Ended By Rajat Argade on 22/10/2019 -->
</asp:Content>