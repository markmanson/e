Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

Partial Public Class WebForm1
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Session.Item("userid").ToString().Trim() <> String.Empty Then
        '    txtID.Text = Session.Item("userid").ToString()
        '    'txtPWD.Text = Session.Item("pwd").ToString()
        'End If
        Session.Add("check", "true")
    End Sub

    'douhnut chart
    <WebMethod(EnableSession:=True)>
    Public Shared Function StatisticExamData() As ArrayList
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

        sqlstr = " select	AppearedFlg, 
	ceiling(round( (COUNT(*)*100)/(totalNumberOfRows+0.0),0)) as AppearedPercentage 
from (
	select 
		main.UserId,
		case when main.Appearedflag=2 then 'Appeared' else 'Not Appeared' end as AppearedFlg,
		temp.totalNumberOfRows 
	from T_Candidate_Status main,
	(select UserId, count(*) as totalNumberOfRows from T_Candidate_Status 
	where convert(date, Written_Test_Date, 126) between convert(date, '" & startOfWeek & "', 126) and convert(date, '" & endOfWeek & "', 126)
	group by UserId) temp
	where convert(date, main.Written_Test_Date, 126) between convert(date, '" & startOfWeek & "', 126) and convert(date, '" & endOfWeek & "', 126)
	and main.UserId='" & UserId & "'
	and main.UserId=temp.UserId
) temp
where UserId='" & UserId & "'
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
        ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
            If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Appeared") Then
                arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                arrlist.Add("0")
            Else
                arrlist.Add("0")
                arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
            End If
        ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
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

    'Pie chart
    <WebMethod(EnableSession:=True)>
    Public Shared Function StatisticExamDataPie() As ArrayList
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

        sqlstr = " select	AppearedFlg, 
    	ceiling(round( (COUNT(*)*100)/(totalNumberOfRows+0.0),0)) as AppearedPercentage 
    from (
    	select 
    		main.UserId,
    		case when main.Appearedflag=2 then 'Appeared' else 'Not Appeared' end as AppearedFlg,
    		temp.totalNumberOfRows 
    	from T_Candidate_Status main,
    	(select UserId, count(*) as totalNumberOfRows from T_Candidate_Status 
    	where convert(date, Written_Test_Date, 126) between convert(date, '" & startOfWeek & "', 126) and convert(date, '" & endOfWeek & "', 126)
    	group by UserId) temp
    	where convert(date, main.Written_Test_Date, 126) between convert(date, '" & startOfWeek & "', 126) and convert(date, '" & endOfWeek & "', 126)
    	and main.UserId='" & UserId & "'
    	and main.UserId=temp.UserId
    ) temp
    where UserId='" & UserId & "'
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
        ElseIf myDataSet.Tables(0).Rows.Count = 1 Then
            If myDataSet.Tables(0).Rows(0)(0).ToString().Equals("Appeared") Then
                arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
                arrlist.Add("0")
            Else
                arrlist.Add("0")
                arrlist.Add(myDataSet.Tables(0).Rows(0)(1).ToString())
            End If
        ElseIf myDataSet.Tables(0).Rows.Count = 0 Then
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
	where can.UserId= '" & UserId & "'
	and can.Appearedflag=2
	and convert(date, can.Written_Test_Appear_Date, 126) between convert(date, '" & startOfWeek & "', 126) and convert(date, '" & endOfWeek & "', 126)
) temp 
where cnt =1
group by UserId, UserName, Description
order by UserId, UserName, Description"
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
End Class