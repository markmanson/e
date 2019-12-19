<%@ Page Language="vb" AutoEventWireup="false" Inherits="Unirecruite.unirecruite.RptPrint"
    CodeBehind="RptPrint.aspx.vb" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>Online Examinations solution------Report Print</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">

    <script language="javascript">
			function printContent() {
		    if (window.print) {
////				document.Form1.BtnPrint.style.display = 'none';
////				document.Form1.BtnBack.style.display = 'none';
                   window.print();
                   
			}
			 else 
			{
					alert("Your Browser doesn't support printing")
				}
			}
    </script>

    <style type="text/css">
        .entryHeader
        {
            font-size: 50px;
            font-family: Arial, Helvetica, sans-serif;
            color: Black;
        }
        .form
        {
            font-size: 15px;
            text-align: center;
        }
        .WrapperDiv
        {
            width: 800px;
            border: 1px solid black;
            font-size: 15px;
            font-family: Arial, Helvetica, sans-serif;
            color: Black;
            position: relative;
        }
    </style>
</head>
<body class="form">
    <form id="Form1" method="post" runat="server">
    <font face="MS UI Gothic">&nbsp;<br />
        <br />
        <br />
        <br />
        <br />
    </font>
    <table align="center">
        <tr>
            <td align="center" style="width: 100px">
                <asp:Label ID="LblCandStatus" runat="server" Width="428px" Font-Italic="True" CssClass="entryHeader"
                    Font-Underline="True">
						Candidate Status
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <input id="BtnPrint" style="width: 104px; height: 24px" type="button" value="Print"
                    onclick="return printContent()" name="BtnPrint" runat="server"><input id="BtnBack"
                        style="width: 104px; height: 24px" type="button" value="Back" name="BtnBack"
                        runat="server">
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 100px">
                <asp:DataGrid ID="DgPrnReport" runat="server" CssClass="WrapperDiv" BorderColor="Black"
                    BorderStyle="Solid" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundColumn DataField="UserName" HeaderText="Candidate Name">
                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Course_name" HeaderText="Course Name">
                            <HeaderStyle HorizontalAlign="Center" Width="30%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="WrittenTestDate" HeaderText="Written Test Date" DataFormatString="{0:dd/MM/yyyy}">
                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="AppearanceDate" HeaderText="Appeared Date" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}">
                            <HeaderStyle HorizontalAlign="Center" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Grad" HeaderText="Grade">
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        </asp:BoundColumn>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
    </table>
    &nbsp;
    </form>
</body>
</html>
