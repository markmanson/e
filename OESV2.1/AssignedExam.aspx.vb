Imports System.Data.SqlClient

Partial Public Class WebForm2
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
                strQuery.Append("select MUI.userid, MUI.LoginName, MUI.Pwd, (Name+' '+SurName) as FullName, ")
                strQuery.Append("MUI.User_Type, MUI.Center_ID, MUI.RollNo, ")
                strQuery.Append("TCS.Course_ID, TCS.LoginName as ExamID, TCS.Pwd as ExamPassword, ")
                'In Query, Convert Function Removed By Bhumi [11/8/2015]
                'Purpose: dd/MM/yyyy Format Not Working Properly While Comparing With Database Values
                strQuery.Append("TCS.Written_Test_Date as ExaminationDate, ")
                strQuery.Append("TCS.Written_Test_Date+3 as LastDate, ")
                'End By Bhumi
                strQuery.Append("MC.Course_Name, MC.Course_Code, MCT.Center_Code, MCT.Center_Name as CenterName ")
                strQuery.Append("from M_User_Info MUI ")
                strQuery.Append("left join T_Candidate_Status TCS on MUI.userid=TCS.userid ")
                strQuery.Append("left join M_Course MC on TCS.Course_ID=MC.Course_ID ")
                strQuery.Append("left join M_Centers MCT on MUI.Center_ID=MCT.Center_ID ")
                strQuery.Append("where MUI.Userid = " + Session.Item("userid").ToString + " And MUI.Delete_Flg = 0 ")
                strQuery.Append("And TCS.Appearedflag = 0 ")
                If ddlClass.SelectedIndex <> 0 And ddlCourse.SelectedIndex <> 0 Then
                    strQuery.Append("And TCS.Course_ID=" + ddlCourse.SelectedItem.Value)
                End If
                If txtAppFromDate.Value.Trim.ToString().Length > 0 And txtAppToDate.Value.Trim.ToString().Length > 0 Then
                    'Code Added By Bhumi [11/8/2015] 
                    'Using Split function Convert Both the Dates [FROM & To] in MM/dd/yyyy Format
                    'Reason: Result Searching Functionality Not Working For all Dates                                       
                    fromDate = txtAppFromDate.Value
                    fromDate = Convert.ToString(fromDate.Split("/")(1)) + "/" + Convert.ToString(fromDate.Split("/")(0)) + "/" + Convert.ToString(fromDate.Split("/")(2))
                    toDate = txtAppToDate.Value()
                    toDate = Convert.ToString(toDate.Split("/")(1)) + "/" + Convert.ToString(toDate.Split("/")(0)) + "/" + Convert.ToString(toDate.Split("/")(2))
                    'Ended By Bhumi
                    strQuery.Append(" and TCS.Written_Test_Date between '" + fromDate + "' and '" + toDate + "'")
                ElseIf txtAppFromDate.Value.Trim.ToString().Length > 0 Then
                    strQuery.Append(" and convert(varchar(10), TCS.Written_Test_Date,103) >= '" + txtAppFromDate.Value().ToString() + "'")
                End If
                ObjDA = New SqlDataAdapter(strQuery.ToString(), Objconn.MyConnection)
                ObjDS = New DataSet()
                ObjDA.Fill(ObjDS)
                If ObjDS.Tables(0).Rows.Count > 0 Then
                    lblMsg.Visible = False
                    GVExam.Visible = True
                    GVExam.DataSource = ObjDS
                    GVExam.DataBind()
                Else
                    lblMsg.Visible = True
                    GVExam.Visible = False
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
                strQuery.Append("where course_id in (select course_id from t_center_course where center_id=" + ddlClass.SelectedValue + ") ")
                strQuery.Append("order by Course_id")
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
    Protected Sub ddlClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlClass.SelectedIndexChanged
        LoadCourses()
    End Sub

    Protected Sub BtnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnSearch.Click
        trGrid.Visible = True
        BindGrid()
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
    Protected Sub GVExam_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVExam.PageIndexChanging
        GVExam.PageIndex = e.NewPageIndex
        BindGrid()
    End Sub
    Protected Sub GVExam_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GVExam.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim objLnkBtn As LinkButton
            objLnkBtn = New LinkButton()
            objLnkBtn = CType(e.Row.Cells(7).FindControl("lnkBtn"),LinkButton)
            objLnkBtn.PostBackUrl = objLnkBtn.PostBackUrl + "?ID=" + e.Row.Cells(1).Text() + "&PWD="+e.Row.Cells(2).Text

        End If
    End Sub
End Class