Imports System.Data.SqlClient

Partial Public Class WebForm3
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Add("check", "true")
        txtAppFromDate.Attributes.Add("type", "date")
        txtAppToDate.Attributes.Add("type", "date")
        If Not IsPostBack Then
            LoadClasses()
            If ddlClass.SelectedIndex <> 0 Then
                LoadCourses()
            End If
        End If
    End Sub
    Protected Sub LoadClasses()
        Dim ObjDS As DataSet
        Dim ObjDA As SqlDataAdapter
        Dim strQuery As StringBuilder
        Dim Objconn As New ConnectDb

        Try
            If Objconn.connect() Then
                strQuery = New StringBuilder()
                strQuery.Append("Select Center_ID, Center_Name from M_Centers ")
                ObjDA = New SqlDataAdapter(strQuery.ToString(), Objconn.MyConnection)
                ObjDS = New DataSet()
                ObjDA.Fill(ObjDS)
                ddlClass.Items.Clear()
                Dim objItem As ListItem
                objItem = New ListItem()
                objItem.Text = "--- Select ---"
                objItem.Value = 0
                ddlClass.Items.Insert(0, objItem)
                ddlClass.SelectedIndex = 0
                objItem = Nothing

                If ObjDS.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ObjDS.Tables(0).Rows.Count - 1
                        objItem = New ListItem()
                        objItem.Text = ObjDS.Tables(0).Rows(i)(1).ToString()
                        objItem.Value = ObjDS.Tables(0).Rows(i)(0).ToString()
                        ddlClass.Items.Add(objItem)
                        objItem = Nothing
                    Next
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString())
        Finally
            Objconn.disconnect()
            Objconn = Nothing
            ObjDS = Nothing
            ObjDA = Nothing
            strQuery = Nothing
        End Try

    End Sub
    Protected Sub LoadCourses()
        Dim ObjDS As DataSet
        Dim ObjDA As SqlDataAdapter
        Dim strQuery As StringBuilder
        Dim Objconn As New ConnectDb

        Try
            If Objconn.connect() Then
                strQuery = New StringBuilder()
                strQuery.Append("Select Course_id, Course_name from m_Course ")
                strQuery.Append("where course_id in (select course_id from t_center_course where center_id=" + ddlClass.SelectedValue + ")")
                ObjDA = New SqlDataAdapter(strQuery.ToString(), Objconn.MyConnection)
                ObjDS = New DataSet()
                ObjDA.Fill(ObjDS)
                ddlCourse.Items.Clear()
                Dim objItem As ListItem
                objItem = New ListItem()
                objItem.Text = "--- Select ---"
                objItem.Value = 0
                ddlCourse.Items.Insert(0, objItem)
                ddlCourse.SelectedIndex = 0
                objItem = Nothing
                If ObjDS.Tables(0).Rows.Count > 0 Then
                    For i As Integer = 0 To ObjDS.Tables(0).Rows.Count - 1
                        objItem = New ListItem()
                        objItem.Text = ObjDS.Tables(0).Rows(i)(1).ToString()
                        objItem.Value = ObjDS.Tables(0).Rows(i)(0).ToString()
                        ddlCourse.Items.Add(objItem)
                        objItem = Nothing
                    Next
                End If
            End If

        Catch ex As Exception
            Response.Write(ex.ToString())

        Finally
            Objconn.disconnect()
            Objconn = Nothing
            ObjDS = Nothing
            ObjDA = Nothing
            strQuery = Nothing
        End Try
    End Sub
    Protected Sub BindGrid()
        Dim ObjDS As DataSet
        Dim ObjDA As SqlDataAdapter
        Dim strQuery As StringBuilder
        Dim Objconn As New ConnectDb
        Dim fromDate As String
        Dim toDate As String

        Try
            If Objconn.connect() Then
                strQuery = New StringBuilder()
                strQuery.Append("select * from ")
                strQuery.Append("  (select tcs.userid, (mui.SurName +' '+mui.Name+' '+isnull(mui.Middlename,''))as username, ")
                strQuery.Append("  tcs.course_id, mc.Course_code, tcs.loginname as ExamID, tcs.pwd as ExamPassword, ")
                'In Query, Convert Function Removed By Bhumi [10/8/2015]
                'Purpose: dd/MM/yyyy Format Not Working Properly While Comparing With Database Values
                strQuery.Append("  tcs.written_test_date as writtentestdate, ")
                strQuery.Append("  tcs.written_test_appear_date as appearancedate, ")
                'End By Bhumi
                strQuery.Append("  tcs.appearedflag as appearedflag, mc.course_name, mce.Center_Name,  mui.Center_ID, ")
                strQuery.Append("  tcs.total_marks, tcs.total_passmarks, isnull(obtained1.obtained_marks,0) as obtained_marks, ")
                strQuery.Append("  (case  when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 75 then  'A+' ")
                strQuery.Append("    when  isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >=60 then 'A' ")
                strQuery.Append("    when isnull((obtained1.obtained_marks*100)/tcs.total_marks,0) >= 50 then 'B' ")
                strQuery.Append("    WHEN (obtained1.obtained_marks*100)/tcs.total_marks is null then null ")
                strQuery.Append("    else 'C' End ) as Grad, ")
                strQuery.Append("  (case WHEN obtained1.obtained_marks >= tcs.total_passmarks then 'Pass' ")
                strQuery.Append("    WHEN obtained1.obtained_marks is null then null ")
                strQuery.Append("    ELSE 'Fail'  END) as Status,")
                strQuery.Append("  mmc.Main_Course_ID, sti.end_time, mc.Total_Time As TotalTime ")
                strQuery.Append("  FROM T_Candidate_Status as tcs ")
                strQuery.Append("  left join M_USER_INFO as mui  on tcs.userid=mui.userid ")
                strQuery.Append("  left join m_course as mc  on mc.course_id=tcs.course_id ")
                strQuery.Append("  Left Join ")
                strQuery.Append("    (select sum(temp.obtained_marks) as obtained_marks,temp.course_id,temp.userid ")
                strQuery.Append("    from (select (case WHEN mq.Qn_Category_ID=3 then ")
                strQuery.Append("        (case WHEN mqa.sub_id=tro.sub_id then ")
                strQuery.Append("          (Case WHEN tro.option_id=mqa.Correct_Opt_Id then  count(mqa.Correct_Opt_Id) ")
                strQuery.Append("            ELSE 0 ")
                strQuery.Append("          END)")
                strQuery.Append("          ELSE 0 ")
                strQuery.Append("        End)")
                strQuery.Append("        WHEN mq.Qn_Category_ID=2 then ")
                strQuery.Append("          (CASE WHEN tro.option_id=mqa.Correct_Opt_Id then  count(mqa.Correct_Opt_Id) ")
                strQuery.Append("            ELSE 0 ")
                strQuery.Append("          End) ")
                strQuery.Append("        WHEN mq.Qn_Category_ID=1 then  ")
                strQuery.Append("          (CASE WHEN tro.option_id=mqa.Correct_Opt_Id then  SUM(mq.Total_Marks) ")
                strQuery.Append("            ELSE 0 ")
                strQuery.Append("          End) ")
                strQuery.Append("        End) as obtained_marks, ")
                strQuery.Append("        mc.course_id, mui.userid ")
                strQuery.Append("      from m_question as mq ")
                strQuery.Append("      left join M_Question_Answer as mqa  on mqa.Qn_ID=mq.qnid and mqa.test_type=mq.test_type ")
                strQuery.Append("      left join t_result as tr  on tr.qno=mq.qnid  AND tr.test_type=mq.test_type ")
                strQuery.Append("      left join m_user_info as mui  on mui.userid=tr.userid ")
                strQuery.Append("      left join m_course as mc  on mc.course_id=tr.course_id")
                strQuery.Append("      left join m_testinfo as mti  on mti.test_type=tr.test_type ")
                strQuery.Append("      left join t_result_option as tro  on tro.result_id=tr.result_id  and tr.test_type=mti.test_type  and tro.option_id=mqa.Correct_Opt_Id ")
                strQuery.Append("      group by mq.total_marks,mc.course_id,  mq.total_marks,mq.qnid,mqa.Sub_id,  mqa.Correct_Opt_Id, tro.option_id, mq.Qn_Category_ID, tro.sub_id, mui.userid ")
                strQuery.Append("    )temp ")
                strQuery.Append("    group by temp.course_id,temp.userid ")
                strQuery.Append("  )as obtained1  on obtained1.course_id=tcs.course_id  and tcs.userid=obtained1.userid ")
                strQuery.Append("  left join M_Centers as mce  on mce.Center_ID=mui.Center_ID ")
                strQuery.Append("  left join M_Main_Course as mmc  on mmc.Main_Course_ID=mc.Main_Course_ID ")
                strQuery.Append("  left join student_time_info as sti  on tcs.userid=sti.user_id  and end_time In (select MAX(end_time) from student_time_info where user_id=tcs.userid and course_id=mc.Course_id) ")
                strQuery.Append(")temp ")
                strQuery.Append("where temp.userid=" + Session.Item("userid").ToString() + " and appearedflag in (2)")
                If ddlClass.SelectedIndex <> 0 And ddlCourse.SelectedIndex <> 0 Then
                    'added by bhumi[1/10/2015]
                    strQuery.Append(" and temp.Center_ID=" + ddlClass.SelectedValue.ToString())
                    'Ended by bhumi
                    strQuery.Append(" and temp.course_id=" + ddlCourse.SelectedValue.ToString())
                End If
                If txtAppFromDate.Value.Trim.ToString().Length > 0 And txtAppToDate.Value.Trim.ToString().Length > 0 Then
                    'Code Added By Bhumi [10/8/2015] 
                    'Using Split function Convert Both the Dates [FROM & To] in MM/dd/yyyy Format
                    'Reason: Result Searching Functionality Not Working For all Dates                                       
                    fromDate = txtAppFromDate.Value
                    fromDate = Convert.ToString(fromDate.Split("/")(1)) + "/" + Convert.ToString(fromDate.Split("/")(0)) + "/" + Convert.ToString(fromDate.Split("/")(2))
                    toDate = txtAppToDate.Value()
                    toDate = Convert.ToString(toDate.Split("/")(1)) + "/" + Convert.ToString(toDate.Split("/")(0)) + "/" + Convert.ToString(toDate.Split("/")(2))
                    'Ended By Bhumi
                    strQuery.Append(" and temp.appearancedate between '" + fromDate + "' and '" + toDate + "'")
                ElseIf txtAppFromDate.Value.Trim.ToString().Length > 0 Then
                    'added by bhumi [1/10/2015]
                    fromDate = txtAppFromDate.Value
                    fromDate = Convert.ToString(fromDate.Split("/")(1)) + "/" + Convert.ToString(fromDate.Split("/")(0)) + "/" + Convert.ToString(fromDate.Split("/")(2))
                    strQuery.Append(" and temp.appearancedate >= '" + fromDate + "'")
                    'Ended by bhumi
                End If
                'added by bhumi[1/10/2015]
                If ddlClass.SelectedIndex <> 0 Then
                    strQuery.Append(" and temp.Center_ID=" + ddlClass.SelectedValue.ToString())
                    'Ended by bhumi
                End If
                'Convert Function Removed By Bhumi [10/8/2015]
                strQuery.Append(" order by temp.appearancedate ")
                ObjDA = New SqlDataAdapter(strQuery.ToString(), Objconn.MyConnection)
                ObjDS = New DataSet()
                ObjDA.Fill(ObjDS)

                If ObjDS.Tables(0).Rows.Count > 0 Then
                    lblMsg.Visible = False
                    GVResult.Visible = True
                    GVResult.DataSource = ObjDS
                    GVResult.DataBind()
                Else
                    GVResult.Visible = False
                    'Dim strTable As StringBuilder
                    'strTable = New StringBuilder()
                    'strTable.Append("<fieldset><legend class='outerframe'>Search Result</legend><table width='100%' border='1' style='border:1px solid grey; border-collapse:collapse;'>")
                    ''Heading
                    'strTable.Append("<tr>")
                    'strTable.Append("<td colspan='2' align='center' style='color: red;'>")
                    'strTable.Append("No any record found...")
                    'strTable.Append("</td>")
                    'strTable.Append("</tr>")
                    'strTable.Append("</table></fieldset>")
                    'trGrid.Cells(0).InnerHtml = strTable.ToString()
                    lblMsg.Visible = True
                    lblMsg.Text = Resources.Resource.Common_NoRecFound
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.ToString())
        Finally
            Objconn.disconnect()
            Objconn = Nothing
            ObjDS = Nothing
            ObjDA = Nothing
            strQuery = Nothing
        End Try
    End Sub

    'Added by Pranit on 02/12/2019
    Protected Sub Selection_Change(sender As Object, e As EventArgs) Handles PageSizeList.SelectedIndexChanged
        Try
            GVResult.PageSize = Convert.ToInt32(PageSizeList.SelectedItem.Text)
            BindGrid()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSearch.Click
        trGrid.Visible = True
        BindGrid()
    End Sub

    Protected Sub ddlClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlClass.SelectedIndexChanged
        LoadCourses()
    End Sub

    Protected Sub btnclear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnclear.Click
        ddlClass.SelectedIndex = 0
        ddlCourse.SelectedIndex = 0
        txtAppFromDate.Value = String.Empty
        txtAppToDate.Value = String.Empty
        BindGrid()
    End Sub

    'Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnBack.Click
    '    Response.Redirect("StudHome.aspx")
    'End Sub

    Protected Sub GVResult_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVResult.PageIndexChanging
        GVResult.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub
End Class