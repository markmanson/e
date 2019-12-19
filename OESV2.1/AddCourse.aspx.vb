Imports System.Data
Imports System.Data.SqlClient
Partial Public Class AddCourse
    Inherits BasePage

#Region "Declarations"
    Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
    Dim objconn As New ConnectDb
    Dim objCommand As SqlCommand                   'SqlCommand object
    Dim objDataReader As SqlDataReader             'SqlDataReader object
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("Addcourse")
    Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

       
        
        If Not IsPostBack Then

            FillCenters()

        End If

    End Sub

#Region "Fill Centre method"
    'Desc: This method fills centre listbox
    'By: Indravadan Vasava, 2011/01/12

    Public Sub FillCenters()

        ddlCenters.Items.Clear()
        'strPathDb = ConfigurationManager.AppSettings("PathDb")
        Dim Centers As String
        Try
            Dim lst1 As New ListItem
            lst1.Text = "--------Select--------"
            lst1.Value = 0
            ddlCenters.Items.Add(lst1)
            If objconn.connect() Then
                'added by bhumi [16/9/2015]
                'Reason: check the Del_Flg 0 or not
                Centers = "Select center_id,Center_Name From M_Centers where Del_flg=0  order by Center_Name"
                'Ended by bhumi
                objCommand = New SqlCommand(Centers, objconn.MyConnection)
                objDataReader = objCommand.ExecuteReader()
                While objDataReader.Read
                    Dim lstItm As New ListItem()
                    lstItm.Enabled = True
                    lstItm.Text = objDataReader.Item(1)
                    lstItm.Value = objDataReader.Item(0)
                    ddlCenters.Items.Add(lstItm)
                End While
                objDataReader.Close()
                objconn.disconnect()
            End If
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
            Centers = Nothing
        End Try
    End Sub
#End Region

    'Added by Pranit on 02/12/2019
    Public Sub FillSectionCourseCombo()
        Try
            ddlSectionDes.Items.Clear()
            Dim l1 As New ListItem
            l1.Text = "---- Select ----"
            l1.Value = 0
            ddlSectionDes.Items.Add(l1)

            Dim lRead, lVocab, lGrammar, lChokai, lTechnical As New ListItem()
            lRead.Enabled = True
            lRead.Text = "Reading"
            lRead.Value = "Reading"

            lVocab.Enabled = True
            lVocab.Text = "Vocabulary"
            lVocab.Value = "Vocab"

            lGrammar.Enabled = True
            lGrammar.Text = "Grammmar"
            lGrammar.Value = "Grammer"

            lChokai.Enabled = True
            lChokai.Text = "Listening"
            lChokai.Value = "Chokai"

            lTechnical.Enabled = True
            lTechnical.Text = "Technical"
            lTechnical.Value = "Technical"

            ddlSectionDes.Items.Add(lRead)
            ddlSectionDes.Items.Add(lVocab)
            ddlSectionDes.Items.Add(lGrammar)
            ddlSectionDes.Items.Add(lChokai)
            ddlSectionDes.Items.Add(lTechnical)

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        Finally
        End Try
    End Sub

#Region "Fill Centre method"
    '********************************************************************
    'Code added by Indravadan Vasava
    'Purpose: To fill the combo box for Course
    '********************************************************************
    Public Sub FillCourses()
        Dim objDataReader1 As SqlDataReader
        Dim query As String = ""
        Try
            ddlCourses.Items.Clear()
            Dim lst1 As New ListItem
            lst1.Text = "--------Select--------"
            lst1.Value = 0
            ddlCourses.Items.Add(lst1)
            If objconn.connect() = True Then
                query = "select Course_ID,Course_Name from M_Course where Description= '" & ddlSectionDes.SelectedItem.Value & "' and del_flag ='0' Order by Course_Name"
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
#End Region

    'Added by Pranit on 02/12/2019
    Protected Sub ddlCenters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlCenters.SelectedIndexChanged
        Try
            FillSectionCourseCombo()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub

    'Added by Pranit on 02/12/2019
    Protected Sub ddlSectionDes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSectionDes.SelectedIndexChanged
        Try
            FillCourses()
        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        End Try
    End Sub


    '********************************************************************
    'Code added by Indravadan Vasava             
    'Purpose: To get CourseID and perform insert update
    '********************************************************************
    Public Sub GetCourseID(ByVal strCourseID As String, ByVal strCenterID As String)
        Dim query As String = ""
        Dim intCouresid As Integer
        Dim DelFlag As Integer
        Dim cnt As Integer
        Dim intCenterID As Integer
        Dim sqlTrans As SqlTransaction
        Dim MyDataReader As SqlDataReader
        Dim Item As DictionaryEntry
        'strPathDb = ConfigurationSettings.AppSettings("PathDb")

        Try
            If objconn.connect() = True Then

                sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                Dim fields As String = "Course_ID,Del_Flag"

                intCenterID = strCenterID.ToString
                '     Couresid = ddlCourses.SelectedItem.Value
                intCouresid = strCourseID.ToString

                'Dim arr(ddlCenters.Items.Count) As Integer
                'Dim arlst As New ArrayList
                'Dim strselecteditems As Integer
                Dim fieldCenters As String = "Course_ID,Center_ID"
                'For lst As Integer = 0 To ddlCenters.Items.Count - 1
                '    If (ddlCenters.Items(lst).Selected = True) Then
                '        strselecteditems = ddlCenters.Items(lst).Value
                '        arlst.Add(strselecteditems)
                '    End If
                'Next
                'arr = arlst.ToArray(System.Type.GetType("System.Int32"))

                If 1 = 1 Then
                    ' For ins As Integer = 0 To arr.Length - 1
                    ' query = "Insert Into t_center_course (" & fieldCenters & ") values('" & Couresid & "','" & arr(ins) & "')"
                    query = "Insert Into t_center_course (" & fieldCenters & ") values('" & intCouresid & "','" & intCenterID & "')"
                    Dim ins_cmd As New SqlCommand(query, objconn.MyConnection, sqlTrans)

                    ins_cmd.ExecuteNonQuery()
                    'by Rajesh nagvanshi 2014/08/19
                    'Register Course in the User_coure(Table)
                    'logic for to insert the value in the T_user_Course table 
                    Dim _UserIDList As ArrayList
                    _UserIDList = getStudentList()

                    For Each _UserID As Integer In _UserIDList

                        If intCenterID <> 0 Then
                            'Dim selectedvalues As String = ddlCourses.SelectedValue.ToString
                            Dim selectedvalues As String = intCouresid
                            Dim arylst As New ArrayList
                            Dim testary() As Integer
                            Dim testtype As New Hashtable

                            Dim strquery As String = " Select test_type from M_Weightage where Course_ID = '" & selectedvalues & "' "
                            Dim cmd As New SqlCommand(strquery, objconn.MyConnection, sqlTrans)

                            MyDataReader = cmd.ExecuteReader()
                            While MyDataReader.Read()
                                arylst.Add(MyDataReader.Item("test_type"))
                            End While
                            MyDataReader.Close()

                            testary = arylst.ToArray(System.Type.GetType("System.Int32"))


                            For g As Integer = 0 To testary.Length - 1
                                testtype.Add(testary(g), selectedvalues)
                            Next


                            Dim intsubweigtage As Integer
                            Dim intsingle As Integer
                            Dim intmultichoise As Integer
                            Dim intblanks As Integer
                            Dim intbasic As Integer
                            Dim intintermediate As Integer

                            For Each Item In testtype
                                Dim booltemp As Boolean = True
                                Dim strq As String = " Select Sub_Weightage,Single,Multi_Choice,Blanks,Basic,InterMed from M_Weightage where test_type = " & Item.Key & " and Course_ID = " & Item.Value
                                Dim ins_cmd3 As New SqlCommand(strq, objconn.MyConnection, sqlTrans)
                                If objconn.MyConnection.State = ConnectionState.Closed Then
                                    objconn.MyConnection.Open()
                                End If
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
                                    InsertIntoUserCourse(Convert.ToInt32(_UserID), Item.Value, Item.Key, intsubweigtage, intsingle, intmultichoise, intblanks, intbasic, intintermediate, sqlTrans)
                                    booltemp = False
                                End If


                                '/*********************************End********************************/
                                arylst = Nothing
                                testary = Nothing
                                testtype = Nothing
                            Next
                        End If
                    Next




                    ' end by rajesh 2014/08/19


                    FillCenters()
                    FillCourses()
                    lblMessageInsertSuccess.Text = Resources.Resource.AddCourse_crssucreg
                    lblMessageInsertSuccess.ForeColor = Drawing.Color.Green
                    lblMessageInsertSuccess.Visible = True
                    'Next

                End If
                sqlTrans.Commit()
                objconn.disconnect()
            End If
        Catch ex As Exception
            sqlTrans.Rollback()
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally
            query = Nothing
            intCouresid = Nothing
            DelFlag = Nothing
            cnt = Nothing
            intCenterID = Nothing
            sqlTrans = Nothing
        End Try

    End Sub

    Protected Sub imgBtnRegister_Click(sender As Object, e As EventArgs) Handles imgBtnRegister.Click
        Dim da As SqlDataAdapter
        Dim Cmd As SqlCommand
        Dim ScalarStr As String
        Dim ds As DataSet
        Dim query As String
        Dim Query_Weightage_Check As String
        Dim strUnsucess As String
        Dim strIncompleWeight As String
        Dim strNotAssignedwei As String
        'Added by Rahul Shukla on 2019/06/03
        ' Reason: To Select Multiple Courses for Single Class.     
        ' Desc: Added the hasttable and Arraylist to iterate the value and uses the StringBuilder to append the value.
        ' BugID: 1097
        Dim arrNotWeighted As ArrayList = New ArrayList
        Dim arrInCompleteWeight As ArrayList = New ArrayList
        Dim arrARegiCourse As ArrayList = New ArrayList
        '--------------------------------------------------------------------
        'Rahul on 27-05-2019
        Dim scCourseValue As StringCollection = New StringCollection()
        Dim scCourseText As StringCollection = New StringCollection()
        'Added by Rahul Shukla on 2019/06/03
        ' Reason: To Store two Value in Hashtable for Particular Selected Value.   
        ' Desc: if we Select particular value in Course it will Fetch the Key and Value for that particular selected value.
        ' BugID: 1097
        Dim htDdlItem As New Hashtable
        For Each item As ListItem In ddlCourses.Items
            If item.Selected Then
                htDdlItem.Add(item.Text, item.Value)
            End If
        Next

        '---------------------------------------------------------------------
        Dim CenterID As String = ddlCenters.SelectedItem.Value

        'Commented by Rahul Shukla
        'Reason: It only fetching Single Value at a Time.
        ' Dim CourseID As String = ddlCourses.SelectedItem.Value
        If ValidateData() = False Then
            lblMsgUnSuccess.Visible = True
            Exit Sub
        End If
        Try
            If objconn.connect() = True Then
                '**************************************************************************************************
                'added by bhumi On 13/8/2015 
                'Reason: whenever New Course Link With Class Intruct User To Assign Weightage Of that Course First 
                '**************************************************************************************************
                'Rahul on 28/05/2019

                For Each CourseID As DictionaryEntry In htDdlItem

                    'Added by Rahul Shukla on 31/05/2019
                    'Reason: Whenever Loop iterate and Going to Insert Multiple value it will open the Connection every time when loop iterate.
                    If objconn.MyConnection.State = ConnectionState.Closed Then
                        objconn.MyConnection.Open()
                    End If

                    Query_Weightage_Check = " select * from M_Weightage where Course_ID = " & CourseID.Value
                    ' Query_Weightage_Check = " select * from M_Weightage where Course_ID = " & CourseID
                    Cmd = New SqlCommand(Query_Weightage_Check, objconn.MyConnection)
                    ScalarStr = Cmd.ExecuteScalar()

                    If ScalarStr = "" Then
                        'Added by Rahul Shukla on  04/06/2019
                        'Reason : Every Time when value condition will false it will save the Particular value and Stores in the Arraylist.
                        arrNotWeighted.Add(CourseID.Key)
                        Dim strbCourseText As StringBuilder = New StringBuilder()
                        Dim strMsgWeight As String
                        strMsgWeight = Resources.Resource.AddCourse_wgtntassgn & "(i.e "
                        For Each x As String In arrNotWeighted
                            strbCourseText.Append(x)
                            strbCourseText.Append(",")
                        Next
                        lblNotWeight.ForeColor = Drawing.Color.Red
                        lblNotWeight.Visible = True
                        Dim strTrimVal As String

                        strTrimVal = strbCourseText.ToString
                        strTrimVal = strTrimVal.TrimEnd(CChar(","))
                        lblNotWeight.Text = strMsgWeight & strTrimVal.ToString
                        lblNotWeight.Text = lblNotWeight.Text & ")"

                        'lblMsgUnSuccess.Text = "Weightage is not assigned for this course"
                        'lblMsgUnSuccess.ForeColor = Drawing.Color.Red
                        'lblMsgUnSuccess.Visible = True
                        'End By Bhumi

                        '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                        'added by bhumi [15/9/2015]
                        'Reason: check that course contents 100% weightage or not
                        '&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&
                    Else

                        Query_Weightage_Check = " select SUM(Sub_Weightage) AS Total_Weightage from M_Weightage where Course_ID =  " & CourseID.Value
                        'Query_Weightage_Check = " select SUM(Sub_Weightage) AS Total_Weightage from M_Weightage where Course_ID = " & CourseID
                        Cmd = New SqlCommand(Query_Weightage_Check, objconn.MyConnection)
                        ScalarStr = Cmd.ExecuteScalar()
                        If ScalarStr <> "100" Then
                            'Added by Rahul Shukla on  04/06/2019
                            'Reason : Every Time when value condition will false it will save the Particular value and Stores in the Arraylist.
                            arrInCompleteWeight.Add(CourseID.Key)
                            Dim strbCourse As StringBuilder = New StringBuilder()
                            Dim strMsg As String
                            strMsg = Resources.Resource.AddCourse_wgtcrs & "(i.e "
                            For Each x As String In arrInCompleteWeight
                                strbCourse.Append(x)
                                strbCourse.Append(",")
                            Next

                            lblInCompleteWeightage.ForeColor = Drawing.Color.Red
                            lblInCompleteWeightage.Visible = True
                            Dim strTrimVal As String

                            strTrimVal = strbCourse.ToString
                            strTrimVal = strTrimVal.TrimEnd(CChar(","))
                            lblInCompleteWeightage.Text = strMsg & strTrimVal.ToString
                            lblInCompleteWeightage.Text = lblInCompleteWeightage.Text & ")"
                            lblInCompleteWeightage.ForeColor = Drawing.Color.Red
                            lblInCompleteWeightage.Visible = True
                            'Ended by bhumi

                        Else
                            query = " select * from T_CENTER_COURSE where Center_ID = " & CenterID & " and Course_ID = " & CourseID.Value
                            ' query = " select * from T_CENTER_COURSE where Center_ID = " & CenterID & " and Course_ID = " & CourseID
                            da = New SqlDataAdapter(query, objconn.MyConnection)
                            ds = New DataSet
                            da.Fill(ds)
                            If (ds.Tables(0).Rows.Count = 0) Then
                                GetCourseID(CourseID.Value, CenterID)
                            Else
                                If (ds.Tables(0).Rows.Count <> 0) Then
                                    'Added by Rahul Shukla on  04/06/2019
                                    'Reason : Every Time when value condition will false it will save the Particular value and Stores in the Arraylist.
                                    arrARegiCourse.Add(CourseID.Key)
                                    Dim strbCourseTxt As StringBuilder = New StringBuilder()
                                    Dim strMsgRegis As String
                                    strMsgRegis = Resources.Resource.AddCourse_crsexisted & "(i.e "
                                    For Each x As String In arrARegiCourse
                                        strbCourseTxt.Append(x)
                                        strbCourseTxt.Append(",")
                                    Next

                                    lblMsgUnSuccess.ForeColor = Drawing.Color.Red
                                    lblMsgUnSuccess.Visible = True
                                    Dim strTrimVal As String

                                    strTrimVal = strbCourseTxt.ToString
                                    strTrimVal = strTrimVal.TrimEnd(CChar(","))

                                    lblMsgUnSuccess.Text = strMsgRegis & strTrimVal.ToString
                                    lblMsgUnSuccess.Text = lblMsgUnSuccess.Text & ")"

                                End If
                            End If
                        End If
                    End If
                Next
                objconn.disconnect()
            End If
        Catch ex As Exception
            If objconn.MyConnection.State = ConnectionState.Open Then
                objconn.disconnect()
            End If
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Response.Redirect("error.aspx")
        Finally

            da = Nothing
            ds = Nothing
        End Try

    End Sub

    Public Function ValidateData() As Boolean
        Dim flag As Boolean = True
        If ddlCenters.SelectedItem.Value = 0 Then
            flag = False
            lblMsgUnSuccess.Text = Resources.Resource.AddCourse_sltclserr
            lblInCompleteWeightage.Visible = False
            lblMessageInsertSuccess.Visible = False
            lblNotWeight.Visible = False
        ElseIf ddlCourses.SelectedIndex = -1 Then
            flag = False
            lblMsgUnSuccess.Text = Resources.Resource.AddCourse_sltcrserr
            lblInCompleteWeightage.Visible = False
            lblMessageInsertSuccess.Visible = False
            lblNotWeight.Visible = False
        End If
        Return flag
    End Function


    Protected Sub imgBtnBack_Click(sender As Object, e As EventArgs) Handles imgBtnBack.Click
        Response.Redirect("ManageCourse.aspx")
    End Sub

    Protected Sub imgBtnClear_Click(sender As Object, e As EventArgs) Handles imgBtnClear.Click
        FillCenters()
        FillSectionCourseCombo()
        FillCourses()
        lblMsgUnSuccess.ForeColor = Drawing.Color.Red
        lblMsgUnSuccess.Text = ""
        lblMsgUnSuccess.Visible = False
        lblInCompleteWeightage.Visible = False
        lblNotWeight.Visible = False
        lblMessageInsertSuccess.Visible = False
    End Sub

    Protected Function clear()
        lblInCompleteWeightage.Visible = False
        lblNotWeight.Visible = False
        lblMessageInsertSuccess.Visible = False
    End Function
#Region "Enter data into t_user_course"
    'Desc:insert data into t_user_course.
    'By: Jatin Gangajaliya, 2011/04/23.

    Private Sub InsertIntoUserCourse(ByVal intuserid As Integer, ByVal intcourseid As Integer, ByVal inttest As Integer, ByVal intsubweigtage As Integer, ByVal intsingle As Integer, ByVal intmultichoise As Integer, ByVal intblanks As Integer, ByVal intbasic As Integer, ByVal intintermediate As Integer, ByVal sqltran As SqlTransaction)
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
            Dim ins_cmd1 As New SqlCommand(querystr, objconn.MyConnection, sqltran)
            ins_cmd1.ExecuteNonQuery()

        Catch ex As Exception
            If log.IsDebugEnabled Then
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Sub
    Function getStudentList() As ArrayList
        Dim sb As StringBuilder
        Dim querystr As String
        Dim _StudentList As ArrayList = New ArrayList
        Dim _query As String
        Dim MyDataReader As SqlDataReader
        'Dim sqlTrans As SqlTransaction
        Try
            If Not objconn.MyConnection Is Nothing Then
                'Delete_Flg added by bhumi[8/10/2015]
                'Reason: for finding Student List who is Enable in Class
                _query = "select Userid From M_User_info where Delete_flg=0 and center_ID='" & ddlCenters.SelectedValue & "'"
                Dim cmd As New SqlCommand(_query, objconnect)
                If objconnect.State = ConnectionState.Closed Then
                    objconnect.Open()
                End If
                MyDataReader = cmd.ExecuteReader()
                While MyDataReader.Read()
                    _StudentList.Add(MyDataReader.Item("Userid"))
                End While
                MyDataReader.Close()
                objconnect.Close()
            End If
            Return _StudentList
        Catch ex As Exception
            If log.IsDebugEnabled Then
                If objconnect.State = ConnectionState.Open Then
                    objconnect.Close()
                End If
                log.Debug("Error :" & ex.ToString())
            End If
            Throw ex
        End Try
    End Function

#End Region
End Class