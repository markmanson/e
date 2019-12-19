#Region "Namespaces"
Imports System
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.UI.WebControls
Imports System.Web.Security
Imports System.Collections.Generic
Imports System.Collections
Imports log4net
Imports System.Web.Mail
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
#End Region
Partial Public Class ReassignExam
    Inherits System.Web.UI.Page

   



    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("CandStatus")
    Dim objconn As New ConnectDb
#Region "Initials"
    ' Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("search")
    Protected WithEvents select2 As System.Web.UI.HtmlControls.HtmlSelect
    Protected WithEvents txt_graduate As System.Web.UI.WebControls.TextBox
    Protected WithEvents lbl_note As System.Web.UI.WebControls.Label

    Dim chkUsers() As HtmlInputCheckBox     'Check box for selecting person for online test
    Dim combooperator As DropDownList       'Combobox for operator = < > <> etc
    Dim comboLogOperator As DropDownList    'Combobox for logical operator And Or etc
    Dim CONS As unirecruite.Errconstants
    Const ENCRYPT_DELIMIT = "h"
    Const ENCRYPT_KEY = 124
    Protected WithEvents Button1 As System.Web.UI.WebControls.Button
#End Region


#Region " Web Form Designer Generated Code "


    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

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
    Dim rdbSelectall, rdbReverseall As New System.Web.UI.WebControls.RadioButton
#End Region

#Region "Page_Load"

    Private Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete

    End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
        Dim Ds As DataSet

        Try

            TxtFrom.Attributes.Add("Readonly", "true")
            TxtTo.Attributes.Add("Readonly", "true")
            txtnewdate.Attributes.Add("Readonly", "true")
            'Put user code to initialize the page here
            If Session("UserName") = Nothing Then
                Response.Redirect("~\login.aspx", False)
            End If
            If Session("UniUserType").ToString <> "1" Then
                Response.Redirect("~\register.aspx", False)
            End If
            If Session("LoginGenuine") Is Nothing Then
                Response.Redirect("error.aspx?err=Session Timeout. Please Login to continue.", False)
            End If
            If Not Page.IsPostBack Then
                Session.Add("DS", Nothing)
                Me.SetFocus(dblCenter)
                FillCombo()
                FillListOfCourse_New()
                ' FillSubjectCombo()

                'FillCenterCombo()
                populate_subjectid()
            Else
                '   BindGrid()
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

        strPathDb = ConfigurationManager.AppSettings("PathDb")
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
        strPathDb = ConfigurationManager.AppSettings("PathDb")
        Dim query As String = ""
        Dim result As String = ""
        Dim datareader As SqlDataReader
        ddlcourse.Items.Clear()
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
        'Dim dtData As DataTable
        Dim strPathDb As String

        Try

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

            If objconn.connect() Then

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



#Region "BindGrid"
    Private Sub BindGrid()
        Dim intpginx As Integer

        '/************Jatin Gangajaliya,2011/2/17*************/
        Dim objCommon As New CommonFunction
        Dim ids As String = objCommon.GetCommaSeperatedIDS("select Main_Course_id from M_Main_Course where marksheet_flag=0")
        Dim query As String
        'Dim g, j, k As Integer
        Dim i As String
        'Static Dim counter As Integer
        Static Dim start As Integer = 33
        Dim dt As New DataTable()
        Dim conn As SqlConnection
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
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
                    LblRecCnt.Text = "Total Records: " & m_DS.Tables(0).Rows.Count.ToString()
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
                    fillPagesCombo()
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

                        DGReport.Columns(16).Visible = True
                        DGReport.Columns(9).Visible = True
                        DGReport.Columns(10).Visible = True

                        If bool = True Then
                            DGReport.Items(intlpcounter).Cells(16).Enabled = False
                            '  DGReport.Items(intlpcounter).Cells(15).ToolTip = "Exam(s) are pending"
                            DGReport.Items(intlpcounter).Cells(16).Controls.Clear()
                        End If

                        If (intflag = 0 Or intflag = 1) Then
                            DGReport.Items(intlpcounter).Cells(16).Enabled = False
                            'DGReport.Items(intlpcounter).Cells(15).FindControl()
                            '  DGReport.Items(intlpcounter).Cells(15).ToolTip = "Exam(s) are pending"
                            DGReport.Items(intlpcounter).Cells(16).Controls.Clear()

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
                                '    DGReport.Columns(16).Visible = False
                                '   DGReport.Columns(9).Visible = True
                            ElseIf intflag = 2 Then
                                '  DGReport.Columns(10).Visible = False
                                ' DGReport.Columns(9).Visible = False
                                DGReport.Items(intlpcounter).Cells(9).Enabled = False
                                DGReport.Items(intlpcounter).Cells(9).Controls.Clear()
                                DGReport.Items(intlpcounter).Cells(10).Enabled = False
                                DGReport.Items(intlpcounter).Cells(10).Controls.Clear()
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
                        DGReport.Items(intlpcounter).Cells(16).Attributes.Add("onclick", "return confirm('Are you sure you want delete this record and want to Reassign the exam ?');")


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
                Else
                    LblMsg.Visible = True
                    DGReport.Visible = False
                    LblMsg.Text = "No Record(s) Found"
                    gridDiv.Visible = False
                    'Disable for Demo
                    ' imgbtnExport.Visible = False
                End If
                '/*****************Jatin Gangajaliya,2011/2/17**********/


                'Session("DS") = m_DS
            End If
        Catch ex As Exception
            DGReport.CurrentPageIndex = DGReport.CurrentPageIndex - 1
            If DGReport.CurrentPageIndex >= 0 Then
                DGReport.DataBind()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
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
        'Dim blnCheck As Boolean
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


            If dblCenter.SelectedItem.Text <> "---- Select ----" Then
                dicSearch.Add("temp.Center_ID", dblCenter.SelectedValue.ToString())
                'If ddlCourse.SelectedItem.Text <> "---- Select ----" Then
                '    dicSearch.Add("temp.Course_ID", ddlCourse.SelectedValue.ToString())
                'End If
            End If
            If ddlcourse.SelectedItem.Text <> "---- Select ----" Then
                dicSearch.Add("temp.Course_ID", ddlcourse.SelectedValue.ToString())
            End If

            'If ddlStatus.SelectedItem.Text = "Appeared" Then
            '    dicSearch.Add("temp.Appeared", "")
            'ElseIf ddlStatus.SelectedItem.Text = "Assigned" Then
            '    dicSearch.Add("temp.Assigned", "")
            '    '******************Monal shah**************
            '    'ElseIf ddlStatus.SelectedItem.Text = "Ongoing" Then
            '    '    dicSearch.Add("temp.Ongoing", "")
            'ElseIf ddlStatus.SelectedItem.Text = "LoginUser" Then
            '    dicSearch.Add("temp.LoginUser", "")
            'ElseIf ddlStatus.SelectedItem.Text = "LinkBreak" Then
            '    dicSearch.Add("temp.LinkBreak", "")
            '********************End*********************
            ' End If


            If Not TxtUserName.Text.Trim() = "" Then
                dicSearch.Add("temp.username", TxtUserName.Text.Trim())
            End If

            'If Not TxtLoginname.Text.Trim() = "" Then
            '    dicSearch.Add("UserInfo.loginname", TxtLoginname.Text.Trim())
            'End If

            'If ddlTestName.SelectedItem.Text <> "ALL" Then
            '    dicSearch.Add("m_testinfo.test_type", ddlTestName.SelectedValue.ToString())
            'End If

            If Not TxtFrom.Text.Trim() = "" And Not TxtTo.Text.Trim() = "" Then
                dicSearch.Add("temp.writtentestdate Between '", ConvertDate(TxtFrom.Text) + "'  and  '" + ConvertDate(TxtTo.Text) + "'")
            ElseIf Not TxtFrom.Text.Trim() = "" Then
                dicSearch.Add("temp.writtentestdate", ConvertDate(TxtFrom.Text))
            End If

           
            strbr.Append(objCommon.GetSelectSearchQuery(dicSearch))
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
    Private Sub DeleteCandidateInfo(ByVal UserID As Integer, ByVal Test_type As String)
        Dim OConn As New ConnectDb
        Dim OCmd As SqlCommand
        Dim StrDel1, StrDel2 As String

        Try
            'Query modify by bhasker(26-11-09)
            '********************** Start *******************
            StrDel1 = "Delete from t_candidate_status where userid='" & UserID & "'  And Test_type = '" & Test_type & "'"
            StrDel2 = "Delete from t_result where userid='" & UserID & "'   And Test_type = '" & Test_type & "'"
            '********************** End *******************
            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If True = OConn.connect() Then
                OCmd = New SqlCommand(StrDel1, OConn.MyConnection)
                OCmd.ExecuteNonQuery()
                OCmd = New SqlCommand(StrDel2, OConn.MyConnection)
                OCmd.ExecuteNonQuery()
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

#Region "btnBack_Click"

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
        Try
            Response.Redirect("admin.aspx", False)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
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

            If Not TxtTo.Text.Trim() = "" Then
                strSql = "Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as testtype,m_testinfo.test_name as TestName, " + _
                                    "t_candidate_status.written_test_date as WrittenTestDate, a_result.attend as attempted, t_candidate_status.written_test_marks as Score,Count (*) as TotalMark," + _
                                    "t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result" + _
                                    " from m_user_info, m_testinfo, t_candidate_status, t_result, " + _
                                    " ( Select t_candidate_status.userid, t_candidate_status.test_type, count(*) as attend from t_result, t_candidate_status " + _
                                    " Where t_result.optionid > 0 and t_candidate_status.userid = t_result.userid and t_candidate_status.test_type = t_result.test_Type " + _
                                    " Group By t_candidate_status.userid, t_candidate_status.test_type ) a_result" + _
                                    " where a_result.test_type = t_candidate_status.test_type and a_result.userid = t_candidate_status.userid and " + _
                                    " t_candidate_status.test_Type=t_result.test_type and t_candidate_status.userid = t_result.userid" + _
                                    " and t_candidate_status.test_type = m_testinfo.test_type and m_user_info.userid=t_candidate_status.userid " + _
                                    " and t_candidate_status.written_test_date Between ' " + TxtFrom.Text + "'  and  '" + TxtTo.Text + "'  " + _
                                    " Group by m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname, m_testinfo.test_type, m_testinfo.test_name, t_candidate_status.written_test_date," + _
                                    " t_candidate_status.written_test_marks,t_candidate_status.written_test_appear_date, t_candidate_status.written_test_remark," + _
                                    " a_result.attend Union Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as test_type, m_testinfo.test_name as TestName, " + _
                                    " t_candidate_status.written_test_date as WrittenTestDate, ' ' as attempted, ' ' as Score,' ' as TotalMark, " + _
                                    " t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result " + _
                                    "  from m_user_info, m_testinfo, t_candidate_status where t_candidate_status.test_type = m_testinfo.test_type and " + _
                                    " m_user_info.userid=t_candidate_status.userid and t_candidate_status.written_test_date Between  ' " + TxtFrom.Text + "'  and  '" + TxtTo.Text + "' and t_candidate_status.written_test_appear_date is Null"
                '+ _                                " Order by 5 desc, 8 desc"
                '" + _    " Order by 5 desc, 8 desc"

                Dim x As String = " + TxtFrom.Text + "
            Else
                strSql = "Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as testtype,m_testinfo.test_name as TestName, " + _
                            "t_candidate_status.written_test_date as WrittenTestDate, a_result.attend as attempted, t_candidate_status.written_test_marks + '-' + count(*)  as Score," + _
                            "t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result" + _
                            " from m_user_info, m_testinfo, t_candidate_status, t_result, " + _
                            " ( Select t_candidate_status.userid, t_candidate_status.test_type, count(*) as attend from t_result, t_candidate_status " + _
                            " Where t_result.optionid > 0 and t_candidate_status.userid = t_result.userid and t_candidate_status.test_type = t_result.test_Type " + _
                            " Group By t_candidate_status.userid, t_candidate_status.test_type ) a_result" + _
                            " where a_result.test_type = t_candidate_status.test_type and a_result.userid = t_candidate_status.userid and " + _
                            " t_candidate_status.test_Type=t_result.test_type and t_candidate_status.userid = t_result.userid" + _
                            " and t_candidate_status.test_type = m_testinfo.test_type and m_user_info.userid=t_candidate_status.userid " + _
                            " and t_candidate_status.written_test_date =  " + TxtFrom.Text + " " + _
                            " Group by m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname, m_testinfo.test_type, m_testinfo.test_name, t_candidate_status.written_test_date," + _
                            " t_candidate_status.written_test_marks,t_candidate_status.written_test_appear_date, t_candidate_status.written_test_remark," + _
                            " a_result.attend Union Select m_user_info.userid, m_user_info.name + ' ' + m_user_info.surname as UserName, m_testinfo.test_type as test_type, m_testinfo.test_name as TestName, " + _
                            " t_candidate_status.written_test_date as WrittenTestDate, ' ' as attempted, ' ' as Score, " + _
                            " t_candidate_status.written_test_appear_date as AppearanceDate, t_candidate_status.written_test_remark as Result " + _
                            "  from m_user_info, m_testinfo, t_candidate_status where t_candidate_status.test_type = m_testinfo.test_type and " + _
                            " m_user_info.userid=t_candidate_status.userid and t_candidate_status.written_test_date Between  '" + TxtFrom.Text + "' " + " and t_candidate_status.written_test_appear_date is Null"

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

    Private Sub BtnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles BtnSearch.Click
        Try
            Dim dt1 As Date
            Dim dt2 As Date
            'Const REGULAR_EXP = "(^[0-9]{4,4}/?[0-1][0-9]/?[0-3][0-9]$)"
            Dim flag As Boolean
            flag = True


            If TxtTo.Text <> "" And flag Then
                If TxtFrom.Text = "" Then
                    ' MsgBox("Please enter Exam Assigned From date.", MsgBoxStyle.Exclamation)
                    LblMsg.Text = "Please enter Exam Assigned From date."
                    LblMsg.ForeColor = Color.Red
                    LblMsg.Visible = True
                    TxtFrom.Focus()
                    flag = False
                End If
            End If

            If (TxtFrom.Text <> "" And IsDate(ConvertDate(TxtFrom.Text))) And (TxtTo.Text <> "" And IsDate(ConvertDate(TxtTo.Text))) And flag Then
                dt1 = Convert.ToDateTime(ConvertDate(TxtTo.Text))
                dt2 = Convert.ToDateTime(ConvertDate(TxtFrom.Text))

                If dt1 < dt2 Then
                    '  MsgBox("Please enter Exam Assigned To date greater then Exam Assigned From date.", MsgBoxStyle.Exclamation)
                    LblMsg.Text = "Please enter Exam Assigned To date greater than Exam Assigned From date."
                    LblMsg.ForeColor = Color.Red
                    LblMsg.Visible = True
                    TxtTo.Focus()
                    flag = False
                End If
            End If

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

    Protected Sub DGReport_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DGReport.SelectedIndexChanged

    End Sub

#Region "DataGrid ItemCommand Event"
    'Desc: This Event is DataGrid ItemCommand Event.
    'By: Jatin Gangajaliya
    'Date: 2011/2/9

    Protected Sub DGReport_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGReport.ItemCommand

        
        Dim strpath, CandidateID As String
        'Dim strquery, name, strsubject As String
        'Dim strrollno, strcenter, strcourse, strgrade, appearedDate As String
        'Dim intcourseid As Integer
        'Dim MaxMarks, ObtMarks, Rmarks, grd, totmark As String
        'Dim strsqlinsert As String
        'Dim j As Integer
        Dim StrTemp As String = "User(s):"
        Dim AtLeastOneChecked As Boolean = False
        Dim blnCheck As Boolean = True
        Dim testtypevalue As Integer
        Dim intquestcnt1(testtypevalue) As Integer
        Dim intlques1(testtypevalue) As Integer
        Dim intmques1(testtypevalue) As Integer
        Dim inthques1(testtypevalue) As Integer
        'Dim totalquestcnt As Integer
        'Dim objCommand As SqlCommand
        'Dim objDataReader As SqlDataReader
        'Dim sendingFail As String
        'Dim Send_Fail As String
        'Dim strSqlSel As String
        'Dim myCommand As SqlCommand
        Dim intquestcntal As New ArrayList
        Dim intlquesal As New ArrayList
        Dim intmquesal As New ArrayList
        Dim inthquesal As New ArrayList        
        CandidateID = e.Item.Cells(1).Text


        If (e.CommandName = "EditDate") Then
            ModalPopupExtender1.Show()
            CandidateID = e.Item.Cells(1).Text
            Dim cid As String = e.Item.Cells(17).Text
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
        End If
        '/************************Start*************************************/
    End Sub
#End Region


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

#Region "Genetare Random Password"
    Public Function GetRandomPasswordUsingGUID() As String
        Dim guidResult As String = System.Guid.NewGuid().ToString()
        Try
            guidResult = guidResult.Replace("-", String.Empty)
            Return guidResult.Substring(0, 10)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString)
            End If
            Throw ex
        Finally
            guidResult = Nothing
        End Try
    End Function
#End Region
    Public Function GetRollNumber(ByVal userid As String) As String
        Dim rds As DataSet
        Dim rda As SqlDataAdapter
        Dim obCon As ConnectDb
        Dim Rollno As String = ""
        Try
            obCon = New ConnectDb
            If obCon.connect() = True Then
                rda = New SqlDataAdapter("select RollNo from M_User_Info where userid =" & userid, obCon.MyConnection)
                rds = New DataSet
                rda.Fill(rds)
                Rollno = rds.Tables(0).Rows(0).Item(0).ToString
                obCon.disconnect()
            End If
        Catch ex As Exception
            If obCon.MyConnection.State = ConnectionState.Open Then
                obCon.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            obCon = Nothing
            rda = Nothing
            rds = Nothing
        End Try
        Return Rollno
    End Function


#Region "makepdffile"
    'Added by :: Saraswati Patel
    'For making file pdf file with password protection
    <System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Assert, Unrestricted:=True)> _
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
        'Dim ie As Integer
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

                                te(0) = text.Substring(0, text.IndexOf(" ", 15))
                                te(1) = text.Substring(text.IndexOf(" ", 15))
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

#Region "ValidateDate"
    Private Function ValidateDate(ByVal YrDt() As String) As Boolean
        Try
            If YrDt.Length > 1 Then
                If Convert.ToInt32(Date.Now.Year.ToString) > Convert.ToInt32(YrDt(0)) Then
                    Return False
                ElseIf Convert.ToInt32(Date.Now.Year.ToString) = Convert.ToInt32(YrDt(0)) Then
                    If Convert.ToInt32(Date.Now.Month.ToString) > Convert.ToInt32(YrDt(1)) Then
                        Return False
                    ElseIf Convert.ToInt32(Date.Now.Month.ToString) = Convert.ToInt32(YrDt(1)) Then
                        If Convert.ToInt32(Date.Now.Day.ToString) > Convert.ToInt32(YrDt(2)) Then
                            Return False
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If
                Else
                    Return True
                End If
            End If
            Return True
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Function
#End Region


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
#Region "MailForOnlineTest"
    Private Function MailForOnlineTest(ByVal userid As Integer, ByVal password As String, ByVal subjectName As String, ByVal TotalMarks As String, ByVal PassingMarks As String, ByVal TotalTime As String) As String
        Dim objconn As New ConnectDb
        Dim sqlstr As String
        Dim myCommand As SqlCommand
        Dim myDataReader As SqlDataReader
        'Dim strPathDb As String
        Dim bool As Boolean
        Dim sendingFail As String = ""
        Try
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                sqlstr = "SELECT  (Name) as UserName,Email,RollNo FROM m_user_info"
                sqlstr = "SELECT  (SurName+' '+Name+' '+isNull(Middlename,'')) as UserName,Email,RollNo FROM m_user_info"
                sqlstr = sqlstr & " WHERE userid=" & userid
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()
                While myDataReader.Read
                    If Not IsDBNull(myDataReader.Item("Email")) Then
                        bool = sendMail(myDataReader.Item("Email"), myDataReader.Item("RollNo"), myDataReader.Item("UserName"), password, subjectName, TotalMarks, PassingMarks, TotalTime)
                        If Not bool Then
                            sendingFail = myDataReader.Item("UserName")
                        End If
                    Else
                        sendingFail = myDataReader.Item("UserName")
                    End If
                End While
                myCommand.Dispose()
                myDataReader.Close()
            End If
            objconn.disconnect()
            'End If
            Return sendingFail
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            objconn.disconnect()
            LblMsg.Text = ex.Message()
        Finally
            objconn.disconnect()
            objconn = Nothing
            sqlstr = Nothing
            myCommand = Nothing
            myDataReader = Nothing
            strPathDb = Nothing
        End Try
    End Function
#End Region

#Region "Clear Button"
    'Desc: This is button event for clearing all controls
    'By: Jatin Gangajaliya,2011/3/9

    Protected Sub btnclear_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnclear.Click
        Try
            TxtUserName.Text = String.Empty
            dblCenter.SelectedIndex = 0
            ddlcourse.Items.Clear()
            'ddlCourse.Items.Insert(0, "---- Select ----")
            FillListOfCourse_New()
            TxtFrom.Text = String.Empty
            TxtTo.Text = String.Empty
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
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
        Dim query As String = ""
        Dim datareader As SqlDataReader
        Dim data1 As SqlDataReader
        ddlcourse.Items.Clear()
        ddlcourse.Items.Insert(0, "---- Select ----")
        ddlcourse.SelectedIndex = 0
        Try
            If objconn.connect() = True Then

                If Session.Item("userid") = Nothing Or Session.Item("userid") = 0 Or Request.QueryString("userid") <> Nothing Then
                    query = "select Distinct M_Course.Course_ID,M_Course.Course_Name,M_Course.Description from M_Course,m_centers,t_Center_course where t_Center_Course.center_id=m_Centers.Center_ID and t_Center_Course.course_id=M_Course.Course_ID and m_centers.Center_id='" & dblCenter.SelectedItem.Value & "' order by M_Course.Course_Name"
                    myCommand = New SqlCommand(query, objconn.MyConnection)
                    datareader = myCommand.ExecuteReader()
                    While datareader.Read()

                        Dim lstItm As New ListItem()
                        lstItm.Enabled = True
                        lstItm.Text = datareader.Item(1)
                        lstItm.Value = datareader.Item(0)
                        ddlcourse.Items.Add(lstItm)
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
                        ddlcourse.Items.Add(lstItm)
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
                        lstItm.Selected = True
                        ddlcourse.Items.Add(lstItm)
                    End While
                End If
                datareader.Close()
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If

                '   GetCoursesFromDB("7")
            End If
            If ddlcourse.SelectedIndex = 0 Then
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
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
        Dim query As String = ""
        Dim datareader As SqlDataReader
        Dim data1 As SqlDataReader
        ddlcourse.Items.Clear()
        ddlcourse.Items.Insert(0, "---- Select ----")
        ddlcourse.SelectedIndex = 0
        Try
            If objconn.connect() = True Then
                query = "select Course_ID,Course_name from M_Course where del_flag=0 order by Course_name"
                myCommand = New SqlCommand(query, objconn.MyConnection)
                datareader = myCommand.ExecuteReader()
                While datareader.Read()
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = datareader.Item(1)
                    lstItm.Value = datareader.Item(0)
                    ddlcourse.Items.Add(lstItm)
                End While
                datareader.Close()
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
            End If
            If ddlcourse.SelectedIndex = 0 Then
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

    Protected Sub dblCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlcourse.SelectedIndexChanged
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
        Dim strPathDb As String

        Try

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

            If objconn.connect() Then
                Dim rows As DataRow

                If ddlcourse.SelectedItem.Text.ToString() <> "---- Select ----" Then
                    sqlstr = "SELECT distinct test_type,test_name FROM m_testinfo WHERE del_flag ='0' and course_id = " & ddlcourse.SelectedValue.ToString()
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
                If ddlcourse.SelectedItem.Text.ToString() = "---- Select ----" Then
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
        Dim strpath As String = ConfigurationManager.AppSettings("PathDb")
        Dim cmd As SqlCommand
        Dim StrTemp As String = "User(s):"
        Dim AtLeastOneChecked As Boolean = False
        Dim blnCheck As Boolean = True
        Dim testtypevalue As Integer
        ' Dim AtLeastOneChecked As Boolean = False
        Dim intquestcnt1(testtypevalue) As Integer
        Dim intlques1(testtypevalue) As Integer
        Dim intmques1(testtypevalue) As Integer
        Dim inthques1(testtypevalue) As Integer
        'Dim totalquestcnt As Integer
        'Dim strrollno, strcenter, strcourse, strgrade, appearedDate As String
        'Dim objCommand As SqlCommand
        'Dim objDataReader As SqlDataReader
        Dim intquestcntal As New ArrayList
        Dim intlquesal As New ArrayList
        Dim strsqlinsert As String
        Dim intmquesal As New ArrayList
        Dim inthquesal As New ArrayList
        Dim myCommand1 As SqlTransaction
        ' Dim UserInfo(5) As String
        Dim sendingFail As String
        Dim Send_Fail As String
        'Dim strSqlSel As String
        Dim myCommand As SqlCommand
        Dim CandidateID As String = Session("cdUID").ToString
        Dim cid As String = Session("cdCID").ToString
        'Dim ccmd As SqlCommand
        If txtnewdate.Text = "" Then
            txtnewdate.Focus()
            Exit Sub
        End If
        Try
            If objconn.connect() Then


                'Delete candidate from t_candidate_status table
                cmd = New SqlCommand("delete from T_candidate_Status where  userid=" & CandidateID & " and course_id=" & cid, objconn.MyConnection)
                cmd.ExecuteNonQuery()

                'Delete candidate course information from t_result_option table also if course is disable.


                cmd = New SqlCommand("delete from t_result_option where result_id in (select result_id from t_result where userid=" & CandidateID & " and course_id=" & cid & ")", objconn.MyConnection)
                cmd.ExecuteNonQuery()

                'Delete candidate course information from t_result table also if course is disabl
                cmd = New SqlCommand("delete from t_result where  userid=" & CandidateID & " and course_id=" & cid, objconn.MyConnection)
                cmd.ExecuteNonQuery()

                If txtnewdate.Text = "" Then
                    LblMsg.ForeColor = Color.FromName("Red")
                    LblMsg.Text = "Empty Examination Date. Please Verify!!"
                    LblMsg.Visible = True
                    Exit Sub
                End If

                If txtnewdate.Text <> "" Then
                    Dim YrDt() As String = Split(ConvertDate(txtnewdate.Text), "/")
                    Dim VldDate As Boolean = ValidateDate(YrDt)
                    Dim objconn As New ConnectDb
                    'Dim j As Integer
                    'Dim emailId As String
                    'Dim myDataReader As SqlDataReader
                    'Dim strPathDb As String
                    If VldDate = False Then
                        LblMsg.ForeColor = Color.FromName("Red")
                        LblMsg.Text = "Examination should be due in the near future, it cannot be conducted in the past. Please change the examination date."
                        LblMsg.Visible = True
                        Exit Sub
                    Else
                        LblMsg.Visible = False
                    End If
                    'objconn.MyConnection.State.Open = ConnectionState.Open
                    If objconn.connect() = True Then
                        Dim rdr As SqlDataReader
                        'Dim myCommand1 As SqlTransaction
                        Dim strbr As New StringBuilder()
                        strbr = New StringBuilder
                        strbr.Append(" Select Total_Time,Total_marks,Total_passmarks From M_Course where Course_id =  ")
                        strbr.Append(cid)
                        Dim strq As String = strbr.ToString()
                        'myCommand.Connection = objconn.MyConnection
                        myCommand = New SqlCommand(strq, objconn.MyConnection)

                        myCommand1 = objconn.MyConnection.BeginTransaction()
                        myCommand.Transaction = myCommand1
                        Dim intcoursetime, intcoursemarks, intcoursepassmarks As Integer
                        rdr = myCommand.ExecuteReader()
                        While rdr.Read
                            intcoursetime = rdr.Item("Total_Time")
                            intcoursemarks = rdr.Item("Total_marks")
                            intcoursepassmarks = rdr.Item("Total_passmarks")
                        End While
                        rdr.Close()
                        myCommand1.Commit()

                        '/*******************************End******************************/


                        Dim strpassword As String = GetRandomPasswordUsingGUID()

                        Dim struserid As String = GetRollNumber(CandidateID)

                        'Enter record for user who is selected to appear for online examination

                        If (struserid <> String.Empty And strpassword <> String.Empty) Then

                            strsqlinsert = " INSERT INTO t_candidate_status "
                            strsqlinsert = strsqlinsert & "(userid,Course_ID,written_test_date,consume_time,LoginName,Pwd,Total_Time,Total_marks,Total_passmarks) "
                            strsqlinsert = strsqlinsert & "Values( "
                            strsqlinsert = strsqlinsert & "" & CandidateID & ","
                            strsqlinsert = strsqlinsert & "'" & cid & "',"
                            strsqlinsert = strsqlinsert & "'" & ConvertDate(txtnewdate.Text) & " ',"
                            'strsqlinsert = strsqlinsert & "'" & myDate & "',"
                            strsqlinsert = strsqlinsert & "'0'"
                            strsqlinsert = strsqlinsert & "," & "'" & struserid & "'" & "," & "'" & strpassword & "' " & ","
                            strsqlinsert = strsqlinsert & intcoursetime & "," & intcoursemarks & "," & intcoursepassmarks
                            strsqlinsert = strsqlinsert & " )"
                            strsqlinsert = Replace(strsqlinsert, "''", "NULL")
                            myCommand.Connection = objconn.MyConnection
                            myCommand = New SqlCommand(strsqlinsert, objconn.MyConnection)
                            myCommand1 = objconn.MyConnection.BeginTransaction()
                            myCommand.Transaction = myCommand1
                            myCommand.ExecuteNonQuery()
                            myCommand1.Commit()
                            sendingFail = MailForOnlineTest(CandidateID, strpassword, ddlcourse.SelectedItem.Text, intcoursemarks, intcoursepassmarks, intcoursetime)
                            If sendingFail <> "" Then
                                Send_Fail += sendingFail + ","
                            End If
                            strbr = New StringBuilder()
                            strbr.Append(" Update T_User_Course set Del_Flag = 1  ")
                            strbr.Append(" where T_User_Course.User_ID = ")
                            strbr.Append(CandidateID)
                            strbr.Append(" and T_User_Course.Course_ID = ")
                            strbr.Append(cid)
                            strq = strbr.ToString()
                            myCommand.Connection = objconn.MyConnection
                            myCommand = New SqlCommand(strq, objconn.MyConnection)
                            myCommand1 = objconn.MyConnection.BeginTransaction()
                            myCommand.Transaction = myCommand1
                            myCommand.ExecuteNonQuery()
                            myCommand1.Commit()
                            LblMsg.ForeColor = Color.FromName("Green")
                            LblMsg.Visible = True
                            LblMsg.Text = "Exam Assigned Successfuly"
                        End If

                        If Send_Fail <> "" Then
                            Send_Fail = Send_Fail.Substring(0, Send_Fail.Length - 1)
                            If Send_Fail.Substring(0, 1) <> "," Then
                                Dim strScript As String = "<script language=JavaScript>alert('" & "Mail not sent " & Send_Fail & " please check there email id " & " ');</script>"
                                Page.RegisterStartupScript("PopUp", strScript)
                            Else
                                Dim strScript As String = "<script language=JavaScript>alert('" & "Mail sent successfully " & "');</script>"
                                Page.RegisterStartupScript("PopUp", strScript)
                            End If
                        End If
                        If objconn.MyConnection.State = ConnectionState.Open Then
                            objconn.disconnect()
                        End If
                    End If
                    ' BindGrid()
                End If
            End If

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
                log.Debug("Error :" & ex.StackTrace)
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
            txtnewdate.Text = ""
            If Not Session("cdCID") Is Nothing Then
                Session.Remove("cdCID")
            End If
            If Not Session("cdUID") Is Nothing Then
                Session.Remove("cdUID")
            End If
            ModalPopupExtender1.Hide()
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
        'Dim blnCheck As Boolean
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

            If dblCenter.SelectedItem.Text <> "---- Select ----" Then
                dicSearch.Add("temp.Center_ID", dblCenter.SelectedValue.ToString())
                If ddlcourse.SelectedItem.Text <> "---- Select ----" Then
                    dicSearch.Add("temp.Course_ID", ddlcourse.SelectedValue.ToString())
                End If
            End If


            If Not TxtUserName.Text.Trim() = "" Then
                dicSearch.Add("temp.username", TxtUserName.Text.Trim())
            End If
            
            If Not TxtFrom.Text.Trim() = "" And Not TxtTo.Text.Trim() = "" Then
                dicSearch.Add("temp.writtentestdate Between '", TxtFrom.Text + "'  and  '" + TxtTo.Text + "'")
            ElseIf Not TxtFrom.Text.Trim() = "" Then
                dicSearch.Add("temp.writtentestdate", TxtFrom.Text)
            End If

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
            Response.Redirect("error.aspx")
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
        'Dim proc As System.Diagnostics.Process
        'Dim strVer As String
        'Dim intPID As Integer
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


            CreateDataSetExport()
            my_DS = DirectCast(Session("DS1"), DataSet)

            Dim cn As New ConnectDb
            Dim strpath As String
            strpath = ConfigurationManager.AppSettings("PathDb")
            
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
            'Const xlEdgeLeft = 7
            'Const xlEdgeTop = 8
            'Const xlEdgeBottom = 9
            'Const xlEdgeRight = 10

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
            strpath = ConfigurationManager.AppSettings("PathDb")

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

    Public Sub fillPagesCombo()
        ddlPages.Items.Clear()
        For cnt As Integer = 1 To DGReport.PageCount
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
#Region "getCheckbox"
    Private Function getCheckbox(ByVal userid As String, ByVal j As Integer) As HtmlInputCheckBox
        Try
            chkUsers(j) = New HtmlInputCheckBox
            chkUsers(j).Value = userid
            chkUsers(j).ID = userid
            getCheckbox = chkUsers(j)

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex

        End Try
    End Function
#End Region



#Region "sendMail"
    Private Function sendMail(ByVal StrTo As String, ByVal userRollNo As String, ByVal UserName As String, ByVal userPassword As String, ByVal subjectName As String, ByVal TotalMarks As String, ByVal PassingMarks As String, ByVal TotalTime As String) As Boolean
        Dim strQuery1 As String
        Dim strQuery2 As String
        Dim myCommand2 As SqlCommand
        Dim myDataReader2 As SqlDataReader
        Dim myCommand3 As SqlCommand
        Dim myDataReader3 As SqlDataReader
        Dim objconn As New ConnectDb

        Dim strEmaiId As String = ""
        'Dim intNum As Int32
        'Dim intQues As Int32
        'Dim strPathDb As String
        Dim strSmtpServer As String
        Dim myDataReader As SqlDataReader
        Try
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then

                strQuery1 = "select mc.Email From M_Centers as mc join T_Center_Course as tcc on mc.Center_ID=tcc.Center_ID where tcc.Course_ID=" + ddlcourse.SelectedItem.Value
                myCommand = New SqlCommand(strQuery1, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()
                While myDataReader.Read
                    If Not IsDBNull(myDataReader.Item("Email")) Then
                        strEmaiId += myDataReader.Item("Email") + ","
                    End If
                End While

                If StrTo <> "" Then
                    ' strEmaiId = ConfigurationSettings.AppSettings("AdminEmailID")
                    'mail.From = strEmaiId

                    Dim mail As New System.Net.Mail.MailMessage(strEmaiId, StrTo)
                    'mail.From = strEmaiId
                    'mail.To = StrTo
                    'mail.Cc = strEmaiId
                    'mail.Cc = strEmaiId
                    mail.Subject = "Request to appear an exam of (" + ddlcourse.SelectedItem.Text + ")"
                    mail.Body = "Hello " + UserName + Environment.NewLine + Environment.NewLine + "Exam of  The " _
                                & ddlcourse.SelectedItem.Text & Environment.NewLine + Environment.NewLine _
                                & "Exam Date:- " + ConvertDate(txtnewdate.Text) + Environment.NewLine + "UserID:- " _
                                & userRollNo + Environment.NewLine + "Password:- " + userPassword + "" '_
                    '& Environment.NewLine & "TotalTime"
                    'mail.Body = getMsg(CInt(userid))
                    'mail.BodyFormat = MailFormat.Text
                    'mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                    ' mail.Priority = MailPriority.High
                    strSmtpServer = ConfigurationManager.AppSettings("SmtpServer")
                    '*************Added code for Server Authentication
                    'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
                    'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
                    'mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                    Dim server As New System.Net.Mail.SmtpClient

                    Dim basicAuthenticationInfo As New System.Net.NetworkCredential("bharat", "PritaP@1984")
                    server.Credentials = basicAuthenticationInfo
                    server.Send(mail)
                    'SmtpMail.SmtpServer = strSmtpServer
                    ' SmtpMail.Send(mail)

                    'lblMsg.Text = "Mail message(s) sent successfully."
                End If
                'Else
                'lblMsg.Text = "There are not enough questions for the selected test."
                'End If
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                Dim err As String = ex.ToString()
                log.Debug("Error :" & ex.ToString())
                Return False
            End If
            LblMsg.Text = ex.Message()
        Finally
            myDataReader = Nothing
            myDataReader3 = Nothing
            myDataReader2 = Nothing
            myCommand = Nothing
            myCommand3 = Nothing
            myCommand2 = Nothing
            strQuery1 = Nothing
            strQuery2 = Nothing
            strPathDb = Nothing
            objconn = Nothing
            ' mail = Nothing
            strEmaiId = Nothing
        End Try
    End Function
#End Region


    Private Sub CreateDataSetForReport()
        Dim OConn As New ConnectDb
        Dim OAdap As SqlDataAdapter
        Dim StrSql As String
        Dim strbr As New StringBuilder
        'Dim blnCheck As Boolean
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


            If dblCenter.SelectedItem.Text <> "---- Select ----" Then
                dicSearch.Add("temp.Center_ID", dblCenter.SelectedValue.ToString())

            End If
            If ddlcourse.SelectedItem.Text <> "---- Select ----" Then
                dicSearch.Add("temp.Course_ID", ddlcourse.SelectedValue.ToString())
            End If

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

    Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
        DGReport.CurrentPageIndex = ddlPages.SelectedItem.Value
        ViewState.Add("selval", ddlPages.SelectedItem.Value)
        ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
        BindGrid()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub btnOkay_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnOkay.Command

    End Sub

    Private Sub btnOkay_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkay.DataBinding

    End Sub
End Class


