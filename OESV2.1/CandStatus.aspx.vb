#Region "Namespaces"
Imports System
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.UI.WebControls
Imports System.Web.Security
Imports System.Collections.Generic
Imports System.Collections
Imports log4net
Imports System.Web.UI
Imports Microsoft.Office.Interop
Imports System.Drawing
Imports System.Globalization
Imports org.apache.pdfbox.pdmodel
Imports org.apache.pdfbox.pdmodel.edit
Imports org.apache.pdfbox.pdmodel.font
Imports org.apache.pdfbox
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Web.Mail

#End Region

Namespace unirecruite

#Region "Class CandStatus"
    'Desc: This is CandStatus class
    'By: Jatin Gangajaliya, 2011/3/10


    Partial Class CandStatus

#Region "Declaration"
        Inherits BasePage

        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("CandStatus")
        Dim objconn As New ConnectDb

#End Region

#Region " Web Form Designer Generated Code "


        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Variables"
        Dim m_DS As DataSet
        Dim m_CntDel As Integer
        Dim strPathDb As String
        Dim myCommand As SqlCommand
        Dim MyVisiblePropertyOnCodeBehind As Boolean = False

#End Region

#Region "Page_Load"

        Private Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim Ds As DataSet
            Try
                TxtFrom.Attributes.Add("type", "date")
                TxtTo.Attributes.Add("type", "date")
                txtAppFromDate.Attributes.Add("type", "date")
                TxtAppToDate.Attributes.Add("type", "date")
                txtnewdate.Attributes.Add("type", "date")
                txtReassign.Attributes.Add("type", "date")
                'TxtFrom.Attributes.Add("Readonly", "true")
                'TxtTo.Attributes.Add("Readonly", "true")
                'txtAppFromDate.Attributes.Add("Readonly", "true")
                'TxtAppToDate.Attributes.Add("Readonly", "true")
                'Disable for Demo
                'btnPrint.Visible = False

                'txtnewdate.Attributes.Add("Readonly", "true")
                'Put user code to initialize the page here
                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                'added by bhumi [9/9/2015]
                'Reason: for UniUserType null value redirect to login page instead of error page
                If Session("UniUserType") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                'Ended by bhumi
                'modified by bhumi [9/9/2015]
                'Reason: for null value .ToString is not working properly so use Convert.Tostring instead of ToString
                ' If Convert.ToString(Session("UniUserType"))<>"1" Then ' commented by pragnesha for super admin
                If Convert.ToString(Session("UniUserType")) > "2" Then
                    Response.Redirect("~\register.aspx", False)
                End If

                'Ended by bhumi
                If Session("LoginGenuine") Is Nothing Then
                    Response.Redirect("error.aspx?err=Session Timeout. Please Login to continue.", False)
                End If

                'Added By   : Pragnesha Kulkarni 
                'Date         : 25/07/24
                'Description : Checks usertype.
                'Bug ID       : 670
                '--------------------------------------------------------------------------------------
                '  If Session("UserName") = "SuperAdmin" Then
                If Convert.ToString(Session("UniUserType")) = "2" Then
                    MyVisiblePropertyOnCodeBehind = True
                End If
                'imgBtnDelete.Attributes.Add("OnClick", "return ConDel();")
                'imgBtnDelete.Attributes.Add("OnClick", "return checkAll();")
                '-------------------------------------------------------------------------------------

                If Not Page.IsPostBack Then
                    Session.Add("DS", Nothing)
                    Me.SetFocus(dblCenter)
                    FillCombo()
                    FillListOfCourse_New()
                    ' FillSubjectCombo()

                    'FillCenterCombo()
                    populate_subjectid()
                Else
                    ' BindGrid() 
                    If DGReport.Visible = True Then
                        fillPageNumbers(DGReport.CurrentPageIndex + 1, 9)
                    End If

                End If


                'If IsPostBack Then
                '    BindGrid()
                'End If

                Page.RegisterHiddenField("__EVENTTARGET", "BtnSearch")
                'btnDelete.Attributes.Add("OnClick", "return checkAll();")
                Session("FromPage") = "CandStatus"

                'If Session("fromprint") = "true" Then
                '    Session.Remove("fromprint")
                '    If Session("gridvisible") = "true" Then
                '        Session.Remove("gridvisible")
                '        If Request.QueryString("pi") <> Nothing Then
                '            DGReport.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                '            Ds = Session("data")
                '            DGReport.DataSource = Ds
                '            DGReport.DataBind()
                '            Session.Remove("data")
                '            gridDiv.Visible = True
                '            btnPrint.Visible = True
                '            LblMsg.Visible = False
                '        End If
                '    End If
                'End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                Ds = Nothing
            End Try
        End Sub
#End Region
#Region "Fill Center DropDownList"


        Public Sub FillCenterCombo()

            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim datareader As SqlDataReader

            dblCenter.Items.Clear()
            Dim l1 As New ListItem
            l1.Text = "---- Select ----"
            l1.Value = 0

            dblCenter.Items.Add(l1)
            Try
                If objconn.connect() = True Then

                    '********************************************************************Raj
                    'Code added by Indravadan Vasava
                    'Purpose: To fill the combo box for Centre 
                    '********************************************************************
                    query = "select Center_ID,Center_Code,Center_Name,Add1 from M_Centers"

                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()
                    Dim result As String = ""
                    If Request.QueryString("userid") <> Nothing Then
                        result = GetCenterIDFromDB(Request.QueryString("userid"))
                    ElseIf Session.Item("userid") <> Nothing Or Session.Item("userid") <> 0 Then
                        result = GetCenterIDFromDB(Session.Item("userid"))
                    End If

                    While datareader.Read()
                        Dim lstItm As New ListItem()
                        lstItm.Enabled = True
                        lstItm.Text = "[" & datareader.Item(1) & "] " & datareader.Item(2) & ", " & datareader.Item(3)
                        lstItm.Value = datareader.Item(0)
                        If result <> "" Then
                            If datareader.Item(0) = result Then
                                lstItm.Selected = True
                            End If
                        End If
                        dblCenter.Items.Add(lstItm)
                    End While

                    datareader.Close()
                    If objconn IsNot Nothing Then
                        objconn.disconnect()
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
                datareader = Nothing
            End Try

        End Sub
#End Region
#Region "Populate Subject ID"

        Private Sub populate_subjectid()
            Dim myDataReader As SqlDataReader
            Dim sqlstr As String
            Dim myTable As DataTable
            Dim myRow As DataRow
            Dim objconn As New ConnectDb
            Dim strbr As StringBuilder
            Try
                sqlstr = ""
                '***********************************************************
                'Order by clause added by Tabrez 
                'Purpose: To get data in sorted order.
                '***********************************************************

                strbr = New StringBuilder
                strbr.Append(" SELECT Center_Id,Center_Name FROM M_Centers order by Center_Name")
                sqlstr = strbr.ToString

                'myDataReader = retTestInfo(sqlstr)

                myTable = New DataTable
                myTable.Columns.Add(New DataColumn("Center_Id", GetType(String)))
                myTable.Columns.Add(New DataColumn("Center_Name", GetType(String)))

                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                If objconn.connect() Then
                    Dim myCommand As SqlCommand
                    myCommand = New SqlCommand(sqlstr.ToString(), objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader



                    ' While loop to populate the Datatable
                    'If objconn.connect(strPathDb) Then
                    While myDataReader.Read
                        myRow = myTable.NewRow
                        myRow(0) = myDataReader.Item("Center_Id")
                        myRow(1) = myDataReader.Item("Center_Name")
                        myTable.Rows.Add(myRow)
                    End While
                End If
                dblCenter.DataSource = myTable
                dblCenter.DataTextField = "Center_Name"
                dblCenter.DataValueField = "Center_Id"
                dblCenter.DataBind()
                dblCenter.Items.Insert(0, New ListItem("---- Select ----", "0"))
                myDataReader.Close()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                Throw ex
            Finally
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                objconn = Nothing
                sqlstr = Nothing
                myDataReader = Nothing
                myTable = Nothing
                myRow = Nothing
                strbr = Nothing
            End Try
        End Sub

#End Region

#Region "Function RetTestinfo"

        Private Function retTestInfo(ByVal sqlstr As String) As SqlDataReader
            Dim objconn As New ConnectDb
            Try
                Dim myCommand As SqlCommand

                'Code added by PRatik
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    retTestInfo = myCommand.ExecuteReader
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
            End Try
        End Function

#End Region


#Region "Get CenterID From Database"
        Public Function GetCenterIDFromDB(ByVal userid As String)
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim result As String = ""
            Dim datareader As SqlDataReader
            dblCourse.Items.Clear()
            Try
                If objconn.connect() = True Then

                    '********************************************************************Raj
                    'Code added by Indravadan Vasava
                    'Purpose: To Course for User from database
                    '********************************************************************
                    query = "select Center_ID from M_User_Info where Userid=" & userid

                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()


                    While datareader.Read()
                        result = datareader.Item(0).ToString()
                    End While

                    datareader.Close()
                    If objconn IsNot Nothing Then
                        objconn.disconnect()
                    End If
                End If
                Return result
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
            End Try
            Return result
        End Function
#End Region

#Region "Fill DropdownList"
        Private Sub FillCombo()
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim dtData As DataTable
            'Dim strPathDb As String

            Try

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                If objconn.connect() Then
                    'Dim rows As DataRow
                    'sqlstr = "SELECT distinct test_type,test_name FROM m_testinfo WHERE del_flag ='0'" ' and course_id = " & dblCourse.SelectedValue.ToString()
                    'myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    'myDataReader = myCommand.ExecuteReader()
                    'dtData = New DataTable
                    'dtData.Columns.Add(New DataColumn("test_type", GetType(String)))
                    'dtData.Columns.Add(New DataColumn("test_name", GetType(String)))
                    'While myDataReader.Read
                    '    rows = dtData.NewRow
                    '    rows(0) = myDataReader.Item("test_type")
                    '    rows(1) = myDataReader.Item("test_name")
                    '    dtData.Rows.Add(rows)
                    'End While
                    'ddlTestName.DataSource = dtData
                    'ddlTestName.DataValueField = "test_type"
                    'ddlTestName.DataTextField = "test_name"
                    'ddlTestName.DataBind()
                    'ddlTestName.Items.Insert(0, "ALL")
                    'ddlResult.Items.Insert(0, "ALL")
                    'ddlResult.Items.Insert(1, "Pass")
                    'ddlResult.Items.Insert(2, "Reject")

                    ddlStatus.Items.Insert(0, "ALL")
                    ddlStatus.Items.Insert(1, "Assigned")
                    ddlStatus.Items.Insert(2, "Appeared")
                    'ddlStatus.Items.Insert(2, "Ongoing")
                    ddlStatus.Items.Insert(3, "LoginUser")
                    ddlStatus.Items.Insert(4, "LinkBreak")
                    dblCourse.Items.Insert(0, "---- Select ----")

                    '  myCommand.Dispose()
                    '  myDataReader.Close()
                    If objconn IsNot Nothing Then
                        objconn.disconnect()
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
                sqlstr = Nothing
                strPathDb = Nothing
                myCommand = Nothing
                myDataReader = Nothing

            End Try

        End Sub
#End Region

        '#Region "Page_Unload"
        '        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '            Try
        '                Dim strPathDb As String
        '                If objconn.connect(strPathDb) = True Then
        '                    objconn.disconnect()
        '                End If
        '            Catch ex As Exception
        '                objconn.disconnect()
        '            End Try
        '            'objconn.disconnect()

        '        End Sub
        '#End Region

#Region "BindGrid"
        Private Sub BindGrid()
            Dim intpginx As Integer

            '/************Jatin Gangajaliya,2011/2/17*************/
            Dim objCommon As New CommonFunction
            Dim ids As String = objCommon.GetCommaSeperatedIDS("select Main_Course_id from M_Main_Course where marksheet_flag=0")
            Dim query As String
            Dim g, j, k As Integer
            Dim i As String
            Static Dim counter As Integer
            Static Dim start As Integer = 33
            Dim dt As New DataTable()
            Dim conn As SqlConnection
            Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
            Dim adap As SqlDataAdapter
            Dim ary() As String
            Dim strbr As StringBuilder
            Dim cmd As SqlCommand
            Dim intflag As Integer = 1
            Dim appearedCount As Integer = 0
            Dim assignedCount As Integer = 0
            Dim assigned_runningCount As Integer = 0

            Dim intget As Integer = 0
            Dim intatt As Integer = 0
            Static Dim intco As Integer = 0

            Static Dim aryindex() As Integer

            Dim rdr As SqlDataReader
            Static Dim intnu As Integer = 0
            '/************Jatin Gangajaliya,2011/2/17*************/

            Try
                CreateDataSet()
                If objconn.connect() = True Then
                    If m_DS.Tables(0).Rows.Count > 0 Then

                        'DGReport.DataSource = m_DS
                        DGReport.Visible = True
                        LblRecCnt.Visible = True
                        LblRecCnt.Text = ": " & m_DS.Tables(0).Rows.Count.ToString()
                        'LblRecCnt.Text = m_DS.Tables(0).Rows.Count.ToString()-
                        'btnDelete.Visible = True

                        'Disable for Demo
                        ' btnPrint.Visible = True

                    Else
                        'Disable for Demo
                        'btnPrint.Visible = False

                        LblRecCnt.Text = ""
                        DGReport.Visible = False
                    End If


                    DGReport.DataMember = "TblResults"

                    If objconn Is Nothing Then
                        objconn = New ConnectDb
                    End If
                    LblMsg.Visible = False
                    If (m_DS.Tables(0).Rows.Count > 0) Then

                        DGReport.Visible = True
                        '/****************start*****************/
                        Dim dttemp As DataTable
                        dttemp = m_DS.Tables(0)

                        dttemp.Columns.Add("Sr.No.")

                        For ii As Integer = 0 To dttemp.Rows.Count - 1
                            dttemp.Rows(ii).Item("Sr.No.") = ii + 1


                        Next

                        DGReport.DataSource = dttemp

                        If dttemp.Rows.Count <= DGReport.CurrentPageIndex * 10 Then
                            DGReport.CurrentPageIndex = DGReport.CurrentPageIndex - 1
                            ViewState.Add("selval", DGReport.CurrentPageIndex)
                            ViewState.Add("pageNo", DGReport.CurrentPageIndex + 1)
                        End If
                        ViewState.Add("selval", DGReport.CurrentPageIndex)
                        DGReport.DataBind()
                        'fillPagesCombo()
                        fillPageNumbers(DGReport.CurrentPageIndex + 1, 9)

                        gridDiv.Visible = True
                        For intlpcounter As Integer = 0 To DGReport.Items.Count - 1
                            Dim intid As Integer = DGReport.Items(intlpcounter).Cells(1).Text
                            Dim strcname As String = DGReport.Items(intlpcounter).Cells(5).Text

                            strbr = New StringBuilder
                            strbr.Append(" Select Appearedflag from t_candidate_status where userid = ")
                            strbr.Append("'")
                            strbr.Append(intid)
                            strbr.Append("'")
                            strbr.Append(" AND Course_id = (Select Course_id from M_Course where Course_name =  ")
                            strbr.Append("'")
                            strbr.Append(strcname)
                            strbr.Append("'")
                            'strbr.Append(" and Main_Course_ID = '1' ")
                            strbr.Append(" and Main_Course_ID IN (" & ids & ") ")

                            strbr.Append(" ) ")
                            Dim bool As Boolean = True
                            Dim Mainid As Integer = 2
                            'If objconn.connect(strPathDb) = True Then
                            cmd = New SqlCommand(strbr.ToString, objconn.MyConnection)
                            rdr = cmd.ExecuteReader()
                            While (rdr.Read())
                                Mainid = 1
                                bool = False
                                If (rdr("appearedflag") = 2) Then
                                    intflag = 2
                                    ' appearedCount = appearedCount + 1
                                ElseIf (rdr("appearedflag") = 1) Then
                                    intflag = 1
                                    Exit While
                                ElseIf (rdr("appearedflag") = 0) Then
                                    intflag = 0
                                    Exit While
                                End If
                            End While
                            rdr.Close()

                            'End If

                            DGReport.Columns(14).Visible = True
                            DGReport.Columns(9).Visible = True
                            DGReport.Columns(10).Visible = True

                            If bool = True Then
                                'Added By Vaibhav Soni
                                'Date :- 2014/04/10
                                '10 => Disable Exam
                                '16 => Provision Marksheet
                                '19 => Remind
                                '20 => Re-assign
                                'Ended By Vaibhav Soni
                                DGReport.Items(intlpcounter).Cells(16).Enabled = False
                                '  DGReport.Items(intlpcounter).Cells(15).ToolTip = "Exam(s) are pending"
                                DGReport.Items(intlpcounter).Cells(16).Controls.Clear()
                                'Added By Vaibhav Soni
                                'Date :- 2014/03/10
                                DGReport.Items(intlpcounter).Cells(19).Enabled = False
                                DGReport.Items(intlpcounter).Cells(19).Controls.Clear()
                                'Ended By Vaibhav Soni
                                'Added By Vaibhav Soni
                                'Date :- 2014/04/10
                                DGReport.Items(intlpcounter).Cells(20).Enabled = False
                                DGReport.Items(intlpcounter).Cells(20).Controls.Clear()
                                'Ended By Vaibhav Soni
                            End If

                            If (intflag = 0 Or intflag = 1) Then
                                DGReport.Items(intlpcounter).Cells(16).Enabled = False
                                'DGReport.Items(intlpcounter).Cells(15).FindControl()
                                '  DGReport.Items(intlpcounter).Cells(15).ToolTip = "Exam(s) are pending"
                                DGReport.Items(intlpcounter).Cells(16).Controls.Clear()
                                'Added By Vaibhav Soni
                                'Date :- 2014/04/10
                                DGReport.Items(intlpcounter).Cells(20).Enabled = False
                                DGReport.Items(intlpcounter).Cells(20).Controls.Clear()
                                'Ended By Vaibhav Soni
                            End If
                            If intflag <> 0 Then
                                '  DGReport.Items(intlpcounter).Cells(9).Enabled = False
                                If intflag = 0 Then
                                    '   DGReport.Columns(16).Visible = False
                                ElseIf intflag = 1 Then
                                    DGReport.Items(intlpcounter).Cells(10).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(10).Controls.Clear()
                                    DGReport.Items(intlpcounter).Cells(16).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(16).Controls.Clear()
                                    'Added By Vaibhav Soni
                                    'Date :- 2014/03/10
                                    DGReport.Items(intlpcounter).Cells(19).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(19).Controls.Clear()
                                    'Ended By Vaibhav Soni
                                    '    DGReport.Columns(16).Visible = False
                                    '   DGReport.Columns(9).Visible = True
                                ElseIf intflag = 2 Then
                                    '  DGReport.Columns(10).Visible = False
                                    ' DGReport.Columns(9).Visible = False
                                    DGReport.Items(intlpcounter).Cells(9).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(9).Controls.Clear()
                                    DGReport.Items(intlpcounter).Cells(10).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(10).Controls.Clear()
                                    'Added By Vaibhav Soni
                                    'Date :- 2014/03/10
                                    DGReport.Items(intlpcounter).Cells(19).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(19).Controls.Clear()
                                    'Ended By Vaibhav Soni
                                End If

                                '   DGReport.Items(intlpcounter).Cells(9).Controls.Clear()

                            End If
                            If Mainid = 2 Then
                                DGReport.Items(intlpcounter).Cells(16).Enabled = False
                                '  DGReport.Items(intlpcounter).Cells(15).ToolTip = "Exam(s) are pending"
                                '  DGReport.Items(intlpcounter).Cells(16).Visible = False

                                DGReport.Items(intlpcounter).Cells(16).Controls.Clear()
                                '    DGReport.Items(intlpcounter).Cells(9).Enabled = False
                                '    DGReport.Items(intlpcounter).Cells(9).ToolTip = "Exam(s) are appeared already."
                            End If
                            DGReport.Items(intlpcounter).Cells(10).Attributes.Add("onclick", "return confirm('Are you sure you want to disable an exam ?');")

                            'Added by : Saraswati Patel
                            'Desc : For grid row color chang if course are disable
                            strbr = Nothing
                            cmd = Nothing
                            strbr = New StringBuilder
                            strbr.Append("select Del_Flag from M_Course where Course_id=(Select Course_id from M_Course ")
                            strbr.Append(" where Course_name =  '")
                            strbr.Append(strcname)
                            strbr.Append("' and Main_Course_ID IN (1,2)  ) ")

                            cmd = New SqlCommand(strbr.ToString, objconn.MyConnection)
                            rdr = cmd.ExecuteReader()
                            While (rdr.Read())
                                Dim ss As String = rdr("Del_Flag").ToString
                                If (rdr("Del_Flag") = "True") Then
                                    DGReport.Items(intlpcounter).BackColor = Drawing.Color.Gray
                                    DGReport.Items(intlpcounter).Cells(16).Enabled = False
                                    'DGReport.Items(intlpcounter).Cells(16).Controls.Clear()

                                    'Added By Vaibhav Soni
                                    'Date :- 2014/04/10
                                    DGReport.Items(intlpcounter).Cells(20).Enabled = False
                                    'DGReport.Items(intlpcounter).Cells(20).Controls.Clear()
                                    'Ended By Vaibhav Soni
                                    Exit While
                                End If
                            End While
                            rdr.Close()

                        Next

                        'appearedCount = appearedCount + 1
                        For m As Integer = 0 To dttemp.Rows.Count - 1
                            If dttemp.Rows(m).Item(8).ToString = "2" Then
                                appearedCount = appearedCount + 1
                            End If
                            If dttemp.Rows(m).Item(8).ToString = "1" Or dttemp.Rows(m).Item(8).ToString = "0" Then
                                assignedCount = assignedCount + 1
                            End If
                            If dttemp.Rows(m).Item(8).ToString = "1" Then
                                assigned_runningCount = assigned_runningCount + 1
                            End If

                        Next

                        If assignedCount = dttemp.Rows.Count Then
                            DGReport.Columns(16).Visible = False
                            If assigned_runningCount = assignedCount Then
                                '      DGReport.Columns(9).Visible = False
                                DGReport.Columns(10).Visible = False
                            End If
                        End If

                        If appearedCount = dttemp.Rows.Count Then
                            DGReport.Columns(9).Visible = False
                            DGReport.Columns(10).Visible = False
                            '  DGReport.Columns(15).Visible = True
                        End If

                        If appearedCount + assigned_runningCount = dttemp.Rows.Count Then

                            DGReport.Columns(10).Visible = False
                        End If


                        Dim asgn As Integer = 0
                        Dim asgnf As Integer = 0
                        Dim app As Integer = 0
                        For Each rowItem As DataGridItem In DGReport.Items
                            If rowItem.Cells(18).Text = "1" Then
                                asgn = asgn + 1
                            End If

                            If rowItem.Cells(18).Text = "0" Then
                                asgn = asgn + 1

                            End If

                            If rowItem.Cells(18).Text = "2" Then
                                app = app + 1

                            End If

                        Next
                        If app = DGReport.Items.Count Then
                            DGReport.Columns(9).Visible = False
                            DGReport.Columns(10).Visible = False
                        End If
                        If asgn = DGReport.Items.Count Then
                            DGReport.Columns(16).Visible = False
                        End If
                        If asgnf = DGReport.Items.Count Then
                            DGReport.Columns(16).Visible = False
                        End If

                        appearedCount = 0
                        assignedCount = 0
                        assigned_runningCount = 0

                        'Disable for Demo
                        'imgbtnExport.Visible = True
                        'imgbtnExport.Enabled = True
                        '**************************************************
                        'added by bhumi [8/10/2015]
                        'Reason: Disable Edit,Disable,Remind and Re-assign Link whose Delete_Flg is 1 or [Disable Student]
                        For intlpcounter As Integer = 0 To DGReport.Items.Count - 1
                            Dim intid As Integer = DGReport.Items(intlpcounter).Cells(1).Text
                            strbr = Nothing
                            cmd = Nothing
                            strbr = New StringBuilder
                            strbr.Append("select Delete_flg from M_USER_INFO ")
                            strbr.Append(" where Userid = ")
                            strbr.Append(intid)
                            cmd = New SqlCommand(strbr.ToString, objconn.MyConnection)
                            rdr = cmd.ExecuteReader()
                            While (rdr.Read())
                                Dim Delflag As String = rdr("Delete_flg").ToString
                                If (Delflag = "True") Then
                                    DGReport.Items(intlpcounter).Cells(9).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(9).Text = "--"
                                    DGReport.Items(intlpcounter).Cells(10).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(10).Text = "--"
                                    DGReport.Items(intlpcounter).Cells(19).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(19).Text = "--"
                                    DGReport.Items(intlpcounter).Cells(20).Enabled = False
                                    DGReport.Items(intlpcounter).Cells(20).Text = "--"
                                    Exit While
                                End If
                            End While
                            rdr.Close()
                        Next
                        'Ended by bhumi
                    Else
                        LblMsg.Visible = True
                        DGReport.Visible = False
                        LblMsg.Text = Resources.Resource.Common_NoRecFound
                        gridDiv.Visible = False
                        'Disable for Demo
                        ' imgbtnExport.Visible = False
                    End If
                    '/*****************Jatin Gangajaliya,2011/2/17**********/


                    'Session("DS") = m_DS
                End If

                'Added By  : Pragnesha Kulkarni 
                'Date        : 25/07/18
                'Description:Make visible "Select All" column and Delete button on SuperAdmin login only.
                'Bug ID      : 670
                '--------------------------------------------------------------------------------------
                If MyVisiblePropertyOnCodeBehind = True Then
                    DGReport.Columns(22).Visible = True
                    imgBtnDelete.Visible = True
                Else
                    DGReport.Columns(22).Visible = False
                    imgBtnDelete.Visible = False
                End If
                '-------------------------------------------------------------------------------------

            Catch ex As Exception
                DGReport.CurrentPageIndex = DGReport.CurrentPageIndex - 1
                If DGReport.CurrentPageIndex >= 0 Then
                    DGReport.DataBind()
                End If
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.StackTrace.ToString())
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                intpginx = Nothing
                objconn = Nothing

                query = Nothing
                strbr = Nothing
                ary = Nothing
                conn = Nothing
                adap = Nothing
                '  strPathDb = Nothing
                cmd = Nothing
                rdr = Nothing
                dt = Nothing
                i = Nothing
                aryindex = Nothing

            End Try
        End Sub
#End Region

#Region "CreateDataSet"
        Private Sub CreateDataSet()
            Dim OConn As New ConnectDb
            Dim OAdap As SqlDataAdapter
            Dim StrSql As String
            Dim strbr As New StringBuilder
            Dim blnCheck As Boolean
            Dim dicSearch As New Dictionary(Of String, String)()
            Dim objCommon As CommonFunction
            objCommon = New CommonFunction()
            Try
                '--------------------------Start--------------------------------

                strbr.Append(" select * from(select tcs.userid, ")
                strbr.Append(" (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as username, ")
                strbr.Append(" tcs.course_id,mc.Course_code, ")
                strbr.Append(" tcs.loginname as LoginName, ")
                strbr.Append(" tcs.pwd as 'Password', ")
                strbr.Append(" tcs.written_test_date as writtentestdate, ")
                strbr.Append(" tcs.written_test_appear_date as appearancedate, ")
                strbr.Append(" tcs.appearedflag as appearedflag, ")
                strbr.Append(" mc.course_name,mce.Center_Name, ")
                strbr.Append(" mui.Center_ID, ")
                strbr.Append(" tcs.total_marks, ")
                strbr.Append(" tcs.total_passmarks, ")
                strbr.Append(" isnull(obtained1.obtained_marks,0) as obtained_marks, ")
                strbr.Append(" (case  when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 75 then  'A+' ")
                strbr.Append(" when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >=60 then 'A' ")
                strbr.Append(" when isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 50 then 'B' ")
                strbr.Append(" WHEN (obtained1.obtained_marks*100)/tcs.total_marks is null then null else 'C' End ) as Grad ")
                strbr.Append(" ,(case WHEN obtained1.obtained_marks >= tcs.total_passmarks then 'Pass' ")
                strbr.Append(" WHEN obtained1.obtained_marks is null then null ")
                strbr.Append(" ELSE 'Fail' ")
                strbr.Append(" END) as Status,mmc.Main_Course_ID ")
                '****************Monal Shah*******************
                strbr.Append(" ,sti.end_time ,mc.Total_Time As TotalTime")
                '*****************End**************************
                strbr.Append(" FROM T_Candidate_Status as tcs ")
                strbr.Append(" left join M_USER_INFO as mui ")
                strbr.Append(" on tcs.userid=mui.userid ")
                strbr.Append(" left join m_course as mc ")
                strbr.Append(" on mc.course_id=tcs.course_id ")
                strbr.Append(" Left Join ")
                strbr.Append(" (select sum(temp.obtained_marks) as obtained_marks,temp.course_id,temp.userid from (select ")
                strbr.Append(" ( ")
                strbr.Append(" case ")
                strbr.Append(" WHEN mq.Qn_Category_ID=3 then ")
                strbr.Append(" ( ")
                strbr.Append(" case ")
                strbr.Append(" WHEN mqa.sub_id=tro.sub_id then ")
                strbr.Append(" (Case ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" count(mqa.Correct_Opt_Id)")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" END) ")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" WHEN mq.Qn_Category_ID=2 then ")
                strbr.Append(" ( ")
                strbr.Append(" CASE  ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" count(mqa.Correct_Opt_Id)")
                strbr.Append(" ELSE 0")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" WHEN mq.Qn_Category_ID=1 then ")
                strbr.Append(" ( ")
                strbr.Append(" CASE  ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" SUM(mq.Total_Marks)")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" End ")
                strbr.Append(" ) as obtained_marks ")
                strbr.Append(" ,mc.course_id,mui.userid ")
                strbr.Append(" from m_question as mq ")
                strbr.Append(" left join M_Question_Answer as mqa ")
                strbr.Append(" on mqa.Qn_ID=mq.qnid and mqa.test_type=mq.test_type ")
                strbr.Append(" left join t_result as tr ")
                strbr.Append(" on tr.qno=mq.qnid ")
                strbr.Append(" AND tr.test_type=mq.test_type ")
                strbr.Append(" left join m_user_info as mui ")
                strbr.Append(" on mui.userid=tr.userid ")
                strbr.Append(" left join m_course as mc ")
                strbr.Append(" on mc.course_id=tr.course_id ")
                strbr.Append(" left join m_testinfo as mti ")
                strbr.Append(" on mti.test_type=tr.test_type ")
                strbr.Append(" left join t_result_option as tro ")
                strbr.Append(" on tro.result_id=tr.result_id ")
                strbr.Append(" and tr.test_type=mti.test_type ")
                strbr.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
                strbr.Append(" group by mq.total_marks,mc.course_id, ")
                strbr.Append(" mq.total_marks,mq.qnid,mqa.Sub_id, ")
                strbr.Append(" mqa.Correct_Opt_Id, tro.option_id, mq.Qn_Category_ID, tro.sub_id, mui.userid ")
                strbr.Append(" )temp ")
                strbr.Append(" group by temp.course_id,temp.userid ")
                strbr.Append(" )as obtained1 ")
                strbr.Append(" on obtained1.course_id=tcs.course_id ")
                strbr.Append(" and tcs.userid=obtained1.userid ")
                strbr.Append(" left join M_Centers as mce ")
                strbr.Append(" on mce.Center_ID=mui.Center_ID left join M_Main_Course as mmc ")
                strbr.Append(" on mmc.Main_Course_ID=mc.Main_Course_ID ")
                '*****************************Monal Shah********************************
                strbr.Append(" left join student_time_info as sti ")
                strbr.Append(" on tcs.userid=sti.user_id ")
                strbr.Append(" and end_time In (select MAX(end_time) from student_time_info where user_id=tcs.userid and course_id=mc.Course_id)")
                '*******************************end*****************************
                strbr.Append(" )temp ")

                '----------------End----------------------------------

                'strQuery.Append(" order by candstatus.userid ")
                'If ddlResult.SelectedItem.Text = "Pass" Then
                '    dicSearch.Add(" CandStatus.written_test_remark", ddlResult.SelectedItem.Text)
                'ElseIf ddlResult.SelectedItem.Text = "Reject" Then
                '    dicSearch.Add(" (CandStatus.written_test_remark", ddlResult.SelectedItem.Text + "' OR (ResAttempted.Attempted IS NULL AND CandStatus.Written_Test_Appear_Date IS NOT NULL))")
                'End If

                If dblCenter.SelectedItem.Text <> "---- Select ----" Then
                    dicSearch.Add("temp.Center_ID", dblCenter.SelectedValue.ToString())
                    'If dblCourse.SelectedItem.Text <> "---- Select ----" Then
                    '    dicSearch.Add("temp.Course_ID", dblCourse.SelectedValue.ToString())
                    'End If
                End If
                If dblCourse.SelectedItem.Text <> "---- Select ----" Then
                    dicSearch.Add("temp.Course_ID", dblCourse.SelectedValue.ToString())
                End If

                If ddlStatus.SelectedItem.Text = "Appeared" Then
                    dicSearch.Add("temp.Appeared", "")
                ElseIf ddlStatus.SelectedItem.Text = "Assigned" Then
                    dicSearch.Add("temp.Assigned", "")
                    '******************Monal shah**************
                    'ElseIf ddlStatus.SelectedItem.Text = "Ongoing" Then
                    '    dicSearch.Add("temp.Ongoing", "")
                ElseIf ddlStatus.SelectedItem.Text = "LoginUser" Then
                    dicSearch.Add("temp.LoginUser", "")
                ElseIf ddlStatus.SelectedItem.Text = "LinkBreak" Then
                    dicSearch.Add("temp.LinkBreak", "")
                    '********************End*********************
                End If


                If Not TxtUserName.Text.Trim() = "" Then
                    dicSearch.Add("temp.username", TxtUserName.Text.Trim())
                End If

                'If Not TxtLoginname.Text.Trim() = "" Then
                '    dicSearch.Add("UserInfo.loginname", TxtLoginname.Text.Trim())
                'End If

                'If ddlTestName.SelectedItem.Text <> "ALL" Then
                '    dicSearch.Add("m_testinfo.test_type", ddlTestName.SelectedValue.ToString())
                'End If

                If Not TxtFrom.Value.Trim() = "" And Not TxtTo.Value.Trim() = "" Then
                    dicSearch.Add("temp.writtentestdate Between '", ConvertDate(TxtFrom.Value) + "'  and  '" + ConvertDate(TxtTo.Value) + "'")
                ElseIf Not TxtFrom.Value.Trim() = "" Then
                    dicSearch.Add("temp.writtentestdate", ConvertDate(TxtFrom.Value))
                End If

                Select Case ddlgrade.SelectedIndex
                    Case 0

                    Case 1
                        dicSearch.Add("grad = ", "A+")
                    Case 2
                        dicSearch.Add("grad = ", "A")
                    Case 3
                        dicSearch.Add("grad = ", "B")
                    Case 4
                        dicSearch.Add("grad = ", "C")
                End Select


                If Not txtAppFromDate.Value.Trim() = "" And Not TxtAppToDate.Value.Trim() = "" Then
                    dicSearch.Add("temp.appearancedate Between '", ConvertDate(txtAppFromDate.Value) + "'  and  '" + ConvertDate(TxtAppToDate.Value) + " 23:59:59'")
                ElseIf Not txtAppFromDate.Value.Trim() = "" Then
                    dicSearch.Add("temp.appearancedate", ConvertDate(txtAppFromDate.Value))
                End If

                'If Not TxtScoreFrom.Text.Trim() = "" And Not TxtScoreTo.Text.Trim() = "" Then
                '    dicSearch.Add("ResScore.Score Between '", TxtScoreFrom.Text + "'  and  '" + TxtScoreTo.Text + "'")
                'ElseIf Not TxtScoreFrom.Text.Trim() = "" Then
                '    dicSearch.Add("ResScore.Score", TxtScoreFrom.Text)
                'End If

                strbr.Append(objCommon.GetSelectSearchQuery(dicSearch))
                'added by bhumi [28/8/2015]
                'Reason: All the Courses were displaying although course is not in center or class but Now only selected center's courses will be display
                If Not dblCenter.SelectedIndex = 0 Then
                    strbr.Append(" and course_id in (SELECT Course_ID From T_Center_Course Where Center_ID=")
                    strbr.Append(+dblCenter.SelectedValue.ToString())
                    strbr.Append(")")
                End If
                'Ended by bhumi
                strbr.Append(" order by temp.username ")
                Dim strq = strbr.ToString

                m_DS = New DataSet



                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If OConn.connect() Then
                    OAdap = New SqlDataAdapter(strq, OConn.MyConnection)
                    OAdap.Fill(m_DS, "TblResults")
                    Session("DS") = m_DS
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                OConn.disconnect()
                Throw ex
            Finally
                OConn.disconnect()
                OConn = Nothing
                StrSql = Nothing
                OAdap = Nothing
                strbr = Nothing

            End Try
        End Sub
#End Region

#Region "DgReport_PageIndexChanged"
        Protected Sub DgReport_PageIndexChanged(ByVal Obj As Object, ByVal E As DataGridPageChangedEventArgs) Handles DGReport.PageIndexChanged
            Try
                DGReport.CurrentPageIndex = E.NewPageIndex
                BindGrid()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "DeleteCandidateInfo"
        Private Sub DeleteCandidateInfo(ByVal UserID As Integer, ByVal Course_ID As String) ' changed by pragnesha --ByVal Test_type As String)
            Dim OConn As New ConnectDb
            Dim OCmd As SqlCommand
            Dim StrDel1, StrDel2, StrDel3 As String

            Try
                'Query modify by bhasker(26-11-09)
                '********************** Start *******************
                'StrDel1 = "Delete from t_candidate_status where userid='" & UserID & "'  And Test_type = '" & Test_type & "'"
                'StrDel2 = "Delete from t_result where userid='" & UserID & "'   And Test_type = '" & Test_type & "'"
                '********************** End *******************
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                'Query modify by: Pragnesha Kulkarni 
                'Date         : 25/07/18
                'Description : Delete records from table.
                'Bug ID       : 670
                '---------------------------------------------------------------------------------------------------------
                'Delete record from T_Result_Option
                StrDel1 = "Delete from T_Result_Option where result_id in (select result_id from T_Result where userid='" & UserID & "'   And Course_ID = '" & Course_ID & "')"
                'Delete record from T_Result
                StrDel2 = "Delete from t_result where userid='" & UserID & "'   And Course_ID = '" & Course_ID & "'"
                'Delete record from T_Candidate_Status
                StrDel3 = "Delete from t_candidate_status where userid='" & UserID & "'  And Course_ID = '" & Course_ID & "'"
                '---------------------------------------------------------------------------------------------------------

                If True = OConn.connect() Then
                    OCmd = New SqlCommand(StrDel1, OConn.MyConnection)
                    OCmd.ExecuteNonQuery()
                    OCmd = New SqlCommand(StrDel2, OConn.MyConnection)
                    OCmd.ExecuteNonQuery()
                    '---------------------Added by pragnesha----------------
                    OCmd = New SqlCommand(StrDel3, OConn.MyConnection)
                    OCmd.ExecuteNonQuery()
                    '--------------------------------------------------------
                    If OConn IsNot Nothing Then
                        OConn.disconnect()
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                If OConn IsNot Nothing Then
                    OConn.disconnect()
                End If
                Throw ex
            Finally
                If OConn IsNot Nothing Then
                    OConn.disconnect()
                End If
                OCmd = Nothing
                OConn = Nothing
                StrDel1 = Nothing
                StrDel2 = Nothing

            End Try
        End Sub
#End Region

        '#Region "btnBack_Click"

        '        Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnBack.Click
        '            Try
        '                Response.Redirect("admin.aspx", False)
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Response.Redirect("error.aspx", False)
        '            End Try
        '        End Sub
        '#End Region

        'Disable for Demo
        '#Region "btnPrint_Click"

        '        Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnPrint.Click
        '            Try
        '                Response.Redirect("RptPrint.aspx", False)
        '                'If DGReport.Visible = True Then
        '                '    Session.Add("gridvisible", "true")
        '                'Else
        '                '    Session.Add("gridvisible", "false")
        '                'End If
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Response.Redirect("error.aspx", False)
        '            End Try
        '        End Sub
        '#End Region

#Region "btnDelete_Click"

        'Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        '    Dim ChkBox As System.Web.UI.WebControls.CheckBox
        '    Dim CheckedCandidate As Integer
        '    Dim DgItem As DataGridItem
        '    Try

        '        m_CntDel = 0

        '        For Each DgItem In DGReport.Items
        '            ChkBox = CType(DgItem.Cells(0).FindControl("ChkSel"), System.Web.UI.WebControls.CheckBox)
        '            If (True = ChkBox.Checked) Then
        '                CheckedCandidate = Convert.ToInt32(DgItem.Cells(1).Text)
        '                DeleteCandidateInfo(CheckedCandidate, DgItem.Cells(2).Text)
        '                m_CntDel += 1
        '            End If
        '        Next
        '        If m_CntDel > 0 Then
        '            LblMsg.ForeColor = System.Drawing.Color.FromName("Green")
        '            LblMsg.Text = "  " & m_CntDel & " Test Record Deleted Successfully."
        '            LblMsg.Visible = True
        '            BindGrid()
        '        End If
        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        Response.Redirect("error.aspx", False)
        '    Finally
        '        ChkBox = Nothing
        '        CheckedCandidate = Nothing
        '        DgItem = Nothing

        '    End Try

        'End Sub
#End Region

#Region "SetFocus"

        Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)
            Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ID & "').focus() </SCRIPT>"
            RegisterStartupScript("focus", s)
        End Sub
#End Region

#Region "RecCont"

        Private Function RecCont() As Integer
            Dim objconn As New ConnectDb
            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim strPathDb As String
            Dim intcnt As Integer
            Dim strSql As String
            Try

                If Not TxtTo.Value.Trim() = "" Then
                    strSql = "Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as testtype,m_testinfo.test_name as TestName, " +
                                        "t_candidate_status.written_test_date as WrittenTestDate, a_result.attend as attempted, t_candidate_status.written_test_marks as Score,Count (*) as TotalMark," +
                                        "t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result" +
                                        " from m_user_info, m_testinfo, t_candidate_status, t_result, " +
                                        " ( Select t_candidate_status.userid, t_candidate_status.test_type, count(*) as attend from t_result, t_candidate_status " +
                                        " Where t_result.optionid > 0 and t_candidate_status.userid = t_result.userid and t_candidate_status.test_type = t_result.test_Type " +
                                        " Group By t_candidate_status.userid, t_candidate_status.test_type ) a_result" +
                                        " where a_result.test_type = t_candidate_status.test_type and a_result.userid = t_candidate_status.userid and " +
                                        " t_candidate_status.test_Type=t_result.test_type and t_candidate_status.userid = t_result.userid" +
                                        " and t_candidate_status.test_type = m_testinfo.test_type and m_user_info.userid=t_candidate_status.userid " +
                                        " and t_candidate_status.written_test_date Between ' " + TxtFrom.Value + "'  and  '" + TxtTo.Value + "'  " +
                                        " Group by m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname, m_testinfo.test_type, m_testinfo.test_name, t_candidate_status.written_test_date," +
                                        " t_candidate_status.written_test_marks,t_candidate_status.written_test_appear_date, t_candidate_status.written_test_remark," +
                                        " a_result.attend Union Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as test_type, m_testinfo.test_name as TestName, " +
                                        " t_candidate_status.written_test_date as WrittenTestDate, ' ' as attempted, ' ' as Score,' ' as TotalMark, " +
                                        " t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result " +
                                        "  from m_user_info, m_testinfo, t_candidate_status where t_candidate_status.test_type = m_testinfo.test_type and " +
                                        " m_user_info.userid=t_candidate_status.userid and t_candidate_status.written_test_date Between  ' " + TxtFrom.Value + "'  and  '" + TxtTo.Value + "' and t_candidate_status.written_test_appear_date is Null"
                    '+ _                                " Order by 5 desc, 8 desc"
                    '" + _    " Order by 5 desc, 8 desc"

                    Dim x As String = " + TxtFrom.Text + "
                Else
                    strSql = "Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as testtype,m_testinfo.test_name as TestName, " +
                                "t_candidate_status.written_test_date as WrittenTestDate, a_result.attend as attempted, t_candidate_status.written_test_marks + '-' + count(*)  as Score," +
                                "t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result" +
                                " from m_user_info, m_testinfo, t_candidate_status, t_result, " +
                                " ( Select t_candidate_status.userid, t_candidate_status.test_type, count(*) as attend from t_result, t_candidate_status " +
                                " Where t_result.optionid > 0 and t_candidate_status.userid = t_result.userid and t_candidate_status.test_type = t_result.test_Type " +
                                " Group By t_candidate_status.userid, t_candidate_status.test_type ) a_result" +
                                " where a_result.test_type = t_candidate_status.test_type and a_result.userid = t_candidate_status.userid and " +
                                " t_candidate_status.test_Type=t_result.test_type and t_candidate_status.userid = t_result.userid" +
                                " and t_candidate_status.test_type = m_testinfo.test_type and m_user_info.userid=t_candidate_status.userid " +
                                " and t_candidate_status.written_test_date =  " + TxtFrom.Value + " " +
                                " Group by m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname, m_testinfo.test_type, m_testinfo.test_name, t_candidate_status.written_test_date," +
                                " t_candidate_status.written_test_marks,t_candidate_status.written_test_appear_date, t_candidate_status.written_test_remark," +
                                " a_result.attend Union Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as test_type, m_testinfo.test_name as TestName, " +
                                " t_candidate_status.written_test_date as WrittenTestDate, ' ' as attempted, ' ' as Score, " +
                                " t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result " +
                                "  from m_user_info, m_testinfo, t_candidate_status where t_candidate_status.test_type = m_testinfo.test_type and " +
                                " m_user_info.userid=t_candidate_status.userid and t_candidate_status.written_test_date Between  '" + TxtFrom.Value + "' " + " and t_candidate_status.written_test_appear_date is Null"

                    '" + _ ' " Order by 5 desc, 8 desc"

                End If

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
                    If objconn IsNot Nothing Then
                        objconn.disconnect()
                    End If
                End If
                RecCont = intcnt
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
                strPathDb = Nothing
                strSql = Nothing
                intcnt = Nothing
                myCommand = Nothing
                myDataReader = Nothing
            End Try
        End Function
#End Region

#Region "BtnSearch_Click"

        Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
            Try
                Dim dt1 As Date
                Dim dt2 As Date
                Const REGULAR_EXP = "(^[0-9]{4,4}/?[0-1][0-9]/?[0-3][0-9]$)"
                Dim flag As Boolean
                flag = True

                'If TxtFrom.Text <> "" Then
                '    If Not Regex.IsMatch(TxtFrom.Text, REGULAR_EXP) Then
                '        MsgBox("Please enter Exam Assigned From date in YYYY/MM/DD format.", MsgBoxStyle.Exclamation)
                '        TxtFrom.Focus()
                '        flag = False
                '        If Not IsDate(TxtFrom.Text) And flag Then
                '            MsgBox("Please enter valid Exam Assigned From date.", MsgBoxStyle.Exclamation)
                '            TxtFrom.Focus()
                '            flag = False
                '        End If
                '    End If
                'End If
                'If TxtTo.Text <> "" And flag Then
                '    If Not Regex.IsMatch(TxtTo.Text, REGULAR_EXP) Then
                '        MsgBox("Please enter Exam Assigned To date in YYYY/MM/DD format.", MsgBoxStyle.Exclamation)
                '        TxtTo.Focus()
                '        flag = False
                '        If Not IsDate(TxtTo.Text) And flag Then
                '            MsgBox("Please enter valid Exam Assigned To date.", MsgBoxStyle.Exclamation)
                '            TxtTo.Focus()
                '            flag = False
                '        End If
                '    End If
                'End If
                If txtAppFromDate.Value <> "" And flag Then
                    If Not Regex.IsMatch(ConvertDate(txtAppFromDate.Value), REGULAR_EXP) Then
                        '   MsgBox("Please enter Exam Appeared From date in YYYY/MM/DD format.", MsgBoxStyle.Exclamation)
                        LblMsg.Text = Resources.Resource.CandStatus_dateerr
                        LblMsg.ForeColor = Color.Red
                        LblMsg.Visible = True
                        txtAppFromDate.Focus()
                        flag = False
                        If Not IsDate(ConvertDate(txtAppFromDate.Value)) And flag Then
                            '  MsgBox("Please enter valid Exam Appeared From date.", MsgBoxStyle.Exclamation)
                            LblMsg.Text = Resources.Resource.CandStatus_vlddateerr
                            LblMsg.ForeColor = Color.Red
                            LblMsg.Visible = True
                            txtAppFromDate.Focus()
                            flag = False
                        End If
                    End If
                End If
                If TxtAppToDate.Value <> "" And flag Then
                    If Not Regex.IsMatch(ConvertDate(TxtAppToDate.Value), REGULAR_EXP) Then
                        ' MsgBox("Please enter Exam Appeared To date in YYYY/MM/DD format.", MsgBoxStyle.Exclamation)
                        LblMsg.Text = Resources.Resource.CandStatus_dateerr
                        LblMsg.ForeColor = Color.Red
                        LblMsg.Visible = True
                        TxtAppToDate.Focus()
                        flag = False
                        If Not IsDate(ConvertDate(TxtAppToDate.Value)) Then
                            '  MsgBox("Please enter valid Exam Appeared To date.", MsgBoxStyle.Exclamation)
                            LblMsg.Text = Resources.Resource.CandStatus_vlddateerr
                            LblMsg.ForeColor = Color.Red
                            LblMsg.Visible = True
                            TxtAppToDate.Focus()
                            flag = False
                        End If
                    End If
                End If

                If TxtTo.Value <> "" And flag Then
                    If TxtFrom.Value = "" Then
                        ' MsgBox("Please enter Exam Assigned From date.", MsgBoxStyle.Exclamation)
                        LblMsg.Text = Resources.Resource.CandStatus_Examasdat
                        LblMsg.ForeColor = Color.Red
                        LblMsg.Visible = True
                        TxtFrom.Focus()
                        flag = False
                    End If
                End If

                If (TxtFrom.Value <> "" And IsDate(ConvertDate(TxtFrom.Value))) And (TxtTo.Value <> "" And IsDate(ConvertDate(TxtTo.Value))) And flag Then
                    dt1 = Convert.ToDateTime(ConvertDate(TxtTo.Value))
                    dt2 = Convert.ToDateTime(ConvertDate(TxtFrom.Value))

                    If dt1 < dt2 Then
                        '  MsgBox("Please enter Exam Assigned To date greater then Exam Assigned From date.", MsgBoxStyle.Exclamation)
                        LblMsg.Text = Resources.Resource.CandStatus_Examdategreater
                        LblMsg.ForeColor = Color.Red
                        LblMsg.Visible = True
                        TxtTo.Focus()
                        flag = False
                    End If
                End If
                'If txtAppFromDate.Text <> "" Then
                '    If Not Regex.IsMatch(txtAppFromDate.Text, REGULAR_EXP) Or Not IsDate(txtAppFromDate.Text) Then
                '        MsgBox("Please, Enter Valid date.", MsgBoxStyle.Exclamation)
                '        txtAppFromDate.Focus()
                '        flag = False
                '    End If
                'End If
                'If TxtAppToDate.Text <> "" Then
                '    If Not Regex.IsMatch(TxtAppToDate.Text, REGULAR_EXP) Or Not IsDate(TxtAppToDate.Text) Then
                '        MsgBox("Please, Enter Valid date.", MsgBoxStyle.Exclamation)
                '        TxtAppToDate.Focus()
                '        flag = False
                '    End If
                'End If
                If TxtAppToDate.Value <> "" And flag Then
                    If txtAppFromDate.Value = "" Then
                        '  MsgBox("Please enter Exam Appeared From date.", MsgBoxStyle.Exclamation)
                        LblMsg.Text = Resources.Resource.CandStatus_Examappdate
                        LblMsg.ForeColor = Color.Red
                        LblMsg.Visible = True
                        txtAppFromDate.Focus()
                        flag = False
                    End If
                End If
                If (txtAppFromDate.Value <> "" And IsDate(ConvertDate(txtAppFromDate.Value))) And (TxtAppToDate.Value <> "" And IsDate(ConvertDate(TxtAppToDate.Value))) And flag Then
                    dt1 = Convert.ToDateTime(ConvertDate(TxtAppToDate.Value))
                    dt2 = Convert.ToDateTime(ConvertDate(txtAppFromDate.Value))

                    If dt1 < dt2 Then
                        ' MsgBox("Please enter Exam Appeared To date greater then Exam Appeared From date.", MsgBoxStyle.Exclamation)
                        LblMsg.Text = Resources.Resource.CandStatus_Examapgrtrexm
                        LblMsg.ForeColor = Color.Red
                        LblMsg.Visible = True
                        TxtAppToDate.Focus()
                        flag = False
                    End If
                End If

                'btnPrint.Visible = True
                DGReport.CurrentPageIndex = 0
                If flag Then
                    BindGrid()
                    'imgbtnExport.Enabled = True
                    'imgbtnExport.Visible = True
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "imgLogoff_Click"


#End Region

        '***********Nisha 2017/09/25************
#Region "CheckImage Function"

        Dim _CourseID As String = String.Empty
        Dim _CenterID As String = String.Empty
        Dim _CandidateID As String = String.Empty
        Dim objHashTable As New Hashtable

        Public Function CheckImage(ByVal value As String) As String()
            Dim str As String
            Dim strVal(1) As String
            Try
                If value.Contains("<br/>") Then
                    str = value.Substring((value.IndexOf("=") + 1),
                                          ((value.IndexOf(" ", (value.IndexOf("=") + 1))) - (value.IndexOf("=") + 1)))
                    strVal(0) = str
                    strVal(1) = value.Substring(0, value.IndexOf("<"))
                    Return strVal
                ElseIf value.Contains("<img") Then
                    str = value.Substring((value.IndexOf("=") + 1),
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


        '***********Nisha 2017/09/25*************
        Public Sub ExportExamDetails(_CourseID As String, _CandidateID As String, _CenterID As String)
            Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
            Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
            Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
            Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
            Dim objOpt As Object = System.Reflection.Missing.Value
            Dim objCn As New ConnectDb
            Dim Sheet As Excel.Worksheet
            Dim sheet1 As Excel.Worksheet
            Dim fileName1 As Object
            Dim strpath, strCorrectOpt, strGivenAns As String
            Dim strDateTime(2) As String
            Dim dsDateTime, dsQuestion, dsOpt, dsCrrctOpt, dsGivenOpt, dsTotalMarks As DataSet
            Dim da As SqlDataAdapter
            Dim sb As StringBuilder
            Dim rows As Integer = 6
            Dim start As Integer = 6
            Dim ends As Integer = 6
            Dim TotalMarks, MarkObtain As Integer
            Dim ImgNotFound(1) As String
            Dim pic As Object
            Dim arrCrrAns(), arrGivenAns() As String
            Try

                'Constants
                Const xlEdgeLeft = 7
                Const xlEdgeTop = 8
                Const xlEdgeBottom = 9
                Const xlEdgeRight = 10
                'Create  Spreadsheet
                Dim datestart As Date = Date.Now 'Added by Pragnesha Kulkarni on 2018/06/01 for stop Excel process
                App = New Excel.Application
                ' App = CreateObject("Excel.Application")
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim myWorkBook As Excel.Workbook = App.Workbooks.Open(Server.MapPath("Excel Import\Student Exam Details.xlt"), 0, True, 5, "", "", False, Excel.XlPlatform.xlWindows, "", True, False, 0, True)
                'WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
                sheet1 = myWorkBook.Worksheets("Exam Details")
                sheet1.Activate()
                ''Batch Name
                With App.ActiveSheet.Range("D2:D2")
                    .MergeCells = True
                    '.Interior.ColorIndex = 40
                    .Font.Bold = True
                    '.Font.ColorIndex = 53
                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    '.Cells.Value = sel_subjectid.SelectedItem.Text
                    .Cells.Value = Convert.ToString(objHashTable("Centrename"))
                End With



                ' ''Course Name
                With App.ActiveSheet.Range("D3:D3")
                    .MergeCells = True
                    '.Interior.ColorIndex = 40
                    .Font.Bold = True
                    '.Font.ColorIndex = 53
                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    '.Cells.Value = ddlcourse.SelectedItem.Text
                    .Cells.Value = Convert.ToString(objHashTable("Coursename"))

                End With



                'strpath = ConfigurationSettings.AppSettings("PathDb")

                If objCn.connect() Then

                    'Get date & time and total marks from database
                    sb = New StringBuilder
                    sb.Append("select T_Candidate_Status.Written_test_Appear_Date,M_Course.Total_marks  ")
                    sb.Append("from T_Candidate_Status inner join M_Course on M_Course.Course_id=T_Candidate_Status.Course_ID")
                    sb.Append(" where T_Candidate_Status.Course_ID=")
                    sb.Append(_CourseID)
                    If (_CandidateID = 0) Then
                    Else
                        sb.Append("and T_Candidate_Status.UserId=")
                        sb.Append(_CandidateID)
                    End If

                    dsDateTime = New DataSet()

                    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
                    da.Fill(dsDateTime)
                    If dsDateTime.Tables(0).Rows.Count <> 0 Then
                        Dim dateTime As String = dsDateTime.Tables(0).Rows(0).Item(0).ToString
                        strDateTime = dateTime.Split("/")
                    End If


                    sb = Nothing
                    da = Nothing
                    sb = New StringBuilder
                    '********************************************************

                    sb.Append("SELECT TEMP.qno, " & vbCrLf)
                    sb.Append("       Sum(TEMP.obtained_marks)AS obtained_marks, " & vbCrLf)
                    sb.Append("       TEMP.course_id, " & vbCrLf)
                    sb.Append("       TEMP.userid, " & vbCrLf)
                    sb.Append("       TEMP.question, " & vbCrLf)
                    sb.Append("       TEMP.total_marks, " & vbCrLf)
                    sb.Append("       TEMP.test_type, " & vbCrLf)
                    sb.Append("       TEMP.test_name          AS subjectname, " & vbCrLf)
                    sb.Append("       TEMP.Names " & vbCrLf)
                    sb.Append("FROM   (SELECT tr.qno, " & vbCrLf)
                    sb.Append("               ( CASE " & vbCrLf)
                    sb.Append("                   WHEN mq.qn_category_id = 3 THEN ( " & vbCrLf)
                    sb.Append("                   CASE " & vbCrLf)
                    sb.Append("                     WHEN mqa.sub_id = tro.sub_id THEN ( CASE " & vbCrLf)
                    sb.Append("                     WHEN " & vbCrLf)
                    sb.Append("                     tro.option_id = mqa.correct_opt_id " & vbCrLf)
                    sb.Append("                                                         THEN " & vbCrLf)
                    sb.Append("                     Count(mqa.correct_opt_id) " & vbCrLf)
                    sb.Append("                     ELSE " & vbCrLf)
                    sb.Append("                     0 " & vbCrLf)
                    sb.Append("                                                         END " & vbCrLf)
                    sb.Append("                     ) " & vbCrLf)
                    sb.Append("                     ELSE 0 " & vbCrLf)
                    sb.Append("                   END ) " & vbCrLf)
                    sb.Append("                   WHEN mq.qn_category_id = 2 THEN ( CASE " & vbCrLf)
                    sb.Append("                                                       WHEN " & vbCrLf)
                    sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
                    sb.Append("                                                     THEN " & vbCrLf)
                    sb.Append("                   Count(mqa.correct_opt_id) " & vbCrLf)
                    sb.Append("                                                       ELSE 0 " & vbCrLf)
                    sb.Append("                                                     END ) " & vbCrLf)
                    sb.Append("                   WHEN mq.qn_category_id = 1 THEN ( CASE " & vbCrLf)
                    sb.Append("                                                       WHEN " & vbCrLf)
                    sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
                    sb.Append("                                                     THEN Sum(mq.total_marks) " & vbCrLf)
                    sb.Append("                                                       ELSE 0 " & vbCrLf)
                    sb.Append("                                                     END ) " & vbCrLf)
                    sb.Append("                 END )                    AS obtained_marks, " & vbCrLf)
                    sb.Append("               mc.course_id, " & vbCrLf)
                    sb.Append("               mui.userid, " & vbCrLf)
                    sb.Append("               Isnull(mui.surname, '') + ' ' + Isnull(mui.Name, '') + ' ' + " & vbCrLf)
                    sb.Append("               Isnull(mui.Middlename, '') AS Names, " & vbCrLf)
                    sb.Append("               mq.question, " & vbCrLf)
                    sb.Append("               mq.total_marks, " & vbCrLf)
                    sb.Append("               mq.test_type, " & vbCrLf)
                    sb.Append("               mti.test_name, " & vbCrLf)
                    sb.Append("               mui.Center_id, " & vbCrLf)
                    sb.Append("               mui.Delete_Flg " & vbCrLf)
                    sb.Append("        FROM   m_question AS mq " & vbCrLf)
                    sb.Append("               LEFT JOIN m_question_answer AS mqa " & vbCrLf)
                    sb.Append("                 ON mqa.qn_id = mq.qnid " & vbCrLf)
                    sb.Append("                    AND mqa.test_type = mq.test_type " & vbCrLf)
                    sb.Append("               LEFT JOIN t_result AS tr " & vbCrLf)
                    sb.Append("                 ON tr.qno = mq.qnid " & vbCrLf)
                    sb.Append("                    AND tr.qno = mqa.qn_id " & vbCrLf)
                    sb.Append("                    AND tr.test_type = mq.test_type " & vbCrLf)
                    sb.Append("               LEFT JOIN m_user_info AS mui " & vbCrLf)
                    sb.Append("                 ON mui.userid = tr.userid " & vbCrLf)
                    sb.Append("               LEFT JOIN M_Centers AS msc " & vbCrLf)
                    sb.Append("                 ON msc.Center_id = mui.Center_ID " & vbCrLf)
                    sb.Append("               LEFT JOIN m_course AS mc " & vbCrLf)
                    sb.Append("                 ON mc.course_id = tr.course_id " & vbCrLf)
                    sb.Append("               LEFT JOIN T_Center_Course AS tcc " & vbCrLf)
                    sb.Append("                 ON tcc.course_id = mc.course_id " & vbCrLf)
                    sb.Append("                    AND tcc.Center_ID = msc.Center_id " & vbCrLf)
                    sb.Append("               LEFT JOIN m_testinfo AS mti " & vbCrLf)
                    sb.Append("                 ON mti.test_type = tr.test_type " & vbCrLf)
                    sb.Append("                    AND mti.test_type = mq.test_type " & vbCrLf)
                    sb.Append("               LEFT JOIN dbo.T_User_Course AS tuc " & vbCrLf)
                    sb.Append("                 ON tuc.course_id = mc.course_id " & vbCrLf)
                    sb.Append("                    AND tuc.user_id = mui.userid " & vbCrLf)
                    sb.Append("                    AND tuc.Test_type = mti.test_type " & vbCrLf)
                    sb.Append("               LEFT JOIN t_result_option AS tro " & vbCrLf)
                    sb.Append("                 ON tro.result_id = tr.result_id " & vbCrLf)
                    sb.Append("                    AND tr.test_type = mti.test_type " & vbCrLf)
                    sb.Append("                    AND tro.option_id = mqa.correct_opt_id " & vbCrLf)
                    sb.Append("        GROUP  BY mq.total_marks, " & vbCrLf)
                    sb.Append("                  mc.course_id, " & vbCrLf)
                    sb.Append("                  mq.test_type, " & vbCrLf)
                    sb.Append("                  mti.test_name, " & vbCrLf)
                    sb.Append("                  mq.total_marks, " & vbCrLf)
                    sb.Append("                  mqa.sub_id, " & vbCrLf)
                    sb.Append("                  tr.qno, " & vbCrLf)
                    sb.Append("                  mq.question, " & vbCrLf)
                    sb.Append("                  mq.total_marks, " & vbCrLf)
                    sb.Append("                  mqa.correct_opt_id, " & vbCrLf)
                    sb.Append("                  tro.option_id, " & vbCrLf)
                    sb.Append("                  mq.qn_category_id, " & vbCrLf)
                    sb.Append("                  tro.sub_id, " & vbCrLf)
                    sb.Append("                  mui.userid, " & vbCrLf)
                    sb.Append("                  Isnull(mui.surname, '') + ' ' + Isnull(mui.Name, '') + ' ' + " & vbCrLf)
                    sb.Append("                  Isnull(mui.Middlename, ''), " & vbCrLf)
                    sb.Append("                  mui.Center_id, " & vbCrLf)
                    sb.Append("                  mui.Delete_Flg)TEMP " & vbCrLf)
                    sb.Append("--left join M_Options as mop   " & vbCrLf)
                    sb.Append("--                    on mop.qnid=temp.qno and mop.test_type=temp.test_type " & vbCrLf)
                    sb.Append("WHERE Temp.Center_ID=" & _CenterID & " and  TEMP.course_id = " & vbCrLf)
                    sb.Append(_CourseID & vbCrLf)
                    If (_CandidateID = 0) Then
                    Else
                        sb.Append("       AND TEMP.userid =  " & vbCrLf)
                        sb.Append(_CandidateID)
                    End If
                    sb.Append("       AND TEMP.Delete_Flg = 0 " & vbCrLf)
                    sb.Append("GROUP  BY TEMP.qno, " & vbCrLf)
                    sb.Append("          TEMP.course_id, " & vbCrLf)
                    sb.Append("          TEMP.userid, " & vbCrLf)
                    sb.Append("          TEMP.Names, " & vbCrLf)
                    sb.Append("          TEMP.question, " & vbCrLf)
                    sb.Append("          TEMP.total_marks, " & vbCrLf)
                    sb.Append("          TEMP.test_type, " & vbCrLf)
                    sb.Append("          TEMP.test_name, " & vbCrLf)
                    sb.Append("          TEMP.Delete_Flg -- order by temp.qno   " & vbCrLf)
                    sb.Append("ORDER  BY TEMP.Names, " & vbCrLf)
                    sb.Append("          TEMP.test_name")


                    Dim st As String = sb.ToString
                    dsQuestion = New DataSet
                    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
                    da.Fill(dsQuestion)
                    '  If dsQuestion.Tables(0).Rows.Count <> 0 Then



                    For i As Integer = 0 To dsQuestion.Tables(0).Rows.Count - 1


                        sb = Nothing
                        da = Nothing
                        sb = New StringBuilder
                        dsOpt = New DataSet
                        'get options of the question
                        sb.Append(" Select mop.Optionid,mop.[Option],mop.test_type,mop.qnid,mq.question ,mq.Qn_Category_ID ")
                        sb.Append(" from M_Options as mop ")
                        sb.Append(" LEFT join m_question as mq ")
                        sb.Append(" on mq.qnid=mop.qnid ")
                        sb.Append(" and mq.test_type=mop.test_type ")
                        sb.Append(" where mop.Test_Type= " & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " and mop.Qnid=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " order by mop.Optionid ")
                        da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
                        da.Fill(dsOpt)
                        For opt As Integer = 0 To dsOpt.Tables(0).Rows.Count - 1
                            ImgNotFound = CheckImage(dsOpt.Tables(0).Rows(opt).Item(1).ToString)
                            If ImgNotFound(0) = "" Then
                                App.ActiveSheet.Cells(rows, 6).Value = dsOpt.Tables(0).Rows(opt).Item(1).ToString
                                App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                'App.ActiveSheet.Cells(11+i, 5+opt).ColumnWidth = 30
                                App.ActiveSheet.Cells(rows, 6).WrapText = True
                                App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
                                'App.ActiveSheet.Cells(11+i, 5+opt).RowHeight = 15

                            Else
                                If System.IO.File.Exists(Server.MapPath(ImgNotFound(0))) Then
                                    App.ActiveSheet.Cells(rows, 6).Value = ImgNotFound(1)
                                    Dim Align As Integer = 0
                                    If ImgNotFound(1) <> "" Then
                                        Align = 12
                                    End If
                                    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(ImgNotFound(0)))
                                    With pic
                                        .top = App.ActiveSheet.Cells(rows, 6).top + 12
                                        .left = App.ActiveSheet.Cells(rows, 6).left + 15
                                        .width = 70
                                        .height = 70
                                    End With

                                    App.ActiveSheet.Cells(rows, 6).RowHeight = 83
                                    App.ActiveSheet.Cells(rows, 6).ColumnWidth = 15
                                    App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
                                    App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignJustify
                                    App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                Else
                                    App.ActiveSheet.Cells(rows, 6).Value = "Image not Found"
                                    App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
                                    App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                    App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                End If

                            End If
                            rows += 1

                        Next
                        ends = rows - 1

                        sb = Nothing
                        da = Nothing
                        sb = New StringBuilder
                        dsCrrctOpt = New DataSet
                        'get correct option of the question
                        sb.Append(" Select M_Question_Answer.Qn_ID,M_Question_Answer.Correct_Opt_Id as correctid,M_Question_Answer.Test_Type ,M_Options.[Option] as givenAnswer ,M_Question_Answer.sub_id ")
                        sb.Append(" from M_Question_Answer  ")
                        sb.Append(" inner join M_Options ")
                        sb.Append(" on M_Options.Optionid=M_Question_Answer.Correct_Opt_Id ")
                        sb.Append(" and M_Options.test_type=M_Question_Answer.test_type ")
                        sb.Append(" and M_Options.Qnid=M_Question_Answer.Qn_ID ")
                        sb.Append(" where M_Question_Answer.Test_Type=" & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " ")
                        sb.Append(" and M_Question_Answer.Qn_ID= " & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " ")
                        sb.Append(" order by M_Question_Answer.sub_id ")
                        da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
                        da.Fill(dsCrrctOpt)
                        For crr As Integer = 0 To dsCrrctOpt.Tables(0).Rows.Count - 1
                            strCorrectOpt += dsCrrctOpt.Tables(0).Rows(crr).Item(3).ToString + Environment.NewLine
                            arrCrrAns = strCorrectOpt.Split(Environment.NewLine)
                        Next

                        sb = Nothing
                        da = Nothing
                        sb = New StringBuilder
                        dsGivenOpt = New DataSet
                        'get answer given by the student
                        sb.Append(" select  Distinct tr.userid,tr.test_type,tr.qno,tro.option_id as givemid,mop.[Option] , tro.sub_id ")
                        sb.Append(" from T_Result as tr ")
                        sb.Append(" left join T_Result_Option as tro ")
                        sb.Append(" on tro.result_id=tr.result_id ")
                        sb.Append(" left join M_Options as mop ")
                        sb.Append(" on mop.Optionid=tro.option_id ")
                        sb.Append(" and mop.test_type=tr.test_type ")
                        sb.Append(" and mop.Qnid=tr.qno ")
                        sb.Append(" where tr.test_type=" & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " ")
                        sb.Append(" and tr.qno=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " and tr.userid= " & dsQuestion.Tables(0).Rows(i).Item(3).ToString & " and  tr.Course_id=" & _CourseID & "")
                        sb.Append(" order by tro.sub_id ")
                        Dim bb As String = sb.ToString
                        da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
                        da.Fill(dsGivenOpt)
                        For given As Integer = 0 To dsGivenOpt.Tables(0).Rows.Count - 1
                            If dsGivenOpt.Tables(0).Rows(given).Item(4).ToString <> "" Then
                                strGivenAns += dsGivenOpt.Tables(0).Rows(given).Item(4).ToString + Environment.NewLine
                                arrGivenAns = strGivenAns.Split(Environment.NewLine)
                            Else
                                strGivenAns = "Not Attempted"

                            End If

                        Next

                        'Serail number
                        With App.ActiveSheet.Range("B" + start.ToString + ":B" + ends.ToString)
                            .MergeCells = True
                            '.Interior.ColorIndex = 40
                            '.Font.Bold = True
                            ' .Font.ColorIndex = 53
                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            .Cells.Value = i + 1
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                            .WrapText = True
                        End With

                        'subject name
                        With App.ActiveSheet.Range("C" + start.ToString + ":C" + ends.ToString)
                            .MergeCells = True
                            .WrapText = True
                            '.Interior.ColorIndex = 40
                            '.Font.Bold = True
                            ' .Font.ColorIndex = 53
                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(7).ToString
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                        End With
                        'student Name
                        With App.ActiveSheet.Range("D" + start.ToString + ":D" + ends.ToString)
                            .MergeCells = True
                            .WrapText = True
                            '.Interior.ColorIndex = 40
                            '.Font.Bold = True
                            ' .Font.ColorIndex = 53
                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(8).ToString
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                        End With
                        'Question
                        With App.ActiveSheet.Range("E" + start.ToString + ":E" + ends.ToString)
                            .MergeCells = True
                            .WrapText = True
                            '.Interior.ColorIndex = 40
                            ' .Font.Bold = True
                            ' .Font.ColorIndex = 53
                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

                            If dsQuestion.Tables(0).Rows.Count <> 0 Then

                                Dim check() As String = CheckImage(dsQuestion.Tables(0).Rows(i).Item(4).ToString)
                                If check(0) = "" Then
                                    .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(4).ToString
                                ElseIf check(0) <> "" Then

                                    If System.IO.File.Exists(Server.MapPath(check(0))) Then
                                        pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
                                        With pic

                                            'Added by Pragnesha Kulkarni on 2018-06-22 
                                            'Desc: Adjust Image on ExamResult Template
                                            '-------------------------------------------------------------------
                                            .top = App.ActiveSheet.Cells(start, 5).top + 5
                                            .left = App.ActiveSheet.Cells(start, 5).left + 80
                                            .width = 100
                                            .height = 70
                                            ' commented by Pragnesha [Purpose: Question and Question image overlapping on each other]
                                            '.top = App.ActiveSheet.Cells(start + 1, 6).top + 15
                                            '.left = App.ActiveSheet.Cells(start + 1, 6).left + 15
                                            '.width = 83
                                            '.height = 83 
                                            'Ended by Pragnesha Kulkarni
                                            '---------------------------------------------------------------------

                                        End With
                                        .Cells.Value = check(1).ToString

                                        'Added by Pragnesha Kulkarni on 2018-06-22 
                                        'Desc: Adjust Image on ExamResult Template
                                        '------------------------------------------------------------------------------------------
                                        .VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                                        ' Commented by Pragnesha [Purpose: Question and Question image overlapping on each other]
                                        '.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                        '.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter 
                                        '---------------------------------Ended by Pragnesha Kulkarni------------------------------

                                    Else
                                        .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
                                    End If
                                Else
                                End If
                            End If
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                        End With

                        'Correct option
                        With App.ActiveSheet.Range("G" + start.ToString + ":G" + ends.ToString)
                            .MergeCells = True
                            .WrapText = True
                            '.Interior.ColorIndex = 40
                            ' .Font.Bold = True

                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

                            Dim check() As String = CheckImage(strCorrectOpt)
                            If check(0) = "" Then
                                .Cells.Value = strCorrectOpt
                            ElseIf check(0) <> "" Then

                                If System.IO.File.Exists(Server.MapPath(check(0))) Then
                                    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
                                    With pic
                                        .top = App.ActiveSheet.Cells(start + 1, 6).top + 15
                                        .left = App.ActiveSheet.Cells(start + 1, 6).left + 15
                                        .width = 83
                                        .height = 83
                                    End With
                                    .Cells.Value = check(1).ToString
                                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                Else
                                    .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
                                End If
                            Else
                            End If


                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                        End With

                        'given answer by student
                        With App.ActiveSheet.Range("H" + start.ToString + ":H" + ends.ToString)
                            .MergeCells = True

                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter



                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                            .FormulaR1C1 = strGivenAns
                            .Characters(start:=1, Length:=0).Font.ColorIndex = Excel.Constants.xlAutomatic
                            If strGivenAns = "Not Attempted" Then
                                .Characters(start:=1, Length:=strGivenAns.Length).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

                            Else
                                Dim check() As String = CheckImage(strGivenAns)
                                Dim lenth As Integer
                                If check(0) = "" Then
                                    If arrGivenAns.Length < arrCrrAns.Length Then
                                        lenth = arrGivenAns.Length
                                    Else
                                        lenth = arrCrrAns.Length
                                    End If


                                    For Match As Integer = 0 To lenth - 1
                                        If arrCrrAns(Match) = arrGivenAns(Match) Then
                                            Dim jk As String = arrGivenAns(Match)

                                            If Match <> lenth - 1 Then
                                                .Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).Length + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Green)
                                            End If

                                            '.Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).LastIndexOf(arrGivenAns(Match)) + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Green)
                                        Else

                                            '.Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrGivenAns(Match).Length + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)
                                            .Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).LastIndexOf(arrGivenAns(Match)) + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

                                        End If

                                    Next
                                ElseIf check(0) <> "" Then

                                    If System.IO.File.Exists(Server.MapPath(check(0))) Then
                                        pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
                                        With pic
                                            .top = App.ActiveSheet.Cells(start + 1, 7).top + 15
                                            .left = App.ActiveSheet.Cells(start + 1, 7).left + 15
                                            .width = 83
                                            .height = 83
                                        End With
                                        .Cells.Value = check(1).ToString
                                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                    Else
                                        .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
                                    End If


                                Else


                                End If

                                'Apply the color to the second word

                            End If
                            'Apply the color to the third word
                            ' .Characters(start:=11, Length:=4).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Blue)
                            '.Columns.AutoFit()
                            .WrapText = True
                            strGivenAns = Nothing
                            strCorrectOpt = Nothing
                        End With

                        'total marks of the question
                        With App.ActiveSheet.Range("I" + start.ToString + ":I" + ends.ToString)
                            .MergeCells = True
                            .WrapText = True
                            '.Interior.ColorIndex = 40
                            '.Font.Bold = True
                            ' .Font.ColorIndex = 53
                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(5).ToString
                            TotalMarks += dsQuestion.Tables(0).Rows(i).Item(5)
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                        End With

                        'mark obtain by the student
                        With App.ActiveSheet.Range("J" + start.ToString + ":J" + ends.ToString)
                            .MergeCells = True
                            .WrapText = True

                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                            .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(1).ToString
                            MarkObtain += dsQuestion.Tables(0).Rows(i).Item(1)
                            '.Font.Size = 15
                            .BORDERS(xlEdgeLeft).Weight = 2
                            .BORDERS(xlEdgeTop).Weight = 2
                            .BORDERS(xlEdgeBottom).Weight = 2
                            .BORDERS(xlEdgeRight).Weight = 2
                        End With
                        start = ends + 1
                    Next
                End If
                If Not objCn.MyConnection Is Nothing Then
                    If objCn.MyConnection.State = ConnectionState.Open Then
                        objCn.disconnect()
                    End If
                End If

                Sheet = myWorkBook.Worksheets("Summary")
                Dim pt As Microsoft.Office.Interop.Excel.PivotTable = Sheet.PivotTables("PivotTable1")
                pt.RefreshTable()
                Dim _PivotFeild As Excel.PivotFields = CType(pt.RowFields(Type.Missing), Excel.PivotFields)
                If dsQuestion.Tables(0).Rows.Count <> 0 Then
                    For Each _PivotField1 As Excel.PivotField In _PivotFeild

                        If _PivotField1.Value.ToString() = "Subject Name" Then
                            Dim Items As Excel.PivotItems = DirectCast(_PivotField1.VisibleItems, Excel.PivotItems)
                            For Each _PivotItem As Excel.PivotItem In Items
                                If _PivotItem.Caption.Equals("(blank)") Then
                                    _PivotItem.Visible = False
                                End If

                            Next
                        End If
                    Next
                End If
                pt.RefreshTable()


                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                If System.IO.File.Exists(Server.MapPath("ExcelImport\StudentExamDetails.xls")) Then
                    System.IO.File.Delete(Server.MapPath("ExcelImport\StudentExamDetails.xls"))
                End If

                fileName1 = Server.MapPath("ExcelImport\StudentExamDetails.xls")

                myWorkBook.SaveAs(fileName1) ', objOpt, objOpt, objOpt, objOpt, objOpt,Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
                myWorkBook.Close()

                'Added by Pragnesha Kulkarni on 2018/06/01
                ' Reason:Excel process didn't stop after downloading excel sheet
                ' BugID: 719
                '----------------------------------------------------------------
                App.Quit()
                Dim dateEnd As Date = Date.Now
                'End_Excel_App(datestart, dateEnd)

                ' Bug ID: 0904 Getting error on end exam click
                ' Desc: Added code of excel process cpu utilization check and kill process as utilization will equal to zero.
                ' Added by Pragnesha on 23-5-2019
                Excel_Stop()
                '-------Ended by Pragnesha Kulkarni on 2018/06/01----------------

                'Dim file As New io.FileInfo(Server.MapPath("Excel Import\StudentExamDetails.XLS"))
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim lfile As System.IO.FileInfo = New System.IO.FileInfo(Server.MapPath("ExcelImport\StudentExamDetails.xls"))

                If (lfile.Exists) Then
                    Response.Clear()
                    Response.Charset = ""
                    Response.AppendHeader("Content-Type", "application/xls")
                    Response.AppendHeader("Content-Length", lfile.Length.ToString())
                    Response.AppendHeader("content-disposition", "attachment; filename=" + lfile.Name)
                    Response.WriteFile(lfile.FullName)
                    Response.Cache.SetExpires(Date.Now())
                    Response.AddHeader("Cache-Control", "no-cache")
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End If


            Catch ex As Exception
                If log.IsDebugEnabled Then
                    ' Dim err As String = ex.ToString()
                    log.Error("Error occure on Export exam detail ")
                    log.Debug("Error :" & ex.ToString())
                    log.Debug("Error :" & ex.StackTrace)
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

                Throw ex
                'Ended by Pragnesha Kulkarni on 2018/06/01
                Response.Redirect("error.aspx", False)

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
                sheet1 = Nothing
                ImgNotFound = Nothing
                pic = Nothing
                fileName1 = Nothing
                strpath = Nothing
                strCorrectOpt = Nothing
                strGivenAns = Nothing
                strDateTime = Nothing
                dsDateTime = Nothing
                dsQuestion = Nothing
                dsOpt = Nothing
                dsCrrctOpt = Nothing
                dsGivenOpt = Nothing
                dsTotalMarks = Nothing
                da = Nothing
                sb = Nothing
                ImgNotFound = Nothing
                arrCrrAns = Nothing
                arrGivenAns = Nothing
            End Try
        End Sub
        '***********Nisha 2017/09/25*************

        'Added by Pragnesha Kulkarni on 2018/06/01
        ' Reason:Excel process didn't stop after downloading excel sheet
        ' BugID: 719
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
                    '   Thread.Sleep(250)
                    If pct = 0.0 Then
                        pExcelProcess.Kill()
                    End If

                End If
            Next
        End Sub
        'Ended by Pragnesha Kulkarni on 2018/06/04

        Public Function GetID(ByVal query As String, ByVal ColumnName As String) As String
            Try
                Dim strQuery1 As String
                Dim objconn As New ConnectDb
                Dim myCommand As SqlCommand
                Dim myDataReader As SqlDataReader
                'Dim strPathDb As String
                Dim outValue As String = String.Empty
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    strQuery1 = query
                    myCommand = New SqlCommand(strQuery1, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read
                        outValue = myDataReader.Item(ColumnName)
                    End While
                    myCommand = Nothing
                    myDataReader = Nothing
                    objconn.disconnect()
                End If
                Return outValue
            Catch ex As Exception
                Throw ex

            Finally

            End Try

        End Function
        '***********Nisha 2017/09/25*************
#Region "DataGrid ItemCommand Event"
        'Desc: This Event is DataGrid ItemCommand Event.
        'By: Jatin Gangajaliya
        'Date: 2011/2/9

        Protected Sub DGReport_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGReport.ItemCommand

            LblMsg.Visible = False

            ' Dim oWordApp As New Microsoft.Office.Interop.Word.ApplicationClass()
            'Dim oTemplate As Object = Server.MapPath("Mark sheet Template\Marksheet.dot")
            ' Dim missing As Object = System.Reflection.Missing.Value
            'Dim isreadOnly As Object = False
            'Dim isVisible As Object = False
            ' Dim fileName1 As Object = Server.MapPath("Mark sheet Template\Mark-sheet.doc") '25
            Dim strbr As StringBuilder
            Dim strquery, strpath, CandidateID, name, strsubject As String
            Dim dt, dtcourse As DataTable
            Dim objconn As New ConnectDb
            Dim adap As SqlDataAdapter
            Static Dim totalmarks As Integer = 0
            Static Dim marksobtained As Integer = 0
            Dim dblgrade As Double = 0.0
            Dim strrollno, strcenter, strcourse, strgrade, appearedDate As String
            Dim intcourseid As Integer
            Dim arysubject() As String
            Dim MaxMarks, ObtMarks, Rmarks, grd, totmark As String
            Dim _CourseID As String = ""
            Dim _CandidateID As String = ""
            Dim _CenterID As String = ""

            Try

                '***********Nisha 2017/09/25*************
                If (e.CommandName = "Excellsheet") Then
                    _CourseID = e.Item.Cells(15).Text
                    _CandidateID = e.Item.Cells(1).Text
                    _CenterID = GetID("select Center_ID from M_USER_INFO where USERID ='" & _CandidateID & "'", "Center_ID")
                    If (_CenterID <> "") Then
                        ExportExamDetails(_CourseID, _CandidateID, _CenterID)
                    Else
                        LblMsg.Visible = True
                        LblMsg.Text = Resources.Resource.Cand_Status_ExcelSheet
                        LblMsg.ForeColor = Drawing.Color.Red
                    End If
                End If
                    '***********Nisha 2017/09/25*************

                    If (e.CommandName = "MarkSheet") Then
                    CandidateID = e.Item.Cells(1).Text
                    intcourseid = CInt(e.Item.Cells(17).Text)

                    strbr = New StringBuilder
                    'old query
                    ''strbr.Append(" Select test_name from M_Testinfo  ")
                    ''strbr.Append(" join M_Weightage on M_Testinfo.test_type = M_Weightage.test_type ")
                    ''strbr.Append(" join M_Course on M_Course.Course_id = M_Weightage.Course_ID ")
                    ''strbr.Append(" where M_Course.Del_Flag = 0 and M_Weightage.Course_id =  ")
                    ''strbr.Append("'")
                    ''strbr.Append(intcourseid)
                    ''strbr.Append("'")
                    ''strbr.Append(" order by test_name ")
                    strbr.Append("  Select test_name from M_Testinfo   ")
                    strbr.Append(" join T_user_Course on M_Testinfo.test_type = T_user_Course.test_type  ")
                    strbr.Append("  join M_Course on M_Course.Course_id = T_user_Course.Course_ID   ")
                    strbr.Append(" where M_Course.Del_Flag = 0 and T_user_Course.Course_id =  '" & intcourseid & "'  ")
                    strbr.Append(" and T_user_Course.user_id= " & CandidateID.ToString & "  order by test_name  ")
                    strbr.Append("  ")
                    strbr.Append("  ")
                    Dim strtemp As String = strbr.ToString

                    'strpath = ConfigurationSettings.AppSettings("PathDb")
                    dtcourse = New DataTable
                    If objconn.connect() Then
                        adap = New SqlDataAdapter(strtemp, objconn.MyConnection)
                        adap.Fill(dtcourse)
                        If objconn IsNot Nothing Then
                            objconn.disconnect()
                        End If
                    End If
                    strbr = New StringBuilder
                    ReDim arysubject(CStr(dtcourse.Rows.Count - 1))

                    For f As Integer = 0 To dtcourse.Rows.Count - 1
                        arysubject(f) = dtcourse.Rows(f).Item(0).ToString()
                    Next

                    '****************************************************
                    Dim items, it As String
                    For ite As Integer = 0 To arysubject.Length - 1
                        items = items & arysubject(ite) & ","
                    Next
                    Dim stritems As String = items.Remove(items.LastIndexOf(","))
                    '****************************************************

                    Dim item As String
                    For Each item In arysubject
                        strbr.AppendLine(item)
                    Next
                    strsubject = strbr.ToString

                    LblMsg.Visible = False

                    strbr = New StringBuilder
                    strbr.Append(" select * from(select tcs.userid, ")
                    strbr.Append(" (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as username, ")
                    strbr.Append(" tcs.course_id, ")
                    strbr.Append(" tcs.loginname as LoginName, ")
                    strbr.Append(" tcs.pwd as 'Password', ")
                    strbr.Append(" tcs.written_test_date as writtentestdate, ")
                    strbr.Append(" tcs.written_test_appear_date as appearancedate, ")
                    strbr.Append(" tcs.appearedflag as appearedflag, ")
                    strbr.Append(" mc.course_name,mce.Center_Name, ")
                    strbr.Append(" mui.Center_ID,mui.rollno, ")
                    strbr.Append(" tcs.total_marks, ")
                    strbr.Append(" tcs.total_passmarks, ")
                    strbr.Append(" isnull(obtained1.obtained_marks,0) as obtained_marks, ")
                    strbr.Append(" (case  when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 75 then  'A+' ")
                    strbr.Append(" when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >=60 then 'A' ")
                    strbr.Append(" when isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 50 then 'B' ")
                    strbr.Append(" WHEN (obtained1.obtained_marks*100)/tcs.total_marks is null then null else 'C' End ) as Grad ")
                    strbr.Append(" ,(case WHEN obtained1.obtained_marks >= tcs.total_passmarks then 'Pass' ")
                    strbr.Append(" WHEN obtained1.obtained_marks is null then null ")
                    strbr.Append(" ELSE 'Fail' ")
                    strbr.Append(" END) as Status ")

                    strbr.Append(" FROM T_Candidate_Status as tcs ")
                    strbr.Append(" left join M_USER_INFO as mui ")
                    strbr.Append(" on tcs.userid=mui.userid ")
                    strbr.Append(" left join m_course as mc ")
                    strbr.Append(" on mc.course_id=tcs.course_id ")
                    strbr.Append(" Left Join ")
                    strbr.Append(" (select sum(temp.obtained_marks) as obtained_marks,temp.course_id,temp.userid from (select ")
                    strbr.Append(" ( ")
                    strbr.Append(" case ")
                    strbr.Append(" WHEN mq.Qn_Category_ID=3 then ")
                    strbr.Append(" ( ")
                    strbr.Append(" case ")
                    strbr.Append(" WHEN mqa.sub_id=tro.sub_id then ")
                    strbr.Append(" (Case ")
                    strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                    strbr.Append(" count(mqa.Correct_Opt_Id)")
                    strbr.Append(" ELSE 0 ")
                    strbr.Append(" END) ")
                    strbr.Append(" ELSE 0 ")
                    strbr.Append(" End ")
                    strbr.Append(" ) ")
                    strbr.Append(" WHEN mq.Qn_Category_ID=2 then ")
                    strbr.Append(" ( ")
                    strbr.Append(" CASE  ")
                    strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                    strbr.Append(" count(mqa.Correct_Opt_Id)")
                    strbr.Append(" ELSE 0")
                    strbr.Append(" End ")
                    strbr.Append(" ) ")
                    strbr.Append(" WHEN mq.Qn_Category_ID=1 then ")
                    strbr.Append(" ( ")
                    strbr.Append(" CASE  ")
                    strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                    strbr.Append(" SUM(mq.Total_Marks)")
                    strbr.Append(" ELSE 0 ")
                    strbr.Append(" End ")
                    strbr.Append(" ) ")
                    strbr.Append(" End ")
                    strbr.Append(" ) as obtained_marks ")
                    strbr.Append(" ,mc.course_id,mui.userid ")
                    strbr.Append(" from m_question as mq ")
                    strbr.Append(" left join M_Question_Answer as mqa ")
                    strbr.Append(" on mqa.Qn_ID=mq.qnid and mqa.test_type=mq.test_type ")
                    strbr.Append(" left join t_result as tr ")
                    strbr.Append(" on tr.qno=mq.qnid ")
                    strbr.Append(" AND tr.test_type=mq.test_type ")
                    strbr.Append(" left join m_user_info as mui ")
                    strbr.Append(" on mui.userid=tr.userid ")
                    strbr.Append(" left join m_course as mc ")
                    strbr.Append(" on mc.course_id=tr.course_id ")
                    strbr.Append(" left join m_testinfo as mti ")
                    strbr.Append(" on mti.test_type=tr.test_type ")
                    strbr.Append(" left join t_result_option as tro ")
                    strbr.Append(" on tro.result_id=tr.result_id ")
                    strbr.Append(" and tr.test_type=mti.test_type ")
                    strbr.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
                    strbr.Append(" group by mq.total_marks,mc.course_id, ")
                    strbr.Append(" mq.total_marks,mq.qnid,mqa.Sub_id, ")
                    strbr.Append(" mqa.Correct_Opt_Id, tro.option_id, mq.Qn_Category_ID, tro.sub_id, mui.userid ")
                    strbr.Append(" )temp ")
                    strbr.Append(" group by temp.course_id,temp.userid ")
                    strbr.Append(" )as obtained1 ")
                    strbr.Append(" on obtained1.course_id=tcs.course_id ")
                    strbr.Append(" and tcs.userid=obtained1.userid ")
                    strbr.Append(" left join M_Centers as mce ")
                    strbr.Append(" on mce.Center_ID=mui.Center_ID ")
                    strbr.Append(" )temp ")
                    strbr.Append(" order by temp.username ")
                    strquery = strbr.ToString()

                    dt = New DataTable

                    If objconn.connect() Then
                        adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                        adap.Fill(dt)
                        If objconn IsNot Nothing Then
                            objconn.disconnect()
                        End If
                    End If

                    Dim introwcount As Integer = 0
                    For j As Int16 = 0 To dt.Rows.Count - 1
                        If (dt.Rows(j).Item(2) = intcourseid.ToString And dt.Rows(j).Item(0) = CandidateID.ToString) Then
                            introwcount = introwcount + 1

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("username").ToString)) Then
                                name = dt.Rows(j).Item("username").ToString
                            Else
                                name = ""
                            End If

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("RollNo").ToString)) Then
                                strrollno = dt.Rows(j).Item("RollNo").ToString
                            End If

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("Center_Name").ToString)) Then
                                strcenter = dt.Rows(j).Item("Center_Name").ToString
                            End If

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("course_name").ToString)) Then
                                strcourse = dt.Rows(j).Item("course_name").ToString
                            End If

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("grad").ToString)) Then
                                strgrade = dt.Rows(j).Item("grad").ToString
                            End If

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("appearancedate").ToString)) Then
                                appearedDate = Convert.ToDateTime((dt.Rows(j).Item("appearancedate").ToString)).ToString("dd/MM/yyyy")
                            End If
                        End If
                    Next
                    '*******************************************************************************************************************************************
                    For o As Integer = 0 To dt.Rows.Count - 1

                        If (dt.Rows(o).Item(2) = intcourseid.ToString And dt.Rows(o).Item(0) = CandidateID.ToString) Then
                            MaxMarks = dt.Rows(o).Item("total_marks").ToString
                            ObtMarks = dt.Rows(o).Item("obtained_marks").ToString
                            Rmarks = dt.Rows(o).Item("Status").ToString

                        End If
                    Next

                    For j As Int16 = 0 To dt.Rows.Count - 1
                        If (dt.Rows(j).Item(2) = intcourseid.ToString And dt.Rows(j).Item(0) = CandidateID.ToString) Then

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("total_marks").ToString)) Then
                                totalmarks = totalmarks + Convert.ToInt32(dt.Rows(j).Item("total_marks"))
                            Else
                                totalmarks = 0
                            End If

                            If Not (String.IsNullOrEmpty(dt.Rows(j).Item("obtained_marks").ToString)) Then
                                marksobtained = marksobtained + Convert.ToInt32(dt.Rows(j).Item("obtained_marks"))
                            Else
                                marksobtained = 0
                            End If
                        End If
                    Next
                    grd = strgrade
                    totmark = Math.Round((marksobtained * 100 / totalmarks), 2).ToString + "%"

                    '========================for PDF file ==============================
                    makepdffile(Server.MapPath("Mark sheet Template\Marksheet.pdf"), strrollno, strcenter, strcourse, name, appearedDate, stritems, MaxMarks, ObtMarks, Rmarks, grd, totmark)

                    '********************************************************************************************************************************************************************************************************************
                    'oWordApp.Visible = False

                    'Dim oWordDoc As Microsoft.Office.Interop.Word.Document

                    'Dim oWordDoc3 As Microsoft.Office.Interop.Word.Document



                    'oWordDoc = oWordApp.Documents.Add(oTemplate, missing, missing, missing)

                    'oWordDoc.Activate()

                    ''To fill data into all bookmarks there in the template file.
                    'Dim i As Integer = 1
                    'For Each b As Microsoft.Office.Interop.Word.Bookmark In oWordDoc.Bookmarks
                    '    b.Range.Font.Name = Constant.strtextcolour
                    '    b.Range.Font.Size = 10

                    '    Dim [option] As String = ""
                    '    Select Case b.Name
                    '        Case "centre"
                    '            [option] = strcenter
                    '            Exit Select

                    '        Case "course"
                    '            [option] = strcourse
                    '            Exit Select

                    '        Case "name"
                    '            [option] = name
                    '            Exit Select

                    '        Case "rno"
                    '            [option] = strrollno
                    '            Exit Select

                    '        Case "date"
                    '            ' [option] = Now.Day & "/" & Now.Month & "/" & Now.Year
                    '            [option] = appearedDate
                    '            Exit Select

                    '        Case "table"
                    '            [option] = "333"
                    '            Dim oTable As Microsoft.Office.Interop.Word.Table
                    '            Dim oRow As Microsoft.Office.Interop.Word.Row

                    '            Dim tbRows As Integer = introwcount + 2

                    '            oTable = oWordDoc.Content.Tables.Add(b.Range, tbRows, 4, missing, missing)
                    '            oTable.Rows(1).Range.Bold = 1

                    '            oTable.Cell(1, 1).Range.Text = "Subject"
                    '            oTable.Cell(1, 1).Range.Font.Name = Constant.strtextcolour

                    '            oTable.Cell(1, 2).Range.Text = "Maximum Marks"
                    '            oTable.Cell(1, 2).Range.Font.Name = Constant.strtextcolour

                    '            oTable.Cell(1, 3).Range.Text = "Marks Obtained"
                    '            oTable.Cell(1, 3).Range.Font.Name = Constant.strtextcolour

                    '            oTable.Cell(1, 4).Range.Text = "Remarks"
                    '            oTable.Cell(1, 4).Range.Font.Name = Constant.strtextcolour

                    '            oTable.Borders.OutsideColor = Microsoft.Office.Interop.Word.WdColor.wdColorGray75
                    '            oTable.Range.ParagraphFormat.SpaceAfter = 6

                    '            'To enter data into table created in MS-Word. 
                    '            Dim r As Integer = 2
                    '            For o As Integer = 0 To dt.Rows.Count - 1

                    '                If (dt.Rows(o).Item(2) = intcourseid.ToString And dt.Rows(o).Item(0) = CandidateID.ToString) Then

                    '                    For col As Integer = 1 To 4
                    '                        Select Case col

                    '                            Case 1
                    '                                oTable.Cell(r, col).Range.Text = strsubject
                    '                                oTable.Cell(r, col).Range.Font.Name = Constant.strtextcolour
                    '                                oTable.Cell(r, col).Range.Font.Size = 8

                    '                                Exit Select

                    '                            Case 2
                    '                                If Not (String.IsNullOrEmpty(dt.Rows(o).Item("total_marks").ToString)) Then
                    '                                    oTable.Cell(r, col).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
                    '                                    oTable.Cell(r, col).Range.Text = dt.Rows(o).Item("total_marks").ToString
                    '                                    oTable.Cell(r, col).Range.Font.Name = Constant.strtextcolour
                    '                                End If
                    '                                Exit Select

                    '                            Case 3
                    '                                If Not (String.IsNullOrEmpty(dt.Rows(o).Item("obtained_marks").ToString)) Then
                    '                                    oTable.Cell(r, col).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
                    '                                    oTable.Cell(r, col).Range.Text = dt.Rows(o).Item("obtained_marks").ToString
                    '                                    oTable.Cell(r, col).Range.Font.Name = Constant.strtextcolour
                    '                                End If
                    '                                Exit Select

                    '                            Case 4
                    '                                'oTable.Cell(r, col).Range.Text = ""
                    '                                'oTable.Cell(r, col).Range.Font.Name = Constant.strtextcolour
                    '                                'Exit Select

                    '                                If Not (String.IsNullOrEmpty(dt.Rows(o).Item("Status").ToString)) Then
                    '                                    oTable.Cell(r, col).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
                    '                                    oTable.Cell(r, col).Range.Text = dt.Rows(o).Item("Status").ToString
                    '                                    oTable.Cell(r, col).Range.Font.Name = Constant.strtextcolour
                    '                                End If
                    '                                Exit Select

                    '                            Case Else
                    '                                Exit Select

                    '                        End Select
                    '                    Next
                    '                    r += 1
                    '                End If
                    '            Next

                    '            oTable.Rows(r).Range.Bold = 1

                    '            For j As Int16 = 0 To dt.Rows.Count - 1
                    '                If (dt.Rows(j).Item(2) = intcourseid.ToString And dt.Rows(j).Item(0) = CandidateID.ToString) Then

                    '                    If Not (String.IsNullOrEmpty(dt.Rows(j).Item("total_marks").ToString)) Then
                    '                        totalmarks = totalmarks + Convert.ToInt32(dt.Rows(j).Item("total_marks"))
                    '                    Else
                    '                        totalmarks = 0
                    '                    End If

                    '                    If Not (String.IsNullOrEmpty(dt.Rows(j).Item("obtained_marks").ToString)) Then
                    '                        marksobtained = marksobtained + Convert.ToInt32(dt.Rows(j).Item("obtained_marks"))
                    '                    Else
                    '                        marksobtained = 0

                    '                    End If
                    '                End If
                    '            Next

                    '            'If (totalmarks <> 0 And marksobtained <> 0) Then
                    '            '    dblgrade = (100 * marksobtained) / totalmarks
                    '            'Else
                    '            '    dblgrade = 0
                    '            'End If

                    '            'If (dblgrade <> 0) Then
                    '            '    If (dblgrade >= 75) Then
                    '            '        [option] = "A+"
                    '            '    ElseIf (dblgrade >= 60 And dblgrade <75) Then
                    '            '        [option] = "A"
                                                                                  '            '    ElseIf (dblgrade >= 50 And dblgrade < 60) Then
                                                                                  '            '        [option] = "B"
                                                                                  '            '    ElseIf (dblgrade >= 40 And dblgrade < 50) Then
                                                                                  '            '        [option] = "C"
                                                                                  '            '    Else
                                                                                  '            '        [option] = "C"
                                                                                  '            '    End If
                                                                                  '            'Else
                                                                                  '            '    [option] = "C"
                                                                                  '            'End If

                                                                                  '            oTable.Rows(r).Cells().Merge()
                                                                                  '            oTable.Cell(r, 1).Range.Font.Name = Constant.strtextcolour
                                                                                  '            oTable.Cell(r, 1).Range.Text = "Grade : " & strgrade & "                                                                                Total : " & marksobtained.ToString & "/" & totalmarks.ToString
                                                                                  '            [option] = ""

                                                                                  '            oTable.Borders(Microsoft.Office.Interop.Word.WdBorderType.wdBorderTop).LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle
                                                                                  '            oTable.Borders(Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom).LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle
                                                                                  '            oTable.Borders(Microsoft.Office.Interop.Word.WdBorderType.wdBorderRight).LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle
                                                                                  '            oTable.Borders(Microsoft.Office.Interop.Word.WdBorderType.wdBorderLeft).LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle
                                                                                  '            oTable.Borders(Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal).LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle
                                                                                  '            oTable.Borders(Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical).LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle

                                                                                  '            Exit Select
                                                                                  '        Case Else
                                                                                  '            Exit Select
                                                                                  '    End Select

                                                                                  '    If [option] <> "" Then
                                                                                  '        b.Range.Text = [option]
                                                                                  '    End If
                                                                                  'Next

                                                                                  'oWordDoc.SaveAs(fileName1, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing)

                                                                                  'oWordDoc.Close()

                                                                                  'oWordApp.Application.Quit(missing, missing, missing)

                                                                                  'Dim file As New IO.FileInfo(Server.MapPath("Mark sheet Template\Mark-sheet.doc"))

                                                                                  'If file.Exists = True Then

                                                                                  '    Response.Clear()
                                                                                  '    Response.ClearHeaders()
                                                                                  '    Response.ClearContent()

                                                                                  '    Response.ContentType = "application/pdf"

                                                                                  '    Response.AppendHeader("Content-disposition", "attachment; filename=" + file.Name)

                                                                                  '    Response.AddHeader("Content-Length", file.Length.ToString)
                                                                                  '    Response.ContentType = "application/octet-stream"

                                                                                  '    Response.WriteFile(file.FullName)

                                                                                  '    HttpContext.Current.ApplicationInstance.CompleteRequest()
                                                                                  '    ' Response.End()
                                                                                  '    'Response.Flush()

                                                                                  'End If





                    ElseIf (e.CommandName = "Reassign") Then
                    lbldate2.Text = e.Item.Cells(8).Text
                    ModalPopupExtender2.Show()
                    CandidateID = e.Item.Cells(1).Text
                    intcourseid = CInt(e.Item.Cells(15).Text)
                    Session.Add("CandidateID", CandidateID)
                    Session.Add("CourseID", intcourseid)
                    'Dim str As String
                    'Dim cda As SqlDataAdapter
                    'Dim cds As DataSet
                    'Dim q As String = "select convert(varchar(10), written_test_date,103)  from T_candidate_status where userid=" & CandidateID & " and Course_id=" & intcourseid


                    ''Select Case MsgBox("The older history will be lost. Are you sure you want to Re-Assign Exam.", MsgBoxStyle.YesNoCancel, "Are You Sure?")
                    ''    Case MsgBoxResult.Yes
                    ''        str = "Yes"
                    ''    Case MsgBoxResult.Cancel
                    ''        str = "Can"
                    ''    Case MsgBoxResult.No
                    ''        str = "No"
                    ''End Select
                    ''If str = "Yes" Then

                    'Dim cmd As SqlCommand
                    'Dim objStrBldr As StringBuilder
                    'Try
                    '    If objconn.connect(strpath) Then
                    '        'date will be shown in popup as OldDate
                    '        cda = New SqlDataAdapter(q, objconn.MyConnection)
                    '        cds = New DataSet
                    '        cda.Fill(cds)
                    '        lbldate2.Text = cds.Tables(0).Rows(0).Item(0).ToString.Substring(0, 10)
                    '        Session.Add("cdCID", intcourseid)
                    '        Session.Add("cdUID", CandidateID)
                    '        'delete rows from t_result_option (Probably 40 rows)
                    '        objStrBldr = New StringBuilder()
                    '        objStrBldr.Append("delete from t_result_option where result_id in (select result_id from t_result where userid=" + CandidateID.ToString() + " and course_id=" + intcourseid.ToString() + ")")
                    '        cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    '        cmd.ExecuteNonQuery()
                    '        objStrBldr = Nothing
                    '        'delete rows from t_result (Probably 40 rows)
                    '        objStrBldr = New StringBuilder()
                    '        objStrBldr.Append("delete from t_result where userid=" + CandidateID.ToString() + " and course_id=" + intcourseid.ToString())
                    '        cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    '        cmd.ExecuteNonQuery()
                    '        objStrBldr = Nothing


                    '        Dim item As DictionaryEntry
                    '        Dim objht As New Hashtable
                    '        objht = CheckQuestions(intcourseid)
                    '        Dim inttotalque As Integer
                    '        Dim sb As StringBuilder
                    '        Dim ary() As Integer
                    '        Try
                    '            'Loop for each HashTable enrty.
                    '            For Each item In objht
                    '                ary = DirectCast(item.Value, Integer())

                    '                For g As Integer = 0 To 2
                    '                    sb = New StringBuilder
                    '                    sb.Append(" select isnull(Sum(Total_Marks),0) from m_question where del_flag=0 and ")
                    '                    sb.Append(" test_type= ")
                    '                    sb.Append(item.Key)
                    '                    sb.Append(" and Qn_category_id= ")
                    '                    sb.Append(g + 1)
                    '                    strquery = sb.ToString()
                    '                    cmd = New SqlCommand(strquery, objconn.MyConnection)
                    '                    inttotalque = cmd.ExecuteScalar()
                    '                    If inttotalque < ary(g) Then
                    '                        LblMsg.ForeColor = Color.FromName("Red")
                    '                        LblMsg.Text = "There are not enough question for the test. Please verify!!"
                    '                        LblMsg.Visible = True
                    '                        Exit Sub
                    '                    End If
                    '                Next
                    '            Next

                    '        Catch ex As Exception
                    '            If log.IsDebugEnabled Then
                    '                log.Debug("Error :" & ex.ToString())
                    '            End If
                    '            If objconn IsNot Nothing Then
                    '                objconn.disconnect()
                    '            End If
                    '            Throw ex
                    '        Finally
                    '            item = Nothing
                    '            objht = Nothing
                    '            cmd = Nothing
                    '            sb = Nothing
                    '            strquery = Nothing
                    '            ary = Nothing
                    '        End Try

                    '        Dim myCommand1 As SqlTransaction
                    '        Dim rdr As SqlDataReader
                    '        objStrBldr = New StringBuilder
                    '        objStrBldr.Append(" Select Total_Time,Total_marks,Total_passmarks From M_Course where Course_id =  ")
                    '        objStrBldr.Append(intcourseid)

                    '        cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    '        myCommand1 = objconn.MyConnection.BeginTransaction()
                    '        cmd.Transaction = myCommand1
                    '        Dim intcoursetime, intcoursemarks, intcoursepassmarks As Integer
                    '        rdr = cmd.ExecuteReader()
                    '        While rdr.Read
                    '            intcoursetime = rdr.Item("Total_Time")
                    '            intcoursemarks = rdr.Item("Total_marks")
                    '            intcoursepassmarks = rdr.Item("Total_passmarks")
                    '        End While
                    '        rdr.Close()
                    '        myCommand1.Commit()

                    '        'Dim strpassword As String = GetRandomPasswordUsingGUID()
                    '        'Dim strname As String = tblResult.Rows(j + 1).Cells(1).InnerText
                    '        ''  Dim struserid As String = strname & chkUsers(j).Value
                    '        'Dim struserid As String = GetRollNumber(chkUsers(j).Value)

                    '        ''Enter record for user who is selected to appear for online examination

                    '        'If (struserid <> String.Empty And strpassword <> String.Empty) Then
                    '        '    'UserInfo(0) = chkUsers(j).Value
                    '        '    'UserInfo(1) = strpassword
                    '        '    'UserInfo(2) = ddlcourse.SelectedItem.Text
                    '        '    'UserInfo(3) = intcoursemarks
                    '        '    'UserInfo(4) = intcoursepassmarks
                    '        '    'UserInfo(5) = intcoursetime

                    '        'strsqlinsert = " INSERT INTO t_candidate_status "
                    '        'strsqlinsert = strsqlinsert & "(userid,Course_ID,written_test_date,consume_time,LoginName,Pwd,Total_Time,Total_marks,Total_passmarks) "
                    '        'strsqlinsert = strsqlinsert & "Values( "
                    '        'strsqlinsert = strsqlinsert & "" & chkUsers(j).Value & ","
                    '        'strsqlinsert = strsqlinsert & "'" & Replace(ddlcourse.SelectedItem.Value, "'", "''") & "',"
                    '        'strsqlinsert = strsqlinsert & "'" & ConvertDate(txtExamDate.Text) & " ',"
                    '        ''strsqlinsert = strsqlinsert & "'" & myDate & "',"
                    '        'strsqlinsert = strsqlinsert & "'0'"
                    '        'strsqlinsert = strsqlinsert & "," & "'" & struserid & "'" & "," & "'" & strpassword & "' " & ","
                    '        'strsqlinsert = strsqlinsert & intcoursetime & "," & intcoursemarks & "," & intcoursepassmarks
                    '        'strsqlinsert = strsqlinsert & " )"
                    '        '    strsqlinsert = Replace(strsqlinsert, "''", "NULL")
                    '        '    myCommand.Connection = objconn.MyConnection
                    '        '    myCommand = New SqlCommand(strsqlinsert, objconn.MyConnection)
                    '        '    myCommand1 = objconn.MyConnection.BeginTransaction()
                    '        '    myCommand.Transaction = myCommand1
                    '        '    myCommand.ExecuteNonQuery()
                    '        '    myCommand1.Commit()
                    '        '    sendingFail = MailForOnlineTest(chkUsers(j).Value, strpassword, ddlcourse.SelectedItem.Text, intcoursemarks, intcoursepassmarks, intcoursetime)
                    '        '    If sendingFail <> "" Then
                    '        '        Send_Fail += sendingFail + ","
                    '        '    End If
                    '        'update user in t_candidate_status as Newly Assigned Student
                    '        objStrBldr = New StringBuilder()
                    '        objStrBldr.Append("update t_candidate_status ")
                    '        'objStrBldr.Append("set AppearedFlag=0, written_test_date=getdate(), written_test_appear_date=null, Consume_time=null, ")
                    '        objStrBldr.Append("set AppearedFlag=0,  written_test_appear_date=null, Consume_time=null, ")
                    '        objStrBldr.Append("Total_Time=" + intcoursetime.ToString() + ", Total_marks=" + intcoursemarks.ToString() + ", Total_passmarks=" + intcoursepassmarks.ToString())
                    '        objStrBldr.Append(" where userid=" + CandidateID.ToString() + " and course_id=" + intcourseid.ToString())
                    '        cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    '        cmd.ExecuteNonQuery()
                    '        objStrBldr = Nothing


                    '        'End If
                    '        'UpdForOnlineTest(0) = struserid
                    '        'UpdForOnlineTest(1) = strpassword
                    '        objStrBldr = New StringBuilder()
                    '        objStrBldr.Append(" Update T_User_Course set Del_Flag = 1  ")
                    '        objStrBldr.Append(" where T_User_Course.User_ID = ")
                    '        objStrBldr.Append(CandidateID.ToString())
                    '        objStrBldr.Append(" and T_User_Course.Course_ID = ")
                    '        objStrBldr.Append(intcourseid.ToString())
                    '        cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    '        myCommand1 = objconn.MyConnection.BeginTransaction()
                    '        cmd.Transaction = myCommand1
                    '        cmd.ExecuteNonQuery()
                    '        myCommand1.Commit()

                    '        Dim strExamURl As String
                    '        Dim strQuery1 As String
                    '        Dim myDataReader As SqlDataReader
                    '        Dim myDataReader2 As SqlDataReader
                    '        Dim strEmaiId As String
                    '        Dim strOwnerName As String
                    '        Dim mail As New MailMessage
                    '        Dim objCommFun As CommonFunction
                    '        Dim strMessage As String

                    '        strExamURl = ConfigurationSettings.AppSettings("ExamURL")
                    '        strQuery1 = "select mc.Email,mc.Owner_Name From M_Centers as mc join T_Center_Course as tcc on mc.Center_ID=tcc.Center_ID where tcc.Course_ID=" + intcourseid.ToString() + " and mc.center_id=(select center_id from m_user_info where userid=" + CandidateID.ToString() + ")"
                    '        cmd = New SqlCommand(strQuery1, objconn.MyConnection)
                    '        myDataReader = cmd.ExecuteReader()
                    '        While myDataReader.Read
                    '            If Not IsDBNull(myDataReader.Item("Email")) Then
                    '                strEmaiId = myDataReader.Item("Email")
                    '                strOwnerName = myDataReader.Item("Owner_Name")
                    '            End If
                    '        End While
                    '        myDataReader.Close()
                    '        myDataReader = Nothing
                    '        'mail.From = strEmaiId
                    '        'mail.From = "vaibhav@usindia.com"
                    '        mail.From = ConfigurationSettings.AppSettings("mailsenderid")
                    '        ' mail.Cc = "vaibhav@usindia.com" 'strEmaiId
                    '        mail.Cc = strEmaiId

                    '        strQuery1 = "select email from m_user_info where userid=" + CandidateID.ToString()
                    '        cmd = New SqlCommand(strQuery1, objconn.MyConnection)
                    '        myDataReader2 = cmd.ExecuteReader()
                    '        While myDataReader2.Read
                    '            If Not IsDBNull(myDataReader2.Item("Email")) Then
                    '                mail.To = myDataReader2.Item("Email")
                    '            End If
                    '        End While
                    '        myDataReader2.Close()
                    '        myDataReader2 = Nothing
                    '        objCommFun = New CommonFunction()
                    '        q = "select (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as Name, "
                    '        q = q + "mc.center_name as ClassName, mti.Test_name as TestName, "
                    '        q = q + "convert(varchar(10), tcs.Written_Test_Date,120) as ExamDate, tcs.total_time as TotalTime, "
                    '        q = q + "tcs.total_Marks as TotalMarks, tcs.userid as UserID,mc.owner_name as Teacher, "
                    '        q = q + "tcs.Loginname as LoginName, tcs.pwd as Password, mui.email as EmailID "
                    '        q = q + "from t_candidate_status as tcs "
                    '        q = q + "left join M_User_info as mui on mui.userid=tcs.userid "
                    '        q = q + "left join M_Centers as mc on mc.center_id=mui.center_id "
                    '        q = q + "left join T_User_course as tuc on tuc.User_ID=tcs.Userid and tuc.course_id=tcs.course_id "
                    '        q = q + "left join M_Testinfo as mti on mti.Test_type=tuc.Test_type "
                    '        q = q + "where tcs.userid=" + CandidateID.ToString() + " and tcs.course_id=" + intcourseid.ToString()
                    '        cda = New SqlDataAdapter(q, objconn.MyConnection)
                    '        cds = New DataSet
                    '        cda.Fill(cds)
                    '        Session.Add("ReassignDataSetValues", cds)
                    '        strMessage = objCommFun.ReadFile(Server.MapPath(ConfigurationSettings.AppSettings("AssignExamMail")))
                    '        strMessage = strMessage.Replace("#Name#", cds.Tables(0).Rows(0)("Name").ToString())
                    '        strMessage = strMessage.Replace("#ClassName#", cds.Tables(0).Rows(0)("ClassName").ToString())
                    '        strMessage = strMessage.Replace("#sub#", cds.Tables(0).Rows(0)("TestName").ToString())
                    '        strMessage = strMessage.Replace("#date#", cds.Tables(0).Rows(0)("ExamDate").ToString())
                    '        strMessage = strMessage.Replace("#Marks#", cds.Tables(0).Rows(0)("TotalMarks").ToString())
                    '        strMessage = strMessage.Replace("#UserId#", cds.Tables(0).Rows(0)("LoginName").ToString())
                    '        strMessage = strMessage.Replace("#Password#", cds.Tables(0).Rows(0)("Password").ToString())
                    '        strMessage = strMessage.Replace("#TeacherName#", cds.Tables(0).Rows(0)("Teacher").ToString())
                    '        strMessage = strMessage.Replace("#links#", "http://idrinternals.usindia.com:8088/StudentLogin.aspx")

                    '        mail.Subject = "Request to appear an exam of (" + cds.Tables(0).Rows(0)("ClassName").ToString() + " - " + cds.Tables(0).Rows(0)("TestName").ToString() + ")"
                    '        mail.Body = strMessage
                    '        mail.BodyFormat = MailFormat.Html
                    '        mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
                    '        mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
                    '        mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                    '        SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
                    '        SmtpMail.Send(mail)

                    '    End If
                    'Catch ex As Exception
                    '    If log.IsDebugEnabled Then
                    '        log.Debug("Error :" & ex.ToString())
                    '    End If
                    '    If objconn IsNot Nothing Then
                    '        objconn.disconnect()
                    '    End If
                    '    Response.Redirect("error.aspx", False)
                    'Finally
                    '    cmd = Nothing
                    '    If objconn IsNot Nothing Then
                    '        objconn.disconnect()
                    '    End If
                    'End Try
                    ' End If

                ElseIf (e.CommandName = "EditDate") Then
                    ModalPopupExtender1.Show()
                    CandidateID = e.Item.Cells(1).Text
                    Dim cid As String = e.Item.Cells(15).Text
                    Dim cda As SqlDataAdapter
                    Dim cds As DataSet
                    Dim q As String = "select convert(varchar(10), written_test_date,103)  from T_candidate_status where userid=" & CandidateID & " and Course_id=" & cid
                    Try
                        If objconn.connect() Then
                            cda = New SqlDataAdapter(q, objconn.MyConnection)
                            cds = New DataSet
                            cda.Fill(cds)
                            lbldate.Text = cds.Tables(0).Rows(0).Item(0).ToString.Substring(0, 10)
                            Session.Add("cdCID", cid)
                            Session.Add("cdUID", CandidateID)
                        End If
                    Catch ex As Exception
                        If log.IsDebugEnabled Then
                            log.Debug("Error :" & ex.ToString())
                        End If
                        Response.Redirect("error.aspx", False)
                    Finally
                        cda = Nothing
                        cds = Nothing
                        If objconn IsNot Nothing Then
                            objconn.disconnect()
                        End If
                    End Try
                ElseIf (e.CommandName = "Disable") Then
                    'select * from T_candidate_Status where  userid=178 and course_id=150 
                    Dim cmd As SqlCommand
                    CandidateID = e.Item.Cells(1).Text
                    Dim cid As String = e.Item.Cells(15).Text
                    Try
                        If objconn.connect() Then

                            'Delete candidate from t_candidate_status table
                            cmd = New SqlCommand("delete from T_candidate_Status where  userid=" & CandidateID & " and course_id=" & cid, objconn.MyConnection)
                            cmd.ExecuteNonQuery()

                            'Date: 2012/08/01
                            'Bharat
                            'Delete candidate course information from t_user_course table also if course is disable.
                            cmd = New SqlCommand("delete from t_user_course where  user_id=" & CandidateID & " and course_id=" & cid, objconn.MyConnection)
                            cmd.ExecuteNonQuery()

                            'If objconn.MyConnection.State = ConnectionState.Open Then
                            '    objconn.disconnect()
                            'End If
                            BindGrid()
                        End If
                    Catch ex As Exception
                        'If objconn.MyConnection.State = ConnectionState.Open Then
                        '    objconn.disconnect()
                        'End If
                        If log.IsDebugEnabled Then
                            log.Debug("Error :" & ex.ToString())
                        End If
                        Response.Redirect("error.aspx", False)
                    Finally
                        'If objconn.MyConnection.State = ConnectionState.Open Then
                        '    objconn.disconnect()
                        'End If
                        'objconn = Nothing
                        cmd = Nothing
                    End Try
                ElseIf (e.CommandName = "Remind") Then
                    CandidateID = e.Item.Cells(1).Text
                    Dim cid As String = e.Item.Cells(15).Text
                    Dim strCourseName = e.Item.Cells(5).Text
                    'if GridView id check appeared or not if appeared then enabled false

                    BindGrid() ' added by rajesh Nagvanshi 2014-06-02 


                    Dim objMailfunction As CommonFunction
                    Dim strMessage As String
                    Dim cda As SqlDataAdapter
                    Dim cds As DataSet
                    'Dim q As String = "select convert(varchar(10), written_test_date,103)  from T_candidate_status where userid=" & CandidateID & " and Course_id=" & cid
                    Dim q As String = "select (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as Name, "
                    q = q + "mc.center_name as ClassName, mti.Test_name as TestName, "
                    'q = q + "convert(varchar(10), tcs.Written_Test_Date+3,120) as ExamDate, "
                    q = q + "convert(varchar(10), tcs.Written_Test_Date+(convert(int,(select value from M_System_Settings where Key1 like '%Exam Within Date%' and Key2 like '%Within Date%'))),120) as ExamDate, "
                    q = q + "tcs.total_time as TotalTime, "
                    q = q + "tcs.total_Marks as TotalMarks, tcs.userid as UserID,mc.owner_name as Teacher, "
                    q = q + "tcs.Loginname as LoginName, tcs.pwd as Password, mui.email as EmailID "
                    q = q + "from t_candidate_status as tcs "
                    q = q + "left join M_User_info as mui on mui.userid=tcs.userid "
                    q = q + "left join M_Centers as mc on mc.center_id=mui.center_id "
                    q = q + "left join T_User_course as tuc on tuc.User_ID=tcs.Userid and tuc.course_id=tcs.course_id "
                    q = q + "left join M_Testinfo as mti on mti.Test_type=tuc.Test_type "
                    q = q + "where tcs.userid=" + CandidateID + " and tcs.course_id=" + cid
                    Try
                        If objconn.connect() Then
                            cda = New SqlDataAdapter(q, objconn.MyConnection)
                            cds = New DataSet
                            cda.Fill(cds)
                            If cds.Tables(0).Rows(0)("Name").ToString().Length > 0 And cds.Tables(0).Rows(0)("EmailID").ToString().Length > 0 Then
                                objMailfunction = New CommonFunction()
                                strMessage = objMailfunction.ReadFile(Server.MapPath(ConfigurationSettings.AppSettings("ReminderMail")))
                                strMessage = strMessage.Replace("#Name#", cds.Tables(0).Rows(0)("Name").ToString())
                                strMessage = strMessage.Replace("#ClassName#", cds.Tables(0).Rows(0)("ClassName").ToString())
                                strMessage = strMessage.Replace("#sub#", strCourseName)
                                strMessage = strMessage.Replace("#date#", cds.Tables(0).Rows(0)("ExamDate").ToString())
                                strMessage = strMessage.Replace("#Marks#", cds.Tables(0).Rows(0)("TotalMarks").ToString())
                                strMessage = strMessage.Replace("#UserId#", cds.Tables(0).Rows(0)("LoginName").ToString())
                                strMessage = strMessage.Replace("#Password#", cds.Tables(0).Rows(0)("Password").ToString())
                                strMessage = strMessage.Replace("#TeacherName#", cds.Tables(0).Rows(0)("Teacher").ToString())
                                strMessage = strMessage.Replace("#links#", "http://adiinternals.usindia.com:8094/StudentLogin.aspx")
                                'objMailfunction.sendMail(cds.Tables(0).Rows(0)("EmailID").ToString(), "", strMessage, ConfigurationSettings.AppSettings("Subject_Reminder"))
                                objMailfunction.sendMail(cds.Tables(0).Rows(0)("EmailID").ToString(), "", strMessage, "Reminder : Request to appear an exam of (" + cds.Tables(0).Rows(0)("ClassName").ToString() + " - " + strCourseName + ")")
                                LblMsg.Visible = True
                                LblMsg.ForeColor = Color.Green
                                LblMsg.Text = Resources.Resource.CandStatus_mailsent
                            Else
                                LblMsg.Visible = True
                                LblMsg.ForeColor = Color.Red
                                LblMsg.Text = Resources.Resource.CandStatus_Mailerr
                            End If
                        End If
                    Catch ex As Exception
                        If log.IsDebugEnabled Then
                            log.Debug("Error :" & ex.ToString())
                        End If
                        Response.Redirect("error.aspx", False)
                    Finally
                        If objconn IsNot Nothing Then
                            objconn.disconnect()
                        End If
                    End Try
                End If

            Catch dividezero As DivideByZeroException
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & dividezero.ToString())
                End If

                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                Response.Redirect("error.aspx", False)

            Catch et As Threading.ThreadAbortException
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & et.ToString())
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If

            Catch sqlex As SqlException
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & sqlex.ToString())
                End If

                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                Response.Redirect("error.aspx", False)

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                Response.Redirect("error.aspx", False)

            Finally
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If

                objconn = Nothing
                dt = Nothing
                adap = Nothing
                strquery = Nothing
                strpath = Nothing
                CandidateID = Nothing
                name = Nothing
                strcenter = Nothing
                strcourse = Nothing
                strsubject = Nothing
            End Try
        End Sub

#End Region
        Public Function CheckQuestions(ByVal Cid As String) As Hashtable
            Dim tot_Marks As Integer = 0
            Dim item As DictionaryEntry
            Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
            objconn = New ConnectDb
            Dim cds As DataSet
            Dim cda As SqlDataAdapter
            Dim htSub As Hashtable ' for subject and marks 
            Dim htSubQues As New Hashtable ' for Each subject Qno for each type  key as sub id and value as arraylist
            Dim htFinal As New Hashtable ' Holds testtype as key and vale as Structre of weightage 
            ' The Structure All the Basic and intermediate values for TF/MC/BL
            ' We can Use The Function Generate Queries

            Try
                If objconn.connect() = True Then
                    cds = New DataSet
                    cda = New SqlDataAdapter("select total_marks from m_course where course_id=" & Cid, objconn.MyConnection)
                    cda.Fill(cds)
                    tot_Marks = CInt(cds.Tables(0).Rows(0).Item(0))
                End If
                htSub = GetMarksForSubject(Cid, tot_Marks)

                For Each item In htSub
                    Dim aa() As Integer = GetQTypeCountForSubject(CInt(item.Key), CInt(htSub(item.Key)), Cid)
                    htSubQues.Add(item.Key, aa)
                Next
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                cds = Nothing
                cda = Nothing
                strPathDb = Nothing
            End Try
            Return htSubQues

        End Function
        Public Function GetQTypeCountForSubject(ByVal test_type As Integer, ByVal totalMarks As Integer, ByVal Course_id As String) As Integer()

            'Dim arrLst As New ArrayList
            Dim tds As DataSet
            Dim tda As SqlDataAdapter
            Dim tf As Integer = 0
            Dim tfper As Double = 0

            Dim mc As Integer = 0
            Dim mcper As Double = 0

            Dim bl As Integer = 0
            Dim blper As Double = 0

            Dim bas As Integer = 0
            Dim basper As Double = 0

            Dim IMed As Integer = 0
            Dim IMedper As Double = 0

            Dim arr(3) As Integer
            Try
                '   If objconn.connect(strpathdb) = True Then
                tds = New DataSet
                tda = New SqlDataAdapter("select test_type,Sub_Weightage,single,multi_Choice,Blanks,Basic,InterMed from m_weightage where del_flag=0 and Course_id=" & Course_id & " and test_type=" & test_type, objconn.MyConnection)
                tda.Fill(tds)
                tf = CInt(tds.Tables(0).Rows(0).Item(2))
                tfper = tf / 100
                mc = CInt(tds.Tables(0).Rows(0).Item(3))
                mcper = mc / 100
                bl = CInt(tds.Tables(0).Rows(0).Item(4))
                blper = bl / 100
                bas = CInt(tds.Tables(0).Rows(0).Item(5))
                basper = bas / 100

                tf = Math.Abs(totalMarks * tfper)
                mc = Math.Abs(totalMarks * mcper)
                bl = Math.Abs(totalMarks * blper)

                arr(0) = tf
                arr(1) = mc
                arr(2) = bl

                If (tf + mc + bl) <> totalMarks Then
                    While ((tf + mc + bl) <> totalMarks)
                        If ((tf + mc + bl) > totalMarks) Then
                            arr = AdjustQTypesMax(arr)
                        ElseIf (tf + mc + bl) < totalMarks Then
                            arr = AdjustQTypesLeast(arr)
                        End If
                        tf = arr(0)
                        mc = arr(1)
                        bl = arr(2)
                    End While
                End If
                Return arr

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                tds = Nothing
                tda = Nothing
            End Try
        End Function
        Public Function AdjustQTypesMax(ByVal ar As Integer()) As Integer()
            Dim max As Integer = 0

            Try
                For i As Integer = 0 To ar.Length - 1
                    If (i = 0) Then

                        max = i

                    Else
                        If ar(i) > max Then
                            max = i
                        End If
                    End If
                Next

                ar(max) = ar(max) - 1
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                max = Nothing
            End Try
            Return ar
        End Function
        Public Function AdjustQTypesLeast(ByVal ar As Integer()) As Integer()
            Dim least As Integer = 0
            Try
                For i As Integer = 0 To ar.Length - 1
                    If (i = 0) Then
                        least = i

                    Else
                        If ar(i) < least Then
                            least = i
                        End If
                    End If
                Next

                ar(least) = ar(least) + 1
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                least = Nothing
            End Try
            Return ar
        End Function
        Public Function GetMarksForSubject(ByVal Course_id As String, ByVal TotalMarks As Integer) As Hashtable
            Dim ht As New Hashtable
            Dim sds As DataSet
            Dim sda As SqlDataAdapter
            Dim subjects As New Hashtable
            Dim subMarks As New Hashtable
            Dim item As DictionaryEntry
            Try
                sds = New DataSet
                sda = New SqlDataAdapter("select test_type,Sub_Weightage from m_weightage where del_flag=0 and Course_id=" & Course_id, objconn.MyConnection)
                sda.Fill(sds)
                For i As Integer = 0 To sds.Tables(0).Rows.Count - 1
                    subjects.Add(sds.Tables(0).Rows(i).Item(0).ToString, sds.Tables(0).Rows(i).Item(1).ToString)
                Next

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                sds = Nothing
                sda = Nothing
            End Try

            For Each item In subjects
                Dim sper As Integer = item.Value
                Dim multiple As Double = sper / 100
                Dim QforSub As Integer = 0

                QforSub = Math.Abs(TotalMarks * (multiple))
                subMarks.Add(item.Key, QforSub)
            Next
            ' Get Least Questions , ID and total to adjust later
            Dim sumtotal As Integer = GetSumTotalSubjects(subMarks)
            Dim leastID As Integer = GetLeastQuesSubjectID(subMarks)
            If (TotalMarks - sumtotal > 0) Then
                While TotalMarks - sumtotal <> 0
                    Dim lval As Integer = CInt(subMarks(leastID.ToString))
                    lval = lval + 1
                    subMarks(leastID.ToString) = lval
                    sumtotal = GetSumTotalSubjects(subMarks)
                    leastID = GetLeastQuesSubjectID(subMarks)
                End While
            End If
            Return subMarks
        End Function
        Public Function GetSumTotalSubjects(ByVal submarks As Hashtable) As Integer
            Dim sumtotal As Integer = 0
            Dim item As DictionaryEntry
            Dim leastQ As Integer = 0
            Dim leastID As Integer = 0

            Try
                For Each item In submarks
                    If (sumtotal = 0) Then
                        leastQ = CInt(submarks(item.Key))
                        leastID = item.Key
                    Else
                        If (leastQ > CInt(submarks(item.Key))) Then
                            leastQ = CInt(submarks(item.Key))
                            leastID = item.Key
                        End If
                    End If
                    sumtotal = sumtotal + CInt(submarks(item.Key))
                Next
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                item = Nothing
                leastID = Nothing
                leastQ = Nothing
            End Try
            Return sumtotal
        End Function
        Public Function GetLeastQuesSubjectID(ByVal submarks As Hashtable) As Integer
            Dim sumtotal As Integer = 0
            Dim item As DictionaryEntry
            Dim leastQ As Integer = 0
            Dim leastID As Integer = 0

            Try
                For Each item In submarks
                    If (sumtotal = 0) Then
                        leastQ = CInt(submarks(item.Key))
                        leastID = item.Key
                    Else
                        If (leastQ > CInt(submarks(item.Key))) Then
                            leastQ = CInt(submarks(item.Key))
                            leastID = item.Key
                        End If
                    End If
                    sumtotal = sumtotal + CInt(submarks(item.Key))
                Next
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                leastQ = Nothing
                item = Nothing
                sumtotal = Nothing
            End Try
            Return leastID
        End Function

        '===============================================START PDF CODE ===================================================================
#Region "CheckForMarksheetPwd"
        'Added by :: Saraswati Patel
        'For provide Password protection in pdf file 
        Public Function CheckForMarksheetPwd() As String
            Dim objconn As New ConnectDb
            Dim strSql As String
            Dim objCommand As SqlCommand
            Dim objDataReader As SqlDataReader
            Dim flagValue As String
            Try
                'strPathDb = ConfigurationManager.AppSettings("PathDb")
                If objconn.connect() Then
                    strSql = "SELECT value FROM m_system_settings Where key1='CandidateStatus' and key2='MarksheetPwd'"
                    objCommand = New SqlCommand(strSql, objconn.MyConnection)
                    objDataReader = objCommand.ExecuteReader
                    If objDataReader.Read() Then
                        flagValue = objDataReader.Item("value")
                    End If
                    objDataReader.Close()
                    If Not objconn.MyConnection Is Nothing Then
                        If objconn.MyConnection.State = ConnectionState.Open Then
                            objconn.disconnect()
                        End If
                    End If
                End If

                Return flagValue
            Catch ex As Exception
                If Not objconn.MyConnection Is Nothing Then
                    If objconn.MyConnection.State = ConnectionState.Open Then
                        objconn.disconnect()
                    End If
                End If
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx?err=" & ex.Message, False)
            Finally
                If Not objconn.MyConnection Is Nothing Then
                    If objconn.MyConnection.State = ConnectionState.Open Then
                        objconn.disconnect()
                    End If
                End If
                objconn = Nothing
                objDataReader = Nothing
                objCommand = Nothing

            End Try

        End Function
#End Region
#Region "makepdffile"
        'Added by :: Saraswati Patel
        'For making file pdf file with password protection
        <System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Assert, Unrestricted:=True)>
        Public Function makepdffile(ByVal strFileName As String, ByVal Rollno As String, ByVal Centername As String, ByVal CourseName As String, ByVal studentName As String, ByVal appdate As String, ByVal subj As String, ByVal MaxMarks As String, ByVal ObtainMarks As String, ByVal Remarks As String, ByVal grade As String, ByVal totmarks As String)
            Try
                log.Debug("File name:" + strFileName)
                log.Info("File name:" + strFileName)


                Dim doc As PDDocument
                log.Debug("Being initialize document")
                doc = New PDDocument()
                log.Debug("load Error document initialization fails")
                doc = PDDocument.load(strFileName)
                log.Debug("loading successful")

                log.Debug("File name:" + strFileName)
                Dim blankPage As PDPage = CType(doc.getDocumentCatalog().getAllPages().get(0), PDPage)
                Dim contentStream As New PDPageContentStream(doc, blankPage, True, True) '(doc, blankPage );

                log.Info("contentStream in makepdffile Line no:2068")
                writeOnPage(contentStream, Rollno, 200, 537)
                writeOnPage(contentStream, studentName, 200, 505)
                writeOnPage(contentStream, Centername, 200, 475)
                writeOnPage(contentStream, CourseName, 200, 444)
                writeOnPage(contentStream, appdate, 100, 78)
                log.Info("End Write on page")
                Dim result As String = subj
                Dim int_values(,) As String = New String(,) {{"Subject", "Maximum Marks", "Marks Obtained", "Remarks"}, {result, MaxMarks, ObtainMarks, Remarks}}
                log.Info("Int_values")
                Dim total_values(,) As String = New String(,) {{"Grade :" & grade & "                                                                                                                     Percentage :" & totmarks}}
                log.Info("total_values")
                log.Info("Calling Drwa table function")
                Dim val As String = drawTable(blankPage, contentStream, 430, 10, int_values, total_values)
                'Dim val As String = "110_30"
                Dim vald() As String = val.Split("_")
                contentStream.close()
                If System.IO.File.Exists(Server.MapPath("Mark sheet Template\Marksheets.pdf")) Then
                    System.IO.File.Delete(Server.MapPath("Mark sheet Template\Marksheets.pdf"))
                End If
                doc.save(Server.MapPath("Mark sheet Template\Marksheets.pdf"))

                'Dim Password As String = CheckForMarksheetPwd()
                ' PasswordProtectPDF(Server.MapPath("Mark sheet Template\Marksheets.pdf"), Password)

                doc.close()
                Dim lfile As System.IO.FileInfo = New System.IO.FileInfo(Server.MapPath("Mark sheet Template\Marksheets.pdf"))

                If (lfile.Exists) Then
                    Response.Clear()
                    Response.Charset = ""
                    Response.AppendHeader("Content-Type", "application/pdf")
                    Response.AppendHeader("Content-Length", lfile.Length.ToString())
                    Response.AppendHeader("content-disposition", "attachment; filename=" + lfile.Name)
                    Response.WriteFile(lfile.FullName)
                    Response.Cache.SetExpires(Date.Now())
                    Response.AddHeader("Cache-Control", "no-cache")
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If

            End Try
        End Function
        'Added by :: Saraswati Patel
        'For provide Password protection in pdf file 
        Private Sub PasswordProtectPDF(ByVal strPDFPath As String, ByVal strPassword As String)
            Try
                Dim input As Stream = New FileStream(strPDFPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Dim btSourceFile() As Byte = StreamToByteArray(input)
                input.Close()
                Dim output As Stream = New FileStream(strPDFPath, FileMode.Create, FileAccess.Write, FileShare.None)
                Dim reader As iTextSharp.text.pdf.PdfReader = New iTextSharp.text.pdf.PdfReader(btSourceFile)
                reader.Close()
                PdfEncryptor.Encrypt(reader, output, True, strPassword, strPassword, PdfWriter.ALLOW_PRINTING)
            Catch ex As Exception

            End Try
        End Sub
        'Added by :: Saraswati Patel
        'For provide Password protection in pdf file 
        Private Shared Function StreamToByteArray(ByVal inputStream As Stream) As Byte()
            If Not inputStream.CanRead Then
                Throw New ArgumentException
            End If
            If inputStream.CanSeek Then
                inputStream.Seek(0, SeekOrigin.Begin)
            End If
            Dim output() As Byte = New Byte((inputStream.Length) - 1) {}
            Dim bytesRead As Integer = inputStream.Read(output, 0, output.Length)
            Debug.Assert((bytesRead = output.Length), "Bytes read from stream matches stream length")
            Return output
        End Function
        'Added by :: Saraswati Patel
        'For provide Password protection in pdf file 
        Private Sub writeOnPage(ByVal content As PDPageContentStream, ByVal text As String, ByVal x As Integer, ByVal y As Integer)
            content.beginText()
            content.setFont(PDType1Font.HELVETICA_BOLD, 11)
            content.moveTextPositionByAmount(x, y)
            content.drawString(text)
            content.endText()
        End Sub
        'Added by :: Saraswati Patel
        'For provide Password protection in pdf file 
        Public Shared Function drawTable(ByVal page As PDPage, ByVal contentStream As PDPageContentStream, ByVal y As Single, ByVal margin As Single, ByVal content(,) As String, ByVal total(,) As String) As String
            Dim log As log4net.ILog = log4net.LogManager.GetLogger("CandStatus")
            Dim s As Integer
            Dim rows As Integer
            Dim cols As Integer = 4
            Dim rowHeight As Single = 18.0F
            Dim tableWidth As Single = page.findMediaBox().getWidth() - (4 * margin)

            Dim tableHeight As Single
            Dim colWidth As Single = (tableWidth / CSng(cols))
            Dim cellMargin As Single = 1.0F

            'now add the text
            contentStream.setFont(PDType1Font.HELVETICA_BOLD, 11)
            log.Info("contentStream.setFont:")
            Dim textx As Single = margin + cellMargin + 10
            Dim texty As Single = y - 15
            Dim cn As Single
            Dim ie As Integer
            Dim maxlength As Integer = 0
            Dim te(1) As String
            log.Info("te(1):" + te(1))
            Try


                For i As Integer = 0 To 1
                    For j As Integer = 0 To 3

                        If i = 1 And j = 0 Then
                            Dim re() As String = content(i, j).Split(",")

                            'modified by :: Saraswati Patel
                            'For display data in pdf
                            s = re.Length
                            rows = s + 2
                            cn = texty + 5
                            For ifd As Integer = 0 To re.Length - 1
                                Dim text As String = re(ifd)
                                If text.Length > 21 Then

                                    If Not text.IndexOf(" ", 15) = -1 Then
                                        te(0) = text.Substring(0, text.IndexOf(" ", 15))
                                        te(1) = text.Substring(text.IndexOf(" ", 15))

                                    ElseIf Not text.IndexOf(" ", 14) = -1 Then
                                        te(0) = text.Substring(0, text.IndexOf(" ", 14))
                                        te(1) = text.Substring(text.IndexOf(" ", 14))
                                    ElseIf Not text.IndexOf(" ", 13) = -1 Then
                                        te(0) = text.Substring(0, text.IndexOf(" ", 13))
                                        te(1) = text.Substring(text.IndexOf(" ", 13))
                                    ElseIf Not text.IndexOf(" ", 12) = -1 Then
                                        te(0) = text.Substring(0, text.IndexOf(" ", 12))
                                        te(1) = text.Substring(text.IndexOf(" ", 12))
                                    ElseIf Not text.IndexOf(" ", 11) = -1 Then
                                        te(0) = text.Substring(0, text.IndexOf(" ", 11))
                                        te(1) = text.Substring(text.IndexOf(" ", 11))
                                    End If
                                    rows = rows + te.Length - 1
                                    Dim no As String
                                    For ss As Integer = 0 To te.Length - 1
                                        contentStream.beginText()
                                        contentStream.moveTextPositionByAmount(textx - 4, cn)
                                        If ss = 0 Then
                                            no = (ifd + 1).ToString + "."
                                        Else
                                            no = "  "

                                        End If
                                        contentStream.drawString(no + te(ss))
                                        contentStream.endText()
                                        cn -= rowHeight
                                    Next
                                Else
                                    contentStream.beginText()
                                    contentStream.moveTextPositionByAmount(textx, cn)
                                    contentStream.drawString((ifd + 1).ToString + "." + text)
                                    colWidth = Math.Round((maxlength * 6.6), 0, MidpointRounding.AwayFromZero)
                                    contentStream.endText()
                                    cn -= rowHeight
                                End If
                                tableHeight = rowHeight * rows

                                If (text.Length > maxlength) Then
                                    maxlength = 21
                                End If
                                If text.Length <= maxlength Then
                                    maxlength = 21
                                End If
                                colWidth = Math.Round((maxlength * 6.6), 0, MidpointRounding.AwayFromZero) - 9
                            Next
                            textx += colWidth + 40

                        Else
                            Dim text As String = content(i, j)
                            contentStream.beginText()
                            contentStream.moveTextPositionByAmount(textx - 4, texty)
                            contentStream.drawString(text)
                            contentStream.endText()
                            textx += colWidth + 10
                        End If
                    Next j
                    texty -= rowHeight
                    textx = margin + cellMargin
                Next i
                log.Info("End of Write data")
                'draw the rows
                Dim nexty As Single = y
                Dim dcre As Integer
                For i As Integer = 0 To rows
                    If i <= 1 Then
                        contentStream.drawLine(margin, nexty, margin + tableWidth, nexty)
                        nexty -= rowHeight
                    ElseIf i >= rows - 1 Then
                        nexty -= rowHeight * (i - 2)
                        contentStream.drawLine(margin, nexty, margin + tableWidth, nexty)
                        If i = rows - 1 Then
                            dcre = nexty
                            nexty += rowHeight * (i - 2)
                        End If

                    End If
                Next i
                log.Info("End of Rows")
                'draw the columns
                Dim nextx As Single = margin
                For i As Integer = 0 To cols
                    If i = 0 Or i = cols Then
                        contentStream.drawLine(nextx, y, nextx, y - tableHeight)
                    End If
                    contentStream.drawLine(nextx, y, nextx, dcre)
                    nextx += colWidth
                Next i
                log.Info("End of Columns")

                Dim grand_total As String = total(0, 0)
                contentStream.beginText()
                contentStream.moveTextPositionByAmount(textx, dcre - 13)
                contentStream.drawString(grand_total)
                contentStream.endText()

                Return textx & "_" & cn
            Catch ex As Exception

                If log.IsDebugEnabled Then
                    log.Debug("Error:" + ex.ToString())
                End If

            End Try
        End Function

#End Region
        '===============================================END PDF CODE ===================================================================



#Region "DataGrid ItemBound Event"
        'Desc: This is datagrid itembound event
        'By: Jatin Gangajaliya, 2011/3/10

        Protected Sub DGReport_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGReport.ItemDataBound
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        End Sub
#End Region

        'Added by Pranit on 02/12/2019
        Sub Selection_Change(sender As Object, e As EventArgs)
            Try
                DGReport.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
                BindGrid()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try '
        End Sub


#Region "Clear Button"
        'Desc: This is button event for clearing all controls
        'By: Jatin Gangajaliya,2011/3/9

        Protected Sub btnclear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnclear.Click
            Try
                TxtUserName.Text = String.Empty
                'TxtLoginname.Text = String.Empty
                'ddlTestName.SelectedIndex = 0
                'TxtScoreFrom.Text = String.Empty
                'TxtScoreTo.Text = String.Empty

                'Disable for Demo
                'imgbtnExport.Enabled = False
                'imgbtnExport.Visible = False


                dblCenter.SelectedIndex = 0
                dblCourse.Items.Clear()
                'dblCourse.Items.Insert(0, "---- Select ----")
                FillListOfCourse_New()
                ddlStatus.SelectedIndex = 0

                'ddlTestName.Items.Clear()
                'ddlTestName.Items.Insert(0, "ALL")

                ddlgrade.SelectedIndex = 0

                TxtFrom.Value = String.Empty
                TxtTo.Value = String.Empty
                txtAppFromDate.Value = String.Empty
                TxtAppToDate.Value = String.Empty
                dblCenter.Focus()
                DGReport.Visible = False
                LblRecCnt.Text = String.Empty
                LblMsg.Text = String.Empty
                gridDiv.Visible = False

                ViewState.Remove("selval")
                ViewState.Remove("pageNo")
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)

            End Try
        End Sub
#End Region

        Protected Sub dblCenter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dblCenter.SelectedIndexChanged
            'FillListOfCourse()
        End Sub
        Public Sub FillListOfCourse()
            Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim datareader As SqlDataReader
            Dim data1 As SqlDataReader
            dblCourse.Items.Clear()
            dblCourse.Items.Insert(0, "---- Select ----")
            dblCourse.SelectedIndex = 0
            Try
                If objconn.connect() = True Then
                    '********************************************************************Raj
                    'Code added by Monal Shah
                    'Purpose: To fill the combo box for Course
                    '********************************************************************
                    'query = "select M_Course.Course_ID,M_Course.Course_Name,M_Course.Description,m_centers.Center_ID from M_Course,m_centers,m_uesr_info,t_user_course where m_centers.Center_ID='" & ddlCenters.SelectedItem.Value & "' and m_uesr_info.Center_ID=m_centers.Center_ID and m_user_info.user_id=t_user_course.user_id and t_user_course.course_id=M_Course.Course_ID"
                    If Session.Item("userid") = Nothing Or Session.Item("userid") = 0 Or Request.QueryString("userid") <> Nothing Then
                        query = "select Distinct M_Course.Course_ID,M_Course.Course_Name,M_Course.Description from M_Course,m_centers,t_Center_course where t_Center_Course.center_id=m_Centers.Center_ID and t_Center_Course.course_id=M_Course.Course_ID and m_centers.Center_id='" & dblCenter.SelectedItem.Value & "' order by M_Course.Course_Name"
                        myCommand = New SqlCommand(query, objconn.MyConnection)
                        datareader = myCommand.ExecuteReader()
                        While datareader.Read()

                            Dim lstItm As New ListItem()
                            lstItm.Enabled = True

                            lstItm.Text = datareader.Item(1)
                            lstItm.Value = datareader.Item(0)

                            'If result <> "" Then
                            '    If (ids(cnt) = datareader.Item(0)) Then
                            '        lstItm.Selected = True
                            '        cnt = cnt + 1
                            '    End If
                            'End If

                            ' lstItm.Selected = True
                            dblCourse.Items.Add(lstItm)
                        End While
                        datareader.Close()
                    End If
                    If Session.Item("userid") <> Nothing And Session.Item("check") = True Then
                        query = "select Distinct M_Course.Course_ID,M_Course.Course_Name,M_Course.Description from M_Course,m_centers,t_Center_course where t_Center_Course.center_id=m_Centers.Center_ID and t_Center_Course.course_id=M_Course.Course_ID and m_centers.Center_id='" & dblCenter.SelectedItem.Value & "'"
                        myCommand = New SqlCommand(query, objconn.MyConnection)
                        datareader = myCommand.ExecuteReader()
                        While datareader.Read()

                            Dim lstItm As New ListItem()
                            lstItm.Enabled = True

                            lstItm.Text = datareader.Item(1)
                            lstItm.Value = datareader.Item(0)

                            'If result <> "" Then
                            '    If (ids(cnt) = datareader.Item(0)) Then
                            '        lstItm.Selected = True
                            '        cnt = cnt + 1
                            '    End If
                            'End If

                            ' lstItm.Selected = True
                            dblCourse.Items.Add(lstItm)
                        End While
                        datareader.Close()
                    End If

                    Dim ids(10) As String
                    Dim cnt As Integer = 0
                    Dim result As String = ""
                    If Session.Item("userid") <> Nothing And Session.Item("check") <> True Or Session.Item("userid") <> 0 And Session.Item("check") <> True Then

                        Dim query1 As String = "Select Distinct M_Course.Course_id,m_Course.Course_name,M_Course.Description from m_user_info,t_user_course,t_Center_Course,M_Course  where m_user_info.userid='" & Session.Item("userid") & "' and t_user_course.Course_id=t_Center_Course.Course_id and m_user_info.userid=t_user_course.User_id and m_Course.Course_id=t_user_course.Course_id"
                        myCommand = New SqlCommand(query1, objconn.MyConnection)
                        datareader = myCommand.ExecuteReader()
                        While datareader.Read()


                            Dim lstItm As New ListItem()
                            lstItm.Enabled = True

                            lstItm.Text = datareader.Item(1)
                            lstItm.Value = datareader.Item(0)


                            'If (ids(cnt) = data1.Item(0)) Then
                            '    lstItm.Selected = True
                            '    cnt = cnt + 1
                            'End If

                            lstItm.Selected = True
                            dblCourse.Items.Add(lstItm)
                        End While

                    End If
                    datareader.Close()
                    If objconn IsNot Nothing Then
                        objconn.disconnect()
                    End If

                    '   GetCoursesFromDB("7")
                End If
                If dblCourse.SelectedIndex = 0 Then
                    FillSubjectCombo()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                datareader = Nothing
                data1 = Nothing
            End Try



        End Sub
        Public Sub FillListOfCourse_New()
            Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
            Dim query As String = ""
            Dim datareader As SqlDataReader
            Dim data1 As SqlDataReader
            dblCourse.Items.Clear()
            dblCourse.Items.Insert(0, "---- Select ----")
            dblCourse.SelectedIndex = 0
            Try
                If objconn.connect() = True Then
                    '********************************************************************Raj
                    'Code added by Monal Shah
                    'Purpose: To fill the combo box for Course
                    '********************************************************************
                    'query = "select M_Course.Course_ID,M_Course.Course_Name,M_Course.Description,m_centers.Center_ID from M_Course,m_centers,m_uesr_info,t_user_course where m_centers.Center_ID='" & ddlCenters.SelectedItem.Value & "' and m_uesr_info.Center_ID=m_centers.Center_ID and m_user_info.user_id=t_user_course.user_id and t_user_course.course_id=M_Course.Course_ID"
                    ''If Session.Item("userid") = Nothing Or Session.Item("userid") = 0 Or Request.QueryString("userid") <> Nothing Then
                    ''    query = "select Distinct M_Course.Course_ID,M_Course.Course_Name,M_Course.Description from M_Course,m_centers,t_Center_course where t_Center_Course.center_id=m_Centers.Center_ID and t_Center_Course.course_id=M_Course.Course_ID and m_centers.Center_id='" & dblCenter.SelectedItem.Value & "' order by M_Course.Course_Name"
                    ''    myCommand = New SqlCommand(query, objconn.MyConnection)
                    ''    datareader = myCommand.ExecuteReader()
                    ''    While datareader.Read()

                    ''        Dim lstItm As New ListItem()
                    ''        lstItm.Enabled = True

                    ''        lstItm.Text = datareader.Item(1)
                    ''        lstItm.Value = datareader.Item(0)

                    ''        'If result <> "" Then
                    ''        '    If (ids(cnt) = datareader.Item(0)) Then
                    ''        '        lstItm.Selected = True
                    ''        '        cnt = cnt + 1
                    ''        '    End If
                    ''        'End If

                    ''        ' lstItm.Selected = True
                    ''        dblCourse.Items.Add(lstItm)
                    ''    End While
                    ''    datareader.Close()
                    ''End If
                    '  If Session.Item("userid") <> Nothing And Session.Item("check") = True Then
                    'query = "select Distinct M_Course.Course_ID,M_Course.Course_Name,M_Course.Description from M_Course,m_centers,t_Center_course where t_Center_Course.center_id=m_Centers.Center_ID and t_Center_Course.course_id=M_Course.Course_ID and m_centers.Center_id='" & dblCenter.SelectedItem.Value & "'"
                    query = "select Course_ID,Course_name from M_Course where del_flag=0 order by Course_name"

                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()
                    While datareader.Read()

                        Dim lstItm As New ListItem()
                        lstItm.Enabled = True

                        lstItm.Text = datareader.Item(1)
                        lstItm.Value = datareader.Item(0)

                        'If result <> "" Then
                        '    If (ids(cnt) = datareader.Item(0)) Then
                        '        lstItm.Selected = True
                        '        cnt = cnt + 1
                        '    End If
                        'End If

                        ' lstItm.Selected = True
                        dblCourse.Items.Add(lstItm)
                    End While
                    datareader.Close()
                    'End If

                    'Dim ids(10) As String
                    'Dim cnt As Integer = 0
                    'Dim result As String = ""
                    'If Session.Item("userid") <> Nothing And Session.Item("check") <> True Or Session.Item("userid") <> 0 And Session.Item("check") <> True Then

                    '    Dim query1 As String = "Select Distinct M_Course.Course_id,m_Course.Course_name,M_Course.Description from m_user_info,t_user_course,t_Center_Course,M_Course  where m_user_info.userid='" & Session.Item("userid") & "' and t_user_course.Course_id=t_Center_Course.Course_id and m_user_info.userid=t_user_course.User_id and m_Course.Course_id=t_user_course.Course_id"
                    '    myCommand = New SqlCommand(query1, objconn.MyConnection)
                    '    datareader = myCommand.ExecuteReader()
                    '    While datareader.Read()


                    '        Dim lstItm As New ListItem()
                    '        lstItm.Enabled = True

                    '        lstItm.Text = datareader.Item(1)
                    '        lstItm.Value = datareader.Item(0)


                    '        'If (ids(cnt) = data1.Item(0)) Then
                    '        '    lstItm.Selected = True
                    '        '    cnt = cnt + 1
                    '        'End If

                    '        lstItm.Selected = True
                    '        dblCourse.Items.Add(lstItm)
                    '    End While

                    'End If
                    'datareader.Close()
                    If objconn IsNot Nothing Then
                        objconn.disconnect()
                    End If

                    '   GetCoursesFromDB("7")
                End If
                If dblCourse.SelectedIndex = 0 Then
                    ' FillSubjectCombo()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                datareader = Nothing
                data1 = Nothing
            End Try



        End Sub

        Protected Sub dblCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dblCourse.SelectedIndexChanged
            'FillSubjectCombo()
        End Sub

#Region "FillSubjectCombo"
        'created by: Aalok Parikh
        'Description: Fills the Subject Dropdownlist with subjects which are comes under currently selected course in Course Dropdownlist

        Private Sub FillSubjectCombo()
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim dtData As DataTable
            'Dim strPathDb As String

            Try

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                If objconn.connect() Then
                    Dim rows As DataRow

                    If dblCourse.SelectedItem.Text.ToString() <> "---- Select ----" Then
                        sqlstr = "SELECT distinct test_type,test_name FROM m_testinfo WHERE del_flag ='0' and course_id = " & dblCourse.SelectedValue.ToString()
                        'ddlTestName.Items.Clear()
                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        myDataReader = myCommand.ExecuteReader()
                        dtData = New DataTable
                        dtData.Columns.Add(New DataColumn("test_type", GetType(String)))
                        dtData.Columns.Add(New DataColumn("test_name", GetType(String)))
                        While myDataReader.Read
                            rows = dtData.NewRow
                            rows(0) = myDataReader.Item("test_type")
                            rows(1) = myDataReader.Item("test_name")
                            dtData.Rows.Add(rows)
                        End While
                        'ddlTestName.DataSource = dtData
                        'ddlTestName.DataValueField = "test_type"
                        'ddlTestName.DataTextField = "test_name"
                        'ddlTestName.DataBind()
                        'ddlTestName.Items.Insert(0, "ALL")
                        'ddlResult.Items.Insert(0, "ALL")
                        'ddlResult.Items.Insert(1, "Pass")
                        'ddlResult.Items.Insert(2, "Reject")

                        'ddlStatus.Items.Insert(0, "ALL")
                        'ddlStatus.Items.Insert(1, "Assigned")
                        'ddlStatus.Items.Insert(2, "Appeared")

                        myCommand.Dispose()
                        myDataReader.Close()
                        If objconn IsNot Nothing Then
                            objconn.disconnect()
                        End If
                    End If
                    If dblCourse.SelectedItem.Text.ToString() = "---- Select ----" Then
                        'ddlTestName.Items.Clear()
                        'ddlTestName.Items.Insert(0, "ALL")
                    End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If

            Finally
                objconn = Nothing
                sqlstr = Nothing
                strPathDb = Nothing


            End Try

        End Sub
#End Region
        'Private Sub btnclear_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnclear.Command

        'End Sub


        '********************************************************************
        'Code added by Indravadan Vasava
        'Method-Name: btnOkay_Click
        'Purpose: To Save the new date enter by the User for Assign Exam
        'Date : 2011/04/27
        '********************************************************************
        Private Sub btnOkay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkay.Click
            Dim strpath As String = ConfigurationSettings.AppSettings("PathDb")
            Dim CandidateID As String = Session("cdUID").ToString
            Dim cid As String = Session("cdCID").ToString
            Dim ccmd As SqlCommand
            If txtnewdate.Value = "" Then
                txtnewdate.Focus()
                Exit Sub
            End If
            Dim q As String = "update T_candidate_status set written_test_date ='" & txtnewdate.Value & " 00:00:00'   where userid=" & CandidateID & " and Course_id=" & cid
            Try
                If objconn.connect() Then
                    ccmd = New SqlCommand(q, objconn.MyConnection)
                    ccmd.ExecuteNonQuery()
                    txtnewdate.Value = ""
                    BindGrid()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                ccmd = Nothing
                'objconn.disconnect()
            End Try
            Session.Remove("cdCID")
            Session.Remove("cdUID")
            ModalPopupExtender1.Hide()
        End Sub

        Private Sub ModalPopupExtender1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles ModalPopupExtender1.Unload


        End Sub

        '********************************************************************
        'Code added by Indravadan Vasava
        'Method-Name: btnPopUpCancel_Click
        'Purpose: To decline Popup Changes
        'Date : 2011/04/27
        '********************************************************************
        Private Sub btnPopUpCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPopUpCancel.Click
            Try
                txtnewdate.Value = ""
                If Not Session("cdCID") Is Nothing Then
                    Session.Remove("cdCID")
                End If
                If Not Session("cdUID") Is Nothing Then
                    Session.Remove("cdUID")
                End If
                ModalPopupExtender1.Hide()
                BindGrid()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally

            End Try
        End Sub

        Private Sub CreateDataSetExport()
            Dim OConn As New ConnectDb
            Dim OAdap As SqlDataAdapter
            Dim StrSql As String
            Dim strbr As New StringBuilder
            Dim blnCheck As Boolean
            Dim dicSearch As New Dictionary(Of String, String)()
            Dim objCommon As CommonFunction
            objCommon = New CommonFunction()

            Try
                '--------------------------Start--------------------------------

                strbr.Append(" select * from(select tcs.userid, ")
                strbr.Append(" (mui.SurName+' '+mui.Name +' '+isnull(mui.Middlename,''))as username, ")
                strbr.Append(" tcs.course_id, ")
                strbr.Append(" tcs.loginname as LoginName, ")
                strbr.Append(" tcs.pwd as 'Password', ")

                'strbr.Append(" tcs.written_test_date as writtentestdate, ")
                ' convert(varchar(10),  tcs.written_test_date ,103)  as writtentestdate
                strbr.Append(" convert(varchar(10),  tcs.written_test_date ,103)  as writtentestdate, ")
                strbr.Append(" tcs.written_test_appear_date as appearancedate, ")
                strbr.Append(" tcs.appearedflag as appearedflag, ")
                strbr.Append(" mc.course_name,mce.Center_Name, ")
                strbr.Append(" mui.Center_ID, ")
                strbr.Append(" tcs.total_marks, ")
                strbr.Append(" tcs.total_passmarks, ")
                strbr.Append(" isnull(obtained1.obtained_marks,0) as obtained_marks, ")
                strbr.Append(" (case  when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 75 then  'A+' ")
                strbr.Append(" when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >=60 then 'A' ")
                strbr.Append(" when isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 50 then 'B' ")
                strbr.Append(" WHEN (obtained1.obtained_marks*100)/tcs.total_marks is null then null else 'C' End ) as Grad ")
                strbr.Append(" ,(case WHEN obtained1.obtained_marks >= tcs.total_passmarks then 'Pass' ")
                strbr.Append(" WHEN obtained1.obtained_marks is null then null ")
                strbr.Append(" ELSE 'Fail' ")
                strbr.Append(" END) as Status,mmc.Main_Course_ID ")

                strbr.Append(" FROM T_Candidate_Status as tcs ")
                strbr.Append(" left join M_USER_INFO as mui ")
                strbr.Append(" on tcs.userid=mui.userid ")
                strbr.Append(" left join m_course as mc ")
                strbr.Append(" on mc.course_id=tcs.course_id ")
                strbr.Append(" Left Join ")
                strbr.Append(" (select sum(temp.obtained_marks) as obtained_marks,temp.course_id,temp.userid from (select ")
                strbr.Append(" ( ")
                strbr.Append(" case ")
                strbr.Append(" WHEN mq.Qn_Category_ID=3 then ")
                strbr.Append(" ( ")
                strbr.Append(" case ")
                strbr.Append(" WHEN mqa.sub_id=tro.sub_id then ")
                strbr.Append(" (Case ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" count(mqa.Correct_Opt_Id)")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" END) ")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" WHEN mq.Qn_Category_ID=2 then ")
                strbr.Append(" ( ")
                strbr.Append(" CASE  ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" count(mqa.Correct_Opt_Id)")
                strbr.Append(" ELSE 0")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" WHEN mq.Qn_Category_ID=1 then ")
                strbr.Append(" ( ")
                strbr.Append(" CASE  ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" SUM(mq.Total_Marks)")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" End ")
                strbr.Append(" ) as obtained_marks ")
                strbr.Append(" ,mc.course_id,mui.userid ")
                strbr.Append(" from m_question as mq ")
                strbr.Append(" left join M_Question_Answer as mqa ")
                strbr.Append(" on mqa.Qn_ID=mq.qnid and mqa.test_type=mq.test_type ")
                strbr.Append(" left join t_result as tr ")
                strbr.Append(" on tr.qno=mq.qnid ")
                strbr.Append(" AND tr.test_type=mq.test_type ")
                strbr.Append(" left join m_user_info as mui ")
                strbr.Append(" on mui.userid=tr.userid ")
                strbr.Append(" left join m_course as mc ")
                strbr.Append(" on mc.course_id=tr.course_id ")
                strbr.Append(" left join m_testinfo as mti ")
                strbr.Append(" on mti.test_type=tr.test_type ")
                strbr.Append(" left join t_result_option as tro ")
                strbr.Append(" on tro.result_id=tr.result_id ")
                strbr.Append(" and tr.test_type=mti.test_type ")
                strbr.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
                strbr.Append(" group by mq.total_marks,mc.course_id, ")
                strbr.Append(" mq.total_marks,mq.qnid,mqa.Sub_id, ")
                strbr.Append(" mqa.Correct_Opt_Id, tro.option_id, mq.Qn_Category_ID, tro.sub_id, mui.userid ")
                strbr.Append(" )temp ")
                strbr.Append(" group by temp.course_id,temp.userid ")
                strbr.Append(" )as obtained1 ")
                strbr.Append(" on obtained1.course_id=tcs.course_id ")
                strbr.Append(" and tcs.userid=obtained1.userid ")
                strbr.Append(" left join M_Centers as mce ")
                strbr.Append(" on mce.Center_ID=mui.Center_ID left join M_Main_Course as mmc ")
                strbr.Append(" on mmc.Main_Course_ID=mc.Main_Course_ID")
                strbr.Append(" )temp ")

                '----------------End----------------------------------

                'strQuery.Append(" order by candstatus.userid ")
                'If ddlResult.SelectedItem.Text = "Pass" Then
                '    dicSearch.Add(" CandStatus.written_test_remark", ddlResult.SelectedItem.Text)
                'ElseIf ddlResult.SelectedItem.Text = "Reject" Then
                '    dicSearch.Add(" (CandStatus.written_test_remark", ddlResult.SelectedItem.Text + "' OR (ResAttempted.Attempted IS NULL AND CandStatus.Written_Test_Appear_Date IS NOT NULL))")
                'End If

                If dblCenter.SelectedItem.Text <> "---- Select ----" Then
                    dicSearch.Add("temp.Center_ID", dblCenter.SelectedValue.ToString())
                    If dblCourse.SelectedItem.Text <> "---- Select ----" Then
                        dicSearch.Add("temp.Course_ID", dblCourse.SelectedValue.ToString())
                    End If
                End If

                If ddlStatus.SelectedItem.Text = "Appeared" Then
                    dicSearch.Add("temp.Appeared", "")
                ElseIf ddlStatus.SelectedItem.Text = "Assigned" Then
                    dicSearch.Add("temp.Assigned", "")
                End If


                If Not TxtUserName.Text.Trim() = "" Then
                    dicSearch.Add("temp.username", TxtUserName.Text.Trim())
                End If

                'If Not TxtLoginname.Text.Trim() = "" Then
                '    dicSearch.Add("UserInfo.loginname", TxtLoginname.Text.Trim())
                'End If

                'If ddlTestName.SelectedItem.Text <> "ALL" Then
                '    dicSearch.Add("m_testinfo.test_type", ddlTestName.SelectedValue.ToString())
                'End If

                If Not TxtFrom.Value.Trim() = "" And Not TxtTo.Value.Trim() = "" Then
                    dicSearch.Add("temp.writtentestdate Between '", TxtFrom.Value + "'  and  '" + TxtTo.Value + "'")
                ElseIf Not TxtFrom.Value.Trim() = "" Then
                    dicSearch.Add("temp.writtentestdate", TxtFrom.Value)
                End If

                Select Case ddlgrade.SelectedIndex
                    Case 0

                    Case 1
                        dicSearch.Add("grad = ", "A+")
                    Case 2
                        dicSearch.Add("grad = ", "A")
                    Case 3
                        dicSearch.Add("grad = ", "B")
                    Case 4
                        dicSearch.Add("grad = ", "C")
                End Select


                If Not txtAppFromDate.Value.Trim() = "" And Not TxtAppToDate.Value.Trim() = "" Then
                    dicSearch.Add("temp.appearancedate Between '", txtAppFromDate.Value + "'  and  '" + TxtAppToDate.Value + " 23:59:59'")
                ElseIf Not txtAppFromDate.Value.Trim() = "" Then
                    dicSearch.Add("temp.appearancedate", txtAppFromDate.Value)
                End If

                'If Not TxtScoreFrom.Text.Trim() = "" And Not TxtScoreTo.Text.Trim() = "" Then
                '    dicSearch.Add("ResScore.Score Between '", TxtScoreFrom.Text + "'  and  '" + TxtScoreTo.Text + "'")
                'ElseIf Not TxtScoreFrom.Text.Trim() = "" Then
                '    dicSearch.Add("ResScore.Score", TxtScoreFrom.Text)
                'End If

                '  strbr.Append(objCommon.GetSelectSearchQuery(dicSearch))
                strbr.Append(" where  temp.appearedflag = 0  order by temp.username ")
                Dim strq = strbr.ToString

                m_DS = New DataSet



                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If OConn.connect() Then
                    OAdap = New SqlDataAdapter(strq, OConn.MyConnection)
                    OAdap.Fill(m_DS, "TblResults")
                    Session("DS1") = m_DS
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                    OConn.disconnect()
                End If
                Throw ex
            Finally
                OConn.disconnect()
                OConn = Nothing
                StrSql = Nothing
                OAdap = Nothing
                strbr = Nothing

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
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

        'Disable for Demo
        'Protected Sub imgbtnExport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnExport.Click
        '    Getit()
        'End Sub

        Public Sub Getit()
            'Excel Objects
            Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
            Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
            Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
            Dim Sheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
            Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
            Dim objOpt As Object = System.Reflection.Missing.Value
            Dim my_DS As DataSet

            'Variable for Filling the Excel Object from Task manager
            Dim proc As System.Diagnostics.Process
            Dim strVer As String
            Dim intPID As Integer
            Dim iHandle As IntPtr
            Try


                'Constants
                Const xlEdgeLeft = 7
                Const xlEdgeTop = 8
                Const xlEdgeBottom = 9
                Const xlEdgeRight = 10

                'Create  Spreadsheet
                App = New Excel.Application
                iHandle = IntPtr.Zero
                'If CInt(strVer) > 9 Then
                'iHandle = New IntPtr(CType(App.Parent.Hwnd, Integer))
                'Else
                'iHandle = FindWindow(Nothing, App.Caption)
                'End If


                WorkBooks = DirectCast(App.Workbooks, Excel.Workbooks)
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim myWorkBook As Excel.Workbook = App.Workbooks.Open(Server.MapPath("ExcelImport\Candidate List.xls"), 0, False, 5, "", "", False, Excel.XlPlatform.xlWindows, "", True, False, 0, True)
                WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
                Dim sit As New Excel.Worksheet
                Dim sit1 As Excel.Worksheet = myWorkBook.Worksheets(1)
                sit1.Activate()
                sit1.Cells.Clear()



                ' CODE TO INSERT CANDIDATE DETAILS
                '*** Write Cell Border ***'  
                ' Set Main Column Headers

                With App.ActiveSheet.Range("B3:I4")
                    .MergeCells = True
                    .Interior.ColorIndex = 40
                    .Font.Bold = True
                    .Font.ColorIndex = 53
                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    .Cells.Value = "Candidate List"
                    .Font.Size = 15
                    .BORDERS(xlEdgeLeft).Weight = 2
                    .BORDERS(xlEdgeTop).Weight = 2
                    .BORDERS(xlEdgeBottom).Weight = 2
                    .BORDERS(xlEdgeRight).Weight = 2
                End With


                ' Set Column Headers

                App.ActiveSheet.Cells(5, 2).Value = "Sr No."
                App.ActiveSheet.Cells(5, 2).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(5, 2).Font.Bold = True
                App.ActiveSheet.Cells(5, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 2).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 2).WrapText = False
                App.ActiveSheet.Cells(5, 2).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 3).Value = "Candidate Name"
                App.ActiveSheet.Cells(5, 3).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 3).Font.ColorIndex = 3
                App.ActiveSheet.Cells(5, 3).Font.Bold = True
                App.ActiveSheet.Cells(5, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 3).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 3).WrapText = False
                App.ActiveSheet.Cells(5, 3).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 4).Value = "Center Name"
                App.ActiveSheet.Cells(5, 4).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(5, 4).Font.Bold = True
                App.ActiveSheet.Cells(5, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 4).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 4).WrapText = False
                App.ActiveSheet.Cells(5, 4).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 5).Value = "Course Name"
                App.ActiveSheet.Cells(5, 5).Interior.ColorIndex = 36
                '  App.ActiveSheet.Cells(5, 5).Font.ColorIndex = 3
                App.ActiveSheet.Cells(5, 5).Font.Bold = True
                App.ActiveSheet.Cells(5, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 5).ColumnWidth = 50
                App.ActiveSheet.Cells(5, 5).WrapText = False
                App.ActiveSheet.Cells(5, 5).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 6).Value = "Login Name"
                App.ActiveSheet.Cells(5, 6).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(5, 6).Font.Bold = True
                App.ActiveSheet.Cells(5, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 6).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 6).WrapText = False
                App.ActiveSheet.Cells(5, 6).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 7).Value = "Password"
                App.ActiveSheet.Cells(5, 7).Interior.ColorIndex = 36
                ' App.ActiveSheet.Cells(5, 7).Font.ColorIndex = 3
                App.ActiveSheet.Cells(5, 7).Font.Bold = True
                App.ActiveSheet.Cells(5, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 7).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 7).WrapText = False
                App.ActiveSheet.Cells(5, 7).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 8).Value = "Exam Assign Date"
                App.ActiveSheet.Cells(5, 8).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(5, 8).Font.Bold = True
                App.ActiveSheet.Cells(5, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 8).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 8).WrapText = False
                App.ActiveSheet.Cells(5, 8).Borders.Weight = 2

                App.ActiveSheet.Cells(5, 9).Value = "Maximum Marks"
                App.ActiveSheet.Cells(5, 9).Interior.ColorIndex = 36
                '  App.ActiveSheet.Cells(5, 9).Font.ColorIndex = 3
                App.ActiveSheet.Cells(5, 9).Font.Bold = True
                App.ActiveSheet.Cells(5, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(5, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(5, 9).ColumnWidth = 20
                App.ActiveSheet.Cells(5, 9).WrapText = False
                App.ActiveSheet.Cells(5, 9).Borders.Weight = 2

                Dim ht As New Hashtable
                ''ht.Add("Delhi", "1")
                ''ht.Add("Mumbai", "2")
                ''ht.Add("Calcutta", "3")
                ''ht.Add("Bangalore", "4")
                ''ht.Add("India", "777")


                CreateDataSetExport()
                my_DS = DirectCast(Session("DS1"), DataSet)

                Dim cn As New ConnectDb
                Dim strpath As String
                strpath = ConfigurationSettings.AppSettings("PathDb")
                'If cn.connect(strpath) Then



                '    Dim ds As New DataSet()


                '    Dim da As New SqlDataAdapter("select center_id,Center_name from m_centers where del_flg=0", cn.MyConnection)
                '    da.Fill(ds)
                '    ht.Clear()
                '    For a As Integer = 0 To ds.Tables(0).Rows.Count - 1
                '        ht.Add(ds.Tables(0).Rows(a).Item(1).ToString, ds.Tables(0).Rows(a).Item(0).ToString)
                '    Next
                'End If
                ''Dim keys(ht.Count) As String
                ''ht.Keys.CopyTo(keys, 0)
                Dim rownum As Integer = 6
                ' Set the data in columns
                For i As Integer = 0 To my_DS.Tables(0).Rows.Count - 1
                    App.ActiveSheet.Cells(rownum, 2).Value = i + 1
                    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 2).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 3).Value = my_DS.Tables(0).Rows(i).Item("username").ToString
                    App.ActiveSheet.Cells(rownum, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 3).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 4).Value = my_DS.Tables(0).Rows(i).Item("Center_Name").ToString
                    App.ActiveSheet.Cells(rownum, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 4).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 5).Value = my_DS.Tables(0).Rows(i).Item("course_name").ToString
                    App.ActiveSheet.Cells(rownum, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 5).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 6).Value = my_DS.Tables(0).Rows(i).Item("LoginName").ToString
                    App.ActiveSheet.Cells(rownum, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 6).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 7).Value = my_DS.Tables(0).Rows(i).Item("Password").ToString
                    App.ActiveSheet.Cells(rownum, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 7).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 8).Value = my_DS.Tables(0).Rows(i).Item("writtentestdate").ToString
                    App.ActiveSheet.Cells(rownum, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 8).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 9).Value = my_DS.Tables(0).Rows(i).Item("total_marks").ToString
                    App.ActiveSheet.Cells(rownum, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 9).Borders.Weight = 2

                    rownum = rownum + 1
                Next

                ' '' CODE FOR CENTER DETAILS
                ''Dim sit2 As Excel.Worksheet = myWorkBook.Worksheets(3)
                ''sit2.Activate()
                ''sit2.Cells.Clear()


                ''With App.ActiveSheet.Range("B3:C4")
                ''    .MergeCells = True
                ''    .Interior.ColorIndex = 40
                ''    .Font.Bold = True
                ''    .Font.ColorIndex = 53
                ''    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ''    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                ''    .Cells.Value = "Course Details"
                ''    .Font.Size = 15
                ''    .BORDERS(xlEdgeLeft).Weight = 2
                ''    .BORDERS(xlEdgeTop).Weight = 2
                ''    .BORDERS(xlEdgeBottom).Weight = 2
                ''    .BORDERS(xlEdgeRight).Weight = 2
                ''End With


                ' Set Column Headers

                ''App.ActiveSheet.Cells(5, 2).Value = "Course Name"
                ''App.ActiveSheet.Cells(5, 2).Interior.ColorIndex = 36
                ''App.ActiveSheet.Cells(5, 2).Font.Bold = True
                ''App.ActiveSheet.Cells(5, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ''App.ActiveSheet.Cells(5, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                ''App.ActiveSheet.Cells(5, 2).ColumnWidth = 20
                ''App.ActiveSheet.Cells(5, 2).WrapText = False
                ''App.ActiveSheet.Cells(5, 2).Borders.Weight = 2

                ''App.ActiveSheet.Cells(5, 3).Value = "Course ID"
                ''App.ActiveSheet.Cells(5, 3).Interior.ColorIndex = 36
                ''App.ActiveSheet.Cells(5, 3).Font.ColorIndex = 3
                ''App.ActiveSheet.Cells(5, 3).Font.Bold = True
                ''App.ActiveSheet.Cells(5, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                ''App.ActiveSheet.Cells(5, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ''App.ActiveSheet.Cells(5, 3).ColumnWidth = 20
                ''App.ActiveSheet.Cells(5, 3).WrapText = False
                ''App.ActiveSheet.Cells(5, 3).Borders.Weight = 2

                ''If cn.connect(strpath) Then



                ''    Dim ds As New DataSet()
                ''    Dim strbldr As New StringBuilder
                ''    strbldr.Append("  select  mw.Course_ID,mc.COURSE_NAME from M_Weightage as mw inner join m_course as mc ")
                ''    strbldr.Append("  on mc.course_id=mw.course_id AND MC.Del_Flag=0 group by mw.Course_ID,mc.course_name ")
                ''    strbldr.Append(" HAVING SUM(MW.Sub_Weightage)=100 ")

                ''    ' Dim da As New SqlDataAdapter("select course_id,Course_name from m_course where del_flag=0", cn.MyConnection)
                ''    Dim da As New SqlDataAdapter(strbldr.ToString, cn.MyConnection)

                ''    da.Fill(ds)
                ''    ht.Clear()
                ''    For a As Integer = 0 To ds.Tables(0).Rows.Count - 1
                ''        ht.Add(ds.Tables(0).Rows(a).Item(1).ToString, ds.Tables(0).Rows(a).Item(0).ToString)
                ''    Next
                ''End If
                ''Dim keys1(ht.Count) As String
                ''ht.Keys.CopyTo(keys1, 0)

                '' rownum = 6
                ' Set the data in columns
                ''For i As Integer = 0 To keys1.Length - 2
                ''    App.ActiveSheet.Cells(rownum, 2).Value = keys1(i).ToString
                ''    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                ''    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                ''    App.ActiveSheet.Cells(rownum, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                ''    App.ActiveSheet.Cells(rownum, 2).Borders.Weight = 2
                ''    App.ActiveSheet.Cells(rownum, 3).Value = ht(keys1(i)).ToString
                ''    App.ActiveSheet.Cells(rownum, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ''    App.ActiveSheet.Cells(rownum, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                ''    App.ActiveSheet.Cells(rownum, 3).Borders.Weight = 2
                ''    rownum = rownum + 1
                ''Next

                ''Dim sitmain As Excel.Worksheet = myWorkBook.Worksheets(1)
                ''sitmain.Activate()


                ' Save Workbook
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                If System.IO.File.Exists(Server.MapPath("ExcelImport\CandidateListing.xls")) Then
                    System.IO.File.Delete(Server.MapPath("ExcelImport\CandidateListing.xls"))
                End If

                Dim fileName1 As Object = Server.MapPath("ExcelImport\CandidateListing.xls")

                myWorkBook.SaveAs(fileName1, objOpt, objOpt, objOpt, objOpt, objOpt, Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
                '  myWorkBook.SaveCopyAs(Server.MapPath("Excel Import\BUSER.XLS"))
                myWorkBook.Close()
                WorkBooks.Close()


                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim file As New System.IO.FileInfo(Server.MapPath("ExcelImport\CandidateListing.xls"))

                If file.Exists = True Then

                    Response.Clear()
                    Response.ClearHeaders()
                    Response.ClearContent()

                    Response.ContentType = "application/ms-excel"

                    Response.AppendHeader("Content-disposition", "attachment; filename=" + file.Name)

                    Response.AddHeader("Content-Length", file.Length.ToString)
                    Response.ContentType = "application/octet-stream"

                    Response.WriteFile(file.FullName)

                    'Response.End()
                    Response.Flush()

                End If

                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                System.IO.File.Delete(Server.MapPath("ExcelImport\BUSER.xls"))
                ' dim intResult As Integer  = GetWindowThreadProcessId(iHandle, intPID)
                'proc = System.Diagnostics.Process.GetProcessById(intPID)
                ' proc.Kill()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                ReleaseObject(App)
                ReleaseObject(WorkBooks)
                ReleaseObject(WorkBook)
                ReleaseObject(Sheet)
                ReleaseObject(Sheets)
                ReleaseObject(objOpt)


                Throw ex
            Finally
                ReleaseObject(App)
                ReleaseObject(WorkBooks)
                ReleaseObject(WorkBook)
                ReleaseObject(Sheet)
                ReleaseObject(Sheets)
                ReleaseObject(objOpt)
            End Try
        End Sub


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

        Public Sub GetReport()
            'Excel Objects
            Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
            Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
            Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
            Dim Sheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
            Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
            Dim objOpt As Object = System.Reflection.Missing.Value
            Dim my_DS As DataSet
            Try

                'Constants
                Const xlEdgeLeft = 7
                Const xlEdgeTop = 8
                Const xlEdgeBottom = 9
                Const xlEdgeRight = 10

                'Create  Spreadsheet
                App = New Excel.Application

                WorkBooks = DirectCast(App.Workbooks, Excel.Workbooks)
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim myWorkBook As Excel.Workbook = App.Workbooks.Open(Server.MapPath("ExcelImport\report.xls"), 0, False, 5, "", "", False, Excel.XlPlatform.xlWindows, "", True, False, 0, True)
                WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
                Dim sit As New Excel.Worksheet
                Dim sit1 As Excel.Worksheet = myWorkBook.Worksheets(1)
                sit1.Activate()
                sit1.Cells.Clear()



                ' CODE TO INSERT CANDIDATE DETAILS
                '*** Write Cell Border ***'  
                ' Set Main Column Headers

                With App.ActiveSheet.Range("D2:D2")
                    ' .MergeCells = True
                    ' .Interior.ColorIndex = 40
                    .Font.Bold = True
                    .Font.Underline = True
                    ' .Font.ColorIndex = 53
                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    .Cells.Value = dblCenter.SelectedItem.Text.ToUpper '"PRAGATI COMPUTER INSTITUTE"
                    .Font.Size = 11
                    ' .BORDERS(xlEdgeLeft).Weight = 4
                    ' .BORDERS(xlEdgeTop).Weight = 4
                    '.BORDERS(xlEdgeBottom).Weight = 4
                    '.BORDERS(xlEdgeRight).Weight = 4
                End With


                ' Set Column Headers

                App.ActiveSheet.Cells(4, 1).Value = "No."
                'App.ActiveSheet.Cells(4, 1).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(4, 1).Font.Bold = True
                App.ActiveSheet.Cells(4, 1).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(4, 1).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '  App.ActiveSheet.Cells(4, 1).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 1).WrapText = False
                App.ActiveSheet.Cells(4, 1).Borders.Weight = 2


                App.ActiveSheet.Cells(4, 2).Value = "Exam Date Time"
                'App.ActiveSheet.Cells(4, 2).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 3).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 2).Font.Bold = True
                App.ActiveSheet.Cells(4, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                '  App.ActiveSheet.Cells(4, 2).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 2).WrapText = False
                App.ActiveSheet.Cells(4, 2).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 3).Value = "Roll No."
                'App.ActiveSheet.Cells(4, 2).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 3).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 3).Font.Bold = True
                App.ActiveSheet.Cells(4, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                '  App.ActiveSheet.Cells(4, 2).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 3).WrapText = False
                App.ActiveSheet.Cells(4, 3).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 4).Value = "Candidate Name"
                ' App.ActiveSheet.Cells(4, 3).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(4, 4).Font.Bold = True
                App.ActiveSheet.Cells(4, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(4, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '  App.ActiveSheet.Cells(4, 3).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 4).WrapText = False
                App.ActiveSheet.Cells(4, 4).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 5).Value = "Exam"
                'App.ActiveSheet.Cells(4, 4).Interior.ColorIndex = 36
                '  App.ActiveSheet.Cells(5, 5).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 5).Font.Bold = True
                App.ActiveSheet.Cells(4, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                '  App.ActiveSheet.Cells(4, 4).ColumnWidth = 50
                App.ActiveSheet.Cells(4, 5).WrapText = False
                App.ActiveSheet.Cells(4, 5).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 6).Value = "T1"
                'App.ActiveSheet.Cells(4, 5).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(4, 6).Font.Bold = True
                App.ActiveSheet.Cells(4, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(4, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '  App.ActiveSheet.Cells(4, 5).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 6).WrapText = False
                App.ActiveSheet.Cells(4, 6).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 7).Value = "T2"
                'App.ActiveSheet.Cells(4, 6).Interior.ColorIndex = 36
                ' App.ActiveSheet.Cells(5, 7).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 7).Font.Bold = True
                App.ActiveSheet.Cells(4, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ' App.ActiveSheet.Cells(4, 6).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 7).WrapText = False
                App.ActiveSheet.Cells(4, 7).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 8).Value = "Pr"
                'App.ActiveSheet.Cells(4, 7).Interior.ColorIndex = 36
                App.ActiveSheet.Cells(4, 8).Font.Bold = True
                App.ActiveSheet.Cells(4, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                App.ActiveSheet.Cells(4, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '  App.ActiveSheet.Cells(4, 7).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 8).WrapText = False
                App.ActiveSheet.Cells(4, 8).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 9).Value = "Total"
                ' App.ActiveSheet.Cells(4, 8).Interior.ColorIndex = 36
                '  App.ActiveSheet.Cells(5, 9).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 9).Font.Bold = True
                App.ActiveSheet.Cells(4, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ' App.ActiveSheet.Cells(4, 8).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 9).WrapText = False
                App.ActiveSheet.Cells(4, 9).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 10).Value = "Gr"
                '  App.ActiveSheet.Cells(4, 8).Interior.ColorIndex = 36
                '  App.ActiveSheet.Cells(5, 9).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 10).Font.Bold = True
                App.ActiveSheet.Cells(4, 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 10).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ' App.ActiveSheet.Cells(4, 8).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 10).WrapText = False
                App.ActiveSheet.Cells(4, 10).Borders.Weight = 2

                App.ActiveSheet.Cells(4, 11).Value = "Status"
                '   App.ActiveSheet.Cells(4, 8).Interior.ColorIndex = 36
                '  App.ActiveSheet.Cells(5, 9).Font.ColorIndex = 3
                App.ActiveSheet.Cells(4, 11).Font.Bold = True
                App.ActiveSheet.Cells(4, 11).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                App.ActiveSheet.Cells(4, 11).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                ' App.ActiveSheet.Cells(4, 8).ColumnWidth = 20
                App.ActiveSheet.Cells(4, 11).WrapText = False
                App.ActiveSheet.Cells(4, 11).Borders.Weight = 2
                Dim ht As New Hashtable


                CreateDataSetForReport()
                my_DS = DirectCast(Session("DS3"), DataSet)
                Session.Remove("DS3")
                Dim cn As New ConnectDb
                Dim strpath As String
                strpath = ConfigurationSettings.AppSettings("PathDb")

                If my_DS.Tables(0).Rows.Count = 0 Then
                    LblMsg.Text = "No Record(s) Found"
                    LblMsg.ForeColor = Color.Red
                    LblMsg.Visible = True
                    Exit Sub
                End If

                Dim rownum As Integer = 5
                ' Set the data in columns

                Dim s As DateTime

                For i As Integer = 0 To my_DS.Tables(0).Rows.Count - 1


                    App.ActiveSheet.Cells(rownum, 1).Value = i + 1
                    '     App.ActiveSheet.Cells(rownum, 1).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 1).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 1).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 1).Borders.Weight = 2



                    s = Convert.ToDateTime(my_DS.Tables(0).Rows(i).Item("appearancedate"))
                    '[String].Format("{0:d}", s)
                    App.ActiveSheet.Cells(rownum, 2).Value = "'" & s.ToString("dd/MM/yyyy hh:mm")
                    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    App.ActiveSheet.Cells(rownum, 2).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 3).Value = my_DS.Tables(0).Rows(i).Item("LoginName").ToString()
                    App.ActiveSheet.Cells(rownum, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    App.ActiveSheet.Cells(rownum, 3).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 4).Value = my_DS.Tables(0).Rows(i).Item("username").ToString
                    App.ActiveSheet.Cells(rownum, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    App.ActiveSheet.Cells(rownum, 4).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 5).Value = my_DS.Tables(0).Rows(i).Item("Course_code").ToString().ToUpper
                    '     App.ActiveSheet.Cells(rownum, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    App.ActiveSheet.Cells(rownum, 5).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 6).Value = my_DS.Tables(0).Rows(i).Item("obtained_marks").ToString
                    App.ActiveSheet.Cells(rownum, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 6).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 7).Value = "NA" 'my_DS.Tables(0).Rows(i).Item("LoginName").ToString
                    '     App.ActiveSheet.Cells(rownum, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 7).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 8).Value = "NA" ' my_DS.Tables(0).Rows(i).Item("Password").ToString
                    App.ActiveSheet.Cells(rownum, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 8).Borders.Weight = 2

                    ' In future it colud be changed to T1 + T2 + Pr 
                    App.ActiveSheet.Cells(rownum, 9).Value = my_DS.Tables(0).Rows(i).Item("obtained_marks").ToString
                    '     App.ActiveSheet.Cells(rownum, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                    App.ActiveSheet.Cells(rownum, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 9).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 10).Value = my_DS.Tables(0).Rows(i).Item("Grad").ToString
                    App.ActiveSheet.Cells(rownum, 10).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                    App.ActiveSheet.Cells(rownum, 10).Borders.Weight = 2

                    App.ActiveSheet.Cells(rownum, 11).Value = my_DS.Tables(0).Rows(i).Item("Status").ToString().ToUpper
                    App.ActiveSheet.Cells(rownum, 11).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                    App.ActiveSheet.Cells(rownum, 11).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                    App.ActiveSheet.Cells(rownum, 11).Borders.Weight = 2

                    rownum = rownum + 1
                Next

                sit1.Name = "Sheet1"

                ' Save Workbook
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                If System.IO.File.Exists(Server.MapPath("ExcelImport\ExamDetails.xls")) Then
                    System.IO.File.Delete(Server.MapPath("ExcelImport\ExamDetails.xls"))
                End If

                Dim fileName1 As Object = Server.MapPath("ExcelImport\ExamDetails.xls")

                myWorkBook.SaveAs(fileName1, objOpt, objOpt, objOpt, objOpt, objOpt, Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
                '  myWorkBook.SaveCopyAs(Server.MapPath("Excel Import\BUSER.XLS"))
                myWorkBook.Close()
                WorkBooks.Close()


                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                Dim file As New System.IO.FileInfo(Server.MapPath("ExcelImport\ExamDetails.xls"))

                If file.Exists = True Then

                    Response.Clear()
                    Response.ClearHeaders()
                    Response.ClearContent()

                    Response.ContentType = "application/ms-excel"

                    Response.AppendHeader("Content-disposition", "attachment; filename=" + file.Name)

                    Response.AddHeader("Content-Length", file.Length.ToString)
                    Response.ContentType = "application/octet-stream"

                    Response.WriteFile(file.FullName)

                    'Response.End()
                    Response.Flush()

                End If
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                System.IO.File.Delete(Server.MapPath("ExcelImport\BUSER.xls"))

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                ReleaseObject(App)
                ReleaseObject(WorkBooks)
                ReleaseObject(WorkBook)
                ReleaseObject(Sheet)
                ReleaseObject(Sheets)
                ReleaseObject(objOpt)

                Throw ex
            Finally
                ReleaseObject(App)
                ReleaseObject(WorkBooks)
                ReleaseObject(WorkBook)
                ReleaseObject(Sheet)
                ReleaseObject(Sheets)
                ReleaseObject(objOpt)
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


            If len >= DGReport.PageCount Then
                len = DGReport.PageCount - 1
            End If

            ' if search clicked again then page 1 should be selected 
            If DGReport.CurrentPageIndex = 0 Then
                ViewState("pageNo") = 1
                ViewState("lastRange") = 1
            End If

            ' Getting the currently selected page value 
            Dim selPage As Integer = 0
            If (ViewState("pageNo") <> Nothing) Then
                selPage = CInt(ViewState("pageNo"))
            Else
                ' selPage = 1
                selPage = DGReport.CurrentPageIndex + 1
            End If

            If (ViewState("lastRange") <> Nothing) Then

                '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DGReport.PageCount Then
                If selPage >= CInt(ViewState("lastRange")) And selPage < CInt(ViewState("lastRange")) + len Then
                    range = CInt(ViewState("lastRange"))
                    If CInt(ViewState("lastRange")) + len >= DGReport.PageCount Then
                        range = range - 1
                        If range = 0 Then
                            range = 1
                        End If
                    End If

                Else
                    'If it is the last page then resetting the page numbers
                    ' last number - provided length
                    'If (len + selPage) >= DGReport.PageCount Then
                    '    If selPage <= len Then
                    '        range = range
                    '    Else
                    '        range = DGReport.PageCount - len
                    '        'Incase range becomes 0 or less than zero than setting it 1 
                    '        If range <= 0 Then
                    '            range = 1
                    '        End If
                    '    End If

                    'Else
                    If selPage <= DGReport.PageCount Then
                        'range = range
                        If range < CInt(ViewState("lastRange")) Then
                            range = CInt(ViewState("lastRange")) - 1
                        Else
                            If selPage - len > 0 And selPage - len <= DGReport.PageCount - len Then
                                range = selPage - len
                            Else
                                range = CInt(ViewState("lastRange")) + 1
                            End If
                            '  range = CInt(ViewState("lastRange")) + 1
                        End If
                        If selPage > DGReport.PageCount Then
                            range = DGReport.PageCount - (len)
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
            'If selPage = 1 And selPage = DGReport.PageCount - 1 Then
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
            If selPage = DGReport.PageCount Then
                imgnext.Enabled = False
                imglast.Enabled = False
            End If
        End Sub
        Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DGReport.CurrentPageIndex < (DGReport.PageCount - 1)) Then
                        DGReport.CurrentPageIndex += 1

                    End If

                Case "prev" 'The prev button was clicked
                    If (DGReport.CurrentPageIndex > 0) Then
                        DGReport.CurrentPageIndex -= 1
                    End If

                Case "last" 'The Last Page button was clicked
                    DGReport.CurrentPageIndex = (DGReport.PageCount - 1)

                Case Else 'The First Page button was clicked
                    DGReport.CurrentPageIndex = Convert.ToInt32(arg)
            End Select
            ViewState.Add("pageNo", DGReport.CurrentPageIndex + 1)
            ViewState.Add("selval", DGReport.CurrentPageIndex)
            BindGrid()
            'Now, bind the data!
            '   BindSQL()
        End Sub

        Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DGReport.CurrentPageIndex < (DGReport.PageCount - 1)) Then
                        DGReport.CurrentPageIndex += 1
                        '    ViewState.Add("selval", DGReport.CurrentPageIndex)
                    End If

                Case "prev" 'The prev button was clicked
                    If (DGReport.CurrentPageIndex > 0) Then
                        DGReport.CurrentPageIndex -= 1
                        '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                    End If

                Case "last" 'The Last Page button was clicked
                    DGReport.CurrentPageIndex = (DGReport.PageCount - 1)
                    'ViewState.Add("selval", ddlPages.SelectedItem.Value)
                Case Else 'The First Page button was clicked
                    DGReport.CurrentPageIndex = Convert.ToInt32(arg) - 1
                    ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
            End Select

            ViewState.Add("pageNo", DGReport.CurrentPageIndex + 1)
            ViewState.Add("selval", DGReport.CurrentPageIndex)
            BindGrid()
            'Now, bind the data!
            '   BindSQL()
        End Sub

        'Public Sub fillPagesCombo()
        '    ddlPages.Items.Clear()
        '    For cnt As Integer = 1 To DGReport.PageCount
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
        'Disable for Demo
        'Private Sub imgbtnReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnReport.Click
        '    If dblCenter.SelectedItem.Value = 0 Then
        '        LblMsg.Text = "Please select a centre."
        '        LblMsg.Visible = True
        '        LblMsg.ForeColor = Color.Red
        '        Exit Sub
        '    Else
        '        Dim lindex As String = ddlStatus.SelectedItem.Text
        '        ddlStatus.SelectedItem.Text = "Appeared"
        '        GetReport()
        '        ddlStatus.SelectedItem.Text = lindex
        '        ' LblMsg.Visible = False
        '    End If

        'End Sub

        Private Sub CreateDataSetForReport()
            Dim OConn As New ConnectDb
            Dim OAdap As SqlDataAdapter
            Dim StrSql As String
            Dim strbr As New StringBuilder
            Dim blnCheck As Boolean
            Dim dicSearch As New Dictionary(Of String, String)()
            Dim objCommon As CommonFunction
            objCommon = New CommonFunction()

            Try
                '--------------------------Start--------------------------------

                strbr.Append(" select * from(select tcs.userid, ")
                strbr.Append(" (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as username, ")
                strbr.Append(" tcs.course_id,mc.Course_code, ")
                strbr.Append(" tcs.loginname as LoginName, ")
                strbr.Append(" tcs.pwd as 'Password', ")
                strbr.Append(" tcs.written_test_date as writtentestdate, ")
                strbr.Append(" tcs.written_test_appear_date as appearancedate, ")
                strbr.Append(" tcs.appearedflag as appearedflag, ")
                strbr.Append(" mc.course_name,mce.Center_Name, ")
                strbr.Append(" mui.Center_ID, ")
                strbr.Append(" tcs.total_marks, ")
                strbr.Append(" tcs.total_passmarks, ")
                strbr.Append(" isnull(obtained1.obtained_marks,0) as obtained_marks, ")
                strbr.Append(" (case  when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 75 then  'A+' ")
                strbr.Append(" when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >=60 then 'A' ")
                strbr.Append(" when isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 50 then 'B' ")
                strbr.Append(" WHEN (obtained1.obtained_marks*100)/tcs.total_marks is null then null else 'C' End ) as Grad ")
                strbr.Append(" ,(case WHEN obtained1.obtained_marks >= tcs.total_passmarks then 'Pass' ")
                strbr.Append(" WHEN obtained1.obtained_marks is null then null ")
                strbr.Append(" ELSE 'Fail' ")
                strbr.Append(" END) as Status,mmc.Main_Course_ID ")

                strbr.Append(" FROM T_Candidate_Status as tcs ")
                strbr.Append(" left join M_USER_INFO as mui ")
                strbr.Append(" on tcs.userid=mui.userid ")
                strbr.Append(" left join m_course as mc ")
                strbr.Append(" on mc.course_id=tcs.course_id ")
                strbr.Append(" Left Join ")
                strbr.Append(" (select sum(temp.obtained_marks) as obtained_marks,temp.course_id,temp.userid from (select ")
                strbr.Append(" ( ")
                strbr.Append(" case ")
                strbr.Append(" WHEN mq.Qn_Category_ID=3 then ")
                strbr.Append(" ( ")
                strbr.Append(" case ")
                strbr.Append(" WHEN mqa.sub_id=tro.sub_id then ")
                strbr.Append(" (Case ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" count(mqa.Correct_Opt_Id)")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" END) ")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" WHEN mq.Qn_Category_ID=2 then ")
                strbr.Append(" ( ")
                strbr.Append(" CASE  ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" count(mqa.Correct_Opt_Id)")
                strbr.Append(" ELSE 0")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" WHEN mq.Qn_Category_ID=1 then ")
                strbr.Append(" ( ")
                strbr.Append(" CASE  ")
                strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                strbr.Append(" SUM(mq.Total_Marks)")
                strbr.Append(" ELSE 0 ")
                strbr.Append(" End ")
                strbr.Append(" ) ")
                strbr.Append(" End ")
                strbr.Append(" ) as obtained_marks ")
                strbr.Append(" ,mc.course_id,mui.userid ")
                strbr.Append(" from m_question as mq ")
                strbr.Append(" left join M_Question_Answer as mqa ")
                strbr.Append(" on mqa.Qn_ID=mq.qnid and mqa.test_type=mq.test_type ")
                strbr.Append(" left join t_result as tr ")
                strbr.Append(" on tr.qno=mq.qnid ")
                strbr.Append(" AND tr.test_type=mq.test_type ")
                strbr.Append(" left join m_user_info as mui ")
                strbr.Append(" on mui.userid=tr.userid ")
                strbr.Append(" left join m_course as mc ")
                strbr.Append(" on mc.course_id=tr.course_id ")
                strbr.Append(" left join m_testinfo as mti ")
                strbr.Append(" on mti.test_type=tr.test_type ")
                strbr.Append(" left join t_result_option as tro ")
                strbr.Append(" on tro.result_id=tr.result_id ")
                strbr.Append(" and tr.test_type=mti.test_type ")
                strbr.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
                strbr.Append(" group by mq.total_marks,mc.course_id, ")
                strbr.Append(" mq.total_marks,mq.qnid,mqa.Sub_id, ")
                strbr.Append(" mqa.Correct_Opt_Id, tro.option_id, mq.Qn_Category_ID, tro.sub_id, mui.userid ")
                strbr.Append(" )temp ")
                strbr.Append(" group by temp.course_id,temp.userid ")
                strbr.Append(" )as obtained1 ")
                strbr.Append(" on obtained1.course_id=tcs.course_id ")
                strbr.Append(" and tcs.userid=obtained1.userid ")
                strbr.Append(" left join M_Centers as mce ")
                strbr.Append(" on mce.Center_ID=mui.Center_ID left join M_Main_Course as mmc ")
                strbr.Append(" on mmc.Main_Course_ID=mc.Main_Course_ID")
                strbr.Append(" )temp ")

                '----------------End----------------------------------

                'strQuery.Append(" order by candstatus.userid ")
                'If ddlResult.SelectedItem.Text = "Pass" Then
                '    dicSearch.Add(" CandStatus.written_test_remark", ddlResult.SelectedItem.Text)
                'ElseIf ddlResult.SelectedItem.Text = "Reject" Then
                '    dicSearch.Add(" (CandStatus.written_test_remark", ddlResult.SelectedItem.Text + "' OR (ResAttempted.Attempted IS NULL AND CandStatus.Written_Test_Appear_Date IS NOT NULL))")
                'End If

                If dblCenter.SelectedItem.Text <> "---- Select ----" Then
                    dicSearch.Add("temp.Center_ID", dblCenter.SelectedValue.ToString())
                    'If dblCourse.SelectedItem.Text <> "---- Select ----" Then
                    '    dicSearch.Add("temp.Course_ID", dblCourse.SelectedValue.ToString())
                    'End If
                End If
                If dblCourse.SelectedItem.Text <> "---- Select ----" Then
                    dicSearch.Add("temp.Course_ID", dblCourse.SelectedValue.ToString())
                End If

                If ddlStatus.SelectedItem.Text = "Appeared" Then
                    dicSearch.Add("temp.Appeared", "")
                ElseIf ddlStatus.SelectedItem.Text = "Assigned" Then
                    dicSearch.Add("temp.Assigned", "")
                End If


                'If Not TxtUserName.Text.Trim() = "" Then
                '    dicSearch.Add("temp.username", TxtUserName.Text.Trim())
                'End If

                'If Not TxtLoginname.Text.Trim() = "" Then
                '    dicSearch.Add("UserInfo.loginname", TxtLoginname.Text.Trim())
                'End If

                'If ddlTestName.SelectedItem.Text <> "ALL" Then
                '    dicSearch.Add("m_testinfo.test_type", ddlTestName.SelectedValue.ToString())
                'End If

                'If Not TxtFrom.Text.Trim() = "" And Not TxtTo.Text.Trim() = "" Then
                '    dicSearch.Add("temp.writtentestdate Between '", ConvertDate(TxtFrom.Text) + "'  and  '" + ConvertDate(TxtTo.Text) + "'")
                'ElseIf Not TxtFrom.Text.Trim() = "" Then
                '    dicSearch.Add("temp.writtentestdate", ConvertDate(TxtFrom.Text))
                'End If

                'Select Case ddlgrade.SelectedIndex
                '    Case 0

                '    Case 1
                '        dicSearch.Add("grad = ", "A+")
                '    Case 2
                '        dicSearch.Add("grad = ", "A")
                '    Case 3
                '        dicSearch.Add("grad = ", "B")
                '    Case 4
                '        dicSearch.Add("grad = ", "C")
                'End Select


                If Not txtAppFromDate.Value.Trim() = "" And Not TxtAppToDate.Value.Trim() = "" Then
                    dicSearch.Add("temp.appearancedate Between '", ConvertDate(txtAppFromDate.Value) + "'  and  '" + ConvertDate(TxtAppToDate.Value) + " 23:59:59'")
                ElseIf Not txtAppFromDate.Value.Trim() = "" Then
                    dicSearch.Add("temp.appearancedate", ConvertDate(txtAppFromDate.Value))
                End If

                'If Not TxtScoreFrom.Text.Trim() = "" And Not TxtScoreTo.Text.Trim() = "" Then
                '    dicSearch.Add("ResScore.Score Between '", TxtScoreFrom.Text + "'  and  '" + TxtScoreTo.Text + "'")
                'ElseIf Not TxtScoreFrom.Text.Trim() = "" Then
                '    dicSearch.Add("ResScore.Score", TxtScoreFrom.Text)
                'End If

                strbr.Append(objCommon.GetSelectSearchQuery(dicSearch))
                strbr.Append(" order by temp.appearancedate  ")
                Dim strq = strbr.ToString
                m_DS = New DataSet
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If OConn.connect() Then
                    OAdap = New SqlDataAdapter(strq, OConn.MyConnection)
                    OAdap.Fill(m_DS, "TblResults")
                    Session("DS3") = m_DS
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                    OConn.disconnect()
                End If
                Throw ex
            Finally

                OConn.disconnect()
                OConn = Nothing
                StrSql = Nothing
                OAdap = Nothing
                strbr = Nothing

            End Try
        End Sub

        'Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
        '    DGReport.CurrentPageIndex = ddlPages.SelectedItem.Value
        '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
        '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
        '    BindGrid()
        'End Sub
        'Public Declare Function GetWindowThreadProcessId Lib "user32.dll" (ByVal hWnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer

        ' Public Declare Function FindWindow Lib "user32.dll" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Protected Sub btnOkay2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOkay2.Click
            Dim strpath As String = ConfigurationSettings.AppSettings("PathDb")
            'Dim CandidateID As String = Session("cdUID").ToString
            'Dim cid As String = Session("cdCID").ToString
            Dim CandidateID As String = Session("CandidateID").ToString
            Dim cid As String = Session("CourseID").ToString
            Dim ccmd As SqlCommand
            If txtReassign.Value = "" Then
                txtReassign.Focus()
                Exit Sub
            End If
            Dim q As String = "update T_candidate_status set written_test_date ='" & txtReassign.Value & " 00:00:00'   where userid=" & CandidateID & " and Course_id=" & cid
            Try
                If objconn.connect() Then
                    ccmd = New SqlCommand(q, objconn.MyConnection)
                    ccmd.ExecuteNonQuery()
                    txtReassign.Value = ""
                    If (Session("CandidateID") IsNot Nothing) Then
                        SendMailFunction()
                    End If
                    BindGrid()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                ccmd = Nothing
                'objconn.disconnect()
            End Try
            Session.Remove("CandidateID")
            Session.Remove("CourseID")
            ModalPopupExtender2.Hide()
        End Sub

        Protected Sub btnPopUpCancel2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPopUpCancel2.Click
            Try
                txtReassign.Value = ""
                If Not Session("cdCID") Is Nothing Then
                    Session.Remove("cdCID")
                End If
                If Not Session("cdUID") Is Nothing Then
                    Session.Remove("cdUID")
                End If
                ModalPopupExtender2.Hide()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally

            End Try
        End Sub
        'This function is to create for the Reassiging mail 
        'by Rajesh Nagvanshi 
        Private Function SendMailFunction()
            'Dim strpath As String
            Dim objconn As New ConnectDb
            Dim strquery As String
            Dim CandidateID As String = Session("CandidateID")
            Dim intcourseid As Integer = Session("CourseID")
            Dim str As String
            Dim cda As SqlDataAdapter
            Dim cds As DataSet
            Dim q As String = "select convert(varchar(10), written_test_date,103)  from T_candidate_status where userid=" & CandidateID & " and Course_id=" & intcourseid
            '  ModalPopupExtender2.Show()
            Dim cmd As SqlCommand
            Dim objStrBldr As StringBuilder
            'strpath = ConfigurationSettings.AppSettings("PathDb")
            Try
                If objconn.connect() Then
                    'date will be shown in popup as OldDate
                    cda = New SqlDataAdapter(q, objconn.MyConnection)
                    cds = New DataSet
                    cda.Fill(cds)

                    Session.Add("cdCID", intcourseid)
                    Session.Add("cdUID", CandidateID)
                    'delete rows from t_result_option (Probably 40 rows)
                    objStrBldr = New StringBuilder()
                    objStrBldr.Append("delete from t_result_option where result_id in (select result_id from t_result where userid=" + CandidateID.ToString() + " and course_id=" + intcourseid.ToString() + ")")
                    cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    cmd.ExecuteNonQuery()
                    objStrBldr = Nothing
                    'delete rows from t_result (Probably 40 rows)
                    objStrBldr = New StringBuilder()
                    objStrBldr.Append("delete from t_result where userid=" + CandidateID.ToString() + " and course_id=" + intcourseid.ToString())
                    cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    cmd.ExecuteNonQuery()
                    objStrBldr = Nothing


                    Dim item As DictionaryEntry
                    Dim objht As New Hashtable
                    objht = CheckQuestions(intcourseid)
                    Dim inttotalque As Integer
                    Dim sb As StringBuilder
                    Dim ary() As Integer
                    Try
                        'Loop for each HashTable enrty.
                        For Each item In objht
                            ary = DirectCast(item.Value, Integer())

                            For g As Integer = 0 To 2
                                sb = New StringBuilder
                                sb.Append(" select isnull(Sum(Total_Marks),0) from m_question where del_flag=0 and ")
                                sb.Append(" test_type= ")
                                sb.Append(item.Key)
                                sb.Append(" and Qn_category_id= ")
                                sb.Append(g + 1)
                                strquery = sb.ToString()
                                cmd = New SqlCommand(strquery, objconn.MyConnection)
                                inttotalque = cmd.ExecuteScalar()
                                If inttotalque < ary(g) Then
                                    LblMsg.ForeColor = Color.FromName("Red")
                                    LblMsg.Text = Resources.Resource.CandStatus_verify
                                    LblMsg.Visible = True
                                    Exit Function
                                End If
                            Next
                        Next

                    Catch ex As Exception
                        If log.IsDebugEnabled Then
                            log.Debug("Error :" & ex.ToString())
                        End If
                        If objconn IsNot Nothing Then
                            objconn.disconnect()
                        End If
                        Throw ex
                    Finally
                        item = Nothing
                        objht = Nothing
                        cmd = Nothing
                        sb = Nothing
                        strquery = Nothing
                        ary = Nothing
                    End Try

                    Dim myCommand1 As SqlTransaction
                    Dim rdr As SqlDataReader
                    objStrBldr = New StringBuilder
                    Dim strCourseName As String = Nothing

                    objStrBldr.Append(" Select Total_Time,Total_marks,Total_passmarks,course_name From M_Course where Course_id =  ")
                    objStrBldr.Append(intcourseid)

                    cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    myCommand1 = objconn.MyConnection.BeginTransaction()
                    cmd.Transaction = myCommand1
                    Dim intcoursetime, intcoursemarks, intcoursepassmarks As Integer
                    rdr = cmd.ExecuteReader()
                    While rdr.Read
                        intcoursetime = rdr.Item("Total_Time")
                        intcoursemarks = rdr.Item("Total_marks")
                        intcoursepassmarks = rdr.Item("Total_passmarks")
                        strCourseName = rdr.Item("course_name")
                    End While
                    rdr.Close()
                    myCommand1.Commit()


                    'update user in t_candidate_status as Newly Assigned Student
                    objStrBldr = New StringBuilder()
                    objStrBldr.Append("update t_candidate_status ")
                    'objStrBldr.Append("set AppearedFlag=0, written_test_date=getdate(), written_test_appear_date=null, Consume_time=null, ")
                    objStrBldr.Append("set AppearedFlag=0,  written_test_appear_date=null, Consume_time=null, ")
                    objStrBldr.Append("Total_Time=" + intcoursetime.ToString() + ", Total_marks=" + intcoursemarks.ToString() + ", Total_passmarks=" + intcoursepassmarks.ToString())
                    objStrBldr.Append(" where userid=" + CandidateID.ToString() + " and course_id=" + intcourseid.ToString())
                    cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    cmd.ExecuteNonQuery()
                    objStrBldr = Nothing


                    'End If
                    'UpdForOnlineTest(0) = struserid
                    'UpdForOnlineTest(1) = strpassword
                    objStrBldr = New StringBuilder()
                    objStrBldr.Append(" Update T_User_Course set Del_Flag = 1  ")
                    objStrBldr.Append(" where T_User_Course.User_ID = ")
                    objStrBldr.Append(CandidateID.ToString())
                    objStrBldr.Append(" and T_User_Course.Course_ID = ")
                    objStrBldr.Append(intcourseid.ToString())
                    cmd = New SqlCommand(objStrBldr.ToString(), objconn.MyConnection)
                    myCommand1 = objconn.MyConnection.BeginTransaction()
                    cmd.Transaction = myCommand1
                    cmd.ExecuteNonQuery()
                    myCommand1.Commit()

                    Dim strExamURl As String
                    Dim strQuery1 As String
                    Dim myDataReader As SqlDataReader
                    Dim myDataReader2 As SqlDataReader
                    Dim strEmaiId As String
                    Dim strOwnerName As String
                    Dim mail As New MailMessage
                    Dim objCommFun As CommonFunction
                    Dim strMessage As String

                    strExamURl = ConfigurationSettings.AppSettings("ExamURL")
                    strQuery1 = "select mc.Email,mc.Owner_Name From M_Centers as mc join T_Center_Course as tcc on mc.Center_ID=tcc.Center_ID where tcc.Course_ID=" + intcourseid.ToString() + " and mc.center_id=(select center_id from m_user_info where userid=" + CandidateID.ToString() + ")"
                    cmd = New SqlCommand(strQuery1, objconn.MyConnection)
                    myDataReader = cmd.ExecuteReader()
                    While myDataReader.Read
                        If Not IsDBNull(myDataReader.Item("Email")) Then
                            strEmaiId = myDataReader.Item("Email")
                            strOwnerName = myDataReader.Item("Owner_Name")
                        End If
                    End While
                    myDataReader.Close()
                    myDataReader = Nothing
                    'mail.From = strEmaiId
                    'mail.From = "vaibhav@usindia.com"
                    mail.From = ConfigurationSettings.AppSettings("mailsenderid")
                    ' mail.Cc = "vaibhav@usindia.com" 'strEmaiId
                    mail.Cc = strEmaiId

                    strQuery1 = "select email from m_user_info where userid=" + CandidateID.ToString()
                    cmd = New SqlCommand(strQuery1, objconn.MyConnection)
                    myDataReader2 = cmd.ExecuteReader()
                    While myDataReader2.Read
                        If Not IsDBNull(myDataReader2.Item("Email")) Then
                            mail.To = myDataReader2.Item("Email")
                        End If
                    End While
                    myDataReader2.Close()
                    myDataReader2 = Nothing
                    objCommFun = New CommonFunction()
                    q = "select (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as Name, "
                    q = q + "mc.center_name as ClassName, mti.Test_name as TestName, "
                    q = q + "convert(varchar(10), tcs.Written_Test_Date,120) as ExamDate, tcs.total_time as TotalTime, "
                    q = q + "tcs.total_Marks as TotalMarks, tcs.userid as UserID,mc.owner_name as Teacher, "
                    q = q + "tcs.Loginname as LoginName, tcs.pwd as Password, mui.email as EmailID "
                    q = q + "from t_candidate_status as tcs "
                    q = q + "left join M_User_info as mui on mui.userid=tcs.userid "
                    q = q + "left join M_Centers as mc on mc.center_id=mui.center_id "
                    q = q + "left join T_User_course as tuc on tuc.User_ID=tcs.Userid and tuc.course_id=tcs.course_id "
                    q = q + "left join M_Testinfo as mti on mti.Test_type=tuc.Test_type "
                    q = q + "where tcs.userid=" + CandidateID.ToString() + " and tcs.course_id=" + intcourseid.ToString()
                    cda = New SqlDataAdapter(q, objconn.MyConnection)
                    cds = New DataSet
                    cda.Fill(cds)
                    Session.Add("ReassignDataSetValues", cds)
                    strMessage = objCommFun.ReadFile(Server.MapPath(ConfigurationSettings.AppSettings("AssignExamMail")))
                    strMessage = strMessage.Replace("#Name#", cds.Tables(0).Rows(0)("Name").ToString())
                    strMessage = strMessage.Replace("#ClassName#", cds.Tables(0).Rows(0)("ClassName").ToString())
                    strMessage = strMessage.Replace("#sub#", strCourseName)
                    strMessage = strMessage.Replace("#date#", cds.Tables(0).Rows(0)("ExamDate").ToString())
                    strMessage = strMessage.Replace("#Marks#", cds.Tables(0).Rows(0)("TotalMarks").ToString())
                    strMessage = strMessage.Replace("#UserId#", cds.Tables(0).Rows(0)("LoginName").ToString())
                    strMessage = strMessage.Replace("#Password#", cds.Tables(0).Rows(0)("Password").ToString())
                    strMessage = strMessage.Replace("#TeacherName#", cds.Tables(0).Rows(0)("Teacher").ToString())
                    strMessage = strMessage.Replace("#links#", "http://adiinternals.usindia.com:8094/StudentLogin.aspx")

                    mail.Subject = "Request to appear an exam of (" + cds.Tables(0).Rows(0)("ClassName").ToString() + " - " + strCourseName + ")"
                    mail.Body = strMessage
                    mail.BodyFormat = MailFormat.Html
                    mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
                    mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
                    mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                   
                    SmtpMail.SmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
                    SmtpMail.Send(mail)

                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
                Response.Redirect("error.aspx", False)
            Finally
                cmd = Nothing
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
            End Try

        End Function

        Private Function MessageBox() As Object
            Throw New NotImplementedException
        End Function

        Protected Sub DGReport_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DGReport.SelectedIndexChanged

        End Sub


        '#Region "Event for Checking and unchecking all checkboxes"
        '       
        '        'Desc      : This is event for checking and unchecking all checkboxes.

        '        Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        '            ' BindGrid()
        '            Dim chk As CheckBox = Nothing
        '            Dim chkbx As CheckBox = DirectCast(sender, CheckBox)
        '            Try

        '                For Each rowItem As DataGridItem In DGReport.Items

        '                    chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
        '                    If chkbx.Checked = True Then
        '                        chk.Checked = DirectCast(sender, CheckBox).Checked
        '                    Else
        '                        chk.Checked = False
        '                    End If
        '                Next

        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("ChkSelect ALL : ", ex)
        '                    Response.Redirect("error.aspx", False)
        '                End If
        '            Finally
        '                chk = Nothing
        '            End Try

        '        End Sub
        '#End Region

        
#Region "Delete_Click"
        'Added by   : Pragnesha Kulkarni
        'Description : Delete candidate status from the list
        'Date by     : 2018/07/20
        'Bug ID       : 670
        '-----------------------------------------------------------
        Protected Sub imgBtnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnDelete.Click
            DeleteCourse()
        End Sub
        '------------------------------------------------------------

        'Added by   : Pragnesha Kulkarni
        'Description : Delete candidate status from the list
        'Date by     : 2018/07/20
        'Bug ID       : 670
        Public Sub DeleteCourse()
            Dim ChkBox As System.Web.UI.WebControls.CheckBox
            Dim CheckedCandidate As Integer
            Dim DgItem As DataGridItem
            Dim bolflg As Boolean = True
            Try
                m_CntDel = 0
                For Each DgItem In DGReport.Items
                    ChkBox = DirectCast((DgItem.Cells(0).FindControl("chkRemove")), CheckBox)
                    If (True = ChkBox.Checked) Then
                        bolflg = False
                        CheckedCandidate = Convert.ToInt32(DgItem.Cells(1).Text)
                        DeleteCandidateInfo(CheckedCandidate, DgItem.Cells(17).Text)
                        m_CntDel += 1
                    End If
                Next
                If bolflg = True Then
                    'Dim strScript As String = "<script language=JavaScript>alert(' Please Select At Least One CheckBox For Delete ');</script>"
                    'Page.RegisterStartupScript("PopUp", strScript)
                    LblMsg.Visible = True
                    LblMsg.Text = Resources.Resource.CandStatus_chkbxdel
                    Exit Sub
                End If
                If m_CntDel > 0 Then
                    BindGrid()
                    LblMsg.ForeColor = System.Drawing.Color.FromName("Green")
                    LblMsg.Text = "  " & m_CntDel & Resources.Resource.CandStatus_delsuc
                    LblMsg.Visible = True
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                ChkBox = Nothing
                CheckedCandidate = Nothing
                DgItem = Nothing

            End Try

        End Sub

#End Region
    End Class


#End Region


End Namespace
