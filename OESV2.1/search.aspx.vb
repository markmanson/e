#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Mail
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Web.UI
Imports System.Resources
Imports System.Threading
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Globalization
Imports System.Web.Security
Imports System.Configuration
Imports log4net

#End Region

Namespace unirecruite
    Partial Class search
        Inherits BasePage
        ''Code for dynamic search
        'Protected WithEvents selFieldlist As System.Web.UI.HtmlControls.HtmlSelect
#Region "Initials"
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("search")
        Protected WithEvents select2 As System.Web.UI.HtmlControls.HtmlSelect
        Protected WithEvents txt_graduate As System.Web.UI.WebControls.TextBox
        Protected WithEvents lbl_note As System.Web.UI.WebControls.Label
        'Commented By Pranit Chimurkar On 2019/10/18
        'Dim chkUsers() As HtmlInputCheckBox     'Check box for selecting person for online test
        Dim combooperator As DropDownList       'Combobox for operator = < > <> etc
        Dim comboLogOperator As DropDownList    'Combobox for logical operator And Or etc
        Dim CONS As unirecruite.Errconstants
        Const ENCRYPT_DELIMIT = "h"
        Const ENCRYPT_KEY = 124
        Protected WithEvents Button1 As System.Web.UI.WebControls.Button
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

#Region "Variable"
        Dim objconn As New ConnectDb
        Dim sqlstr, strpathdb As String
        Dim myCommand As SqlCommand
        Dim myDataReader As SqlDataReader
        Dim myTable, myTable1 As DataTable
        Dim myRow As DataRow
        Dim strselval As String
        Dim strseltest As String
        Dim fieldDataReader As SqlDataReader
        Dim linkselect As New System.Web.UI.WebControls.LinkButton
        Dim chk As New System.Web.UI.WebControls.CheckBox
        Dim rdbSelectall, rdbReverseall As New System.Web.UI.WebControls.RadioButton
#End Region

#Region "onErrors"
        '*************On Error Go to Error Page****************
        Private Sub onErrors(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Error
            Dim Err As New CreateLog
            Try
                'Err.ErrorLog(Server.MapPath("Logs/RMS"), Server.GetLastError().ToString().Trim, "search.aspx.vb")
                'Response.Redirect("error.aspx?err=Error On Page", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                'objconn.disconnect()
                Err = Nothing
            End Try
        End Sub
#End Region

        '**************************************************************************
        'Function               :   Page_Load
        '
        'Return                 :   None
        '
        'Argument               :   sender : system object
        '                           e      : event
        '
        'Explanation            :   Function call on load of form
        '                           This will display the search result for 
        '                           default search criteria
        '                           
        'Note                   :   None
        '**************************************************************************
        'Commented By Pranit Chimurkar on 2019/10/18
#Region "frmSearch_Init"
        'Private Sub frmSearch_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles frmSearch.Init
        '    Dim sCheck As String
        '    Try
        '        If Not IsPostBack Then
        '            'searchResult()
        '            tblResult.Visible = False
        '            tblExam.Visible = False
        '        End If
        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        Response.Redirect("error.aspx", False)
        '    Finally
        '        'objconn.disconnect()
        '        sCheck = Nothing
        '    End Try
        'End Sub
#End Region

#Region "Page_Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
            Try
                'txtExamDate.Attributes.Add("Readonly", "true")
                txtExamDate.Attributes.Add("type", "date")
                'txt_birth.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")


                rdbSelectall.Attributes.Add("onclick", "selectAll(this.form)")
                rdbReverseall.Attributes.Add("onclick", "deselectAll(this.form)")
                rdbReverseall.Checked = True
                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                ' If Session("UniUserType").ToString <> "1" Then ' commented by pragnesha for super admin
                If Convert.ToString(Session("UniUserType")) > "2" Then
                    Response.Redirect("~\register.aspx", False)
                End If
                lblMsg.Visible = False
                If Session("LoginGenuine") Is Nothing Then
                    Response.Redirect("error.aspx?err=Session Timeout. Please Login to continue.", False)
                End If
                searchUser.ToolTip = Resources.Resource.Search_SelCenFirst
                If Not IsPostBack Then
                    'code added by Tabrez
                    '***************************************************************************
                    'gettable()
                    'BindCourse()

                    populate_subjectid()
                    'searchResult()
                    'loadvalue()
                    Me.SetFocus(sel_subjectid)
                    '  emId.Visible = False


                ElseIf IsPostBack Then
                    'If (sel_subjectid.SelectedIndex > 0) Then
                    '    If (ddlcourse.SelectedIndex > 0) Then
                    '        searchResult()
                    '    End If
                    'End If
                    'added by bhumi [17/9/2015]
                    'Reason: while no result found invisible search result and Exam Date section
                    'gridDiv.Visible = False
                    'examSection.Visible = False
                    'tblExam.Visible = False
                    'Ended by bhumi
                    'tblResult.Visible = True
                End If


                'ForNewOrOld()

                'Dim objconn As New ConnectDb
                'Dim sqlstr As String
                'Dim myCommand As SqlCommand
                'Dim myDataReader As SqlDataReader
                'Dim myTable1 As DataTable
                ''modified
                'Dim strselsym As String

                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")

                ''Coded added by kamal on 2006.02.06
                'strselsym = "Select"
                'If lstsymbol.SelectedIndex > 0 Then
                '    strselsym = lstsymbol.SelectedValue
                'End If
                'lstsymbol.Items.Clear()
                'lstsymbol.Items.Add("Select")
                'lstsymbol.Items.Add("<>")
                'lstsymbol.Items.Add("=")
                'If Not strselsym = "Select" Then
                '    lstsymbol.SelectedValue = strselsym
                'End If

                'End of code by kamal on 2006.02.06
                'If objconn.connect(strpathdb) Then
                '    strseltest = ""
                '    'If lsttest.SelectedIndex > 0 Then
                '    '    strseltest = lsttest.SelectedValue
                '    'End If
                '    strselval = ""
                '    'If sel_test_type.SelectedIndex >= 0 Then
                '    '    strselval = sel_test_type.SelectedValue
                '    'End If
                '    'Coded added by kamal on 2006.02.06
                '    'end of kamal
                '    Dim rows As DataRow
                '    sqlstr = "SELECT test_type,test_name FROM m_testinfo"
                '    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                '    myDataReader = myCommand.ExecuteReader()
                '    myTable1 = New DataTable
                '    myTable1.Columns.Add(New DataColumn("test_type", GetType(String)))
                '    myTable1.Columns.Add(New DataColumn("test_name", GetType(String)))
                '    While myDataReader.Read
                '        rows = myTable1.NewRow
                '        rows(0) = myDataReader.Item("test_type")
                '        rows(1) = myDataReader.Item("test_name")
                '        myTable1.Rows.Add(rows)
                '    End While
                '    sel_test_type.DataSource = myTable1
                '    '    lsttest.DataSource = myTable1
                '    sel_test_type.DataValueField = "test_type"
                '    sel_test_type.DataTextField = "test_name"
                '    'lsttest.DataValueField = "test_type"
                '    'lsttest.DataTextField = "test_name"
                '    sel_test_type.DataBind()
                '    'lsttest.DataBind()
                '    'lsttest.Items.Insert(0, "--Select--")
                '    objconn.disconnect()
                'End If
                'myCommand.Dispose()
                ' ''Code for dynamic search
                'myDataReader.Close()
                'objconn.disconnect()
                'strsel method is called here.
                strsel()
                Session("FromPage") = "search"
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                'objconn.disconnect()
                'objconn = Nothing
                'sqlstr = Nothing
                'strpathdb = Nothing
            End Try
        End Sub
#End Region

#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload
            Try
                If Not strpathdb = Nothing Then
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
            'objconn.disconnect()

        End Sub
#End Region

#Region "BindGrid Method"
        'Desc: This is Bindgrid method for datagridview binding.
        'By: Jatin Gangajaliya

        'Added by Pranit on 25/11/2019
        Public Sub BindGrid()
            Dim objconn As New ConnectDb
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim adap As SqlDataAdapter
            Dim myTable As DataTable
            Dim iColumns As Integer
            Dim strPathDb As String
            Dim col As DataColumn
            Dim strtbls As String
            Dim strcriteria As String
            Dim sqlSelect, sqlGroupby As String
            Dim strbr As StringBuilder
            Dim strName As String
            Dim strSurname As String
            Dim strBirth As String
            Dim strPersonal As String

            Try
                myTable = New DataTable
                col = New DataColumn("SrNo")
                col.AutoIncrement = True
                col.AutoIncrementSeed = 1
                col.AutoIncrementStep = 1
                myTable.Columns.Add(col)

                strPathDb = ConfigurationSettings.AppSettings("PathDb")
                sqlSelect = ""
                strtbls = ""
                strcriteria = ""
                sqlGroupby = ""
                strbr = New StringBuilder
                strbr.Append(" SELECT u.surname+' '+u.Name+' '+isnull(u.Middlename,'') as Name,u.email,u.userid,u.center_id,t.course_id,u.RollNo  FROM M_user_info as u  ")
                strbr.Append(" inner join t_user_course as t on u.userid=t.user_id and t.course_id= '")
                strbr.Append(ddlcourse.SelectedValue)
                strbr.Append("'")
                strbr.Append(" where u.userid NOT in ")
                strbr.Append(" (select  userid from t_candidate_status where course_id = ")
                strbr.Append("'")
                strbr.Append(ddlcourse.SelectedValue)
                strbr.Append("'")
                strbr.Append("  ) ")
                strbr.Append(" and u.Delete_Flg='0' and u.center_id= ")
                strbr.Append("'")
                strbr.Append(sel_subjectid.SelectedValue)
                strbr.Append("'")
                If Not (sel_subjectid.SelectedValue = 0) Then
                    strName = " AND Center_ID = " & sel_subjectid.SelectedValue
                End If
                strPersonal = strName & strSurname & strBirth
                sqlSelect = strbr.ToString & strPersonal & " group by t.user_id,t.course_id,u.Name,u.surname,u.email,u.userid,u.center_id,u.Middlename,u.RollNo Order By u.surname "
                objconn.connect()
                myCommand = New SqlCommand(sqlSelect, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader()
                iColumns = myDataReader.FieldCount()


                If objconn.connect() Then
                    adap = New SqlDataAdapter(sqlSelect, objconn.MyConnection)
                    adap.Fill(myTable)
                    If (myTable.Rows.Count > 0) Then
                        lblReocrds.Visible = False
                        DGData.Visible = True
                        DGData.DataSource = myTable
                        DGData.DataBind()
                        'fillPagesCombo()
                        lblNum.Text = " " & myTable.Rows.Count
                        gridDiv.Visible = True
                    Else
                        lblReocrds.Visible = True
                        lblReocrds.Text = Resources.Resource.Common_NoRecFound
                        gridDiv.Visible = False
                        DGData.Visible = False
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
                strPathDb = Nothing
                strName = Nothing
                strBirth = Nothing
            End Try
        End Sub
#End Region

#Region "DataGrid PageIndex Change Event"
        Protected Sub DGData_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGData.PageIndexChanged
            Try
                DGData.CurrentPageIndex = e.NewPageIndex
                BindGrid()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

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

#Region "FillPagesCombo"
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
#End Region

        'Added by Pranit on 02/12/2019
        Sub Selection_Change(sender As Object, e As EventArgs)
            Try
                DGData.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
                BindGrid()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#Region "DropDown for Page"
        'Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
        '    DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
        '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
        '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
        '    BindGrid()
        'End Sub
#End Region

#Region "Event for Checking and unchecking all checkboxes"
        Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

            Dim chk As CheckBox = Nothing
            Try

                For Each rowItem As DataGridItem In DGData.Items

                    chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

                    chk.Checked = DirectCast(sender, CheckBox).Checked
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
#End Region

#Region "chkRemove_CheckedChanged1"
        Protected Sub chkRemove_CheckedChanged1(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim chk As CheckBox = Nothing
            Dim bool As Boolean = True
            Try
                'For Each rowItem As DataGridItem In DGData.Items

                '    chk = DirectCast((rowItem.Cells(0).FindControl("chkSelectAll")), CheckBox)
                '    chk = DirectCast((DGData.Items(0).Cells(9).Controls(0)), CheckBox)

                '    bool = DirectCast(sender, CheckBox).Checked
                '    If bool = False Then
                '        Exit For
                '    End If

                'Next

                'If bool = False Then
                '    chk.Checked = False
                'End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("ChkSelect ALL : ", ex)
                    Response.Redirect("error.aspx", False)
                End If
            Finally
                chk = Nothing
            End Try
        End Sub
#End Region
#Region "strsel"
        Public Sub strsel()
            Try
                'If Not strselval = "" Then
                '    sel_test_type.SelectedValue = strselval
                'End If

                If Not strseltest = "" Then
                    'lsttest.SelectedValue = strseltest
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

#Region "Bind Course DropdownList"
        'Desc: This method Binds Course DropdownList.
        'By: Jatin Gangajaliya, 2011/2/22

        Public Sub BindCourse()
            Dim myDataReader As SqlDataReader
            Dim sqlstr As String
            Dim myTable As DataTable
            Dim myRow As DataRow
            Dim strbr As New StringBuilder()
            Try

                'strbr = New StringBuilder()
                'strbr.Append(" select c.course_id,c.course_name from M_USER_INFO as u left join t_user_course as usco    ")
                'strbr.Append(" on usco.User_ID=u.userid left join m_course as c on c.course_id=usco.course_id ")
                'strbr.Append(" where u.center_id = ")
                'strbr.Append(sel_subjectid.SelectedValue)
                'strbr.Append(" group by c.course_name,usco.course_id,c.course_id ")
                'strbr.Append(" order by c.course_name ")

                'Change By: Saraswati Patel
                'Desc: Change For Select only enable course name
                strbr = New StringBuilder()
                '********************************************************
                'Commented By Bhumi[24/8/2015]
                'strbr.Append(" select c.course_id,c.course_name from M_USER_INFO as u left join t_user_course as usco    ")
                'strbr.Append(" on usco.User_ID=u.userid left join m_course as c on c.course_id=usco.course_id ")
                'strbr.Append(" where u.center_id = ")
                'strbr.Append(sel_subjectid.SelectedValue)
                'strbr.Append("and c.Del_Flag=0")
                'strbr.Append(" group by c.course_name,usco.course_id,c.course_id ")
                'strbr.Append(" order by c.course_name ")
                'Ended by bhumi
                '************************************************************
                '************************************************************
                'Added By Bhumi [24/8/2015]
                'Reason:While Update the Center[class] of the student  not proper courses list come in dropdownlist of Course
                strbr.Append("SELECT tcc.Course_ID,mc.Course_name FROM T_Center_Course as tcc left join M_Course as mc ")
                strbr.Append("on tcc.Course_ID=mc.Course_id where tcc.Center_ID = ")
                strbr.Append(sel_subjectid.SelectedValue)
                strbr.Append(" and tcc.Del_Flag=0")
                strbr.Append(" order by mc.Course_name")
                sqlstr = strbr.ToString()
                'Ended by bhumi
                '************************************************************
                myDataReader = retTestInfo(sqlstr)

                myTable = New DataTable
                myTable.Columns.Add(New DataColumn("CourseID", GetType(String)))
                myTable.Columns.Add(New DataColumn("CourseName", GetType(String)))

                'While loop to populate the Datatable
                While myDataReader.Read
                    myRow = myTable.NewRow
                    myRow(0) = myDataReader.Item("Course_id")
                    myRow(1) = myDataReader.Item("Course_name")
                    myTable.Rows.Add(myRow)
                End While
                ddlcourse.DataSource = myTable
                ddlcourse.DataTextField = "CourseName"
                ddlcourse.DataValueField = "CourseID"
                ddlcourse.DataBind()
                ddlcourse.Items.Insert(0, New ListItem("--Select--", "0"))
                myDataReader.Close()

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
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
            End Try
        End Function

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
                'Where condition for del_flg added by bhumi [15/9/2015]
                'Reason: Display only those courses which are enable
                strbr.Append(" SELECT Center_Id,Center_Name FROM M_Centers WHERE Del_Flg=0 Order by Center_Name")
                sqlstr = strbr.ToString

                myDataReader = retTestInfo(sqlstr)

                myTable = New DataTable
                myTable.Columns.Add(New DataColumn("Center_Id", GetType(String)))
                myTable.Columns.Add(New DataColumn("Center_Name", GetType(String)))

                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                ' While loop to populate the Datatable
                If objconn.connect() Then
                    While myDataReader.Read
                        myRow = myTable.NewRow
                        myRow(0) = myDataReader.Item("Center_Id")
                        myRow(1) = myDataReader.Item("Center_Name")
                        myTable.Rows.Add(myRow)
                    End While
                End If
                sel_subjectid.DataSource = myTable
                sel_subjectid.DataTextField = "Center_Name"
                sel_subjectid.DataValueField = "Center_Id"
                sel_subjectid.DataBind()
                sel_subjectid.Items.Insert(0, New ListItem("--Select--", "0"))
                myDataReader.Close()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn.disconnect()
                sqlstr = Nothing
                myDataReader = Nothing
                myTable = Nothing
                myRow = Nothing
                strbr = Nothing
            End Try
        End Sub

#End Region

#Region "ForNewOrOld"

        'If new Search is clicked then
        Private Function ForNewOrOld()
            'Try


            '    If Not IsDBNull(Request("searchname")) And Request("searchname") <> "" Then
            '        'txtsearch.Text = Request("searchname")
            '        'txtsearch.Enabled = False
            '        'cmb_rpt.Visible = False
            '        'txtreport.Visible = True
            '        'txtreport.Enabled = False
            '    Else
            '        'txtsearch.Enabled = True
            '        'cmb_rpt.Visible = True
            '        'txtreport.Visible = False
            '        'If Not IsPostBack Then
            '        '    Dim strPathDb As String
            '        '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
            '        '    If objconn.connect(strPathDb) Then
            '        '        sqlstr = "SELECT Distinct report_name FROM t_report"
            '        '        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
            '        '        myDataReader = myCommand.ExecuteReader()
            '        '        myTable = New DataTable
            '        '        myTable.Columns.Add(New DataColumn("report_name", GetType(String)))
            '        '        While myDataReader.Read
            '        '            myRow = myTable.NewRow
            '        '            myRow(0) = myDataReader.Item("report_name")
            '        '            myTable.Rows.Add(myRow)
            '        '        End While
            '        '        cmb_rpt.DataSource = myTable
            '        '        cmb_rpt.DataValueField = "report_name"
            '        '        cmb_rpt.DataTextField = "report_name"
            '        '        cmb_rpt.DataBind()
            '        '        cmb_rpt.Items.Insert(0, "--Select--")
            '        '        myDataReader.Close()
            '        '        objconn.disconnect()
            '        '        If Not cmb_rpt.Items.Count >= 1 Then
            '        '            Response.Redirect("admin.aspx", False)
            '        '        End If
            '        '    End If
            '        'End If
            '    End If
            'Catch ex As Exception
            '    If log.IsDebugEnabled Then

            '        log.Debug("Error :" & ex.ToString())
            '    End If
            '    objconn.disconnect()
            'End Try

        End Function
#End Region

        '**************************************************************************
        'Function               :   gettxtbox
        '
        'Return                 :   TextBox
        '
        'Argument               :   txtboxname : text box name
        '                           
        '
        'Explanation            :   Create a new text box to feel text box
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "gettxtbox"
        Private Function gettxtbox(ByVal txtboxname As String) As TextBox
            Dim txtCriteria As New TextBox
            Try
                txtCriteria.ID = txtboxname
                gettxtbox = txtCriteria
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                txtCriteria = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getoperatorcombo
        '
        'Return                 :   Dropdownlist
        '
        'Argument               :   cmbboxname : dropdown listname 
        '                           
        '
        'Explanation            :   To create a drop down menu for operator
        '                           And & Or operator
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getoperatorcombo"
        Private Function getoperatorcombo(ByVal cmbboxname As String) As DropDownList
            Try
                combooperator = New DropDownList
                combooperator.Items.Add(New ListItem("And", "and"))
                combooperator.Items.Add(New ListItem("Or", "or"))
                combooperator.ID = cmbboxname
                getoperatorcombo = combooperator
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getcomparecombo
        '
        'Return                 :   Dropdownlist
        '
        'Argument               :   cmbboxname : dropdown listname 
        '                           
        '
        'Explanation            :   To create a drop down menu for operator like 
        '                           less than, greater than, equl to and not equel to 
        '                           
        'Note                   :   None
        '
        '**************************************************************************
        '
#Region "getcomparecombo"
        Private Function getcomparecombo(ByVal cmbboxname As String) As DropDownList
            Try
                comboLogOperator = New DropDownList
                comboLogOperator.Items.Add(New ListItem("=", "="))
                comboLogOperator.Items.Add(New ListItem("<", "<"))
                comboLogOperator.Items.Add(New ListItem(">", ">"))
                comboLogOperator.Items.Add(New ListItem("<>", "<>"))
                comboLogOperator.ID = cmbboxname
                getcomparecombo = comboLogOperator
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getcomparecombo
        '
        'Return                 :   None
        '
        'Argument               :   noOfSearchCri : number of search criteria
        '                           
        '
        'Explanation            :   This will generate the list of search criteria
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "gettable"
        Private Sub gettable()
            Dim row As Integer = 0
            ' Generate rows and cells.
            Dim j As Integer
            '***************************************
            Try

                strpathdb = ConfigurationSettings.AppSettings("PathDb")

                If Not Request("searchname") = "" Then
                    If objconn.connect() Then
                        sqlstr = "SELECT report_name, field_name,field_criteria FROM t_search_criteria "
                        sqlstr = sqlstr & " WHERE search_name='" & Request("searchname") & "'"
                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        fieldDataReader = myCommand.ExecuteReader()
                    End If
                    '*************************************** 
                    j = 0
                    While fieldDataReader.Read()
                        'for report name
                        'If Not IsDBNull(fieldDataReader.Item("report_name")) Then txtreport.Text = fieldDataReader.Item("report_name")
                        personaldetail()
                        'academicdetail()
                        'experiencedetail()

                    End While
                    '************************************
                    myCommand.Dispose()
                    fieldDataReader.Close()
                    objconn.disconnect()
                    '************************************
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                row = Nothing
                j = Nothing
            End Try
        End Sub
#End Region

#Region "personaldetail"
        Public Sub personaldetail()
            'for name
            'If fieldDataReader.Item("field_name") = "name" Then
            '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_name.Text = fieldDataReader.Item("field_criteria")
            'End If
            'for surname
            'If fieldDataReader.Item("field_name") = "surname" Then
            '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_surname.Text = fieldDataReader.Item("field_criteria")
            'End If
            'for birth date
            'If fieldDataReader.Item("field_name") = "birthdate" Then
            '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_birth.Text = fieldDataReader.Item("field_criteria")
            'End If
            'If fieldDataReader.Item("field_name") = "city" Then
            '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_city.Text = fieldDataReader.Item("field_criteria")
            'End If
            'If fieldDataReader.Item("field_name") = "CampusId" Then
            '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then cmbCampusid.SelectedValue = fieldDataReader.Item("field_criteria")
            'End If

        End Sub
#End Region

#Region "academicdetail"
        'Public Sub academicdetail()
        '    'academic detail---------------
        '    'If fieldDataReader.Item("field_name") = "academic_id" Then
        '    '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then cmb_grad.Value = fieldDataReader.Item("field_criteria")
        '    'End If

        '    'If fieldDataReader.Item("field_name") = "pass_year" Then
        '    '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_year.Text = fieldDataReader.Item("field_criteria")
        '    'End If

        '    'If fieldDataReader.Item("field_name") = "percentage" Then
        '    '    If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_percentage.Text = fieldDataReader.Item("field_criteria")
        '    'End If

        'End Sub
#End Region

        '#Region "experiencedetail"
        '        Public Sub experiencedetail()
        '            'experience detail
        '            If fieldDataReader.Item("field_name") = "prog_language" Then
        '                If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_progLang.Text = fieldDataReader.Item("field_criteria")
        '            End If

        '            If fieldDataReader.Item("field_name") = "database_known" Then
        '                If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_db.Text = fieldDataReader.Item("field_criteria")
        '            End If
        '            '------------------
        '            If fieldDataReader.Item("field_name") = "field" Then
        '                If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then sel_field.Value = fieldDataReader.Item("field_criteria")
        '            End If

        '            If fieldDataReader.Item("field_name") = "org_duration" Then
        '                If Not IsDBNull(fieldDataReader.Item("field_criteria")) Then txt_experienceMonths.Text = fieldDataReader.Item("field_criteria")
        '            End If

        '        End Sub
        '#End Region


        '**************************************************************************
        'Function               :   getField
        '
        'Return                 :   None
        '
        'Argument               :   strRptName : Report Name
        '                           srchName   : Search Name 
        '                           
        '
        'Explanation            :   This will list out the field name in drop down manu
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getField"

        Private Function getField(ByVal strRptName As String, ByVal srchName As String) As SqlDataReader
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim fieldDataReader As SqlDataReader
            'Dim strPathDb As String
            Try
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlstr = "SELECT field_name FROM t_search_criteria "
                    sqlstr = sqlstr & " WHERE report_name='reportname'"
                    'If Not IsDBNull(txtsearch.Text) And txtsearch.Text <> "" Then
                    '    sqlstr = sqlstr & " AND search_name='" & txtsearch.Text & "'"
                    'End If
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    fieldDataReader = myCommand.ExecuteReader()
                End If
                getField = fieldDataReader
                myCommand.Dispose()
                fieldDataReader.Close()
                objconn.disconnect()
            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                myCommand = Nothing
                fieldDataReader = Nothing
                sqlstr = Nothing
                strPathDb = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   addnew_ServerClick
        '
        'Return                 :   None
        '
        'Argument               :   sender : system object
        '                           e      : Event
        '                           
        '
        'Explanation            :   To delete the search criteria data from search table
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "removesearchcri"
        Private Sub removesearchcri()
            Dim objconn As New ConnectDb
            Dim strsqlDelete As String
            Dim myCommand As SqlCommand
            Dim j As Integer
            Try
                j = 0
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    strsqlDelete = " Delete from t_search_criteria WHERE"
                    If Not IsDBNull(Request("searchname")) And Request("searchname") <> "" Then
                        strsqlDelete = strsqlDelete & " search_name='" & Request("searchname") & "'"
                    Else
                        'strsqlDelete = strsqlDelete & " search_name='" & txtsearch.Text & "'"
                    End If
                    myCommand = New SqlCommand(strsqlDelete, objconn.MyConnection)
                    myCommand.ExecuteNonQuery()

                    strsqlDelete = " Delete from t_search_criteria WHERE"
                    If Not IsDBNull(Request("searchname")) And Request("searchname") <> "" Then
                        strsqlDelete = strsqlDelete & " report_name='" & Request("searchname") & "'"
                        strsqlDelete = strsqlDelete & " AND search_name='" & Request("searchname") & "'"
                    Else
                        'strsqlDelete = strsqlDelete & " report_name='" & cmb_rpt.SelectedItem.Value & "'"
                        'strsqlDelete = strsqlDelete & " AND search_name='" & txtsearch.Text & "'"
                    End If
                    myCommand = New SqlCommand(strsqlDelete, objconn.MyConnection)
                    myCommand.ExecuteNonQuery()

                    myCommand.Dispose()
                    objconn.disconnect()
                End If

            Catch ex As Exception
                objconn.disconnect()
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                strsqlDelete = Nothing
                j = Nothing
                myCommand = Nothing
            End Try
        End Sub
#End Region

        '**************************************************************************
        'Function               :   tmpsavesearchcri
        '
        'Return                 :   None
        '
        'Argument               :   None
        '                           
        '
        'Explanation            :   Save the search criteria in table 
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "saveSearchCri"
        Private Sub saveSearchCri(ByVal strCols As String, ByVal strCri As String)
            Dim objconn As New ConnectDb
            Dim strsqlinsert As String
            Dim myCommand As SqlCommand
            'Dim strPathDb As String
            Try

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then

                    'write a code to check the search name here
                    strsqlinsert = " INSERT INTO t_search_criteria "
                    strsqlinsert = strsqlinsert & "(search_name,report_name,field_name,field_criteria) "
                    strsqlinsert = strsqlinsert & "Values( "

                    If Not IsDBNull(Request("searchname")) And Request("searchname") <> "" Then
                        strsqlinsert = strsqlinsert & "'" & Replace(Request("searchname"), "'", "''") & "',"
                        'strsqlinsert = strsqlinsert & "'" & Replace(txtreport.Text, "'", "''") & "',"
                    Else
                        'strsqlinsert = strsqlinsert & "'" & Replace(txtsearch.Text, "'", "''") & "',"
                        'strsqlinsert = strsqlinsert & "'" & Replace(cmb_rpt.SelectedItem.Value, "'", "''") & "',"
                    End If

                    ''Code for dynamic search
                    strsqlinsert = strsqlinsert & "'" & Replace(strCols, "'", "''") & "',"
                    strsqlinsert = strsqlinsert & "'" & Replace(strCri, "'", "''") & "'"
                    strsqlinsert = strsqlinsert & " )"

                    strsqlinsert = Replace(strsqlinsert, "''", "NULL")
                    myCommand = New SqlCommand(strsqlinsert, objconn.MyConnection)
                    myCommand.ExecuteNonQuery()
                    myCommand.Dispose()
                    objconn.disconnect()
                End If

            Catch ex As Exception
                objconn.disconnect()
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                strsqlinsert = Nothing
                strPathDb = Nothing
                myCommand = Nothing
            End Try
        End Sub
#End Region

        '
        '**************************************************************************
        'Function               :   removelast_Click
        '
        'Return                 :   None
        '
        'Argument               :   sender : system object
        '                           e      : Event
        '
        'Explanation            :   This will delete the last record of search criteria
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "removelast_Click"
        Private Sub removelast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim iCnt As Integer
            Try
                If Not IsPostBack Then
                    iCnt = Session.Item("noOfSearchCri")
                    If iCnt < 0 Then
                        iCnt = 0
                    Else
                        iCnt = iCnt - 1
                    End If
                    Session.Add("noOfSearchCri", iCnt)
                    iCnt = Session.Item("noOfSearchCri")
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                'objconn.disconnect()
                iCnt = Nothing
            End Try
        End Sub
#End Region

        '**************************************************************************
        'Function               :   getResultDetails
        '
        'Return                 :   None
        '
        'Argument               :   sqlStr : sql query
        '
        'Explanation            :   Get the search result
        '                           
        'Note                   :   None
        '**************************************************************************
        '#Region "getResultDetails"
        '        Private Function getResultDetails(ByVal sqlStr As String) As String
        '            Dim objconn As New ConnectDb
        '            Dim myCommand As SqlCommand
        '            Dim myDataReader As SqlDataReader
        '            Dim ResultRow As HtmlTableRow
        '            Dim ResultCol As HtmlTableCell
        '            Dim iColumns As Integer
        '            Dim sqlStrHeader As String
        '            Dim strtbls As String
        '            Dim strcriteria As String
        '            Dim sqlSelect, sqlGroupby As String
        '            Dim i, j As Integer
        '            Dim links As Label
        '            Dim iuser As Integer
        '            Dim strbr As StringBuilder
        '            Dim strName As String
        '            Dim strSurname As String
        '            Dim strBirth As String
        '            Dim strPersonal As String

        '            Try
        '                'Dim strPathDb As String
        '                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        '                If objconn.connect() Then
        '                    'query to select the field name to be displayed
        '                    'sqlStrHeader = "SELECT tbl_name,field_name FROM m_fields "
        '                    'sqlStrHeader = sqlStrHeader & " WHERE field_name in ( "
        '                    'sqlStrHeader = sqlStrHeader & " SELECT field_name from t_report "
        '                    ''If Not IsDBNull(Request("searchname")) And Request("searchname") <> "" Then
        '                    ''    sqlStrHeader = sqlStrHeader & " WHERE report_name='" & txtreport.Text & "' )"
        '                    ''Else
        '                    ''    sqlStrHeader = sqlStrHeader & " WHERE report_name='" & cmb_rpt.SelectedItem.Value & "' )"
        '                    ''End If
        '                    'myCommand = New SqlCommand(sqlStrHeader, objconn.MyConnection)
        '                    'myDataReader = myCommand.ExecuteReader()
        '                    sqlSelect = ""
        '                    strtbls = ""
        '                    strcriteria = ""
        '                    sqlGroupby = ""

        '                    strbr = New StringBuilder

        '                    strbr.Append(" SELECT u.surname+' '+u.Name+' '+isnull(u.Middlename,'') as Name,u.email,u.userid,u.center_id,t.course_id,u.RollNo  FROM M_user_info as u  ")
        '                    strbr.Append(" inner join t_user_course as t on u.userid=t.user_id and t.course_id= '")
        '                    strbr.Append(ddlcourse.SelectedValue)
        '                    strbr.Append("'")
        '                    strbr.Append(" where u.userid NOT in ")
        '                    strbr.Append(" (select  userid from t_candidate_status where course_id = ")
        '                    strbr.Append("'")
        '                    strbr.Append(ddlcourse.SelectedValue)
        '                    strbr.Append("'")
        '                    strbr.Append("  ) ")
        '                    strbr.Append(" and u.Delete_Flg='0' and u.center_id= ")
        '                    strbr.Append("'")
        '                    strbr.Append(sel_subjectid.SelectedValue)
        '                    strbr.Append("'")


        '                    'If Not (ddlcourse.SelectedValue = 0) Then
        '                    '    strName = " AND Course_ID = " & ddlcourse.SelectedValue
        '                    'End If

        '                    If Not (sel_subjectid.SelectedValue = 0) Then
        '                        strName = " AND Center_ID = " & sel_subjectid.SelectedValue
        '                    End If

        '                    'If Not IsDBNull(txt_name.Text) And txt_name.Text <> "" Then
        '                    '    strName = strName & " AND u.name LIKE '%" & Replace(txt_name.Text, "'", "''") & "% OR' "
        '                    '     strName = strName & " AND u.surname LIKE '%" & Replace(txt_name.Text, "'", "''") & "%' "
        '                    'Else
        '                    '    strName = ""
        '                    'End If

        '                    'If Not IsDBNull(txt_surname.Text) And txt_surname.Text <> "" Then
        '                    '    strSurname = " AND u.surname LIKE '%" & Replace(txt_surname.Text, "'", "''") & "%' "
        '                    'Else
        '                    '    strSurname = ""
        '                    'End If

        '                    'If Not IsDBNull(txt_birth.Text) And txt_birth.Text <> "" Then
        '                    '    strBirth = " AND convert(int,Year(u.birthdate)) = " & Convert.ToInt32(txt_birth.Text) & ""
        '                    'Else
        '                    '    strBirth = ""
        '                    'End If

        '                    strPersonal = strName & strSurname & strBirth

        '                    sqlSelect = strbr.ToString & strPersonal & " group by t.user_id,t.course_id,u.Name,u.surname,u.email,u.userid,u.center_id,u.Middlename,u.RollNo Order By u.surname "

        '                    'While myDataReader.Read
        '                    '    sqlSelect = sqlSelect & myDataReader.Item("tbl_name") & "." & myDataReader.Item("field_name") & ","

        '                    '    If strtbls.IndexOf(myDataReader.Item("tbl_name")) < 0 Then
        '                    '        If strtbls = "" Then
        '                    '            strtbls = myDataReader.Item("tbl_name")
        '                    '        Else
        '                    '            strtbls = strtbls & ", " & myDataReader.Item("tbl_name")
        '                    '        End If
        '                    '    End If
        '                    'End While
        '                    'myDataReader.Close()
        '                    'strcriteria = "m_user_info LEFT JOIN m_user_academic ON m_user_info.userid = m_user_academic.userid"
        '                    'strcriteria = "m_user_experience RIGHT JOIN (" & strcriteria & " ) ON m_user_info.userid = m_user_experience.userid"
        '                    'strcriteria = "t_candidate_status RIGHT JOIN (" & strcriteria & " ) ON m_user_info.userid = t_candidate_status.userid"
        '                    'strcriteria = "m_testinfo RIGHT JOIN (" & strcriteria & " ) ON m_testinfo.test_type = t_candidate_status.test_type"

        '                    'sqlSelect = sqlSelect.TrimEnd(",")
        '                    'If cmb_rpt.SelectedIndex > 0 Then
        '                    '    If Not IsDBNull(sqlSelect) Then
        '                    '        sqlSelect = ", " & sqlSelect
        '                    '    End If
        '                    'End If
        '                    'sqlGroupby = sqlSelect
        '                    'sqlSelect = "SELECT m_user_info.userid " & sqlSelect & " FROM " & strcriteria & " where not m_user_info.userid = 1 " & getWhereCriteria()
        '                    'sqlSelect = sqlSelect & " Group by m_user_info.userid " & sqlGroupby
        '                    'objconn.disconnect()
        '                    'myCommand.Dispose()
        '                    'myDataReader.Close()

        '                    objconn.connect()

        '                    'Dim sqlorder As String
        '                    'If cmb_rpt.SelectedIndex > 0 Then
        '                    '    If Not IsDBNull(Request("searchname")) And Request("searchname") <> "" Then
        '                    '        sqlorder = "select order_by from t_report where report_name='" & txtreport.Text & "'"
        '                    '    Else
        '                    '        sqlorder = "select order_by from t_report where report_name='" & cmb_rpt.SelectedItem.Value & "'"
        '                    '    End If

        '                    '    myCommand = New SqlCommand(sqlorder, objconn.MyConnection)
        '                    '    myDataReader = myCommand.ExecuteReader()
        '                    '    If myDataReader.Read And Not IsDBNull(myDataReader.Item(0)) Then
        '                    '        If myDataReader.Item(0) = "name" Then
        '                    '            sqlSelect = sqlSelect & " Order by " & myDataReader.Item(0) & ", m_user_info.userid"
        '                    '        Else
        '                    '            sqlSelect = sqlSelect & " Order by " & myDataReader.Item(0)
        '                    '        End If
        '                    '    End If
        '                    'End If
        '                    'myCommand.Dispose()
        '                    'myDataReader.Close()

        '                    myCommand = New SqlCommand(sqlSelect, objconn.MyConnection)
        '                    myDataReader = myCommand.ExecuteReader()
        '                    iColumns = myDataReader.FieldCount()

        '                    'Change by Bhasker(30/11/09)
        '                    '********* Start *****************
        '                    If Not myDataReader.Read Then
        '                        lblReocrds.Visible = True
        '                        'tblResult.Visible = False
        '                        gridDiv.Visible = True
        '                        'examSection.Visible = False
        '                        '    emId.Visible = False
        '                        lblReocrds.Text = Resources.Resource.Common_NoRecFound
        '                        '   lblReocrds.Font.Bold = True

        '                        'ResultRow = New HtmlTableRow
        '                        'ResultCol = New HtmlTableCell
        '                        'ResultCol.InnerText = CStr("No Data found for above search")
        '                        'ResultRow.Cells.Add(ResultCol)
        '                        'ResultRow.Align = "center"
        '                        'tblResult.Rows.Insert(1, ResultRow)
        '                    Else
        '                        lblReocrds.Visible = False
        '                        'tblResult.Visible = True
        '                        gridDiv.Visible = True
        '                        'examSection.Visible = False
        '                        ' emId.Visible = True
        '                    End If
        '                    '********* End *****************
        '                    ReDim chkUsers(0)
        '                    j = 0
        '                    'creating dynamic table for displaying search result

        '                    'If cmb_rpt.SelectedIndex > 0 Then

        '                    If myDataReader.HasRows And iColumns <> 1 Then
        '                        ' tblResult.Visible = True
        '                        iuser = 0
        '                        Do
        '                            'If myDataReader.Item(0) <> iuser Then
        '                            'iuser = myDataReader.Item(0)
        '                            ResultRow = New HtmlTableRow

        '                            For i = 0 To iColumns - 1
        '                                If i = 0 Then
        '                                    ResultCol = New HtmlTableCell
        '                                    links = New Label
        '                                    'links.NavigateUrl = "register.aspx?userid=" & myDataReader.Item(0)
        '                                    links.Text = "" & j + 1 & ""
        '                                    ResultCol.Controls.Add(links)
        '                                    'ResultCol.Align = "center"
        '                                    'ResultCol.Width = "4%"
        '                                    'ResultCol.BorderColor = "#cccccc"
        '                                    ResultCol.Attributes.Add("align", "center")
        '                                    ResultCol.Attributes.Add("class", "tdDynaNum")
        '                                    ResultRow.Cells.Add(ResultCol)
        '                                End If

        '                                ' If Not IsDBNull(myDataReader.Item(i)) Then
        '                                'If IsDBNull(myDataReader.Item(i)) Then
        '                                ResultCol = New HtmlTableCell
        '                                If (myDataReader.GetName(i) = "Name") Then
        '                                    ResultCol.InnerText = CStr(myDataReader.Item("Name"))
        '                                    ResultCol.Attributes.Add("align", "left")
        '                                    ResultCol.Attributes.Add("class", "tdDynaText")
        '                                    ResultRow.Cells.Add(ResultCol)
        '                                    '  ResultCol.BorderColor = "#cccccc"
        '                                    'ElseIf (myDataReader.GetName(i) = "surname") Then
        '                                    '    ResultCol.InnerText = CStr(myDataReader.Item("surname"))
        '                                    '    ResultCol.Attributes.Add("align", "left")
        '                                    '    ResultCol.Attributes.Add("class", "tdDynaText")
        '                                    '    ResultRow.Cells.Add(ResultCol)
        '                                    '    '  ResultCol.BorderColor = "#cccccc"
        '                                ElseIf (myDataReader.GetName(i) = "RollNo") Then
        '                                    ResultCol.Attributes.Add("align", "left")
        '                                    ResultCol.Attributes.Add("class", "tdDynaText")
        '                                    'If Not IsDBNull(myDataReader.Item("email")) Or myDataReader.Item("email").ToString <> String.Empty Or myDataReader.Item("email").ToString <> Nothing Then
        '                                    'Dim strtemp As String = myDataReader.Item("email").ToString()
        '                                    'If strtemp = "" Then
        '                                    '    ResultCol.InnerText = "NA"
        '                                    'ElseIf myDataReader.Item("email") Is Nothing Then
        '                                    '    ResultCol.InnerText = "NA"
        '                                    'Else
        '                                    ResultCol.InnerText = CStr(myDataReader.Item("RollNo"))
        '                                    'End If
        '                                    'ResultCol.Width = "100"
        '                                    ResultRow.Cells.Add(ResultCol)
        '                                    '   ResultCol.BorderColor = "#cccccc"
        '                                    'Else
        '                                    '    ResultCol.InnerText = CStr(myDataReader.Item(0))
        '                                End If
        '                                ResultCol.Align = "Center"
        '                                'Else
        '                                '    If (myDataReader.GetName(i) = "percentage") Then
        '                                '        ResultCol.InnerText = CStr(getPercentage(myDataReader.Item(0)))
        '                                '        ResultCol.Align = "center"
        '                                '    End If
        '                                '    ResultCol.Controls.Add(New LiteralControl("&nIDbsp;"))
        '                                '    ResultRow.Cells.Add(ResultCol)

        '                                ' End If

        '                                'ResultRow.Cells.Add(ResultCol)
        '                                'If i = iColumns - 1 Then
        '                                If i = 5 Then
        '                                    ReDim Preserve chkUsers(UBound(chkUsers) + 1)
        '                                    ResultCol = New HtmlTableCell
        '                                    ResultCol.Controls.Add(getCheckbox(myDataReader.Item("userid"), j))
        '                                    ResultCol.Align = "Center"
        '                                    'ResultCol.BorderColor = "#cccccc"
        '                                    ResultRow.Cells.Add(ResultCol)
        '                                End If
        '                            Next
        '                            'tblResult.Border = 1
        '                            'tblResult.Rows.Insert(j + 1, ResultRow)
        '                            j = j + 1
        '                            'End If
        '                        Loop While myDataReader.Read
        '                        tblExam.Visible = True
        '                        gridDiv.Visible = True
        '                        'examSection.Visible = False
        '                    Else
        '                        'emId.Visible = False
        '                        gridDiv.Visible = False
        '                        'examSection.Visible = False
        '                        tblResult.Visible = False
        '                        tblExam.Visible = False
        '                        lblReocrds.Visible = True
        '                        lblReocrds.Text = Resources.Resource.Common_NoRecFound
        '                        lblReocrds.Font.Bold = True
        '                        tblResult.Width = "100%"
        '                    End If
        '                End If
        '                'End If
        '                myCommand.Dispose()
        '                myDataReader.Close()
        '                objconn.disconnect()

        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                objconn.disconnect()
        '                Throw ex
        '            Finally
        '                'objconn.disconnect()
        '                objconn = Nothing
        '                sqlStr = Nothing
        '                strtbls = Nothing
        '                strpathdb = Nothing
        '                strcriteria = Nothing
        '                sqlStrHeader = Nothing
        '                sqlSelect = Nothing
        '                iuser = Nothing
        '                links = Nothing
        '                myDataReader = Nothing
        '                myCommand = Nothing
        '                ResultCol = Nothing
        '                ResultRow = Nothing
        '                i = Nothing
        '                j = Nothing
        '                iColumns = Nothing
        '                strPersonal = Nothing
        '                strBirth = Nothing
        '                strbr = Nothing
        '                strSurname = Nothing
        '                strName = Nothing
        '                sqlSelect = Nothing
        '                sqlGroupby = Nothing

        '            End Try
        '        End Function
        '#End Region

        '**************************************************************************
        'Function               :   getOrgDuration
        '
        'Return                 :   iDuration 
        '
        'Argument               :   userid : User Id
        '
        'Explanation            :   Will return the Sum of Experience for that userId
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getOrgDuration"
        Private Function getOrgDuration(ByVal userid As Integer) As String
            Dim objconn As New ConnectDb
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim iDuration As Integer
            Dim strDuration As String
            Dim sqlStr As String
            'Dim strPathDb As String
            Try
                iDuration = 0
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlStr = "SELECT sum(org_duration) as duration from m_user_experience "
                    sqlStr = sqlStr & "WHERE userid=" & userid
                    myCommand = New SqlCommand(sqlStr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    If myDataReader.Read() Then
                        If Not IsDBNull(myDataReader.Item("duration")) Then
                            iDuration = myDataReader.Item("duration")
                            If iDuration > 12 Then
                                strDuration = CStr(Fix(iDuration / 12)) & "Yr "
                                If (iDuration Mod 12) <> 0 Then
                                    strDuration = strDuration & CStr(iDuration Mod 12) & "M"
                                End If
                            Else
                                strDuration = iDuration & "M"
                            End If
                        End If
                    End If
                End If
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()
                getOrgDuration = strDuration

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                iDuration = Nothing
                strDuration = Nothing
                sqlStr = Nothing
                strPathDb = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getOrgDuration
        '
        'Return                 :   iDuration 
        '
        'Argument               :   userid : User Id
        '
        'Explanation            :   Will return the Sum of Experience for that userId
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getPercentage"
        Private Function getPercentage(ByVal userid As Integer) As Integer
            Dim objconn As New ConnectDb
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim intPerc As Integer
            Dim sqlStr As String
            'Dim strPathDb As String
            Try

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlStr = "SELECT [Percentage] FROM (m_user_academic) WHERE userid = " & userid & " and academic_id=(                          SELECT max(u.academic_id) FROM m_user_academic u where u.userid=" & userid & "and percentage > 0)"
                    myCommand = New SqlCommand(sqlStr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    If myDataReader.Read() Then
                        intPerc = myDataReader(0)
                    Else
                        objconn.disconnect()
                    End If
                End If
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()
                getPercentage = intPerc

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                intPerc = Nothing
                sqlStr = Nothing
                strPathDb = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getCheckbox
        '
        'Return                 :   HtmlInputCheckBox
        '
        'Argument               :   userid : User Id
        '
        'Explanation            :   This will generate check box for each user
        '                           
        'Note                   :   None
        '**************************************************************************

        'Commented By Pranit on 26/11/2019
        '#Region "getCheckbox"
        '        Private Function getCheckbox(ByVal userid As String, ByVal j As Integer) As HtmlInputCheckBox
        '            Try
        '                chkUsers(j) = New HtmlInputCheckBox
        '                chkUsers(j).Value = userid
        '                chkUsers(j).ID = userid
        '                getCheckbox = chkUsers(j)

        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Throw ex

        '            End Try
        '        End Function
        '#End Region

        '**************************************************************************
        'Function               :   getSelectCriteria
        '
        'Return                 :   HtmlInputCheckBox
        '
        'Argument               :   sqlstr : Query String
        '
        'Explanation            :   This will get the field to be displayed in report
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getSelectCriteria"
        Private Function getSelectCriteria(ByVal sqlStr As String) As String
            Dim objconn As New ConnectDb
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim strFields As String
            'Dim strPathDb As String
            Try
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    myCommand = New SqlCommand(sqlStr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read
                        strFields = myDataReader.Item("field_name") & ","
                    End While
                End If
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()
                getSelectCriteria = strFields

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                strFields = Nothing
                strPathDb = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getWhereCriteria
        '
        'Return                 :   None
        '
        'Argument               :   None
        '
        'Explanation            :   Create a Where criteria from the search created.
        '                           
        'Note                   :   None
        '**************************************************************************
        '#Region "getWhereCriteria"
        '        Private Function getWhereCriteria() As String
        '            Dim strPersonal, strName, strSurname, strBirth, strCity As String    'Personal Info
        '            Dim strAcademic, strGraduate, strYear, strPercentage As String           'Academic
        '            Dim strComputer, strProgLang, strDatabase As String                   'Computer
        '            Dim strExperience, strExpField, strExpMonths As String                'Experience
        '            Dim strSelFrom, strSelWhere, strSelect
        '            Dim strCampusid As String                         'campusid
        '            Dim strTest                                                           'Test

        '            Try
        '                ''Code for Personal 

        '                If Not (ddlcourse.SelectedValue = 0) Then
        '                    strName = " AND Course_ID = " & ddlcourse.SelectedValue
        '                End If

        '                If Not (sel_subjectid.SelectedValue = 0) Then
        '                    strName = strName & " AND Center_ID = " & sel_subjectid.SelectedValue
        '                End If

        '                If Not IsDBNull(txt_name.Text) And txt_name.Text <> "" Then
        '                    strName = strName & " AND m_User_Info.name LIKE '%" & Replace(txt_name.Text, "'", "''") & "%' "
        '                Else
        '                    strName = ""
        '                End If
        '                'changed by sandeep for taking the value of campus_id field in strcampusid
        '                'If Not IsDBNull(cmbCampusid.SelectedItem.Text) And cmbCampusid.SelectedItem.Text <> "--Select--" Then
        '                '    strCampusid = "And m_user_info.campus_id = '" & cmbCampusid.SelectedItem.Text.ToString() & "'"
        '                'Else
        '                '    strCampusid = ""
        '                'End If
        '                'end change by sandeep
        '                If Not IsDBNull(txt_surname.Text) And txt_surname.Text <> "" Then
        '                    strSurname = " AND m_User_Info.surname LIKE '%" & Replace(txt_surname.Text, "'", "''") & "%' "
        '                Else
        '                    strSurname = ""
        '                End If

        '                If Not IsDBNull(txt_birth.Text) And txt_birth.Text <> "" Then
        '                    strBirth = " AND convert(int,Year(m_User_Info.birthdate)) = " & Convert.ToInt32(txt_birth.Text) & ""
        '                Else
        '                    strBirth = ""
        '                End If

        '                'If Not IsDBNull(txt_city.Text) And txt_city.Text <> "" Then
        '                '    strCity = "AND m_User_Info.city LIKE '%" & Replace(txt_city.Text, "'", "''") & "%'"

        '                'Else
        '                '    strCity = ""
        '                'End If
        '                'changed by sandeep for adding the strcampusid
        '                strPersonal = strName & strCampusid & strSurname & strBirth & strCity
        '                'end change by sandeep

        '                ''Code for Academic Information
        '                'If Not IsDBNull(cmb_grad.Value) And cmb_grad.Value <> "" Then
        '                '    strGraduate = "AND m_user_academic.academic_type LIKE '%" & Replace(cmb_grad.Value, "'", "''") & "%'"
        '                '    If cmb_grad.Value = "OT" And Not IsDBNull(txt_other.Text) And txt_other.Text <> "" Then
        '                '        strGraduate = strGraduate & " AND m_user_academic.other_name LIKE '%" & Replace(txt_other.Text, "'", "''") & "%'"
        '                '    ElseIf cmb_grad.Value <> "OT" And Not IsDBNull(txt_other.Text) And txt_other.Text <> "" Then
        '                '        strGraduate = strGraduate & " AND m_user_academic.specialization LIKE '%" & Replace(txt_other.Text, "'", "''") & "%'"
        '                '    End If
        '                '    strGraduate = strGraduate & " AND m_user_academic.percentage > 0 "
        '                'Else
        '                '    strGraduate = ""
        '                'End If

        '                'If Not IsDBNull(txt_year.Text) And txt_year.Text <> "" Then
        '                '    strYear = "AND m_user_academic.pass_year = " & Replace(txt_year.Text, "'", "''") & ""
        '                'Else
        '                '    strYear = ""
        '                'End If

        '                'If Not IsDBNull(txt_percentage.Text) And txt_percentage.Text <> "" Then
        '                '    strPercentage = "AND academic_id=3 OR academic_id=4 AND m_user_academic.percentage >= " & txt_percentage.Text
        '                'Else
        '                '    strPercentage = ""
        '                'End If

        '                'Coded aded by kamal
        '                'If lsttest.SelectedIndex > 0 And lstsymbol.SelectedIndex > 0 Then
        '                '    If lstsymbol.SelectedIndex > 1 Then
        '                '        strTest = "AND m_testinfo.test_type " & lstsymbol.SelectedValue & " '" & lsttest.SelectedValue & "'"
        '                '    Else
        '                '        strTest = "AND ( m_testinfo.test_type " & lstsymbol.SelectedValue & " '" & lsttest.SelectedValue & "' or test_name is null ) "
        '                '    End If
        '                'Else
        '                '    strTest = ""
        '                'End If
        '                'End of kamal

        '                strAcademic = strGraduate & strYear & strPercentage & strTest

        '                ''Code for Computer
        '                'If Not IsDBNull(txt_progLang.Text) And txt_progLang.Text <> "" Then
        '                '    strProgLang = "AND m_user_info.prog_language LIKE '%" & Replace(txt_progLang.Text, "'", "''") & "%'"
        '                'Else
        '                '    strProgLang = ""
        '                'End If

        '                'If Not IsDBNull(txt_db.Text) And txt_db.Text <> "" Then
        '                '    strDatabase = "AND m_user_info.database_known LIKE '%" & Replace(txt_db.Text, "'", "''") & "%'"
        '                'Else
        '                '    strDatabase = ""
        '                'End If

        '                strPersonal = strPersonal & strProgLang & strDatabase

        '                ''Code for Experience
        '                'If Not IsDBNull(sel_field.Value) And sel_field.Value <> "" Then
        '                '    strExpField = "AND m_user_experience.field LIKE '%" & Replace(sel_field.Value, "'", "''") & "%'"
        '                'Else
        '                '    strExpField = ""
        '                'End If

        '                'If Not IsDBNull(txt_experienceMonths.Text) And txt_experienceMonths.Text <> "" Then
        '                '    strExpMonths = "AND m_user_info.userid IN (SELECT userid from m_user_experience where org_duration >= " & Replace(txt_experienceMonths.Text, "'", "''") & ") "
        '                'Else
        '                '    strExpMonths = ""
        '                'End If

        '                strExperience = strExpField & strExpMonths

        '                Dim strTbls As String
        '                Dim strJoins As String

        '                If strAcademic <> "" And strPersonal <> "" And strExperience <> "" Then
        '                    strJoins = " m_user_info.userid=m_user_academic.userid AND m_user_info.userid=m_user_experience.userid AND m_user_experience.userid=m_user_academic.userid AND "
        '                    strTbls = ",m_user_academic,m_user_experience "
        '                ElseIf strPersonal <> "" And strAcademic <> "" Then
        '                    strJoins = " m_user_info.userid=m_user_academic.userid AND  "
        '                    strTbls = ",m_user_academic "
        '                ElseIf strPersonal <> "" And strExperience <> "" Then
        '                    strJoins = " m_user_info.userid=m_user_experience.userid AND  "
        '                    strTbls = ",m_user_experience "
        '                ElseIf strAcademic <> "" And strExperience <> "" Then
        '                    strJoins = " m_user_info.userid=m_user_experience.userid AND m_user_experience.userid=m_user_academic.userid AND m_user_info.userid=m_user_academic.userid AND "
        '                    strTbls = ",m_user_academic,m_user_experience "
        '                ElseIf strPersonal <> "" Then
        '                    strTbls = ""
        '                ElseIf strAcademic <> "" Then
        '                    strJoins = " m_user_info.userid=m_user_academic.userid AND "
        '                    strTbls = ",m_user_academic "
        '                ElseIf strExperience <> "" Then
        '                    strJoins = " m_user_info.userid=m_user_academic.userid AND "
        '                    strTbls = ",m_user_experience "
        '                End If

        '                strSelFrom = strTbls
        '                strSelWhere = strPersonal & strAcademic & strExperience

        '                If strSelWhere = "" Then
        '                    strSelect = ""
        '                Else

        '                    strSelect = strSelWhere
        '                End If

        '                getWhereCriteria = strSelect

        '                ''Code for dynamic search
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                lblMsg.Text = ex.Message()
        '            Finally
        '                'objconn.disconnect()
        '                strPersonal = Nothing
        '                strName = Nothing
        '                strSurname = Nothing
        '                strBirth = Nothing
        '                strCity = Nothing
        '                strAcademic = Nothing
        '                strGraduate = Nothing
        '                strYear = Nothing
        '                strPercentage = Nothing
        '                strComputer = Nothing
        '                strProgLang = Nothing
        '                strDatabase = Nothing
        '                strExperience = Nothing
        '                strExpField = Nothing
        '                strExpMonths = Nothing
        '                strSelFrom = Nothing
        '                strSelWhere = Nothing
        '                strSelect = Nothing
        '                strCampusid = Nothing
        '                strTest = Nothing

        '            End Try
        '        End Function
        '#End Region

#Region "forMultipleTbl"
        Private Function forMultipleTbl(ByVal strSql As String)
            Dim strSqlNew As String
            Dim tbls As String
            Dim whereCri As String
            Dim selectPart As String
            Dim iFromPosition As Integer
            Dim iWherePosition As Integer
            Dim aStrSql As Array
            Dim iCnt As Integer

            Try
                iCnt = 0
                strSql = strSql.Replace(" FROM ", " | ")
                strSql = strSql.Replace(" WHERE ", " | ")
                aStrSql = strSql.Split("|")
                selectPart = aStrSql(0)
                tbls = aStrSql(1)
                whereCri = aStrSql(2)
                'join m_user_info and m_user_academic table
                If selectPart.IndexOf("m_user_academic") > 0 And tbls.IndexOf("m_user_academic") < 0 Then
                    tbls = tbls & ",m_user_academic "
                    whereCri = whereCri & " AND m_user_info.userid=m_user_academic.userid "
                    iCnt = iCnt + 1
                End If

                'join m_user_info and m_user_experience table
                If selectPart.IndexOf("m_user_experience") > 0 And tbls.IndexOf("m_user_experience") < 0 Then
                    tbls = tbls & ",m_user_experience "
                    whereCri = whereCri & " AND m_user_info.userid=m_user_experience.userid "
                    iCnt = iCnt + 1
                End If

                'join m_user_experience and m_user_academic table
                If iCnt <> 0 And Not tbls.IndexOf("m_user_academic") >= 0 And Not tbls.IndexOf("m_user_experience") >= 0 Then
                    whereCri = whereCri & " AND m_user_academic.userid=m_user_experience.userid "
                End If

                strSqlNew = selectPart & " FROM " & tbls & "WHERE" & whereCri

                forMultipleTbl = strSqlNew

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                strSqlNew = Nothing
                aStrSql = Nothing
                iCnt = Nothing
                iWherePosition = Nothing
                iFromPosition = Nothing
                selectPart = Nothing
                whereCri = Nothing
                tbls = Nothing

            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getResultHeaders
        '
        'Return                 :   None
        '
        'Argument               :   None
        '
        'Explanation            :   Generate the header for search result
        '                           
        'Note                   :   None
        '**************************************************************************
        '#Region "getResultHeaders"
        '        Private Sub getResultHeaders()
        '            Try
        '                Dim objconn As New ConnectDb
        '                Dim sqlstr As String
        '                Dim myCommand As SqlCommand
        '                Dim myDataReader As SqlDataReader
        '                Dim HeaderRow As HtmlTableRow
        '                Dim HeaderCol As HtmlTableCell

        '                'Dim strPathDb As String
        '                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
        '                If objconn.connect() Then

        '                    HeaderRow = New HtmlTableRow
        '                    HeaderRow.Attributes.Add("style", "COLOR:white; FONT-WEIGHT:bold") 'Coded by kamal on 2006/01/28
        '                    HeaderRow.Attributes.Add("bgcolor", "#189599")
        '                    'display the search result header
        '                    HeaderCol = New HtmlTableCell
        '                    HeaderCol.InnerText = Resources.Resource.Common_SrNo
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    HeaderRow.Cells.Add(HeaderCol)

        '                    HeaderCol = New HtmlTableCell
        '                    HeaderCol.InnerText = Resources.Resource.StudentSearch_CndNm
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    HeaderRow.Cells.Add(HeaderCol)

        '                    'HeaderCol = New HtmlTableCell
        '                    'HeaderCol.InnerText = "Sur name"
        '                    'HeaderCol.Attributes.Add("class", "tblhead")
        '                    'HeaderRow.Cells.Add(HeaderCol)

        '                    HeaderCol = New HtmlTableCell
        '                    HeaderCol.InnerText = Resources.Resource.StudentSearch_RNo
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    HeaderRow.Cells.Add(HeaderCol)


        '                    'While myDataReader.Read
        '                    '    HeaderCol = New HtmlTableCell
        '                    '    HeaderCol.InnerText = "" & myDataReader.Item("display_name")
        '                    '    HeaderCol.Align = "center"
        '                    '    HeaderCol.BgColor = "#006699" 'Coded by kamal on 2006/01/28
        '                    '    HeaderRow.Cells.Add(HeaderCol)
        '                    'End While
        '                    '**********modification starts from here
        '                    HeaderCol = New HtmlTableCell
        '                    'HeaderCol.BorderColor = "#cccccc"
        '                    rdbSelectall.GroupName = "ss"
        '                    rdbReverseall.GroupName = "ss"
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    'Change by bhasker(30/11/09)
        '                    '********* Start *****************
        '                    rdbSelectall.ID = "rbtSelectAll"
        '                    rdbReverseall.ID = "rbtReverseAll"
        '                    '********* End *****************
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    rdbSelectall.Text = Resources.Resource.Common_SelectAll
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    HeaderCol.Controls.Add(rdbSelectall)
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    rdbReverseall.Text = Resources.Resource.Common_DeSelAll
        '                    HeaderCol.Controls.Add(rdbReverseall)
        '                    'HeaderCol.Align = "center"
        '                    'HeaderCol.BgColor = "#006699" 'Coded by kamal on 2006/01/28
        '                    HeaderCol.Attributes.Add("class", "tblhead")
        '                    HeaderRow.Cells.Add(HeaderCol)
        '                    tblResult.Rows.Insert(0, HeaderRow)
        '                End If
        '                'myCommand.Dispose()
        '                'myDataReader.Close()
        '                objconn.disconnect()

        '                '*************modified  code
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Throw ex
        '            Finally
        '                'objconn.disconnect()
        '                objconn = Nothing
        '                sqlstr = Nothing
        '                myCommand = Nothing
        '                myDataReader = Nothing
        '                strpathdb = Nothing
        '            End Try
        '        End Sub
        '#End Region

        '**************************************************************************
        'Function               :   UpdForOnlineTest
        '
        'Return                 :   None
        '
        'Argument               :   None
        '
        'Explanation            :   Update record for user selected for the online
        '                           examination
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "UpdForOnlineTest"
        Private Function UpdForOnlineTest(formatDate As String) ' As String()
            '***************************************************************************************
            'Code modified by Tabrez
            'Purpose: Date Validation
            '***************************************************************************************
            Dim testtypevalue As Integer
            Dim AtLeastOneChecked As Boolean = False
            Dim intquestcnt1(testtypevalue) As Integer
            Dim intlques1(testtypevalue) As Integer
            Dim intmques1(testtypevalue) As Integer
            Dim inthques1(testtypevalue) As Integer
            Dim totalquestcnt As Integer
            Dim objCommand As SqlCommand                   'SqlCommand object
            Dim objDataReader As SqlDataReader
            Dim intquestcntal As New ArrayList
            Dim intlquesal As New ArrayList
            Dim intmquesal As New ArrayList
            Dim inthquesal As New ArrayList
            Dim myCommand1 As SqlTransaction
            ' Dim UserInfo(5) As String
            Dim sendingFail As String
            Dim Send_Fail As String
            Try

                If formatDate = "" Then
                    lblMsg.ForeColor = Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.Search_EmExamDate
                    lblMsg.Visible = True
                    Exit Function
                End If

                If formatDate <> "" Then
                    Dim YrDt() As String = Split(ConvertDate(formatDate), "/")
                    Dim VldDate As Boolean = ValidateDate(YrDt)
                    Dim objconn As New ConnectDb
                    Dim strsqlinsert As String
                    Dim strSqlSel As String
                    Dim myCommand As SqlCommand
                    '   Dim myCommand1 As SqlTransaction

                    Dim j As Integer

                    Dim emailId As String
                    Dim myDataReader As SqlDataReader
                    Dim strPathDb As String
                    If VldDate = False Then
                        lblMsg.ForeColor = Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.Search_ExamDueInFut
                        lblMsg.Visible = True
                        Exit Function
                    Else
                        lblMsg.Visible = False
                    End If
                    'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                    j = 0
                    'code added by kamal
                    If objconn.connect() Then
                        '***************************************************************************************
                        'Code modified by Tabrez
                        'Purpose: Acknowledging mail message status
                        '***************************************************************************************
                        Dim StrTemp As String = "User(s):"
                        Dim blnCheck As Boolean = True
                        Dim cid As String
                        'objconn.connect(strPathDb)
                        For Each rowItem As DataGridItem In DGData.Items
                            chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                            If chk.Checked Then
                                cid = rowItem.Cells(1).Text

                                'For j = 0 To UBound(chkUsers) - 1
                                '    Dim StrChkApp As String = "User(s):"
                                '    If chkUsers(j).Checked = True Then
                                AtLeastOneChecked = True
                                strSqlSel = " SELECT userid from t_candidate_status WHERE userid=" & cid
                                strSqlSel = strSqlSel & " AND Course_ID = '" & ddlcourse.SelectedItem.Value & "'"
                                myCommand = New SqlCommand(strSqlSel, objconn.MyConnection)

                                myDataReader = myCommand.ExecuteReader()
                                If myDataReader.Read Then
                                    'Person already appeared for test or mail is already sent to him
                                    'commented by kamal
                                    myDataReader.Close()
                                    'objconn.disconnect()
                                    'StrTemp = StrTemp & GetUserNameFromID(chkUsers(j).Value, sel_test_type.SelectedItem.Value) & ","
                                    'StrChkApp = StrChkApp & GetUserNameFromID(chkUsers(j).Value, sel_test_type.SelectedItem.Value)
                                    'If UCase(StrChkApp) = UCase("User(s):") Then
                                    '    emailId = getEmailId(chkUsers(j).Value)
                                    '    strsqlinsert = "update t_candidate_status set written_test_date = '" & txtExamDate.Text & " '" & _
                                    '                    " where userid = " & chkUsers(j).Value & " and test_type = '" & sel_test_type.SelectedItem.Value & "'"
                                    '    objconn.connect(strPathDb)
                                    '    myCommand.Connection = objconn.MyConnection
                                    '    myCommand = New SqlCommand(strsqlinsert, objconn.MyConnection)
                                    '    myCommand1 = objconn.MyConnection.BeginTransaction()
                                    '    myCommand.Transaction = myCommand1
                                    '    myCommand.ExecuteNonQuery()
                                    '    myCommand1.Commit()
                                    '    MailForOnlineTest(CInt(chkUsers(j).Value))
                                    'Else
                                    '    blnCheck = False
                                    'End If
                                Else
                                    myDataReader.Close()

                                    'strSqlSel = "Select Count(*) from m_testinfo, m_question where m_testinfo.Course_ID = '" & _
                                    '            ddlcourse.SelectedValue & "' " & _
                                    '            " and Del_Flag='0'  and m_testinfo.test_type = m_question.test_type" & _
                                    '            " group by m_testinfo.test_Type " & _
                                    '            "having Count(*) < ( select Count(t.no_of_ques) from m_testinfo t " & _
                                    '            "where t.Course_ID= '" & ddlcourse.SelectedValue & "' and Del_Flag='0' )"


                                    '/*******************************start********************************/
                                    'Desc:Commented due to check against a new functionality.
                                    'By: Jatin Gangajaliya, 2011/04/22.

                                    'strSqlSel = "Select Distinct m_question.test_type from m_question,m_testinfo where m_testinfo.Course_id='" & ddlcourse.SelectedValue & "' and m_testinfo.test_type=m_question.test_type and m_testinfo.Del_Flag='0'"
                                    ''myCommand = New SqlCommand(strSqlSel, objconn.MyConnection)
                                    'Dim adp As New SqlDataAdapter(strSqlSel, objconn.MyConnection)
                                    'Dim dt As New DataTable()
                                    'adp.Fill(dt)
                                    'Dim strSqlSel1 As String = "Select Count(test_type) as test_type from m_testinfo where m_testinfo.Course_id='" & ddlcourse.SelectedValue & "' and  Del_Flag='0'"
                                    '' myCommand = New SqlCommand(strSqlSel1, objconn.MyConnection)
                                    'Dim adp1 As New SqlDataAdapter(strSqlSel1, objconn.MyConnection)
                                    'Dim dt1 As New DataTable()
                                    'adp1.Fill(dt1)
                                    'If (dt.Rows.Count = 0) Then
                                    '    lblMsg.ForeColor = Color.FromName("Red")
                                    '    lblMsg.Text = "There is no enough question for the test. Please verify!!"
                                    '    lblMsg.Visible = True
                                    '    Exit Function
                                    'End If

                                    'If (dt1.Rows.Count > 0) Or (dt.Rows.Count > 0) Then
                                    '    Dim rows As Integer = dt.Rows.Count
                                    '    Dim totaltesttype As Integer = Convert.ToInt32(dt1.Rows(0)("test_type"))
                                    '    If (rows <> totaltesttype) Then
                                    '        lblMsg.ForeColor = Color.FromName("Red")
                                    '        lblMsg.Text = "There is no enough question for the test. Please verify!!"
                                    '        lblMsg.Visible = True
                                    '        Exit Function
                                    '    ElseIf (rows = totaltesttype) Then
                                    '        Dim testtypevaluearr(testtypevalue) As Integer
                                    '        Dim testtypevalueal As New ArrayList
                                    '        Dim test_types As String = "Select test_type From m_testinfo WHERE course_id= '" & ddlcourse.SelectedValue & "' and del_flag='0'"
                                    '        objCommand = New SqlCommand(test_types, objconn.MyConnection)
                                    '        objDataReader = objCommand.ExecuteReader()
                                    '        While objDataReader.Read
                                    '            testtypevalueal.Add(objDataReader.Item("test_type"))
                                    '        End While
                                    '        testtypevaluearr = testtypevalueal.ToArray(System.Type.GetType("System.Int32"))
                                    '        Session.Add("AllTestTypeValue", testtypevaluearr)
                                    '        objDataReader.Close()
                                    '        objCommand.Dispose()

                                    '        For k As Integer = 0 To testtypevaluearr.Length - 1
                                    '            Dim strSql As String = "SELECT test_name,no_of_ques,l_ques,m_ques,h_ques FROM m_testinfo WHERE Course_ID = '" & ddlcourse.SelectedValue & "' and test_type='" & testtypevaluearr(k) & "' and del_flag='0'"
                                    '            objCommand = New SqlCommand(strSql, objconn.MyConnection)
                                    '            objDataReader = objCommand.ExecuteReader
                                    '            If objDataReader.Read() Then
                                    '                intquestcntal.Add(objDataReader.Item("no_of_ques"))
                                    '                intlquesal.Add(objDataReader.Item("l_ques"))
                                    '                intmquesal.Add(objDataReader.Item("m_ques"))
                                    '                inthquesal.Add(objDataReader.Item("h_ques"))
                                    '            End If
                                    '            objDataReader.Close()
                                    '        Next
                                    '        objCommand.Dispose()
                                    '        intquestcnt1 = intquestcntal.ToArray(System.Type.GetType("System.Int32"))
                                    '        intlques1 = intlquesal.ToArray(System.Type.GetType("System.Int32"))
                                    '        intmques1 = intmquesal.ToArray(System.Type.GetType("System.Int32"))
                                    '        inthques1 = inthquesal.ToArray(System.Type.GetType("System.Int32"))
                                    '        totalquestcnt = 0
                                    '        For h As Integer = 0 To testtypevaluearr.Length - 1
                                    '            totalquestcnt = totalquestcnt + intquestcnt1(h)
                                    '        Next
                                    '        For k As Integer = 0 To testtypevaluearr.Length - 1
                                    '            For level As Integer = 0 To 2
                                    '                Dim strSqlSel3 As String = "Select Count(m_question.question) as qtn,Count(m_question.qlevel) As level from m_question,m_testinfo where m_testinfo.Course_id='" & ddlcourse.SelectedValue & "' and m_question.test_type='" & testtypevaluearr(k) & "' and m_question.test_type=m_testinfo.test_type and qlevel='" & level & "'"
                                    '                Dim adp2 As New SqlDataAdapter(strSqlSel3, objconn.MyConnection)
                                    '                Dim dt2 As New DataTable()
                                    '                adp2.Fill(dt2)
                                    '                If (dt2.Rows.Count > 0) Then
                                    '                    Dim valueis As Integer = Convert.ToInt32(dt2.Rows(0)("qtn"))
                                    '                    Dim levelValue As Integer = Convert.ToInt32(dt2.Rows(0)("level"))
                                    '                    'If (valueis = totalquestcnt) Or (valueis >= totalquestcnt) Then
                                    '                    If (levelValue = intlques1(k)) Or levelValue >= intlques1(k) Then
                                    '                    ElseIf (levelValue = intmques1(k)) Or levelValue >= intmques1(k) Then
                                    '                    ElseIf (levelValue = inthques1(k)) Or levelValue >= inthques1(k) Then
                                    '                    Else
                                    '                        lblMsg.ForeColor = Color.FromName("Red")
                                    '                        lblMsg.Text = "There is no enough question for the test. Please verify!!"
                                    '                        lblMsg.Visible = True
                                    '                        Exit Function
                                    '                    End If
                                    '                    'End If
                                    '                End If
                                    '            Next
                                    '        Next
                                    '    End If
                                    'End If
                                    '/*******************************End******************************/



                                    '/***************************************Start**************************************/
                                    'Desc:Added By: Jatin Gangajaliya, 2011/04/22.
                                    'Reason:To do checking of availability of questions based on new database design.

                                    Dim item As DictionaryEntry
                                    Dim objht As New Hashtable
                                    objht = CheckQuestions(ddlcourse.SelectedValue)
                                    Dim inttotalque As Integer
                                    Dim cmd As SqlCommand
                                    Dim sb As StringBuilder
                                    Dim strquery As String
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
                                                'If objconn.connect(strPathDb) Then
                                                cmd = New SqlCommand(strquery, objconn.MyConnection)
                                                inttotalque = cmd.ExecuteScalar()
                                                ' End If
                                                If inttotalque < ary(g) Then
                                                    lblMsg.ForeColor = Color.FromName("Red")
                                                    lblMsg.Text = Resources.Resource.Search_NotEnoughQ
                                                    lblMsg.Visible = True
                                                    Exit Function

                                                End If
                                            Next
                                        Next

                                    Catch ex As Exception
                                        If log.IsDebugEnabled Then
                                            log.Debug("Error :" & ex.ToString())
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
                                    '/****************************************End***************************************/


                                End If
                                'emailId = getEmailId(chkUsers(j).Value)

                                '/************************Start,Jatin Gangajaliya,2011/04/04*************************************/
                                Dim rdr As SqlDataReader
                                Dim strbr As New StringBuilder()
                                strbr = New StringBuilder
                                strbr.Append(" Select Total_Time,Total_marks,Total_passmarks From M_Course where Course_id =  ")
                                strbr.Append(ddlcourse.SelectedValue)
                                Dim strq As String = strbr.ToString()
                                myCommand.Connection = objconn.MyConnection
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

                                Dim strpassword As String = GetRandomPasswordUsingGUID()
                                'Dim strname As String = tblResult.Rows(j + 1).Cells(1).InnerText
                                Dim strname As String = rowItem.Cells(1).Text
                                '  Dim struserid As String = strname & chkUsers(j).Value
                                Dim struserid As String = GetRollNumber(cid)

                                'Enter record for user who is selected to appear for online examination

                                If (struserid <> String.Empty And strpassword <> String.Empty) Then
                                    'UserInfo(0) = chkUsers(j).Value
                                    'UserInfo(1) = strpassword
                                    'UserInfo(2) = ddlcourse.SelectedItem.Text
                                    'UserInfo(3) = intcoursemarks
                                    'UserInfo(4) = intcoursepassmarks
                                    'UserInfo(5) = intcoursetime

                                    strsqlinsert = " INSERT INTO t_candidate_status "
                                    strsqlinsert = strsqlinsert & "(userid,Course_ID,written_test_date,consume_time,LoginName,Pwd,Total_Time,Total_marks,Total_passmarks) "
                                    strsqlinsert = strsqlinsert & "Values( "
                                    strsqlinsert = strsqlinsert & "" & cid & ","
                                    strsqlinsert = strsqlinsert & "'" & Replace(ddlcourse.SelectedItem.Value, "'", "''") & "',"
                                    strsqlinsert = strsqlinsert & "'" & ConvertDate(formatDate) & " ',"
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
                                    sendingFail = MailForOnlineTest(cid, strpassword, ddlcourse.SelectedItem.Text, intcoursemarks, intcoursepassmarks, intcoursetime)
                                    If sendingFail <> "" Then
                                        Send_Fail += sendingFail + ","
                                    End If


                                End If
                                'UpdForOnlineTest(0) = struserid
                                'UpdForOnlineTest(1) = strpassword
                                strbr = New StringBuilder()
                                strbr.Append(" Update T_User_Course set Del_Flag = 1  ")
                                strbr.Append(" where T_User_Course.User_ID = ")
                                strbr.Append(cid)
                                strbr.Append(" and T_User_Course.Course_ID = ")
                                strbr.Append(ddlcourse.SelectedValue)
                                strq = strbr.ToString()
                                myCommand.Connection = objconn.MyConnection
                                myCommand = New SqlCommand(strq, objconn.MyConnection)
                                myCommand1 = objconn.MyConnection.BeginTransaction()
                                myCommand.Transaction = myCommand1
                                myCommand.ExecuteNonQuery()
                                myCommand1.Commit()

                                '/*******************************End********************************/

                                'MailForOnlineTest(CInt(chkUsers(j).Value))
                            End If


                        Next
                        objconn.disconnect()
                        If True = AtLeastOneChecked Then
                            If blnCheck Then
                                lblMsg.ForeColor = Color.FromName("Green")
                                lblMsg.Visible = True
                                lblMsg.Text = Resources.Resource.Search_ExAssignSucc

                                If Send_Fail <> "" Then
                                    Send_Fail = Send_Fail.Substring(0, Send_Fail.Length - 1)
                                    If Send_Fail.Substring(0, 1) <> "," Then
                                        Dim strScript As String = "<script language=JavaScript>alert('" & Resources.Resource.Search_MailNSent & Send_Fail & " " & Resources.Resource.Search_CheEmaID & " ');</script>"
                                        Page.RegisterStartupScript("PopUp", strScript)
                                    Else
                                        Dim strScript As String = "<script language=JavaScript>alert(' " & Resources.Resource.Search_MailSentSucc & "');</script>"
                                        Page.RegisterStartupScript("PopUp", strScript)
                                    End If
                                Else
                                    Dim strScript As String = "<script language=JavaScript>alert(' " & Resources.Resource.Search_MailSentSucc & "');</script>"
                                    Page.RegisterStartupScript("PopUp", strScript)
                                End If
                                'Commented by kamal.
                            Else
                                StrTemp = Left(StrTemp, Len(StrTemp) - 1)
                                StrTemp = StrTemp & " " & Resources.Resource.Search_AlreAppForExam
                                lblMsg.ForeColor = Color.FromName("Red")
                                lblMsg.Text = StrTemp
                                lblMsg.Visible = True
                            End If
                        Else
                            lblMsg.ForeColor = Color.FromName("Red")
                            lblMsg.Text = Resources.Resource.Search_SelAtLeOneChck
                            lblMsg.Visible = True
                            Exit Function
                        End If
                        '***************************************************************************************
                    End If

                    'myCommand.Dispose()
                    objconn.disconnect()

                End If
                'Return UserInfo
            Catch ex As Exception
                myCommand1.Rollback()
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                strpathdb = Nothing
                intquestcntal = Nothing
                intlquesal = Nothing
                intmquesal = Nothing
                inthquesal = Nothing
                totalquestcnt = Nothing
                testtypevalue = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        '@(f)
        'Function               :   GetUserNameFromID
        'Return                 :   string
        'Argument               :   ARG1 [IN]: ID
        'Coded by Tabrez
        'Explanation: Returns the username of the ID provided.
        '**************************************************************************
#Region "GetUserNameFromID"
        Private Function GetUserNameFromID(ByVal ID As Integer, ByVal test As String) As String
            Dim objconn As New ConnectDb
            Dim StrSql As String
            Dim myCommand As SqlCommand
            Dim ORdr As SqlDataReader
            'Dim strPathDb As String

            Try
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    StrSql = "select name from m_user_info,t_candidate_status where m_user_info.userid = t_candidate_status.userid and t_candidate_status.userid=" & ID & " and t_candidate_status.test_type = '" & test & "' and appearedflag = 99"
                    myCommand = New SqlCommand(StrSql, objconn.MyConnection)
                    ORdr = myCommand.ExecuteReader
                    If ORdr.Read() Then
                        Return ORdr(0)
                    Else
                        Return ""
                    End If
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                StrSql = Nothing
                ORdr = Nothing
                strPathDb = Nothing
                myCommand = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getEmailId
        '
        'Return                 :   None
        '
        'Argument               :   userid  : User Id
        '
        'Explanation            :   This will return user id for specific user id
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getEmailId"
        Function getEmailId(ByVal userid As String) As String
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader

            Try
                sqlstr = "SELECT email FROM m_user_info"
                sqlstr = sqlstr & " WHERE userid=" & CInt(userid)

                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    If myDataReader.Read() Then
                        getEmailId = myDataReader.Item("email")
                    End If
                End If
                myCommand.Dispose()
                myDataReader.Close()
                '     objconn.disconnect()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                lblMsg.Text = ex.Message()
            Finally
                'objconn.disconnect()
                'objconn = Nothing
                sqlstr = Nothing
                myCommand = Nothing
                myDataReader = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   MailForOnlineTest
        '
        'Return                 :   None
        '
        'Argument               :   userid  : User Id
        '
        'Explanation            :   This will send the mail for selected user
        '                           
        'Note                   :   None
        '**************************************************************************
        'Modified by :: Saraswati Patel
        'Description :: For sending mail to the student when exam assign
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
                    'sqlstr = "SELECT  (Name) as UserName,Email,RollNo FROM m_user_info"
                    sqlstr = "SELECT  (SurName+' '+Name+' '+isNull(Middlename,'')) as UserName,Email,RollNo FROM m_user_info"
                    sqlstr = sqlstr & " WHERE userid=" & userid
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read
                        If Not IsDBNull(myDataReader.Item("Email")) Then
                            bool = sendMail(userid.ToString(), myDataReader.Item("Email"), myDataReader.Item("RollNo"), myDataReader.Item("UserName"), password, subjectName, TotalMarks, PassingMarks, TotalTime)
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
                lblMsg.Text = ex.Message()
            Finally
                'objconn.disconnect()
                objconn = Nothing
                sqlstr = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                strPathDb = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   getMsg
        '
        'Return                 :   message for mail
        '
        'Argument               :   userid  : User Id
        '
        'Explanation            :   get the message for the mail
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "getMsg"
        Private Function getMsg(ByVal userid As Integer) As String
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim msg As String
            Dim fileName As String = ConfigurationSettings.AppSettings("Mail")
            Dim objStreamReader As StreamReader
            Dim C_HOMEPAGEURL As String

            Try
                C_HOMEPAGEURL = ConfigurationSettings.AppSettings("C_HOMEPAGEURL")
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    'sqlstr = "SELECT b.name,b.email,a.written_test_date,c.test_name,c.Time  "
                    'sqlstr = sqlstr & " FROM t_candidate_status as a,"
                    'sqlstr = sqlstr & " m_user_info as b, m_testinfo as c "
                    'sqlstr = sqlstr & " WHERE(a.userid = b.userid)"
                    'sqlstr = sqlstr & " AND (c.test_type='" & sel_test_type.SelectedItem.Value & "')"
                    'sqlstr = sqlstr & " AND b.userid=" & userid
                    'sqlstr = sqlstr & " AND a.test_type='" & sel_test_type.SelectedItem.Value & "'"
                    'myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    'myDataReader = myCommand.ExecuteReader()
                    fileName = Server.MapPath(fileName)
                    objStreamReader = File.OpenText(fileName)
                    If myDataReader.Read() Then
                        'msg = objStreamReader.ReadToEnd
                        'msg = Replace(msg, "&#Name#&", myDataReader.Item("name"))
                        'msg = Replace(msg, "&#sub#&", myDataReader.Item("test_name"))
                        'msg = Replace(msg, "&#date#&", Left(myDataReader.Item("written_test_date"), 10))
                        'msg = Replace(msg, "&#time#&", myDataReader.Item("Time"))
                        'msg = Replace(msg, "&#links#&", C_HOMEPAGEURL & "examinfo.aspx?lnks=" & C_HOMEPAGEURL & "question_paper_new.aspx?usr=" & sEncodeString(userid & "|" & sel_test_type.SelectedItem.Value))
                        getMsg = msg
                    End If
                End If
                objStreamReader.Close()
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                lblMsg.Text = ex.Message()
            Finally
                'objconn.disconnect()
                objconn = Nothing
                sqlstr = Nothing
                myCommand = Nothing
                msg = Nothing
                C_HOMEPAGEURL = Nothing
                objStreamReader = Nothing
                fileName = Nothing
                myDataReader = Nothing
            End Try
        End Function
#End Region

#Region "GetExamTimeOut"
        Private Function GetExamTimeOut() As Integer
            'Dim OConn As New ConnectDb
            'Dim OCmd As SqlCommand
            'Dim ORdr As SqlDataReader
            'Dim strPathDb As String
            'Dim StrSql As String

            'Try
            '    strPathDb = ConfigurationSettings.AppSettings("PathDb")
            '    If True = OConn.connect(strPathDb) Then
            '        StrSql = "select time from m_testinfo where test_type='" & sel_test_type.SelectedItem.Value & "'"
            '        OCmd = New SqlCommand(StrSql, OConn.MyConnection)
            '        ORdr = OCmd.ExecuteReader
            '        ORdr.Read()
            '        If ORdr.HasRows Then
            '            Return Convert.ToInt32(ORdr.Item(0))
            '        Else
            '            Return 0
            '        End If
            '    End If

            'Catch ex As Exception
            '    If log.IsDebugEnabled Then
            '        log.Debug("Error :" & ex.ToString())
            '    End If
            '    objconn.disconnect()
            '    lblMsg.Text = ex.Message()
            'Finally
            '    'objconn.disconnect()
            '    OConn = Nothing
            '    OCmd = Nothing
            '    ORdr = Nothing
            '    strPathDb = Nothing
            '    StrSql = Nothing
            'End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   sendMail
        '
        'Return                 :   message for mail
        '
        '
        'Explanation            :   sending mail through smtp
        '                           
        'Note                   :   None
        '**************************************************************************
        'Send mail to the Student
        '#Region "sendMail"
        '        Private Function sendMail(ByVal StrTo As String, ByVal userRollNo As String, ByVal UserName As String, ByVal userPassword As String, ByVal subjectName As String, ByVal TotalMarks As String, ByVal PassingMarks As String, ByVal TotalTime As String)
        '            Dim strQuery1 As String
        '            Dim strQuery2 As String
        '            Dim myCommand2 As SqlCommand
        '            Dim myDataReader2 As SqlDataReader
        '            Dim myCommand3 As SqlCommand
        '            Dim myDataReader3 As SqlDataReader
        '            Dim objconn As New ConnectDb
        '            Dim mail As New MailMessage
        '            Dim strEmaiId As String = ""
        '            Dim intNum As Int32
        '            Dim intQues As Int32
        '            Dim strPathDb As String
        '             Dim strSmtpServer As String
        '            Try
        '                strPathDb = ConfigurationSettings.AppSettings("PathDb")
        '                If objconn.connect(strPathDb) Then
        '                    'strQuery2 = " SELECT m_testinfo.no_of_ques FROM m_testinfo"
        '                    'strQuery2 = strQuery2 & " WHERE (test_name = '" & sel_test_type.SelectedItem.Text & "')"
        '                    'myCommand3 = New SqlCommand(strQuery2, objconn.MyConnection)
        '                    'myDataReader3 = myCommand3.ExecuteReader()
        '                    'While myDataReader3.Read
        '                    '    intQues = Convert.ToInt32(myDataReader3.Item("no_of_ques").ToString())
        '                    'End While
        '                    'myDataReader3.Close()
        '                    'myCommand2 = Nothing

        '                    'strQuery1 = "SELECT COUNT (m_question.test_type) as Number FROM m_question CROSS JOIN"
        '                    'strQuery1 = strQuery1 & " m_testinfo WHERE m_question.test_type IN"
        '                    'strQuery1 = strQuery1 & "(SELECT m_testinfo.test_type WHERE m_testinfo.test_name = "
        '                    'strQuery1 = strQuery1 & "'" & sel_test_type.SelectedItem.Text & "')"

        '                    'myCommand2 = New SqlCommand(strQuery1, objconn.MyConnection)
        '                    'myDataReader2 = myCommand2.ExecuteReader()
        '                    'If myDataReader2.Read() Then
        '                    '    intNum = myDataReader2.Item("Number")
        '                    'Else
        '                    '    intNum = 0
        '                    'End If

        '                    'myDataReader2.Close()
        '                    'myCommand2 = Nothing

        '                    ' If intNum > intQues Then
        '                    strQuery1 = "select mc.Email From M_Centers as mc join T_Center_Course as tcc on mc.Center_ID=tcc.Center_ID where tcc.Course_ID=" + ddlcourse.SelectedItem.Value
        '                    myCommand = New SqlCommand(strQuery1, objconn.MyConnection)
        '                    myDataReader = myCommand.ExecuteReader()
        '                    While myDataReader.Read
        '                        If Not IsDBNull(myDataReader.Item("Email")) Then
        '                            strEmaiId += myDataReader.Item("Email") + ","

        '                        End If

        '                    End While

        '                    If StrTo <> "" Then

        '                        ' strEmaiId = ConfigurationSettings.AppSettings("AdminEmailID")
        '                        'mail.From = strEmaiId

        '                        mail.To = StrTo
        '                        'If strEmaiId <> "" Then
        '                        '    strEmaiId = strEmaiId.Substring(0, strEmaiId.Length - 1)
        '                        'End If
        '                        'mail.Cc = "saraswati@usindia.com"
        '                        mail.Cc = strEmaiId
        '                        mail.Subject = "Request to appear for Unikaihatsu Online Examination"
        '                        mail.Body = "Hello " + UserName + Environment.NewLine + Environment.NewLine + "Exam of  The " _
        '                                    & ddlcourse.SelectedItem.Text & Environment.NewLine + Environment.NewLine _
        '                                    & "Exam Date:- " + ConvertDate(txtExamDate.Text) + Environment.NewLine + "UserID:- " _
        '                                    & userRollNo + Environment.NewLine + "Password:- " + userPassword + "" '_
        '                        '& Environment.NewLine & "TotalTime"
        '                        'mail.Body = getMsg(CInt(userid))
        '                        ' mail.BodyFormat = MailFormat.Text


        '                        strSmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
        '                        '*************Added code for Server Authentication

        '                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
        '                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
        '                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
        '                        SmtpMail.SmtpServer = strSmtpServer
        '                        SmtpMail.Send(mail)
        '                        'lblMsg.Text = "Mail message(s) sent successfully."

        '                    End If
        '                    'Else
        '                    'lblMsg.Text = "There are not enough questions for the selected test."
        '                    'End If
        '                End If
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    Dim err As String = ex.ToString()
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                lblMsg.Text = ex.Message()
        '            Finally
        '                myDataReader = Nothing
        '                myDataReader3 = Nothing
        '                myDataReader2 = Nothing
        '                myCommand = Nothing
        '                myCommand3 = Nothing
        '                myCommand2 = Nothing
        '                strQuery1 = Nothing
        '                strQuery2 = Nothing
        '                strPathDb = Nothing
        '                objconn = Nothing
        '                mail = Nothing
        '                strEmaiId = Nothing
        '            End Try
        '        End Function
        '#End Region


        'Added by :: Saraswati Patel
        'Description :: For sending mail to the student when exam assign


#Region "sendMail"
        Private Function sendMail(ByVal StrUserID As String, ByVal StrTo As String, ByVal userRollNo As String, ByVal UserName As String, ByVal userPassword As String, ByVal subjectName As String, ByVal TotalMarks As String, ByVal PassingMarks As String, ByVal TotalTime As String) As Boolean
            Dim strQuery1 As String
            Dim strQuery2 As String
            Dim myCommand2 As SqlCommand
            Dim myDataReader2 As SqlDataReader
            Dim myCommand3 As SqlCommand
            Dim myDataReader3 As SqlDataReader
            Dim objconn As New ConnectDb
            Dim mail As New MailMessage
            Dim objCommFun As CommonFunction
            Dim strEmaiId As String = ""
            Dim strOwnerName As String = ""
            Dim intNum As Int32
            Dim intQues As Int32
            'Dim strPathDb As String
            Dim strSmtpServer As String
            Dim strMessage As String
            Dim cda As SqlDataAdapter
            Dim cds As DataSet

            Try
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                Dim strExamURl = ConfigurationSettings.AppSettings("ExamURL")

                If objconn.connect() Then

                    strQuery1 = "select mc.Email,mc.Owner_Name From M_Centers as mc join T_Center_Course as tcc on mc.Center_ID=tcc.Center_ID where tcc.Course_ID=" + ddlcourse.SelectedItem.Value + " and mc.center_id=" + sel_subjectid.SelectedItem.Value
                    myCommand = New SqlCommand(strQuery1, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read
                        If Not IsDBNull(myDataReader.Item("Email")) Then
                            strEmaiId += myDataReader.Item("Email") + ","
                            strOwnerName = myDataReader.Item("Owner_Name")
                        End If
                    End While
                    myCommand = Nothing
                    myDataReader = Nothing
                    objconn.disconnect()
                End If
                If objconn.connect() Then
                    If StrTo <> "" Then
                        ' strEmaiId = ConfigurationSettings.AppSettings("AdminEmailID")
                        'mail.From = strEmaiId

                        objCommFun = New CommonFunction()
                        Dim q As String = "select (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as Name, "
                        q = q + "mc.center_name as ClassName, mti.Test_name as TestName, "
                        q = q + "convert(varchar(10), tcs.Written_Test_Date,120) as ExamDate, tcs.total_time as TotalTime, "
                        q = q + "tcs.total_Marks as TotalMarks, tcs.userid as UserID,mc.owner_name as Teacher, "
                        q = q + "tcs.Loginname as LoginName, tcs.pwd as Password, mui.email as EmailID,mc.Email as TeacherEmail "
                        q = q + "from t_candidate_status as tcs "
                        q = q + "left join M_User_info as mui on mui.userid=tcs.userid "
                        q = q + "left join M_Centers as mc on mc.center_id=mui.center_id "
                        q = q + "left join T_User_course as tuc on tuc.User_ID=tcs.Userid and tuc.course_id=tcs.course_id "
                        q = q + "left join M_Testinfo as mti on mti.Test_type=tuc.Test_type "
                        q = q + "where tcs.userid=" + StrUserID + " and tcs.course_id=" + ddlcourse.SelectedItem.Value
                        cda = New SqlDataAdapter(q, objconn.MyConnection)
                        cds = New DataSet
                        cda.Fill(cds)

                        mail.From = ConfigurationSettings.AppSettings("mailsenderid")
                        mail.To = StrTo
                        mail.Cc = cds.Tables(0).Rows(0)("TeacherEmail").ToString() 'strEmaiId


                        strMessage = objCommFun.ReadFile(Server.MapPath(ConfigurationSettings.AppSettings("AssignExamMail")))
                        strMessage = strMessage.Replace("#Name#", cds.Tables(0).Rows(0)("Name").ToString())
                        strMessage = strMessage.Replace("#ClassName#", cds.Tables(0).Rows(0)("ClassName").ToString())
                        strMessage = strMessage.Replace("#sub#", subjectName)
                        strMessage = strMessage.Replace("#date#", cds.Tables(0).Rows(0)("ExamDate").ToString())
                        strMessage = strMessage.Replace("#Marks#", cds.Tables(0).Rows(0)("TotalMarks").ToString())
                        strMessage = strMessage.Replace("#UserId#", cds.Tables(0).Rows(0)("LoginName").ToString())
                        strMessage = strMessage.Replace("#Password#", cds.Tables(0).Rows(0)("Password").ToString())
                        strMessage = strMessage.Replace("#TeacherName#", cds.Tables(0).Rows(0)("Teacher").ToString())
                        strMessage = strMessage.Replace("#links#", strExamURl)


                        'mail.Subject = "Request to appear an exam of (" + ddlcourse.SelectedItem.Text + ")"
                        'mail.Body = String.Format("Hello " + UserName + Environment.NewLine + Environment.NewLine + "" _
                        '                          & "Your Exam details are as below" + Environment.NewLine + Environment.NewLine + "" _
                        '            & "Course Name: " _
                        '            & ddlcourse.SelectedItem.Text & Environment.NewLine _
                        '            & "Exam Date: " + ConvertDate(txtExamDate.Text) + Environment.NewLine + "UserID: " _
                        '            & userRollNo + Environment.NewLine + "Password: " + userPassword + Environment.NewLine + "" _
                        '            & "Exam URL: " + strExamURl + "" _
                        '            & Environment.NewLine + Environment.NewLine + "Regards," + Environment.NewLine + strOwnerName)

                        mail.Subject = "Request to appear an exam of (" + cds.Tables(0).Rows(0)("ClassName").ToString() + " - " + subjectName + ")"
                        mail.Body = strMessage

                        '& Environment.NewLine & "TotalTime"
                        'mail.Body = getMsg(CInt(userid))
                        mail.BodyFormat = MailFormat.Html

                        'strSmtpServer = "122.169.107.93"
                        strSmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
                        '*************Added code for Server Authentication
                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")

                        mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
                        mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
                        mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2

                        'Added for authentication
                        'mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1") 'basic authentication
                        'If(strEmaiId="") then 

                        '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "bharat") 'set your username here
                        '    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "PritaP@1984")  'set your password here

                        'Else

                        'End If
                        SmtpMail.SmtpServer = strSmtpServer
                        SmtpMail.Send(mail)
                        Return True
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
                lblMsg.Text = ex.Message()
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
                mail = Nothing
                strEmaiId = Nothing
            End Try
        End Function
#End Region




#Region "sendMail"
        Private Function sendMail_new(ByVal StrTo As String, ByVal userRollNo As String, ByVal UserName As String, ByVal userPassword As String, ByVal subjectName As String, ByVal TotalMarks As String, ByVal PassingMarks As String, ByVal TotalTime As String)
            Dim strQuery1 As String
            Dim strQuery2 As String
            Dim myCommand2 As SqlCommand
            Dim myDataReader2 As SqlDataReader
            Dim myCommand3 As SqlCommand
            Dim myDataReader3 As SqlDataReader
            Dim objconn As New ConnectDb

            Dim strEmaiId As String = ""
            Dim intNum As Int32
            Dim intQues As Int32
            'Dim strPathDb As String
            Dim strSmtpServer As String
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
                                    & "Exam Date:- " + ConvertDate(txtExamDate.Value) + Environment.NewLine + "UserID:- " _
                                    & userRollNo + Environment.NewLine + "Password:- " + userPassword + "" '_
                        '& Environment.NewLine & "TotalTime"
                        'mail.Body = getMsg(CInt(userid))
                        'mail.BodyFormat = MailFormat.Text
                        'mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                        ' mail.Priority = MailPriority.High
                        strSmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
                        '*************Added code for Server Authentication
                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
                        'mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
                        Dim server As New System.Net.Mail.SmtpClient

                        Dim basicAuthenticationInfo As New System.Net.NetworkCredential("bharat", "PritaP@1984")
                        server.Credentials = basicAuthenticationInfo
                        server.Send(mail)
                        'SmtpMail.SmtpServer = strSmtpServer
                        'SmtpMail.Send(mail)

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
                End If
                lblMsg.Text = ex.Message()
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
                Mail = Nothing
                strEmaiId = Nothing
            End Try
        End Function
#End Region


        '**************************************************************************
        'Function               :   searchResult
        '
        'Return                 :   message for mail
        '
        'Argument               :   None

        '
        'Explanation            :   This functio will called onload of page 
        '                           This will display the search result
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "searchResult"
        Private Sub searchResult()
            Dim sqlStr As String
            Try
                DGData.CurrentPageIndex = 0
                BindGrid()
                tblExam.Visible = True

                'tblResult.Rows.Clear()
                'getResultHeaders()
                'getResultDetails(sqlStr)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                sqlStr = Nothing
            End Try
        End Sub
#End Region

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
#Region "sEncodeString"
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
                'objconn.disconnect()
                iCnt = Nothing
                sChar = Nothing
                sEncode = Nothing
            End Try

        End Function
#End Region

        '**************************************************************************
        'Function               :   sDecodeString
        '
        'Return                 :   decoded string
        '
        'Argument               :   None
        '
        'Explanation            :   This will convert the encoded string into 
        '                           normal string
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "sDecodeString"
        Function sDecodeString(ByVal sText As String) As String
            Dim iCnt
            Dim sChar
            Dim sDecode

            Try
                sDecode = ""
                sChar = Split(sText, ENCRYPT_DELIMIT)
                For iCnt = 1 To UBound(sChar)
                    sDecode = sDecode & Chr(CLng("&H" & sChar(iCnt)) - ENCRYPT_KEY)
                Next
                sDecodeString = sDecode

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                sDecode = Nothing
                iCnt = Nothing
                sChar = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   ExiamineeValidation
        '
        'Return                 :   sUser   : 
        '
        'Argument               :   None
        '
        'Explanation            :   This will convert the encoded string into 
        '                           normal string
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "ExiamineeValidation"
        Private Function ExiamineeValidation(ByVal sUsr As String)
            Dim aUsr As Array
            Dim userid As Integer
            Dim testType As String
            Dim noOfDays As Long
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader

            Try
                sUsr = sDecodeString(sUsr)
                aUsr = Split(sUsr, "|")
                userid = CInt(aUsr(1))
                testType = aUsr(2)

                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlstr = "SELECT written_test_date FROM t_candidate_status"
                    sqlstr = sqlstr & " WHERE userid=" & userid
                    sqlstr = sqlstr & " AND test_type='" & testType & "'"
                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read
                        If Not IsDBNull(myDataReader.Item("written_test_date")) Then
                            noOfDays = DateDiff(DateInterval.Day, myDataReader.Item("written_test_date"), Now())
                            If noOfDays > 3 Or noOfDays <= 0 Then
                                Response.Write("Error")
                            End If
                        Else
                            Response.Write("Error")
                        End If
                    End While
                End If
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                sqlstr = Nothing
                objconn = Nothing
                myDataReader = Nothing
                myCommand = Nothing
                noOfDays = Nothing
                testType = Nothing
                aUsr = Nothing
                userid = Nothing
            End Try
        End Function
#End Region

#Region "ImageButton1_Click"
        'Added by Pranit Chimurkar on 2019/10/18
        Private Sub ImageButton1_Click(sender As Object, e As EventArgs) Handles Mail.Click

            lblMsg.Text = String.Empty
            lblReocrds.Text = String.Empty

            Const REGULAR_EXP = "(^[0-9]{4,4}/?[0-1][0-9]/?[0-3][0-9]$)"
            Dim flag As Boolean = True
            Try
                If IsPostBack Then
                    Dim fromDt As Date = txtExamDate.Value
                    Dim fdt As String = Format(fromDt, "dd/MM/yyyy")
                    If fdt <> "" Then
                        'If Not Regex.IsMatch(txtExamDate.Text, REGULAR_EXP) Then
                        '    MsgBox("Please enter Examination date in YYYY/MM/DD format.", MsgBoxStyle.Exclamation)
                        '    txtExamDate.Focus()
                        '    flag = False
                        'End If
                        If Not IsDate(ConvertDate(fdt)) And flag Then
                            MsgBox(Resources.Resource.Search_PlEnValExDate & " ", MsgBoxStyle.Exclamation)
                            txtExamDate.Focus()
                            flag = False
                        End If
                        If flag Then
                            Dim UserInfo As String() = UpdForOnlineTest(fdt)

                            'MailForOnlineTest(UserInfo(0), UserInfo(1), UserInfo(2), UserInfo(3), UserInfo(4), UserInfo(5))


                            Dim sqlStr As String
                            'tblResult.Rows.Clear()
                            'getResultHeaders()
                            'getResultDetails(sqlStr)
                            BindGrid()
                            DGData.Visible = True
                            gridDiv.Visible = True
                            If lblReocrds.Text = Resources.Resource.Common_NoRecFound Then
                                lblReocrds.Text = String.Empty
                            End If
                        End If
                    Else
                        MsgBox(Resources.Resource.Search_PlEnExamDate, MsgBoxStyle.Exclamation)
                        txtExamDate.Focus()
                    End If
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                sqlstr = Nothing
            End Try
        End Sub
#End Region


        '----------
        ' Random Password Generator
        ' We needed a small password routine for our distance learning project.
        ' It is used to provide our users with new passwords upon request.
        ' This is not a high security algoritm, but still useful for 
        ' smallscale user-databases.
        '
        '----------
        '--StrRandomize(strSeed)
        '--Make string to numbers and initialize the random generator
        '----------
#Region "GeneratePassword"
        Function GeneratePassword(ByVal nLength As Integer)
            Dim i, bMadeConsonant, c, nRnd
            'You may adjust the below constants to include local,
            'eg. scandinavian characters. This way your passwords
            'will not be limited to latin characters.
            Const strDoubleConsonants = "bdfglmnpst"
            Const strConsonants = "bcdfghklmnpqrstv"
            Const strVocal = "aeiou"

            Try
                GeneratePassword = ""
                bMadeConsonant = False

                For i = 0 To nLength
                    'Get a random number number between 0 and 1
                    nRnd = Rnd()
                    'Simple or double consonant, or a new vocal?
                    'Does not start with a double consonant
                    '15% or less chance for the next letter being a double consonant
                    If GeneratePassword <> "" And _
                        (bMadeConsonant <> True) And (nRnd < 0.15) Then
                        'double consonant
                        c = Mid(strDoubleConsonants, Len(strDoubleConsonants) * Rnd() + 1, 1)
                        c = c & c
                        i = i + 1
                        bMadeConsonant = True
                    Else
                        '80% or less chance for the next letter being a consonant,
                        'depending on wether the last letter was a consonant or not.
                        If (bMadeConsonant <> True) And (nRnd < 0.95) Then
                            'Simple consonant
                            c = Mid(strConsonants, Len(strConsonants) * Rnd() + 1, 1)
                            bMadeConsonant = True
                            '5% or more chance for the next letter being a vocal. 100% if last
                            'letter was a consonant - theoreticaly speacing...
                        Else
                            'If last one was a consonant, make vocal
                            c = Mid(strVocal, Len(strVocal) * Rnd() + 1, 1)
                            bMadeConsonant = False
                        End If
                    End If

                    'Add letter
                    GeneratePassword = GeneratePassword & c
                Next

                'Is the password long enough, or perhaps too long?
                If Len(GeneratePassword) > nLength Then
                    GeneratePassword = Left(GeneratePassword, nLength)
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                'objconn.disconnect()
                i = Nothing
                bMadeConsonant = Nothing
                c = Nothing
                nRnd = Nothing
            End Try
        End Function
#End Region

        '**************************************************************************
        'Function               :   img_save_Click
        '
        'Return                 :   None 
        '
        'Argument               :   None
        '
        'Explanation            :   Insert record and send mail for online examination
        '                           
        'Note                   :   None
        '**************************************************************************
        '#Region "Mail_Click"
        '        Private Sub Mail_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Mail.Click
        '            Try
        '                'Update Database for examination 
        '                UpdForOnlineTest()
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Response.Redirect("error.aspx")

        '            End Try
        '        End Sub
        '#End Region

        '**************************************************************************
        '@(f)
        'Function               :   ValidateDate
        'Return                 :   Boolean 
        'Argument               :   ARGS1 [IN] YrDt()
        'Explanation            :   Validate the date to todays date 
        '**************************************************************************    
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

        '**************************************************************************
        'Function               :   img_save_Click
        '
        'Return                 :   None 
        '
        'Argument               :   None
        '
        'Explanation            :   Save the Search criteria into a table
        '                           
        'Note                   :   None
        '**************************************************************************
        '#Region "img_save_Click"
        '        Private Sub img_save_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_save.Click
        '            Try
        '                Dim search As Boolean = False

        '                'Kamal Code
        '                'If txtsearch.Text = "" Then
        '                '    lblMsg.ForeColor = Color.FromName("Red")
        '                '    lblMsg.Text = "Empty New Search Name. Please Verify!!"
        '                '    lblMsg.Visible = True
        '                '    Exit Sub
        '                'End If
        '                'End of Kamal Code

        '                removesearchcri()

        '                If Not IsDBNull(txt_name.Text) And txt_name.Text <> "" Then
        '                    saveSearchCri("name", txt_name.Text)
        '                    search = True
        '                End If

        '                If Not IsDBNull(txt_surname.Text) And txt_surname.Text <> "" Then
        '                    saveSearchCri("surname", txt_surname.Text)
        '                    search = True
        '                End If

        '                If Not IsDBNull(txt_birth.Text) And txt_birth.Text <> "" Then
        '                    saveSearchCri("birthdate", txt_birth.Text)
        '                    search = True
        '                End If
        '                'added by sandeep sharma to show the cmcampusid value on the page.
        '                'If Not IsDBNull(cmbCampusid.SelectedItem.Text) And cmbCampusid.SelectedItem.Text <> "--Select--" Then
        '                '    saveSearchCri("CampusId", cmbCampusid.SelectedItem.Text)
        '                '    search = True
        '                'End If
        '                'end of change done by sandeep sharma

        '                'If Not IsDBNull(txt_city.Text) And txt_city.Text <> "" Then
        '                '    saveSearchCri("city", txt_city.Text)
        '                '    search = True
        '                'End If

        '                ''Code for Academic Information
        '                'If Not IsDBNull(cmb_grad.Value) And cmb_grad.Value <> "" Then
        '                '    saveSearchCri("academic_id", cmb_grad.Value)
        '                '    search = True
        '                'End If

        '                'If Not IsDBNull(txt_year.Text) And txt_year.Text <> "" Then
        '                '    saveSearchCri("pass_year", txt_year.Text)
        '                '    search = True
        '                'End If

        '                'If Not IsDBNull(txt_percentage.Text) And txt_percentage.Text <> "" Then
        '                '    saveSearchCri("percentage", txt_percentage.Text)
        '                '    search = True
        '                'End If

        '                ''Code for Computer
        '                'If Not IsDBNull(txt_progLang.Text) And txt_progLang.Text <> "" Then
        '                '    saveSearchCri("prog_language", txt_progLang.Text)
        '                '    search = True
        '                'End If

        '                'If Not IsDBNull(txt_db.Text) And txt_db.Text <> "" Then
        '                '    saveSearchCri("database_known", txt_db.Text)
        '                '    search = True
        '                'End If

        '                ''Code for Experience
        '                'If Not IsDBNull(sel_field.Value) And sel_field.Value <> "" Then
        '                '    saveSearchCri("field", sel_field.Value)
        '                '    search = True
        '                'End If

        '                'If Not IsDBNull(txt_experienceMonths.Text) And txt_experienceMonths.Text <> "" Then
        '                '    saveSearchCri("org_duration", txt_experienceMonths.Text)
        '                '    search = True
        '                'End If

        '                'Kamal Code


        '                If search = True Then
        '                    lblMsg.ForeColor = Color.FromName("Green")
        '                    lblMsg.Text = "Search has been saved successfully"
        '                Else
        '                    lblMsg.ForeColor = Color.FromName("Red")
        '                    lblMsg.Text = "Please select any searching criteria"
        '                End If
        '                lblMsg.Visible = True

        '                'End of Kamal Code
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Response.Redirect("error.aspx")
        '            End Try
        '        End Sub
        '#End Region

#Region "img_cancel_Click"
        'Added by Pranit Chimurkar on 2019/10/18
        'Private Sub img_cancel_Click(sender As Object, e As EventArgs) Handles img_cancel.Click
        '    Dim C_HOMEPAGEURL As String
        '    Try
        '        C_HOMEPAGEURL = ConfigurationSettings.AppSettings("C_HOMEPAGEURL")
        '        Response.Redirect("admin.aspx", False)
        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        Response.Redirect("error.aspx", False)

        '    Finally
        '        'objconn.disconnect()
        '        C_HOMEPAGEURL = Nothing
        '    End Try
        'End Sub
#End Region

#Region "SetFocus"

        Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)

            Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ID & "').focus() </SCRIPT>"
            RegisterStartupScript("focus", s)

        End Sub
#End Region

#Region "getAttdTest"
        Private Function getAttdTest(ByVal userid As Integer) As String
            Dim objconn As New ConnectDb
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim strTest As String
            Dim sqlStr As String
            'Dim strPathDb As String

            Try

                strTest = ""
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlStr = "SELECT Test_Name FROM m_TestInfo, t_candidate_status  WHERE userid = " & userid & " and m_Testinfo.test_type = t_candidate_status.test_type"
                    myCommand = New SqlCommand(sqlStr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader()
                    While myDataReader.Read()
                        If strTest = "" Then
                            strTest = strTest & myDataReader.Item(0)
                        Else
                            strTest = strTest & "," & myDataReader.Item(0)
                        End If
                    End While
                End If
                myCommand.Dispose()
                myDataReader.Close()
                objconn.disconnect()
                getAttdTest = strTest
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                objconn.disconnect()
                Throw ex
            Finally
                'objconn.disconnect()
                objconn = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                strTest = Nothing
                sqlStr = Nothing
                strPathDb = Nothing
            End Try
        End Function
#End Region

        '#Region "imgLogoff_Click"
        '        Private Sub imgLogoff_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgLogoff.Click
        '            Try
        '                FormsAuthentication.SignOut()
        '                Response.Redirect("login.aspx", False)
        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                lblMsg.Text = ex.Message()

        '            End Try
        '        End Sub
        '#End Region
        '***********************************Fuction for loading the value in ddlCampusid control**********
#Region "loadvalue"
        'Public Sub loadvalue()
        '    Try
        '        strpathdb = ConfigurationSettings.AppSettings("PathDb")
        '        If objconn.connect(strpathdb) Then
        '            sqlstr = "SELECT DISTINCT campus_id FROM m_user_info WHERE campus_id <> ''"
        '            myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
        '            myDataReader = myCommand.ExecuteReader()

        '            cmbCampusid.Items.Add("--Select--")
        '            While myDataReader.Read
        '                cmbCampusid.Items.Add(myDataReader.Item(0).ToString())

        '            End While
        '            cmbCampusid.DataSource = myTable
        '            cmbCampusid.DataValueField = "report_name"
        '            cmbCampusid.DataTextField = "report_name"
        '            cmbCampusid.DataBind()
        '            myDataReader.Close()
        '            objconn.disconnect()
        '        End If

        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        objconn.disconnect()
        '        lblMsg.Text = ex.Message()

        '    End Try
        'End Sub
#End Region


        'Protected Sub ddlcourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlcourse.SelectedIndexChanged
        '    lblMsg.Text = String.Empty

        '    If (ddlcourse.SelectedIndex > 0) Then
        '        'sel_subjectid.Enabled = True
        '        ''searchbtn.Enabled = True
        '        'searchbtn.ToolTip = ""
        '        'populate_subjectid()
        '    ElseIf (ddlcourse.SelectedIndex = 0) Then
        '        If (sel_subjectid.Items.Count > 0) Then
        '            'sel_subjectid.SelectedValue = 0
        '        End If
        '        'searchbtn.Enabled = False
        '        'searchbtn.ToolTip = "Select the Couese First"
        '        sel_subjectid.Enabled = False
        '    End If
        'End Sub

#Region "Subject selected index changed event"

        Private Sub sel_subjectid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles sel_subjectid.Load

        End Sub
        'Desc:This is Subject selected index changed event
        'By: Jatin Gangajaliya
        Protected Sub sel_subjectid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sel_subjectid.SelectedIndexChanged

            'txt_name.Text = ""
            ''txt_surname.Text = ""
            'txt_birth.Text = ""

            Try
                If (sel_subjectid.SelectedIndex > 0) Then
                    ddlcourse.Enabled = True
                    searchUser.Enabled = True
                    BindCourse()
                ElseIf (sel_subjectid.SelectedIndex = 0) Then
                    If (sel_subjectid.Items.Count > 0) Then
                        ddlcourse.SelectedValue = 0
                    End If
                    ddlcourse.Enabled = False
                    searchUser.Enabled = False
                    'searchUser.ToolTip = Resources.Resource.Search_SelCenFirst
                    searchUser.Enabled = False
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)

            End Try
        End Sub
#End Region

#Region "Search Button Click"
        'Desc: This is search button click.
        'By: Jatin Gangajaliaya
        'Added by Pranit Chimurkar on 2019/10/16
        Protected Sub searchUser_Click(sender As Object, e As EventArgs) Handles searchUser.Click
            Try
                If sel_subjectid.SelectedIndex = 0 Then
                    lblMsg.Text = Resources.Resource.Search_PlSelClName
                    lblMsg.Visible = True
                    lblMsg.ForeColor = Color.FromName("Red")
                    lblReocrds.Visible = False
                    'added by bhumi [17/9/2015]
                    'Reason: while no result found invisible search result and Exam Date section
                    gridDiv.Visible = False
                    DGData.Visible = False
                    'examSection.Visible = False
                    tblExam.Visible = False
                    'Ended by bhumi
                    sel_subjectid.Focus()
                End If
                If sel_subjectid.SelectedValue > 0 Then
                    If ddlcourse.SelectedValue > 0 Then
                        searchResult()
                    Else
                        lblMsg.Text = Resources.Resource.Search_PlSelCoName
                        lblMsg.ForeColor = Color.FromName("Red")
                        lblMsg.Visible = True
                        lblReocrds.Visible = False
                        'added by bhumi [17/9/2015]
                        'Reason: while no result found invisible search result and Exam Date section
                        gridDiv.Visible = False
                        DGData.Visible=False
                        'examSection.Visible = False
                        tblExam.Visible = False
                        'Ended by bhumi
                        ddlcourse.Focus()
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

#Region "Button Clear Click Event"
        'Desc: This Clear button click event
        'By: Jatin Gangajaliya
        'Added by Pranit Chimurkar on 2019/10/18
        Protected Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
            Try
                sel_subjectid.SelectedIndex = 0
                ddlcourse.SelectedIndex = 0
                'txt_name.Text = String.Empty
                ''txt_surname.Text = String.Empty
                'txt_birth.Text = String.Empty
                lblNum.Visible = False
                lblNum.Text = String.Empty
                lblReocrds.Text = String.Empty
                lblReocrds.Visible = False
                lblMsg.Text = String.Empty
                gridDiv.Visible = False
                'examSection.Visible = False
                'tblResult.Visible = False
                txtExamDate.Value = String.Empty
                tblExam.Visible = False
                ddlcourse.Enabled = False
            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
#End Region

        'Private Function ValidationForNumber(ByVal value As Char) As Boolean
        '    ' Check key pressed for 0 to 9 and Backspace.
        '    If (value >= "0" And value <= "9") Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function



        'Protected Sub txt_birth_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_birth.TextChanged
        '    Try
        '        Dim bolNumber As Boolean
        '        bolNumber = ValidationForNumber(e.ToString)
        '        If bolNumber Then
        '            e.handled = False
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End Sub

        Private Sub btnclear_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnclear.Load

        End Sub


#Region "Genetare Random Password"
        Public Function GetRandomPasswordUsingGUID() As String
            Dim guidResult As String = System.Guid.NewGuid().ToString()
            Try
                guidResult = guidResult.Replace("-", String.Empty)
                'Make sure length is valid
                'Return the first length bytes
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



        Public Function CheckQuestions(ByVal Cid As String) As Hashtable
            Dim tot_Marks As Integer = 0
            Dim item As DictionaryEntry
            'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
            '  objconn = Nothing
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

                'For Each item In htSubQues
                '  htFinal.Add(item.Key, GetWeightStructure(item.Key, Cid, DirectCast(item.Value, Integer())))

                'Next
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                cds = Nothing
                cda = Nothing
                strPathDb = Nothing
                '    objconn = Nothing
                'item = Nothing
                'htFinal = Nothing
                'htSub = Nothing
            End Try
            Return htSubQues

        End Function

        Public Function GetMarksForSubject(ByVal Course_id As String, ByVal TotalMarks As Integer) As Hashtable
            Dim ht As New Hashtable

            Dim sds As DataSet
            Dim sda As SqlDataAdapter
            Dim subjects As New Hashtable
            Dim subMarks As New Hashtable
            Dim item As DictionaryEntry
            Try
                '    If objconn.connect(strPathDb) = True Then
                sds = New DataSet
                sda = New SqlDataAdapter("select test_type,Sub_Weightage from m_weightage where del_flag=0 and Course_id=" & Course_id, objconn.MyConnection)
                sda.Fill(sds)
                For i As Integer = 0 To sds.Tables(0).Rows.Count - 1
                    subjects.Add(sds.Tables(0).Rows(i).Item(0).ToString, sds.Tables(0).Rows(i).Item(1).ToString)
                Next
                '    End If

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



                '   End If
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


        'Public Function GetWeightStructure(ByVal testtype As String, ByVal courseid As String, ByVal arrVal As Integer()) As StructWeight
        '    Dim wtStruct As New StructWeight
        '    wtStruct.TestType = testtype
        '    Dim BQ As Integer = 0
        '    Dim Bper As Double = 0
        '    Dim IMQ As Integer = 0
        '    Dim IMPer As Double = 0

        '    Dim wds As DataSet
        '    Dim wda As SqlDataAdapter
        '    Dim query As String = "select Weightage_Id,Basic,InterMed from M_weightage where course_id=" & courseid & " and del_flag=0 and test_type=" & testtype
        '    Try
        '        If objconn.connect(strPathDb) = True Then
        '            wds = New DataSet
        '            wda = New SqlDataAdapter(query, objconn.MyConnection)
        '            wda.Fill(wds)
        '            Bper = CInt(wds.Tables(0).Rows(0).Item(1))
        '            Bper = Bper / 100
        '            IMPer = CInt(wds.Tables(0).Rows(0).Item(2))
        '            IMPer = IMPer / 100
        '        End If


        '        For i As Integer = 0 To 2
        '            Dim weight As Integer = arrVal(i)
        '            BQ = Math.Abs(weight * Bper)
        '            IMQ = Math.Abs(weight * IMPer)

        '            If (BQ + IMQ) <> weight Then
        '                While (BQ + IMQ) <> weight
        '                    If (BQ + IMQ) < weight Then
        '                        If (Bper > IMPer) Then
        '                            BQ = BQ + 1
        '                        Else
        '                            IMQ = IMQ + 1
        '                        End If


        '                    ElseIf (BQ + IMQ) > weight Then
        '                        If (Bper > IMPer) Then
        '                            BQ = BQ - 1
        '                        Else
        '                            IMQ = IMQ - 1
        '                        End If
        '                    End If
        '                End While
        '            End If

        '            If i = 0 Then
        '                wtStruct.TFBasic = BQ
        '                wtStruct.TFIMed = IMQ
        '            ElseIf i = 1 Then
        '                wtStruct.MCBasic = BQ
        '                wtStruct.MCIMed = IMQ
        '            ElseIf i = 2 Then
        '                wtStruct.BLBasic = BQ
        '                wtStruct.BLIMed = IMQ

        '            End If
        '        Next

        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        Throw ex
        '    Finally
        '        wda = Nothing
        '        wds = Nothing
        '    End Try
        '    Return wtStruct
        'End Function

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

    End Class

End Namespace