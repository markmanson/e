Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Public Class ManageCourse
    Inherits BasePage

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Managecourse")
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Try
                FillCenters()
                FillCourses()
                'Added by Pranit on 06/11/2019
                'DataGridCourseDetails.DataSource = BindGrid()
                'DataGridCourseDetails.DataBind()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        Else
            '  BindGrid()
        End If

        If DataGridCourseDetails.Visible = True Then
            fillPageNumbers(DataGridCourseDetails.CurrentPageIndex + 1, 9)
        End If
    End Sub

    '********************************************************************
    'Code added by Indravadan Vasava
    'Purpose: Fill Centres Combobox
    '********************************************************************
    Public Sub FillCenters()
        Dim objDataReader1 As SqlDataReader
        Dim query As String = ""
        Try
            ddlCenters.Items.Clear()
            Dim lst1 As New ListItem
            lst1.Text = "--------All--------"
            lst1.Value = 0
            ddlCenters.Items.Add(lst1)
            If objconn.connect() = True Then
                query = "select Center_ID,Center_Name  from  M_Centers  where del_flg=0 order by Center_Name"
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objDataReader1 = objCommand.ExecuteReader()
                While objDataReader1.Read()
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader1.Item(1)
                    lstItm.Value = objDataReader1.Item(0)
                    ddlCenters.Items.Add(lstItm)
                End While
                objDataReader1.Close()
                objconn.disconnect()
            End If
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
    'Code added by Indravadan Vasava
    'Purpose: Fill Courses Combobox
    '********************************************************************
    Public Sub FillCourses()
        Dim objDataReader1 As SqlDataReader
        Dim query As String = ""
        Try
            ddlCourses.Items.Clear()
            Dim lst1 As New ListItem
            lst1.Text = "--------All--------"
            lst1.Value = 0
            ddlCourses.Items.Add(lst1)
            If objconn.connect() = True Then
                query = "select Course_ID,Course_Name from M_Course where  del_flag='0' Order by Course_Name"
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
    'Code added by Indravadan Vasava
    'Purpose: Bind Grid
    '********************************************************************
    'Modified by Pranit on 06/11/2019
    Public Function BindGrid()
        Dim query As String = ""
        Dim sb As New StringBuilder()
        Dim dt As New DataTable()
        'strPathDb = ConfigurationManager.AppSettings("PathDb")

        Try
            'Added by Pranit on 06/11/2019
            'Session("Source") = dt
            'Dim dv As DataView = New DataView(dt)
            If objconn.connect() = True Then

                sb.Append(" select MCO.Course_ID,MCO.course_name,MCE.Center_ID,MCE.Center_Name,TCE.Del_Flag As del_flag  from M_Course as MCO ")
                sb.Append(" left join T_Center_Course as TCE on  MCO.Course_id = TCE.Course_id  ")
                sb.Append(" join M_Centers as MCE on MCE.Center_ID=TCE.Center_ID where ")
                'added by bhumi [16/9/2015]
                'Reason: where condition changed, Display only Enable Courses and Classes
                sb.Append(" MCO.Del_Flag=0 and MCE.Del_Flg=0 ")
                'Ended by bhumi
                sb.Append("  ")

                'If (Trim(txtCenterName.Text) <> String.Empty) Then
                '    sb.Append("  and MCE.Center_Name like  '" & txtCenterName.Text & "%' ")
                'End If
                If ddlCenters.SelectedValue <> 0 Then
                    'Modified By Bhumi [28/8/2015]
                    'Reason:Like clause removed because Not Giving Accurate Result while Centers names are N4R-I & N4R-II
                    sb.Append("  and MCE.Center_Name=  '" & ddlCenters.SelectedItem.Text & "' ")
                    'Ended by bhumi
                End If

                If ddlCourses.SelectedValue <> 0 Then 'Or ddlCenters.SelectedItem.Value <> 0) Then
                    sb.Append(" and MCO.course_name like '" & ddlCourses.SelectedItem.Text & "%' ")
                End If

                'If (Trim(txtCourseName.Text) <> String.Empty) Then 'Or ddlCenters.SelectedItem.Value <> 0) Then
                '    sb.Append(" and MCO.course_name like '" & txtCourseName.Text & "%' ")
                'End If


                sb.Append("order by  MCO.course_name")
                query = sb.ToString()
                Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
                adp.Fill(dt)
                If (dt.Rows.Count > 0) Then
                    DataGridCourseDetails.DataSource = dt


                    ' To maintin last page index
                    ''If Session("fromcourseregi") = "true" Then
                    ''    If Request.QueryString("pi") <> Nothing Then
                    ''        DataGridCourseDetails.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                    ''    End If
                    ''    Session.Remove("fromcourseregi")
                    ''End If

                    ''If ViewState("fromsearch") = "true" Then
                    ''    DataGridCourseDetails.CurrentPageIndex = 0
                    ''    ViewState.Remove("fromsearch")
                    ''End If

                    Try
                        DataGridCourseDetails.DataBind()
                    Catch ex As Exception
                        DataGridCourseDetails.CurrentPageIndex = 0
                        DataGridCourseDetails.DataBind()
                    End Try
                    'fillPagesCombo()
                    fillPageNumbers(DataGridCourseDetails.CurrentPageIndex + 1, 9)


                    For i As Integer = 0 To DataGridCourseDetails.Items.Count - 1
                        If DataGridCourseDetails.Items(i).Cells(5).Text = True Then
                            'DataGridCourseDetails.Items(i).Cells(4).Attributes.Remove("href")
                            'DataGridCourseDetails.Items(i).Cells(4).Attributes.Remove("className")
                            'DataGridCourseDetails.Items(i).Cells(4).Attributes.Add("onclick", "return false")
                            'DataGridCourseDetails.Items(i).Cells(7).Enabled = False
                            'DataGridCourseDetails.Items(i).Cells(7).ToolTip = "Disabled"
                            DataGridCourseDetails.Items(i).BackColor = Drawing.Color.Gray
                        ElseIf DataGridCourseDetails.Items(i).Cells(5).Text = False Then
                            DataGridCourseDetails.Items(i).Enabled = True
                        End If
                    Next
                    lblrecords.Text = ": " & dt.Rows.Count
                    DivGrid.Style.Item("heigth") = "40px"
                    lblMsg.Text = ""
                    lblMsg.Visible = False
                    DivGrid.Visible = True
                    DataGridCourseDetails.Visible = True
                Else
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.Common_NoRecFound
                    DataGridCourseDetails.Visible = False
                    DivGrid.Visible = False
                End If

            End If
            'Added by Pranit on 06/11/2019
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

    'Added by Pranit on 06/11/2019
#Region "Sorting"
    'Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '    Dim dt As DataTable = CType(Session("Source"), DataTable)
    '    Dim dv As DataView = New DataView(dt)
    '    dv.Sort = e.SortExpression
    '    DataGridCourseDetails.DataSource = dv
    '    DataGridCourseDetails.DataBind()
    'End Sub 'Sort_Grid
#End Region

    Protected Sub imgBtnSearch_Click(sender As Object, e As EventArgs) Handles imgBtnSearch.Click
        DataGridCourseDetails.CurrentPageIndex = 0
        ViewState.Remove("selval")
        ViewState.Remove("pageNo")
        BindGrid()
        Try

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Manage Course ", ex)
                Response.Redirect("error.aspx", False)
            End If
        Finally

        End Try
    End Sub

    Private Sub DataGridCourseDetails_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridCourseDetails.PageIndexChanged
        Try
            DataGridCourseDetails.CurrentPageIndex = e.NewPageIndex
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

#Region "Enable Disable Function"
    'Desc: This is method to enable and disable candidates.
    'By: Indravadan Vasava, 2011/05/11

    Public Function EnableDisable(ByVal i As Integer, ByVal bool As Boolean)
        Dim chk As CheckBox = Nothing
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
        Dim strbr As StringBuilder
        Dim Str, strquery As String
        Dim strid As String
        Dim boldecision As Boolean = True
        Dim Ar As ArrayList
        Try
            Ar = New ArrayList
            strbr = New StringBuilder
            ' strbr.Append(" ( ")
            For Each rowItem As DataGridItem In DataGridCourseDetails.Items
                chk = DirectCast((rowItem.Cells(6).FindControl("chkRemove")), CheckBox)
                If chk.Checked = True Then
                    boldecision = False
                    strid = DirectCast(rowItem.Cells(1).Text, String) + "," + DirectCast(rowItem.Cells(2).Text, String)
                    Ar.Add(strid)
                    '  strbr.Append(strid)
                    '  strbr.Append(" , ")
                End If
            Next

            If boldecision = True Then
                lblMsg.Visible = True
                If bool = True Then
                    lblMsg.Text = Resources.Resource.CourseRegistration_sltoneade
                ElseIf bool = False Then
                    lblMsg.Text = Resources.Resource.CourseRegistration_sltadDis
                End If

                Exit Function
            End If

            'Str = strbr.ToString
            'Str = Str.Substring(0, Str.Length - 2)
            'Str = Str & " ) "

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                strbr = New StringBuilder
                Dim keys() As String
                For j As Integer = 0 To Ar.Count - 1
                    keys = Ar(j).ToString.Split(",")
                    strbr.Append(" Update T_Center_Course set Del_Flag = ")
                    strbr.Append(i)
                    strbr.Append(" where  Center_id=" & keys(0) & " and Course_id=" & keys(1))
                    'strbr.Append(Str)
                    strquery = strbr.ToString()
                    myCommand = New SqlCommand(strquery, objconn.MyConnection)
                    myCommand.ExecuteNonQuery()
                    strbr.Remove(0, strbr.Length)
                Next


            End If
            objconn.disconnect()
            Dim intindex As Integer = DataGridCourseDetails.CurrentPageIndex
            BindGrid()
            DataGridCourseDetails.CurrentPageIndex = intindex

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            chk = Nothing
            myCommand = Nothing
            strPathDb = Nothing
            strid = Nothing
            strquery = Nothing
            strquery = Nothing
            Str = Nothing
            boldecision = True
            objconn = Nothing
            strbr = Nothing
        End Try

    End Function

#End Region

    '********************************************************************
    'Code added by Indravadan Vasava
    'Purpose: chkSelectAll_CheckedChanged
    '********************************************************************
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox = Nothing
        '   Dim chkbx As CheckBox = DirectCast(sender, CheckBox)
        Try


            For Each rowItem As DataGridItem In DataGridCourseDetails.Items

                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

                '    If chkbx.Checked = True Then
                chk.Checked = DirectCast(sender, CheckBox).Checked
                '    Else
                '    chk.Checked = False
                '    End If
            Next
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx?ex=" & ex.ToString, False)
        Finally
            chk = Nothing
        End Try

    End Sub

    Protected Sub imgBtnEnable_Click(sender As Object, e As EventArgs) Handles imgBtnEnable.Click
        EnableDisable(0, True)
    End Sub

    Protected Sub imgBtnDisable_Click(sender As Object, e As EventArgs) Handles imgBtnDisable.Click
        EnableDisable(1, False)
    End Sub

    Protected Sub imgBtnNewCourse_Click(sender As Object, e As EventArgs) Handles imgBtnNewCourse.Click
        Try
            Response.Redirect("AddCourse.aspx", False)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Private Sub ImgBtnBack_Click(sender As Object, e As EventArgs) Handles ImgBtnBack.Click
        Response.Redirect("admin.aspx")
    End Sub

    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        txtCenterName.Text = ""
        ddlCenters.SelectedValue = 0
        ddlCourses.SelectedValue = 0
        txtCourseName.Text = ""
        DivGrid.Visible = False
        lblMsg.Visible = False
        DataGridCourseDetails.CurrentPageIndex = 0
        ViewState.Remove("selval")
        ViewState.Remove("pageNo")
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
                        '  range = CInt(ViewState("lastRange")) + 1
                        If selPage - len > 0 And selPage - len <= DataGridCourseDetails.PageCount - len Then
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

    'Added by Pranit on 28/11/2019
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

    Protected Sub ddlCenters_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCenters.SelectedIndexChanged

    End Sub
End Class