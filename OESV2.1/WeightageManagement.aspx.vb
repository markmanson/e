Imports System.Data
Imports System.Data.SqlClient
Partial Public Class WeightageManagement
    Inherits BasePage
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("WeightageManagement")
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader
    Dim CourseId As Integer
    Dim sqlTrans As SqlTransaction
    Dim flg As Boolean
    Dim strtemp As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                'Added by Pranit on 06/12/2019
                ddlSubject.Enabled = False
                txtWeight.Enabled = False
                txtBasicq.Enabled = False
                txtIntermediateq.Enabled = False
                txtTF.Enabled = False
                txtBlank.Enabled = False
                txtMChoice.Enabled = False
                FillCourseCombo()
                FillSubCombo()
                If Not Session("wid") Is Nothing Then
                    txtWeight.Enabled = True
                    txtBasicq.Enabled = True
                    txtIntermediateq.Enabled = True
                    txtTF.Enabled = True
                    txtBlank.Enabled = True
                    txtMChoice.Enabled = True
                    EditMode(Session("wid").ToString)
                    imgBtnSubmit.Visible = False
                    imgBtnClear.Visible = False
                    imgBtnUpdate.Visible = True
                    imgBtnReset.Visible = True
                End If
            End If
            ' EditMode("9")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

#Region "Page_Unload"
    'Desc: This is page unload event.
    'By: Jatin Gangajaliya, 2011/04/27.
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
    Public Sub EditMode(ByVal WID As String)

        Dim vds As DataSet
        Dim vda As SqlDataAdapter
        Dim Vacancy As Integer = 100
        Dim dbVacancy As Integer = 0
        Dim query As String = "select  Weightage_ID as wid , mw.Course_id as Course_id,mc.Course_name,s.test_name as Subject_name,s.test_type as Subject_id,Sub_Weightage as sub_Weight,single as single,Multi_choice as multi_choice,blanks as blanks,basic as basic,interMed as intermed, mw.del_flag as del_flag from M_weightage as mw join M_Course as mc on mw.course_id=mc.course_id join M_testinfo as s on mw.test_type=s.test_type  where Weightage_ID = " & WID

        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try
            vds = New DataSet
            If objconn.connect() = True Then
                vda = New SqlDataAdapter(query, objconn.MyConnection)
                vda.Fill(vds)

                For i As Integer = 0 To ddlCourse.Items.Count - 1
                    If ddlCourse.Items(i).Text = vds.Tables(0).Rows(0).Item(2).ToString() Then
                        ddlCourse.Items(i).Selected = True
                        Exit For
                    End If
                Next
                ddlCourse.Enabled = False
                For j As Integer = 0 To ddlSubject.Items.Count - 1
                    If ddlSubject.Items(j).Text = vds.Tables(0).Rows(0).Item(3).ToString() Then
                        ddlSubject.Items(j).Selected = True
                        Exit For
                    End If
                Next
                ddlSubject.Enabled = False

                'ddlCourse.SelectedItem.Text = vds.Tables(0).Rows(0).Item(2).ToString()
                'ddlSubject.SelectedItem.Text = vds.Tables(0).Rows(0).Item(3).ToString()
                txtWeight.Text = vds.Tables(0).Rows(0).Item(5).ToString()
                txtTF.Text = vds.Tables(0).Rows(0).Item(6).ToString()
                txtMChoice.Text = vds.Tables(0).Rows(0).Item(7).ToString()
                txtBlank.Text = vds.Tables(0).Rows(0).Item(8).ToString()
                txtBasicq.Text = vds.Tables(0).Rows(0).Item(9).ToString()
                txtIntermediateq.Text = vds.Tables(0).Rows(0).Item(10).ToString()
            End If

            Dim vcnt As Integer = Convert.ToInt32(GetVacantSpace(vds.Tables(0).Rows(0).Item(1).ToString()))
            If (vcnt < 0) Then
                lblVacant.ForeColor = Drawing.Color.Red
            Else
                lblVacant.ForeColor = Drawing.Color.Green
            End If
            lblVacant.Text = vcnt.ToString
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn.disconnect()
            vda = Nothing
            vds = Nothing
            Vacancy = Nothing
            dbVacancy = Nothing
            query = Nothing
        End Try
    End Sub



    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the combo box for Course
    '********************************************************************
    Public Sub FillCourseCombo()

        ddlCourse.Items.Clear()
        Dim l1 As New ListItem
        l1.Text = "---- Select ----"
        l1.Value = 0

        ddlCourse.Items.Add(l1)
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try
            If objconn.connect() Then
                Dim Main_Course As String = "Select Course_ID, Course_Name From M_Course where del_flag=0 order by Course_Name"
                objCommand = New SqlCommand(Main_Course, objconn.MyConnection)
                objDataReader = objCommand.ExecuteReader()
                While objDataReader.Read
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader.Item(1)
                    lstItm.Value = objDataReader.Item(0)
                    ddlCourse.Items.Add(lstItm)
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
            objconn.disconnect()
            l1 = Nothing
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the ListBox box for Center
    '********************************************************************
    Public Sub FillSubCombo()

        'multiBoxId.Items.Clear()
        ddlSubject.Items.Clear()

        Dim l1 As New ListItem
        l1.Text = "---- Select ----"
        l1.Value = 0

        ddlSubject.Items.Add(l1)
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try
            If objconn.connect() Then
                Dim subjects As String = "Select test_type,test_name,Sub_Code From M_Testinfo where del_flag=0 order by test_name"
                objCommand = New SqlCommand(subjects, objconn.MyConnection)
                objDataReader = objCommand.ExecuteReader()
                While objDataReader.Read
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader.Item(1)
                    lstItm.Value = objDataReader.Item(0)
                    ddlSubject.Items.Add(lstItm)
                    'multiBoxId.Items.Add(lstItm)
                End While
                objDataReader.Close()
                objconn.disconnect()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
            objconn.disconnect()
        Finally
            l1 = Nothing
        End Try
    End Sub

    '********************************************************************
    'Code added by Indravadan Vasava
    'Purpose: To Insert Data into database
    '********************************************************************
    Public Sub InsertData()
        Dim query As String = ""
        Dim Couresid As Integer

        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try

            If objconn.connect() = True Then

                Dim fields As String = "Course_ID,test_type,Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed"
                query = "Insert Into M_Weightage (" & fields & ") values('" & ddlCourse.SelectedValue.ToString & "','" & ddlSubject.SelectedValue.ToString & "','" & txtWeight.Text & "','" & txtTF.Text & "','" & txtMChoice.Text & "','" & txtBlank.Text & "','" & txtBasicq.Text & "','" & txtIntermediateq.Text & "')"
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objCommand.ExecuteNonQuery()
                ClearControls()
                lblMsg.ForeColor = Drawing.Color.Green
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.WeightManage_SuccessReg
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSuccRegis.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSuccRegis.ToString
                'End If


            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            objCommand = Nothing
            objconn.disconnect()
            query = Nothing
            CourseId = Nothing
            strPathDb = Nothing
        End Try

    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To get CourseID and perform insert update
    '********************************************************************
    Public Sub GetCourseID()
        'Dim query As String = ""
        'Dim Couresid As Integer
        'Dim DelFlag As Integer
        'Dim cnt As Integer
        'Dim CenterID As Integer
        'Dim sqlTrans As SqlTransaction
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        'Try
        '    If objconn.connect(strPathDb) = True Then

        '        Dim fields As String = "Course_ID,Del_Flag"
        '        query = "Select " & fields & " from M_Course  where Course_Name='" & TxtCourseName.Text & "' and Del_Flag='0'"
        '        objCommand = New SqlCommand(query, objconn.MyConnection)
        '        objDataReader = objCommand.ExecuteReader()
        '        While objDataReader.Read()
        '            Couresid = objDataReader.Item("Course_ID")
        '            DelFlag = objDataReader.Item("Del_Flag")
        '        End While
        '        objDataReader.Close()
        '        Dim arr(multiBoxId.Items.Count) As Integer
        '        Dim arlst As New ArrayList
        '        Dim strselecteditems As Integer
        '        Dim fieldCenters As String = "Course_ID,Center_ID"
        '        For lst As Integer = 0 To multiBoxId.Items.Count - 1
        '            If (multiBoxId.Items(lst).Selected = True) Then
        '                strselecteditems = multiBoxId.Items(lst).Value
        '                arlst.Add(strselecteditems)
        '            End If
        '        Next
        '        arr = arlst.ToArray(System.Type.GetType("System.Int32"))
        '        'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
        '        'If TxtCourseCode.Enabled = True Then
        '        '***************For Submit******start*************
        '        'If centernames.Visible = False And 
        '        If imgBtnReset.Visible = False Then
        '            For ins As Integer = 0 To arr.Length - 1
        '                query = "Insert Into t_center_course (" & fieldCenters & ") values('" & Couresid & "','" & arr(ins) & "')"
        '                Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
        '                ins_cmd.ExecuteNonQuery()
        '                'objCommand = New SqlCommand(query, objconn.MyConnection)
        '                'objCommand.ExecuteNonQuery()
        '                ''sqlTrans.Commit()
        '            Next
        '            '***************For Submit******End*************

        '            '***************For update******start*************
        '            'ElseIf centernames.Visible = True Then
        '        ElseIf imgBtnReset.Visible = True Then
        '            'If (flg = False) Then
        '            'query = "Delete From t_user_course where Course_ID='" & Couresid & "'"
        '            'objCommand = New SqlCommand(query, objconn.MyConnection)
        '            'objCommand.ExecuteNonQuery()
        '            query = ""
        '            query = "Delete From t_center_course where Course_ID='" & Couresid & "'"
        '            Dim del_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
        '            'objCommand = New SqlCommand(query, objconn.MyConnection)
        '            'objCommand.ExecuteNonQuery()
        '            del_cmd.ExecuteNonQuery()
        '            'objCommand = New SqlCommand(query, objconn.MyConnection)
        '            'objCommand.ExecuteNonQuery()
        '            For ins As Integer = 0 To arr.Length - 1
        '                query = "Insert Into t_center_course (" & fieldCenters & ") values('" & Couresid & "','" & arr(ins) & "')"
        '                Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
        '                ins_cmd.ExecuteNonQuery()
        '                'sqlTrans.Commit()
        '                'objCommand = New SqlCommand(query, objconn.MyConnection)
        '                'objCommand.ExecuteNonQuery()
        '            Next
        '            'End If

        '            '*********************For Remove Button******Start************

        '            'If (flg = True) Then 
        '            '    For ins As Integer = 0 To arr.Length - 1                    '        
        '            '        'query = "Update t_center_Course Set Del_Flag='1' where Course_ID='" & Session.Item("CourseIDValue") & "' and center_ID='" & arr(ins) & "'"
        '            '        'objCommand = New SqlCommand(query, objconn.MyConnection)
        '            '        'objCommand.ExecuteNonQuery()
        '            '    Next
        '            'End If

        '            '*********************For Remove Button****End**************

        '            '*********************For Update Course Value******Start************
        '            query = "Select * from t_center_course where course_id ='" & Couresid & "'"
        '            Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
        '            Dim dt As New DataTable()
        '            adp.Fill(dt)
        '            If (dt.Rows.Count = 0) Then
        '                query = "Update M_Course Set Del_Flag='0' where Course_ID='" & Session.Item("CourseID") & "'"
        '                Dim update_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
        '                update_cmd.ExecuteNonQuery()
        '                'sqlTrans.Commit()
        '                ''objCommand = New SqlCommand(query, objconn.MyConnection)
        '                ''objCommand.ExecuteNonQuery()
        '            End If
        '            '*********************For Update Course Value****End**************

        '            '***************For update******end*************
        '        End If
        '        objconn.disconnect()
        '        'sqlTrans.Commit()
        '    End If

        'Catch ex As Exception
        '    'sqlTrans.Rollback()
        '    If log.IsDebugEnabled Then
        '        log.Debug("Error :" & ex.ToString())
        '    End If
        '    lblMsg.Visible = True
        '    lblMsg.Text = ex.Message()
        '    Response.Redirect("error.aspx?err=" & ex.Message, False)
        'Finally
        '    'sqlTrans.Commit()
        'End Try

    End Sub


    '********************************************************************
    'Code added by Indaravdan Vasava
    'Purpose: To Update Data into database
    '********************************************************************
    Public Sub UpdateData(ByVal weightid As String)
        Dim query As String = ""
        Dim back As Boolean = False
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try
            If objconn.connect() = True Then

                query = "Update M_Weightage Set Course_ID='" & ddlCourse.SelectedValue.ToString & "',test_type='" & ddlSubject.SelectedValue.ToString & "',Sub_Weightage='" & txtWeight.Text & "',Single='" & txtTF.Text & "',Multi_Choice='" & txtMChoice.Text & "',Blanks='" & txtBlank.Text & "',Basic='" & txtBasicq.Text & "',InterMed='" & txtIntermediateq.Text & "' where Weightage_ID='" & weightid & "'"
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objCommand.ExecuteNonQuery()
                ClearControls()
                lblMsg.ForeColor = Drawing.Color.Green
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.WeightManage_SuccessUpd
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSuccUpdte.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSuccUpdte.ToString
                'End If

                Session.Remove("wid")
                back = True
                '  Response.Redirect("WeightSearch.aspx")
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            objconn.disconnect()
        End Try
        If (back = True) Then
            Session.Add("FromWm", "true")
            If (Request.QueryString("pi") <> Nothing) Then
                Response.Redirect("WeightSearch.aspx?pi=" & Request.QueryString("pi").ToString, False)
            End If
        End If
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
            txtBasicq.Text = String.Empty
            txtBlank.Text = String.Empty
            txtIntermediateq.Text = String.Empty
            txtMChoice.Text = String.Empty
            txtTF.Text = String.Empty
            FillCourseCombo()
            FillSubCombo()
            txtWeight.Text = String.Empty
            lblVacant.Text = ""
            lblMsg.Visible = False
            'added by bhumi [22/9/2015]
            'Reason: Clear Button is not working for below given Labels
            lblMarks.Text = "0"
            lblTotalQuestion.Text = "0"
            lblTotalMarks.Text = "0"
            lblBasicQuestion.Text = "0"
            lblInterQuestion.Text = "0"
            ddlSubject.Enabled = False
            txtWeight.Enabled = False
            txtBasicq.Enabled = False
            txtIntermediateq.Enabled = False
            txtTF.Enabled = False
            txtBlank.Enabled = False
            txtMChoice.Enabled = False
            'Ended by bhumi
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub


    'Added by Pranit Chimurkar on 2019/10/24
    Protected Sub imgBtnSubmit_Click(sender As Object, e As EventArgs) Handles imgBtnSubmit.Click
        Dim strValue As String
        Try

            If Validate() = True Then
                If CheckQuestionTypeTotal(txtTF.Text, txtMChoice.Text, txtBlank.Text) = True Then
                    If (CheckBasicTotal(txtBasicq.Text, txtIntermediateq.Text)) = True Then
                        If IsVacant() = True Then

                            If CheckSubcjectExixsts(ddlSubject.SelectedValue.ToString, ddlCourse.SelectedValue.ToString) = True Then
                                If CInt(txtWeight.Text) <= CInt(lblVacant.Text) Then
                                    strValue = CalculatePercentage()
                                    If strValue = "Nothing" Then
                                        InsertData()
                                    Else
                                        lblMsg.Text = Resources.Resource.WeightManage_AssSubWeigh + strValue + "%"
                                        'Commented by Pranit Chimurkar on 2019/10/23
                                        'If Request.Cookies("Lang").Value = "en-us" Then
                                        '    lblMsg.Text = CommonMessageLingual.lblMsgAssignWeig.ToString + strValue + "%"

                                        'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                                        '    lblMsg.Text = CommonMessageLingual.lblMsgAssignWeig.ToString + strValue + "%"
                                        'End If

                                        lblMsg.Visible = True
                                        txtWeight.Focus()
                                    End If
                                Else

                                    lblMsg.Text = Resources.Resource.WeightManage_WeighExceed
                                    'Commented by Pranit Chimurkar on 2019/10/23
                                    'If Request.Cookies("Lang").Value = "en-us" Then
                                    '    lblMsg.Text = CommonMessageLingual.lblMsgSubWeighExceed.ToString
                                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                                    '    lblMsg.Text = CommonMessageLingual.lblMsgSubWeighExceed.ToString
                                    'End If

                                    lblMsg.Visible = True
                                    lblMsg.ForeColor = Drawing.Color.Red
                                    txtWeight.Focus()
                                End If

                            Else
                                lblMsg.Text = Resources.Resource.WeightManage_SubjAlready
                                'Commented by Pranit Chimurkar on 2019/10/23
                                'If Request.Cookies("Lang").Value = "en-us" Then
                                '    lblMsg.Text = CommonMessageLingual.lblMsgAlredyCrs.ToString
                                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                                '    lblMsg.Text = CommonMessageLingual.lblMsgAlredyCrs.ToString
                                'End If

                                lblMsg.Visible = True
                                lblMsg.ForeColor = Drawing.Color.Red
                            End If

                        End If

                    End If
                Else
                    lblMsg.Visible = True

                End If
            Else
                lblMsg.Visible = True
            End If


            'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
            'updatespan.Visible = True
            '' centernames.Visible = False
            '' insertspan.Visible = True
            'Dim totalTimeValue As Boolean = ValidationForNumber(txtTotalTime.Text)
            ''If (TxtCourseName.Text = String.Empty And TxtCourseCode.Text = String.Empty And ddlMainCourse.SelectedItem.Value = 0 And multiBoxId.SelectedIndex = -1 And txtTotalTime.Text = String.Empty) Then

            ''    lblMsg.Visible = True
            ''    lblMsg.Text = "Please Select Main Course.<br>Please Select At Least One Center.<br>Please Enter Course Code.<br>Please Enter Course Name.<br>Please Enter Total Time In Minute.<br>"

            ''    'lblMsg.Visible = True
            ''    'lblMsg.Text = "Please Select Main Course.<br>Please Select At Least One Center.<br>Please Enter Course Code.<br>Please Enter Course Name.<br>Please Enter Total Time In Minute.<br>"
            ''Else
            'If ddlMainCourse.SelectedItem.Value = 0 Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Select Main Course."
            '    ddlMainCourse.Focus()
            'ElseIf multiBoxId.SelectedIndex = -1 Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Select At Least One Centre Name."
            '    multiBoxId.Focus()
            'ElseIf TxtCourseCode.Text = String.Empty Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Enter Course Code."
            '    TxtCourseCode.Focus()
            'ElseIf TxtCourseName.Text = String.Empty Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Enter Course Name."
            '    TxtCourseName.Focus()
            'ElseIf txtTotalTime.Text = String.Empty Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Enter Total Alloted Exam Time in Minutes."
            '    txtTotalTime.Focus()
            'ElseIf (totalTimeValue = False) Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Enter Numbers In Total Alloted Exam Time(Minute)."
            '    txtTotalTime.Focus()
            'ElseIf txtTotalMarks.Text = String.Empty Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Enter Total Marks."
            '    txtTotalMarks.Focus()
            'ElseIf txtPassingMarks.Text = String.Empty Then
            '    lblMsg.Visible = True
            '    lblMsg.Text = "Please Enter Total Passing Marks."
            '    txtPassingMarks.Focus()
            'ElseIf (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And txtTotalTime.Text <> String.Empty And txtTotalMarks.Text <> String.Empty And txtPassingMarks.Text <> String.Empty) Then

            '    Dim objcommon As New CommonFunction()
            '    Dim booldecision As Boolean = objcommon.ValidateCourseName(TxtCourseName.Text)

            '    If booldecision = True Then
            '        strPathDb = ConfigurationSettings.AppSettings("PathDb")
            '        Try
            '            If objconn.connect(strPathDb) = True Then
            '                sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
            '                InsertData()
            '                sqlTrans.Commit()
            '            End If
            '        Catch ex As Exception
            '            sqlTrans.Rollback()
            '        Finally
            '            objconn.disconnect()
            '        End Try

            '        If TxtCourseCode.Enabled = True Then

            '            imgBtnSubmit.Visible = True
            '            imgBtnUpdate.Visible = False
            '            'lblMsg.Style.Add("color", "Green")
            '            lblMsg.ForeColor = Drawing.Color.Green
            '            lblMsg.Visible = True
            '            lblMsg.Text = "Course Registered Successfully."
            '            'Response.Redirect("CourseMaintenance.aspx", False)
            '            'ElseIf TxtCourseCode.Enabled = False Then
            '            '    'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
            '            '    If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty) Then
            '            '        UpdateData()
            '            '    End If
            '        End If
            '    ElseIf booldecision = False Then
            '        lblMsg.ForeColor = Drawing.Color.Red
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Entered Course Name is already exists. Please enter different Course Name."
            '    End If
            '    'ElseIf (TxtCourseName.Text = String.Empty And TxtCourseCode.Text = String.Empty And ddlMainCourse.SelectedItem.Value = 0 And multiBoxId.SelectedIndex = -1 And txtTotalTime.Text = String.Empty) Then
            '    '    lblMsg.Visible = True
            '    '    lblMsg.Text = "All Fields Are Mandatory."
            'End If

            ''End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub



    'Added by Pranit Chimurkar on 2019/10/24
    Protected Sub ImgBtnBack_Click(sender As Object, e As EventArgs) Handles ImgBtnBack.Click
        'Session.Remove("CourseID")
        Try
            If Request.QueryString("pi") Is Nothing Then
                Response.Redirect("WeightSearch.aspx", False)
                Session.Remove("FromWm")
            Else
                Session.Add("FromWm", "true")
                If Not Request.QueryString("pi") Is Nothing Then
                    Dim intpi As Integer = CInt(Request.QueryString("pi").ToString())
                    Response.Redirect("WeightSearch.aspx?pi=" & intpi, False)
                Else
                    Response.Redirect("WeightSearch.aspx", True)
                End If

            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
        ' Response.Redirect("WeightSearch.aspx")
    End Sub


    'Added by Pranit Chimurkar on 2019/10/24
    Protected Sub imgBtnUpdate_Click(sender As Object, e As EventArgs) Handles imgBtnUpdate.Click

        Try
            Dim booldecision As Boolean = True
            If Validate() = True Then
                If CheckQuestionTypeTotal(txtTF.Text, txtMChoice.Text, txtBlank.Text) = True Then
                    If (CheckBasicTotal(txtBasicq.Text, txtIntermediateq.Text)) = True Then
                        '   If (IsVacant() = True) Then
                        '  If CheckSubcjectExixsts(ddlSubject.SelectedValue.ToString, ddlCourse.SelectedValue.ToString) = True Then
                        '  If lblVacant.Text = 0 Then
                        'Else
                        If CheckNewWeight(Session("wid").ToString, CInt(txtWeight.Text), ddlCourse.SelectedValue) = True Then
                            UpdateData(Session("wid").ToString)
                        Else
                            lblMsg.Text = Resources.Resource.WeightManage_NotSufficien
                            'Commented by Pranit Chimurkar on 2019/10/23
                            'If Request.Cookies("Lang").Value = "en-us" Then
                            '    lblMsg.Text = CommonMessageLingual.lblMsgNotSuffi.ToString
                            'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                            '    lblMsg.Text = CommonMessageLingual.lblMsgNotSuffi.ToString
                            'End If

                            lblMsg.ForeColor = Drawing.Color.Red
                            lblMsg.Visible = True

                        End If

                        ' End If

                        'Else
                        ' lblMsg.Text = "This Subject is Alrea"
                        ' lblMsg.Visible = True
                        ' lblMsg.ForeColor = Drawing.Color.Red
                        ' End If
                        'End If
                    End If
                Else
                    lblMsg.Visible = True

                End If
            Else
                lblMsg.Visible = True
            End If
            'If TxtCourseCode.Enabled = True Then
            '    imgBtnReset.Visible = True
            '    imgBtnClear.Visible = False
            '    updatespan.Visible = False
            '    'centernames.Visible = True
            '    '   insertspan.Visible = False
            '    'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
            '    'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty) Then
            '    Dim totalTimeValue As Boolean = ValidationForNumber(txtTotalTime.Text)
            '    If ddlMainCourse.SelectedItem.Value = 0 Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Select Main Course."
            '        ddlMainCourse.Focus()
            '        'ElseIf multiBoxId.SelectedIndex = -1 Then
            '        '    lblMsg.Visible = True
            '        '    lblMsg.Text = "Please Select At Least One Center."
            '    ElseIf TxtCourseCode.Text = String.Empty Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Enter Course Code."
            '        TxtCourseCode.Focus()
            '    ElseIf TxtCourseName.Text = String.Empty Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Enter Course Name."
            '        TxtCourseName.Focus()
            '    ElseIf txtTotalTime.Text = String.Empty Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Enter Total Alloted Exam Time in Minutes."
            '        txtTotalTime.Focus()
            '    ElseIf (totalTimeValue = False) Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Enter Numbers In Total Alloted Exam Time(Minute)."
            '        txtTotalTime.Focus()
            '    ElseIf txtTotalMarks.Text = String.Empty Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Enter Total Marks."
            '        txtTotalMarks.Focus()
            '    ElseIf txtPassingMarks.Text = String.Empty Then
            '        lblMsg.Visible = True
            '        lblMsg.Text = "Please Enter Total Passing Marks."
            '        txtPassingMarks.Focus()
            '    ElseIf (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty And txtTotalMarks.Text <> String.Empty And txtPassingMarks.Text <> String.Empty) Then


            '        '/********************Added By:Jatin Gangajaliya,2011/3/11****************/
            '        If Not (Session.Item("cname").ToString = TxtCourseName.Text) Then
            '            Dim objcommon As New CommonFunction()
            '            booldecision = objcommon.ValidateCourseName(TxtCourseName.Text)
            '        End If

            '        If booldecision = True Then
            '            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            '            Try
            '                If objconn.connect(strPathDb) = True Then
            '                    sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
            '                    UpdateData()
            '                    sqlTrans.Commit()
            '                End If
            '            Catch ex As Exception
            '                sqlTrans.Rollback()
            '            Finally
            '                Session.Remove("CourseID")
            '                objconn.disconnect()
            '            End Try
            '            lblMsg.ForeColor = Drawing.Color.Green
            '            lblMsg.Visible = True
            '            lblMsg.Text = "Course Updated Successfully..."
            '            Session.Add("updatecheck", "true")
            '            Response.Redirect("CourseMaintenance.aspx", False)
            '        ElseIf booldecision = False Then
            '            lblMsg.ForeColor = Drawing.Color.Red
            '            lblMsg.Visible = True
            '            lblMsg.Text = "Entered Course Name is already exists. Please enter different Course Name."
            '        End If
            '    End If
            '    '/*******************************end*******************************/               
            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
    'Added by Pranit Chimurkar on 2019/10/24
    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        Try
            ClearControls()
            'ddlCourses.Items.Clear()
            'Dim l1 As New ListItem
            'l1.Text = "---- Select ----"
            'l1.Value = 0
            'ddlCourses.Items.Add(l1)
            'ddlCourses.SelectedIndex = 0
            'ddlMainCourse.SelectedIndex = 0
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

    'Added by Pranit Chimurkar on 2019/10/24
    Protected Sub imgBtnReset_Click(sender As Object, e As EventArgs) Handles imgBtnReset.Click
        Try
            EditMode(Session("wid").ToString)
            'CourseId = Session.Item("CourseID")
            'lblhead.Text = "Course Modification"
            'legendLabel.Text = "Course Modification Details"
            'EditMode()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Private Sub ddlCourse_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCourse.SelectedIndexChanged
        Dim vcnt As Integer
        Dim objComnFun As CommonFunction
        Try
            vcnt = Convert.ToInt32(GetVacantSpace(ddlCourse.SelectedValue))
            ''Modified By Irfan On 24/02/2015
            ''Purpose: When Index is 0 of ddlCourse show vacant blank.
            If ddlCourse.SelectedIndex <> 0 Then
                If vcnt < 0 Then
                    lblVacant.ForeColor = Drawing.Color.Red
                Else
                    lblVacant.ForeColor = Drawing.Color.Green
                End If
                lblVacant.Text = vcnt.ToString
            Else
                lblVacant.Text = " "
            End If
            ''End Modify

            'Added by Pranit on 06/12/2019
            If vcnt > 0 Then
                ddlSubject.Enabled = True
                txtWeight.Enabled = True
                txtBasicq.Enabled = True
                txtIntermediateq.Enabled = True
                txtTF.Enabled = True
                txtBlank.Enabled = True
                txtMChoice.Enabled = True
            Else
                ddlSubject.Enabled = False
                txtWeight.Enabled = False
                txtBasicq.Enabled = False
                txtIntermediateq.Enabled = False
                txtTF.Enabled = False
                txtBlank.Enabled = False
                txtMChoice.Enabled = False
            End If
            ''Added By Irfan Mansuri On 23/02/2015
            ''Purpose: Get total marks of assigned for this selected Course in Label
            objComnFun = New CommonFunction()
            If ddlCourse.SelectedIndex <> 0 Then
                lblMarks.Text = objComnFun.getSingleValue("Total_Marks", "M_Course", "Course_Name='" + ddlCourse.SelectedItem.Text + "'")
            Else
                lblMarks.Text = " "
            End If
            ''End By Irfan

            'If ddlCourse.SelectedValue <> 0 Then

            '    Dim ds As DataSet = Nothing
            '    Dim da As SqlDataAdapter = Nothing
            '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
            '    Try
            '        If objconn.connect(strPathDb) = True Then
            '            ds = New DataSet
            '            da = New SqlDataAdapter("select Course_id, total_Marks,Total_passmarks from m_Course where Course_id = " & ddlCourse.SelectedValue, objconn.MyConnection)
            '            ds.Clear()
            '            da.Fill(ds)

            '        End If
            '    Catch ex As Exception

            '    Finally
            '        ds = Nothing
            '        da = Nothing
            '    End Try
            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            vcnt = Nothing
        End Try
    End Sub

    ''***************************************************************************************''
    ''Author: Irfan Mansuri                                                                  ''
    ''Description: Subject Dropdownlist index change event to display subject information.   ''
    ''Created Date: 23/02/2015                                                               '' 
    ''***************************************************************************************''
    Private Sub ddlSubject_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubject.SelectedIndexChanged
        Dim objComnFun As CommonFunction
        Try
            objComnFun = New CommonFunction()
            If ddlSubject.SelectedIndex <> 0 Then
                ''Get Total Number of Questions for this Subject
                lblTotalQuestion.Text = objComnFun.getSingleValue("count(qnid)", "m_question", "test_type IN (select test_type from m_testinfo where test_name='" + ddlSubject.SelectedItem.Text + "')")
                ''Get Total marks of those questions
                lblTotalMarks.Text = objComnFun.getSingleValue("sum(total_marks)", "m_question", "test_type IN (select test_type from m_testinfo where test_name='" + ddlSubject.SelectedItem.Text + "')")
                ''Get Total Difficulty lebel:Basic Questions
                lblBasicQuestion.Text = objComnFun.getSingleValue("count(qnid)", "m_question", "test_type IN (select test_type from m_testinfo where test_name='" + ddlSubject.SelectedItem.Text + "') and qlevel='0'")
                ''Get Total Difficulty lebel:Intermediate Questions
                lblInterQuestion.Text = objComnFun.getSingleValue("count(qnid)", "m_question", "test_type IN (select test_type from m_testinfo where test_name='" + ddlSubject.SelectedItem.Text + "') and qlevel='1'")
            Else
                lblTotalQuestion.Text = " "
                lblTotalMarks.Text = " "
                lblBasicQuestion.Text = " "
                lblInterQuestion.Text = " "
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objComnFun = Nothing
        End Try
    End Sub

    Public Function CheckQuestionTypeTotal(ByVal tf As String, ByVal mc As String, ByVal blank As String) As Boolean
        Try
            Dim itf As Integer = Convert.ToInt32(tf)
            Dim imc As Integer = Convert.ToInt32(mc)
            Dim ibk As Integer = Convert.ToInt32(blank)
            If (itf + imc + ibk) < 100 Then
                lblMsg.Text = Resources.Resource.WeightManage_EqualtoHundred
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlQuesPercentage.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlQuesPercentage.ToString
                'End If

                lblMsg.Visible = True
                txtTF.Focus()
                Return False
            ElseIf (itf + imc + ibk) > 100 Then
                lblMsg.Text = Resources.Resource.WeightManage_EqualtoHundred
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlQuesPercentage.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlQuesPercentage.ToString
                'End If

                lblMsg.Visible = True
                txtTF.Focus()
                Return False
            Else
                lblMsg.Text = ""
                lblMsg.Visible = False
                Return True
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Function

    Public Function CheckBasicTotal(ByVal basic As String, ByVal Inter As String) As Boolean
        Try
            Dim ibasic As Integer = Convert.ToInt32(basic)
            Dim iinter As Integer = Convert.ToInt32(Inter)

            If (ibasic + iinter) < 100 Then
                lblMsg.Text = Resources.Resource.WeightManage_Intermediate
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlIntermediate.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlIntermediate.ToString
                'End If

                lblMsg.Visible = True
                lblMsg.ForeColor = Drawing.Color.Red
                txtBasicq.Focus()
                Return False
            ElseIf (ibasic + iinter) > 100 Then
                lblMsg.Text = Resources.Resource.WeightManage_Intermediate
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlIntermediate.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTtlIntermediate.ToString
                'End If

                lblMsg.Visible = True
                txtBasicq.Focus()
                Return False
            Else
                lblMsg.Text = ""
                lblMsg.Visible = False
                Return True
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Function
#Region "CheckWeightagePercent"
    'Added by    :: Saraswati Patel
    'Description :: Check whether percentage is valid or not
    'Date        :: 2012/07/26
    Public Function CheckWeightagePercent() As String
        Dim Total_marks As String
        Dim cds As DataSet
        Dim cda As SqlDataAdapter
        Dim Course_id As String = ddlCourse.SelectedValue.ToString
        Dim test_type As String = ddlSubject.SelectedValue.ToString
        Dim query As String = "select  Total_marks from M_Course where Course_id=" & Course_id '& " and test_type=" & test_type
        'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Try
            If objconn.connect() = True Then
                cda = New SqlDataAdapter(query, objconn.MyConnection)
                cds = New DataSet
                cda.Fill(cds)
                If (cds.Tables(0).Rows.Count > 0) Then
                    Total_marks = cds.Tables(0).Rows(0).Item(0).ToString()
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn.disconnect()
            cda = Nothing
            cds = Nothing
            query = Nothing
            strPathDb = Nothing
        End Try
        Return Total_marks
    End Function
#End Region
#Region "CalculatePercentage"
    'Added by    :: Saraswati Patel
    'Description :: Calculate percentage which you can add.
    'Date        :: 2012/07/26
    Public Function CalculatePercentage() As String
        Dim Total_marks As Integer = Convert.ToInt16(CheckWeightagePercent())
        Dim Subject_Weightage As Integer = Convert.ToInt16(txtWeight.Text)
        Dim cal As Double = Total_marks * Subject_Weightage / 100
        Dim strCal As String = cal.ToString()
        Dim strCalA() As String
        Dim dblPer As Double
        Dim dblPerA() As Double
        Dim intLoop As Integer = 0
        Dim intValue As Integer
        Dim bool As Boolean = True
        strCalA = strCal.Split(".")
        intValue = Convert.ToInt16(strCalA(0))
        While strCal.Contains(".")
            bool = False
            dblPer = (intValue + intLoop) * 100 / Total_marks
            strCal = dblPer.ToString
            strCalA = strCal.Split(".")
            cal = Total_marks * strCalA(0) / 100
            If cal = 0.0 Then
                strCal = "1.0"
            Else
                strCal = cal.ToString
            End If
            intLoop += 1
        End While
        If bool Then
            Return "Nothing"
        Else
            Return strCalA(0)
        End If

    End Function
#End Region


    Public Function Validate() As Boolean
        Dim flag As Boolean = True
        Try
            If ddlCourse.SelectedValue = 0 Then
                lblMsg.Text = Resources.Resource.WeightManage_SelectCours
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgCrsWei.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgCrsWei.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                ddlCourse.Focus()
                Return False
            End If
            If ddlSubject.SelectedValue = 0 Then
                lblMsg.Text = Resources.Resource.WeightManage_SelectSubj
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSub.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSub.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                ddlSubject.Focus()
                Return False
            End If

            If txtWeight.Text = String.Empty Then
                lblMsg.Text = Resources.Resource.WeightManage_EnterSubWeig
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSubWeig.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgSubWeig.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                txtWeight.Focus()
                Return False
            Else
                If Not (IsNumeric(txtWeight.Text)) Then
                    lblMsg.Text = Resources.Resource.WeightManage_Enternumval
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumericVal.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumericVal.ToString
                    'End If

                    lblMsg.Visible = True
                    flag = False
                    txtWeight.Focus()
                    Return False
                    'ElseIf CalculatePercentage() <> "Nothing" Then

                    '    lblMsg.Text = "You can assign weightage " + CalculatePercentage()
                    '    lblMsg.Visible = True
                    '    flag = False
                    '    txtWeight.Focus()
                    '    Return False

                Else
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                End If
            End If
            ' CalculatePercentage()
            If txtTF.Text = String.Empty Then
                lblMsg.Text = Resources.Resource.WeightManage_Entertruefa
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTF.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgTF.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                txtTF.Focus()
                Return False
            Else
                If Not (IsNumeric(txtTF.Text)) Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NumTrFaVal
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumTF.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumTF.ToString
                    'End If

                    lblMsg.Visible = True
                    flag = False
                    txtTF.Focus()
                    Return False
                Else
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                End If
            End If

            If txtMChoice.Text = String.Empty Then
                lblMsg.Text = Resources.Resource.WeightMgt_MCQ
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgMulChoi.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgMulChoi.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                txtMChoice.Focus()
                Return False
            Else
                If Not (IsNumeric(txtMChoice.Text)) Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NumvalMCQ
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumMultiChoic.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumMultiChoic.ToString
                    'End If

                    lblMsg.Visible = True
                    flag = False
                    txtMChoice.Focus()
                    Return False
                Else
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                End If
            End If

            If txtBlank.Text = String.Empty Then
                lblMsg.Text = Resources.Resource.WeightMgt_ValBlanks
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgBlank.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgBlank.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                txtBlank.Focus()
                Return False
            Else
                If Not (IsNumeric(txtBlank.Text)) Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NumvalBlanks
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumBlank.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumBlank.ToString
                    'End If

                    lblMsg.Visible = True
                    flag = False
                    txtBlank.Focus()
                    Return False
                Else
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                End If
            End If

            If txtBasicq.Text = String.Empty Then
                lblMsg.Text = Resources.Resource.WeightMgt_ValBasic
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgBasQue.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgBasQue.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                txtBasicq.Focus()
                Return False
            Else
                If Not (IsNumeric(txtBasicq.Text)) Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NumValBasicQ
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumBasQue.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumBasQue.ToString
                    'End If

                    lblMsg.Visible = True
                    flag = False
                    txtBasicq.Focus()
                    Return False
                Else
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                End If
            End If
            If txtIntermediateq.Text = String.Empty Then
                lblMsg.Text = Resources.Resource.WeightMgt_IntQ
                'Commented by Pranit Chimurkar on 2019/10/23
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgIntQue.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    lblMsg.Text = CommonMessageLingual.lblMsgIntQue.ToString
                'End If

                lblMsg.Visible = True
                flag = False
                txtIntermediateq.Focus()
                Return False
            Else
                If Not (IsNumeric(txtIntermediateq.Text)) Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NVIQ
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumInterQue.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNumInterQue.ToString
                    'End If

                    lblMsg.Visible = True
                    flag = False
                    txtIntermediateq.Focus()
                    Return False
                Else
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
        Return flag
    End Function

    Public Function GetVacantSpace(ByVal courseid As String) As String
        Dim vds As DataSet
        Dim vda As SqlDataAdapter
        Dim Vacancy As Integer = 100
        Dim dbVacancy As Integer = 0
        Dim query As String = "select isnull(sum(Sub_Weightage),0) from M_weightage where   course_id=" & courseid
        'Dim query As String = "select isnull(sum(Sub_Weightage),0) from M_weightage where del_flag=0 and course_id=" & courseid
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        Try
            vds = New DataSet
            If objconn.connect() = True Then
                vda = New SqlDataAdapter(query, objconn.MyConnection)
                vda.Fill(vds)
                dbVacancy = Convert.ToInt32(vds.Tables(0).Rows(0).Item(0))
                Vacancy = Vacancy - dbVacancy
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            vds = Nothing
            vda = Nothing
            query = Nothing
            dbVacancy = Nothing
        End Try
        Return Vacancy.ToString
    End Function

    Public Function IsVacant() As Boolean
        Try
            If (Convert.ToInt32(lblVacant.Text) <= 0) Then
                If (Convert.ToInt32(lblVacant.Text) = 0) Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NoSpace
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNoSpace.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNoSpace.ToString
                    'End If

                ElseIf Convert.ToInt32(lblVacant.Text) < 0 Then
                    lblMsg.Text = Resources.Resource.WeightMgt_NewSubjectCoLi
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNewSub.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblMsg.Text = CommonMessageLingual.lblMsgNewSub.ToString
                    'End If

                End If
                lblMsg.ForeColor = Drawing.Color.Red
                lblMsg.Visible = True
                Return False
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

    Public Function CheckSubcjectExixsts(ByVal test_type As String, ByVal Course_id As String) As Boolean
        Dim decision As Boolean
        Dim cds As DataSet
        Dim cda As SqlDataAdapter
        Dim query As String = "select * from M_weightage where course_id=" & Course_id & " and test_type=" & test_type
        'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Try
            If objconn.connect() = True Then
                cda = New SqlDataAdapter(query, objconn.MyConnection)
                cds = New DataSet
                cda.Fill(cds)
                If (cds.Tables(0).Rows.Count > 0) Then
                    decision = False
                Else
                    decision = True
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn.disconnect()
            cda = Nothing
            cds = Nothing
            query = Nothing
            strPathDb = Nothing
        End Try
        Return decision
    End Function

    Public Function CheckNewWeight(ByVal Wid As String, ByVal NewWeight As Integer, ByVal Courseid As String) As Boolean
        Dim flag As Boolean = True
        Dim q As String = "select sum(sub_weightage) from M_weightage where Course_id =" & Courseid
        Dim q2 As String = " select sub_weightage from M_weightage where weightage_id = " & Wid & " and Course_id =" & Courseid
        Dim mcmd As SqlCommand
        Dim mda As SqlDataAdapter
        Dim mds As DataSet
        Try
            If objconn.connect() = True Then
                mda = New SqlDataAdapter(q, objconn.MyConnection)
                mds = New DataSet
                mda.Fill(mds)
                Dim Num As Integer = CInt(mds.Tables(0).Rows(0).Item(0))
                mda = Nothing
                mds = Nothing
                mda = New SqlDataAdapter(q2, objconn.MyConnection)
                mds = New DataSet
                mda.Fill(mds)
                Num = Num - CInt(mds.Tables(0).Rows(0).Item(0))

                If NewWeight + Num <= 100 Then
                    flag = True
                Else
                    flag = False
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            mda = Nothing
            mds = Nothing
            objconn.disconnect()
            mcmd = Nothing
            q = Nothing
            q2 = Nothing
        End Try

        Return flag
    End Function

End Class