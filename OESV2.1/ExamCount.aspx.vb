#Region "Name Spaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Web.Security
Imports log4net
Imports System.Drawing

#End Region

#Region "Class ExamCount"
Partial Public Class ExamCount
    Inherits BasePage
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("ExamCount")


#Region "Page Load Event"
    'Desc: This is PageLoad Event for this page.
    'By: Jatin Gangajaliya , 2011/2/7

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (Page.IsPostBack) Then
            Try

                lblMsg.Text = ""
                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                '  If Session("UniUserType").ToString <> "1" Then ' commented by pragnesha for super admin
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

                'Commented by Pranit Chimurkar on 2019/10/23 ( Temporary Solution )
                'If Not (Session("FromPage") = "Admin" Or Session("FromPage") = "Maint") Then
                '    Response.Redirect("login.aspx", False)
                '    Response.End()
                'End If

                If Not IsPostBack Then
                    sel_subjectid.Visible = True
                    'searchbtn.Enabled = False
                    'ddlmainCourse.SelectedValue = 0
                    searchbtn.ToolTip = Resources.Resource.ExamCount_SelMainCoFir
                    BindCourse()

                    If Not Session("ECourse") Is Nothing Then

                        ddlcourse.SelectedValue = CInt(Session("ECourse"))
                        Session.Remove("ECourse")
                        BindSubject()
                        If Not Session("Etest") Is Nothing Then
                            sel_subjectid.SelectedValue = CInt(Session("Etest").ToString())
                            Session.Remove("Etest")
                        End If
                        BindData()
                    End If

                    'BindCourse()
                    'lbltext.Visible = False
                Else
                End If
                Session("FromPage") = "Maint"
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx")

            End Try
        Else
            '     BindData()
            If DataGrid2.Visible = True Then
                If DataGrid2.Visible = True Then
                    'fillPageNumbers(DataGrid2.CurrentPageIndex + 1, 9)
                End If

            End If
        End If
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
            strbr = New StringBuilder
            strbr.Append(" Select distinct t_result.test_type ,mti.test_name  ")
            strbr.Append(" from t_result  ")
            strbr.Append(" LEFT JOIN m_testinfo as mti ")
            strbr.Append(" on mti.test_type=t_result.test_type ")
            strbr.Append(" where course_id = ")
            strbr.Append("'")
            strbr.Append(ddlcourse.SelectedValue)
            strbr.Append("'")
            strbr.Append(" order by mti.test_name ")
            sqlstr = strbr.ToString

            myDataReader = retTestInfo(sqlstr)

            myTable = New DataTable
            myTable.Columns.Add(New DataColumn("subjectID", GetType(String)))
            myTable.Columns.Add(New DataColumn("subjectName", GetType(String)))

            'Dim strPathDb As String
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            ' While loop to populate the Datatable
            If objconn.connect() Then
                While myDataReader.Read
                    myRow = myTable.NewRow
                    myRow(0) = myDataReader.Item("test_type")
                    myRow(1) = myDataReader.Item("test_name")
                    myTable.Rows.Add(myRow)
                End While
            End If
            sel_subjectid.DataSource = myTable
            sel_subjectid.DataTextField = "subjectName"
            sel_subjectid.DataValueField = "subjectID"
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

#Region "DataGrid PageIndexChange Event"

    Private Sub DataGrid2_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DataGrid2.ItemCommand
        If (e.CommandName = "view") Then
            Dim strTesttype As String = e.Item.Cells(4).Text
            Dim strQno As String = e.Item.Cells(3).Text
            Dim strQuestion As String = e.Item.Cells(1).Text

            Session.Add("EQuestion", strQuestion)
            Session.Add("Etesttype", strTesttype)
            Session.Add("Eqno", strQno)

            Session.Add("ECourse", ddlcourse.SelectedItem.Value)
            Session.Add("Etest", sel_subjectid.SelectedItem.Value)


            Response.Redirect("UserQuestion.aspx?pi=" & DataGrid2.CurrentPageIndex, False)
        End If


    End Sub
    'Desc: This is DataGrid PageIndexChange Event.
    'By: Jatin Gangajaliya , 2011/2/7

    Protected Sub DataGrid2_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid2.PageIndexChanged
        Try
            DataGrid2.CurrentPageIndex = e.NewPageIndex
            BindData()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

#End Region

#Region "BindData SubRoutine"
    'Desc: This is BindData SubRoutine to Bind the DataGrid.
    'By: Jatin Gangajaliya , 2011/2/7

    Private Sub BindData()
        Dim strquery As String
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String
        Dim myTable As DataTable
        Dim strbr As StringBuilder
        Dim adap As SqlDataAdapter
        Dim col As DataColumn
        Dim intcount As Integer
        Try

            myTable = New DataTable
            myTable.Columns.Add(New DataColumn("question", GetType(String)))
            myTable.Columns.Add(New DataColumn("Publish", GetType(String)))
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")

            strbr = New StringBuilder()
            strbr.Append(" Select Count(Distinct userid) as Count From t_result where course_id = ")
            strbr.Append(ddlcourse.SelectedValue)
            strquery = strbr.ToString()
            If objconn.connect() Then
                myCommand = New SqlCommand(strquery, objconn.MyConnection)
                intcount = myCommand.ExecuteScalar()
                lblcount.Visible = True
                'lbltext.Visible = True
                If (intcount > 0) Then
                    'lbltext.Text = "" '"Total Exam Conducted : "
                    lblcount.Text = Resources.Resource.ExamCount_TtlXamCon & intcount.ToString
                ElseIf (intcount <= 0) Then
                    DataGrid2.Visible = False
                    gridDiv.Visible = False
                End If

            End If

            If (intcount > 0) Then
                strbr.Append("  ")
                strbr = New StringBuilder()

                strbr.Append(" select Distinct mq.qnid,mq.question,mq.test_type,isnull(ress.fre_count,0)as fre_count from m_question as mq ")
                strbr.Append(" left join( ")
                strbr.Append(" select count(tr.qno) as fre_count ,tr.qno,tr.test_type ")
                strbr.Append(" from t_result as tr ")
                strbr.Append(" LEFT join m_question as mq ")
                strbr.Append(" on mq.qnid=tr.qno ")
                strbr.Append(" and mq.test_type=tr.test_type ")
                strbr.Append("where tr.course_id=")
strbr.Append(ddlcourse.SelectedValue)
                strbr.Append(" group by tr.qno,tr.test_type ")
                strbr.Append(" )as ress ")
                strbr.Append(" on ress.qno=mq.qnid ")
                strbr.Append(" and mq.test_type=ress.test_type ")
                strbr.Append(" LEFT JOIN t_result as tr ")
                strbr.Append(" on mq.qnid=tr.qno ")
                strbr.Append(" and mq.test_type=tr.test_type ")
                strbr.Append(" where mq.test_type ")

                If sel_subjectid.SelectedIndex > 0 Then
                    strbr.Append(" = ")
                    strbr.Append(sel_subjectid.SelectedValue)
                End If

                If sel_subjectid.SelectedIndex = 0 Then
                    strbr.Append(" in (Select distinct t_result.test_type ")
                    strbr.Append(" from t_result ")
                    strbr.Append(" LEFT JOIN m_testinfo as mti ")
                    strbr.Append(" on mti.test_type=t_result.test_type ")
                    strbr.Append(" where course_id =  ")
                    strbr.Append(ddlcourse.SelectedValue)
                    strbr.Append(" ) ")
                End If
               strbr.Append(" order by fre_count desc ")

                strquery = strbr.ToString()
                Session("CrsName")=ddlcourse.SelectedValue
                If objconn.connect() Then
                    adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                    adap.Fill(myTable)
                    DataGrid2.Visible = True
                    gridDiv.Visible = True
                    lblrecords.Text = Resources.Resource.ExamCount_TtlRcrds + myTable.Rows.Count.ToString
                    DataGrid2.DataSource = myTable

                    If Not Session("Epi") Is Nothing Then
                        DataGrid2.CurrentPageIndex = CInt(Session("Epi"))
                        '   Request.QueryString.Remove("pi")
                        Session.Remove("Epi")
                    End If
                    DataGrid2.DataBind()
                    ViewState.Add("selval", DataGrid2.CurrentPageIndex)
                    fillPagesCombo()
                    'fillPageNumbers(DataGrid2.CurrentPageIndex + 1, 9)

                    For i As Integer = 0 To DataGrid2.Items.Count - 1
                        If DataGrid2.Items(i).Cells(2).Text = "0" Then
                            DataGrid2.Items(i).Cells(5).Enabled = False
                            DataGrid2.Items(i).Cells(5).ToolTip = "Disabled"
                            ' DataGrid2.Items(i).BackColor = Drawing.Color.Gray
                        Else

                        End If
                    Next


                    DataGrid2.Visible = True
                End If
            Else
                lblMsg.Visible = True
                lblMsg.Text = Resources.Resource.Common_NoRecFound
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
            strquery = Nothing
            strPathDb = Nothing
        End Try
    End Sub

#End Region

#Region "DropDownList SelectedIndexChanged Event"
    'Desc: This is DropDownList SelectedIndexChanged Event.
    'By: Jatin Gangajaliya , 2011/2/7

    Protected Sub sel_subjectid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sel_subjectid.SelectedIndexChanged
        'Try
        '    DataGrid2.CurrentPageIndex = 0
        'Catch ex As Exception
        '    If log.IsDebugEnabled Then
        '        log.Debug("Error :" & ex.ToString())
        '    End If
        '    Response.Redirect("error.aspx")
        'End Try

    End Sub

#End Region

#Region "Search Button Click Event"
    'Desc: This is Search Button click event.
    'By: Jatin Gangajaliya , 2011/2/7
    'Added by Pranit Chimurkar on 2019/10/18
    Protected Sub searchbtn_Click(sender As Object, e As EventArgs) Handles searchbtn.Click
        Try
            If ddlcourse.SelectedIndex <= 0 Then
                lblMsg.Text = Resources.Resource.Search_PlSelCoName
                lblcount.Text = String.Empty
                'lbltext.Visible = False
                DataGrid2.Visible = False
                gridDiv.Visible = False
                Exit Sub
            End If

            'If ddlcourse.SelectedIndex > 0 Then
            '    If sel_subjectid.SelectedIndex = 0 Then
            '        lblMsg.Text = "Please select any Subject name"
            '        lblcount.Text = String.Empty
            '        lbltext.Visible = False
            '        DataGrid2.Visible = False
            '    End If

            'If sel_subjectid.SelectedIndex > 0 Then
            'lbltext.Visible = True
            lblMsg.Text = String.Empty
            DataGrid2.CurrentPageIndex = 0
            BindData()
            'End If
            'End If


        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally
        End Try
    End Sub
#End Region

#Region "Back Button ClickEvent"
    'Added by Pranit Chimurkar on 2019/10/18
    'Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
    '    Response.Redirect("admin.aspx")
    'End Sub
#End Region


#Region "Course DropdownList Selected index Change"
    'Desc: This is Course DropdownList Selected index Change event.
    'By: Jatin Gangajaliya, 2011/2/22

    Protected Sub ddlcourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlcourse.SelectedIndexChanged

        BindSubject()
    End Sub
#End Region

    Public Sub BindSubject()
        lblMsg.Text = String.Empty
        'lbltext.Visible = False
        lblcount.Text = String.Empty

        Try
            If (ddlcourse.SelectedIndex > 0) Then
                sel_subjectid.Enabled = True
                searchbtn.Enabled = True
                searchbtn.ToolTip = ""
                populate_subjectid()
            ElseIf (ddlcourse.SelectedIndex = 0) Then
                If (sel_subjectid.Items.Count > 0) Then
                    sel_subjectid.SelectedValue = 0
                End If
                'searchbtn.Enabled = False
                searchbtn.ToolTip = Resources.Resource.ExamCount_SelCoNameFir
                sel_subjectid.Enabled = False
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")

        End Try
    End Sub
#Region "Bind Course DropdownList"
    'Desc: This method Binds Course DropdownList.
    'By: Jatin Gangajaliya, 2011/2/22

    Public Sub BindCourse()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Try
            sqlstr = ""

            
            'sqlstr = sqlstr & " select distinct tcs.course_id,mc.course_name from t_candidate_status as tcs LEFT JOIN m_course as mc  on mc.course_id=tcs.course_id where tcs.Appearedflag=2 and mc.Del_Flag=0"

            'Changed by : Sarasawti Patel
            'Description: Change For Select only enable course name
            sqlstr = sqlstr & " select distinct tcs.course_id,mc.course_name from t_candidate_status as tcs LEFT JOIN m_course as mc  on mc.course_id=tcs.course_id where tcs.Appearedflag=2 and mc.Del_Flag=0 order by Course_name"
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
        End Try

    End Sub
#End Region

#Region "Datagrid itembound event"
    'Desc: This is datagrid itembound eent
    'By: Jatin Gangajaliya, 2011/3/12

    Protected Sub DataGrid2_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid2.ItemDataBound
        If Not e.Item.ItemType = DataControlRowType.Header Then
            e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
        End If
    End Sub
#End Region

#Region "Main Course Name SelectedIndex Change Event"
    'Desc: This is Main Course Name SelectedIndex Change Event.
    'By: Jatin Gangajaliya, 2011/3/4

    'Protected Sub ddlmainCourse_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlmainCourse.SelectedIndexChanged
    '    lblMsg.Text = String.Empty
    '    lbltext.Visible = False
    '    lblcount.Text = String.Empty

    '    Try
    '        If (ddlmainCourse.SelectedIndex > 0) Then
    '            ddlcourse.Enabled = True
    '            searchbtn.Enabled = True
    '            searchbtn.ToolTip = ""
    '            BindCourse()

    '        ElseIf (ddlmainCourse.SelectedIndex = 0) Then
    '            If (ddlcourse.Items.Count > 0) Then
    '                ddlcourse.SelectedValue = 0
    '                sel_subjectid.SelectedValue = 0
    '            End If
    '            'searchbtn.Enabled = False
    '            searchbtn.ToolTip = "Select the Main Course First"
    '            ddlcourse.Enabled = False
    '            sel_subjectid.Enabled = False

    '        End If
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx")
    '    End Try
    'End Sub
#End Region

    '#Region "Method For Binding MainCourse Name"
    '    'Desc: This method binds maincourse Dropdownlist
    '    'By: Jatin Gangajaliya,2011/3/4

    '    Public Sub BindMainCourse()
    '        Dim myDataReader As SqlDataReader
    '        Dim sqlstr As String
    '        Dim myTable As DataTable
    '        Dim myRow As DataRow

    '        Try
    '            sqlstr = ""
    '            sqlstr = sqlstr & " SELECT * FROM M_Main_Course order by Main_Course_Name"
    '            myDataReader = retTestInfo(sqlstr)

    '            myTable = New DataTable
    '            myTable.Columns.Add(New DataColumn("Main_Course_ID", GetType(String)))
    '            myTable.Columns.Add(New DataColumn("Main_Course_Name", GetType(String)))

    '            While myDataReader.Read
    '                myRow = myTable.NewRow
    '                myRow(0) = myDataReader.Item("Main_Course_ID")
    '                myRow(1) = myDataReader.Item("Main_Course_Name")
    '                myTable.Rows.Add(myRow)
    '            End While
    '            ddlmainCourse.DataSource = myTable
    '            ddlmainCourse.DataTextField = "Main_Course_Name"
    '            ddlmainCourse.DataValueField = "Main_Course_ID"
    '            ddlmainCourse.DataBind()
    '            ddlmainCourse.Items.Insert(0, New ListItem("--Select--", "0"))
    '            myDataReader.Close()

    '        Catch ex As Exception
    '            If log.IsDebugEnabled Then
    '                log.Debug("Error :" & ex.ToString())
    '            End If
    '            Throw ex
    '        Finally
    '            sqlstr = Nothing
    '            myDataReader = Nothing
    '            myTable = Nothing
    '            myRow = Nothing
    '        End Try

    '    End Sub
    '#End Region

#Region "Clear Button Click"
    'Desc: This method Clears all Selection
    'By: AP,2011/3/9
    'Added by Pranit Chimurkar on 2019/10/18
    Protected Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        Try
            ddlcourse.SelectedIndex = 0
            sel_subjectid.SelectedIndex = 0
            sel_subjectid.Enabled = False
            lblMsg.Text = ""
            DataGrid2.Visible = False
            gridDiv.Visible = False
            lblcount.Text = ""
            'lbltext.Text = ""
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


    '    If len > DataGrid2.PageCount Then
    '        len = DataGrid2.PageCount - 1
    '    End If

    '    ' if search clicked again then page 1 should be selected 
    '    If DataGrid2.CurrentPageIndex = 0 Then
    '        ViewState("pageNo") = 1
    '        ViewState("lastRange") = 1
    '    End If

    '    ' Getting the currently selected page value 
    '    Dim selPage As Integer = 0
    '    If (ViewState("pageNo") <> Nothing) Then
    '        selPage = CInt(ViewState("pageNo"))
    '    Else
    '        ' selPage = 1
    '        selPage = DataGrid2.CurrentPageIndex + 1
    '    End If

    '    If (ViewState("lastRange") <> Nothing) Then

    '        '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DataGrid2.PageCount Then
    '        If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
    '            range = CInt(ViewState("lastRange"))
    '        Else
    '            'If it is the last page then resetting the page numbers
    '            ' last number - provided length
    '            'If (len + selPage) >= DataGrid2.PageCount Then
    '            '    If selPage <= len Then
    '            '        range = range
    '            '    Else
    '            '        range = DataGrid2.PageCount - len
    '            '        'Incase range becomes 0 or less than zero than setting it 1 
    '            '        If range <= 0 Then
    '            '            range = 1
    '            '        End If
    '            '    End If

    '            'Else
    '            If selPage <= DataGrid2.PageCount Then
    '                'range = range
    '                If range < CInt(ViewState("lastRange")) Then
    '                    If (CInt(ViewState("lastRange")) - 1) > selPage Then
    '                        range = selPage
    '                    Else
    '                        range = CInt(ViewState("lastRange")) - 1
    '                    End If

    '                    'range = CInt(ViewState("lastRange")) - 1
    '                Else
    '                    'range = CInt(ViewState("lastRange")) + 1
    '                    If selPage - len > 0 And selPage - len <= DataGrid2.PageCount - len Then
    '                        range = selPage - len
    '                    Else
    '                        range = CInt(ViewState("lastRange")) + 1
    '                    End If
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
    '    'If selPage = 1 And selPage = DataGrid2.PageCount - 1 Then
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
    '    If selPage = DataGrid2.PageCount Then
    '        imgnext.Enabled = False
    '        imglast.Enabled = False
    '    End If
    'End Sub
    Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGrid2.CurrentPageIndex < (DataGrid2.PageCount - 1)) Then
                    DataGrid2.CurrentPageIndex += 1

                End If

            Case "prev" 'The prev button was clicked
                If (DataGrid2.CurrentPageIndex > 0) Then
                    DataGrid2.CurrentPageIndex -= 1
                End If

            Case "last" 'The Last Page button was clicked
                DataGrid2.CurrentPageIndex = (DataGrid2.PageCount - 1)

            Case Else 'The First Page button was clicked
                DataGrid2.CurrentPageIndex = Convert.ToInt32(arg)
        End Select
        ViewState.Add("pageNo", DataGrid2.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGrid2.CurrentPageIndex)
        BindData()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
        'used by external paging UI
        Dim arg As String = sender.CommandArgument

        Select Case arg
            Case "next" 'The next Button was Clicked
                If (DataGrid2.CurrentPageIndex < (DataGrid2.PageCount - 1)) Then
                    DataGrid2.CurrentPageIndex += 1
                    '    ViewState.Add("selval", DataGrid2.CurrentPageIndex)
                End If

            Case "prev" 'The prev button was clicked
                If (DataGrid2.CurrentPageIndex > 0) Then
                    DataGrid2.CurrentPageIndex -= 1
                    '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                End If

            Case "last" 'The Last Page button was clicked
                DataGrid2.CurrentPageIndex = (DataGrid2.PageCount - 1)
                'ViewState.Add("selval", ddlPages.SelectedItem.Value)
            Case Else 'The First Page button was clicked
                DataGrid2.CurrentPageIndex = Convert.ToInt32(arg) - 1
                ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
        End Select

        ViewState.Add("pageNo", DataGrid2.CurrentPageIndex + 1)
        ViewState.Add("selval", DataGrid2.CurrentPageIndex)
        BindData()
        'Now, bind the data!
        '   BindSQL()
    End Sub

    Public Sub fillPagesCombo()
        ddlPages.Items.Clear()
        For cnt As Integer = 1 To DataGrid2.PageCount
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
        DataGrid2.CurrentPageIndex = ddlPages.SelectedItem.Value
        ViewState.Add("selval", ddlPages.SelectedItem.Value)
        ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
        BindData()
    End Sub
End Class
#End Region