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
    Partial Public Class ClientInformation
        Inherits System.Web.UI.Page
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("ClientInformation")
        Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Dim lnkEditID As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                If Request.QueryString("pi") <> Nothing Then
                    search()
                    If DG_ClientDetail.Items.Count > 0 Then
                        DivGrid.Visible = True
                        DG_ClientDetail.Visible = True
                    End If
                End If
            End If

            If DG_ClientDetail.Visible = True Then
                fillPageNumbers(DG_ClientDetail.CurrentPageIndex + 1, 9)
            End If
        End Sub
#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload
            Try
                If Not strPathDb = Nothing Then
                    If objconn.connect(strPathDb) Then
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

        Protected Sub imgBtnNewCentreRegistration_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnNewCentreRegistration.Click
            Session.Add("ClientRegistration", "true")
            Response.Redirect("ClientRegistration.aspx", False)

        End Sub

        Protected Sub ImgBtnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImgBtnBack.Click
            Response.Redirect("Admin.aspx", False)
        End Sub

        Protected Sub imgBtnClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnClear.Click
            txtClientName.Text = String.Empty
        End Sub

        Protected Sub imgBtnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnSearch.Click
            search()
        End Sub
        Sub search()
            Dim strSb As StringBuilder
            Dim ad As SqlDataAdapter
            Dim ds As DataSet
            Try
                If Session.Item("oldVal") <> Nothing Then
                    txtClientName.Text = Session.Item("oldVal").ToString
                    Session.Remove("oldVal")
                End If
                strSb = New StringBuilder
                strSb.Append("select * from M_Client_Master")
                If txtClientName.Text.ToString <> "" Then
                    strSb.Append(" where client_id='")
                    strSb.Append(txtClientName.Text)
                    strSb.Append("'")
                End If
                strSb.Append(" order by client_name")
                If objconn.connect(strPathDb) Then
                    ds = New DataSet
                    ad = New SqlDataAdapter(strSb.ToString, objconn.MyConnection)
                    ad.Fill(ds)
                    DG_ClientDetail.DataSource = ds
                    If ds.Tables(0).Rows.Count > 0 Then
                        If Session("ClientRegistration") = "true" Then
                            If Request.QueryString("pi") <> Nothing Then
                                DG_ClientDetail.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                                ViewState.Add("selval", DG_ClientDetail.CurrentPageIndex)
                            End If
                            Session.Remove("ClientRegistration")
                        End If
                        DG_ClientDetail.DataBind()
                        fillPagesCombo()
                        fillPageNumbers(DG_ClientDetail.CurrentPageIndex + 1, 9)
                        DG_ClientDetail.Visible = True
                        DivGrid.Visible = True
                        lblMsg.Visible = False

                        For i As Integer = 0 To DG_ClientDetail.Items.Count - 1
                            If DG_ClientDetail.Items(i).Cells(8).Text = "True" Then
                                DG_ClientDetail.Items(i).Cells(6).Enabled = False
                                DG_ClientDetail.Items(i).BackColor = Drawing.Color.Gray
                            ElseIf DG_ClientDetail.Items(i).Cells(8).Text = "False" Then
                                DG_ClientDetail.Items(i).Enabled = True
                            End If
                        Next
                    Else
                        DG_ClientDetail.Visible = False
                        DivGrid.Visible = False
                        lblMsg.Text = "Record(s) not found."
                        lblMsg.Visible = True
                    End If

                End If
            Catch ex As Exception
                objconn.disconnect()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                lblMsg.Text = ex.Message()
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                objconn.disconnect()
                strSb = Nothing
                ad = Nothing
                ds = Nothing
            End Try
        End Sub
        Private Sub fillPageNumbers(ByVal range As Integer, ByVal len As Integer)


            If (Session("last") Is Nothing) Then
                Session.Add("last", 1)
                ViewState.Add("lastRange", 1)
            Else
                ViewState("lastRange") = CInt(Session("last"))
            End If

            If len >= DG_ClientDetail.PageCount Then
                len = DG_ClientDetail.PageCount - 1
            End If

            ' if search clicked again then page 1 should be selected 
            If DG_ClientDetail.CurrentPageIndex = 0 Then
                ViewState("pageNo") = 1
                ViewState("lastRange") = 1
            End If

            ' Getting the currently selected page value 
            Dim selPage As Integer = 0
            If (ViewState("pageNo") <> Nothing) Then
                selPage = CInt(ViewState("pageNo"))
            Else
                ' selPage = 1
                selPage = DG_ClientDetail.CurrentPageIndex + 1
            End If

            If (ViewState("lastRange") <> Nothing) Then
                If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
                    range = CInt(ViewState("lastRange"))
                Else

                    If selPage <= DG_ClientDetail.PageCount Then

                        If range < CInt(ViewState("lastRange")) Then
                            range = CInt(ViewState("lastRange")) - 1
                        Else

                            If selPage - len > 0 And selPage - len <= DG_ClientDetail.PageCount - len Then
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

            imgprev.Enabled = True
            imgfirst.Enabled = True
            imgnext.Enabled = True
            imglast.Enabled = True

            If selPage = 1 Then
                imgprev.Enabled = False
                imgfirst.Enabled = False
            End If
            If selPage = DG_ClientDetail.PageCount Then
                imgnext.Enabled = False
                imglast.Enabled = False
            End If
        End Sub
        Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DG_ClientDetail.CurrentPageIndex < (DG_ClientDetail.PageCount - 1)) Then
                        DG_ClientDetail.CurrentPageIndex += 1

                    End If

                Case "prev" 'The prev button was clicked
                    If (DG_ClientDetail.CurrentPageIndex > 0) Then
                        DG_ClientDetail.CurrentPageIndex -= 1
                    End If

                Case "last" 'The Last Page button was clicked
                    DG_ClientDetail.CurrentPageIndex = (DG_ClientDetail.PageCount - 1)

                Case Else 'The First Page button was clicked
                    DG_ClientDetail.CurrentPageIndex = Convert.ToInt32(arg)
            End Select
            ViewState.Add("pageNo", DG_ClientDetail.CurrentPageIndex + 1)
            ViewState.Add("selval", DG_ClientDetail.CurrentPageIndex)
            search()
           
        End Sub

        Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DG_ClientDetail.CurrentPageIndex < (DG_ClientDetail.PageCount - 1)) Then
                        DG_ClientDetail.CurrentPageIndex += 1
                    End If

                Case "prev" 'The prev button was clicked
                    If (DG_ClientDetail.CurrentPageIndex > 0) Then
                        DG_ClientDetail.CurrentPageIndex -= 1
                    End If

                Case "last" 'The Last Page button was clicked
                    DG_ClientDetail.CurrentPageIndex = (DG_ClientDetail.PageCount - 1)
                Case Else 'The First Page button was clicked
                    DG_ClientDetail.CurrentPageIndex = Convert.ToInt32(arg) - 1
            End Select

            ViewState.Add("pageNo", DG_ClientDetail.CurrentPageIndex + 1)
            ViewState.Add("selval", DG_ClientDetail.CurrentPageIndex)
            search()

        End Sub

        Public Sub fillPagesCombo()
            ddlPages.Items.Clear()
            For cnt As Integer = 1 To DG_ClientDetail.PageCount
                Dim lstitem As New ListItem
                lstitem.Value = cnt - 1
                lstitem.Text = cnt
                ddlPages.Items.Add(lstitem)
                If Not ViewState("selval") Is Nothing Then
                    If CInt(ViewState("selval")) = lstitem.Value Then
                        ddlPages.SelectedValue = lstitem.Value
                    End If
                End If

                lstitem = Nothing
            Next

        End Sub
        Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPages.SelectedIndexChanged
            DG_ClientDetail.CurrentPageIndex = ddlPages.SelectedItem.Value
            ViewState.Add("selval", ddlPages.SelectedItem.Value)
            ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
            search()
        End Sub

        
        Protected Sub DG_ClientDetail_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DG_ClientDetail.PageIndexChanged
            Try
                DG_ClientDetail.CurrentPageIndex = e.NewPageIndex
                If Request.QueryString("pi") <> Nothing Then
                    If DG_ClientDetail.Items.Count > 0 Then
                        DivGrid.Visible = True
                        DG_ClientDetail.Visible = True
                        imgBtnDisable.Visible = True
                        imgBtnEnable.Visible = True
                        lblMsg.Visible = True
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub

        Protected Sub DG_ClientDetail_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DG_ClientDetail.ItemCommand
            Try
                If e.CommandName.ToString = "lnkEdit" Then
                    lnkEditID = e.CommandArgument.ToString
                    Session.Add("ClientIDValue", lnkEditID)
                    Session.Add("oldVal", txtClientName.Text)
                    Response.Redirect("ClientRegistration.aspx?pi=" & DG_ClientDetail.CurrentPageIndex, False)
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub

        Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim chk As CheckBox = Nothing
            Try

                For Each rowItem As DataGridItem In DG_ClientDetail.Items

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

        Protected Sub chkRemove_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            Dim chk As CheckBox = Nothing
            Dim bool As Boolean = True
            Dim filterControl As Control = Nothing
            Try
               
                filterControl = GetFilterControlByID("chkSelectAll")

                Dim chks As CheckBox = TryCast(filterControl, CheckBox)

                Dim c As CheckBox = DirectCast(sender, CheckBox)
                For Each rowItem As DataGridItem In DG_ClientDetail.Items

                    chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

                    chks.Checked = True
                    If chk.Checked = False Then
                        chks.Checked = False
                        Exit For
                    End If

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
        Private Function GetFilterControlByID(ByVal controlID As String) As Control
            Dim tableHeader As Table = DirectCast(DG_ClientDetail.Controls(0), Table)
            Dim gridItem As DataGridItem = Nothing
            Dim filterControl As Control = Nothing
            For nCtr As Integer = 0 To tableHeader.Rows.Count - 1
                gridItem = DirectCast(tableHeader.Rows(nCtr), DataGridItem)
                If gridItem.ItemType = ListItemType.Header Then
                    Exit For
                End If
            Next
            For Each cntrl As Control In gridItem.Controls
                If cntrl IsNot Nothing Then
                    filterControl = cntrl.FindControl(controlID)
                    If filterControl IsNot Nothing Then
                        Return filterControl
                    End If
                End If
            Next
            Return filterControl
        End Function
        Public Function enableDisable(ByVal flg As Boolean, ByVal blflg As Boolean)
            Dim chk As New CheckBox
            Dim strid As String
            Dim q As String = String.Empty
            Dim cid As String
            Dim bolflg As Boolean = True
            Try
                For Each rowItem As DataGridItem In DG_ClientDetail.Items
                    chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                    If chk.Checked Then
                        bolflg = False
                        If objconn.connect(strPathDb) = True Then
                            cid = rowItem.Cells(1).Text
                            If (flg = True) Then
                                q = "update M_Client_Master set Del_Flag=1 where client_id ='" & cid & "'"
                            Else
                                q = "update M_Client_Master set Del_Flag=0 where client_id ='" & cid & "'"
                            End If
                            Dim cmd As New SqlCommand(q, objconn.MyConnection)
                            cmd.ExecuteNonQuery()
                        End If
                    End If
                Next
                If bolflg = True Then
                    If blflg = False Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Select At Least One CheckBox For Disable Class"
                    ElseIf blflg = True Then
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Select At Least One CheckBox For Enable Class"
                    End If
                Else
                    lblMsg.Visible = False
                End If
                Dim intindex As Integer = DG_ClientDetail.CurrentPageIndex
                search()
                DG_ClientDetail.CurrentPageIndex = intindex

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

        Protected Sub imgBtnEnable_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnEnable.Click
            Try
                enableDisable(False, True)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub

        Protected Sub imgBtnDisable_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnDisable.Click
            Try
                enableDisable(True, False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub

        Protected Sub DG_ClientDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DG_ClientDetail.ItemDataBound
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        End Sub
    End Class
End Namespace