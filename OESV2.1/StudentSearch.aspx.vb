#Region "NameSpaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
Imports System.Drawing
Imports System.Web.Services

#End Region

#Region "Class for studentsearch screen"

Partial Public Class StudentSearch

#Region "Declaration"
    Inherits BasePage
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("StudentSearch")
    Dim objconn As New ConnectDb
    Dim strPathDb As String
#End Region

#Region "Page Load Event"
    'Desc: This is pageload event of page.
    'By: Jatin Gangajaliya, 2011/3/8

    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            'txtbd.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            errorMsg.Text = String.Empty



            If DGData.Visible = True Then
                fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
            End If

            If Not Page.IsPostBack Then
                txtfn.Focus()
                BindCenters()
                'Session.Add("fname", txtfn.Text)
                'Session.Add("lname", txtln.Text)
                'Session.Add("cname", ddlcenter.SelectedItem.Value)
                'Session.Add("dob", txtbd.Text)
                If Not Session("cname") Is Nothing Then
                    ddlcenter.SelectedValue = CInt(Session("cname").ToString)
                    Session.Remove("cname")
                End If
                If Not Session("fname") Is Nothing Then
                    txtfn.Text = Session("fname").ToString
                    ViewState.Add("selval", Session("selval"))
                    BindGrid()
                    Session.Remove("fname")
                    Session.Remove("selval")
                End If
                If Not Session("lname") Is Nothing Then
                    '   txtln.Text = Session("lname").ToString
                    Session.Remove("lname")
                End If

                If Not Session("dob") Is Nothing Then
                    'txtbd.Text = Session("dob").ToString
                    Session.Remove("dob")
                End If


                errorMsg.Text = String.Empty
                ' gridDiv.Visible = False
                imgbtndisable.Visible = False
                imgbtnenable.Visible = False

                '  BindGrid()

                If Request.QueryString("pi") <> Nothing Then
                    If DGData.Items.Count > 0 Then
                        gridDiv.Visible = True
                        imgbtndisable.Visible = True
                        imgbtnenable.Visible = True
                    End If
                End If
            Else
                '    BindGrid()
            End If

            If Session.Item("Bool") <> Nothing Then
                errorMsg.Text = Resources.Resource.StudentSearch_recupdated
                errorMsg.ForeColor = Drawing.Color.Green
                Session.Item("Bool") = Nothing
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try

    End Sub
#End Region

#Region "Bind Centers DropdownList"
    'Desc: This method Binds Center DropdownList.
    'By: Jatin Gangajaliya, 2011/3/9

    Public Sub BindCenters()
        Dim myDataReader As SqlDataReader
        Dim sqlstr As String
        Dim myTable As DataTable
        Dim myRow As DataRow
        Try
            sqlstr = ""
            sqlstr = sqlstr & " SELECT Center_ID,Center_Name FROM M_Centers where Del_Flg = 0 order by Center_Name"
            myDataReader = retTestInfo(sqlstr)

            myTable = New DataTable
            myTable.Columns.Add(New DataColumn("Center_ID", GetType(String)))
            myTable.Columns.Add(New DataColumn("Center_Name", GetType(String)))

            'While loop to populate the Datatable
            While myDataReader.Read
                myRow = myTable.NewRow
                myRow(0) = myDataReader.Item("Center_ID")
                myRow(1) = myDataReader.Item("Center_Name")
                myTable.Rows.Add(myRow)
            End While
            ddlcenter.DataSource = myTable
            ddlcenter.DataTextField = "Center_Name"
            ddlcenter.DataValueField = "Center_ID"

            ddlcenter.DataBind()
            ddlcenter.Items.Insert(0, New ListItem("--Select--", "0"))
            myDataReader.Close()

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            If objconn.connect() Then
                objconn.disconnect()
            End If
            sqlstr = Nothing
            myDataReader = Nothing
            myTable = Nothing
            myRow = Nothing
        End Try
    End Sub
#End Region

#Region "Function RetTestinfo"

    Private Function retTestInfo(ByVal sqlstr As String) As SqlDataReader
        Dim objconn As New ConnectDb
        Try
            Dim myCommand As SqlCommand

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

#Region "Event for Checking and unchecking all checkboxes"
    'Desc: This is event for checking and unchecking all checkboxes.
    'By: Jatin Gangajaliya, 2011/3/9

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

#Region "Back Button Click Event"
    'Desc: This is back button Click Event.
    'By: Jatin Gangajaliay,2011/3/9

    'Protected Sub btnBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnBack.Click
    '    Session.Remove("userid")
    '    Response.Redirect("admin.aspx", False)
    'End Sub
#End Region

#Region "Clear Button Click Event"
    'Desc: This is Clear button Click Event.
    'By: Jatin Gangajaliay,2011/3/9

    Protected Sub btnclear_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnclear.Click
        Try
            txtfn.Text = String.Empty
            ' txtln.Text = String.Empty
            'txtbd.Text = String.Empty
            ddlcenter.SelectedIndex = 0
            DGData.Visible = False
            errorMsg.Text = String.Empty
            txtfn.Focus()
            gridDiv.Visible = False
            lblrecords.Text = String.Empty
            imgbtndisable.Visible = False
            imgbtnenable.Visible = False
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

#Region "Search Button Click Event"
    'Desc: This is search button click event.
    'By: Jatin Gangajaliaya, 2011/3/9

    Protected Sub BtnSearch_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles BtnSearch.Click
        Try
            DGData.CurrentPageIndex = 0
            BindGrid()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try

    End Sub
#End Region

#Region "DataGrid PageIndex Change Event"
    'Desc: This is DataGrid PageIndex Change Event.
    'By: Jatin Gangajaliaya, 2011/3/9

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

#Region "BindGrid Method"
    'Desc: This is Bindgrid method for datagridview binding.
    'By: Jatin Gangajaliya

    Public Sub BindGrid()
        Dim strquery As String
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strPathDb As String
        Dim myTable As DataTable
        Dim strbr As StringBuilder
        Dim adap As SqlDataAdapter
        Dim col As DataColumn
        Dim strsearchdetails As String
        Dim strName As String = ""
        Dim strBirth As String = ""
        Dim strlastname As String = ""
        Dim strcenterid As String = ""
        Try

            myTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            strbr = New StringBuilder()
            'old query replaced because Center Name was not present at the grid
            'Author Name : Indravadan Vasava
            'Modified date : 2011/03/16
            '            strbr.Append(" Select u.userid,u.name,u.surname,u.email,u.loginname,u.pwd,convert(char(10),u.birthdate, 103) as BirthDate from M_USER_INFO as u ")

            'Commented & Added By Vaibhav Soni
            'Date : 2014/03/14
            'changed INNER JOIN to LEFT JOIN
            'strbr.Append(" Select u.Userid,(SurName)+' '+name+' '+isnull(Middlename,'') as Name,u.Delete_Flg, u.Email,u.RollNo, u.LoginName, u.Pwd, CONVERT(char(10), u.Birthdate, 103) AS BirthDate, c.Center_Name FROM M_USER_INFO u INNER JOIN M_Centers c ON u.Center_ID = c.Center_ID ")
            strbr.Append(" Select u.Userid,(SurName)+' '+name+' '+isnull(Middlename,'') as Name,u.Delete_Flg, u.Email,u.RollNo, u.LoginName, u.Pwd, CONVERT(char(10), u.Birthdate, 103) AS BirthDate, c.Center_Name FROM M_USER_INFO u LEFT JOIN M_Centers c ON u.Center_ID = c.Center_ID ")
            strbr.Append(" Where User_Type = 0 ")
            'Ended By Vaibhav Soni

            If Not (ddlcenter.SelectedValue = 0) Then
                strcenterid = " u.Center_ID = " & ddlcenter.SelectedValue & " AND "
            End If

            If Not IsDBNull(txtfn.Text) And txtfn.Text <> "" Then
                strName = " ( (SurName)+' '+name+' '+isnull(Middlename,'') LIKE '%" & Replace(txtfn.Text, "'", "''") & "%' or " & "  name+' '+(SurName)+' '+isnull(Middlename,'') LIKE '%" & Replace(txtfn.Text, "'", "''") & "%') " & " AND"
                '' strName = " ( u.Name LIKE '%" & Replace(txtfn.Text, "'", "''") & "%' " & " OR "

            Else
                strName = ""
            End If

            ''If Not IsDBNull(txtfn.Text) And txtfn.Text <> "" Then
            ''    strlastname = " u.SurName LIKE '%" & Replace(txtfn.Text, "'", "''") & "%' " & " Or "
            ''Else
            ''    strlastname = ""
            ''End If

            ''If Not IsDBNull(txtfn.Text) And txtfn.Text <> "" Then
            ''    strBirth = " u.Middlename LIKE '%" & Replace(txtfn.Text, "'", "''") & "%' " & " ) "
            ''Else
            ''    strBirth = ""
            ''End If
            'If Not IsDBNull(txtbd.Text) And txtbd.Text <> "" Then
            '    strBirth = " convert(int,Year(u.birthdate)) = " & Convert.ToInt32(txtbd.Text) & " AND "
            'Else
            '    strBirth = ""
            'End If

            strsearchdetails = strName & strlastname & strBirth
            If strcenterid <> "" Then
                strsearchdetails = strsearchdetails & strcenterid
            End If

            If strsearchdetails <> "" Then
                strsearchdetails = strsearchdetails.Substring(0, strsearchdetails.Length - 4)
                '' strsearchdetails = strsearchdetails & " ) "
            End If


            If strsearchdetails <> "" Then
                strbr.Append(" AND ")
            End If

            strquery = strbr.ToString() & strsearchdetails & "order by u.Name"

            If objconn.connect() Then
                adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                adap.Fill(myTable)

                If (myTable.Rows.Count > 0) Then
                    DGData.Visible = True
                    DGData.DataSource = myTable


                    If Session("fromregister") <> Nothing Then
                        If Request.QueryString("pi") <> Nothing Then
                            DGData.CurrentPageIndex = CInt(Request.QueryString("pi").ToString())
                        End If
                        Session.Remove("fromregister")
                    End If
                    Try
                        DGData.DataBind()
                    Catch ex As Exception
                        DGData.CurrentPageIndex = 0
                        DGData.DataBind()
                    End Try
                    'fillPagesCombo()
                    fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
                    'Making datagrid rows gray coloured for students which are disabled.
                    For i As Integer = 0 To DGData.Items.Count - 1
                        If DGData.Items(i).Cells(7).Text = "True" Then
                            DGData.Items(i).Cells(9).Enabled = True
                            DGData.Items(i).Cells(8).Enabled = False
                            DGData.Items(i).Cells(8).ToolTip = "Disabled"
                            DGData.Items(i).BackColor = Drawing.Color.Gray
                        ElseIf DGData.Items(i).Cells(7).Text = "False" Then
                            DGData.Items(i).Enabled = True
                        End If
                    Next

                    lblrecords.Text = " " & myTable.Rows.Count
                    gridDiv.Visible = True
                    imgbtndisable.Visible = True
                    imgbtnenable.Visible = True
                    imgbtndelete.Visible = True
                    errorMsg.Text = String.Empty
                Else
                    gridDiv.Visible = False
                    DGData.Visible = False
                    imgbtndisable.Visible = False
                    imgbtnenable.Visible = False
                    imgbtndelete.Visible = False
                    errorMsg.ForeColor = Color.Red
                    errorMsg.Text = Resources.Resource.Common_NoRecFound
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
            strquery = Nothing
            strPathDb = Nothing
            strsearchdetails = Nothing
            strName = Nothing
            strBirth = Nothing
            strlastname = Nothing
            strcenterid = Nothing
        End Try

    End Sub
#End Region

    'Added by Pranit on 05/12/2019
#Region "AutoComplete Search"
    <WebMethod(EnableSession:=True)>
    Public Shared Function GetStudentList() As ArrayList
        Dim myDataReader As SqlDataReader
        Dim myCommand As SqlCommand
        Dim myDataSet As DataSet
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlstr As String
        Dim conn As SqlConnection
        Dim arrlist As New ArrayList
        conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        conn.Open()

        sqlstr = " Select distinct (SurName)+' '+name+' '+isnull(Middlename,'') from M_USER_INFO Where User_Type = 0 "
        myDataSet = New DataSet()
        myDataAdapter = New SqlDataAdapter(sqlstr, conn)
        myDataAdapter.Fill(myDataSet)

        If myDataSet.Tables(0).Rows.Count >= 1 Then
            For count As Integer = 0 To (myDataSet.Tables(0).Rows.Count - 1)
                arrlist.Add(myDataSet.Tables(0).Rows(count)(0).ToString())
            Next
        End If
        conn.Close()
        Return arrlist
    End Function
#End Region

#Region "DataGrid ItemCommand Event"
    'Desc: This is DataGrid ItemCommand Event.
    'By: Jatin Gangajaliya, 2011/3/9

    Protected Sub DGData_ItemCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles DGData.ItemCommand
        Try
            If (e.CommandName = "Details") Then
                Dim strcandidateid As String = e.Item.Cells(1).Text
                Session.Add("userid", strcandidateid)
                Session.Add("check", "true")
                Session.Add("checkforback", "true")
                Session.Add("tostudentlist", "true")
                Session.Remove("toadminlist")

                Session.Add("fname", txtfn.Text)
                '  Session.Add("lname", txtln.Text)
                Session.Add("cname", ddlcenter.SelectedItem.Value)
                'Session.Add("selval", ddlPages.SelectedItem.Value)
                '   Session.Add("dob", txtbd.Text)

                Response.Redirect("register.aspx?pi=" & DGData.CurrentPageIndex, False)
            End If

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
#End Region

#Region "DataGrid ItemData Bound Event"
    'Desc: This is datagrid item data bound event.
    'By: Jatin Gangajaliya, 2011/3/10

    Protected Sub DGData_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DGData.ItemDataBound
        Try
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try
    End Sub
#End Region

#Region "chkRemove_CheckedChanged1"
    Protected Sub chkRemove_CheckedChanged1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim chk As CheckBox = Nothing
        Dim bool As Boolean = True
        Try
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

#Region "Enable button Click Event"
    'Desc: This is Enable button Click Event.
    'By: Jatin Gangajaliya, 2011/03/17

    Protected Sub imgbtnenable_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgbtnenable.Click
        Try
            EnableDisable(0, True)
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)

        End Try
    End Sub
#End Region

#Region "Disable button Click Event"
    'Desc: This is Disable button Click Event.
    'By: Jatin Gangajaliya, 2011/03/17

    Protected Sub imgbtndisable_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgbtndisable.Click
        Try
            EnableDisable(1, False)

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)

        End Try
    End Sub
#End Region

#Region "Enable Disable Function"
    'Desc: This is method to enable and disable candidates.
    'By: Jatin Gangajaliya, 2011/03/16

    Public Function EnableDisable(ByVal i As Integer, ByVal bool As Boolean)
        Dim chk As CheckBox = Nothing
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
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
                    errorMsg.Text = Resources.Resource.StudentSearch_sltonecand
                ElseIf bool = False Then
                    errorMsg.Text = Resources.Resource.StudentSearch_sltonedisable
                End If

                Exit Function
            End If

            Str = strbr.ToString
            Str = Str.Substring(0, Str.Length - 2)
            Str = Str & " ) "

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                strbr = New StringBuilder
                strbr.Append(" Update M_USER_INFO set Delete_Flg = ")
                strbr.Append(i)
                strbr.Append(" where M_USER_INFO.Userid IN ")
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
    '    'If Not Session("selval") Is Nothing Then
    '    '    ddlPages.SelectedValue = CInt(Session("selval"))
    '    'End If
    '    DGData.CurrentPageIndex = ddlPages.SelectedItem.Value
    '    ViewState.Add("selval", ddlPages.SelectedItem.Value)
    '    ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
    '    BindGrid()
    'End Sub

    Protected Sub imgbtndelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles imgbtndelete.Click
        Dim chk As CheckBox = Nothing
        Dim myCommand As SqlCommand
        Dim objconn As New ConnectDb
        'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
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
                errorMsg.Text = Resources.Resource.StudentSearch_sltoneDelete
                Exit Sub

            End If
            Str = strbr.ToString
            Str = Str.Substring(0, Str.Length - 2)
            Str = Str & " ) "
            Dim boolchk As Boolean = CheckStatus(Str) ' Added by Pragnesha Kulkarni on 25/07/18 for checking student exam status
            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                If boolchk = True Then  ' Added by Pragnesha Kulkarni on 25/07/18 for checking student exam status
                    strbr = New StringBuilder
                    strbr.Append(" delete M_USER_INFO ")
                    strbr.Append(" where M_USER_INFO.Userid IN ")
                    strbr.Append(Str)
                    strquery = strbr.ToString()
                    myCommand = New SqlCommand(strquery, objconn.MyConnection)
                    myCommand.ExecuteNonQuery()
                    errorMsg.Visible = True
                    errorMsg.Text = Resources.Resource.StudentSearch_sndelsuc
                    errorMsg.ForeColor = Drawing.Color.Green

                    'Added By: Pragnesha Kulkarni 
                    'Date: 25/07/18
                    'Description: This function checks the status of course before deleting a student name.
                    '--------------------------------------------------------------------------------------
                Else
                    errorMsg.Visible = True
                    errorMsg.Text = Resources.Resource.StudentSearch_delerr
                    Exit Sub
                    '--------------------------------------------------------------------------------------
                End If
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
        End Try
    End Sub
#Region "Function checkstatus"

    'Added By   : Pragnesha Kulkarni
    'Date         : 25/07/18
    'Description: This function checks the status of course before deleting a student name.

    Private Function CheckStatus(ByVal userid As String) As Boolean
        Dim strq As String
        Dim strbr As StringBuilder
        Dim MyCommand As SqlCommand
        Dim bol As Boolean = True
        Dim intcount As Integer
        Try
            strbr = New StringBuilder
            strbr.Append("select count(*) from T_Result where userid=  ")
            strbr.Append(userid)
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
        End Try
    End Function
#End Region
End Class
#End Region