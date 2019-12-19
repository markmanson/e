Imports log4net
Imports System.Data.SqlClient
Imports System.Drawing

Partial Public Class WeightSearch
    Inherits BasePage

    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Wieisghtsearch")
    Dim objconn As New ConnectDb
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objCommand As SqlCommand

    Public Sub FillCourses()
        Dim objDataReader1 As SqlDataReader
        Dim query As String = ""
        Try
            ddlCourses.Items.Clear()
            Dim lst1 As New ListItem
            lst1.Text = "--------All--------"
            lst1.Value = 0
            ddlCourses.Items.Add(lst1)
            If objconn.connect() = True Then
                query = "select Course_ID,Course_Name from M_Course where  del_flag='0' Order by Course_Name"
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objDataReader1 = objCommand.ExecuteReader()
                While objDataReader1.Read()
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader1.Item(1)
                    lstItm.Value = objDataReader1.Item(0)
                    ddlCourses.Items.Add(lstItm)
                End While
                objDataReader1.Close()
                objconn.disconnect()
            End If
            '/**********************End**************************/

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objDataReader1 = Nothing
            query = Nothing
        End Try
    End Sub
    Public Sub FillSubjects()
        Dim objDataReader1 As SqlDataReader
        Dim query As String = ""
        Try
            ddlSubjects.Items.Clear()
            Dim lst1 As New ListItem
            lst1.Text = "--------All--------"
            lst1.Value = 0
            ddlSubjects.Items.Add(lst1)
            If objconn.connect() = True Then
                query = "select test_type,test_name,sub_code from M_Testinfo where del_flag=0 order by test_name"
                objCommand = New SqlCommand(query, objconn.MyConnection)
                objDataReader1 = objCommand.ExecuteReader()
                While objDataReader1.Read()
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader1.Item(1)
                    lstItm.Value = objDataReader1.Item(0)
                    ddlSubjects.Items.Add(lstItm)
                End While
                objDataReader1.Close()
                objconn.disconnect()
            End If
            '/**********************End**************************/

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objDataReader1 = Nothing
            query = Nothing
        End Try
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            FillCourses()
            FillSubjects()
            If Not Session("selval") Is Nothing Then
                ViewState.Add("selval", Session("selval"))
                Session.Remove("selval")
            End If
            'Added by Pranit on 06/11/2019
            'DGData.DataSource = BindGrid()
            'DGData.DataBind()
        Else
            ' BindGrid()

            'Commented by Pranit Chimurkar on 2019/10/23
            If DGData.Visible = True Then
                'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
            End If
        End If
        Try
            If Session.Item("FromWm") = "true" Then
                If Request.QueryString("pi") <> Nothing Then
                    DGData.CurrentPageIndex = CInt(Request.QueryString("pi"))
                    BindGrid()
                    Session.Remove("FromWm")
                End If
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

#Region "Page_Unload"
    'Desc: This is page unload event.
    'By: Jatin Gangajaliya, 2011/04/27.

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
    'Added by Pranit Chimurkar on 2019/10/23
    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        'tblCourse.Rows.Clear()
        'Dim mtr As New HtmlTableRow
        'Dim mtd As New HtmlTableCell
        'Dim tr As New HtmlTableRow
        'Dim td1 As New HtmlTableCell
        'Dim td2 As New HtmlTableCell
        'Dim td3 As New HtmlTableCell

        'Dim temptbl As New HtmlTable
        'temptbl.Border = 1

        'td1.InnerHtml = "1"
        'tr.Cells.Add(td1)

        'Dim lbtn As New LinkButton
        'lbtn.Text = ddlCourses.SelectedItem.Text
        'lbtn.ToolTip = ddlCourses.SelectedItem.Text
        'td2.Controls.Add(lbtn)
        'tr.Cells.Add(td2)

        'td3.InnerHtml = "100"
        'tr.Cells.Add(td3)
        'temptbl.Rows.Add(tr)






        'mtd.Controls.Add(temptbl)
        'mtr.Cells.Add(mtd)
        'tblCourse.Rows.Add(mtr)



        'gridDiv.Visible = True

        Try

            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

    Protected Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

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

    'Modified by Pranit on 06/11/2019
    Public Sub BindGrid()
        Dim strquery As String
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String
        Dim myTable As DataTable
        Dim strbr As StringBuilder
        Dim adap As SqlDataAdapter
        Dim col As DataColumn
        '    Dim strsearchdetails As String
        '    Dim strName As String = ""
        '    Dim strBirth As String = ""
        '    Dim strlastname As String = ""
        'Dim strcenterid As String = ""
        Try

            If Session("lastCourse") <> Nothing Then
                ddlCourses.SelectedValue = CInt(Session("lastCourse"))
                Session.Remove("lastCourse")
            End If
            If Session("lastSubject") <> Nothing Then
                ddlSubjects.SelectedValue = CInt(Session("lastSubject"))
                Session.Remove("lastSubject")
            End If


            myTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            strbr = New StringBuilder()
            strbr.Append("select  Weightage_ID as wid , m_course.Course_name as Course_name,s.test_name as Subject_name,Sub_Weightage as sub_Weight,single as single,Multi_choice as multi_choice,blanks as blanks,basic as basic,interMed as intermed, w.del_flag as del_flag,m_course.del_flag as df from M_weightage as w join  m_course on w.course_id= m_course.Course_id join m_testinfo as s on w.test_type = s.test_type where 1=1 ")
            If ddlCourses.SelectedValue <> 0 Then
                'where m_course.Course_Name like 'd%' and s.test_name like 'j%'
                strbr.Append(" and m_course.Course_Name like '" & ddlCourses.SelectedItem.Text & "%' ")
            End If
            If ddlSubjects.SelectedValue <> 0 Then
                'where m_course.Course_Name like 'd%' and s.test_name like 'j%'
                strbr.Append(" and s.test_name like '" & ddlSubjects.SelectedItem.Text & "%' ")
            End If
            'If Not txtcn.Text = String.Empty Then
            '    'where m_course.Course_Name like 'd%' and s.test_name like 'j%'
            '    strbr.Append(" and m_course.Course_Name like '" & txtcn.Text & "%' ")
            'End If
            'If Not txtsn.Text = String.Empty Then
            '    'where m_course.Course_Name like 'd%' and s.test_name like 'j%'
            '    strbr.Append(" and s.test_name like '" & txtsn.Text & "%' ")
            'End If

            '        'old query replaced because Center Name was not present at the grid
            '        'Author Name : Indravadan Vasava
            '        'Modified date : 2011/03/16
            '        '            strbr.Append(" Select u.userid,u.name,u.surname,u.email,u.loginname,u.pwd,convert(char(10),u.birthdate, 103) as BirthDate from M_USER_INFO as u ")
            '        strbr.Append(" SELECT u.Userid, u.Name, u.SurName,u.Delete_Flg, u.Email, u.LoginName, u.Pwd, CONVERT(char(10), u.Birthdate, 103) AS BirthDate, c.Center_Name FROM M_USER_INFO u INNER JOIN M_Centers c ON u.Center_ID = c.Center_ID ")
            '        strbr.Append(" Where User_Type = 0 ")

            '        If Not (ddlcenter.SelectedValue = 0) Then
            '            strcenterid = "u.Center_ID = " & ddlcenter.SelectedValue & " AND "
            '        End If

            '        If Not IsDBNull(txtfn.Text) And txtfn.Text <> "" Then
            '            strName = "u.Name LIKE '%" & Replace(txtfn.Text, "'", "''") & "%' " & " AND "
            '        Else
            '            strName = ""
            '        End If

            '        If Not IsDBNull(txtln.Text) And txtln.Text <> "" Then
            '            strlastname = " u.SurName LIKE '%" & Replace(txtln.Text, "'", "''") & "%' " & " AND "
            '        Else
            '            strlastname = ""
            '        End If

            '        If Not IsDBNull(txtbd.Text) And txtbd.Text <> "" Then
            '            strBirth = " convert(int,Year(u.birthdate)) = " & Convert.ToInt32(txtbd.Text) & " AND "
            '        Else
            '            strBirth = ""
            '        End If

            '        strsearchdetails = strName & strlastname & strBirth
            '        If strcenterid <> "" Then
            '            strsearchdetails = strsearchdetails & strcenterid
            '        End If

            '        If strsearchdetails <> "" Then
            '            strsearchdetails = strsearchdetails.Substring(0, strsearchdetails.Length - 4)
            '        End If


            '        If strsearchdetails <> "" Then
            '            strbr.Append(" AND ")
            '        End If

            strquery = strbr.ToString() & " order by m_course.course_name"

            'Added by Pranit on 06/11/2019
            'Session("Source") = myTable
            'Dim dv As DataView = New DataView(myTable)

            If objconn.connect() Then
                adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                adap.Fill(myTable)

                If (myTable.Rows.Count > 0) Then
                    DGData.Visible = True

                    DGData.DataSource = myTable


                    '                If Session("fromregister") <> Nothing Then
                    '                    If Request.QueryString("pi") <> Nothing Then
                    '                        DGData.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                    '                    End If
                    '                    Session.Remove("fromregister")
                    '                End If
                    'If Not DGData.CurrentPageIndex < DGData.PageCount Then
                    '    DGData.CurrentPageIndex = 0
                    'End If

                    If myTable.Rows.Count <= DGData.CurrentPageIndex * 10 Then
                        DGData.CurrentPageIndex = DGData.CurrentPageIndex - 1
                        ViewState.Add("selval", DGData.CurrentPageIndex)
                        ViewState.Add("pageNo", DGData.CurrentPageIndex + 1)
                    End If

                    Try
                        DGData.DataBind()
                    Catch ex As Exception
                        DGData.CurrentPageIndex = 0
                        DGData.DataBind()
                    End Try

                    'fillPagesCombo()
                    'Commented by Pranit Chimurkar on 2019/10/23
                    'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                    'Making datagrid rows gray coloured for students which are disabled.
                    For i As Integer = 0 To DGData.Items.Count - 1
                        If DGData.Items(i).Cells(13).Text = "True" Then
                            DGData.Items(i).Cells(9).Enabled = True
                            DGData.Items(i).Cells(11).Enabled = False
                            DGData.Items(i).Cells(11).ToolTip = " Disabled"
                            DGData.Items(i).Cells(12).Enabled = False
                            DGData.Items(i).Cells(12).ToolTip = " Disabled"
                            DGData.Items(i).BackColor = Drawing.Color.Gray
                        ElseIf DGData.Items(i).Cells(13).Text = "False" Then
                            DGData.Items(i).Enabled = True
                        End If
                        DGData.Items(i).Cells(12).Attributes.Add("onclick", "return confirm('Are you sure you want to delete this subject Weightage?');")
                    Next

                    lblrecords.Text = Resources.Resource.AdminList_TotRecord & ": " & myTable.Rows.Count
                    gridDiv.Visible = True
                    'imgbtndisable.Visible = True
                    'imgbtnenable.Visible = True
                    errorMsg.Text = String.Empty
                Else
                    gridDiv.Visible = False
                    DGData.Visible = False
                    imgbtndisable.Visible = False
                    imgbtnenable.Visible = False
                    errorMsg.Text = Resources.Resource.Common_NoRecFound
                    errorMsg.Visible = True
                End If
            End If

            'Added by Pranit on 06/11/2019
            'Return dv

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex

        Finally
            objconn.disconnect()
            objconn = Nothing
            myCommand = Nothing
            strquery = Nothing
            strbr = Nothing
            adap = Nothing
            col = Nothing
            strPathDb = Nothing
            '        myTable = Nothing
            '        adap = Nothing
            '        strbr = Nothing
            '        col = Nothing
            '        strquery = Nothing
            '        strPathDb = Nothing
            '        strsearchdetails = Nothing
            '        strName = Nothing
            '        strBirth = Nothing
            '        strlastname = Nothing
            '        strcenterid = Nothing
        End Try

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

    Private Sub DGData_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGData.ItemCommand
        Try
            If (e.CommandName = "Edit") Then
                Dim selWeight As String = e.Item.Cells(1).Text
                Session.Add("wid", selWeight)
                Session.Add("check", "true")
                Session.Add("checkforback", "true")
                Session.Add("lastCourse", ddlCourses.SelectedItem.Value)
                Session.Add("lastSubject", ddlSubjects.SelectedItem.Value)
                Session.Add("selval", DGData.CurrentPageIndex)

                Response.Redirect("WeightageManagement.aspx?pi=" & DGData.CurrentPageIndex, False)
            ElseIf (e.CommandName = "Remove") Then
                Dim objcon As New ConnectDb
                Dim cmd As SqlCommand
                'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
                Try
                    If objcon.connect() = True Then
                        Dim selWeight As String = e.Item.Cells(1).Text
                        Session.Add("wid", selWeight)
                        Dim query As String = "delete from M_weightage where  weightage_id = " & selWeight
                        cmd = New SqlCommand(query, objcon.MyConnection)
                        cmd.ExecuteNonQuery()
                        BindGrid()
                        errorMsg.Text = Resources.Resource.WeightSearch_subrmcrs
                        errorMsg.ForeColor = Drawing.Color.Green
                        errorMsg.Visible = True
                    End If
                Catch ex As Exception
                    If log.IsDebugEnabled Then
                        log.Debug("Error :" & ex.ToString())
                    End If
                    Throw ex
                Finally
                    cmd = Nothing
                    objcon.disconnect()
                End Try
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
    'Added by Pranit Chimurkar on 2019/10/23
    Private Sub btnNewLink_Click(sender As Object, e As EventArgs) Handles btnNewLink.Click
        Try
            If Not Session("wid") Is Nothing Then
                Session.Remove("wid")
            End If
            Response.Redirect("WeightageManagement.aspx", False)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

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

    Private Sub DGData_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DGData.PageIndexChanged
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
    'Added by Pranit Chimurkar on 2019/10/23
    Protected Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("admin.aspx")
    End Sub

    Public Function EnableDisable(ByVal i As Integer, ByVal bool As Boolean)
        Dim chk As CheckBox = Nothing
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Dim strbr As StringBuilder
        Dim Str, strquery As String
        Dim strid As String
        Dim boldecision As Boolean = True
        Try

            strbr = New StringBuilder
            strbr.Append(" ( ")
            For Each rowItem As DataGridItem In DGData.Items
                chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)
                If chk.Checked Then
                    boldecision = False
                    strid = DirectCast(rowItem.Cells(1).Text, String)
                    strbr.Append(strid)
                    strbr.Append(" , ")
                End If
            Next

            If boldecision = True Then
                errorMsg.Visible = True
                If bool = True Then
                    errorMsg.Text = Resources.Resource.WeightSearch_sltrecenable
                ElseIf bool = False Then
                    errorMsg.Text = Resources.Resource.WeightSearch_sltrecdisable
                End If

                Exit Function
            End If

            Str = strbr.ToString
            Str = Str.Substring(0, Str.Length - 2)
            Str = Str & " ) "

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                strbr = New StringBuilder
                strbr.Append(" Update M_Weightage set Del_Flag = ")
                strbr.Append(i)
                strbr.Append(" where M_Weightage.Weightage_ID IN ")
                strbr.Append(Str)
                strquery = strbr.ToString()
                myCommand = New SqlCommand(strquery, objconn.MyConnection)
                myCommand.ExecuteNonQuery()
            End If
            objconn.disconnect()
            Dim intindex As Integer = DGData.CurrentPageIndex
            BindGrid()
            DGData.CurrentPageIndex = intindex

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            chk = Nothing
            myCommand = Nothing
            strPathDb = Nothing
            strid = Nothing
            strquery = Nothing
            Str = Nothing
            boldecision = True
            chk = Nothing
        End Try

    End Function

    'Added by Pranit Chimurkar on 2019/10/23
    Protected Sub imgbtnenable_Click(sender As Object, e As EventArgs) Handles imgbtnenable.Click
        Try
            EnableDisable(0, True)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
    'Added by Pranit Chimurkar on 2019/10/23
    Protected Sub imgbtndisable_Click(sender As Object, e As EventArgs) Handles imgbtndisable.Click
        Try
            EnableDisable(1, False)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
    'Added by Pranit Chimurkar on 2019/10/23
    Protected Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
        Try
            gridDiv.Visible = False
            errorMsg.Text = String.Empty
            errorMsg.Visible = False
            txtcn.Text = ""
            'txtsn.Text = ""
            ddlCourses.SelectedValue = 0
            ddlSubjects.SelectedValue = 0
            DGData.CurrentPageIndex = 0
            ViewState.Remove("selval")
            ViewState.Remove("pageNo")
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub

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
    '                    '   range = CInt(ViewState("lastRange")) + 1
    '                End If
    '                If selPage > DGData.PageCount Then
    '                    range = DGData.PageCount - (len)
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
End Class