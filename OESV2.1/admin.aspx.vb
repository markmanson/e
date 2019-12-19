Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Configuration
Imports System.Web.Security
Imports System.Web.Services

Namespace unirecruite


    Partial Class admin
        'Inherits System.Web.UI.Page
        Inherits BasePage
        Protected WithEvents HyperLink1 As System.Web.UI.WebControls.HyperLink


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
        '*************On Error Go to Error Page****************
        'Private Sub onErrors(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Error
        '  Response.Redirect("error.aspx?err=Error On Page")
        'End Sub

        '**************************************************************************
        'Function               :   Page_Load
        'simrandosti@hotmail.com
        '
        'Return                 :   None
        '
        'Argument               :   sender : system object
        '                           e      : Image click event
        '
        'Explanation            :   Function call on load of page
        '                           this will list out the name of report 
        '                           with modification link for that report
        '                           
        'Note                   :   None
        '**************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Session("DDLSelectedValue") = Nothing
            'Session.Remove("sname")
            If Not Session("sname") Is Nothing Then
                Session.Remove("sname")
            End If
            If Not Session("ques") Is Nothing Then
                Session.Remove("ques")
            End If

            'If Session("userid") <> Nothing Then
            '    Session.Remove("userid")
            'End If

            If Session("last") <> Nothing Then
                Session.Remove("last")
            End If

            If Session("ECourse") <> Nothing Then
                Session.Remove("ECourse")
                If Session("Etest") <> Nothing Then
                    Session.Remove("Etest")
                End If

            End If
            If Not Session("cname") Is Nothing Then
                Session.Remove("cname")
            End If
            'added by bhumi [9/10/2015]
            'Reason: on CentreRegistration page getting wrong centerid
            If Not Session("CenterID") Is Nothing Then
                Session.Remove("CenterID")
            End If
            'Ended by bhumi
            'Put user code to initialize the page here
            '
            '***********************************************************
            'Added by Tabrez 
            'Purpose: Disabling Forward Button functionality, so 
            'that user cannot return without logging in.
            '***********************************************************
            If Session("LoginGenuine") Is Nothing Then
                Response.Redirect("error.aspx?err=admin Please Login to continue")
            End If
            Response.Buffer = True
            Response.ExpiresAbsolute = Date.Now()
            Response.Expires = 0
            Response.CacheControl = "no-cache"

            If Not Session("IsGenuineUser") Then
                Response.Redirect("login.aspx")
                Response.End()
            End If

            If Not Session("CourseIDValue") Is Nothing Then
                Session.Remove("CourseIDValue")
            End If
            If Not Session("CourseID") Is Nothing Then
                Session.Remove("CourseID")
            End If

            Session("FromPage") = "Admin"

            'Change by Bhasker(2009/11/25)
            'Session value to be null if redirect to home page
            Session("BackQuery") = ""

            'Code added by Pratik - 2004/06/02


            '******************************Monal Shah***********************
            '******************************Unneccesary Code******************
            'Dim CONS As New unirecruite.Errconstants

            'Dim objconn As New ConnectDb

            'Dim myDataReader As SqlDataReader
            'Dim myCommand As SqlCommand
            'Dim sqlstr As String
            'Dim myRow As HtmlTableRow
            'Dim myCol As HtmlTableCell
            'Dim links As HyperLink
            'Dim conn As SqlConnection


            'Code commented by Pratik
            'If objconn.connect(objconn.PATHDB) Then
            'Dim strPathDb As String
            'conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            'conn.Open()
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

            'sqlstr = "SELECT DISTINCT report_name FROM t_report"
            'myCommand = New SqlCommand(sqlstr, conn)
            'myDataReader = myCommand.ExecuteReader()
            'While myDataReader.Read
            '    myRow = New HtmlTableRow
            '    myCol = New HtmlTableCell

            '    links = New HyperLink
            '    links.NavigateUrl = "report.aspx?rptname=" & myDataReader.Item("report_name")
            '    links.Text = "" & myDataReader.Item("report_name")
            '    myCol.Controls.Add(links)
            '    myRow.Controls.Add(myCol)
            '    tblReport.Rows.Insert(1, myRow)

            'End While
            'myDataReader.Close()
            '----------------------------------
            'sqlstr = "SELECT Distinct search_name FROM t_search_criteria"
            'myCommand = New SqlCommand(sqlstr, conn)
            'myDataReader = myCommand.ExecuteReader()
            'While myDataReader.Read
            '    myRow = New HtmlTableRow
            '    myCol = New HtmlTableCell
            '    links = New HyperLink
            '    links.NavigateUrl = "search.aspx?searchname=" & myDataReader.Item("search_name")
            '    links.Text = "" & myDataReader.Item("search_name")
            '    myCol.Controls.Add(links)
            '    myRow.Controls.Add(myCol)
            '    tblSearch.Rows.Insert(1, myRow)
            'End While
            'conn.Close()
            '' myDataReader.Close()
            'myCommand.Dispose()
            'myDataReader.Close()
            ' End If
            '******************************End***********************

            'If Not Me.IsPostBack Then
            '    Dim constr As String = ConfigurationSettings.AppSettings("PathDb")
            '    Using con As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            '        Using cmd As New SqlCommand("SELECT [User_ID],[Course_ID] FROM [OES_Final1].[dbo].[T_User_Course]")
            '            cmd.CommandType = CommandType.Text
            '            'cmd.Connection = con
            '            con.Open()
            '            ddlCenters.DataSource = cmd.ExecuteReader()
            '            ddlCenters.DataTextField = "User_ID"
            '            ddlCenters.DataValueField = "Course_ID"
            '            ddlCenters.DataBind()
            '            con.Close()
            '        End Using
            '    End Using
            '    ddlCenters.Items.Insert(0, New ListItem("--Select Class--", "0"))
            'End If

            Dim UserId As String
            UserId = Session("adminid")
            Dim myCommand As SqlCommand
            Dim sqlstr As String
            Dim conn As SqlConnection
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()
            sqlstr = " select Center_ID,Center_Name
from M_Centers 
where Del_Flg=0 
and Email in (select Email from M_USER_INFO where Delete_Flg=0 and User_Type=1 and Userid='" & UserId & "') 
order by Center_Name"
            myCommand = New SqlCommand(sqlstr, conn)
            ddlCenters.DataSource = myCommand.ExecuteReader()
            ddlCenters.DataTextField = "Center_Name"
            ddlCenters.DataValueField = "Center_ID"
            ddlCenters.DataBind()
            conn.Close()
            'ddlCenters.Items.Insert(0, New ListItem("--Select Class--", "0"))   
            'Dim obj As admin = New admin
            'Dim temp As String = "51"
            'admin.NotApperedDountChart(temp)
            'ddlCenters_SelectedIndexChanged(sender, e)
            ddlCenters.SelectedIndex = 0
        End Sub

        'Protected Sub ddlCenters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCenters.SelectedIndexChanged
        '    ddlCenters.SelectedIndex = 0
        'End Sub


        '    Private Sub imgLogoff_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgLogoff.Click
        '        FormsAuthentication.SignOut()
        '        Session.Remove("LoginGenuine")
        '        Response.Redirect("login.aspx")
        '    End Sub
        <WebMethod(EnableSession:=True)>
        Public Shared Function StatisticData() As ArrayList


            'Dim ctx As HttpContext = System.Web.HttpContext.Current
            'uname = ctx.Session("Username")


            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            Dim i As Integer
            i = 0

            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select  Top 7 LoginName FROM [OES_Final1].[dbo].[M_USER_INFO]"
            myCommand = New SqlCommand(sqlstr, conn)
            myDataReader = myCommand.ExecuteReader()
            While myDataReader.Read()
                Dim dict As New Dictionary(Of String, Object)
                For count As Integer = 0 To (myDataReader.FieldCount - 1)
                    dict.Add(myDataReader.GetName(count), myDataReader(count))
                Next
                arrlist.Add(dict.Values)
                'arrlist.Add(myDataReader(i).ToString)
                'i = i + 1
            End While
            myCommand.Dispose()
            myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function

        'douhnut chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function StatisticExamData() As ArrayList
            Dim uname As String
            Dim ctx As HttpContext = System.Web.HttpContext.Current
            uname = ctx.Session("Username")
            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            Dim i As Integer
            i = 0

            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select	AppearedFlg, 
	ceiling(round( (COUNT(*)*100)/(totalNumberOfRows+0.0),0)) as AppearedPercentage 
from (
	select 
		main.UserId,
		case when main.Appearedflag=2 then 'Appeared' else 'Not Appeared' end as AppearedFlg,
		temp.totalNumberOfRows 
	from T_Candidate_Status main,
	(select UserId, count(*) as totalNumberOfRows from T_Candidate_Status 
	where convert(date, Written_Test_Date, 126) between convert(date, '2019-11-01', 126) and convert(date, '2019-11-06', 126)
	group by UserId) temp
	where convert(date, main.Written_Test_Date, 126) between convert(date, '2019-11-01', 126) and convert(date, '2019-11-06', 126)
	and main.UserId=499
	and main.UserId=temp.UserId
) temp
where UserId=499
group by UserId, AppearedFlg, totalNumberOfRows
order by UserId, AppearedFlg"
            'myCommand = New SqlCommand(sqlstr, conn)
            'myDataReader = myCommand.ExecuteReader()
            'While myDataReader.Read()
            '    Dim dict As New Dictionary(Of String, Object)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    arrlist.Add(dict.Values)
            'End While
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count > 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
            Else
                If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Appeared") Then
                    arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                    arrlist.Add("0")
                Else
                    arrlist.Add("0")
                    arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                End If
            End If

            'While myDataReader.Read()
            '    'If myDataReader("AppearedFlg") IsNot "Appeared" Then
            '    Dim dict As New Dictionary(Of String, Object)
            '    dict.Add("AppearedPercentage", 0)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    'dict.Add(myDataReader("AppearedFlg"), myDataReader("AppearedPercentage"))
            '    arrlist.Add(dict.Values)
            '    ' End If
            'End While

            'myCommand.Dispose()
            'myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function

        'Bar chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function NotApperedDountChart(Classid1 As String) As ArrayList

            'Dim UserId As Integer
            'Dim ctx As HttpContext = System.Web.HttpContext.Current
            'UserId = Convert.ToInt32(ctx.Session("userid"))

            'Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
            'Dim startOfWeek = DateTime.Today.AddDays(-1 * (dayOfWeek - 1)).ToString("yyyy-MM-dd")
            'Dim endOfWeek = DateTime.Today.AddDays(5 - dayOfWeek).ToString("yyyy-MM-dd")

            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = "select	
	mui.Name + ' ' + mui.SurName as UserName,
	ceiling(round( (COUNT(*)*100)/(totalNumberOfRows+0.0),0)) as AppearedPercentage 	
from (
	select 
		main.UserId,
		main.Course_ID,
		case when main.Appearedflag=2 then 'Appeared' else 'Not Appeared' end as AppearedFlg,
		temp1.totalNumberOfRows 
	from T_Candidate_Status main,
	(select UserId, count(*) as totalNumberOfRows from T_Candidate_Status 
		where convert(date, Written_Test_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
		group by UserId
	) temp1
	
	where convert(date, main.Written_Test_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
	and main.UserId in (select Userid from M_USER_INFO where Delete_Flg=0 and User_Type=0 and center_id='" & Classid1 & "')
	and main.UserId=temp1.UserId
	and main.Appearedflag != 2
) temp
left join M_USER_INFO mui on temp.UserId=mui.UserID 
where temp.UserId in (select Userid from M_USER_INFO where Delete_Flg=0 and User_Type=0 and center_id='" & Classid1 & "')
group by temp.UserId, AppearedFlg, mui.Name + ' ' + mui.SurName,temp.totalNumberOfRows
order by AppearedPercentage DESC 
"
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count > 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
                'ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
                '    If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Appeared") Then
                '        arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                '        arrlist.Add("0")
                '    Else
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                '    End If
                'ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
                '    arrlist.Add("0")
                '    arrlist.Add("0")
            End If

            conn.Close()
            Return arrlist
        End Function

        'Bar chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function StatisticBarChart() As ArrayList
            Dim UserId As Integer
            Dim ctx As HttpContext = System.Web.HttpContext.Current
            UserId = Convert.ToInt32(ctx.Session("userid"))

            Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
            Dim startOfWeek = DateTime.Today.AddDays(-1 * (dayOfWeek - 1)).ToString("yyyy-MM-dd")
            Dim endOfWeek = DateTime.Today.AddDays(5 - dayOfWeek).ToString("yyyy-MM-dd")

            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select 
	 
	Description,
	left(avg(percentage),2) as percentage

from (
	select 
		ROW_NUMBER() over(partition by can.UserId,mc.Description,can.Course_ID order by mc.Description, fin.CourseCode,fin.Exam_status desc) as cnt,
		can.UserId,
		fin.UserName, 
		can.Course_ID,
		fin.CourseCode, 
		mc.Description,
		can.Written_Test_Date,
		can.Written_Test_Appear_Date,
		fin.Exam_status, 
		fin.MailSent_flag, 
		fin.TotalMarks,
		fin.Obtained_Marks,
		--left(round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2),5) + '%' as percentage
		round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2) as percentage
	from T_Candidate_Status can
	left join Final_Result fin on can.UserId=fin.UserID and can.Course_ID=fin.CourseID
	left join M_Course mc on mc.Course_id=can.Course_ID
	where can.UserId=635
	and can.Appearedflag=2
	and convert(date, can.Written_Test_Appear_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
) temp 
where cnt =1
group by UserId, UserName, Description
order by percentage"
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count >= 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
            ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
                If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Grammer") Then
                    arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                    arrlist.Add("0")
                    arrlist.Add("0")
                    arrlist.Add("0")

                ElseIf myDataSet.Tables(0).Rows(1)(0).ToString().Equals("Vocab") Then
                    arrlist.Add("0")
                    arrlist.Add(myDataSet.Tables(0).Rows(1)(1).ToString())
                    arrlist.Add("0")
                    arrlist.Add("0")

                ElseIf myDataSet.Tables(0).Rows(2)(0).ToString().Equals("Reading") Then
                    arrlist.Add("0")
                    arrlist.Add("0")
                    arrlist.Add(myDataSet.Tables(0).Rows(2)(1).ToString())
                    arrlist.Add("0")
                ElseIf myDataSet.Tables(0).Rows(3)(0).ToString().Equals("Reading") Then
                    arrlist.Add("0")
                    arrlist.Add("0")
                    arrlist.Add("0")
                    arrlist.Add(myDataSet.Tables(0).Rows(3)(1).ToString())
                End If
            ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
                arrlist.Add("0")
                arrlist.Add("0")
                arrlist.Add("0")
                arrlist.Add("0")
            End If

            'While myDataReader.Read()
            '    'If myDataReader("AppearedFlg") IsNot "Appeared" Then
            '    Dim dict As New Dictionary(Of String, Object)
            '    dict.Add("AppearedPercentage", 0)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    'dict.Add(myDataReader("AppearedFlg"), myDataReader("AppearedPercentage"))
            '    arrlist.Add(dict.Values)
            '    ' End If
            'End While

            'myCommand.Dispose()
            'myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function


        'Grammer Bar chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function GrammerBarChart(Classid1 As String) As ArrayList
            'Dim UserId As String
            'UserId = Session("adminid")
            Dim UserId As Integer
            Dim ctx As HttpContext = System.Web.HttpContext.Current
            UserId = Convert.ToInt32(ctx.Session("adminid"))

            'Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
            'Dim startOfWeek = DateTime.Today.AddDays(-1 * (dayOfWeek - 1)).ToString("yyyy-MM-dd")
            'Dim endOfWeek = DateTime.Today.AddDays(5 - dayOfWeek).ToString("yyyy-MM-dd")

            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select 	
	UserName,  	
	left(avg(percentage),2) as percentage
from (
	select 
		ROW_NUMBER() over(partition by can.UserId,mc.Description,can.Course_ID order by mc.Description, fin.CourseCode,fin.Exam_status desc) as cnt,
		can.UserId,
		fin.UserName, 
		can.Course_ID,
		fin.CourseCode, 
		mc.Description,
		can.Written_Test_Date,
		can.Written_Test_Appear_Date,
		fin.Exam_status, 
		fin.MailSent_flag, 
		fin.TotalMarks,
		fin.Obtained_Marks,
		--left(round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2),5) + '%' as percentage
		round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2) as percentage
	from T_Candidate_Status can
	left join Final_Result fin on can.UserId=fin.UserID and can.Course_ID=fin.CourseID
	left join M_Course mc on mc.Course_id=can.Course_ID
	where can.UserId in (select Userid from M_USER_INFO where Delete_Flg=0 and User_Type=0 and center_id='" & Classid1 & "')
	and can.Appearedflag=2
	and convert(date, can.Written_Test_Appear_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
) temp 
where cnt =1 and Description='Grammer'
group by UserId, UserName, Description
order by percentage DESC"
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count >= 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
                'ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
                '    If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Grammer") Then
                '        arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(1)(0).ToString().Equals("Vocab") Then
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(1)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(2)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(2)(1).ToString())
                '        arrlist.Add("0")
                '    ElseIf myDataSet.Tables(0).Rows(3)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(3)(1).ToString())
                '    End If
                'ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
            End If

            'While myDataReader.Read()
            '    'If myDataReader("AppearedFlg") IsNot "Appeared" Then
            '    Dim dict As New Dictionary(Of String, Object)
            '    dict.Add("AppearedPercentage", 0)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    'dict.Add(myDataReader("AppearedFlg"), myDataReader("AppearedPercentage"))
            '    arrlist.Add(dict.Values)
            '    ' End If
            'End While

            'myCommand.Dispose()
            'myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function


        'Vocab Bar chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function VocabBarChart(Classid1 As String) As ArrayList
            'Dim UserId As String
            'UserId = Session("adminid")
            'Dim UserId As Integer
            'Dim ctx As HttpContext = System.Web.HttpContext.Current
            'UserId = Convert.ToInt32(ctx.Session("adminid"))

            'Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
            'Dim startOfWeek = DateTime.Today.AddDays(-1 * (dayOfWeek - 1)).ToString("yyyy-MM-dd")
            'Dim endOfWeek = DateTime.Today.AddDays(5 - dayOfWeek).ToString("yyyy-MM-dd")

            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select 	
	UserName,  	
	left(avg(percentage),2) as percentage
from (
	select 
		ROW_NUMBER() over(partition by can.UserId,mc.Description,can.Course_ID order by mc.Description, fin.CourseCode,fin.Exam_status desc) as cnt,
		can.UserId,
		fin.UserName, 
		can.Course_ID,
		fin.CourseCode, 
		mc.Description,
		can.Written_Test_Date,
		can.Written_Test_Appear_Date,
		fin.Exam_status, 
		fin.MailSent_flag, 
		fin.TotalMarks,
		fin.Obtained_Marks,
		--left(round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2),5) + '%' as percentage
		round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2) as percentage
	from T_Candidate_Status can
	left join Final_Result fin on can.UserId=fin.UserID and can.Course_ID=fin.CourseID
	left join M_Course mc on mc.Course_id=can.Course_ID
	where can.UserId in (select Userid from M_USER_INFO where Delete_Flg=0 and User_Type=0 and center_id='" & Classid1 & "')
	and can.Appearedflag=2
	and convert(date, can.Written_Test_Appear_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
) temp 
where cnt =1 and Description='Vocab'
group by UserId, UserName, Description
order by percentage DESC"
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count >= 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
                'ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
                '    If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Grammer") Then
                '        arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(1)(0).ToString().Equals("Vocab") Then
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(1)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(2)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(2)(1).ToString())
                '        arrlist.Add("0")
                '    ElseIf myDataSet.Tables(0).Rows(3)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(3)(1).ToString())
                '    End If
                'ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
            End If

            'While myDataReader.Read()
            '    'If myDataReader("AppearedFlg") IsNot "Appeared" Then
            '    Dim dict As New Dictionary(Of String, Object)
            '    dict.Add("AppearedPercentage", 0)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    'dict.Add(myDataReader("AppearedFlg"), myDataReader("AppearedPercentage"))
            '    arrlist.Add(dict.Values)
            '    ' End If
            'End While

            'myCommand.Dispose()
            'myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function


        'Reading/Chokai Bar chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function ReadingBarChart(Classid1 As String) As ArrayList
            'Dim UserId As String
            'UserId = Session("adminid")
            'Dim UserId As Integer
            'Dim ctx As HttpContext = System.Web.HttpContext.Current
            'UserId = Convert.ToInt32(ctx.Session("adminid"))

            'Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
            'Dim startOfWeek = DateTime.Today.AddDays(-1 * (dayOfWeek - 1)).ToString("yyyy-MM-dd")
            'Dim endOfWeek = DateTime.Today.AddDays(5 - dayOfWeek).ToString("yyyy-MM-dd")

            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select 	
	UserName,  	
	left(avg(percentage),2)+'%' as percentage
from (
	select 
		ROW_NUMBER() over(partition by can.UserId,mc.Description,can.Course_ID order by mc.Description, fin.CourseCode,fin.Exam_status desc) as cnt,
		can.UserId,
		fin.UserName, 
		can.Course_ID,
		fin.CourseCode, 
		mc.Description,
		can.Written_Test_Date,
		can.Written_Test_Appear_Date,
		fin.Exam_status, 
		fin.MailSent_flag, 
		fin.TotalMarks,
		fin.Obtained_Marks,
		--left(round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2),5) + '%' as percentage
		round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2) as percentage
	from T_Candidate_Status can
	left join Final_Result fin on can.UserId=fin.UserID and can.Course_ID=fin.CourseID
	left join M_Course mc on mc.Course_id=can.Course_ID
	where can.UserId in (select Userid from M_USER_INFO where Delete_Flg=0 and User_Type=0 and center_id='" & Classid1 & "')
	and can.Appearedflag=2
	and convert(date, can.Written_Test_Appear_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
) temp 
where cnt =1 and Description='Reading'
group by UserId, UserName, Description
order by percentage DESC"
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count >= 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
                'ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
                '    If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Grammer") Then
                '        arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(1)(0).ToString().Equals("Vocab") Then
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(1)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(2)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(2)(1).ToString())
                '        arrlist.Add("0")
                '    ElseIf myDataSet.Tables(0).Rows(3)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(3)(1).ToString())
                '    End If
                'ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
            End If

            'While myDataReader.Read()
            '    'If myDataReader("AppearedFlg") IsNot "Appeared" Then
            '    Dim dict As New Dictionary(Of String, Object)
            '    dict.Add("AppearedPercentage", 0)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    'dict.Add(myDataReader("AppearedFlg"), myDataReader("AppearedPercentage"))
            '    arrlist.Add(dict.Values)
            '    ' End If
            'End While

            'myCommand.Dispose()
            'myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function

        'Listening Bar chart
        <WebMethod(EnableSession:=True)>
        Public Shared Function ListeningBarChart(Classid1 As String) As ArrayList
            'Dim UserId As String
            'UserId = Session("adminid")
            'Dim UserId As Integer
            'Dim ctx As HttpContext = System.Web.HttpContext.Current
            'UserId = Convert.ToInt32(ctx.Session("adminid"))

            'Dim dayOfWeek = CInt(DateTime.Today.DayOfWeek)
            'Dim startOfWeek = DateTime.Today.AddDays(-1 * (dayOfWeek - 1)).ToString("yyyy-MM-dd")
            'Dim endOfWeek = DateTime.Today.AddDays(5 - dayOfWeek).ToString("yyyy-MM-dd")

            Dim myDataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim myDataSet As DataSet
            Dim myDataAdapter As SqlDataAdapter
            Dim sqlstr As String
            Dim conn As SqlConnection
            Dim arrlist As New ArrayList
            conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
            conn.Open()

            sqlstr = " select 	
	UserName,  	
	left(avg(percentage),2)+'%' as percentage
from (
	select 
		ROW_NUMBER() over(partition by can.UserId,mc.Description,can.Course_ID order by mc.Description, fin.CourseCode,fin.Exam_status desc) as cnt,
		can.UserId,
		fin.UserName, 
		can.Course_ID,
		fin.CourseCode, 
		mc.Description,
		can.Written_Test_Date,
		can.Written_Test_Appear_Date,
		fin.Exam_status, 
		fin.MailSent_flag, 
		fin.TotalMarks,
		fin.Obtained_Marks,
		--left(round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2),5) + '%' as percentage
		round( (fin.Obtained_Marks*100)/(fin.TotalMarks+0.0),2) as percentage
	from T_Candidate_Status can
	left join Final_Result fin on can.UserId=fin.UserID and can.Course_ID=fin.CourseID
	left join M_Course mc on mc.Course_id=can.Course_ID
	where can.UserId in (select Userid from M_USER_INFO where Delete_Flg=0 and User_Type=0 and center_id='" & Classid1 & "')
	and can.Appearedflag=2
	and convert(date, can.Written_Test_Appear_Date, 126) between convert(date, '2019-10-01', 126) and convert(date, '2019-10-31', 126)
) temp 
where cnt =1 and Description='Listening'
group by UserId, UserName, Description
order by percentage DESC"
            myDataSet = New DataSet()
            myDataAdapter = New SqlDataAdapter(sqlstr, conn)
            myDataAdapter.Fill(myDataSet)

            If myDataSet.Tables(0).Rows.Count >= 1 Then
                For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                    arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString() + "_" + myDataSet.Tables(0).Rows(count)(1).ToString())
                Next
                'ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
                '    If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Grammer") Then
                '        arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(1)(0).ToString().Equals("Vocab") Then
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(1)(1).ToString())
                '        arrlist.Add("0")
                '        arrlist.Add("0")

                '    ElseIf myDataSet.Tables(0).Rows(2)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(2)(1).ToString())
                '        arrlist.Add("0")
                '    ElseIf myDataSet.Tables(0).Rows(3)(0).ToString().Equals("Reading") Then
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add("0")
                '        arrlist.Add(myDataSet.Tables(0).Rows(3)(1).ToString())
                '    End If
                'ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
                '    arrlist.Add("0")
            End If

            'While myDataReader.Read()
            '    'If myDataReader("AppearedFlg") IsNot "Appeared" Then
            '    Dim dict As New Dictionary(Of String, Object)
            '    dict.Add("AppearedPercentage", 0)
            '    For count As Integer = 0 To (myDataReader.FieldCount - 1)
            '        dict.Add(myDataReader.GetName(count), myDataReader(count))
            '    Next
            '    'dict.Add(myDataReader("AppearedFlg"), myDataReader("AppearedPercentage"))
            '    arrlist.Add(dict.Values)
            '    ' End If
            'End While

            'myCommand.Dispose()
            'myDataReader.Close()
            conn.Close()
            Return arrlist
        End Function


    End Class

End Namespace
