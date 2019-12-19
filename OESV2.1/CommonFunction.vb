Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Collections
Imports System.Data.SqlClient
Imports System

Imports System.Configuration
Imports System.Web.UI.WebControls
Imports System.Web.Security

Imports log4net
Imports System.Web.UI
Imports Microsoft.Office.Interop
Imports System.Drawing
Imports System.Globalization
Imports System.Web.SessionState
Imports System.Net
Imports System.Net.Mail

Public Class CommonFunction

#Region "Get Single Value"
    ''*******************************************************************''
    ''Author: Irfan Mansuri                                              ''
    ''Description: Return Single Value from datatable.                   ''
    ''Created Date; 23/02/2015                                           ''
    ''*******************************************************************''
    Public Function getSingleValue(ByVal AggregateFunction As String, ByVal TableName As String, ByVal WhereClause As String) As Object
        Dim strBuild As StringBuilder
        Dim objConnect As ConnectDb
        Dim dtSingle As DataTable
        Dim sqlAdap As SqlDataAdapter
        Dim strPathDb As String
        Dim strReturnValue As String
        Try
            strPathDb = ConfigurationManager.AppSettings("PathDb")
            objConnect = New ConnectDb()
            dtSingle = New DataTable()
            strBuild = New StringBuilder()
            strBuild.Append("Select ")
            strBuild.Append(AggregateFunction)
            strBuild.Append(" from ")
            strBuild.Append(TableName)
            If WhereClause.ToString <> "" AndAlso WhereClause.ToString() <> String.Empty Then
                strBuild.Append(" Where ")
                strBuild.Append(WhereClause)
            End If

            If objConnect.connect() = True Then
                sqlAdap = New SqlDataAdapter(strBuild.ToString(), objConnect.MyConnection)
                sqlAdap.Fill(dtSingle)
                strReturnValue = dtSingle.Rows(0).Item(0).ToString()
                Return strReturnValue
            End If
        Catch ex As Exception
            sqlAdap = Nothing
            strBuild = Nothing
        Finally
            strBuild = Nothing
            sqlAdap = Nothing
            dtSingle = Nothing
            strPathDb = Nothing
            'objConnect.disconnect()
        End Try
    End Function
#End Region

#Region "Get Datatable"
    ''*******************************************************************''
    ''Author: Irfan Mansuri                                              ''
    ''Description: Return datatable                                      ''
    ''Created Date: 23/02/2015                                           ''
    ''*******************************************************************''
    Public Function getDatatable(ByVal selectColumns As String, ByVal tableName As String, ByVal whereCluase As String, ByVal orderBy As String) As Object
        Dim strBuild As StringBuilder
        Dim dtTable As DataTable
        Dim objConnect As ConnectDb
        Dim sqlAdap As SqlDataAdapter
        Dim strPathDb As String
        Try
            strPathDb = ConfigurationManager.AppSettings("PathDb")
            objConnect = New ConnectDb()
            dtTable = New DataTable()
            strBuild = New StringBuilder("Select ")
            strBuild.Append(selectColumns)
            strBuild.Append(" from ")
            strBuild.Append(tableName)
            If whereCluase.ToString <> "" AndAlso whereCluase.ToString <> String.Empty Then
                strBuild.Append(" Where ")
                strBuild.Append(whereCluase)
            End If
            If orderBy.ToString <> "" AndAlso orderBy.ToString <> String.Empty Then
                strBuild.Append(" Order by ")
                strBuild.Append(orderBy)
            End If

            If objConnect.connect() = True Then
                sqlAdap = New SqlDataAdapter(strBuild.ToString(), objConnect.MyConnection)
                sqlAdap.Fill(dtTable)
            End If
            Return dtTable
        Catch ex As Exception
            objConnect = Nothing
            strPathDb = Nothing
        Finally
            objConnect = Nothing
            strPathDb = Nothing
            sqlAdap = Nothing
            objConnect.disconnect()
        End Try
    End Function
#End Region
    ' <summary>
    'This Function is replace special character in proper format. For eg. < to replace &lt;  
    '</summary>
    '<param name="strQuestion">will contains for whole question details</param>
    Public Function checkString(ByVal strQuestion As String)
        Dim strData As String = ""
        Dim strLess As String
        Dim strGreater As String
        Dim ch As Char
        Try
            strLess = "&lt;"
            strGreater = "&gt;"
            For Each ch In strQuestion
                If ch.ToString().Equals("<") Then
                    strData = strData & strLess
                ElseIf ch.ToString().Equals(">") Then
                    strData = strData & strGreater
                Else
                    strData = strData & ch
                End If

            Next
            Return strData
        Catch ex As Exception
            strLess = Nothing
            strGreater = Nothing
            ch = Nothing
        End Try
    End Function
#Region "CheckForUpdateTimeValue"
    '****************************************************************************************************************************************
    'Code added by Monal Shah
    'MethodName:GetUpdateTime
    'Purpose:CheckForUpdateTimeValue from m_systemSettings
    '***************************************************************************************************************************************
    Public Function GetUpdateTime() As Integer
        Dim strPathDb As String
        Dim objconn As New ConnectDb 'object of the ConnectClass class
        Dim strSql As String
        Dim objCommand As SqlCommand                  'SqlCommand object
        Dim objDataReader As SqlDataReader
        Dim flagValue As Integer
        Try
            strPathDb = ConfigurationManager.AppSettings("PathDb")
            If objconn.connect() Then
                strSql = "SELECT value FROM m_system_settings Where key1='QuestionPaper' and key2='UpdateTime'"
                objCommand = New SqlCommand(strSql, objconn.MyConnection)
                objDataReader = objCommand.ExecuteReader
                If objDataReader.Read() Then
                    flagValue = objDataReader.Item("value")
                End If
            End If
            Return flagValue
        Catch ex As Exception
            If Not objconn.MyConnection Is Nothing Then
                If objconn.MyConnection.State = ConnectionState.Open Then
                    objconn.disconnect()
                End If
            End If
            'If log.IsDebugEnabled Then
            '    log.Debug("Error :" & ex.ToString())
            'End If
            'HttpResponse.Redirect("error.aspx?err=" & ex.Message, False)
        Finally
            If Not objconn.MyConnection Is Nothing Then
                If objconn.MyConnection.State = ConnectionState.Open Then
                    objconn.disconnect()
                End If
            End If
            objDataReader = Nothing
            objCommand = Nothing
            objconn = Nothing
        End Try

    End Function
#End Region
#Region "GetSelectSearchQuery"
    ' <summary>
    'This Function is sql create query  using dictionay object.
    '</summary>
    '<param name="htSelectFieldValue">will contains dictionary values</param>

    Public Function GetSelectSearchQuery(ByVal htSelectFieldValue As Dictionary(Of String, String)) As StringBuilder

        Dim sbQuery As New StringBuilder()
        Dim chktime As Integer = GetUpdateTime()

        If htSelectFieldValue.Count <> 0 Then
            sbQuery.Append(" ")
            sbQuery.Append("Where ")
            sbQuery.Append(" ")
        End If
        For Each val As KeyValuePair(Of String, String) In htSelectFieldValue

            If val.Key.ToString() = "temp.username" Then
                sbQuery.Append(val.Key)
                sbQuery.Append(" Like ")
                sbQuery.Append(" ")
                sbQuery.Append("'%")
                sbQuery.Append(val.Value.ToString())
                sbQuery.Append("%'")

            ElseIf val.Key.ToString() = "grad = " Then
                sbQuery.Append(val.Key)
                sbQuery.Append("'")
                sbQuery.Append(val.Value.ToString())
                sbQuery.Append("'")

            ElseIf val.Key.ToString() = " (CandStatus.written_test_remark" Then
                sbQuery.Append(val.Key)
                sbQuery.Append(" =")
                sbQuery.Append("'")
                sbQuery.Append(val.Value.ToString())

            ElseIf val.Key.ToString() = "temp.Assigned" Then
                sbQuery.Append(" (temp.appearedflag = 0  ) ")
            ElseIf val.Key.ToString() = "temp.Appeared" Then
                sbQuery.Append(" temp.appearancedate is not null ")
                '**************************Monal Shah*******************
                'ElseIf val.Key.ToString() = "temp.Ongoing" Then
                '    sbQuery.Append(" (  temp.appearedflag = 1) ")
                ''dicSearch.Add("temp.Ongoing", "")

                '++++++++++*******************LOGIN USER*******************+++++++++++
                ' ElseIf val.Key.ToString() = "temp.Ongoing" Then

            ElseIf (val.Key.ToString() = "temp.LoginUser") Then
                sbQuery.Append(" (  temp.appearedflag = 1) ")
                sbQuery.Append(" and temp.end_time >= dateadd(ss,-(temp.TotalTime*" & chktime & "*60 / 100)-5,getdate()) ")
                '++++++++++*******************LOGIN USER*******************+++++++++++

                '++++++++++*******************Link break*******************+++++++++++
            ElseIf (val.Key.ToString() = "temp.LinkBreak") Then
                sbQuery.Append(" (  temp.appearedflag = 1) ")
                sbQuery.Append(" and temp.end_time < dateadd(ss,-(temp.TotalTime*" & chktime & "*60 / 100)-5,getdate()) ")

                'End If
                '*********end***********************************************
            ElseIf val.Key.ToString() = "temp.writtentestdate Between '" Then
                sbQuery.Append(val.Key)
                sbQuery.Append(val.Value.ToString())
            ElseIf val.Key.ToString() = "temp.writtentestdate" Then
                sbQuery.Append(val.Key)
                sbQuery.Append(">=")
                sbQuery.Append("'" + val.Value.ToString() + "'")
            ElseIf val.Key.ToString() = "temp.appearancedate Between '" Then
                sbQuery.Append(val.Key)
                sbQuery.Append(val.Value.ToString())
            ElseIf val.Key.ToString() = "temp.appearancedate" Then
                sbQuery.Append(val.Key)
                sbQuery.Append(">=")
                sbQuery.Append("'" + val.Value.ToString() + "'")
                'ElseIf val.Key.ToString() = "ResScore.Score Between '" Then
                '    sbQuery.Append(val.Key)
                '    sbQuery.Append(val.Value.ToString())
                'ElseIf val.Key.ToString() = "ResScore.Score" Then
                '    sbQuery.Append(val.Key)
                '    sbQuery.Append(">=")
                '    sbQuery.Append(val.Value.ToString())
            ElseIf val.Key.ToString() = "m_testinfo.test_type" Then
                sbQuery.Append("candstatus.Course_ID in (Select distinct course_id from m_testinfo where test_type =")
                If (val.Value.ToString() <> "") Then
                    sbQuery.Append("'" + val.Value.ToString() + "')")
                Else
                    sbQuery.Append("NULL")
                End If
            Else
                sbQuery.Append(val.Key)
                sbQuery.Append(" =")
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
        Return (sbQuery)
    End Function
#End Region

    Public Function ReadFile(ByVal fileName As String) As String
        Try
            If Not System.IO.File.Exists(fileName) Then Exit Function
            Dim strContent As String()
            Dim strbrContent As New StringBuilder()
            strContent = System.IO.File.ReadAllLines(fileName)
            For Each s As String In strContent
                strbrContent.Append(s)
            Next
            ReadFile = strbrContent.ToString
        Catch ex As Exception

        End Try
    End Function

    Public Sub sendMail(ByVal strTo As String, ByVal strCC As String, ByVal strMessage As String, ByVal strSubject As String)
        Dim strFrom As String
        Dim strName As String
        'Dim strCC As String
        Dim smtpClient As SmtpClient
        Dim message As MailMessage
        Dim fromAddress As MailAddress
        Dim crd As NetworkCredential
        Dim strArrTo() As String
        Dim intSize As Integer = 0
        Try
            strFrom = ConfigurationManager.AppSettings("mailsenderid")
            strName = ConfigurationManager.AppSettings("mailidusername")
            smtpClient = New SmtpClient()
            message = New MailMessage()
            fromAddress = New MailAddress(strFrom, strName)

            ' You can specify the host name or ipaddress of your server
            ' Default in IIS will be localhost 
            smtpClient.Host = ConfigurationManager.AppSettings("smtpserveraddress")
            crd = New NetworkCredential()
            crd.UserName = ConfigurationManager.AppSettings("mailsenderid")
            crd.Password = ConfigurationManager.AppSettings("mailidpassword")
            smtpClient.Credentials = crd
            smtpClient.Port = 25

            'From address will be given as a MailAddress Object
            message.From = fromAddress

            ' To address collection of MailAddress
            If strTo <> String.Empty Then
                If strTo.Contains(";") Then
                    strArrTo = strTo.Split(";")
                    For Each s As String In strArrTo
                        message.To.Add(s)
                    Next
                Else
                    message.[To].Add(strTo)
                End If
            End If
            'message.[To].Add(strTo)

            'If strCC <> String.Empty Then
            '    strCC = strCC & ";" & ConfigurationSettings.AppSettings("proactiveHOmailid")
            'Else
            '    strCC = ConfigurationSettings.AppSettings("proactiveHOmailid")
            'End If
            strArrTo = Nothing
            'If strCC <> String.Empty Then
            '    If strCC.Contains(";") Then
            '        strArrTo = strCC.Split(";")
            '        For Each s As String In strArrTo
            '            message.CC.Add(s)
            '        Next
            '    Else
            '        message.CC.Add(strCC)
            '    End If
            'End If

            'message.CC.Add("ankit@usindia.com")
            message.Subject = strSubject

            'Body can be Html or text format
            'Specify true if it  is html message
            message.IsBodyHtml = True


            message.Body = strMessage

            ' Send SMTP mail

            smtpClient.Send(message)
        Catch ex As Exception
        Finally
            strMessage = Nothing
            strArrTo = Nothing
            strFrom = Nothing
            strName = Nothing
            strCC = Nothing
            smtpClient = Nothing
            message = Nothing
            fromAddress = Nothing
            crd = Nothing
        End Try
    End Sub


#Region "Function for Validating CourseName"
    'Desc: This Function Validates for duplicate entry for coursename
    'By: Jatin Gangajaliaya

    Public Function ValidateCourseName(ByRef strval As String) As Boolean
        Dim MyCommand As SqlCommand
        Dim objconn As New ConnectDb
        Dim strbr As StringBuilder
        Dim strquery As String
        Dim intcount As Integer
        'Dim booldec As Boolean
        Dim strPathDb As String = ConfigurationManager.AppSettings("PathDb")
        Try
            strbr = New StringBuilder
            strbr.Append(" Select Count(Course_id) from M_Course where Course_name = ")
            strbr.Append("'")
            strbr.Append(strval)
            strbr.Append("'")
            strquery = strbr.ToString()
            If objconn.connect() Then
                MyCommand = New SqlCommand(strquery, objconn.MyConnection)
                intcount = MyCommand.ExecuteScalar()
            End If
            If (intcount > 0) Then
                Return False
            ElseIf (intcount = 0) Then
                Return True
            End If
        Catch ex As Exception
            Throw ex
        Finally
            objconn.disconnect()
            strbr = Nothing
            strquery = Nothing
            objconn = Nothing
            MyCommand = Nothing
        End Try
    End Function


#End Region

    Public Function GetCommaSeperatedIDS(ByVal query As String) As String
        Dim log As log4net.ILog = log4net.LogManager.GetLogger("ForGetCommas")
        Dim result As String = ""
        Dim objconn As New ConnectDb
        Dim ds As DataSet
        Dim da As SqlDataAdapter
        'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        Try
            If objconn.connect() = True Then
                da = New SqlDataAdapter(query, objconn.MyConnection)
                ds = New DataSet
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then


                    For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                        If ds.Tables(0).Rows.Count = 1 Then
                            result = ds.Tables(0).Rows(i).Item(0).ToString
                            Exit For
                        ElseIf ds.Tables(0).Rows.Count > 1 Then
                            result = result & ds.Tables(0).Rows(i).Item(0).ToString & ","
                        End If
                    Next
                    If result.Contains(",") Then
                        result = result.Substring(0, result.Length - 1)
                    End If
                Else
                    result = "0"
                End If
                objconn.disconnect()
            End If
        Catch ex As Exception
            If objconn.MyConnection.State = ConnectionState.Open Then
                objconn.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            objconn = Nothing
        End Try
        Return result
    End Function
#Region "DeleteQuestion"

    ''---------------------------------------------------------------------------------------------------------------------------------------------------
    ''     NAME            :- DeleteQuestion
    ''     AUTHOR NAME     :- Abhay Awasthi
    ''     DESCRIPTION     :- This function is used to delect Question A/C to their qurstionId and TestType
    ''     CREATED DATE    :- 2012/08/30
    ''----------------------------------------------------------------------------------------------------------------------------------------------------

    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objConnection As SqlConnection
    Dim objTransaction As SqlTransaction
    Dim objCommand As SqlCommand
    Dim objConstan As Constant
    Dim sb As StringBuilder
    Dim flg As Boolean
    'Dim msg As String
    'Dim title As String
    'Dim style As MsgBoxStyle
    'Dim response As MsgBoxResult

    Public Function DeleteQuestion(ByVal strQueid As String, ByVal strTType As String) As Boolean

        Try
            objConnection = New SqlConnection(strPathDb)
            objConnection.Open()
            objTransaction = objConnection.BeginTransaction()
            objCommand = objConnection.CreateCommand()
            sb = New StringBuilder()
            objConstan = New Constant()

            'style = MsgBoxStyle.Question Or _
            'MsgBoxStyle.Critical Or MsgBoxStyle.YesNo
            '' Display message.
            'response = MsgBox(objConstan.strMessageDefine, style, objConstan.strMsgTitle)
            'If response = MsgBoxResult.Yes Then ' User chose Yes.

            sb.Append(objConstan.strDelete)
            sb.Append(objConstan.strM_Options)
            sb.Append(objConstan.strWhere)
            sb.Append(objConstan.strPopen)
            sb.Append(objConstan.strQnid)

            sb.Append(strQueid)
            sb.Append(objConstan.strAnd)
            sb.Append(objConstan.strTest_Type)
            sb.Append(strTType)

            sb.Append(objConstan.strPclose)

            objCommand = New SqlCommand(sb.ToString(), objConnection, objTransaction)
            Dim rowaff As Integer = objCommand.ExecuteNonQuery()

            sb = Nothing
            sb = New StringBuilder()
            sb.Append(objConstan.strDelete)
            sb.Append(objConstan.strM_question)
            sb.Append(objConstan.strWhere)
            sb.Append(objConstan.strPopen)
            sb.Append(objConstan.strM_question_qnid)

            sb.Append(strQueid)
            sb.Append(objConstan.strAnd)
            sb.Append(objConstan.strTest_Type)
            sb.Append(strTType)

            sb.Append(objConstan.strPclose)
            objCommand = Nothing
            objCommand = New SqlCommand(sb.ToString(), objConnection, objTransaction)
            Dim rowaff2 As Integer = objCommand.ExecuteNonQuery()

            sb = Nothing
            sb = New StringBuilder()
            sb.Append(objConstan.strDelete)
            sb.Append(objConstan.strM_Question_Answer)
            sb.Append(objConstan.strWhere)
            sb.Append(objConstan.strPopen)
            sb.Append(objConstan.strM_Question_Answer_Qn_ID)

            sb.Append(strQueid)
            sb.Append(objConstan.strAnd)
            sb.Append(objConstan.strTest_Type)
            sb.Append(strTType)

            sb.Append(objConstan.strPclose)
            objCommand = Nothing
            objCommand = New SqlCommand(sb.ToString(), objConnection, objTransaction)
            Dim rowaff3 As Integer = objCommand.ExecuteNonQuery()

            objTransaction.Commit()

            Return True
            'End If

        Catch ex As Exception

            objTransaction.Rollback()
            Return False

        Finally
            objConnection = Nothing
            objTransaction = Nothing
            objCommand = Nothing
            objConstan = Nothing
            sb = Nothing

        End Try

    End Function
#End Region
End Class
