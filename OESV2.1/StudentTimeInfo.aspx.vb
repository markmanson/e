#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Web.UI
Imports System.Collections
Imports System.Web
Imports System.Configuration
Imports log4net
Imports System.Drawing
#End Region

#Region "Class StudentTimeInfo"
'Desc: This is class StudentTimeInfo.
'By: Monal Shah, 2011/07/12
Partial Public Class StudentTimeInfo
    Inherits BasePage
    Dim objconn As New ConnectDb

#Region "initials"
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("")
    Private strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
#End Region
#Region "Pageload event"
    'Desc: This is pageload event.
    'By: Monal Shah, 2011/07/12
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            errorMsg.Text = String.Empty
            TextFromApp.Attributes.Add("type", "date")
            TxtToApp.Attributes.Add("type", "date")
            If Not Page.IsPostBack Then
                populate_subjectid()
                BindCourse()
                gridDiv.Visible = False
                'Added by Pranit on 06/11/2019
                'DGData.DataSource = BindGrid()
                'DGData.DataBind()
            Else
                ' BindGrid()
                'Commented by Pranit Chimurkar on 2019/10/23
                'If DGData.Visible = True Then
                '    fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                'End If
            End If

            If Session("pid") <> Nothing Then

                Dim centre As Integer = CInt(Session("centre").ToString)
                Dim course As Integer = CInt(Session("course").ToString)
                'Dim cand As Integer = CInt(Session("cand").ToString)
                sel_subjectid.SelectedValue = centre
                'BindCourse()
                ddlcourse.SelectedValue = course
                ' BindCandidates()
                'ddlcandidates.SelectedValue = cand
                BindGrid()
                ddlcourse.Enabled = True
                'ddlcandidates.Enabled = True
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
#End Region
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
#Region "Method for Binding Centres"
    'Desc: This method binds centername dropdownlist.
    'By: Monal Shah, 2011/07/12

    Private Sub populate_subjectid()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Dim objconn As New ConnectDb
        Dim strbr As StringBuilder
        Try
            sqlstr = ""

            strbr = New StringBuilder
            strbr.Append(" SELECT Center_Id,Center_Name FROM M_Centers where del_flg=0 Order by Center_Name")
            sqlstr = strbr.ToString

            'myDataReader = retTestInfo(sqlstr)
            'Dim strPathDb As String
            If objconn.connect() Then
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                Dim myCommand As SqlCommand
                myCommand = New SqlCommand(strbr.ToString(), objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader

                myTable = New DataTable
                myTable.Columns.Add(New DataColumn("Center_Id", GetType(String)))
                myTable.Columns.Add(New DataColumn("Center_Name", GetType(String)))


                ' While loop to populate the Datatable

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
            If objconn IsNot Nothing Then
                objconn.disconnect()
            End If
            Throw ex
        Finally
            If objconn IsNot Nothing Then
                objconn.disconnect()
            End If
            sqlstr = Nothing
            myDataReader = Nothing
            myTable = Nothing
            myRow = Nothing
            strbr = Nothing
        End Try
    End Sub

#End Region

#Region "Function RetTestinfo"
    'Desc: This method returns datareader.
    'By: Jatin Gangajaliya, 2011/03/21

    'Private Function retTestInfo(ByVal sqlstr As String) As SqlDataReader
    '    Dim objconn As New ConnectDb
    '    Try
    '        Dim myCommand As SqlCommand
    '        If objconn.connect(strPathDb) Then
    '            myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
    '            retTestInfo = myCommand.ExecuteReader
    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Throw ex
    '    Finally
    '        objconn = Nothing
    '    End Try
    'End Function

#End Region

#Region "Bind Course DropdownList"
    'Desc: This method Binds Course DropdownList.
    'By: Monal Shah, 2011/2/22

    Public Sub BindCourse()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Dim strbr As StringBuilder
        Dim objconn As New ConnectDb
        Try

            strbr = New StringBuilder()
            strbr.Append(" Select Course_id,Course_name from M_Course ")
            strbr.Append(" where del_flag=0 order by Course_name ")
            sqlstr = strbr.ToString()

            'myDataReader = retTestInfo(sqlstr)
            myTable = New DataTable
            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                Dim myCommand As SqlCommand
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader

                myTable.Columns.Add(New DataColumn("CourseID", GetType(String)))
                myTable.Columns.Add(New DataColumn("CourseName", GetType(String)))

                'While loop to populate the Datatable
                While myDataReader.Read
                    myRow = myTable.NewRow
                    myRow(0) = myDataReader.Item("Course_id")
                    myRow(1) = myDataReader.Item("Course_name")
                    myTable.Rows.Add(myRow)
                End While
            End If
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
            strbr = Nothing
            myDataReader = Nothing
            myTable = Nothing
            myRow = Nothing
        End Try

    End Sub
#End Region

    '#Region "Bind Course DropdownList"
    '    'Desc: This method Binds Course DropdownList.
    '    'By: Monal Shah, 2011/2/22

    '    Public Sub BindCourse()
    '        Dim myDataReader As SqlDataReader
    '        Dim sqlstr As String
    '        Dim myTable As DataTable
    '        Dim myRow As DataRow
    '        Dim strbr As StringBuilder
    '        Dim objconn As New ConnectDb
    '        Try

    '            strbr = New StringBuilder()
    '            strbr.Append(" Select M_Course.Course_id,M_Course.Course_name from M_Course ")
    '            strbr.Append(" join T_Candidate_Status on T_Candidate_Status.Course_ID = M_Course.Course_id ")
    '            strbr.Append(" join T_Center_Course on M_Course.Course_id = T_Center_Course.Course_id ")
    '            strbr.Append(" where T_Candidate_Status.Appearedflag = 2  ")
    '            strbr.Append(" AND T_Center_Course.Center_id = ")
    '            strbr.Append(sel_subjectid.SelectedValue)
    '            strbr.Append(" group by M_Course.Course_id,M_Course.Course_name ")
    '            strbr.Append(" order by M_Course.Course_name ")
    '            sqlstr = strbr.ToString()

    '            'myDataReader = retTestInfo(sqlstr)
    '            myTable = New DataTable
    '            Dim strPathDb As String
    '            strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '            If objconn.connect(strPathDb) Then
    '                Dim myCommand As SqlCommand
    '                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
    '                myDataReader = myCommand.ExecuteReader

    '                myTable.Columns.Add(New DataColumn("CourseID", GetType(String)))
    '                myTable.Columns.Add(New DataColumn("CourseName", GetType(String)))

    '                'While loop to populate the Datatable
    '                While myDataReader.Read
    '                    myRow = myTable.NewRow
    '                    myRow(0) = myDataReader.Item("Course_id")
    '                    myRow(1) = myDataReader.Item("Course_name")
    '                    myTable.Rows.Add(myRow)
    '                End While
    '            End If
    '            ddlcourse.DataSource = myTable
    '            ddlcourse.DataTextField = "CourseName"
    '            ddlcourse.DataValueField = "CourseID"
    '            ddlcourse.DataBind()
    '            ddlcourse.Items.Insert(0, New ListItem("--Select--", "0"))
    '            myDataReader.Close()

    '        Catch ex As Exception
    '            If log.IsDebugEnabled Then
    '                log.Debug("Error :" & ex.ToString())
    '            End If
    '            If objconn IsNot Nothing Then
    '                objconn.disconnect()
    '            End If
    '            Throw ex
    '        Finally
    '            If objconn IsNot Nothing Then
    '                objconn.disconnect()
    '            End If
    '            objconn = Nothing
    '            sqlstr = Nothing
    '            strbr = Nothing
    '            myDataReader = Nothing
    '            myTable = Nothing
    '            myRow = Nothing
    '        End Try

    '    End Sub
    '#End Region

#Region "BindCandidates"
    'Desc: This method binds candidates.
    'By: Monal Shah, 2011/07/12

    Public Sub BindCandidates()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Dim strbr As StringBuilder
        Dim objconn As New ConnectDb
        Try
            strbr = New StringBuilder()
            strbr.Append(" Select M_USER_INFO.userid, M_USER_INFO.surname+' '+M_USER_INFO.name+ ' ' + isnull(M_USER_INFO.Middlename,'') as FullName from M_USER_INFO  ")
            strbr.Append(" inner join T_User_Course on M_USER_INFO.userid = T_User_Course.user_id ")
            strbr.Append(" inner join T_Candidate_Status on T_Candidate_Status.UserId = M_USER_INFO.userid ")
            strbr.Append(" where T_Candidate_Status.Appearedflag = 2 ")
            strbr.Append(" AND M_USER_INFO.user_type = 0 ")
            strbr.Append(" AND M_USER_INFO.Center_ID = ")
            strbr.Append(sel_subjectid.SelectedValue)
            strbr.Append(" AND T_User_Course.Course_ID = ")
            strbr.Append(ddlcourse.SelectedValue)
            strbr.Append(" Group by M_USER_INFO.userid,M_USER_INFO.name,M_USER_INFO.surname,M_USER_INFO.Middlename ")
            strbr.Append(" Order by FullName  ")
            sqlstr = strbr.ToString()

            'myDataReader = retTestInfo(sqlstr)

            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                myTable = New DataTable
                Dim myCommand As SqlCommand
                myCommand = New SqlCommand(strbr.ToString(), objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader

                myTable.Columns.Add(New DataColumn("userid", GetType(String)))
                myTable.Columns.Add(New DataColumn("FullName", GetType(String)))

                'While loop to populate the Datatable
                While myDataReader.Read
                    myRow = myTable.NewRow
                    myRow(0) = myDataReader.Item("userid")
                    myRow(1) = myDataReader.Item("FullName")
                    myTable.Rows.Add(myRow)
                End While
            End If
            'ddlcandidates.DataSource = myTable
            'ddlcandidates.DataTextField = "FullName"
            'ddlcandidates.DataValueField = "userid"
            'ddlcandidates.DataBind()
            'ddlcandidates.Items.Insert(0, New ListItem("--Select--", "0"))
            myDataReader.Close()

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
                If objconn IsNot Nothing Then
                    objconn.disconnect()
                End If
            End If
            Throw ex
        Finally
            If objconn IsNot Nothing Then
                objconn.disconnect()
            End If
            objconn = Nothing
            sqlstr = Nothing
            strbr = Nothing
            myDataReader = Nothing
            myTable = Nothing
            myRow = Nothing
        End Try
    End Sub
#End Region
#Region "Search Button Click event"
    'Desc: This is search button click event.
    'By: Monal Shah, 2011/07/12.
    'Added by Pranit Chimurkar on 2019/10/21
    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        Try
            'If (sel_subjectid.SelectedValue = 0) Then
            '    errorMsg.Text = "Please select Centre Name"
            '    gridDiv.Visible = False
            'ElseIf ddlcourse.SelectedValue = 0 Then
            '    errorMsg.Text = "Please select Course Name"
            '    gridDiv.Visible = False
            'ElseIf ddlcandidates.SelectedValue = 0 Then
            '    errorMsg.Text = "Please select Candidate Name"
            '    gridDiv.Visible = False
            'Else
            DGData.CurrentPageIndex = 0
            BindGrid()
            'gridDiv.Visible = True
            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try


    End Sub
#End Region
#Region "Clear Button Click eveent"
    'Desc: This is clear button click event.
    'By: Monal Shah, 2011/07/12.
    'Added by Pranit Chimurkar on 2019/10/21
    Protected Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        Try
            sel_subjectid.SelectedValue = 0
            If ddlcourse.Items.Count >= 0 Then
                ddlcourse.SelectedValue = 0
                'ddlcourse.Enabled = False
            End If
            'If ddlcandidates.Items.Count >= 0 Then
            '    ddlcandidates.SelectedValue = 0
            '    ddlcandidates.Enabled = False
            'End If
            errorMsg.Text = String.Empty
            gridDiv.Visible = False
            sel_subjectid.Focus()
            TxtToApp.Value = String.Empty
            TextFromApp.Value = String.Empty
            txtCandidateName.Text = String.Empty
            ViewState.Remove("selval")
            ViewState.Remove("pageNo")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
#End Region

#Region "Back Button Click event"
    'Desc: This is back button click event.
    'By: Monal Shah, 2011/07/12

    'Added by Pranit Chimurkar on 2019/10/21
    'Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click

    '    Response.Redirect("admin.aspx", False)
    'End Sub
#End Region
#Region "Bindgrid method"
    'Desc: This method binds data to datagrid.
    'By: Monal Shah, 2011/07/12.

    'Modified by Pranit on 06/11/2019
    Private Sub BindGrid()
        Dim strquery As String
        Dim strquerys As String
        Dim objconn As New ConnectDb
        Dim myTable, dtDisable As DataTable
        Dim myLBTable As DataTable
        Dim strbr As StringBuilder
        'Dim str As StringBuilder
        Dim adap As SqlDataAdapter
        Dim adapLb As SqlDataAdapter
        Dim col As DataColumn

        Try
            myTable = New DataTable
            myLBTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strbr = New StringBuilder()
            strbr.Append("  ")

            '************Start
            'strbr.Append(" select sf.User_id,sf.Course_id,convert(varchar(10),sf.start_time,103)+' '+convert(varchar,sf.start_time,108) as Start_time, ")
            'strbr.Append(" convert(varchar(10),sf.end_time,103)+' '+convert(varchar,sf.end_time,108) as End_time, ")
            'strbr.Append(" DATEDIFF(minute, sf.start_time, sf.End_time) AS Differences,TS.Total_Time AS Total_Time ")
            'strbr.Append(" from ")
            'strbr.Append(" student_time_info AS sf Inner Join T_Candidate_Status AS TS On sf.Course_id=TS.Course_ID AND sf.User_id=TS.Userid where sf.user_id='" & ddlcandidates.SelectedValue & "' and sf.course_id='" & ddlcourse.SelectedValue & "'")
            'strbr.Append(" order by sf.user_id ")

            'strbr.Append("select sf.User_id,sf.Course_id,convert(varchar(10),sf.start_time,103)+' '+convert(varchar,sf.start_time,108) as Start_time,convert(varchar(10),sf.end_time,103)+' '+convert(varchar,sf.end_time,108) as End_time,DATEDIFF(minute, sf.start_time, sf.End_time) AS Differences,TS.Total_Time AS Total_Time,mu.Surname+' '+mu.Name+' '+mu.middlename As User_Name,MC.Course_Name as Course_Name from student_time_info AS sf Inner Join T_Candidate_Status AS TS On sf.Course_id=TS.Course_ID AND sf.User_id=TS.Userid Inner join m_user_info AS mu on  sf.User_id=mu.userid inner join m_Course as MC on sf.course_id=MC.Course_id   where sf.user_id='" & ddlcandidates.SelectedValue & "' and sf.course_id='" & ddlcourse.SelectedValue & "'")
            'strbr.Append(" where sf.user_id='" & ddlcandidates.SelectedValue & "' and sf.course_id='" & ddlcourse.SelectedValue & "'")
            'Commented code Monal shah
            'strbr.Append("select sf.User_id,sf.Course_id,convert(varchar(10),sf.start_time,103)+' '+convert(varchar,sf.start_time,108) as Start_time,convert(varchar(10),sf.end_time,103)+' '+convert(varchar,sf.end_time,108) as End_time,DATEDIFF(minute, sf.start_time, sf.End_time) AS Differences,TS.Total_Time AS Total_Time,mu.Surname+' '+mu.Name+' '+mu.middlename As User_Name,MC.Course_Name as Course_Name,CT.Center_Name As Center_Name from student_time_info AS sf Inner Join T_Candidate_Status AS TS On sf.Course_id=TS.Course_ID AND sf.User_id=TS.Userid Inner join m_user_info AS mu on  sf.User_id=mu.userid inner join m_Course as MC on sf.course_id=MC.Course_id Inner join m_Centers as CT on CT.Center_ID='" & sel_subjectid.SelectedValue & "'")
            'If (sel_subjectid.SelectedIndex > 0 Or ddlcourse.SelectedIndex > 0 Or ddlcandidates.SelectedIndex > 0 Or TextFromApp.Text <> String.Empty Or TextFromApp.Text <> "" Or TxtToApp.Text <> String.Empty Or TxtToApp.Text <> "" Or txtFromTime.Text <> String.Empty Or txtFromTime.Text <> "" Or TxtToTime.Text <> String.Empty Or TxtToTime.Text <> "") Then
            '    strbr.Append(" where ")
            '    '     strbr.Append(" where sf.user_id='" & ddlcandidates.SelectedValue & "' and sf.course_id='" & ddlcourse.SelectedValue & "'")
            'End If
            'If (sel_subjectid.SelectedIndex > 0) Then
            '    strbr.Append("CT.Center_ID='" & sel_subjectid.SelectedValue & "'")
            'End If
            'If (ddlcourse.SelectedIndex > 0) Then
            '    If (sel_subjectid.SelectedIndex > 0) Then
            '        strbr.Append(" and ")
            '        strbr.Append("sf.course_id='" & ddlcourse.SelectedValue & "'")
            '    Else
            '        strbr.Append("sf.course_id='" & ddlcourse.SelectedValue & "'")
            '    End If

            'End If
            'If (ddlcandidates.SelectedIndex > 0) Then
            '    If (ddlcourse.SelectedIndex > 0 Or sel_subjectid.SelectedIndex > 0) Then
            '        strbr.Append(" and ")
            '        strbr.Append("sf.user_id='" & ddlcandidates.SelectedValue & "'")
            '    Else
            '        strbr.Append("sf.user_id='" & ddlcandidates.SelectedValue & "'")
            '    End If

            'End If
            'If (TextFromApp.Text <> String.Empty Or TextFromApp.Text <> "") Then

            '    If (TextFromApp.Text <> "" And IsDate(ConvertDate(TextFromApp.Text))) Then
            '        dt2 = Convert.ToDateTime(ConvertDate(TextFromApp.Text))
            '    End If


            '    If (ddlcandidates.SelectedIndex > 0 Or ddlcourse.SelectedIndex > 0 Or sel_subjectid.SelectedIndex > 0) Then
            '        strbr.Append(" and ")
            '        strbr.Append("sf.Start_time='" & dt2 & "'")
            '    Else
            '        strbr.Append("sf.Start_time='" & dt2 & "'")
            '    End If

            'End If
            'If (TxtToApp.Text <> "" Or TxtToApp.Text <> String.Empty) Then
            '    If (TxtToApp.Text <> "" And IsDate(ConvertDate(TxtToApp.Text))) Then
            '        dt1 = Convert.ToDateTime(ConvertDate(TxtToApp.Text))
            '    End If
            '    If (TextFromApp.Text <> String.Empty Or TextFromApp.Text <> "" Or ddlcandidates.SelectedIndex > 0 Or ddlcourse.SelectedIndex > 0 Or sel_subjectid.SelectedIndex > 0) Then
            '        strbr.Append(" and ")
            '        strbr.Append("sf.End_time='" & dt1 & "'")
            '    Else
            '        strbr.Append("sf.End_time='" & dt1 & "'")
            '    End If

            'End If
            'If (txtFromTime.Text <> "" Or txtFromTime.Text <> String.Empty) Then
            '    If (TextFromApp.Text <> String.Empty Or TextFromApp.Text <> "" Or TxtToApp.Text <> "" Or TxtToApp.Text <> String.Empty Or ddlcandidates.SelectedIndex > 0 Or ddlcourse.SelectedIndex > 0 Or sel_subjectid.SelectedIndex > 0) Then
            '        strbr.Append(" and ")
            '        strbr.Append("sf.Start_time='" & txtFromTime.Text & "'")
            '    Else
            '        strbr.Append("sf.Start_time='" & txtFromTime.Text & "'")
            '    End If

            'End If
            'If (TxtToTime.Text <> "" Or TxtToTime.Text <> String.Empty) Then
            '    If (TextFromApp.Text <> String.Empty Or TextFromApp.Text <> "" Or TxtToApp.Text <> "" Or txtFromTime.Text <> "" Or txtFromTime.Text <> String.Empty Or TxtToApp.Text <> String.Empty Or ddlcandidates.SelectedIndex > 0 Or ddlcourse.SelectedIndex > 0 Or sel_subjectid.SelectedIndex > 0) Then
            '        strbr.Append(" and ")
            '        strbr.Append("sf.End_time='" & TxtToTime.Text & "'")
            '    Else
            '        strbr.Append("sf.End_time='" & TxtToTime.Text & "'")
            '    End If

            'End If

            'Comment End

            '************End
            '***********************************Monal Shah***********************************************************************
            'strbr.Append("select sf.User_id,sf.Course_id,convert(varchar(10),sf.start_time,103)+' '+convert(varchar,sf.start_time,108) as Start_time, ")
            'strbr.Append(" convert(varchar(10),sf.end_time,103)+' '+convert(varchar,sf.end_time,108) as End_time, ")
            'strbr.Append(" DATEDIFF(minute, sf.start_time, sf.End_time) AS Differences,TS.Total_Time AS Total_Time, ")
            'strbr.Append(" isnull(mu.Surname,' ')+' '+isnull(mu.Name,' ')+' '+isnull(mu.middlename,' ') As User_Name, ")
            'strbr.Append(" MC.Course_Name as Course_Name from student_time_info AS sf ")
            'strbr.Append(" Inner Join T_Candidate_Status AS TS On sf.Course_id=TS.Course_ID AND sf.User_id=TS.Userid ")
            'strbr.Append(" Inner join m_user_info AS mu on  sf.User_id=mu.userid ")
            'strbr.Append(" inner join m_Course as MC on sf.course_id=MC.Course_id ")
            ''strbr.Append(" where sf.user_id='" & ddlcandidates.SelectedValue & "' and sf.course_id='" & ddlcourse.SelectedValue & "'")
            'str = New StringBuilder()
            'str.Append("  ")

            ''************Start
            ''str.Append(" select Sum(DATEDIFF(minute,start_time,End_time)) AS Differences ")
            ''str.Append(" from ")
            ''str.Append(" student_time_info where user_id='" & ddlcandidates.SelectedValue & "' and course_id='" & ddlcourse.SelectedValue & "'")
            'str.Append(" select Sum(DATEDIFF(minute,start_time,End_time)) AS Differences ")
            'str.Append(" from ")
            ''str.Append(" student_time_info where user_id='" & ddlcandidates.SelectedValue & "' and course_id='" & ddlcourse.SelectedValue & "'")

            ''************End

            'strquery = strbr.ToString()
            'strquerys = str.ToString()
            'If objconn.connect(strPathDb) Then
            '    adap = New SqlDataAdapter(strquery, objconn.MyConnection)
            '    adapLb = New SqlDataAdapter(strquerys, objconn.MyConnection)
            '    adap.Fill(myTable)
            '    adapLb.Fill(myLBTable)
            '    If myTable.Rows.Count > 0 Then
            '        gridDiv.Visible = True
            '        DGData.DataSource = myTable

            '        If Session("pid") <> Nothing Then
            '            'If Request.QueryString("pi") <> Nothing Then
            '            DGData.CurrentPageIndex = CInt(Session("pid").ToString())
            '            ViewState.Add("selval", DGData.CurrentPageIndex)
            '            'End If
            '            Session.Remove("pid")
            '        End If
            '        DGData.DataBind()
            '        If myLBTable.Rows.Count > 0 Then
            '            'lblTotaltime.Visible = True
            '            ' lblTotaltime.Text = "Total Minutes:" & myLBTable.Rows(0)(0).ToString() & ""
            '        End If

            '        fillPagesCombo()
            '        fillPageNumbers(DGData.CurrentPageIndex + 1, 9)

            '        lblrecords.Text = "Total Records:" & myTable.Rows.Count
            '        errorMsg.Text = String.Empty


            '    Else
            '        gridDiv.Visible = False
            '        ' lblTotaltime.Visible = False
            '        errorMsg.Text = "No Record(s) Found"
            '    End If
            'End If
            '***********************************Monal Shah***********************************************************************
            '***************Add by Monal shah*****2011/8/5*******************************
            strbr.Append("select temp.attempt as attempt,isnull(u.Surname,' ')+' '+isnull(u.Name,' ')+' '+isnull(u.middlename,' ') As User_Name,mc.center_name as CenterName,mco.course_name, ")
            'strbr.Append(" tcs.written_test_appear_date as AppearedDate,tcs.total_time as Total_Time,temp.sum as Summ,mco.course_id,mc.center_id,sti.user_id as User_Id ")
            strbr.Append(" tcs.written_test_appear_date as AppearedDate,tcs.total_time as Total_Time,temp.sum as Summ,mco.course_id,mc.center_id")
            strbr.Append(" from student_time_info as sti left join m_user_info as u  on u.userid=sti.user_id ")
            strbr.Append(" left join m_centers as mc  on u.center_id=mc.center_id left join t_candidate_status as tcs ")
            strbr.Append(" on tcs.userid=u.userid and tcs.userid=u.userid left join m_course as mco on mco.course_id=tcs.course_id ")
            strbr.Append(" left join (select sum(datediff(minute,sti.start_time,sti.end_time)) as SUM,sti.user_id,sti.course_id,COUNT(STI.user_id)as attempt from student_time_info as sti ")
            strbr.Append(" left join t_candidate_status as tcs on tcs.userid=sti.user_id and tcs.course_id=sti.course_id group by sti.user_id,sti.course_id) temp ")
            strbr.Append(" on temp.user_id=tcs.userid and temp.course_id=tcs.course_id ")

            If (sel_subjectid.SelectedIndex <> 0 Or ddlcourse.SelectedIndex <> 0 Or txtCandidateName.Text <> Nothing Or txtCandidateName.Text <> "" Or TextFromApp.Value <> Nothing Or TextFromApp.Value <> "" Or TxtToApp.Value <> Nothing Or TxtToApp.Value <> "") Then
                strbr.Append(" Where ")
                If (sel_subjectid.SelectedIndex <> 0) Then
                    strbr.Append(" mc.center_id ='" & sel_subjectid.SelectedValue & "' ")
                End If
                If (ddlcourse.SelectedIndex <> 0) Then
                    If (sel_subjectid.SelectedIndex <> 0) Then
                        strbr.Append(" and ")
                        strbr.Append(" mco.course_id='" & ddlcourse.SelectedValue & "' ")
                    Else
                        strbr.Append(" mco.course_id='" & ddlcourse.SelectedValue & "' ")
                    End If
                End If
                If (txtCandidateName.Text <> Nothing Or txtCandidateName.Text <> "") Then
                    If (sel_subjectid.SelectedIndex <> 0 Or ddlcourse.SelectedIndex <> 0) Then
                        strbr.Append(" and ")
                        strbr.Append(" (isnull(u.Surname,' ') LIKE '%" & txtCandidateName.Text & "%' or isnull(u.Name,' ') LIKE '%" & txtCandidateName.Text & "%' or isnull(u.middlename,' ') LIKE '%" & txtCandidateName.Text & "%') ")
                    Else
                        strbr.Append(" (isnull(u.Surname,' ') LIKE '%" & txtCandidateName.Text & "%' or isnull(u.Name,' ') LIKE '%" & txtCandidateName.Text & "%' or isnull(u.middlename,' ') LIKE '%" & txtCandidateName.Text & "%') ")
                    End If
                End If
                If ((TextFromApp.Value <> Nothing Or TextFromApp.Value <> "") Or (TxtToApp.Value <> Nothing Or TxtToApp.Value <> "")) Then
                    If (sel_subjectid.SelectedIndex <> 0 Or ddlcourse.SelectedIndex <> 0 Or txtCandidateName.Text <> Nothing Or txtCandidateName.Text <> "") Then
                        strbr.Append(" and ")
                        If ((TextFromApp.Value <> Nothing Or TextFromApp.Value <> "") And (TxtToApp.Value <> Nothing Or TxtToApp.Value <> "")) Then
                            strbr.Append("tcs.written_test_appear_date Between '" & ConvertDate2(TextFromApp.Value) & " 00:00:00'  and  '" & ConvertDate2(TxtToApp.Value) & " 23:59:59'")
                        ElseIf Not TextFromApp.Value.Trim() = "" Then
                            strbr.Append("tcs.written_test_appear_date >'" & ConvertDate2(TextFromApp.Value) & " 00:00:00'")
                        ElseIf Not TxtToApp.Value.Trim() = "" Then
                            strbr.Append("tcs.written_test_appear_date <= '" & ConvertDate2(TxtToApp.Value) & " 23:59:59' ")
                        End If
                    Else
                        If ((TextFromApp.Value <> Nothing Or TextFromApp.Value <> "") And (TxtToApp.Value <> Nothing Or TxtToApp.Value <> "")) Then
                            strbr.Append("tcs.written_test_appear_date Between '" & ConvertDate2(TextFromApp.Value) & " 00:00:00'  and  '" & ConvertDate2(TxtToApp.Value) & " 23:59:59'")
                        ElseIf Not TextFromApp.Value.Trim() = "" Then
                            strbr.Append("tcs.written_test_appear_date >'" & ConvertDate2(TextFromApp.Value) & " 00:00:00'")
                        ElseIf Not TxtToApp.Value.Trim() = "" Then
                            strbr.Append("tcs.written_test_appear_date <= '" & ConvertDate2(TxtToApp.Value) & " 23:59:59' ")
                        End If
                    End If

                End If

                'If ((TextFromApp.Text <> Nothing Or TextFromApp.Text <> "") Or (TxtToApp.Text <> Nothing Or TxtToApp.Text <> "")) Then
                '    If Not TextFromApp.Text.Trim() = "" And Not TxtToApp.Text.Trim() = "" Then
                '        strbr.Append("tcs.written_test_appear_date '", TextFromApp.Text + "'  and  '" + TextFromApp.Text + " 23:59:59'")
                '    ElseIf Not TextFromApp.Text.Trim() = "" Then
                '        strbr.Append("tcs.written_test_appear_date", TextFromApp.Text)
                '    End If
                'End If
            End If
            strbr.Append(" and tcs.appearedflag=2 ")
            strbr.Append(" group by sti.user_id ,u.surName,u.name,mc.center_name,tcs.total_time,mco.course_name,temp.attempt,temp.sum,mco.course_id, mc.center_id, tcs.written_test_appear_date, u.middlename order by temp.attempt desc")


            strquery = strbr.ToString()
            'strquerys = str.ToString()

            'Added by Pranit on 06/11/2019
            'Session("Source") = myTable
            'Dim dv As DataView = New DataView(myTable)

            If objconn.connect() Then
                adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                'adapLb = New SqlDataAdapter(strquerys, objconn.MyConnection)
                adap.Fill(myTable)
                'adapLb.Fill(myLBTable)
                If myTable.Rows.Count > 0 Then
                    gridDiv.Visible = True
                    DGData.DataSource = myTable

                    If Session("pid") <> Nothing Then
                        'If Request.QueryString("pi") <> Nothing Then
                        DGData.CurrentPageIndex = CInt(Session("pid").ToString())
                        ViewState.Add("selval", DGData.CurrentPageIndex)
                        'End If
                        Session.Remove("pid")
                    End If
                    DGData.DataBind()
                    'If myLBTable.Rows.Count > 0 Then
                    '    'lblTotaltime.Visible = True
                    '    ' lblTotaltime.Text = "Total Minutes:" & myLBTable.Rows(0)(0).ToString() & ""
                    'End If

                    'fillPagesCombo()
                    'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)

                    lblrecords.Text = ":" & myTable.Rows.Count
                    errorMsg.Text = String.Empty

                    'Added By : Saraswati Patel
                    'Disc   : for datagrid row color is gray if course is diable.
                    For dis As Integer = 0 To DGData.Items.Count - 1
                        If DGData.Items(dis).Cells(1).Text <> "&nbsp;" Then
                            Dim query As String = "select Del_Flag from M_Course where Course_id=" + DGData.Items(dis).Cells(1).Text
                            adap = Nothing
                            dtDisable = Nothing
                            dtDisable = New DataTable
                            adap = New SqlDataAdapter(query, objconn.MyConnection)
                            adap.Fill(dtDisable)
                            If dtDisable.Rows.Count <> 0 Then
                                If dtDisable.Rows(0).Item(0).ToString = "True" Then
                                    DGData.Items(dis).BackColor = Drawing.Color.Gray
                                End If
                            End If
                        End If
                    Next

                Else
                    gridDiv.Visible = False
                    ' lblTotaltime.Visible = False
                    errorMsg.Text = Resources.Resource.Common_NoRecFound
                End If
            End If

            '*********End******Add by Monal shah*****2011/8/5*******************************
            'Added by Pranit on 05/11/2019
            'Return dv
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            If objconn IsNot Nothing Then
                objconn.disconnect()
            End If
            Throw ex
        Finally
            strquery = Nothing
            strquerys = Nothing
            strbr = Nothing
            col = Nothing
            adap = Nothing
            adapLb = Nothing
            myLBTable = Nothing
            strPathDb = Nothing
            If objconn IsNot Nothing Then
                objconn.disconnect()
            End If
            objconn = Nothing
        End Try
    End Sub
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
        End Try '
    End Sub

    'Added by Pranit on 06/11/2019
#Region "Sorting"
    'Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '    Dim dt As DataTable = CType(Session("Source"), DataTable)
    '    Dim dv As DataView = New DataView(dt)
    '    dv.Sort = e.SortExpression
    '    DGData.DataSource = dv
    '    DGData.DataBind()
    'End Sub 'Sort_Grid
#End Region

#Region "Course selectedindex change event"
    'Desc: This is course dropdown selected index change event.
    'By: Monal Shah, 2011/07/12
    Protected Sub ddlcourse_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlcourse.SelectedIndexChanged
        Try
            'If (sel_subjectid.SelectedIndex > 0) And (ddlcourse.SelectedIndex > 0) Then
            '    'ddlcandidates.Enabled = True
            '    'BtnSearch.Enabled = True
            '    BtnSearch.ToolTip = ""
            '    ' BindCandidates()
            'ElseIf (ddlcourse.SelectedIndex = 0) Then
            '    If ddlcourse.Items.Count > 0 Then
            '        ' ddlcandidates.SelectedValue = 0
            '    End If
            '    ' ddlcandidates.Enabled = False
            '    gridDiv.Visible = False
            '    'BtnSearch.Enabled = False
            '    'BtnSearch.ToolTip = "Select the Course First"
            '    'ElseIf (ddlcandidates.SelectedIndex = 0) Then
            '    '    BtnSearch.Enabled = False
            '    '    BtnSearch.ToolTip = "Select the Candidate First"
            'End If
        Catch ex As Exception
            If log.IsDebugEnabled Then

                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
#End Region

#Region "Center dropdown selectedindex change event"
    'Desc: This is Center dropdown selectedindex change event.
    'By: Monal Shah, 2011/07/12
    Protected Sub sel_subjectid_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles sel_subjectid.SelectedIndexChanged

        Try
            If (sel_subjectid.SelectedIndex > 0) Then
                'ddlcourse.Enabled = True
                ' BtnSearch.Enabled = True
                BtnSearch.ToolTip = ""
                ' BindCourse()
            ElseIf (sel_subjectid.SelectedIndex = 0) Then
                If (sel_subjectid.Items.Count > 0) Then
                    ddlcourse.SelectedValue = 0
                    ' ddlcandidates.SelectedIndex = 0
                End If
                gridDiv.Visible = False
                'ddlcourse.Enabled = False
                'ddlcandidates.Enabled = False
                'BtnSearch.Enabled = False
                'BtnSearch.ToolTip = "Select the Center First"
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then

                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")

        End Try
    End Sub
#End Region
    'Commented by Pranit Chimurkar on 2019/10/23
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


    '    If len >= DGData.PageCount Then
    '        len = DGData.PageCount - 1
    '    End If

    '    ' if search clicked again then page 1 should be selected 
    '    If DGData.CurrentPageIndex = 0 Then
    '        ViewState("pageNo") = 1
    '        ViewState("lastRange") = 1
    '    End If

    '    ' Getting the currently selected page value 
    '    Dim selPage As Integer = 0
    '    If (ViewState("pageNo") <> Nothing) Then
    '        selPage = CInt(ViewState("pageNo"))
    '    Else
    '        ' selPage = 1
    '        selPage = DGData.CurrentPageIndex + 1
    '    End If

    '    If (ViewState("lastRange") <> Nothing) Then

    '        '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DGData.PageCount Then
    '        If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
    '            range = CInt(ViewState("lastRange"))
    '        Else
    '            'If it is the last page then resetting the page numbers
    '            ' last number - provided length
    '            'If (len + selPage) >= DGData.PageCount Then
    '            '    If selPage <= len Then
    '            '        range = range
    '            '    Else
    '            '        range = DGData.PageCount - len
    '            '        'Incase range becomes 0 or less than zero than setting it 1 
    '            '        If range <= 0 Then
    '            '            range = 1
    '            '        End If
    '            '    End If

    '            'Else
    '            If selPage <= DGData.PageCount Then
    '                'range = range
    '                If range < CInt(ViewState("lastRange")) Then
    '                    range = CInt(ViewState("lastRange")) - 1
    '                Else
    '                    If selPage - len > 0 And selPage - len <= DGData.PageCount - len Then
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

    '    'tblPagebuttons.Rows(0).Cells.Clear()

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
    '    'If selPage = 1 And selPage = DGData.PageCount - 1 Then
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
    '    If selPage = DGData.PageCount Then
    '        imgnext.Enabled = False
    '        imglast.Enabled = False
    '    End If
    'End Sub
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

    'Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPages.SelectedIndexChanged
    '    DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    BindGrid()
    'End Sub

#Region "Datagrid pageindex change event"

    Private Sub DGData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGData.ItemCommand
        Try
            If (e.CommandName = "View") Then
                Dim strcandidateid As String = e.Item.Cells(1).Text
                ' Session variables to be used on AnswerDetail page
                Session.Add("userid", strcandidateid)
                Session.Add("test_type", e.Item.Cells(2).Text)
                Session.Add("totMarks", e.Item.Cells(4).Text)
                Session.Add("obMarks", e.Item.Cells(5).Text)
                Session.Add("qno", e.Item.Cells(7).Text)
                Session.Add("srno", e.Item.Cells(0).Text)

                ' Session variables to be used for maintaining last page state
                Session.Add("centre", sel_subjectid.SelectedValue)
                Session.Add("course", ddlcourse.SelectedValue)
                'Session.Add("cand", ddlcandidates.SelectedValue)

                Response.Redirect("AnswerDetail.aspx?pi=" & DGData.CurrentPageIndex, False)
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Private Sub DGData_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGData.ItemDataBound
        If Not e.Item.ItemType = DataControlRowType.Header Then
            e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
        End If
    End Sub
    'Desc: This is Datagrid pageindex change event.
    'By: Monal Shah, 2011/07/12.
    Protected Sub DGData_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGData.PageIndexChanged
        Try
            DGData.CurrentPageIndex = e.NewPageIndex
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
#End Region


End Class
#End Region