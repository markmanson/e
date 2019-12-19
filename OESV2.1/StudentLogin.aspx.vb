Imports System.Data
Imports System.Data.SqlClient
Partial Public Class StudentLogin
    Inherits System.Web.UI.Page
    Dim strPathDb As String
    Dim sqlstr As String
    Dim userid As String
    Dim usertype As String
    Dim strReDirect As String
    Dim MyCommand As SqlCommand
    Dim MyDataReader As SqlDataReader
    Dim objconn As New ConnectDb
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
    Dim testtype As Integer
    Dim testtypecount As Integer
    Dim testtypearrlst As New ArrayList
    Dim CONS As New unirecruite.Errconstants
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Studentlogin")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'txtStreamCourse.Text = Request.QueryString("CourseValue")
        'CheckQuestions("177")
        imgBtnSubmit.Attributes.Add("onclick", "validation();")
        If Not IsPostBack Then
            Session.RemoveAll()
            Session.Add("UserNew", True)
            If Not Request.QueryString("ID") Is Nothing Then
                If Request.QueryString("ID").ToString().Length > 0 Then
                    txt_login.Text = Request.QueryString("ID").ToString()
                End If
                If Request.QueryString("PWD").ToString().Length > 0 Then
                    txt_pwd.TextMode = TextBoxMode.SingleLine
                    txt_pwd.Text = Request.QueryString("PWD").ToString()
                    txt_pwd.TextMode = TextBoxMode.Password
                    txt_pwd.Attributes.Add("value", Request.QueryString("PWD").ToString())
                End If
            End If
        End If
        'If Not Request.QueryString("PWD").ToString().Length > 0 Then
        '    txt_pwd.Attributes.Add("value", Request.QueryString("PWD").ToString())
        'End If

    End Sub
    Protected Sub imgBtnSubmit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnSubmit.Click
        Session("UserNew") = False
        Dim strbr As New StringBuilder
        Try
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If txt_login.Text <> "" And txt_pwd.Text <> "" Then
                If objconn.connect() = True Then

                    '/**************Start,Jatin Gangajaliya,2011/03/28*****************/
                    strbr = New StringBuilder
                    strbr.Append(" Select m_user_info.userid,m_user_info.loginname as loginname,m_user_info.user_type, ")
                    strbr.Append(" t_candidate_status.loginname as loginname1,t_candidate_status.pwd from m_user_info ")
                    strbr.Append(" left join t_candidate_status on t_candidate_status.userid = m_user_info.userid where  ")
                    strbr.Append(" t_candidate_status.loginname = ")
                    strbr.Append("'")
                    strbr.Append(txt_login.Text)
                    strbr.Append("'")
                    strbr.Append(" and t_candidate_status.Pwd = ")
                    strbr.Append("'")
                    strbr.Append(txt_pwd.Text)
                    strbr.Append("'")
                    'added by bhumi [9/10/2015]
                    'reason: Don't allow to appear Exam of old Class if student's Class is Changed 
                    strbr.Append(" and m_user_info.Delete_Flg = 0 ")
                    'Ended by bhumi
                    strbr.Append(" and m_user_info.user_type = '0' ")                    
                    sqlstr = strbr.ToString()
                    '/*****************************End********************************/

                    Session.Add("login", txt_login.Text)
                    Session.Add("pwd", txt_pwd.Text)
                    'sqlstr = "SELECT Userid,LoginName,User_Type from M_USER_INFO"
                    'sqlstr = sqlstr & " WHERE LoginName= '" & txt_login.Text & "'"
                    'sqlstr = sqlstr & " AND pwd= '" & txt_pwd.Text & "'"

                    MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    MyDataReader = MyCommand.ExecuteReader()
                    If MyDataReader.Read() Then
                        userid = MyDataReader.Item("Userid")
                        usertype = MyDataReader.Item("User_Type")
                        Session("UserName") = MyDataReader.Item("LoginName")
                        Session("UniUserType") = usertype
                        If Not IsNothing(MyDataReader.Item("User_Type")) Then
                            If usertype = 0 Then
                                Session.Add("userid", MyDataReader.Item("userid"))
                                Session.Add("LoginGenuine", True)
                                Session.Add("studentusertype", MyDataReader.Item("User_Type"))
                            ElseIf usertype <> 0 Then
                                Session.Add("userid", "err")
                            End If
                        End If
                        MyDataReader.Close()
                        FormsAuthentication.Initialize()
                        Dim Ticket As New FormsAuthenticationTicket(1, txt_login.Text, DateTime.Now, DateTime.Now.AddMinutes(20), False, "", FormsAuthentication.FormsCookiePath)
                        Dim strHash As String

                        strHash = FormsAuthentication.Encrypt(Ticket)
                        Dim cookie As New HttpCookie(FormsAuthentication.FormsCookieName, strHash)

                        If Ticket.IsPersistent Then
                            cookie.Expires = Ticket.Expiration
                        End If

                        Response.Cookies.Add(cookie)

                        strReDirect = Request("ReturnURL")
                        If Not strReDirect = "" Then
                            Response.Redirect(strReDirect, True)
                        Else
                            If usertype = 0 Then
                                'sqlstr = "SELECT written_test_date FROM t_candidate_status Where userid='" & userid & "'"
                                sqlstr = "SELECT written_test_date FROM t_candidate_status Where userid='" & userid & "' and LoginName='" & txt_login.Text & "' and Pwd ='" & txt_pwd.Text & "'"
                                MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                MyDataReader = MyCommand.ExecuteReader()
                                If MyDataReader.Read() Then
                                    WrittenTestdate = MyDataReader.Item("written_test_date")
                                End If
                                MyDataReader.Close()
                                'sqlstr = "Select Count(test_type) as testtype,Count(appearedflag) as appearedflagcount FROM t_candidate_status Where userid='" & userid & "'"
                                sqlstr = "Select Count(Course_ID) as testtype,Count(appearedflag) as appearedflagcount FROM t_candidate_status Where userid='" & userid & "' and LoginName='" & txt_login.Text & "' and Pwd ='" & txt_pwd.Text & "'"
                                MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                MyDataReader = MyCommand.ExecuteReader()
                                If MyDataReader.Read() Then
                                    testtypecount = MyDataReader.Item("testtype")
                                    flgAppearcount = MyDataReader.Item("appearedflagcount")
                                End If
                                MyDataReader.Close()

                                'sqlstr = "SELECT test_type,appearedflag FROM t_candidate_status Where userid='" & userid & "'"
                                sqlstr = "SELECT Course_ID,appearedflag FROM t_candidate_status Where  userid='" & userid & "' and LoginName='" & txt_login.Text & "' and Pwd ='" & txt_pwd.Text & "'"
                                MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                MyDataReader = MyCommand.ExecuteReader()
                                Dim flgAppeartotalvaluearr(flgAppearcount) As Integer
                                Dim testtotalvaluearr(testtypecount) As Integer
                                While MyDataReader.Read()
                                    'testtypearrlst.Add(Integer.Parse(MyDataReader.Item("test_type")))
                                    testtypearrlst.Add(Integer.Parse(MyDataReader.Item("Course_ID")))
                                    flgAppeararrlst.Add(MyDataReader.Item("appearedflag"))
                                End While
                                testtotalvaluearr = testtypearrlst.ToArray(System.Type.GetType("System.Int32"))
                                flgAppeartotalvaluearr = flgAppeararrlst.ToArray(System.Type.GetType("System.Int32"))
                                Session.Add("TotalTestName", testtotalvaluearr)
                                Session.Add("TotalFlagAppearValue", flgAppeartotalvaluearr)

                                MyDataReader.Close()
                                Dim totaltesttypevalue As Integer = 0
                                Dim totalflgAppearvalue As Integer = 0
                                For totaltesttypevalue = 0 To testtotalvaluearr.Length - 1
                                    For totalflgAppearvalue = 0 To flgAppeartotalvaluearr.Length - 1
                                        If flgAppeartotalvaluearr(totalflgAppearvalue) = "0" Or flgAppeartotalvaluearr(totalflgAppearvalue) = "1" Then
                                            'If flgAppeartotalvaluearr(totalflgAppearvalue) = "2" Or flgAppeartotalvaluearr(totalflgAppearvalue) = "1" Then

                                            '  Dim ard() As String = FromStartDate.Split("/")
                                            Dim dateFrom As New Date() 'ard(2), ard(2), ard(2))
                                            '  ard = DateTime.Now.ToShortDateString().Split("/")
                                            Dim dateToday As New Date(Today.Year, Today.Month, Today.Day) 'ard(0), ard(1), ard(2))
                                            '  ard = ToEndDate.Split("/")
                                            Dim dateEnd As New Date() 'ard(0), ard(1), ard(2))


                                            '************get FromDate Value*************
                                            FromTestdateValue = getFromAndStartDateGap("m_system_settings", "Exam from date", "from date")
                                            Dim adddate = CDate(WrittenTestdate)
                                            FromStartDate = DateAdd(DateInterval.Day, FromTestdateValue, adddate)
                                            dateFrom = DateAdd(DateInterval.Day, FromTestdateValue, adddate).Date
                                            'Dim startdate As String = Format(CDate(FromStartDate), "dd/MM/yyyy")
                                            'FromStartDate = startdate
                                            '*************get ToTestdateValue*************
                                            ToTestdateValue = getFromAndStartDateGap("m_system_settings", "Exam Within date", "Within date")
                                            Dim FromStartToEndDate = CDate(FromStartDate)
                                            'ToEndDate = DateAdd(DateInterval.Day, ToTestdateValue, FromStartToEndDate)
                                            dateEnd = DateAdd(DateInterval.Day, ToTestdateValue, FromStartToEndDate).Date
                                            'Dim Enddate As String = Format(CDate(ToEndDate), "dd/MM/yyyy")
                                            'ToEndDate = Enddate
                                            '*************To get Time Value**************
                                            sqlstr = "SELECT value FROM m_system_settings Where key1='exam time' and key2='start and end time'"
                                            MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                                            MyDataReader = MyCommand.ExecuteReader()
                                            If MyDataReader.Read() Then
                                                TestTime = MyDataReader.Item("value")
                                                strArray = TestTime.Split(",")
                                                Dim i As Integer = 0
                                                For i = 0 To strArray.Length - 1
                                                    formatedTime = FormatDate(strArray(i))
                                                    strArray(i) = formatedTime
                                                Next
                                            End If
                                            MyDataReader.Close()
                                            '******************Check For Valid Date****************
                                            'If FromStartDate <= DateTime.Now.ToString("dd/MM/yyyy") And ToEndDate >= DateTime.Now.ToString("dd/MM/yyyy") Then


                                            'Dim ard() As String = FromStartDate.Split("/")
                                            'Dim dateFrom As New Date(ard(2), ard(2), ard(2))
                                            'ard = DateTime.Now.ToShortDateString().Split("/")
                                            'Dim dateToday As New Date(ard(0), ard(1), ard(2))
                                            'ard = ToEndDate.Split("/")
                                            'Dim dateEnd As New Date(ard(0), ard(1), ard(2))


                                            Dim FDiff As Long = DateDiff(DateInterval.Day, dateFrom, dateToday)
                                            Dim LDiff As Long = DateDiff(DateInterval.Day, dateEnd, dateToday)
                                           

                                            If FromStartDate >= DateTime.Now.ToShortDateString() Or dateEnd <= DateTime.Now.ToShortDateString() Then
                                                If FDiff >= 0 And LDiff <= 0 Then
                                                    Dim i As Integer = 0
                                                    '************************Check For Time**************
                                                    For i = 0 To strArray.Length - 1
                                                        Dim currentdatetime As String = DateTime.Now.ToShortTimeString()
                                                        If currentdatetime.Substring(currentdatetime.Length - 2, 2).ToString() = "AM" Then
                                                            If (currentdatetime.Substring(0, 2).ToString() = "12") Then
                                                                If currentdatetime < strArray(0) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(0).Length = "7" And currentdatetime.Length = "7") Then
                                                                If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) >= strArray(0).Concat("0", strArray(0).Substring(0, 1).ToString()) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(0).Length = "7" And currentdatetime.Length = "8") Then
                                                                If currentdatetime >= strArray(0).Concat("0", strArray(0).Substring(0, 1).ToString()) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(0).Length = "8" And currentdatetime.Length = "7") Then
                                                                If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) >= strArray(0) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(0).Length = "8" And currentdatetime.Length = "8") Then
                                                                If currentdatetime >= strArray(0) Then
                                                                    flgValue = True
                                                                End If
                                                            End If

                                                        ElseIf currentdatetime.Substring(currentdatetime.Length - 2, 2).ToString() = "PM" Then
                                                            If (currentdatetime.Substring(0, 2).ToString() = "12") Then
                                                                If currentdatetime > strArray(1).Concat("0", strArray(1).Substring(0, 1).ToString()) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(1).Length = "7" And currentdatetime.Length = "7") Then
                                                                If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) < strArray(1).Concat("0", strArray(1).Substring(0, 1).ToString()) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(1).Length = "7" And currentdatetime.Length = "8") Then
                                                                If currentdatetime < strArray(1).Concat("0", strArray(1).Substring(0, 1).ToString()) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(1).Length = "8" And currentdatetime.Length = "7") Then
                                                                If currentdatetime.Concat("0", currentdatetime.Substring(0, 1).ToString()) < strArray(1) Then
                                                                    flgValue = True
                                                                End If
                                                            ElseIf (strArray(1).Length = "8" And currentdatetime.Length = "8") Then
                                                                If currentdatetime < strArray(1) Then
                                                                    flgValue = True
                                                                End If
                                                            End If

                                                        End If
                                                    Next
                                                    ' If flgValue = True Then ' Commented By : saraswati patel Description:Time related problem 
                                                    Dim uda As SqlDataAdapter
                                                    Dim uds As DataSet
                                                    Dim q As String = ""
                                                    Try
                                                        q = "select RollNo from M_User_Info where userid=" & Session("userid").ToString
                                                        '   If objconn.connect(strPathDb) = True Then
                                                        uda = New SqlDataAdapter(q, objconn.MyConnection)
                                                        uds = New DataSet
                                                        uda.Fill(uds)
                                                        Session.Add("RollNo", uds.Tables(0).Rows(0).Item(0).ToString)
                                                        '  End If
                                                    Catch ex As Exception
                                                        Response.Write(ex.Message)
                                                    Finally
                                                        uda = Nothing
                                                        uds = Nothing
                                                    End Try

                                                    strReDirect = "StudentInfo.aspx"
                                                    'strReDirect = "Register.aspx"
                                                    Response.Redirect(strReDirect, False)
                                                    'ElseIf flgValue = False Then ' Commented By : saraswati patel Description:Time related problem 
                                                    ' lbl_saved.Visible = True ' Commented By : saraswati patel Description:Time related problem 
                                                    ' lbl_saved.Text = "Your Exam Time Is Passed. Give Exam During " & strArray(0) & " To " & strArray(1) & " ." ' Commented By : saraswati patel Description:Time related problem 
                                                    'End If ' Commented By : saraswati patel Description:Time related problem 
                                                Else

                                                    'Dim ardEr() As String = FromStartDate.Split("/")
                                                    'Dim dateFromEr As New Date(ard(2), ard(0), ard(1))
                                                    'ardEr = DateTime.Now.ToShortDateString().Split("/")
                                                    'Dim dateTodayEr As New Date(ard(2), ard(0), ard(1))
                                                    'ardEr = ToEndDate.Split("/")
                                                    'Dim dateEndEr As New Date(ard(2), ard(0), ard(1))


                                                    'Dim FDiffEr As Long = DateDiff(DateInterval.Day, dateFromEr, dateTodayEr)
                                                    'Dim LDiffEr As Long = DateDiff(DateInterval.Day, dateEndEr, dateTodayEr)

                                                    lbl_saved.Visible = True
                                                    'If DateTime.Now.ToShortDateString() < FromStartDate Then
                                                    If FDiff < 0 Then
                                                        lbl_saved.Text = "Your Exam Date Is " & dateFrom.ToString("dd/MM/yyyy") & ".You Can Not Login For Exam Before This Date."
                                                        'ElseIf DateTime.Now.ToShortDateString() > ToEndDate Then
                                                    ElseIf LDiff > 0 Then
                                                        lbl_saved.Text = "Your Exam Date Is Passed.Please Contact To Administrator."

                                                    End If

                                                End If


                                            End If
                                            'Added By Rahul Shukla on 23/09/2019
                                        Else
                                            lbl_saved.Visible = True
                                            lbl_saved.Text = "You Already Given The Exam"
                                        End If
                                        
                                    Next totalflgAppearvalue
                                Next totaltesttypevalue
                            End If
                        End If        
                    Else
                        Session.Add("userid", "err")
                    End If
                End If
                MyDataReader.Close()
                MyCommand.Dispose()
                objconn.disconnect()
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            strbr = Nothing
        End Try
    End Sub
    Shared Function getFromAndStartDateGap(ByVal TableName As String, ByVal Field1 As String, ByVal Field2 As String) As Integer
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
        Finally
            MyCommand.Dispose()
            objconn.disconnect()
        End Try
    End Function

    Shared Function FormatDate(ByVal s As String) As String
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
    End Function
    '*************On Error Go to Error Page****************
    Private Sub onErrors(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Error
        'Dim Err As New CreateLog
        'Err.ErrorLog(Server.MapPath("Logs/RMS"), Server.GetLastError().ToString().Trim, "login.aspx.vb")
        Response.Redirect("error.aspx?err=Error On Page Login")
    End Sub

    Protected Sub imgBtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnBack.Click
        'Response.Redirect("ExamPortal.aspx")
    End Sub

    'Public Sub CheckQuestions(ByVal Cid As String)
    '    Dim tot_Marks As Integer = 0
    '    Dim item As DictionaryEntry
    '    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    '    Dim cds As DataSet
    '    Dim cda As SqlDataAdapter
    '    Dim htSub As Hashtable ' for subject and marks 
    '    Dim htSubQues As New Hashtable ' for Each subject Qno for each type  key as sub id and value as arraylist
    '    Dim htFinal As New Hashtable ' Holds testtype as key and vale as Structre of weightage 
    '    ' The Structure All the Basic and intermediate values for TF/MC/BL
    '    ' We can Use The Function Generate Queries
    '    Dim htQuestions As New Hashtable

    '    Dim SelQuesTFB As New Hashtable
    '    Dim selItem As DictionaryEntry

    '    Dim SelQuesTFI As New Hashtable
    '    Dim selItemTFI As DictionaryEntry

    '    Dim SelQuesMCB As New Hashtable
    '    Dim selItemMCB As DictionaryEntry

    '    Dim SelQuesMCI As New Hashtable
    '    Dim selItemMCI As DictionaryEntry

    '    Dim SelQuesBLB As New Hashtable
    '    Dim selItemBLB As DictionaryEntry

    '    Dim SelQuesBLI As New Hashtable
    '    Dim selItemBLI As DictionaryEntry



    '    Dim htdata As New Hashtable
    '    Dim testTotal As Integer = 0

    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            cds = New DataSet
    '            cda = New SqlDataAdapter("select total_marks from m_course where course_id=" & Cid, objconn.MyConnection)
    '            cda.Fill(cds)
    '            tot_Marks = CInt(cds.Tables(0).Rows(0).Item(0))
    '        End If
    '        htSub = GetMarksForSubject(Cid, tot_Marks)

    '        For Each item In htSub
    '            Dim aa() As Integer = GetQTypeCountForSubject(CInt(item.Key), CInt(htSub(item.Key)), Cid)
    '            htSubQues.Add(item.Key, aa)
    '        Next

    '        For Each item In htSubQues
    '            htFinal.Add(item.Key, GetWeightStructure(item.Key, Cid, DirectCast(item.Value, Integer())))
    '        Next


    '        For Each item In htFinal
    '            Dim testtype As String = item.Key.ToString
    '            Dim SWeight As StructWeight = DirectCast(item.Value, StructWeight)

    '            For i As Integer = 1 To 3
    '                Dim Bas As Integer = 0
    '                Dim Imed As Integer = 0
    '                If i = 1 Then
    '                    Bas = SWeight.TFBasic
    '                    Imed = SWeight.TFIMed
    '                ElseIf i = 2 Then
    '                    Bas = SWeight.MCBasic
    '                    Imed = SWeight.MCIMed
    '                ElseIf i = 3 Then
    '                    Bas = SWeight.BLBasic
    '                    Imed = SWeight.BLIMed
    '                End If

    '                For j As Integer = 0 To 1
    '                    Dim stbldr As New StringBuilder

    '                    stbldr.Append(" select  qnid, test_type,Qn_Category_ID,qlevel,total_marks as que_mark,SUM(total_marks) as total_marks ")
    '                    stbldr.Append(" from m_question ")
    '                    stbldr.Append(" where del_flag=0 AND TEST_TYPE=" & testtype & " and Qn_Category_ID=" & i.ToString & " and qlevel=" & j.ToString)
    '                    stbldr.Append(" group by  test_type,Qn_Category_ID,total_marks,qlevel,qnid ")
    '                    stbldr.Append(" order by Qn_Category_ID,qlevel,newid() ")


    '                    Dim query As String = stbldr.ToString
    '                    testTotal = 0
    '                    Dim wds As DataSet
    '                    Dim wda As SqlDataAdapter
    '                    Dim htMandTotal As New Hashtable
    '                    Dim sitem As DictionaryEntry
    '                    Try
    '                        If objconn.connect(strPathDb) = True Then
    '                            wds = New DataSet
    '                            wda = New SqlDataAdapter(query, objconn.MyConnection)
    '                            wda.Fill(wds)
    '                            For k As Integer = 0 To wds.Tables(0).Rows.Count - 1
    '                                Dim ar(4) As Integer
    '                                ar(0) = wds.Tables(0).Rows(k).Item(1).ToString
    '                                ar(1) = wds.Tables(0).Rows(k).Item(2).ToString
    '                                ar(2) = wds.Tables(0).Rows(k).Item(3).ToString
    '                                ar(3) = wds.Tables(0).Rows(k).Item(5).ToString
    '                                htMandTotal.Add(wds.Tables(0).Rows(k).Item(0).ToString, ar)
    '                            Next
    '                        End If


    '                        For Each sitem In htMandTotal
    '                            If j = 0 Then
    '                                If Bas <= testTotal Then
    '                                    ' Set the key value as testtype,qcategory,qlevel || value as qnid
    '                                    '      Exit For
    '                                    Exit For
    '                                    'GoTo jatin
    '                                End If
    '                                Dim nar() As Integer = DirectCast(sitem.Value, Integer())

    '                                Dim nextNum As Integer = testTotal + CInt(nar(3))
    '                                If (nextNum <= Imed) Then
    '                                    testTotal = testTotal + CInt(nar(3))
    '                                    If i = 1 And j = 0 Then
    '                                        SelQuesTFB.Add(sitem.Key, sitem.Value)
    '                                        'ElseIf i = 1 And j = 1 Then
    '                                        '    SelQuesTFI.Add(sitem.Key, sitem.Value)
    '                                    ElseIf i = 2 And j = 0 Then
    '                                        SelQuesMCB.Add(sitem.Key, sitem.Value)
    '                                        'ElseIf i = 2 And j = 1 Then
    '                                        '    SelQuesMCI.Add(sitem.Key, sitem.Value)
    '                                    ElseIf i = 3 And j = 0 Then
    '                                        SelQuesBLB.Add(sitem.Key, sitem.Value)
    '                                        'ElseIf i = 3 And j = 1 Then
    '                                        '    SelQuesBLI.Add(sitem.Key, sitem.Value)
    '                                    End If

    '                                End If

    '                            ElseIf j = 1 Then
    '                                If Imed <= testTotal Then
    '                                    ' Set the key value as testtype,qcategory,qlevel || value as qnid
    '                                    'j = 2                                       'i = i + 1
    '                                    '  j = 0
    '                                    ' i = i + 1
    '                                    Exit For
    '                                End If
    '                                Dim nar() As Integer = DirectCast(sitem.Value, Integer())

    '                                Dim nextNum As Integer = testTotal + CInt(nar(3))
    '                                If (nextNum <= Imed) Then
    '                                    testTotal = testTotal + CInt(nar(3))
    '                                    If i = 1 And j = 1 Then
    '                                        SelQuesTFI.Add(sitem.Key, sitem.Value)
    '                                        'ElseIf i = 2 And j = 0 Then
    '                                        '    SelQuesMCB.Add(sitem.Key, sitem.Value)
    '                                    ElseIf i = 2 And j = 1 Then
    '                                        SelQuesMCI.Add(sitem.Key, sitem.Value)
    '                                        'ElseIf i = 3 And j = 0 Then
    '                                        '    SelQuesBLB.Add(sitem.Key, sitem.Value)
    '                                    ElseIf i = 3 And j = 1 Then
    '                                        SelQuesBLI.Add(sitem.Key, sitem.Value)
    '                                    End If
    '                                End If



    '                                'If i = 1 And j = 0 Then
    '                                '    SelQuesTFB.Add(sitem.Key, sitem.Value)

    '                            End If


    '                        Next


    '                        If j = 0 Then
    '                            If (testTotal < Bas) Then
    '                                Dim diff As Integer = Bas - testTotal
    '                                    Imed = Imed + diff
    '                            End If

    '                        ElseIf j = 1 Then



    '                            If (testTotal < Imed) Then
    '                                Dim diff As Integer = Imed - testTotal
    '                                If (i = 1) Then
    '                                    AdjustQuestions(SWeight.TestType, diff, 0, i, 0, SelQuesTFB)
    '                                ElseIf i = 2 Then
    '                                    AdjustQuestions(SWeight.TestType, diff, 0, i, 0, SelQuesMCB)
    '                                ElseIf i = 3 Then
    '                                    AdjustQuestions(SWeight.TestType, diff, 0, i, 0, SelQuesBLB)
    '                                End If

    '                            End If
    '                        End If


    '                        If 1 = 1 Then

    '                        End If
    '                    Catch ex As Exception
    '                    Finally
    '                        wds = Nothing
    '                        wda = Nothing

    '                    End Try

    '                Next
    '                If testTotal = Bas Or testTotal = Imed Then

    '                    i = i
    '                End If

    '            Next

    '        Next


    '        Dim stfb As String = ""
    '        For Each selItem In SelQuesTFB

    '            stfb = stfb & selItem.Key.ToString & ","
    '        Next

    '        Dim stfi As String = ""
    '        For Each selItemTFI In SelQuesTFI

    '            stfi = stfi & selItemTFI.Key.ToString & ","
    '        Next


    '        Dim smcb As String = ""
    '        For Each selItemMCB In SelQuesMCB
    '            smcb = smcb & selItemMCB.Key.ToString & ","
    '        Next


    '        Dim smci As String = ""
    '        For Each selItemMCI In SelQuesMCI
    '            smci = smci & selItemMCI.Key.ToString & ","
    '        Next



    '        Dim sblb As String = ""
    '        For Each selItemBLB In SelQuesBLB
    '            sblb = sblb & selItemBLB.Key.ToString & ","
    '        Next

    '        Dim sbli As String = ""
    '        For Each selItemBLI In SelQuesBLI
    '            sbli = sbli & selItemBLI.Key.ToString & ","
    '        Next


    '        'Dim ss As String = ""
    '        'For aa As Integer = 0 To SelQues.Count - 1
    '        '    ss = ss & SelQues(aa) & ","
    '        'Next

    '        Dim it As DictionaryEntry

    '        For Each it In SelQuesTFB
    '            htQuestions.Add(it.Key, it.Value)
    '        Next

    '        For Each it In SelQuesTFI
    '            htQuestions.Add(it.Key, it.Value)
    '        Next

    '        For Each it In SelQuesMCB
    '            htQuestions.Add(it.Key, it.Value)
    '        Next


    '        For Each it In SelQuesMCI
    '            htQuestions.Add(it.Key, it.Value)
    '        Next


    '        For Each it In SelQuesBLB
    '            htQuestions.Add(it.Key, it.Value)
    '        Next

    '        For Each it In SelQuesBLI
    '            htQuestions.Add(it.Key, it.Value)
    '        Next

    '        If 1 = 1 Then

    '        End If



    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Throw ex
    '    Finally
    '        cds = Nothing
    '        cda = Nothing
    '    End Try


    'End Sub

    'Public Function GetMarksForSubject(ByVal Course_id As String, ByVal TotalMarks As Integer) As Hashtable
    '    Dim ht As New Hashtable

    '    Dim sds As DataSet
    '    Dim sda As SqlDataAdapter
    '    Dim subjects As New Hashtable
    '    Dim subMarks As New Hashtable
    '    Dim item As DictionaryEntry
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            sds = New DataSet
    '            sda = New SqlDataAdapter("select test_type,Sub_Weightage from m_weightage where del_flag=0 and Course_id=" & Course_id, objconn.MyConnection)
    '            sda.Fill(sds)
    '            For i As Integer = 0 To sds.Tables(0).Rows.Count - 1
    '                subjects.Add(sds.Tables(0).Rows(i).Item(0).ToString, sds.Tables(0).Rows(i).Item(1).ToString)
    '            Next
    '        End If

    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Throw ex
    '    Finally
    '        sds = Nothing
    '        sda = Nothing
    '    End Try

    '    For Each item In subjects
    '        Dim sper As Integer = item.Value
    '        Dim multiple As Double = sper / 100
    '        Dim QforSub As Integer = 0

    '        QforSub = Math.Abs(TotalMarks * (multiple))
    '        subMarks.Add(item.Key, QforSub)
    '    Next


    '    ' Get Least Questions , ID and total to adjust later

    '    Dim sumtotal As Integer = GetSumTotalSubjects(subMarks)
    '    Dim leastID As Integer = GetLeastQuesSubjectID(subMarks)


    '    If (TotalMarks - sumtotal > 0) Then
    '        While TotalMarks - sumtotal <> 0
    '            Dim lval As Integer = CInt(subMarks(leastID.ToString))
    '            lval = lval + 1
    '            subMarks(leastID.ToString) = lval
    '            sumtotal = GetSumTotalSubjects(subMarks)
    '            leastID = GetLeastQuesSubjectID(subMarks)
    '        End While


    '    End If

    '    Return subMarks
    'End Function

    'Public Function GetSumTotalSubjects(ByVal submarks As Hashtable) As Integer
    '    Dim sumtotal As Integer = 0
    '    Dim item As DictionaryEntry
    '    Dim leastQ As Integer = 0
    '    Dim leastID As Integer = 0
    '    For Each item In subMarks
    '        If (sumtotal = 0) Then
    '            leastQ = CInt(subMarks(item.Key))
    '            leastID = item.Key
    '        Else
    '            If (leastQ > CInt(subMarks(item.Key))) Then
    '                leastQ = CInt(subMarks(item.Key))
    '                leastID = item.Key
    '            End If
    '        End If
    '        sumtotal = sumtotal + CInt(subMarks(item.Key))
    '    Next
    '    Return sumtotal
    'End Function

    'Public Function GetLeastQuesSubjectID(ByVal submarks As Hashtable) As Integer
    '    Dim sumtotal As Integer = 0
    '    Dim item As DictionaryEntry
    '    Dim leastQ As Integer = 0
    '    Dim leastID As Integer = 0
    '    For Each item In submarks
    '        If (sumtotal = 0) Then
    '            leastQ = CInt(submarks(item.Key))
    '            leastID = item.Key
    '        Else
    '            If (leastQ > CInt(submarks(item.Key))) Then
    '                leastQ = CInt(submarks(item.Key))
    '                leastID = item.Key
    '            End If
    '        End If
    '        sumtotal = sumtotal + CInt(submarks(item.Key))
    '    Next
    '    Return leastID
    'End Function

    'Public Function GetQTypeCountForSubject(ByVal test_type As Integer, ByVal totalMarks As Integer, ByVal Course_id As String) As Integer()

    '    'Dim arrLst As New ArrayList
    '    Dim tds As DataSet
    '    Dim tda As SqlDataAdapter
    '    Dim tf As Integer = 0
    '    Dim tfper As Double = 0

    '    Dim mc As Integer = 0
    '    Dim mcper As Double = 0

    '    Dim bl As Integer = 0
    '    Dim blper As Double = 0

    '    Dim bas As Integer = 0
    '    Dim basper As Double = 0

    '    Dim IMed As Integer = 0
    '    Dim IMedper As Double = 0

    '    Dim arr(3) As Integer
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            tds = New DataSet
    '            tda = New SqlDataAdapter("select test_type,Sub_Weightage,single,multi_Choice,Blanks,Basic,InterMed from m_weightage where del_flag=0 and Course_id=" & Course_id & " and test_type=" & test_type, objconn.MyConnection)
    '            tda.Fill(tds)
    '            tf = CInt(tds.Tables(0).Rows(0).Item(2))
    '            tfper = tf / 100
    '            mc = CInt(tds.Tables(0).Rows(0).Item(3))
    '            mcper = mc / 100
    '            bl = CInt(tds.Tables(0).Rows(0).Item(4))
    '            blper = bl / 100
    '            bas = CInt(tds.Tables(0).Rows(0).Item(5))
    '            basper = bas / 100

    '            tf = Math.Abs(totalMarks * tfper)
    '            mc = Math.Abs(totalMarks * mcper)
    '            bl = Math.Abs(totalMarks * blper)

    '            arr(0) = tf
    '            arr(1) = mc
    '            arr(2) = bl

    '            If (tf + mc + bl) <> totalMarks Then
    '                While ((tf + mc + bl) <> totalMarks)
    '                    If ((tf + mc + bl) > totalMarks) Then
    '                        arr = AdjustQTypesMax(arr)



    '                    ElseIf (tf + mc + bl) < totalMarks Then
    '                        arr = AdjustQTypesLeast(arr)

    '                    End If
    '                    tf = arr(0)
    '                    mc = arr(1)
    '                    bl = arr(2)

    '                End While
    '            End If



    '        End If
    '        Return arr

    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Throw ex
    '    Finally

    '    End Try
    'End Function

    'Public Function AdjustQTypesLeast(ByVal ar As Integer()) As Integer()
    '    Dim least As Integer = 0

    '    For i As Integer = 0 To ar.Length - 1
    '        If (i = 0) Then
    '            least = i

    '        Else
    '            If ar(i) < least Then
    '                least = i
    '            End If
    '        End If
    '    Next

    '    ar(least) = ar(least) + 1
    '    Return ar
    'End Function


    'Public Function AdjustQTypesMax(ByVal ar As Integer()) As Integer()
    '    Dim max As Integer = 0

    '    For i As Integer = 0 To ar.Length - 1
    '        If (i = 0) Then

    '            max = i

    '        Else
    '            If ar(i) > max Then
    '                max = i
    '            End If
    '        End If
    '    Next

    '    ar(max) = ar(max) - 1
    '    Return ar
    'End Function


    'Public Function GetWeightStructure(ByVal testtype As String, ByVal courseid As String, ByVal arrVal As Integer()) As StructWeight
    '    Dim wtStruct As New StructWeight
    '    wtStruct.TestType = testtype
    '    Dim BQ As Integer = 0
    '    Dim Bper As Double = 0
    '    Dim IMQ As Integer = 0
    '    Dim IMPer As Double = 0

    '    Dim wds As DataSet
    '    Dim wda As SqlDataAdapter
    '    Dim query As String = "select Weightage_Id,Basic,InterMed from M_weightage where course_id=" & courseid & " and del_flag=0 and test_type=" & testtype
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            wds = New DataSet
    '            wda = New SqlDataAdapter(query, objconn.MyConnection)
    '            wda.Fill(wds)
    '            Bper = CInt(wds.Tables(0).Rows(0).Item(1))
    '            Bper = Bper / 100
    '            IMPer = CInt(wds.Tables(0).Rows(0).Item(2))
    '            IMPer = IMPer / 100
    '        End If


    '        For i As Integer = 0 To 2
    '            Dim weight As Integer = arrVal(i)
    '            BQ = Math.Abs(weight * Bper)
    '            IMQ = Math.Abs(weight * IMPer)

    '            If (BQ + IMQ) <> weight Then
    '                While (BQ + IMQ) <> weight
    '                    If (BQ + IMQ) < weight Then
    '                        If (Bper > IMPer) Then
    '                            BQ = BQ + 1
    '                        Else
    '                            IMQ = IMQ + 1
    '                        End If


    '                    ElseIf (BQ + IMQ) > weight Then
    '                        If (Bper > IMPer) Then
    '                            BQ = BQ - 1
    '                        Else
    '                            IMQ = IMQ - 1
    '                        End If
    '                    End If
    '                End While
    '            End If

    '            If i = 0 Then
    '                wtStruct.TFBasic = BQ
    '                wtStruct.TFIMed = IMQ
    '            ElseIf i = 1 Then
    '                wtStruct.MCBasic = BQ
    '                wtStruct.MCIMed = IMQ
    '            ElseIf i = 2 Then
    '                wtStruct.BLBasic = BQ
    '                wtStruct.BLIMed = IMQ

    '            End If
    '        Next

    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Throw ex
    '    Finally
    '        wda = Nothing
    '        wds = Nothing
    '    End Try
    '    Return wtStruct
    'End Function

    'Public Function AdjustQuestions(ByVal test As String, ByVal diff As Integer, ByVal AdjustFromlevel As Integer, ByVal Qn_cat As Integer, ByVal qlevel As Integer, ByRef ht As Hashtable)
    '    '    testtype, difference, Basic, intermed,qcat
    '    Dim hitem As DictionaryEntry
    '    Dim ids As String = ""

    '    For Each hitem In ht
    '        ids = ids & hitem.Key.ToString & ","
    '    Next

    '    If ids <> "" Then
    '        ids = ids.Substring(0, ids.Length - 1)
    '        ids = "and qnid not in (" & ids & ")"
    '    End If
    '    Dim stbldr As New StringBuilder


    '    stbldr.Append(" select  qnid, test_type,Qn_Category_ID,qlevel,total_marks as que_mark,SUM(total_marks) as total_marks ")
    '    stbldr.Append(" from m_question ")
    '    stbldr.Append(" where del_flag=0 AND TEST_TYPE=" & test & " and Qn_Category_ID=" & Qn_cat & " and qlevel=" & AdjustFromlevel)
    '    stbldr.Append(" " & ids & " ")
    '    stbldr.Append(" group by  test_type,Qn_Category_ID,total_marks,qlevel,qnid ")
    '    stbldr.Append(" order by Qn_Category_ID,qlevel,newid() ")

    '    Dim hds As DataSet
    '    Dim hda As SqlDataAdapter
    '    Dim ttotal As Integer = 0
    '    Try
    '        If objconn.connect(strPathDb) = True Then

    '            hds = New DataSet
    '            hda = New SqlDataAdapter(stbldr.ToString, objconn.MyConnection)
    '            hda.Fill(hds)

    '            For i As Integer = 0 To hds.Tables(0).Rows.Count - 1
    '                If diff <= ttotal Then
    '                    Exit For
    '                End If

    '                Dim nar(3) As Integer ' = DirectCast(sitem.Value, Integer())
    '                nar(0) = hds.Tables(0).Rows(i).Item(1).ToString
    '                nar(1) = hds.Tables(0).Rows(i).Item(2).ToString
    '                nar(2) = hds.Tables(0).Rows(i).Item(5).ToString

    '                If ttotal + CInt(hds.Tables(0).Rows(i).Item(5).ToString) <= diff Then
    '                    ttotal = ttotal + CInt(hds.Tables(0).Rows(i).Item(5).ToString)
    '                    ht.Add(hds.Tables(0).Rows(i).Item(0).ToString, nar)
    '                End If

    '            Next

    '        End If
    '    Catch ex As Exception

    '    Finally
    '        hda = Nothing
    '        hds = Nothing
    '        objconn.disconnect()
    '    End Try


    'End Function

    'Public Function FinalQuestions(ByRef htTFB As Hashtable, ByRef htTFI As Hashtable, ByRef htMCB As Hashtable, ByRef htMCI As Hashtable, ByRef htBLB As Hashtable, ByRef htBLI As Hashtable, ByVal WStruct As StructWeight)
    '    Dim TF As Integer = WStruct.TFBasic + WStruct.TFIMed
    '    Dim TFCnt As Integer = 0
    '    Dim tfdiff As Integer = 0

    '    Dim MC As Integer = WStruct.MCBasic + WStruct.MCIMed
    '    Dim MCCnt As Integer = 0
    '    Dim mcdiff As Integer = 0

    '    Dim BL As Integer = WStruct.BLBasic + WStruct.BLIMed
    '    Dim BLCnt As Integer = 0
    '    Dim bldiff As Integer = 0

    '    Dim titem As DictionaryEntry
    '    Dim ntitem As DictionaryEntry

    '    Try

    '        For Each titem In htTFB
    '            Dim arr() As Integer = DirectCast(titem.Value, Integer())
    '            TFCnt = TFCnt + arr(3)
    '        Next
    '        For Each titem In htTFI
    '            Dim arr() As Integer = DirectCast(titem.Value, Integer())
    '            TFCnt = TFCnt + arr(3)
    '        Next

    '        If TF <> TFCnt Then
    '            tfdiff = TFCnt - TF
    '            While tfdiff <> 0
    '                tfdiff = TFCnt - TF
    '                For Each titem In htTFB
    '                    htTFB.Remove(titem.Key)
    '                    Exit For
    '                Next
    '                TFCnt = 0
    '                For Each titem In htTFB
    '                    Dim arr() As Integer = DirectCast(titem.Value, Integer())
    '                    TFCnt = TFCnt + arr(3)
    '                Next
    '                For Each titem In htTFI
    '                    Dim arr() As Integer = DirectCast(titem.Value, Integer())
    '                    TFCnt = TFCnt + arr(3)
    '                Next
    '            End While
    '        End If

    '        If MC <> MCCnt Then
    '            mcdiff = MCCnt - MC
    '            If MC <> MCCnt Then

    '            End If

    '        End If






    '    Catch ex As Exception
    '    Finally
    '    End Try
    'End Function

End Class