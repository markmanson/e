#Region "Namespace"
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports log4net
Imports System.IO
Imports System.Drawing
#End Region
Namespace unirecruite
    Partial Class BulkQuestionsInsert
        Inherits BasePage
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(("question_paper"))

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()

        End Sub

#End Region
#Region "Variable"

        Shared flg As Boolean = False
        Dim sqlcon, sqlstr As String
        Dim sqlTrans As SqlTransaction
        Dim objdataset1, dsCheckQue As DataSet
        Dim con, con1, con2 As SqlConnection
        Dim cmd As SqlCommand
        Dim i As Integer = 0
        Dim quesid, paraid As Integer
        Dim dr As SqlDataReader
        Dim j As Integer = 0
        Dim k As Integer = 0
        Dim userid, campusid As String
        Dim objda As SqlDataAdapter
        Dim flag As Boolean = False
        Dim mytable1 As DataTable
        Dim col1, col2 As DataColumn
        Dim duplicate As Integer = 0
        Dim inserted As Integer = 0
        Dim total As Integer = 0
        Dim connStr As String
        Dim b As New Random
        Dim adapOleDb As OleDbDataAdapter
        Dim adapSql As New SqlDataAdapter
        Dim dsOledb As New DataSet
        Dim strTemp, query, strTestType As String
        Dim rows As DataRow
        Dim intTempRow, intTempCol, intFirstNameRow, intFirstNameCol As Integer



#End Region
#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                'Session.Remove("sname")
                If Not Session("sname") Is Nothing Then
                    Session.Remove("sname")
                End If
                If Not Session("ques") Is Nothing Then
                    Session.Remove("ques")
                End If

                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx")
                End If
                'If Session("UniUserType").ToString <> "1" Then ' commented by pragnesha for super admin
                If Convert.ToString(Session("UniUserType")) > "2" Then
                    Response.Redirect("~\register.aspx", False)
                End If
                lblCsvFormat.Visible = False
                If Not IsPostBack Then
                    tblrow2.Visible = False
                End If
                If Request.QueryString.Count = 0 Then
                    Response.Redirect("SearchQuestion.aspx")
                Else
                    strTestType = Request.QueryString("Test_Type").ToString
                End If 'Pranit
                If IsPostBack Then
                    If DataGrid1.Visible = True Then
                        'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)
                        tblpages.Visible = True
                    Else
                        tblpages.Visible = False
                    End If

                End If
                ' This Condition Added by Rajesh 2014-05-29
                ' For the Loading image when Click on the Import Data 
                If Not IsPostBack Then
                    If MyFile.HasFile Then
                        Dim script As String = "$(document).ready(function () { $('[id*=imgBtnImpData]').click(); });"
                        ClientScript.RegisterStartupScript(Me.GetType, "load", script, True)
                    End If
                End If


                ' btnImport.Attributes.Add("OnClick", "return Validate()")
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
            End Try
        End Sub
#End Region
#Region "Display Label Message"
        Public Sub labelmessage()
            Try
                If duplicate + inserted = 0 Then
                    'lblMessage.Text = "No Data was found in the file."
                    lblMessage.Text = Resources.Resource.BulQImpo_FiAlImpo
                    'Add By Bhasker(01/12/2009)
                    '************* Start *****************
                    'btnDetails.Enabled = False
                    lblMessage.ForeColor = Color.Red
                    DataGrid1.Visible = False
                    '************* End *****************
                    Exit Sub
                End If
                If duplicate + inserted = Convert.ToString(Convert.ToInt16(duplicate)) Then
                    lblMessage.Text = Resources.Resource.BulQImpo_FiAlImpo
                    'Add By Bhasker(01/12/2009)
                    '************* Start *****************
                    'btnDetails.Enabled = False
                    lblMessage.ForeColor = Color.Red
                    DataGrid1.Visible = False
                    '************* End ********************
                Else
                    lblMessage.Text = Resources.Resource.BulQImpo_DataImpoSucc
                    lblMessage.ForeColor = Color.Green
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = Resources.Resource.BulQImpo_ErrCont
            End Try
        End Sub
#End Region
#Region "Open Connection1"
        Public Sub connectionopen()
            Try
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = "An Error was occured Please contact the technical person"
            End Try
        End Sub
#End Region
#Region "Close Connection1"
        Public Sub connectionclose()
            Try
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = "An Error was occured Please contact the technical person"
            Finally
            End Try
        End Sub
#End Region
#Region "Close Connection2"
        Public Sub connection2close()
            Try
                If con2.State = ConnectionState.Open Then
                    con2.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = "An Error was occured Please contact the technical person"
            Finally
            End Try
        End Sub
#End Region

#Region "New Method Generate New Quesition ID"
        Public Function GetNewQuestionID(ByVal test_type As String) As String
            Dim id As String = ""
            Try
                sqlcon = ConfigurationSettings.AppSettings("PathDb")
                con1 = New SqlConnection(sqlcon)
                sqlstr = "SELECT MAX(qnid) as questionid FROM m_question WHERE test_type = '" & test_type & "'"
                If con1.State = ConnectionState.Closed Then
                    con1.Open()
                End If
                cmd = New SqlCommand(sqlstr, con1)
                dr = cmd.ExecuteReader()
                While dr.Read()
                    If dr.IsDBNull(0) = True Then
                        'quesid = 1
                        id = "1"
                    Else
                        'quesid = dr.Item(0) + 1
                        id = (dr.Item(0) + 1).ToString
                    End If
                End While
                If con1.State = ConnectionState.Open Then
                    con1.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                Throw ex
            Finally
                con1 = Nothing
                cmd = Nothing
                sqlstr = Nothing
                dr = Nothing
            End Try
            Return id
        End Function
#End Region


#Region "Generate Random Quesition ID"
        Public Sub generatequestionid()
            Try
                sqlcon = ConfigurationManager.AppSettings("PathDb")
                con1 = New SqlConnection(sqlcon)
                sqlstr = "SELECT MAX(qnid) as questionid FROM m_question WHERE test_type = '" & strTestType & "'"
                If con1.State = ConnectionState.Closed Then
                    con1.Open()
                End If
                cmd = New SqlCommand(sqlstr, con1)
                dr = cmd.ExecuteReader()
                While dr.Read()
                    If dr.IsDBNull(0) = True Then
                        quesid = 1
                    Else
                        quesid = dr.Item(0) + 1
                    End If
                End While
                If con1.State = ConnectionState.Open Then
                    con1.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                Throw ex
            Finally
                con1 = Nothing
                cmd = Nothing
                sqlstr = Nothing
                dr = Nothing
            End Try
        End Sub
#End Region
#Region "Import Button Click"

        'Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        '    'Try
        '    '    DataGrid1.Visible = True
        '    '    tbl_feedback.Rows(2).Visible = True
        '    'Catch ex As Exception
        '    '    If log.IsDebugEnabled Then
        '    '        log.Debug("Error :" & ex.ToString)
        '    '    End If
        '    '    lblCsvFormat.Text = "An Error was occured Please contact the technical person"
        '    'End Try
        'End Sub
#End Region
#Region "DropDown Selected Index Chnange"
        Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim drop As DropDownList = CType(sender, DropDownList)
            Session("value") = drop.SelectedValue
            Dim dd As DataTable
            Dim strRowFilter As String = ""
            Try
                dd = New DataTable
                dd = Session("DuplicatedData")
                If drop.SelectedItem.Value = 0 Then
                    dd.DefaultView.RowFilter = "criteria = 'Inserted Records' or criteria = 'Duplicate Records'"
                    dd.DefaultView.Sort = "name"
                    Session("DuplicateDataFiltered") = dd
                    flag = True
                    DataGrid1.DataSource = dd.DefaultView
                    DataGrid1.CurrentPageIndex = 0
                    DataGrid1.DataBind()
                    Response.Write(strRowFilter)
                Else
                    strRowFilter = dd.Columns("criteria").ColumnName & " = '" & drop.SelectedItem.Text & "'"
                    dd.DefaultView.RowFilter = strRowFilter
                    dd.DefaultView.Sort = "name"
                    Session("DuplicateDataFiltered") = dd
                    DataGrid1.DataSource = dd.DefaultView
                    DataGrid1.CurrentPageIndex = 0
                    DataGrid1.DataBind()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = "An Error was occured Please contact the technical person"
            Finally
                dd = Nothing
                strRowFilter = Nothing
            End Try
        End Sub
#End Region
#Region "DataGrid Page Index Changed"
        ' [Func] <This function changes the index of the page>
        Private Sub DataGrid1_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged
            Try
                Dim ds As New DataTable
                DataGrid1.CurrentPageIndex = e.NewPageIndex
                If flag = False Then
                    ds = Session("DuplicatedData")
                ElseIf flag = True Then
                    ds = Session("DuplicateDataFiltered")
                End If

                DataGrid1.DataSource = ds
                DataGrid1.DataBind()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = "An Error was occured Please contact the technical person"
            Finally
            End Try
        End Sub
#End Region
#Region "Back Button Click"
        'Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        '    'Try
        '    '    Response.Redirect("SearchQuestion.aspx")
        '    'Catch ex As Exception
        '    '    If log.IsDebugEnabled Then
        '    '        log.Debug("Error :" & ex.ToString)
        '    '    End If
        '    '    lblCsvFormat.Text = "An Error was occured Please contact the technical person"
        '    'Finally
        '    'End Try
        'End Sub
#End Region
        Protected Function RetrunDate()
            Dim strNow As DateTime
            strNow = DateTime.Now
            Dim strdate As String

            strdate = strNow.Year.ToString + "_" + strNow.Month.ToString + "_" + strNow.Day.ToString + "_" + strNow.Hour.ToString + "_" + strNow.Minute.ToString + "_" + strNow.Second.ToString + "_" + strNow.Millisecond.ToString

            Return strdate
        End Function
#Region "Import Button Clicked --Comment"
        ' [ID] <BtnImpData_Click>
        ' [Func] <This Btn gets excel file from user and enter the data into Sql>
        ' [Note] <Excel Template "Unirecruite Bulk Question Import Format" is inside the folder>
        ' [Date] 2009/03/05 by Vathsal Ravi
        'Protected Sub BtnImpData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImpData.Click
        '    'Dim intDiffLevel As Integer
        '    'Dim strQuestion As String
        '    'Dim strOptions As String
        '    'Dim strDiffLevel, strCheckQue, strCorrectID, strQue, strFileName, strQueType, strQuePara As String
        '    'Dim adapSqlCheckQue As SqlDataAdapter
        '    'Dim strExt
        '    'Dim dtCheckQue As DataTable
        '    'Dim objCommon As CommonFunction
        '    'objCommon = New CommonFunction()
        '    'Try
        '    '    dtCheckQue = New DataTable
        '    '    dsCheckQue = New DataSet
        '    '    mytable1 = New DataTable
        '    '    mytable1.Columns.Add(New DataColumn("Name", GetType(String)))
        '    '    mytable1.Columns.Add(New DataColumn("criteria", GetType(String)))
        '    '    sqlcon = ConfigurationSettings.AppSettings("PathDb")
        '    '    con = New SqlConnection(sqlcon)
        '    '    connectionopen()
        '    '    strTemp = MyFile.PostedFile.FileName
        '    '    If strTemp = "" Then
        '    '        lblCsvFormat.Visible = True
        '    '        'lblCsvFormat.Text = "Please browse the file before importing the data"
        '    '        lblCsvFormat.Text = unirecruite.Errconstants.FILEIMPORT
        '    '        lblCsvFormat.ForeColor = Color.Red
        '    '        Exit Sub
        '    '    End If
        '    '    strFileName = MyFile.FileName
        '    '    strFileName = RetrunDate() + "_" + strFileName



        '    '    MyFile.SaveAs(Server.MapPath("Excel Import\") + strFileName)
        '    '    strTemp = Server.MapPath("Excel Import\" + strFileName)
        '    '    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        '    '    "Data Source=" + strTemp + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""
        '    '    adapOleDb = New OleDbDataAdapter("SELECT * FROM [Questions$]", connStr)
        '    '    adapOleDb.TableMappings.Add("Table", "Excel")
        '    '    adapOleDb.Fill(dsOledb)
        '    '    While intTempRow <= dsOledb.Tables(0).Rows.Count - 1
        '    '        For intTempCol = 0 To dsOledb.Tables(0).Columns.Count - 1
        '    '            strTemp = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
        '    '            If strTemp.ToUpper = "QUE_TYPE" Then
        '    '                intFirstNameRow = intTempRow
        '    '                intFirstNameCol = intTempCol
        '    '                Exit While
        '    '            ElseIf strTemp.ToUpper = "QUESTION" Then
        '    '            intFirstNameRow = intTempRow
        '    '            intFirstNameCol = intTempCol
        '    '                Exit While
        '    '            End If
        '    '        Next
        '    '        intTempRow = intTempRow + 1
        '    '    End While
        '    '    intTempRow = intTempRow + 1
        '    '    While intTempRow < dsOledb.Tables(0).Rows.Count
        '    '        generatequestionid()
        '    '        dtCheckQue = New DataTable
        '    '        If strTemp.ToUpper = "QUE_TYPE" Then
        '    '            strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 8).ToString
        '    '        ElseIf strTemp.ToUpper = "QUESTION" Then
        '    '            strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 6).ToString
        '    '        End If
        '    '        If strDiffLevel.ToUpper = "L" Then
        '    '            intDiffLevel = 0
        '    '        ElseIf strDiffLevel.ToUpper = "M" Then
        '    '            intDiffLevel = 1
        '    '        ElseIf strDiffLevel.ToUpper = "H" Then
        '    '            intDiffLevel = 2
        '    '        End If
        '    '        If strTemp.ToUpper = "QUE_TYPE" Then
        '    '            strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString)
        '    '            strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
        '    '            strQueType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
        '    '            strQuePara = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
        '    '        ElseIf strTemp.ToUpper = "QUESTION" Then
        '    '            strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString)
        '    '            strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString
        '    '            'strQueType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
        '    '            strQuePara = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
        '    '        End If

        '    '        If strQue = "" Then
        '    '            Exit While
        '    '        End If
        '    '        strQuestion = Replace(strQue, "'", """")
        '    '        'Added By Bhasker(01-12-09)
        '    '        '*********** Start ****************
        '    '        strQuestion = objCommon.checkString(strQuestion)
        '    '        '*********** End ****************
        '    '        sqlstr = "select question from m_question where question ='" + Trim(strQuestion) + "' and test_type='" + strTestType + "'"
        '    '        If strDiffLevel.ToUpper <> "" Then
        '    '            sqlstr += "and qlevel=" + intDiffLevel.ToString + ""
        '    '        End If
        '    '        cmd = New SqlCommand(sqlstr, con)
        '    '        adapSqlCheckQue = New SqlDataAdapter(cmd)
        '    '        adapSqlCheckQue.Fill(dtCheckQue)

        '    '        If dtCheckQue.Rows.Count <> 0 Then
        '    '            If dtCheckQue.Rows(0)(0).ToString = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString Then
        '    '                rows = mytable1.NewRow
        '    '                rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString
        '    '                rows(1) = "Duplicate Records"
        '    '                mytable1.Rows.Add(rows)
        '    '                duplicate += 1
        '    '            End If
        '    '        Else
        '    '            rows = mytable1.NewRow
        '    '            rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString
        '    '            rows(1) = "Inserted Records"
        '    '            inserted += 1
        '    '            mytable1.Rows.Add(rows)

        '    '            If strQueType = "WP" Then
        '    '                If Session("Para") <> Nothing Then
        '    '                    If Session("Para").ToString <> strQuePara And strQuePara <> "" Then
        '    '                        Session("Para") = strQuePara
        '    '                        generateParagraphid()
        '    '                        sqlstr = "insert into m_paragraph (Paragraph_Id,Paragraph) values ('" + paraid.ToString + "','" + strQuePara + "')"
        '    '                        cmd = New SqlCommand(sqlstr, con)
        '    '                        cmd.ExecuteNonQuery()
        '    '                    End If
        '    '                Else
        '    '                    Session("Para") = strQuePara
        '    '                    generateParagraphid()
        '    '                    sqlstr = "insert into m_paragraph (Paragraph_Id,Paragraph) values ('" + paraid.ToString + "','" + strQuePara + "')"
        '    '                    cmd = New SqlCommand(sqlstr, con)
        '    '                    cmd.ExecuteNonQuery()
        '    '                End If
        '    '            End If

        '    '            If strQueType = "WP" Then
        '    '                strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString, "'", """")
        '    '                'Added By Bhasker(01-12-09)
        '    '                '*********** Start ****************
        '    '                strQuestion = objCommon.checkString(strQuestion)
        '    '                '*********** End ******************
        '    '                sqlstr = "insert into m_question (qnid,question,correct_ansid,test_type,qlevel,Paragraph_Id) values ('" & quesid & "','" & Trim(strQuestion) & "','" & dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString & "','" & strTestType & "','" & intDiffLevel & "','" + paraid.ToString + "')"
        '    '            ElseIf strQueType = "WOP" Then
        '    '                strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString, "'", """")
        '    '                'Added By Bhasker(01-12-09)
        '    '                '*********** Start ****************
        '    '                strQuestion = objCommon.checkString(strQuestion)
        '    '                '*********** End ******************
        '    '                sqlstr = "insert into m_question (qnid,question,correct_ansid,test_type,qlevel) values ('" & quesid & "','" & Trim(strQuestion) & "','" & dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString & "','" & strTestType & "','" & intDiffLevel & "')"
        '    '            Else
        '    '                strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString, "'", """")
        '    '                'Added By Bhasker(01-12-09)
        '    '                '*********** Start ****************
        '    '                strQuestion = objCommon.checkString(strQuestion)
        '    '                '*********** End ******************
        '    '                sqlstr = "insert into m_question (qnid,question,correct_ansid,test_type,qlevel) values ('" & quesid & "','" & Trim(strQuestion) & "','" & dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString & "','" & strTestType & "','" & intDiffLevel & "')"
        '    '            End If
        '    '            cmd = New SqlCommand(sqlstr, con)
        '    '            cmd.ExecuteNonQuery()
        '    '            For j As Integer = 1 To 4 Step 1
        '    '                If strQueType = "WP" Then
        '    '                    strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j + 2).ToString(), "'", """")
        '    '                    'Added By Bhasker(01-12-09)
        '    '                    '*********** Start ****************
        '    '                    strOptions = objCommon.checkString(strOptions)
        '    '                    '************ End *****************
        '    '                    sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "','" & Trim(strOptions) & "','" & strTestType & "')"
        '    '                ElseIf strQueType = "WOP" Then
        '    '                    strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j + 2).ToString(), "'", """")
        '    '                    'Added By Bhasker(01-12-09)
        '    '                    '*********** Start ****************
        '    '                    strOptions = objCommon.checkString(strOptions)
        '    '                    '************ End *****************
        '    '                    sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "','" & Trim(strOptions) & "','" & strTestType & "')"
        '    '                Else
        '    '                    strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j).ToString(), "'", """")
        '    '                    'Added By Bhasker(01-12-09)
        '    '                    '*********** Start ****************
        '    '                    strOptions = objCommon.checkString(strOptions)
        '    '                    '************ End *****************
        '    '                    sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "','" & Trim(strOptions) & "','" & strTestType & "')"
        '    '                End If
        '    '                cmd = New SqlCommand(sqlstr, con)
        '    '                cmd.ExecuteNonQuery()
        '    '            Next
        '    '        End If
        '    '        intTempRow = intTempRow + 1
        '    '        dtCheckQue = Nothing
        '    '    End While
        '    '    btnDetails.Enabled = True
        '    '    connectionclose()
        '    '    tbl_feedback.Rows(2).Visible = True
        '    '    lblDuplicate.Text = "Total No. Of Duplicate Records Are:" & duplicate
        '    '    lblInserted.Text = "Total No. Of Inserted Records Are:" & inserted
        '    '    lblTotal.Text = "Total No. Of Records Are:" & duplicate + inserted
        '    '    lblTotal.Visible = True
        '    '    lblDuplicate.Visible = True
        '    '    lblInserted.Visible = True
        '    '    lblSummary.Visible = True
        '    '    Session("DuplicatedData") = mytable1
        '    '    DataGrid1.DataSource = mytable1.DefaultView
        '    '    DataGrid1.DataBind()
        '    '    labelmessage()
        '    'Catch ex As Exception
        '    '    If log.IsDebugEnabled Then
        '    '        log.Debug("Error :" & ex.ToString)
        '    '    End If
        '    '    lblCsvFormat.Visible = True
        '    '    'lblCsvFormat.Text = "An Error was occured Please contact the technical person"
        '    '    lblCsvFormat.Text = unirecruite.Errconstants.ERRFILEIMPORT
        '    '    tbl_feedback.Rows(2).Visible = False
        '    '    lblDuplicate.Text = "Total No. Of Duplicate Records Are:" & duplicate
        '    '    lblInserted.Text = "Total No. Of Inserted Records Are:" & inserted
        '    '    lblTotal.Text = "Total No. Of Records Are:" & duplicate + inserted
        '    '    lblTotal.Visible = True
        '    '    lblDuplicate.Visible = True
        '    '    lblInserted.Visible = True
        '    '    lblSummary.Visible = True
        '    '    sqlstr = "Delete from m_options where Test_Type='" + strTestType + "'"
        '    '    cmd = New SqlCommand(sqlstr, con)
        '    '    cmd.ExecuteNonQuery()
        '    '    sqlstr = "Delete from m_question where Test_Type='" + strTestType + "'"
        '    '    cmd = New SqlCommand(sqlstr, con)
        '    '    cmd.ExecuteNonQuery()
        '    'Finally
        '    '    connectionclose()
        '    '    strDiffLevel = Nothing
        '    '    con = Nothing
        '    '    adapOleDb = Nothing
        '    '    cmd = Nothing
        '    '    sqlstr = Nothing
        '    '    mytable1 = Nothing
        '    '    dtCheckQue = Nothing
        '    '    adapOleDb = Nothing
        '    '    adapSql = Nothing
        '    '    strCheckQue = Nothing
        '    '    strTemp = Nothing
        '    '    strTestType = Nothing
        '    '    connStr = Nothing
        '    '    sqlcon = Nothing
        '    '    strQue = Nothing
        '    '    strCorrectID = Nothing
        '    '    strQuePara = Nothing
        '    '    strQueType = Nothing
        '    '    Session("Para") = Nothing
        '    'End Try
        'End Sub
#End Region

        Public Sub generateParagraphid()
            Try
                sqlcon = ConfigurationManager.AppSettings("PathDb")
                con1 = New SqlConnection(sqlcon)
                sqlstr = "SELECT MAX(Paragraph_Id) as questionid FROM m_paragraph"
                If con1.State = ConnectionState.Closed Then
                    con1.Open()
                End If
                cmd = New SqlCommand(sqlstr, con1)
                dr = cmd.ExecuteReader()
                While dr.Read()
                    If dr.IsDBNull(0) = True Then
                        paraid = 1
                    Else
                        paraid = dr.Item(0) + 1
                    End If
                End While
                If con1.State = ConnectionState.Open Then
                    con1.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = Resources.Resource.BulQImpo_ErrCont
            Finally
                con1 = Nothing
                cmd = Nothing
                sqlstr = Nothing
                dr = Nothing
            End Try
        End Sub


        ' [ID] <imgBtnImpData_Click>
        ' [Func] <This Btn gets excel file from user and enter the data into Sql>
        ' [Note] The buttons are changed to ImageButton , so the code changes are done accordingly.
        ' [Date] 2011/05/02 by Indarvadan Vasava
        Protected Sub imgBtnImpData_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgBtnImpData.Click
            Dim intDiffLevel As Integer
            Dim strQuestion As String
            Dim strOptions As String = ""
            Dim strDiffLevel, strCheckQue, strCorrectID, strQue, strFileName, strQueType, strQuePara As String
            Dim stropt1, stropt2, stropt3, stropt4 As String
            Dim strQsType, strMarks As String
            Dim intAnsID As Integer
            Dim intQsType As Integer
            Dim adapSqlCheckQue As SqlDataAdapter
            'Dim strExt
            Dim dtCheckQue As DataTable
            Dim objCommon As CommonFunction
            Dim cnStr As String
            ' Varible for Blanks Template
            Dim options As New ArrayList()
            Dim CorrectOPtions As New ArrayList()
            objCommon = New CommonFunction()
            lblCsvFormat.Text = ""

            ' System.Threading.Thread.Sleep(5000)


            Try
                dtCheckQue = New DataTable
                dsCheckQue = New DataSet
                mytable1 = New DataTable
                mytable1.Columns.Add(New DataColumn("Name", GetType(String)))
                mytable1.Columns.Add(New DataColumn("criteria", GetType(String)))
                sqlcon = ConfigurationManager.AppSettings("PathDb")
                con = New SqlConnection(sqlcon)
                connectionopen()
                sqlTrans = con.BeginTransaction(IsolationLevel.ReadCommitted)
                strTemp = MyFile.PostedFile.FileName
                If strTemp = "" Then
                    lblCsvFormat.Visible = True
                    lblCsvFormat.Text = Resources.Resource.BulQImpo_BrowImp
                    lblCsvFormat.ForeColor = Color.Red
                    Exit Sub
                End If
                strFileName = MyFile.FileName
                strFileName = RetrunDate() + "_" + strFileName


                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                MyFile.SaveAs(Server.MapPath("Excel Import\") + strFileName)
                strTemp = Server.MapPath("Excel Import\" + strFileName)

                If strFileName.EndsWith("xls") Then
                    cnStr = ConfigurationManager.AppSettings("xls")
                ElseIf strFileName.EndsWith("xlsx") Then
                    ' cnStr = ConfigurationSettings.AppSettings("xlsx")
                    lblCsvFormat.Text = Resources.Resource.BulQImpo_2007
                    lblCsvFormat.Visible = True
                    Exit Sub
                Else
                    lblCsvFormat.Text = Resources.Resource.BulQImpo_InvalidFile
                    lblCsvFormat.Visible = True
                    Exit Sub
                End If
                connStr = cnStr + "Data Source=" & strTemp & ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""
                'connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                '"Data Source=" + strTemp + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""
                adapOleDb = New OleDbDataAdapter("SELECT * FROM [Questions$]", connStr)
                adapOleDb.TableMappings.Add("Table", "Excel")
                adapOleDb.Fill(dsOledb)
                While intTempRow <= dsOledb.Tables(0).Rows.Count - 1



                    For intTempCol = 0 To dsOledb.Tables(0).Columns.Count - 1
                        strTemp = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                        If strTemp.ToUpper = "QUE_TYPE" Then
                            intFirstNameRow = intTempRow
                            intFirstNameCol = intTempCol
                            Exit While
                        ElseIf strTemp.ToUpper = "QUESTION" Then
                            intFirstNameRow = intTempRow
                            intFirstNameCol = intTempCol
                            Exit While
                        End If
                    Next
                    intTempRow = intTempRow + 1
                End While
                intTempRow = intTempRow + 1

                'If cols = 44 Then blanks()
                'If cols = 10 then Simple and MC

                If dsOledb.Tables(0).Columns.Count > 11 Then ' if columns greater than 10 means sheet for blank type questions
                    While intTempRow < dsOledb.Tables(0).Rows.Count
                        quesid = CInt(GetNewQuestionID(strTestType))
                        dtCheckQue = New DataTable
                        If strTemp.ToUpper = "QUE_TYPE" Then
                            strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 8).ToString
                        ElseIf strTemp.ToUpper = "QUESTION" Then
                            strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 42).ToString
                        End If
                        If strDiffLevel.ToUpper = "B" Then
                            intDiffLevel = 0
                        ElseIf strDiffLevel.ToUpper = "I" Then
                            intDiffLevel = 1
                        End If

                        options.Clear()
                        CorrectOPtions.Clear()

                        If strTemp.ToUpper = "QUE_TYPE" Then
                            strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString)
                            strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                            strQueType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            strQuePara = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
                        ElseIf strTemp.ToUpper = "QUESTION" Then
                            strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString)
                            strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString
                            For oi As Integer = 1 To 20
                                If dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + oi).ToString <> "" Then
                                    options.Add(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + oi).ToString)
                                End If
                            Next
                            For coi As Integer = 21 To 40
                                If dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + coi).ToString <> "" Then
                                    CorrectOPtions.Add(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + coi).ToString)
                                End If
                            Next
                            strQsType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                            strMarks = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 41).ToString
                        End If

                        If strQue = "" Then
                            Exit While
                        End If


                        intAnsID = 1
                        intQsType = 3

                        strQuestion = Replace(strQue, "'", """")
                        strQuestion = objCommon.checkString(strQuestion)

                        sqlstr = "select question from m_question where question =N'" + Trim(strQuestion) + "' and test_type='" + strTestType + "'"
                        If strDiffLevel.ToUpper <> "" Then
                            sqlstr += "and qlevel=" + intDiffLevel.ToString + ""
                        End If
                        cmd = New SqlCommand(sqlstr, con, sqlTrans)
                        adapSqlCheckQue = New SqlDataAdapter(cmd)
                        adapSqlCheckQue.Fill(dtCheckQue)

                        If dtCheckQue.Rows.Count <> 0 Then
                            'If dtCheckQue.Rows(0)(0).ToString = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString Then
                            rows = mytable1.NewRow
                            rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            rows(1) = "Duplicate Records"
                            mytable1.Rows.Add(rows)
                            duplicate += 1
                            'End If
                        Else
                            'rows = mytable1.NewRow
                            'rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            'rows(1) = "Inserted Records"
                            'inserted += 1
                            'mytable1.Rows.Add(rows)


                            'Insert Query in M_Question table
                            strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString, "'", """")
                            strQuestion = objCommon.checkString(strQuestion)
                            sqlstr = "insert into m_question (qnid,question,test_type,qlevel,Qn_Category_Id,Ans_Category_ID,Total_Marks)  values ('" & quesid & "',N'" & Trim(strQuestion) & "','" & strTestType & "','" & intDiffLevel & "','" & intQsType & "','" & intAnsID & "','" & strMarks & "')"
                            cmd = New SqlCommand(sqlstr, con, sqlTrans)
                            cmd.ExecuteNonQuery()

                            strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j).ToString(), "'", """")
                            strOptions = objCommon.checkString(strOptions)

                            'Insert Query For M_Options
                            For oi As Integer = 0 To options.Count - 1
                                sqlstr = "insert into m_options  values ('" & oi + 1 & "','" & quesid & "',N'" & options(oi) & "','" & strTestType & "')"
                                cmd = New SqlCommand(sqlstr, con, sqlTrans)
                                cmd.ExecuteNonQuery()
                            Next




                            'Insert Query for M_Question_Answer
                            Dim subid_Cnt As Integer = 1
                            For coi As Integer = 0 To CorrectOPtions.Count - 1
                                If CorrectOPtions(coi).ToString.Contains(",") = True Then
                                    Dim car() As String = CorrectOPtions(coi).Split(",")
                                    For k As Integer = 0 To car.Length - 1
                                        sqlstr = "insert into M_Question_Answer( Qn_ID,Sub_ID,Correct_Opt_Id,Test_Type)	Values ('" & quesid & "','" & subid_Cnt & "','" & car(k) & "', '" & strTestType & "')"
                                        cmd = New SqlCommand(sqlstr, con, sqlTrans)
                                        cmd.ExecuteNonQuery()
                                    Next
                                Else
                                    sqlstr = "insert into M_Question_Answer( Qn_ID,Sub_ID,Correct_Opt_Id,Test_Type)	Values ('" & quesid & "','" & subid_Cnt & "','" & CorrectOPtions(coi) & "', '" & strTestType & "')"
                                    cmd = New SqlCommand(sqlstr, con, sqlTrans)
                                    cmd.ExecuteNonQuery()
                                End If
                                subid_Cnt = subid_Cnt + 1
                            Next
                            subid_Cnt = 1
                            rows = mytable1.NewRow
                            rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            rows(1) = "Inserted Records"
                            inserted += 1
                            mytable1.Rows.Add(rows)

                        End If
                        intTempRow = intTempRow + 1
                        dtCheckQue = Nothing
                        sqlTrans.Commit()
                        sqlTrans = con.BeginTransaction(IsolationLevel.ReadCommitted)
                    End While
                Else
                    While intTempRow < dsOledb.Tables(0).Rows.Count

                        quesid = CInt(GetNewQuestionID(strTestType))
                        dtCheckQue = New DataTable
                        If strTemp.ToUpper = "QUE_TYPE" Then
                            strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 8).ToString
                        ElseIf strTemp.ToUpper = "QUESTION" Then
                            strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 6).ToString
                        End If
                        If strDiffLevel.ToUpper = "B" Then
                            intDiffLevel = 0
                        ElseIf strDiffLevel.ToUpper = "I" Then
                            intDiffLevel = 1
                        End If
                        If strTemp.ToUpper = "QUE_TYPE" Then
                            strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString)
                            strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                            strQueType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            strQuePara = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
                        ElseIf strTemp.ToUpper = "QUESTION" Then
                            strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString)
                            strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString
                            stropt1 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
                            stropt2 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString
                            stropt3 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 3).ToString
                            stropt4 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 4).ToString
                            strQsType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                            strMarks = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 8).ToString
                        End If

                        If strQue = "" Then
                            Exit While
                        End If

                        If strCorrectID.Contains(",") Then
                            intAnsID = 1
                        Else
                            intAnsID = 0
                        End If

                        If strQsType = "True/False or Single Answer" Then
                            intQsType = 1
                        ElseIf strQsType = "Multiple choice" Then
                            intQsType = 2
                        End If

                        strQuestion = Replace(strQue, "'", """")
                        strQuestion = objCommon.checkString(strQuestion)
                        sqlstr = "select question from m_question where question =N'" + Trim(strQuestion) + "' and test_type='" + strTestType + "'"
                        If strDiffLevel.ToUpper <> "" Then
                            sqlstr += "and qlevel=" + intDiffLevel.ToString + ""
                        End If
                        cmd = New SqlCommand(sqlstr, con, sqlTrans)
                        adapSqlCheckQue = New SqlDataAdapter(cmd)
                        adapSqlCheckQue.Fill(dtCheckQue)

                        If dtCheckQue.Rows.Count <> 0 Then
                            'Commented By Rahul Shukla on 16-10-2019
                            'Reason : It was Not Showing Duplicates Records Counting on Page.
                            'If dtCheckQue.Rows(0)(0).ToString = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString Then
                            rows = mytable1.NewRow
                            rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            rows(1) = "Duplicate Records"
                            mytable1.Rows.Add(rows)
                            duplicate += 1
                            'End If
                        Else
                            rows = mytable1.NewRow
                            rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                            rows(1) = "Inserted Records"
                            inserted += 1
                            mytable1.Rows.Add(rows)


                            If 1 = 1 Then
                                strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString, "'", """")
                                strQuestion = objCommon.checkString(strQuestion)
                                sqlstr = "insert into m_question (qnid,question,test_type,qlevel,Qn_Category_Id,Ans_Category_ID,Total_Marks)  values ('" & quesid & "',N'" & Trim(strQuestion) & "','" & strTestType & "','" & intDiffLevel & "','" & intQsType & "','" & intAnsID & "','" & strMarks & "')"
                            End If
                            cmd = New SqlCommand(sqlstr, con, sqlTrans)
                            cmd.ExecuteNonQuery()
                            For j As Integer = 1 To 4 Step 1
                                If 1 = 1 Then
                                    strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j).ToString(), "'", """")
                                    strOptions = objCommon.checkString(strOptions)
                                    sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "',N'" & Trim(strOptions) & "','" & strTestType & "')"
                                End If
                                If strOptions <> "" Then
                                    cmd = New SqlCommand(sqlstr, con, sqlTrans)
                                    cmd.ExecuteNonQuery()
                                End If
                            Next

                            ' Code to insert in M_Question_Answer
                            If intAnsID > 0 Then 'Multiple Answer
                                Dim ar() As String = strCorrectID.Split(",")
                                For k As Integer = 0 To ar.Length - 1
                                    sqlstr = "insert into M_Question_Answer( Qn_ID,Correct_Opt_Id,Test_Type)	Values ('" & quesid & "','" & ar(k) & "', '" & strTestType & "')"
                                    cmd = New SqlCommand(sqlstr, con, sqlTrans)
                                    cmd.ExecuteNonQuery()
                                Next
                            Else ' Single Answer
                                sqlstr = "insert into M_Question_Answer( Qn_ID,Correct_Opt_Id,Test_Type)	Values ('" & quesid & "','" & strCorrectID & "', '" & strTestType & "')"
                                cmd = New SqlCommand(sqlstr, con, sqlTrans)
                                cmd.ExecuteNonQuery()
                            End If

                        End If
                        intTempRow = intTempRow + 1
                        dtCheckQue = Nothing
                        sqlTrans.Commit()
                        sqlTrans = con.BeginTransaction(IsolationLevel.ReadCommitted)
                    End While

                End If

                ' ''While intTempRow < dsOledb.Tables(0).Rows.Count
                ' ''    '   generatequestionid()
                ' ''    quesid = CInt(GetNewQuestionID(strTestType))
                ' ''    dtCheckQue = New DataTable
                ' ''    If strTemp.ToUpper = "QUE_TYPE" Then
                ' ''        strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 8).ToString
                ' ''    ElseIf strTemp.ToUpper = "QUESTION" Then
                ' ''        strDiffLevel = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 6).ToString
                ' ''    End If
                ' ''    If strDiffLevel.ToUpper = "B" Then
                ' ''        intDiffLevel = 0
                ' ''    ElseIf strDiffLevel.ToUpper = "I" Then
                ' ''        intDiffLevel = 1
                ' ''    End If
                ' ''    If strTemp.ToUpper = "QUE_TYPE" Then
                ' ''        strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString)
                ' ''        strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                ' ''        strQueType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                ' ''        strQuePara = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
                ' ''    ElseIf strTemp.ToUpper = "QUESTION" Then
                ' ''        strQue = Trim(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString)
                ' ''        strCorrectID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString
                ' ''        'strQueType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                ' ''        stropt1 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString
                ' ''        stropt2 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString
                ' ''        stropt3 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 3).ToString
                ' ''        stropt4 = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 4).ToString
                ' ''        strQsType = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                ' ''        strMarks = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 8).ToString
                ' ''    End If

                ' ''    If strQue = "" Then
                ' ''        Exit While
                ' ''    End If

                ' ''    If strCorrectID.Contains(",") Then
                ' ''        intAnsID = 1
                ' ''    Else
                ' ''        intAnsID = 0
                ' ''    End If

                ' ''    If strQsType = "True/False or Single Answer" Then
                ' ''        intQsType = 1
                ' ''    ElseIf strQsType = "Multiple choice" Then
                ' ''        intQsType = 2
                ' ''    End If

                ' ''    strQuestion = Replace(strQue, "'", """")
                ' ''    'Added By Bhasker(01-12-09)
                ' ''    '*********** Start ****************
                ' ''    strQuestion = objCommon.checkString(strQuestion)
                ' ''    '*********** End ****************
                ' ''    sqlstr = "select question from m_question where question ='" + Trim(strQuestion) + "' and test_type='" + strTestType + "'"
                ' ''    If strDiffLevel.ToUpper <> "" Then
                ' ''        sqlstr += "and qlevel=" + intDiffLevel.ToString + ""
                ' ''    End If
                ' ''    cmd = New SqlCommand(sqlstr, con)
                ' ''    adapSqlCheckQue = New SqlDataAdapter(cmd)
                ' ''    adapSqlCheckQue.Fill(dtCheckQue)

                ' ''    If dtCheckQue.Rows.Count <> 0 Then
                ' ''        If dtCheckQue.Rows(0)(0).ToString = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString Then
                ' ''            rows = mytable1.NewRow
                ' ''            rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                ' ''            rows(1) = "Duplicate Records"
                ' ''            mytable1.Rows.Add(rows)
                ' ''            duplicate += 1
                ' ''        End If
                ' ''    Else
                ' ''        rows = mytable1.NewRow
                ' ''        rows(0) = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                ' ''        rows(1) = "Inserted Records"
                ' ''        inserted += 1
                ' ''        mytable1.Rows.Add(rows)

                ' ''        ''If strQueType = "WP" Then
                ' ''        ''    If Session("Para") <> Nothing Then
                ' ''        ''        If Session("Para").ToString <> strQuePara And strQuePara <> "" Then
                ' ''        ''            Session("Para") = strQuePara
                ' ''        ''            generateParagraphid()
                ' ''        ''            sqlstr = "insert into m_paragraph (Paragraph_Id,Paragraph) values ('" + paraid.ToString + "','" + strQuePara + "')"
                ' ''        ''            cmd = New SqlCommand(sqlstr, con)
                ' ''        ''            cmd.ExecuteNonQuery()
                ' ''        ''        End If
                ' ''        ''    Else
                ' ''        ''        Session("Para") = strQuePara
                ' ''        ''        generateParagraphid()
                ' ''        ''        sqlstr = "insert into m_paragraph (Paragraph_Id,Paragraph) values ('" + paraid.ToString + "','" + strQuePara + "')"
                ' ''        ''        cmd = New SqlCommand(sqlstr, con)
                ' ''        ''        cmd.ExecuteNonQuery()
                ' ''        ''    End If
                ' ''        ''End If

                ' ''        ''If strQueType = "WP" Then
                ' ''        ''    strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString, "'", """")
                ' ''        ''    'Added By Bhasker(01-12-09)
                ' ''        ''    '*********** Start ****************
                ' ''        ''    strQuestion = objCommon.checkString(strQuestion)
                ' ''        ''    '*********** End ******************
                ' ''        ''    sqlstr = "insert into m_question (qnid,question,correct_ansid,test_type,qlevel,Paragraph_Id) values ('" & quesid & "','" & Trim(strQuestion) & "','" & dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString & "','" & strTestType & "','" & intDiffLevel & "','" + paraid.ToString + "')"
                ' ''        ''ElseIf strQueType = "WOP" Then
                ' ''        ''    strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString, "'", """")
                ' ''        ''    'Added By Bhasker(01-12-09)
                ' ''        ''    '*********** Start ****************
                ' ''        ''    strQuestion = objCommon.checkString(strQuestion)
                ' ''        ''    '*********** End ******************
                ' ''        ''    sqlstr = "insert into m_question (qnid,question,correct_ansid,test_type,qlevel) values ('" & quesid & "','" & Trim(strQuestion) & "','" & dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString & "','" & strTestType & "','" & intDiffLevel & "')"
                ' ''        ''Else
                ' ''        If 1 = 1 Then
                ' ''            strQuestion = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString, "'", """")
                ' ''            'Added By Bhasker(01-12-09)
                ' ''            '*********** Start ****************
                ' ''            strQuestion = objCommon.checkString(strQuestion)
                ' ''            '*********** End ******************
                ' ''            sqlstr = "insert into m_question (qnid,question,test_type,qlevel,Qn_Category_Id,Ans_Category_ID,Total_Marks)  values ('" & quesid & "','" & Trim(strQuestion) & "','" & strTestType & "','" & intDiffLevel & "','" & intQsType & "','" & intAnsID & "','" & strMarks & "')"
                ' ''        End If
                ' ''        cmd = New SqlCommand(sqlstr, con)
                ' ''        cmd.ExecuteNonQuery()
                ' ''        For j As Integer = 1 To 4 Step 1
                ' ''            ''If strQueType = "WP" Then
                ' ''            ''    strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j + 2).ToString(), "'", """")
                ' ''            ''    'Added By Bhasker(01-12-09)
                ' ''            ''    '*********** Start ****************
                ' ''            ''    strOptions = objCommon.checkString(strOptions)
                ' ''            ''    '************ End *****************
                ' ''            ''    sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "','" & Trim(strOptions) & "','" & strTestType & "')"
                ' ''            ''ElseIf strQueType = "WOP" Then
                ' ''            ''    strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j + 2).ToString(), "'", """")
                ' ''            ''    'Added By Bhasker(01-12-09)
                ' ''            ''    '*********** Start ****************
                ' ''            ''    strOptions = objCommon.checkString(strOptions)
                ' ''            ''    '************ End *****************
                ' ''            ''    sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "','" & Trim(strOptions) & "','" & strTestType & "')"
                ' ''            ''Else
                ' ''            If 1 = 1 Then
                ' ''                strOptions = Replace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + j).ToString(), "'", """")
                ' ''                'Added By Bhasker(01-12-09)
                ' ''                '*********** Start ****************
                ' ''                strOptions = objCommon.checkString(strOptions)
                ' ''                '************ End *****************
                ' ''                sqlstr = "insert into m_options  values ('" & j & "','" & quesid & "','" & Trim(strOptions) & "','" & strTestType & "')"
                ' ''            End If
                ' ''            If strOptions <> "" Then
                ' ''                cmd = New SqlCommand(sqlstr, con)
                ' ''                cmd.ExecuteNonQuery()
                ' ''            End If

                ' ''        Next

                ' ''        ' Code to insert in M_Question_Answer
                ' ''        If intAnsID > 0 Then 'Multiple Answer
                ' ''            Dim ar() As String = strCorrectID.Split(",")
                ' ''            For k As Integer = 0 To ar.Length - 1
                ' ''                sqlstr = "insert into M_Question_Answer( Qn_ID,Correct_Opt_Id,Test_Type)	Values ('" & quesid & "','" & ar(k) & "', '" & strTestType & "')"
                ' ''                cmd = New SqlCommand(sqlstr, con)
                ' ''                cmd.ExecuteNonQuery()
                ' ''            Next
                ' ''        Else ' Single Answer
                ' ''            sqlstr = "insert into M_Question_Answer( Qn_ID,Correct_Opt_Id,Test_Type)	Values ('" & quesid & "','" & strCorrectID & "', '" & strTestType & "')"
                ' ''            cmd = New SqlCommand(sqlstr, con)
                ' ''            cmd.ExecuteNonQuery()
                ' ''        End If

                ' ''    End If
                ' ''    intTempRow = intTempRow + 1
                ' ''    dtCheckQue = Nothing
                ' ''End While

                '''''''

                imgBtnDetails.Enabled = True
                connectionclose()
                tblrow2.Visible = True
                lblDuplicate.Text = Resources.Resource.BulQImpo_TotDuRec & duplicate
                lblInserted.Text = Resources.Resource.BulQImpo_TotInRec & inserted
                lblTotal.Text = Resources.Resource.BulQImpo_TotRec & duplicate + inserted
                lblTotal.Visible = True
                lblDuplicate.Visible = True
                lblInserted.Visible = True
                lblSummary.Visible = True
                Session("DuplicatedData") = mytable1
                DataGrid1.DataSource = mytable1.DefaultView
                DataGrid1.DataBind()
                ViewState.Add("Table", mytable1)
                fillPagesCombo()
                'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)
                labelmessage()


            Catch exDir As DirectoryNotFoundException
                sqlTrans.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & exDir.ToString)
                End If
                lblCsvFormat.Visible = True
                'lblCsvFormat.Text = "An Error was occured Please contact the technical person"
                lblCsvFormat.Text = Resources.Resource.BulQImpo_ErrCont
                tblrow2.Visible = False
                'lblDuplicate.Text = "Total No. Of Duplicate Records Are:" & duplicate
                'lblInserted.Text = "Total No. Of Inserted Records Are:" & inserted
                'lblTotal.Text = "Total No. Of Records Are:" & duplicate + inserted
                'lblTotal.Visible = True
                'lblDuplicate.Visible = True
                'lblInserted.Visible = True
                'lblSummary.Visible = True
            Catch exOLEB As OleDbException
                sqlTrans.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & exOLEB.ToString)
                End If
                lblCsvFormat.Visible = True
                'lblCsvFormat.Text = "An Error was occured Please contact the technical person"
                lblCsvFormat.Text = Resources.Resource.BulQImpo_FileHavPro
                tblrow2.Visible = False
                'lblDuplicate.Text = "Total No. Of Duplicate Records Are:" & duplicate
                'lblInserted.Text = "Total No. Of Inserted Records Are:" & inserted
                'lblTotal.Text = "Total No. Of Records Are:" & duplicate + inserted
                'lblTotal.Visible = True
                'lblDuplicate.Visible = True
                'lblInserted.Visible = True
                'lblSummary.Visible = True
            Catch ex As Exception
                sqlTrans.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)

                End If
                lblCsvFormat.Visible = True
                'lblCsvFormat.Text = "An Error was occured Please contact the technical person"
                lblCsvFormat.Text = Resources.Resource.BulQImpo_ErrCont
                tblrow2.Visible = False
                lblDuplicate.Text = Resources.Resource.BulQImpo_TotDuRec & duplicate
                lblInserted.Text = Resources.Resource.BulQImpo_TotInRec & inserted
                lblTotal.Text = Resources.Resource.BulQImpo_TotRec & duplicate + inserted
                lblTotal.Visible = True
                lblDuplicate.Visible = True
                lblInserted.Visible = True
                lblSummary.Visible = True
                sqlstr = "Delete from m_options where Test_Type='" + strTestType + "'"
                cmd = New SqlCommand(sqlstr, con)
                cmd.ExecuteNonQuery()
                sqlstr = "Delete from m_question where Test_Type='" + strTestType + "'"
                cmd = New SqlCommand(sqlstr, con)
                cmd.ExecuteNonQuery()
            Finally

                options = Nothing
                CorrectOPtions = Nothing
                connectionclose()
                sqlTrans = Nothing
                strDiffLevel = Nothing
                con = Nothing
                adapOleDb = Nothing
                cmd = Nothing
                sqlstr = Nothing
                mytable1 = Nothing
                dtCheckQue = Nothing
                adapOleDb = Nothing
                adapSql = Nothing
                strCheckQue = Nothing
                strTemp = Nothing
                strTestType = Nothing
                connStr = Nothing
                sqlcon = Nothing
                strQue = Nothing
                strCorrectID = Nothing
                strQuePara = Nothing
                strQueType = Nothing
                Session("Para") = Nothing
            End Try
        End Sub


        ' [ID] <imgBtnDetails_Click>
        ' [Func] <This Btn give the detail of the import>
        ' [Note] The buttons are changed to ImageButton , so the code changes are done accordingly.
        ' [Date] 2011/03/03 by Indarvadan Vasava
        Protected Sub imgBtnDetails_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgBtnDetails.Click
            Try
                DataGrid1.Visible = True
                tblpages.Visible = True
                'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)

                tblrow2.Visible = True
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = Resources.Resource.BulQImpo_ErrCont
            End Try
        End Sub


        ' [ID] <imgBtnBack_Click>
        ' [Func] <This Btn takes the admin back to SearchQuestion.aspx>
        ' [Note] The buttons are changed to ImageButton , so the code changes are done accordingly.
        ' [Date] 2011/03/03 by Indarvadan Vasava
        Protected Sub imgBtnBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgBtnBack.Click
            Try
                flag = True

                Session("flg") = "SomeDefaultSetting"

                Response.Redirect("SearchQuestion.aspx", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString)
                End If
                lblCsvFormat.Text = Resources.Resource.BulQImpo_ErrCont
            Finally
            End Try
        End Sub

        Protected Sub DataGrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        End Sub



        'Private Sub fillPageNumbers(ByVal range As Integer, ByVal len As Integer)


        '    If (Session("last") Is Nothing) Then
        '        Session.Add("last", 1)
        '        ViewState.Add("lastRange", 1)
        '    Else
        '        ViewState("lastRange") = CInt(Session("last"))
        '    End If

        '    'If (ViewState("lastRange") Is Nothing) Then
        '    '    ViewState.Add("lastRange", 1)
        '    'End If


        '    If len >= DataGrid1.PageCount Then
        '        len = DataGrid1.PageCount - 1
        '    End If

        '    ' if search clicked again then page 1 should be selected 
        '    If DataGrid1.CurrentPageIndex = 0 Then
        '        ViewState("pageNo") = 1
        '        ViewState("lastRange") = 1
        '    End If

        '    ' Getting the currently selected page value 
        '    Dim selPage As Integer = 0
        '    If (ViewState("pageNo") <> Nothing) Then
        '        selPage = CInt(ViewState("pageNo"))
        '    Else
        '        ' selPage = 1
        '        selPage = DataGrid1.CurrentPageIndex + 1
        '    End If

        '    If (ViewState("lastRange") <> Nothing) Then

        '        '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DataGrid1.PageCount Then
        '        If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
        '            range = CInt(ViewState("lastRange"))
        '        Else
        '            'If it is the last page then resetting the page numbers
        '            ' last number - provided length
        '            'If (len + selPage) >= DataGrid1.PageCount Then
        '            '    If selPage <= len Then
        '            '        range = range
        '            '    Else
        '            '        range = DataGrid1.PageCount - len
        '            '        'Incase range becomes 0 or less than zero than setting it 1 
        '            '        If range <= 0 Then
        '            '            range = 1
        '            '        End If
        '            '    End If

        '            'Else
        '            If selPage <= DataGrid1.PageCount Then
        '                'range = range
        '                If range < CInt(ViewState("lastRange")) Then
        '                    range = CInt(ViewState("lastRange")) - 1
        '                Else
        '                    If selPage - len > 0 And selPage - len <= DataGrid1.PageCount - len Then
        '                        range = selPage - len
        '                    Else
        '                        range = CInt(ViewState("lastRange")) + 1
        '                    End If
        '                    ' range = CInt(ViewState("lastRange")) + 1
        '                End If

        '            End If
        '        End If
        '    Else
        '        range = 1
        '    End If

        '    tblPagebuttons.Rows(0).Cells.Clear()

        '    'Creating the Page numbers
        '    Dim lim As Integer = range + len
        '    For i As Integer = range To lim
        '        Dim lbtn As New LinkButton()
        '        lbtn.ID = "lbtn" & i.ToString()
        '        'lbtn.ID = i.ToString()
        '        lbtn.Text = i.ToString()
        '        lbtn.CommandName = i.ToString
        '        lbtn.CommandArgument = i.ToString
        '        AddHandler lbtn.Command, New CommandEventHandler(AddressOf PagerButtonClickLinks)
        '        Dim colorBackground As Color = Color.FromArgb(0, 71, 117)
        '        lbtn.ForeColor = colorBackground

        '        If selPage = i Then
        '            lbtn.Font.Overline = False
        '        End If


        '        Dim c1 As New HtmlTableCell()
        '        c1.Controls.Add(lbtn)
        '        tblPagebuttons.Rows(0).Cells.Add(c1)
        '    Next
        '    Session("last") = range
        '    ViewState("lastRange") = range
        '    ViewState.Add("Llimit", range)
        '    ViewState.Add("Ulimit", lim)

        '    'Setting Enable / Disable on Navigation buttons
        '    'If selPage = 1 And selPage = DataGrid1.PageCount - 1 Then
        '    'Else
        '    imgprev.Enabled = True
        '    imgfirst.Enabled = True
        '    imgnext.Enabled = True
        '    imglast.Enabled = True
        '    'End If

        '    If selPage = 1 Then
        '        imgprev.Enabled = False
        '        imgfirst.Enabled = False
        '    End If
        '    If selPage = DataGrid1.PageCount Then
        '        imgnext.Enabled = False
        '        imglast.Enabled = False
        '    End If
        'End Sub
        Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DataGrid1.CurrentPageIndex < (DataGrid1.PageCount - 1)) Then
                        DataGrid1.CurrentPageIndex += 1

                    End If

                Case "prev" 'The prev button was clicked
                    If (DataGrid1.CurrentPageIndex > 0) Then
                        DataGrid1.CurrentPageIndex -= 1
                    End If

                Case "last" 'The Last Page button was clicked
                    DataGrid1.CurrentPageIndex = (DataGrid1.PageCount - 1)

                Case Else 'The First Page button was clicked
                    DataGrid1.CurrentPageIndex = Convert.ToInt32(arg)
            End Select
            ViewState.Add("pageNo", DataGrid1.CurrentPageIndex + 1)
            ViewState.Add("selval", DataGrid1.CurrentPageIndex)
            BindGrid()
            'Now, bind the data!
            '   BindSQL()
        End Sub

        Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DataGrid1.CurrentPageIndex < (DataGrid1.PageCount - 1)) Then
                        DataGrid1.CurrentPageIndex += 1
                        '    ViewState.Add("selval", DataGrid1.CurrentPageIndex)
                    End If

                Case "prev" 'The prev button was clicked
                    If (DataGrid1.CurrentPageIndex > 0) Then
                        DataGrid1.CurrentPageIndex -= 1
                        '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                    End If

                Case "last" 'The Last Page button was clicked
                    DataGrid1.CurrentPageIndex = (DataGrid1.PageCount - 1)
                    'ViewState.Add("selval", ddlPages.SelectedItem.Value)
                Case Else 'The First Page button was clicked
                    DataGrid1.CurrentPageIndex = Convert.ToInt32(arg) - 1
                    ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
            End Select

            ViewState.Add("pageNo", DataGrid1.CurrentPageIndex + 1)
            ViewState.Add("selval", DataGrid1.CurrentPageIndex)
            BindGrid()
            'Now, bind the data!
            '   BindSQL()
        End Sub

        Public Sub fillPagesCombo()
            ddlPages.Items.Clear()
            For cnt As Integer = 1 To DataGrid1.PageCount
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

        Public Sub BindGrid()
            Dim table As DataTable = DirectCast(ViewState("Table"), DataTable)
            DataGrid1.DataSource = table.DefaultView
            DataGrid1.DataBind()
            fillPagesCombo()
            'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)


        End Sub


        Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
            DataGrid1.CurrentPageIndex = ddlPages.SelectedItem.Value
            ViewState.Add("selval", ddlPages.SelectedItem.Value)
            ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
            BindGrid()
        End Sub
    End Class
End Namespace