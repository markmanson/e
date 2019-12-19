#Region "NameSpaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
Imports System.Drawing

#End Region

Partial Public Class VideoLive
    Inherits BasePage

#Region "Declaration"

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("vidoelist")
    Dim objconn As New ConnectDb
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session.Add("check", "true")



        If Not IsPostBack() Then
            'FillTreeView()
            PopulateRootLevel()
        Else
            'Try
            '    Dim lbtn As LinkButton = DirectCast(sender, LinkButton)
            '    lbtn.Attributes.Add("class", "selectedVDO")
            'Catch ex As Exception

            'End Try


            Dim tn As TreeNode = trvwCategory.SelectedNode
            If Not tn Is Nothing Then
                DisplayVideoList(tn.Value)
                Dim ctrlname As String = Page.Request.Params.[Get]("__EVENTTARGET")
                If ctrlname IsNot Nothing AndAlso ctrlname <> String.Empty Then

                    Try
                        Dim lbtn As LinkButton = DirectCast(Master.FindControl(ctrlname), LinkButton)
                        ' lbtn.Attributes.Add("CssClass", "selectedVDO")
                        'lbtn.CssClass = "selectedVDO"
                        'lbtn.Attributes.Add("color", "#000000")
                        'lbtn.Attributes.Add("background-color ", "#FFFFCC ")
                        'lbtn.Attributes.Add("border-color", "Black")
                        'lbtn.Attributes.Add("border-style", "Dotted")
                        'lbtn.Attributes.Add("border-width", "1px")







                        If 1 = 1 Then

                        End If
                    Catch ex As Exception

                    End Try


                End If

            End If
        End If

    End Sub
    Public Sub FillTreeView()
        Dim ds As DataSet
        Dim da As SqlDataAdapter
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

        DisplayVideoList(trvwCategory.SelectedNode.Value)
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

        tblvdoList.Controls.Clear()

        Try
            If objconn.connect() = True Then
                ds = New DataSet
                da = New SqlDataAdapter("select * from M_Video where  Category_ID=" & CategoryID & " order by title", objconn.MyConnection)
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
                        'lbtn.Attributes.Add("OnClick","ChangeStyle(" & lbtn.ClientID & ")")
                        ' On Postback if the button was clicked , the css is applied to it.
                        'Dim ctrlname As String = Page.Request.Params.[Get]("__EVENTTARGET")
                        'If ctrlname IsNot Nothing AndAlso ctrlname <> String.Empty Then
                        '    If lbtn.ID = ctrlname Then
                        '        lbtn.CssClass = "selectedVDO"
                        '    End If
                        'End If

                        '    Dim extension As String = FileExists(ds.Tables(0).Rows(i).Item(0).ToString)
                        Dim fileName As String = GetFileExtension(ds.Tables(0).Rows(i).Item(0).ToString)

                        If Not fileName Is Nothing Then

                            ' duration = GetFileDuration(fileName)
                            If Not fileName.EndsWith(".ppt") Then
                                'lbtn.Attributes.Add("OnClick", "return playSelected('videos/" & fileName & "'," & ds.Tables(0).Rows(i).Item(0).ToString & ",'" & ds.Tables(0).Rows(i).Item(2).ToString & "');")
                                AddHandler lbtn.Click, AddressOf Me.LinkButton1_Click
                            End If

                            '      duration = GetFileDuration(ds.Tables(0).Rows(i).Item(0).ToString & ".flv")
                            'OnClientClick="return playSelected('/videos/bunny.flv');"
                            'lbtn.Attributes.Add("OnClick", "return playSelected('/videos/" & ds.Tables(0).Rows(i).Item(0).ToString & ".flv'," & duration.ToString & ");")
                            '  lbtn.Attributes.Add("OnClientClick", "return playSelected('/videos/" & ds.Tables(0).Rows(i).Item(0).ToString & ".flv');")
                        End If
                        td = New HtmlTableCell
                        td.InnerHtml = "<img src='images/Nav-Blue_right.jpg' /> "
                        td.Controls.Add(lbtn)
                        tr = New HtmlTableRow
                        tr.Cells.Add(td)
                        tblvdoList.Rows.Add(tr)
                        lbtn = Nothing
                        tr = Nothing
                        td = Nothing
                    Next
                Else
                    td = New HtmlTableCell
                    td.InnerHtml = "No video found"
                    tr = New HtmlTableRow
                    tr.Cells.Add(td)
                    tblvdoList.Rows.Add(tr)
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs)
        Dim strLinkID As String = sender.id
        Dim fileName As String = "videos/" & GetFileExtension(strLinkID)
        Dim fileTitle As String = "Currently playing: " + sender.text
        LoadVDO(fileName, fileTitle)
    End Sub
    Public Sub LoadVDO(ByVal filename As String, ByVal filetitle As String)
        VDO.Attributes.Add("src", filename)
        LblTitle.Text = filetitle
    End Sub
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
        ' PopulateNodes(dtSub, trvwCategory.Nodes)
        'Your sublevel Datatable ie. dtSub
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
        ElseIf arLst.Contains(ID & ".wmv") Then
            Return ".wmv"
        End If


    End Function
End Class