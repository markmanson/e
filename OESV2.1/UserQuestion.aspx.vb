#Region "Namespaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
Imports System.Drawing
#End Region


Partial Public Class UserQuestion
    Inherits System.Web.UI.Page
#Region "Declaration"
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("userquestion")
    Dim objconn As New ConnectDb
    Dim strPathDb As String
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            BindGrid()
        Else
            If DGData.Visible = True Then
                fillPageNumbers(1, DGData.PageCount - 1)
            End If
        End If
        
    End Sub

    Public Sub BindGrid()
        Dim strquery As String
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String
        Dim myTable As DataTable
        Dim strbr As StringBuilder
        Dim adap As SqlDataAdapter
        Dim col As DataColumn
        Dim strsearchdetails As String

        Dim qnid As String = ""
        Dim course_id As String=""
        Dim testtype As String = ""
        
        Try

         
            qnid = Session("Eqno").ToString
            testtype = Session("Etesttype").ToString
            lblQuestion.Text = ""
            lblQuestion.Text = "<b>Question : </b>" & Session("EQuestion").ToString 'EQuestion

            myTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            strbr = New StringBuilder()
            

            'Added By Bharat Prajapati
            course_id=Convert.ToString( Session("CrsName"))
           
            'Author Name : Indravadan Vasava
            'Creation date : 2011/06/09
            strbr.Append(" select tr.qno,tr.userid,tr.test_type,ui.SurName+' '+ui.Name+''+isnull(ui.Middlename,'') as Name,tr.course_id, ")
            strbr.Append(" mc.course_name,mt.test_name,mce.Center_name,ui.center_id ")
            strbr.Append("  from t_result as tr  ")
            strbr.Append(" Left join m_user_info as ui ")
            strbr.Append(" on ui.Userid=tr.userid ")
            strbr.Append(" Left join m_course as mc ")
            strbr.Append("  on mc.course_id=tr.course_id ")
            strbr.Append(" Left join m_testinfo as mt ")
            strbr.Append(" on mt.test_type=tr.test_type ")
            strbr.Append(" Left Join M_Centers as mce ")
            strbr.Append(" on mce.center_id=ui.center_id ")
            strbr.Append(" where tr.qno=" & qnid & "  and tr.test_type=" & testtype & " ")
            strbr.Append("and tr.course_id=" & course_id & " ")
            'strbr.Append(" and ui.Delete_Flg=0 ")
            '  Session("CrsName")=Nothing 





            strquery = strbr.ToString

            If objconn.connect() Then
                adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                adap.Fill(myTable)

                If (myTable.Rows.Count > 0) Then
                    DGData.Visible = True
                    DGData.DataSource = myTable


                    If Session("fromregister") <> Nothing Then
                        If Request.QueryString("pi") <> Nothing Then
                            DGData.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                        End If
                        Session.Remove("fromregister")
                    End If
                    Try
                        DGData.DataBind()
                    Catch ex As Exception
                        DGData.CurrentPageIndex = 0
                        DGData.DataBind()
                    End Try
                    ViewState.Add("selval", DGData.CurrentPageIndex)
                    fillPagesCombo()
                    fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                    'Making datagrid rows gray coloured for students which are disabled.
                    ''For i As Integer = 0 To DGData.Items.Count - 1
                    ''    If DGData.Items(i).Cells(8).Text = "True" Then
                    ''        DGData.Items(i).Cells(10).Enabled = True
                    ''        DGData.Items(i).Cells(9).Enabled = False
                    ''        DGData.Items(i).Cells(9).ToolTip = "Disabled"
                    ''        DGData.Items(i).BackColor = Drawing.Color.Gray
                    ''    ElseIf DGData.Items(i).Cells(8).Text = "False" Then
                    ''        DGData.Items(i).Enabled = True
                    ''    End If
                    ''Next

                    lblrecords.Text = "Total Records:" & myTable.Rows.Count
                    ' gridDiv.Visible = True
                    'imgbtndisable.Visible = True
                    'imgbtnenable.Visible = True
                    errorMsg.Text = String.Empty
                    DGData.Visible = True
                Else
                    ' gridDiv.Visible = False
                    DGData.Visible = False
                    'imgbtndisable.Visible = False
                    ' imgbtnenable.Visible = False
                    errorMsg.ForeColor = Color.Red
                    errorMsg.Text = "No Record(s) Found"
                End If
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex

        Finally
            objconn.disconnect()
            objconn = Nothing
            myCommand = Nothing
            myTable = Nothing
            adap = Nothing
            strbr = Nothing
            col = Nothing
            strquery = Nothing
            strPathDb = Nothing
            strsearchdetails = Nothing

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


        If len >= DGData.PageCount Then
            len = DGData.PageCount - 1
        End If

        ' if search clicked again then page 1 should be selected 
        If DGData.CurrentPageIndex = 0 Then
            ViewState("pageNo") = 1
            ViewState("lastRange") = 1
        End If

        ' Getting the currently selected page value 
        Dim selPage As Integer = 0
        If (ViewState("pageNo") <> Nothing) Then
            selPage = CInt(ViewState("pageNo"))
        Else
            ' selPage = 1
            selPage = DGData.CurrentPageIndex + 1
        End If

        If (ViewState("lastRange") <> Nothing) Then

            '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DGData.PageCount Then
            If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
                range = CInt(ViewState("lastRange"))
            Else
                'If it is the last page then resetting the page numbers
                ' last number - provided length
                ''If (len + selPage) >= DGData.PageCount Then
                ''    If selPage <= len Then
                ''        range = range
                ''    Else
                ''        range = DGData.PageCount - len
                ''        'Incase range becomes 0 or less than zero than setting it 1 
                ''        If range <= 0 Then
                ''            range = 1
                ''        End If
                ''    End If

                ''Else
                If selPage <= DGData.PageCount Then
                    'range = range
                    If range < CInt(ViewState("lastRange")) Then
                        range = CInt(ViewState("lastRange")) - 1
                    Else
                        If selPage - len > 0 And selPage - len <= DGData.PageCount - len Then
                            range = selPage - len
                        Else
                            range = CInt(ViewState("lastRange")) + 1
                        End If
                        '   range = CInt(ViewState("lastRange")) + 1
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
        'If selPage = 1 And selPage = DGData.PageCount - 1 Then
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
        If selPage = DGData.PageCount Then
            imgnext.Enabled = False
            imglast.Enabled = False
        End If

    End Sub
    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DGData.CurrentPageIndex < (DGData.PageCount - 1)) Then
                    DGData.CurrentPageIndex += 1

                End If

            Case "prev" 'The prev button was clicked
                If (DGData.CurrentPageIndex > 0) Then
                    DGData.CurrentPageIndex -= 1
                End If

            Case "last" 'The Last Page button was clicked
                DGData.CurrentPageIndex = (DGData.PageCount - 1)

            Case Else 'The First Page button was clicked
                DGData.CurrentPageIndex = Convert.ToInt32(arg)
        End Select
        ViewState.Add("pageNo", DGData.CurrentPageIndex + 1)
        ViewState.Add("selval", DGData.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DGData.CurrentPageIndex < (DGData.PageCount - 1)) Then
                    DGData.CurrentPageIndex += 1
                    '    ViewState.Add("selval", DGData.CurrentPageIndex)
                End If

            Case "prev" 'The prev button was clicked
                If (DGData.CurrentPageIndex > 0) Then
                    DGData.CurrentPageIndex -= 1
                    '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                End If

            Case "last" 'The Last Page button was clicked
                DGData.CurrentPageIndex = (DGData.PageCount - 1)
                'ViewState.Add("selval", ddlPages.SelectedItem.Value)
            Case Else 'The First Page button was clicked
                DGData.CurrentPageIndex = Convert.ToInt32(arg) - 1
                ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
        End Select

        ViewState.Add("pageNo", DGData.CurrentPageIndex + 1)
        ViewState.Add("selval", DGData.CurrentPageIndex)
        BindGrid()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Public Sub fillPagesCombo()
        ddlPages.Items.Clear()
        For cnt As Integer = 1 To DGData.PageCount
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

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        If Not Request.QueryString("pi") Is Nothing Then
            Session.Add("Epi", Request.QueryString("pi").ToString)
        End If
        Response.Redirect("ExamCount.aspx", False)
    End Sub

    Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
        DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
        ViewState.Add("selval", ddlPages.SelectedItem.Value)
        ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
        BindGrid()
    End Sub
End Class