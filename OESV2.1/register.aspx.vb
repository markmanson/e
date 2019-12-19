#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports log4net
#End Region

Namespace unirecruite


    Partial Class register
        Inherits BasePage
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("register")
        Protected WithEvents img_qpaper As System.Web.UI.WebControls.ImageButton
        Protected WithEvents l6bl_peradd As System.Web.UI.WebControls.Label
        'Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton

#Region "Variables"
        Const ENCRYPT_DELIMIT = "h"
        Const ENCRYPT_KEY = 124
        Dim CONS As New unirecruite.Errconstants
        Dim myTable1, myTable2, myTable3 As DataTable
        Dim myDataReader As SqlDataReader                 'SqlDataReader object
        Dim myCommand As SqlCommand                       'SqlCommand object
        Dim strPathDb As String
        Dim i, j As Integer
        Dim countryIndex, stateIndex, cityIndex As Integer  'Variables of Integer Type
        Dim objconn As New ConnectDb        'Object of the connectclass class
        Dim sqlstr As String                                'Variable of string Type
        Dim sqlstr2 As String                                'variable to strore query from m_certi_master
        Dim myRow1, myRow2, myRow3 As DataRow               'DataRow objects
        Protected WithEvents C As System.Web.UI.WebControls.CompareValidator
        Protected WithEvents lblVldDOI As System.Web.UI.WebControls.Label
        Protected WithEvents txt_other As System.Web.UI.WebControls.TextBox
        Dim sqlTrans As SqlTransaction
        Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        'Dim VldDate As Boolean
#End Region

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
        '******** The Page Load Event *********
#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'added by bhumi [15/9/2015]
            'reason: validation message display on screen while reset button click
            txt_dob.Attributes.Add("type", "date")
            Dim thisd As String = ConvertDateForDOB(Date.Today)
            txt_dob.Attributes.Add("max", thisd)
            lblMsg.Visible = False
            'Ended by bhumi
            'Added by Vaibhav Soni
            'Date : 2014/03/05
            If ddlCenters.SelectedIndex = 0 Then
                '  courserowt.Visible = False
                'Commented by rajesh
                '   courserow.Visible = False
                'trSpace2.Visible = False
            Else
                '  courserowt.Visible = True
                'Commented by rajesh
                'courserow.Visible = True
                'trSpace2.Visible = True
            End If
            'Ended by Vaibhav Soni

            Dim YrDt() As String = Split(txt_dob.Value, "/")
            Dim C_HOMEPAGEURL As String
            Dim strPathDb As String
            Dim strselcountry As String
            Dim strselstate As String
            Dim strselcity As String
            Dim strisscountry As String

            Try
                '                GetRollNumber()
                'txt_dob.Attributes.Add("Readonly", "true")

                If Session("LoginGenuine") And False = Session("UserNew") Is Nothing Then
                    Response.Redirect("error.aspx?err=Session Timeout. Please Login to continue.", False)
                End If
                C_HOMEPAGEURL = ConfigurationSettings.AppSettings("C_HOMEPAGEURL")
                'Added by Vaibhav Soni
                'Date : 2014/03/05

                headrow.Visible = True
                contentrow.Visible = True
                'Ended By Vaibhav Soni
                If cmb_usertype.SelectedItem.Text = "Admin" Then
                    'Commented By Vaibhav Soni
                    'Date : 2014/03/12
                    'headrow.Visible = True
                    'contentrow.Visible = True
                    'Ended By Vaibhav Soni

                    'Added by Vaibhav Soni
                    'Date : 2014/03/05
                    trClassHead.Visible = False
                    div1.Visible = False
                    div2.Visible = False
                    'Ended by Vaibhav Soni
                ElseIf cmb_usertype.SelectedItem.Text = "Student" Then
                    'Commented By Vaibhav Soni
                    'Date : 2014/03/12
                    'headrow.Visible = False
                    'contentrow.Visible = False
                    'Ended By Vaibhav Soni

                    'Added by Vaibhav Soni
                    'Date : 2014/03/05
                    trClassHead.Visible = True
                    div1.Visible = True
                    div2.Visible = True
                    'Ended by Vaibhav Soni
                End If

                If Session.Item("check2") = "true" Then
                    'Commented By Vaibhav Soni
                    'Date : 2014/03/12
                    'headrow.Visible = True
                    'contentrow.Visible = True
                    'Ended By Vaibhav Soni
                    'Added by Vaibhav Soni
                    'Date : 2014/03/05
                    trClassHead.Visible = False
                    div1.Visible = False
                    div2.Visible = False
                    'Ended by Vaibhav Soni
                End If

                strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If (Session.Item("check") <> "true") Then
                    If Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                        'Commented By Bhumi[18/8/2015]
                        'Reason: Home & Logoff Buttons Get Visible False while Update Or Reset Button Click
                        'tdAdmin.Visible = False
                        'end by bhumi
                    Else
                    End If
                Else
                    'tdAdmin.Visible = True
                End If
                strselcountry = ""
                strselstate = ""
                strselcity = ""
                strisscountry = ""

                '********Checking if the page loading for the first time or not*********
                If Not IsPostBack Then
                    txt_firstname.Focus()
                    FillCenterCombo()
                    If Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                        FillListOfCourse()
                        cmb_usertype.Enabled = False
                        txtrollnumber.Enabled = False
                    End If
                    '********* Checking if it is a fresh registration*********
                    If Len(Session.Item("userid")) <> 0 Or Request.QueryString("userid") <> 0 Then
                        If Request.QueryString("userid") <> Nothing Then
                            StudentImage.ImageUrl = "StudentImageHandler.ashx?id=" & Request.QueryString("userid")
                        ElseIf Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                            StudentImage.ImageUrl = "StudentImageHandler.ashx?id=" & Session.Item("userid")
                        End If

                        Session.Add("userType", "old")
                        GetPersonalData()
                        imgClear.Visible = False
                        imgReset.Visible = True
                        If (Session.Item("check") <> "true") Then
                            If Session.Item("studentusertype") <> Nothing Or Session.Item("studentusertype") = 0 Then
                                If Convert.ToInt32(Session.Item("studentusertype")) = 0 Then
                                    '***************************************************************
                                    'Commented by bhumi [14/9/2015]
                                    'Reason: On reset button displaying Exams Details is not proper
                                    '***************************************************************

                                    'GetTestDetail()

                                    'Ended by bhumi

                                End If
                            End If
                        End If

                        img_update.Visible = True
                        img_saveexit.Visible = False
                        'If Session("check") = True And Session("checkforback") = True Then
                        lblLegend.Text = Resources.Resource.Register_UsMoDet
                        'End If
                        If (Session.Item("check") <> "true") Then
                            If Session.Item("studentusertype") <> Nothing Or Session.Item("studentusertype") = 0 Then
                                If Convert.ToInt32(Session.Item("studentusertype")) = 0 Then
                                    txt_firstname.Enabled = False
                                    txt_middlename.Enabled = False
                                    txt_surname.Enabled = False
                                    'txt_dob.Enabled = False
                                    'txt_dob.Attributes.Add("readonly", "true")
                                    rblist_sex.Enabled = False
                                    txtperadd.Enabled = False
                                    cmb_usertype.Enabled = False
                                    txt_phone.Enabled = False
                                    txt_login.Enabled = False
                                    txt_password.Enabled = False
                                    txt_confpassword.Enabled = False
                                    lnk_chpass.Enabled = False
                                    ddlCenters.Enabled = False
                                    ' lstCourses.Enabled = False
                                    Uploader.Enabled = False
                                    img_saveexit.Enabled = False
                                    img_update.Enabled = False
                                End If
                            End If
                        ElseIf Request.QueryString("E") <> Nothing Then
                            '   txt_password.Visible = True
                            txt_email.Enabled = False
                            ddlCenters.Enabled = False
                            If ddlCenters.SelectedIndex = 0 Then
                                'lstCourses.Visible = False
                                'courserowt.Visible = False
                                ' courserow.Visible = False
                            End If
                            'lstCourses.Enabled = False
                            'trSpace2.Visible = False
                            headrow.Visible = False
                            contentrow.Visible = False
                        End If
                    Else
                        txt_firstname.Focus()
                        FillCenterCombo()
                        If Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                            FillListOfCourse()
                        End If

                        If Request.QueryString("userid") <> Nothing Then
                            StudentImage.ImageUrl = "StudentImageHandler.ashx?id=" & Request.QueryString("userid")
                        ElseIf Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                            StudentImage.ImageUrl = "StudentImageHandler.ashx?id=" & Session.Item("userid")
                        End If

                        txt_password.Visible = True
                        txt_confpassword.Visible = True
                        trpwd.Visible = True
                        star.Visible = True
                        lbl_confpassword.Visible = True
                        lnk_chpass.Visible = False
                        img_update.Visible = False
                        img_saveexit.Visible = True
                        Session.Add("userType", "new")
                    End If

                    ' ****** Freeing the allocated memory ******
                End If

                ' This Condition added by Rajesh Nagvanshi 2014-06-05
                'Reason : when opening page from the Profile link click (user self login)
                If Request.QueryString("E") <> Nothing Then
                    If Request.QueryString("E").Equals("1") Then
                        txt_password.Visible = False
                        'txt_password.Enabled = False
                        headrow.Visible = False
                        contentrow.Visible = False
                    End If
                End If

                ' This Condition added by Rajesh Nagvanshi 2014-06-05
                'Reason : when opening page from the Profile link click (user self login)
                If Request.QueryString("pi") <> Nothing Then
                    If Request.QueryString("pi").Equals("0") Then
                        txt_password.Visible = False
                        'txt_password.Enabled = False
                        headrow.Visible = False
                        contentrow.Visible = False
                    End If
                End If
                '------------------------

                'Condition Added by Bhumi 12/8/2015
                'Reason: When page Index Changed of gridview make Login Details Visible false
                If Request.QueryString("pi") <> Nothing Then
                    If Not Request.QueryString("pi").Equals("0") Then
                        txt_password.Visible = False
                        'txt_password.Enabled = False
                        headrow.Visible = False
                        contentrow.Visible = False
                    End If
                End If
                '------------------------

                If Not IsPostBack Then
                    If (Session("UniUserType") = Nothing Or Session("UniUserType") = "0") Then
                    Else
                        'cmb_usertype.Enabled = True
                        'cmb_usertype.Enabled = False
                    End If
                    If Not Request.QueryString("userid") = Nothing And Session.Item("userid") = Nothing Then
                    ElseIf Request.QueryString("userid") = Nothing And Not Session.Item("userid") = Nothing Then
                    End If
                    txt_firstname.Focus()
                    If Session.Item("userid") <> Nothing And Session.Item("check") = True Then
                        If ddlCenters.SelectedIndex <> 0 Then
                            FillCourseCombo()
                        Else
                            'courserowt.Visible = False
                            'below one line only commented by rajesh
                            ' courserow.Visible = False

                            'lstCourses.Visible = False
                        End If
                    End If
                End If

                'Vaibhav Soni

                If cmb_usertype.Enabled = False And cmb_usertype.SelectedItem.Text = "Admin" Then
                    trChangePass.Visible = True
                Else
                    trChangePass.Visible = False
                End If

                'Ended by Vaibhav Soni
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                strPathDb = Nothing
                strselcity = Nothing
                strselcountry = Nothing
                strselstate = Nothing
                strisscountry = Nothing
                C_HOMEPAGEURL = Nothing
            End Try
        End Sub
        '********End of Page_Load event********
#End Region

#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Unload
            Try
                If objconn.connect() = True Then
                    objconn.disconnect()
                End If
            Catch ex As Exception
                objconn.disconnect()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try

        End Sub
#End Region

#Region "fnc_ins_data"
        Private Function fnc_ins_data() As Boolean
            Dim sqlstrr, strStateTemp, sqlTemp As String
            Dim sqlins As String       'string type variable to store the queries
            Dim MyCommand As SqlCommand                   'SqlCommand object
            Dim MyDataReader As SqlDataReader             'SqlDataReader object
            Dim var_userid As Long                          'long type variable to store the generated userid
            Dim objconn As New ConnectDb    'Object of the connectclass class
            Dim strPathDb As String
            '/***************Start,Jatin Gangajaliya,2011/04/04**********************/
            Dim Item As DictionaryEntry
            '/*******************************End************************************/

            '*********** Checking if the Database is getting connected **********
            Try
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() = True Then
                    '/****************************Start,Jatin Gangajaliya,2011/3/14******************************/
                    'added by bhumi [1/10/2015]
                    If txt_password.Text = "" Then
                        txt_password.Text = ViewState("strpassword").ToString()
                    End If
                    'Ended by bhumi
                    Dim strbr As StringBuilder
                    strbr = New StringBuilder

                    strbr.Append(" Select Count(Userid) From M_USER_INFO Where Delete_Flg=0 and ")
                    strbr.Append(" LoginName =  ")
                    strbr.Append("'")
                    strbr.Append(txt_login.Text)
                    strbr.Append("'")
                    strbr.Append(" AND Pwd =  ")
                    strbr.Append("'")
                    strbr.Append(txt_password.Text)
                    strbr.Append("'")

                    sqlstr = strbr.ToString
                    If objconn.MyConnection.State = ConnectionState.Open Then
                        MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        Dim intcount As Integer = MyCommand.ExecuteScalar()
                        '/*****************************End*******************************/
                        If intcount = 0 Then
                            '*******Select query to select the last userid********
                            sqlstr = "SELECT MAX(userid) FROM m_user_info"
                            MyCommand.Dispose()
                            MyCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                            MyDataReader = MyCommand.ExecuteReader()

                            '********While Loop to generate the userid**********
                            While MyDataReader.Read
                                If MyDataReader.IsDBNull(0) = True Then
                                    var_userid = 1
                                Else
                                    var_userid = CLng(MyDataReader.GetValue(0)) + 1
                                End If
                            End While
                            MyDataReader.Close()
                            MyCommand.Dispose()
                            objconnect.Close()
                            objconnect.Open()
                            sqlTrans = objconnect.BeginTransaction(IsolationLevel.ReadCommitted)

                            Dim filds As String
                            If cmb_usertype.SelectedItem.Text = "Admin" Then
                                filds = "LoginName,Pwd,Name,Middlename,SurName,Sex,Birthdate,Email,Telno,Permanent_Address,User_Type,Center_Id,user_photo"
                            Else
                                'Added By Vaibhav Soni
                                'Date : 2014/03/12
                                'LoginName & Pwd fields added in query. even if STUDENT selected
                                If centerrow.Visible Then
                                    filds = "LoginName,Pwd,Name,Middlename,SurName,Sex,Birthdate,Email,Telno,Permanent_Address,User_Type,Center_Id,user_photo,RollNo"
                                Else
                                    filds = "LoginName,Pwd,Name,Middlename,SurName,Sex,Birthdate,Email,Telno,Permanent_Address,User_Type,user_photo,"
                                End If
                            End If
                            sqlins = ""
                            sqlins = sqlins & "INSERT INTO m_user_info (" & filds & ") VALUES("

                            '2 Login name
                            'Commented By Vaibhav Soni
                            'Date : 2014/03/12
                            'If cmb_usertype.SelectedItem.Text = "Admin" Then
                            'Ended By Vaibhav Soni
                            sqlins = sqlins & "'" & Replace(txt_login.Text, "'", "''", 1, -1, 1) & "', "

                            '3 Password
                            sqlins = sqlins & "'" & Replace(txt_password.Text, "'", "''", 1, -1, 1) & "', "
                            'Commented By Vaibhav Soni
                            'Date : 2014/03/12
                            'End If
                            'Ended By Vaibhav Soni

                            '4 first name
                            sqlins = sqlins & "'" & Replace(txt_firstname.Text, "'", "''", 1, -1, 1) & "', "

                            '5 Middle name 
                            sqlins = sqlins & "'" & Replace(txt_middlename.Text, "'", "''", 1, -1, 1) & "', "

                            '6 Surname
                            sqlins = sqlins & "'" & Replace(txt_surname.Text, "'", "''", 1, -1, 1) & "', "

                            '7 Sex
                            sqlins = sqlins & "'" & rblist_sex.SelectedItem.Value & "', "

                            '8 DOB
                            sqlins = sqlins & "'" & txt_dob.Value & " 00:00:00', "

                            '9 Email
                            sqlins = sqlins & "'" & txt_email.Text & "', "

                            '10 Telephone No
                            sqlins = sqlins & "'" & txt_phone.Text & "', "

                            '11 Address
                            sqlins = sqlins & "'" & Replace(txtperadd.Text, "'", "''", 1, -1, 1) & "', "

                            '12 User Type
                            sqlins = sqlins & "'" & cmb_usertype.SelectedValue

                            Dim imglen As Integer = Uploader.PostedFile.ContentLength
                            Dim picbyte(imglen) As Byte
                            Uploader.PostedFile.InputStream.Read(picbyte, 0, imglen)

                            If cmb_usertype.SelectedItem.Text = "Student" Then
                                If centerrow.Visible = False Then
                                    sqlins = sqlins & "'," & "NULL" & ",@img, " & "'" & txtrollnumber.Text & "'" & " ) "
                                Else
                                    sqlins = sqlins & "'," & GetCenterIDFromControl() & ",@img, " & "'" & txtrollnumber.Text & "'" & " ) "
                                End If

                            Else
                                sqlins = sqlins & "',NULL,@img)"
                            End If

                            sqlins = Replace(sqlins, ", '',", ", NULL,", 1, -1, 1)
                            sqlins = Replace(sqlins, ", '',", ", NULL,", 1, -1, 1)
                            sqlins = Replace(sqlins, ", '')", ", NULL)", 1, -1, 1)

                            Dim ins_cmd As New SqlCommand(sqlins, objconnect, sqlTrans)
                            ins_cmd.Parameters.Add("@img", picbyte)
                            ins_cmd.ExecuteNonQuery()

                            '/********************End,By: Jatin Gangajaliya*********************/

                            Session.Add("userid", var_userid) 'Code added  by Kamal on 2006/02/14

                            ' below code Commented by Rajesh Nagvanshi 2014/08/19
                            'Reason : Not inserting the Course at the time of Student Registration .

                            'If cmb_usertype.SelectedItem.Text = "Student" Then
                            If ddlCenters.SelectedIndex <> 0 Then
                                'First find out the value of the Courses 

                                Dim selectedvalues As ArrayList = getCoursesList()
                                'Dim arr() As String = selectedvalues.Substring(0, selectedvalues.Length - 1).Split(",")

                                'Dim query2 As String = "Delete from t_user_course where user_id='" & Convert.ToInt32(Session.Item("userid")) & "' "
                                'Dim del_cmd As New SqlCommand(query2, objconnect, sqlTrans)
                                'del_cmd.ExecuteNonQuery()

                                'For dk As Integer = 0 To arr.Length - 1
                                For Each _CourseID As Integer In selectedvalues

                                    '/*****************Start,Jatin Gangajaliya,2011/04/04*****************/

                                    Dim arylst As New ArrayList
                                    Dim testary() As Integer
                                    Dim testtype As New Hashtable

                                    Dim strquery As String = " Select test_type from M_Weightage where Course_ID = '" & _CourseID & "' "
                                    Dim cmd As New SqlCommand(strquery, objconnect, sqlTrans)
                                    MyDataReader = cmd.ExecuteReader()
                                    While MyDataReader.Read()
                                        arylst.Add(MyDataReader.Item("test_type"))
                                    End While
                                    MyDataReader.Close()
                                    testary = arylst.ToArray(System.Type.GetType("System.Int32"))
                                    For g As Integer = 0 To testary.Length - 1
                                        testtype.Add(testary(g), _CourseID)
                                    Next


                                    Dim intsubweigtage As Integer
                                    Dim intsingle As Integer
                                    Dim intmultichoise As Integer
                                    Dim intblanks As Integer
                                    Dim intbasic As Integer
                                    Dim intintermediate As Integer

                                    For Each Item In testtype
                                        Dim booltemp As Boolean = True
                                        Dim strq As String = " Select Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed from M_Weightage where test_type = " & Item.Key & " and Course_ID = " & Item.Value
                                        Dim ins_cmd3 As New SqlCommand(strq, objconnect, sqlTrans)
                                        Dim rdr As SqlDataReader
                                        rdr = ins_cmd3.ExecuteReader()
                                        If booltemp = True Then
                                            rdr.Read()
                                            If Not IsDBNull(rdr.Item("Sub_Weightage")) Then
                                                intsubweigtage = rdr.Item("Sub_Weightage")
                                            End If
                                            If Not IsDBNull(rdr.Item("Single")) Then
                                                intsingle = rdr.Item("Single")
                                            End If
                                            If Not IsDBNull(rdr.Item("Multi_Choice")) Then
                                                intmultichoise = rdr.Item("Multi_Choice")
                                            End If
                                            If Not IsDBNull(rdr.Item("Blanks")) Then
                                                intblanks = rdr.Item("Blanks")
                                            End If
                                            If Not IsDBNull(rdr.Item("Basic")) Then
                                                intbasic = rdr.Item("Basic")
                                            End If
                                            If Not IsDBNull(rdr.Item("InterMed")) Then
                                                intintermediate = rdr.Item("InterMed")
                                            End If
                                            rdr.Close()
                                            InsertIntoUserCourse(Convert.ToInt32(Session.Item("userid")), Item.Value, Item.Key, intsubweigtage, intsingle, intmultichoise, intblanks, intbasic, intintermediate)
                                            booltemp = False
                                        End If
                                    Next

                                    '/*********************************End********************************/
                                    arylst = Nothing
                                    testary = Nothing
                                    testtype = Nothing
                                Next
                            End If
                            'End If

                            sqlTrans.Commit()
                            objconnect.Close()
                            MyCommand.Dispose()
                            objconn.disconnect()
                            fnc_ins_data = True
                        Else
                            lblMsg.Visible = True
                            lblMsg.Text = Resources.Resource.Registration_existuser
                            fnc_ins_data = False
                        End If
                    End If
                Else
                    Response.Redirect("error.aspx?err=register.aspx.vb" & CONS.ERR_DBCON, False)
                End If
            Catch ex As Exception
                sqlTrans.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                sqlstrr = Nothing
                sqlins = Nothing
                MyCommand = Nothing
                MyDataReader = Nothing
                var_userid = Nothing
                strPathDb = Nothing
                objconn = Nothing
                Item = Nothing
            End Try
        End Function
#End Region

#Region "Enter data into t_user_course"
        'Desc:insert data into t_user_course.
        'By: Jatin Gangajaliya, 2011/04/23.

        Private Sub InsertIntoUserCourse(ByVal intuserid As Integer, ByVal intcourseid As Integer, ByVal inttest As Integer, ByVal intsubweigtage As Integer, ByVal intsingle As Integer, ByVal intmultichoise As Integer, ByVal intblanks As Integer, ByVal intbasic As Integer, ByVal intintermediate As Integer)
            Dim sb As StringBuilder
            Dim querystr As String
            'Dim sqlTrans As SqlTransaction
            Try
                sb = New StringBuilder
                sb.Append(" Insert into t_user_course ")
                sb.Append(" (User_id,course_id,test_type,Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed) ")
                sb.Append(" Values( ")
                sb.Append(intuserid)
                sb.Append(" , ")
                sb.Append(intcourseid)
                sb.Append(" , ")
                sb.Append(inttest)
                sb.Append(" , ")
                sb.Append(intsubweigtage)
                sb.Append(" , ")
                sb.Append(intsingle)
                sb.Append(" , ")
                sb.Append(intmultichoise)
                sb.Append(" , ")
                sb.Append(intblanks)
                sb.Append(" , ")
                sb.Append(intbasic)
                sb.Append(" , ")
                sb.Append(intintermediate)
                sb.Append(" ) ")
                querystr = sb.ToString()
                Dim ins_cmd1 As New SqlCommand(querystr, objconnect, sqlTrans)
                ins_cmd1.ExecuteNonQuery()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region
        '******* Function to update the Different user tables******
#Region "fnc_update"
        Private Sub fnc_update()

            Dim sqlUpdate As String                 'String type variables to store queries
            Dim MyCommand As SqlCommand                   'SqlClientcommad object
            Dim objconn As New ConnectDb    'object of connectclass class
            Dim strPathDb, sqlTemp, strStateTemp As String
            '/***************Start,Jatin Gangajaliya,2011/04/04**********************/
            Dim Item As DictionaryEntry
            Dim strbr As New StringBuilder()
            Dim strBrClass As New StringBuilder()
            Dim strBrPassword As New StringBuilder()
            Dim sqlrd As SqlDataReader
            Dim classid As String
            '/*******************************End************************************/

            objconnect.Open()
            sqlTrans = objconnect.BeginTransaction(IsolationLevel.ReadCommitted)

            Try

                If Session.Item("userid") = Nothing Then
                    'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                    If objconn.connect() = True Then

                        '****** The Update query******
                        sqlUpdate = ""
                        sqlUpdate = sqlUpdate & "UPDATE m_user_info SET "
                        sqlUpdate = sqlUpdate & "name = '" & Replace(txt_firstname.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "middlename = '" & Replace(txt_middlename.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "surname = '" & Replace(txt_surname.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "sex = '" & rblist_sex.SelectedItem.Value & "', "
                        sqlUpdate = sqlUpdate & "birthdate = '" & txt_dob.Value & " 00:00:00', "
                        sqlUpdate = sqlUpdate & "email = '" & txt_email.Text & "', "
                        sqlUpdate = sqlUpdate & "telno = '" & txt_phone.Text & "', "

                        sqlUpdate = sqlUpdate & "Permanent_Address = '" & Replace(txtperadd.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "update_date = '" & DateTime.Today & "' WHERE "
                        sqlUpdate = sqlUpdate & "delete_flg = 'A'"

                        sqlUpdate = Replace(sqlUpdate, "= '',", "= NULL,", 1, -1, 1)
                        sqlUpdate = Replace(sqlUpdate, "= '' WHERE", "= NULL WHERE", 1, -1, 1)

                        MyCommand = New SqlCommand(sqlUpdate, objconnect, sqlTrans)

                        MyCommand.ExecuteNonQuery()

                        MyCommand.Dispose()
                        objconn.disconnect()
                    Else
                        Response.Redirect("error.aspx?err=register.aspx.vb" & CONS.ERR_DBCON, False)
                    End If
                Else
                    'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                    If objconn.connect() = True Then

                        '****** The Update query******
                        sqlUpdate = ""
                        sqlUpdate = sqlUpdate & "UPDATE m_user_info SET "
                        sqlUpdate = sqlUpdate & "name = '" & Replace(txt_firstname.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "middlename = '" & Replace(txt_middlename.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "surname = '" & Replace(txt_surname.Text, "'", "''", 1, -1, 1) & "', "
                        sqlUpdate = sqlUpdate & "sex = '" & rblist_sex.SelectedItem.Value & "', "
                        sqlUpdate = sqlUpdate & "birthdate = '" & txt_dob.Value & " 00:00:00', "
                        sqlUpdate = sqlUpdate & "email = '" & txt_email.Text & "', "
                        sqlUpdate = sqlUpdate & "telno = '" & txt_phone.Text & "', "

                        sqlUpdate = sqlUpdate & "Permanent_Address = '" & txtperadd.Text & "', "
                        'commented by bhumi[24/9/2015]
                        'sqlUpdate = sqlUpdate & "Center_ID = " & GetCenterIDFromControl() & ", "
                        'Ended by bhumi
                        sqlUpdate = sqlUpdate & "update_date = '" & DateTime.Today & "' WHERE "

                        sqlUpdate = sqlUpdate & "userid = " & IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid"))

                        sqlUpdate = Replace(sqlUpdate, "= '',", "= NULL,", 1, -1, 1)
                        sqlUpdate = Replace(sqlUpdate, "= '' WHERE", "= NULL WHERE", 1, -1, 1)

                        MyCommand = New SqlCommand(sqlUpdate, objconnect, sqlTrans)
                        MyCommand.ExecuteNonQuery()

                        If Uploader.FileName.ToString() <> "" Then

                            Dim imglen As Integer = Uploader.PostedFile.ContentLength
                            Dim picbyte(imglen) As Byte
                            Uploader.PostedFile.InputStream.Read(picbyte, 0, imglen)
                            objconn.connect()
                            If objconn.MyConnection.State = ConnectionState.Open Then
                                objconn.MyConnection.Close()
                            End If
                            objconn.MyConnection.Open()

                            Dim cmd As New SqlCommand("Update m_user_info SET user_photo = @img where userid='" & IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid")) & "'", objconnect, sqlTrans)

                            cmd.Parameters.AddWithValue("@img", picbyte)
                            cmd.ExecuteNonQuery()
                        End If

                        '/**********************Start,Jatin Gangajaliya,2011/04/04*******************/

                        If cmb_usertype.SelectedItem.Text = "Student" Then
                            '''<summary>
                            ''' Delete Query Removed [commented]
                            '''Description: After User Details Updation, User get Invisible while searching for assigning Exam
                            '''By Bhumi,2015/08/04
                            '''</summary>
                            'sqlUpdate = "Delete From T_User_Course Where User_ID ='" & IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid")) & "' and Del_Flag = 0"
                            'If objconn.MyConnection.State = ConnectionState.Closed Then
                            '    objconn.MyConnection.Open()
                            'End If
                            'Dim cmd_UP As New SqlCommand(sqlUpdate, objconnect, sqlTrans)
                            'cmd_UP.ExecuteNonQuery()
                            ' whole section commented by rajesh nagvanshi 2014/08/19
                            'Dim selectedvalues As String = GetCoursesFromControl()
                            'this Condition Added by rajesh 
                            'If selectedvalues.Length > 0 Then
                            '    Dim arr() As String = selectedvalues.Substring(0, selectedvalues.Length - 1).Split(",")


                            '    For dk As Integer = 0 To arr.Length - 1

                            '        Dim arylst As New ArrayList
                            '        Dim testary() As Integer
                            '        Dim testtype As New Hashtable

                            '        Dim strquery As String = " Select test_type from M_Weightage where Course_ID = '" & arr(dk) & "' "
                            '        Dim cmd As New SqlCommand(strquery, objconnect, sqlTrans)
                            '        myDataReader = cmd.ExecuteReader()
                            '        arylst = New ArrayList()
                            '        While myDataReader.Read()
                            '            arylst.Add(myDataReader.Item("test_type"))
                            '        End While
                            '        myDataReader.Close()
                            '        testary = arylst.ToArray(System.Type.GetType("System.Int32"))
                            '        Dim booldecision As Boolean = True
                            '        For g As Integer = 0 To testary.Length - 1
                            '            strbr = New StringBuilder()
                            '            strbr.Append(" select User_ID from T_User_Course where User_ID = ")
                            '            strbr.Append(IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid")))
                            '            strbr.Append(" and Course_ID = ")
                            '            strbr.Append(arr(dk))
                            '            strbr.Append(" and Test_type = ")
                            '            strbr.Append(testary(g))
                            '            strbr.Append(" and Del_Flag = 1 ")
                            '            strquery = strbr.ToString()
                            '            Dim cmddel As New SqlCommand(strquery, objconnect, sqlTrans)
                            '            myDataReader = cmddel.ExecuteReader()
                            '            While myDataReader.Read()
                            '                booldecision = False
                            '            End While
                            '            myDataReader.Close()

                            '            If booldecision = True Then

                            '                testtype.Add(testary(g), arr(dk))

                            '            End If
                            '        Next

                            '        Dim querystr As String
                            '        Dim intsubweigtage As Integer
                            '        Dim intsingle As Integer
                            '        Dim intmultichoise As Integer
                            '        Dim intblanks As Integer
                            '        Dim intbasic As Integer
                            '        Dim intintermediate As Integer
                            '        For Each Item In testtype
                            '            Dim booltemp As Boolean = True
                            '            Dim strq As String = " Select Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed from M_Weightage where test_type = " & Item.Key & " and Course_ID = " & Item.Value
                            '            Dim ins_cmd3 As New SqlCommand(strq, objconnect, sqlTrans)
                            '            Dim rdr As SqlDataReader
                            '            rdr = ins_cmd3.ExecuteReader()
                            '            If booltemp = True Then
                            '                rdr.Read()

                            '                If Not IsDBNull(rdr.Item("Sub_Weightage")) Then
                            '                    intsubweigtage = rdr.Item("Sub_Weightage")
                            '                End If
                            '                If Not IsDBNull(rdr.Item("Single")) Then
                            '                    intsingle = rdr.Item("Single")
                            '                End If
                            '                If Not IsDBNull(rdr.Item("Multi_Choice")) Then
                            '                    intmultichoise = rdr.Item("Multi_Choice")
                            '                End If
                            '                If Not IsDBNull(rdr.Item("Blanks")) Then
                            '                    intblanks = rdr.Item("Blanks")
                            '                End If
                            '                If Not IsDBNull(rdr.Item("Basic")) Then
                            '                    intbasic = rdr.Item("Basic")
                            '                End If
                            '                If Not IsDBNull(rdr.Item("InterMed")) Then
                            '                    intintermediate = rdr.Item("InterMed")
                            '                End If

                            '                Dim userid As Integer = IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid"))
                            '                rdr.Close()
                            '                InsertIntoUserCourse(userid, Item.Value, Item.Key, intsubweigtage, intsingle, intmultichoise, intblanks, intbasic, intintermediate)
                            '                booltemp = False
                            '            End If
                            '        Next

                            '        testary = Nothing
                            '        arylst = Nothing
                            '        testtype = Nothing
                            '    Next
                            'End If 'by rajesh
                        End If
                        sqlTrans.Commit()
                        MyCommand.Dispose()
                        'added by bhumi[30/9/2015]
                        'Reason: whenever Class of student is updated Insertion in M_User_Info & T_user_Course
                        If objconn.connect() = True Then
                            strBrClass.Append("select Center_ID from M_USER_INFO where Userid=")
                            strBrClass.Append(IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid")))
                            Dim CmdstrBrClass As New SqlCommand(strBrClass.ToString, objconnect)
                            sqlrd = CmdstrBrClass.ExecuteReader()
                            sqlrd.Read()
                            classid = sqlrd.Item(0)
                            sqlrd.Close()
                            If String.Equals(classid, GetCenterIDFromControl()) Then
                            Else
                                Dim cmd_flag As New SqlCommand("Update m_user_info SET Delete_Flg=1 where userid='" & IIf(Request.QueryString("userid") <> Nothing, Request.QueryString("userid"), Session.Item("userid")) & "'", objconnect)
                                cmd_flag.ExecuteNonQuery()
                                fnc_ins_data()
                            End If
                        End If
                        'Ended by bhumi
                    Else
                        Response.Redirect("error.aspx?err=register.aspx.vb" & CONS.ERR_DBCON, False)
                    End If
                    objconnect.Close()
                    objconn.disconnect()
                End If

            Catch ex As Exception
                sqlTrans.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                objconn = Nothing
                MyCommand = Nothing
                strPathDb = Nothing
                sqlUpdate = Nothing

                Item = Nothing
            End Try
        End Sub
#End Region
        '**********The Event called on clicking img_update button********
#Region "img_update_Click"
        Private Sub img_update_Click(sender As Object, e As EventArgs) Handles img_update.Click
            '*******Checking if the page is valid******
            Dim sqlTrans As SqlTransaction
            Try
                If (Page.IsValid) Then


                    LblMsg1.Visible = False

                    If Not PageValidation() Then
                        Exit Sub
                    End If
                    '***********End Code *****************

                    If (CStr(Session.Item("userid")) <> "" And Not IsDBNull(Session.Item("userid"))) _
                        Or Request.QueryString("userid") <> "" Then

                        '/************************Start,Jatin Gangajaliya,2011/04/04************************/
                        Try
                            If objconn.connect() = True Then
                                sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                                fnc_update()
                            End If
                        Catch ex As Exception
                            sqlTrans.Rollback()
                            If log.IsDebugEnabled Then
                                log.Debug("Error :" & ex.ToString())
                            End If
                        Finally
                            'sqlTrans.Commit()
                        End Try
                        '/**********************************End********************************************/                       

                        lblMsg.Visible = True
                        lblMsg.Text = Resources.Resource.StudentSearch_recupdated

                        lblMsg.ForeColor = Color.Green 'Added by rajesh                                                 
                        ' Session.Item("userid") = Nothing 'Added by rajesh
                        If (Session.Item("check") = "true") Then
                            Response.Redirect("StudentSearch.aspx", False)
                            Session.Add("Bool", "true")
                        End If

                        If (Session.Item("check") = "true" And Session.Item("check2") = "true") Then
                            Response.Redirect("AdminList.aspx", False)
                            Session.Add("Bool", "true")
                            Session.Item("check2") = Nothing
                        End If


                        Session.Remove("check")

                    Else
                        Response.Redirect("login.aspx", False)
                    End If
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                sqlTrans = Nothing
            End Try
        End Sub
#End Region
        '**********The Event called on clicking img_saveexit Image button********
#Region "img_saveexit_Click"
        Private Sub img_saveexit_Click(sender As Object, e As EventArgs) Handles img_saveexit.Click
            Dim objMailfunction As CommonFunction
            Dim strMessage As String
            '*******Checking if the page is valid******
            Try
                If (Page.IsValid) Then
                    '***********Created By Aalok Parikh **************
                    'Date : 2011/03/15
                    'Desc. : Validation through serverside code
                    '*************************************************
                    If Not PageValidation() Then
                        Exit Sub
                    End If
                    '***********End Code *****************

                    '******Calling function to insert data *******
                    If fnc_ins_data() = True Then
                        Session.Remove("userid")
                        Session.Add("success", "true")
                        objMailfunction = New CommonFunction()
                        strMessage = objMailfunction.ReadFile(Server.MapPath(ConfigurationSettings.AppSettings("Registration")))
                        strMessage = strMessage.Replace("%Name%", txt_firstname.Text & " " & txt_middlename.Text & " " & txt_surname.Text)
                        strMessage = strMessage.Replace("%LoginID%", txt_login.Text)
                        strMessage = strMessage.Replace("%Password%", txt_password.Text)
                        objMailfunction.sendMail(txt_email.Text, "", strMessage, ConfigurationSettings.AppSettings("Subject"))
                        MsgBox(Resources.Resource.Registration_msgbox)
                        Response.Redirect("register.aspx", False)
                    Else
                        End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                objMailfunction = Nothing
                strMessage = String.Empty
            End Try
        End Sub
#End Region

#Region "Page Validation"
        '***********Created By Aalok Parikh **************
        'Date : 2011/03/15
        'Desc. : Validation through serverside code
        '*************************************************
        Function PageValidation() As Boolean
            '********First Name Validation***************
            If Not ReqTextFieldCheck(txt_firstname.Text.Trim()) Then
                If txt_firstname.Text.Trim() = "" Then
                    lblMsg.Text = Resources.Resource.CenterRegistration_fnerr
                    lblMsg.Visible = True
                    txt_firstname.Focus()
                    Return False
                    Exit Function
                Else
                    lblMsg.Text = Resources.Resource.Registration_vldalpha
                    lblMsg.Visible = True
                    txt_firstname.Focus()
                    Return False
                    Exit Function
                End If
            End If
            '*********************************************
            '********Middle Name Validation***************
            If Not TextFieldCheck(txt_middlename.Text.Trim()) Then
                lblMsg.Text = Resources.Resource.Registration_vldalpha
                lblMsg.Visible = True
                txt_middlename.Focus()
                Return False
                Exit Function
            End If
            '*********************************************
            '********Last Name Validation*****************
            If Not ReqTextFieldCheck(txt_surname.Text.Trim()) Then
                If txt_surname.Text.Trim() = "" Then
                    lblMsg.Text = Resources.Resource.CenterRegistration_lnerr
                    lblMsg.Visible = True
                    txt_surname.Focus()
                    Return False
                    Exit Function
                Else
                    lblMsg.Text = Resources.Resource.Registration_vldalpha
                    lblMsg.Visible = True
                    txt_surname.Focus()
                    'txt_surname.Text = ""
                    Exit Function
                    Return False
                End If
            End If
            '*********************************************
            '*********Date Of Birth Validation************
            'If Not dateCheck(txt_dob.Text.Trim()) Then
            '    txt_dob.Focus()
            '    Exit Function
            '    Return False
            'End If

            '*************Email Address********************
            If Not EmailAddressCheck(txt_email.Text.Trim()) Then
                If txt_email.Text.Trim() = "" Then
                    lblMsg.Text = Resources.Resource.Registration_emailvalid
                    lblMsg.Visible = True
                    txt_email.Focus()
                    Return False
                    Exit Function
                Else
                    lblMsg.Text = Resources.Resource.Registration_validemail
                    lblMsg.Visible = True
                    txt_email.Focus()
                    Return False
                    Exit Function
                End If

            End If


            '*********************************************
            '*********Mobile Number Validation************
            If txt_phone.Text.Trim().Length < 10 And txt_phone.Text.Trim().Length > 0 Then
                If Not NumberFieldCheck(txt_phone.Text.Trim()) Then
                    lblMsg.Text = Resources.Resource.QuestionAns_Digerr
                    lblMsg.Visible = True
                    txt_phone.Focus()
                    Return False
                    Exit Function
                Else
                    lblMsg.Text = Resources.Resource.Registration_contactno
                    lblMsg.Visible = True
                    txt_phone.Focus()
                    txt_phone.Text = ""
                    Return False
                    Exit Function
                End If
            Else
                If Not NumberFieldCheck(txt_phone.Text.Trim()) Then
                    lblMsg.Text = Resources.Resource.QuestionAns_Digerr
                    lblMsg.Visible = True
                    txt_phone.Focus()
                    Return False
                    Exit Function
                End If
            End If
            '*********************************************
            '*********Address Details Validation**********
            If txtperadd.Text.Trim().Length > 100 Then
                lblMsg.Text = Resources.Resource.Registration_addvalid
                lblMsg.Visible = True
                txtperadd.Focus()
                Return False
                Exit Function
            End If

            '***********Added By Pranit Chimurkar ***************
            'Date : 2019/10/16
            'Desc. : File Upload validation for photo
            '******************************************************
            Dim ext = Uploader.FileName
            Dim extension As String = System.IO.Path.GetExtension(ext)
            If ext = "" Then

            ElseIf ((extension <> ".jpeg") And (extension <> ".jpg") And (extension <> ".png") And (extension <> ".bmp") And (extension <> ".tiff")) Then
                lblMsg.Text = Resources.Resource.Registration_invldpicerr
                lblMsg.Visible = True
                Uploader.Focus()
                Return False
                Exit Function
            End If
            '***********End Code **********************************

            'Added by Indravadan vasava (2011/05/27)
            '*********************************************
            '*********Roll Number Validation**********
            If cmb_usertype.SelectedItem.Text <> "Admin" Then
                If txtrollnumber.Text.Trim() = "" Then
                    lblMsg.Text = Resources.Resource.Registration_rollnoval
                    lblMsg.Visible = True
                    txtrollnumber.Focus()
                    Return False
                    Exit Function
                End If
            End If


            '*********************************************
            '*********Select Center Validation************
            'Commented code removed by bhumi [15/9/2015]
            'Reason: Center is mandatory field for students
            If (cmb_usertype.SelectedItem.Text = "Student" And GetCenterIDFromControl() = "0") Then
                lblMsg.Text = Resources.Resource.Search_PlSelClName
                lblMsg.Visible = True
                ddlCenters.Focus()
                Return False
                Exit Function
            End If
            'Ended by bhumi
            '*********************************************
            '*********Select Course Validation************
            'If (cmb_usertype.SelectedItem.Text = "Student" And GetCoursesFromControl() = "") Then
            '    lblMsg.Text = "Please select at least one Course."
            '    lblMsg.Visible = True
            '    lstCourses.Focus()
            '    Return False
            '    Exit Function
            'End If
            '*********************************************
            '*********Login ID Validation*****************

            'If cmb_usertype.SelectedItem.Text = "Admin" Then
            ' Visible  Condition Added by Rajesh
            If txt_login.Visible = True Then
                If txt_login.Text.Trim() = "" Then
                    lblMsg.Text = Resources.Resource.Registration_loginid
                    lblMsg.Visible = True
                    txt_login.Focus()
                    Return False
                    Exit Function
                End If
            End If
            If Not ReqField(txt_login.Text.Trim()) Then
                lblMsg.Text = Resources.Resource.Registration_loginidvalid
                lblMsg.Visible = True
                txt_login.Focus()
                Return False
                Exit Function
            End If

            If txt_password.Text.Trim() = "" And txt_password.Enabled = True And txt_password.Visible = True Then
                lblMsg.Text = Resources.Resource.Registration_passd
                lblMsg.Visible = True
                txt_password.Focus()
                Return False
                Exit Function
            End If

            If txt_confpassword.Text.Trim() = "" And txt_confpassword.Enabled = True And txt_confpassword.Visible = True Then
                lblMsg.Text = Resources.Resource.Registration_pswdcnpsd
                lblMsg.Visible = True
                txt_password.Focus()
                Return False
                Exit Function
            Else
                If txt_confpassword.Text.Trim() <> txt_password.Text.Trim() Then
                    lblMsg.Text = Resources.Resource.Registration_pswdcnfpswdvalid
                    lblMsg.Visible = True
                    txt_password.Focus()
                    Return False
                    Exit Function
                End If
            End If
            'End If
            Return True
            lblMsg.Visible = False
        End Function
#End Region
        '**** Required Validation Functions ********
        'Created By : Aalok Parikh
        'Date: 2011/03/15

#Region "Email Address Validation"
        Function EmailAddressCheck(ByVal emailAddress As String) As Boolean
            Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
            Dim emailAddressMatch As Match = Regex.Match(emailAddress, pattern)
            If emailAddressMatch.Success Then
                EmailAddressCheck = True
            Else
                EmailAddressCheck = False
            End If
        End Function
#End Region

#Region "Required Text Field Validtion"
        Function ReqTextFieldCheck(ByVal Field As String) As Boolean
            Dim pattern As String = "^[a-zA-Z]{1,50}$"
            Dim reqTextFieldMatch As Match = Regex.Match(Field, pattern)
            If reqTextFieldMatch.Success Then
                ReqTextFieldCheck = True
            Else
                ReqTextFieldCheck = False
            End If
        End Function
#End Region

#Region "Text Field Validation"
        Function TextFieldCheck(ByVal Field As String) As Boolean
            Dim pattern As String = "^[a-zA-Z]{0,50}$"
            Dim textFieldMatch As Match = Regex.Match(Field, pattern)
            If textFieldMatch.Success Then
                TextFieldCheck = True
            Else
                TextFieldCheck = False
            End If
        End Function
#End Region

#Region "Number Field Validation"
        Function NumberFieldCheck(ByVal Field As String) As Boolean
            Dim pattern As String = "^[0-9]*$"
            Dim numberFieldMatch As Match = Regex.Match(Field, pattern)
            If numberFieldMatch.Success Then
                NumberFieldCheck = True
            Else
                NumberFieldCheck = False
            End If
        End Function
#End Region

#Region "Date Field Validation"
        Function dateCheck(ByVal dateOfBirth As String) As Boolean
            Dim dt1 As Date
            Dim dt2 As Date
            Const REGULAR_EXP = "(^[0-9]{4,4}/?[0-1][0-9]/?[0-3][0-9]$)"
            Dim flag As Boolean
            flag = True
            If dateOfBirth <> "" Then

                'If Not Regex.IsMatch(dateOfBirth, REGULAR_EXP) Then
                '    lblMsg.Text = "Please enter date of birth in YYYY/MM/DD format."
                '    lblMsg.Visible = True
                '    flag = False
                '    Return False
                '    If Not IsDate(dateOfBirth) And flag Then
                '        lblMsg.Text = "Please enter valid date of birth."
                '        lblMsg.Visible = True
                '        flag = False
                '        Return False
                '    End If
                'Else
                '    If IsDate(dateOfBirth) Then
                '        dt2 = Now()
                '        dt1 = Convert.ToDateTime(dateOfBirth)
                '        If dt1 > dt2 Then
                '            lblMsg.Text = "Please enter date of birth less then current date."
                '            lblMsg.Visible = True
                '            flag = False
                '            Return False
                '        End If
                '    Else
                '        lblMsg.Text = "Please enter valid date of birth."
                '        lblMsg.Visible = True
                '        flag = False
                '        Return False
                '    End If
                'End If
                'Return True

                Return True
            Else
                lblMsg.Text = Resources.Resource.Registration_DOBvalid
                lblMsg.Visible = True
                Return False
            End If
        End Function

#End Region

#Region "Required Field Validation"
        Function ReqField(ByVal Field As String) As Boolean
            Dim pattern As String = "^[a-zA-Z0-9]*$"
            Dim reqFieldMatch As Match = Regex.Match(Field, pattern)
            If reqFieldMatch.Success Then
                ReqField = True
            Else
                ReqField = False
            End If
        End Function
#End Region

#Region "img_cancel_Click"
        Private Sub img_cancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            Try
                Session.Remove("userid")
                Response.Redirect("login.aspx", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try
        End Sub
#End Region

#Region "lnk_chpass_Click"
        Private Sub lnk_chpass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
            Handles lnk_chpass.Click
            Try

                If Request.QueryString("ip").ToString() <> Nothing Then
                    Response.Redirect("changepass.aspx?ip=" & Request.QueryString("ip").ToString(), False)
                Else
                    Response.Redirect("changepass.aspx", False)
                End If







            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try
        End Sub
#End Region

#Region "IsValidPhoneNo"
        Public Function IsValidPhoneNo(ByVal strphoneno As String) As Boolean
            Dim strpattern As String
            Dim blnvalid As Boolean
            Try
                strpattern = "(\()?\d{3,5}(\))?-\d{6,8}"
                Dim Regexp As New Regex(strpattern)
                blnvalid = Regexp.IsMatch(strphoneno.Trim())

                Return blnvalid
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                strpattern = Nothing
                blnvalid = Nothing

            End Try
        End Function
#End Region

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
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                iCnt = Nothing
                sChar = Nothing
                sEncode = Nothing
            End Try
        End Function
#End Region

#Region "GetTestDetail"
        Private Function GetTestDetail()

            Dim ResultRow As HtmlTableRow
            Dim ResultCol As HtmlTableCell
            Dim links As HyperLink
            Dim sqlstr As String
            Try
                If ((Not Request.QueryString("userid") <> 0) Or (Session.Item("userid") <> 1)) Then
                    sqlstr = "SELECT M_Course.Course_ID, M_Course.Course_Name As Course_Name,t_candidate_status.appearedflag FROM t_candidate_status,M_Course  "
                    sqlstr = sqlstr & " WHERE userid = " & Session.Item("userid") & Request.QueryString("userid") & " "
                    sqlstr = sqlstr & " and M_Course.Course_ID = t_candidate_status.Course_ID"
                    objconn.connect()
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader
                    Dim C_HOMEPAGEURL As String

                    While myDataReader.Read
                        Dim appearflag As Integer = myDataReader.Item("appearedflag")
                        ResultRow = New HtmlTableRow
                        ResultCol = New HtmlTableCell
                        links = New HyperLink
                        links.NavigateUrl = C_HOMEPAGEURL & "ExaminationInfo.aspx?lnks=" & C_HOMEPAGEURL & "QuestionPaper.aspx?usr=" _
                        & sEncodeString(Request.QueryString("userid") & "|" & Session.Item("userid") & "|" &
                        myDataReader.Item("Course_ID"))

                        If appearflag <> 2 Then
                            links.Text = myDataReader.Item("Course_Name")
                        Else
                            links.Text = myDataReader.Item("Course_Name")
                            links.Enabled = False
                        End If

                        ResultCol.Controls.Add(links)
                        ResultRow.Cells.Add(ResultCol)
                        ResultRow.Align = "left"
                        'Added by Pranit on 2019/10/15
                        tablediv.Visible = True
                        tblResult.Rows.Add(ResultRow)
                    End While
                End If
                myDataReader.Close()
                myCommand.Dispose()
                objconn.disconnect()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                ResultCol = Nothing
                ResultRow = Nothing
                links = Nothing
                sqlstr = Nothing
            End Try
        End Function
#End Region

#Region "GetCertificationCombo"
        Private Function GetCertificationCombo()
            Try
                If (Len(Session.Item("userid")) <> 0 Or Request.QueryString("userid") <> 0) Then
                    Session.Add("userType", "new")
                Else
                    Session.Add("userType", "old")
                End If
                sqlstr2 = "SELECT certi_name, certi_id FROM m_certi_master order by certi_id"
                If objconn.connect() Then
                    myCommand = New SqlCommand(sqlstr2, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read()
                    End While
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try
        End Function
#End Region

#Region "GetPersonalData"
        Private Function GetPersonalData()

            Dim strselcountry As String
            Dim strselstate As String
            Dim strselcity As String
            Dim strlandlineno As String
            Dim strisscountry As String
            Dim sqlstr As String
            Try
                strPathDb = ConfigurationSettings.AppSettings("PathDb")

                If Request.QueryString("userid") <> 0 Then
                    sqlstr = "SELECT * FROM m_user_info WHERE userid = " & Request.QueryString("userid")
                    Session.Item("userid") = Request.QueryString("userid")
                Else
                    sqlstr = "SELECT * FROM m_user_info WHERE userid = " & Session.Item("userid")
                End If
                If objconn.connect() Then
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    If myDataReader.Read() Then

                        If Not IsDBNull(myDataReader.Item("loginname")) Then
                            txt_login.Text = myDataReader.Item("loginname")
                            'txt_login.Enabled = False
                        End If
                        If Not IsDBNull(myDataReader.Item("pwd")) Then
                            txt_password.Text = myDataReader.Item("pwd")
                            ViewState("strpassword") = txt_password.Text
                            'txt_password.Enabled = False
                        End If
                        'txt_password.Visible = False
                        txt_confpassword.Visible = False
                        lbl_confpassword.Visible = False
                        star.Visible = False

                        lnk_chpass.Visible = True
                        txt_firstname.Text = myDataReader.Item("name")
                        If Not IsDBNull(myDataReader.Item("middlename")) Then
                            txt_middlename.Text = myDataReader.Item("middlename")
                        Else
                            txt_middlename.Text = ""
                        End If

                        If Not IsDBNull(myDataReader.Item("surname")) Then txt_surname.Text = myDataReader.Item("surname")
                        If Not IsDBNull(myDataReader.Item("email")) Then txt_email.Text = myDataReader.Item("email")
                        If Not IsDBNull(myDataReader.Item("telno")) Then
                            txt_phone.Text = myDataReader.Item("telno")
                        Else
                            txt_phone.Text = ""
                        End If

                        If Not IsDBNull(myDataReader.Item("Permanent_Address")) Then
                            txtperadd.Text = myDataReader.Item("Permanent_Address")
                        Else
                            txtperadd.Text = ""
                        End If

                        If myDataReader.Item("sex") = "M" Then
                            rblist_sex.SelectedIndex = 0
                        Else
                            rblist_sex.SelectedIndex = 1
                        End If


                        If Not IsDBNull(myDataReader.Item("birthdate")) Then txt_dob.Value = ConvertDate(Format(myDataReader.Item("birthdate"), "yyyy/MM/dd"))
                        '/*************Start,Jatin Gangajaliya,2011/3/14******************/
                        If Not IsDBNull(myDataReader.Item("User_Type")) Then
                            If myDataReader.Item("User_Type") = 0 Then
                                cmb_usertype.SelectedValue = 0
                                ddlCenters.Enabled = True
                                centerrow.Visible = True
                                ' courserowt.Visible = True
                                'lstCourses.Visible = True
                                'courserow.Visible = True
                                'trSpace2.Visible = True
                            ElseIf myDataReader.Item("User_Type") = 1 Then
                                cmb_usertype.SelectedValue = 1
                                ddlCenters.Enabled = False
                                centerrow.Visible = False
                                ' courserowt.Visible = False
                                ' lstCourses.Visible = False
                                'courserow.Visible = False
                                rollnumber.Visible = False
                                'trSpace2.Visible = False
                            End If
                        End If

                        If Not IsDBNull(myDataReader.Item("RollNo")) Then
                            txtrollnumber.Text = myDataReader.Item("RollNo")
                        Else
                            txtrollnumber.Text = ""
                        End If

                    End If
                    myDataReader.Close()
                    myCommand.Dispose()
                    objconn.disconnect()

                    If Not strlandlineno = "" Then
                        Dim arrLandline(3) As String
                        arrLandline = strlandlineno.Split("-")
                        Dim strCountryCd As String
                        Dim strStdCd As String
                        Dim strNo As String

                        strCountryCd = arrLandline(0)
                        strStdCd = arrLandline(1)
                        strNo = arrLandline(2)
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                strselcity = Nothing
                strselcountry = Nothing
                strselstate = Nothing
                sqlstr = Nothing
                strisscountry = Nothing
            End Try
        End Function
#End Region
        ' Code Indravadan
#Region "FillCenterCombo"
        Public Sub FillCenterCombo()

            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim datareader As SqlDataReader

            ddlCenters.Items.Clear()
            Dim l1 As New ListItem
            l1.Text = "---- Select ----"
            l1.Value = 0

            ddlCenters.Items.Add(l1)
            Try
                If objconn.connect() = True Then

                    '********************************************************************Raj
                    'Code added by Indravadan Vasava
                    'Purpose: To fill the combo box for Centre 
                    '********************************************************************
                    query = "select Center_ID,Center_Code,Center_Name from M_Centers where Del_Flg = 0 order by Center_Name"

                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()
                    Dim result As String = ""
                    If Request.QueryString("userid") <> Nothing Then
                        result = GetCenterIDFromDB(Request.QueryString("userid"))

                    ElseIf Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                        result = GetCenterIDFromDB(Session.Item("userid"))
                    End If

                    While datareader.Read()

                        Dim lstItm As New ListItem()
                        lstItm.Enabled = True

                        lstItm.Text = datareader.Item(2)
                        lstItm.Value = datareader.Item(0)

                        If result <> "" Then
                            If datareader.Item(0) = result Then
                                lstItm.Selected = True
                            End If
                        End If

                        ddlCenters.Items.Add(lstItm)

                    End While
                    datareader.Close()
                    objconn.disconnect()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try

        End Sub
#End Region

#Region "FillCourseCombo"
        Public Sub FillCourseCombo()

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim datareader As SqlDataReader
            Dim sb As StringBuilder
            ' lstCourses.Items.Clear()
            Try
                If objconn.connect() = True Then

                    '********************************************************************Raj
                    'Code added by Indravadan Vasava
                    'Purpose: To fill the combo box for Course
                    '********************************************************************
                    'query = "select Course_ID,Course_Name,Description from M_Course"
                    'query = "select c.Course_ID,c.Course_Name,c.Description from M_Course as c join T_Center_Course as t on(t.Course_ID = c.Course_ID) where t.Center_ID = " & ddlCenters.SelectedValue()

                    '/*******************Start,Jatin Gangajaliya, 2011/04/27*********************/
                    sb = New StringBuilder
                    sb.Append(" select mw.Course_ID,mc.COURSE_NAME,mce.Center_ID,mce.Center_Name ")
                    sb.Append(" from M_Weightage as mw left join m_course as mc on mc.course_id=mw.course_id ")
                    sb.Append(" left join T_Center_Course as tcc on mw.Course_ID=tcc.course_id LEFT JOIN M_Centers AS MCE ")
                    sb.Append(" ON MCE.Center_ID=TCC.Center_ID WHERE MCE.Center_ID= ")
                    sb.Append(ddlCenters.SelectedValue)
                    sb.Append(" AND MC.Del_Flag=0 group by mw.Course_ID,mc.course_name,mce.Center_ID,mce.Center_Name ")
                    sb.Append(" HAVING SUM(MW.Sub_Weightage)=100 ")
                    query = sb.ToString()
                    '/*****************************End*******************************************/



                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()

                    Dim ids(10) As String
                    Dim cnt As Integer = 0
                    Dim result As String = ""
                    If Request.QueryString("userid") <> Nothing Then
                        result = GetCoursesFromDB(Request.QueryString("userid"))
                    ElseIf Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                        result = GetCoursesFromDB(Session.Item("userid"))
                    End If

                    If result.Contains(",") Then
                        ids = result.Split(",")
                        'cnt = ids.Length - 1
                    Else
                        ids(0) = result
                    End If
                    While datareader.Read()
                        Dim lstItm As New ListItem()
                        lstItm.Enabled = True

                        lstItm.Text = datareader.Item(1)
                        lstItm.Value = datareader.Item(0)

                        If result <> "" And cnt <> -1 Then
                            cnt = 0
                            While (cnt <> ids.Length - 1)
                                If (ids(cnt) = datareader.Item(0).ToString()) Then
                                    lstItm.Selected = True
                                End If
                                cnt = cnt + 1
                            End While
                        End If

                        '            lstCourses.Items.Add(lstItm)
                    End While
                    datareader.Close()
                    objconn.disconnect()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try

        End Sub
#End Region

#Region "GetCenterIDFromControl"
        Public Function GetCenterIDFromControl()
            Dim s As String = ""
            For i As Integer = 0 To ddlCenters.Items.Count - 1

                If ddlCenters.Items(i).Selected = True Then
                    s = ddlCenters.SelectedItem.Value
                End If
            Next
            Return s
        End Function
#End Region

#Region "GetCoursesFromControl"

        'Commented by rajesh 2014/08/19
        'Public Function GetCoursesFromControl() As String
        '    Dim s As String = ""
        '    For i As Integer = 0 To lstCourses.Items.Count - 1
        '        If lstCourses.Items(i).Selected = True Then
        '            s += lstCourses.Items(i).Value & ","
        '        End If
        '    Next

        '    Return s
        'End Function
#End Region

#Region "GetCoursesFromDB"
        Public Function GetCoursesFromDB(ByVal userid As String)
            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim result As String = ""
            Dim datareader As SqlDataReader

            '  lstCourses.Items.Clear()
            Try
                If objconn.connect() = True Then

                    '********************************************************************Raj
                    'Code added by Indravadan Vasava
                    'Purpose: To Course for User from database
                    '********************************************************************M_User_Info
                    query = "select Distinct Course_ID from T_User_Course where User_ID=" & userid
                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()
                    While datareader.Read()
                        result = datareader.Item(0).ToString() + "," + result
                    End While
                    datareader.Close()
                    objconn.disconnect()
                End If
                Return result
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try

        End Function
#End Region

#Region "GetCenterIDFromDB"
        Public Function GetCenterIDFromDB(ByVal userid As String)
            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim result As String = ""
            Dim datareader As SqlDataReader
            'lstCourses.Items.Clear()
            Try
                If objconn.connect() = True Then

                    '********************************************************************Raj
                    'Code added by Indravadan Vasava
                    'Purpose: To Course for User from database
                    '********************************************************************
                    query = "select Center_ID from M_User_Info where Userid=" & userid
                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()
                    While datareader.Read()
                        result = datareader.Item(0).ToString()
                    End While
                    datareader.Close()
                    objconn.disconnect()
                End If
                Return result
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            End Try

        End Function
#End Region

#Region "ddlCenters_SelectedIndexChanged"
        Private Sub ddlCenters_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCenters.SelectedIndexChanged
            If ddlCenters.SelectedIndex = 0 Then
                ' courserowt.Visible = False
                ' courserow.Visible = False
                'trSpace2.Visible = False
            Else
                ' courserowt.Visible = True
                'courserow.Visible = True
                'trSpace2.Visible = True
                FillListOfCourse()
            End If

        End Sub
#End Region

#Region "FillListOfCourse"
        Public Sub FillListOfCourse()
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim datareader As SqlDataReader
            Dim data1 As SqlDataReader
            Dim objDA As SqlDataAdapter
            Dim objDS As DataSet
            '  lstCourses.Items.Clear()
            Dim sb As StringBuilder
            Try
                If objconn.connect() = True Then
                    '/************************Start,Jatin Gangajaliya,2011/04/25*************************/
                    sb = New StringBuilder
                    sb.Append(" select mw.Course_ID,mc.COURSE_NAME,mce.Center_ID,mce.Center_Name ")
                    sb.Append(" from M_Weightage as mw left join m_course as mc on mc.course_id=mw.course_id ")
                    sb.Append(" left join T_Center_Course as tcc on mw.Course_ID=tcc.course_id LEFT JOIN M_Centers AS MCE ")
                    sb.Append(" ON MCE.Center_ID=TCC.Center_ID WHERE MCE.Center_ID= ")
                    sb.Append(ddlCenters.SelectedValue)
                    sb.Append(" AND MC.Del_Flag=0 group by mw.Course_ID,mc.course_name,mce.Center_ID,mce.Center_Name ")
                    sb.Append(" HAVING SUM(MW.Sub_Weightage)=100 order by mc.COURSE_NAME")
                    query = sb.ToString()
                    '/*******************************End*************************************************/
                    'myCommand = New SqlCommand(query, objconn.MyConnection)
                    'datareader = myCommand.ExecuteReader()
                    'While datareader.Read()
                    '    Dim tblItem As Table
                    '    Dim lstItm As New ListItem()
                    '    lstItm.Enabled = True

                    '    lstItm.Text = datareader.Item(1)
                    '    lstItm.Value = datareader.Item(0)
                    '    lstCourses.Items.Add(lstItm)
                    'End While
                    'datareader.Close()

                    'Added By Vaibhav Soni
                    'Date : 2014/03/18
                    objDS = New DataSet()
                    objDA = New SqlDataAdapter(query, objconn.MyConnection)
                    objDA.Fill(objDS)
                    If Session("UniUserType").ToString() = "1" Then
                        'lstCourses.Visible = True
                        For i As Integer = 0 To objDS.Tables(0).Rows.Count - 1
                            Dim lstItm As New ListItem()
                            lstItm.Enabled = True
                            lstItm.Text = objDS.Tables(0).Rows(i)(1).ToString()
                            lstItm.Value = objDS.Tables(0).Rows(i)(0).ToString()
                            'lstCourses.Items.Add(lstItm)
                        Next
                    Else
                        Dim strTable As StringBuilder
                        strTable = New StringBuilder()
                        strTable.Append("<div style='height:318px; overflow:scroll;'>")
                        strTable.Append("<table width='100%' border='1' style='border:1px solid grey; border-collapse:collapse;'>")
                        'Heading
                        'strTable.Append("<tr>")
                        'strTable.Append("<td colspan='2' align='center'>")
                        'strTable.Append("Selected Course")
                        'strTable.Append("</td>")
                        'strTable.Append("</tr>")
                        'Datarow

                        For i As Integer = 0 To objDS.Tables(0).Rows.Count - 1
                            If (i Mod 2 = 0) Then
                                strTable.Append("<tr>")
                                strTable.Append("<td>")
                                strTable.Append(objDS.Tables(0).Rows(i)(1).ToString())
                                strTable.Append("</td>")
                            Else
                                strTable.Append("<td>")
                                strTable.Append(objDS.Tables(0).Rows(i)(1).ToString())
                                strTable.Append("</td>")
                                strTable.Append("</tr>")
                            End If
                        Next
                        strTable.Append("</table>")
                        strTable.Append("</div>")
                        '  courserow.Cells(0).InnerHtml = strTable.ToString()
                        'lstCourses.Visible = False
                    End If
                    'Ended By Vaibhav Soni
                    objconn.disconnect()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                sb = Nothing
                query = Nothing
                data1 = Nothing
                datareader = Nothing
            End Try
        End Sub
#End Region

#Region "cmb_usertype_SelectedIndexChanged"
        Protected Sub cmb_usertype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_usertype.SelectedIndexChanged
            lblMsg.Text = String.Empty
            If cmb_usertype.SelectedItem.Text = "Admin" Then
                ddlCenters.Enabled = False
                ' lstCourses.Visible = False
                centerrow.Visible = False
                'Commented & Added By Vaibhav Soni
                'Date : 2014/03/12
                'headrow.Visible = True
                'contentrow.Visible = True
                txtrollnumber.Text = String.Empty
                ddlCenters.SelectedIndex = 0
                '  courserowt.Visible = False
                'courserow.Visible = False
                'trSpace2.Visible = False
                'Ended By Vaibhav Soni
                rollnumber.Visible = False
                trClassHead.Visible = False
                div1.Visible = False
                div2.Visible = False
            Else
                ddlCenters.Enabled = True
                ' lstCourses.Visible = True
                centerrow.Visible = True
                rollnumber.Visible = True
                'Commented & Added By Vaibhav Soni
                'Date : 2014/03/12
                'headrow.Visible = False
                'contentrow.Visible = False
                If ddlCenters.SelectedIndex <> 0 Then
                    ' courserowt.Visible = True
                    'courserow.Visible = True
                    'trSpace2.Visible = True
                End If
                'Ended By Vaibhav Soni
                trClassHead.Visible = True
                div1.Visible = True
                div2.Visible = True
            End If
        End Sub
#End Region

#Region "Back Button Click Event"
        'Desc: This is Back Button Click Event.
        'By: Jatin Gangajaliya, 2011/3/14
        Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
            If (Session.Item("check2") = "true") Then
                Session.Remove("userid")
                Session.Add("toadminlist", "true")

                Dim intip As Integer
                If Request.QueryString("ip").ToString() <> Nothing Then
                    intip = CInt(Request.QueryString("ip").ToString())
                End If

                Response.Redirect("AdminList.aspx?ip=" & intip, False)
                Session.Remove("check2")
            ElseIf Session.Item("check") = "true" And Session.Item("checkforback") = "true" Then
                If (Session.Item("checkforback") = "true") Then
                    Session.Remove("userid")
                    Dim intpi As Integer
                    If Request.QueryString("pi") <> Nothing Then
                        intpi = CInt(Request.QueryString("pi").ToString())
                        Response.Redirect("StudentSearch.aspx?pi=" & intpi, False)
                        Session.Add("fromregister", "true")
                    End If
                    Session.Remove("checkforback")
                    Session.Remove("check")
                    Exit Sub
                End If
            Else

                If Request.QueryString("E") = Nothing Then
                    Session.Remove("userid")
                    Response.Redirect("admin.aspx")
                Else
                    Response.Redirect("StudHome.aspx")
                End If
            End If
        End Sub
#End Region

#Region "Clear Button Click Event"
        Protected Sub imgClear_Click(sender As Object, e As EventArgs) Handles imgClear.Click
            txt_dob.Value = ""
            txt_email.Text = ""
            txt_firstname.Text = ""
            txt_login.Text = ""
            txt_middlename.Text = ""
            txt_password.Text = ""
            txt_phone.Text = ""
            txt_surname.Text = ""
            txtperadd.Text = ""
            cmb_usertype.SelectedIndex = 0
            ddlCenters.SelectedIndex = 0
            'lstCourses.Items.Clear()
            rblist_sex.SelectedIndex = 0
            lblMsg.Text = String.Empty
        End Sub
#End Region

#Region "Reset Button Click Event"
        Protected Sub imgReset_Click(sender As Object, e As EventArgs) Handles imgReset.Click
            GetPersonalData()
            FillCenterCombo()
            If ddlCenters.SelectedIndex <> 0 Then
                FillCourseCombo()
            Else
                ' courserowt.Visible = False
                'courserow.Visible = False
                'lstCourses.Visible = False
            End If
            If (Session.Item("check") <> "true") Then
                If Session.Item("studentusertype") <> Nothing Or Session.Item("studentusertype") = 0 Then
                    If Convert.ToInt32(Session.Item("studentusertype")) = 0 Then
                        '***************************************************************
                        'Commented by bhumi [10/9/2015]
                        'Reason: On reset button displaying Exams Details is not proper
                        '***************************************************************

                        'GetTestDetail()

                        'Ended by bhumi
                    End If
                End If
            End If
        End Sub
#End Region

#Region "ImageButton1_Click"

        'Protected Sub ImageButton1_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        '    If Session.Item("UniUserType").ToString() = 1 Then
        '        Session.Remove("userid")
        '        Response.Redirect("admin.aspx")

        '        ' Added by pragnesha for super admin
        '        'Bug ID:670
        '        'Desc: Go back to admin home page
        '        'Date: 30/7/2018
        '        '---------------------------------------------------
        '    ElseIf Session.Item("UniUserType").ToString() = 2 Then
        '        Response.Redirect("admin.aspx")
        '        '---------------------------------------------------
        '    Else
        '        Response.Redirect("StudHome.aspx")
        '    End If
        'End Sub
#End Region

#Region "Method for getting rollnumber"
        'Desc:This method gives rollnumber details for a candidate.
        'By: Jatin Gangajaliya, 2011/05/02.

        Public Sub GetRollNumber()
            Dim introllnumber As Integer
            Dim strprefix, strquery As String
            Dim cmd As SqlCommand
            Dim sb As StringBuilder
            Try

                sb = New StringBuilder
                sb.Append(" Select value from M_System_Settings where key1 = 'Registration' and key2 = 'RollNo' ")
                strquery = sb.ToString()
                If objconn.connect() = True Then
                    cmd = New SqlCommand(strquery, objconn.MyConnection)
                    strprefix = CStr(cmd.ExecuteScalar())
                End If

                sb = New StringBuilder
                sb.Append(" Select Max(Userid+1) as Userid from M_USER_INFO ")
                strquery = sb.ToString()
                If objconn.connect() = True Then
                    cmd = New SqlCommand(strquery, objconn.MyConnection)
                    introllnumber = cmd.ExecuteScalar()
                End If

                If strprefix <> Nothing And introllnumber <> Nothing Then
                    txtrollnumber.Text = strprefix & introllnumber
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                introllnumber = Nothing
                strprefix = Nothing
                strquery = Nothing
                cmd = Nothing
                sb = Nothing
            End Try
        End Sub
#End Region

        Private Sub Page_Unload1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
            Session.Remove("check")
            Session.Remove("check2")
        End Sub

        Function getCoursesList() As ArrayList
            Dim _CourseList As ArrayList = New ArrayList
            Dim _query As String
            Dim MyDataReader As SqlDataReader
            'Dim sqlTrans As SqlTransaction
            Try
                If objconn.connect() = True Then
                    _query = "select Course_ID  from T_Center_Course where Center_ID='" & ddlCenters.SelectedValue & "'"
                    Dim cmd As New SqlCommand(_query, objconnect, sqlTrans)
                    MyDataReader = cmd.ExecuteReader()
                    While MyDataReader.Read()
                        _CourseList.Add(MyDataReader.Item("Course_ID"))
                    End While
                    MyDataReader.Close()
                    objconn.disconnect()
                End If
                Return _CourseList
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    objconn.disconnect()
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function


    End Class

End Namespace
