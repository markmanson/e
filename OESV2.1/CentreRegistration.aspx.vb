Imports System.Data
Imports System.Data.SqlClient
Partial Public Class CenterRegistration

#Region "Variables"
    Inherits BasePage

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("CourseRegistration")
    Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader
    Dim CenterId As Integer
    Dim sqlTrans As SqlTransaction
    Dim flg As Boolean
    Dim strtemp As String = ""
    '************Added By Nisha on 2018/05/14******************
    Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
    '************Ended By Nisha on 2018/05/14******************
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            txtContactNo.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            If Session.Item("CenterIDValue") <> Nothing Or Session.Item("CenterIDValue") <> 0 Then
                lblhead.Text = Resources.Resource.CenterRegistration_clsmdf
                legendLabel.Text = Resources.Resource.CenterRegistration_clsmdfdts
            Else
                legendLabel.Text = Resources.Resource.CenterRegistration_ClsRegDts
                lblhead.Text = Resources.Resource.CenterRegistration_ClassRegis
            End If

            If Not IsPostBack Then
                CenterId = Convert.ToInt32(Session.Item("CenterIDValue"))
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
    'Purpose: To fill the All fields for Course
    '********************************************************************
    Public Sub EditMode()
        Dim fbl As Boolean

        If (Session.Item("CenterID") = Nothing Or Session.Item("CenterID") = 0) Then
            CenterId = Convert.ToInt32(Session.Item("CenterIDValue"))
            Session.Add("CenterID", CenterId)
        Else
            CenterId = Convert.ToInt32(Session.Item("CenterID"))
        End If

        If CenterId <> Nothing Or CenterId <> 0 Then
            Dim query As String = ""
            'strPathDb = ConfigurationManager.AppSettings("PathDb")
            Try
                If objconn.connect() = True Then
                    Dim query1 As String = "select Distinct Center_ID,Center_Name,Center_Code,Owner_Name,City_Name,State_Name,Country_Name,Contact_No,Email,Pin_Code,Centre_Address from M_Centers where Center_ID= '" & CenterId & "'"
                    objCommand = New SqlCommand(query1, objconn.MyConnection)
                    objDataReader = objCommand.ExecuteReader()

                    While objDataReader.Read()
                        Dim str As String = objDataReader.Item("Owner_Name")
                        Dim strOwnerName() As String = str.Split(" ")
                        txtCenterName.Text = objDataReader.Item("Center_Name")
                        txtCentreCode.Text = objDataReader.Item("Center_Code")


                        '/*****************Start,Jatin Gangajaliya, 2011/03/30**************/

                        If strOwnerName.Length = 2 Then
                            If Not strOwnerName(0).ToString() = Nothing Then
                                TxtFName.Text = strOwnerName(0).ToString()
                            End If

                            If Not strOwnerName(1).ToString() = Nothing Then
                                TxtLName.Text = strOwnerName(1).ToString()
                            End If
                        ElseIf strOwnerName.Length = 3 Then
                            If Not strOwnerName(0).ToString() = Nothing Then
                                TxtFName.Text = strOwnerName(0).ToString()
                            End If
                            If Not strOwnerName(1).ToString() = Nothing Then
                                TxtMName.Text = strOwnerName(1).ToString()
                            End If
                            If Not strOwnerName(2).ToString() = Nothing Then
                                TxtLName.Text = strOwnerName(2).ToString()
                            End If
                        End If

                        '/*************************End*************************************/

                        txtCity.Text = objDataReader.Item("City_Name")
                        txtstate.Text = objDataReader.Item("State_Name")
                        txtCountry.Text = objDataReader.Item("Country_Name")
                        txtContactNo.Text = objDataReader.Item("Contact_No")
                        txtEmail.Text = objDataReader.Item("Email")
                        txtPinCode.Text = objDataReader.Item("Pin_Code")
                        TxtCentreAddress.Text = objDataReader.Item("Centre_Address")
                    End While

                    'If Not txtCenterName.Text = "" Then
                    '    Session.Item("cname") = txtCenterName.Text
                    'End If
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
                Session.Item("CenterIDValue") = Nothing
                query = Nothing
                fbl = Nothing
                strPathDb = Nothing
            End Try
        End If

    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose:ValidationForNumber
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

    Protected Sub imgBtnSubmit_Click(sender As Object, e As EventArgs) Handles imgBtnSubmit.Click
        Try
            Dim bolflg As Boolean = validations()
            If (bolflg = True) Then
                If (txtCenterName.Text <> String.Empty And txtFName.Text <> String.Empty And txtLName.Text <> String.Empty And TxtCentreAddress.Text <> String.Empty And txtCity.Text <> String.Empty And txtstate.Text <> String.Empty And txtCountry.Text <> String.Empty And txtPinCode.Text <> String.Empty And txtContactNo.Text <> String.Empty And txtEmail.Text <> String.Empty) Then

                    'Dim objcommon As New CommonFunction()
                    '  Dim booldecision As Boolean = objcommon.ValidateCourseName(TxtCourseName.Text)

                    'If booldecision = True Then
                    'strPathDb = ConfigurationManager.AppSettings("PathDb")
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
                    imgBtnSubmit.Visible = True
                    imgBtnUpdate.Visible = False


                    ClearControls()
                    lblMsg.ForeColor = Drawing.Color.Green
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CenterRegistration_clsregs
                    'Response.Redirect("CourseMaintenance.aspx", False)
                    'ElseIf TxtCourseCode.Enabled = False Then
                    '    'If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalQues.Text <> String.Empty And txtTotalTime.Text <> String.Empty And TxtCorrectAns.Text <> String.Empty) Then
                    '    If (TxtCourseName.Text <> String.Empty And TxtCourseCode.Text <> String.Empty And ddlMainCourse.SelectedItem.Value <> 0 And multiBoxId.SelectedIndex <> -1 And ddlCourses.SelectedItem.Value <> 0 And txtTotalTime.Text <> String.Empty) Then
                    '        UpdateData()
                    '    End If
                    'End If
                    'ElseIf booldecision = False Then
                    '    lblMsg.ForeColor = Drawing.Color.Red
                    '    lblMsg.Visible = True
                    '    lblMsg.Text = "Entered Course Name is already exists. Please enter different Course Name."
                    'End If
                    'ElseIf (TxtCourseName.Text = String.Empty And TxtCourseCode.Text = String.Empty And ddlMainCourse.SelectedItem.Value = 0 And multiBoxId.SelectedIndex = -1 And txtTotalTime.Text = String.Empty) Then
                    '    lblMsg.Visible = True
                    '    lblMsg.Text = "All Fields Are Mandatory."
                End If
            End If
            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Public Function validations() As Boolean
        Dim contactno As Boolean
        Try
            contactno = ValidationForNumber(txtContactNo.Text)
            Static emailExpression As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")
            If txtCenterName.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_clsnmerr
                txtCenterName.Focus()
                Return False
            ElseIf TxtFName.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_fnerr
                txtFName.Focus()
                Return False
            ElseIf TxtLName.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_lnerr
                txtLName.Focus()
                Return False
            ElseIf TxtCentreAddress.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_adderr
                TxtCentreAddress.Focus()
                Return False
            ElseIf txtCity.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_cityerr
                txtCity.Focus()
                Return False
            ElseIf txtstate.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_stateerr
                txtstate.Focus()
                Return False
            ElseIf txtCountry.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_countryerr
                txtCountry.Focus()
                Return False
            ElseIf txtPinCode.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_pinerr
                txtPinCode.Focus()
                Return False
            ElseIf txtContactNo.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_Contacterr
                txtContactNo.Focus()
                Return False
            ElseIf (contactno = False) Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterMaintanance_noerr
                txtContactNo.Focus()
                Return False
            ElseIf txtEmail.Text = String.Empty Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_emailerr
                txtEmail.Focus()
                Return False
            ElseIf Not (emailExpression.IsMatch(txtEmail.Text)) Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_validemailerr
                txtEmail.Focus()
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            contactno = Nothing
        End Try
    End Function

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Insert Data into database
    '********************************************************************
    Public Sub InsertData()
        Dim query As String = ""
        Dim sqlTrans As SqlTransaction
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try

            If objconn.connect() = True Then
                Dim fieldsCourse As String = "Center_Name,Center_Code,Owner_Name,City_Name,State_Name,Country_Name,Contact_No,Email,Pin_Code,Centre_Address"

                query = "Insert Into M_Centers (" & fieldsCourse & ") values('" & txtCenterName.Text & "','" & txtCentreCode.Text & "','" & TxtFName.Text & " " & TxtMName.Text & " " & TxtLName.Text & "','" & txtCity.Text & "','" & txtstate.Text & "','" & txtCountry.Text & "','" & txtContactNo.Text & "','" & txtEmail.Text & "','" & txtPinCode.Text & "','" & TxtCentreAddress.Text & "')"
                Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)
                ins_cmd.ExecuteNonQuery()
                'GetCourseID()
                objconn.disconnect()
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
            sqlTrans = Nothing
        End Try
    End Sub

    ''********************************************************************
    ''Code added by Monal Shah
    ''Purpose: To get CourseID and perform insert update
    ''********************************************************************
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
    '        lblMsg.Visible = True
    '        lblMsg.Text = ex.Message()
    '        Response.Redirect("error.aspx?err=" & ex.Message, False)
    '    Finally
    '        'sqlTrans.Commit()
    '    End Try

    'End Sub
    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        Try
            ClearControls()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub imgBtnUpdate_Click(sender As Object, e As EventArgs) Handles imgBtnUpdate.Click
        Dim bolflg As Boolean = validations()
        Dim booldecision As Boolean = True
        Try
            imgBtnReset.Visible = True
            imgBtnClear.Visible = False

            If (bolflg = True) Then
                If (txtCenterName.Text <> String.Empty And txtFName.Text <> String.Empty And txtLName.Text <> String.Empty And TxtCentreAddress.Text <> String.Empty And txtCity.Text <> String.Empty And txtstate.Text <> String.Empty And txtCountry.Text <> String.Empty And txtPinCode.Text <> String.Empty And txtContactNo.Text <> String.Empty And txtEmail.Text <> String.Empty) Then


                    'If Not (Session.Item("cname").ToString = TxtCourseName.Text) Then
                    '    Dim objcommon As New CommonFunction()
                    '    booldecision = objcommon.ValidateCourseName(TxtCourseName.Text)
                    'End If

                    'If booldecision = True Then
                    'strPathDb = ConfigurationManager.AppSettings("PathDb")
                    Try
                        If objconn.connect() = True Then
                            sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            UpdateData()

                            '************Commented By Nisha on 2018/05/14**************
                            'Reason:Information is not updated
                            ' sqlTrans.Commit()Nisha
                            '************Ended By Nisha on 2018/05/14******************
                        End If
                    Catch ex As Exception
                        sqlTrans.Rollback()
                    Finally
                        ' Session.Remove("CenterID")
                        objconn.disconnect()
                    End Try
                    lblMsg.ForeColor = Drawing.Color.Green
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CenterRegistration_Clsupdt
                    Session.Add("updatecheck", "true")
                    Response.Redirect("CentreMaintenance.aspx", False)
                    'ElseIf booldecision = False Then
                    '    lblMsg.ForeColor = Drawing.Color.Red
                    '    lblMsg.Visible = True
                    '    lblMsg.Text = "Entered Course Name is already exists. Please enter different Course Name."
                    'End If
                    ' End If
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            booldecision = Nothing
            bolflg = Nothing
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Update Data into database
    '********************************************************************
    Public Sub UpdateData()
        Dim query As String = ""
        '**********Commented By Nisha on 2018/05/14*****************
        'Reason:Information is not updated
        'Dim sqlTrans1 As SqlTransaction
        'sqlTrans1.Connection.BeginTransaction()
        '**********Ended By Nisha on 2018/05/14*********************

        '************Added By Nisha on 2018/05/14******************
        objconnect.Open()
        sqlTrans = objconnect.BeginTransaction(IsolationLevel.ReadCommitted)
        '**********Ended By Nisha on 2018/05/14*********************

        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try
            Dim fieldsCourse As String = "Center_Name,Center_Code,Owner_Name,City_Name,State_Name,Country_Name,Contact_No,Email,Pin_Code,Centre_Address"
            If objconn.connect() = True Then
                query = "Update M_Centers Set Center_Name='" & txtCenterName.Text & "',Center_Code='" & txtCentreCode.Text & "',Owner_Name='" & TxtFName.Text & " " & TxtMName.Text & " " & TxtLName.Text & "', City_Name='" & txtCity.Text & "',State_Name='" & txtstate.Text & "',Country_Name='" & txtCountry.Text & "', Contact_No='" & txtContactNo.Text & "',Email='" & txtEmail.Text & "',Pin_Code='" & txtPinCode.Text & "',Centre_Address='" & TxtCentreAddress.Text & "' where Center_ID='" & Session.Item("CenterId") & "'"

                '**********Commented By Nisha on 2018/05/14*****************
                'Reason:Information is not updated
                ' Dim update_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans1)
                '**********Ended By Nisha on 2018/05/14*********************

                '************Modified By Nisha on 2018/05/14******************
                Dim update_cmd As New SqlCommand(query, objconnect, sqlTrans)
                '**********Ended By Nisha on 2018/05/14*********************

                update_cmd.ExecuteNonQuery()
                sqlTrans.Commit()
                'GetCourseID()
                objconn.disconnect()

            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            query = Nothing
            sqlTrans = Nothing
        End Try
    End Sub

    Protected Sub ImgBtnBack_Click(sender As Object, e As EventArgs) Handles ImgBtnBack.Click
        Try
            Session.Remove("CenterID")
            If Session("fornewcenter") = "true" Then
                Response.Redirect("CentreMaintenance.aspx", False)
                Session.Remove("fornewcenter")
            Else
                Session.Add("fromcentreupdate", "true")
                Dim intpi As Integer = CInt(Request.QueryString("pi").ToString())
                Response.Redirect("CentreMaintenance.aspx?pi=" & intpi, False)
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub imgBtnReset_Click(sender As Object, e As EventArgs) Handles imgBtnReset.Click
        Try
            CenterId = Session.Item("CenterID")
            lblhead.Text = Resources.Resource.CenterRegistration_clsmdf
            legendLabel.Text = Resources.Resource.CenterRegistration_clsmdfdts
            EditMode()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Clear Controls
    '********************************************************************
    Public Sub ClearControls()
        Try
            lblMsg.Visible = False
            txtCenterName.Text = String.Empty
            txtCentreCode.Text = String.Empty
            TxtFName.Text = String.Empty
            TxtLName.Text = String.Empty
            TxtMName.Text = String.Empty
            txtCity.Text = String.Empty
            txtstate.Text = String.Empty
            txtCountry.Text = String.Empty
            txtContactNo.Text = String.Empty
            txtEmail.Text = String.Empty
            txtPinCode.Text = String.Empty
            TxtCentreAddress.Text = String.Empty
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

End Class