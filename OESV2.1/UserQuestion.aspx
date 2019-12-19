<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserQuestion.aspx.vb"
    MasterPageFile="~/MasterPage.Master" Title="Online Examinations solution------Admin Home"
    Inherits="Unirecruite.UserQuestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script runat="server">
        Protected Sub btnDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        End Sub
    </script>

    <style type="text/css">
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
    </style>

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
    <form id="Form1" method="post" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="errorMsg" runat="server" Visible="false" ForeColor="Red" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblQuestion" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <fieldset>
                    <legend class="outerframe">Question Publish Deatils </legend>
                    <div style="text-align: right">
                        <asp:Label runat="server" CssClass="genMsg" ID="lblrecords"></asp:Label>
                    </div>
                    <asp:DataGrid ID="DGData" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        CssClass="gridClass" ItemStyle-Wrap="true" Width="100%">
                        <SelectedItemStyle CssClass="gSelectedItem"></SelectedItemStyle>
                        <PagerStyle CssClass="gPagerStyle" HorizontalAlign="Left" Visible="false" Mode="NumericPages" />
                        <ItemStyle CssClass="gItemStyle"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="SrNo" HeaderText="Sr.No." HeaderStyle-Width="5%" ItemStyle-CssClass="gItemStyleNum">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Name" HeaderText="Candidate name" HeaderStyle-Width="25%">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="gItemStyle"></ItemStyle>
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Center_Name" HeaderText="Centre name" HeaderStyle-Width="25%"
                                ItemStyle-Width="15%" ItemStyle-CssClass="gItemStyle"></asp:BoundColumn>
                            <asp:BoundColumn DataField="course_name" HeaderText="Course name" HeaderStyle-Width="25%"
                                ItemStyle-Width="15%" ItemStyle-CssClass="gItemStyle"></asp:BoundColumn>
                            <asp:BoundColumn DataField="test_name" HeaderText="Subject name" HeaderStyle-Width="20%"
                                ItemStyle-Width="15%" ItemStyle-CssClass="gItemStyle"></asp:BoundColumn>
                            <%--<asp:BoundColumn DataField="Center_Name" HeaderText="Centre Name" ItemStyle-Width="20%"
                                                ItemStyle-CssClass="gItemStyle" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="Email" HeaderText="Email" ItemStyle-Width="23%" ItemStyle-CssClass="gItemStyle" Visible="False">
                                            </asp:BoundColumn>--%>
                        </Columns>
                        <HeaderStyle CssClass="ghead"></HeaderStyle>
                    </asp:DataGrid>
                    <table align="left">
                        <tr>
                            <%-- Display the First Page/Previous Page buttons --%>
                            <td>
                                <%--<asp:LinkButton ID="Firstbutton" Text="First" CommandArgument="0" runat="server" OnClick="PagerButtonClick" />--%>
                                <asp:ImageButton ID="imgfirst" runat="server" Height="15px" ImageUrl="images/first1.gif"
                                    Width="10px" CommandArgument="0" ToolTip="First" OnClick="PagerButtonClick" />
                            </td>
                            <td>
                                <%--<asp:LinkButton ID="Prevbutton" Text="Prev" CommandArgument="prev" runat="server" OnClick="PagerButtonClick" />--%>
                                <asp:ImageButton ID="imgprev" runat="server" Height="15px" ImageUrl="images/prev1.gif"
                                    Width="10px" CommandArgument="prev" ToolTip="Previous" OnClick="PagerButtonClick" />
                            </td>
                            <%-- Display the Page No. buttons --%>
                            <td>
                                <table id="tblPagebuttons" runat="server">
                                    <tr>
                                    </tr>
                                </table>
                            </td>
                            <%-- Display the Next Page/Last Page buttons --%>
                            <td>
                                <asp:ImageButton ID="imgnext" runat="server" Height="15px" ImageUrl="images/next1.gif"
                                    Width="10px" CommandArgument="next" ToolTip="Next" OnClick="PagerButtonClick" /><%--<asp:LinkButton ID="Nextbutton" Text="Next" CommandArgument="next" runat="server" OnClick="PagerButtonClick" />--%>
                            </td>
                            <td>
                                <%--<asp:LinkButton ID="Lastbutton" Text="Last" CommandArgument="last" runat="server"
                                        OnClick="PagerButtonClick" />--%><asp:ImageButton ID="imglast" runat="server" Height="15px"
                                            ImageUrl="images/last1.gif" Width="10px" CommandArgument="last" ToolTip="Last"
                                            OnClick="PagerButtonClick" />
                            </td>
                            <td>
                                &nbsp;&nbsp;<span style="color: #526B94"> Go to page no.</span>
                                <asp:DropDownList ID="ddlPages" AutoPostBack="true" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:ImageButton ID="btnBack" runat="server" ToolTip="Back" AlternateText="Back"
                    ImageUrl="images/BtnBack.jpg" CausesValidation="False"></asp:ImageButton>
            </td>
        </tr>
    </table>
    </form>
</asp:Content>
