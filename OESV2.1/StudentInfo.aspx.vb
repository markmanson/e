Imports System.Data.SqlClient
Imports System.Configuration
Imports log4net
Partial Public Class StudentInfo
    Inherits System.Web.UI.Page
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Studentinfo")
    Const ENCRYPT_DELIMIT = "h"
    Const ENCRYPT_KEY = 124
    Dim objconn As New ConnectDb
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim C_HOMEPAGEURL As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '  StudentImage.ImageUrl = "StudentImageHandler.ashx?id=" & Request.QueryString("userid")
        Dim introllnumber As Integer
        Dim strprefix, strquery As String
        Dim cmd As SqlCommand
        Dim courseid As String = ""
        Dim centername As String = ""
        Dim dob As String = ""

        C_HOMEPAGEURL = ConfigurationManager.AppSettings("C_HOMEPAGEURL")

        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim sb As StringBuilder
        Try
            ds = New DataSet
            sb = New StringBuilder
            '            sb.Append(" select Course_ID,LoginName,Pwd from T_Candidate_status where userid=276  and LoginName='Asad276' and Pwd='4e33087322'")
            'sb.Append(" select Course_ID,LoginName,Pwd from T_Candidate_status where userid=" & Session("Userid").ToString & "  and LoginName='" & Session("login").ToString & "' and Pwd='" & Session("pwd").ToString & "'")
            sb.Append(" Select tcs.UserId,tcs.Course_ID,tcs.LoginName,tcs.Pwd,mc.course_name,mui.Center_ID,mce.Center_name,mui.Name+' '+mui.SurName as Name, ")
            sb.Append(" convert(varchar(10),mui.Birthdate,101) as Birthdate from T_Candidate_Status as tcs LEFT JOIN m_course as mc ")
            sb.Append(" on mc.course_id=tcs.course_id LEFT JOIN M_USER_INFO as mui on mui.Userid=tcs.UserId LEFT JOIN M_Centers as mce ")
            sb.Append(" on mce.Center_ID=mui.Center_ID where tcs.LoginName='" & Session("login").ToString & "' and tcs.Pwd='" & Session("pwd").ToString & "' ")


            strquery = sb.ToString()
            If objconn.connect() = True Then
                da = New SqlDataAdapter(sb.ToString, objconn.MyConnection)
                da.Fill(ds)
                lblCenterName.Text = ds.Tables(0).Rows(0).Item(6).ToString
                '  lblDOB.Text = Convert.ToDateTime(ds.Tables(0).Rows(0).Item(8).ToString()).ToString("dd/MM/yyyy")
                lblDOB.Text = ds.Tables(0).Rows(0).Item(8).ToString()
                lblName.Text = ds.Tables(0).Rows(0).Item(7).ToString
                Session("StdName") = ds.Tables(0).Rows(0).Item(7).ToString
                StudentImage.ImageUrl = "StudentImageHandler.ashx?id=" & Session("Userid").ToString


                Dim newlink As String = ""
                C_HOMEPAGEURL = ""

                newlink = C_HOMEPAGEURL & "ExaminationInfo.aspx?lnks=" & C_HOMEPAGEURL & "QuestionPaper.aspx?usr=" _
                & sEncodeString(Request.QueryString("userid") & "|" & Session.Item("userid") & "|" & _
                ds.Tables(0).Rows(0).Item(1).ToString)

                ViewState.Add("redirectUrl", newlink)
                objconn.disconnect()
            End If

             

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            'Throw ex
            Response.Redirect("error.aspx?err=" & ex.Message, False)
        Finally
            introllnumber = Nothing
            strprefix = Nothing
            strquery = Nothing
            cmd = Nothing
            objconn = Nothing
            da = Nothing
            ds = Nothing
            sb = Nothing
        End Try


    End Sub



  
#Region "sEncodeString"
    Function sEncodeString(ByVal sText As String) As String
        Dim iCnt
        Dim sChar
        Dim sEncode
        Try
            sEncode = ""
            For iCnt = 1 To Len(sText)
                sChar = Mid(sText, iCnt, 1)
                sEncode = sEncode & ENCRYPT_DELIMIT & LCase(CStr(Hex(Asc(sChar) + ENCRYPT_KEY)))
            Next
            sEncodeString = sEncode
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            ' lblMsg.Text = ex.Message()
            Response.Redirect("error.aspx?err=" & ex.Message, False)
        Finally
            iCnt = Nothing
            sChar = Nothing
            sEncode = Nothing
        End Try
    End Function
#End Region

    Protected Sub lnkbtnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkbtnNext.Click
        Response.Redirect(ViewState("redirectUrl").ToString, False)
    End Sub
End Class