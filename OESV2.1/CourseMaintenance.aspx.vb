Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Public Class CourseMaintenance

#Region "Variables"
    Inherits BasePage

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("CourseMaintenance")
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader             'SqlDataReader object
    Dim flg As Boolean = True
    Dim lnkEditID As Integer
    Dim con As SqlConnection
    Dim sqlcon As String
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'Added By bhumi On 21/8/2015
            'Reason: while index changed, previous page validation message till visible on next page
            lblMsg.Visible = False
            'ended by bhumi
            txtTotalTime.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            ddlSectionDes.Attributes.Add("onkeypress", "addtitleattributesSection()")
            ddlCourses.Attributes.Add("onkeypress", "addTitleAttributes1()")
            ddlMainCourse.Attributes.Add("onkeypress", "addTitleAttributes()")
            If Not IsPostBack Then
                FillMainCourseCombo()
                'ddlSectionDes.Items.Clear()
                'Dim l1 As New ListItem
                'l1.Text = "---- Select ----"
                'l1.Value = 0
                'ddlSectionDes.Items.Add(l1)

                'Added by Pranit on 02/12/2019
                FillSectionCourseCombo()
                FillCoursesCombo()

                If Not Session("mainCourse") Is Nothing Then
                    ddlMainCourse.SelectedValue = CInt(Session("mainCourse"))
                    FillSectionCourseCombo()
                    DataGridCourseDetails.Visible = True
                    DivGrid.Visible = True
                    Session.Remove("mainCourse")
                End If
                'Added by Pranit on 02/12/2019
                If Not Session("SectionCourse") Is Nothing Then
                    ddlSectionDes.SelectedValue = CInt(Session("SectionCourse"))
                    FillCoursesCombo()
                    DataGridCourseDetails.Visible = True
                    DivGrid.Visible = True
                    Session.Remove("SectionCourse")
                End If
                If Not Session("CourseName") Is Nothing Then
                    ddlCourses.SelectedValue = CInt(Session("CourseName"))
                    Session.Remove("CourseName")
                End If
                If Not Session("Time") Is Nothing Then
                    txtTotalTime.Text = Session("Time").ToString
                    Session.Remove("Time")
                End If


                BindGrid()
                lblMsg.Text = String.Empty
                If Request.QueryString("pi") <> Nothing Then
                    If DataGridCourseDetails.Items.Count > 0 Then
                        DataGridCourseDetails.Visible = True
                        DivGrid.Visible = True
                    End If
                End If
                'Added by Pranit on 05/11/2019
                'DataGridCourseDetails.DataSource = BindGrid()
                'DataGridCourseDetails.DataBind()
            Else
                '   BindGrid()
            End If

            If DataGridCourseDetails.Visible = True Then
                fillPageNumbers(DataGridCourseDetails.CurrentPageIndex + 1, 9)
            End If

            If Session.Item("updatecheck") = "true" Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CourseMaintanance_crsupdted
                lblMsg.ForeColor = Drawing.Color.Green
                Session.Item("updatecheck") = Nothing
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

    'Added by Pranit on 02/12/2019
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

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To fill the combo box for Course
    '********************************************************************
    Public Sub FillCoursesCombo()
        Dim objDataReader1 As SqlDataReader
        ddlCourses.Items.Clear()
        Dim l1 As New ListItem
        l1.Text = "---- Select ----"
        l1.Value = 0
        ddlCourses.Items.Add(l1)
        Dim query As String = ""
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try
            If objconn.connect() = True Then
                query = "select Course_ID,Course_Name from M_Course where main_course_id='" & ddlMainCourse.SelectedItem.Value & "' and Description='" & ddlSectionDes.SelectedItem.Value & "' and Del_Flag = 0 order by Course_Name "
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objDataReader1 = objCommand.ExecuteReader()
                While objDataReader1.Read()
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader1.Item(1)
                    lstItm.Value = objDataReader1.Item(0)
                    ddlCourses.Items.Add(lstItm)
                End While
                objDataReader1.Close()
                objconn.disconnect()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            l1 = Nothing
            query = Nothing
            objDataReader1 = Nothing
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Clear Controls
    '********************************************************************
    Public Sub ClearControls()
        Try
            txtTotalTime.Text = String.Empty
            ddlCourses.SelectedIndex = 0
            ddlMainCourse.SelectedIndex = 0
            lblMsg.Visible = False
            ddlCourses.Items.Clear()
            Dim l1 As New ListItem
            l1.Text = "---- Select ----"
            l1.Value = 0
            ddlCourses.Items.Add(l1)
            DivGrid.Visible = False
            DataGridCourseDetails.Visible = False
            lblrecords.Text = String.Empty
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: Bind Grid
    '********************************************************************
    Public Function BindGrid()
        Dim query As String = ""
        Dim sb As New StringBuilder()
        Dim dt As New DataTable()
        'strPathDb = ConfigurationManager.AppSettings("PathDb")

        Try
            'Added by Pranit on 05/11/2019
            'Session("Source") = dt
            'Dim dv As DataView = New DataView(dt)
            If objconn.connect() = True Then

                'Dim fields As String = " Distinct Course_ID,Course_Name,Total_time"
                'sb.Append("Select " & fields & " From m_course where ")
                'If (ddlMainCourse.SelectedItem.Value <> 0) Then
                '    sb.Append("main_Course_id='" & ddlMainCourse.SelectedItem.Value & "'")
                '    sb.Append(" AND ")
                'End If
                'If (ddlCourses.SelectedItem.Value <> 0) Then
                '    sb.Append("Course_id='" & ddlCourses.SelectedItem.Value & "'")
                '    sb.Append(" AND ")
                'End If

                'Dim fields As String = " Distinct Course_ID,Course_Name,Total_time,del_flag"
                'sb.Append("Select " & fields & " From m_course  ")
                'If (ddlMainCourse.SelectedItem.Value <> 0 Or ddlCourses.SelectedItem.Value <> 0 Or txtTotalTime.Text <> String.Empty) Then
                '    sb.Append(" where ")
                'End If

                'If (ddlMainCourse.SelectedItem.Value <> 0) Then
                '    sb.Append("main_Course_id='" & ddlMainCourse.SelectedItem.Value & "'")
                'End If

                'If (ddlCourses.SelectedItem.Value <> 0) Then
                '    If (ddlMainCourse.SelectedItem.Value <> 0) Then
                '        sb.Append(" AND ")
                '    End If
                '    sb.Append("Course_id='" & ddlCourses.SelectedItem.Value & "'")
                'End If

                'If (txtTotalTime.Text <> String.Empty) Then
                '    Dim totalTimeValue As Boolean = ValidationForNumber(txtTotalTime.Text)
                '    If (totalTimeValue = False) Then
                '        lblMsg.Visible = True
                '        lblMsg.Text = "Please Enter Numbers In Total Alloted Exam Time(Minute)."
                '        txtTotalTime.Focus()
                '        DivGrid.Visible = False
                '        DataGridCourseDetails.Visible = False
                '        Exit Sub
                '    End If
                '    If (ddlMainCourse.SelectedItem.Value <> 0 Or ddlCourses.SelectedItem.Value <> 0) Then
                '        sb.Append(" AND ")
                '    End If
                '    sb.Append(" total_time = '" & txtTotalTime.Text & "' ")
                'End If
                'sb.Append("del_flag='0'")
                'query = sb.ToString()

                'Session.Add("mainCourse", lnkEditID)
                'Session.Add("CourseName", lnkEditID)
                'Session.Add("Time", lnkEditID)

                'If Not Session("mainCourse") Is Nothing Then
                '    ddlMainCourse.SelectedValue = CInt(Session("mainCourse"))
                '    Session.Remove("mainCourse")
                'End If
                'If Not Session("CourseName") Is Nothing Then
                '    ddlCourses.SelectedValue = CInt(Session("CourseName"))
                '    Session.Remove("CourseName")
                'End If
                'If Not Session("Time") Is Nothing Then
                '    txtTotalTime.Text = Session("Time").ToString
                '    Session.Remove("Time")
                'End If


                Dim fields As String = " Distinct Course_ID,Course_Name,Total_time,Total_Marks,Total_passmarks_Per,del_flag"
                sb.Append("Select " & fields & " From m_course  ")
                If (ddlMainCourse.SelectedItem.Value <> 0 Or ddlCourses.SelectedItem.Value <> 0 Or txtTotalTime.Text <> String.Empty) Then
                    sb.Append(" where ")
                End If

                If (ddlMainCourse.SelectedItem.Value <> 0) Then
                    sb.Append("main_Course_id='" & ddlMainCourse.SelectedItem.Value & "'")
                End If

                If (ddlSectionDes.SelectedItem.Value <> "") Then
                    If (ddlMainCourse.SelectedItem.Value <> 0) Then
                        sb.Append(" AND ")
                    End If
                    sb.Append("Description='" & ddlSectionDes.SelectedItem.Value & "'")
                End If

                If (ddlCourses.SelectedItem.Value <> 0) Then
                    If (ddlSectionDes.SelectedItem.Value <> 0) Then
                        sb.Append(" AND ")
                    End If
                    sb.Append("Course_id='" & ddlCourses.SelectedItem.Value & "'")
                End If

                If (txtTotalTime.Text <> String.Empty) Then
                    Dim totalTimeValue As Boolean = ValidationForNumber(txtTotalTime.Text)
                    If (totalTimeValue = False) Then
                        lblMsg.Visible = True
                        lblMsg.Text = Resources.Resource.CourseMaintananace_totalalltedtime
                        txtTotalTime.Focus()
                        DivGrid.Visible = False
                        DataGridCourseDetails.Visible = False
                        Exit Function
                    End If
                    If (ddlMainCourse.SelectedItem.Value <> 0 Or ddlCourses.SelectedItem.Value <> 0) Then
                        sb.Append(" AND ")
                    End If
                    sb.Append(" total_time = '" & txtTotalTime.Text & "' ")
                End If
                'sb.Append("del_flag='0'")
                sb.Append("order by Course_Name")
                query = sb.ToString()
                Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
                adp.Fill(dt)
                If (dt.Rows.Count > 0) Then
                    DataGridCourseDetails.DataSource = dt

                    'If Session("fromcourseregi") = "true" Then
                    '    If Session("pageindexcourse") <> Nothing Then
                    '        DataGridCourseDetails.CurrentPageIndex = CInt(Session("pageindexcourse").ToString())
                    '    End If
                    'End If

                    If Session("fromcourseregi") = "true" Then
                        If Request.QueryString("pi") <> Nothing Then
                            DataGridCourseDetails.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                            ViewState.Add("selval", DataGridCourseDetails.CurrentPageIndex)
                        End If
                        Session.Remove("fromcourseregi")
                    End If

                    If ViewState("fromsearch") = "true" Then
                        DataGridCourseDetails.CurrentPageIndex = 0
                        ViewState.Remove("fromsearch")
                    End If

                    DataGridCourseDetails.DataBind()
                    'fillPagesCombo()
                    fillPageNumbers(DataGridCourseDetails.CurrentPageIndex + 1, 9)


                    For i As Integer = 0 To DataGridCourseDetails.Items.Count - 1
                        If DataGridCourseDetails.Items(i).Cells(6).Text = True Then
                            DataGridCourseDetails.Items(i).Cells(4).Attributes.Remove("href")
                            DataGridCourseDetails.Items(i).Cells(4).Attributes.Remove("className")
                            DataGridCourseDetails.Items(i).Cells(4).Attributes.Add("onclick", "return false")
                            DataGridCourseDetails.Items(i).Cells(7).Enabled = False
                            DataGridCourseDetails.Items(i).Cells(7).ToolTip = "Disabled"
                            DataGridCourseDetails.Items(i).BackColor = Drawing.Color.Gray
                        ElseIf DataGridCourseDetails.Items(i).Cells(6).Text = False Then
                            DataGridCourseDetails.Items(i).Enabled = True
                        End If
                    Next
                    lblrecords.Text = Resources.Resource.AdminList_TotRecord & ": " & dt.Rows.Count
                    DivGrid.Style.Item("heigth") = "40px"
                Else
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.Common_NoRecFound
                    DataGridCourseDetails.Visible = False
                    DivGrid.Visible = False
                End If

            End If
            'Added by Pranit on 05/11/2019
            'Return dv
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

    'Added by Pranit on 05/11/2019
#Region "Sorting"
    'Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '    Dim dt As DataTable = CType(Session("Source"), DataTable)
    '    Dim dv As DataView = New DataView(dt)
    '    dv.Sort = e.SortExpression
    '    DataGridCourseDetails.DataSource = dv
    '    DataGridCourseDetails.DataBind()
    'End Sub 'Sort_Grid
#End Region

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: enableDisable
    '********************************************************************
    Public Function enableDisable(ByVal flg As Boolean, ByVal blflg As Boolean)
        Dim chk As New CheckBox
        Dim strid As String = ""
        Dim q As String = String.Empty
        Dim cid As String
        Dim bolflg As Boolean = True
        Try
            For Each rowItem As DataGridItem In DataGridCourseDetails.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                If chk.Checked Then
                    bolflg = False
                    If objconn.connect() = True Then
                        cid = rowItem.Cells(1).Text
                        If (flg = True) Then
                            q = "update M_course set del_flag=1 where Course_Id =" & cid & ""
                        Else
                            q = "update M_course set del_flag=0 where Course_Id =" & cid & ""
                        End If
                        Dim cmd As New SqlCommand(q, objconn.MyConnection)

                        If blflg = True Then
                            cmd.ExecuteNonQuery()
                        Else
                            Dim booldecision As Boolean = CheckStatus(cid)
                            If booldecision = True Then
                                'lblMsg.Text = String.Empty
                                ' lblMsg.Visible = False
                                cmd.ExecuteNonQuery()
                            Else
                                lblMsg.Visible = True
                                lblMsg.ForeColor = Drawing.Color.Red
                                lblMsg.Text = Resources.Resource.CourseMaintanance_crsdisable
                            End If
                        End If
                    End If
                End If
            Next
            If bolflg = True Then
                If blflg = False Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CenterMaintainance_sltchkdisable
                ElseIf blflg = True Then
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.CenterMaintainance_sltchk
                End If
                ' Else
                'lblMsg.Visible = False
            End If
            Dim intindex As Integer = DataGridCourseDetails.CurrentPageIndex

            BindGrid()

            DataGridCourseDetails.CurrentPageIndex = intindex
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally
            objconn.disconnect()
            chk = Nothing
            strid = Nothing
            q = Nothing
            cid = Nothing
            bolflg = Nothing
        End Try

    End Function
#Region "Function checkstatus"

    'Added By: Saraswati Patel
    'Description: This function checks the status of course before disabling a question.

    Private Function CheckStatus(ByVal CourseID As Integer) As Boolean
        Dim strq As String
        Dim strbr As StringBuilder
        Dim MyCommand As SqlCommand
        Dim bol As Boolean = True
        Dim intcount As Integer
        Try
            strbr = New StringBuilder
            strbr.Append("select count(*) from T_Candidate_Status where course_id=  ")
            strbr.Append(CourseID)
            strbr.Append(" and Appearedflag in (0,1) ")
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
    '********************************************************************
    'Code added by Monal Shah
    'Purpose: chkSelectAll_CheckedChanged
    '********************************************************************
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox = Nothing
        Try

            For Each rowItem As DataGridItem In DataGridCourseDetails.Items

                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

                chk.Checked = DirectCast(sender, CheckBox).Checked

            Next
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally
            chk = Nothing
        End Try

    End Sub


    'Protected Sub btnNewCourseRegistration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewCourseRegistration.Click
    '    Response.Redirect("CourseRegistration.aspx", False)
    'End Sub
    Protected Sub DataGridCourseDetails_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridCourseDetails.ItemCommand
        Try
            If e.CommandName = "lnkEdit" Then
                lnkEditID = e.CommandArgument
                Session.Add("CourseIDValue", lnkEditID)

                Session.Add("mainCourse", ddlMainCourse.SelectedItem.Value)
                Session.Add("CourseName", ddlCourses.SelectedItem.Value)
                Session.Add("Time", txtTotalTime.Text)

                Response.Redirect("CourseRegistration.aspx?pi=" & DataGridCourseDetails.CurrentPageIndex, False)
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    Protected Sub ddlMainCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlMainCourse.SelectedIndexChanged
        Try
            FillSectionCourseCombo()
            DivGrid.Visible = False
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    'Added by Pranit on 02/12/2019
    Protected Sub ddlSectionCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSectionDes.SelectedIndexChanged
        Try
            FillCoursesCombo()
            DivGrid.Visible = False
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    Protected Sub ImgBtnBack_Click(sender As Object, e As EventArgs) Handles ImgBtnBack.Click
        Response.Redirect("admin.aspx", False)
    End Sub

    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        Try
            ClearControls()
            DivGrid.Visible = False
            DataGridCourseDetails.Visible = False
            ViewState.Remove("selval")
            ViewState.Remove("pageNo")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    Protected Sub imgBtnSearch_Click(sender As Object, e As EventArgs) Handles imgBtnSearch.Click
        Try
            lblMsg.Visible = False
            DataGridCourseDetails.Visible = True
            DivGrid.Visible = True
            DataGridCourseDetails.CurrentPageIndex = 0
            ViewState.Add("fromsearch", "true")
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    'Added by Pranit on 02/12/2019
    Sub Selection_Change(sender As Object, e As EventArgs)
        Try
            DataGridCourseDetails.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

    Protected Sub DataGridCourseDetails_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridCourseDetails.PageIndexChanged
        Try            
            DataGridCourseDetails.CurrentPageIndex = e.NewPageIndex
            'If Session("pageindexcourse") <> Nothing Then
            '    Session.Remove("pageindexcourse")
            'End If
            BindGrid()
            'Session.Add("pageindexcourse", e.NewPageIndex)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    Protected Sub imgBtnNewCourseRegistration_Click(sender As Object, e As EventArgs) Handles imgBtnNewCourseRegistration.Click
        Session.Add("fornew", "true")
        Response.Redirect("CourseRegistration.aspx", False)
    End Sub

    Protected Sub DataGridCourseDetails_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridCourseDetails.ItemDataBound
        If Not e.Item.ItemType = DataControlRowType.Header Then
            e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
        End If
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


        If len >= DataGridCourseDetails.PageCount Then
            len = DataGridCourseDetails.PageCount - 1
        End If

        ' if search clicked again then page 1 should be selected 
        If DataGridCourseDetails.CurrentPageIndex = 0 Then
            ViewState("pageNo") = 1
            ViewState("lastRange") = 1
        End If

        ' Getting the currently selected page value 
        Dim selPage As Integer = 0
        If (ViewState("pageNo") <> Nothing) Then
            selPage = CInt(ViewState("pageNo"))
        Else
            ' selPage = 1
            selPage = DataGridCourseDetails.CurrentPageIndex + 1
        End If

        If (ViewState("lastRange") <> Nothing) Then

            '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DataGridCourseDetails.PageCount Then
            If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
                range = CInt(ViewState("lastRange"))
            Else
                'If it is the last page then resetting the page numbers
                ' last number - provided length
                'If (len + selPage) >= DataGridCourseDetails.PageCount Then
                '    If selPage <= len Then
                '        range = range
                '    Else
                '        range = DataGridCourseDetails.PageCount - len
                '        'Incase range becomes 0 or less than zero than setting it 1 
                '        If range <= 0 Then
                '            range = 1
                '        End If
                '    End If

                'Else
                If selPage <= DataGridCourseDetails.PageCount Then
                    'range = range
                    If range < CInt(ViewState("lastRange")) Then
                        range = CInt(ViewState("lastRange")) - 1
                    Else
                        If selPage - len > 0 And selPage - len <= DataGridCourseDetails.PageCount - len Then
                            range = selPage - len
                        Else
                            range = CInt(ViewState("lastRange")) + 1
                        End If
                        ' range = CInt(ViewState("lastRange")) + 1
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
        'If selPage = 1 And selPage = DataGridCourseDetails.PageCount - 1 Then
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
        If selPage = DataGridCourseDetails.PageCount Then
            imgnext.Enabled = False
            imglast.Enabled = False
        End If
    End Sub
    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGridCourseDetails.CurrentPageIndex < (DataGridCourseDetails.PageCount - 1)) Then
                    DataGridCourseDetails.CurrentPageIndex += 1

                End If

            Case "prev" 'The prev button was clicked
                If (DataGridCourseDetails.CurrentPageIndex > 0) Then
                    DataGridCourseDetails.CurrentPageIndex -= 1
                End If

            Case "last" 'The Last Page button was clicked
                DataGridCourseDetails.CurrentPageIndex = (DataGridCourseDetails.PageCount - 1)

            Case Else 'The First Page button was clicked
                DataGridCourseDetails.CurrentPageIndex = Convert.ToInt32(arg)
        End Select
        ViewState.Add("pageNo", DataGridCourseDetails.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGridCourseDetails.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGridCourseDetails.CurrentPageIndex < (DataGridCourseDetails.PageCount - 1)) Then
                    DataGridCourseDetails.CurrentPageIndex += 1
                    '    ViewState.Add("selval", DataGridCourseDetails.CurrentPageIndex)
                End If

            Case "prev" 'The prev button was clicked
                If (DataGridCourseDetails.CurrentPageIndex > 0) Then
                    DataGridCourseDetails.CurrentPageIndex -= 1
                    '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                End If

            Case "last" 'The Last Page button was clicked
                DataGridCourseDetails.CurrentPageIndex = (DataGridCourseDetails.PageCount - 1)
                'ViewState.Add("selval", ddlPages.SelectedItem.Value)
            Case Else 'The First Page button was clicked
                DataGridCourseDetails.CurrentPageIndex = Convert.ToInt32(arg) - 1
                ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
        End Select

        ViewState.Add("pageNo", DataGridCourseDetails.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGridCourseDetails.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    'Public Sub fillPagesCombo()
    '    ddlPages.Items.Clear()
    '    For cnt As Integer = 1 To DataGridCourseDetails.PageCount
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
    '    DataGridCourseDetails.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    BindGrid()
    'End Sub

    'Add by      : Saraswati Patel
    'Description : Delete course from the list
    'Date by     : 2012/07/24
    Protected Sub imgBtnDelete_Click(sender As Object, e As EventArgs) Handles imgBtnDelete.Click
        DeleteCourse()
        BindGrid()
        DivGrid.Visible = True
    End Sub

    'Add by      : Saraswati Patel
    'Description : Delete course from the list
    'Date by     : 2012/07/24
    Public Sub DeleteCourse()
        Dim chk As New CheckBox
        Dim strQuery As String = String.Empty
        Dim cid As String
        Dim bolflg As Boolean = True
        Dim SqlTran As SqlTransaction
        Dim cmd As SqlCommand
        Try
            For Each rowItem As DataGridItem In DataGridCourseDetails.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                'sqlcon = ConfigurationSettings.AppSettings("PathDb")
                con = New SqlConnection(strPathDb)
                If chk.Checked Then
                    bolflg = False
                    openconnection()
                    SqlTran = con.BeginTransaction()
                    cid = rowItem.Cells(1).Text
                    Dim booldecision As Boolean = CheckStatus(cid)
                    If booldecision = True Then
                        'Delete course from T_Candidate_Status
                        strQuery = "Delete from T_Candidate_Status where Course_Id= " + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        'Delete course from T_Result
                        strQuery = "Delete from T_Result where Course_Id=" + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        'Delete course from T_Center_Course
                        strQuery = "Delete from T_Center_Course where Course_Id=" + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        'Delete course from T_User_Course
                        strQuery = "Delete from T_User_Course  where Course_Id=" + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        'Delete course from Student_Time_info
                        strQuery = "Delete from Student_Time_info  where Course_Id=" + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        'Delete course from M_Weightage
                        strQuery = "Delete from  M_Weightage  where Course_Id=" + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        'Delete course from M_Course
                        strQuery = "Delete from M_Course  where Course_Id=" + cid
                        cmd = New SqlCommand(strQuery, con, SqlTran)
                        cmd.ExecuteNonQuery()

                        SqlTran.Commit()
                        lblMsg.Text = Resources.Resource.CourseMaintanance_crsdel
                        lblMsg.ForeColor = Drawing.Color.Green
                        lblMsg.Visible = True
                        ddlCourses.SelectedIndex = 0
                        FillCoursesCombo()
                        DivGrid.Visible = False
                    Else
                        lblMsg.Visible = True
                        lblMsg.ForeColor = Drawing.Color.Red
                        lblMsg.Text = Resources.Resource.CourseMaintanance_delcrsass
                    End If
                   
                End If

            Next
            If bolflg = True Then
                lblMsg.Visible = True
                lblMsg.ForeColor = Drawing.Color.Red
                lblMsg.Text = Resources.Resource.CourseMaintananace_delcrs
            End If
            ' Dim intindex As Integer = DataGridCourseDetails.CurrentPageIndex
            '  DataGridCourseDetails.CurrentPageIndex = intindex
        Catch ex As Exception
            SqlTran.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally
            chk = Nothing
            strQuery = Nothing
            cid = Nothing
            bolflg = Nothing
            closeconnection()
        End Try


    End Sub


#Region "Open Connection"
    Public Sub openconnection()
        Try
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub
#End Region

#Region "Close connection"
    Public Sub closeconnection()
        Try
            If con.State = ConnectionState.Open Then
                con.Close()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub
#End Region
End Class