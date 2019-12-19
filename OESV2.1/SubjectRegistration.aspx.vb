Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web.UI.WebControls


Partial Public Class SubjectRegistration
    Inherits BasePage

#Region "Declarations"
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader             'SqlDataReader object
    Dim flg As Boolean = True
    Dim lnkEditID As String
    Dim testType As String
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("SubjectRegistration")
    Dim sqlTransin As SqlTransaction
    Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
    Dim dblpercentage As Double
    Dim intpercentage As Integer
#End Region

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: Fill combo when page load
    '********************************************************************
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Added By bhumi On 21/8/2015
            'Reason: while index changed, previous page validation message till visible on next page
            lblMsg.Visible = False
            'ended by bhumi
            GetTotal()
            'txtHQues.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            'txtMQtn.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            'txtLQtn.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            'txtCorretAns.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            'txtHQues.Attributes.Add("onblur", "CalculateTotQues()")
            'txtMQtn.Attributes.Add("onblur", "CalculateTotQues()")
            'txtLQtn.Attributes.Add("onblur", "CalculateTotQues()")

            txtTotalTime.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            txtTotalMarks.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            txtPassMarks.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")


            ' txtCorretAns.Attributes.Add("onblur", "checkLimit()")
            If Not IsPostBack Then
                FillMainCourseCombo()
                '  FillCenterList()

                '***************Start**********/
                'By:Jatin Gangajaliya, 2011/04/21.
                'multiBoxId.Items.Clear()
                FillCoursesCombo()
                '/*************End************/

                'Dim l1 As New ListItem
                'l1.Text = "---- Select ----"
                'l1.Value = 0
                'multiBoxId.Items.Add(l1)
                'If DataGridSubjectDetails.Visible = True Then
                '    fillPageNumbers(DataGridSubjectDetails.CurrentPageIndex + 1, 9)
                'End If
                'Added by Pranit on 05/11/2019
                'DataGridSubjectDetails.DataSource = BindGrid()
                'DataGridSubjectDetails.DataBind()
            Else
                If DataGridSubjectDetails.Visible = True Then
                    fillPageNumbers(DataGridSubjectDetails.CurrentPageIndex + 1, 9)
                End If
                ' BindGrid()
            End If


        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub


#Region "Page_Unload"
    Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            If Not strPathDb = Nothing Then
                If objconn.connect() Then
                    objconn.disconnect()
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
#End Region

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the combo box for Course
    '********************************************************************
    'Public Sub FillMainCourseCombo()

    '    DdlMainCourse.Items.Clear()
    '    Dim l1 As New ListItem
    '    l1.Text = "---- Select ----"
    '    l1.Value = 0

    '    DdlMainCourse.Items.Add(l1)
    '    strPathDb = ConfigurationManager.AppSettings("PathDb")
    '    If objconn.connect(strPathDb) Then
    '        Dim Main_Course As String = "Select Main_Course_ID,Main_Course_Name From M_Main_Course"
    '        objCommand = New SqlCommand(Main_Course, objconn.MyConnection)
    '        objDataReader = objCommand.ExecuteReader()
    '        While objDataReader.Read
    '            Dim lstItm As New ListItem()
    '            lstItm.Enabled = True
    '            lstItm.Text = objDataReader.Item(1)
    '            lstItm.Value = objDataReader.Item(0)
    '            DdlMainCourse.Items.Add(lstItm)

    '        End While
    '        objDataReader.Close()
    '        objconn.disconnect()
    '    End If

    'End Sub
    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the combo box for Course
    '********************************************************************
    Public Sub FillCoursesCombo()
        Dim objDataReader1 As SqlDataReader
        Dim query As String = ""
        Try

            '/************************Start***********************/
            'By:Jatin Gangajaliya, 2011/04/21.

            'multiBoxId.Items.Clear()
            ''Dim l1 As New ListItem
            ''l1.Text = "---- Select ----"
            ''l1.Value = 0
            ''multiBoxId.Items.Add(l1)
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            'If objconn.connect(strPathDb) = True Then
            '    query = "select Course_ID,Course_Name from M_Course where  del_flag='0' Order by Course_Name"
            '    objCommand = New SqlCommand(query, objconn.MyConnection)
            '    objDataReader1 = objCommand.ExecuteReader()
            '    While objDataReader1.Read()
            '        Dim lstItm As New ListItem()
            '        lstItm.Enabled = True
            '        lstItm.Text = objDataReader1.Item(1)
            '        lstItm.Value = objDataReader1.Item(0)
            '        multiBoxId.Items.Add(lstItm)
            '    End While
            '    objDataReader1.Close()
            '    objconn.disconnect()
            'End If
            '/**********************End**************************/

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objDataReader1 = Nothing
            query = Nothing
        End Try
    End Sub


    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Insert Data into database
    '********************************************************************
    Public Sub InsertData()
        Try
            Dim strTestType = getNextTestType()
            If chkcourse.Checked = True Then
                strTestType = strTestType - 1
            End If

            'For i As Integer = 0 To multiBoxId.Items.Count - 1
            '    If multiBoxId.Items(i).Selected = True Then
            Dim query As String = ""
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() = True Then
                'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)

                '/********************Start**********************/
                'Dim fields As String = "test_type,test_Name,Sub_Code,no_of_ques,l_ques,m_ques,h_ques,Course_ID,Correct_Ans"
                'query = "Insert Into M_Testinfo (" & fields & ") values('" & strTestType & "','" & txtSubjectName.Text & "','" & txtSubCode.Text & "','" & lblTotalAllotedQtn.Text & "','" & txtLQtn.Text & "','" & txtMQtn.Text & "','" & txtHQues.Text & "','" & multiBoxId.Items(i).Value & "','" & txtCorretAns.Text & "')"
                '/********************End************************/


                Dim fields As String = "test_Name,Sub_Code"
                query = "Insert Into M_Testinfo (" & fields & ") values(" & "'" & txtSubjectName.Text & "','" & txtSubCode.Text & "'" & " )"
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objCommand.ExecuteNonQuery()
                objconn.disconnect()
                'sqlTrans.Commit()
                'If (DdlMainCourse.SelectedItem.Value <> 0) Or (multiBoxId.SelectedItem.Value <> 0) Then
                '    ' FillMainCourseCombo()
                '    multiBoxId.Items.Clear()
                '    Dim l1 As New ListItem
                '    l1.Text = "---- Select ----"
                '    l1.Value = 0
                '    multiBoxId.Items.Add(l1)
                '    ClearControls()
                'End If

            End If
            '    End If
            'Next
        Catch ex As Exception
            'sqlTrans.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally

        End Try
    End Sub


    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Update Data into database
    '********************************************************************
    Public Sub UpdateData(ByVal strtestname As String, ByVal strsubcode As String)
        Dim query As String = ""
        Dim test_type As String = ""
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")

        If objconn.connect() = True Then

            'Dim sda As New SqlDataAdapter("select test_type from m_testinfo where test_name='" & txtSubjectName.Text & "'", objconn.MyConnection)
            'Dim dss As New DataSet
            'sda.Fill(dss)
            If Session.Item("testid") <> Nothing Then
                test_type = Session.Item("testid").ToString()
                Session.Remove("testid")
            End If

            Dim sqlTrans As SqlTransaction = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
            Try

                'Commented by: Jatin Gangajaliya, 2011/04/21
                'Due to : Accomodate new functionality.
                'Dim del_query As String = "delete from m_testinfo where test_name='" & txtSubjectName.Text & "'"
                'Dim cmd As New SqlCommand(del_query, objconn.MyConnection, sqlTrans)
                'cmd.ExecuteNonQuery()


                'insert
                '/*********************Start,2011/04/20,Jatin Gangajaliya*************************/
                'For i As Integer = 0 To multiBoxId.Items.Count - 1
                '    If multiBoxId.Items(i).Selected = True Then
                '        Dim fields As String = "test_type,test_Name,Sub_Code,no_of_ques,l_ques,m_ques,h_ques,Course_ID,Correct_Ans"
                '        Dim ins_query As String = "Insert Into M_Testinfo (" & fields & ") values('" & test_type & "','" & txtSubjectName.Text & "','" & txtSubCode.Text & "','" & lblTotalAllotedQtn.Text & "','" & txtLQtn.Text & "','" & txtMQtn.Text & "','" & txtHQues.Text & "','" & multiBoxId.Items(i).Value & "','" & txtCorretAns.Text & "')"
                '        Dim ins_cmd As New SqlCommand(ins_query, objconn.MyConnection, sqlTrans)
                '        ins_cmd.ExecuteNonQuery()
                '    End If
                'Next

                'For i As Integer = 0 To multiBoxId.Items.Count - 1
                '    If multiBoxId.Items(i).Selected = True Then
                'Dim fields As String = "test_type,test_name,Sub_Code"
                'Dim ins_query As String = "Insert Into M_Testinfo (" & fields & ") values('" & test_type & "','" & txtSubjectName.Text & "','" & txtSubCode.Text & "' )"
                '/**********************************End******************************************/

                Dim ins_query As String = " Update M_Testinfo Set test_name = '" & strtestname & "' ," & " Sub_Code = '" & strsubcode & "' where test_type ='" & test_type & "'"
                Dim ins_cmd As New SqlCommand(ins_query, objconn.MyConnection, sqlTrans)
                ins_cmd.ExecuteNonQuery()
                'End If
                'Next



                sqlTrans.Commit()
                'query = "Update M_Testinfo Set  test_Name='" & txtSubjectName.Text & "',no_of_ques='" & lblTotalAllotedQtn.Text & "',M_Testinfo.Course_ID='" & multiBoxId.SelectedItem.Value & "',l_ques='" & txtLQtn.Text & "',m_ques='" & txtMQtn.Text & "',h_ques='" & txtHQues.Text & "' where Sub_Code='" & txtSubCode.Text & "' and test_type='" & txtTestType.Text & "'"
                'objCommand = New SqlCommand(query, objconn.MyConnection)
                'objCommand.Transaction = sqlTrans

                'objCommand.ExecuteNonQuery()

                'query = ""
                'query = "Update m_course Set  m_Course.main_course_id ='" & DdlMainCourse.SelectedItem.Value & "' where M_course.Course_ID='" & multiBoxId.SelectedItem.Value & "' "
                'objCommand = New SqlCommand(query, objconn.MyConnection)
                'objCommand.ExecuteNonQuery()

            Catch ex As Exception
                sqlTrans.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                query = Nothing
                test_type = Nothing
                objconn.disconnect()
            End Try
        End If
        ' FillMainCourseCombo()
        UpdateQuestionDetails("")
        FillCoursesCombo()

        ClearControls()
        'multiBoxId.Items.Clear()
        Dim l1 As New ListItem
        l1.Text = "---- Select ----"
        l1.Value = 0
        'multiBoxId.Items.Add(l1)
        ''DdlMainCourse.SelectedIndex = 0
        'multiBoxId.SelectedIndex = 0

    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: ValidationForNumber
    '********************************************************************
    Private Function ValidationForNumber(ByVal value As Char) As Boolean
        Try
            If ((value >= "0" And value <= "9") Or value.GetHashCode() = "524296") Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Function

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Clear Controls
    '********************************************************************
    Public Sub ClearControls()
        'txtTestType.Text = String.Empty
        Try
            gridDiv.Visible = False
            txtSubCode.Text = String.Empty
            txtSubjectName.Text = String.Empty
            'txtHQues.Text = String.Empty
            'txtMQtn.Text = String.Empty
            'txtLQtn.Text = String.Empty
            'lblTotalAllotedQtn.Text = 0
            'multiBoxId.SelectedIndex = -1

            '/***********start,2011/03/24,Jatin Gangajaliya*************/
            chkcourse.Checked = False
            ' multiBoxIdcenter.SelectedIndex = -1
            txtTotalTime.Text = String.Empty
            ddlMainCourse.SelectedValue = 0

            maincourserow.Visible = False
            ' centrerow.Visible = False
            textboxrow.Visible = False
            textboxcoderow.Visible = False
            textboxrow.Visible = False
            textMarksrow.Visible = False
            textPassmarksrow.Visible = False

            txtTotalMarks.Text = String.Empty
            txtPassMarks.Text = String.Empty
            txtcoursecode.Text = String.Empty
            '/*********************End***********************/

            ' DdlMainCourse.Enabled = True
            lblMsg.Visible = False
            imgBtnSubmit.Visible = True
            'imgBtnUpdate.Visible = False
            'txtCorretAns.Text = String.Empty
            txtSubCode.Enabled = True
            LblTotalRecCnt.Text = String.Empty
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub


    '********************************************************************
    'Code added by Monal Shah
    'Purpose:back
    '********************************************************************
    Protected Sub imgBtnBack_Click(sender As Object, e As EventArgs) Handles imgBtnBack.Click
        DataGridSubjectDetails.Visible = False
        Response.Redirect("admin.aspx")
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Insert into database
    '********************************************************************
    Protected Sub imgBtnSubmit_Click(sender As Object, e As EventArgs) Handles imgBtnSubmit.Click
        Try
            Dim bolFlag As Boolean = Validations()
            If (bolFlag = True) Then

                'ElseIf multiBoxId.SelectedIndex > -1 And txtSubjectName.Text <> String.Empty And SubCode.Text <> String.Empty And txtHQues.Text <> String.Empty And txtMQtn.Text <> String.Empty And txtLQtn.Text <> String.Empty And txtCorretAns.Text <> String.Empty Then
                'If multiBoxId.SelectedIndex > -1 And txtSubjectName.Text <> String.Empty And txtHQues.Text <> String.Empty And txtMQtn.Text <> String.Empty And txtLQtn.Text <> String.Empty And txtCorretAns.Text <> String.Empty Then

                If txtSubjectName.Text <> String.Empty Then
                    GetTotal()
                    DataGridSubjectDetails.Visible = False


                    '/************************start*********************/
                    'By: Jatin Gangajaliya.
                    'If (Convert.ToInt32(txtCorretAns.Text) > Convert.ToInt32(lblTotalAllotedQtn.Text)) Then
                    '    lblMsg.Text = "Correct Answer required must be less than Alloted Question"
                    '    lblMsg.ForeColor = Drawing.Color.Red
                    '    lblMsg.Visible = True
                    '    Exit Sub
                    'End If
                    '/*******************End*********************/



                    '/************************start*********************/
                    If CheckName(txtSubjectName.Text) = True Then

                        If chkcourse.Checked = True Then
                            strPathDb = ConfigurationSettings.AppSettings("PathDb")
                            Dim strcourseid As String = getnextcourseid()
                            'If objconn.connect(strPathDb) = True Then
                            objconnect.Open()
                            sqlTransin = objconnect.BeginTransaction(IsolationLevel.ReadCommitted)
                            'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            InsertCourse(strcourseid)
                            sqlTransin.Commit()
                            objconnect.Close()
                            'End If
                            'If ddlMainCourse.SelectedValue = 0 Then
                            '    lblMsg.Text = "Please select MainCourse"
                            '    Exit Sub
                            'ElseIf multiBoxIdcenter.SelectedIndex = 0 Then
                            '    lblMsg.Text = "Please select Centre"
                            'ElseIf txtTotalTime.Text = String.Empty Then
                            '    lblMsg.Text = "Please Enter Total Alloted Exam Time in Minutes"
                            '    Exit Sub
                            'Else
                            '    If objconn.connect(strPathDb) = True Then
                            '        sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            '        InsertCourse()
                            '    End If
                            'End If
                        Else
                            InsertData()
                        End If
                        '/************************End**********************/
                        UpdateQuestionDetails("")
                        ClearControls()
                        FillCoursesCombo()
                        'BindGrid()
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Drawing.Color.Green
                        lblMsg.Text = Resources.Resource.SubjectMaintenance_subregsuc

                    Else
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Drawing.Color.Red
                        DataGridSubjectDetails.Visible = False
                        gridDiv.Visible = False
                        'lblMsg.Text = "This Subject is already registered in system. If you want to add this subject in other courses then select subject, select course name and press the update button."

                        lblMsg.Text = Resources.Resource.SubjectMaintenance_subregerr
                    End If

                Else
                    GetTotal()
                    DataGridSubjectDetails.Visible = False
                    'lblMsg.Visible = True
                    'lblMsg.ForeColor = Drawing.Color.Red
                    gridDiv.Visible = False
                    'lblMsg.Text = "All Fields Are Mandatory......"
                End If

            End If

        Catch ex As Exception
            sqlTransin.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    Public Function CheckName(ByVal subject As String) As Boolean
        Dim query As String = ""
        Try
            Dim flag As Boolean = True
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

            If objconn.connect() = True Then
                query = "SELECT count(test_name) FROM M_Testinfo WHERE test_name = '" & subject & "'"
                Dim sqlDA As New SqlDataAdapter(query, objconn.MyConnection)
                Dim dss As New DataSet
                sqlDA.Fill(dss)
                If dss.Tables(0).Rows(0).Item(0) > 0 Then
                    flag = False
                Else
                    flag = True
                End If

            End If

            Return flag
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
        End Try
    End Function

    Public Function GetTotal() As String
        Dim h, m, l As Integer
        'Try
        '    h = Convert.ToInt32(txtHQues.Text)
        'Catch ex As Exception
        '    h = 0
        'End Try
        'Try
        '    m = Convert.ToInt32(txtMQtn.Text)
        'Catch ex As Exception
        '    m = 0
        'End Try
        'Try
        '    l = Convert.ToInt32(txtLQtn.Text)
        'Catch ex As Exception
        '    l = 0
        'End Try

        'lblTotalAllotedQtn.Text = h + m + l

    End Function

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: Fill course combo
    '********************************************************************
    'Protected Sub DdlMainCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DdlMainCourse.SelectedIndexChanged
    '    FillCoursesCombo()
    'End Sub
    '********************************************************************
    'Code added by Monal Shah
    'Purpose: Search according to criteria and display records in grid
    '********************************************************************
    Protected Sub imgBtnSearch_Click(sender As Object, e As EventArgs) Handles imgBtnSearch.Click
        Try
            lblMsg.Visible = False
            DataGridSubjectDetails.Visible = True

            gridDiv.Visible = True
            DataGridSubjectDetails.CurrentPageIndex = 0
            SearchGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            'Response.Redirect("error.aspx?err=" & ex.Message, False)
            Response.Redirect("error.aspx", False)
        End Try
        'gridDiv.Attributes.AddAttributes["style"] = "width:100px; height:200px;"

        'ClearControls()
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: Bind Grid
    '********************************************************************
    Public Sub SearchGrid()
        Dim query As String = ""
        Dim sb As New StringBuilder()
        Dim dt As New DataTable()
        Try
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() = True Then

                'Dim fields As String = " Distinct m_course.Course_Name + ',' + m_testinfo.test_Name as combine,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques,m_testinfo.Course_ID as Course_ID,m_testinfo.Sub_Code As Sub_Code,m_testinfo.Correct_Ans As Correct_Ans,m_testinfo.del_flag as del_flag"
                'sb.Append("Select " & fields & " From m_testinfo,m_course,m_main_course where ")

                Dim fields As String = " test_type,test_name,Sub_Code,del_flag "
                sb.Append(" Select " & fields & " from M_Testinfo ")

                'If (DdlMainCourse.SelectedIndex <> 0) Then
                '    sb.Append("m_course.main_Course_id='" & DdlMainCourse.SelectedItem.Value & "'")
                '    sb.Append(" AND ")
                'End If
                'If (multiBoxId.SelectedIndex <> 0) Then
                '    sb.Append("m_course.Course_id='" & multiBoxId.SelectedItem.Value & "'")
                '    sb.Append(" AND ")
                'End If
                'For i As Integer = 0 To multiBoxId.Items.Count - 1
                '    If multiBoxId.Items(i).Selected = True Then
                '        strPathDb = ConfigurationSettings.AppSettings("PathDb")
                '        If objconn.connect(strPathDb) = True Then
                '            ' add search criteria here
                '            sb.Append(" m_Course.Course_Name Like '" & multiBoxId.Items(i).Text & "%' ")
                '            sb.Append(" or ")
                '        End If
                '    End If
                'Next
                'Dim ss As String = sb.ToString
                'sb.Remove(sb.Length - 4, 4)
                'sb.Append(" and ")
                'ss = sb.ToString()
                If (txtSubjectName.Text <> "") Or (txtSubCode.Text <> String.Empty) Then
                    sb.Append(" where ")
                End If

                If (txtSubjectName.Text <> "") Then
                    sb.Append(" test_name Like '%" & txtSubjectName.Text & "%'")
                End If

                If (txtSubjectName.Text <> "" And txtSubCode.Text <> String.Empty) Then
                    sb.Append(" AND ")
                End If

                If (txtSubCode.Text <> String.Empty) Then
                    sb.Append("  Sub_Code LIke '%" & txtSubCode.Text & "%' ")
                End If

                'If (txtTestType.Text <> String.Empty) Then
                '    sb.Append("  m_testinfo.test_type = '" & txtTestType.Text & "' ")
                '    sb.Append(" and ")
                'End If


                '/*************************Start, Jatin Gangajaliya, 2011/04/20*****************************/
                'Desc: Commented to accomodate new functionality.

                'If (txtHQues.Text <> String.Empty) Then
                '    sb.Append("  m_testinfo.h_ques = '" & txtHQues.Text & "' ")
                '    sb.Append(" and ")
                'End If

                'If (txtMQtn.Text <> String.Empty) Then
                '    sb.Append("  m_testinfo.m_ques = '" & txtMQtn.Text & "' ")
                '    sb.Append(" and ")
                'End If

                'If (txtLQtn.Text <> String.Empty) Then
                '    sb.Append("  m_testinfo.l_ques = '" & txtLQtn.Text & "' ")
                '    sb.Append(" and ")
                'End If

                'If (txtCorretAns.Text <> String.Empty) Then
                '    sb.Append("  m_testinfo.Correct_Ans = '" & txtCorretAns.Text & "' ")
                '    sb.Append(" and ")
                'End If

                '/*******************************End******************************/

                'If (lblTotalAllotedQtn.Text <> String.Empty) Then
                '    sb.Append("  m_testinfo.no_of_ques = '" & lblTotalAllotedQtn.Text & "' ")
                '    sb.Append(" and ")
                'End If

                '/**************************Start****************/
                'sb.Append(" m_course.Course_id=m_testinfo.Course_ID  and m_course.del_flag='0' and")
                'sb.Append("(  ")
                'For i As Integer = 0 To multiBoxId.Items.Count - 1

                '    If multiBoxId.Items(i).Selected = True Then
                '        strPathDb = ConfigurationSettings.AppSettings("PathDb")
                '        If objconn.connect(strPathDb) = True Then
                '            ' add search criteria here
                '            sb.Append(" m_Course.Course_Name Like '" & multiBoxId.Items(i).Text & "%' ")
                '            sb.Append(" or   ")
                '        End If
                '    End If
                'Next
                '/**********************End**********************/

                'Dim ss As String = sb.ToString
                'sb.Remove(sb.Length - 6, 6)
                '' sb.Append(" and ")
                'ss = sb.ToString()

                'If ss.Contains("(") Then
                '    sb.Append(" )")
                'End If

                'sb.Append(" M_Testinfo.del_flag = '0' ")
                sb.Append(" order by test_type ")

                query = sb.ToString()
                'objCommand = New SqlCommand(query, objconn.MyConnection)
                'objDataReader = objCommand.ExecuteReader()
                Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
                adp.Fill(dt)
                If (dt.Rows.Count > 0) Then

                    dt.Columns.Add("SrNo")
                    For ii As Integer = 0 To dt.Rows.Count - 1
                        dt.Rows(ii).Item("SrNo") = ii + 1
                    Next
                    DataGridSubjectDetails.DataSource = dt

                    DataGridSubjectDetails.DataBind()
                    'fillPagesCombo()
                    fillPageNumbers(DataGridSubjectDetails.CurrentPageIndex + 1, 9)
                    DataGridSubjectDetails.Visible = True

                    For i As Integer = 0 To DataGridSubjectDetails.Items.Count - 1

                        If DataGridSubjectDetails.Items(i).Cells(3).Text = True Then
                            'Boolean mybol = Boolean.Parse(DataGridSubjectDetails.Items(i).Cells(3).Text) 
                            'DataGridSubjectDetails.Items(i).Cells(4).Attributes.Remove("href")
                            'DataGridSubjectDetails.Items(i).Cells(4).Attributes.Remove("className")
                            'DataGridSubjectDetails.Items(i).Cells(4).Attributes.Add("onclick", "return false")
                            DataGridSubjectDetails.Items(i).Cells(4).Enabled = False
                            DataGridSubjectDetails.Items(i).Cells(4).ToolTip = "Disabled"
                            DataGridSubjectDetails.Items(i).Cells(5).Enabled = True
                            'DataGridSubjectDetails.Items(i).Cells(5).ToolTip = "Disabled"
                            DataGridSubjectDetails.Items(i).BackColor = Drawing.Color.Gray
                            ' Next
                        ElseIf DataGridSubjectDetails.Items(i).Cells(3).Text = False Then
                            DataGridSubjectDetails.Items(i).Enabled = True
                        End If
                    Next

                    LblTotalRecCnt.Text = Resources.Resource.AdminList_TotRecord & ": " & dt.Rows.Count
                    gridDiv.Style.Item("heigth") = "40px"
                Else
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.Common_NoRecFound
                    gridDiv.Visible = False
                    DataGridSubjectDetails.Visible = False
                End If

            End If
            'objDataReader.Close()
            objconn.disconnect()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :")
            End If
            Throw ex
        Finally
            query = Nothing
            sb = Nothing
            dt = Nothing
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: Bind Grid
    '********************************************************************
    'Modified by Pranit on 06/11/2019
    Public Function BindGrid()
        Dim query As String = ""
        Dim sb As New StringBuilder()
        Dim dt As New DataTable()
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try
            'Added by Pranit on 06/11/2019
            'Session("Source") = dt
            'Dim dv As DataView = New DataView(dt)

            If objconn.connect() = True Then

                'Dim fields As String = " Distinct m_course.Course_Name + ',' + m_testinfo.test_Name as combine,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques,m_testinfo.Course_ID as Course_ID,m_testinfo.Sub_Code As Sub_Code,m_testinfo.Correct_Ans As Correct_Ans"
                'sb.Append("Select " & fields & " From m_testinfo,m_course,m_main_course where ")

                ''If (DdlMainCourse.SelectedIndex <> 0) Then
                ''    sb.Append("m_course.main_Course_id='" & DdlMainCourse.SelectedItem.Value & "'")
                ''    sb.Append(" AND ")
                ''End If
                ''If (multiBoxId.SelectedIndex <> 0) Then
                ''    sb.Append("m_course.Course_id='" & multiBoxId.SelectedItem.Value & "'")
                ''    sb.Append(" AND ")
                ''End If
                ''For i As Integer = 0 To multiBoxId.Items.Count - 1
                ''    If multiBoxId.Items(i).Selected = True Then
                ''        strPathDb = ConfigurationSettings.AppSettings("PathDb")
                ''        If objconn.connect(strPathDb) = True Then
                ''            ' add search criteria here
                ''            sb.Append(" m_Course.Course_Name Like '" & multiBoxId.Items(i).Text & "%' ")
                ''            sb.Append(" or ")
                ''        End If
                ''    End If
                ''Next
                ''Dim ss As String = sb.ToString
                ''sb.Remove(sb.Length - 4, 4)
                ''sb.Append(" and ")
                ''ss = sb.ToString()

                ''If (txtSubjectName.Text <> "") Then
                ''    sb.Append(" m_testinfo.test_name Like '" & txtSubjectName.Text & "%' ")
                ''    sb.Append(" and ")
                ''End If

                ''If (txtSubCode.Text <> String.Empty) Then
                ''    sb.Append("  m_testinfo.sub_code = '" & txtSubCode.Text & "' ")
                ''    sb.Append(" and ")
                ''End If

                ' ''If (txtTestType.Text <> String.Empty) Then
                ' ''    sb.Append("  m_testinfo.test_type = '" & txtTestType.Text & "' ")
                ' ''    sb.Append(" and ")
                ' ''End If

                ''If (txtHQues.Text <> String.Empty) Then
                ''    sb.Append("  m_testinfo.h_ques = '" & txtHQues.Text & "' ")
                ''    sb.Append(" and ")
                ''End If

                ''If (txtMQtn.Text <> String.Empty) Then
                ''    sb.Append("  m_testinfo.m_ques = '" & txtMQtn.Text & "' ")
                ''    sb.Append(" and ")
                ''End If

                ''If (txtLQtn.Text <> String.Empty) Then
                ''    sb.Append("  m_testinfo.l_ques = '" & txtLQtn.Text & "' ")
                ''    sb.Append(" and ")
                ''End If

                ''If (txtCorretAns.Text <> String.Empty) Then
                ''    sb.Append("  m_testinfo.Correct_Ans = '" & txtCorretAns.Text & "' ")
                ''    sb.Append(" and ")
                ''End If

                ''If (lblTotalAllotedQtn.Text <> String.Empty) Then
                ''    sb.Append("  m_testinfo.no_of_ques = '" & lblTotalAllotedQtn.Text & "' ")
                ''    sb.Append(" and ")
                ''End If
                ''''''sb.Append(" m_course.Course_id=m_testinfo.Course_ID and m_testinfo.del_flag='0' and m_course.del_flag='0' ")
                'sb.Append(" m_course.Course_id=m_testinfo.Course_ID and m_course.del_flag='0' ")
                ''sb.Append("(  ")
                ' ''For i As Integer = 0 To multiBoxId.Items.Count - 1

                ''    If multiBoxId.Items(i).Selected = True Then
                ''        strPathDb = ConfigurationSettings.AppSettings("PathDb")
                ''        If objconn.connect(strPathDb) = True Then
                ''            ' add search criteria here
                ''            sb.Append(" m_Course.Course_Name Like '" & multiBoxId.Items(i).Text & "%' ")
                ''            sb.Append(" or   ")
                ''        End If
                ''    End If
                ''Next
                ''Dim ss As String = sb.ToString
                ''sb.Remove(sb.Length - 6, 6)
                ' '' sb.Append(" and ")
                ''ss = sb.ToString()

                ''If ss.Contains("(") Then
                ''    sb.Append(" )")
                ''End If

                'sb.Append(" order by m_course.Course_Name ")

                'query = sb.ToString()

                '                Dim fields As String = " Distinct m_course.Course_Name + ',' + m_testinfo.test_Name as combine,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.del_flag as dflag,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques,m_testinfo.Course_ID as Course_ID,m_testinfo.Sub_Code As Sub_Code,m_testinfo.Correct_Ans As Correct_Ans,m_testinfo.del_flag as del_flag"
                'Dim fields As String = " Distinct m_course.Course_Name + ',' + m_testinfo.test_Name as combine,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques,m_testinfo.Course_ID as Course_ID,m_testinfo.Sub_Code As Sub_Code,m_testinfo.Correct_Ans As Correct_Ans,m_testinfo.del_flag as del_flag"
                'sb.Append("Select " & fields & " From m_testinfo,m_course,m_main_course where ")
                'sb.Append(" m_course.Course_id=m_testinfo.Course_ID and m_testinfo.del_flag='0' and m_course.del_flag='0' ")
                'sb.Append(" order by m_course.Course_Name ")
                'sb.Append(" m_course.Course_id=m_testinfo.Course_ID and m_course.del_flag='0' ")
                'sb.Append(" order by m_course.Course_Name ")

                Dim fields As String = " test_type,test_name,Sub_Code,del_flag "
                sb.Append(" Select " & fields & " from M_Testinfo order by test_type")

                query = sb.ToString()

                'objCommand = New SqlCommand(query, objconn.MyConnection)
                'objDataReader = objCommand.ExecuteReader()
                Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
                adp.Fill(dt)
                If (dt.Rows.Count > 0) Then

                    dt.Columns.Add("SrNo")
                    For ii As Integer = 0 To dt.Rows.Count - 1
                        dt.Rows(ii).Item("SrNo") = ii + 1
                    Next
                    DataGridSubjectDetails.DataSource = dt

                    DataGridSubjectDetails.DataBind()
                    'fillPagesCombo()
                    fillPageNumbers(DataGridSubjectDetails.CurrentPageIndex + 1, 9)

                    'Code for Enable disable
                    For i As Integer = 0 To DataGridSubjectDetails.Items.Count - 1
                        If DataGridSubjectDetails.Items(i).Cells(3).Text = True Then
                            'DataGridSubjectDetails.Items(i).Cells(4).Attributes.Remove("href")
                            'DataGridSubjectDetails.Items(i).Cells(4).Attributes.Remove("className")
                            'DataGridSubjectDetails.Items(i).Cells(4).Attributes.Add("onclick", "return false")
                            DataGridSubjectDetails.Items(i).Cells(4).Enabled = False
                            DataGridSubjectDetails.Items(i).Cells(4).ToolTip = "Disabled"
                            DataGridSubjectDetails.Items(i).Cells(5).Enabled = True
                            'DataGridSubjectDetails.Items(i).Cells(5).ToolTip = "Disabled"
                            DataGridSubjectDetails.Items(i).BackColor = Drawing.Color.Gray
                            ' Next
                        ElseIf DataGridSubjectDetails.Items(i).Cells(3).Text = False Then
                            DataGridSubjectDetails.Items(i).Enabled = True
                        End If
                    Next

                    DataGridSubjectDetails.Visible = True
                    LblTotalRecCnt.Text = Resources.Resource.AdminList_TotRecord & dt.Rows.Count
                    gridDiv.Style.Item("heigth") = "40px"
                Else
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.Common_NoRecFound
                    gridDiv.Visible = False
                    DataGridSubjectDetails.Visible = False
                End If

            End If
            'Added by Pranit on 06/11/2019
            'Return dv
            'objDataReader.Close()
            objconn.disconnect()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
            sb = Nothing
            dt = Nothing
        End Try
    End Function

    'Added by Pranit on 06/11/2019
#Region "Sorting"
    'Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '    Dim dt As DataTable = CType(Session("Source"), DataTable)
    '    Dim dv As DataView = New DataView(dt)
    '    dv.Sort = e.SortExpression
    '    DataGridSubjectDetails.DataSource = dv
    '    DataGridSubjectDetails.DataBind()
    'End Sub 'Sort_Grid
#End Region

    'Protected Sub DataGridSubjectDetails_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridSubjectDetails.ItemCommand
    '    Try
    '        If e.CommandName = "lnkEdit" Then
    '            lnkEditID = e.CommandArgument
    '            Session.Add("testid", lnkEditID)
    '            Dim cid As String
    '            Dim sname As String = ""
    '            Dim str() As String = lnkEditID.Split(",")
    '            sname = str(0)
    '            Dim query As String = ""
    '            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

    '            If objconn.connect() = True Then

    '                'Dim fields1 As String = "course_id"
    '                'Dim query1 As String = "Select " & fields1 & " From m_course where Course_name='" & str(0) & "' "
    '                'Dim adp As New SqlDataAdapter(query1, objconn.MyConnection)
    '                'Dim dt As New DataTable()
    '                'adp.Fill(dt)
    '                'cid = dt.Rows(0)("Course_id").ToString()

    '                'Dim fields As String = "m_main_Course.main_course_name as main_Course,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.Sub_Code As Sub_Code,m_testinfo.test_type as test_type,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques,m_testinfo.Correct_Ans as Correct_Ans"
    '                'query = "Select " & fields & " From m_testinfo,m_course,m_main_course where m_testinfo.test_name='" & str(1) & "' and m_course.Course_id=m_testinfo.Course_ID and m_course.main_Course_id=m_main_course.main_course_id and m_testinfo.del_flag='0' and m_course.del_flag='0' and m_testinfo.Course_ID=" & cid & ""
    '                'objCommand = New SqlCommand(query, objconn.MyConnection)

    '                Dim fields As String = " test_type,test_name,Sub_Code "
    '                query = " Select " & fields & " from M_Testinfo where  M_Testinfo.del_flag = '0'  and test_type = " & lnkEditID
    '                objCommand = New SqlCommand(query, objconn.MyConnection)
    '                objDataReader = objCommand.ExecuteReader()

    '                While objDataReader.Read()
    '                    txtSubjectName.Text = objDataReader.Item("test_Name")
    '                    'txtSubCode.Enabled = False
    '                    'txtTestType.Enabled = False
    '                    txtSubCode.Text = objDataReader.Item("Sub_Code")
    '                    testType = objDataReader.Item("test_type")
    '                    'lblTotalAllotedQtn.Text = objDataReader.Item("no_of_ques")
    '                    'txtCorretAns.Text = objDataReader.Item("Correct_Ans")
    '                    'txtLQtn.Text = objDataReader.Item("l_ques")
    '                    'txtMQtn.Text = objDataReader.Item("m_ques")
    '                    'txtHQues.Text = objDataReader.Item("h_ques")

    '                    'For Each item As ListItem In DdlMainCourse.Items

    '                    '    If (item.Text = objDataReader.Item("main_Course")) Then

    '                    '        item.Selected = True
    '                    '        DdlMainCourse.SelectedIndex = item.Value

    '                    '    End If

    '                    'Next
    '                    lblMsg.Visible = False
    '                    'FillCoursesCombo()

    '                    'Commented by; Jatin Ganagajaliya.
    '                    'Dim sda As New SqlDataAdapter("select Course_ID from m_testinfo where test_name='" & str(1) & "' and del_flag=0 ", objconn.MyConnection)
    '                    'Dim dss As New DataSet
    '                    'sda.Fill(dss)

    '                    'For i As Integer = 0 To dss.Tables(0).Rows.Count - 1
    '                    '    For j As Integer = 0 To multiBoxId.Items.Count - 1
    '                    '        If multiBoxId.Items(j).Value.ToString = dss.Tables(0).Rows(i).Item(0).ToString Then
    '                    '            multiBoxId.Items(j).Selected = True
    '                    '        End If
    '                    '    Next
    '                    'Next


    '                End While
    '                imgBtnSubmit.Visible = False
    '                'imgBtnUpdate.Visible = True

    '                objDataReader.Close()
    '                objconn.disconnect()
    '            End If

    '        ElseIf e.CommandName = "lnkDelete" Then

    '            lnkEditID = e.CommandArgument
    '            Dim str() As String = lnkEditID.Split(",")
    '            Dim query As String = ""
    '            Dim Cid As String = ""
    '            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

    '            If objconn.connect() = True Then
    '                Dim qr As String = "select Course_id from M_testinfo where test_name='" & str(1) & "'"
    '                Dim sadp As New SqlDataAdapter(qr, objconn.MyConnection)
    '                Dim sd As New DataSet
    '                sadp.Fill(sd)
    '                For ind As Integer = 0 To sd.Tables(0).Rows.Count - 1

    '                    'Dim fields1 As String = "course_id"
    '                    'Dim query1 As String = "Select " & fields1 & " From m_course where Course_name='" & sd.Tables(0).Rows(ind).Item(0).ToString & "' "
    '                    'Dim adp As New SqlDataAdapter(query1, objconn.MyConnection)
    '                    'Dim dt As New DataTable()
    '                    'adp.Fill(dt)
    '                    Cid = sd.Tables(0).Rows(ind).Item(0).ToString

    '                    'Dim sda As New SqlDataAdapter("select Course_Id from m_testinfo where test_name='" & str(1) & "'", objconn.MyConnection)
    '                    'Dim ds1 As New DataSet
    '                    'sda.Fill(ds1)
    '                    'Cid = ds1.Tables(0).Rows(0).Item(0).ToString

    '                    '    Dim fields As String = "m_main_Course.main_course_name as main_Course,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.Sub_Code As Sub_Code,m_testinfo.test_type as test_type,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques"
    '                    query = "Update M_Testinfo set del_flag=1 where test_name ='" & str(1) & "'"
    '                    Dim cmd As New SqlCommand(query, objconn.MyConnection)
    '                    cmd.ExecuteNonQuery()

    '                    UpdateQuestionDetails(Cid)


    '                Next
    '                ' find cid

    '                objconn.disconnect()
    '                BindGrid()
    '                ' UpdateQuestionDetails(Cid)
    '                ClearControls()
    '                lblMsg.ForeColor = Drawing.Color.Green
    '                lblMsg.Visible = True
    '                lblMsg.Text = Resources.Resource.SubjectMaintenance_Subdelsuc

    '            End If

    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx?err=" & ex.Message, False)
    '    End Try
    'End Sub
    ' Commented by Rajat 
    'Protected Sub imgBtnUpdate_Click(sender As Object, e As EventArgs) Handles imgBtnUpdate.Click
    '    Try

    '        Dim bolFlag As Boolean = Validations()
    '        If (bolFlag = True) Then
    '            'ElseIf multiBoxId.SelectedIndex > -1 And txtSubjectName.Text <> String.Empty And SubCode.Text <> String.Empty And txtHQues.Text <> String.Empty And txtMQtn.Text <> String.Empty And txtLQtn.Text <> String.Empty And txtCorretAns.Text <> String.Empty Then

    '            'If multiBoxId.SelectedIndex > -1 And txtSubjectName.Text <> String.Empty And txtHQues.Text <> String.Empty And txtMQtn.Text <> String.Empty And txtLQtn.Text <> String.Empty And txtCorretAns.Text <> String.Empty Then
    '            If txtSubjectName.Text <> String.Empty Then
    '                GetTotal()
    '                UpdateData()

    '                'multiBoxId.Items.Clear()
    '                FillCoursesCombo()
    '                BindGrid()
    '                DataGridSubjectDetails.Visible = True
    '                gridDiv.Visible = True
    '                lblMsg.Visible = True
    '                lblMsg.ForeColor = Drawing.Color.Green
    '                lblMsg.Text = Resources.Resource.SubjectMaintenance_Subdtsupdated
    '                txtSubCode.Enabled = True

    '                'Else
    '                '    lblMsg.Visible = True
    '                '    lblMsg.ForeColor = Drawing.Color.Red
    '                '    lblMsg.Text = "All Fields Are Mandatory......"
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx")
    '    End Try
    'End Sub

    Protected Sub DataGridSubjectDetails_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridSubjectDetails.PageIndexChanged
        Try
            DataGridSubjectDetails.CurrentPageIndex = e.NewPageIndex
            ' SearchGrid()
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub


    'Protected Sub multiBoxId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles multiBoxId.SelectedIndexChanged

    '    'Dim query As String = "SELECT isnull(MAX(test_type) + 1,1) FROM M_Testinfo "
    '    'strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '    'If objconn.connect(strPathDb) = True Then
    '    '    Dim sqlDA As New SqlDataAdapter(query, objconn.MyConnection)
    '    '    Dim dss As New DataSet
    '    '    sqlDA.Fill(dss)

    '    '    testType = dss.Tables(0).Rows(0).Item(0).ToString()
    '    'End If



    'End Sub

    '********************************************************************
    'Code added by Monal shah
    'Purpose: All Validation
    '********************************************************************
    Public Function Validations() As Boolean
        Dim objcommon As New CommonFunction()
        Try
            'Dim HighQuestion As Boolean = ValidationForNumber(txtHQues.Text)
            'Dim MiddleQuestion As Boolean = ValidationForNumber(txtMQtn.Text)
            'Dim LowQuestion As Boolean = ValidationForNumber(txtLQtn.Text)
            'Dim CorrectAnswer As Boolean = ValidationForNumber(txtCorretAns.Text)
            'If multiBoxId.SelectedIndex = -1 Then
            '    lblMsg.Visible = True
            '    lblMsg.ForeColor = Drawing.Color.Red
            '    lblMsg.Text = "Please Select At Least One Course."
            '    multiBoxId.Focus()
            '    Return False

            If txtSubjectName.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.ForeColor = Drawing.Color.Red
                lblMsg.Text = Resources.Resource.SubjectRegistration_subname
                txtSubjectName.Focus()
                Return False
                'ElseIf txtHQues.Text = String.Empty Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Number Of High Level Question Value."
                '    txtHQues.Focus()
                '    Return False
                'ElseIf txtMQtn.Text = String.Empty Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Number Of Middle Level Question Value."
                '    txtMQtn.Focus()
                '    Return False
                'ElseIf txtLQtn.Text = String.Empty Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Number Of Low Level Question Value."
                '    txtLQtn.Focus()
                '    Return False
                'ElseIf txtCorretAns.Text = String.Empty Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Number Of Correct Answer Value."
                '    txtCorretAns.Focus()
                'Return False
                'ElseIf (HighQuestion = False) Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Numbers Only In High Level Question."
                '    txtHQues.Focus()
                '    Return False
                'ElseIf (MiddleQuestion = False) Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Numbers Only In Middle Level Question."
                '    txtMQtn.Focus()
                '    Return False
                'ElseIf (LowQuestion = False) Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Numbers Only In Low Level Question."
                '    txtLQtn.Focus()
                '    Return False
                'ElseIf (CorrectAnswer = False) Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please Enter Numbers Only In Correct Answer."
                '    txtCorretAns.Focus()
                '    Return False
                'ElseIf (Convert.ToInt32(txtCorretAns.Text) > Convert.ToInt32(lblTotalAllotedQtn.Text)) Then
                '    lblMsg.Text = "Correct Answer must be less than or equal to Alloted Question"
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Visible = True
                '    txtCorretAns.Focus()

            ElseIf chkcourse.Checked = True Then
                If ddlMainCourse.SelectedValue = 0 Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.SubjectRegistration_maincrs
                    Return False
                End If
                'If multiBoxIdcenter.SelectedIndex = -1 Then
                '    lblMsg.Visible = True
                '    lblMsg.ForeColor = Drawing.Color.Red
                '    lblMsg.Text = "Please select Centre Name."
                '    Return False
                'End If
                If txtcoursecode.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.CourseRegistration_crscd
                    txtcoursecode.Focus()
                    Return False
                End If
                If txtTotalTime.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.CourseRegistration_taet
                    txtTotalTime.Focus()
                    Return False
                End If

                If txtTotalMarks.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.CourseRegistration_totalmrks
                    txtTotalMarks.Focus()
                    Return False
                End If


                If txtPassMarks.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.SubjectMaintenance_passingmarks
                    txtPassMarks.Focus()
                    Return False
                End If

                If Not txtSubjectName.Text = String.Empty Then
                    Dim booldecision As Boolean = objcommon.ValidateCourseName(txtSubjectName.Text)
                    If Not booldecision Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Drawing.Color.Red
                        lblMsg.Text = Resources.Resource.SubjectMaintenance_subexist
                        Return False
                    End If
                End If
                Return True
            Else
                Return True
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Function


    '********************************************************************
    'Code added by Indravadan Vasava
    'Purpose: To get next Test_Type to be inserted in m_testinfo table
    '********************************************************************
    Public Function getNextTestType()
        Dim query As String = "SELECT isnull(MAX(test_type) + 1,1) FROM M_Testinfo "
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Dim dss As New DataSet
        Try
            If objconn.connect() = True Then
                Dim sqlDA As New SqlDataAdapter(query, objconn.MyConnection)

                sqlDA.Fill(dss)
                testType = dss.Tables(0).Rows(0).Item(0).ToString()
                Return testType
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
            dss = Nothing
        End Try
    End Function

#Region "Get next courseid"
    'Desc: This method returns next courseid.
    'By: Jatin Gangajaliya, 2011/03/23

    Private Function getnextcourseid()
        Dim query As String = "SELECT isnull(MAX(Course_id) + 1,1) FROM M_Course "
        Dim dss As New DataSet
        Try
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() = True Then
                Dim sqlDA As New SqlDataAdapter(query, objconn.MyConnection)
                sqlDA.Fill(dss)
                testType = dss.Tables(0).Rows(0).Item(0).ToString()
                Return testType
                objconn.disconnect()
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
            dss = Nothing
        End Try
    End Function
#End Region


    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        Try
            ClearControls()
            ViewState.Remove("selval")
            ViewState.Remove("pageNo")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx?err=" & ex.Message, False)
        End Try
    End Sub


    '********************************************************************
    'Code added by Indravadan Vasava
    'Purpose: To Update total questions in M_Course table
    '********************************************************************
    Public Sub UpdateQuestionDetails(ByVal Cid As String)
        Dim query As String = ""
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try

            If objconn.connect() = True Then
                '    Dim fields As String = "m_main_Course.main_course_name as main_Course,m_course.Course_Name as Course_name,m_testinfo.test_Name As test_Name,m_testinfo.Sub_Code As Sub_Code,m_testinfo.test_type as test_type,m_testinfo.no_of_ques as no_of_ques,m_testinfo.l_ques as l_ques,m_testinfo.m_ques as m_ques,m_testinfo.h_ques as h_ques"
                'query = "Update M_Testinfo set del_flag=1 where Sub_Code ='" & e.CommandArgument & "'"
                'Dim cmd As New SqlCommand(query, objconn.MyConnection)
                'cmd.ExecuteNonQuery()
                'objconn.disconnect()
                'BindGrid()
                'lblMsg.ForeColor = Drawing.Color.Green
                'lblMsg.Visible = True
                'lblMsg.Text = "Subject Deleted Successfully"

                Dim sda As SqlDataAdapter
                Dim dss As New DataSet
                Dim totalQues As String = ""
                Dim totalCorrect As String = ""

                If (Cid = "") Then
                    'For i As Integer = 0 To multiBoxId.Items.Count - 1
                    '    If multiBoxId.Items(i).Selected = True Then
                    Try
                        '/*********************Start*********************/
                        'Note: Commented by Jatin Gangajaliya to accomodate new changes, 2011/04/20.
                        'query = "SELECT   SUM(no_of_ques) , SUM(Correct_Ans)  FROM M_Testinfo WHERE del_flag='0' AND Course_ID = " & multiBoxId.Items(i).Value
                        'sda = New SqlDataAdapter(query, objconn.MyConnection)
                        'dss.Clear()
                        'sda.Fill(dss)
                        'totalQues = dss.Tables(0).Rows(0).Item(0).ToString
                        'totalCorrect = dss.Tables(0).Rows(0).Item(1).ToString

                        'Note: Commented by Jatin Gangajaliya to accomodate new changes, 2011/04/20.
                        'Dim up_query As String = "UPDATE M_Course SET Total_Question = " & totalQues & ", Correct_Ans = " & totalCorrect & " WHERE Course_id =" & multiBoxId.Items(i).Value
                        'Dim cmd As New SqlCommand(up_query, objconn.MyConnection)
                        'cmd.ExecuteNonQuery()
                        '/**********************End**********************/

                    Catch ex As Exception
                        If log.IsDebugEnabled Then
                            log.Debug("Error :" & ex.ToString())
                        End If
                        Throw ex
                    Finally
                        'sda.Dispose()
                        'dss.Dispose()
                    End Try
                End If
                'Next

            Else

                '/*********************Start*********************/
                'Note: Commented by Jatin Gangajaliya to accomodate new changes, 2011/04/20.
                'Try
                '    query = "SELECT  isnull(SUM(no_of_ques),0) , isnull(SUM(Correct_Ans),0)  FROM M_Testinfo WHERE del_flag='0' AND Course_ID = " & Cid
                '    sda = New SqlDataAdapter(query, objconn.MyConnection)
                '    dss.Clear()
                '    sda.Fill(dss)
                '    totalQues = dss.Tables(0).Rows(0).Item(0).ToString
                '    totalCorrect = dss.Tables(0).Rows(0).Item(1).ToString

                '    Dim up_query As String = "UPDATE M_Course SET Total_Question = " & totalQues & ", Correct_Ans = " & totalCorrect & " WHERE Course_id =" & Cid
                '    Dim cmd As New SqlCommand(up_query, objconn.MyConnection)
                '    cmd.ExecuteNonQuery()

                'Catch ex As Exception
                '    If log.IsDebugEnabled Then
                '        log.Debug("Error :" & ex.ToString())
                '    End If
                '    Throw ex
                'Finally
                '    sda.Dispose()
                '    dss.Dispose()
                'End Try
                '/**********************End**********************/

            End If

            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

    Protected Sub DataGridSubjectDetails_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridSubjectDetails.ItemDataBound
        Try
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx?err=" & ex.Message, False)
        End Try
    End Sub


    Protected Sub DataGridSubjectDetails_EditCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridSubjectDetails.EditCommand

    End Sub


    ' Code Added By Rajat Argade on 10/12/2019 
    Private Sub dgProblems_EditCommand(ByVal source As Object,
                                  ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
                              Handles DataGridSubjectDetails.EditCommand
        If e.CommandName = "lnkEdit" Then
            lnkEditID = e.CommandArgument
            Session.Add("testid", lnkEditID)
        End If
        DataGridSubjectDetails.EditItemIndex = e.Item.ItemIndex
        BindGrid()
    End Sub  'dgProblems_EditCommand

    '*************************************************************************
    '
    '   ROUTINE: dgProblems_CancelCommand
    '
    '   DESCRIPTION: This routine provides the event handler for the cancel 
    '                command click event.  It is responsible for resetting the 
    '                edit item index to no item and rebinding the data.
    '-------------------------------------------------------------------------
    Private Sub dgProblems_CancelCommand(ByVal source As Object,
                                  ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
                              Handles DataGridSubjectDetails.CancelCommand
        DataGridSubjectDetails.EditItemIndex = -1
        BindGrid()
    End Sub  'dgProblems_CancelCommand

    '*************************************************************************
    '
    '   ROUTINE: dgProblems_UpdateCommand
    '
    '   DESCRIPTION: This routine provides the event handler for the update 
    '                command click event.  It is responsible for updating 
    '                the contents of the database with the date entered for
    '                the item currently being edited.
    '-------------------------------------------------------------------------
    Private Sub dgProblems_UpdateCommand(ByVal source As Object,
                                  ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) _
                              Handles DataGridSubjectDetails.UpdateCommand

        'Dim dbConn As OleDbConnection
        'Dim dCmd As OleDbCommand
        Dim strtestname As String
        Dim strsubcode As String
        Dim strSQL As String
        Dim mycmd As SqlCommand
        'Dim test_type As String = ""
        Dim test_type As Int64
        Dim arrlist As New ArrayList
        Dim dt As DataTable = CType(Session("Source"), DataTable)
        'Dim myDataSet As DataSet
        'Dim myDataAdapter As SqlDataAdapter
        'Dim sqlstr As String
        'Dim strSr As Int64
        Try
            objconn.connect()
            strtestname = CType(e.Item.FindControl("txttestnamegrid"),
                                                 TextBox).Text()
            strsubcode = CType(e.Item.FindControl("txtsubcodegrid"), TextBox).Text()
            'UpdateData(strtestname, strsubcode)
            'test_type = CType(e.Item.FindControl("hfid"),  HiddenField).Value()

            test_type = DataGridSubjectDetails.DataKeys(e.Item.ItemIndex)
            'sqlstr = "Select test_type from M_Testinfo"
            'myDataSet = New DataSet()
            'myDataAdapter = New SqlDataAdapter(sqlstr, objconnect)
            'myDataAdapter.Fill(myDataSet)

            'If myDataSet.Tables(0).Rows.Count >= 1 Then
            '    For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
            '        arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString())
            '    Next
            'End If
            'strSr = CType(e.Item.FindControl("txttestnamegrid"),
            '                                     TextBox).Text()
            'test_type

            'update data in database
            'NOTE: The primary key used to uniquely identify the row being edited  
            '      is accessed through the DataKeys collection of the DataGrid.
            strSQL = "Update M_Testinfo Set test_name = '" & strtestname & "' ," & " Sub_Code = '" & strsubcode & "' where test_type ='" & test_type & "'"
            mycmd = New SqlCommand(strSQL, objconn.MyConnection)
            mycmd.ExecuteNonQuery()


            'TODO: production code should check the number of rows affected here to
            'make sure it is exactly 1 and output the appropriate success or
            'failure informatio to the user.

            'reset the edit item and rebind the data
            DataGridSubjectDetails.EditItemIndex = -1
            BindGrid()

        Finally
            'cleanup
            If (Not IsNothing(objconn)) Then
                objconn.disconnect()
            End If
        End Try
    End Sub  'dgProblems_UpdateCommand
    ' Code Ended By Rajat Argade.










#Region "Main CourseCombo Binding Method"
    'Desc: This method binds maincourse combobox.
    'By: Jatin Gangajaliya, 2011/03/23

    Public Sub FillMainCourseCombo()
        ddlMainCourse.Items.Clear()
        Dim l1 As New ListItem
        l1.Text = "---- Select ----"
        l1.Value = 0

        ddlMainCourse.Items.Add(l1)
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Dim Main_Course As String
        Try
            If objconn.connect() Then
                Main_Course = "Select Main_Course_ID,Main_Course_Name From M_Main_Course order by Main_Course_Name"
                objCommand = New SqlCommand(Main_Course, objconn.MyConnection)
                objDataReader = objCommand.ExecuteReader()
                While objDataReader.Read
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader.Item(1)
                    lstItm.Value = objDataReader.Item(0)
                    ddlMainCourse.Items.Add(lstItm)
                End While
                objDataReader.Close()
                objconn.disconnect()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            Main_Course = Nothing
        End Try
    End Sub
#End Region

#Region "Fill Centre method"
    'Desc: This method fills centre listbox
    'By: Jatin Gangajaliya, 2011/03/23

    Public Sub FillCenterList()

        '    multiBoxIdcenter.Items.Clear()
        '    strPathDb = ConfigurationManager.AppSettings("PathDb")
        '    Dim Centers As String
        '    Try
        '        If objconn.connect(strPathDb) Then
        '            Centers = "Select center_id,Center_Name From M_Centers order by Center_Name"
        '            objCommand = New SqlCommand(Centers, objconn.MyConnection)
        '            objDataReader = objCommand.ExecuteReader()
        '            While objDataReader.Read
        '                Dim lstItm As New ListItem()
        '                lstItm.Enabled = True
        '                lstItm.Text = objDataReader.Item(1)
        '                lstItm.Value = objDataReader.Item(0)
        '                multiBoxIdcenter.Items.Add(lstItm)
        '            End While
        '            objDataReader.Close()
        '            objconn.disconnect()
        '        End If
        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        Throw ex
        '    Finally
        '        Centers = Nothing
        '    End Try
    End Sub
#End Region

#Region "Checkbox checkchanged event"
    'Desc: This is checkbox checkchanged event.
    'By: Jatin Gangajaliya, 2011/03/23.

    Protected Sub chkcourse_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcourse.CheckedChanged
        Try
            If chkcourse.Checked = True Then
                maincourserow.Visible = True
                'centrerow.Visible = True
                textboxcoderow.Visible = True
                textboxrow.Visible = True
                textMarksrow.Visible = True
                textPassmarksrow.Visible = True
            Else
                maincourserow.Visible = False
                ' centrerow.Visible = False
                textboxcoderow.Visible = False
                textboxrow.Visible = False
                textMarksrow.Visible = False
                textPassmarksrow.Visible = False
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx?err=" & ex.Message, False)
        End Try
    End Sub
#End Region

    Sub Selection_Change(sender As Object, e As EventArgs)
        Try
            DataGridSubjectDetails.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

#Region "Insert subject as a course"
    'Desc: This method inserts subject as a course.
    'By: Jatin Gangajaliya, 2011/03/23

    Private Sub InsertCourse(ByVal strcourseid As String)
        Dim strquery, strfields, strtestno As String
        Dim objconn As New ConnectDb
        Dim strbr As New StringBuilder
        Dim sqlcmd As New SqlCommand
        'Dim sqlTrans As SqlTransaction
        strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try

            dblpercentage = (CInt(txtTotalMarks.Text) * CInt(txtPassMarks.Text)) / 100
            intpercentage = Math.Round(dblpercentage)

            'Insert into M_course table.
            strfields = " Course_name,Course_code,Main_Course_id,total_time,Total_Marks,Total_passmarks,Total_passmarks_Per "
            strquery = "Insert Into M_Course (" & strfields & ") values('" & txtSubjectName.Text & "','" & txtcoursecode.Text & "','" & ddlMainCourse.SelectedValue & "'," & "'" & txtTotalTime.Text & "'," & "'" & txtTotalMarks.Text & "','" & intpercentage & "','" & txtPassMarks.Text & "')"
            'If objconn.connect(strPathDb) = True Then
            sqlcmd = New SqlCommand(strquery, objconnect, sqlTransin)
            sqlcmd.ExecuteNonQuery()
            'End If

            'Insert into T_Center_Course table.
            ''For i As Integer = 0 To multiBoxIdcenter.Items.Count - 1
            ''    If multiBoxIdcenter.Items(i).Selected = True Then
            ''        strbr = New StringBuilder
            ''        strbr.Append(" Insert Into T_Center_Course (Course_ID,Center_ID) values ")
            ''        strbr.Append(" ( ")
            ''        strbr.Append(strcourseid)
            ''        strbr.Append(" , ")
            ''        strbr.Append(multiBoxIdcenter.Items(i).Value)
            ''        strbr.Append(" ) ")
            ''        strquery = strbr.ToString()
            ''        'If objconn.connect(strPathDb) = True Then
            ''        sqlcmd = New SqlCommand(strquery, objconnect, sqlTransin)
            ''        sqlcmd.ExecuteNonQuery()
            ''        'End If
            ''    End If
            ''Next

            'Insert into M_Testinfo table.
            'strtestno = getNextTestType()
            'strfields = " test_type,test_Name,Sub_Code"
            'strquery = "Insert Into M_Testinfo (" & strfields & ") values('" & strtestno & "','" & txtSubjectName.Text & "','" & txtSubCode.Text & "'" & ")"

            strfields = "test_Name,Sub_Code"
            strquery = "Insert Into M_Testinfo (" & strfields & ") values(" & "'" & txtSubjectName.Text & "','" & txtSubCode.Text & "'" & ")"
            'If objconn.connect(strPathDb) = True Then
            sqlcmd = New SqlCommand(strquery, objconnect, sqlTransin)
            sqlcmd.ExecuteNonQuery()
            'End If
            'objconn.disconnect()

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            strquery = Nothing
            strfields = Nothing
            objconn = Nothing
            strbr = Nothing
            sqlcmd = Nothing
        End Try
    End Sub
#End Region


#Region "Enable Disable"
    Public Function enableDisable(ByVal flg As Boolean, ByVal blflg As Boolean)
        Dim chk As New CheckBox
        Dim strid As String
        Dim q As String = String.Empty
        Dim cid As String
        Dim bolflg As Boolean = True
        Try
            For Each rowItem As DataGridItem In DataGridSubjectDetails.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                If chk.Checked Then
                    bolflg = False

                    strid = rowItem.Cells(6).Text

                    If objconn.connect() = True Then
                        If blflg = True Then
                            q = "update M_testinfo set del_flag=0 where test_type='" & strid & "'"
                        Else
                            q = "update M_testinfo set del_flag=1 where test_type='" & strid & "'"
                        End If

                        '/*********************Start*******************/
                        'By: Jatin Ganagajaliya, 2011/04/21.
                        'Dim strbr() As String = strid.Split(",")
                        'Dim fields1 As String = "course_id"
                        'Dim query1 As String = "Select " & fields1 & " From m_course where Course_name='" & strbr(0) & "' "
                        'Dim adp As New SqlDataAdapter(query1, objconn.MyConnection)
                        'Dim dt As New DataTable()
                        'adp.Fill(dt)
                        'cid = dt.Rows(0)("Course_id").ToString()
                        'If (flg = True) Then
                        'Else
                        'q = "update M_testinfo set del_flag=0 where Course_Id =" & cid & " and test_name='" & strbr(1) & "'"
                        'End If
                        '/********************End**********************/

                        Dim cmd As New SqlCommand(q, objconn.MyConnection)

                        If blflg = True Then
                            cmd.ExecuteNonQuery()
                        Else
                            Dim booldecision As Boolean = CheckStatus(strid)
                            If booldecision = True Then
                                cmd.ExecuteNonQuery()
                            Else
                                lblMsg.Visible = True
                                lblMsg.ForeColor = Drawing.Color.Red
                                lblMsg.Text = Resources.Resource.SubjectMaintenance_subdisreff
                            End If
                        End If

                    End If
                End If
            Next
            If bolflg = True Then
                If blflg = False Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.SubjectMaintenance_sltonechkdisable
                ElseIf blflg = True Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Text = Resources.Resource.SubjectMaintenance_sltonechkenable
                End If
                'Else
                '    lblMsg.Visible = False
            End If
            '  Dim intindex As Integer = DataGridSubjectDetails.CurrentPageIndex
            BindGrid()
            ' DataGridSubjectDetails.CurrentPageIndex = intindex
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("ChkSelect ALL : ", ex)
                Response.Redirect("error.aspx", False)
            End If
        Finally
            chk = Nothing
            strid = Nothing
            q = Nothing
            cid = Nothing
        End Try

    End Function
#End Region

#Region "Event for Checking and unchecking all checkboxes"
    'Desc: This is event for checking and unchecking all checkboxes.
    'By: monal shah, 2011/3/18

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim chk As CheckBox = Nothing
        Try

            For Each rowItem As DataGridItem In DataGridSubjectDetails.Items

                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

                chk.Checked = DirectCast(sender, CheckBox).Checked

            Next
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("ChkSelect ALL : ", ex)
                Response.Redirect("error.aspx", False)
            End If
        Finally
            chk = Nothing
        End Try

    End Sub
#End Region

    Protected Sub chkRemove_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Protected Sub imgBtnEnable_Click(sender As Object, e As EventArgs) Handles imgBtnEnable.Click
        Try
            lblMsg.Text = String.Empty
            enableDisable(False, True)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    Protected Sub imgBtnDisable_Click(sender As Object, e As EventArgs) Handles imgBtnDisable.Click
        Try
            lblMsg.Text = String.Empty
            enableDisable(True, False)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    '**************************************************************************'
    ''Author: Irfan Mansuri                                                    '
    ''Description: To delete(Physically from database) selected Subjects.      '
    ''Created Date: 25/02/2015                                                 '
    '**************************************************************************'
    Protected Sub imgBtnDelete_Click(sender As Object, e As EventArgs) Handles imgBtnDelete.Click
        Dim chk As CheckBox = Nothing
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Dim strBuild As StringBuilder
        Dim strQ, strQuery As String
        Dim strTest_type_id As String
        Dim boldecision As Boolean = True
        Dim objConstant As Constant
        Try
            lblMsg.Visible = False
            objConstant = New Constant()
            strBuild = New StringBuilder
            strBuild.Append(" (")
            For Each rowItem As DataGridItem In DataGridSubjectDetails.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                If chk.Checked Then
                    boldecision = False
                    'strid = DirectCast(rowItem.Cells(1).Text, String)
                    strTest_type_id = Convert.ToString(DataGridSubjectDetails.DataKeys(rowItem.ItemIndex))
                    strBuild.Append(strTest_type_id)
                    strBuild.Append(" , ")
                End If
            Next

            If boldecision = True Then
                lblMsg.Visible = True
                lblMsg.ForeColor = Color.Red
                lblMsg.Text = Resources.Resource.SubjectMaintenance_sltonedel ''Alert message
                Exit Sub
            End If


            strQ = strBuild.ToString
            strQ = strQ.Substring(0, strQ.Length - 2)
            strQ = strQ & ")"

            If objconn.connect() Then
                strBuild = New StringBuilder
                strBuild.Append("Delete M_TestInfo ")
                strBuild.Append(" where M_TestInfo.test_type IN ")
                strBuild.Append(strQ)
                strQuery = strBuild.ToString()
                myCommand = New SqlCommand(strQuery, objconn.MyConnection)
                myCommand.ExecuteNonQuery()
            End If
            objconn.disconnect()
            lblMsg.Visible = True
            lblMsg.ForeColor = Color.Green
            lblMsg.Text = Resources.Resource.SubjectMaintenance_sltdelsuc
            Dim intindex As Integer = DataGridSubjectDetails.CurrentPageIndex
            BindGrid()
            DataGridSubjectDetails.CurrentPageIndex = intindex
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objConstant = Nothing
            strQ = Nothing
            strQuery = Nothing
            strBuild = Nothing
            strPathDb = Nothing
            chk = Nothing
            objconn = Nothing
            boldecision = Nothing
        End Try

    End Sub


#Region "Function checkstatus"
    'Desc: This function checks the status of subject before disabling a question.
    'By: Jatin Gangajaliya, 2011/04/22.

    Private Function CheckStatus(ByVal testtype As Integer) As Boolean
        Dim strq As String
        Dim strbr As StringBuilder
        Dim MyCommand As SqlCommand
        Dim bol As Boolean = True
        Dim intcount As Integer
        Try
            strbr = New StringBuilder
            strbr.Append(" Select Count(Weightage_ID) from M_Weightage where test_type = ")
            strbr.Append(testtype)
            strbr.Append(" and Del_Flag = 0 ")
            strq = strbr.ToString()
            If objconn.connect() = True Then
                MyCommand = New SqlCommand(strq, objconn.MyConnection)
                intcount = MyCommand.ExecuteScalar()
            End If
            If intcount = 0 Then
                Return bol = True
            Else
                Return bol = False
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("ChkSelect ALL : ", ex)
                Response.Redirect("error.aspx", False)
            End If
            Throw ex
        Finally
            strq = Nothing
            strbr = Nothing
            MyCommand = Nothing
        End Try
    End Function
#End Region

    Private Sub fillPageNumbers(ByVal range As Integer, ByVal len As Integer)



        If (Session("last") Is Nothing) Then
            Session.Add("last", 1)
            ViewState.Add("lastRange", 1)
        Else
            ViewState("lastRange") = CInt(Session("last"))
        End If

        'If (ViewState("lastRange") Is Nothing) Then
        '    ViewState.Add("lastRange", 1)
        'End If


        If len >= DataGridSubjectDetails.PageCount Then
            len = DataGridSubjectDetails.PageCount - 1
        End If

        ' if search clicked again then page 1 should be selected 
        If DataGridSubjectDetails.CurrentPageIndex = 0 Then
            ViewState("pageNo") = 1
            ViewState("lastRange") = 1
        End If

        ' Getting the currently selected page value 
        Dim selPage As Integer = 0
        If (ViewState("pageNo") <> Nothing) Then
            selPage = CInt(ViewState("pageNo"))
        Else
            ' selPage = 1
            selPage = DataGridSubjectDetails.CurrentPageIndex + 1
        End If

        If (ViewState("lastRange") <> Nothing) Then

            '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DataGridSubjectDetails.PageCount Then
            If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
                range = CInt(ViewState("lastRange"))
            Else
                'If it is the last page then resetting the page numbers
                ' last number - provided length
                'If (len + selPage) >= DataGridSubjectDetails.PageCount Then
                '    If selPage <= len Then
                '        range = range
                '    Else
                '        range = DataGridSubjectDetails.PageCount - len
                '        'Incase range becomes 0 or less than zero than setting it 1 
                '        If range <= 0 Then
                '            range = 1
                '        End If
                '    End If

                'Else
                If selPage <= DataGridSubjectDetails.PageCount Then
                    'range = range
                    If range < CInt(ViewState("lastRange")) Then
                        range = CInt(ViewState("lastRange")) - 1
                    Else
                        ' range = CInt(ViewState("lastRange")) + 1
                        If selPage - len > 0 And selPage - len <= DataGridSubjectDetails.PageCount - len Then
                            range = selPage - len
                        Else
                            range = CInt(ViewState("lastRange")) + 1
                        End If
                    End If

                End If
            End If
        Else
            range = 1
        End If

        tblPagebuttons.Rows(0).Cells.Clear()

        'Creating the Page numbers
        Dim lim As Integer = range + len
        For i As Integer = range To lim
            Dim lbtn As New LinkButton()
            lbtn.ID = "lbtn" & i.ToString()
            'lbtn.ID = i.ToString()
            lbtn.Text = i.ToString()
            lbtn.CommandName = i.ToString
            lbtn.CommandArgument = i.ToString
            AddHandler lbtn.Command, New CommandEventHandler(AddressOf PagerButtonClickLinks)
            Dim colorBackground As Color = Color.FromArgb(0, 71, 117)
            lbtn.ForeColor = colorBackground

            If selPage = i Then
                lbtn.Font.Overline = False
            End If


            Dim c1 As New HtmlTableCell()
            c1.Controls.Add(lbtn)
            tblPagebuttons.Rows(0).Cells.Add(c1)
        Next
        Session("last") = range
        ViewState("lastRange") = range
        ViewState.Add("Llimit", range)
        ViewState.Add("Ulimit", lim)

        'Setting Enable / Disable on Navigation buttons
        'If selPage = 1 And selPage = DataGridSubjectDetails.PageCount - 1 Then
        'Else
        imgprev.Enabled = True
        imgfirst.Enabled = True
        imgnext.Enabled = True
        imglast.Enabled = True
        'End If

        If selPage = 1 Then
            imgprev.Enabled = False
            imgfirst.Enabled = False
        End If
        If selPage = DataGridSubjectDetails.PageCount Then
            imgnext.Enabled = False
            imglast.Enabled = False
        End If

    End Sub
    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGridSubjectDetails.CurrentPageIndex < (DataGridSubjectDetails.PageCount - 1)) Then
                    DataGridSubjectDetails.CurrentPageIndex += 1

                End If

            Case "prev" 'The prev button was clicked
                If (DataGridSubjectDetails.CurrentPageIndex > 0) Then
                    DataGridSubjectDetails.CurrentPageIndex -= 1
                End If

            Case "last" 'The Last Page button was clicked
                DataGridSubjectDetails.CurrentPageIndex = (DataGridSubjectDetails.PageCount - 1)

            Case Else 'The First Page button was clicked
                DataGridSubjectDetails.CurrentPageIndex = Convert.ToInt32(arg)
        End Select
        ViewState.Add("pageNo", DataGridSubjectDetails.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGridSubjectDetails.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGridSubjectDetails.CurrentPageIndex < (DataGridSubjectDetails.PageCount - 1)) Then
                    DataGridSubjectDetails.CurrentPageIndex += 1
                    '    ViewState.Add("selval", DataGridSubjectDetails.CurrentPageIndex)
                End If

            Case "prev" 'The prev button was clicked
                If (DataGridSubjectDetails.CurrentPageIndex > 0) Then
                    DataGridSubjectDetails.CurrentPageIndex -= 1
                    '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                End If

            Case "last" 'The Last Page button was clicked
                DataGridSubjectDetails.CurrentPageIndex = (DataGridSubjectDetails.PageCount - 1)
                'ViewState.Add("selval", ddlPages.SelectedItem.Value)
            Case Else 'The First Page button was clicked
                DataGridSubjectDetails.CurrentPageIndex = Convert.ToInt32(arg) - 1
                ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
        End Select

        ViewState.Add("pageNo", DataGridSubjectDetails.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGridSubjectDetails.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    'Public Sub fillPagesCombo()
    '    ddlPages.Items.Clear()
    '    For cnt As Integer = 1 To DataGridSubjectDetails.PageCount
    '        Dim lstitem As New ListItem
    '        lstitem.Value = cnt - 1
    '        lstitem.Text = cnt
    '        ddlPages.Items.Add(lstitem)
    '        If Not ViewState("selval") Is Nothing Then
    '            If CInt(ViewState("selval")) = lstitem.Value Then
    '                ddlPages.SelectedValue = lstitem.Value
    '            End If
    '        End If

    '        lstitem = Nothing
    '    Next

    'End Sub

    'Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
    '    DataGridSubjectDetails.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    BindGrid()
    'End Sub

End Class