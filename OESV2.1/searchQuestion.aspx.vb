#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Configuration
Imports System.Web.Security
Imports log4net
Imports System.Drawing
Imports Microsoft.Office.Interop
Imports System.Threading

#End Region

Namespace unirecruite

    Partial Class searchQuestion
        Inherits BasePage

        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("searchQuestion")
#Region "Variables"
        Dim blnchanged As Boolean = False
        'Dim CONS As Constant
        Const ENCRYPT_DELIMIT = "h"
        Const ENCRYPT_KEY = 124
        Dim g_Q
        Dim objconn As New ConnectDb
        Dim myDataReader As SqlDataReader
        Dim myCommand As SqlCommand
        Dim strTypeCheck As String
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Dim intcnt As Integer
        Dim flg As Boolean
        Dim intCo As Integer
#End Region

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

#Region "Error"
        '*************On Error Go to Error Page****************
        Private Sub onErrors(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Error
            Dim Err As New CreateLog
            Try
                Err.ErrorLog(Server.MapPath("Logs/RMS"), Server.GetLastError().ToString().Trim, "searchQuestion.aspx.vb")
                Response.Redirect("error.aspx?err=Searchquestion.aspx Error On Page", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                Err = Nothing
            End Try
        End Sub
#End Region

#Region "Page Load"
        '**************************************************************************
        'Function               :   Page_Load
        '
        'Return                 :   None
        '
        'Argument               :   sender : system object
        '                           e      : event
        '
        'Explanation            :   Function call on load of form
        '                           This will generate the combo box for Test type
        '                           
        'Note                   :   None
        '**************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            '***********************************************************
            'Added by Tabrez 
            'Purpose: Disabling Forward Button functionality, so 
            'that user cannot return without logging in.
            '***********************************************************

            Try
                ' If Session("flg") Is Nothing Then
                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                ' If Session("UniUserType").ToString <> "1" Then ' commented by pragnesha for super admin
                If Convert.ToString(Session("UniUserType")) > "2" Then
                    Response.Redirect("~\register.aspx", False)
                End If

                If Session("LoginGenuine") Is Nothing Then
                    Response.Redirect("error.aspx?err=Session Timeout. Please Login to continue.", False)
                End If
                Response.Buffer = True
                Response.ExpiresAbsolute = Date.Now()
                Response.Expires = 0
                Response.CacheControl = "no-cache"
                If Not (Session("FromPage") = "Admin" Or Session("FromPage") = "Question") Then
                    Response.Redirect("login.aspx", False)
                End If
                '***********************************************************
                Dim userObj As New checkUser
                'Code Added by Pratik
                'With DGData
                '    .AllowPaging = True
                '    .PagerStyle.Mode = PagerMode.NumericPages
                '    .PageSize = 10
                'End With
                DGData.Visible = True
                'DGData.Attributes.Add("style", "table-layout:fixed")
                ' BtnRemove.Visible = True
                lblMsg.Text = ""
                If Not IsPostBack Then
                    DGData.Visible = False
                    BtnRemove.Visible = False
                    'Dim objconn As New ConnectDb
                    Dim sqlstr As String
                    'Dim myCommand As SqlCommand
                    'Dim myDataReader As SqlDataReader
                    Dim myTable
                    Dim rows As DataRow
                    'Code commented by PRatik
                    'Dim strPathDb As String
                    'strPathDb = ConfigurationManager.AppSettings("PathDb")



                    If objconn.connect() Then
                        sqlstr = "SELECT Distinct test_type,test_name FROM m_testinfo where del_flag='0' order by test_name "
                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        myDataReader = myCommand.ExecuteReader()
                        myTable = New DataTable
                        myTable.Columns.Add(New DataColumn("test_type", GetType(String)))
                        myTable.Columns.Add(New DataColumn("test_name", GetType(String)))

                        rows = myTable.NewRow
                        rows(0) = ""
                        rows(1) = Resources.Resource.searchQues_selectsub
                        myTable.Rows.Add(rows)

                        While myDataReader.Read
                            rows = myTable.NewRow
                            rows(0) = myDataReader.Item("test_type")
                            rows(1) = myDataReader.Item("test_name")
                            myTable.Rows.Add(rows)
                        End While
                        sel_test_type.DataSource = myTable
                        sel_test_type.DataValueField = "test_type"
                        sel_test_type.DataTextField = "test_name"
                        sel_test_type.EnableViewState = True
                        sel_test_type.DataBind()
                        'sel_test_type.AutoPostBack = True


                        If Session("ListItem") Is Nothing Then
                            '
                            'sel_test_type.SelectedValue = Session("ListItem")
                            'sel_test_type.SelectedIndex = 5
                        End If
                    Else
                        'sel_test_type.SelectedValue = Session("ListItem")
                        'sel_test_type.SelectedIndex = 5
                    End If

                    'Added by Bhasker(2009/11/24)
                    '**************** Start Code *************************
                    'If Not Session("sname") Is Nothing Then
                    '    sel_test_type.SelectedItem.Value = CInt(Session("sname"))
                    '    Session.Remove("sname")
                    'End If
                    'If Not Session("ques") Is Nothing Then
                    '    txt_question.Text = Session("ques").ToString
                    '    Session.Remove("ques")
                    'End If



                    Dim strData(4) As String
                    If Session("BackQuery") <> "" Then
                        strData = Convert.ToString(Session("BackQuery")).Split("|")
                        sel_test_type.SelectedValue = strData(0)
                        If Not Session("sname") Is Nothing Then
                            If Session("sname") = "" Then
                                sel_test_type.SelectedIndex = 0
                                gridDiv.Visible = False
                            Else
                                sel_test_type.SelectedValue = CInt(Session("sname"))
                                DGData.Visible = True
                                '   BtnRemove.Visible = True
                                Result_Display(strData(0))
                            End If
                            ' Session.Remove("sname")
                        Else
                            sel_test_type.SelectedIndex = 0
                            DGData.Visible = False
                            gridDiv.Visible = False
                        End If
                        If Not Session("ques") Is Nothing Then
                            txt_question.Text = Session("ques").ToString
                            DGData.Visible = True
                            Search_Result()
                            'Session.Remove("ques")
                        End If

                        ''DGData.Visible = True
                        ' ''   BtnRemove.Visible = True
                        ''Result_Display(strData(0))


                        '/************************start,Jatin Gangajaliya,2011/03/31*******************/
                        If Session("newvisible") <> Nothing Then
                            If Session("newvisible") = "true" Then
                                gridDiv.Visible = True
                                Session.Remove("newvisible")
                            ElseIf Session("newvisible") = "false" Then
                                gridDiv.Visible = False
                                Session.Remove("newvisible")
                            End If
                        End If


                        If Session("bulkvisible") <> Nothing Then
                            If Session("bulkvisible") = "true" Then
                                gridDiv.Visible = True
                                Session.Remove("bulkvisible")
                            ElseIf Session("bulkvisible") = "false" Then
                                gridDiv.Visible = False
                                Session.Remove("bulkvisible")
                            End If
                        End If




                        If Session("sname") Is Nothing Then
                            gridDiv.Visible = False
                        End If
                        '/*******************************End*****************************/

                        'Session("BackQuery") = ""
                    End If
                    '************* End Code *********************
                    myCommand.Dispose()
                    myTable.Dispose()
                    myDataReader.Close()
                    objconn.disconnect()
                    'Added by Tabrez
                    Session("FromPage") = "Question"
                Else
                    '   Search_Result()
                    If DGData.Visible = True Then
                        fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                    End If

                End If
                ' End If


                If Not IsPostBack Then
                    If Session("DDLSelectedValue") <> "" Then

                        'If intCo = 1 Then

                        sel_test_type.SelectedValue = Convert.ToString(Session("DDLSelectedValue"))
                        flg = False
                        intCo = 2
                        'End If
                    End If
                End If


                Me.SetFocus(sel_test_type)

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                myDataReader = Nothing
                myCommand = Nothing
                strPathDb = Nothing
            End Try
        End Sub
#End Region

#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload
            Try
                'Dim strPathDb As String
                If objconn.connect() = True Then
                    objconn.disconnect()

                End If
            Catch ex As Exception
                objconn.disconnect()
            End Try
        End Sub
#End Region

#Region "Search Result"
        '**************************************************************************
        'Function               :   Search_Result
        '
        'Return                 :   None
        '
        'Argument               :   None
        '
        'Explanation            :   This will generate th SQL Query for 
        '                           Search Criteria
        '                           
        'Note                   :   None
        '**************************************************************************
        Private Sub Search_Result()
            Dim mainSql, strSql, WhereSql, WhereSql1, WhereSql2, WhereSql3, strSession, WhereSql4 As String
            'Dim OrderbySql As String
            Try
                '**************************************************************************
                'Query modified by Tabrez to display Subject info.
                '**************************************************************************
                'Select Query
                'strSql = "SELECT m_question.qnid, m_question.question, m_question.test_type, m_testinfo.test_name FROM m_question, m_testinfo where m_question.test_type = m_testinfo.test_type"
                ''Test Type
                'If Not IsDBNull(sel_test_type.SelectedItem.Value) And sel_test_type.SelectedItem.Value <> "" Then
                '    WhereSql1 = " AND m_question.test_type = '" & sel_test_type.SelectedItem.Value & "'"
                'Else
                '    WhereSql1 = ""
                'End If
                ''Option
                'If Not IsDBNull(txt_question.Text) And txt_question.Text <> "" Then
                '    WhereSql2 = " AND m_question.question LIKE '%" & Replace(txt_question.Text, "'", "''") & "%'  "
                'Else
                '    WhereSql2 = ""
                'End If
                'WhereSql4 = " AND  m_testinfo.del_flag='0' "
                'WhereSql = WhereSql1 & WhereSql2 & WhereSql4

                'If Not IsDBNull(WhereSql) And WhereSql <> "" Then
                '    mainSql = strSql & " " & WhereSql ' & OrderbySql
                'Else
                '    mainSql = strSql ' & OrderbySql
                'End If

                ''Added by Bhasker(2009/11/24)
                ''**************** Start Code *************************
                'If mainSql <> "" Then
                '    strSession = mainSql
                'End If

                'If sel_test_type.SelectedItem.Value <> "" Then
                '    strSession = strSession + "|" + sel_test_type.SelectedItem.Value
                'End If

                'If txt_question.Text <> "" Then
                '    strSession = strSession + "|" + txt_question.Text
                'End If
                'If strSession <> "" Then
                '    Session("BackQuery") = strSession
                'End If
                ''**************** End Code *************************

                'Result_Display(mainSql)
                '  strSql = "SELECT m_question.qnid, m_question.question, m_question.test_type, m_testinfo.test_name,m_question.del_flag as dflg FROM m_question, m_testinfo where m_question.test_type = m_testinfo.test_type"
                'Session.Add("sname", sel_test_type.SelectedItem.Value)
                'Session.Add("ques", txt_question.Text)

                'If Not Session("sname") Is Nothing Then
                '    sel_test_type.SelectedItem.Value = CInt(Session("sname"))
                '    Session.Remove("sname")
                'End If
                'If Not Session("ques") Is Nothing Then
                '    txt_question.Text = Session("ques").ToString
                '    Session.Remove("ques")
                'End If


                'strSql = "SELECT Distinct m_question.qnid, m_question.question, m_question.test_type, m_testinfo.test_name,m_Question.del_flag as dflg  FROM m_question Join m_testinfo on m_question.test_type = m_testinfo.test_type " 'where m_question.del_flag = '0'"

                strSql = "SELECT Distinct m_question.qnid, m_question.question, m_question.test_type, m_testinfo.test_name,m_Question.del_flag as dflg , m_testinfo.test_name as CourseName    FROM m_question Join m_testinfo on m_question.test_type = m_testinfo.test_type " 'where m_question.del_flag = '0'"

                'Test Type
                If Not IsDBNull(sel_test_type.SelectedItem.Value) And sel_test_type.SelectedItem.Value <> "" Then
                    WhereSql1 = " AND m_question.test_type = '" & sel_test_type.SelectedItem.Value & "'"
                Else
                    WhereSql1 = ""
                End If
                'Option
                If Not IsDBNull(txt_question.Text) And txt_question.Text <> "" Then
                    If ChkExactSearch.Checked Then
                        'Exact search
                        WhereSql2 = "AND m_question.question='" & txt_question.Text & "'"
                    Else
                        'Like Search
                        WhereSql2 = " AND m_question.question LIKE N'%" & Replace(txt_question.Text, "'", "''") & "%'  "
                    End If

                Else
                    WhereSql2 = ""
                End If
                WhereSql4 = " AND  m_testinfo.del_flag='0' "
                WhereSql = WhereSql1 & WhereSql2 & WhereSql4

                If Not IsDBNull(WhereSql) And WhereSql <> "" Then
                    mainSql = strSql & " " & WhereSql ' & OrderbySql
                Else
                    mainSql = strSql ' & OrderbySql
                End If

                'Added by Bhasker(2009/11/24)
                '**************** Start Code *************************
                If mainSql <> "" Then
                    strSession = mainSql
                End If

                If sel_test_type.SelectedItem.Value <> "" Then
                    strSession = strSession + "|" + sel_test_type.SelectedItem.Value
                End If

                If txt_question.Text <> "" Then
                    strSession = strSession + "|" + txt_question.Text
                End If
                If strSession <> "" Then
                    Session("BackQuery") = strSession
                End If
                '**************** End Code *************************
                mainSql = mainSql & " where m_Question.del_flag=0"
                Result_Display(mainSql)

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                mainSql = Nothing
                strSql = Nothing
                WhereSql = Nothing
                WhereSql1 = Nothing
                WhereSql2 = Nothing
                WhereSql3 = Nothing
                'OrderbySql = Nothing
            End Try
        End Sub
#End Region

#Region "Serach Button's Click Event"
        Private Sub imgBtnSearch_Click(sender As Object, e As EventArgs) Handles imgBtnSearch.Click
            Try


                lblMsg.Text = ""
                DGData.CurrentPageIndex = 0
                ViewState.Add("fromsearch", "true")
                Session("pageindex") = Nothing
                Search_Result()
                Session("ListItem") = sel_test_type.SelectedItem.Value

                Session.Add("sname", sel_test_type.SelectedItem.Value)
                Session.Add("ques", txt_question.Text)

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "New Question Button Click Event"
        Private Sub btnNewQuestion_Click(sender As Object, e As EventArgs) Handles btnNewQuestion.Click
            Try
                Session("testtype") = sel_test_type.SelectedIndex
                Response.Redirect("question_ans.aspx?qid=&test=" & sel_test_type.SelectedValue, False)
                'Response.Redirect("QuestionAnsInsert.aspx?qid=&test=" & sel_test_type.SelectedValue, False)
                If DGData.Visible = True Then
                    Session.Add("newvisible", "true")
                Else
                    Session.Add("newvisible", "false")
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "Back button Click Event"
        Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
            Try
                Session("DDLSelectedValue") = Nothing
                Response.Redirect("admin.aspx", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
            End Try
        End Sub
#End Region

#Region "Function Result Display"
        '**************************************************************************
        'Function               :   Result_Display
        '
        'Return                 :   None
        '
        'Argument               :   strSql : Sql query for Search
        '
        'Explanation            :   This will generate the Records in Datagrid
        '                           
        'Note                   :   None
        '**************************************************************************
        Private Sub Result_Display(ByVal strSql As String)
            Dim objconn As New ConnectDb
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            'Dim strPathDb As String
            Dim myTable As New DataTable
            Dim rows As DataRow
            Dim intpginx As Integer
            Dim count As Integer
            Try
                myTable.Columns.Add(New DataColumn("qnid", GetType(Int32)))
                myTable.Columns.Add(New DataColumn("Serial_no", GetType(Int32)))

                myTable.Columns.Add(New DataColumn("question", GetType(String)))
                myTable.Columns.Add(New DataColumn("test_type", GetType(String)))
                myTable.Columns.Add(New DataColumn("test_name", GetType(String)))
                myTable.Columns.Add(New DataColumn("dflg", GetType(Boolean)))

                myTable.Columns.Add(New DataColumn("CourseName", GetType(String)))


                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                If objconn.connect() Then
                    myCommand = New SqlCommand(strSql, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    count = 0
                    While myDataReader.Read
                        count = count + 1
                        rows = myTable.NewRow
                        'rows(0) = myDataReader.Item("qnid")
                        rows(0) = myDataReader.Item("qnid").ToString
                        rows(1) = count.ToString
                        '                        rows(2) = "<PRE>" & myDataReader.Item("question") & "<PRE>"
                        'removed PRE tag 

                        rows(2) = myDataReader.Item("question")
                        'rows(2) = myDataReader.Item("question")
                        rows(3) = myDataReader.Item("test_type")
                        rows(4) = myDataReader.Item("test_name")
                        rows(5) = myDataReader.Item("dflg")

                        rows(6) = myDataReader.Item("CourseName")

                        myTable.Rows.Add(rows)
                    End While
                    myDataReader.Close()
                    myCommand.Dispose()
                    objconn.disconnect()
                    If count > 0 Then
                        gridDiv.Visible = True
                        If sel_test_type.SelectedIndex > 0 Then
                            DGData.Columns(0).Visible = False
                            DGData.Columns(2).Visible = False

                            DGData.Columns(0).HeaderStyle.Width = Unit.Pixel(100)
                            DGData.Columns(1).HeaderStyle.Width = Unit.Pixel(100)
                            DGData.Columns(2).HeaderStyle.Width = Unit.Pixel(100)
                            DGData.Columns(4).HeaderStyle.Width = Unit.Pixel(524)
                        Else
                            DGData.Columns(0).Visible = False
                            DGData.Columns(1).Visible = True
                            DGData.Columns(2).Visible = False
                            DGData.Columns(1).HeaderStyle.Width = Unit.Pixel(100)
                            DGData.Columns(2).HeaderStyle.Width = Unit.Pixel(100)
                            DGData.Columns(4).HeaderStyle.Width = Unit.Pixel(624)
                        End If
                        DGData.DataSource = myTable
                        Try
                            'If blnchanged Then
                            '    intpginx = DGData.CurrentPageIndex
                            'Else
                            '    DGData.CurrentPageIndex = 0
                            '    intpginx = 0
                            'End If
                            Try
                                intpginx = DGData.CurrentPageIndex
                            Catch ex As Exception
                                intpginx = 0
                            End Try

                            If Session("fromquestionans") = "true" Then
                                If Session("pageindex") <> Nothing Then
                                    Dim intpi = CInt(Session("pageindex").ToString())
                                    DGData.CurrentPageIndex = intpi
                                    ViewState.Add("selval", intpi)
                                    '    Session.Remove("pageindex")
                                End If
                            Else
                                DGData.CurrentPageIndex = intpginx
                            End If

                            If ViewState("fromsearch") = "true" Then
                                DGData.CurrentPageIndex = 0
                                ViewState.Remove("fromsearch")
                            End If

                            'DGData.CurrentPageIndex = intpginx
                            ' Used session variable on ques_ans.aspx because there was no option to use query string
                            ' related session variable is  pi , used in fillpagenumbers() method
                            If Not (Session("Qlast") Is Nothing) Then
                                DGData.CurrentPageIndex = CInt(Session("Qlast"))
                                ddlPages.SelectedItem.Value = DGData.CurrentPageIndex
                                Session.Remove("Qlast")
                            End If
                            DGData.DataBind()
                            fillPagesCombo()
                            fillPageNumbers(DGData.CurrentPageIndex + 1, 9)


                            'DGData.CurrentPageIndex = intpginx
                            For i As Integer = 0 To DGData.Items.Count - 1
                                '   If myTable.Rows(i).Item(5) = True Then
                                If DGData.Items(i).Cells(4).Text = True Then
                                    DGData.Items(i).Cells(4).Attributes.Remove("href")
                                    DGData.Items(i).Cells(4).Attributes.Remove("className")
                                    DGData.Items(i).Cells(4).Attributes.Add("onclick", "return false")
                                    DGData.Items(i).Cells(5).Enabled = False
                                    DGData.Items(i).Cells(5).ToolTip = "Disabled"
                                    DGData.Items(i).BackColor = Drawing.Color.Gray
                                ElseIf DGData.Items(i).Cells(4).Text = False Then
                                    DGData.Items(i).Enabled = True
                                End If
                            Next



                            If RecCont(strSql) Then
                                LblRecCnt.Text = "Total Records : " & RecCont(strSql)
                            Else
                                LblRecCnt.Text = "No Records"
                            End If
                        Catch ex As Exception
                            DGData.CurrentPageIndex = DGData.CurrentPageIndex - 1
                            If DGData.CurrentPageIndex >= 0 Then
                                DGData.DataBind()
                            End If
                        End Try
                    Else
                        ' lblMsg.Text = "There are no questions of " & sel_test_type.SelectedItem.Text & " test."
                        lblMsg.Text = "No Record(s) Found"
                        DGData.Visible = False
                        gridDiv.Visible = False
                        BtnRemove.Visible = False
                        LblRecCnt.Text = ""
                    End If
                End If
                blnchanged = False
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
                strPathDb = Nothing
                myTable = Nothing
                intpginx = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                rows = Nothing
            End Try
        End Sub
#End Region

#Region "Page Indexchanged Event"
        Protected Sub DgData_PageIndexChanged(ByVal Obj As Object, ByVal E As DataGridPageChangedEventArgs) Handles DGData.PageIndexChanged
            Try
                Dim sname As String = Session("sname").ToString
                Dim ques As String = Session("ques").ToString

                blnchanged = True
                DGData.CurrentPageIndex = E.NewPageIndex
                If Session("pageindex") <> Nothing Then
                    Session.Remove("pageindex")
                End If
                Call Search_Result()
                Session.Add("sname", sname)
                Session.Add("ques", ques)
                Session.Add("pageindex", E.NewPageIndex)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "Function Setfocus"
        Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)
            Try
                Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ID & "').focus() </SCRIPT>"
                RegisterStartupScript("focus", s)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

#Region "RecCont"
        Private Function RecCont(ByVal strSql As String) As Integer
            Try
                intcnt = 0
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    strSql = "Select count(*) from (" + strSql + ") " + " RecordCount "
                    myCommand = New SqlCommand(strSql, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    If myDataReader.Read() Then
                        intcnt = Convert.ToInt32(myDataReader.GetValue(0).ToString())
                    Else
                        intcnt = 0
                    End If
                    myDataReader.Close()
                    myCommand.Dispose()
                    objconn.disconnect()
                End If
                RecCont = intcnt
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function
#End Region

#Region "Function sEncodeString"
        '**************************************************************************
        'Function               :   sEncodeString
        '
        'Return                 :   encoded string
        '
        'Argument               :   None
        '
        'Explanation            :   This will convert the string in to some encoded string
        '                           
        'Note                   :   None
        '**************************************************************************
        Function sEncodeString(ByVal sText As String) As String
            Dim iCnt
            Dim sChar
            Dim sEncode
            Try
                sEncode = ""
                For iCnt = 1 To Len(sText)
                    sChar = Mid(sText, iCnt, 1)
                    sEncode = sEncode & ENCRYPT_DELIMIT & LCase(CStr(Hex(Asc(sChar) + ENCRYPT_KEY)))
                Next
                sEncodeString = sEncode
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                iCnt = Nothing
                sChar = Nothing
                sEncode = Nothing
            End Try
        End Function
#End Region

#Region "Button Logoff's Click Event"

#End Region

        'Added by Pranit on 04/12/2019
        Sub Selection_Change(sender As Object, e As EventArgs)
            Try
                DGData.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
                Search_Result()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub

#Region "Button bulkinsert's Click Event"
        Protected Sub btnQuestion_Click1(sender As Object, e As EventArgs) Handles btnQuestion.Click
            Dim strPathDb As String
            If DGData.Visible = True Then
                Session.Add("bulkvisible", "true")
            Else
                Session.Add("bulkvisible", "false")
            End If
            Try
                'Changed by Bhumi [24/8/2015]
                'Reason: Select Test to Select Subject in If Condition
                If sel_test_type.SelectedItem.Text = "Select Subject" Then
                    'Ended by bhumi
                    Response.Write("<script language=javascript>")
                    Response.Write("alert('You did not select the Test Type !')")
                    Response.Write("</script>")
                    BtnRemove.Visible = False
                Else
                    'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                    If objconn.connect() Then
                        strTypeCheck = "Select test_type from m_testinfo where test_name ='" + sel_test_type.SelectedItem.Text + "'"
                        myCommand = New SqlCommand(strTypeCheck, objconn.MyConnection)
                        strTypeCheck = myCommand.ExecuteScalar()

                        Response.Redirect("BulkQuestionsImport.aspx?Test_Type=" + HttpUtility.UrlEncode(strTypeCheck), False)
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                strPathDb = Nothing
            End Try
        End Sub
#End Region




#Region "chkSelectAll_CheckedChanged"

        Protected Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

            Dim chk As CheckBox = Nothing

            Try

                For Each rowItem As DataGridItem In DGData.Items

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

#End Region



        'Private Sub BtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemove.Click


        '    Dim chk As CheckBox = Nothing
        '    Dim strQid As String = Nothing
        '    Dim strTestType As String = Nothing

        '    Dim MyCommand As SqlCommand
        '    Dim objconn As New ConnectDb
        '    Dim strPathDb As String
        '    Dim strSql As String
        '    Dim bolFlag As Boolean

        '    Try
        '        For Each rowItem As DataGridItem In DGData.Items
        '            chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
        '            If chk.Checked Then
        '                bolFlag = True
        '                strQid = DirectCast(rowItem.Cells(0).Text, String)
        '                strTestType = DirectCast(rowItem.Cells(2).Text, String)

        '                strPathDb = ConfigurationSettings.AppSettings("PathDb")
        '                If objconn.connect(strPathDb) Then
        '                    strSql = "Delete  from m_options where m_options.qnid = " & strQid & _
        '                        " and m_options.test_type = '" & strTestType & "'"
        '                    MyCommand = New SqlCommand(strSql, objconn.MyConnection)
        '                    MyCommand.ExecuteNonQuery()
        '                    MyCommand.Dispose()
        '                End If
        '                objconn.disconnect()

        '                If objconn.connect(strPathDb) Then
        '                    strSql = "Delete  from m_question where m_question.qnid = " & strQid & _
        '                        " and m_question.test_type = '" & strTestType & "'"
        '                    MyCommand = New SqlCommand(strSql, objconn.MyConnection)
        '                    MyCommand.ExecuteNonQuery()
        '                    MyCommand.Dispose()
        '                End If
        '                objconn.disconnect()
        '            End If
        '        Next

        '        If Not bolFlag = True Then
        '            lblMsg.Text = "Please select at least one check box to remove question"
        '        Else
        '            lblMsg.Text = ""
        '            blnchanged = True
        '            Search_Result()
        '        End If

        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        lblMsg.Text = ex.Message()
        '    Finally
        '        objconn = Nothing
        '        strPathDb = Nothing
        '        strSql = Nothing
        '        MyCommand = Nothing
        '    End Try

        'End Sub

        Protected Sub BtnRemove_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
            Dim chk As CheckBox = Nothing
            Dim strQid As String = Nothing
            Dim strTestType As String = Nothing

            Dim MyCommand As SqlCommand
            Dim objconn As New ConnectDb
            'Dim strPathDb As String
            Dim strSql As String
            Dim bolFlag As Boolean

            Try
                For Each rowItem As DataGridItem In DGData.Items
                    chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                    If chk.Checked Then
                        bolFlag = True
                        strQid = DirectCast(rowItem.Cells(0).Text, String)
                        strTestType = DirectCast(rowItem.Cells(2).Text, String)

                        'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                        If objconn.connect() Then
                            strSql = "Delete  from m_options where m_options.qnid = " & strQid &
                                " and m_options.test_type = '" & strTestType & "'"
                            MyCommand = New SqlCommand(strSql, objconn.MyConnection)
                            MyCommand.ExecuteNonQuery()
                            MyCommand.Dispose()
                        End If
                        objconn.disconnect()

                        If objconn.connect() Then
                            strSql = "Delete  from m_question where m_question.qnid = " & strQid &
                                " and m_question.test_type = '" & strTestType & "'"
                            MyCommand = New SqlCommand(strSql, objconn.MyConnection)
                            MyCommand.ExecuteNonQuery()
                            MyCommand.Dispose()
                        End If

                        lblMsg.ForeColor = Drawing.Color.Green
                        lblMsg.Visible = True
                        lblMsg.Text = " Question Deleted Successfully."
                        objconn.disconnect()
                    End If
                Next

                If Not bolFlag = True Then
                    lblMsg.ForeColor = Drawing.Color.Red
                    lblMsg.Visible = True
                    lblMsg.Text = "Please select at least one check box to remove question."
                Else
                    'lblMsg.Text=""  
                    blnchanged = True
                    Search_Result()
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                objconn = Nothing
                strPathDb = Nothing
                strSql = Nothing
                MyCommand = Nothing
                strTestType = Nothing
                strQid = Nothing
                chk = Nothing
            End Try

        End Sub

        Protected Sub DGData_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGData.ItemDataBound
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        End Sub

        Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
            Try
                If Not Session("sname") Is Nothing Then
                    Session.Remove("sname")
                End If
                If Not Session("ques") Is Nothing Then
                    Session.Remove("ques")
                End If


                sel_test_type.SelectedIndex = 0
                txt_question.Text = String.Empty
                DGData.Visible = False
                gridDiv.Visible = False
                BtnRemove.Visible = False
                lblMsg.Text = String.Empty
                Session.Remove("fromquestionans")
                Session.Remove("pageindex")
                ViewState.Remove("selval")
                ViewState.Remove("pageNo")
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub

        '********************************************************************
        'Code added by Monal Shah
        'Purpose:enableDisable
        '********************************************************************
        Public Function enableDisable(ByVal flg As Boolean, ByVal blflg As Boolean)
            Dim chk As New CheckBox
            Dim strid As String
            Dim q As String = String.Empty
            Dim cid As String
            Dim bolflg As Boolean = True
            Dim al As New ArrayList()
            Dim intchkcount As Integer = 0
            Dim intdonecount As Integer = 0
            Dim flgDelete As Boolean
            Dim objCommonFunction As CommonFunction

            Try
                For Each rowItem As DataGridItem In DGData.Items
                    chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                    If chk.Checked Then
                        intchkcount = intchkcount + 1
                        bolflg = False
                        strid = DirectCast(rowItem.Cells(0).Text, String) & "," & DirectCast(rowItem.Cells(2).Text, String)
                        al.Add(strid.ToString())
                    End If
                Next

                Dim msg As String
                Dim title As String
                Dim style As MsgBoxStyle
                Dim response As MsgBoxResult

                Dim objConstan As Constant = New Constant()

                Dim flgMsg As Boolean

                If bolflg = True Then
                    If blflg = False Then
                        lblMsg.ForeColor = Color.Red
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Select At Least One CheckBox For Disable Question"
                        flgMsg = True
                    ElseIf blflg = True Then
                        lblMsg.ForeColor = Color.Red
                        lblMsg.Visible = True
                        lblMsg.Text = "Please Select At Least One CheckBox For Enable Question"
                    End If
                End If

                If flgMsg <> True Then
                    For i As Integer = 0 To al.Count - 1
                        If objconn.connect() = True Then
                            Dim strbr() As String = al(i).ToString().Split(",") 'strid.Split(",")
                            If (flg = True) Then
                                objCommonFunction = New CommonFunction()
                                flgDelete = objCommonFunction.DeleteQuestion(strbr(0), strbr(1))
                                If flgDelete = True Then
                                    lblMsg.Text = "Question(s) Deleted Successfully"
                                    lblMsg.Visible = True
                                    lblMsg.ForeColor = Color.Green
                                    Call Search_Result()
                                Else
                                    lblMsg.Visible = True
                                    lblMsg.Text = "Error occured, Question not deleted, Please try again"
                                End If

                            End If
                        End If
                    Next
                End If
                Dim intindex As Integer = DGData.CurrentPageIndex

                Call Search_Result()

                DGData.CurrentPageIndex = intindex

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("ChkSelect ALL : ", ex)
                    Response.Redirect("error.aspx", False)
                End If
                Throw ex
            End Try
        End Function
        Protected Sub imgBtnEnable_Click(sender As Object, e As EventArgs) Handles imgBtnEnable.Click
            Try
                lblMsg.Text = String.Empty
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
                Dim confirmValue As String = Request.Form("confirm_value")
                If confirmValue = "Yes" Then
                    lblMsg.Text = String.Empty
                    enableDisable(True, False)
                    'ClientScript.RegisterStartupScript(Me.[GetType](), "alert", "alert('You clicked YES!')", True)
                End If


            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub


#Region "Function checkstatus"
        'Desc: This function checks the status of subject before disabling a question.
        'By: Jatin Gangajaliya, 2011/04/22.

        Private Function CheckStatus(ByVal testtype As Integer) As Boolean
            Dim strq As String
            Dim strbr As StringBuilder
            Dim MyCommand As SqlCommand
            Dim bol As Boolean = True
            Dim intcount As Integer
            Try
                strbr = New StringBuilder
                strbr.Append(" select count(*) from t_candidate_status where (Appearedflag=0 or Appearedflag=1 ) and course_id in  ")
                strbr.Append(" (select distinct course_id from m_weightage where test_type = ")
                strbr.Append(testtype)
                strbr.Append(" and Del_flag=0) ")
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
                intcount = Nothing
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
                            If (CInt(ViewState("lastRange")) - 1) > selPage Then
                                range = selPage
                            Else
                                range = CInt(ViewState("lastRange")) - 1
                            End If

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
            Session.Add("pageindex", DGData.CurrentPageIndex)
            'BindGrid()
            Search_Result()
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
            Session.Add("pageindex", DGData.CurrentPageIndex)
            Search_Result()
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

        Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
            DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
            ViewState.Add("selval", ddlPages.SelectedItem.Value)
            ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
            Session.Add("pageindex", DGData.CurrentPageIndex)
            Search_Result()
        End Sub

#Region "populatesubject"
        'Added by:: Saraswati Patel 
        'For eport question and answer in excel file
        Private Function populatesubject(Optional ByVal test_type As String = "C") As String
            Dim myDataReader As SqlDataReader             ' SqlDataReader type object
            Dim myCommand As SqlCommand                   ' SqlCommand type object
            Dim objconn As New ConnectDb    ' Object of the ConnectClass class
            Dim sqlstr As String                            ' String type variable to store string    
            Dim myTable As DataTable                        ' DataTable type object
            Dim myRow As DataRow                            ' DataRow type object  
            Dim SubjectName As String
            Try
                ' Checking if the Database is getting connected
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlstr = ""
                    sqlstr = sqlstr & "SELECT Distinct test_type,test_name FROM m_testinfo where del_flag='0' order by test_name"

                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader

                    myTable = New DataTable
                    myTable.Columns.Add(New DataColumn("subjectID", GetType(String)))
                    myTable.Columns.Add(New DataColumn("subjectName", GetType(String)))

                    Dim StrTemp As String
                    Dim SelVal As String

                    ' While loop to populate the Datatable
                    While myDataReader.Read
                        myRow = myTable.NewRow
                        myRow(0) = myDataReader.Item("test_type")
                        StrTemp = Convert.ToString(myDataReader.Item("test_type"))
                        If UCase(Trim(test_type)) = UCase(Trim(StrTemp)) Then
                            SubjectName = Trim(Convert.ToString(myDataReader.Item("test_name")))
                        End If
                        myRow(1) = myDataReader.Item("test_name")
                        myTable.Rows.Add(myRow)
                    End While
                End If
                Return SubjectName
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Return SubjectName
                Throw ex
            Finally
                objconn = Nothing
                sqlstr = Nothing
                myDataReader = Nothing
                myCommand = Nothing
                myRow = Nothing
                myTable = Nothing
            End Try
        End Function
#End Region





#Region "Getit"
        'Added by:: Saraswati Patel 
        'For eport question and answer in excel file
        Public Sub Getit(ByVal SubjectName As String, ByVal category As String)

            Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
            Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
            Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
            Dim Sheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
            Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
            Dim objOpt As Object = System.Reflection.Missing.Value
            Dim strMainSql, strSql, strWhereSql, strWhereSql1, strWhereSql2, sqlstr, strWhereSql4, strstartCell, strpath, strLevel As String
            Dim crrOptstr, crr As String
            Dim rowCorrectAns As Integer = 6
            Dim ImgNotFound(1) As String
            Dim objCn As New ConnectDb
            Dim sit1 As Excel.Worksheet
            Dim rownum As Integer = 6
            Dim rowOption As Integer = 6
            Dim st As Integer = 6
            Dim ed As Integer
            Dim index As Integer = 1
            Dim ds, dsCorrectAns, ds1 As DataSet
            Dim da, da1, daCorrectAns As SqlDataAdapter
            Dim ht, htOpt, htOptStr As Hashtable
            Dim strtext As String = ""
            Dim pic As Object
            Dim fileName1 As Object
            Dim ans As String

            Try

                'Constants
                Const xlEdgeLeft = 7
                Const xlEdgeTop = 8
                Const xlEdgeBottom = 9
                Const xlEdgeRight = 10

                'Create  Spreadsheet
                Dim datestart As Date = Date.Now 'Added by Pragnesha Kulkarni on 2018/06/01 for stop Excel process
                App = New Excel.Application

                WorkBooks = DirectCast(App.Workbooks, Excel.Workbooks)
                WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)


                sit1 = WorkBook.Worksheets(1)
                sit1.Name = "Questions"
                sit1.Activate()
                sit1.Cells.Clear()

                With App.ActiveSheet.Range("C2:F3")
                    .MergeCells = True
                    .Interior.ColorIndex = 40
                    .Font.Bold = True
                    .Font.ColorIndex = 53
                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    .Cells.Value = "UNITECH SYSTEMS"
                    .Font.Size = 15
                    '.BORDERS(xlEdgeLeft).Weight = 2
                    '.BORDERS(xlEdgeTop).Weight = 2
                    '.BORDERS(xlEdgeBottom).Weight = 2
                    '.BORDERS(xlEdgeRight).Weight = 2
                End With

                With App.ActiveSheet.Range("B4:C4")
                    .MergeCells = True
                    .Interior.ColorIndex = 40
                    .Font.Bold = True
                    .Font.ColorIndex = 53
                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    .Cells.Value = "SubjectName :: " + SubjectName
                    .Font.Size = 12
                    '.BORDERS(xlEdgeLeft).Weight = 2
                    '.BORDERS(xlEdgeTop).Weight = 2
                    ' .BORDERS(xlEdgeBottom).Weight = 2
                    '.BORDERS(xlEdgeRight).Weight = 2
                End With
                If category = "1" Then

                    With App.ActiveSheet.Range("C2:F3")
                        .MergeCells = True
                        .Interior.ColorIndex = 40
                        .Font.Bold = True
                        .Font.ColorIndex = 53
                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        .Cells.Value = "UNITECH SYSTEMS"
                        .Font.Size = 15
                        '.BORDERS(xlEdgeLeft).Weight = 2
                        '.BORDERS(xlEdgeTop).Weight = 2
                        '.BORDERS(xlEdgeBottom).Weight = 2
                        '.BORDERS(xlEdgeRight).Weight = 2
                    End With
                    ' Set Column Headers
                    App.ActiveSheet.Cells(5, 2).Value = "Question"
                    App.ActiveSheet.Cells(5, 2).Interior.ColorIndex = 36
                    App.ActiveSheet.Cells(5, 2).Font.Bold = True
                    App.ActiveSheet.Cells(5, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 2).ColumnWidth = 30
                    App.ActiveSheet.Cells(5, 2).WrapText = True
                    App.ActiveSheet.Cells(5, 2).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 3).Value = "Option 1"
                    App.ActiveSheet.Cells(5, 3).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(4, 3).Font.ColorIndex = 2
                    App.ActiveSheet.Cells(5, 3).Font.Bold = True
                    App.ActiveSheet.Cells(5, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 3).ColumnWidth = 20
                    App.ActiveSheet.Cells(5, 3).WrapText = True
                    App.ActiveSheet.Cells(5, 3).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 4).Value = "Option 2"
                    App.ActiveSheet.Cells(5, 4).Interior.ColorIndex = 36
                    App.ActiveSheet.Cells(5, 4).Font.Bold = True
                    App.ActiveSheet.Cells(5, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 4).ColumnWidth = 20
                    App.ActiveSheet.Cells(5, 4).WrapText = True
                    App.ActiveSheet.Cells(5, 4).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 5).Value = "Option 3"
                    App.ActiveSheet.Cells(5, 5).Interior.ColorIndex = 36
                    'App.ActiveSheet.Cells(6, 5).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 5).Font.Bold = True
                    App.ActiveSheet.Cells(5, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 5).ColumnWidth = 20
                    App.ActiveSheet.Cells(5, 5).WrapText = True
                    App.ActiveSheet.Cells(5, 5).Borders.Weight = 2
                    'App.ActiveSheet.Columns("E").autofit()

                    App.ActiveSheet.Cells(5, 6).Value = "Option 4"
                    App.ActiveSheet.Cells(5, 6).Interior.ColorIndex = 36
                    App.ActiveSheet.Cells(5, 6).Font.Bold = True
                    App.ActiveSheet.Cells(5, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 6).ColumnWidth = 20
                    App.ActiveSheet.Cells(5, 6).WrapText = True
                    App.ActiveSheet.Cells(5, 6).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 7).Value = "Correct Option"
                    App.ActiveSheet.Cells(5, 7).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(5,7).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 7).Font.Bold = True
                    App.ActiveSheet.Cells(5, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 7).ColumnWidth = 10
                    App.ActiveSheet.Cells(5, 7).WrapText = True
                    App.ActiveSheet.Cells(5, 7).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 8).Value = "Difficulty Level"
                    App.ActiveSheet.Cells(5, 8).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(5,8).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 8).Font.Bold = True
                    App.ActiveSheet.Cells(5, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 8).ColumnWidth = 10
                    App.ActiveSheet.Cells(5, 8).WrapText = True
                    App.ActiveSheet.Cells(5, 8).Borders.Weight = 2


                    App.ActiveSheet.Cells(5, 9).Value = "Question Type"
                    App.ActiveSheet.Cells(5, 9).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(5,9).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 9).Font.Bold = True
                    App.ActiveSheet.Cells(5, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 9).ColumnWidth = 30
                    App.ActiveSheet.Cells(5, 9).WrapText = True
                    App.ActiveSheet.Cells(5, 9).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 10).Value = "Marks"
                    App.ActiveSheet.Cells(5, 10).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(5,10).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 10).Font.Bold = True
                    App.ActiveSheet.Cells(5, 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 10).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 10).ColumnWidth = 8
                    App.ActiveSheet.Cells(5, 10).WrapText = True
                    App.ActiveSheet.Cells(5, 10).Borders.Weight = 2
                ElseIf category = "2,3" Then
                    With App.ActiveSheet.Range("C2:G3")
                        .MergeCells = True
                        .Interior.ColorIndex = 40
                        .Font.Bold = True
                        .Font.ColorIndex = 53
                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        .Cells.Value = "UNITECH SYSTEMS"
                        .Font.Size = 15
                        '.BORDERS(xlEdgeLeft).Weight = 2
                        '.BORDERS(xlEdgeTop).Weight = 2
                        '.BORDERS(xlEdgeBottom).Weight = 2
                        '.BORDERS(xlEdgeRight).Weight = 2
                    End With

                    App.ActiveSheet.Cells(5, 2).Value = "Question"
                    App.ActiveSheet.Cells(5, 2).Interior.ColorIndex = 36
                    App.ActiveSheet.Cells(5, 2).Font.Bold = True
                    App.ActiveSheet.Cells(5, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 2).ColumnWidth = 30
                    App.ActiveSheet.Cells(5, 2).WrapText = True
                    App.ActiveSheet.Cells(5, 2).Borders.Weight = 2
                    For row As Integer = 0 To 19
                        App.ActiveSheet.Cells(5, 3 + row).Value = "Option " + (row + 1).ToString  'Option for Blank1
                        App.ActiveSheet.Cells(5, 3 + row).Interior.ColorIndex = 36
                        ' App.ActiveSheet.Cells(4, 3).Font.ColorIndex = 2
                        App.ActiveSheet.Cells(5, 3 + row).Font.Bold = True
                        App.ActiveSheet.Cells(5, 3 + row).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        App.ActiveSheet.Cells(5, 3 + row).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                        App.ActiveSheet.Cells(5, 3 + row).ColumnWidth = 10
                        App.ActiveSheet.Cells(5, 3 + row).WrapText = True
                        App.ActiveSheet.Cells(5, 3 + row).Borders.Weight = 2

                        App.ActiveSheet.Cells(5, 23 + row).Value = "Option for Blank " + (row + 1).ToString  'Option for Blank1
                        App.ActiveSheet.Cells(5, 23 + row).Interior.ColorIndex = 36
                        ' App.ActiveSheet.Cells(4, 3).Font.ColorIndex = 2
                        App.ActiveSheet.Cells(5, 23 + row).Font.Bold = True
                        App.ActiveSheet.Cells(5, 23 + row).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        App.ActiveSheet.Cells(5, 23 + row).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                        App.ActiveSheet.Cells(5, 23 + row).ColumnWidth = 5
                        App.ActiveSheet.Cells(5, 23 + row).WrapText = True
                        App.ActiveSheet.Cells(5, 23 + row).Borders.Weight = 2


                    Next
                    App.ActiveSheet.Cells(5, 43).Value = "Marks"
                    App.ActiveSheet.Cells(5, 43).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(5,43).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 43).Font.Bold = True
                    App.ActiveSheet.Cells(5, 43).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 43).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 43).ColumnWidth = 8
                    App.ActiveSheet.Cells(5, 43).WrapText = True
                    App.ActiveSheet.Cells(5, 43).Borders.Weight = 2

                    App.ActiveSheet.Cells(5, 44).Value = "Difficulty Level"
                    App.ActiveSheet.Cells(5, 44).Interior.ColorIndex = 36
                    ' App.ActiveSheet.Cells(5,44).Font.ColorIndex = 3
                    App.ActiveSheet.Cells(5, 44).Font.Bold = True
                    App.ActiveSheet.Cells(5, 44).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(5, 44).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(5, 44).ColumnWidth = 8
                    App.ActiveSheet.Cells(5, 44).WrapText = True
                    App.ActiveSheet.Cells(5, 44).Borders.Weight = 2

                End If


                strpath = ConfigurationSettings.AppSettings("PathDb")

                'Query for geting question from database
                strSql = "SELECT Distinct m_question.question,m_question.qnid FROM m_question Join m_testinfo on m_question.test_type = m_testinfo.test_type " 'where m_question.del_flag = '0'"
                'Test Type
                If Not IsDBNull(sel_test_type.SelectedItem.Value) And sel_test_type.SelectedItem.Value <> "" Then
                    strWhereSql1 = " AND m_question.test_type = '" & sel_test_type.SelectedItem.Value & "'"
                Else
                    strWhereSql1 = ""
                End If
                'Option
                If Not IsDBNull(txt_question.Text) And txt_question.Text <> "" Then
                    strWhereSql2 = " AND m_question.question LIKE N'%" & Replace(txt_question.Text, "'", "''") & "%'  "
                Else
                    strWhereSql2 = ""
                End If
                strWhereSql4 = " AND  m_testinfo.del_flag='0' where  m_question.Qn_Category_ID in(" + category + ")"
                strWhereSql = strWhereSql1 & strWhereSql2 & strWhereSql4

                If Not IsDBNull(strWhereSql) And strWhereSql <> "" Then
                    strMainSql = strSql & " " & strWhereSql & " and  m_Question.del_flag=0 order by m_question.qnid "
                Else
                    strMainSql = strSql & " and  m_Question.del_flag=0 order by m_question.qnid"
                End If

                If objCn.connect() Then

                    'Dim i As Integer
                    ds = New DataSet()
                    da = New SqlDataAdapter(strMainSql, objCn.MyConnection)
                    da.Fill(ds)
                    ht = New Hashtable
                    ht.Clear()


                    For a As Integer = 0 To ds.Tables(0).Rows.Count - 1

                        'Query for geting option of given question id.
                        sqlstr = ""
                        sqlstr = sqlstr & "SELECT ques.question, ques.Total_Marks,opt.[option],qlevel FROM "
                        sqlstr = sqlstr & "m_question AS ques, m_options AS opt WHERE "
                        sqlstr = sqlstr & "ques.qnid = opt.qnid AND "
                        sqlstr = sqlstr & "ques.test_type = opt.test_type AND "
                        sqlstr = sqlstr & "ques.qnid = " & ds.Tables(0).Rows(a).Item(1).ToString & "AND "
                        sqlstr = sqlstr & "ques.test_type = '" & sel_test_type.SelectedItem.Value & "' order by opt.optionid"
                        ds1 = New DataSet()
                        da1 = New SqlDataAdapter(sqlstr, objCn.MyConnection)
                        da1.Fill(ds1)
                        ht.Add(ds.Tables(0).Rows(a).Item(1).ToString, ds1.Tables(0).Rows(0).Item(0).ToString)

                        If ds1.Tables(0).Rows(0).Item(3) = 0 Then
                            strLevel = "B"
                        Else
                            strLevel = "I"
                        End If

                        htOpt = New Hashtable
                        htOptStr = New Hashtable
                        htOptStr.Clear()
                        htOpt.Clear()
                        Dim cell As Integer = 3
                        For opt As Integer = 0 To ds1.Tables(0).Rows.Count - 1

                            ImgNotFound = CheckImage(ds1.Tables(0).Rows(opt).Item(2).ToString)

                            '================================= Image in excel =====================================================
                            'Create option cell in Excel
                            If ImgNotFound(0) = "" Then
                                htOpt.Add(opt, ds1.Tables(0).Rows(opt).Item(2).ToString)
                                htOptStr.Add(opt, "")
                                App.ActiveSheet.Cells(rowOption, cell).Value = ds1.Tables(0).Rows(opt).Item(2).ToString
                                App.ActiveSheet.Cells(rowOption, cell).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                App.ActiveSheet.Cells(rowOption, cell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                'cell += 1
                            Else

                                If System.IO.File.Exists(Server.MapPath(ImgNotFound(0))) Then
                                    App.ActiveSheet.Cells(rowOption, cell).Value = ImgNotFound(1)
                                    Dim Align As Integer = 0
                                    If ImgNotFound(1) <> "" Then
                                        Align = 12
                                    End If
                                    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(ImgNotFound(0)))
                                    With pic
                                        .top = App.ActiveSheet.Cells(rowOption, cell).top + 12
                                        .left = App.ActiveSheet.Cells(rowOption, cell).left + 5
                                        .width = 70
                                        .height = 70
                                    End With
                                    htOpt.Add(opt, ImgNotFound(0))
                                    htOptStr.Add(opt, ImgNotFound(1))
                                    App.ActiveSheet.Cells(rowOption, cell).RowHeight = 83
                                    App.ActiveSheet.Cells(rowOption, cell).ColumnWidth = 15
                                    App.ActiveSheet.Cells(rowOption, cell).VerticalAlignment = Excel.XlVAlign.xlVAlignJustify
                                    App.ActiveSheet.Cells(rowOption, cell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                Else
                                    htOpt.Add(opt, ImgNotFound(0))
                                    htOptStr.Add(opt, ImgNotFound(1))
                                    App.ActiveSheet.Cells(rowOption, cell).Value = "Image Not Found"
                                    App.ActiveSheet.Cells(rowOption, cell).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                    App.ActiveSheet.Cells(rowOption, cell).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                End If


                            End If



                            App.ActiveSheet.Cells(rowOption, cell).Borders.Weight = 2
                            App.ActiveSheet.Cells(rowOption, cell).WrapText = True
                            cell += 1
                            '===============================================================================================================
                        Next
                        rowOption += 1
                        If category = "2,3" Then
                            For line As Integer = 0 To 41
                                App.ActiveSheet.Cells(rowOption - 1, 2 + line).Borders.Weight = 2
                            Next
                        ElseIf category = "1" Then
                            For line As Integer = 0 To 4
                                App.ActiveSheet.Cells(rowOption - 1, 2 + line).Borders.Weight = 2
                            Next

                        End If


                        'Query for geting correct answer for given question id
                        Dim CorrectAns As String = " select Correct_Opt_Id,Sub_ID from M_Question_Answer where Qn_ID = " _
                                        & ds.Tables(0).Rows(a).Item(1).ToString & " and Test_Type = " & sel_test_type.SelectedItem.Value
                        dsCorrectAns = New DataSet()
                        daCorrectAns = New SqlDataAdapter(CorrectAns, objCn.MyConnection)
                        daCorrectAns.Fill(dsCorrectAns)
                        strstartCell = rowCorrectAns.ToString
                        Dim cellOption As Integer = 23
                        If dsCorrectAns.Tables(0).Rows.Count > 0 Then
                            Dim No As Integer = 1
                            For g As Integer = 0 To dsCorrectAns.Tables(0).Rows.Count - 1
                                If category = "2,3" Then
                                    strtext = (dsCorrectAns.Tables(0).Rows(g).Item(0)).ToString
                                    App.ActiveSheet.Cells(rowOption - 1, cellOption).Value = strtext
                                    App.ActiveSheet.Cells(rowOption - 1, cellOption).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                    App.ActiveSheet.Cells(rowOption - 1, cellOption).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                    cellOption += 1
                                    If (g + 1 <= dsCorrectAns.Tables(0).Rows.Count - 1) Then
                                    Else
                                        crr += strtext
                                        ans = crr.Substring(0, crr.Length - 1) + Environment.NewLine
                                        No += 1
                                    End If
                                Else
                                    crr += (dsCorrectAns.Tables(0).Rows(g).Item(0)).ToString + ","
                                    ans = crr.Substring(0, crr.Length - 1)
                                    No += 1
                                End If

                            Next
                        End If

                        ed = rowOption - 1

                        'Question Cell
                        With App.ActiveSheet.Range("B" + st.ToString + ":B" + ed.ToString)
                            .MergeCells = True
                            .Columns.AutoFit()
                            '.Interior.ColorIndex = 40
                            '.Font.Bold = True
                            '.Font.ColorIndex = 53
                            '.Cells.Value = ds1.Tables(0).Rows(0).Item(0).ToString
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                            .WrapText = True
                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            Dim QuestImage(1) As String ' = CheckImage(ds1.Tables(0).Rows(0).Item(0).ToString)
                            QuestImage = CheckImage(ds1.Tables(0).Rows(0).Item(0))

                            '================================= Image in excel =====================================================
                            If QuestImage(0) <> "" Then

                                If System.IO.File.Exists(Server.MapPath(QuestImage(0))) Then
                                    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(QuestImage(0)))
                                    With pic

                                        .top = App.ActiveSheet.Cells(st, 2).top + 15
                                        .left = App.ActiveSheet.Cells(st, 2).left + 30
                                        .width = 68
                                        .height = 68
                                    End With
                                    .Cells.Value = QuestImage(1).ToString
                                    .VerticalAlignment = Excel.XlVAlign.xlVAlignJustify
                                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                Else
                                    .Cells.Value = QuestImage(1).ToString + Environment.NewLine + "Image not found"
                                End If
                            Else
                                '.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                '.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                .Cells.Value = ds1.Tables(0).Rows(0).Item(0).ToString
                            End If


                        End With
                        If category = "1" Then

                            '=============================== Image in excel =========================================
                            'Correct Option
                            With App.ActiveSheet.Range("G" + st.ToString + ":G" + ed.ToString)
                                '.MergeCells = True
                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                .BORDERS(xlEdgeLeft).Weight = 2
                                .BORDERS(xlEdgeTop).Weight = 2
                                .BORDERS(xlEdgeBottom).Weight = 2
                                .BORDERS(xlEdgeRight).Weight = 2
                                .WrapText = True
                                If ImgNotFound(0) <> "" Then
                                    'If System.IO.File.Exists(Server.MapPath(crr)) Then
                                    '    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(crr))
                                    '    With pic
                                    '        Dim jk As Integer = App.ActiveSheet.Cells(st, 5).left
                                    '        .top = App.ActiveSheet.Cells(st, 5).top
                                    '        .left = App.ActiveSheet.Cells(st, 5).left + 50
                                    '        .width = 70
                                    '        .height = 70
                                    '    End With
                                    '    .Cells.Value = crrOptstr.ToString
                                    '    .VerticalAlignment = Excel.XlVAlign.xlVAlignJustify
                                    '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                    'Else
                                    '    .Cells.Value = crr
                                    'End If
                                    ''  .Cells.Value = ans
                                    '.Cells.Value = crr
                                Else
                                    '' .Cells.Value = crr
                                End If
                                .Cells.Value = ans
                                crrOptstr = String.Empty

                            End With
                            '======================================================================


                            'Difficulty Level
                            With App.ActiveSheet.Range("H" + st.ToString + ":H" + ed.ToString)
                                '.MergeCells = True
                                '.Interior.ColorIndex = 40
                                '.Font.Bold = True
                                '.Font.ColorIndex = 53
                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                .Cells.Value = strLevel
                                '.Font.Size = 15
                                .BORDERS(xlEdgeLeft).Weight = 2
                                .BORDERS(xlEdgeTop).Weight = 2
                                .BORDERS(xlEdgeBottom).Weight = 2
                                .BORDERS(xlEdgeRight).Weight = 2
                                .WrapText = True
                            End With

                            'Question Type
                            With App.ActiveSheet.Range("I" + st.ToString + ":I" + ed.ToString)
                                '.MergeCells = True
                                '.Interior.ColorIndex = 40
                                '.Font.Bold = True
                                '.Font.ColorIndex = 53
                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

                                '.Font.Size = 15
                                .BORDERS(xlEdgeLeft).Weight = 2
                                .BORDERS(xlEdgeTop).Weight = 2
                                .BORDERS(xlEdgeBottom).Weight = 2
                                .BORDERS(xlEdgeRight).Weight = 2
                                .WrapText = True
                                If dsCorrectAns.Tables(0).Rows.Count = 1 Then
                                    .Cells.Value = "True/False or Single Answer"
                                Else
                                    .Cells.Value = "Multiple choice"
                                End If

                            End With

                            'Marks
                            With App.ActiveSheet.Range("J" + st.ToString + ":J" + ed.ToString)
                                '.MergeCells = True
                                '.Interior.ColorIndex = 40
                                '.Font.Bold = True
                                '.Font.ColorIndex = 53
                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                .Cells.Value = ds1.Tables(0).Rows(0).Item(1).ToString
                                '.Font.Size = 15
                                .BORDERS(xlEdgeLeft).Weight = 2
                                .BORDERS(xlEdgeTop).Weight = 2
                                .BORDERS(xlEdgeBottom).Weight = 2
                                .BORDERS(xlEdgeRight).Weight = 2
                                .WrapText = True
                            End With
                        ElseIf category = "2,3" Then
                            'Marks
                            With App.ActiveSheet.Range("AQ" + st.ToString + ":AQ" + ed.ToString)
                                '.MergeCells = True
                                '.Interior.ColorIndex = 40
                                '.Font.Bold = True
                                '.Font.ColorIndex = 53
                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                .Cells.Value = ds1.Tables(0).Rows(0).Item(1).ToString
                                '.Font.Size = 15
                                .BORDERS(xlEdgeLeft).Weight = 2
                                .BORDERS(xlEdgeTop).Weight = 2
                                .BORDERS(xlEdgeBottom).Weight = 2
                                .BORDERS(xlEdgeRight).Weight = 2
                                .WrapText = True
                            End With

                            'Difficulty Level
                            With App.ActiveSheet.Range("AR" + st.ToString + ":AR" + ed.ToString)
                                '.MergeCells = True
                                '.Interior.ColorIndex = 40
                                '.Font.Bold = True
                                '.Font.ColorIndex = 53
                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                .Cells.Value = strLevel
                                '.Font.Size = 15
                                .BORDERS(xlEdgeLeft).Weight = 2
                                .BORDERS(xlEdgeTop).Weight = 2
                                .BORDERS(xlEdgeBottom).Weight = 2
                                .BORDERS(xlEdgeRight).Weight = 2
                                .WrapText = True
                            End With
                        End If

                        st = rowOption
                        rownum = rownum + 1
                        crr = String.Empty
                        ans = String.Empty
                    Next

                End If
                If Not objCn.MyConnection Is Nothing Then
                    If objCn.MyConnection.State = ConnectionState.Open Then
                        objCn.disconnect()
                    End If
                End If

                'Save Workbook
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                If System.IO.File.Exists(Server.MapPath("Excel Import\ExportedQuestion.xls")) Then
                    System.IO.File.Delete(Server.MapPath("Excel Import\ExportedQuestion.xls"))
                End If

                fileName1 = Server.MapPath("Excel Import\ExportedQuestion.xls")


                WorkBook.SaveAs(fileName1, objOpt, objOpt, objOpt, objOpt, objOpt, _
                                Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
                WorkBooks.Close()

                'Added by Pragnesha Kulkarni on 2018/06/01
                ' Reason:Excel process didn't stop after downloading excel sheet
                ' BugID: 719
                App.Quit()
                Dim dateEnd As Date = Date.Now
                ' End_Excel_App(datestart, dateEnd)

                ' Bug ID: 0904 Getting error on end exam click
                ' Desc: Added code of excel process cpu utilization check and kill process as utilization will equal to zero.
                ' Added by Pragnesha on 23-5-2019
                Excel_Stop()
                
        'Ended by Pragnesha Kulkarni on 2018/06/01

        ' By Nisha on 2018/05/17
        ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim file As New IO.FileInfo(Server.MapPath("Excel Import\ExportedQuestion.xls"))

                If file.Exists = True Then

                    Response.Clear()
                    Response.ClearHeaders()
                    Response.ClearContent()
                    Response.ContentType = "application/ms-excel"
                    Response.AppendHeader("Content-disposition", "attachment; filename=" + file.Name)
                    Response.AddHeader("Content-Length", file.Length.ToString)
                    Response.ContentType = "application/octet-stream"
                    Response.WriteFile(file.FullName)
                    Response.Flush()

                End If
        ' By Nisha on 2018/05/17
        ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                System.IO.File.Delete(Server.MapPath("Excel Import\ExportedQuestion.xls"))

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If

                If Not objCn.MyConnection Is Nothing Then
                    If objCn.MyConnection.State = ConnectionState.Open Then
                        objCn.disconnect()
                    End If
                End If
        'Added by Pragnesha Kulkarni on 2018/06/01
        ' Reason:To release objects
        ' BugID: 719
                ReleaseObject(App)
                ReleaseObject(WorkBooks)
                ReleaseObject(WorkBook)
                ReleaseObject(Sheet)
                ReleaseObject(Sheets)
                ReleaseObject(objOpt)
                Response.Redirect("error.aspx")
                Throw ex
        'Ended by Pragnesha Kulkarni on 2018/06/01

            Finally
                If Not objCn.MyConnection Is Nothing Then
                    If objCn.MyConnection.State = ConnectionState.Open Then
                        objCn.disconnect()
                    End If
                End If

        'Added by Pragnesha Kulkarni on 2018/06/01
        ' Reason:To release objects
        ' BugID: 719
                ReleaseObject(App)
                ReleaseObject(WorkBooks)
                ReleaseObject(WorkBook)
                ReleaseObject(Sheet)
                ReleaseObject(Sheets)
                ReleaseObject(objOpt)
        'Ended by Pragnesha Kulkarni on 2018/06/01

                App = Nothing
                WorkBooks = Nothing
                WorkBook = Nothing
                Sheet = Nothing
                Sheets = Nothing
                objOpt = Nothing
                objCn = Nothing
                sit1 = Nothing
                strMainSql = Nothing
                strSql = Nothing
                strWhereSql = Nothing
                strWhereSql1 = Nothing
                strWhereSql2 = Nothing
                sqlstr = Nothing
                strWhereSql4 = Nothing
                strstartCell = Nothing
                strpath = Nothing
                strLevel = Nothing
                strtext = Nothing
                ImgNotFound = Nothing
        'crrOptstr = Nothing
        ' crr = Nothing
                ds = Nothing
                dsCorrectAns = Nothing
                ds1 = Nothing
                da = Nothing
                da1 = Nothing
                daCorrectAns = Nothing
                ht = Nothing
                htOpt = Nothing
                htOptStr = Nothing
                pic = Nothing
                fileName1 = Nothing
        'htCrr = Nothing



            End Try
        End Sub

        'Added by Pragnesha Kulkarni on 2018/06/01
        ' Reason:Excel process didn't stop after downloading excel sheet
        ' BugID: 719
        Private Sub ReleaseObject(ByVal obj As Object)
            Try
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
                obj = Nothing
            Catch ex As Exception
                obj = Nothing
            Finally
                GC.Collect()
                GC.WaitForPendingFinalizers()
            End Try
        End Sub
        Private Sub End_Excel_App(ByVal datestart As Date, ByVal dateEnd As Date)
            'Dim xlp() As Process = Process.GetProcessesByName("EXCEL")
            'For Each Process As Process In xlp
            '    If Process.StartTime >= datestart And Process.StartTime <= dateEnd Then
            '        Process.Kill()
            '        Exit For
            '    End If
            'Next
        End Sub
        ' Bug ID: 0904 Getting error on end exam click
        ' Desc: Added code of excel process cpu utilization check and kill process as utilization will equal to zero.
        ' Added by Pragnesha on 23-5-2019
        Private Sub Excel_Stop()
            Dim pListOfProcesses() As Process
            Dim pExcelProcess As System.Diagnostics.Process
            pListOfProcesses = Process.GetProcesses
            For Each pExcelProcess In pListOfProcesses
                If pExcelProcess.ProcessName.ToUpper = "EXCEL" Then
                    Dim myAppCpu As PerformanceCounter = New PerformanceCounter("Process", "% Processor Time", "EXCEL", True)


                    Dim pct As Double = myAppCpu.NextValue()
                    ' Console.WriteLine("EXCEL'S CPU % = " & pct)
                    Thread.Sleep(250)
                    If pct = 0.0 Then
                        pExcelProcess.Kill()
                    End If

                End If
            Next
        End Sub
        'Ended by Pragnesha Kulkarni on 2018/06/01

#End Region
#Region "CheckImage Function"
        'Added by    : Saraswati Patel
        'Description : For Check Whether string are html type or not

        Public Function CheckImage(ByVal value As String) As String()
            Dim str As String
            Dim strVal(1) As String
            Try
                If value.Contains("<br/>") Then
                    'Dim a As String = (value.IndexOf("=") + 1)
                    'Dim b As String = (value.IndexOf(" ", (value.IndexOf("=") + 1)))
                    str = value.Substring((value.IndexOf("=") + 1), _
                                          ((value.IndexOf(" ", (value.IndexOf("=") + 1))) - (value.IndexOf("=") + 1)))
                    strVal(0) = str
                    strVal(1) = value.Substring(0, value.IndexOf("<"))
                    'str = value.Substring(14, (value.IndexOf(" ", 15) - 14))
                    Return strVal
                ElseIf value.Contains("<img") Then
                    ' str = value.Substring((value.IndexOf("=") + 1), (value.IndexOf(" ", 15) - (value.IndexOf("=") + 1)))
                    str = value.Substring((value.IndexOf("=") + 1), _
                                          ((value.IndexOf(" ", (value.IndexOf("=") + 1))) - (value.IndexOf("=") + 1)))
                    strVal(0) = str
                    strVal(1) = value.Substring(0, value.IndexOf("<"))
                    Return strVal
                Else
                    strVal(0) = ""
                    strVal(1) = ""
                End If
                Return strVal
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                str = Nothing
                strVal = Nothing
            End Try
        End Function
#End Region
#Region "ExportQueAnsInExcel_Click"
        'Added by:: Saraswati Patel 
        'For eport question and answer in excel file
        'Protected Sub ExportQueAnsInExcel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ExportQueAnsInExcel.Click

        'End Sub
#End Region



        Protected Sub chkRemove_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        End Sub

        'Protected Sub ExportMatchTheCol_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ExportMatchTheCol.Click

        'End Sub
        '#Region "ExportQueAnsInExcel_Click"
        '        'Added by:: Saraswati Patel 
        '        'For eport question and answer in excel file
        '        Protected Sub BtnExTF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExTF.Click
        '            If sel_test_type.SelectedIndex > 0 Then
        '                lblMsg.Text = ""
        '                Dim SubjectName As String = populatesubject(sel_test_type.SelectedItem.Value)
        '                Getit(SubjectName, "1")

        '            Else
        '            End If
        '        End Sub

        '        Protected Sub BtnExMultiCho_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExMultiCho.Click
        '            If sel_test_type.SelectedIndex > 0 Then
        '                lblMsg.Text = ""
        '                Dim SubjectName As String = populatesubject(sel_test_type.SelectedItem.Value)
        '                ' Getit(SubjectName)
        '                Getit(SubjectName, "2,3")

        '            Else
        '                ' lblMsg.Text = "Please Select Atleast One Iteam"
        '            End If
        '        End Sub
        '#End Region
        '#Region "ExportQueAnsInExcel_Click"
        '        'Added by:: Saraswati Patel 
        '        'For eport question and answer in excel file
        '        Protected Sub BtnExTF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExTF.Click
        '            If sel_test_type.SelectedIndex > 0 Then
        '                lblMsg.Text = ""
        '                Dim SubjectName As String = populatesubject(sel_test_type.SelectedItem.Value)
        '                Getit(SubjectName, "1")

        '            Else
        '            End If
        '        End Sub

        '        Protected Sub BtnExMultiCho_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnExMultiCho.Click
        '            If sel_test_type.SelectedIndex > 0 Then
        '                lblMsg.Text = ""
        '                Dim SubjectName As String = populatesubject(sel_test_type.SelectedItem.Value)
        '                ' Getit(SubjectName)
        '                Getit(SubjectName, "2,3")

        '            Else
        '                ' lblMsg.Text = "Please Select Atleast One Iteam"
        '            End If
        '        End Sub
        '#End Region

#Region "ExportQueAnsInExcel_Click"
        'Added by:: Saraswati Patel 
        'For eport question and answer in excel file  
        Protected Sub ExportQueAnsTF_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExportQueAnsTF.Click
            lblMsg.Text = ""
            gridDiv.Visible = False
            DGData.Visible = False
            If sel_test_type.SelectedIndex > 0 Then

                Dim SubjectName As String = populatesubject(sel_test_type.SelectedItem.Value)
                Getit(SubjectName, "1")

            Else
            End If
            'sel_test_type.AutoPostBack = True
            sel_test_type.SelectedIndex = 0
        End Sub

        Protected Sub ExportQA_Match_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExportQA_Match.Click
            lblMsg.Text = String.Empty
            DGData.Visible = False
            gridDiv.Visible = False
            If sel_test_type.SelectedIndex > 0 Then

                Dim SubjectName As String = populatesubject(sel_test_type.SelectedItem.Value)
                ' Getit(SubjectName)
                Getit(SubjectName, "2,3")

            Else
                ' lblMsg.Text = "Please Select Atleast One Iteam"
            End If
            'sel_test_type.AutoPostBack = True
            sel_test_type.SelectedIndex = 0
        End Sub
#End Region

        Protected Sub sel_test_type_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles sel_test_type.SelectedIndexChanged

            Session("DDLSelectedValue") = sel_test_type.SelectedValue.ToString()
            flg = True
            intCo = 1

        End Sub


        Protected Sub DGData_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DGData.SelectedIndexChanged

        End Sub
    End Class
End Namespace
