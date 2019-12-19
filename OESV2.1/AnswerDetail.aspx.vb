#Region "NameSpaces"
Imports System.Data.SqlClient
Imports log4net
Imports System.Web
#End Region

Partial Public Class AnswerDetail
#Region "Declaration"
    Inherits Basepage
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Answerdetail")
    Dim objconn As New ConnectDb
    'Dim strPathDb As String
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim strBldr As StringBuilder
        Dim query_Ques As String
        Dim query_CorrectOpt As String
        Dim query_GivenOpt As String
        Dim ar As ArrayList
        Dim intQuesCat As Integer

        Dim userid As String = Session("userid").ToString
        Dim testtype As String = Session("test_type").ToString
        Dim totMarks As String = Session("totMarks").ToString
        Dim obMarks As String = Session("obMarks").ToString
        Dim qno As String = Session("qno").ToString


        Try

            'strPathDb = ConfigurationSettings.AppSettings("PathDb")
            strBldr = New StringBuilder()

            strBldr.Append(" Select mop.Optionid,mop.[Option],mop.test_type,mop.qnid,mq.question ,mq.Qn_Category_ID ")
            strBldr.Append(" from M_Options as mop ")
            strBldr.Append(" LEFT join m_question as mq ")
            strBldr.Append(" on mq.qnid=mop.qnid ")
            strBldr.Append(" and mq.test_type=mop.test_type ")
            strBldr.Append(" where mop.Test_Type= " & testtype & " and mop.Qnid=" & qno & " order by mop.Optionid ")
            query_Ques = strBldr.ToString()

            strBldr.Remove(0, strBldr.Length - 1)

            strBldr.Append(" Select M_Question_Answer.Qn_ID,M_Question_Answer.Correct_Opt_Id as correctid,M_Question_Answer.Test_Type ,M_Options.[Option] as givenAnswer ,M_Question_Answer.sub_id ")
            strBldr.Append(" from M_Question_Answer  ")
            strBldr.Append(" inner join M_Options ")
            strBldr.Append(" on M_Options.Optionid=M_Question_Answer.Correct_Opt_Id ")
            strBldr.Append(" and M_Options.test_type=M_Question_Answer.test_type ")
            strBldr.Append(" and M_Options.Qnid=M_Question_Answer.Qn_ID ")
            strBldr.Append(" where M_Question_Answer.Test_Type=" & testtype & " ")
            strBldr.Append(" and M_Question_Answer.Qn_ID= " & qno & " ")
            strBldr.Append(" order by M_Question_Answer.sub_id ")
            query_CorrectOpt = strBldr.ToString()

            strBldr.Remove(0, strBldr.Length - 1)

            strBldr.Append(" select Distinct tr.userid,tr.test_type,tr.qno,tro.option_id as givemid,mop.[Option] , tro.sub_id ")
            strBldr.Append(" from T_Result as tr ")
            strBldr.Append(" left join T_Result_Option as tro ")
            strBldr.Append(" on tro.result_id=tr.result_id ")
            strBldr.Append(" left join M_Options as mop ")
            strBldr.Append(" on mop.Optionid=tro.option_id ")
            strBldr.Append(" and mop.test_type=tr.test_type ")
            strBldr.Append(" and mop.Qnid=tr.qno ")
            strBldr.Append(" where tr.test_type=" & testtype & " and tr.qno=" & qno & " and tr.userid= " & userid & " and  tr.Course_id=" & Session.Item("course") & "")
            strBldr.Append(" order by tro.sub_id ")
            query_GivenOpt = strBldr.ToString()

            strBldr.Remove(0, strBldr.Length - 1)

            If objconn.connect() Then

                ' ************* Code for Question 

                da = New SqlDataAdapter(query_Ques, objconn.MyConnection)
                ds = New DataSet
                da.Fill(ds)

                Dim tblq As New HtmlTable
                Dim trq As New HtmlTableRow
                Dim tdq As New HtmlTableCell
                'tdq.Attributes.Add("class", "tdcontent_labelQues")
                Dim ques As String = "<strong>" & Resources.Resource.ExamCount_Question & ": " & Session("srno").ToString & "</strong> " & ds.Tables(0).Rows(0).Item(4).ToString
                intQuesCat = CInt(ds.Tables(0).Rows(0).Item(5).ToString)
                tdq.InnerHtml = ques
                trq.Cells.Add(tdq)
                tblq.Rows.Add(trq)
                tdQues.Controls.Add(tblq)
                objconn.disconnect()

                ' ************* Code for Options 

                Dim tblo As New HtmlTable
                Dim tdo As New HtmlTableCell
                Dim tro As New HtmlTableRow
                Dim troup As New HtmlTableRow

                'Dim text As String = " <b>Options</b> <br> <hr  style='color:#cccccc' />  "
                'tdo.InnerHtml = text
                'troup.Cells.Add(tdo)
                'tblo.Rows.Add(troup)
                'tdo.InnerHtml = ""

                Dim cnt As Integer = 1
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    Dim tr1 As New HtmlTableRow
                    Dim td1 As New HtmlTableCell
                    Dim td2 As New HtmlTableCell
                    td1.Attributes.Add("class", "tddata")
                    td1.InnerHtml = cnt & "."
                    td2.InnerHtml = "&nbsp;" & ds.Tables(0).Rows(i).Item(1).ToString
                    td2.Attributes.Add("class", "tddata")
                    tr1.Cells.Add(td1)
                    tr1.Cells.Add(td2)
                    tblo.Rows.Add(tr1)
                    cnt = cnt + 1
                Next

                tro.Cells.Add(tdo)
                tblo.Rows.Add(tro)
                tdOptions.Controls.Add(tblo)




                ' ************* Clear before reuse then re-fill
                ds.Clear()
                da = Nothing


                da = New SqlDataAdapter(query_CorrectOpt, objconn.MyConnection)
                ds = New DataSet
                da.Fill(ds)

                ' ************* Code for Corrct Options
                Dim tblco As New HtmlTable
                Dim tdco As New HtmlTableCell
                Dim trco As New HtmlTableRow
                'Dim textco As String = " <b>Correct Options</b> <br> <hr  style='color:#cccccc' />  "
                'tdco.InnerHtml = textco
                ar = New ArrayList
                Dim cntco As Integer = 0
                Dim htcopts As New Hashtable
                'Dim ids As String
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    If intQuesCat = 3 Then
                        If (cntco <> CInt(ds.Tables(0).Rows(i).Item(4).ToString)) Then
                            cntco = cntco + 1
                            ar.Add(ds.Tables(0).Rows(i).Item(3).ToString)
                        End If
                    Else
                        cntco = cntco + 1
                        ar.Add(ds.Tables(0).Rows(i).Item(3).ToString)
                    End If

                    Dim tr1 As New HtmlTableRow
                    Dim td1 As New HtmlTableCell
                    Dim td2 As New HtmlTableCell
                    td1.Attributes.Add("class", "tddata")
                    td1.InnerHtml = cntco & "."
                    td2.InnerHtml = "&nbsp;" & ds.Tables(0).Rows(i).Item(3).ToString
                    td2.Attributes.Add("class", "tddata")
                    tr1.Cells.Add(td1)
                    tr1.Cells.Add(td2)
                    tblco.Rows.Add(tr1)

                Next

                trco.Cells.Add(tdco)
                tblco.Rows.Add(trco)
                tdCorrectOption.Controls.Add(tblco)




                ' ************* Clear before reuse then re-fill
                ds.Clear()
                da = Nothing


                da = New SqlDataAdapter(query_GivenOpt, objconn.MyConnection)
                ds = New DataSet
                da.Fill(ds)

                ' ************* Code for Given Options
                Dim tblgo As New HtmlTable
                Dim tdgo As New HtmlTableCell
                Dim trgo As New HtmlTableRow
                Dim NoAttempt As Boolean = False


                Dim cntgo As Integer = 1
                For i As Integer = 0 To ds.Tables(0).Rows.Count - 1
                    ' If Not attempted
                    For k As Integer = 0 To ds.Tables(0).Rows.Count - 1

                    Next
                    If ds.Tables(0).Rows(i).Item(4).ToString = "" Then
                        tdgo.InnerHtml = Resources.Resource.Ans_Not
                        tdgo.Attributes.Add("class", "wrong")
                        'trgo.Cells.Add(tdgo)
                        'tblgo.Rows.Add(trgo)
                        'tdGivenAnswer.Controls.Add(tblgo)
                        Exit For
                    End If

                    Dim tr1 As New HtmlTableRow
                    Dim td1 As New HtmlTableCell
                    Dim td2 As New HtmlTableCell
                    td1.InnerHtml = cntgo & "."
                    td1.Width = "5%"
                    td1.Attributes.Add("class", "tddata")
                    If intQuesCat = 3 Then
                        If ar.Count > 0 Then
                            If i <= ar.Count - 1 Then
                                If (ds.Tables(0).Rows(i).Item(4).ToString = ar(i).ToString) Then
                                    td2.Attributes.Add("class", "correct")
                                Else
                                    td2.Attributes.Add("class", "wrong")
                                End If
                            End If
                        End If
                    Else
                        If ar.Count > 0 Then
                            For j As Integer = 0 To ar.Count - 1
                                If ar.Contains(ds.Tables(0).Rows(i).Item(4).ToString) = True Then
                                    td2.Attributes.Add("class", "correct")
                                Else
                                    td2.Attributes.Add("class", "wrong")
                                End If

                            Next
                        End If
                    End If

                    td2.InnerHtml = "&nbsp;" & ds.Tables(0).Rows(i).Item(4).ToString
                    td2.Width = "95%"
                    tr1.Cells.Add(td1)
                    tr1.Cells.Add(td2)
                    tblgo.Rows.Add(tr1)
                    cntgo = cntgo + 1
                Next

                trgo.Cells.Add(tdgo)
                tblgo.Rows.Add(trgo)
                tdGivenAnswer.Controls.Add(tblgo)


                lblMarksObtained.Text = Resources.Resource.CandStatus_MrkObt & ": " & obMarks
                lblTotalMarks.Text = Resources.Resource.CourseMaintenance_TtlMrks & ": " & totMarks
                objconn.disconnect()
            End If
        Catch ex As Exception
            If objconn.MyConnection.State = ConnectionState.Open Then
                objconn.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        Finally
            ar = Nothing
            objconn = Nothing
            strBldr = Nothing
            da = Nothing
            ds = Nothing
        End Try



    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        Try
            If Request.QueryString("pi") <> Nothing Then
                Session.Add("pid", Request.QueryString("pi").ToString)
                Response.Redirect("AppearedExam.aspx", False)
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx", False)
        End Try

    End Sub
End Class