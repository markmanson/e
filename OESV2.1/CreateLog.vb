Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Mail
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Web.UI
Imports System.Resources
Imports System.Threading
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Globalization


Imports System.Configuration


Namespace unirecruite

Public Class CreateLog
    Private strLogFormat As String
    Private strErrorTime As String

    Public Sub New()
        strLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + "  "
        'Commented by kamal on 25/01/2006
        'strErrorTime = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString()
        strErrorTime = DateTime.Now.ToString("yyyyMMdd")
    End Sub

    Public Sub ErrorLog(ByVal strPath As String, ByVal strErrMsg As String, ByVal strScrName As String)
        Dim sw As New StreamWriter(strPath + strErrorTime, True)

        sw.WriteLine(strLogFormat + "  " + strScrName + "  " + strErrMsg)
        sw.Flush()
        sw.Close()
    End Sub
End Class

End Namespace
