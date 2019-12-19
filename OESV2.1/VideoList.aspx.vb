#Region "NameSpaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
Imports System.Drawing
Imports System.Web.Services

#End Region
#Region "VideoList Monal Shah 2011/8/10"
Partial Public Class VideoList
    Inherits BasePage

#Region "Declaration"

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("vidoelist")
    Dim objconn As New ConnectDb
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim Categ As String = ""

#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack() Then
            'FillTreeView()

            PopulateRootLevel()
            'Added by Pranit on 08/11/2019
            'DGData.DataSource = DisplayVideoListGrid(Categ)
            'DGData.DataBind()
        Else
            Dim tn As TreeNode = trvwCategory.SelectedNode
            'If Not tn Is Nothing Then
            '    'DisplayVideoList(tn.Value)
            '    If DGData.Visible = True Then
            '        DGData.CurrentPageIndex = 0
            '        fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
            '    End If
            '    Dim ctrlname As String = Page.Request.Params.[Get]("__EVENTTARGET")
            '    If ctrlname IsNot Nothing AndAlso ctrlname <> String.Empty Then
            '    End If
            'End If
        End If
    End Sub
    Public Sub FillTreeView()
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        gridDiv.Visible = False
        errorMsg.Visible = False
        Try

            If objconn.connect() Then
                da = New SqlDataAdapter("select Category_id,Category_name from M_Category where parent_ID=0", objconn.MyConnection)
                ds = New DataSet
                da.Fill(ds)
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    Dim tnode As New TreeNode(ds.Tables(0).Rows(i).Item(1).ToString, ds.Tables(0).Rows(i).Item(0).ToString)
                    trvwCategory.Nodes.Add(tnode)
                    tnode = Nothing
                Next
                objconn.disconnect()
            End If

        Catch ex As Exception
            If objconn.connect() Then
                objconn.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn = Nothing
            da = Nothing
            ds = Nothing

        End Try
    End Sub

    'Added by Pranit on 11/12/2019
#Region "Tree"
    <WebMethod>
    Public Shared Function GetVideoList() As ArrayList
        Dim myDataSet As DataSet
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlstr As String
        Dim conn As SqlConnection
        Dim arrlist As New ArrayList
        conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        conn.Open()

        sqlstr = " Select Category_ID, Parent_ID, Category_Name from M_Category order by Parent_ID "
        myDataSet = New DataSet()
        myDataAdapter = New SqlDataAdapter(sqlstr, conn)
        myDataAdapter.Fill(myDataSet)
        Dim hash As String = "#"
        If myDataSet.Tables(0).Rows.Count >= 1 Then
            For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + If(myDataSet.Tables(0).Rows(count)(1).ToString() = 0, hash, myDataSet.Tables(0).Rows(count)(1).ToString()) + "_" + myDataSet.Tables(0).Rows(count)(2).ToString())
            Next
        End If
        conn.Close()
        Return arrlist
    End Function
#End Region

    Private Sub trvwCategory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles trvwCategory.DataBound
        'Dim List As New SortedList
        'For I As Integer = 0 To trvwCategory.Nodes.Count - 1
        '    Dim Text As String = trvwCategory.Nodes(I).Text
        '    Dim Un As Integer = 1
        '    If List.Contains(Text) Then
        '        While List.Contains(Text)
        '            Text = trvwCategory.Nodes(I).Text & " -- # " & Un
        '        End While
        '        Un += 1
        '    End If
        '    List.Add(Text, trvwCategory.Nodes(I))
        'Next
        'trvwCategory.Nodes.Clear()
        'For I As Integer = 0 To List.Count - 1
        '    trvwCategory.Nodes.Add(List.GetByIndex(I))
        'Next
    End Sub

    Private Sub trvwCategory_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trvwCategory.SelectedNodeChanged
        ' DisplayVideoList(trvwCategory.SelectedNode.Value)
        DisplayVideoListGrid(trvwCategory.SelectedNode.Value)
    End Sub
    Public Function GetFileExtension(ByVal ID As String) As String
        Dim result As String = ""

        For Each sFile As String In System.IO.Directory.GetFiles(Server.MapPath("videos"))
            Dim sJustFile As String = New System.IO.FileInfo(sFile).Name
            If sJustFile.StartsWith(ID & ".") Then
                result = sJustFile
                Exit For
            End If
        Next

        Return result
    End Function
    Public Sub DisplayVideoList(ByVal CategoryID As String)
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim tr As HtmlTableRow
        Dim td As HtmlTableCell
        Dim lbtn As LinkButton
        Dim duration As Double = 0.0

        ' tblvdoList.Controls.Clear()

        Try
            If objconn.connect() = True Then
                ds = New DataSet
                da = New SqlDataAdapter("select * from M_Video where  Category_ID=" & CategoryID & " order by title", objconn.MyConnection)
                'da = New SqlDataAdapter("select Title,MC.Category_Name as CategoryName,(cast(Vedio_ID as varchar) +'.'+ Extension)  as FileNames from M_Video as M inner join M_Category as MC on M.Category_ID =MC.Category_ID  where  MC.Category_ID=" & CategoryID & " order by title", objconn.MyConnection)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        lbtn = New LinkButton()
                        lbtn.ID = ds.Tables(0).Rows(i).Item(0).ToString
                        If ds.Tables(0).Rows(i).Item(4).ToString = "ppt" Then
                            lbtn.Attributes.Add("href", "videos/" & ds.Tables(0).Rows(i).Item(0).ToString & "." & ds.Tables(0).Rows(i).Item(4).ToString)
                            lbtn.Attributes.Add("target", "_blank")
                        End If

                        lbtn.Text = " " & ds.Tables(0).Rows(i).Item(2).ToString
                        lbtn.Font.Overline = False
                        lbtn.Font.Size = 10

                        Dim fileName As String = GetFileExtension(ds.Tables(0).Rows(i).Item(0).ToString)

                        If Not fileName Is Nothing Then
                            If Not fileName.EndsWith(".ppt") Then
                                lbtn.Attributes.Add("OnClick", "return playSelected('videos/" & fileName & "'," & ds.Tables(0).Rows(i).Item(0).ToString & ");")
                            End If
                        End If
                        td = New HtmlTableCell
                        td.InnerHtml = "<img src='images/Nav-Blue_right.jpg' /> "
                        td.Controls.Add(lbtn)
                        tr = New HtmlTableRow
                        tr.Cells.Add(td)
                        'tblvdoList.Rows.Add(tr)
                        lbtn = Nothing
                        tr = Nothing
                        td = Nothing
                    Next
                Else
                    td = New HtmlTableCell
                    td.InnerHtml = Resources.Resource.VideoList_NoVFound
                    tr = New HtmlTableRow
                    tr.Cells.Add(td)
                    'tblvdoList.Rows.Add(tr)
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    <WebMethod(EnableSession:=True)>
    Public Shared Function CallVideoList(CategoryID As String) As Integer
        Dim objVideoList As New VideoList
        Dim getID As String = CategoryID
        objVideoList.DisplayVideoListGrid(CategoryID)
        Return 0
    End Function

    'Added by Pranit on 13/12/2019
    '#Region "Fill Data"
    '    <WebMethod>
    '    Public Shared Function GetVideoListData(CategoryID As String) As ArrayList
    '        Dim myDataSet As DataSet
    '        Dim myDataAdapter As SqlDataAdapter
    '        Dim sqlstr As String
    '        Dim conn As SqlConnection
    '        Dim arrlist As New ArrayList
    '        conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
    '        conn.Open()
    '        sqlstr = " select ROW_NUMBER() OVER (ORDER BY title) AS SrNo, Title,MC.Category_Name as CategoryName,(cast(Vedio_ID as varchar) +'.'+ isnull(Extension,Null))  as FileNames from M_Video as M inner join M_Category as MC on M.Category_ID =MC.Category_ID  where  MC.Category_ID=" & CategoryID & " and M.Del_flag=0 order by title"
    '        myDataSet = New DataSet()
    '        myDataAdapter = New SqlDataAdapter(sqlstr, conn)
    '        myDataAdapter.Fill(myDataSet)
    '        If myDataSet.Tables(0).Rows.Count >= 1 Then
    '            For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
    '                arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "*#" + myDataSet.Tables(0).Rows(count)(1).ToString() + "*#" + myDataSet.Tables(0).Rows(count)(2).ToString() + "*#" + myDataSet.Tables(0).Rows(count)(3).ToString())
    '            Next
    '        End If
    '        conn.Close()
    '        Return arrlist
    '    End Function
    '#End Region

    Public Function DisplayVideoListGrid(CategoryID As String) As Integer
        Dim myTable As DataTable
        Dim da As SqlDataAdapter
        Dim duration As Double = 0.0
        Dim col As DataColumn
        Dim strbr As StringBuilder
        Dim str As String
        Dim cnt As Integer
        Dim filevaluearr(cnt) As String
        Dim filevalueal As New ArrayList
        Try
            myTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strbr = New StringBuilder()
            strbr.Append("  ")
            strbr.Append(" select Title,MC.Category_Name as CategoryName,(cast(Vedio_ID as varchar) +'.'+ isnull(Extension,Null))  as FileNames,Vedio_ID ")
            strbr.Append(" from M_Video as M inner join M_Category as MC on M.Category_ID =MC.Category_ID  where  MC.Category_ID=" & CategoryID & " and M.Del_flag=0 order by title")
            str = strbr.ToString()
            'Added by Pranit on 08/11/2019
            'Session("Source") = myTable
            'Dim dv As DataView = New DataView(myTable)
            If objconn.connect() = True Then
                da = New SqlDataAdapter(str, objconn.MyConnection)
                da.Fill(myTable)
                If myTable.Rows.Count > 0 Then
                    'DGData.Visible = True
                    'gridDiv.Visible = True
                    'errorMsg.Visible = False
                    DGData.DataSource = myTable.DefaultView
                    DGData.DataBind()
                    'fillPagesCombo()
                    fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                    lblrecords.Text = " " & myTable.Rows.Count
                Else
                    errorMsg.Visible = True
                    DGData.Visible = False
                    gridDiv.Visible = False
                    errorMsg.Text = Resources.Resource.Common_NoRecFound
                End If

            End If
            'Added by Pranit on 08/11/2019
            'Return dv

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
        Return 0
    End Function

    'Added by Pranit on 08/11/2019
#Region "Sorting"
    'Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '    Dim dt As DataTable = CType(Session("Source"), DataTable)
    '    Dim dv As DataView = New DataView(dt)
    '    dv.Sort = e.SortExpression
    '    DGData.DataSource = dv
    '    DGData.DataBind()
    'End Sub 'Sort_Grid
#End Region

    Public Function GetFileDuration(ByVal Filename As String) As Double
        Dim ss As FlvMetaInfo

        Dim rr As New FLVMetaData()

        Dim file As String = "videos/" & Filename

        ss = rr.GetFlvMetaInfo(Server.MapPath(file))

        Dim dur As Double = ss.Duration
        Return dur
    End Function

    Private Sub PopulateRootLevel()
        Dim rootNode As New TreeNode
        rootNode.Text = "Categories"
        rootNode.Value = "null"
        trvwCategory.Nodes.Add(rootNode)

        Dim objConn As New SqlConnection(strPathDb)

        Dim objCommand As New SqlCommand("SELECT sc.Category_ID,sc.Category_Name, sc.Parent_ID,(SELECT COUNT(*) FROM M_Category WHERE (Parent_ID= sc.Category_ID)) AS childnodecount FROM M_category AS sc where parent_id=0 order by sc.Category_Name ", objConn)
        Dim da As New SqlDataAdapter(objCommand)
        Dim dt As New DataTable()

        da.Fill(dt)

        PopulateNodes(dt, trvwCategory.Nodes)
    End Sub

    Private Sub PopulateNodes(ByVal dt As DataTable, ByVal nodes As TreeNodeCollection)
        For Each dr As DataRow In dt.Rows
            Dim tn As New TreeNode()
            tn.Value = dr("Category_Id").ToString()
            tn.Text = dr("Category_Name").ToString()
            nodes.Add(tn)
            'If node has child nodes, then enable on-demand populating
            tn.PopulateOnDemand = (CInt(dr("childnodecount")) > 0)
        Next

    End Sub



    Private Sub trvwCategory_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles trvwCategory.TreeNodePopulate
        Dim ParentCatagoryID As Integer = Int32.Parse(e.Node.Value)
        PopulateSubLevel(ParentCatagoryID, e.Node)

    End Sub
    Private Sub PopulateSubLevel(ByVal ParentCatagoryID As Integer, ByVal parentNode As TreeNode)
        Dim objConn As New SqlConnection(strPathDb)

        Dim objCommand As New SqlCommand("SELECT sc.Category_ID,sc.Category_Name, sc.Parent_ID,(SELECT COUNT(*) FROM M_Category WHERE (Parent_ID= sc.Category_ID)) AS childnodecount FROM M_category AS sc where parent_id=" & ParentCatagoryID & " order by sc.Category_Name ", objConn)
        Dim da As New SqlDataAdapter(objCommand)
        Dim dtSub As New DataTable()

        da.Fill(dtSub)
        PopulateNodes(dtSub, parentNode.ChildNodes)
    End Sub

    Public Function FileExists(ByVal ID As String) As String
        Dim arLst As New ArrayList()
        For Each sFile As String In System.IO.Directory.GetFiles(Server.MapPath("videos"))
            Dim sJustFile As String = New System.IO.FileInfo(sFile).Name
            arLst.Add(sJustFile)
        Next

        If arLst.Contains(ID & ".flv") Then
            Return ".flv"
        ElseIf arLst.Contains(ID & ".mp4") Then
            Return ".mp4"
        ElseIf arLst.Contains(ID & ".m4v") Then
            Return ".m4v"
        ElseIf arLst.Contains(ID & ".mp3") Then
            Return ".mp3"
        ElseIf arLst.Contains(ID & ".mpeg") Then
            Return ".mpeg"
        ElseIf arLst.Contains(ID & ".mov") Then
            Return ".mov"
        End If

    End Function

    Protected Sub LnkDelete_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bolflgs As Boolean = False

        Try
            Dim drop As LinkButton = CType(sender, LinkButton)
            Dim row As DataGridItem = CType(drop.NamingContainer, DataGridItem)
            Dim idValue As Integer = Convert.ToInt32(row.Cells(1).Text)       'here id value of clicked linkbutton will be gat)
            bolflgs = DeleteVideo(idValue) 'Call method DeleteEmployee 
            If (bolflgs) Then
                DGData.CurrentPageIndex = 0
                DisplayVideoListGrid(trvwCategory.SelectedNode.Value)
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub
#Region "DeleteVideo"

    Public Function DeleteVideo(ByVal idval As Integer) As Boolean
        Dim sbDeleteValue As StringBuilder
        Dim sr As String
        Dim cmd As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Dim i As Integer
        Dim bolflg As Boolean = False
        Try
            sbDeleteValue = New StringBuilder()
            sbDeleteValue.Append("DELETE ")
            sbDeleteValue.Append("FROM ")
            sbDeleteValue.Append("M_Video ")
            sbDeleteValue.Append("WHERE ")
            sbDeleteValue.Append("Vedio_ID")
            sbDeleteValue.Append("=")
            sbDeleteValue.Append("'")
            sbDeleteValue.Append(idval)
            sbDeleteValue.Append("'")

            sr = sbDeleteValue.ToString()
            If (objconn.connect()) Then
                cmd = New SqlCommand(sr, objconn.MyConnection)
                i = cmd.ExecuteNonQuery()
                If (i > 0) Then
                    bolflg = True
                    Dim fileName As String = GetFileExtension(Convert.ToString(idval))
                    Dim file As New IO.FileInfo(Server.MapPath("videos\" & fileName))
                    If file.Exists = True Then
                        System.IO.File.Delete(Server.MapPath("videos\" & fileName))
                    End If
                Else
                    bolflg = False
                End If
                objconn.disconnect()
            End If
            Return bolflg
        Catch ex As Exception
            If objconn.connect() Then
                objconn.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn = Nothing
            cmd = Nothing
            sbDeleteValue = Nothing
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
            If selPage >= CInt(ViewState("lastRange")) And selPage < CInt(ViewState("lastRange")) + len Then
                range = CInt(ViewState("lastRange"))
                If CInt(ViewState("lastRange")) + len >= DGData.PageCount Then
                    range = range - 1
                    If range = 0 Then
                        range = 1
                    End If
                End If

            Else

                If selPage <= DGData.PageCount Then
                    If range < CInt(ViewState("lastRange")) Then
                        range = CInt(ViewState("lastRange")) - 1
                    Else
                        If selPage - len > 0 And selPage - len <= DGData.PageCount - len Then
                            range = selPage - len
                        Else
                            range = CInt(ViewState("lastRange")) + 1
                        End If
                    End If
                    If selPage > DGData.PageCount Then
                        range = DGData.PageCount - (len)
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
        'End If

        If selPage = 1 Then
            imgprev.Enabled = False
            imgfirst.Enabled = False
        End If
        If selPage = DGData.PageCount Then
            imgnext.Enabled = False
            imglast.Enabled = False
        End If
        DGData.CurrentPageIndex = CInt(ViewState("pageNo")) - 1

        ViewState("selval") = Nothing

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
        DisplayVideoListGrid(trvwCategory.SelectedNode.Value)
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
                    '    ViewState.Add("selval", DGReport.CurrentPageIndex)
                End If

            Case "prev" 'The prev button was clicked
                If (DGData.CurrentPageIndex > 0) Then
                    DGData.CurrentPageIndex -= 1

                End If

            Case "last" 'The Last Page button was clicked
                DGData.CurrentPageIndex = (DGData.PageCount - 1)

            Case Else 'The First Page button was clicked
                DGData.CurrentPageIndex = Convert.ToInt32(arg) - 1

        End Select

        ViewState.Add("pageNo", DGData.CurrentPageIndex + 1)
        ViewState.Add("selval", DGData.CurrentPageIndex)
        DisplayVideoListGrid(trvwCategory.SelectedNode.Value)

    End Sub

    'Added by Pranit on 02/12/2019
    Sub Selection_Change(sender As Object, e As EventArgs)
        Try
            DGData.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
            DisplayVideoListGrid(trvwCategory.SelectedNode.Value)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try '
    End Sub

    'Public Sub fillPagesCombo()
    '    ddlPages.Items.Clear()
    '    For cnt As Integer = 1 To DGData.PageCount
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
    '    DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    DisplayVideoListGrid(trvwCategory.SelectedNode.Value)
    'End Sub

    Protected Sub DGData_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGData.ItemDataBound
        If Not e.Item.ItemType = DataControlRowType.Header Then
            e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
        End If
    End Sub

    Protected Sub DGData_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGData.PageIndexChanged
        Try
            DGData.CurrentPageIndex = e.NewPageIndex
            DisplayVideoListGrid(trvwCategory.SelectedNode.Value)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
        End Try
    End Sub
End Class
#End Region