Imports System.Collections
Imports System.Web.Mail
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.IO
Imports System.Web.UI
Imports System.Web
Imports System.Configuration
Imports System.Runtime.InteropServices
Imports log4net
Imports System.Drawing
Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Threading

Partial Public Class ExamResult
    Inherits System.Web.UI.Page
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("ExamResult")
    'variable added by rajesh 2014/08/25
    Dim _CourseID As String = String.Empty
    Dim _CenterID As String = String.Empty
    Dim _CandidateID As String = String.Empty
    Dim objHashTable As New Hashtable


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Session.Item("result") IsNot Nothing) Then
            objHashTable = Session.Item("result")
            LblClassName.Text = Convert.ToString(objHashTable("Centrename"))

            LblDate.Text = Convert.ToString(objHashTable("Date"))
            LblMarksObtained.Text = Convert.ToString(objHashTable("MarksObtained"))
            LblName.Text = Convert.ToString(objHashTable("Name"))
            LblStatus.Text = Convert.ToString(objHashTable("Status"))
            If (LblStatus.Text = "Fail") Then
                LblStatus.Style.Add("color", "red")
            End If
            LblTestName.Text = Convert.ToString(objHashTable("Coursename"))
            LblTotalMarks.Text = Convert.ToString(objHashTable("TotalMark"))
        End If
    End Sub
    'objHashTable = Session.Item("result")
    'LblClassName.Text = Convert.ToString(objHashTable("Centrename"))

    'LblDate.Text = Convert.ToString(objHashTable("Date"))
    'LblMarksObtained.Text = Convert.ToString(objHashTable("MarksObtained"))
    'LblName.Text = Convert.ToString(objHashTable("Name"))
    'LblStatus.Text = Convert.ToString(objHashTable("Status"))
    'If (LblStatus.Text = "Fail") Then
    '    LblStatus.Style.Add("color", "red")
    'End If
    'LblTestName.Text = Convert.ToString(objHashTable("Coursename"))
    'LblTotalMarks.Text = Convert.ToString(objHashTable("TotalMark"))
    'method created by rajesh Nagvanshi 

    '_CourseID = GetID("select Course_Id From M_Course where Course_Name='" & objHashTable("Coursename") & "'", "Course_Id")
    '_CenterID = GetID("select Center_ID From M_Centers where Center_Name='" & objHashTable("Centrename") & "'", "Center_ID")
    '_CandidateID = Session.Item("userid")

    '  ExportExamDetails()  


    'Send Mail to students and respective tacher in cc
    '        Dim mail As New MailMessage
    '        Dim strMessage As String
    '        Dim objCommFun As CommonFunction
    '        Dim strSmtpServer As String
    '        ' Dim strQuery1 As String
    '        Dim objconn As New ConnectDb
    '        Dim myCommand As SqlCommand
    '        Dim myDataReader As SqlDataReader
    '        Dim StrBrquery As StringBuilder
    '        'Dim strPathDb As String
    '        'strPathDb = ConfigurationSettings.AppSettings("PathDb")

    '        mail.From = ConfigurationSettings.AppSettings("mailsenderid")

    '        If objconn.connect() Then

    '            StrBrquery = New StringBuilder
    '            StrBrquery.Append(" select mui.Email as studentemail,mc.Email as teacheremail from M_USER_INFO as mui ")
    '            StrBrquery.Append(" inner join M_Centers as mc on  mui.Center_ID=mc.Center_ID ")
    '            StrBrquery.Append(" where mui.Userid=" + Convert.ToString(Session.Item("userid")))

    '            myCommand = New SqlCommand(StrBrquery.ToString, objconn.MyConnection)
    '            myDataReader = myCommand.ExecuteReader()
    '            While myDataReader.Read
    '                mail.To = myDataReader.Item("studentemail")
    '                mail.Cc = myDataReader.Item("teacheremail")
    '            End While
    '            myCommand = Nothing
    '            myDataReader = Nothing
    '            StrBrquery = Nothing
    '            objconn.disconnect()
    '        End If



    '        ' mail.To = "bharat@usindia.com"
    '        ' mail.Cc = "priyesh@usindia.com"

    '        'Replace templat containt
    '        objCommFun = New CommonFunction()
    '        strMessage = objCommFun.ReadFile(Server.MapPath(ConfigurationSettings.AppSettings("ExamResultMail")))
    '        strMessage = strMessage.Replace("&#Name#&", Convert.ToString(objHashTable("Name")))
    '        strMessage = strMessage.Replace("&#ClassName#&", Convert.ToString(objHashTable("Centrename")))
    '        strMessage = strMessage.Replace("&#TestName#&", Convert.ToString(objHashTable("Coursename")))
    '        strMessage = strMessage.Replace("&#Date#&", Convert.ToString(objHashTable("Date")))
    '        strMessage = strMessage.Replace("&#TotalMarks#&", Convert.ToString(objHashTable("TotalMark")))
    '        strMessage = strMessage.Replace("&#Marksobtained#&", Convert.ToString(objHashTable("MarksObtained")))
    '        strMessage = strMessage.Replace("&#status#&", Convert.ToString(objHashTable("Status")))

    '        mail.Subject = "Exam Result (" + Convert.ToString(objHashTable("Coursename")) + ")"
    '        mail.Body = strMessage
    '        mail.BodyFormat = MailFormat.Html
    '        Dim attachment As System.Web.Mail.MailAttachment
    '        attachment = New System.Web.Mail.MailAttachment(Server.MapPath("Excel Import\StudentExamDetails.xls"))
    '        mail.Attachments.Add(attachment)

    '        strSmtpServer = ConfigurationSettings.AppSettings("SmtpServer")
    '        mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtsperver") = ConfigurationSettings.AppSettings("SmtpServerIP")
    '        mail.Fields("http://schemas.microsoft.com/cdo/configuration/smtpserverport") = ConfigurationSettings.AppSettings("SmtpServerPort")
    '        mail.Fields("http://schemas.microsoft.com/cdo/configuration/sendusing") = 2
    '        SmtpMail.SmtpServer = strSmtpServer
    '        SmtpMail.Send(mail)

    '        System.IO.File.Delete(Server.MapPath("Excel Import\StudentExamDetails.xls"))

    '    End If

    'End Sub
    '    Public Sub ExportExamDetails()
    '        Dim App As Microsoft.Office.Interop.Excel.Application = Nothing
    '        Dim WorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
    '        Dim WorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
    '        Dim Sheets As Microsoft.Office.Interop.Excel.Worksheets = Nothing
    '        Dim objOpt As Object = System.Reflection.Missing.Value
    '        Dim objCn As New ConnectDb
    '        Dim Sheet As Excel.Worksheet
    '        Dim sheet1 As Excel.Worksheet
    '        Dim fileName1 As Object
    '        Dim strpath, strCorrectOpt, strGivenAns As String
    '        Dim strDateTime(2) As String
    '        Dim dsDateTime, dsQuestion, dsOpt, dsCrrctOpt, dsGivenOpt, dsTotalMarks As DataSet
    '        Dim da As SqlDataAdapter
    '        Dim sb As StringBuilder
    '        Dim rows As Integer = 6
    '        Dim start As Integer = 6
    '        Dim ends As Integer = 6
    '        Dim TotalMarks, MarkObtain As Integer
    '        Dim ImgNotFound(1) As String
    '        Dim pic As Object
    '        Dim arrCrrAns(), arrGivenAns() As String

    '        Try

    '            'Constants
    '            Const xlEdgeLeft = 7
    '            Const xlEdgeTop = 8
    '            Const xlEdgeBottom = 9
    '            Const xlEdgeRight = 10
    '            'Create  Spreadsheet
    '            Dim datestart As Date = Date.Now 'Added by Pragnesha Kulkarni on 2018/06/01 for stop Excel process
    '            App = New Excel.Application
    '            ' App = CreateObject("Excel.Application")
    '            Dim myWorkBook As Excel.Workbook = App.Workbooks.Open(Server.MapPath("Excel Import\Student Exam Details.XLT"), 0, False, 5, "", "", False, Excel.XlPlatform.xlWindows, "", True, False, 0, True)
    '            'WorkBook = DirectCast(WorkBooks.Add(objOpt), Excel.Workbook)
    '            sheet1 = myWorkBook.Worksheets("Exam Details")
    '            sheet1.Activate()
    '            ''Batch Name
    '            With App.ActiveSheet.Range("D2:D2")
    '                .MergeCells = True
    '                '.Interior.ColorIndex = 40
    '                .Font.Bold = True
    '                '.Font.ColorIndex = 53
    '                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '                '.Cells.Value = sel_subjectid.SelectedItem.Text
    '                .Cells.Value = Convert.ToString(objHashTable("Centrename"))
    '            End With



    '            ' ''Course Name
    '            With App.ActiveSheet.Range("D3:D3")
    '                .MergeCells = True
    '                '.Interior.ColorIndex = 40
    '                .Font.Bold = True
    '                '.Font.ColorIndex = 53
    '                .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '                '.Cells.Value = ddlcourse.SelectedItem.Text
    '                .Cells.Value = Convert.ToString(objHashTable("Coursename"))

    '            End With



    '            'strpath = ConfigurationSettings.AppSettings("PathDb")

    '            If objCn.connect() Then

    '                'Get date & time and total marks from database
    '                sb = New StringBuilder
    '                sb.Append("select T_Candidate_Status.Written_test_Appear_Date,M_Course.Total_marks  ")
    '                sb.Append("from T_Candidate_Status inner join M_Course on M_Course.Course_id=T_Candidate_Status.Course_ID")
    '                sb.Append(" where T_Candidate_Status.Course_ID=")
    '                sb.Append(_CourseID)
    '                If (_CandidateID = 0) Then
    '                Else
    '                    sb.Append("and T_Candidate_Status.UserId=")
    '                    sb.Append(_CandidateID)
    '                End If

    '                dsDateTime = New DataSet()

    '                da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                da.Fill(dsDateTime)
    '                If dsDateTime.Tables(0).Rows.Count <> 0 Then
    '                    Dim dateTime As String = dsDateTime.Tables(0).Rows(0).Item(0).ToString
    '                    strDateTime = dateTime.Split("/")
    '                End If


    '                sb = Nothing
    '                da = Nothing
    '                sb = New StringBuilder
    '                '********************************************************

    '                sb.Append("SELECT TEMP.qno, " & vbCrLf)
    '                sb.Append("       Sum(TEMP.obtained_marks)AS obtained_marks, " & vbCrLf)
    '                sb.Append("       TEMP.course_id, " & vbCrLf)
    '                sb.Append("       TEMP.userid, " & vbCrLf)
    '                sb.Append("       TEMP.question, " & vbCrLf)
    '                sb.Append("       TEMP.total_marks, " & vbCrLf)
    '                sb.Append("       TEMP.test_type, " & vbCrLf)
    '                sb.Append("       TEMP.test_name          AS subjectname, " & vbCrLf)
    '                sb.Append("       TEMP.Names " & vbCrLf)
    '                sb.Append("FROM   (SELECT tr.qno, " & vbCrLf)
    '                sb.Append("               ( CASE " & vbCrLf)
    '                sb.Append("                   WHEN mq.qn_category_id = 3 THEN ( " & vbCrLf)
    '                sb.Append("                   CASE " & vbCrLf)
    '                sb.Append("                     WHEN mqa.sub_id = tro.sub_id THEN ( CASE " & vbCrLf)
    '                sb.Append("                     WHEN " & vbCrLf)
    '                sb.Append("                     tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '                sb.Append("                                                         THEN " & vbCrLf)
    '                sb.Append("                     Count(mqa.correct_opt_id) " & vbCrLf)
    '                sb.Append("                     ELSE " & vbCrLf)
    '                sb.Append("                     0 " & vbCrLf)
    '                sb.Append("                                                         END " & vbCrLf)
    '                sb.Append("                     ) " & vbCrLf)
    '                sb.Append("                     ELSE 0 " & vbCrLf)
    '                sb.Append("                   END ) " & vbCrLf)
    '                sb.Append("                   WHEN mq.qn_category_id = 2 THEN ( CASE " & vbCrLf)
    '                sb.Append("                                                       WHEN " & vbCrLf)
    '                sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '                sb.Append("                                                     THEN " & vbCrLf)
    '                sb.Append("                   Count(mqa.correct_opt_id) " & vbCrLf)
    '                sb.Append("                                                       ELSE 0 " & vbCrLf)
    '                sb.Append("                                                     END ) " & vbCrLf)
    '                sb.Append("                   WHEN mq.qn_category_id = 1 THEN ( CASE " & vbCrLf)
    '                sb.Append("                                                       WHEN " & vbCrLf)
    '                sb.Append("                   tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '                sb.Append("                                                     THEN Sum(mq.total_marks) " & vbCrLf)
    '                sb.Append("                                                       ELSE 0 " & vbCrLf)
    '                sb.Append("                                                     END ) " & vbCrLf)
    '                sb.Append("                 END )                    AS obtained_marks, " & vbCrLf)
    '                sb.Append("               mc.course_id, " & vbCrLf)
    '                sb.Append("               mui.userid, " & vbCrLf)
    '                sb.Append("               Isnull(mui.surname, '') + ' ' + Isnull(mui.Name, '') + ' ' + " & vbCrLf)
    '                sb.Append("               Isnull(mui.Middlename, '') AS Names, " & vbCrLf)
    '                sb.Append("               mq.question, " & vbCrLf)
    '                sb.Append("               mq.total_marks, " & vbCrLf)
    '                sb.Append("               mq.test_type, " & vbCrLf)
    '                sb.Append("               mti.test_name, " & vbCrLf)
    '                sb.Append("               mui.Center_id, " & vbCrLf)
    '                sb.Append("               mui.Delete_Flg " & vbCrLf)
    '                sb.Append("        FROM   m_question AS mq " & vbCrLf)
    '                sb.Append("               LEFT JOIN m_question_answer AS mqa " & vbCrLf)
    '                sb.Append("                 ON mqa.qn_id = mq.qnid " & vbCrLf)
    '                sb.Append("                    AND mqa.test_type = mq.test_type " & vbCrLf)
    '                sb.Append("               LEFT JOIN t_result AS tr " & vbCrLf)
    '                sb.Append("                 ON tr.qno = mq.qnid " & vbCrLf)
    '                sb.Append("                    AND tr.qno = mqa.qn_id " & vbCrLf)
    '                sb.Append("                    AND tr.test_type = mq.test_type " & vbCrLf)
    '                sb.Append("               LEFT JOIN m_user_info AS mui " & vbCrLf)
    '                sb.Append("                 ON mui.userid = tr.userid " & vbCrLf)
    '                sb.Append("               LEFT JOIN M_Centers AS msc " & vbCrLf)
    '                sb.Append("                 ON msc.Center_id = mui.Center_ID " & vbCrLf)
    '                sb.Append("               LEFT JOIN m_course AS mc " & vbCrLf)
    '                sb.Append("                 ON mc.course_id = tr.course_id " & vbCrLf)
    '                sb.Append("               LEFT JOIN T_Center_Course AS tcc " & vbCrLf)
    '                sb.Append("                 ON tcc.course_id = mc.course_id " & vbCrLf)
    '                sb.Append("                    AND tcc.Center_ID = msc.Center_id " & vbCrLf)
    '                sb.Append("               LEFT JOIN m_testinfo AS mti " & vbCrLf)
    '                sb.Append("                 ON mti.test_type = tr.test_type " & vbCrLf)
    '                sb.Append("                    AND mti.test_type = mq.test_type " & vbCrLf)
    '                sb.Append("               LEFT JOIN dbo.T_User_Course AS tuc " & vbCrLf)
    '                sb.Append("                 ON tuc.course_id = mc.course_id " & vbCrLf)
    '                sb.Append("                    AND tuc.user_id = mui.userid " & vbCrLf)
    '                sb.Append("                    AND tuc.Test_type = mti.test_type " & vbCrLf)
    '                sb.Append("               LEFT JOIN t_result_option AS tro " & vbCrLf)
    '                sb.Append("                 ON tro.result_id = tr.result_id " & vbCrLf)
    '                sb.Append("                    AND tr.test_type = mti.test_type " & vbCrLf)
    '                sb.Append("                    AND tro.option_id = mqa.correct_opt_id " & vbCrLf)
    '                sb.Append("        GROUP  BY mq.total_marks, " & vbCrLf)
    '                sb.Append("                  mc.course_id, " & vbCrLf)
    '                sb.Append("                  mq.test_type, " & vbCrLf)
    '                sb.Append("                  mti.test_name, " & vbCrLf)
    '                sb.Append("                  mq.total_marks, " & vbCrLf)
    '                sb.Append("                  mqa.sub_id, " & vbCrLf)
    '                sb.Append("                  tr.qno, " & vbCrLf)
    '                sb.Append("                  mq.question, " & vbCrLf)
    '                sb.Append("                  mq.total_marks, " & vbCrLf)
    '                sb.Append("                  mqa.correct_opt_id, " & vbCrLf)
    '                sb.Append("                  tro.option_id, " & vbCrLf)
    '                sb.Append("                  mq.qn_category_id, " & vbCrLf)
    '                sb.Append("                  tro.sub_id, " & vbCrLf)
    '                sb.Append("                  mui.userid, " & vbCrLf)
    '                sb.Append("                  Isnull(mui.surname, '') + ' ' + Isnull(mui.Name, '') + ' ' + " & vbCrLf)
    '                sb.Append("                  Isnull(mui.Middlename, ''), " & vbCrLf)
    '                sb.Append("                  mui.Center_id, " & vbCrLf)
    '                sb.Append("                  mui.Delete_Flg)TEMP " & vbCrLf)
    '                sb.Append("--left join M_Options as mop   " & vbCrLf)
    '                sb.Append("--                    on mop.qnid=temp.qno and mop.test_type=temp.test_type " & vbCrLf)
    '                sb.Append("WHERE Temp.Center_ID=" & _CenterID & " and  TEMP.course_id = " & vbCrLf)
    '                sb.Append(_CourseID & vbCrLf)
    '                If (_CandidateID = 0) Then
    '                Else
    '                    sb.Append("       AND TEMP.userid =  " & vbCrLf)
    '                    sb.Append(_CandidateID)
    '                End If
    '                sb.Append("       AND TEMP.Delete_Flg = 0 " & vbCrLf)
    '                sb.Append("GROUP  BY TEMP.qno, " & vbCrLf)
    '                sb.Append("          TEMP.course_id, " & vbCrLf)
    '                sb.Append("          TEMP.userid, " & vbCrLf)
    '                sb.Append("          TEMP.Names, " & vbCrLf)
    '                sb.Append("          TEMP.question, " & vbCrLf)
    '                sb.Append("          TEMP.total_marks, " & vbCrLf)
    '                sb.Append("          TEMP.test_type, " & vbCrLf)
    '                sb.Append("          TEMP.test_name, " & vbCrLf)
    '                sb.Append("          TEMP.Delete_Flg -- order by temp.qno   " & vbCrLf)
    '                sb.Append("ORDER  BY TEMP.Names, " & vbCrLf)
    '                sb.Append("          TEMP.test_name")


    '                Dim st As String = sb.ToString
    '                dsQuestion = New DataSet
    '                da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                da.Fill(dsQuestion)
    '                '  If dsQuestion.Tables(0).Rows.Count <> 0 Then



    '                For i As Integer = 0 To dsQuestion.Tables(0).Rows.Count - 1


    '                    sb = Nothing
    '                    da = Nothing
    '                    sb = New StringBuilder
    '                    dsOpt = New DataSet
    '                    'get options of the question
    '                    sb.Append(" Select mop.Optionid,mop.[Option],mop.test_type,mop.qnid,mq.question ,mq.Qn_Category_ID ")
    '                    sb.Append(" from M_Options as mop ")
    '                    sb.Append(" LEFT join m_question as mq ")
    '                    sb.Append(" on mq.qnid=mop.qnid ")
    '                    sb.Append(" and mq.test_type=mop.test_type ")
    '                    sb.Append(" where mop.Test_Type= " & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " and mop.Qnid=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " order by mop.Optionid ")
    '                    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                    da.Fill(dsOpt)
    '                    For opt As Integer = 0 To dsOpt.Tables(0).Rows.Count - 1
    '                        ImgNotFound = CheckImage(dsOpt.Tables(0).Rows(opt).Item(1).ToString)
    '                        If ImgNotFound(0) = "" Then
    '                            App.ActiveSheet.Cells(rows, 6).Value = dsOpt.Tables(0).Rows(opt).Item(1).ToString
    '                            App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                            App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                            'App.ActiveSheet.Cells(11+i, 5+opt).ColumnWidth = 30
    '                            App.ActiveSheet.Cells(rows, 6).WrapText = True
    '                            App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
    '                            'App.ActiveSheet.Cells(11+i, 5+opt).RowHeight = 15

    '                        Else
    '                            If System.IO.File.Exists(Server.MapPath(ImgNotFound(0))) Then
    '                                App.ActiveSheet.Cells(rows, 6).Value = ImgNotFound(1)
    '                                Dim Align As Integer = 0
    '                                If ImgNotFound(1) <> "" Then
    '                                    Align = 12
    '                                End If
    '                                pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(ImgNotFound(0)))
    '                                With pic
    '                                    .top = App.ActiveSheet.Cells(rows, 6).top + 12
    '                                    .left = App.ActiveSheet.Cells(rows, 6).left + 15
    '                                    .width = 70
    '                                    .height = 70
    '                                End With

    '                                App.ActiveSheet.Cells(rows, 6).RowHeight = 83
    '                                App.ActiveSheet.Cells(rows, 6).ColumnWidth = 15
    '                                App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
    '                                App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignJustify
    '                                App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                            Else
    '                                App.ActiveSheet.Cells(rows, 6).Value = "Image not Found"
    '                                App.ActiveSheet.Cells(rows, 6).Borders.Weight = 2
    '                                App.ActiveSheet.Cells(rows, 6).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                                App.ActiveSheet.Cells(rows, 6).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                            End If

    '                        End If
    '                        rows += 1

    '                    Next
    '                    ends = rows - 1

    '                    sb = Nothing
    '                    da = Nothing
    '                    sb = New StringBuilder
    '                    dsCrrctOpt = New DataSet
    '                    'get correct option of the question
    '                    sb.Append(" Select M_Question_Answer.Qn_ID,M_Question_Answer.Correct_Opt_Id as correctid,M_Question_Answer.Test_Type ,M_Options.[Option] as givenAnswer ,M_Question_Answer.sub_id ")
    '                    sb.Append(" from M_Question_Answer  ")
    '                    sb.Append(" inner join M_Options ")
    '                    sb.Append(" on M_Options.Optionid=M_Question_Answer.Correct_Opt_Id ")
    '                    sb.Append(" and M_Options.test_type=M_Question_Answer.test_type ")
    '                    sb.Append(" and M_Options.Qnid=M_Question_Answer.Qn_ID ")
    '                    sb.Append(" where M_Question_Answer.Test_Type=" & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " ")
    '                    sb.Append(" and M_Question_Answer.Qn_ID= " & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " ")
    '                    sb.Append(" order by M_Question_Answer.sub_id ")
    '                    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                    da.Fill(dsCrrctOpt)
    '                    For crr As Integer = 0 To dsCrrctOpt.Tables(0).Rows.Count - 1
    '                        strCorrectOpt += dsCrrctOpt.Tables(0).Rows(crr).Item(3).ToString + Environment.NewLine
    '                        arrCrrAns = strCorrectOpt.Split(Environment.NewLine)
    '                    Next

    '                    sb = Nothing
    '                    da = Nothing
    '                    sb = New StringBuilder
    '                    dsGivenOpt = New DataSet
    '                    'get answer given by the student
    '                    sb.Append(" select  Distinct tr.userid,tr.test_type,tr.qno,tro.option_id as givemid,mop.[Option] , tro.sub_id ")
    '                    sb.Append(" from T_Result as tr ")
    '                    sb.Append(" left join T_Result_Option as tro ")
    '                    sb.Append(" on tro.result_id=tr.result_id ")
    '                    sb.Append(" left join M_Options as mop ")
    '                    sb.Append(" on mop.Optionid=tro.option_id ")
    '                    sb.Append(" and mop.test_type=tr.test_type ")
    '                    sb.Append(" and mop.Qnid=tr.qno ")
    '                    sb.Append(" where tr.test_type=" & dsQuestion.Tables(0).Rows(i).Item(6).ToString & " ")
    '                    sb.Append(" and tr.qno=" & dsQuestion.Tables(0).Rows(i).Item(0).ToString & " and tr.userid= " & dsQuestion.Tables(0).Rows(i).Item(3).ToString & " and  tr.Course_id=" & _CourseID & "")
    '                    sb.Append(" order by tro.sub_id ")
    '                    Dim bb As String = sb.ToString
    '                    da = New SqlDataAdapter(sb.ToString, objCn.MyConnection)
    '                    da.Fill(dsGivenOpt)
    '                    For given As Integer = 0 To dsGivenOpt.Tables(0).Rows.Count - 1
    '                        If dsGivenOpt.Tables(0).Rows(given).Item(4).ToString <> "" Then
    '                            strGivenAns += dsGivenOpt.Tables(0).Rows(given).Item(4).ToString + Environment.NewLine
    '                            arrGivenAns = strGivenAns.Split(Environment.NewLine)
    '                        Else
    '                            strGivenAns = "Not Attempted"

    '                        End If

    '                    Next

    '                    'Serail number
    '                    With App.ActiveSheet.Range("B" + start.ToString + ":B" + ends.ToString)
    '                        .MergeCells = True
    '                        '.Interior.ColorIndex = 40
    '                        '.Font.Bold = True
    '                        ' .Font.ColorIndex = 53
    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        .Cells.Value = i + 1
    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                        .WrapText = True
    '                    End With

    '                    'subject name
    '                    With App.ActiveSheet.Range("C" + start.ToString + ":C" + ends.ToString)
    '                        .MergeCells = True
    '                        .WrapText = True
    '                        '.Interior.ColorIndex = 40
    '                        '.Font.Bold = True
    '                        ' .Font.ColorIndex = 53
    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(7).ToString
    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                    End With
    '                    'student Name
    '                    With App.ActiveSheet.Range("D" + start.ToString + ":D" + ends.ToString)
    '                        .MergeCells = True
    '                        .WrapText = True
    '                        '.Interior.ColorIndex = 40
    '                        '.Font.Bold = True
    '                        ' .Font.ColorIndex = 53
    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(8).ToString
    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                    End With
    '                    'Question
    '                    With App.ActiveSheet.Range("E" + start.ToString + ":E" + ends.ToString)
    '                        .MergeCells = True
    '                        .WrapText = True
    '                        '.Interior.ColorIndex = 40
    '                        ' .Font.Bold = True
    '                        ' .Font.ColorIndex = 53
    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

    '                        If dsQuestion.Tables(0).Rows.Count <> 0 Then

    '                            Dim check() As String = CheckImage(dsQuestion.Tables(0).Rows(i).Item(4).ToString)
    '                            If check(0) = "" Then
    '                                .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(4).ToString
    '                            ElseIf check(0) <> "" Then

    '                                If System.IO.File.Exists(Server.MapPath(check(0))) Then
    '                                    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
    '                                    With pic

    '                                        'Added by Pragnesha Kulkarni on 2018-06-22 
    '                                        'Desc: Adjust Image on ExamResult Template
    '                                        .top = App.ActiveSheet.Cells(start, 5).top + 5
    '                                        .left = App.ActiveSheet.Cells(start, 5).left + 80
    '                                        .width = 100
    '                                        .height = 70
    '                                        ' commented by Pragnesha [Purpose: Question and Question image overlapping on each other]
    '                                        '.top = App.ActiveSheet.Cells(start + 1, 6).top + 15
    '                                        '.left = App.ActiveSheet.Cells(start + 1, 6).left + 15
    '                                        '.width = 83
    '                                        '.height = 83 
    '                                        'Ended by Pragnesha Kulkarni

    '                                    End With
    '                                    .Cells.Value = check(1).ToString

    '                                    'Added by Pragnesha Kulkarni on 2018-06-22 
    '                                    'Desc: Adjust Image on ExamResult Template
    '                                    .VerticalAlignment = Excel.XlVAlign.xlVAlignTop
    '                                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft
    '                                    ' Commented by Pragnesha [Purpose: Question and Question image overlapping on each other]
    '                                    '.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                                    '.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter 
    '                                    'Ended by Pragnesha Kulkarni

    '                                Else
    '                                    .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
    '                                End If
    '                            Else
    '                            End If
    '                        End If
    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                    End With

    '                    'Correct option
    '                    With App.ActiveSheet.Range("G" + start.ToString + ":G" + ends.ToString)
    '                        .MergeCells = True
    '                        .WrapText = True
    '                        '.Interior.ColorIndex = 40
    '                        ' .Font.Bold = True

    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter

    '                        Dim check() As String = CheckImage(strCorrectOpt)
    '                        If check(0) = "" Then
    '                            .Cells.Value = strCorrectOpt
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


    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                    End With

    '                    'given answer by student
    '                    With App.ActiveSheet.Range("H" + start.ToString + ":H" + ends.ToString)
    '                        .MergeCells = True

    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter



    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                        .FormulaR1C1 = strGivenAns
    '                        .Characters(start:=1, Length:=0).Font.ColorIndex = Excel.Constants.xlAutomatic
    '                        If strGivenAns = "Not Attempted" Then
    '                            .Characters(start:=1, Length:=strGivenAns.Length).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

    '                        Else
    '                            Dim check() As String = CheckImage(strGivenAns)
    '                            Dim lenth As Integer
    '                            If check(0) = "" Then
    '                                If arrGivenAns.Length < arrCrrAns.Length Then
    '                                    lenth = arrGivenAns.Length
    '                                Else
    '                                    lenth = arrCrrAns.Length
    '                                End If


    '                                For Match As Integer = 0 To lenth - 1
    '                                    If arrCrrAns(Match) = arrGivenAns(Match) Then
    '                                        Dim jk As String = arrGivenAns(Match)

    '                                        If Match <> lenth - 1 Then
    '                                            .Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).Length + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Green)
    '                                        End If

    '                                        '.Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).LastIndexOf(arrGivenAns(Match)) + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Green)
    '                                    Else

    '                                        '.Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrGivenAns(Match).Length + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)
    '                                        .Characters(start:=strGivenAns.IndexOf(arrGivenAns(Match)), Length:=arrCrrAns(Match).LastIndexOf(arrGivenAns(Match)) + 1).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Red)

    '                                    End If

    '                                Next
    '                            ElseIf check(0) <> "" Then

    '                                If System.IO.File.Exists(Server.MapPath(check(0))) Then
    '                                    pic = App.ActiveSheet.Pictures.Insert(Server.MapPath(check(0)))
    '                                    With pic
    '                                        .top = App.ActiveSheet.Cells(start + 1, 7).top + 15
    '                                        .left = App.ActiveSheet.Cells(start + 1, 7).left + 15
    '                                        .width = 83
    '                                        .height = 83
    '                                    End With
    '                                    .Cells.Value = check(1).ToString
    '                                    .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                                    .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                                Else
    '                                    .Cells.Value = check(1).ToString + Environment.NewLine + "Image not found"
    '                                End If


    '                            Else


    '                            End If

    '                            'Apply the color to the second word

    '                        End If
    '                        'Apply the color to the third word
    '                        ' .Characters(start:=11, Length:=4).Font.Color = System.Drawing.ColorTranslator.ToOle(Color.Blue)
    '                        '.Columns.AutoFit()
    '                        .WrapText = True
    '                        strGivenAns = Nothing
    '                        strCorrectOpt = Nothing
    '                    End With

    '                    'total marks of the question
    '                    With App.ActiveSheet.Range("I" + start.ToString + ":I" + ends.ToString)
    '                        .MergeCells = True
    '                        .WrapText = True
    '                        '.Interior.ColorIndex = 40
    '                        '.Font.Bold = True
    '                        ' .Font.ColorIndex = 53
    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(5).ToString
    '                        TotalMarks += dsQuestion.Tables(0).Rows(i).Item(5)
    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                    End With

    '                    'mark obtain by the student
    '                    With App.ActiveSheet.Range("J" + start.ToString + ":J" + ends.ToString)
    '                        .MergeCells = True
    '                        .WrapText = True

    '                        .VerticalAlignment = Excel.XlVAlign.xlVAlignCenter
    '                        .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
    '                        .Cells.Value = dsQuestion.Tables(0).Rows(i).Item(1).ToString
    '                        MarkObtain += dsQuestion.Tables(0).Rows(i).Item(1)
    '                        '.Font.Size = 15
    '                        .BORDERS(xlEdgeLeft).Weight = 2
    '                        .BORDERS(xlEdgeTop).Weight = 2
    '                        .BORDERS(xlEdgeBottom).Weight = 2
    '                        .BORDERS(xlEdgeRight).Weight = 2
    '                    End With
    '                    start = ends + 1
    '                Next
    '            End If
    '            If Not objCn.MyConnection Is Nothing Then
    '                If objCn.MyConnection.State = ConnectionState.Open Then
    '                    objCn.disconnect()
    '                End If
    '            End If

    '            Sheet = myWorkBook.Worksheets("Summary")
    '            Dim pt As Microsoft.Office.Interop.Excel.PivotTable = Sheet.PivotTables("PivotTable1")
    '            pt.RefreshTable()
    '            Dim _PivotFeild As Excel.PivotFields = CType(pt.RowFields(Type.Missing), Excel.PivotFields)
    '            If dsQuestion.Tables(0).Rows.Count <> 0 Then
    '                For Each _PivotField1 As Excel.PivotField In _PivotFeild

    '                    If _PivotField1.Value.ToString() = "Subject Name" Then
    '                        Dim Items As Excel.PivotItems = DirectCast(_PivotField1.VisibleItems, Excel.PivotItems)
    '                        For Each _PivotItem As Excel.PivotItem In Items
    '                            If _PivotItem.Caption.Equals("(blank)") Then
    '                                _PivotItem.Visible = False
    '                            End If

    '                        Next
    '                    End If
    '                Next
    '            End If
    '            pt.RefreshTable()

    '            If System.IO.File.Exists(Server.MapPath("Excel Import\StudentExamDetails.xls")) Then
    '                System.IO.File.Delete(Server.MapPath("Excel Import\StudentExamDetails.xls"))
    '            End If

    '            fileName1 = Server.MapPath("Excel Import\StudentExamDetails.xls")

    '            myWorkBook.SaveAs(fileName1) ', objOpt, objOpt, objOpt, objOpt, objOpt,Excel.XlSaveAsAccessMode.xlExclusive, objOpt, objOpt, objOpt, objOpt)
    '            myWorkBook.Close()

    '            'Added by Pragnesha Kulkarni on 2018/06/01
    '            ' Reason:Excel process didn't stop after downloading excel sheet
    '            ' BugID: 719
    '            App.Quit()
    '            Dim dateEnd As Date = Date.Now
    '            'End_Excel_App(datestart, dateEnd)
    '            Excel_Stop()
    '            'Ended by Pragnesha Kulkarni on 2018/06/01

    '            Dim file As New IO.FileInfo(Server.MapPath("Excel Import\StudentExamDetails.xls"))
    '            'System.IO.File.Delete(Server.MapPath("Excel Import\StudentExamDetails.XLS"))
    '        Catch ex As Exception
    '            If log.IsDebugEnabled Then
    '                ' Dim err As String = ex.ToString()
    '                log.Error("Error occure on Export exam detail ")
    '                log.Debug("Error :" & ex.ToString())
    '                log.Debug("Error :" & ex.StackTrace)
    '            End If

    '            'Added by Pragnesha Kulkarni on 2018/06/01
    '            ' Reason:To release objects
    '            ' BugID: 719
    '            ReleaseObject(App)
    '            ReleaseObject(Sheet)

    '            ''Commented by Rahul Shukla on 2019/05/09
    '            ' Reason:Null Value was Passing to the object
    '            ' BugID: 904

    '            'ReleaseObject(WorkBooks)
    '            ' ReleaseObject(WorkBook)
    '            'ReleaseObject(Sheets)
    '            'ReleaseObject(objOpt)

    '            Throw ex
    '            'Ended by Pragnesha Kulkarni on 2018/06/01
    '            Response.Redirect("error.aspx", False)
    '        Finally
    '            If Not objCn.MyConnection Is Nothing Then
    '                If objCn.MyConnection.State = ConnectionState.Open Then
    '                    objCn.disconnect()
    '                End If
    '            End If
    '            'Added by Pragnesha Kulkarni on 2018/06/01
    '            ' Reason:To release objects
    '            ' BugID: 719
    '            ReleaseObject(App)
    '            ReleaseObject(Sheet)
    '            ReleaseObject(objOpt)

    '            ''Commented by Rahul Shukla on 2019/05/09
    '            ' Reason:Null Value was Passing to the object
    '            ' BugID: 904

    '            '' ReleaseObject(WorkBooks)
    '            ''ReleaseObject(WorkBook)
    '            '' ReleaseObject(Sheets)

    '            'Ended by Pragnesha Kulkarni on 2018/06/01

    '            App = Nothing
    '            WorkBooks = Nothing
    '            WorkBook = Nothing
    '            Sheet = Nothing
    '            Sheets = Nothing
    '            objOpt = Nothing
    '            objCn = Nothing
    '            sheet1 = Nothing
    '            ImgNotFound = Nothing
    '            pic = Nothing
    '            fileName1 = Nothing
    '            strpath = Nothing
    '            strCorrectOpt = Nothing
    '            strGivenAns = Nothing
    '            strDateTime = Nothing
    '            dsDateTime = Nothing
    '            dsQuestion = Nothing
    '            dsOpt = Nothing
    '            dsCrrctOpt = Nothing
    '            dsGivenOpt = Nothing
    '            dsTotalMarks = Nothing
    '            da = Nothing
    '            sb = Nothing
    '            ImgNotFound = Nothing
    '            arrCrrAns = Nothing
    '            arrGivenAns = Nothing
    '        End Try
    '    End Sub
    '    'Added by Pragnesha Kulkarni on 2018/06/01
    '    ' Reason:Excel process didn't stop after downloading excel sheet
    '    ' BugID: 719
    '    Private Sub ReleaseObject(ByVal obj As Object)
    '        Try
    '            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
    '            obj = Nothing
    '        Catch ex As Exception
    '            obj = Nothing
    '        Finally
    '            GC.Collect()
    '            GC.WaitForPendingFinalizers()
    '        End Try
    '    End Sub
    '    Private Sub End_Excel_App(ByVal datestart As Date, ByVal dateEnd As Date)

    '        'Dim xlp() As Process = Process.GetProcessesByName("EXCEL")
    '        'For Each Process As Process In xlp
    '        '    If Process.StartTime >= datestart And Process.StartTime <= dateEnd Then
    '        '        Process.Kill()
    '        '        Exit For
    '        '    End If
    '        'Next
    '    End Sub
    '    ' Bug ID: 0904 Getting error on end exam click
    '    ' Desc: Added code of excel process cpu utilization check and kill process as utilization will equal to zero.
    '    ' Added by Pragnesha on 23-5-2019
    '    Private Sub Excel_Stop()
    '        Dim pListOfProcesses() As Process
    '        Dim pExcelProcess As System.Diagnostics.Process
    '        pListOfProcesses = Process.GetProcesses
    '        For Each pExcelProcess In pListOfProcesses
    '            If pExcelProcess.ProcessName.ToUpper = "EXCEL" Then
    '                Dim myAppCpu As PerformanceCounter = New PerformanceCounter("Process", "% Processor Time", "EXCEL", True)


    '                Dim pct As Double = myAppCpu.NextValue()
    '                ' Console.WriteLine("EXCEL'S CPU % = " & pct)
    '                Thread.Sleep(250)
    '                If pct = 0.0 Then
    '                    pExcelProcess.Kill()
    '                End If

    '            End If
    '        Next
    '    End Sub
    '    'Ended by Pragnesha Kulkarni on 2018/06/04


    '#Region "CheckImage Function"
    '    'Added by    : Saraswati Patel
    '    'Description : For Check Whether string are html type or not

    '    Public Function CheckImage(ByVal value As String) As String()
    '        Dim str As String
    '        Dim strVal(1) As String
    '        Try
    '            If value.Contains("<br/>") Then
    '                'Dim a As String = (value.IndexOf("=") + 1)
    '                'Dim b As String = (value.IndexOf(" ", (value.IndexOf("=") + 1)))
    '                str = value.Substring((value.IndexOf("=") + 1), _
    '                                      ((value.IndexOf(" ", (value.IndexOf("=") + 1))) - (value.IndexOf("=") + 1)))
    '                strVal(0) = str
    '                strVal(1) = value.Substring(0, value.IndexOf("<"))
    '                'str = value.Substring(14, (value.IndexOf(" ", 15) - 14))
    '                Return strVal
    '            ElseIf value.Contains("<img") Then
    '                ' str = value.Substring((value.IndexOf("=") + 1), (value.IndexOf(" ", 15) - (value.IndexOf("=") + 1)))
    '                str = value.Substring((value.IndexOf("=") + 1), _
    '                                      ((value.IndexOf(" ", (value.IndexOf("=") + 1))) - (value.IndexOf("=") + 1)))
    '                strVal(0) = str
    '                strVal(1) = value.Substring(0, value.IndexOf("<"))
    '                Return strVal
    '            Else
    '                strVal(0) = ""
    '                strVal(1) = ""
    '            End If
    '            Return strVal
    '        Catch ex As Exception
    '            If log.IsDebugEnabled Then
    '                log.Debug("Error :" & ex.ToString())
    '            End If
    '            Throw ex
    '        Finally
    '            str = Nothing
    '            strVal = Nothing
    '        End Try
    '    End Function
    '#End Region

    '    'Method added by rajesh nagvanshi 2014/08/25
    '    Public Function GetID(ByVal query As String, ByVal ColumnName As String) As String
    '        Try
    '            Dim strQuery1 As String
    '            Dim objconn As New ConnectDb
    '            Dim myCommand As SqlCommand
    '            Dim myDataReader As SqlDataReader
    '            'Dim strPathDb As String
    '            Dim outValue As String = String.Empty
    '            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
    '            If objconn.connect() Then
    '                strQuery1 = query
    '                myCommand = New SqlCommand(strQuery1, objconn.MyConnection)
    '                myDataReader = myCommand.ExecuteReader()
    '                While myDataReader.Read
    '                    outValue = myDataReader.Item(ColumnName)
    '                End While
    '                myCommand = Nothing
    '                myDataReader = Nothing
    '                objconn.disconnect()
    '            End If
    '            Return outValue
    '        Catch ex As Exception
    '            Throw ex

    '        Finally

    '        End Try

    '    End Function

    '    Public Function IsFileInUse(path As String) As Boolean
    '        If IO.File.Exists(path) Then
    '            Try
    '                Using fs = IO.File.OpenWrite(path)
    '                    'If stream can write to the file, it suggests the file is not in use.
    '                    Return Not fs.CanWrite
    '                End Using
    '            Catch ex As Exception
    '                'An exception was raised when trying to create a write stream
    '                'This suggests the file is in use.
    '                Return True
    '            End Try
    '        Else
    '            'File does not exists, therefore it is not in use:
    '            'a file could be written anew to the provided path.
    '            Return False
    '        End If
    '    End Function


End Class