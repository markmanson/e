Imports System.Data
Imports System.Data.SqlClient
Partial Public Class CourseRegistration
    Inherits BasePage

#Region "declarations"
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("CourseRegistration")
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand
    Dim objDataReader As SqlDataReader
    Dim CourseId As Integer
    Dim sqlTrans As SqlTransaction
    Dim flg As Boolean
    Dim strtemp As String = ""
    Dim dblpercentage As Double
    Dim intpercentage As Integer
    Dim intPassPer As Integer
#End Region



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            txtTotalTime.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            'ddlMainCourse.Attributes.Add("onkeypress", "addTitleAttributes()")
            'ddlSectionDes.Attributes.Add("onkeypress", "addtitleattributesSection()")
            'strtemp = TxtCourseName.Text

            If Session.Item("CourseIDValue") <> Nothing Or Session.Item("CourseIDValue") <> 0 Then
                lblhead.Text = Resources.Resource.CourseRegistration_crsmd
                legendLabel.Text = Resources.Resource.CourseRegistration_crsmddts
            Else
                legendLabel.Text = Resources.Resource.CourseRegistration_CrsRegisDts
                lblhead.Text = Resources.Resource.CourseRegistration_CrsRegis
            End If

            If Not IsPostBack Then
                FillMainCourseCombo()
                FillSectionCourseCombo()
                '   FillCenterList()
                CourseId = Convert.ToInt32(Session.Item("CourseIDValue"))
                'ddlCourses.Items.Clear()
                'Dim l1 As New ListItem
                'l1.Text = "---- Select ----"
                'l1.Value = 0
                'ddlCourses.Items.Add(l1)
                EditMode()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            'Session.Item("CourseIDValue") = Nothing
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
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
#End Region


    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the combo box for Course
    '********************************************************************
    Public Sub EditMode()
        Dim fbl As Boolean
        If (Session.Item("CourseID") = Nothing Or Session.Item("CourseID") = 0) Then
            CourseId = Convert.ToInt32(Session.Item("CourseIDValue"))
            Session.Add("CourseID", CourseId)
        Else
            CourseId = Convert.ToInt32(Session.Item("CourseID"))
        End If

        If CourseId <> Nothing Or CourseId <> 0 Then
            Dim query As String = ""
            'buupdatespan.Visible = False
            'centernames.Visible = True
            '   insertspan.Visible = False
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Try
                If objconn.connect() = True Then
                    Dim query1 As String = "select Distinct t_center_Course.Course_id,t_center_Course.Center_id as Center_id from M_Course,t_center_Course where M_Course.Course_ID = '" & CourseId & "' and t_center_Course.Course_ID=M_Course.Course_ID"
                    objCommand = New SqlCommand(query1, objconn.MyConnection)
                    objDataReader = objCommand.ExecuteReader()
                    While objDataReader.Read()
                        fbl = True
                    End While
                    objDataReader.Close()
                    If (fbl) Then
                        query = "select Distinct M_Course.Course_name As Course_name,M_Course.Course_Code as Course_Code,M_Course.Total_Time as Total_Time,M_Course.Main_Course_ID as Main_Course_ID,t_center_Course.Center_id as Center_id,M_Course.Total_marks as TotalMarks,M_Course.Total_passmarks as PassMarks,M_Course.Total_passmarks_Per as PassMarksPer from M_Course,t_center_Course where M_Course.Course_ID = '" & CourseId & "' and t_center_Course.Course_ID=M_Course.Course_ID"
                    Else
                        query = "select Distinct M_Course.Course_name As Course_name,M_Course.Course_Code as Course_Code,M_Course.Total_Time as Total_Time,M_Course.Main_Course_ID as Main_Course_ID,M_Course.Total_marks as TotalMarks,M_Course.Total_passmarks as PassMarks,M_Course.Total_passmarks_Per as PassMarksPer  from M_Course where M_Course.Course_ID = '" & CourseId & "'"
                    End If
                    objCommand = New SqlCommand(query, objconn.MyConnection)
                    objDataReader = objCommand.ExecuteReader()
                    While objDataReader.Read()
                        TxtCourseName.Text = objDataReader.Item("Course_name")

                        TxtCourseCode.Text = objDataReader.Item("Course_Code")
                        'txtTotalQues.Text = objDataReader.Item(1)
                        'TxtCorrectAns.Text = objDataReader.Item(2)
                        txtTotalTime.Text = objDataReader.Item("Total_Time")
                        ddlMainCourse.Text = objDataReader.Item("Main_Course_ID")
                        txtTotalMarks.Text = objDataReader.Item("TotalMarks")

                        '/********************Start,Jatin Gangajaliya, 2011/04/20*********/
                        'If Not IsDBNull(objDataReader.Item("PassMarks")) And Not IsDBNull(objDataReader.Item("TotalMarks")) Then
                        '    Dim inttemp As Integer
                        '    Dim dbltemp As Double
                        '    dbltemp = CInt(objDataReader.Item("PassMarks")) * 100 / CInt(objDataReader.Item("TotalMarks"))
                        '    inttemp = Math.Round(dbltemp)
                        '    txtPassingMarks.Text = CStr(inttemp)
                        'End If
                        '/****************************End******************************/
                        txtPassingMarks.Text = objDataReader.Item("PassMarksPer") 'PassMarksPer
                        'If (fbl) Then
                        '    For lst As Integer = 0 To multiBoxId.Items.Count - 1
                        '        If (multiBoxId.Items(lst).Value = objDataReader.Item("Center_id").ToString()) Then
                        '            multiBoxId.Items(lst).Selected = True
                        '        End If
                        '    Next
                        'End If
                    End While

                    If Not TxtCourseName.Text = "" Then
                        Session.Item("cname") = TxtCourseName.Text
                    End If
                    imgBtnUpdate.Visible = True
                    imgBtnReset.Visible = True
                    imgBtnSubmit.Visible = False
                    imgBtnClear.Visible = False
                    objDataReader.Close()
                    objconn.disconnect()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                Session.Item("CourseIDValue") = Nothing
                query = Nothing
            End Try
        End If
    End Sub



    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the combo box for Course
    '********************************************************************
    Public Sub FillMainCourseCombo()

        'ddlMainCourse.Items.Clear()
        'Dim l1 As New ListItem
        'l1.Text = "---- Select ----"
        'l1.Value = 0

        'ddlMainCourse.Items.Add(l1)
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try
            If objconn.connect() Then
                Dim Main_Course As String = "Select Main_Course_ID,Main_Course_Name From M_Main_Course order by Main_Course_Name"
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
            'l1 = Nothing
        End Try
    End Sub

    'Added by Pranit on 16/12/2019
    Public Sub FillSectionCourseCombo()
        Try
            ddlSectionDes.Items.Clear()
            'Dim l1 As New ListItem
            'l1.Text = "---- Select ----"
            'l1.Value = 0
            'ddlSectionDes.Items.Add(l1)


            Dim lRead, lVocab, lGrammar, lChokai, lTechnical As New ListItem()
            lRead.Enabled = True
            lRead.Text = "Reading"
            lRead.Value = "Reading"

            lVocab.Enabled = True
            lVocab.Text = "Vocabulary"
            lVocab.Value = "Vocab"

            lGrammar.Enabled = True
            lGrammar.Text = "Grammmar"
            lGrammar.Value = "Grammer"

            lChokai.Enabled = True
            lChokai.Text = "Listening"
            lChokai.Value = "Chokai"

            lTechnical.Enabled = True
            lTechnical.Text = "Technical"
            lTechnical.Value = "Technical"

            If ddlMainCourse.SelectedItem.Value = 1 Then
                ddlSectionDes.Items.Add(lRead)
                ddlSectionDes.Items.Add(lVocab)
                ddlSectionDes.Items.Add(lGrammar)
                ddlSectionDes.Items.Add(lChokai)
            ElseIf ddlMainCourse.SelectedItem.Value = 2 Then
                ddlSectionDes.Items.Add(lTechnical)
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the ListBox box for Center
    '********************************************************************
    '  Public Sub FillCenterList()
    'multiBoxId.Items.Clear()
    'strPathDb = ConfigurationManager.AppSettings("PathDb")
    'Try
    '    If objconn.connect(strPathDb) Then
    '        Dim Centers As String = "Select center_id,Center_Name From M_Centers"
    '        objCommand = New SqlCommand(Centers, objconn.MyConnection)
    '        objDataReader = objCommand.ExecuteReader()
    '        While objDataReader.Read
    '            Dim lstItm As New ListItem()
    '            lstItm.Enabled = True
    '            lstItm.Text = objDataReader.Item(1)
    '            lstItm.Value = objDataReader.Item(0)
    '            multiBoxId.Items.Add(lstItm)
    '        End While
    '        objDataReader.Close()
    '        objconn.disconnect()
    '    End If
    'Catch ex As Exception
    '    If log.IsDebugEnabled Then
    '        log.Debug("Error :" & ex.ToString())
    '    End If
    '    Throw ex
    'End Try
    '  End Sub


    ''********************************************************************
    ''Code added by Monal Shah
    ''Purpose: To fill the combo box for Course
    ''********************************************************************
    'Public Sub FillCoursesCombo()
    '    ddlCourses.Items.Clear()
    '    Dim l1 As New ListItem
    '    l1.Text = "---- Select ----"
    '    l1.Value = 0

    '    ddlCourses.Items.Add(l1)
    '    Dim query As String = ""
    '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            query = "select Course_ID,Course_Name from M_Course where Main_Course_ID='" & ddlMainCourse.SelectedItem.Value & "' and Del_Flag='0'"
    '            objCommand = New SqlCommand(query, objconn.MyConnection)
    '            objDataReader = objCommand.ExecuteReader()
    '            While objDataReader.Read()
    '                Dim lstItm As New ListItem()
    '                lstItm.Enabled = True
    '                lstItm.Text = objDataReader.Item(1)
    '                lstItm.Value = objDataReader.Item(0)
    '                ddlCourses.Items.Add(lstItm)
    '            End While
    '            objDataReader.Close()
    '            objconn.disconnect()
    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        lblMsg.Visible = True
    '        lblMsg.Text = ex.Message()
    '        Response.Redirect("error.aspx?err=" & ex.Message, False)
    '    End Try


    'End Sub
    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Insert Data into database
    '********************************************************************
    Public Sub InsertData()
        Dim query As String = ""
        Dim Couresid As Integer
        Dim sqlTrans As SqlTransaction
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try

            If objconn.connect() = True Then
                'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                'Dim fieldsCourse As String = "Course_Name,Course_Code,Main_Course_ID,Total_Question,Total_Time,Correct_Ans"
                Dim fieldsCourse As String = "Course_Name,Course_Code,Main_Course_ID,Total_Time,Total_marks,Total_passmarks,Total_passmarks_Per, Description"
                'query = "Insert Into M_Course (" & fieldsCourse & ") values('" & TxtCourseName.Text & "','" & TxtCourseCode.Text & "','" & ddlMainCourse.SelectedItem.Value & "','" & txtTotalQues.Text & "','" & txtTotalTime.Text & "','" & TxtCorrectAns.Text & "')"
                query = "Insert Into M_Course (" & fieldsCourse & ") values('" & TxtCourseName.Text & "','" & TxtCourseCode.Text & "','" & ddlMainCourse.SelectedItem.Value & "','" & txtTotalTime.Text & "','" & txtTotalMarks.Text & "','" & intpercentage & "','" & intPassPer & "','" & ddlSectionDes.SelectedItem.Value & "')"
                Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
                'objCommand = New SqlCommand(query, objconn.MyConnection)
                'objCommand.ExecuteNonQuery()
                ins_cmd.ExecuteNonQuery()
                'sqlTrans.Commit()
                '   GetCourseID()

                objconn.disconnect()
                'FillCoursesCombo()
                If (ddlMainCourse.SelectedItem.Value <> 0) Then
                    FillMainCourseCombo()
                    ' FillCenterList()
                    'ddlCourses.Items.Clear()
                    'Dim l1 As New ListItem
                    'l1.Text = "---- Select ----"
                    'l1.Value = 0
                    'ddlCourses.Items.Add(l1)
                    ddlMainCourse.SelectedIndex = 0
                    'ddlCourses.SelectedIndex = 0
                    ClearControls()
                End If
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
            Couresid = Nothing
            sqlTrans = Nothing
        End Try

    End Sub


    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To get CourseID and perform insert update
    '********************************************************************
    'Public Sub GetCourseID()
    '    Dim query As String = ""
    '    Dim Couresid As Integer
    '    Dim DelFlag As Integer
    '    Dim cnt As Integer
    '    Dim CenterID As Integer
    '    Dim sqlTrans As SqlTransaction
    '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '    Try
    '        If objconn.connect(strPathDb) = True Then

    '            Dim fields As String = "Course_ID,Del_Flag"
    '            query = "Select " & fields & " from M_Course  where Course_Name='" & TxtCourseName.Text & "' and Del_Flag='0'"
    '            objCommand = New SqlCommand(query, objconn.MyConnection)
    '            objDataReader = objCommand.ExecuteReader()
    '            While objDataReader.Read()
    '                Couresid = objDataReader.Item("Course_ID")
    '                DelFlag = objDataReader.Item("Del_Flag")
    '            End While
    '            objDataReader.Close()
    '            Dim arr(multiBoxId.Items.Count) As Integer
    '            Dim arlst As New ArrayList
    '            Dim strselecteditems As Integer
    '            Dim fieldCenters As String = "Course_ID,Center_ID"
    '            For lst As Integer = 0 To multiBoxId.Items.Count - 1
    '                If (multiBoxId.Items(lst).Selected = True) Then
    '                    strselecteditems = multiBoxId.Items(lst).Value
    '                    arlst.Add(strselecteditems)
    '                End If
    '            Next
    '            arr = arlst.ToArray(System.Type.GetType("System.Int32"))
    '            'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
    '            'If TxtCourseCode.Enabled = True Then
    '            '***************For Submit******start*************
    '            'If centernames.Visible = False And 
    '            If imgBtnReset.Visible = False Then
    '                For ins As Integer = 0 To arr.Length - 1
    '                    query = "Insert Into t_center_course (" & fieldCenters & ") values('" & Couresid & "','" & arr(ins) & "')"
    '                    Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
    '                    ins_cmd.ExecuteNonQuery()
    '                    'objCommand = New SqlCommand(query, objconn.MyConnection)
    '                    'objCommand.ExecuteNonQuery()
    '                    ''sqlTrans.Commit()
    '                Next
    '                '***************For Submit******End*************

    '                '***************For update******start*************
    '                'ElseIf centernames.Visible = True Then
    '            ElseIf imgBtnReset.Visible = True Then
    '                'If (flg = False) Then
    '                'query = "Delete From t_user_course where Course_ID='" & Couresid & "'"
    '                'objCommand = New SqlCommand(query, objconn.MyConnection)
    '                'objCommand.ExecuteNonQuery()
    '                query = ""
    '                query = "Delete From t_center_course where Course_ID='" & Couresid & "'"
    '                Dim del_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
    '                'objCommand = New SqlCommand(query, objconn.MyConnection)
    '                'objCommand.ExecuteNonQuery()
    '                del_cmd.ExecuteNonQuery()
    '                'objCommand = New SqlCommand(query, objconn.MyConnection)
    '                'objCommand.ExecuteNonQuery()
    '                For ins As Integer = 0 To arr.Length - 1
    '                    query = "Insert Into t_center_course (" & fieldCenters & ") values('" & Couresid & "','" & arr(ins) & "')"
    '                    Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
    '                    ins_cmd.ExecuteNonQuery()
    '                    'sqlTrans.Commit()
    '                    'objCommand = New SqlCommand(query, objconn.MyConnection)
    '                    'objCommand.ExecuteNonQuery()
    '                Next
    '                'End If

    '                '*********************For Remove Button******Start************

    '                'If (flg = True) Then 
    '                '    For ins As Integer = 0 To arr.Length - 1                    '        
    '                '        'query = "Update t_center_Course Set Del_Flag='1' where Course_ID='" & Session.Item("CourseIDValue") & "' and center_ID='" & arr(ins) & "'"
    '                '        'objCommand = New SqlCommand(query, objconn.MyConnection)
    '                '        'objCommand.ExecuteNonQuery()
    '                '    Next
    '                'End If

    '                '*********************For Remove Button****End**************

    '                '*********************For Update Course Value******Start************
    '                query = "Select * from t_center_course where course_id ='" & Couresid & "'"
    '                Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
    '                Dim dt As New DataTable()
    '                adp.Fill(dt)
    '                If (dt.Rows.Count = 0) Then
    '                    query = "Update M_Course Set Del_Flag='0' where Course_ID='" & Session.Item("CourseID") & "'"
    '                    Dim update_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
    '                    update_cmd.ExecuteNonQuery()
    '                    'sqlTrans.Commit()
    '                    ''objCommand = New SqlCommand(query, objconn.MyConnection)
    '                    ''objCommand.ExecuteNonQuery()
    '                End If
    '                '*********************For Update Course Value****End**************

    '                '***************For update******end*************
    '            End If
    '            objconn.disconnect()
    '            'sqlTrans.Commit()
    '        End If

    '    Catch ex As Exception
    '        'sqlTrans.Rollback()
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx")
    '    Finally
    '        'sqlTrans.Commit()
    '        query = Nothing
    '        Couresid = Nothing
    '        DelFlag = Nothing
    '        cnt = Nothing
    '        CenterID = Nothing
    '        sqlTrans = Nothing
    '    End Try

    'End Sub


    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Update Data into database
    '********************************************************************
    Public Sub UpdateData()
        Dim query As String = ""
        Dim Couresid As Integer
        Dim sqlTrans1 As SqlTransaction

        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try
            If objconn.connect() = True Then

                dblpercentage = (CInt(txtTotalMarks.Text) * CInt(txtPassingMarks.Text)) / 100
                intpercentage = Math.Round(dblpercentage)
                intPassPer = CInt(txtPassingMarks.Text)

                'sqlTrans1 = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                'query = "Update M_Course Set Course_Name='" & TxtCourseName.Text & "',Total_Question='" & txtTotalQues.Text & "',Total_Time='" & txtTotalTime.Text & "',Correct_Ans='" & TxtCorrectAns.Text & "' where Course_ID='" & ddlCourses.SelectedItem.Value & "'"
                'query = "Update M_Course Set Course_Name='" & TxtCourseName.Text & "',Total_Time='" & txtTotalTime.Text & "' where Course_ID='" & ddlCourses.SelectedItem.Value & "'"
                query = "Update M_Course Set Course_Name='" & TxtCourseName.Text & "',Total_Time='" & txtTotalTime.Text & "',Total_Marks='" & txtTotalMarks.Text & "',Total_passmarks='" & intpercentage & "',Total_passmarks_Per='" & txtPassingMarks.Text & "',Main_Course_ID='" & ddlMainCourse.SelectedItem.Value & "' where Course_ID='" & Session.Item("CourseID") & "'"
                'objCommand = New SqlCommand(query, objconn.MyConnection)
                'objCommand.ExecuteNonQuery()
                Dim update_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans1)
                update_cmd.ExecuteNonQuery()
                ' sqlTrans1.Commit()
                ' GetCourseID()

                objconn.disconnect()
                'FillCoursesCombo()
                If (ddlMainCourse.SelectedItem.Value <> 0) Then
                    FillMainCourseCombo()
                    '  FillCenterList()
                    'ddlCourses.Items.Clear()
                    'Dim l1 As New ListItem
                    'l1.Text = "---- Select ----"
                    'l1.Value = 0
                    'ddlCourses.Items.Add(l1)
                    ddlMainCourse.SelectedIndex = 0
                    'ddlCourses.SelectedIndex = 0
                    ClearControls()
                End If
            End If
        Catch ex As Exception
            ' sqlTrans1.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            lblMsg.Visible = True
            lblMsg.Text = ex.Message()
            Response.Redirect("error.aspx?err=" & ex.Message, False)
        Finally
            'sqlTrans1.Commit()
            query = Nothing
            CourseId = Nothing
            sqlTrans1 = Nothing
        End Try


    End Sub


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


    ''********************************************************************
    ''Code added by Monal Shah
    ''Purpose: To Delete Records From database
    ''********************************************************************
    'Public Sub DeleteRecords()
    '    Dim query As String = ""
    '    Dim CenterID As Integer
    '    Dim DelFlag As Integer
    '    Dim cnt As Integer

    '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            For lst As Integer = 0 To multiBoxId.Items.Count - 1
    '                If (multiBoxId.Items(lst).Selected = True) Then
    '                    cnt = cnt + 1
    '                End If
    '            Next
    '            Dim fields As String = "Count(t_Center_course.Center_ID) as CenterID,Count(t_Center_course.Del_Flag) As DelFlag"
    '            query = "Select " & fields & " from t_Center_course,M_Course  where M_Course.Course_name='" & TxtCourseName.Text & "' and M_course.Course_ID=t_Center_Course.Course_ID and t_Center_Course.Del_Flag='0'"
    '            objCommand = New SqlCommand(query, objconn.MyConnection)
    '            objDataReader = objCommand.ExecuteReader()
    '            While objDataReader.Read()
    '                CenterID = objDataReader.Item("CenterID")
    '                DelFlag = objDataReader.Item("DelFlag")
    '            End While
    '            objDataReader.Close()
    '            If (cnt = CenterID) Then
    '                query = "Update M_Course Set Del_Flag='1' where Course_ID='" & ddlCourses.SelectedItem.Value & "'"
    '                objCommand = New SqlCommand(query, objconn.MyConnection)
    '                objCommand.ExecuteNonQuery()
    '            End If
    '            GetCourseID()
    '            objDataReader.Close()
    '            If objconn.connect(strPathDb) = True Then
    '                Dim CourseID As Integer
    '                Dim DelFlag1 As Integer
    '                query = ""
    '                query = "Select Course_ID,Del_Flag from M_Course where Course_name='" & TxtCourseName.Text & "'"
    '                objCommand = New SqlCommand(query, objconn.MyConnection)
    '                objDataReader = objCommand.ExecuteReader()
    '                While objDataReader.Read()
    '                    CourseID = objDataReader.Item(0)
    '                    DelFlag1 = Convert.ToInt32(objDataReader.Item(1))
    '                End While
    '                objDataReader.Close()
    '                If (DelFlag1 = "1") Then
    '                    query = "Delete From t_center_course where Course_ID='" & CourseID & "'"
    '                    objCommand = New SqlCommand(query, objconn.MyConnection)
    '                    objCommand.ExecuteNonQuery()
    '                End If

    '            End If

    '            objconn.disconnect()
    '            FillCoursesCombo()
    '            If (ddlMainCourse.SelectedItem.Value <> 0) Or (ddlCourses.SelectedItem.Value <> 0) Then
    '                FillMainCourseCombo()
    '                FillCenterList()
    '                ddlCourses.Items.Clear()
    '                Dim l1 As New ListItem
    '                l1.Text = "---- Select ----"
    '                l1.Value = 0
    '                ddlCourses.Items.Add(l1)
    '                ddlMainCourse.SelectedIndex = 0
    '                ddlCourses.SelectedIndex = 0
    '                ClearControls()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        lblMsg.Visible = True
    '        lblMsg.Text = ex.Message()
    '        Response.Redirect("error.aspx?err=" & ex.Message, False)
    '    End Try

    '    'End If
    'End Sub
    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Clear Controls
    '********************************************************************
    Public Sub ClearControls()
        Try
            TxtCourseName.Text = String.Empty
            TxtCourseCode.Text = String.Empty
            'txtTotalQues.Text = String.Empty
            'TxtCorrectAns.Text = String.Empty
            txtTotalTime.Text = String.Empty
            ddlMainCourse.Enabled = True
            TxtCourseCode.Enabled = True
            imgBtnSubmit.Visible = True
            imgBtnUpdate.Visible = False
            lblMsg.Visible = False
            txtTotalMarks.Text = String.Empty
            txtPassingMarks.Text = String.Empty
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub


    Protected Sub imgBtnSubmit_Click(sender As Object, e As EventArgs) Handles imgBtnSubmit.Click
        Try
            'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
            'updatespan.Visible = True
            ' centernames.Visible = False
            ' insertspan.Visible = True
            Dim totalTimeValue As Boolean = ValidationForNumber(txtTotalTime.Text)
            'If (TxtCourseName.Text = String.Empty And TxtCourseCode.Text = String.Empty And ddlMainCourse.SelectedItem.Value = 0 And multiBoxId.SelectedIndex = -1 And txtTotalTime.Text = String.Empty) Then

            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Select Main Course.<br>Please Select At Least One Center.<br>Please Enter Course Code.<br>Please Enter Course Name.<br>Please Enter Total Time In Minute.<br>"

            '    'lblMsg.Visible = True
            '    'lblMsg.Text = "Please Select Main Course.<br>Please Select At Least One Center.<br>Please Enter Course Code.<br>Please Enter Course Name.<br>Please Enter Total Time In Minute.<br>"
            'Else
            If ddlMainCourse.SelectedItem.Value = 0 Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseMaintanance_sltmaincrs
                ddlMainCourse.Focus()
                Exit Sub
                'ElseIf multiBoxId.SelectedIndex = -1 Then
                '    lblMsg.Visible = True
                '    lblMsg.Text = "Please Select At Least One Centre Name."
                '    multiBoxId.Focus()
                'Exit Sub
            ElseIf TxtCourseCode.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseRegistration_crscd
                TxtCourseCode.Focus()
                'Exit Sub
            ElseIf TxtCourseName.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseRegistration_crsnmer
                TxtCourseName.Focus()
            ElseIf txtTotalTime.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseRegistration_taet
                txtTotalTime.Focus()
                'Exit Sub
            ElseIf (totalTimeValue = False) Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseRegistration_notaeerr
                txtTotalTime.Focus()
                'Exit Sub
            ElseIf txtTotalMarks.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseRegistration_totalmrks
                txtTotalMarks.Focus()
                'Exit Sub
            ElseIf txtPassingMarks.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseRegistration_pasmrks
                txtPassingMarks.Focus()
                'Exit Sub
                'ElseIf (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And txtTotalTime.Text <> String.Empty And txtTotalMarks.Text <> String.Empty And txtPassingMarks.Text <> String.Empty) Then
            ElseIf (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty And txtTotalMarks.Text <> String.Empty And txtPassingMarks.Text <> String.Empty) Then


                dblpercentage = (CInt(txtTotalMarks.Text) * CInt(txtPassingMarks.Text)) / 100
                intpercentage = Math.Round(dblpercentage)
                intPassPer = CInt(txtPassingMarks.Text)

                Dim objcommon As New CommonFunction()
                Dim booldecision As Boolean = objcommon.ValidateCourseName(TxtCourseName.Text)


                If booldecision = True Then
                    'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                    Try
                        If objconn.connect() = True Then
                            sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            InsertData()
                            sqlTrans.Commit()
                        End If
                    Catch ex As Exception
                        sqlTrans.Rollback()
                    Finally
                        objconn.disconnect()
                    End Try

                    If TxtCourseCode.Enabled = True Then

                        imgBtnSubmit.Visible = True
                        imgBtnUpdate.Visible = False
                        'lblMsg.Style.Add("color", "Green")
                        lblMsg.ForeColor = Drawing.Color.Green
                        lblMsg.Visible = True
                        lblMsg.Text = Resources.Resource.CourseRegistration_crsregsucl
                        'Response.Redirect("CourseMaintenance.aspx", False)
                        'ElseIf TxtCourseCode.Enabled = False Then
                        '    'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
                        '    If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty) Then
                        '        UpdateData()
                        '    End If
                    End If
                ElseIf booldecision = False Then
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_creerr
                End If
                'ElseIf (TxtCourseName.Text = String.Empty And TxtCourseCode.Text = String.Empty And ddlMainCourse.SelectedItem.Value = 0 And multiBoxId.SelectedIndex = -1 And txtTotalTime.Text = String.Empty) Then
                '    lblMsg.Visible = True
                '    lblMsg.Text = "All Fields Are Mandatory."
            End If

            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub ddlMainCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMainCourse.SelectedIndexChanged
        Try
            FillSectionCourseCombo()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    'Protected Sub ddlCourses_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCourses.SelectedIndexChanged
    '    Dim query As String = ""
    '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            '*************************************************************************************************************
    '            'query = "select Course_Code,Total_Question,Correct_Ans,Total_Time,Main_Course_ID from M_Course where Course_ID='" & ddlCourses.SelectedItem.Value & "'"
    '            '*************************************************************************************************************

    '            'query = "select Distinct M_Course.Course_Code,M_Course.Total_Question,M_Course.Correct_Ans,M_Course.Total_Time,M_Course.Main_Course_ID,t_center_Course.Center_id from M_Course,t_center_Course where M_Course.Course_ID IN ('" & ddlCourses.SelectedItem.Value & "') and M_Course.Del_Flag='0' and t_center_Course.Del_Flag='0'"
    '            query = "select Distinct M_Course.Course_Code,M_Course.Total_Time,M_Course.Main_Course_ID,t_center_Course.Center_id from M_Course,t_center_Course where t_center_course.Course_ID= '" & ddlCourses.SelectedItem.Value & "' and M_Course.Course_ID= '" & ddlCourses.SelectedItem.Value & "' and M_Course.Del_Flag='0' and t_center_Course.Del_Flag='0'"
    '            objCommand = New SqlCommand(query, objconn.MyConnection)
    '            objDataReader = objCommand.ExecuteReader()
    '            While objDataReader.Read()
    '                TxtCourseName.Text = ddlCourses.SelectedItem.Text
    '                TxtCourseCode.Enabled = False
    '                TxtCourseCode.Text = objDataReader.Item(0)
    '                'txtTotalQues.Text = objDataReader.Item(1)
    '                'TxtCorrectAns.Text = objDataReader.Item(2)
    '                txtTotalTime.Text = objDataReader.Item(1)
    '                ddlMainCourse.Text = objDataReader.Item(2)
    '                For lst As Integer = 0 To multiBoxId.Items.Count - 1
    '                    If (multiBoxId.Items(lst).Value = objDataReader.Item(3).ToString()) Then
    '                        multiBoxId.Items(lst).Selected = True
    '                    End If
    '                Next
    '            End While
    '            objDataReader.Close()
    '            objconn.disconnect()
    '            If (ddlCourses.SelectedItem.Value = 0) Then
    '                FillMainCourseCombo()
    '                FillCenterList()
    '                ddlCourses.Items.Clear()
    '                Dim l1 As New ListItem
    '                l1.Text = "---- Select ----"
    '                l1.Value = 0
    '                ddlCourses.Items.Add(l1)
    '                ddlCourses.SelectedIndex = 0
    '                ClearControls()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        lblMsg.Visible = True
    '        lblMsg.Text = ex.Message()
    '        Response.Redirect("error.aspx?err=" & ex.Message, False)
    '    End Try

    'End Sub

    'Protected Sub imgBtnRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnRemove.Click
    '    'flg = True
    '    ''Response.Write("<script language=javascript>javascript:if(!confirm('Are you sure you want to delete this entry?')) return false;</script>")

    '    'DeleteRecords()
    'End Sub
    Protected Sub ImgBtnBack_Click(sender As Object, e As EventArgs) Handles ImgBtnBack.Click
        Session.Remove("CourseID")
        Try


            If Session("fornew") = "true" Then
                Response.Redirect("CourseMaintenance.aspx", False)
                Session.Remove("fornew")
            Else
                Session.Add("fromcourseregi", "true")
                Dim intpi As Integer = CInt(Request.QueryString("pi").ToString())
                Response.Redirect("CourseMaintenance.aspx?pi=" & intpi, False)
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error:" & ex.ToString)

            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    ' Protected Sub multiBoxId_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles multiBoxId.SelectedIndexChanged
    '    Dim query As String = ""
    '    FillCoursesCombo()
    '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '    Try
    '        If objconn.connect(strPathDb) = True Then
    '            'query = "select Course_Code,Total_Question,Correct_Ans,Total_Time,Main_Course_ID from M_Course where Course_ID='" & ddlCourses.SelectedItem.Value & "'"

    '            objDataReader.Close()
    '            query = "select Distinct M_Course.Course_Name,m_Course.Main_Course_id from M_Course,t_center_Course where t_center_Course.Center_id IN ('" & multiBoxId.SelectedItem.Value & "') and t_center_Course.Course_id=m_course.Course_id and M_Course.Del_Flag='0' and t_center_Course.Del_Flag='0'"
    '            objCommand = New SqlCommand(query, objconn.MyConnection)
    '            objDataReader = objCommand.ExecuteReader()
    '            While objDataReader.Read()
    '                ddlCourses.Text = objDataReader.Item(0)
    '                ddlMainCourse.Text = objDataReader.Item(1)
    '            End While
    '            objDataReader.Close()
    '            objconn.disconnect()
    '            If (ddlCourses.SelectedItem.Value = 0) Then
    '                FillMainCourseCombo()
    '                FillCenterList()
    '                ddlCourses.Items.Clear()
    '                Dim l1 As New ListItem
    '                l1.Text = "---- Select ----"
    '                l1.Value = 0
    '                ddlCourses.Items.Add(l1)
    '                ddlCourses.SelectedIndex = 0
    '                ClearControls()
    '            End If
    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        lblMsg.Visible = True
    '        lblMsg.Text = ex.Message()
    '    End Try
    ' End Sub

    Protected Sub imgBtnUpdate_Click(sender As Object, e As EventArgs) Handles imgBtnUpdate.Click
        Dim booldecision As Boolean = True

        Try
            If TxtCourseCode.Enabled = True Then
                imgBtnReset.Visible = True
                imgBtnClear.Visible = False
                ' updatespan.Visible = False
                'centernames.Visible = True
                '   insertspan.Visible = False
                'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
                'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty) Then
                Dim totalTimeValue As Boolean = ValidationForNumber(txtTotalTime.Text)
                If ddlMainCourse.SelectedItem.Value = 0 Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseMaintanance_sltmaincrs
                    ddlMainCourse.Focus()
                    'ElseIf multiBoxId.SelectedIndex = -1 Then
                    '    lblMsg.Visible = True
                    '    lblMsg.Text = "Please Select At Least One Center."
                ElseIf TxtCourseCode.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_crscd
                    TxtCourseCode.Focus()
                ElseIf TxtCourseName.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_crsnmer
                    TxtCourseName.Focus()
                ElseIf txtTotalTime.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_taet
                    txtTotalTime.Focus()
                ElseIf (totalTimeValue = False) Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_notaeerr
                    txtTotalTime.Focus()
                ElseIf txtTotalMarks.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_totalmrks
                    txtTotalMarks.Focus()
                ElseIf txtPassingMarks.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CourseRegistration_pasmrks
                    txtPassingMarks.Focus()
                ElseIf (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty And txtTotalMarks.Text <> String.Empty And txtPassingMarks.Text <> String.Empty) Then


                    '/********************Added By:Jatin Gangajaliya,2011/3/11****************/
                    If Not (Session.Item("cname").ToString = TxtCourseName.Text) Then
                        Dim objcommon As New CommonFunction()
                        booldecision = objcommon.ValidateCourseName(TxtCourseName.Text)
                    End If

                    If booldecision = True Then
                        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                        Try
                            If objconn.connect() = True Then
                                sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                                UpdateData()
                                sqlTrans.Commit()
                            End If
                        Catch ex As Exception
                            sqlTrans.Rollback()
                        Finally
                            Session.Remove("CourseID")
                            objconn.disconnect()
                        End Try
                        lblMsg.ForeColor = Drawing.Color.Green
                        lblMsg.Visible = True
                        lblMsg.Text = Resources.Resource.CourseMaintanance_crsupdted
                        Session.Add("updatecheck", "true")
                        Response.Redirect("CourseMaintenance.aspx", False)
                    ElseIf booldecision = False Then
                        lblMsg.ForeColor = Drawing.Color.Red
                        lblMsg.Visible = True
                        lblMsg.Text = Resources.Resource.CourseRegistration_creerr
                    End If
                End If
                '/*******************************end*******************************/               
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally
            booldecision = Nothing
        End Try
    End Sub

    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        Try
            ClearControls()
            'ddlCourses.Items.Clear()
            'Dim l1 As New ListItem
            'l1.Text = "---- Select ----"
            'l1.Value = 0
            'ddlCourses.Items.Add(l1)
            'ddlCourses.SelectedIndex = 0
            ddlMainCourse.SelectedIndex = 0
            'For lst As Integer = 0 To multiBoxId.Items.Count - 1
            '    If (multiBoxId.Items(lst).Selected = True) Then
            '        multiBoxId.Items(lst).Selected = False
            '    End If
            'Next
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub imgBtnReset_Click(sender As Object, e As EventArgs) Handles imgBtnReset.Click
        Try
            CourseId = Session.Item("CourseID")
            lblhead.Text = Resources.Resource.CourseRegistration_crsmd
            legendLabel.Text = Resources.Resource.CourseRegistration_crsmddts
            EditMode()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

End Class