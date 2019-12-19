
#Region "NameSpaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
Imports System.Drawing
Imports System.Web.Script.Services
Imports System.Web.Services

#End Region

#Region "Class Admin List"
'Desc: This is adminlist class
'By: Jatin Gangajaliya,2011/3/12

Partial Public Class AdminList

#Region "Declaration"
    Inherits BasePage
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("StudentSearch")
    Dim objconn As New ConnectDb
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim flagSort As Boolean = False
#End Region

#Region "Page Load Event"
    'Desc: This is pageload event of page.
    'By: Jatin Gangajaliya, 2011/3/8

    Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            'txtby.Attributes.Add("onkeypress", "event.returnValue=numbersonly(this, event, 'true')")
            'If DGData.Visible = True Then
            'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)
            'End If


            If Not Page.IsPostBack Then
                txtfn.Focus()
                'Session.Add("fname", txtfn.Text)
                'Session.Add("lname", txtln.Text)
                'Session.Add("yob", txtby.Text)
                If Not Session("fname") Is Nothing Then
                    txtfn.Text = Session("fname").ToString
                    BindGrid()
                    Session.Remove("fname")
                End If
                'If Not Session("lname") Is Nothing Then
                '    txtln.Text = Session("lname").ToString
                '    Session.Remove("lname")
                'End If
                'If Not Session("yob") Is Nothing Then
                '    'txtby.Text = Session("yob").ToString
                '    Session.Remove("yob")
                'End If

                '  BindGrid()
                errorMsg.Text = String.Empty
                '  gridDiv.Visible = False
                'imgbtndisable.Visible = False
                'imgbtnenable.Visible = False
                If Request.QueryString("ip") <> Nothing Then
                    If DGData.Items.Count > 0 Then
                        gridDiv.Visible = True
                        imgbtndisable.Visible = True
                        imgbtnenable.Visible = True
                    End If
                End If
            Else
                'Added by Pranit on 05/11/2019
                'If flagSort = True Then
                '    DGData.DataSource = BindGrid()
                '    DGData.DataBind()
                'End If
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
    'Commented by Pranit Chimurkar on 2019/10/16
    'Protected Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
    '    Response.Redirect("admin.aspx", False)
    'End Sub
#End Region

#Region "Clear Button Click Event"
    'Desc: This is Clear button Click Event.
    'By: Jatin Gangajaliay,2011/3/9

    'Added by Pranit Chimurkar on 2019/10/16
    'Protected Sub btnclear_Click(sender As Object, e As EventArgs) Handles btnclear.Click
    '    'End
    '    Try
    '        txtfn.Text = String.Empty
    '        'txtln.Text = String.Empty
    '        'txtby.Text = String.Empty
    '        'ddlcenter.SelectedIndex = 0
    '        DGData.Visible = False
    '        errorMsg.Text = String.Empty
    '        txtfn.Focus()
    '        'BtnRemove.Visible = False
    '        gridDiv.Visible = False
    '        lblrecords.Text = String.Empty
    '        imgbtndisable.Visible = False
    '        imgbtnenable.Visible = False
    '        ViewState.Remove("selval")
    '        ViewState.Remove("pageNo")
    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx", False)
    '    End Try
    'End Sub
#End Region


#Region "Search Button Click Event"
    'Desc: This is search button click event.
    'By: Jatin Gangajaliaya, 2011/3/9

    'Added By Pranit Chimurkar on 2019/10/16
    Protected Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        'End for ImageButton to Button
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


#Region "Remove Button Click Event"
    'Desc: This is remove button click event.
    'By: Jatin Gangajaliya, 2011/3/9

    'Protected Sub BtnRemove_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles BtnRemove.Click
    '    Dim chk As CheckBox = Nothing
    '    Dim MyCommand As SqlCommand
    '    Dim objconn As New ConnectDb
    '    Dim bolFlag As Boolean
    '    Dim strid, strquery, str As String
    '    Dim strbr As StringBuilder
    '    Static Dim boldecision As Boolean = False

    '    Try
    '        strbr = New StringBuilder
    '        strbr.Append(" ( ")

    '        For Each rowItem As DataGridItem In DGData.Items
    '            chk = DirectCast((rowItem.Cells(0).FindControl("chkRemove")), CheckBox)

    '            If chk.Checked Then
    '                boldecision = True
    '                strid = DirectCast(rowItem.Cells(1).Text, String)
    '                strbr.Append(strid)
    '                strbr.Append(" , ")
    '            End If

    '        Next

    '        If Not (boldecision) Then
    '            errorMsg.Visible = True
    '            errorMsg.Text = "Please select atleast one candidate for deletion"
    '            Exit Sub
    '        End If

    '        str = strbr.ToString
    '        str = str.Substring(0, str.Length - 2)
    '        str = str & " ) "

    '        strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '        If objconn.connect(strPathDb) Then
    '            strbr = New StringBuilder
    '            strbr.Append(" Update M_USER_INFO set Delete_Flg = 1 where M_USER_INFO.Userid IN ")
    '            strbr.Append(str)
    '            strquery = strbr.ToString()

    '            MyCommand = New SqlCommand(strquery, objconn.MyConnection)
    '            MyCommand.ExecuteNonQuery()
    '            MyCommand.Dispose()
    '            DGData.CurrentPageIndex = 0
    '            BindGrid()
    '            errorMsg.Text = "Record(s) Deleted Successfully"
    '            errorMsg.ForeColor = Drawing.Color.Green
    '            errorMsg.Visible = True
    '        End If
    '        objconn.disconnect()

    '    Catch ex As Exception
    '        If log.IsDebugEnabled Then
    '            log.Debug("Error :" & ex.ToString())
    '        End If
    '        Response.Redirect("error.aspx", False)
    '    Finally
    '        chk = Nothing
    '        objconn = Nothing
    '        strid = Nothing
    '        strbr = Nothing
    '        strbr = Nothing
    '        str = Nothing
    '        boldecision = False
    '    End Try
    'End Sub
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

    'Added by Pranit on 04/12/2019
#Region "AutoComplete Search"
    <WebMethod(EnableSession:=True)>
    Public Shared Function GetAdminList() As ArrayList
        Dim myDataReader As SqlDataReader
        Dim myCommand As SqlCommand
        Dim myDataSet As DataSet
        Dim myDataAdapter As SqlDataAdapter
        Dim sqlstr As String
        Dim conn As SqlConnection
        Dim arrlist As New ArrayList
        conn = New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        conn.Open()

        sqlstr = " Select (SurName)+' '+name+' '+isnull(Middlename,'') from M_USER_INFO Where User_Type = 1 "
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

#Region "BindGrid Method"
    'Desc: This is Bindgrid method for datagridview binding.
    'By: Jatin Gangajaliya

    'Modified by Pranit on 05/11/2019
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
        'Dim strcenterid As String = ""
        Try

            myTable = New DataTable
            col = New DataColumn("SrNo")
            col.AutoIncrement = True
            col.AutoIncrementSeed = 1
            col.AutoIncrementStep = 1
            myTable.Columns.Add(col)

            strPathDb = ConfigurationSettings.AppSettings("PathDb")
            strbr = New StringBuilder()
            strbr.Append(" Select u.userid,(SurName)+' '+name+' '+isnull(Middlename,'') as Name,u.surname,u.Delete_Flg,u.email,u.loginname,u.pwd,convert(char(10),u.birthdate, 103) as BirthDate from M_USER_INFO as u ")
            strbr.Append(" Where User_Type = 1 ")
            'strbr.Append("and delete_flg = 0 ")
            'If Not (ddlcenter.SelectedValue = 0) Then
            '    strcenterid = "Center_ID = " & ddlcenter.SelectedValue & " AND "
            'End If

            If Not IsDBNull(txtfn.Text) And txtfn.Text <> "" Then
                strName = "( (SurName)+' '+name+' '+isnull(Middlename,'') LIKE '%" & Replace(txtfn.Text, "'", "''") & "%' " & " or  name+' '+(SurName)+' '+isnull(Middlename,'') LIKE '%" & Replace(txtfn.Text, "'", "''") & "%') " & " AND "
            Else
                strName = ""
            End If

            'If Not IsDBNull(txtln.Text) And txtln.Text <> "" Then
            '    strlastname = " u.SurName LIKE '%" & Replace(txtln.Text, "'", "''") & "%' " & " AND "
            'Else
            '    strlastname = ""
            'End If

            'If Not IsDBNull(txtby.Text) And txtby.Text <> "" Then
            '    strBirth = " convert(int,Year(u.birthdate)) = " & Convert.ToInt32(txtby.Text) & " AND "
            'Else
            '    strBirth = ""
            'End If

            strsearchdetails = strName & strlastname & strBirth
            'If strcenterid <> "" Then
            '    strsearchdetails = strsearchdetails & strcenterid
            'End If

            If strsearchdetails <> "" Then
                strsearchdetails = strsearchdetails.Substring(0, strsearchdetails.Length - 4)
            End If


            If strsearchdetails <> "" Then
                strbr.Append(" AND ")
            End If

            strquery = strbr.ToString() & strsearchdetails & "order by u.Name"

            'Added by Pranit on 05/11/2019
            'Session("Source") = myTable
            'Dim dv As DataView = New DataView(myTable)

            If objconn.connect() Then
                adap = New SqlDataAdapter(strquery, objconn.MyConnection)
                adap.Fill(myTable)

                If (myTable.Rows.Count > 0) Then
                    DGData.Visible = True
                    DGData.DataSource = myTable

                    If Session("toadminlist") = "true" Then
                        If Request.QueryString("ip") <> Nothing Then
                            DGData.CurrentPageIndex = CInt(Request.QueryString("ip").ToString())
                        End If
                        Session.Remove("toadminlist")
                    End If
                    DGData.DataBind()
                    'fillPagesCombo()
                    'fillPageNumbers(DGData.CurrentPageIndex + 1, 9)

                    lblRecords.Text = " " & myTable.Rows.Count
                    gridDiv.Visible = True
                    errorMsg.Text = String.Empty
                    imgbtndisable.Visible = True
                    imgbtnenable.Visible = True
                    imgbtndelete.Visible = True
                    'Making datagrid rows gray coloured for admins which are disabled.
                    For i As Integer = 0 To DGData.Items.Count - 1
                        If DGData.Items(i).Cells(6).Text = "True" Then
                            DGData.Items(i).Cells(8).Enabled = True
                            DGData.Items(i).Cells(7).Enabled = False
                            DGData.Items(i).Cells(7).ToolTip = "Disabled"
                            DGData.Items(i).BackColor = Drawing.Color.Gray
                        ElseIf DGData.Items(i).Cells(6).Text = "False" Then
                            DGData.Items(i).Enabled = True
                        End If
                    Next

                Else
                    gridDiv.Visible = False
                    DGData.Visible = False
                    imgbtndisable.Visible = False
                    imgbtnenable.Visible = False
                    imgbtndelete.Visible = False
                    errorMsg.Text = Resources.Resource.Common_NoRecFound
                End If
            End If
            'Added by Pranit on 05/11/2019
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
        End Try

    End Sub
#End Region

    'Added by Pranit on 05/11/2019
#Region "Sorting"
    'Sub Sort_Grid(sender As Object, e As DataGridSortCommandEventArgs)
    '    Dim dt As DataTable = CType(Session("Source"), DataTable)
    '    Dim dv As DataView = New DataView(dt)
    '    dv.Sort = e.SortExpression
    '    DGData.DataSource = dv
    '    DGData.DataBind()
    'End Sub 'Sort_Grid
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
                Session.Add("check2", "true")

                Session.Add("fname", txtfn.Text)
                Session.Add("toadminlist", "true")
                Session.Remove("tostudentlist")
                'Session.Add("lname", txtln.Text)
                'Session.Add("yob", txtby.Text)

                Response.Redirect("register.aspx?ip=" & DGData.CurrentPageIndex, False)
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


#Region "Enable Disable Function"
    'Desc: This is method to enable and disable candidates.
    'By: Jatin Gangajaliya, 2011/03/16

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
                    errorMsg.Text = Resources.Resource.CourseRegistration_sltoneade
                ElseIf bool = False Then
                    errorMsg.Text = Resources.Resource.CourseRegistration_sltadDis
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
            strquery = Nothing
            Str = Nothing
            boldecision = True
            objconn = Nothing
            strbr = Nothing
        End Try

    End Function
#End Region


#Region "Enable button click event"
    'Desc: This is enable button click event.
    'By: Jatin Gangajaliya, 2011/03/21.
    'Added by Pranit Chimurkar on 2019/10/16
    Protected Sub imgbtnenable_Click(sender As Object, e As EventArgs) Handles imgbtnenable.Click
        'End
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


#Region "Disable button click event"
    'Desc: This is disable button click event.
    'By: Jatin Gangajaliya, 2011/03/21.
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
#End Region

    'Commented by Pranit Chimurkar on 05/11/2019
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
    '            ''If (len + selPage) >= DGData.PageCount Then
    '            ''    If selPage <= len Then
    '            ''        range = range
    '            ''    Else
    '            ''        range = DGData.PageCount - len
    '            ''        'Incase range becomes 0 or less than zero than setting it 1 
    '            ''        If range <= 0 Then
    '            ''            range = 1
    '            ''        End If
    '            ''    End If

    '            ''Else
    '            If selPage <= DGData.PageCount Then
    '                'range = range
    '                If range < CInt(ViewState("lastRange")) Then
    '                    If (CInt(ViewState("lastRange")) - 1) > selPage Then
    '                        range = selPage
    '                    Else
    '                        range = CInt(ViewState("lastRange")) - 1
    '                    End If

    '                Else
    '                    If selPage - len > 0 And selPage - len <= DGData.PageCount - len Then
    '                        range = selPage - len
    '                    Else
    '                        range = CInt(ViewState("lastRange")) + 1
    '                    End If
    '                    '   range = CInt(ViewState("lastRange")) + 1
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

    'Added by Pranit on 26/11/2019
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

    Protected Sub DGData_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DGData.SelectedIndexChanged

    End Sub

    Protected Sub imgbtndelete_Click(sender As Object, e As EventArgs) Handles imgbtndelete.Click
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
                errorMsg.Text = Resources.Resource.AdminList_SelDel
                Exit Sub
            End If

            Str = strbr.ToString
            Str = Str.Substring(0, Str.Length - 2)
            Str = Str & " ) "

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            If objconn.connect() Then
                strbr = New StringBuilder
                strbr.Append(" delete M_USER_INFO ")
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
            strquery = Nothing
            Str = Nothing
            boldecision = True
            objconn = Nothing
            strbr = Nothing
        End Try
    End Sub
End Class
#End Region
