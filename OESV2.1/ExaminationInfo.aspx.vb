Imports System.Data
Imports System.Data.SqlClient
Partial Public Class ExaminationInfo1
    Inherits System.Web.UI.Page
    Protected WithEvents btnStart As System.Web.UI.WebControls.Button
    Const ENCRYPT_DELIMIT = "h"
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Const ENCRYPT_KEY = 124

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("Status") <> Nothing Then
            'LblError.Text = Request.QueryString(0)
            imgBtnStartExamination.Enabled = False
        Else
            'LblError.Text = ""
            'GetQuePerPage()
            GetTime()
            imgBtnStartExamination.Enabled = True
        End If
    End Sub
    Protected Sub imgBtnStartExamination_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnStartExamination.Click
        Dim urlName As String
        urlName = Request.QueryString("lnks")
        Response.Redirect(urlName)
    End Sub
    Public Function GetQuePerPage() As String
        Dim sqlstr As String
        Dim myCommand As SqlCommand
        Dim myDataReader As SqlDataReader
        Dim objconn As New ConnectDb
        'Dim strPathDb As String
        'Dim strQuePerPage As String

        Try
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                sqlstr = "select value from m_system_settings where key1 = 'que_paper_page' and key2 = 'que_per_page'"
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()

                While myDataReader.Read
                    If Not IsDBNull(myDataReader.Item("value")) Then
                        GetQuePerPage = myDataReader.Item("value")
                    End If
                End While

                myCommand.Dispose()
                myDataReader.Close()
            End If
        Catch ex As Exception
            Response.Redirect("error.aspx", False)
        Finally
            objconn = Nothing
            myCommand = Nothing
            myDataReader = Nothing
        End Try
    End Function

    Public Function GetTime() As Integer
        Dim aUsr As Array
        'Dim ArrTemp() As String
        Dim userid As String
        Dim testType As String
        'Dim noOfDays As Long
        Dim objconn As New ConnectDb
        Dim sqlstr, strCheck As String
        Dim myCommand As SqlCommand
        Dim myDataReader As SqlDataReader
        Dim sUsr As String
        'Dim strPathDb As String
        Dim intCheck As Integer
        If Request.QueryString("Status") = Nothing Then


            sUsr = Request.QueryString("lnks")
            aUsr = Split(sUsr, "=")
            'sUsr = "hb2hf8hbf"
            'ArrTemp = Split(sUsr, "&")
            sUsr = sDecodeString(aUsr(1))
            'Response.Write(sUsr)

            If sUsr(0) = "|" Then
                sUsr = sUsr.Substring(1, sUsr.Length - 1)
            End If
            strCheck = ""
            For intCheck = 0 To sUsr.Length - 1
                If sUsr(intCheck) = "|" Then
                    strCheck += sUsr(intCheck)
                    While sUsr(intCheck + 1) = "|"
                        intCheck = intCheck + 1
                    End While
                Else
                    strCheck += sUsr(intCheck)
                End If
            Next
            aUsr = Split(strCheck, "|")
            userid = Convert.ToInt32(aUsr(0))
            testType = aUsr(1)


            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                'sqlstr = "SELECT time FROM m_testinfo"
                'sqlstr = sqlstr & " Where test_type= '" & testType & "'"
                sqlstr = "SELECT total_time FROM M_Course"
                sqlstr = sqlstr & " Where Course_ID= '" & testType & "' and del_flag='0'"
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()
                While myDataReader.Read
                    'Session.Add("TestXX", myDataReader.Item("time"))
                    'GetTime = myDataReader.Item("time")
                    Session.Add("TestXX", myDataReader.Item("total_time"))
                    GetTime = myDataReader.Item("total_time")
                End While
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()
            End If
        End If
    End Function
    Function sDecodeString(ByVal sText As String) As String
        Dim iCnt
        Dim sChar
        Dim sDecode

        sDecode = ""

        sChar = Split(sText, ENCRYPT_DELIMIT)
        For iCnt = 1 To UBound(sChar)
            sDecode = sDecode & Chr(CLng("&H" & sChar(iCnt)) - ENCRYPT_KEY)
        Next
        sDecodeString = sDecode
    End Function
End Class