<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.search"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Search"
    CodeBehind="search.aspx.vb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />

    <script language="JavaScript" type="text/JavaScript">
<!--
function MM_reloadPage(init) {  //reloads the window if Nav4 resized
  if (init==true) with (navigator) {if ((appName=="Netscape")&&(parseInt(appVersion)==4)) {
    document.MM_pgW=innerWidth; document.MM_pgH=innerHeight; onresize=MM_reloadPage; }}
  else if (innerWidth!=document.MM_pgW || innerHeight!=document.MM_pgH) location.reload();
}
MM_reloadPage(true);

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_showHideLayers() { //v6.0
  var i,p,v,obj,args=MM_showHideLayers.arguments;
  for (i=0; i<(args.length-2); i+=3) if ((obj=MM_findObj(args[i]))!=null) { v=args[i+2];
    if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v=='hide')?'hidden':v; }
    obj.visibility=v; }
}

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
          debugger;
                numOptions = document.getElementById('ddlcenter').options.length;
                for (i = 0; i < numOptions; i++)
                document.getElementById('ddlcenter').options[i].title = document.getElementById('ddlcenter').options[i].text; 
          }




//-->
//This script is for check/uncheck of the checkbox on the form.

	function selectAll(formObj) 
  {
  
   for (var i=0;i < formObj.length;i++) 
   {
      fldObj = formObj.elements[i];
      if (fldObj.type == 'checkbox')
      { 
       fldObj.checked = true;
         //if(isInverse)
          //  fldObj.checked = (fldObj.checked) ? false : true;
         //else fldObj.checked = true; 
       }
   }
}
function deselectAll(formObj) 
  {
  
   for (var i=0;i < formObj.length;i++) 
   {
      fldObj = formObj.elements[i];
      if (fldObj.type == 'checkbox')
      { 
       fldObj.checked = false;
         //if(isInverse)
          //  fldObj.checked = (fldObj.checked) ? false : true;
         //else fldObj.checked = true; 
       }
   }
}

 function addTitleAttributes(field)
{
        numOptions = document.getElementById(field.id).options.length;
        for (i = 0; i < numOptions; i++)
        document.getElementById(field.id).options[i].title = document.getElementById(field.id).options[i].text;  

}

    </script>

    <style type="text/css">
        td.tblhead{
            background-color:#189599;
            text-align:center;
        }
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
    </style>

    <%--Created by Pranit Chimurkar on 2019/10/18--%>
    <%--<form id="frmSearch" name="frmSearch" runat="server">--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Content Header (Page header) -->
      <div class="container">
        <%--Search Condtition--%>
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-12">
                  <h1><%=Resources.Resource.Search_Head%></h1>
              </div>
            </div>
            <div class="row">
              <div class="col-sm-12">
                    <asp:Label ID="lblMsg" runat="server" Visible="False" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lblReocrds" runat="server" Visible="False" ForeColor="Red"></asp:Label>
              </div>
            </div>                        
          </div><!-- /.container-fluid -->
        </section>

        <!-- main content -->
        <section class="content">
                    <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">                          
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.Search_CenterCrsInfo%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-4">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.Search_ClsNm %></label>
                                       <asp:DropDownList ID="sel_subjectid" runat="server" AutoPostBack="True" onmouseover="addTitleAttributes(sel_subjectid);" class="form-control">
                                            <asp:ListItem Text="<%$Resources: Resource, Search_Select %>"></asp:ListItem> <%--<%$Resources:Resource, Search_Select %>">--%>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4">
                                    <div class="form-group">  
                                       <label><%=Resources.Resource.Search_CrsNm %></label>
                                       <asp:DropDownList ID="ddlcourse" runat="server" Enabled="false" onmouseover="addTitleAttributes(ddlcourse);" class="form-control">
                                            <asp:ListItem Text="<%$Resources: Resource, Search_Select %>"></asp:ListItem>
                                       </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-sm-4 button_aligned" style="text-align:center">
                                      <asp:Button ID="searchUser" Text="<%$Resources:Resource, Common_BtnSrch %>" runat="server" class="btn btn-primary"></asp:Button>&nbsp;&nbsp;
                                      <asp:Button runat="server" ID="btnclear" Text="<%$Resources:Resource, Common_btnClr %>" CausesValidation="false" class="btn btn-primary"></asp:Button>
                                      <%--<asp:Button ID="img_cancel" runat="server" Text="<%$Resources:Resource, Common_btnBck %>" class="btn btn-primary"></asp:Button>--%>
                                </div>
                              </div>
                          </div>
                        </div>
                      </div>
                    </div>
                </section>
            </div>
        </div>

        <%--Search result--%>
        <section class="content" id="gridDiv" visible="false" runat="server">
        <div class="row">
         <div class="col-sm-8">
        <!-- main content -->        
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
                                       <strong><%=Resources.Resource.AdminList_TotRecord%>:<asp:Label runat="server" ID="lblNum"></asp:Label></strong>
                                     </div>
                                </div>
                            <div class="row">
                                <div class="col-sm-12">
                                    <div class="table-responsive" style="max-height:263px">
                                <asp:DataGrid ID="DGData" runat="server" AutoGenerateColumns="False" AllowPaging="True" CssClass="table table-bordered table-striped table-hover">
                                    <SelectedItemStyle></SelectedItemStyle>
                                    <PagerStyle HorizontalAlign="Left" Visible="false" ForeColor="#000066" BackColor="Transparent"
                                        Mode="NumericPages" ></PagerStyle>
                                    <ItemStyle></ItemStyle>
                                    <Columns>
                                        <asp:BoundColumn DataField="SrNo" HeaderText="<%$Resources:Resource, Common_SrNo %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"/>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="userid" Visible="False">
                                            <ItemStyle/>
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Name" HeaderText="<%$Resources:Resource, AppearedExam_CndNm %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>                                        
                                        <asp:BoundColumn DataField="RollNo" HeaderText="<%$Resources:Resource,AssignedExam_Rollno %>">
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="<%$Resources:Resource,Common_Sltall%>">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkSelectAll" type="checkbox" runat="server" Text="<%$Resources:Resource,Common_Sltall%>" AutoPostBack="true"
                                                    OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                            </HeaderTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Font-Bold></HeaderStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRemove" runat="server" Text="" OnCheckedChanged="chkRemove_CheckedChanged1"
                                                    AutoPostBack="True" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <HeaderStyle BackColor="#189599" ForeColor="#ffffff"></HeaderStyle>
                                </asp:DataGrid>
                            </div>
                            </div>
                            </div>
                              <%--Pagination--%>
                             <%--<div class="row">
                                       <div class="col-sm-6">--%>
                                            <%--<div class="row">
                                            <span><%=Resources.Resource.Common_GoToPage%></span>&nbsp;&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddlPages" AutoPostBack="true" width="60px" runat="server" class="form-control">
                                            </asp:DropDownList>
                                       </div>--%>
                                  <%--</div>
                             </div>--%>
                             <%--End of Pagination--%>
                                    </div>
                                    <%--<div>
                                        <table id="tblMessage" width="100%" cellspacing="0"></table>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
             </div>

             <%--Examination Date--%>
            <div class="col-sm-4">
                   <div class="row" id="tblExam" visible="false" runat="server">
                    <div class="col-md-12">
                      <div class="card card-info">                          
                        <div class="card-header">
                          <h1 class="card-title"><%=Resources.Resource.Search_ExamDate%></h1>
                        </div>
                         <div class="card-body" style="text-align:center">
                             <div class="row">
                                 <div class="col-sm-2"></div>
                                 <div class="col-sm-8">
                                     <div class="form-group">
                                         <label><%=Resources.Resource.Search_ExamDate%></label>
                                         <input id="txtExamDate" runat="server" class="form-control" onkeydown="return false"/>
                                     </div>
                                 </div>
                                 <div class="col-sm-2"></div>
                             </div>
                             <div class="form-group">
                                 <asp:Button ID="Mail" runat="server" Text="<%$Resources:Resource, Search_AssignExam %>" class="btn btn-primary"></asp:Button>
                             </div>
                         </div>
                       </div>
                    </div>
                  </div>
                  <div>
                      <asp:ValidationSummary ID="summarycont" runat="server" ShowMessageBox="True" ShowSummary="False"></asp:ValidationSummary>
                  </div>
            </div>
        </div>
        </section>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>