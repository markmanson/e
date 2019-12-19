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
Imports System.Runtime.InteropServices
Imports log4net
Imports System.Drawing
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel

#End Region

#Region "Class AppearedExam"
'Desc: This is class AppearedExam.
'By: Jatin Gangajaliya, 2011/03/21

Partial Public Class AppearedExam
    Inherits BasePage
    Dim objconn As New ConnectDb

#Region "initials"
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("searchExamAppeared")
    Private strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
#End Region

#Region "Pageload event"
    'Desc: This is pageload event.
    'By: Jatin Gangajaliya, 2011/03/21

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            errorMsg.Text = String.Empty
            If Not Page.IsPostBack Then
                populate_subjectid()
            Else
                ' BindGrid()
                If DGData.Visible = True Then
                    'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                End If
            End If

            If Session("pid") <> Nothing Then

                Dim centre As Integer = CInt(Session("centre").ToString)
                Dim course As Integer = CInt(Session("course").ToString)
                Dim cand As Integer = CInt(Session("cand").ToString)
                sel_subjectid.SelectedValue = centre
                BindCourse()
                ddlcourse.SelectedValue = course
                BindCandidates()
                ddlcandidates.SelectedValue = cand
                BindGrid()
                ddlcourse.Enabled = True
                ddlcandidates.Enabled = True
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
    'By: Jatin Gangajaliya, 2011/03/21

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
            strbr.Append(" SELECT Center_Id,Center_Name FROM M_Centers Order by Center_Name")
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

#Region "Function RetTestinfo"
    'Desc: This method returns datareader.
    'By: Jatin Gangajaliya, 2011/03/21

    Private Function retTestInfo(ByVal sqlstr As String) As SqlDataReader
        Dim objconn As New ConnectDb
        Try
            Dim myCommand As SqlCommand
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

#Region "Center dropdown selectedindex change event"
    'Desc: This is Center dropdown selectedindex change event.
    'By: Jatin Gangajaliya, 2011/03/21
    Protected Sub sel_subjectid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sel_subjectid.SelectedIndexChanged
        Try
            If (sel_subjectid.SelectedIndex > 0) Then
                ddlcourse.Enabled = True
                BtnSearch.Enabled = True
                ddlcandidates.Enabled = False
                BtnSearch.ToolTip = ""
                ddlcandidates.SelectedIndex = 0
                BindCourse()
            ElseIf (sel_subjectid.SelectedIndex = 0) Then
                If (sel_subjectid.Items.Count > 0) Then
                    ddlcourse.SelectedValue = 0
                    ddlcandidates.SelectedIndex = 0
                End If
                gridDiv.Visible = False
                ddlcourse.Enabled = False
                ddlcandidates.Enabled = False
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

#Region "Bind Course DropdownList"
    'Desc: This method Binds Course DropdownList.
    'By: Jatin Gangajaliya, 2011/2/22

    Public Sub BindCourse()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Dim strbr As StringBuilder
        Try

            strbr = New StringBuilder()
            'strbr.Append(" Select M_Course.Course_id,M_Course.Course_name from M_Course ")
            'strbr.Append(" join T_Candidate_Status on T_Candidate_Status.Course_ID = M_Course.Course_id ")
            'strbr.Append(" join T_Center_Course on M_Course.Course_id = T_Center_Course.Course_id ")
            'strbr.Append(" where T_Candidate_Status.Appearedflag = 2  ")
            'strbr.Append(" AND T_Center_Course.Center_id = ")
            'strbr.Append(sel_subjectid.SelectedValue)
            'strbr.Append(" group by M_Course.Course_id,M_Course.Course_name ")
            'strbr.Append(" order by M_Course.Course_name ")

            'Change By: Saraswati Patel
            'Description:Change For Select only enable course name
            strbr.Append(" Select M_Course.Course_id,M_Course.Course_name from M_Course ")
            strbr.Append(" join T_Candidate_Status on T_Candidate_Status.Course_ID = M_Course.Course_id ")
            strbr.Append(" join T_Center_Course on M_Course.Course_id = T_Center_Course.Course_id ")
            strbr.Append(" where T_Candidate_Status.Appearedflag = 2  ")
            strbr.Append(" AND T_Center_Course.Center_id = ")
            strbr.Append(sel_subjectid.SelectedValue)
            strbr.Append("and M_Course.Del_Flag=0")
            strbr.Append(" group by M_Course.Course_id,M_Course.Course_name ")
            strbr.Append(" order by M_Course.Course_name ")
            sqlstr = strbr.ToString()

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
            strbr = Nothing
            myDataReader = Nothing
            myTable = Nothing
            myRow = Nothing
        End Try

    End Sub
#End Region

#Region "Course selectedindex change event"
    'Desc: This is course dropdown selected index change event.
    'By: Jatin Gangajaliya, 2011/03/21
    Protected Sub ddlcourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlcourse.SelectedIndexChanged
        Try
            If (sel_subjectid.SelectedIndex > 0) And (ddlcourse.SelectedIndex > 0) Then
                ddlcandidates.Enabled = True
                BtnSearch.Enabled = True
                BtnSearch.ToolTip = ""
                BindCandidates()
            ElseIf (ddlcourse.SelectedIndex = 0) Then
                If ddlcourse.Items.Count > 0 Then
                    ddlcandidates.SelectedValue = 0
                End If
                ddlcandidates.Enabled = False
                gridDiv.Visible = False
                'BtnSearch.Enabled = False
                'BtnSearch.ToolTip = "Select the Course First"
                'ElseIf (ddlcandidates.SelectedIndex = 0) Then
                '    BtnSearch.Enabled = False
                '    BtnSearch.ToolTip = "Select the Candidate First"
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then

                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
#End Region

#Region "BindCandidates"
    'Desc: This method binds candidates.
    'By: Jatin Gangajaliya, 2011/03/21

    Public Sub BindCandidates()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Dim strbr As StringBuilder
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
            strbr.Append("AND M_USER_INFO.Delete_Flg=0")
            strbr.Append(" Group by M_USER_INFO.userid,M_USER_INFO.name,M_USER_INFO.surname,M_USER_INFO.Middlename ")
            strbr.Append(" Order by FullName  ")
            sqlstr = strbr.ToString()

            myDataReader = retTestInfo(sqlstr)

            myTable = New DataTable
            myTable.Columns.Add(New DataColumn("userid", GetType(String)))
            myTable.Columns.Add(New DataColumn("FullName", GetType(String)))

            'While loop to populate the Datatable
            While myDataReader.Read
                myRow = myTable.NewRow
                myRow(0) = myDataReader.Item("userid")
                myRow(1) = myDataReader.Item("FullName")
                myTable.Rows.Add(myRow)
            End While
            ddlcandidates.DataSource = myTable
            ddlcandidates.DataTextField = "FullName"
            ddlcandidates.DataValueField = "userid"
            ddlcandidates.DataBind()
            ddlcandidates.Items.Insert(0, New ListItem("--Select--", "0"))
            myDataReader.Close()

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
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
    'By: Jatin Gangajaliya, 2011/03/21.
    'Added by Pranit Chimurkar on 2019/10/21
    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        Try
            If (sel_subjectid.SelectedValue = 0) Then
                'errorMsg.Text = "Please select Centre Name"
                errorMsg.Text = Resources.Resource.Search_PlSelClName
                gridDiv.Visible = False
            ElseIf ddlcourse.SelectedValue = 0 Then
                'errorMsg.Text = "Please select Course Name"
                errorMsg.Text = Resources.Resource.CourseRegistration_ValNm
                gridDiv.Visible = False
            ElseIf ddlcandidates.SelectedValue = 0 Then
                'errorMsg.Text = "Please select Candidate Name"
                errorMsg.Text = Resources.Resource.AppearedExam_PSCandidateName
                gridDiv.Visible = False
            Else
                DGData.CurrentPageIndex = 0
                BindGrid()
            End If
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
    'By: Jatin Gangajaliya, 2011/03/21.
    'Added by Pranit Chimurkar on 2019/10/21
    Protected Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        Try
            sel_subjectid.SelectedValue = 0
            If ddlcourse.Items.Count >= 0 Then
                ddlcourse.SelectedValue = 0
                ddlcourse.Enabled = False
            End If
            If ddlcandidates.Items.Count >= 0 Then
                ddlcandidates.SelectedValue = 0
                ddlcandidates.Enabled = False
            End If
            errorMsg.Text = String.Empty
            gridDiv.Visible = False
            sel_subjectid.Focus()

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
    'By: Jatin Gangajaliya, 2011/03/21
    'Added by Pranit Chimurkar on 2019/10/21
    'Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
    '    Response.Redirect("admin.aspx", False)
    'End Sub
#End Region

#Region "Bindgrid method"
    'Desc: This method binds data to datagrid.
    'By: Jatin Gangajaliya, 2011/03/21.

    Private Sub BindGrid()
        Dim strquery As String
        Dim objconn As New ConnectDb
        Dim myTable As DataTable
        Dim strbr As StringBuilder
        Dim adap As SqlDataAdapter
        Dim col As DataColumn
        Try
            myTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strbr = New StringBuilder()
            strbr.Append("  ")

            '*****Commented By Bharat Prajapati
            '*** Date: 2011/05/09
            ' Description: query changed
            'strbr.Append(" select M.userid,u.name+' '+u.surname as Name,gaveAns.givenanswer,q.question,CorrectAns.CorrectAns,co.course_name, ")
            'strbr.Append(" (case ")
            'strbr.Append(" when gaveAns.givenanswer=CorrectAns.CorrectAns ")
            'strbr.Append(" then ")
            'strbr.Append(" 'Correct'")
            'strbr.Append(" else ")
            'strbr.Append(" 'Incorrect'")
            'strbr.Append(" End ")
            'strbr.Append(" ) as Result ")
            'strbr.Append(" From t_result as M ")
            'strbr.Append(" left join m_user_info as u ")
            'strbr.Append(" on u.userid=m.userid ")
            'strbr.Append(" Left Join ")
            'strbr.Append(" ( ")
            'strbr.Append(" select t.test_type,t.userid,t.qno,t.optionid as givenid,opt.[option] as givenanswer from t_result as t ")
            'strbr.Append(" left join m_options as opt ")
            'strbr.Append(" on opt.Qnid=t.qno ")
            'strbr.Append(" and opt.test_type=t.test_type ")
            'strbr.Append("  and opt.Optionid=t.optionid ")
            'strbr.Append(" where t.userid= ")
            'strbr.Append(ddlcandidates.SelectedValue)
            'strbr.Append(" and t.course_id= ")
            'strbr.Append(ddlcourse.SelectedValue)
            'strbr.Append(" )gaveAns ")
            'strbr.Append(" on gaveAns.userid=m.userid ")
            'strbr.Append(" and gaveAns.test_type=m.test_type ")
            'strbr.Append(" and gaveAns.givenid=m.optionid ")
            'strbr.Append(" and gaveAns.qno=m.qno ")
            'strbr.Append(" left join m_question as q ")
            'strbr.Append(" on q.qnid=gaveAns.qno ")
            'strbr.Append(" and q.test_type=gaveAns.test_type ")
            'strbr.Append(" Left Join ")
            'strbr.Append(" ( ")
            'strbr.Append(" select t.test_type,t.userid,t.qno,q.correct_ansid,opt.[option]as CorrectAns ")
            'strbr.Append(" from t_result as t ")
            'strbr.Append(" left join m_question as q ")
            'strbr.Append(" on q.qnid=t.qno ")
            'strbr.Append(" and q.test_type=t.test_type ")
            'strbr.Append(" left join m_options as opt ")
            'strbr.Append("  on opt.Qnid=t.qno ")
            'strbr.Append(" and opt.test_type=t.test_type ")
            'strbr.Append(" and q.correct_ansid=opt.optionid ")
            'strbr.Append(" where t.userid= ")
            'strbr.Append(ddlcandidates.SelectedValue)
            'strbr.Append(" and t.course_id= ")
            'strbr.Append(ddlcourse.SelectedValue)
            'strbr.Append(" )CorrectAns ")
            'strbr.Append(" on CorrectAns.userid=m.userid ")
            'strbr.Append(" and CorrectAns.test_type=gaveAns.test_type ")
            'strbr.Append(" and CorrectAns.qno=gaveAns.qno ")
            'strbr.Append(" left join T_User_Course as tc ")
            'strbr.Append(" on tc.User_ID=u.userid ")
            'strbr.Append(" right join m_course as co ")
            'strbr.Append(" on co.course_id=tc.course_id ")
            'strbr.Append(" and co.course_id=m.course_id")
            'strbr.Append(" where m.userid= ")
            'strbr.Append(ddlcandidates.SelectedValue)
            'strbr.Append(" and m.course_id= ")
            'strbr.Append(ddlcourse.SelectedValue)
            'strbr.Append(" group by M.userid,gaveAns.givenanswer,q.question,CorrectAns.CorrectAns,co.course_name,u.name+' '+u.surname ")


            '************Start
            strbr.Append(" select  temp.qno,sum(temp.obtained_marks)as obtained_Marks ")
            strbr.Append(" ,temp.course_id,temp.userid, ")
            strbr.Append(" temp.question,temp.total_marks ,temp.test_type ")
            strbr.Append(" from ( ")
            strbr.Append(" select  ")
            strbr.Append(" tr.qno, ")
            strbr.Append(" ( ")
            strbr.Append(" case  ")
            strbr.Append(" WHEN mq.Qn_Category_ID=3 then ")
            strbr.Append(" ( ")
            strbr.Append("  case  ")
            strbr.Append(" WHEN mqa.sub_id=tro.sub_id then ")
            strbr.Append(" (Case  ")
            strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
            strbr.Append(" count(mqa.Correct_Opt_Id) ")
            strbr.Append(" ELSE 0 ")
            strbr.Append(" END) ")
            strbr.Append(" ELSE 0 ")
            strbr.Append(" END ")
            strbr.Append(" ) ")
            strbr.Append(" WHEN mq.Qn_Category_ID=2 then ")
            strbr.Append(" ( ")
            strbr.Append(" CASE   ")
            strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
            strbr.Append(" count(mqa.Correct_Opt_Id) ")
            strbr.Append(" ELSE 0 ")
            strbr.Append(" END ")
            strbr.Append("  ) ")
            strbr.Append("  WHEN mq.Qn_Category_ID=1 then ")
            strbr.Append(" ( ")
            strbr.Append("  CASE   ")
            strbr.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
            strbr.Append(" SUM(mq.Total_Marks) ")
            strbr.Append("  ELSE 0 ")
            strbr.Append("  END ")
            strbr.Append("  ) ")
            strbr.Append("  END ")
            strbr.Append(" ) as obtained_marks ")
            strbr.Append(" ,mc.course_id,mui.userid,mq.question,mq.Total_Marks,mq.test_type ")
            strbr.Append(" from m_question as mq ")
            strbr.Append(" left join M_Question_Answer as mqa ")
            strbr.Append(" on mqa.Qn_ID=mq.qnid ")
            strbr.Append(" and mqa.test_type=mq.test_type ")
            strbr.Append(" left join t_result as tr ")
            strbr.Append(" on tr.qno=mq.qnid ")
            strbr.Append(" and tr.qno=mqa.Qn_ID ")
            strbr.Append(" AND tr.test_type=mq.test_type ")
            strbr.Append(" left join m_user_info as mui ")
            strbr.Append(" on mui.userid=tr.userid ")
            strbr.Append(" left join m_course as mc ")
            strbr.Append("  on mc.course_id=tr.course_id ")
            strbr.Append(" left join m_testinfo as mti ")
            strbr.Append(" on mti.test_type=tr.test_type ")
            strbr.Append(" and mti.test_type=mq.test_type ")
            strbr.Append(" left join t_result_option as tro ")
            strbr.Append(" on tro.result_id=tr.result_id ")
            strbr.Append(" and tr.test_type=mti.test_type ")
            strbr.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
            strbr.Append(" group by mq.total_marks,mc.course_id,mq.test_type, ")
            strbr.Append(" mq.total_marks,mqa.Sub_id,tr.qno,mq.question,mq.Total_Marks, ")
            strbr.Append(" mqa.Correct_Opt_Id,tro.option_id,mq.Qn_Category_ID,tro.sub_id,mui.userid)temp ")
            strbr.Append(" where temp.course_id= ")
            strbr.Append(ddlcourse.SelectedValue)
            strbr.Append(" and temp.userid= ")
            strbr.Append(ddlcandidates.SelectedValue)
            strbr.Append(" group by temp.qno,temp.course_id,temp.userid,temp.question,temp.total_marks,temp.test_type ")
            strbr.Append(" order by temp.qno ")




            '************End




            strquery = strbr.ToString()

            If objconn.connect() Then
                adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                adap.Fill(myTable)
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
                    'fillPagesCombo()
                    'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)


                    'lblrecords.Text = "Total Records:" & myTable.Rows.Count
                    'errorMsg.Text = String.Empty
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    lblrecords.Text = CommonMessageLingual.lblMsgTttlRecrds.ToString & myTable.Rows.Count
                    '    errorMsg.Text = String.Empty
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    lblrecords.Text = CommonMessageLingual.lblMsgTttlRecrds.ToString & myTable.Rows.Count
                    '    errorMsg.Text = String.Empty
                    'End If
                    lblrecords.Text = ": " & myTable.Rows.Count



                Else
                    gridDiv.Visible = False
                    errorMsg.Text = Resources.Resource.Common_NoRecFound
                    'If Request.Cookies("Lang").Value = "en-us" Then
                    '    errorMsg.Text = CommonMessageLingual.lblMsgNoRcrd.ToString
                    'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                    '    errorMsg.Text = CommonMessageLingual.lblMsgNoRcrd.ToString
                    'End If

                End If
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            strquery = Nothing
            strbr = Nothing
            col = Nothing
            adap = Nothing
            strPathDb = Nothing
            objconn = Nothing
        End Try
    End Sub
#End Region

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
                Session.Add("cand", ddlcandidates.SelectedValue)

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
    'By: Jatin Gangajaliya, 2011/03/21.
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

    'Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
    '    DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    BindGrid()
    'End Sub



    Public Sub ExportExamDetails()
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
            errorMsg.Text = String.Empty
            errorMsg.Visible = False
            'Constants
            Const xlEdgeLeft = 7
            Const xlEdgeTop = 8
            Const xlEdgeBottom = 9
            Const xlEdgeRight = 10

            'Create  Spreadsheet

            Dim datestart As Date = Date.Now  'Added by Pragnesha Kulkarni on 2018/06/01 for stop Excel process
            App = New Excel.Application
            ' App = CreateObject("Excel.Application")

            ' App.Workbooks.Add(oTemplate)
            'WorkBooks = DirectCast(App.Workbooks, Excel.Workbooks)
            ' Dim myWorkBook As Excel.Workbook = App.Workbooks.Add(oTemplate) ', objOpt, objOpt, objOpt, objOpt, objOpt, objOpt, Excel.XlPlatform.xlWindows, objOpt, objOpt, objOpt, objOpt, objOpt)
            ' By Nisha on 2018/05/17
            ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
            Dim myWorkBook As Excel.Workbook = App.Workbooks.Open(Server.MapPath("Excel Import\Student Exam Details.XLT"), 0, False, 5, "", "", False, Excel.XlPlatform.xlWindows, "", True, False, 0, True)



            'WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
            sheet1 = myWorkBook.Worksheets("Exam Details")
            sheet1.Activate()

            'sheet1.Cells.Clear()
            'sheet1.Name = "Exam Details"


            ' ''Batch Name
            'With App.ActiveSheet.Range("B2:C2")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = "Batch Name :"
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ''Batch Name
            With App.ActiveSheet.Range("D2:D2")
                .MergeCells = True
                '.Interior.ColorIndex = 40
                .Font.Bold = True
                '.Font.ColorIndex = 53
                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                .Cells.Value = sel_subjectid.SelectedItem.Text
                '.Font.Size = 15
                '.BORDERS(xlEdgeLeft).Weight = 2
                '.BORDERS(xlEdgeTop).Weight = 2
                '.BORDERS(xlEdgeBottom).Weight = 2
                '.BORDERS(xlEdgeRight).Weight = 2
            End With

            ' ''Course Name
            'With App.ActiveSheet.Range("B3:C3")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = "Course Name :"
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ' ''Course Name
            With App.ActiveSheet.Range("D3:D3")
                .MergeCells = True
                '.Interior.ColorIndex = 40
                .Font.Bold = True
                '.Font.ColorIndex = 53
                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
                .Cells.Value = ddlcourse.SelectedItem.Text
                '.Font.Size = 15
                '.BORDERS(xlEdgeLeft).Weight = 2
                '.BORDERS(xlEdgeTop).Weight = 2
                '.BORDERS(xlEdgeBottom).Weight = 2
                '.BORDERS(xlEdgeRight).Weight = 2
            End With

            ''Exam Date
            'With App.ActiveSheet.Range("B6:C6")
            '    .MergeCells = True
            '    ' .Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = "Exam Date :"
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            'strpath = ConfigurationSettings.AppSettings("PathDb")

            If objCn.connect() Then

                'Get date & time and total marks from database
                sb = New StringBuilder
                sb.Append("select T_Candidate_Status.Written_test_Appear_Date,M_Course.Total_marks  ")
                sb.Append("from T_Candidate_Status inner join M_Course on M_Course.Course_id=T_Candidate_Status.Course_ID")
                sb.Append(" where T_Candidate_Status.Course_ID=")
                sb.Append(ddlcourse.SelectedValue)
                If (ddlcandidates.SelectedValue = 0) Then
                Else
                    sb.Append("and T_Candidate_Status.UserId=")
                    sb.Append(ddlcandidates.SelectedValue)
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
                sb.Append("WHERE Temp.Center_ID=" & sel_subjectid.SelectedValue & " and  TEMP.course_id = " & vbCrLf)
                sb.Append(ddlcourse.SelectedValue & vbCrLf)
                If (ddlcandidates.SelectedValue = 0) Then
                Else
                    sb.Append("       AND TEMP.userid =  " & vbCrLf)
                    sb.Append(ddlcandidates.SelectedValue)
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

                '********************************************************



                '********************************************************
                '**************************************************************
                ''get question and question details
                'sb.Append("SELECT TEMP.qno, " & vbCrLf)
                'sb.Append("       SUM(TEMP.obtained_marks)AS obtained_marks, " & vbCrLf)
                'sb.Append("       TEMP.course_id, " & vbCrLf)
                'sb.Append("       TEMP.userid, " & vbCrLf)
                'sb.Append("       TEMP.question, " & vbCrLf)
                'sb.Append("       TEMP.total_marks, " & vbCrLf)
                'sb.Append("       TEMP.test_type, " & vbCrLf)
                'sb.Append("       TEMP.test_name          AS subjectname, " & vbCrLf)
                'sb.Append("       TEMP.Names " & vbCrLf)
                'sb.Append("FROM   (SELECT tr.qno, " & vbCrLf)
                'sb.Append("               ( CASE " & vbCrLf)
                'sb.Append("                   WHEN mq.qn_category_id = 3 THEN ( " & vbCrLf)
                'sb.Append("                   CASE " & vbCrLf)
                'sb.Append("                     WHEN mqa.sub_id = tro.sub_id THEN ( CASE " & vbCrLf)
                'sb.Append("                     WHEN " & vbCrLf)
                'sb.Append("                     tro.option_id = mqa.correct_opt_id " & vbCrLf)
                'sb.Append("                                                         THEN " & vbCrLf)
                'sb.Append("                     COUNT(mqa.correct_opt_id) " & vbCrLf)
                'sb.Append("                     ELSE " & vbCrLf)
                'sb.Append("                     0 " & vbCrLf)
                'sb.Append("                                                         END " & vbCrLf)
                'sb.Append("                     ) " & vbCrLf)
                'sb.Append("                     ELSE 0 " & vbCrLf)
                'sb.Append("                   END ) " & vbCrLf)
                'sb.Append("                   WHEN mq.qn_category_id = 2 THEN ( CASE " & vbCrLf)
                'sb.Append("                                                       WHEN " & vbCrLf)
                'sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
                'sb.Append("                                                     THEN " & vbCrLf)
                'sb.Append("                   COUNT(mqa.correct_opt_id) " & vbCrLf)
                'sb.Append("                                                       ELSE 0 " & vbCrLf)
                'sb.Append("                                                     END ) " & vbCrLf)
                'sb.Append("                   WHEN mq.qn_category_id = 1 THEN ( CASE " & vbCrLf)
                'sb.Append("                                                       WHEN " & vbCrLf)
                'sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
                'sb.Append("                                                     THEN SUM(mq.total_marks) " & vbCrLf)
                'sb.Append("                                                       ELSE 0 " & vbCrLf)
                'sb.Append("                                                     END ) " & vbCrLf)
                'sb.Append("                 END ) AS obtained_marks, " & vbCrLf)
                'sb.Append("               mc.course_id, " & vbCrLf)
                'sb.Append("               mui.userid, " & vbCrLf)
                'sb.Append("                isnull(mui.surname,'')+' '+isnull(mui.Name,'')+' ' +isnull(mui.Middlename,'')  AS Names, " & vbCrLf)
                'sb.Append("               mq.question, " & vbCrLf)
                'sb.Append("               mq.total_marks, " & vbCrLf)
                'sb.Append("               mq.test_type, " & vbCrLf)
                'sb.Append("               mti.test_name,mui.Center_id " & vbCrLf)
                'sb.Append("        FROM   m_question AS mq " & vbCrLf)
                'sb.Append("               LEFT JOIN m_question_answer AS mqa " & vbCrLf)
                'sb.Append("                 ON mqa.qn_id = mq.qnid " & vbCrLf)
                'sb.Append("                    AND mqa.test_type = mq.test_type " & vbCrLf)
                'sb.Append("               LEFT JOIN t_result AS tr " & vbCrLf)
                'sb.Append("                 ON tr.qno = mq.qnid " & vbCrLf)
                'sb.Append("                    AND tr.qno = mqa.qn_id " & vbCrLf)
                'sb.Append("                    AND tr.test_type = mq.test_type " & vbCrLf)
                'sb.Append("               LEFT JOIN m_user_info AS mui " & vbCrLf)
                'sb.Append("                 ON mui.userid = tr.userid LEFT JOIN M_Centers AS msc ON msc.Center_id = mui.Center_ID" & vbCrLf)
                'sb.Append("               LEFT JOIN m_course AS mc " & vbCrLf)
                'sb.Append("                 ON mc.course_id = tr.course_id LEFT JOIN T_Center_Course AS tcc ON tcc.course_id = mc.course_id and tcc.Center_ID =msc.Center_id	" & vbCrLf)
                'sb.Append("               LEFT JOIN m_testinfo AS mti " & vbCrLf)
                'sb.Append("                 ON mti.test_type = tr.test_type " & vbCrLf)
                'sb.Append("                    AND mti.test_type = mq.test_type LEFT JOIN dbo.T_User_Course AS tuc ON tuc.course_id = mc.course_id and tuc.user_id=mui.userid and tuc.Test_type=mti.test_type" & vbCrLf)
                'sb.Append("               LEFT JOIN t_result_option AS tro " & vbCrLf)
                'sb.Append("                 ON tro.result_id = tr.result_id " & vbCrLf)
                'sb.Append("                    AND tr.test_type = mti.test_type " & vbCrLf)
                'sb.Append("                    AND tro.option_id = mqa.correct_opt_id " & vbCrLf)
                'sb.Append("        GROUP  BY mq.total_marks, " & vbCrLf)
                'sb.Append("                  mc.course_id, " & vbCrLf)
                'sb.Append("                  mq.test_type, " & vbCrLf)
                'sb.Append("                  mti.test_name, " & vbCrLf)
                'sb.Append("                  mq.total_marks, " & vbCrLf)
                'sb.Append("                  mqa.sub_id, " & vbCrLf)
                'sb.Append("                  tr.qno, " & vbCrLf)
                'sb.Append("                  mq.question, " & vbCrLf)
                'sb.Append("                  mq.total_marks, " & vbCrLf)
                'sb.Append("                  mqa.correct_opt_id, " & vbCrLf)
                'sb.Append("                  tro.option_id, " & vbCrLf)
                'sb.Append("                  mq.qn_category_id, " & vbCrLf)
                'sb.Append("                  tro.sub_id, " & vbCrLf)
                'sb.Append("                  mui.userid, " & vbCrLf)
                'sb.Append("                  isnull(mui.surname,'')+' '+isnull(mui.Name,'')+' ' +isnull(mui.Middlename,''),mui.Center_id)TEMP " & vbCrLf)
                'sb.Append("--  left join M_Options as mop " & vbCrLf)
                'sb.Append("--                        on mop.qnid=temp.qno and mop.test_type=temp.test_type                      " & vbCrLf)
                'sb.Append("WHERE Temp.Center_ID=" & sel_subjectid.SelectedValue & " and  TEMP.course_id = " & vbCrLf)
                'sb.Append(ddlcourse.SelectedValue & vbCrLf)
                'If (ddlcandidates.SelectedValue = 0) Then
                'Else
                '    sb.Append("       AND TEMP.userid =  " & vbCrLf)
                '    sb.Append(ddlcandidates.SelectedValue)
                'End If
                'sb.Append("GROUP  BY TEMP.qno, " & vbCrLf)
                'sb.Append("          TEMP.course_id, " & vbCrLf)
                'sb.Append("          TEMP.userid, " & vbCrLf)
                'sb.Append("          TEMP.Names, " & vbCrLf)
                'sb.Append("          TEMP.question, " & vbCrLf)
                'sb.Append("          TEMP.total_marks, " & vbCrLf)
                'sb.Append("          TEMP.test_type, " & vbCrLf)
                'sb.Append("          TEMP.test_name " & vbCrLf)
                'sb.Append("-- order by temp.qno  " & vbCrLf)
                ''sb.Append("ORDER  BY TEMP.test_name ")
                'sb.Append("ORDER  BY TEMP.Names,TEMP.test_name  ")

                '******************************************************************
                '******************************************************************
                'sb.Append(" select  temp.qno,sum(temp.obtained_marks)as obtained_Marks ")
                'sb.Append(" ,temp.course_id,temp.userid, ")
                'sb.Append(" temp.question,temp.total_marks ,temp.test_type ")
                'sb.Append(" from ( ")
                'sb.Append(" select  ")
                'sb.Append(" tr.qno, ")
                'sb.Append(" ( ")
                'sb.Append(" case  ")
                'sb.Append(" WHEN mq.Qn_Category_ID=3 then ")
                'sb.Append(" ( ")
                'sb.Append("  case  ")
                'sb.Append(" WHEN mqa.sub_id=tro.sub_id then ")
                'sb.Append(" (Case  ")
                'sb.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                'sb.Append(" count(mqa.Correct_Opt_Id) ")
                'sb.Append(" ELSE 0 ")
                'sb.Append(" END) ")
                'sb.Append(" ELSE 0 ")
                'sb.Append(" END ")
                'sb.Append(" ) ")
                'sb.Append(" WHEN mq.Qn_Category_ID=2 then ")
                'sb.Append(" ( ")
                'sb.Append(" CASE   ")
                'sb.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                'sb.Append(" count(mqa.Correct_Opt_Id) ")
                'sb.Append(" ELSE 0 ")
                'sb.Append(" END ")
                'sb.Append("  ) ")
                'sb.Append("  WHEN mq.Qn_Category_ID=1 then ")
                'sb.Append(" ( ")
                'sb.Append("  CASE   ")
                'sb.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
                'sb.Append(" SUM(mq.Total_Marks) ")
                'sb.Append("  ELSE 0 ")
                'sb.Append("  END ")
                'sb.Append("  ) ")
                'sb.Append("  END ")
                'sb.Append(" ) as obtained_marks ")
                'sb.Append(" ,mc.course_id,mui.userid,mq.question,mq.Total_Marks,mq.test_type ")
                'sb.Append(" from m_question as mq ")
                'sb.Append(" left join M_Question_Answer as mqa ")
                'sb.Append(" on mqa.Qn_ID=mq.qnid ")
                'sb.Append(" and mqa.test_type=mq.test_type ")
                'sb.Append(" left join t_result as tr ")
                'sb.Append(" on tr.qno=mq.qnid ")
                'sb.Append(" and tr.qno=mqa.Qn_ID ")
                'sb.Append(" AND tr.test_type=mq.test_type ")
                'sb.Append(" left join m_user_info as mui ")
                'sb.Append(" on mui.userid=tr.userid ")
                'sb.Append(" left join m_course as mc ")
                'sb.Append("  on mc.course_id=tr.course_id ")
                'sb.Append(" left join m_testinfo as mti ")
                'sb.Append(" on mti.test_type=tr.test_type ")
                'sb.Append(" and mti.test_type=mq.test_type ")
                'sb.Append(" left join t_result_option as tro ")
                'sb.Append(" on tro.result_id=tr.result_id ")
                'sb.Append(" and tr.test_type=mti.test_type ")
                'sb.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
                'sb.Append(" group by mq.total_marks,mc.course_id,mq.test_type, ")
                'sb.Append(" mq.total_marks,mqa.Sub_id,tr.qno,mq.question,mq.Total_Marks, ")
                'sb.Append(" mqa.Correct_Opt_Id,tro.option_id,mq.Qn_Category_ID,tro.sub_id,mui.userid)temp ")
                'sb.Append(" where temp.course_id= ")
                'sb.Append(ddlcourse.SelectedValue)
                'sb.Append(" and temp.userid= ")
                'sb.Append(ddlcandidates.SelectedValue)
                'sb.Append(" group by temp.qno,temp.course_id,temp.userid,temp.question,temp.total_marks,temp.test_type ")
                'sb.Append(" order by temp.qno ")
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
                    sb.Append(" and tr.qno=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " and tr.userid= " & dsQuestion.Tables(0).Rows(i).Item(3).ToString & " and  tr.Course_id=" & ddlcourse.SelectedValue & "")
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
                        '.Interior.ColorIndex = 40
                        ' .Font.Bold = True
                        ' .Font.ColorIndex = 53
                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                        'If strGivenAns = "Not Attempted" Then
                        '    .Font.ColorIndex = 3
                        'End If
                        '.Cells.Value = strGivenAns


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

                                '.Cells.Value = crr
                            Else
                                '.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                                '.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                                ' .Cells.Value = ds1.Tables(0).Rows(0).Item(0).ToString

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
                        '.Interior.ColorIndex = 40
                        ' .Font.Bold = True
                        ' .Font.ColorIndex = 53
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
                ' End If
                'If Not IsDBNull(dsQuestion) Then
                '    da = Nothing
                '    sb = Nothing
                '    sb = New StringBuilder
                '    dsTotalMarks = New DataSet
                '    sb.Append("Select Total_marks from M_Course where Course_id=")
                '    sb.Append(dsQuestion.Tables(0).Rows(0).Item(2).ToString)
                '    Dim aa As String = dsQuestion.Tables(0).Rows(0).Item(2).ToString
                '    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
                '    da.Fill(dsTotalMarks)
                'End If

            End If
            If Not objCn.MyConnection Is Nothing Then
                If objCn.MyConnection.State = ConnectionState.Open Then
                    objCn.disconnect()
                End If
            End If

            ''date and time of exam given
            'With App.ActiveSheet.Range("D6:D6")
            '    .MergeCells = True
            '    ' .Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    '.Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    If strDateTime(0) <> "" Then
            '        .Cells.Value = strDateTime(1) + "/" + strDateTime(0) + "/" + strDateTime(2)
            '    End If

            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ''total marks
            'With App.ActiveSheet.Range("B7:C7")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = "Total Marks :"
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ' ''total marks
            'With App.ActiveSheet.Range("D7:D7")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    '.Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    If dsDateTime.Tables(0).Rows.Count <> 0 Then
            '        .Cells.Value = dsDateTime.Tables(0).Rows(0).Item(1).ToString
            '    End If

            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ''total mark obtain in exam
            'With App.ActiveSheet.Range("B8:C8")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = "Mark Obtained :"
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ''total mark obtain in exam
            'With App.ActiveSheet.Range("D8:D8")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    ' .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = MarkObtain
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            'With App.ActiveSheet.Range("B9:C9")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    .Cells.Value = "Percentage :"
            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With

            ''total mark obtain in exam
            'With App.ActiveSheet.Range("D9:D9")
            '    .MergeCells = True
            '    '.Interior.ColorIndex = 40
            '    .Font.Bold = True
            '    ' .Font.ColorIndex = 53
            '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
            '    If dsDateTime.Tables(0).Rows.Count <> 0 Then
            '        .Cells.Value = Math.Round((MarkObtain * 100 / dsDateTime.Tables(0).Rows(0).Item(1)), 2).ToString + "%"
            '    End If


            '    '.Font.Size = 15
            '    '.BORDERS(xlEdgeLeft).Weight = 2
            '    '.BORDERS(xlEdgeTop).Weight = 2
            '    '.BORDERS(xlEdgeBottom).Weight = 2
            '    '.BORDERS(xlEdgeRight).Weight = 2
            'End With
            'App.ActiveSheet.Cells(11, 1).ColumnWidth = 1

            'App.ActiveSheet.Cells(11, 2).Value = "Sr.No."
            'App.ActiveSheet.Cells(11, 2).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 2).Font.Bold = True
            'App.ActiveSheet.Cells(11, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            '' App.ActiveSheet.Cells(11, 2).ColumnWidth = 30
            'App.ActiveSheet.Cells(11, 2).WrapText = True
            'App.ActiveSheet.Cells(11, 2).Borders.Weight = 2
            '' App.ActiveSheet.Cells(11, 2).RowHeight = 15

            'App.ActiveSheet.Cells(11, 3).Value = "Subject Name"
            'App.ActiveSheet.Cells(11, 3).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 3).Font.Bold = True
            'App.ActiveSheet.Cells(11, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            'App.ActiveSheet.Cells(11, 3).ColumnWidth = 15
            'App.ActiveSheet.Cells(11, 3).WrapText = True
            'App.ActiveSheet.Cells(11, 3).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 4).Value = "Student Name"
            'App.ActiveSheet.Cells(11, 4).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 4).Font.Bold = True
            'App.ActiveSheet.Cells(11, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            'App.ActiveSheet.Cells(11, 4).ColumnWidth = 15
            'App.ActiveSheet.Cells(11, 4).WrapText = True
            'App.ActiveSheet.Cells(11, 4).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 5).Value = "Question"
            'App.ActiveSheet.Cells(11, 5).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 5).Font.Bold = True
            'App.ActiveSheet.Cells(11, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            'App.ActiveSheet.Cells(11, 5).ColumnWidth = 40
            'App.ActiveSheet.Cells(11, 5).WrapText = True
            'App.ActiveSheet.Cells(11, 5).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 6).Value = "Option"
            'App.ActiveSheet.Cells(11, 6).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 6).Font.Bold = True
            'App.ActiveSheet.Cells(11, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            'App.ActiveSheet.Cells(11, 6).ColumnWidth = 20
            'App.ActiveSheet.Cells(11, 6).WrapText = True
            'App.ActiveSheet.Cells(11, 6).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 7).Value = "Correct Answer"
            'App.ActiveSheet.Cells(11, 7).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 7).Font.Bold = True
            'App.ActiveSheet.Cells(11, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            'App.ActiveSheet.Cells(11, 7).ColumnWidth = 20
            'App.ActiveSheet.Cells(11, 7).WrapText = True
            'App.ActiveSheet.Cells(11, 7).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 8).Value = "Given Answer"
            'App.ActiveSheet.Cells(11, 8).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 8).Font.Bold = True
            'App.ActiveSheet.Cells(11, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            'App.ActiveSheet.Cells(11, 8).ColumnWidth = 20
            'App.ActiveSheet.Cells(11, 8).WrapText = True
            'App.ActiveSheet.Cells(11, 8).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 9).Value = "Total Marks"
            'App.ActiveSheet.Cells(11, 9).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 9).Font.Bold = True
            'App.ActiveSheet.Cells(11, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            '' App.ActiveSheet.Cells(11,8).ColumnWidth = 30
            'App.ActiveSheet.Cells(11, 9).WrapText = True
            'App.ActiveSheet.Cells(11, 9).Borders.Weight = 2

            'App.ActiveSheet.Cells(11, 10).Value = "Marks Obtained"
            'App.ActiveSheet.Cells(11, 10).Interior.ColorIndex = 36
            'App.ActiveSheet.Cells(11, 10).Font.Bold = True
            'App.ActiveSheet.Cells(11, 10).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
            'App.ActiveSheet.Cells(11, 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
            '' App.ActiveSheet.Cells(11,9).ColumnWidth = 30
            'App.ActiveSheet.Cells(11, 10).WrapText = True
            'App.ActiveSheet.Cells(11, 10).Borders.Weight = 2

            'Save WorkBook

            ' App.ActiveWorkbook.Sheets("Exam Details").Range("B12", "B12").EntireRow.FreezePanes = True
            ' App.ActiveWorkbook.Sheets("Exam Details").Range("A5", "A10").EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp)
            '(Excel.XlDeleteShiftDirection.xlShiftUp)
            'App.ActiveSheet.Cells(11, 10).EntireRow()
            'Dim rng As Excel.Range = App.ActiveWorkbook.Sheets("Exam Details").Range("B1", "B1").EntireRow
            'Dim rng1 As Excel.Range = App.ActiveWorkbook.Sheets("Exam Details").Range("F1", "F1").EntireColumn
            'App.ActiveWindow.FreezePanes = True
            'Dim ViewName() As String = {"Sheet2", "Sheet3"}

            'For Each ws As Excel.Worksheet In WorkBook.Sheets
            '    For de As Integer = 0 To ViewName.Length - 1
            '        If ws.Name.ToString().Trim().ToUpper() = ViewName(de).ToUpper() Then
            '            ws.Delete()
            '            Exit For
            '        End If
            '    Next
            'Next

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
            'If System.IO.File.Exists(Server.MapPath("Excel Import\Student Exam Details.XLT")) Then
            '    System.IO.File.Delete(Server.MapPath("Excel Import\Student Exam Details.XLT"))
            'End If

            fileName1 = Server.MapPath("Excel Import\Student Exam Details1.XLT")

            myWorkBook.SaveAs(fileName1) ', objOpt, objOpt, objOpt, objOpt, objOpt,Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
            myWorkBook.Close()
            'Added by Pragnesha Kulkarni on 2018/06/01
            ' Reason:Excel process didn't stop after downloading excel sheet
            ' BugID: 719
            App.Quit() ' releaseobject
            Dim dateEnd As Date = Date.Now 'By Pragnesha on 2018/06/01 to releaseobject
            End_Excel_App(datestart, dateEnd) ' This closes excel proces
            'Ended by Pragnesha Kulkarni on 2018/06/01

            ' By Nisha on 2018/05/17
            ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
            Dim file As New IO.FileInfo(Server.MapPath("Excel Import\Student Exam Details.XLT"))

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
            System.IO.File.Delete(Server.MapPath("Excel Import\Student Exam Details.xls"))
        Catch ex As Exception
            If log.IsDebugEnabled Then
                ' Dim err As String = ex.ToString()
                log.Debug("Error :" & ex.ToString())
                log.Debug("Error :" & ex.StackTrace)
            End If
            Response.Redirect("error.aspx")
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
            'Ended by Pragnesha Kulkarni on 2018/06/04
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
            'Ended by Pragnesha Kulkarni on 2018/06/04

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

    'Added by Pragnesha Kulkarni on 2018/06/04
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
        Dim xlp() As Process = Process.GetProcessesByName("EXCEL")
        For Each Process As Process In xlp
            If Process.StartTime >= datestart And Process.StartTime <= dateEnd Then
                Process.Kill()
                Exit For
            End If
        Next
    End Sub
    'Ended by Pragnesha Kulkarni on 2018/06/04

    'Public Sub ExportExamDetails()
    '    Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
    '    Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
    '    Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
    '    Dim Sheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
    '    Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
    '    Dim objOpt As Object = System.Reflection.Missing.Value
    '    Dim objCn As New ConnectDb
    '    Dim sheet1 As Excel.Worksheet
    '    Dim fileName1 As Object
    '    Dim strpath, strCorrectOpt, strGivenAns As String
    '    Dim strDateTime(2) As String
    '    Dim dsDateTime, dsQuestion, dsOpt, dsCrrctOpt, dsGivenOpt, dsTotalMarks As DataSet
    '    Dim da As SqlDataAdapter
    '    Dim sb As StringBuilder
    '    Dim rows As Integer = 12
    '    Dim start As Integer = 12
    '    Dim ends As Integer = 12
    '    Dim TotalMarks, MarkObtain As Integer
    '    Dim ImgNotFound(1) As String
    '    Dim pic As Object
    '    Dim arrCrrAns(), arrGivenAns() As String

    '    Try
    '        errorMsg.Text = String.Empty
    '        errorMsg.Visible = False
    '        'Constants
    '        Const xlEdgeLeft = 7
    '        Const xlEdgeTop = 8
    '        Const xlEdgeBottom = 9
    '        Const xlEdgeRight = 10

    '        'Create  Spreadsheet
    '        App = New Excel.Application
    '        WorkBooks = DirectCast(App.Workbooks, Excel.Workbooks)
    '        WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
    '        sheet1 = WorkBook.Worksheets(1)
    '        sheet1.Activate()
    '        sheet1.Cells.Clear()
    '        sheet1.Name = "Exam Details"
    '        'Student Name
    '        'With App.ActiveSheet.Range("B3:C3")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = "Student Name :"

    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ''Student Name
    '        'With App.ActiveSheet.Range("D3:D3")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    '.Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = ddlcandidates.SelectedItem.Text
    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ''Batch Name
    '        With App.ActiveSheet.Range("B2:C2")
    '            .MergeCells = True
    '            '.Interior.ColorIndex = 40
    '            .Font.Bold = True
    '            .Font.ColorIndex = 53
    '            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '            .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '            .Cells.Value = "Batch Name :"
    '            '.Font.Size = 15
    '            '.BORDERS(xlEdgeLeft).Weight = 2
    '            '.BORDERS(xlEdgeTop).Weight = 2
    '            '.BORDERS(xlEdgeBottom).Weight = 2
    '            '.BORDERS(xlEdgeRight).Weight = 2
    '        End With

    '        ''Batch Name
    '        With App.ActiveSheet.Range("D2:D2")
    '            .MergeCells = True
    '            '.Interior.ColorIndex = 40
    '            .Font.Bold = True
    '            '.Font.ColorIndex = 53
    '            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '            .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '            .Cells.Value = sel_subjectid.SelectedItem.Text
    '            '.Font.Size = 15
    '            '.BORDERS(xlEdgeLeft).Weight = 2
    '            '.BORDERS(xlEdgeTop).Weight = 2
    '            '.BORDERS(xlEdgeBottom).Weight = 2
    '            '.BORDERS(xlEdgeRight).Weight = 2
    '        End With

    '        ''Course Name
    '        With App.ActiveSheet.Range("B3:C3")
    '            .MergeCells = True
    '            '.Interior.ColorIndex = 40
    '            .Font.Bold = True
    '            .Font.ColorIndex = 53
    '            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '            .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '            .Cells.Value = "Course Name :"
    '            '.Font.Size = 15
    '            '.BORDERS(xlEdgeLeft).Weight = 2
    '            '.BORDERS(xlEdgeTop).Weight = 2
    '            '.BORDERS(xlEdgeBottom).Weight = 2
    '            '.BORDERS(xlEdgeRight).Weight = 2
    '        End With

    '        ' ''Course Name
    '        With App.ActiveSheet.Range("D3:D3")
    '            .MergeCells = True
    '            '.Interior.ColorIndex = 40
    '            .Font.Bold = True
    '            '.Font.ColorIndex = 53
    '            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '            .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '            .Cells.Value = ddlcourse.SelectedItem.Text
    '            '.Font.Size = 15
    '            '.BORDERS(xlEdgeLeft).Weight = 2
    '            '.BORDERS(xlEdgeTop).Weight = 2
    '            '.BORDERS(xlEdgeBottom).Weight = 2
    '            '.BORDERS(xlEdgeRight).Weight = 2
    '        End With

    '        ''Exam Date
    '        'With App.ActiveSheet.Range("B6:C6")
    '        '    .MergeCells = True
    '        '    ' .Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = "Exam Date :"
    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        strpath = ConfigurationSettings.AppSettings("PathDb")

    '        If objCn.connect(strpath) Then

    '            'Get date & time and total marks from database
    '            sb = New StringBuilder
    '            sb.Append("select T_Candidate_Status.Written_test_Appear_Date,M_Course.Total_marks  ")
    '            sb.Append("from T_Candidate_Status inner join M_Course on M_Course.Course_id=T_Candidate_Status.Course_ID")
    '            sb.Append(" where T_Candidate_Status.Course_ID=")
    '            sb.Append(ddlcourse.SelectedValue)
    '            If (ddlcandidates.SelectedValue = 0) Then
    '            Else
    '                sb.Append("and T_Candidate_Status.UserId=")
    '                sb.Append(ddlcandidates.SelectedValue)
    '            End If

    '            dsDateTime = New DataSet()

    '            da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '            da.Fill(dsDateTime)
    '            If dsDateTime.Tables(0).Rows.Count <> 0 Then
    '                Dim dateTime As String = dsDateTime.Tables(0).Rows(0).Item(0).ToString
    '                strDateTime = dateTime.Split("/")
    '            End If


    '            sb = Nothing
    '            da = Nothing
    '            sb = New StringBuilder
    '            'get question and question details
    '            sb.Append("SELECT TEMP.qno, " & vbCrLf)
    '            sb.Append("       SUM(TEMP.obtained_marks)AS obtained_marks, " & vbCrLf)
    '            sb.Append("       TEMP.course_id, " & vbCrLf)
    '            sb.Append("       TEMP.userid, " & vbCrLf)
    '            sb.Append("       TEMP.question, " & vbCrLf)
    '            sb.Append("       TEMP.total_marks, " & vbCrLf)
    '            sb.Append("       TEMP.test_type, " & vbCrLf)
    '            sb.Append("       TEMP.test_name          AS subjectname, " & vbCrLf)
    '            sb.Append("       TEMP.Names " & vbCrLf)
    '            sb.Append("FROM   (SELECT tr.qno, " & vbCrLf)
    '            sb.Append("               ( CASE " & vbCrLf)
    '            sb.Append("                   WHEN mq.qn_category_id = 3 THEN ( " & vbCrLf)
    '            sb.Append("                   CASE " & vbCrLf)
    '            sb.Append("                     WHEN mqa.sub_id = tro.sub_id THEN ( CASE " & vbCrLf)
    '            sb.Append("                     WHEN " & vbCrLf)
    '            sb.Append("                     tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '            sb.Append("                                                         THEN " & vbCrLf)
    '            sb.Append("                     COUNT(mqa.correct_opt_id) " & vbCrLf)
    '            sb.Append("                     ELSE " & vbCrLf)
    '            sb.Append("                     0 " & vbCrLf)
    '            sb.Append("                                                         END " & vbCrLf)
    '            sb.Append("                     ) " & vbCrLf)
    '            sb.Append("                     ELSE 0 " & vbCrLf)
    '            sb.Append("                   END ) " & vbCrLf)
    '            sb.Append("                   WHEN mq.qn_category_id = 2 THEN ( CASE " & vbCrLf)
    '            sb.Append("                                                       WHEN " & vbCrLf)
    '            sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '            sb.Append("                                                     THEN " & vbCrLf)
    '            sb.Append("                   COUNT(mqa.correct_opt_id) " & vbCrLf)
    '            sb.Append("                                                       ELSE 0 " & vbCrLf)
    '            sb.Append("                                                     END ) " & vbCrLf)
    '            sb.Append("                   WHEN mq.qn_category_id = 1 THEN ( CASE " & vbCrLf)
    '            sb.Append("                                                       WHEN " & vbCrLf)
    '            sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '            sb.Append("                                                     THEN SUM(mq.total_marks) " & vbCrLf)
    '            sb.Append("                                                       ELSE 0 " & vbCrLf)
    '            sb.Append("                                                     END ) " & vbCrLf)
    '            sb.Append("                 END ) AS obtained_marks, " & vbCrLf)
    '            sb.Append("               mc.course_id, " & vbCrLf)
    '            sb.Append("               mui.userid, " & vbCrLf)
    '            sb.Append("                isnull(mui.surname,'')+' '+isnull(mui.Name,'')+' ' +isnull(mui.Middlename,'')  AS Names, " & vbCrLf)
    '            sb.Append("               mq.question, " & vbCrLf)
    '            sb.Append("               mq.total_marks, " & vbCrLf)
    '            sb.Append("               mq.test_type, " & vbCrLf)
    '            sb.Append("               mti.test_name,mui.Center_id " & vbCrLf)
    '            sb.Append("        FROM   m_question AS mq " & vbCrLf)
    '            sb.Append("               LEFT JOIN m_question_answer AS mqa " & vbCrLf)
    '            sb.Append("                 ON mqa.qn_id = mq.qnid " & vbCrLf)
    '            sb.Append("                    AND mqa.test_type = mq.test_type " & vbCrLf)
    '            sb.Append("               LEFT JOIN t_result AS tr " & vbCrLf)
    '            sb.Append("                 ON tr.qno = mq.qnid " & vbCrLf)
    '            sb.Append("                    AND tr.qno = mqa.qn_id " & vbCrLf)
    '            sb.Append("                    AND tr.test_type = mq.test_type " & vbCrLf)
    '            sb.Append("               LEFT JOIN m_user_info AS mui " & vbCrLf)
    '            sb.Append("                 ON mui.userid = tr.userid LEFT JOIN M_Centers AS msc ON msc.Center_id = mui.Center_ID" & vbCrLf)
    '            sb.Append("               LEFT JOIN m_course AS mc " & vbCrLf)
    '            sb.Append("                 ON mc.course_id = tr.course_id LEFT JOIN T_Center_Course AS tcc ON tcc.course_id = mc.course_id and tcc.Center_ID =msc.Center_id	" & vbCrLf)
    '            sb.Append("               LEFT JOIN m_testinfo AS mti " & vbCrLf)
    '            sb.Append("                 ON mti.test_type = tr.test_type " & vbCrLf)
    '            sb.Append("                    AND mti.test_type = mq.test_type LEFT JOIN dbo.T_User_Course AS tuc ON tuc.course_id = mc.course_id and tuc.user_id=mui.userid and tuc.Test_type=mti.test_type" & vbCrLf)
    '            sb.Append("               LEFT JOIN t_result_option AS tro " & vbCrLf)
    '            sb.Append("                 ON tro.result_id = tr.result_id " & vbCrLf)
    '            sb.Append("                    AND tr.test_type = mti.test_type " & vbCrLf)
    '            sb.Append("                    AND tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '            sb.Append("        GROUP  BY mq.total_marks, " & vbCrLf)
    '            sb.Append("                  mc.course_id, " & vbCrLf)
    '            sb.Append("                  mq.test_type, " & vbCrLf)
    '            sb.Append("                  mti.test_name, " & vbCrLf)
    '            sb.Append("                  mq.total_marks, " & vbCrLf)
    '            sb.Append("                  mqa.sub_id, " & vbCrLf)
    '            sb.Append("                  tr.qno, " & vbCrLf)
    '            sb.Append("                  mq.question, " & vbCrLf)
    '            sb.Append("                  mq.total_marks, " & vbCrLf)
    '            sb.Append("                  mqa.correct_opt_id, " & vbCrLf)
    '            sb.Append("                  tro.option_id, " & vbCrLf)
    '            sb.Append("                  mq.qn_category_id, " & vbCrLf)
    '            sb.Append("                  tro.sub_id, " & vbCrLf)
    '            sb.Append("                  mui.userid, " & vbCrLf)
    '            sb.Append("                  isnull(mui.surname,'')+' '+isnull(mui.Name,'')+' ' +isnull(mui.Middlename,''),mui.Center_id)TEMP " & vbCrLf)
    '            sb.Append("--  left join M_Options as mop " & vbCrLf)
    '            sb.Append("--                        on mop.qnid=temp.qno and mop.test_type=temp.test_type                      " & vbCrLf)
    '            sb.Append("WHERE Temp.Center_ID=" & sel_subjectid.SelectedValue & " and  TEMP.course_id = " & vbCrLf)
    '            sb.Append(ddlcourse.SelectedValue & vbCrLf)
    '            If (ddlcandidates.SelectedValue = 0) Then
    '            Else
    '                sb.Append("       AND TEMP.userid =  " & vbCrLf)
    '                sb.Append(ddlcandidates.SelectedValue)
    '            End If
    '            sb.Append("GROUP  BY TEMP.qno, " & vbCrLf)
    '            sb.Append("          TEMP.course_id, " & vbCrLf)
    '            sb.Append("          TEMP.userid, " & vbCrLf)
    '            sb.Append("          TEMP.Names, " & vbCrLf)
    '            sb.Append("          TEMP.question, " & vbCrLf)
    '            sb.Append("          TEMP.total_marks, " & vbCrLf)
    '            sb.Append("          TEMP.test_type, " & vbCrLf)
    '            sb.Append("          TEMP.test_name " & vbCrLf)
    '            sb.Append("-- order by temp.qno  " & vbCrLf)
    '            'sb.Append("ORDER  BY TEMP.test_name ")
    '            sb.Append("ORDER  BY TEMP.Names,TEMP.test_name  ")
    '            'sb.Append(" select  temp.qno,sum(temp.obtained_marks)as obtained_Marks ")
    '            'sb.Append(" ,temp.course_id,temp.userid, ")
    '            'sb.Append(" temp.question,temp.total_marks ,temp.test_type ")
    '            'sb.Append(" from ( ")
    '            'sb.Append(" select  ")
    '            'sb.Append(" tr.qno, ")
    '            'sb.Append(" ( ")
    '            'sb.Append(" case  ")
    '            'sb.Append(" WHEN mq.Qn_Category_ID=3 then ")
    '            'sb.Append(" ( ")
    '            'sb.Append("  case  ")
    '            'sb.Append(" WHEN mqa.sub_id=tro.sub_id then ")
    '            'sb.Append(" (Case  ")
    '            'sb.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
    '            'sb.Append(" count(mqa.Correct_Opt_Id) ")
    '            'sb.Append(" ELSE 0 ")
    '            'sb.Append(" END) ")
    '            'sb.Append(" ELSE 0 ")
    '            'sb.Append(" END ")
    '            'sb.Append(" ) ")
    '            'sb.Append(" WHEN mq.Qn_Category_ID=2 then ")
    '            'sb.Append(" ( ")
    '            'sb.Append(" CASE   ")
    '            'sb.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
    '            'sb.Append(" count(mqa.Correct_Opt_Id) ")
    '            'sb.Append(" ELSE 0 ")
    '            'sb.Append(" END ")
    '            'sb.Append("  ) ")
    '            'sb.Append("  WHEN mq.Qn_Category_ID=1 then ")
    '            'sb.Append(" ( ")
    '            'sb.Append("  CASE   ")
    '            'sb.Append(" WHEN tro.option_id=mqa.Correct_Opt_Id then ")
    '            'sb.Append(" SUM(mq.Total_Marks) ")
    '            'sb.Append("  ELSE 0 ")
    '            'sb.Append("  END ")
    '            'sb.Append("  ) ")
    '            'sb.Append("  END ")
    '            'sb.Append(" ) as obtained_marks ")
    '            'sb.Append(" ,mc.course_id,mui.userid,mq.question,mq.Total_Marks,mq.test_type ")
    '            'sb.Append(" from m_question as mq ")
    '            'sb.Append(" left join M_Question_Answer as mqa ")
    '            'sb.Append(" on mqa.Qn_ID=mq.qnid ")
    '            'sb.Append(" and mqa.test_type=mq.test_type ")
    '            'sb.Append(" left join t_result as tr ")
    '            'sb.Append(" on tr.qno=mq.qnid ")
    '            'sb.Append(" and tr.qno=mqa.Qn_ID ")
    '            'sb.Append(" AND tr.test_type=mq.test_type ")
    '            'sb.Append(" left join m_user_info as mui ")
    '            'sb.Append(" on mui.userid=tr.userid ")
    '            'sb.Append(" left join m_course as mc ")
    '            'sb.Append("  on mc.course_id=tr.course_id ")
    '            'sb.Append(" left join m_testinfo as mti ")
    '            'sb.Append(" on mti.test_type=tr.test_type ")
    '            'sb.Append(" and mti.test_type=mq.test_type ")
    '            'sb.Append(" left join t_result_option as tro ")
    '            'sb.Append(" on tro.result_id=tr.result_id ")
    '            'sb.Append(" and tr.test_type=mti.test_type ")
    '            'sb.Append(" and tro.option_id=mqa.Correct_Opt_Id ")
    '            'sb.Append(" group by mq.total_marks,mc.course_id,mq.test_type, ")
    '            'sb.Append(" mq.total_marks,mqa.Sub_id,tr.qno,mq.question,mq.Total_Marks, ")
    '            'sb.Append(" mqa.Correct_Opt_Id,tro.option_id,mq.Qn_Category_ID,tro.sub_id,mui.userid)temp ")
    '            'sb.Append(" where temp.course_id= ")
    '            'sb.Append(ddlcourse.SelectedValue)
    '            'sb.Append(" and temp.userid= ")
    '            'sb.Append(ddlcandidates.SelectedValue)
    '            'sb.Append(" group by temp.qno,temp.course_id,temp.userid,temp.question,temp.total_marks,temp.test_type ")
    '            'sb.Append(" order by temp.qno ")
    '            dsQuestion = New DataSet
    '            da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '            da.Fill(dsQuestion)


    '            For i As Integer = 0 To dsQuestion.Tables(0).Rows.Count - 1


    '                sb = Nothing
    '                da = Nothing
    '                sb = New StringBuilder
    '                dsOpt = New DataSet
    '                'get options of the question
    '                sb.Append(" Select mop.Optionid,mop.[Option],mop.test_type,mop.qnid,mq.question ,mq.Qn_Category_ID ")
    '                sb.Append(" from M_Options as mop ")
    '                sb.Append(" LEFT join m_question as mq ")
    '                sb.Append(" on mq.qnid=mop.qnid ")
    '                sb.Append(" and mq.test_type=mop.test_type ")
    '                sb.Append(" where mop.Test_Type= " & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " and mop.Qnid=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " order by mop.Optionid ")
    '                da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                da.Fill(dsOpt)
    '                For opt As Integer = 0 To dsOpt.Tables(0).Rows.Count - 1
    '                    ImgNotFound = CheckImage(dsOpt.Tables(0).Rows(opt).Item(1).ToString)
    '                    If ImgNotFound(0) = "" Then
    '                        App.ActiveSheet.Cells(rows, 6).Value = dsOpt.Tables(0).Rows(opt).Item(1).ToString
    '                        App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        'App.ActiveSheet.Cells(11+i, 5+opt).ColumnWidth = 30
    '                        App.ActiveSheet.Cells(rows, 6).WrapText = True
    '                        App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
    '                        'App.ActiveSheet.Cells(11+i, 5+opt).RowHeight = 15

    '                    Else
    '                        If System.IO.File.Exists(Server.MapPath(ImgNotFound(0))) Then
    '                            App.ActiveSheet.Cells(rows, 6).Value = ImgNotFound(1)
    '                            Dim Align As Integer = 0
    '                            If ImgNotFound(1) <> "" Then
    '                                Align = 12
    '                            End If
    '                            pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(ImgNotFound(0)))
    '                            With pic
    '                                .top = App.ActiveSheet.Cells(rows, 6).top + 12
    '                                .left = App.ActiveSheet.Cells(rows, 6).left + 15
    '                                .width = 70
    '                                .height = 70
    '                            End With

    '                            App.ActiveSheet.Cells(rows, 6).RowHeight = 83
    '                            App.ActiveSheet.Cells(rows, 6).ColumnWidth = 15
    '                            App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
    '                            App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignJustify
    '                            App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        Else
    '                            App.ActiveSheet.Cells(rows, 6).Value = "Image not Found"
    '                            App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
    '                            App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                            App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        End If

    '                    End If
    '                    rows += 1

    '                Next
    '                ends = rows - 1

    '                sb = Nothing
    '                da = Nothing
    '                sb = New StringBuilder
    '                dsCrrctOpt = New DataSet
    '                'get correct option of the question
    '                sb.Append(" Select M_Question_Answer.Qn_ID,M_Question_Answer.Correct_Opt_Id as correctid,M_Question_Answer.Test_Type ,M_Options.[Option] as givenAnswer ,M_Question_Answer.sub_id ")
    '                sb.Append(" from M_Question_Answer  ")
    '                sb.Append(" inner join M_Options ")
    '                sb.Append(" on M_Options.Optionid=M_Question_Answer.Correct_Opt_Id ")
    '                sb.Append(" and M_Options.test_type=M_Question_Answer.test_type ")
    '                sb.Append(" and M_Options.Qnid=M_Question_Answer.Qn_ID ")
    '                sb.Append(" where M_Question_Answer.Test_Type=" & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " ")
    '                sb.Append(" and M_Question_Answer.Qn_ID= " & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " ")
    '                sb.Append(" order by M_Question_Answer.sub_id ")
    '                da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                da.Fill(dsCrrctOpt)
    '                For crr As Integer = 0 To dsCrrctOpt.Tables(0).Rows.Count - 1
    '                    strCorrectOpt += dsCrrctOpt.Tables(0).Rows(crr).Item(3).ToString + Environment.NewLine
    '                    arrCrrAns = strCorrectOpt.Split(Environment.NewLine)
    '                Next

    '                sb = Nothing
    '                da = Nothing
    '                sb = New StringBuilder
    '                dsGivenOpt = New DataSet
    '                'get answer given by the student
    '                sb.Append(" select  Distinct tr.userid,tr.test_type,tr.qno,tro.option_id as givemid,mop.[Option] , tro.sub_id ")
    '                sb.Append(" from T_Result as tr ")
    '                sb.Append(" left join T_Result_Option as tro ")
    '                sb.Append(" on tro.result_id=tr.result_id ")
    '                sb.Append(" left join M_Options as mop ")
    '                sb.Append(" on mop.Optionid=tro.option_id ")
    '                sb.Append(" and mop.test_type=tr.test_type ")
    '                sb.Append(" and mop.Qnid=tr.qno ")
    '                sb.Append(" where tr.test_type=" & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " ")
    '                sb.Append(" and tr.qno=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " and tr.userid= " & dsQuestion.Tables(0).Rows(i).Item(3).ToString & " and  tr.Course_id=" & ddlcourse.SelectedValue & "")
    '                sb.Append(" order by tro.sub_id ")
    '                Dim bb As String = sb.ToString
    '                da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                da.Fill(dsGivenOpt)
    '                For given As Integer = 0 To dsGivenOpt.Tables(0).Rows.Count - 1
    '                    If dsGivenOpt.Tables(0).Rows(given).Item(4).ToString <> "" Then
    '                        strGivenAns += dsGivenOpt.Tables(0).Rows(given).Item(4).ToString + Environment.NewLine
    '                        arrGivenAns = strGivenAns.Split(Environment.NewLine)
    '                    Else
    '                        strGivenAns = "Not Attempted"

    '                    End If

    '                Next

    '                'Serail number
    '                With App.ActiveSheet.Range("B" + start.ToString + ":B" + ends.ToString)
    '                    .MergeCells = True
    '                    '.Interior.ColorIndex = 40
    '                    '.Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                    .Cells.Value = i + 1
    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                    .WrapText = True
    '                End With

    '                'subject name
    '                With App.ActiveSheet.Range("C" + start.ToString + ":C" + ends.ToString)
    '                    .MergeCells = True
    '                    .WrapText = True
    '                    '.Interior.ColorIndex = 40
    '                    '.Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                    .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(7).ToString
    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                End With
    '                'student Name
    '                With App.ActiveSheet.Range("D" + start.ToString + ":D" + ends.ToString)
    '                    .MergeCells = True
    '                    .WrapText = True
    '                    '.Interior.ColorIndex = 40
    '                    '.Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                    .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(8).ToString
    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                End With
    '                'Question
    '                With App.ActiveSheet.Range("E" + start.ToString + ":E" + ends.ToString)
    '                    .MergeCells = True
    '                    .WrapText = True
    '                    '.Interior.ColorIndex = 40
    '                    ' .Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

    '                    If dsQuestion.Tables(0).Rows.Count <> 0 Then

    '                        Dim check() As String = CheckImage(dsQuestion.Tables(0).Rows(i).Item(4).ToString)
    '                        If check(0) = "" Then
    '                            .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(4).ToString
    '                        ElseIf check(0) <> "" Then

    '                            If System.IO.File.Exists(Server.MapPath(check(0))) Then
    '                                pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
    '                                With pic

    '                                    .top = App.ActiveSheet.Cells(start + 1, 6).top + 15
    '                                    .left = App.ActiveSheet.Cells(start + 1, 6).left + 15
    '                                    .width = 83
    '                                    .height = 83
    '                                End With
    '                                .Cells.Value = check(1).ToString
    '                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                            Else
    '                                .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
    '                            End If
    '                        Else
    '                        End If
    '                    End If
    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                End With

    '                'Correct option
    '                With App.ActiveSheet.Range("G" + start.ToString + ":G" + ends.ToString)
    '                    .MergeCells = True
    '                    .WrapText = True
    '                    '.Interior.ColorIndex = 40
    '                    ' .Font.Bold = True

    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

    '                    Dim check() As String = CheckImage(strCorrectOpt)
    '                    If check(0) = "" Then
    '                        .Cells.Value = strCorrectOpt
    '                    ElseIf check(0) <> "" Then

    '                        If System.IO.File.Exists(Server.MapPath(check(0))) Then
    '                            pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
    '                            With pic
    '                                .top = App.ActiveSheet.Cells(start + 1, 6).top + 15
    '                                .left = App.ActiveSheet.Cells(start + 1, 6).left + 15
    '                                .width = 83
    '                                .height = 83
    '                            End With
    '                            .Cells.Value = check(1).ToString
    '                            .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        Else
    '                            .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
    '                        End If
    '                    Else
    '                    End If


    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                End With

    '                'given answer by student
    '                With App.ActiveSheet.Range("H" + start.ToString + ":H" + ends.ToString)
    '                    .MergeCells = True
    '                    '.Interior.ColorIndex = 40
    '                    ' .Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                    'If strGivenAns = "Not Attempted" Then
    '                    '    .Font.ColorIndex = 3
    '                    'End If
    '                    '.Cells.Value = strGivenAns


    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                    .FormulaR1C1 = strGivenAns
    '                    .Characters(start:=1, Length:=0).Font.ColorIndex = Excel.Constants.xlAutomatic
    '                    If strGivenAns = "Not Attempted" Then
    '                        .Characters(start:=1, Length:=strGivenAns.Length).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

    '                    Else
    '                        Dim check() As String = CheckImage(strGivenAns)
    '                        Dim lenth As Integer
    '                        If check(0) = "" Then
    '                            If arrGivenAns.Length < arrCrrAns.Length Then
    '                                lenth = arrGivenAns.Length
    '                            Else
    '                                lenth = arrCrrAns.Length
    '                            End If


    '                            For Match As Integer = 0 To lenth - 1
    '                                If arrCrrAns(Match) = arrGivenAns(Match) Then
    '                                    Dim jk As String = arrGivenAns(Match)

    '                                    If Match <> lenth - 1 Then
    '                                        .Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).Length + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Green)
    '                                    End If

    '                                    '.Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).LastIndexOf(arrGivenAns(Match)) + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Green)
    '                                Else

    '                                    '.Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrGivenAns(Match).Length + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)
    '                                    .Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).LastIndexOf(arrGivenAns(Match)) + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

    '                                End If

    '                            Next
    '                        ElseIf check(0) <> "" Then

    '                            If System.IO.File.Exists(Server.MapPath(check(0))) Then
    '                                pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
    '                                With pic
    '                                    .top = App.ActiveSheet.Cells(start + 1, 7).top + 15
    '                                    .left = App.ActiveSheet.Cells(start + 1, 7).left + 15
    '                                    .width = 83
    '                                    .height = 83
    '                                End With
    '                                .Cells.Value = check(1).ToString
    '                                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                                .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                            Else
    '                                .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
    '                            End If

    '                            '.Cells.Value = crr
    '                        Else
    '                            '.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                            '.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                            ' .Cells.Value = ds1.Tables(0).Rows(0).Item(0).ToString

    '                        End If

    '                        'Apply the color to the second word

    '                    End If
    '                    'Apply the color to the third word
    '                    ' .Characters(start:=11, Length:=4).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Blue)
    '                    '.Columns.AutoFit()
    '                    .WrapText = True
    '                    strGivenAns = Nothing
    '                    strCorrectOpt = Nothing
    '                End With

    '                'total marks of the question
    '                With App.ActiveSheet.Range("I" + start.ToString + ":I" + ends.ToString)
    '                    .MergeCells = True
    '                    .WrapText = True
    '                    '.Interior.ColorIndex = 40
    '                    '.Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                    .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(5).ToString
    '                    TotalMarks += dsQuestion.Tables(0).Rows(i).Item(5)
    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                End With

    '                'mark obtain by the student
    '                With App.ActiveSheet.Range("J" + start.ToString + ":J" + ends.ToString)
    '                    .MergeCells = True
    '                    .WrapText = True
    '                    '.Interior.ColorIndex = 40
    '                    ' .Font.Bold = True
    '                    ' .Font.ColorIndex = 53
    '                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                    .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(1).ToString
    '                    MarkObtain += dsQuestion.Tables(0).Rows(i).Item(1)
    '                    '.Font.Size = 15
    '                    .BORDERS(xlEdgeLeft).Weight = 2
    '                    .BORDERS(xlEdgeTop).Weight = 2
    '                    .BORDERS(xlEdgeBottom).Weight = 2
    '                    .BORDERS(xlEdgeRight).Weight = 2
    '                End With
    '                start = ends + 1
    '            Next
    '            'If Not IsDBNull(dsQuestion) Then
    '            '    da = Nothing
    '            '    sb = Nothing
    '            '    sb = New StringBuilder
    '            '    dsTotalMarks = New DataSet
    '            '    sb.Append("Select Total_marks from M_Course where Course_id=")
    '            '    sb.Append(dsQuestion.Tables(0).Rows(0).Item(2).ToString)
    '            '    Dim aa As String = dsQuestion.Tables(0).Rows(0).Item(2).ToString
    '            '    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '            '    da.Fill(dsTotalMarks)
    '            'End If

    '        End If
    '        If Not objCn.MyConnection Is Nothing Then
    '            If objCn.MyConnection.State = ConnectionState.Open Then
    '                objCn.disconnect()
    '            End If
    '        End If

    '        ''date and time of exam given
    '        'With App.ActiveSheet.Range("D6:D6")
    '        '    .MergeCells = True
    '        '    ' .Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    '.Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    If strDateTime(0) <> "" Then
    '        '        .Cells.Value = strDateTime(1) + "/" + strDateTime(0) + "/" + strDateTime(2)
    '        '    End If

    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ''total marks
    '        'With App.ActiveSheet.Range("B7:C7")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = "Total Marks :"
    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ' ''total marks
    '        'With App.ActiveSheet.Range("D7:D7")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    '.Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    If dsDateTime.Tables(0).Rows.Count <> 0 Then
    '        '        .Cells.Value = dsDateTime.Tables(0).Rows(0).Item(1).ToString
    '        '    End If

    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ''total mark obtain in exam
    '        'With App.ActiveSheet.Range("B8:C8")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = "Mark Obtained :"
    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ''total mark obtain in exam
    '        'With App.ActiveSheet.Range("D8:D8")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    ' .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = MarkObtain
    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        'With App.ActiveSheet.Range("B9:C9")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    .Cells.Value = "Percentage :"
    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With

    '        ''total mark obtain in exam
    '        'With App.ActiveSheet.Range("D9:D9")
    '        '    .MergeCells = True
    '        '    '.Interior.ColorIndex = 40
    '        '    .Font.Bold = True
    '        '    ' .Font.ColorIndex = 53
    '        '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '        '    If dsDateTime.Tables(0).Rows.Count <> 0 Then
    '        '        .Cells.Value = Math.Round((MarkObtain * 100 / dsDateTime.Tables(0).Rows(0).Item(1)), 2).ToString + "%"
    '        '    End If


    '        '    '.Font.Size = 15
    '        '    '.BORDERS(xlEdgeLeft).Weight = 2
    '        '    '.BORDERS(xlEdgeTop).Weight = 2
    '        '    '.BORDERS(xlEdgeBottom).Weight = 2
    '        '    '.BORDERS(xlEdgeRight).Weight = 2
    '        'End With
    '        App.ActiveSheet.Cells(11, 1).ColumnWidth = 1

    '        App.ActiveSheet.Cells(11, 2).Value = "Sr.No."
    '        App.ActiveSheet.Cells(11, 2).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 2).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        ' App.ActiveSheet.Cells(11, 2).ColumnWidth = 30
    '        App.ActiveSheet.Cells(11, 2).WrapText = True
    '        App.ActiveSheet.Cells(11, 2).Borders.Weight = 2
    '        ' App.ActiveSheet.Cells(11, 2).RowHeight = 15

    '        App.ActiveSheet.Cells(11, 3).Value = "Subject Name"
    '        App.ActiveSheet.Cells(11, 3).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 3).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        App.ActiveSheet.Cells(11, 3).ColumnWidth = 15
    '        App.ActiveSheet.Cells(11, 3).WrapText = True
    '        App.ActiveSheet.Cells(11, 3).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 4).Value = "Student Name"
    '        App.ActiveSheet.Cells(11, 4).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 4).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 4).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 4).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        App.ActiveSheet.Cells(11, 4).ColumnWidth = 15
    '        App.ActiveSheet.Cells(11, 4).WrapText = True
    '        App.ActiveSheet.Cells(11, 4).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 5).Value = "Question"
    '        App.ActiveSheet.Cells(11, 5).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 5).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 5).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 5).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        App.ActiveSheet.Cells(11, 5).ColumnWidth = 40
    '        App.ActiveSheet.Cells(11, 5).WrapText = True
    '        App.ActiveSheet.Cells(11, 5).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 6).Value = "Option"
    '        App.ActiveSheet.Cells(11, 6).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 6).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        App.ActiveSheet.Cells(11, 6).ColumnWidth = 20
    '        App.ActiveSheet.Cells(11, 6).WrapText = True
    '        App.ActiveSheet.Cells(11, 6).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 7).Value = "Correct Answer"
    '        App.ActiveSheet.Cells(11, 7).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 7).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 7).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 7).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        App.ActiveSheet.Cells(11, 7).ColumnWidth = 20
    '        App.ActiveSheet.Cells(11, 7).WrapText = True
    '        App.ActiveSheet.Cells(11, 7).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 8).Value = "Given Answer"
    '        App.ActiveSheet.Cells(11, 8).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 8).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 8).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 8).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        App.ActiveSheet.Cells(11, 8).ColumnWidth = 20
    '        App.ActiveSheet.Cells(11, 8).WrapText = True
    '        App.ActiveSheet.Cells(11, 8).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 9).Value = "Total Marks"
    '        App.ActiveSheet.Cells(11, 9).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 9).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 9).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 9).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        ' App.ActiveSheet.Cells(11,8).ColumnWidth = 30
    '        App.ActiveSheet.Cells(11, 9).WrapText = True
    '        App.ActiveSheet.Cells(11, 9).Borders.Weight = 2

    '        App.ActiveSheet.Cells(11, 10).Value = "Marks Obtained"
    '        App.ActiveSheet.Cells(11, 10).Interior.ColorIndex = 36
    '        App.ActiveSheet.Cells(11, 10).Font.Bold = True
    '        App.ActiveSheet.Cells(11, 10).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '        App.ActiveSheet.Cells(11, 10).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '        ' App.ActiveSheet.Cells(11,9).ColumnWidth = 30
    '        App.ActiveSheet.Cells(11, 10).WrapText = True
    '        App.ActiveSheet.Cells(11, 10).Borders.Weight = 2

    '        'Save WorkBook

    '        ' App.ActiveWorkbook.Sheets("Exam Details").Range("B12", "B12").EntireRow.FreezePanes = True
    '        App.ActiveWorkbook.Sheets("Exam Details").Range("A5", "A10").EntireRow.Delete(Excel.XlDeleteShiftDirection.xlShiftUp)
    '        '(Excel.XlDeleteShiftDirection.xlShiftUp)
    '        'App.ActiveSheet.Cells(11, 10).EntireRow()
    '        'Dim rng As Excel.Range = App.ActiveWorkbook.Sheets("Exam Details").Range("B1", "B1").EntireRow
    '        'Dim rng1 As Excel.Range = App.ActiveWorkbook.Sheets("Exam Details").Range("F1", "F1").EntireColumn
    '        'App.ActiveWindow.FreezePanes = True
    '        Dim ViewName() As String = {"Sheet2", "Sheet3"}

    '        For Each ws As Excel.Worksheet In WorkBook.Sheets
    '            For de As Integer = 0 To ViewName.Length - 1
    '                If ws.Name.ToString().Trim().ToUpper() = ViewName(de).ToUpper() Then
    '                    ws.Delete()
    '                    Exit For
    '                End If
    '            Next
    '        Next

    '        If System.IO.File.Exists(Server.MapPath("Excel Import\StudentExamDetails.XLS")) Then
    '            System.IO.File.Delete(Server.MapPath("Excel Import\StudentExamDetails.XLS"))
    '        End If

    '        fileName1 = Server.MapPath("Excel Import\StudentExamDetails.XLS")



    '        WorkBook.SaveAs(fileName1, objOpt, objOpt, objOpt, objOpt, objOpt, _
    '                        Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
    '        WorkBooks.Close()

    '        Dim file As New IO.FileInfo(Server.MapPath("Excel Import\StudentExamDetails.XLS"))

    '        If file.Exists = True Then

    '            Response.Clear()
    '            Response.ClearHeaders()
    '            Response.ClearContent()
    '            Response.ContentType = "application/ms-excel"
    '            Response.AppendHeader("Content-disposition", "attachment; filename=" + file.Name)
    '            Response.AddHeader("Content-Length", file.Length.ToString)
    '            Response.ContentType = "application/octet-stream"
    '            Response.WriteFile(file.FullName)
    '            Response.Flush()

    '        End If
    '        System.IO.File.Delete(Server.MapPath("Excel Import\StudentExamDetails.XLS"))
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx")
    '    Finally
    '        If Not objCn.MyConnection Is Nothing Then
    '            If objCn.MyConnection.State = ConnectionState.Open Then
    '                objCn.disconnect()
    '            End If
    '        End If
    '        App = Nothing
    '        WorkBooks = Nothing
    '        WorkBook = Nothing
    '        Sheet = Nothing
    '        Sheets = Nothing
    '        objOpt = Nothing
    '        objCn = Nothing
    '        sheet1 = Nothing
    '        ImgNotFound = Nothing
    '        pic = Nothing
    '        fileName1 = Nothing
    '        strpath = Nothing
    '        strCorrectOpt = Nothing
    '        strGivenAns = Nothing
    '        strDateTime = Nothing
    '        dsDateTime = Nothing
    '        dsQuestion = Nothing
    '        dsOpt = Nothing
    '        dsCrrctOpt = Nothing
    '        dsGivenOpt = Nothing
    '        dsTotalMarks = Nothing
    '        da = Nothing
    '        sb = Nothing
    '        ImgNotFound = Nothing
    '        arrCrrAns = Nothing
    '        arrGivenAns = Nothing
    '    End Try
    'End Sub

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
                str = value.Substring((value.IndexOf("=") + 1),
                                      ((value.IndexOf(" ", (value.IndexOf("=") + 1))) - (value.IndexOf("=") + 1)))
                strVal(0) = str
                strVal(1) = value.Substring(0, value.IndexOf("<"))
                'str = value.Substring(14, (value.IndexOf(" ", 15) - 14))
                Return strVal
            ElseIf value.Contains("<img") Then
                ' str = value.Substring((value.IndexOf("=") + 1), (value.IndexOf(" ", 15) - (value.IndexOf("=") + 1)))
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

    'Added by Pranit Chimurkar on 2019/10/21
    Protected Sub ExportAppearedExamDetails_Click(sender As Object, e As EventArgs) Handles ExportAppearedExamDetails.Click
        Try
            If (sel_subjectid.SelectedValue = 0) Then
                errorMsg.Text = Resources.Resource.Search_PlSelClName
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    errorMsg.Text = CommonMessageLingual.lblMsgCenterNm.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    errorMsg.Text = CommonMessageLingual.lblMsgCenterNm.ToString
                'End If

                'gridDiv.Visible = False
            ElseIf ddlcourse.SelectedValue = 0 Then
                errorMsg.Text = Resources.Resource.CourseRegistration_ValNm
                'If Request.Cookies("Lang").Value = "en-us" Then
                '    errorMsg.Text = CommonMessageLingual.lblMsgCrsNmSele.ToString
                'ElseIf Request.Cookies("Lang").Value = "ja-jp" Then
                '    errorMsg.Text = CommonMessageLingual.lblMsgCrsNmSele.ToString
                'End If

                'gridDiv.Visible = False
                'ElseIf ddlcandidates.SelectedValue = 0 Then
                '    errorMsg.Text = "Please select Candidate Name"
                '    'gridDiv.Visible = False
            Else
                errorMsg.Text = String.Empty
                errorMsg.Visible = False
                DGData.CurrentPageIndex = 0
                'BindGrid()               
                ExportExamDetails()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub
End Class
#End Region