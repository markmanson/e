<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VideoLive.aspx.vb" MasterPageFile="~/MasterPage.Master" Title="OESV2---Video Live" Inherits="Unirecruite.VideoLive" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script runat="server">
    Protected Sub btnDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
</script>

    <script type="text/javascript" src="jwplayer/jwplayer.js"></script>

    <script type="text/javascript" src="swfobject.js"></script>
    <%-- Added by Pragnesha for Bug ID 970(Web browser Icon) Date: 13-12-18--%>
 <link rel="shortcut icon" type="image/png" href="images/OES_ICON.png" />
<%-- ---------Ended by Pragnesha-------------------------------------------%>

    <style type="text/css">
        newclass
        {
        	background-color:#FFFF2A;
        }
        </style>

    <script language="javascript" type="text/javascript">
   function Validate()
    {
     debugger;
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
    <link href="images/css.css" type="text/css" rel="stylesheet">
    <%--<form id="Form1" method="post" runat="server">--%>
    <asp:ScriptManager runat="server" ID="Script1">
    </asp:ScriptManager>
     <%--Created by Pranit Chimurkar on 07/11/2019--%>
    <br />
    <div class="container-fluid">
       <div class="row">
         <div class="col-sm-12">

               <section class="content-header">
                  <div class="container-fluid">                        
                        <div class="row">
                            <div class="col-sm-12">
                                <h1><%=Resources.Resource.VideoLive_livevdo%></h1>
                            </div>
                        </div>
                      </div><!-- /.container-fluid -->               
            </section>

             <section class="content">
                <div class="container-fluid">
                  <div class="row">
                    <div class="col-md-12">
                      <div class="card card-info">
                         <div class="card-body">
                            <div class="row">
                                       <div class="col-sm-2 border border-info rounded" style="background-color:white">
                                          <asp:UpdatePanel runat="server" ID="Up1">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="trvwCategory" EventName="trvwCategory_TreeNodePopulate" />
                                                    <asp:AsyncPostBackTrigger ControlID="trvwCategory" EventName="trvwCategory_SelectedNodeChanged" />
                                                    <%-- <asp:PostBackTrigger ControlID="trvwCategory" />--%>
                                                </Triggers>
                                                <ContentTemplate>
                                                    <div style="vertical-align: top; height: 420px; overflow: auto;">
                                                        <%-- <b>Categories</b>--%>
                                                        <asp:TreeView ID="trvwCategory" runat="server" Font-Size="15px" Font-Names="Arial"
                                                            ExpandDepth="0" NodeWrap="True" ShowLines="True">
                                                            <ParentNodeStyle ForeColor="#526B94" />
                                                            <SelectedNodeStyle />
                                                            <%--ForeColor="#000000" BackColor="#FFFFCC" BorderColor="Black" 
                                                        BorderStyle="Dotted" BorderWidth="1px" />--%>
                                                            <RootNodeStyle ForeColor="#526B94" />
                                                            <Nodes>
                                                            </Nodes>
                                                        </asp:TreeView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                       </div>
                                       <div class="col-sm-2 border border-info rounded">
                                           <div style="vertical-align: top; height: 420px; overflow: auto;">
                                                <%-- <b>Videos</b>--%>
                                                <table runat="server" id="tblvdoList"></table>
                                                <asp:LinkButton ID="LinkButton1"  Visible="false" runat="server" OnClick="LinkButton1_Click">LinkButton</asp:LinkButton>
                                            </div>
                                       </div>
                                        <div class="col-sm-8 border border-info rounded">
                                            <%--<div id="container" runat="server" style="width:400px; height:400px">
                                            </div>--%>
                                             <video  style="width:100%; height:400px" controls="controls"  id="VDO" runat="server" controlslist="nodownload" ></video>
                                            <br />
                                            <asp:Label runat="server" ID="LblTitle" ForeColor="#526B94" Font-Bold="true"></asp:Label>
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
    <script type="text/javascript"> 
     //function playSelected(fileURL,ID,msgtitle)
     //    {
     //   document.getElementById("LblTitle").innerText="Currently playing: "+msgtitle;
     //   jwplayer("container").setup({
     //   autostart: false , controlbar: "none",
     //   file: fileURL,
     //   //  duration: duration, 
     //   flashplayer: "jwplayer/player.swf",
     //   image: 'uploads/preview.jpg',
     //   allowscriptaccess:"always",
     //   allowfullscreen:"true",
     //   wmode :'opaque',
     //   title: 'Main Video',
     //   description: 'This is the main video.',
     //   skin: 'mediaplayer/player.swf',
     //   repeat : 'false',
     //   stretching : 'exactfit',
     //   controlbar:'bottom',
     //   icons : 'true',
     //   //frontcolor : '86C29D', 
     //   //backcolor : '849BC1', 
     //   //lightcolor : 'C286BA', 
     //   screencolor : '000000', 
     //   autostart: 'false',
     //   //flashvars :'config=config.xml',
     //   volume: 100, width: 660}); 
        
    
    
    //to get every element of a link button
       // var _aryElm=document.getElementsByTagName('a');  //return an array with every <li> of the page
        //for( x in _aryElm)
        //{
            // _elm=_aryElm[x];
              //_elm.style.backgroundColor="red";
              //_elm.style.border="red";
            // _elm.className="a_class";
       // }
        
    // Apply CSS on selected link button
        //var _elm=document.getElementById(ID);
        //_elm.className="selectedVDO";
  
    
    //return false  ;
    }
    //    function ChangeStyle(id)
    //{

    //    document.getElementById(id).style.color="#FFFF2A";
    //}
    //function msgTitle(msgtitle)
    //{


    //return false;
    //}
   </script>
<%--</form>--%>
</asp:Content>