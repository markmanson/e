Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing

Partial Public Class CenterMaintenance
    Inherits BasePage
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("CourseMaintenance")
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader             'SqlDataReader object
    Dim flg As Boolean = True
    Dim lnkEditID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'txtContactNo.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")

            '/******************Jatin Gangajaliya,2011/03/30****************/
            If Not IsPostBack Then

                DataGridCenterDetails.Visible = False
                imgBtnDisable.Visible = False
                imgBtnEnable.Visible = False
                imgBtnDelete.Visible = False
                lblMsg.Visible = False
                DivGrid.Visible = False


                If Not Session("oldVal") Is Nothing Then
                    DivGrid.Visible = True
                    DataGridCenterDetails.Visible = True
                    imgBtnDisable.Visible = True
                    imgBtnEnable.Visible = True
                    imgBtnDelete.Visible = True

                    lblMsg.Visible = True
                End If

                BindGrid()
                lblMsg.Text = String.Empty
                If Request.QueryString("pi") <> Nothing Then
                    If DataGridCenterDetails.Items.Count > 0 Then
                        DivGrid.Visible = True
                        DataGridCenterDetails.Visible = True
                        imgBtnDisable.Visible = True
                        imgBtnEnable.Visible = True
                        imgBtnDelete.Visible = True
                        lblMsg.Visible = True
                        'Session.Remove("fromcentreupdate")
                        'Session.Remove("pageindex")
                    End If
                End If
                'Added by Pranit on 05/11/2019
                'DataGridCenterDetails.DataSource = BindGrid()
                'DataGridCenterDetails.DataBind()
            Else
                '    BindGrid()
            End If

            If DataGridCenterDetails.Visible = True Then
                fillPageNumbers(DataGridCenterDetails.CurrentPageIndex + 1, 9)
            End If

            '/*****************************End*****************************/

            If Session.Item("updatecheck") = "true" Then
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.CenterRegistration_Clsupdt
                lblMsg.ForeColor = Drawing.Color.Green
                Session.Item("updatecheck") = Nothing
            End If
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


    Protected Sub imgBtnSearch_Click(sender As Object, e As EventArgs) Handles imgBtnSearch.Click
        Try
            lblMsg.Visible = False
            DataGridCenterDetails.Visible = True
            DivGrid.Visible = True
            DataGridCenterDetails.CurrentPageIndex = 0
            Session.Remove("fromcentreupdate")
            BindGrid()
            If DivGrid.Visible = True Then
                imgBtnDisable.Visible = True
                imgBtnEnable.Visible = True
                imgBtnDelete.Visible = True
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        Try
            ClearControls()
            imgBtnDisable.Visible = False
            imgBtnEnable.Visible = False
            imgBtnDelete.Visible = False
            DivGrid.Visible = False
            DataGridCenterDetails.Visible = False
            ViewState.Remove("selval")
            ViewState.Remove("pageNo")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    'Protected Sub ImgBtnBack_Click(sender As Object, e As EventArgs) Handles ImgBtnBack.Click
    '    Response.Redirect("admin.aspx", False)
    'End Sub

    Protected Sub imgBtnNewCentreRegistration_Click(sender As Object, e As EventArgs) Handles imgBtnNewCentreRegistration.Click
        Try
            Session.Remove("CenterId")
            Session.Remove("CenterIDValue")
            Response.Redirect("CentreRegistration.aspx", False)
            Session.Add("fornewcenter", "true")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
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
    'Purpose: Bind Grid
    '********************************************************************
    'Modified by Pranit on 05/11/2019
    Public Sub BindGrid()
        Dim query As String = ""
        Dim sb As New StringBuilder()
        Dim dt As New DataTable()
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Try
            'Added by Pranit on 05/11/2019
            'Session("Source") = dt
            'Dim dv As DataView = New DataView(dt)

            If objconn.connect() = True Then

                If Not Session("oldVal") Is Nothing Then
                    Dim ht As Hashtable = DirectCast(Session("oldVal"), Hashtable)

                    If ht("name") <> "" Then
                        txtCenterName.Text = ht("name")
                    End If
                    If ht("code") <> "" Then
                        txtCenterCode.Text = ht("code")
                    End If
                    If ht("owner") <> "" Then
                        txtOwnerName.Text = ht("owner")
                    End If
                    If ht("city") <> "" Then
                        txtCity.Text = ht("city")
                    End If
                    'If ht("state") <> "" Then
                    '    txtstate.Text = ht("state")
                    'End If
                    'If ht("country") <> "" Then
                    '    txtCountry.Text = ht("country")
                    'End If
                    'If ht("contact") <> "" Then
                    '    txtContactNo.Text = ht("contact")
                    'End If

                    ht = Nothing
                    Session.Remove("oldVal")
                End If

                Dim fields As String = " Distinct Center_ID,Center_Name,Center_Code,Owner_Name,City_Name,State_Name,Country_Name,Contact_No,del_flg"
                sb.Append("Select " & fields & " From M_Centers  ")
                If (txtCenterName.Text <> String.Empty Or txtCenterCode.Text <> String.Empty Or txtOwnerName.Text <> String.Empty Or txtCity.Text <> String.Empty) Then
                    sb.Append(" where ")
                End If

                If (txtCenterName.Text <> String.Empty) Then
                    sb.Append("Center_Name Like '" & txtCenterName.Text & "%'")
                End If

                If (txtCenterCode.Text <> String.Empty) Then
                    If (txtCenterName.Text <> String.Empty) Then
                        sb.Append(" AND ")
                    End If
                    sb.Append("Center_Code Like '" & txtCenterCode.Text & "%'")
                End If
                If (txtOwnerName.Text <> String.Empty) Then
                    If (txtCenterName.Text <> String.Empty Or txtCenterCode.Text <> String.Empty) Then
                        sb.Append(" AND ")
                    End If
                    sb.Append(" Owner_Name Like '%" & txtOwnerName.Text & "%' ")
                End If

                If (txtCity.Text <> String.Empty) Then
                    If (txtCenterName.Text <> String.Empty Or txtCenterCode.Text <> String.Empty Or txtOwnerName.Text <> String.Empty) Then
                        sb.Append(" AND ")
                    End If
                    sb.Append("City_Name Like '" & txtCity.Text & "%'")

                End If
                'If (txtstate.Text <> String.Empty) Then
                '    If (txtCenterName.Text <> String.Empty Or txtCenterCode.Text <> String.Empty Or txtOwnerName.Text <> String.Empty Or txtCity.Text <> String.Empty) Then
                '        sb.Append(" AND ")
                '    End If
                '    sb.Append("State_Name Like'" & txtstate.Text & "%'")
                'End If

                'If (txtCountry.Text <> String.Empty) Then
                '    If (txtCenterName.Text <> String.Empty Or txtCenterCode.Text <> String.Empty Or txtOwnerName.Text <> String.Empty Or txtCity.Text <> String.Empty Or txtstate.Text <> String.Empty) Then
                '        sb.Append(" AND ")
                '    End If
                '    sb.Append("Country_Name Like'" & txtCountry.Text & "%'")
                'End If

                'If (txtContactNo.Text <> String.Empty) Then
                '    Dim contactno As Boolean = ValidationForNumber(txtContactNo.Text)
                '    If (contactno = False) Then
                '        lblMsg.Visible = True
                '        lblMsg.Text = Resources.Resource.CenterMaintanance_noerr
                '        txtContactNo.Focus()
                '        DivGrid.Visible = False
                '        DataGridCenterDetails.Visible = False
                '        Exit Sub
                '    End If
                '    If (txtCenterName.Text <> String.Empty Or txtCenterCode.Text <> String.Empty Or txtOwnerName.Text <> String.Empty Or txtCity.Text <> String.Empty Or txtstate.Text <> String.Empty Or txtCountry.Text <> String.Empty) Then
                '        sb.Append(" AND ")
                '    End If
                '    sb.Append("Contact_No Like'" & txtContactNo.Text & "%'")
                'End If

                query = sb.ToString()
                Dim adp As New SqlDataAdapter(query, objconn.MyConnection)
                adp.Fill(dt)
                If (dt.Rows.Count > 0) Then
                    'imgBtnDisable.Visible = True
                    'imgBtnEnable.Visible = True
                    DataGridCenterDetails.DataSource = dt

                    '/*********************Jatin Gangajaliya, 2011/03/30****************/
                    If Session("fromcentreupdate") = "true" Then
                        If Request.QueryString("pi") <> Nothing Then
                            DataGridCenterDetails.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                            ViewState.Add("selval", DataGridCenterDetails.CurrentPageIndex)
                        End If
                        Session.Remove("fromcentreupdate")
                        'Else
                        '    DataGridCenterDetails.CurrentPageIndex = 0
                    End If
                    '/******************************End*******************************/

                    DataGridCenterDetails.DataBind()
                    'fillPagesCombo()
                    fillPageNumbers(DataGridCenterDetails.CurrentPageIndex + 1, 9)

                    For i As Integer = 0 To DataGridCenterDetails.Items.Count - 1
                        If DataGridCenterDetails.Items(i).Cells(1).Text = "True" Then
                            DataGridCenterDetails.Items(i).Cells(10).Enabled = False
                            DataGridCenterDetails.Items(i).Cells(10).ToolTip = "Disabled"
                            DataGridCenterDetails.Items(i).Cells(9).Attributes.Remove("href")
                            DataGridCenterDetails.Items(i).Cells(9).Attributes.Remove("className")
                            DataGridCenterDetails.Items(i).Cells(9).Attributes.Add("onclick", "return false")
                            DataGridCenterDetails.Items(i).BackColor = Drawing.Color.Gray
                        ElseIf DataGridCenterDetails.Items(i).Cells(1).Text = "False" Then
                            DataGridCenterDetails.Items(i).Enabled = True
                        End If
                    Next
                    lblrecords.Text = Resources.Resource.AdminList_TotRecord & ":" & dt.Rows.Count
                    DivGrid.Style.Item("heigth") = "40px"
                Else
                    imgBtnDisable.Visible = False
                    imgBtnEnable.Visible = False
                    imgBtnDelete.Visible = False
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.Common_NoRecFound
                    DataGridCenterDetails.Visible = False
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
    End Sub

    'Added by Pranit on 05/11/2019
    '#Region "Sorting"
    '    Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '        Dim dt As DataTable = CType(Session("Source"), DataTable)
    '        Dim dv As DataView = New DataView(dt)
    '        dv.Sort = e.SortExpression
    '        DataGridCenterDetails.DataSource = dv
    '        DataGridCenterDetails.DataBind()
    '    End Sub 'Sort_Grid
    '#End Region

    '********************************************************************
    'Code added by Monal Shah
    'Purpose: To Clear Controls
    '********************************************************************
    Public Sub ClearControls()
        Try
            lblMsg.Visible = False
            txtCenterName.Text = String.Empty
            txtCenterCode.Text = String.Empty
            txtOwnerName.Text = String.Empty
            txtCity.Text = String.Empty
            'txtstate.Text = String.Empty
            'txtCountry.Text = String.Empty
            'txtContactNo.Text = String.Empty
            DivGrid.Visible = False
            DataGridCenterDetails.Visible = False
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
    'Purpose: chkSelectAll_CheckedChanged
    '********************************************************************
    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox = Nothing
        Try

            For Each rowItem As DataGridItem In DataGridCenterDetails.Items

                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

                chk.Checked = DirectCast(sender, CheckBox).Checked

            Next
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            chk = Nothing
        End Try

    End Sub


    '********************************************************************
    'Code added by Monal Shah
    'Purpose: enableDisable
    '********************************************************************
    Public Function enableDisable(ByVal flg As Boolean, ByVal blflg As Boolean)
        Dim chk As New CheckBox
        Dim strid As String
        Dim q As String = String.Empty
        Dim cid As String
        Dim bolflg As Boolean = True
        Try
            For Each rowItem As DataGridItem In DataGridCenterDetails.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                If chk.Checked Then
                    bolflg = False
                    If objconn.connect() = True Then
                        cid = rowItem.Cells(2).Text
                        If (flg = True) Then
                            q = "update M_Centers set del_flg=1 where Center_Id =" & cid & ""
                        Else
                            q = "update M_Centers set del_flg=0 where Center_Id =" & cid & ""
                        End If
                        Dim cmd As New SqlCommand(q, objconn.MyConnection)
                        cmd.ExecuteNonQuery()
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
            Else
                lblMsg.Visible = False
            End If
            Dim intindex As Integer = DataGridCenterDetails.CurrentPageIndex
            BindGrid()
            DataGridCenterDetails.CurrentPageIndex = intindex

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            chk = Nothing
            strid = Nothing
            q = Nothing
            cid = Nothing
        End Try
    End Function

    'Added by Pranit on 27/11/2019
    Sub Selection_Change(sender As Object, e As EventArgs)
        Try
            DataGridCenterDetails.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub

    Protected Sub imgBtnEnable_Click(sender As Object, e As EventArgs) Handles imgBtnEnable.Click
        Try
            enableDisable(False, True)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub imgBtnDisable_Click(sender As Object, e As EventArgs) Handles imgBtnDisable.Click
        Try
            enableDisable(True, False)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub DataGridCenterDetails_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGridCenterDetails.ItemCommand
        Try
            If e.CommandName = "lnkEdit" Then
                lnkEditID = e.CommandArgument
                Session.Add("CenterIDValue", lnkEditID)
                Dim oldVal As New Hashtable
                oldVal.Add("name", txtCenterName.Text)
                oldVal.Add("code", txtCenterCode.Text)
                oldVal.Add("owner", txtOwnerName.Text)
                oldVal.Add("city", txtCity.Text)
                'oldVal.Add("state", txtstate.Text)
                'oldVal.Add("country", txtCountry.Text)
                'oldVal.Add("contact", txtContactNo.Text)
                Session.Add("oldVal", oldVal)

                Response.Redirect("CentreRegistration.aspx?pi=" & DataGridCenterDetails.CurrentPageIndex, False)
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub DataGridCenterDetails_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGridCenterDetails.ItemDataBound
        If Not e.Item.ItemType = DataControlRowType.Header Then
            e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
        End If
    End Sub

    Private Sub DataGridCenterDetails_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGridCenterDetails.PageIndexChanged
        Try
            DataGridCenterDetails.CurrentPageIndex = e.NewPageIndex
            'If Session("pageindex") <> Nothing Then
            '    Session.Remove("pageindex")
            'End If
            BindGrid()
            'Session.Add("pageindex", e.NewPageIndex)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
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


        If len >= DataGridCenterDetails.PageCount Then
            len = DataGridCenterDetails.PageCount - 1
        End If

        ' if search clicked again then page 1 should be selected 
        If DataGridCenterDetails.CurrentPageIndex = 0 Then
            ViewState("pageNo") = 1
            ViewState("lastRange") = 1
        End If

        ' Getting the currently selected page value 
        Dim selPage As Integer = 0
        If (ViewState("pageNo") <> Nothing) Then
            selPage = CInt(ViewState("pageNo"))
        Else
            ' selPage = 1
            selPage = DataGridCenterDetails.CurrentPageIndex + 1
        End If

        If (ViewState("lastRange") <> Nothing) Then

            '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <=  DataGridCenterDetails.PageCount Then
            If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
                range = CInt(ViewState("lastRange"))
            Else
                'If it is the last page then resetting the page numbers
                ' last number - provided length
                'If (len + selPage) >= DataGridCenterDetails.PageCount Then
                '    If selPage <= len Then
                '        range = range
                '    Else
                '        range = DataGridCenterDetails.PageCount - len
                '        'Incase range becomes 0 or less than zero than setting it 1 
                '        If range <= 0 Then
                '            range = 1
                '        End If
                '    End If

                'Else
                If selPage <= DataGridCenterDetails.PageCount Then
                    'range = range
                    If range < CInt(ViewState("lastRange")) Then
                        range = CInt(ViewState("lastRange")) - 1
                    Else
                        '  range = CInt(ViewState("lastRange")) + 1
                        If selPage - len > 0 And selPage - len <= DataGridCenterDetails.PageCount - len Then
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
        'If selPage = 1 And selPage =  DataGridCenterDetails.PageCount - 1 Then
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
        If selPage = DataGridCenterDetails.PageCount Then
            imgnext.Enabled = False
            imglast.Enabled = False
        End If
    End Sub
    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGridCenterDetails.CurrentPageIndex < (DataGridCenterDetails.PageCount - 1)) Then
                    DataGridCenterDetails.CurrentPageIndex += 1

                End If

            Case "prev" 'The prev button was clicked
                If (DataGridCenterDetails.CurrentPageIndex > 0) Then
                    DataGridCenterDetails.CurrentPageIndex -= 1
                End If

            Case "last" 'The Last Page button was clicked
                DataGridCenterDetails.CurrentPageIndex = (DataGridCenterDetails.PageCount - 1)

            Case Else 'The First Page button was clicked
                DataGridCenterDetails.CurrentPageIndex = Convert.ToInt32(arg)
        End Select
        ViewState.Add("pageNo", DataGridCenterDetails.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGridCenterDetails.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGridCenterDetails.CurrentPageIndex < (DataGridCenterDetails.PageCount - 1)) Then
                    DataGridCenterDetails.CurrentPageIndex += 1
                    '    ViewState.Add("selval", DataGridCenterDetails.CurrentPageIndex)
                End If

            Case "prev" 'The prev button was clicked
                If (DataGridCenterDetails.CurrentPageIndex > 0) Then
                    DataGridCenterDetails.CurrentPageIndex -= 1
                    '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                End If

            Case "last" 'The Last Page button was clicked
                DataGridCenterDetails.CurrentPageIndex = (DataGridCenterDetails.PageCount - 1)
                'ViewState.Add("selval", ddlPages.SelectedItem.Value)
            Case Else 'The First Page button was clicked
                DataGridCenterDetails.CurrentPageIndex = Convert.ToInt32(arg) - 1
                ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
        End Select

        ViewState.Add("pageNo", DataGridCenterDetails.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGridCenterDetails.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    'Public Sub fillPagesCombo()
    '    ddlPages.Items.Clear()
    '    For cnt As Integer = 1 To DataGridCenterDetails.PageCount
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
    '    DataGridCenterDetails.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    BindGrid()
    'End Sub

    '****************************************************************'
    ''Author: Irfan Mansuri                                          '
    ''Description: To Delete class/Center.                           '
    ''Created Date: 25/02/2015                                       '
    '****************************************************************'
    Protected Sub imgBtnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgBtnDelete.Click
        Dim chk As CheckBox = Nothing
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
        Dim strBuild As StringBuilder
        Dim strQ, strQuery As String
        Dim strCenter_id As String
        Dim boldecision As Boolean = True

        Try
            lblMsg.Visible = False
            strBuild = New StringBuilder
            strBuild.Append(" (")
            For Each rowItem As DataGridItem In DataGridCenterDetails.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                If chk.Checked Then
                    boldecision = False
                    'strid = DirectCast(rowItem.Cells(1).Text, String)
                    strCenter_id = Convert.ToString(DataGridCenterDetails.DataKeys(rowItem.ItemIndex))
                    strBuild.Append(strCenter_id)
                    strBuild.Append(", ")
                End If
            Next

            If boldecision = True Then
                lblMsg.Visible = True
                lblMsg.ForeColor = Color.Red
                lblMsg.Text = Resources.Resource.CenterMaintanance_del
                Exit Sub
            End If

            strQ = strBuild.ToString
            strQ = strQ.Substring(0, strQ.Length - 2)
            strQ = strQ & ")"

            If objconn.connect() Then
                strBuild = New StringBuilder
                strBuild.Append("Alter table M_user_info Nocheck Constraint FK_M_USER_INFO_M_Centers; Delete M_Centers ")
                strBuild.Append(" where M_Centers.Center_ID IN ")
                strBuild.Append(strQ)
                strQuery = strBuild.ToString()
                myCommand = New SqlCommand(strQuery, objconn.MyConnection)
                myCommand.ExecuteNonQuery()
            End If
            objconn.disconnect()
            lblMsg.Visible = True
            lblMsg.ForeColor = Color.Green
            lblMsg.Text = Resources.Resource.CenterMaintanance_delerr
            Dim intindex As Integer = DataGridCenterDetails.CurrentPageIndex
            BindGrid()
            DataGridCenterDetails.CurrentPageIndex = intindex
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            strQ = Nothing
            strQuery = Nothing
            strBuild = Nothing
            strPathDb = Nothing
            chk = Nothing
            objconn = Nothing
            boldecision = Nothing
        End Try
    End Sub
End Class