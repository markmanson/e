<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AddCourse.aspx.vb" Inherits="Unirecruite.AddCourse"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Add Course" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">
        function numbersonly(myfield, e, str)
        {
            var key;
            var keychar;
            var strValidate;  
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
            if ((key==null) || (key==0) || (key==8) || (key==9) || (key==13) || (key==27) )
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
        function IsValid(val)
        {
            var flag = true;
            if(val != "")
            {
                if(!isNaN(val) )
                {
                    return true;
                }else
                {
                 return false;
                }        
            }
            else
                return false;
        }
        function CalculateTotQues() {}
        function validateNumber() {}
        function validateMNumber() {}
        function validateLNumber() {}    
        function checkLimit(){}  
        function addTitleAttributes(field)
        {
            numOptions = document.getElementById(field.id).options.length;
            for (i = 0; i < numOptions; i++)
            document.getElementById(field.id).options[i].title = document.getElementById(field.id).options[i].text; 
        }
    </script>

    <script type="text/javascript" src="JQuerys/jquery-1.3.2.js"></script>

    <script type="text/javascript" src="JQuerys/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="JQuerys/jquery-ui-1.8.custom.min.js"></script>

    <script type="text/javascript" src="JQuerys/ui.multiselect.js"></script>

    <script type="text/javascript">
        $(function(){
            $(".multiselect").multiselect();
        });
    </script>

    <script type="text/javascript">
        $(function(){
            $(".multiselect").multiselect();
        });
    </script>

    <div class="container">
        <div class="row">
      <div class="col-sm-12">
               <section class="content-header">
                    <div class="container-fluid">
                        <div class="row">
                            <div class="col-sm-12">
                                <h1><%=Resources.Resource.AddCourse_ClsCrsDts %></h1>
                            </div>
                        </div>
                        <div class="row">
                           <div class="col-sm-12">
                                <asp:Label ID="lblMessageInsertSuccess" runat="server"></asp:Label>                
                                <asp:Label ID="lblMsgUnSuccess" runat="server" ForeColor="Red"></asp:Label>               
                                <asp:Label ID="lblNotWeight" runat="server"></asp:Label>               
                                <asp:Label ID="lblInCompleteWeightage" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </section>

          <section class="content">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                        <div class="card-header">
                            <h1 class="card-title"><%=Resources.Resource.AddCourse_ClsCrsDts %></h1>
                            </div>
                               <div class="card-body">
                            <div class="row">
                                 <div class="col-sm-4">
                                     <div class="form-group" >
                                         <label><%=Resources.Resource.AddCourse_ClsNm%></label>
                                         <asp:DropDownList ID="ddlCenters" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes();" class="form-control">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                         </asp:DropDownList>
                                     </div>
                                 </div>
                                 <div class="col-sm-4">
                                     <div class="form-group">
                                         <label><%=Resources.Resource.CourseMain_SectionDesc%></label>
                                         <asp:DropDownList ID="ddlSectionDes" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes();" class="form-control">
                                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                         </asp:DropDownList>
                                     </div>
                                 </div>
                                 <div class="col-md-4">
                                     <div class="form-group" >
                                          <label><%=Resources.Resource.AddCourse_CrsNm%></label>
                                          <asp:ListBox ID="ddlCourses" runat="server" class="form-control" SelectionMode="Multiple" ></asp:ListBox>  
                                     </div>
                                 </div>
                            </div>
                                <div class="row">
                                    <div class="col-sm-12" style="text-align:center">
                                         <asp:Button ID="imgBtnRegister" class="btn btn-primary" Text="<%$Resources: Resource, Common_Regis %>" runat="server"  />&nbsp;
                                         <asp:Button ID="ImageButton2" class="btn btn-primary" Text="<%$Resources: Resource, Common_Regis %>" runat="server" Visible="false" />&nbsp;
                                         <asp:Button ID="imgBtnClear" class="btn btn-primary" Text="<%$Resources: Resource, Common_btnClr %>" runat="server"  />&nbsp;
                                         <asp:Button ID="imgBtnBack" class="btn btn-primary" Text="<%$Resources:Resource, Common_btnBck %>"  runat="server"  />&nbsp;
                                         <asp:Button ID="ImageButton5" class="btn btn-primary"  runat="server" Visible="False" />
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
</asp:Content>
