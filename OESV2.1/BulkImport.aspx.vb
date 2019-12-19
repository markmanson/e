'This file is Added by sandeep sharma for bulk data import from csv file.
#Region "Namespace"
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Security.Permissions
Imports System.Web
Imports System.Web.Hosting
Imports System.Web.Mail
Imports System.IO
Imports log4net
Imports System.Drawing

Imports System.Linq
Imports Ionic.Zip

#End Region
Namespace unirecruite
    Partial Class WebForm3
        Inherits BasePage
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(("WebForm3"))


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

#Region "Variables"
        Dim objdataset1 As New DataSet
        Dim sqlcon, sqlstr, campusvalue As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim con, con1, con2 As SqlConnection
        Dim cmd As SqlCommand
        Dim userid, campusid As String
        Dim dr As SqlDataReader
        Dim objda As SqlDataAdapter
        Dim flag As Boolean = False
        Dim mytable1, dtCheckQue As DataTable
        Dim col1, col2 As DataColumn
        Dim duplicate As Integer = 0
        Dim inserted As Integer = 0
        Dim rows As DataRow
        Dim total As Integer = 0
        Dim connStr As String
        Dim adapOleDb As OleDbDataAdapter
        Dim adapSql As New SqlDataAdapter
        Dim dsOledb As New DataSet
        Dim strTemp, query As String
        Dim intTempRow, intTempCol, intFirstNameRow, intFirstNameCol As Integer
        Dim adapSqlCheckQue As SqlDataAdapter
        Dim cnStr As String
        Dim sqlTrans As SqlTransaction

        'By:Jatin Gangajaliya, 2011/04/27.
        Dim objconn As New ConnectDb
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
#End Region

#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            'for validating the csv file
            If Not IsPostBack Then
                Getit()
            Else
                ' If DataGrid1.Visible = True Then
                'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)
                'End If

            End If

            Try
                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                '  If Session("UniUserType").ToString <> "1" Then ' commented by pragnesha for super admin
                If Convert.ToString(Session("UniUserType")) > "2" Then
                    Response.Redirect("~\register.aspx", False)
                End If
                lblmessage.Text = ""
                'btnImport.Attributes.Add("OnClick", "return Validate()")
                If Not IsPostBack Then
                    tblrow2.Visible = False
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
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "Open Connection"
        Public Sub openconnection()
            Try
                If con.State = ConnectionState.Closed Then
                    con.Open()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

#Region "Close connection"
        Public Sub closeconnection()
            Try
                If con.State = ConnectionState.Open Then
                    con.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

        'This is for generating the user id on the basis of maximum user id got from the m_user_info table
        'Added by sandeep sharma
#Region "Get user ID"
        Public Sub generateuserid()
            Try
                sqlcon = ConfigurationSettings.AppSettings("PathDb")
                con1 = New SqlConnection(sqlcon)
                sqlstr = "select max(userid) from m_user_info "
                If con1.State = ConnectionState.Closed Then
                    con1.Open()
                End If
                cmd = New SqlCommand(sqlstr, con1)
                dr = cmd.ExecuteReader()
                While dr.Read()
                    'If dr.Item(0) = 0 Then
                    If dr.IsDBNull(0) = True Then
                        userid = 1
                    Else
                        userid = dr.Item(0) + 1
                    End If
                End While
                If con1.State = ConnectionState.Open Then
                    con1.Close()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                con1 = Nothing
                sqlcon = Nothing
                sqlstr = Nothing
                cmd = Nothing
            End Try
        End Sub
#End Region

        'This function is for generating the random password
        'Added by sandeep sharma
#Region "Genetare Random Password"
        Public Function GetRandomPasswordUsingGUID() As String
            'Get the GUID

            Dim guidResult As String = System.Guid.NewGuid().ToString()
            'Remove the hyphens
            Try
                guidResult = guidResult.Replace("-", String.Empty)
                'Make sure length is valid
                'Return the first length bytes
                Return guidResult.Substring(0, 10)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                guidResult = Nothing
            End Try
        End Function
#End Region

#Region "DropDown Index Changed"
        Protected Sub DropDownList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim drop As DropDownList = CType(sender, DropDownList)
            Dim dd As New DataTable
            Dim strRowFilter As String
            Try
                Session("value") = drop.SelectedValue
                dd = Session("DuplicatedData")
                If drop.SelectedItem.Value = 0 Then
                    dd.DefaultView.RowFilter = "criteria = 'Inserted Records' or criteria = 'Duplicate Records'"
                    dd.DefaultView.Sort = "name"
                    Session("dd") = dd
                    flag = True
                    DataGrid1.DataSource = dd.DefaultView
                    DataGrid1.CurrentPageIndex = 0
                    DataGrid1.DataBind()
                    Response.Write(strRowFilter)
                    dd.Dispose()
                Else
                    strRowFilter = dd.Columns("criteria").ColumnName & " = '" & drop.SelectedItem.Text & "'"
                    dd.DefaultView.RowFilter = strRowFilter
                    dd.DefaultView.Sort = "name"
                    Session("dd") = dd
                    DataGrid1.DataSource = dd.DefaultView
                    DataGrid1.CurrentPageIndex = 0
                    DataGrid1.DataBind()
                    dd.Dispose()
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                drop = Nothing
                strRowFilter = Nothing
                dd = Nothing
            End Try
        End Sub
#End Region

#Region "DataGrid Page Change"
        Private Sub DataGrid1_PageIndexChanged(ByVal source As Object,
             ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles DataGrid1.PageIndexChanged

            Dim ds As New DataTable
            Try
                DataGrid1.CurrentPageIndex = e.NewPageIndex
                If flag = False Then
                    ds = Session("DuplicatedData")
                ElseIf flag = True Then
                    ds = Session("dd")
                End If
                DataGrid1.DataSource = ds
                DataGrid1.DataBind()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                ds = Nothing
            End Try
        End Sub
#End Region

#Region "Button Detail"
        'Private Sub btnDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetails.Click
        '    'Try
        '    '    tbl_feedback.Rows(2).Visible = True
        '    '    DataGrid1.Visible = True
        '    'Catch ex As Exception
        '    '    If log.IsDebugEnabled Then
        '    '        log.Debug("Error :" & ex.ToString)
        '    '    End If
        '    'End Try
        'End Sub
#End Region

#Region "Back Button"
        'Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        '    'Try
        '    '    Response.Redirect("admin.aspx")
        '    'Catch ex As Exception
        '    '    If log.IsDebugEnabled Then
        '    '        log.Debug("Error :" & ex.ToString)
        '    '    End If
        '    'End Try
        'End Sub
#End Region

#Region "Trim Space"
        ' [ID] <TrimSpace>
        ' [Func] <this function removes the whitespace from the strig>
        ' [Note] <Excel Template "Unirecruite Bulk Data Import Format" is inside the folder>
        ' [Date] 2009/03/12 by Vathsal Ravi
        Public Function TrimSpace(ByVal StrScr As String)
            Dim strTemp As String = ""
            Try
                For Each ch As Char In StrScr
                    If ch <> " " Then
                        strTemp += ch
                    End If
                Next
                Return strTemp
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                strTemp = Nothing
            End Try
        End Function
#End Region

        Protected Function RetrunDate()
            Dim strNow As DateTime
            strNow = DateTime.Now
            Dim strdate As String
            Try
                strdate = strNow.Year.ToString + "_" + strNow.Month.ToString + "_" + strNow.Day.ToString + "_" + strNow.Hour.ToString + "_" + strNow.Minute.ToString + "_" + strNow.Second.ToString + "_" + strNow.Millisecond.ToString
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                strNow = Nothing
            End Try
            Return strdate
        End Function

#Region "Import Button"
        ' [ID] <BtnImpData_Click>
        ' [Func] <This Btn gets excel file from user and enter the data into Sql>
        ' [Note] <Excel Template "Unirecruite Bulk Data Import Format" is inside the folder>
        ' [Date] 2009/03/12 by Vathsal Ravi
        'Protected Sub BtnImpData_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnImpData.Click
        '    '         Dim msg, pwd, strSexCheck, strFName, strFileName, strMname, strCampID, strLName, strSex, filename, strEmail, strDob As String
        '    '         Dim objDictionary As New Dictionary(Of String, String)
        '    '         Dim objStreamReader As StreamReader
        '    '         Try
        '    '             mytable1 = New DataTable
        '    '             mytable1.Columns.Add(New DataColumn("Name", GetType(String)))
        '    '             mytable1.Columns.Add(New DataColumn("criteria", GetType(String)))
        '    '             sqlcon = ConfigurationSettings.AppSettings("PathDb")
        '    '             con = New SqlConnection(sqlcon)
        '    '             openconnection()
        '    '             strTemp = MyFile.PostedFile.FileName
        '    '             If strTemp = "" Then
        '    '                 lblmessage.Visible = True
        '    '                 lblmessage.Text = unirecruite.Errconstants.FILEIMPORT
        '    '                 lblmessage.ForeColor = Color.Red
        '    '                 Exit Sub
        '    '             End If
        '    '             strFileName = MyFile.FileName
        '    '             strFileName = RetrunDate() + "_" + strFileName

        '    '             If (strTemp.Substring(strTemp.LastIndexOf(".") + 1, _
        '    '             strTemp.Length - strTemp.LastIndexOf(".") - 1) <> "xls") Then
        '    '                 'lblmessage.Text = Constant.FILEIMPORT
        '    '                 lblmessage.ForeColor = Color.Red
        '    '                 Exit Sub
        '    '             End If
        '    '             If strTemp = "" Then
        '    '                 'lblmessage.Text = Constant.FILEIMPORT
        '    '                 lblmessage.ForeColor = Color.Red
        '    '                 Exit Sub
        '    '             End If
        '    '             MyFile.SaveAs(Server.MapPath("Excel Import\") + strFileName)
        '    '             strTemp = Server.MapPath("Excel Import\" + strFileName)
        '    '             connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
        '    '             "Data Source=" + strTemp + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""
        '    '             adapOleDb = New OleDbDataAdapter("SELECT * FROM [BulkData$]", connStr)
        '    '             adapOleDb.TableMappings.Add("Table", "Excel")
        '    '             adapOleDb.Fill(dsOledb)
        '    '             While intTempRow <= dsOledb.Tables(0).Rows.Count - 1
        '    '                 For intTempCol = 0 To dsOledb.Tables(0).Columns.Count - 1
        '    '                     strTemp = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
        '    '                     'If strTemp.ToUpper = "CAMPUS ID (COLGNAME+YEAR)" Then
        '    '                     If TrimSpace(strTemp.ToUpper) = "CAMPUSID" Then
        '    '                         intFirstNameRow = intTempRow
        '    '                         intFirstNameCol = intTempCol
        '    '                         Exit While
        '    '                     End If
        '    '                 Next
        '    '                 intTempRow = intTempRow + 1
        '    '             End While
        '    '             intTempRow = intTempRow + 1
        '    '             While intTempRow < dsOledb.Tables(0).Rows.Count
        '    '                 generateuserid()
        '    '                 sqlstr = ""
        '    '                 strFName = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString)
        '    '                 Dim loginname As String = strFName & userid.ToString()
        '    '                 dtCheckQue = New DataTable
        '    '                 strMname = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString)
        '    '                 strLName = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 3).ToString)
        '    '                 strSex = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 4).ToString)
        '    '                 strDob = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString
        '    '                 strEmail = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 6).ToString
        '    '                 strCampID = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
        '    '                 strSexCheck = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 4).ToString

        '    '                 pwd = GetRandomPasswordUsingGUID.ToString()
        '    '                 If strFName = "" And strLName = "" And strMname = "" Then
        '    '                     Exit While
        '    '                 End If
        '    '                 objDictionary.Clear()
        '    '                 If strCampID <> String.Empty Then
        '    '                     objDictionary.Add("campus_id", strCampID)
        '    '                 End If
        '    '                 If strFName <> String.Empty Then
        '    '                     objDictionary.Add("name", strFName)
        '    '                 End If
        '    '                 If strMname <> String.Empty Then
        '    '                     objDictionary.Add("middlename", strMname)
        '    '                 End If
        '    '                 If strLName <> String.Empty Then
        '    '                     objDictionary.Add("surname", strLName)
        '    '                 End If
        '    '                 If strSexCheck <> String.Empty Then
        '    '                     objDictionary.Add("sex", strSexCheck)
        '    '                 End If
        '    '                 If strDob <> String.Empty Then
        '    '                     objDictionary.Add("birthdate", strDob)
        '    '                 End If
        '    '                 If strEmail <> String.Empty Then
        '    '                     objDictionary.Add("email", strEmail)
        '    '                 End If
        '    '                 sqlstr = GetSelectQuery(objDictionary)
        '    '                 'sqlstr = "select name from m_user_info where name ='" + strFName + _
        '    '                 '"' and middlename='" + strMname + "'  and surname='" + strLName + "' and sex='" + _
        '    '                 'strSex + "' and birthdate='" + strDob + "' and email='" + strEmail + "'"
        '    '                 cmd = New SqlCommand(sqlstr, con)
        '    '                 adapSqlCheckQue = New SqlDataAdapter(cmd)
        '    '                 adapSqlCheckQue.Fill(dtCheckQue)
        '    '                 If dtCheckQue.Rows.Count <> 0 Then
        '    '                     rows = mytable1.NewRow
        '    '                     rows(0) = strFName + " " + strMname + " " + strLName
        '    '                     rows(1) = "Duplicate Records"
        '    '                     mytable1.Rows.Add(rows)
        '    '                     duplicate += 1
        '    '                 Else
        '    '                     rows = mytable1.NewRow
        '    '                     rows(0) = strFName + " " + strMname + " " + strLName
        '    '                     rows(1) = "Inserted Records"
        '    '                     inserted += 1
        '    '                     mytable1.Rows.Add(rows)

        '    '                     objDictionary.Add("userid", userid)
        '    '                     objDictionary.Add("loginname", loginname)
        '    '                     objDictionary.Add("pwd", pwd)
        '    '                     objDictionary.Add("user_type", "0")
        '    '                     sqlstr = GetInsertQuery(objDictionary)
        '    '                     ' sqlstr = "insert into m_user_info (campus_id,userid,loginname,pwd,name,middlename,surname,sex,birthdate,email,user_type) values ('" _
        '    '                     ' & strCampID & "','" & userid & "','" & loginname & "','" & _
        '    '                     ' GetRandomPasswordUsingGUID.ToString() & "','" & strFName & "','" & strMname & "','" & strLName & "','" & _
        '    '                     'strSex & "','" & strDob & "','" & strEmail & "',0" & ")"
        '    '                     cmd = New SqlCommand(sqlstr, con)
        '    '                     cmd.ExecuteNonQuery()
        '    '                     'Send Mail Begin
        '    '                     Dim mail As New MailMessage
        '    '                     Dim strEmaiId As String
        '    '                     strEmaiId = ConfigurationSettings.AppSettings("AdminEmailID")
        '    '                     mail.From = strEmaiId
        '    '                     mail.To = strEmail
        '    '                     mail.Cc = strEmaiId
        '    '                     mail.Subject = "Registration Detail for Unikaihatsu Online Examination"
        '    '                     filename = Server.MapPath("~\mailtemplate\RegistrationMail.txt")
        '    '                     objStreamReader = File.OpenText(filename)
        '    '                     msg = objStreamReader.ReadToEnd
        '    '                     msg = Replace(msg, "&#Name#&", strFName)
        '    '                     msg = Replace(msg, "&#login#&", loginname)
        '    '                     msg = Replace(msg, "&#pass#&", pwd)
        '    '                     'sendMail(myDataReader.Item("email_address"), CInt(userid))
        '    '                     mail.Body = msg
        '    '                     mail.BodyFormat = MailFormat.Text
        '    '                     Dim strSmtpServer As String
        '    '                     strSmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
        '    '                     '*************Added code for Server Authentication
        '    '                     mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
        '    '                     mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
        '    '                     mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
        '    '                     SmtpMail.SmtpServer = strSmtpServer
        '    '' added by Naved 2010/06/24 Send in Try and Catch
        '    '                     Try 
        '    '	SmtpMail.Send(mail)
        '    'Catch Ex As Exception
        '    '	lblmessage.Text = "Data might have been imported, but few or all mails might have not been sent.<BR>"
        '    'End Try

        '    '                     'send mail ends
        '    '                     End If
        '    '                     intTempRow = intTempRow + 1
        '    '                     dtCheckQue = Nothing
        '    '             End While
        '    '             btnDetails.Enabled = True
        '    '             closeconnection()
        '    '             tbl_feedback.Rows(2).Visible = True
        '    '             lblDuplicate.Text = "Total No. Of Duplicate Records Are:" & duplicate
        '    '             lblInserted.Text = "Total No. Of Inserted Records Are:" & inserted
        '    '             lblTotal.Text = "Total No. Of Records Are:" & duplicate + inserted
        '    '             lblTotal.Visible = True
        '    '             lblDuplicate.Visible = True
        '    '             lblInserted.Visible = True
        '    '             lblSummary.Visible = True
        '    '             Session("DuplicatedData") = mytable1
        '    '             DataGrid1.DataSource = mytable1.DefaultView
        '    '             DataGrid1.DataBind()
        '    '             labelmessage()
        '    '         Catch ex As OleDbException
        '    '             lblmessage.Text = "Either the file is not in the prescribed Format or" & vbCrLf & "It is a protected file"

        '    '         Catch ex As Exception
        '    '             If log.IsDebugEnabled Then
        '    '                 log.Debug("Error :" & ex.ToString)
        '    '             End If
        '    '             'lblmessage.Text = "The File was not imported!! Please contact the Technical Person"
        '    '             'lblmessage.Text = Constant.ERRFILEIMPORT '& "<BR>" & ex.ToString
        '    '         Finally
        '    '             closeconnection()
        '    '             con = Nothing
        '    '             adapOleDb = Nothing
        '    '             cmd = Nothing
        '    '             sqlstr = Nothing
        '    '             mytable1 = Nothing
        '    '             dtCheckQue = Nothing
        '    '             adapOleDb = Nothing
        '    '             adapSql = Nothing
        '    '             strTemp = Nothing
        '    '             connStr = Nothing
        '    '             sqlcon = Nothing
        '    '             msg = Nothing
        '    '             pwd = Nothing
        '    '             strFName = Nothing
        '    '             strMname = Nothing
        '    '             strLName = Nothing
        '    '             strSex = Nothing
        '    '             filename = Nothing
        '    '             strEmail = Nothing
        '    '             strDob = Nothing
        '    '             objStreamReader = Nothing
        '    '             objDictionary.Clear()
        '    '             objDictionary = Nothing

        '    '         End Try
        'End Sub
#End Region

#Region "Display "

        Public Sub labelmessage()
            Try
                If duplicate + inserted = 0 Then
                    lblmessage.Text = "The File Is Already Imported."
                    lblmessage.ForeColor = Color.Red
                    DataGrid1.Visible = False
                    Exit Sub
                End If
                If duplicate + inserted = Convert.ToString(Convert.ToInt16(duplicate)) Then
                    lblmessage.Text = "The File Is Already Imported."
                    lblmessage.ForeColor = Color.Red
                    DataGrid1.Visible = False
                Else
                    Dim str As String
                    str = lblmessage.Text
                    lblmessage.Text = "Data Imported Sucessfully."
                    lblmessage.ForeColor = Color.Green
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

#Region "GetInsertQuery"
        Protected Function GetInsertQuery(ByVal htInsertFieldValue As Dictionary(Of String, String)) As String
            Dim sbInsertQuery As New StringBuilder()
            Try
                'Insert Query Field Name
                sbInsertQuery.Append("Insert into")
                sbInsertQuery.Append(" ")
                sbInsertQuery.Append("m_user_info")
                sbInsertQuery.Append(" (")

                'Insert Query Value
                Dim sbValue As New StringBuilder()
                sbValue.Append(" values ")
                sbValue.Append("(")

                Dim intHashSize As Integer = htInsertFieldValue.Count

                For Each val As KeyValuePair(Of String, String) In htInsertFieldValue
                    Dim strVal As String = val.Value
                    If strVal.Contains("'") Then
                        strVal = strVal.Replace("'", "''")
                    End If

                    sbInsertQuery.Append(val.Key)
                    sbInsertQuery.Append(",")
                    sbValue.Append("'")
                    If val.Key = "birthdate" Then
                        sbValue.Append(ConvertDate(val.Value))
                    Else
                        sbValue.Append(strVal)
                    End If

                    sbValue.Append("'")

                    sbValue.Append(",")
                Next
                'Remove last comma
                sbInsertQuery.Remove(sbInsertQuery.Length - 1, 1)
                sbValue.Remove(sbValue.Length - 1, 1)
                sbInsertQuery.Append(")")
                sbValue.Append(")")

                sbInsertQuery.Append(sbValue.ToString())
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
            Return sbInsertQuery.ToString()

        End Function
#End Region

#Region "Get Select Query"
        Public Function GetSelectQuery(ByVal htSelectFieldValue As Dictionary(Of String, String)) As String
            Dim sbQuery As New StringBuilder("select name from ")
            Try
                sbQuery.Append("m_user_info")
                sbQuery.Append(" ")
                sbQuery.Append("WHERE")
                sbQuery.Append(" ")
                For Each val As KeyValuePair(Of String, String) In htSelectFieldValue
                    If val.Key = "birthdate" Then
                        sbQuery.Append(" convert(varchar(10), " & val.Key & ",103) ")
                        sbQuery.Append("=")
                        sbQuery.Append("'")
                        sbQuery.Append(val.Value)
                        sbQuery.Append("'")
                    Else
                        sbQuery.Append(val.Key)
                        sbQuery.Append("=")
                        sbQuery.Append("'")
                        sbQuery.Append(val.Value)
                        sbQuery.Append("'")
                    End If


                    If htSelectFieldValue.Count > 1 Then
                        sbQuery.Append(" AND ")
                    End If
                Next
                If htSelectFieldValue.Count > 1 Then
                    sbQuery.Remove(sbQuery.Length - 4, 3)
                End If
                'Adding Order By
                'sbQuery.Append(Me.GetOrderBy())
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
            Return sbQuery.ToString()
        End Function
#End Region



        ' [ID] <imgBtnImpData_Click>
        ' [Func] <This Btn gets excel file from user and enter the data into Sql>
        ' [Note] <Excel Template "Unirecruite Bulk Data Import Format" is inside the folder>
        '        The buttons are changed to ImageButton , so the code changes are done accordingly.
        ' [Date] 2011/03/03 by Indarvadan Vasava
        Protected Sub imgBtnImpData_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgBtnImpData.Click
            Dim strRollno, msg, pwd, strSexCheck, strFName, strFileName, strMname, strCampID, strLName, strSex, filename, strEmail, strDob, strCenter, strCourse As String
            Dim objDictionary As New Dictionary(Of String, String)
            Dim objStreamReader As StreamReader
            Dim MyDataReader As SqlDataReader

            gridDiv.Visible = False
            Try
                mytable1 = New DataTable
                mytable1.Columns.Add(New DataColumn("Name", GetType(String)))
                mytable1.Columns.Add(New DataColumn("criteria", GetType(String)))
                sqlcon = ConfigurationSettings.AppSettings("PathDb")
                con = New SqlConnection(sqlcon)
                openconnection()
                sqlTrans = con.BeginTransaction
                strTemp = MyFile.PostedFile.FileName
                If strTemp = "" Then
                    lblmessage.Visible = True
                    lblmessage.Text = unirecruite.Errconstants.FILEIMPORT
                    lblmessage.ForeColor = Color.Red
                    Exit Sub
                End If
                strFileName = MyFile.FileName
                strFileName = RetrunDate() + "_" + strFileName



                If strFileName.ToLower.EndsWith("xls") Then
                    cnStr = ConfigurationSettings.AppSettings("xls")
                ElseIf strFileName.ToLower.EndsWith("xlsx") Then
                    ' cnStr = ConfigurationSettings.AppSettings("xlsx")
                    lblmessage.Visible = True
                    lblmessage.Text = unirecruite.Errconstants.NoXLSXSUPPORT
                    lblmessage.ForeColor = Color.Red
                    Exit Sub
                Else
                    lblmessage.Visible = True
                    lblmessage.Text = unirecruite.Errconstants.INVALIDFILE
                    lblmessage.ForeColor = Color.Red
                    Exit Sub
                End If


                If strTemp = "" Then
                    lblmessage.Text = Errconstants.FILEIMPORT
                    lblmessage.ForeColor = Color.Red
                    Exit Sub
                End If
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                MyFile.SaveAs(Server.MapPath("Excel Import\") + strFileName)
                strTemp = Server.MapPath("Excel Import\" + strFileName)
                ' connStr = cnStr + "Data Source=" & strTemp & ";"
                connStr = cnStr + "Data Source=" & strTemp & ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""
                'connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" & _
                '"Data Source=" + strTemp + ";Extended Properties=""Excel 8.0;HDR=NO;IMEX=1;"""

                'adapOleDb = New OleDbDataAdapter("SELECT * FROM [BulkData$]", connStr)
                'adapOleDb.TableMappings.Add("Table", "Excel")
                'adapOleDb.Fill(dsOledb)

                Using conn As New OleDbConnection(connStr)
                    conn.Open()

                    Using dataAdapter As New OleDbDataAdapter("SELECT * FROM [BulkData$]", conn)

                        dataAdapter.Fill(dsOledb)
                        'Dim dt As DataTable 
                        'dt= dsOledb.Tables(0)
                        'dt=dt.Rows.Cast(Of DataRow)().Where(Function(row) Not row.ItemArray.All(Function(field) TypeOf field Is System.DBNull OrElse String.Compare(TryCast(field, String).Trim(), String.Empty) = 0)).CopyToDataTable()
                        'spleft = Convert.ToString(dt.Rows.Count)
                        'spRight = Convert.ToString(dt.Columns.Count)
                        'dsOledb.Tables.Add(dt)
                    End Using

                    conn.Close()
                End Using

                While intTempRow <= dsOledb.Tables(0).Rows.Count - 1
                    For intTempCol = 0 To dsOledb.Tables(0).Columns.Count - 1
                        strTemp = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol).ToString
                        If TrimSpace(strTemp.ToString()) = "FirstName" Then
                            intFirstNameRow = intTempRow
                            intFirstNameCol = intTempCol
                            Exit While
                        End If
                    Next
                    intTempRow = intTempRow + 1
                End While
                intTempRow = intTempRow + 1
                intTempCol = intTempCol - 2
                'intTempRow = 0 ' intTempRow + 1
                'intTempCol = 0 'intTempCol - 1
                While intTempRow < dsOledb.Tables(0).Rows.Count
                    generateuserid()
                    sqlstr = ""
                    strFName = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 2).ToString)
                    Dim loginname As String = strFName & userid.ToString()
                    dtCheckQue = New DataTable
                    strRollno = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 1).ToString)
                    strMname = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 3).ToString)
                    strLName = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 4).ToString)
                    strSex = TrimSpace(dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString)
                    strDob = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 6).ToString
                    strEmail = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 7).ToString
                    strSexCheck = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 5).ToString
                    strCenter = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 10).ToString
                    strCourse = dsOledb.Tables(0).Rows(intTempRow).Item(intTempCol + 11).ToString
                    pwd = GetRandomPasswordUsingGUID.ToString()
                    If strFName = "" And strLName = "" And strMname = "" Then
                        Exit While
                    End If
                    objDictionary.Clear()
                    'If strRollno <> String.Empty Then
                    '    objDictionary.Add("RollNo", strRollno)
                    'End If
                    'If strFName <> String.Empty Then
                    '    objDictionary.Add("name", strFName)
                    'End If
                    'If strMname <> String.Empty Then
                    '    objDictionary.Add("middlename", strMname)
                    'End If
                    'If strLName <> String.Empty Then
                    '    objDictionary.Add("surname", strLName)
                    'End If
                    'If strSexCheck <> String.Empty Then
                    '    objDictionary.Add("sex", strSexCheck)
                    'End If
                    'If strDob <> String.Empty Then
                    '    objDictionary.Add("birthdate", strDob)
                    'End If
                    'If strEmail <> String.Empty Then
                    '    objDictionary.Add("email", strEmail)
                    'End If
                    'If strCenter <> String.Empty Then
                    '    objDictionary.Add("Center_ID", strCenter)
                    'End If

                    If strRollno <> String.Empty Or strRollno <> Nothing Or strRollno <> "" Then
                        objDictionary.Add("RollNo", strRollno)
                    Else
                        lblmessage.Visible = True
                        lblmessage.ForeColor = Color.Red
                        lblmessage.Text = "Please Enter Roll No. In Bulk Import Excel Sheet."
                        Exit Sub
                    End If
                    If strFName <> String.Empty Or strFName <> Nothing Or strFName <> "" Then
                        objDictionary.Add("name", strFName)
                    Else
                        lblmessage.Visible = True
                        lblmessage.Text = "Please Enter First Name In Bulk Import Excel Sheet."
                        lblmessage.ForeColor = Color.Red
                        Exit Sub
                    End If
                    If strMname <> String.Empty Or strMname <> Nothing Or strMname <> "" Then
                        objDictionary.Add("middlename", strMname)

                    End If
                    If strLName <> String.Empty Or strLName <> Nothing Or strLName <> "" Then
                        objDictionary.Add("surname", strLName)
                    Else
                        lblmessage.Visible = True
                        lblmessage.ForeColor = Color.Red
                        lblmessage.Text = "Please Enter Last Name In Bulk Import Excel Sheet."
                        Exit Sub
                    End If
                    If strSexCheck <> String.Empty Or strSexCheck <> Nothing Or strSexCheck <> "" Then
                        objDictionary.Add("sex", strSexCheck)
                    Else
                        lblmessage.Visible = True
                        lblmessage.Text = "Please Enter Gender In Bulk Import Excel Sheet."
                        lblmessage.ForeColor = Color.Red
                        Exit Sub
                    End If
                    If strDob <> String.Empty Or strDob <> Nothing Or strDob <> "" Then
                        objDictionary.Add("birthdate", strDob)
                    End If
                    If strEmail <> String.Empty Or strEmail <> Nothing Or strEmail <> "" Then
                        objDictionary.Add("email", strEmail)
                    End If
                    If strCenter <> String.Empty Or strCenter <> Nothing Or strCenter <> "" Then
                        objDictionary.Add("Center_ID", strCenter)
                    Else
                        lblmessage.Visible = True
                        lblmessage.ForeColor = Color.Red
                        lblmessage.Text = "Please Enter Center ID In Bulk Import Excel Sheet."
                        Exit Sub
                    End If
                    If strCourse = String.Empty Or strCourse = Nothing Or strCourse = "" Then
                        lblmessage.Visible = True
                        lblmessage.ForeColor = Color.Red
                        lblmessage.Text = "Please Enter Course ID In Bulk Import Excel Sheet."
                        Exit Sub
                    End If

                    sqlstr = GetSelectQuery(objDictionary)
                    cmd = New SqlCommand(sqlstr, con, sqlTrans)
                    adapSqlCheckQue = New SqlDataAdapter(cmd)
                    adapSqlCheckQue.Fill(dtCheckQue)
                    If dtCheckQue.Rows.Count <> 0 Then
                        rows = mytable1.NewRow
                        rows(0) = strFName + " " + strMname + " " + strLName
                        rows(1) = "Duplicate Records"
                        mytable1.Rows.Add(rows)
                        duplicate += 1
                    Else

                        objDictionary.Add("loginname", loginname)
                        objDictionary.Add("pwd", pwd)
                        objDictionary.Add("user_type", "0")
                        sqlstr = GetInsertQuery(objDictionary)
                        cmd = New SqlCommand(sqlstr, con, sqlTrans)
                        cmd.ExecuteNonQuery()


                        '**********************************




                        'Variables for Weightage
                        Dim intsubweigtage As Integer
                        Dim intsingle As Integer
                        Dim intmultichoise As Integer
                        Dim intblanks As Integer
                        Dim intbasic As Integer
                        Dim intintermediate As Integer
                        Dim item As DictionaryEntry
                        Dim cmdt As SqlCommand
                        Try
                            'Get Id of last record inserted
                            cmdt = New SqlCommand("SELECT MAX(Userid) FROM M_USER_INFO", con, sqlTrans)
                            Dim da As New SqlDataAdapter(cmdt)
                            Dim dss As New DataSet
                            da.Fill(dss)
                            Dim NewId As String = dss.Tables(0).Rows(0).Item(0).ToString()
                            'Code to insert the Course details for the user
                            If strCourse.Contains(",") = True Then

                                Dim courses As String() = strCourse.Split(",")
                                For i As Integer = 0 To courses.Length - 1
                                    Dim arylst As New ArrayList
                                    Dim testary() As Integer
                                    Dim testtype As New Hashtable

                                    Dim strquery As String = " Select test_type from M_Weightage where Course_ID = '" & courses(i) & "' "
                                    cmd = New SqlCommand(strquery, con, sqlTrans)
                                    MyDataReader = cmd.ExecuteReader()
                                    While MyDataReader.Read()
                                        arylst.Add(MyDataReader.Item("test_type"))
                                    End While
                                    MyDataReader.Close()

                                    testary = arylst.ToArray(System.Type.GetType("System.Int32"))


                                    For g As Integer = 0 To testary.Length - 1
                                        testtype.Add(testary(g), courses(i))
                                    Next

                                    For Each item In testtype
                                        Dim booltemp As Boolean = True
                                        Dim strq As String = " Select Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed from M_Weightage where test_type = " & item.Key & " and Course_ID = " & item.Value
                                        Dim ins_cmd3 As New SqlCommand(strq, con, sqlTrans)
                                        Dim rdr As SqlDataReader
                                        rdr = ins_cmd3.ExecuteReader()
                                        If booltemp = True Then
                                            rdr.Read()
                                            If Not IsDBNull(rdr.Item("Sub_Weightage")) Then
                                                intsubweigtage = rdr.Item("Sub_Weightage")
                                            End If
                                            If Not IsDBNull(rdr.Item("Single")) Then
                                                intsingle = rdr.Item("Single")
                                            End If
                                            If Not IsDBNull(rdr.Item("Multi_Choice")) Then
                                                intmultichoise = rdr.Item("Multi_Choice")
                                            End If
                                            If Not IsDBNull(rdr.Item("Blanks")) Then
                                                intblanks = rdr.Item("Blanks")
                                            End If
                                            If Not IsDBNull(rdr.Item("Basic")) Then
                                                intbasic = rdr.Item("Basic")
                                            End If
                                            If Not IsDBNull(rdr.Item("InterMed")) Then
                                                intintermediate = rdr.Item("InterMed")
                                            End If
                                            rdr.Close()
                                            InsertIntoUserCourse(NewId, item.Value, item.Key, intsubweigtage, intsingle, intmultichoise, intblanks, intbasic, intintermediate)
                                            booltemp = False
                                        End If
                                    Next



                                    '  InsertCouseDetails(NewId, courses(i))
                                Next
                                sqlTrans.Commit()
                                rows = mytable1.NewRow
                                rows(0) = strFName + " " + strMname + " " + strLName
                                rows(1) = "Inserted Records"
                                inserted += 1
                                mytable1.Rows.Add(rows)
                                sqlTrans = con.BeginTransaction(IsolationLevel.ReadCommitted)
                            Else
                                Dim arylst As New ArrayList
                                Dim testary() As Integer
                                Dim testtype As New Hashtable

                                Dim strquery As String = " Select test_type from M_Weightage where Course_ID = '" & strCourse & "' "
                                cmd = New SqlCommand(strquery, con, sqlTrans)
                                MyDataReader = cmd.ExecuteReader()
                                While MyDataReader.Read()
                                    arylst.Add(MyDataReader.Item("test_type"))
                                End While
                                MyDataReader.Close()

                                testary = arylst.ToArray(System.Type.GetType("System.Int32"))


                                For g As Integer = 0 To testary.Length - 1
                                    testtype.Add(testary(g), strCourse)
                                Next

                                For Each item In testtype
                                    Dim booltemp As Boolean = True
                                    Dim strq As String = " Select Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed from M_Weightage where test_type = " & item.Key & " and Course_ID = " & item.Value
                                    Dim ins_cmd3 As New SqlCommand(strq, con, sqlTrans)
                                    Dim rdr As SqlDataReader
                                    rdr = ins_cmd3.ExecuteReader()
                                    If booltemp = True Then
                                        rdr.Read()
                                        If Not IsDBNull(rdr.Item("Sub_Weightage")) Then
                                            intsubweigtage = rdr.Item("Sub_Weightage")
                                        End If
                                        If Not IsDBNull(rdr.Item("Single")) Then
                                            intsingle = rdr.Item("Single")
                                        End If
                                        If Not IsDBNull(rdr.Item("Multi_Choice")) Then
                                            intmultichoise = rdr.Item("Multi_Choice")
                                        End If
                                        If Not IsDBNull(rdr.Item("Blanks")) Then
                                            intblanks = rdr.Item("Blanks")
                                        End If
                                        If Not IsDBNull(rdr.Item("Basic")) Then
                                            intbasic = rdr.Item("Basic")
                                        End If
                                        If Not IsDBNull(rdr.Item("InterMed")) Then
                                            intintermediate = rdr.Item("InterMed")
                                        End If
                                        rdr.Close()
                                        InsertIntoUserCourse(NewId, item.Value, item.Key, intsubweigtage, intsingle, intmultichoise, intblanks, intbasic, intintermediate)
                                        booltemp = False
                                    End If
                                Next
                                sqlTrans.Commit()
                                rows = mytable1.NewRow
                                rows(0) = strFName + " " + strMname + " " + strLName
                                rows(1) = "Inserted Records"
                                inserted += 1
                                mytable1.Rows.Add(rows)

                                sqlTrans = con.BeginTransaction(IsolationLevel.ReadCommitted)
                                ' InsertCouseDetails(NewId, strCourse)
                            End If
                        Catch ex As Exception
                            sqlTrans.Rollback()
                            If log.IsDebugEnabled Then
                                log.Debug("Error :" & ex.ToString())
                            End If
                            Response.Redirect("error.aspx", False)
                        Finally

                        End Try

                    End If
                    intTempRow = intTempRow + 1
                    dtCheckQue = Nothing
                End While
                imgBtnDetails.Enabled = True
                closeconnection()
                tblrow2.Visible = True
                lblDuplicate.Text = "Total No. Of Duplicate Records Are:" & duplicate
                lblInserted.Text = "Total No. Of Inserted Records Are:" & inserted
                lblTotal.Text = "Total No. Of Records Are:" & duplicate + inserted
                lblTotal.Visible = True
                lblDuplicate.Visible = True
                lblInserted.Visible = True
                lblSummary.Visible = True
                Session("DuplicatedData") = mytable1
                DataGrid1.DataSource = mytable1.DefaultView
                DataGrid1.DataBind()
                ViewState.Add("Table", mytable1)
                fillPagesCombo()
                'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)
                labelmessage()

                'tbl_feedback.Rows(2).Visible = True
                'DataGrid1.Visible = True
                'gridDiv.Visible = True

            Catch ex As OleDbException
                lblmessage.ForeColor = Color.Red
                lblmessage.Text = "Either the file is not in the prescribed Format or" & vbCrLf & "It is a protected file"

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
                'lblmessage.Text = "The File was not imported!! Please contact the Technical Person"
                'lblmessage.Text = Constant.ERRFILEIMPORT '& "<BR>" & ex.ToString
            Finally
                closeconnection()
                sqlTrans = Nothing
                con = Nothing
                adapOleDb = Nothing
                cmd = Nothing
                sqlstr = Nothing
                mytable1 = Nothing
                dtCheckQue = Nothing
                adapOleDb = Nothing
                adapSql = Nothing
                strTemp = Nothing
                connStr = Nothing
                sqlcon = Nothing
                msg = Nothing
                pwd = Nothing
                strFName = Nothing
                strMname = Nothing
                strLName = Nothing
                strSex = Nothing
                filename = Nothing
                strEmail = Nothing
                strDob = Nothing
                objStreamReader = Nothing
                objDictionary.Clear()
                objDictionary = Nothing

            End Try
        End Sub

        ' [ID] <imgBtnDetails_Click>
        ' [Func] <This Btn gives the detail of import done.>
        ' [Note] The buttons are changed to ImageButton , so the code changes are done accordingly.
        ' [Date] 2011/03/03 by Indarvadan Vasava
        Protected Sub imgBtnDetails_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgBtnDetails.Click
            Try
                tblrow2.Visible = True
                DataGrid1.Visible = True
                gridDiv.Visible = True
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub

        ' [ID] <imgBtnBack_Click>
        ' [Func] <This takes user back to the admin homepage>
        ' [Note] The buttons are changed to ImageButton , so the code changes are done accordingly.
        ' [Date] 2011/03/03 by Indarvadan Vasava
        Protected Sub imgBtnBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgBtnBack.Click
            Try
                Response.Redirect("admin.aspx", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If

            End Try
        End Sub

        Protected Sub DataGrid1_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles DataGrid1.ItemDataBound
            If Not e.Item.ItemType = DataControlRowType.Header Then
                e.Item.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#A4C8EE'")
                e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")
            End If
        End Sub

        Public Sub InsertCouseDetails(ByVal Userid As String, ByVal CourseID As String)
            Try
                Dim query As String = "INSERT INTO T_User_Course(User_ID, Course_ID) VALUES(" & Userid & ", " & CourseID & ")"
                Dim cmd As New SqlCommand(query, con)
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#Region "Enter data into t_user_course"
        'Desc:insert data into t_user_course.
        'By: Indravadan Vasava, 2011/05/05.

        Private Sub InsertIntoUserCourse(ByVal intuserid As Integer, ByVal intcourseid As Integer, ByVal inttest As Integer, ByVal intsubweigtage As Integer, ByVal intsingle As Integer, ByVal intmultichoise As Integer, ByVal intblanks As Integer, ByVal intbasic As Integer, ByVal intintermediate As Integer)
            Dim sb As StringBuilder
            Dim querystr As String

            'Dim sqlTrans As SqlTransaction
            Try
                sb = New StringBuilder
                sb.Append(" Insert into t_user_course ")
                sb.Append(" (User_id,course_id,test_type,Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed) ")
                sb.Append(" Values( ")
                sb.Append(intuserid)
                sb.Append(" , ")
                sb.Append(intcourseid)
                sb.Append(" , ")
                sb.Append(inttest)
                sb.Append(" , ")
                sb.Append(intsubweigtage)
                sb.Append(" , ")
                sb.Append(intsingle)
                sb.Append(" , ")
                sb.Append(intmultichoise)
                sb.Append(" , ")
                sb.Append(intblanks)
                sb.Append(" , ")
                sb.Append(intbasic)
                sb.Append(" , ")
                sb.Append(intintermediate)
                sb.Append(" ) ")
                querystr = sb.ToString()
                Dim ins_cmd1 As New SqlCommand(querystr, con, sqlTrans)
                ins_cmd1.ExecuteNonQuery()

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

        ' To update the teplate wit latest courses and Center details 
        Public Sub Getit()
            'Excel Objects
            'Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
            'Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
            'Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
            'Dim Sheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing
            'Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
            'Dim objOpt As Object = System.Reflection.Missing.Value
            Dim cn As New ConnectDb
            Dim strpath As String
            Dim ds As New DataSet()

            Try

                'Constants
                'Const xlEdgeLeft = 7
                'Const xlEdgeTop = 8
                'Const xlEdgeBottom = 9
                'Const xlEdgeRight = 10

                ''Create  Spreadsheet
                'App = New Excel.Application

                'WorkBooks = DirectCast(App.Workbooks, Excel.Workbooks)
                'Dim myWorkBook As Excel.Workbook = App.Workbooks.Open(Server.MapPath("Excel Import\Bulk user data.XLT"), 0, False, 5, "", "", False, Excel.XlPlatform.xlWindows, "", True, False, 0, True)
                'WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
                'Dim sit As New Excel.Worksheet
                'Dim sit1 As Excel.Worksheet = myWorkBook.Worksheets(2)
                'sit1.Activate()
                'sit1.Cells.Clear()



                '' CODE FOR CENTER DETAILS
                ''*** Write Cell Border ***'  
                '' Set Main Column Headers

                'With App.ActiveSheet.Range("B3:C4")
                '    .MergeCells = True
                '    .Interior.ColorIndex = 40
                '    .Font.Bold = True
                '    .Font.ColorIndex = 53
                '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '    .Cells.Value = "Center Details"
                '    .Font.Size = 15
                '    .BORDERS(xlEdgeLeft).Weight = 2
                '    .BORDERS(xlEdgeTop).Weight = 2
                '    .BORDERS(xlEdgeBottom).Weight = 2
                '    .BORDERS(xlEdgeRight).Weight = 2
                'End With


                '' Set Column Headers

                'App.ActiveSheet.Cells(5, 2).Value = "Center Name"
                'App.ActiveSheet.Cells(5, 2).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 2).Font.Bold = True
                'App.ActiveSheet.Cells(5, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                'App.ActiveSheet.Cells(5, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                'App.ActiveSheet.Cells(5, 2).ColumnWidth = 20
                'App.ActiveSheet.Cells(5, 2).WrapText = False
                'App.ActiveSheet.Cells(5, 2).Borders.Weight = 2

                'App.ActiveSheet.Cells(5, 3).Value = "Center ID"
                'App.ActiveSheet.Cells(5, 3).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 3).Font.ColorIndex = 3
                'App.ActiveSheet.Cells(5, 3).Font.Bold = True
                'App.ActiveSheet.Cells(5, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                'App.ActiveSheet.Cells(5, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                'App.ActiveSheet.Cells(5, 3).ColumnWidth = 20
                'App.ActiveSheet.Cells(5, 3).WrapText = False
                'App.ActiveSheet.Cells(5, 3).Borders.Weight = 2

                'Dim ht As New Hashtable
                'ht.Add("Delhi", "1")
                'ht.Add("Mumbai", "2")
                'ht.Add("Calcutta", "3")
                'ht.Add("Bangalore", "4")
                'ht.Add("India", "777")


                'Dim cn As New ConnectDb
                'Dim strpath As String
                'strpath = ConfigurationSettings.AppSettings("PathDb")
                'If cn.connect(strpath) Then



                '    Dim ds As New DataSet()


                '    Dim da As New SqlDataAdapter("select center_id,Center_name from m_centers where del_flg=0", cn.MyConnection)
                '    da.Fill(ds)
                '    ht.Clear()
                '    For a As Integer = 0 To ds.Tables(0).Rows.Count - 1
                '        ht.Add(ds.Tables(0).Rows(a).Item(1).ToString, ds.Tables(0).Rows(a).Item(0).ToString)
                '    Next
                'End If
                'Dim keys(ht.Count) As String
                'ht.Keys.CopyTo(keys, 0)
                'Dim rownum As Integer = 6
                '' Set the data in columns
                'For i As Integer = 0 To keys.Length - 2
                '    App.ActiveSheet.Cells(rownum, 2).Value = keys(i).ToString
                '    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                '    App.ActiveSheet.Cells(rownum, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignTop
                '    App.ActiveSheet.Cells(rownum, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '    App.ActiveSheet.Cells(rownum, 2).Borders.Weight = 2
                '    App.ActiveSheet.Cells(rownum, 3).Value = ht(keys(i)).ToString
                '    App.ActiveSheet.Cells(rownum, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                '    App.ActiveSheet.Cells(rownum, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '    App.ActiveSheet.Cells(rownum, 3).Borders.Weight = 2
                '    rownum = rownum + 1
                'Next

                '' CODE FOR CENTER DETAILS
                'Dim sit2 As Excel.Worksheet = myWorkBook.Worksheets(3)
                'sit2.Activate()
                'sit2.Cells.Clear()


                'With App.ActiveSheet.Range("B3:C4")
                '    .MergeCells = True
                '    .Interior.ColorIndex = 40
                '    .Font.Bold = True
                '    .Font.ColorIndex = 53
                '    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                '    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                '    .Cells.Value = "Course Details"
                '    .Font.Size = 15
                '    .BORDERS(xlEdgeLeft).Weight = 2
                '    .BORDERS(xlEdgeTop).Weight = 2
                '    .BORDERS(xlEdgeBottom).Weight = 2
                '    .BORDERS(xlEdgeRight).Weight = 2
                'End With


                '' Set Column Headers

                'App.ActiveSheet.Cells(5, 2).Value = "Course Name"
                'App.ActiveSheet.Cells(5, 2).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 2).Font.Bold = True
                'App.ActiveSheet.Cells(5, 2).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                'App.ActiveSheet.Cells(5, 2).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                'App.ActiveSheet.Cells(5, 2).ColumnWidth = 20
                'App.ActiveSheet.Cells(5, 2).WrapText = False
                'App.ActiveSheet.Cells(5, 2).Borders.Weight = 2

                'App.ActiveSheet.Cells(5, 3).Value = "Course ID"
                'App.ActiveSheet.Cells(5, 3).Interior.ColorIndex = 36
                'App.ActiveSheet.Cells(5, 3).Font.ColorIndex = 3
                'App.ActiveSheet.Cells(5, 3).Font.Bold = True
                'App.ActiveSheet.Cells(5, 3).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                'App.ActiveSheet.Cells(5, 3).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
                'App.ActiveSheet.Cells(5, 3).ColumnWidth = 20
                'App.ActiveSheet.Cells(5, 3).WrapText = False
                'App.ActiveSheet.Cells(5, 3).Borders.Weight = 2

                'strpath = ConfigurationSettings.AppSettings("PathDb")
                If cn.connect() Then
                    Dim da As New SqlDataAdapter("select Distinct center_id,Center_name from m_centers where del_flg=0 order by center_id", cn.MyConnection)
                    da.Fill(ds)

                End If


                'Added By bharat Prajapati
                'Create csv file for cneter information.
                '*********start*********************
                Dim dt As DataTable = ds.Tables(0)
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                CreateCSVFile(dt, Server.MapPath("ExcelImport\Upload Bulk User Data\Center_Details.csv"))

                'Select Query to retrive course information. This course information to be exported in csv file.

                Dim strbldr As New StringBuilder
                strbldr.Append("  select Distinct mw.Course_ID,mc.COURSE_NAME from M_Weightage as mw inner join m_course as mc ")
                strbldr.Append("  on mc.course_id=mw.course_id AND MC.Del_Flag=0 group by mw.Course_ID,mc.course_name ")
                strbldr.Append(" HAVING SUM(MW.Sub_Weightage)=100 order by mw.Course_ID")

                ' Dim da As New SqlDataAdapter("select course_id,Course_name from m_course where del_flag=0", cn.MyConnection)
                Dim daa As New SqlDataAdapter(strbldr.ToString, cn.MyConnection)
                ds = New DataSet()
                daa.Fill(ds)
                'Added By bharat Prajapati
                'Create csv file for course information.
                'Date: 2012/07/12
                '*********start*********************
                dt = ds.Tables(0)
                'Save csv file with the course details
                ' By Nisha on 2018/05/17
                ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                CreateCSVFile(dt, Server.MapPath("ExcelImport\Upload Bulk User Data\Course_Details.csv"))

                'Now create zip file. zip file will be made using Ionic.dll file. This file need to be include in reference.
                Using zip As New ZipFile()
                    ' By Nisha on 2018/05/17
                    ' Modify folder name from Excel Import to ExcelImport and extension from .XLS to .xls
                    zip.AddDirectory(Server.MapPath("ExcelImport\Upload Bulk User Data"))
                    zip.Save(Server.MapPath("ExcelImport\Upload Bulk User Data.zip"))
                End Using


                '*************End********************

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                If cn IsNot Nothing Then
                    cn.disconnect()
                End If
                '  Throw ex
            Finally
                If cn IsNot Nothing Then
                    cn.disconnect()
                End If
                cn = Nothing
            End Try
        End Sub
        'Added By: Bharat 
        'Date: 2012/07/12
        'Description: generate csv file from dataTable.
        Public Sub CreateCSVFile(ByVal dt As DataTable, ByVal strFilePath As String)
            '#Region "Export Grid to CSV"
            ' Create the CSV file to which grid data will be exported.
            Dim sw As New StreamWriter(strFilePath, False)
            ' First we will write the headers.
            'DataTable dt = m_dsProducts.Tables[0];
            Dim iColCount As Integer = dt.Columns.Count
            For i As Integer = 0 To iColCount - 1
                sw.Write(dt.Columns(i))
                If i < iColCount - 1 Then
                    sw.Write(",")
                End If
            Next
            sw.Write(sw.NewLine)
            ' Now write all the rows.
            For Each dr As DataRow In dt.Rows
                For i As Integer = 0 To iColCount - 1
                    If Not Convert.IsDBNull(dr(i)) Then
                        sw.Write(dr(i).ToString().Replace(",", " "))
                    End If
                    If i < iColCount - 1 Then
                        sw.Write(",")
                    End If
                Next
                sw.Write(sw.NewLine)
            Next
            sw.Close()
            '#End Region
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


        '    If len >= DataGrid1.PageCount Then
        '        len = DataGrid1.PageCount - 1
        '    End If

        '    ' if search clicked again then page 1 should be selected 
        '    If DataGrid1.CurrentPageIndex = 0 Then
        '        ViewState("pageNo") = 1
        '        ViewState("lastRange") = 1
        '    End If

        '    ' Getting the currently selected page value 
        '    Dim selPage As Integer = 0
        '    If (ViewState("pageNo") <> Nothing) Then
        '        selPage = CInt(ViewState("pageNo"))
        '    Else
        '        ' selPage = 1
        '        selPage = DataGrid1.CurrentPageIndex + 1
        '    End If

        '    If (ViewState("lastRange") <> Nothing) Then

        '        '  If selPage >= CInt(ViewState("lastRange")) And CInt(ViewState("lastRange")) + len <= DataGrid1.PageCount Then
        '        If selPage >= CInt(ViewState("lastRange")) And selPage <= CInt(ViewState("lastRange")) + len Then
        '            range = CInt(ViewState("lastRange"))
        '        Else
        '            'If it is the last page then resetting the page numbers
        '            ' last number - provided length
        '            'If (len + selPage) >= DataGrid1.PageCount Then
        '            '    If selPage <= len Then
        '            '        range = range
        '            '    Else
        '            '        range = DataGrid1.PageCount - len
        '            '        'Incase range becomes 0 or less than zero than setting it 1 
        '            '        If range <= 0 Then
        '            '            range = 1
        '            '        End If
        '            '    End If

        '            'Else
        '            If selPage <= DataGrid1.PageCount Then
        '                'range = range
        '                If range < CInt(ViewState("lastRange")) Then
        '                    range = CInt(ViewState("lastRange")) - 1
        '                Else
        '                    If selPage - len > 0 And selPage - len <= DataGrid1.PageCount - len Then
        '                        range = selPage - len
        '                    Else
        '                        range = CInt(ViewState("lastRange")) + 1
        '                    End If
        '                    'range = CInt(ViewState("lastRange")) + 1
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
        '    'If selPage = 1 And selPage = DataGrid1.PageCount - 1 Then
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
        '    If selPage = DataGrid1.PageCount Then
        '        imgnext.Enabled = False
        '        imglast.Enabled = False
        '    End If
        'End Sub

        Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DataGrid1.CurrentPageIndex < (DataGrid1.PageCount - 1)) Then
                        DataGrid1.CurrentPageIndex += 1

                    End If

                Case "prev" 'The prev button was clicked
                    If (DataGrid1.CurrentPageIndex > 0) Then
                        DataGrid1.CurrentPageIndex -= 1
                    End If

                Case "last" 'The Last Page button was clicked
                    DataGrid1.CurrentPageIndex = (DataGrid1.PageCount - 1)

                Case Else 'The First Page button was clicked
                    DataGrid1.CurrentPageIndex = Convert.ToInt32(arg)
            End Select
            ViewState.Add("pageNo", DataGrid1.CurrentPageIndex + 1)
            ViewState.Add("selval", DataGrid1.CurrentPageIndex)
            BindGrid()
            'Now, bind the data!
            '   BindSQL()
        End Sub

        Sub PagerButtonClickLinks(ByVal sender As Object, ByVal e As CommandEventArgs)
            'used by external paging UI
            Dim arg As String = sender.CommandArgument

            Select Case arg
                Case "next" 'The next Button was Clicked
                    If (DataGrid1.CurrentPageIndex < (DataGrid1.PageCount - 1)) Then
                        DataGrid1.CurrentPageIndex += 1
                        '    ViewState.Add("selval", DataGrid1.CurrentPageIndex)
                    End If

                Case "prev" 'The prev button was clicked
                    If (DataGrid1.CurrentPageIndex > 0) Then
                        DataGrid1.CurrentPageIndex -= 1
                        '  ViewState.Add("selval", ddlPages.SelectedItem.Value)
                    End If

                Case "last" 'The Last Page button was clicked
                    DataGrid1.CurrentPageIndex = (DataGrid1.PageCount - 1)
                    'ViewState.Add("selval", ddlPages.SelectedItem.Value)
                Case Else 'The First Page button was clicked
                    DataGrid1.CurrentPageIndex = Convert.ToInt32(arg) - 1
                    ' ViewState.Add("selval", ddlPages.SelectedItem.Value)
            End Select

            ViewState.Add("pageNo", DataGrid1.CurrentPageIndex + 1)
            ViewState.Add("selval", DataGrid1.CurrentPageIndex)
            BindGrid()
            'Now, bind the data!
            '   BindSQL()
        End Sub

        Public Sub fillPagesCombo()
            ddlPages.Items.Clear()
            For cnt As Integer = 1 To DataGrid1.PageCount
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

        Public Sub BindGrid()
            Dim table As DataTable = DirectCast(ViewState("Table"), DataTable)
            DataGrid1.DataSource = table.DefaultView
            DataGrid1.DataBind()
            fillPagesCombo()
            'fillPageNumbers(DataGrid1.CurrentPageIndex + 1, 9)


        End Sub

        Private Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
            DataGrid1.CurrentPageIndex = ddlPages.SelectedItem.Value
            ViewState.Add("selval", ddlPages.SelectedItem.Value)
            ViewState.Add("pageNo", ddlPages.SelectedItem.Value + 1)
            BindGrid()
        End Sub
    End Class

End Namespace