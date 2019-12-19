Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.HttpRequest
Imports System.Configuration
Imports System.Web.Security
Imports System.Web
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls



Namespace unirecruite

    Partial Class login
        Inherits System.Web.UI.Page
        Protected WithEvents Form1 As System.Web.UI.HtmlControls.HtmlForm
        Protected WithEvents ResultRow1 As System.Web.UI.HtmlControls.HtmlTableRow
        Protected WithEvents ResultCol1 As System.Web.UI.HtmlControls.HtmlTableCell
        Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink

        'Protected WithEvents ResultRow As System.Web.UI.HtmlControls.HtmlTableRow
        'Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink
        'Protected WithEvents ResultCol As System.Web.UI.HtmlControls.HtmlTableCell
        'Private Shared C_PROFILE_PATH = "C:\Unirecruite\Files\Unikaihatsu Software Pvt. Ltd._files\frame.htm"
        'Private Shared C_PROFILE_PATH = "CompanyProfile_files\frame.htm"
        'Private Shared C_PROFILE_PATH = "CompanyProfile_files\UKS_Campus_PPT_2009.mht"
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Login")

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region
        Private Sub set_session()
            'set the userid in session for reference
        End Sub
        '*************On Error Go to Error Page****************
        Private Sub onErrors(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Error
            Dim Err As New CreateLog
            Err.ErrorLog(Server.MapPath("Logs/RMS"), Server.GetLastError().ToString().Trim, "login.aspx.vb")
            Response.Redirect("error.aspx?err=Error On Page Login")
        End Sub



        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            Try

                'If Not IsPostBack Then
                '    Getit()
                'End If
                'Put user code to initialize the page here 
                Session.Remove("LoginGenuine")

                ' btn_login.Attributes.Add("onclick", "validation();")

                If Not IsPostBack Then
                    Session.RemoveAll()
                    Session.Add("UserNew", True)

                End If

                'Code comment by Bhasker(01-12-2009)
                '************ Start **********************
                'code added by Abdur Rasheed
                'Added a Hyperlink on Login page which opens .ppt file about Company Details
                'ResultRow1 = New HtmlTableRow
                'ResultCol1 = New HtmlTableCell
                'HyperLink1 = New HyperLink
                'HyperLink1.NavigateUrl = C_PROFILE_PATH
                'HyperLink1.Text = "Click here to view Presentation on Company Profile"
                ''HyperLink1.Font.Size = System.Web.UI.WebControls.FontUnit.Smaller
                'ResultCol1.Controls.Add(HyperLink1)
                'ResultRow1.Cells.Add(ResultCol1)
                'ResultRow1.Align = "center"
                'tblLink.Rows.Add(ResultRow1)
                'End of code as on 2006/07/21
                '************ End **********************

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
        Protected Sub Btn_login_Click(sender As Object, e As EventArgs)
            Session("UserNew") = False
            Dim sqlstr As String
            Dim userid As String
            Dim usertype As String
            Dim MyCommand As SqlCommand
            Dim MyDataReader As SqlDataReader
            Dim objconn As New ConnectDb
            Dim kishor As Array
            Dim strReDirect As String

            '*****Monal shah*******
            Dim WrittenTestdate As String
            Dim FromTestdateValue As Integer
            Dim ToTestdateValue As Integer
            Dim TestTime As String
            Dim FromStartDate As String
            Dim ToEndDate As String
            Dim strArray() As String
            Dim formatedTime As String
            Dim CurrentDateTimeValue As String
            Dim flgValue As Boolean
            Dim flgAppear As Integer
            Dim flgAppearcount As Integer
            Dim flgAppeararrlst As New ArrayList
            'Dim flgAppeartotalvaluearr() As Integer
            Dim testtype As Integer
            Dim testtypecount As Integer
            Dim testtypearrlst As New ArrayList
            'Dim testtotalvaluearr() As Integer
            Dim CONS As New unirecruite.Errconstants
            Try

                'Dim strPathDb As String

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                If txt_login.Value <> "" And txt_pwd.Value <> "" Then
                    If objconn.connect() = True Then
                        sqlstr = "SELECT userid,loginname,user_type from m_user_info"
                        'added by bhumi [5/10/2015]
                        'Reason: Delete_Flag checks that Student is active or not
                        sqlstr = sqlstr & " WHERE Delete_Flg=0 AND loginname= '" & txt_login.Value & "'"
                        'Ended by bhumi
                        sqlstr = sqlstr & " AND pwd= '" & txt_pwd.Value & "'"
                        'Response.Write(sqlstr)
                        MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        MyDataReader = MyCommand.ExecuteReader()
                        If MyDataReader.Read() Then

                            'Code added by kamal on 2006/02/13
                            userid = MyDataReader.Item("userid")
                            Session.Add("userid", userid)
                            usertype = MyDataReader.Item("user_type")

                            'If Not IsNothing(MyDataReader.Item("userid")) Then
                            '    If userid = 1 Then
                            '        Session.Add("loginname", MyDataReader.Item("loginname"))
                            '        Session.Add("IsGenuineUser", True)
                            '        Session.Add("LoginGenuine", True)
                            '        'Response.Redirect("admin.aspx")
                            '    Else
                            '        Session.Add("userid", MyDataReader.Item("userid"))
                            '        Session.Add("LoginGenuine", True)
                            '        'Response.Redirect("register.aspx")
                            '    End If
                            '    'Response.Write("<br>" & Session.Item("userid") & "<br>")
                            '    'Response.Write("<br>" & "session value:" & Session.Item("userid") & "<br>")
                            'End If

                            Session("UserName") = MyDataReader.Item("loginname")
                            Session("UniUserType") = usertype

                            If Not IsNothing(MyDataReader.Item("user_type")) Then
                                'Commented & Added By Vaibhav Soni
                                'Date : 2014/03/13
                                '-If usertype = 1 Then
                                Session.Add("loginname", MyDataReader.Item("loginname"))
                                Session.Add("IsGenuineUser", True)
                                Session.Add("LoginGenuine", True)
                                Session.Add("adminid", MyDataReader.Item("userid"))

                                'Response.Redirect("admin.aspx")
                                '-Else
                                '-Session.Add("userid", MyDataReader.Item("userid"))
                                '-Session.Add("LoginGenuine", True)

                                'sqlstr = "SELECT key1,key2,value from m_system_settings where "
                                'sqlstr = sqlstr & " WHERE loginname= '" & txt_login.Text & "'"
                                'sqlstr = sqlstr & " AND pwd= '" & txt_pwd.Text & "'"
                                'Response.Write(sqlstr)
                                'MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                'MyDataReader = MyCommand.ExecuteReader()

                                'Response.Redirect("register.aspx")
                                '-End If
                                'Response.Write("<br>" & Session.Item("userid") & "<br>")
                                'Response.Write("<br>" & "session value:" & Session.Item("userid") & "<br>")
                            End If

                            FormsAuthentication.Initialize()
                            Dim Ticket As New FormsAuthenticationTicket(1, txt_login.Value, DateTime.Now, DateTime.Now.AddMinutes(20), False, "", FormsAuthentication.FormsCookiePath)
                            Dim strHash As String

                            strHash = FormsAuthentication.Encrypt(Ticket)
                            Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, strHash)

                            If Ticket.IsPersistent Then
                                cookie.Expires = Ticket.Expiration
                            End If

                            Response.Cookies.Add(cookie)

                            strReDirect = Request("ReturnURL")
                            If Not strReDirect = "" Then
                                Response.Redirect(strReDirect, False)
                            Else
                                If usertype = 1 Then
                                    strReDirect = "admin.aspx"
                                    Response.Redirect(strReDirect, False)

                                    'Added By  : Pragnesha Kulkarni 
                                    'Date        : 24/07/18
                                    'Description: It checks the usertype for SuperAdmin.
                                    'Bug ID      : 670  
                                    '-----------------------------------------------------------------------------------
                                ElseIf usertype = 2 Then
                                    strReDirect = "admin.aspx"
                                    Session("UserName") = MyDataReader.Item("loginname")
                                    Response.Redirect(strReDirect, False)
                                    '----------------------------------------------------------------------------------
                                Else
                                    '******************Monal shah****************************
                                    'For Checkind Date Validation For student Appear in the the exam in given date and Time or not

                                    'Commented & Added By Vaibhav Soni
                                    'Date :- 2014/03/13
                                    Response.Redirect("StudHome.aspx", False)

                                    'If objconn.connect(strPathDb) = True Then
                                    '    If objconn.connect(strPathDb) = True Then

                                    '        'sqlstr = "SELECT written_test_date,appearedflag as testtypeTotal FROM t_candidate_status Where userid='" & userid & "'"
                                    '        sqlstr = "SELECT written_test_date FROM t_candidate_status Where userid='" & userid & "'"
                                    '        MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                    '        MyDataReader = MyCommand.ExecuteReader()
                                    '        If MyDataReader.Read() Then
                                    '            WrittenTestdate = MyDataReader.Item("written_test_date")
                                    '            'flgAppear = MyDataReader.Item("appearedflag")
                                    '        End If
                                    '        MyDataReader.Close()
                                    '        sqlstr = "Select Count(test_type) as testtype,Count(appearedflag) as appearedflagcount FROM t_candidate_status Where userid='" & userid & "'"
                                    '        MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                    '        MyDataReader = MyCommand.ExecuteReader()
                                    '        If MyDataReader.Read() Then
                                    '            testtypecount = MyDataReader.Item("testtype")
                                    '            flgAppearcount = MyDataReader.Item("appearedflagcount")
                                    '        End If
                                    '        MyDataReader.Close()

                                    '        sqlstr = "SELECT test_type,appearedflag FROM t_candidate_status Where userid='" & userid & "'"
                                    '        MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                    '        MyDataReader = MyCommand.ExecuteReader()
                                    '        Dim flgAppeartotalvaluearr(flgAppearcount) As Integer
                                    '        Dim testtotalvaluearr(testtypecount) As Integer
                                    '        While MyDataReader.Read()
                                    '            testtypearrlst.Add(Integer.Parse(MyDataReader.Item("test_type")))
                                    '            flgAppeararrlst.Add(MyDataReader.Item("appearedflag"))
                                    '        End While
                                    '        testtotalvaluearr = testtypearrlst.ToArray(System.Type.GetType("System.Int32"))
                                    '        flgAppeartotalvaluearr = flgAppeararrlst.ToArray(System.Type.GetType("System.Int32"))
                                    '        Session.Add("TotalTestName", testtotalvaluearr)
                                    '        Session.Add("TotalFlagAppearValue", flgAppeartotalvaluearr)

                                    '        MyDataReader.Close()
                                    '        Dim totaltesttypevalue As Integer = 0
                                    '        Dim totalflgAppearvalue As Integer = 0
                                    '        For totaltesttypevalue = 0 To testtotalvaluearr.Length - 1
                                    '            For totalflgAppearvalue = 0 To flgAppeartotalvaluearr.Length - 1
                                    '                If flgAppeartotalvaluearr(totalflgAppearvalue) = "0" Or flgAppeartotalvaluearr(totalflgAppearvalue) = "1" Then
                                    '                    '************get FromDate Value*************
                                    '                    FromTestdateValue = getFromAndStartDateGap("m_system_settings", "Exam from date", "from date")
                                    '                    Dim adddate = CDate(WrittenTestdate)
                                    '                    FromStartDate = DateAdd(DateInterval.Day, FromTestdateValue, adddate)
                                    '                    '*************get ToTestdateValue*************
                                    '                    ToTestdateValue = getFromAndStartDateGap("m_system_settings", "Exam Within date", "Within date")
                                    '                    Dim FromStartToEndDate = CDate(FromStartDate)
                                    '                    ToEndDate = DateAdd(DateInterval.Day, ToTestdateValue, FromStartToEndDate)
                                    '                    '*************To get Time Value**************
                                    '                    sqlstr = "SELECT value FROM m_system_settings Where key1='exam time' and key2='start and end time'"
                                    '                    MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                    '                    MyDataReader = MyCommand.ExecuteReader()
                                    '                    If MyDataReader.Read() Then
                                    '                        TestTime = MyDataReader.Item("value")
                                    '                        strArray = TestTime.Split(",")
                                    '                        Dim i As Integer = 0
                                    '                        For i = 0 To strArray.Length - 1
                                    '                            formatedTime = FormatDate(strArray(i))
                                    '                            strArray(i) = formatedTime
                                    '                        Next
                                    '                    End If
                                    '                    MyDataReader.Close()
                                    '                    '******************Check For Valid Date****************
                                    '                    If FromStartDate <= DateTime.Now.ToShortDateString() And ToEndDate >= DateTime.Now.ToShortDateString() Then
                                    '                        Dim i As Integer = 0
                                    '                        '************************Check For Time**************
                                    '                        For i = 0 To strArray.Length - 1
                                    '                            ' If (strArray(0).Substring(strArray(0).Length - 2, 2).ToString() = "AM") Then
                                    '                            'If DateTime.Now.ToShortTimeString() >= strArray(0) Or DateTime.Now.ToShortTimeString() < strArray(1) Then
                                    '                            '    flgValue = True
                                    '                            'End If
                                    '                            Dim currentdatetime As String = DateTime.Now.ToShortTimeString()
                                    '                            If currentdatetime.Substring(currentdatetime.Length - 2, 2).ToString() = "AM" Then
                                    '                                If (currentdatetime.Substring(0, 2).ToString() = "12") Then
                                    '                                    If currentdatetime < strArray(0) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(0).Length = "7" And currentdatetime.Length = "7") Then
                                    '                                    If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) >= strArray(0).Concat("0", strArray(0).Substring(0, 1).ToString()) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(0).Length = "7" And currentdatetime.Length = "8") Then
                                    '                                    If currentdatetime >= strArray(0).Concat("0", strArray(0).Substring(0, 1).ToString()) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(0).Length = "8" And currentdatetime.Length = "7") Then
                                    '                                    If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) >= strArray(0) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(0).Length = "8" And currentdatetime.Length = "8") Then
                                    '                                    If currentdatetime >= strArray(0) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                End If

                                    '                                ' End If
                                    '                                'End If

                                    '                                'If currentdatetime >= strArray(0).Concat("0", strArray(0).Substring(0, 1).ToString()) Then
                                    '                                '    flgValue = True
                                    '                                'End If
                                    '                            ElseIf currentdatetime.Substring(currentdatetime.Length - 2, 2).ToString() = "PM" Then
                                    '                                If (currentdatetime.Substring(0, 2).ToString() = "12") Then
                                    '                                    If currentdatetime > strArray(1).Concat("0", strArray(1).Substring(0, 1).ToString()) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(1).Length = "7" And currentdatetime.Length = "7") Then
                                    '                                    If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) < strArray(1).Concat("0", strArray(1).Substring(0, 1).ToString()) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(1).Length = "7" And currentdatetime.Length = "8") Then
                                    '                                    If currentdatetime < strArray(1).Concat("0", strArray(1).Substring(0, 1).ToString()) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(1).Length = "8" And currentdatetime.Length = "7") Then
                                    '                                    If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) < strArray(1) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                ElseIf (strArray(1).Length = "8" And currentdatetime.Length = "8") Then
                                    '                                    If currentdatetime < strArray(1) Then
                                    '                                        flgValue = True
                                    '                                    End If
                                    '                                End If

                                    '                                ' End If

                                    '                                'If currentdatetime < strArray(1) Then
                                    '                                '    flgValue = True
                                    '                                'End If
                                    '                                'ElseIf DateTime.Now.ToShortTimeString() < strArray(1) Then
                                    '                                '    flgValue = True
                                    '                            End If
                                    '                        Next
                                    '                        If flgValue = True Then
                                    '                            'sqlstr = "Update t_candidate_status SET written_test_appear_date='" & DateTime.Now & "',appearedflag='1' where userid='" & userid & "'"
                                    '                            'MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                    '                            'MyCommand.ExecuteNonQuery()
                                    '                            strReDirect = "register.aspx"
                                    '                            Response.Redirect(strReDirect, False)
                                    '                        ElseIf flgValue = False Then
                                    '                            lbl_saved.Visible = True
                                    '                            lbl_saved.Text = "Your exam time is passed. Give exam during " & strArray(0) & " to " & strArray(1) & " ."
                                    '                        End If
                                    '                    Else
                                    '                        lbl_saved.Visible = True
                                    '                        lbl_saved.Text = "Your Exam date is passed. Please contact to administrator"
                                    '                    End If
                                    '                    'End If
                                    '                Else
                                    '                    lbl_saved.Visible = True
                                    '                    lbl_saved.Text = "You already given the exam"
                                    '                End If
                                    '            Next totalflgAppearvalue
                                    '        Next totaltesttypevalue
                                    '    End If
                                    'End If
                                    'Ended By Vaibhav Soni

                                End If

                            End If

                            'End of Kamal on 2006/02/13

                            'Commented by kamal on 2006/02/13
                            'userid = MyDataReader.Item("userid")
                            'If Not IsNothing(MyDataReader.Item("userid")) Then
                            '    If userid = 1 Then
                            '        Session.Add("loginname", MyDataReader.Item("loginname"))
                            '        Session.Add("IsGenuineUser", True)
                            '        Session.Add("LoginGenuine", True)
                            '        Response.Redirect("admin.aspx")
                            '    Else
                            '        Session.Add("userid", MyDataReader.Item("userid"))
                            '        Session.Add("LoginGenuine", True)
                            '        Response.Redirect("register.aspx")
                            '    End If
                            '    'Response.Write("<br>" & Session.Item("userid") & "<br>")
                            '    'Response.Write("<br>" & "session value:" & Session.Item("userid") & "<br>")
                            'End If
                            'End of comment by kamal on 2006/02/13
                        Else

                            Session.Add("userid", "err")
                            'Session.CopyTo(kishor, 1)
                            'Response.Write("aaaa:" & kishor(0))
                            'Response.Write("<br>" & Session.Item("userid") & "<br>")
                            'Response.Write("session value:" & Session.Item("userid"))
                        End If
                        '*****************************Monal shah**********************************
                        'For Checkind Date Validation For student Appear in the the exam in given date and Time or not

                        'Dim YrDt() As String = Split(txt_dob.Text, "/")
                        'Dim VldDate As Boolean = ValidateDate(YrDt)
                        ''VldDate = ValidateDate(YrDt)
                        'Dim C_HOMEPAGEURL As String
                        'Dim PIYrDt() As String = Split(txt_dopi.Text, "/")

                        '*****************************End*****************************************
                        MyDataReader.Close()
                        MyCommand.Dispose()
                        objconn.disconnect()

                    Else
                        'Response.Redirect("error.aspx?err=login" & CONS.ERR_DBCON)
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                sqlstr = Nothing
                WrittenTestdate = Nothing
                userid = Nothing
                usertype = Nothing
                MyCommand = Nothing
                objconn = Nothing
                kishor = Nothing
                MyDataReader = Nothing
                TestTime = Nothing
                strReDirect = Nothing
                FromStartDate = Nothing
                ToEndDate = Nothing
                strArray = Nothing
                formatedTime = Nothing
                CurrentDateTimeValue = Nothing
                testtypearrlst = Nothing
                flgAppeararrlst = Nothing
            End Try
        End Sub

        'Private Sub lnk_newuser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnk_newuser.Click
        '    Session.Add("userid", "")
        '    Response.Redirect("register.aspx")
        'End Sub

        Public Function getFromAndStartDateGap(ByVal TableName As String, ByVal Field1 As String, ByVal Field2 As String) As Integer
            Dim sqlstr As String
            Dim MyCommand As SqlCommand
            Dim MyDataReader As SqlDataReader
            Dim objconn As New ConnectDb

            '*****Monal shah*******
            Dim TestdateValue As Integer
            'Dim strPathDb As String
            Try

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() = True Then
                    If objconn.connect() = True Then
                        sqlstr = "SELECT value FROM " & TableName & " Where key1='" & Field1 & "' and key2='" & Field2 & "'"
                        MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        MyDataReader = MyCommand.ExecuteReader()
                        If MyDataReader.Read() Then
                            TestdateValue = CInt(MyDataReader.Item("value"))
                        End If
                        MyDataReader.Close()
                        Return TestdateValue
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                MyCommand.Dispose()
                objconn.disconnect()
                objconn = Nothing
                MyDataReader = Nothing
                sqlstr = Nothing
            End Try
        End Function

        Public Function FormatDate(ByVal s As String) As String
            Try
                '' this function takes a time string from the database
                '' and changes it from 24h to 12h

                Dim oSplit As String() = s.Split(":"c)
                Dim oRet As String = "", suffix As String = ""
                Dim hour, minute As String

                If CInt(oSplit(0)) > 12 Then
                    hour = CStr(CInt(oSplit(0)) - 12)
                    suffix = "PM"
                Else
                    hour = oSplit(0)
                    suffix = "AM"
                End If
                'minute = oSplit(1)
                minute = "00"
                oRet = String.Format("{0}:{1} {2}", hour, minute, suffix)
                Return oRet
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function

        Private Sub btn_login_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_login.Load

        End Sub

    End Class

End Namespace