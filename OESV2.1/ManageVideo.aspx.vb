
#Region "NameSpaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
Imports System.Drawing

#End Region
Partial Public Class ManageVideo
    Inherits BasePage

#Region "Declaration"

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("managevideo")
    Dim objconn As New ConnectDb
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")


#End Region
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Dim tn As TreeNode = trvwCategory.SelectedNode
        'If Not tn Is Nothing Then
        '    Page.Title = tn.Value & "   " & tn.Text
        'End If

        If Not IsPostBack Then
            PopulateRootLevel()
            'FillTreeView()
            'createCategoryTree()
        End If
    End Sub
    Protected Sub trvwCategory_SelectedNodeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles trvwCategory.SelectedNodeChanged
        txtCat.Text = trvwCategory.SelectedNode.Text
        '   createCategoryTree(trvwCategory.SelectedNode.Value.ToString)

    End Sub
    'Added by Pranit on 07/11/2019
    Protected Sub imgbtnAddCat_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgbtnAddCat.Click
        Try
            Dim tn1 As TreeNode = trvwCategory.SelectedNode

            If Not tn1 Is Nothing Then
                If trvwCategory.SelectedNode.Value = "null" Then
                    If txtNewCat.Text <> "" Then
                        AddNewCategory("0", txtNewCat.Text)
                        lblMsg.ForeColor = Color.Green
                        lblMsg.Text = Resources.Resource.ManVideo_CatAddSucc
                        txtNewCat.Text = ""
                        trvwCategory.Nodes.Clear()
                        PopulateRootLevel()
                    Else
                        lblMsg.ForeColor = Color.Red
                        lblMsg.Text = Resources.Resource.ManVideo_EnNCatName
                    End If
                    Exit Sub
                End If

                If Not trvwCategory.SelectedValue = Nothing Then
                    'If trvwCategory.SelectedNode.Value = "null" Then
                    '    lblMsg.Text = "Please select  a category."
                    '    Exit Sub
                    'End If
                    If txtNewCat.Text <> "" Then
                        AddNewCategory(tn1.Value.ToString, txtNewCat.Text)
                        Dim tn As New TreeNode(txtNewCat.Text, trvwCategory.SelectedNode.ChildNodes.Count)
                        trvwCategory.SelectedNode.ChildNodes.Add(tn)
                        txtNewCat.Text = ""
                        lblMsg.ForeColor = Color.Green
                        lblMsg.Text = Resources.Resource.ManVideo_CatAdd
                        trvwCategory.Nodes.Clear()
                        PopulateRootLevel()
                    Else
                        lblMsg.ForeColor = Color.Red
                        lblMsg.Text = Resources.Resource.ManVideo_EnCatName
                    End If
                Else
                    lblMsg.ForeColor = Color.Red
                    lblMsg.Text = Resources.Resource.ManVideo_SelCat
                End If
            Else
                lblMsg.ForeColor = Color.Red
                lblMsg.Text = Resources.Resource.ManVideo_SelAnCat
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally

        End Try
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
                    '  createCategoryTree(ds.Tables(0).Rows(i).Item(0).ToString)
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
    'Added by Pranit on 07/11/2019
    Protected Sub imgbtnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgbtnSubmit.Click
        Dim query As String = ""
        Dim cmd As SqlCommand
        Dim trv As TreeNode = trvwCategory.SelectedNode
        Dim sqlTrans As SqlTransaction

        If trv Is Nothing Then
            lblMsg.ForeColor = Color.Red
            lblMsg.Text = Resources.Resource.ManVideo_SelCat
            Exit Sub
        End If

        If txtTitle.Text = "" Then
            lblMsg.ForeColor = Color.Red
            lblMsg.Text = Resources.Resource.ManVideo_GiSoTitle
            Exit Sub
        End If


        If Not Uploader.HasFile Then
            lblMsg.ForeColor = Color.Red
            lblMsg.Text = Resources.Resource.ManVideo_SelFile
            Exit Sub
        End If

        Try

            objconn = New ConnectDb
            If objconn.connect() = True Then
                sqlTrans = objconn.MyConnection.BeginTransaction
                If Not String.IsNullOrEmpty(Uploader.FileName) Then
                    Dim s() As String = Uploader.FileName.ToString.Split(".")


                    'query = "insert into M_Video (Category_ID,Title,extension) Values ('" & trvwCategory.SelectedNode.Value & "',N'" & txtTitle.Text & "','" & s(1) & "')"
                    'cmd = New SqlCommand(query, objconn.MyConnection, sqlTrans)
                    'cmd.ExecuteNonQuery()
                    'Dim newid As String = getMaxID("M_Video", "Vedio_ID", objconn.MyConnection, sqlTrans)
                    'Uploader.SaveAs(Server.MapPath("videos/" & newid & SubString(4, 3, Uploader.FileName.ToString())))
                    'sqlTrans.Commit()
                    'lblMsg.ForeColor = Color.Green
                    'If s(1).ToString = "ppt" Then
                    '    lblMsg.Text = "ppt file uploaded successfully."
                    'Else
                    '    lblMsg.Text = "Video uploaded successfully."
                    'End If

                    ' --------------------------------------------------------------------------
                    'Code added by Pragnesha
                    'Date: 2018/12/11
                    'Purpose: To resolve bug ID 881, Allowed to select ppt,pptx,mp4 format 
                    '--------------------------------------------------------------------------

                    lblMsg.ForeColor = Color.Green
                    If s(1).ToString = "ppt" Or s(1).ToString = "pptx" Then
                        query = "insert into M_Video (Category_ID,Title,extension) Values ('" & trvwCategory.SelectedNode.Value & "',N'" & txtTitle.Text & "','" & s(1) & "')"
                        cmd = New SqlCommand(query, objconn.MyConnection, sqlTrans)
                        cmd.ExecuteNonQuery()
                        Dim newid As String = getMaxID("M_Video", "Vedio_ID", objconn.MyConnection, sqlTrans)
                        Uploader.SaveAs(Server.MapPath("videos/" & newid & SubString(4, 3, Uploader.FileName.ToString())))
                        sqlTrans.Commit()
                        lblMsg.Text = Resources.Resource.ManVideo_PptUpSucc

                    ElseIf s(1).ToString = "mp4" Then
                        query = "insert into M_Video (Category_ID,Title,extension) Values ('" & trvwCategory.SelectedNode.Value & "',N'" & txtTitle.Text & "','" & s(1) & "')"
                        cmd = New SqlCommand(query, objconn.MyConnection, sqlTrans)
                        cmd.ExecuteNonQuery()
                        Dim newid As String = getMaxID("M_Video", "Vedio_ID", objconn.MyConnection, sqlTrans)
                        Uploader.SaveAs(Server.MapPath("videos/" & newid & SubString(4, 3, Uploader.FileName.ToString())))
                        sqlTrans.Commit()
                        lblMsg.Text = Resources.Resource.ManVideo_ViUpSucc

                    ElseIf s(1).ToString = "flv" Then
                        lblMsg.ForeColor = Color.Red
                        lblMsg.Text = Resources.Resource.ManageVideo_flv

                    Else
                        lblMsg.ForeColor = Color.Red
                        lblMsg.Text = Resources.Resource.ManVideo_SelMp4Fo
                    End If
                    'ended by pragnesha (2018/12/11)
                    '----------------------------------------------------------------------------------------------
                    txtTitle.Text = ""
                    objconn.disconnect()
                End If
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
            If Not objconn.MyConnection Is Nothing Then
                objconn.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn = Nothing
            cmd = Nothing

        End Try
    End Sub
    Public Function getMaxID(ByVal tablename As String, ByVal fieldname As String, ByVal con As SqlConnection, ByVal trans As SqlTransaction) As String
        Dim result As String = ""
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim cmd As SqlCommand
        Try
            If con.State = ConnectionState.Open Then
                cmd = New SqlCommand("select   isnull(Max(" & fieldname & "),0) from " & tablename, con, trans)
                da = New SqlDataAdapter(cmd)
                ds = New DataSet
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim cnt As Integer = CInt(ds.Tables(0).Rows(0).Item(0))
                    result = cnt.ToString
                Else
                    result = "1"
                End If
                '   
            End If
        Catch ex As Exception
            'If objconn.connect(strPathDb) Then
            '    objconn.disconnect()
            'End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            'objconn = Nothing
            da = Nothing
            ds = Nothing
        End Try
        Return result
    End Function
    Public Function SubString(ByVal index As Integer, ByVal len As Integer, ByVal inputStr As String) As String
        Dim result As String = inputStr.Substring(inputStr.Length - 4)
        Return result
    End Function
    Public Sub AddNewCategory(ByVal ParentID As String, ByVal Cat_Name As String)
        ' insert ne categorry
        objconn = New ConnectDb
        Dim query As String = "insert into M_Category (Category_Name,Parent_ID)  Values(N'" & Cat_Name & "','" & ParentID & "')"
        Dim cmd As SqlCommand
        Try
            If objconn.connect() = True Then
                cmd = New SqlCommand(query, objconn.MyConnection)
                cmd.ExecuteNonQuery()
                objconn.disconnect()
            End If

        Catch ex As Exception
            If objconn.MyConnection.State = ConnectionState.Open Then
                objconn.disconnect()
            End If
        Finally
            objconn = Nothing
            cmd = Nothing
        End Try
    End Sub
    Public Sub createCategoryTree(ByVal catid As String)
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        Dim mainHT As New Hashtable
        Dim subHT As New Hashtable
        objconn = New ConnectDb
        Dim query As String = "select * from M_Category where del_flag=0 and  Parent_ID = "
        'Dim item As DictionaryEntry
        'Dim subitem As DictionaryEntry
        Dim tnode As New TreeNode
        Dim nodeCnt As Integer = 0

        Try
            If objconn.connect() = True Then
                da = New SqlDataAdapter(query & catid, objconn.MyConnection)
                ds = New DataSet
                da.Fill(ds)
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    tnode.Value = ds.Tables(0).Rows(i).Item(0).ToString
                    tnode.Text = ds.Tables(0).Rows(i).Item(1).ToString
                    trvwCategory.SelectedNode.ChildNodes.Add(tnode)

                Next
                da = Nothing
                ds = Nothing

                objconn.disconnect()
            End If
        Catch ex As Exception
            da = Nothing
            ds = Nothing
        Finally
        End Try
    End Sub
    ''''''''''''''''
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
    'Added by Pranit on 07/11/2019
    Private Sub imgbtnDelCat_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgbtnDelCat.Click
        Dim cmd As SqlCommand
        Dim objconn As New ConnectDb
        Dim sqlTrans As SqlTransaction
        Try
            Dim tn1 As TreeNode = trvwCategory.SelectedNode
            If Not tn1 Is Nothing Then
                If trvwCategory.SelectedNode.Value = "null" Then
                    lblMsg.Text = Resources.Resource.ManVideo_RootCntdel
                    lblMsg.ForeColor = Color.Red
                    Exit Sub
                End If

                If Not trvwCategory.SelectedValue = Nothing Then
                    'get file list to be deleted
                    Dim fileArr As ArrayList = GetVideoIDs(trvwCategory.SelectedNode.Value.ToString)
                    Dim queryCAT As String = "delete from M_Category where category_Id=" & trvwCategory.SelectedNode.Value.ToString
                    Dim querySubCAT As String = "delete from M_Category where Parent_ID= " & trvwCategory.SelectedNode.Value.ToString 'category_Id in (select Category_ID from M_Category  where Parent_ID= " & trvwCategory.SelectedNode.Value.ToString & ")"
                    '    Dim queryVDO1 As String = "delete from M_Video where category_Id=" & trvwCategory.SelectedNode.Value.ToString
                    '   Dim queryVDO2 As String = "select * from M_Video where category_Id in (select category_Id from M_Category where Parent_ID= " & trvwCategory.SelectedNode.Value.ToString & ")"
                    Dim queryVDO1 As String = "delete from M_Video where category_Id=" & trvwCategory.SelectedNode.Value.ToString
                    Dim queryVDO2 As String = "delete from M_Video where category_Id in (select category_Id from M_Category where Parent_ID= " & trvwCategory.SelectedNode.Value.ToString & ")"


                    If objconn.connect() = True Then
                        sqlTrans = objconn.MyConnection.BeginTransaction
                        cmd = New SqlCommand(queryVDO1, objconn.MyConnection, sqlTrans)
                        cmd.ExecuteNonQuery()
                        cmd = Nothing
                        cmd = New SqlCommand(queryVDO2, objconn.MyConnection, sqlTrans)
                        cmd.ExecuteNonQuery()
                        cmd = Nothing
                        cmd = New SqlCommand(querySubCAT, objconn.MyConnection, sqlTrans)
                        cmd.ExecuteNonQuery()
                        cmd = Nothing
                        cmd = New SqlCommand(queryCAT, objconn.MyConnection, sqlTrans)
                        cmd.ExecuteNonQuery()


                        For i As Integer = 0 To fileArr.Count - 1
                            ' To delete file physically
                            '  Dim extension As String = FileExists(fileArr(i))
                            Dim fileName As String = GetFileExtension(fileArr(i))

                            Dim file As New IO.FileInfo(Server.MapPath("videos\" & fileName))
                            If file.Exists = True Then
                                System.IO.File.Delete(Server.MapPath("videos\" & fileName))
                            End If

                        Next

                        lblMsg.ForeColor = Color.Green
                        lblMsg.Text = Resources.Resource.ManVideo_CatDelSucc
                        trvwCategory.Nodes.Clear()


                        sqlTrans.Commit()

                        objconn.disconnect()
                        PopulateRootLevel()
                    End If

                Else
                    lblMsg.ForeColor = Color.Red
                    lblMsg.Text = Resources.Resource.ManVideo_SelCat
                End If
            Else
                lblMsg.ForeColor = Color.Red
                lblMsg.Text = Resources.Resource.ManVideo_SelCat
            End If

        Catch ex As Exception
            sqlTrans.Rollback()
        End Try
    End Sub
    'Added by Pranit on 07/11/2019
    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ImageButton1.Click
        txtCat.Text = ""
        txtNewCat.Text = ""
        txtTitle.Text = ""
        trvwCategory.Nodes.Clear()
        PopulateRootLevel()
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
        End If

    End Function
    Public Function GetVideoIDs(ByVal MainID As String) As ArrayList
        Dim ar As New ArrayList
        Dim objconn As New ConnectDb
        Dim ds As New DataSet
        Dim da As SqlDataAdapter
        Dim strBldr As New StringBuilder

        'strBldr.Append(" select v.vedio_id,v.category_id from m_video as v ")
        'strBldr.Append(" inner join m_category as c ")
        'strBldr.Append(" on c.category_id=v.category_id ")
        'strBldr.Append(" inner join (select category_id from m_category where parent_id= ")
        'strBldr.Append(MainID)

        'strBldr.Append(" ) as ref ")
        'strBldr.Append(" on ref.category_id=v.category_id ")

        'select * from M_Category where category_Id=276
        'select * from M_Video where category_Id in (select category_Id from M_Category where Parent_ID= 276)

        Dim query1 As String = "select * from M_Video where category_Id=" & MainID
        Dim query2 As String = "select * from M_Video where category_Id in (select category_Id from M_Category where Parent_ID= " & MainID & ")"
        Try
            If objconn.connect() = True Then
                da = New SqlDataAdapter(query1, objconn.MyConnection)
                da.Fill(ds)

                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    ar.Add(ds.Tables(0).Rows(i).Item(0).ToString)

                Next
                da = Nothing
                da = New SqlDataAdapter(query2, objconn.MyConnection)
                ds.Clear()
                da.Fill(ds)

                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    ar.Add(ds.Tables(0).Rows(i).Item(0).ToString)

                Next
                objconn.disconnect()
            End If
        Catch ex As Exception
            If objconn.MyConnection.State = ConnectionState.Open Then
                objconn.disconnect()
            End If
        Finally
            da = Nothing
            ds = Nothing
            objconn = Nothing
        End Try
        Return ar
    End Function
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
End Class