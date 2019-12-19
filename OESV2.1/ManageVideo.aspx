<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ManageVideo.aspx.vb" Inherits="Unirecruite.ManageVideo"
    MasterPageFile="~/MasterPage.Master" Title="OESV2---Upload Videos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        #ContentPlaceHolder1_Uploader{
            display: block;
            width: 100%;
            height: calc(1.75rem + 2px);
            padding: .0rem .75rem;
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

    <script type="text/javascript">
        var $attach = $('#attach-project-file'),
    $remove = $('#remove-project-file'),
    $name = $('#attached-project-file');

// initially hide the remove button
$remove.hide();

// do this when file input has changed
// i.e.: a file has been selected
$attach.on('change', function() {
    var val = $(this).val();
    if (val !== '') {
        // if value different than empty

        // show the file name as text
        // hide/text/fadeIn creates a nice effect when changing the text
        $name
            .hide()
            .text(val)
            .fadeIn();

        // show the remove button
        $remove.fadeIn();
    } else {
        // if value empty, means the file has been removed

        // show the default text
        $name
            .hide()
            .text('Click to select a file')
            .fadeIn();

        // hide remove button
        $remove.fadeOut();
    }
});

// remove selected file when clicking the remove button
// prevent click bubbling to the parent label and triggering file selection
$remove.on('click', function(e) {
    e.preventDefault();
    e.stopPropagation();

    $attach
        .val('')
        .change(); // trigger change event
});
    </script>

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
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="Content-Type" content="text/html; charset=Shift_JIS">
    <asp:ScriptManager runat="server" ID="Script1">
    </asp:ScriptManager>
<%--    <form id="form1" runat="server">--%>
    <%--Created by Pranit Chimurkar on 07/11/2019--%>
    <div class="container">
        <div class="row">
         <div class="col-sm-12">
        <section class="content-header">
          <div class="container-fluid">
              <div class="row">
                    <div class="col-sm-12">
                            <h1><%=Resources.Resource.ManageVideo_UpVideo %></h1>
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
                          <h1 class="card-title"><%=Resources.Resource.ManageVideo_UpVideo%></h1>
                        </div>
                         <div class="card-body">
                            <div class="row">
                                <div class="col-sm-4 border border-info rounded" style="background-color:beige">
                                    <div class="form-group">
                                                    <div class="col-sm-12" style="vertical-align: top; height: 300px; overflow: auto;">
                                                        <asp:UpdatePanel runat="server" ID="Up1">
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="trvwCategory" EventName="trvwCategory_TreeNodePopulate" />
                                                                <asp:PostBackTrigger ControlID="trvwCategory" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                <asp:TreeView ID="trvwCategory" ShowLines="true" runat="server" ExpandDepth="0" NodeWrap="True"
                                                                    Font-Size="15px" Font-Names="Arial">
                                                                    <ParentNodeStyle ForeColor="#526B94" />
                                                                    <SelectedNodeStyle ForeColor="#000000" BackColor="#FFFF2A" BorderColor="Black" BorderStyle="Solid"
                                                                        BorderWidth="1px" />
                                                                    <RootNodeStyle ForeColor="#526B94" />
                                                                    <Nodes>
                                                                        <%-- <asp:TreeNode Text="Categories" Value="null" ></asp:TreeNode>--%>
                                                                    </Nodes>
                                                                </asp:TreeView>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                    
                                                    <%--Add New/Remove Category--%>
                                                    <div class="col-sm-12">
                                                            <div class="form-group">
                                                               <label><%=Resources.Resource.ManageVideo_AddCategory %></label>
                                                                <asp:TextBox ID="txtNewCat" runat="server" MaxLength="255" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="form-group" style="text-align:center">
                                                                <asp:Button ID="imgbtnAddCat" runat="server" Text="<%$Resources:Resource, Common_Add%>" class="btn btn-primary" Width="90px"/>&nbsp;&nbsp;
                                                                <asp:Button ID="imgbtnDelCat" OnClientClick="return confirm('Are you sure you want to delete this category and its sub-categories?');" runat="server" Text="<%$Resources:Resource, Common_Revm%>" class="btn btn-primary" Width="90px"/>
                                                            </div>
                                                    </div>
                                            </div>
                                     </div>

                                <div class="col-sm-8 border border-info rounded">
                                    
                                        <div class="col-sm-12">
                                                <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-sm-12">
                                            <h4><%=Resources.Resource.ManageVideo_UpVideo%></h4>
                                        </div>
                                        <div class="form-group">                                               
                                               <label><%=Resources.Resource.VideoList_Category%></label>
                                               <asp:TextBox ID="txtCat" runat="server" Enabled="false" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">                                               
                                               <label><%=Resources.Resource.VideoList_Title%></label>
                                               <asp:TextBox ID="txtTitle" runat="server" Columns="90" MaxLength="1000" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label><%=Resources.Resource.ManageVideo_SelectFile%></label><span style="font-size:12px; color:black; background-color:bisque">(<%=Resources.Resource.ManageVideo_VideoFormat%>)</span>                                               
                                            <div class="btn btn-default btn-file">
                                    <i class="fas fa-paperclip"></i><%=Resources.Resource.Common_ChoosePhoto%>
                                   <asp:FileUpload ID="Uploader" runat="server" accept=".mp4,.ppt,.pptx"/>
                                </div>
                                            
                                           
                                        </div>
                                        <div class="form-group" style="text-align:center">
                                            <asp:Button ID="imgbtnSubmit" runat="server" Text="<%$Resources:Resource, Common_Save%>" class="btn btn-primary"/>&nbsp;&nbsp;
                                            <asp:Button ID="ImageButton1" runat="server" Text="<%$Resources:Resource, Common_Cancel%>" class="btn btn-primary"/>
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
    <%--Ended by Pranit--%>
 <%--   </form>--%>
</asp:Content>