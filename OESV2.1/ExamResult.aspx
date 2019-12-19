<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ExamResult.aspx.vb" Inherits="Unirecruite.ExamResult" 
    title="Exam Result" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<table style="font-size:14px;font-family:Calibri" width="100%">


<tr>
<td style="text-align:left" colspan="2">
Exam Result:
</td>
</tr>
<tr>
<td style="text-align:left;width:20%">
<b>Candidate Name:</b>
</td>
<td style="text-align:left;width:80%">
<asp:Label runat="server" ID="LblName"></asp:Label>
</td>
</tr>
<tr>
<td style="text-align:left;width:20%">
<b>Class Name:</b>
</td>
<td style="text-align:left;width:80%">
<asp:Label runat="server" ID="LblClassName"></asp:Label>
</td>
</tr>
<tr>
<td style="text-align:left">
<b>Test Name:</b>
</td>
<td style="text-align:left">
<asp:Label runat="server" ID="LblTestName"></asp:Label>
</td>
</tr>
<tr>
<td style="text-align:left; ">
<b>Exam appeared date</b>
</td>
<td style="text-align:left;">
<asp:Label runat="server" ID="LblDate"></asp:Label>
</td>
</tr>
<tr>
<td style="text-align:left; height: 21px;">
<b>Total Marks:</b>
</td>
<td style="text-align:left; height: 21px;">
<asp:Label runat="server" ID="LblTotalMarks"></asp:Label>
</td>
</tr>
<tr>
<td style="text-align:left">
<b>Marks obtained:</b>
</td>
<td style="text-align:left">
<asp:Label runat="server" ID="LblMarksObtained"></asp:Label>
</td>
</tr>
<tr>
<td style="text-align:left">
<b>Status:</b>
</td>
<td style="text-align:left">
<asp:Label runat="server" ID="LblStatus"></asp:Label>
</td>
</tr>

<tr>
<td colspan="2" style="text-align:center">
<a href="login.aspx" onclick="javascript.window.close();">Close</a>
</td>
</tr>
</table>

</asp:Content>
