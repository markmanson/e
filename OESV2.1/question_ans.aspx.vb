#Region "Namespaces"
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Configuration
Imports System.Web.Security
Imports log4net
Imports System.Text.RegularExpressions
Imports System.IO

#End Region

Namespace unirecruite

    Partial Class question_ans

#Region "Declaration"
        Inherits BasePage
        Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger("question_ans")
        Dim tbrow As HtmlTableRow   ' Htmltablerow type object similar to <tr> tag of HTML
        Dim tbdat As HtmlTableCell  ' Htmltablecell type object similar to <td> tag of HTML
        Dim optarray() As System.Web.UI.HtmlControls.HtmlInputText  ' An array of HTML text type objects

        '/****************start,Jatin Gangajaliya,2011/04/08*******************/
        Dim fileuploader() As FileUpload
        Dim lstoptions As ListBox
        Dim txtreadonly As TextBox
        Dim imagearray() As Literal
        Dim lnkbtnary() As LinkButton
        Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
        ' Dim optarrayblank() As System.Web.UI.HtmlControls.HtmlInputText
        Dim optarrayans() As System.Web.UI.HtmlControls.HtmlInputText
        Dim int As Integer
        Dim Sqltransc As SqlTransaction
        Dim objconnect As New SqlConnection(ConfigurationSettings.AppSettings("PathDb"))
        Dim cmd As New SqlCommand

        '/*********************************End********************************/

        Dim cmb_rgtans As System.Web.UI.HtmlControls.HtmlSelect ' HTML select type object
        Dim WithEvents cmb_QueLevel As DropDownList  ' HTML select type object
        Protected WithEvents ImageButton1 As System.Web.UI.WebControls.ImageButton
        Dim validation() As RequiredFieldValidator
        Dim CONS As unirecruite.Errconstants
        Const ENCRYPT_DELIMIT = "h"
        Const ENCRYPT_KEY = 124
        Dim g_OriginalVal As String
        Dim objconn As New ConnectDb
#End Region

#Region "Structure OptSame"
        Structure OptSame
            Dim Status As Boolean
            Dim Opt1 As Integer
            Dim Opt2 As Integer
        End Structure
#End Region

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
        '*************On Error Go to Error Page****************
        '************************************************************************************************
        'Function               :   This function is called every time the page is loaded
        '
        'Return                 :   None
        '
        'Argument               :   sender = system.object
        '                           e = system.eventargs
        '
        'Explanation            :   Whenever the page makes a trip to the server and is loaded on the
        '                           client this event is called
        '
        'Note                   :   The arguments are builtin
        '************************************************************************************************

#Region "Page_Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Session("UserName") = Nothing Then
                    Response.Redirect("~\login.aspx", False)
                End If
                '  If Session("UniUserType").ToString <> "1" Then ' commented by pragnesha for super admin
                If Convert.ToString(Session("UniUserType")) > "2" Then
                    Response.Redirect("~\register.aspx", False)
                End If


                'code added by Tabrez
                If Session("LoginGenuine") Is Nothing Then
                    Response.Redirect("error.aspx?err=question_ans.aspx.vb Please Login to continue", False)
                End If
                '****************************************************************************
                img_savecont.Visible = False
                btnDel.Attributes.Add("OnClick", "return ConDel();")

                ' Checking if the form is posted back
                If Not IsPostBack Then
                    FillSectionCourseCombo()
                    g_OriginalVal = txt_Question.Text
                    If sDecodeString(Request.QueryString("test")) = "" Then
                        populatesubject()
                        cmb_subject.SelectedIndex = Session("testtype") - 1
                    Else
                        populatesubject(sDecodeString(Request.QueryString("test")))
                    End If
                    If Request.QueryString("qid").ToString <> "" Then
                        'Session.Remove("sname")
                        'If Not Session("sname") Is Nothing Then
                        '    Session.Remove("sname")
                        'End If
                        ''Added By Irfan On 25/02/2015
                        'If Not Session("blanks") Is Nothing Then
                        '    Session.Remove("blanks")
                        'End If
                        If Not Session("normal") Is Nothing Then
                            Session.Remove("normal")
                        End If
                        '------------Added by Nisha----------
                        If Not Session("listen") Is Nothing Then
                            Session.Remove("listen")
                        End If

                        TABLE3.Controls.Clear()
                        TABLE4.Controls.Clear()
                        ''End By Irfan
                        lbl.Text = Resources.Resource.QuestionAns_QuesDts
                        img_saveexit.Visible = False
                        img_addmore.Visible = False
                        cmb_subject.Visible = False
                        TxtSubject.Visible = True
                        img_update.Visible = True
                        btnDel.Visible = True
                        imgbtnreset.Visible = True
                        Session.Add("quesid", Request.QueryString("qid").ToString)
                        populatequesans(CInt(Request.QueryString("qid")), sDecodeString(Request.QueryString("test")))
                        If int = 2 Then
                            playS(CInt(Request.QueryString("qid")), sDecodeString(Request.QueryString("test")))
                        End If
                        Session.Add("Qlast", Session("pi"))
                        Dim sqlstr As String = ""
                        sqlstr = "select ISNULL(Audios,'') as Audios from m_question where qnid = " & Request.QueryString("qid").ToString & " and test_type='" & sDecodeString(Request.QueryString("test")) & "'"
                        If objconn.connect() Then
                            Dim myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                            Dim myDataReader As SqlDataReader = myCommand.ExecuteReader
                            myDataReader.Read()
                            If (myDataReader.Item(0) <> "") Then
                                queAudio.Visible = True
                            Else
                                queAudio.Visible = False
                            End If
                            myDataReader = Nothing
                            myCommand = Nothing
                        End If
                        'lnkquestion.Visible = True
                    Else
                        'Session.Remove("sname")
                        If Not Session("sname") Is Nothing Then
                            Session.Remove("sname")
                        End If
                        If Not Session("ques") Is Nothing Then
                            Session.Remove("ques")
                        End If
                        lbl.Text = Resources.Resource.QuestionAns_QRD
                        img_addmore.Visible = False
                        img_update.Visible = False
                        cmb_subject.Visible = True
                        TxtSubject.Visible = False
                        imgbtnclr.Visible = True
                        btnBack.Visible = True
                        img_saveexit.Visible = False
                        imgbtnclr.Visible = False
                        imgbtnprev.Visible = False
                        'Nisha

                        Img.Visible = False
                        ImgSection.Visible = False
                        'fileUpldaudio.Visible = False
                        rowAudio.Visible = False
                        queAudio.Visible = False


                    End If
                End If
                'Me.SetFocus(txt_Question)

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region
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


#Region "Page_Unload"
        Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload
            Try
                'Dim strPathDb As String
                If objconn.connect() = True Then
                    objconn.disconnect()
                End If
            Catch ex As Exception
                objconn.disconnect()
            End Try
        End Sub
#End Region

        '************************************************************************************************
        'Function               :   This function is called to populate the subject combo box
        '
        'Return                 :   None
        '
        'Argument               :   None
        '
        'Explanation            :   The function takes the different subject name from the database and 
        '                           populates the combo box.
        '
        'Note                   :   
        '************************************************************************************************
#Region "populatesubject"
        Private Sub populatesubject(Optional ByVal test_type As String = "C")
            Dim myDataReader As SqlDataReader             ' SqlDataReader type object
            Dim myCommand As SqlCommand                   ' SqlCommand type object
            Dim objconn As New ConnectDb    ' Object of the ConnectClass class
            Dim sqlstr As String                            ' String type variable to store string    
            Dim myTable As DataTable                        ' DataTable type object
            Dim myRow As DataRow                            ' DataRow type object  

            Try
                ' Checking if the Database is getting connected
                'Dim strPathDb As String
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() Then
                    sqlstr = ""
                    sqlstr = sqlstr & "SELECT Distinct m_testinfo.test_type, m_testinfo.test_name FROM m_testinfo full outer join M_Course on M_Course.Course_name= M_Testinfo.test_name  where M_Course.Description= '" & ddlSectionDes.SelectedItem.Value & "' and m_testinfo.del_flag='0' order by test_name"

                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    myDataReader = myCommand.ExecuteReader

                    myTable = New DataTable
                    myTable.Columns.Add(New DataColumn("subjectID", GetType(String)))
                    myTable.Columns.Add(New DataColumn("subjectName", GetType(String)))

                    '***********************************************************
                    'Modified by Tabrez 
                    'Purpose: To display the subject selected on the 
                    'previous page.
                    '***********************************************************
                    Dim StrTemp As String
                    Dim SelVal As String

                    SelVal = ""
                    If cmb_subject.SelectedIndex >= 0 Then
                        SelVal = cmb_subject.SelectedValue
                    End If

                    ' While loop to populate the Datatable
                    While myDataReader.Read
                        myRow = myTable.NewRow
                        myRow(0) = myDataReader.Item("test_type")
                        StrTemp = Convert.ToString(myDataReader.Item("test_type"))
                        If UCase(Trim(sDecodeString(Request.QueryString("test")))) = UCase(Trim(StrTemp)) Then
                            TxtSubject.Text = Trim(Convert.ToString(myDataReader.Item("test_name")))
                        End If
                        myRow(1) = myDataReader.Item("test_name")
                        myTable.Rows.Add(myRow)
                    End While
                    cmb_subject.DataSource = myTable
                    cmb_subject.DataTextField = "subjectName"
                    cmb_subject.DataValueField = "subjectID"
                    cmb_subject.DataBind()
                    myDataReader.Close()
                    myCommand.Dispose()
                    objconn.disconnect()


                    'If Not SelVal = "" Then
                    '    cmb_subject.SelectedValue = SelVal
                    'End If
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
                sqlstr = Nothing
                myDataReader = Nothing
                myCommand = Nothing
                myRow = Nothing
                myTable = Nothing
            End Try
        End Sub
#End Region

        '************************************************************************************************
        'Function               :   This function is called to populate the question textbox and the
        '                           option text boxes
        '
        'Return                 :   None
        '
        'Argument               :   None
        '
        'Explanation            :   This function check for the records for a particular quesID and 
        '                           populates the accordingly
        '
        'Note                   :   
        '************************************************************************************************
#Region "populatequesans"
        Private Sub populatequesans(ByVal qid As Integer, ByVal testType As String)
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim i As Integer
            Dim strPathDb As String
            Dim strarray() As String
            Dim strauarray() As String
            Dim adap As SqlDataAdapter
            Dim dt As DataTable
            Try

                lblMsg.Text = String.Empty
                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                btnDel.Visible = False
                ''    For normal type questions.
                'If int <> 3 Then
                '    If objconn.connect() Then
                '        sqlstr = ""
                '        sqlstr = sqlstr & "SELECT ques.question, ques.Total_Marks,opt.[option],qlevel,ques.Audios FROM "
                '        sqlstr = sqlstr & "m_question AS ques, m_options AS opt WHERE "
                '        sqlstr = sqlstr & "ques.qnid = opt.qnid AND "
                '        sqlstr = sqlstr & "ques.test_type = opt.test_type AND "
                '        sqlstr = sqlstr & "ques.qnid = " & qid & "AND "
                '        sqlstr = sqlstr & "ques.Audios = " & qid & "AND " ' 3/10/2017
                '        sqlstr = sqlstr & "ques.test_type = '" & testType & "' order by opt.optionid"

                '        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                '        myDataReader = myCommand.ExecuteReader
                '        i = 0
                '        cmb_subject.SelectedValue = testType

                '        While myDataReader.Read
                '            cmb_QueLevel.SelectedIndex = Convert.ToInt32(myDataReader.Item("qlevel")) 'Convert.ToInt32(cmb_QueLevel.SelectedIndex.ToString())

                '            'txt_Question.Text = Server.HtmlDecode(myDataReader.Item("question"))

                '            '/**************Start,Jatin Gangajaliya,2011/04/13****************************/
                '            'Dim strtemp As String = myDataReader.Item("question")
                '            Dim str As String
                '            str = myDataReader.Item("question")

                '            'Dim strA As String
                '            ' strA = myDataReader.Item("Audios")
                '            'strauarray = CheckAudio(strA) 'ZX
                '            ' queAudio.Text = strauarray(1)

                '            'strarray = CheckImage(myDataReader.Item("question"))
                '            strarray = CheckImage(str)
                '            ' strArrAud = CheckAudio(str)
                '            '/******************************End**************************/
                '            'fileuldque.Visible = True
                '            'lnkquestion.Visible = True
                '            'TABLE1.Visible = True
                '            rowsubject.Visible = True
                '            rowquestion.Visible = True

                '            txt_Question.Text = strarray(0)
                '            quesimage.Text = strarray(1)
                '            txt_Question.Visible = True

                '            'optarray(i).Value = Server.HtmlDecode(myDataReader.Item("option"))
                '            strarray = CheckImage(myDataReader.Item("option"))
                '            optarray(i).Value = strarray(0)
                '            imagearray(i).Text = strarray(1)

                '            txtreadonly.Text = Server.HtmlDecode(myDataReader.Item("Total_Marks"))
                '            i = i + 1

                '        End While

                '        For g As Integer = 0 To imagearray.Length - 1
                '            imagearray(g).Visible = True
                '            If imagearray(g).Text <> "" Then
                '                lnkbtnary(g).Visible = True
                '            Else
                '                lnkbtnary(g).Visible = False
                '            End If
                '        Next

                '        If quesimage.Text <> "" Then
                '            lnkquestion.Visible = True
                '        End If

                '        myDataReader.Close()

                '        '/*******************************start*******************************/
                '        For l As Integer = 0 To lstoptions.Items.Count - 1
                '            If lstoptions.Items(l).Selected = True Then
                '                lstoptions.Items(l).Selected = False
                '            End If
                '        Next

                '        Dim query As String = "select Correct_Opt_Id from M_Question_Answer where Qn_ID=" & qid & " AND test_type = " & testType
                '        myCommand = New SqlCommand(query, objconn.MyConnection)

                '        myDataReader = myCommand.ExecuteReader
                '        While myDataReader.Read
                '            lstoptions.Items(CInt(myDataReader("Correct_Opt_Id")) - 1).Selected = True
                '        End While
                '        '/*******************************End***********************************/
                '        myCommand.Dispose()
                '    End If
                'End If

                ''For normal type questions.
                If int = 1 Then

                    If objconn.connect() Then
                        sqlstr = ""
                        sqlstr = sqlstr & "SELECT ques.question, ques.Total_Marks,opt.[option],qlevel FROM "
                        sqlstr = sqlstr & "m_question AS ques, m_options AS opt WHERE "
                        sqlstr = sqlstr & "ques.qnid = opt.qnid AND "
                        sqlstr = sqlstr & "ques.test_type = opt.test_type AND "
                        sqlstr = sqlstr & "ques.qnid = " & qid & "AND "
                        sqlstr = sqlstr & "ques.test_type = '" & testType & "' order by opt.optionid"

                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        myDataReader = myCommand.ExecuteReader
                        i = 0
                        cmb_subject.SelectedValue = testType

                        While myDataReader.Read
                            cmb_QueLevel.SelectedIndex = Convert.ToInt32(myDataReader.Item("qlevel")) 'Convert.ToInt32(cmb_QueLevel.SelectedIndex.ToString())

                            txt_Question.Text = Server.HtmlDecode(myDataReader.Item("question"))

                            '   /**************Start,Jatin Gangajaliya,2011/04/13****************************/
                            Dim strtemp As String = myDataReader.Item("question")
                            Dim str As String
                            str = myDataReader.Item("question")

                            strarray = CheckImage(myDataReader.Item("question"))
                            strarray = CheckImage(str)
                            '    /******************************End**************************/
                            fileuldque.Visible = True
                            lnkquestion.Visible = True
                            TABLE1.Visible = True
                            rowsubject.Visible = True

                            rowquestion.Visible = True
                            Img.Visible = True
                            ImgSection.Visible = True

                            txt_Question.Text = strarray(0)
                            quesimage.Text = strarray(1)

                            txt_Question.Visible = True

                            optarray(i).Value = Server.HtmlDecode(myDataReader.Item("option"))
                            strarray = CheckImage(myDataReader.Item("option"))
                            optarray(i).Value = strarray(0)
                            imagearray(i).Text = strarray(1)

                            txtreadonly.Text = Server.HtmlDecode(myDataReader.Item("Total_Marks"))
                            i = i + 1

                        End While

                        For g As Integer = 0 To imagearray.Length - 1
                            imagearray(g).Visible = True
                            If imagearray(g).Text <> "" Then
                                lnkbtnary(g).Visible = True
                            Else
                                lnkbtnary(g).Visible = False
                            End If
                        Next

                        If quesimage.Text <> "" Then
                            lnkquestion.Visible = True
                        End If '5

                        myDataReader.Close()

                        '  /*******************************start*******************************/
                        For l As Integer = 0 To lstoptions.Items.Count - 1
                            If lstoptions.Items(l).Selected = True Then
                                lstoptions.Items(l).Selected = False
                            End If
                        Next

                        Dim query As String = "select Correct_Opt_Id from M_Question_Answer where Qn_ID=" & qid & " AND test_type = " & testType
                        myCommand = New SqlCommand(query, objconn.MyConnection)

                        myDataReader = myCommand.ExecuteReader
                        While myDataReader.Read
                            lstoptions.Items(CInt(myDataReader("Correct_Opt_Id")) - 1).Selected = True
                        End While
                        '   /*******************************End***********************************/
                        myCommand.Dispose()
                    End If
                    'Editted by Rahul Shukla on 16/05/2019
                    HiddenField1.Value = ""
                    'HiddenField2.Value = ""  'Added by Pragnesha to hide audio control from search question
                End If


                'For listen type questions.
                'Dim strA As String
                '' Dim myDataReader As SqlDataReader
                If int = 2 Then
                    If objconn.connect() Then
                        sqlstr = ""
                        sqlstr = sqlstr & "SELECT ques.question, ques.Total_Marks,ques.Audios,opt.[option],qlevel FROM "
                        sqlstr = sqlstr & "m_question AS ques, m_options AS opt WHERE "
                        sqlstr = sqlstr & "ques.qnid = opt.qnid AND "
                        sqlstr = sqlstr & "ques.test_type = opt.test_type AND "
                        sqlstr = sqlstr & "ques.qnid = " & qid & "AND "
                        sqlstr = sqlstr & "ques.test_type = '" & testType & "' order by opt.optionid"

                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        myDataReader = myCommand.ExecuteReader
                        i = 0
                        cmb_subject.SelectedValue = testType

                        While myDataReader.Read
                            cmb_QueLevel.SelectedIndex = Convert.ToInt32(myDataReader.Item("qlevel")) 'Convert.ToInt32(cmb_QueLevel.SelectedIndex.ToString())

                            Dim str As String
                            str = myDataReader.Item("question")


                            strarray = CheckImage(str)

                            'Added for update img by pragnesha (21-5-2019)
                            fileuldque.Visible = True
                            lnkquestion.Visible = True
                            Img.Visible = True
                            ImgSection.Visible = True
                            '-----------------------------------

                            Dim strA As String
                            strA = myDataReader.Item("Audios")
                            strauarray = CheckAudio(strA) 'ZX

                            rowsubject.Visible = True
                            rowAudio.Visible = True
                            rowquestion.Visible = True

                            txt_Question.Text = strarray(0)
                            quesimage.Text = strarray(1)
                            'queAudio.Text = strauarray(1) 'empty value

                            queAudio.Text = strauarray(0)

                            txt_Question.Visible = True

                            'optarray(i).Value = Server.HtmlDecode(myDataReader.Item("option"))
                            strarray = CheckImage(myDataReader.Item("option"))
                            optarray(i).Value = strarray(0)
                            imagearray(i).Text = strarray(1)

                            txtreadonly.Text = Server.HtmlDecode(myDataReader.Item("Total_Marks"))
                            i = i + 1

                        End While

                        For g As Integer = 0 To imagearray.Length - 1
                            imagearray(g).Visible = True
                            If imagearray(g).Text <> "" Then
                                lnkbtnary(g).Visible = True
                            Else
                                lnkbtnary(g).Visible = False
                            End If
                        Next

                        If quesimage.Text <> "" Then
                            lnkquestion.Visible = True
                            'fileuldque.Visible = True
                        End If

                        If queAudio.Text <> "" Then
                            'fileUpldaudio.Visible = True
                            rowAudio.Visible = True
                            queAudio.Text = "Note: File Update will Take few minuites"

                        End If

                        'Pragnesha


                        myDataReader.Close()

                        '/*******************************start*******************************/
                        For l As Integer = 0 To lstoptions.Items.Count - 1
                            If lstoptions.Items(l).Selected = True Then
                                lstoptions.Items(l).Selected = False
                            End If
                        Next

                        Dim query As String = "select Correct_Opt_Id from M_Question_Answer where Qn_ID=" & qid & " AND test_type = " & testType
                        myCommand = New SqlCommand(query, objconn.MyConnection)

                        myDataReader = myCommand.ExecuteReader
                        While myDataReader.Read
                            lstoptions.Items(CInt(myDataReader("Correct_Opt_Id")) - 1).Selected = True
                        End While
                        '/*******************************End***********************************/
                        myCommand.Dispose()
                    End If
                End If




                'For fill in the blanks type questions.
                If int = 3 Then
                    TABLE3.Visible = True
                    TABLE4.Visible = True

                    'For hiding file uploader,literal and link button.
                    fileuldque.Visible = False
                    quesimage.Text = String.Empty
                    lnkquestion.Visible = False

                    For j As Integer = 0 To optarray.Length - 1
                        optarray(j).Value = String.Empty
                    Next

                    If objconn.connect() Then
                        sqlstr = ""
                        sqlstr = sqlstr & "SELECT ques.question, ques.Total_Marks,opt.[option],qlevel FROM "
                        sqlstr = sqlstr & "m_question AS ques, m_options AS opt WHERE "
                        sqlstr = sqlstr & "ques.qnid = opt.qnid AND "
                        sqlstr = sqlstr & "ques.test_type = opt.test_type AND "
                        sqlstr = sqlstr & "ques.qnid = " & qid & "AND "
                        sqlstr = sqlstr & "ques.test_type = '" & testType & "' order by opt.optionid"

                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        myDataReader = myCommand.ExecuteReader
                        i = 0
                        cmb_subject.SelectedValue = testType

                        While myDataReader.Read()

                            If Not IsDBNull(myDataReader.Item("qlevel")) Then
                                cmb_QueLevel.SelectedIndex = Convert.ToInt32(myDataReader.Item("qlevel"))
                            End If

                            If Not IsDBNull(myDataReader.Item("question")) Then
                                txt_Question.Text = myDataReader.Item("question")
                                rowsubject.Visible = True
                                rowquestion.Visible = True
                            End If

                            If Not IsDBNull(myDataReader.Item("option")) Then
                                optarray(i).Value = myDataReader.Item("option")
                            End If

                            If Not IsDBNull(myDataReader.Item("Total_Marks")) Then
                                txtreadonly.Text = Server.HtmlDecode(myDataReader.Item("Total_Marks"))
                            End If
                            i = i + 1
                        End While
                    End If

                    For j As Integer = 0 To optarrayans.Length - 1
                        optarrayans(j).Value = String.Empty
                    Next

                    If objconn.connect() Then
                        sqlstr = " select Correct_Opt_Id,Sub_ID from M_Question_Answer where Qn_ID = " & qid & " and Test_Type = " & testType
                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                        'myDataReader = myCommand.ExecuteReader
                        'i = 0
                        'While myDataReader.Read()
                        '    If Not IsDBNull(myDataReader.Item("Correct_Opt_Id")) Then
                        '        optarrayans(i).Value = myDataReader.Item("Correct_Opt_Id")
                        '    End If
                        '    i = i + 1
                        'End While

                        '/***********************Start***************************/

                        adap = New SqlDataAdapter(myCommand)
                        dt = New DataTable
                        adap.Fill(dt)
                        Dim ht As New Hashtable
                        Dim htMod As New Hashtable
                        Dim index As Integer = 1
                        Dim strtext As String = ""
                        If dt.Rows.Count > 0 Then
                            For g As Integer = 0 To dt.Rows.Count - 1

                                If index = CInt(dt.Rows(g).Item(1)) Then
                                    strtext = strtext & dt.Rows(g).Item(0).ToString & ","
                                Else
                                    strtext = dt.Rows(g).Item(0).ToString & ","
                                    index = CInt(dt.Rows(g).Item(1))
                                End If

                                If (g + 1 <= dt.Rows.Count - 1) Then
                                    If index <> CInt(dt.Rows(g + 1).Item(1)) Then
                                        ht.Add(dt.Rows(g).Item(1).ToString, strtext)
                                        strtext = ""
                                    End If
                                Else
                                    ht.Add(dt.Rows(g).Item(1).ToString, strtext)
                                    strtext = ""
                                End If

                            Next
                            Dim counter As Integer = 0
                            For Each item As DictionaryEntry In ht
                                optarrayans(CInt(item.Key) - 1).Value = item.Value.ToString.Substring(0, CInt(item.Value.ToString.Length - 1)).ToString
                                counter = counter + 1
                            Next

                        End If
                        '/************************End****************************/
                    End If
                    'Editted by Rahul Shukla on 16/05/2019
                    HiddenField1.Value = ""
                    'HiddenField2.Value = ""  'Added by Pragnesha to hide audio control from search question
                End If

                If int <> 3 Then
                    lnkquestion.Visible = False
                    ' fileuldque.Visible = False
                    For g As Integer = 0 To imagearray.Length - 1
                        fileuploader(g).Visible = False
                        lnkbtnary(g).Visible = False
                    Next
                End If
                ''Modified By Irfan On 25/02/2015
                If Request.QueryString("qid").ToString = "" And Request.QueryString("test").ToString = "" Then
                    img_update.Visible = False
                End If
                ''End Modify
                'img_update.Visible = False
                imgbtnreset.Visible = False
                btnBack.Visible = True

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                sqlstr = Nothing
                strPathDb = Nothing
                i = Nothing
                myCommand = Nothing
                myDataReader = Nothing
                objconn = Nothing
                strarray = Nothing
                adap = Nothing
                dt = Nothing
            End Try
        End Sub
#End Region


#Region "CheckImage Function"
        'Desc: This is check image method that checks data return from database.
        'By: Jatin Gangajaliya, 2011/04/11.

        Public Function CheckImage(ByVal value As String) As String()
            Dim str(1) As String
            Try
                If value.Contains("<br/>") Then
                    str(0) = value.Substring(0, value.IndexOf("<"))
                    If value.Contains("<img") Then
                        str(1) = value.Substring(value.IndexOf("<img"))
                    End If

                ElseIf value.Contains("<img") Then
                    str(0) = ""
                    str(1) = value

                Else
                    str(0) = value
                    str(1) = ""
                End If
                Return str
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function
#End Region

#Region "CheckAudio Function"
        'Desc: This is check Audio method that checks data return from database.
        'By:
        Public Function CheckAudio(ByVal value As String) As String()
            Dim str(1) As String
            Try
                If value.Contains("<br/>") Then
                    str(0) = value.Substring(0, value.IndexOf("<"))
                    str(1) = value.Substring(value.IndexOf("<audio"))

                ElseIf value.Contains("<audio") Then
                    str(0) = ""
                    str(1) = value

                Else
                    str(0) = value
                    str(1) = ""
                End If
                Return str
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function
#End Region

        '************************************************************************************************
        'Function               :   This function is called to generate the textboxes for options
        '
        'Return                 :   None
        '
        'Argument               :   optionno = integer type to keep track of no of textboxes to be 
        '                           generated
        '
        'Explanation            :   The function takes an integer as argument and generate those many 
        '                           textboxes for options
        '
        'Note                   :   The argument is By Value
        '************************************************************************************************
#Region "gen_textbox"
        Private Sub gen_textbox(ByVal optionno As Integer)
            Dim i As Integer

            '/**********Start,Jatin Gangajaliya,2011/04/08**********/
            ReDim fileuploader(optionno)
            ReDim imagearray(optionno)
            ReDim lnkbtnary(optionno)
            '/**********************End***************************/
            Try
                cmb_rgtans = New System.Web.UI.HtmlControls.HtmlSelect
                cmb_QueLevel = New DropDownList
                cmb_QueLevel.AutoPostBack = False
                lstoptions = New ListBox
                lstoptions.SelectionMode = ListSelectionMode.Multiple
                ReDim optarray(0)
                ReDim validation(0)

                tbrow = New HtmlTableRow
                TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)

                'tbdat = New HtmlTableCell
                'tbrow.Cells.Add(tbdat)
                tbdat = New HtmlTableCell
                tbdat.Attributes.Add("class", "tdcontent_label")

                Dim lmend As New Literal()
                lmend.Text = Resources.Resource.QuestionAns_Correctoptn & "<span class='mand'> *</span>"
                lmend.EnableViewState = True

                tbdat.Controls.Add(lmend)

                'tbdat.InnerText = "Correct Option" & "*"
                tbrow.Cells.Add(tbdat)
                tbdat = New HtmlTableCell
                tbdat.Attributes.Add("class", "tdcontent_data")
                lstoptions.EnableViewState = True
                lstoptions.Attributes.Add("id", "Listopt")
                lstoptions.Attributes.Add("style", "width:60px")
                tbdat.Controls.Add(lstoptions)
                tbrow.Cells.Add(tbdat)

                'cmb_QueLevel.Attributes.Add("onmouseover", "addTitleAttributesLev()")
                'Dim l1 As New ListItem
                'l1.Text = "Basic"
                'l1.Value = "0"
                'cmb_QueLevel.Items.Add(l1)
                'Dim l2 As New ListItem
                'l2.Text = "Intermediate"
                'l2.Value = "1"
                'cmb_QueLevel.Items.Add(l2)


                'Dim l3 As New ListItem
                'l3.Text = "High"
                'l3.Value = "2"
                'cmb_QueLevel.Items.Add(l3)
                'tbrow = New HtmlTableRow

                'TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)
                ''tbdat = New HtmlTableCell
                ''tbrow.Cells.Add(tbdat)
                'tbdat = New HtmlTableCell
                'tbdat.InnerText = Resources.Resource.QuestionAns_Quesnlvl
                'tbdat.Attributes.Add("class", "tdcontent_label")
                'tbrow.Cells.Add(tbdat)
                'tbdat = New HtmlTableCell
                'cmb_QueLevel.EnableViewState = True
                'tbdat.Controls.Add(cmb_QueLevel)
                'tbdat.Attributes.Add("class", "tdcontent_data")
                'tbrow.Cells.Add(tbdat)

                ''/*****************Start,Jatin Gangajaliya,2011/04/08*****************/
                'tbrow = New HtmlTableRow
                'TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)
                'tbdat = New HtmlTableCell
                'tbdat.Attributes.Add("class", "tdcontent_label")
                'Dim g As New Literal
                'g.Text = Resources.Resource.WeightMgt_ttlMrk & "<span class='mand'> *</span>"
                'tbdat.Controls.Add(g)
                ''tbdat.InnerText = "Total Marks" & "<span class='mand'> *</span>"
                'txtreadonly = New TextBox
                'txtreadonly.EnableViewState = True
                'txtreadonly.ID = "TxtReadOnly"
                'txtreadonly.MaxLength = 2
                ''txtreadonly.Text = "1"
                'txtreadonly.Enabled = True
                'tbrow.Cells.Add(tbdat)
                'tbdat = New HtmlTableCell
                'tbdat.Controls.Add(txtreadonly)
                'tbdat.Attributes.Add("class", "tdcontent_data")
                'tbrow.Cells.Add(tbdat)

                tbrow = New HtmlTableRow
                TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)

                For i = 0 To optionno
                    'tbrow = New HtmlTableRow
                    'TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)
                    tbdat = New HtmlTableCell
                    'tbdat.Width = "50px"

                    Dim l As New Literal()
                    If i = 0 Or i = 1 Then
                        l.Text = Resources.Resource.QuestionAns_optn & i + 1 & "<span class='mand'>*</span>"
                    Else
                        l.Text = Resources.Resource.QuestionAns_optn & i + 1
                    End If
                    l.EnableViewState = True
                    tbdat.Controls.Add(l)


                    'tbrow.Cells.Add(tbdat)
                    'tbdat = New HtmlTableCell
                    tbdat.Width = "180px"
                    ReDim Preserve optarray(UBound(optarray) + 1)
                    ReDim Preserve validation(UBound(validation) + 1)
                    optarray(i) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarray(i).EnableViewState = True
                    validation(i) = New RequiredFieldValidator
                    optarray(i).ID = "txt_option" & i + 1
                    optarray(i).MaxLength = 50
                    optarray(i).Size = 40

                    'Note: Code for first two textboxes (that can't be blank) validation.
                    'If (i = 0 Or i = 1) Then
                    '    validation(i).Display = ValidatorDisplay.None
                    '    validation(i).ErrorMessage = "Option" & i + 1 & " can't be blank"
                    '    validation(i).ControlToValidate = optarray(i).ID
                    '    tbdat.Controls.Add(validation(i))
                    'End If

                    'tbdat.Attributes.Add("class", "tdcontent_data")

                    '/******************start,Jatin Gangajaliya,2011/04/08***************/
                    fileuploader(i) = New FileUpload
                    fileuploader(i).ID = "fileuldrop" & i + 1
                    fileuploader(i).EnableViewState = True

                    imagearray(i) = New Literal
                    imagearray(i).ID = "imageoption" & i
                    imagearray(i).Visible = False
                    imagearray(i).EnableViewState = True

                    If Request.QueryString("qid") <> Nothing Then
                        lnkbtnary(i) = New LinkButton
                        lnkbtnary(i).ID = "lnkbtn" & i
                        lnkbtnary(i).Text = Resources.Resource.Common_btnClr
                        lnkbtnary(i).EnableViewState = True
                        If imagearray(i).Text <> Nothing Then
                            lnkbtnary(i).Visible = True
                        End If
                        lnkbtnary(i).CommandName = "lnkbtn" & i
                        AddHandler lnkbtnary(i).Click, AddressOf lnkbtnary_click
                    End If

                    tbdat.Controls.Add(optarray(i))
                    tbdat.Controls.Add(fileuploader(i))
                    tbdat.Controls.Add(imagearray(i))
                    If Request.QueryString("qid") <> Nothing Then
                        tbdat.Controls.Add(lnkbtnary(i))
                    End If
                    '/******************End*****************/
                    tbrow.Cells.Add(tbdat)

                    lstoptions.Width = Unit.Pixel(50)
                    lstoptions.Items.Add(CStr(i + 1))
                    lstoptions.Items(i).Value = i + 1
                    lstoptions.Items(i).Text = i + 1

                    '/*****************Start,Jatin Gangajaliya,2011/04/08*****************/
                    'cmb_rgtans.Items.Add(CStr(i + 1))
                    'cmb_rgtans.Items(i).Value = i + 1
                    'cmb_rgtans.Items(i).Text = i + 1
                    'cmb_rgtans.Attributes.Add("onmouseover", "addTitleAttributesOp()")
                    '/***************End**********************/
                    tbdat = New HtmlTableCell
                    tbdat.Attributes.Add("width", "5px")
                Next i


                '/***************************End*****************************/

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                i = Nothing
            End Try
        End Sub
#End Region
        '28
        '#Region "gen_textboxLis" '28
        '        Private Sub gen_textboxLis(ByVal optionno As Integer)
        '            Dim j As Integer

        '            '/**********Start,Jatin Gangajaliya,2011/04/08**********/
        '            ReDim fileuploader(optionno)
        '            ReDim imagearray(optionno)
        '            ReDim lnkbtnary(optionno)
        '            '/**********************End***************************/
        '            Try
        '                cmb_rgtans = New System.Web.UI.HtmlControls.HtmlSelect
        '                cmb_QueLevel = New DropDownList
        '                cmb_QueLevel.AutoPostBack = False
        '                lstoptions = New ListBox
        '                lstoptions.SelectionMode = ListSelectionMode.Multiple
        '                ReDim optarray(0)
        '                ReDim validation(0)

        '                tbrow = New HtmlTableRow
        '                TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)

        '                For j = 0 To optionno
        '                    tbrow = New HtmlTableRow
        '                    TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)
        '                    tbdat = New HtmlTableCell
        '                    tbdat.Width = "11%"

        '                    Dim l As New Literal()
        '                    If j = 0 Or j = 1 Then
        '                        l.Text = "Option" & j + 1 & "<span class='mand'> *</span>"
        '                    Else
        '                        l.Text = "Option" & j + 1
        '                    End If
        '                    l.EnableViewState = True
        '                    tbdat.Controls.Add(l)

        '                    tbdat.Attributes.Add("class", "tdcontent_label")
        '                    tbrow.Cells.Add(tbdat)
        '                    tbdat = New HtmlTableCell
        '                    tbdat.Width = "89%"
        '                    ReDim Preserve optarray(UBound(optarray) + 1)
        '                    ReDim Preserve validation(UBound(validation) + 1)
        '                    optarray(j) = New System.Web.UI.HtmlControls.HtmlInputText
        '                    optarray(j).EnableViewState = True
        '                    validation(j) = New RequiredFieldValidator
        '                    optarray(j).ID = "txt_optionn" & j + 1
        '                    optarray(j).MaxLength = 50
        '                    optarray(j).Size = 70

        '                    'Note: Code for first two textboxes (that can't be blank) validation.
        '                    'If (i = 0 Or i = 1) Then
        '                    '    validation(i).Display = ValidatorDisplay.None
        '                    '    validation(i).ErrorMessage = "Option" & i + 1 & " can't be blank"
        '                    '    validation(i).ControlToValidate = optarray(i).ID
        '                    '    tbdat.Controls.Add(validation(i))
        '                    'End If

        '                    tbdat.Attributes.Add("class", "tdcontent_data")

        '                    '/******************start,Jatin Gangajaliya,2011/04/08***************/
        '                    fileuploader(j) = New FileUpload
        '                    fileuploader(j).ID = "fileuldrop" & j + 1
        '                    fileuploader(j).EnableViewState = True

        '                    imagearray(j) = New Literal
        '                    imagearray(j).ID = "imageoption" & j
        '                    imagearray(j).Visible = False
        '                    imagearray(j).EnableViewState = True

        '                    If Request.QueryString("qid") <> Nothing Then
        '                        lnkbtnary(j) = New LinkButton
        '                        lnkbtnary(j).ID = "lnkbtn" & j
        '                        lnkbtnary(j).Text = "Clear"
        '                        lnkbtnary(j).EnableViewState = True
        '                        If imagearray(j).Text <> Nothing Then
        '                            lnkbtnary(j).Visible = True
        '                        End If
        '                        lnkbtnary(j).CommandName = "lnkbtn" & j
        '                        AddHandler lnkbtnary(j).Click, AddressOf lnkbtnary_click
        '                    End If

        '                    tbdat.Controls.Add(optarray(j))
        '                    tbdat.Controls.Add(fileuploader(j))
        '                    tbdat.Controls.Add(imagearray(j))
        '                    If Request.QueryString("qid") <> Nothing Then
        '                        tbdat.Controls.Add(lnkbtnary(j))
        '                    End If
        '                    '/******************End*****************/
        '                    tbrow.Cells.Add(tbdat)

        '                    lstoptions.Width = Unit.Pixel(50)
        '                    lstoptions.Items.Add(CStr(j + 1))
        '                    lstoptions.Items(j).Value = j + 1
        '                    lstoptions.Items(j).Text = j + 1

        '                    '/*****************Start,Jatin Gangajaliya,2011/04/08*****************/
        '                    'cmb_rgtans.Items.Add(CStr(i + 1))
        '                    'cmb_rgtans.Items(i).Value = i + 1
        '                    'cmb_rgtans.Items(i).Text = i + 1
        '                    'cmb_rgtans.Attributes.Add("onmouseover", "addTitleAttributesOp()")
        '                    '/***************End**********************/
        '                Next j

        '                tbrow = New HtmlTableRow
        '                TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)

        '                'tbdat = New HtmlTableCell
        '                'tbrow.Cells.Add(tbdat)
        '                tbdat = New HtmlTableCell
        '                tbdat.Attributes.Add("class", "tdcontent_label")

        '                Dim lmend As New Literal()
        '                lmend.Text = "Correct Option" & "<span class='mand'> *</span>"
        '                lmend.EnableViewState = True

        '                tbdat.Controls.Add(lmend)

        '                'tbdat.InnerText = "Correct Option" & "*"
        '                tbrow.Cells.Add(tbdat)
        '                tbdat = New HtmlTableCell
        '                tbdat.Attributes.Add("class", "tdcontent_data")
        '                lstoptions.EnableViewState = True
        '                tbdat.Controls.Add(lstoptions)
        '                tbrow.Cells.Add(tbdat)
        '                'nirav
        '                'cmb_QueLevel.Attributes.Add("onmouseover", "addTitleAttributesLev()")
        '                Dim l1 As New ListItem
        '                l1.Text = "Basic"
        '                l1.Value = "0"
        '                cmb_QueLevel.Items.Add(l1)
        '                Dim l2 As New ListItem
        '                l2.Text = "Intermediate"
        '                l2.Value = "1"
        '                cmb_QueLevel.Items.Add(l2)

        '                tbrow = New HtmlTableRow

        '                TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)
        '                'tbdat = New HtmlTableCell
        '                'tbrow.Cells.Add(tbdat)
        '                tbdat = New HtmlTableCell
        '                tbdat.InnerText = "Question Level"
        '                tbdat.Attributes.Add("class", "tdcontent_label")
        '                tbrow.Cells.Add(tbdat)
        '                tbdat = New HtmlTableCell
        '                cmb_QueLevel.EnableViewState = True
        '                tbdat.Controls.Add(cmb_QueLevel)
        '                tbdat.Attributes.Add("class", "tdcontent_data")
        '                tbrow.Cells.Add(tbdat)

        '                '/*****************Start,Jatin Gangajaliya,2011/04/08*****************/
        '                tbrow = New HtmlTableRow
        '                TABLE2.Rows.Insert(TABLE2.Rows.Count - 1, tbrow)
        '                tbdat = New HtmlTableCell
        '                tbdat.Attributes.Add("class", "tdcontent_label")
        '                Dim g As New Literal
        '                g.Text = "Total Marks" & "<span class='mand'> *</span>"
        '                tbdat.Controls.Add(g)
        '                'tbdat.InnerText = "Total Marks" & "<span class='mand'> *</span>"
        '                txtreadonly = New TextBox
        '                txtreadonly.EnableViewState = True
        '                txtreadonly.ID = "TxtReadOnly"
        '                txtreadonly.MaxLength = 2
        '                'txtreadonly.Text = "1"
        '                txtreadonly.Enabled = True
        '                tbrow.Cells.Add(tbdat)
        '                tbdat = New HtmlTableCell
        '                tbdat.Controls.Add(txtreadonly)
        '                tbdat.Attributes.Add("class", "tdcontent_data")
        '                tbrow.Cells.Add(tbdat)
        '                '/***************************End*****************************/

        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Throw ex
        '            Finally
        '                j = Nothing
        '            End Try
        '        End Sub
        '#End Region


        '************************************************************************************************
        'Function               :   This function is called every time the img_saveexit image button is
        '                           clicked
        '
        'Return                 :   None
        '
        'Argument               :   sender = system.object
        '                           e = system.eventargs
        '
        'Explanation            :   Whenever the img_saveexit image button is clicked this event is called
        '
        'Note                   :   The arguments are builtin
        '************************************************************************************************
        'Rajat
        Protected Sub ddlSectionDes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSectionDes.SelectedIndexChanged
            Try
                populatesubject()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx")
            End Try
        End Sub
#Region "img_saveexit_Click"
        'Desc: This is Save button click event.
        'By: Jatin Gangajaliya, 2011/04/11.

        Private Sub img_saveexit_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles img_saveexit.Click

            Dim RetOptsValue As OptSame
            Dim intloop As Integer
            Dim sqlTrans As SqlTransaction
            Dim RetInsert As Integer

            Try
                '/******************Start,Jatin Gangajaliya,2011/04/11*******************/

                Dim intopcount As Integer = CInt(Session.Item("noOfOptions").ToString)
                Dim Txt(intopcount - 1) As System.Web.UI.HtmlControls.HtmlInputText
                Dim strcombo As String = cmb_subject.SelectedValue.ToString()
                ' txtreadonly.Text = hdn.Value ' Commented by rajesh 2014-05-29
                If txt_Question.Text = String.Empty And Not (fileuldque.HasFile) Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.QuestionAns_qcb
                    txt_Question.Focus()
                    Exit Sub
                ElseIf chkblanks.Checked = True Then
                    Dim blankcountt As Integer = 0
                    blankcountt = Regex.Matches(txt_Question.Text, "---").Count
                    If blankcountt = 0 Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_Fiberr
                        Exit Sub
                    End If
                    If txt_Question.Text <> String.Empty Then
                        RetInsert = CheckforSameQuestion()
                        If RetInsert = -1 Then
                            lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                            lblMsg.Text = Resources.Resource.QuestionAns_errms
                            lblMsg.Visible = True
                            txt_Question.Focus()
                            Exit Sub
                        End If
                    End If

                ElseIf txt_Question.Text <> String.Empty Then
                    RetInsert = CheckforSameQuestion()
                    If RetInsert = -1 Then
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_errms
                        lblMsg.Visible = True
                        txt_Question.Focus()
                        Exit Sub
                    End If
                End If

                If chknormal.Checked = True Then
                    If optarray.Length >= 1 Then
                        With TABLE2
                            For g As Integer = 0 To intopcount - 1
                                Txt(g) = .FindControl("txt_option" & g + 1)

                                If g = 0 Then
                                    If Txt(0).Value = String.Empty And Not (fileuploader(0).HasFile) Then
                                        lblMsg.Visible = True
                                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                        lblMsg.Text = Resources.Resource.QuestionAns_opterr1
                                        optarray(0).Focus()
                                        Exit Sub
                                    End If
                                End If
                                If g = 1 Then
                                    If Txt(1).Value = String.Empty And Not (fileuploader(1).HasFile) Then
                                        lblMsg.Visible = True
                                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                        lblMsg.Text = Resources.Resource.QuestionAns_opterr2
                                        optarray(1).Focus()
                                        Exit Sub
                                    End If
                                End If
                            Next
                        End With
                    End If
                End If

                '-----------Added by Nisha-----------

                If chkcho.Checked = True Then


                    'Added by Rahul Shukla on 2019/06/07
                    ' Reason: It will Validate Upload File only if File is empty or File Format is not Mp3/Wav.    
                    ' Desc: File Upload Should not Empty.
                    ' BugID: 1097

                    If Not (fileUpldaudio.HasFile) Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_audioerr
                        fileUpldaudio.Focus()
                        Exit Sub

                    Else
                        If fileUpldaudio.HasFile Then ' Added for Upload Video check(is it selected or not) [Pragnesha 21-5-2019]

                            If fileUpldaudio.PostedFile.ContentType = "audio/wav" OrElse fileUpldaudio.PostedFile.ContentType = "audio/mp3" Then
                                lblMsg.Visible = True
                                lblMsg.ForeColor = System.Drawing.Color.FromName("Green")
                                lblMsg.Text = Resources.Resource.QuestionAns_uperr
                                fileUpldaudio.Focus()
                            Else
                                lblMsg.Visible = True
                                lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                lblMsg.Text = Resources.Resource.QuestionAns_fileerr
                                fileUpldaudio.Focus()
                                Exit Sub
                            End If
                        End If
                    End If

                    If optarray.Length >= 1 Then
                        With TABLE2
                            For g As Integer = 0 To intopcount - 1
                                Txt(g) = .FindControl("txt_option" & g + 1)

                                If g = 0 Then
                                    If Txt(0).Value = String.Empty And Not (fileuploader(0).HasFile) Then
                                        lblMsg.Visible = True
                                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                        lblMsg.Text = Resources.Resource.QuestionAns_opterr1
                                        optarray(0).Focus()
                                        Exit Sub
                                    End If
                                End If
                                If g = 1 Then
                                    If Txt(1).Value = String.Empty And Not (fileuploader(1).HasFile) Then
                                        lblMsg.Visible = True
                                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                        lblMsg.Text = Resources.Resource.QuestionAns_opterr2
                                        optarray(1).Focus()
                                        Exit Sub
                                    End If
                                End If
                            Next
                        End With
                    End If
                End If



                If chkblanks.Checked = True Then
                    If optarray(0).Value = String.Empty Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_opterr1
                        optarray(0).Focus()
                        Exit Sub
                    End If
                End If

                RetOptsValue = New OptSame
                RetOptsValue = CompareOptions()
                If RetOptsValue.Status = True Then
                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.QuestionAns_ep1 & (RetOptsValue.Opt1 + 1) & Resources.Resource.QuestionAns_ep2 & (RetOptsValue.Opt2 + 1) & Resources.Resource.QuestionAns_ep3
                    lblMsg.Visible = True
                    optarray(RetOptsValue.Opt2).Focus()
                    Exit Sub
                End If

                If Session.Item("normal") = "true" Then
                    Dim bool As Boolean = True
                    For n As Integer = 0 To lstoptions.Items.Count - 1
                        If lstoptions.Items(n).Selected = True Then
                            bool = False
                            Exit For
                        End If
                    Next

                    If bool = True Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_atone
                        lstoptions.Focus()
                        Exit Sub
                    End If
                End If

                '----------Added by Nisha-----------
                If Session.Item("listen") = "true" Then
                    Dim bool As Boolean = True
                    For n As Integer = 0 To lstoptions.Items.Count - 1
                        If lstoptions.Items(n).Selected = True Then
                            bool = False
                            Exit For
                        End If
                    Next

                    If bool = True Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_atone
                        lstoptions.Focus()
                        Exit Sub
                    End If
                End If

                If txtreadonly.Text = String.Empty Then
                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.CourseRegistration_ValTtlMrk
                    lblMsg.Visible = True
                    txtreadonly.Focus()
                    Exit Sub
                Else
                    Dim booltotalmark As Boolean
                    booltotalmark = FieldCheck(txtreadonly.Text)
                    If Not booltotalmark Then
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_Digerr
                        lblMsg.Visible = True
                        txtreadonly.Focus()
                        Exit Sub
                    End If
                End If

                Dim blankcount As Integer = 0
                Dim boolans As Boolean
                If chkblanks.Checked = True Then
                    blankcount = Regex.Matches(txt_Question.Text, "---").Count
                    If blankcount <> 0 Then
                        For h As Integer = 0 To blankcount - 1
                            If optarrayans(h).Value = String.Empty Then
                                lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                lblMsg.Text = Resources.Resource.QuestionAns_Anserr & h + 1
                                lblMsg.Visible = True
                                optarrayans(h).Focus()
                                Exit Sub
                                'Else
                                '    boolans = FieldCheck(optarrayans(h).Value)
                                '    If Not boolans Then
                                '        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                '        lblMsg.Text = "Please Enter Digits only"
                                '        lblMsg.Visible = True
                                '        optarrayans(h).Focus()
                                '        Exit Sub
                                '    End If
                            End If
                        Next
                    End If

                    For s As Integer = 0 To optarrayans.Length - 1
                        If s < blankcount Then
                        Else
                            If optarrayans(s).Value <> String.Empty Then
                                lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                lblMsg.Text = Resources.Resource.QuestionAns_onlyans & blankcount & Resources.Resource.QuestionAns_Ans
                                'If lblMsg.Text = "Please Enter only 0 Answers" Then
                                '    lblMsg.Text = "Please Enter fill in the blanks or match the following type question"
                                'End If
                                lblMsg.Visible = True
                                optarrayans(s).Focus()
                                Exit Sub
                            End If
                        End If
                    Next

                End If



                'If chkblanks.Checked = True Then
                '    For r As Integer = 0 To optarrayans.Length - 1
                '        If optarrayans(r).Value <> String.Empty Then
                '            boolans = FieldCheck(optarrayans(r).Value)
                '            If Not boolans Then
                '                lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                '                lblMsg.Text = "Please Enter Digits only"
                '                lblMsg.Visible = True
                '                optarrayans(r).Focus()
                '                Exit Sub
                '            End If
                '        End If
                '    Next
                'End If

                If RetOptsValue.Status = False Then
                    If RetInsert <> -1 Then
                        If objconn.connect() = True Then
                            sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            RetInsert = ins_questions()
                            sqlTrans.Commit()
                        End If
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Green")
                        lblMsg.Text = Resources.Resource.QuestionAns_regsuces
                        lblMsg.Visible = True
                        txt_Question.Text = ""
                        fnc_txtmakeblank()
                        If strcombo <> "" Then
                            cmb_subject.SelectedValue = strcombo
                        End If
                        Me.SetFocus(txt_Question)
                    End If
                End If

                'If RetOptsValue.Status = False Then
                '    If RetInsert <> -1 Then
                '        If objconn.connect(strPathDb) = True Then
                '            sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                '            RetInsert = ins_questions()
                '            sqlTrans.Commit()
                '        End If
                '        lblMsg.ForeColor = System.Drawing.Color.FromName("Green")
                '        lblMsg.Text = "Question Registered Successfully."
                '        lblMsg.Visible = True
                '        txt_Question.Text = ""
                '        fnc_txtmakeblank()
                '        Me.SetFocus(txt_Question)
                '    End If
                'Else
                '    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                '    lblMsg.Text = "The Options " & (RetOptsValue.Opt1 + 1) & " and " & (RetOptsValue.Opt2 + 1) & " have the same value. Please provide unique options."
                '    lblMsg.Visible = True
                'End If
                '/******************************End**************************************/

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                sqlTrans.Rollback()
                Response.Redirect("error.aspx", False)
            Finally
                intloop = Nothing
                RetInsert = Nothing
                RetOptsValue = Nothing
                sqlTrans = Nothing
                'Session.Remove("normal")
                'Session.Remove("blanks")
            End Try
        End Sub
#End Region

#Region "Field Validtion"
        'Desc: This method checks for input data for digits only.
        'By: Jatin Gangajaliya, 2011/04/19.

        Function FieldCheck(ByVal Field As String) As Boolean
            Try
                Dim pattern As String = "^[0-9]{1,2}$"
                Dim reqTextFieldMatch As Match = Regex.Match(Field, pattern)
                If reqTextFieldMatch.Success Then
                    FieldCheck = True
                Else
                    FieldCheck = False
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Function
#End Region


        '************************************************************************************************
        '@(f)
        'Function               :   This function is called to compare four options and 
        '                           return the similar two with the lowest index as
        '                           members of type optsame.
        '
        'Return                 :   optsame
        '
        'Argument               :   None
        '
        'Author                 :   Tabrez
        '************************************************************************************************
#Region "CompareOptions"
        Private Function CompareOptions() As OptSame
            Dim RetOptsSame As OptSame
            Dim intopcount As Integer = CInt(Session.Item("noOfOptions").ToString)
            Dim Txt(intopcount - 1) As System.Web.UI.HtmlControls.HtmlInputText
            Dim Txtblanks(19) As System.Web.UI.HtmlControls.HtmlInputText

            '/*******************Start,Jatin Gangajaliya,2011/04/11***********************/
            Try

                If Session.Item("normal") = "true" Or int <> 3 Then
                    With TABLE2
                        For g As Integer = 0 To intopcount - 1
                            Txt(g) = .FindControl("txt_option" & g + 1)
                        Next
                    End With



                    Dim I, J As Integer
                    For I = 1 To intopcount - 1
                        For J = 0 To I - 1
                            If Txt(J).Value = Txt(I).Value And (Txt(J).Value <> String.Empty) And (Txt(I).Value <> String.Empty) Then
                                RetOptsSame.Status = True
                                RetOptsSame.Opt1 = J
                                RetOptsSame.Opt2 = I
                                Return RetOptsSame
                            End If
                        Next
                    Next
                    RetOptsSame.Status = False
                    RetOptsSame.Opt1 = -1
                    RetOptsSame.Opt2 = -1
                    Return RetOptsSame
                End If

                '-----------Added by Nisha-----------

                If Session.Item("listen") = "true" Or int <> 3 Then
                    With TABLE2
                        For g As Integer = 0 To intopcount - 1
                            Txt(g) = .FindControl("txt_option" & g + 1)
                        Next
                    End With



                    Dim I, J As Integer
                    For I = 1 To intopcount - 1
                        For J = 0 To I - 1
                            If Txt(J).Value = Txt(I).Value And (Txt(J).Value <> String.Empty) And (Txt(I).Value <> String.Empty) Then
                                RetOptsSame.Status = True
                                RetOptsSame.Opt1 = J
                                RetOptsSame.Opt2 = I
                                Return RetOptsSame
                            End If
                        Next
                    Next
                    RetOptsSame.Status = False
                    RetOptsSame.Opt1 = -1
                    RetOptsSame.Opt2 = -1
                    Return RetOptsSame
                End If



                If Session.Item("blanks") = "true" Or int = 3 Then
                    With TABLE3
                        For g As Integer = 0 To 19
                            Txtblanks(g) = .FindControl("txt_option" & g + 1)
                        Next
                    End With

                    Dim I, J As Integer
                    For I = 1 To 19
                        For J = 0 To I - 1
                            If Txtblanks(J).Value = Txtblanks(I).Value And (Txtblanks(J).Value <> String.Empty) And (Txtblanks(I).Value <> String.Empty) Then
                                RetOptsSame.Status = True
                                RetOptsSame.Opt1 = J
                                RetOptsSame.Opt2 = I
                                Return RetOptsSame
                            End If
                        Next
                    Next
                    RetOptsSame.Status = False
                    RetOptsSame.Opt1 = -1
                    RetOptsSame.Opt2 = -1
                    Return RetOptsSame
                End If

                '/*******************************End***********************************/
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                Txt = Nothing
            End Try
        End Function
#End Region


        '************************************************************************************************
        'Function               :   This function is called to insert the questions in the m_question 
        '                           table and options in the m_options table 
        '
        'Return                 :   integer
        '
        'Argument               :   None
        '
        'Explanation            :   The function inserts the questions with the correct answerid and the 
        '                           calls the funtion to insert the options for that question.
        '
        'Note                   :   
        '************************************************************************************************
#Region "ins_questions"
        Private Function ins_questions() As Integer
            Dim mydataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim objconn As New ConnectDb
            Dim OConn As New ConnectDb
            Dim sqlstr As String
            Dim ques_id As Long
            Dim i As Integer
            Dim strPathDb As String
            Dim x As String
            Dim sqlTrans As SqlTransaction
            Dim strbr As StringBuilder
            Try

                '/*************************Start***********************/
                'Note: Commented due to new method has been created for same functionality.
                'By: Jatin Gangajaliya, 2011/04/11.

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                'OConn.connect(strPathDb)
                'x = Replace(Server.HtmlEncode(txt_Question.Text), "'", ",")

                'sqlstr = "select qnid from m_question where question like '% "
                'sqlstr = sqlstr & Replace(Server.HtmlEncode(txt_Question.Text), "'", "''")
                'sqlstr = sqlstr & "%' and test_type='" & cmb_subject.SelectedItem.Value & "' "

                'myCommand = New SqlCommand(sqlstr, OConn.MyConnection)
                'mydataReader = myCommand.ExecuteReader()
                'If True = mydataReader.HasRows Then
                '    Return -1
                'End If
                'OConn.disconnect()
                '/**********************End****************************/

                If objconn.connect() Then

                    sqlstr = ""
                    sqlstr = sqlstr & "SELECT MAX(qnid) FROM m_question WHERE test_type = "
                    sqlstr = sqlstr & "'" & cmb_subject.SelectedItem.Value & "' "

                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                    mydataReader = myCommand.ExecuteReader()

                    While mydataReader.Read
                        If mydataReader.IsDBNull(0) = True Then
                            ques_id = 1
                        Else
                            ques_id = CLng(mydataReader.GetValue(0)) + 1
                        End If
                        Session.Add("qid", ques_id)
                    End While

                    mydataReader.Close()
                    myCommand.Dispose()

                    '/****************Start,Jatin Gangajaliya,2011/04/08******************/
                    Dim strtemp1 As String = "<br/><img src=QuestionImages/"

                    Dim strtempA As String = "<br/><audio src=QuestionAudio/"
                    Dim filename As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".jpg"
                    '---------------Added by Nisha---------------

                    Dim filenameA As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".wav"

                    'Added by Pragnesha Kulkarni on 2018/06/07
                    ' Reason: Chokai question do not display image properly
                    ' BugID: 775 
                    ' Desc: Increased height and width from 100 to 300  
                    Dim strtemp2 As String = " alt= height=300 width=300 >"
                    'Ended by Pragnesha Kulkarni on 2018/06/07

                    Dim strtemp3 As String = "  >"
                    Dim strimageinfo As String = ""
                    Dim straudioinfo As String = ""

                    If fileuldque.HasFile Then
                        fileuldque.SaveAs(Server.MapPath("QuestionImages/" & filename))
                        strimageinfo = strtemp1 & filename & strtemp2
                    End If

                    '---------------Added by Nisha---------------

                    If fileUpldaudio.HasFile Then
                        'Added By Rahul Shukla on 2019/06/07
                        If fileUpldaudio.PostedFile.ContentType = "audio/wav" OrElse fileUpldaudio.PostedFile.ContentType = "audio/mp3" Then
                            fileUpldaudio.SaveAs(Server.MapPath("QuestionAudio/" & filenameA))
                            straudioinfo = strtempA & filenameA & strtemp3
                        End If
                    End If

                    Dim intlistcount As Integer = 1
                    Dim intanscategory As Integer
                    Dim intcategory As Integer = 0

                    If Session.Item("normal") = "true" Then
                        For k As Integer = 0 To lstoptions.Items.Count - 1
                            If lstoptions.Items(k).Selected = True Then
                                intlistcount = intlistcount + 1
                            End If
                        Next
                        If intlistcount = 2 Then
                            intanscategory = 1
                            intcategory = 0
                        ElseIf intlistcount > 2 Then
                            intanscategory = 2
                            intcategory = 1
                        End If

                    ElseIf Session.Item("blanks") = "true" Then
                        intanscategory = 3
                        Dim inttemp As Integer = 0
                        For s As Integer = 0 To optarray.Length - 1
                            If optarray(s).Value <> "" Then
                                inttemp = inttemp + 1
                            End If
                        Next
                        If inttemp = 1 Then
                            intcategory = 0
                        ElseIf inttemp > 1 Then
                            intcategory = 1
                        End If
                        '---------------Added by Nisha---------------
                    Else : Session.Item("listen") = "true"
                        For k As Integer = 0 To lstoptions.Items.Count - 1
                            If lstoptions.Items(k).Selected = True Then
                                intlistcount = intlistcount + 1
                            End If
                        Next
                        If intlistcount = 2 Then
                            intanscategory = 2 ' changed by pragnesha
                            intcategory = 0
                        ElseIf intlistcount > 2 Then
                            intanscategory = 2
                            intcategory = 1
                        End If

                    End If


                    Dim strfinal As String
                    If txt_Question.Text.Contains("---") Then
                        'strfinal = ReturnString()
                        strfinal = txt_Question.Text
                    Else
                        strfinal = txt_Question.Text
                    End If

                    sqlstr = ""
                    sqlstr = sqlstr & "INSERT INTO m_question(qnid,question,Qn_Category_ID,Ans_Category_ID,test_type,qlevel,Total_Marks,Audios) VALUES ("
                    sqlstr = sqlstr & ques_id & ", "
                    sqlstr = sqlstr & "N'" & Replace(strfinal & strimageinfo, "'", "''") & "', "
                    sqlstr = sqlstr & intanscategory & ", "
                    sqlstr = sqlstr & intcategory & ", "
                    sqlstr = sqlstr & "'" & cmb_subject.SelectedItem.Value & "', "
                    sqlstr = sqlstr & "'" & Convert.ToInt32(cmb_QueLevel.SelectedValue.ToString) & "'," & txtreadonly.Text & ","
                    '---------------Added by Nisha---------------
                    sqlstr = sqlstr & "'" & filenameA & "')"

                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection, sqlTrans)
                    myCommand.ExecuteNonQuery()


                    'Insert into M_Question_Answer for normal type questions.
                    If Session.Item("normal") = "true" Then
                        For n As Integer = 0 To lstoptions.Items.Count - 1
                            If lstoptions.Items(n).Selected = True Then
                                strbr = New StringBuilder
                                strbr.Append(" Insert into M_Question_Answer (Qn_ID,Correct_Opt_Id,Test_Type) values ")
                                strbr.Append(" ( ")
                                strbr.Append(ques_id)
                                strbr.Append(" , ")
                                strbr.Append(lstoptions.Items(n).Value)
                                strbr.Append(" , ")
                                strbr.Append(cmb_subject.SelectedItem.Value)
                                strbr.Append(" ) ")
                                sqlstr = strbr.ToString()
                                myCommand = New SqlCommand(sqlstr, objconn.MyConnection, sqlTrans)
                                myCommand.ExecuteNonQuery()
                            End If
                        Next
                    End If


                    'Insert into M_Question_Answer for fill in the blank type questions.
                    If Session.Item("blanks") = "true" Then
                        For n As Integer = 0 To 19
                            'optarray(n) = Nothing
                            If (optarrayans(n).ID = "txtans" & n + 1 And optarrayans(n).Value <> "") Then

                                If optarrayans(n).Value.Contains(",") Then
                                    Dim arydata() As String = optarrayans(n).Value.Split(",")
                                    For k As Integer = 0 To arydata.Length - 1
                                        strbr = New StringBuilder
                                        strbr.Append(" Insert into M_Question_Answer (Qn_ID,Sub_Id,Correct_Opt_Id,Test_Type) values ")
                                        strbr.Append(" ( ")
                                        strbr.Append(ques_id)
                                        strbr.Append(" , ")
                                        strbr.Append(n + 1)
                                        strbr.Append(" , ")
                                        strbr.Append(arydata(k))
                                        strbr.Append(" , ")
                                        strbr.Append(cmb_subject.SelectedItem.Value)
                                        strbr.Append(" ) ")
                                        sqlstr = strbr.ToString()
                                        myCommand = New SqlCommand(sqlstr, objconn.MyConnection, sqlTrans)
                                        myCommand.ExecuteNonQuery()
                                    Next
                                Else
                                    strbr = New StringBuilder
                                    strbr.Append(" Insert into M_Question_Answer (Qn_ID,Sub_Id,Correct_Opt_Id,Test_Type) values ")
                                    strbr.Append(" ( ")
                                    strbr.Append(ques_id)
                                    strbr.Append(" , ")
                                    strbr.Append(n + 1)
                                    strbr.Append(" , ")
                                    strbr.Append(optarrayans(n).Value)
                                    strbr.Append(" , ")
                                    strbr.Append(cmb_subject.SelectedItem.Value)
                                    strbr.Append(" ) ")
                                    sqlstr = strbr.ToString()
                                    myCommand = New SqlCommand(sqlstr, objconn.MyConnection, sqlTrans)
                                    myCommand.ExecuteNonQuery()
                                End If
                            End If
                        Next
                    End If

                    '---------------Added by Nisha---------------
                    ' Insert into M_Question_Answer for listen type questions.
                    If Session.Item("listen") = "true" Then
                        For n As Integer = 0 To lstoptions.Items.Count - 1
                            If lstoptions.Items(n).Selected = True Then
                                strbr = New StringBuilder
                                strbr.Append(" Insert into M_Question_Answer (Qn_ID,Correct_Opt_Id,Test_Type) values ")
                                strbr.Append(" ( ")
                                strbr.Append(ques_id)
                                strbr.Append(" , ")
                                strbr.Append(lstoptions.Items(n).Value)
                                strbr.Append(" , ")
                                strbr.Append(cmb_subject.SelectedItem.Value)
                                strbr.Append(" ) ")
                                sqlstr = strbr.ToString()
                                myCommand = New SqlCommand(sqlstr, objconn.MyConnection, sqlTrans)
                                myCommand.ExecuteNonQuery()
                            End If
                        Next
                    End If

                    'Insert into M_options for normal type questions.
                    If Session.Item("normal") = "true" Then
                        For i = 0 To Session.Item("noOfOptions") - 1
                            ' If Option textbox is blank then It is not inserted
                            Dim k As String = Replace(optarray(i).Value(), "'", "''")
                            If (k <> Nothing Or fileuploader(i).HasFile) Then

                                Dim strtempop1 As String = "<br/><img src=AnswerImages/"
                                Dim filenameop As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & "_" & i + 1 & ".jpg"

                                ' Added by Pragnesha Kulkarni on 2018/06/07
                                ' Reason: Chokai question do not display image properly
                                ' Desc: Increased height and width from 100 to 300  
                                ' BugID: 775
                                Dim strtempop2 As String = "alt= height=300 width=300 >"
                                'Ended by Pragnesha Kulkarni on 2018/06/07 

                                Dim strimageinfoop As String = ""

                                If fileuploader(i).HasFile Then
                                    fileuploader(i).SaveAs(Server.MapPath("AnswerImages/" & filenameop))
                                    strimageinfoop = strtempop1 & filenameop & strtempop2
                                End If
                                ins_options(i + 1, ques_id, Replace(optarray(i).Value() & strimageinfoop, "'", "''"), myCommand, objconn)
                            End If
                        Next
                    End If


                    'Insert into M_options for listen type questions.
                    If Session.Item("listen") = "true" Then
                        For i = 0 To Session.Item("noOfOptions") - 1
                            ' If Option textbox is blank then It is not inserted
                            Dim k As String = Replace(optarray(i).Value(), "'", "''")
                            If (k <> Nothing Or fileuploader(i).HasFile) Then

                                Dim strtempop1 As String = "<br/><img src=AnswerImages/"
                                Dim filenameop As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & "_" & i + 1 & ".jpg"

                                'Added by Pragnesha Kulkarni on 2018/06/07
                                ' Reason: Chokai question do not display image properly
                                ' Desc: Increased height and width from 100 to 300  
                                ' BugID: 775
                                Dim strtempop2 As String = "alt= height=300 width=300 >"
                                'Ended by Pragnesha Kulkarni on 2018/06/07

                                Dim strimageinfoop As String = ""

                                If fileuploader(i).HasFile Then
                                    fileuploader(i).SaveAs(Server.MapPath("AnswerImages/" & filenameop))
                                    strimageinfoop = strtempop1 & filenameop & strtempop2
                                End If
                                ins_options(i + 1, ques_id, Replace(optarray(i).Value() & strimageinfoop, "'", "''"), myCommand, objconn)
                            End If
                        Next
                    End If



                    'Insert into M_options for fill in the blanks type questions.
                    If Session.Item("blanks") = "true" Then
                        For m As Integer = 0 To 19
                            'If (optarrayans(m).ID = "txtans" & m + 1 And optarrayans(m).Value <> "") Then
                            If (optarray(m).Value <> "") Then
                                ins_options(m + 1, ques_id, Replace(optarray(m).Value(), "'", "''"), myCommand, objconn)
                            End If
                        Next
                    End If

                    myCommand.Dispose()
                    objconn.disconnect()
                    Return 0
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                objconn = Nothing
                OConn = Nothing
                sqlstr = Nothing
                ques_id = Nothing
                i = Nothing
                strPathDb = Nothing
                mydataReader = Nothing
                myCommand = Nothing
                x = Nothing
                strbr = Nothing
                sqlTrans = Nothing
            End Try
        End Function



#End Region

        '************************************************************************************************
        'Function               :   This function is called to insert the options in the m_options 
        '                           table. 
        '
        'Return                 :   None
        '
        'Argument               :   i as integer (for option no)
        '                           quesid as long (for question no)
        '                           options as string (for the text of option)
        '                           myCommand as SqlCommand (SqlCommand type object)
        '                           objconn as ConnectDb (object of ConnectClass class
        '
        'Explanation            :   The function inserts the options with the corresponding ques no.
        '
        'Note                   :   
        '************************************************************************************************#Region ""
#Region "ins_options"

        Private Sub ins_options(ByVal i As Integer, ByVal quesid As Long, ByVal options As String, ByVal myCommand As SqlCommand, ByVal objconn As ConnectDb)
            Dim sqlstr As String
            Dim sqlTrans As SqlTransaction
            Try
                ''For i = 1 To 4
                ''********************

                'Dim strtemp1 As String = "<br/><img src=QuestionImages/"
                'Dim filename As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".jpg"
                'Dim strtemp2 As String = " alt= hieght=100 width=100 >"
                'Dim strimageinfo As String = ""

                'If fileuldque.HasFile Then
                '    fileuldque.SaveAs(Server.MapPath("QuestionImages/" & filename))
                '    strimageinfo = strtemp1 & filename & strtemp2
                'End If
                ''*********************

                sqlstr = ""
                sqlstr = sqlstr & "INSERT INTO m_options VALUES("
                sqlstr = sqlstr & i & ", "
                sqlstr = sqlstr & quesid & ", "
                sqlstr = sqlstr & "N'" & options & "', "
                sqlstr = sqlstr & "'" & cmb_subject.SelectedItem.Value & "')"
                sqlstr = Replace(sqlstr, ",'',", ",NULL,", 1, -1, 1)
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection, sqlTrans)
                myCommand.ExecuteNonQuery()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                sqlTrans.Rollback()
                Throw ex
            Finally
                sqlstr = Nothing
                sqlTrans = Nothing
            End Try
        End Sub
#End Region

#Region "update_question"

        Private Function update_question(ByVal qid As Integer, ByVal testType As String, ByVal QuesFlag As Boolean) As Integer
            '*****************************************************************************
            'Code Added by Tabrez 
            'Purpose: Same question should not be accepted twice.
            '*****************************************************************************
            Dim mydataReader As SqlDataReader
            Dim OConn As New ConnectDb
            Dim sqlstr As String
            Dim sqlstr1 As String
            Dim ques_id As Long
            Dim i As Integer
            Dim MyCommand1 As SqlCommand
            Dim MyCommand As SqlCommand
            Dim StrPathDb As String
            Dim mydataReader1 As SqlClient.SqlDataReader
            Try
                If True = QuesFlag Then
                    'StrPathDb = ConfigurationSettings.AppSettings("PathDb")
                    'OConn.connect(StrPathDb)
                    sqlstr = "select qnid from m_question where question like '" & Replace(Server.HtmlEncode(txt_Question.Text), "'", "''") & "%' and test_type='" & testType & "' and not qnid =" & qid
                    MyCommand = New SqlCommand(sqlstr, objconnect, Sqltransc)
                    mydataReader = MyCommand.ExecuteReader
                    If True = mydataReader.HasRows Then
                        Return -1
                    End If
                    mydataReader.Close()
                    'OConn.disconnect()
                End If

                ques_id = qid


                '*****************************************************************************
                Dim sqlupdate As String
                Dim objconn As New ConnectDb

                'Dim optext As String = txt_Question.Text
                'Dim filename As String = "QuestionImages/" & testType & "_" & testType & "_" & i & ".jpg"
                'If (fileuploader(i - 1).HasFile) Then

                '    fileuploader(i - 1).SaveAs(Server.MapPath(filename))
                '    optext = optext & "<br/><img src=" & filename & " hieght=100 width=100 >"
                'Else
                '    If (imagearray(i - 1).Text <> "") Then
                '        optext = optext & "<br/>" & imagearray(i - 1).Text
                '    End If

                'End If

                Dim intlistcount As Integer = 1
                Dim intcategory As Integer = 0
                Dim intanscategory As Integer




                'Normal & Blank type of question Below code is used
                'If int <> 3 Then
                '    For k As Integer = 0 To lstoptions.Items.Count - 1
                '        If lstoptions.Items(k).Selected = True Then
                '            intlistcount = intlistcount + 1
                '        End If
                '    Next
                '    If intlistcount = 2 Then
                '        intanscategory = 1
                '        intcategory = 0
                '    ElseIf intlistcount > 2 Then
                '        intanscategory = 2
                '        intcategory = 1
                '    End If
                'Else
                '    intanscategory = 3
                '    Dim inttemp As Integer = 0
                '    For s As Integer = 0 To optarray.Length - 1
                '        If optarray(s).Value <> "" Then
                '            inttemp = inttemp + 1
                '        End If
                '    Next
                '    If inttemp = 1 Then
                '        intcategory = 0
                '    ElseIf inttemp > 1 Then
                '        intcategory = 1
                '    End If
                'End If

                ''For normal type questions.
                'If int <> 3 Then
                '    Dim strtemp1 As String = "<br/><img src=QuestionImages/"
                '    '-----------Added by Nisha-----------
                '    Dim strtempA As String = "<br/><audio src=QuestionAudio/"
                '    Dim filename As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".jpg"
                '    Dim filenameA As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".wav"

                '    '  Dim strtemp2 As String = " alt= height=100 width=100 >"
                '    ' Added by Pragnesha Kulkarni on 2018/06/07
                '    ' Reason: Chokai question do not display image properly
                '    ' Desc: Increased height and width from 100 to 300  
                '    ' BugID: 775
                '    Dim strtemp2 As String = " alt= height=300 width=300 >"
                '    'Ended by Pragnesha Kulkarni on 2018/06/07

                '    Dim strtemp3 As String = " >"
                '    Dim strimageinfo As String = ""
                '    Dim straudioinfo As String = ""

                '    Dim strfinal As String
                '    strfinal = txt_Question.Text


                '    Dim optext As String = strfinal
                '    If fileuldque.HasFile Then
                '        fileuldque.SaveAs(Server.MapPath("QuestionImages/" & filename))
                '        strimageinfo = strtemp1 & filename & strtemp2
                '        optext = optext & strimageinfo
                '    Else
                '        If (quesimage.Text <> "") Then
                '            optext = optext & "<br/>" & quesimage.Text
                '        End If
                '    End If
                '    ' --------------Added by Nisha---------------

                '    If fileUpldaudio.HasFile Then
                '        fileUpldaudio.SaveAs(Server.MapPath("QuestionAudio/" & filenameA))
                '        straudioinfo = strtempA & filenameA & strtemp3
                '        optext = optext & straudioinfo
                '    Else
                '        If (queAudio.Text <> "") Then
                '            optext = optext & "<br/>" & queAudio.Text
                '        End If
                '    End If

                '    'If objconn.connect(StrPathDb) Then
                '    sqlupdate = ""
                '    sqlupdate = sqlupdate & "UPDATE m_question SET "
                '    sqlupdate = sqlupdate & "question = '" & Replace(optext, "'", "''") & "', "
                '    sqlupdate = sqlupdate & "qlevel = '" & cmb_QueLevel.SelectedIndex & "'" & ", "

                '    sqlupdate = sqlupdate & "Qn_Category_ID = '" & intanscategory & "'" & ", "
                '    sqlupdate = sqlupdate & "Ans_Category_ID = '" & intcategory & "'"

                '    '  sqlupdate = sqlupdate & ", correct_ansid = " & cmb_rgtans.Value & " WHERE "
                '    sqlupdate = sqlupdate & ", Total_Marks = " & txtreadonly.Text & " WHERE "
                '    '   sqlupdate = sqlupdate & " WHERE "
                '    sqlupdate = sqlupdate & "qnid = " & qid & " AND "
                '    sqlupdate = sqlupdate & "test_type = '" & testType & "'"
                '    MyCommand = New SqlCommand(sqlupdate, objconnect, Sqltransc)
                '    MyCommand.ExecuteNonQuery()
                '    MyCommand.Dispose()
                '    'objconn.disconnect()
                '    Return 0
                '    'End If
                'End If


                ' Changed Question category as per session by Pragnesha 22/4/2019
                'If Session.Item("normal") = "true" Then
                If int = 1 Then
                    For k As Integer = 0 To lstoptions.Items.Count - 1
                        If lstoptions.Items(k).Selected = True Then
                            intlistcount = intlistcount + 1
                        End If
                    Next
                    If intlistcount = 2 Then
                        intanscategory = 1
                        intcategory = 0
                    ElseIf intlistcount > 2 Then
                        intanscategory = 2
                        intcategory = 1
                    End If
                ElseIf int = 2 Then
                    For k As Integer = 0 To lstoptions.Items.Count - 1
                        If lstoptions.Items(k).Selected = True Then
                            intlistcount = intlistcount + 1
                        End If
                    Next
                    If intlistcount = 2 Then
                        intanscategory = 2
                        intcategory = 0
                    ElseIf intlistcount > 2 Then
                        intanscategory = 2
                        intcategory = 1
                    End If

                Else
                    intanscategory = 3
                    Dim inttemp As Integer = 0
                    For s As Integer = 0 To optarray.Length - 1
                        If optarray(s).Value <> "" Then
                            inttemp = inttemp + 1
                        End If
                    Next
                    If inttemp = 1 Then
                        intcategory = 0
                    ElseIf inttemp > 1 Then
                        intcategory = 1
                    End If
                End If



                'For normal type questions.
                If int = 1 Then
                    Dim strtemp1 As String = "<br/><img src=QuestionImages/"
                    '-----------Added by Nisha-----------     
                    Dim strtempA As String = "<br/><audio src=QuestionAudio/"
                    Dim filename As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".jpg"
                    Dim filenameA As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".wav"

                    '  Dim strtemp2 As String = " alt= height=100 width=100 >"
                    ' Added by Pragnesha Kulkarni on 2018/06/07
                    ' Reason: Chokai question do not display image properly
                    ' Desc: Increased height and width from 100 to 300  
                    ' BugID: 775
                    Dim strtemp2 As String = " alt= height=300 width=300 >"
                    'Ended by Pragnesha Kulkarni on 2018/06/07

                    Dim strtemp3 As String = " >"
                    Dim strimageinfo As String = ""
                    Dim straudioinfo As String = ""

                    Dim strfinal As String
                    strfinal = txt_Question.Text


                    Dim optext As String = strfinal
                    If fileuldque.HasFile Then
                        fileuldque.SaveAs(Server.MapPath("QuestionImages/" & filename))
                        strimageinfo = strtemp1 & filename & strtemp2
                        optext = optext & strimageinfo
                    Else
                        If (quesimage.Text <> "") Then
                            optext = optext & "<br/>" & quesimage.Text
                        End If
                    End If
                    ' --------------Added by Nisha---------------

                    If fileUpldaudio.HasFile Then
                        fileUpldaudio.SaveAs(Server.MapPath("QuestionAudio/" & filenameA))
                        straudioinfo = strtempA & filenameA & strtemp3
                        optext = optext & straudioinfo
                    Else
                        If (queAudio.Text <> "") Then
                            optext = optext & "<br/>" & queAudio.Text
                        End If
                    End If

                    'If objconn.connect(StrPathDb) Then
                    sqlupdate = ""
                    sqlupdate = sqlupdate & "UPDATE m_question SET "
                    sqlupdate = sqlupdate & "question = '" & Replace(optext, "'", "''") & "', "
                    sqlupdate = sqlupdate & "qlevel = '" & cmb_QueLevel.SelectedIndex & "'" & ", "

                    sqlupdate = sqlupdate & "Qn_Category_ID = '" & intanscategory & "'" & ", "
                    sqlupdate = sqlupdate & "Ans_Category_ID = '" & intcategory & "'"

                    '  sqlupdate = sqlupdate & ", correct_ansid = " & cmb_rgtans.Value & " WHERE "
                    sqlupdate = sqlupdate & ", Total_Marks = " & txtreadonly.Text & " WHERE "
                    '   sqlupdate = sqlupdate & " WHERE "
                    sqlupdate = sqlupdate & "qnid = " & qid & " AND "
                    sqlupdate = sqlupdate & "test_type = '" & testType & "'"
                    MyCommand = New SqlCommand(sqlupdate, objconnect, Sqltransc)
                    MyCommand.ExecuteNonQuery()
                    MyCommand.Dispose()
                    'objconn.disconnect()
                    Return 0
                    'End If
                End If


                'For listening type questions.
                If int = 2 Then
                    Dim strtemp1 As String = "<br/><img src=QuestionImages/"
                    '-----------Added by Nisha-----------
                    Dim strtempA As String = "<br/><audio src=QuestionAudio/"
                    Dim filename As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".jpg"
                    Dim filenameA As String = cmb_subject.SelectedValue.ToString & "_" & ques_id & ".wav"

                    '  Dim strtemp2 As String = " alt= height=100 width=100 >"
                    ' Added by Pragnesha Kulkarni on 2018/06/07
                    ' Reason: Chokai question do not display image properly
                    ' Desc: Increased height and width from 100 to 300  
                    ' BugID: 775
                    Dim strtemp2 As String = " alt= height=100 width=300 >"
                    'Ended by Pragnesha Kulkarni on 2018/06/07

                    Dim strtemp3 As String = " >"
                    Dim strimageinfo As String = ""
                    Dim straudioinfo As String = ""

                    Dim strfinal As String
                    strfinal = txt_Question.Text

                    Dim strPath As String
                    Dim optext As String = strfinal

                    If fileuldque.HasFile Then

                        'Added by Rahul Shukla on 2019/05/22
                        ' Reason: It was Selecting any file but only Image File should be Select    
                        ' Desc: It was Choose only Image type Format.
                        ' BugID: 1098

                        Dim extension As String = System.IO.Path.GetExtension(fileuldque.FileName)

                        If extension = ".jpeg" Or extension = ".jpg" Or extension = ".png" Or extension = ".bmp" Or extension = ".tiff" Then
                            fileuldque.SaveAs(Server.MapPath("QuestionImages/" & filename))
                            strimageinfo = strtemp1 & filename & strtemp2
                            optext = optext & strimageinfo

                        End If

                    Else
                        If (quesimage.Text <> "") Then
                            optext = optext & "<br/>" & quesimage.Text

                        End If
                    End If
                    ' --------------Added by Nisha---------------

                    If fileUpldaudio.HasFile Then
                        If fileUpldaudio.PostedFile.ContentType = "audio/wav" OrElse fileUpldaudio.PostedFile.ContentType = "audio/mp3" Then

                            'Added by Rahul Shukla on 2019/05/15
                            ' Reason: In optext Variable repeated value/Duplicate Value was coming into audio File path ,Which I separated the Value    
                            ' Desc: Separated the Duplicate value from optext variable 
                            ' BugID: 1098

                            Dim strAudio(1) As String
                            If optext.Contains("<br/><audio") Then
                                strAudio(0) = optext.Substring(0, optext.IndexOf("<br/><audio"))
                                strPath = strAudio(0).ToString()
                            Else
                                strPath = optext.ToString()
                            End If
                            fileUpldaudio.SaveAs(Server.MapPath("QuestionAudio/" & filenameA))
                            straudioinfo = strtempA & filenameA & strtemp3
                            'Added by Rahul Shukla on 15/05/2019
                            strPath = strPath & straudioinfo
                            'optext = optext & straudioinfo
                        Else
                            Response.Write("<script>alert('Invalid Solution');</script>")
                            lblValidmessage.Text = "Invalid Format"
                            Dim strAudio(1) As String
                            If optext.Contains("<br/><audio") Then
                                strAudio(0) = optext.Substring(0, optext.IndexOf("<br/><audio"))
                                strPath = strAudio(0).ToString()
                            Else
                                strPath = optext.ToString()
                            End If
                        End If
                    Else

                        'Added by Rahul Shukla on 2019/05/16
                        If (queAudio.Text <> "") Then
                            strPath = optext.ToString()
                        End If

                        'Else
                        '    If (queAudio.Text <> "") Then
                        '        optext = optext & "<br/>" & queAudio.Text
                        '    End If
                    End If

                    'If objconn.connect(StrPathDb) Then
                    sqlupdate = ""
                    sqlupdate = sqlupdate & "UPDATE m_question SET "
                    'Added by Rahul Shukla on 15/05/2019
                    sqlupdate = sqlupdate & "question = '" & Replace(strPath, "'", "''") & "', "
                    'sqlupdate = sqlupdate & "question = '" & Replace(optext, "'", "''") & "', "
                    sqlupdate = sqlupdate & "qlevel = '" & cmb_QueLevel.SelectedIndex & "'" & ", "

                    sqlupdate = sqlupdate & "Qn_Category_ID = '" & intanscategory & "'" & ", "
                    sqlupdate = sqlupdate & "Ans_Category_ID = '" & intcategory & "'"

                    '  sqlupdate = sqlupdate & ", correct_ansid = " & cmb_rgtans.Value & " WHERE "
                    '  sqlupdate = sqlupdate & ", Total_Marks = " & txtreadonly.Text & " WHERE " ' Commented for adding updated audio file By pragnesha 25-4-2019
                    sqlupdate = sqlupdate & ", Total_Marks = '" & txtreadonly.Text & "'"
                    sqlupdate = sqlupdate & ", Audios = '" & filenameA & "'WHERE "
                    '   sqlupdate = sqlupdate & " WHERE "
                    sqlupdate = sqlupdate & "qnid = " & qid & " AND "
                    sqlupdate = sqlupdate & "test_type = '" & testType & "'"
                    MyCommand = New SqlCommand(sqlupdate, objconnect, Sqltransc)
                    MyCommand.ExecuteNonQuery()
                    MyCommand.Dispose()
                    'objconn.disconnect()
                    Return 0
                    'End If
                End If


                'For blank type questions.
                If int = 3 Then
                    'If objconn.connect(StrPathDb) Then
                    sqlupdate = ""
                    sqlupdate = sqlupdate & "UPDATE m_question SET "
                    sqlupdate = sqlupdate & "question = '" & Replace(txt_Question.Text, "'", "''") & "', "
                    sqlupdate = sqlupdate & "qlevel = '" & cmb_QueLevel.SelectedIndex & "'" & ", "

                    sqlupdate = sqlupdate & "Qn_Category_ID = '" & intanscategory & "'" & ", "
                    sqlupdate = sqlupdate & "Ans_Category_ID = '" & intcategory & "'"
                    sqlupdate = sqlupdate & ", Total_Marks = " & txtreadonly.Text & " WHERE "

                    sqlupdate = sqlupdate & "qnid = " & qid & " AND "
                    sqlupdate = sqlupdate & "test_type = '" & testType & "'"
                    MyCommand = New SqlCommand(sqlupdate, objconnect, Sqltransc)
                    MyCommand.ExecuteNonQuery()
                    MyCommand.Dispose()
                    'objconn.disconnect()
                    Return 0
                    'End If
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then

                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                OConn = Nothing
                sqlstr = Nothing
                StrPathDb = Nothing
                ques_id = Nothing
                i = Nothing
                MyCommand = Nothing
                mydataReader = Nothing
                'sqlTrans = Nothing
            End Try
        End Function
#End Region

#Region "update_options"

        Private Sub update_options(ByVal i As Integer, ByVal somequno As Integer, ByVal options As String, ByVal testType As String)
            Dim sqlupdate As String
            Dim myCommand As SqlCommand
            Dim objconn As New ConnectDb
            'Dim sqlTrans As SqlTransaction

            Dim strPathDb As String
            Dim optext As String = options
            Dim filename As String = "AnswerImages/" & testType & "_" & somequno & "_" & i & ".jpg"

            If int <> 3 Then
                If (fileuploader(i - 1).HasFile) Then
                    fileuploader(i - 1).SaveAs(Server.MapPath(filename))

                    ' Added by Pragnesha Kulkarni on 2018/06/07
                    ' Reason: Chokai question do not display image properly
                    ' Desc: Increased height and width from 100 to 300  
                    ' BugID: 775
                    optext = optext & "<br/><img src=" & filename & " height=300 width=300 >"
                    'Added by Pragnesha Kulkarni on 2018/06/07

                Else
                    If (imagearray(i - 1).Text <> "") Then
                        optext = optext & "<br/>" & imagearray(i - 1).Text
                    End If

                End If
            End If

            If int = 3 Then
                optext = options
            End If

            Try
                strPathDb = ConfigurationSettings.AppSettings("PathDb")
                'If objconn.connect(strPathDb) Then
                sqlupdate = ""

                sqlupdate = sqlupdate & "Insert into M_Options (Optionid,Qnid,[Option],Test_Type) values"
                sqlupdate = sqlupdate & " ( "
                sqlupdate = sqlupdate & i & ","
                sqlupdate = sqlupdate & somequno & ","
                sqlupdate = sqlupdate & "'" & Replace(optext, "'", "''") & "'" & ","
                sqlupdate = sqlupdate & testType
                sqlupdate = sqlupdate & " ) "

                'sqlupdate = sqlupdate & "UPDATE m_options SET "
                ''    sqlupdate = sqlupdate & "[option] = '" & Replace(Server.HtmlEncode(options), "'", "''") & "' WHERE "
                'sqlupdate = sqlupdate & "[option] = '" & Replace(optext, "'", "''") & "' WHERE "
                'sqlupdate = sqlupdate & "test_type = '" & testType & "' AND "
                'sqlupdate = sqlupdate & "qnid = " & somequno & " AND "
                'sqlupdate = sqlupdate & "optionid = " & i

                myCommand = New SqlCommand(sqlupdate, objconnect, Sqltransc)
                myCommand.ExecuteNonQuery()
                myCommand.Dispose()
                'objconn.disconnect()
                'End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                sqlupdate = Nothing
                objconn = Nothing
                strPathDb = Nothing
                myCommand = Nothing
                filename = Nothing
                'sqlTrans = Nothing
            End Try
        End Sub
#End Region

#Region "img_savecont_Click"

        Private Sub img_savecont_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles img_savecont.Click
            Try
                ins_questions()
                fnc_txtmakeblank()
                'Response.Redirect("question_ans.aspx")
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "fnc_txtmakeblank"
        'Desc: To make form data clear.
        'By: Jatin Gangajaliya, 2011/04/18.

        Private Sub fnc_txtmakeblank()
            Try

                If int = 2 Or int = 1 Then
                    MakeCleanforNormal()
                    For i As Integer = 0 To Session.Item("noOfOptions") - 1
                        lnkbtnary(i).Visible = False
                    Next
                End If
                '-------------Added By Nisha-----------
                If int = 2 Or int = 1 Then
                    MakeCleanforListen()
                    For i As Integer = 0 To Session.Item("noOfOptions") - 1
                        lnkbtnary(i).Visible = False
                    Next
                End If

                If int = 3 Then
                    MakeCleanforBlanks()
                End If

                If Session.Item("normal") = "true" Then
                    MakeCleanforNormal()
                End If

                If Session.Item("blanks") = "true" Then
                    MakeCleanforBlanks()
                End If
                '-----------Added by Nisha-----------
                If Session.Item("listen") = "true" Then
                    MakeCleanforListen()
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

#Region "Makecleanfornormal method"
        'Desc:This makes form data clean.
        'By: Jatin Gangajaliya, 2011/04/18.

        Private Sub MakeCleanforNormal()
            Dim i As Integer
            Try
                txt_Question.Text = ""
                lnkquestion.Visible = False
                For i = 0 To Session.Item("noOfOptions") - 1
                    optarray(i).Value = String.Empty
                    imagearray(i).Text = String.Empty
                Next
                For k As Integer = 0 To lstoptions.Items.Count - 1
                    lstoptions.Items(k).Selected = False
                Next
                cmb_QueLevel.SelectedIndex = 0
                quesimage.Text = String.Empty
                cmb_subject.SelectedIndex = -1
                txtreadonly.Text = String.Empty
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                i = Nothing
            End Try
        End Sub
#End Region

        '---------Added by Nisha----------
#Region "Makecleanforlisten method"
        'Desc:This makes form data clean.
        'By: Jatin Gangajaliya, 2011/04/18.

        Private Sub MakeCleanforListen()
            Dim i As Integer
            Try
                txt_Question.Text = ""
                lnkquestion.Visible = False
                For i = 0 To Session.Item("noOfOptions") - 1
                    optarray(i).Value = String.Empty
                    imagearray(i).Text = String.Empty
                Next
                For k As Integer = 0 To lstoptions.Items.Count - 1
                    lstoptions.Items(k).Selected = False
                Next
                cmb_QueLevel.SelectedIndex = 0
                quesimage.Text = String.Empty
                queAudio.Text = String.Empty
                cmb_subject.SelectedIndex = -1
                txtreadonly.Text = String.Empty
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                i = Nothing
            End Try
        End Sub
#End Region



#Region "MakeCleanforBlanks method"
        'Desc:This makes form data clean.
        'By: Jatin Gangajaliya, 2011/04/18.

        Private Sub MakeCleanforBlanks()
            Dim i As Integer
            Try
                txt_Question.Text = ""
                For i = 0 To 19
                    optarray(i).Value = String.Empty
                    optarrayans(i).Value = String.Empty
                Next
                cmb_QueLevel.SelectedIndex = 0
                cmb_subject.SelectedIndex = -1
                txtreadonly.Text = String.Empty
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                i = Nothing
            End Try
        End Sub
#End Region


        'Private Sub img_addmore_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_addmore.Click
        'Session.Add("noOfOptions", Session.Item("noOfOptions") + 1)

        'gen_textbox(Session.Item("noOfOptions") - 1)
        'ins_options(1, ques_id, txt_option1.Value, myCommand, objconn)
        'ins_options(2, ques_id, txt_option2.Value, myCommand, objconn)
        'ins_options(3, ques_id, txt_option3.Value, myCommand, objconn)
        'ins_options(4, ques_id, txt_option4.Value, myCommand, objconn)
        'Dim i As Integer
        'For i = 0 To Session.Item("noOfOptions")
        'tbrow = New HtmlTableRow()
        'TABLE1.Rows.Insert(TABLE1.Rows.Count - 1, tbrow)

        'tbdat = New HtmlTableCell()
        'tbdat.InnerText = "Option" & i + 1
        'tbrow.Cells.Add(tbdat)
        'tbdat = New HtmlTableCell()
        'optarray = New System.Web.UI.HtmlControls.HtmlInputText()
        'optarray.ID = "txt_option" & i + 1
        'tbdat.Controls.Add(optarray)
        'tbrow.Cells.Add(tbdat)
        'Next
        'End Sub


        'Private Sub frm_quesans_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles frm_quesans.Init
        'Dim tbrow As HtmlTableRow
        'Dim tbdat As HtmlTableCell
        'Dim i As Integer
        'Dim sqlstr As String
        'Dim mydataReader As SqlDataReader
        'Dim myCommand As SqlCommand
        'Dim objconn As New ConnectDb()

        '        If objconn.connect(objconn,PATHDB) Then
        '       sqlstr = ""
        '      sqlstr = "select noofoptions from noofoptions"
        '     myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
        '    mydataReader = myCommand.ExecuteReader
        'For i = 0 To 3
        '    tbrow = New HtmlTableRow()
        '    TABLE1.Rows.Insert(TABLE1.Rows.Count - 1, tbrow)

        '    tbdat = New HtmlTableCell()
        '    tbdat.InnerText = "Option" & i + 1
        '    tbrow.Cells.Add(tbdat)
        '    tbdat = New HtmlTableCell()
        '    optarray = New System.Web.UI.HtmlControls.HtmlInputText()
        '    optarray.ID = "txt_option" & i + 1
        '    tbdat.Controls.Add(optarray)
        '    tbrow.Cells.Add(tbdat)
        'Next i
        '   End If
        'End Sub

        'Private Sub img_addmore_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles img_addmore.Init
        'End Sub

        'Private Sub img_addmore_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles img_addmore.Init
        '    Session.Add("noOfOptions", Session.Item("noOfOptions") + 1)
        'End Sub

        '******* The Form Initialize event ********
#Region "frm_quesans_Init"

        Private Sub frm_quesans_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles frm_quesans.Init
            Dim strquery As String
            Dim myCommand As SqlCommand
            Dim objconn As New ConnectDb
            Dim strbr As StringBuilder
            'Dim strPathDb As String = ConfigurationSettings.AppSettings("PathDb")
            Dim intopcount As Integer = 0
            Try
                strbr = New StringBuilder
                strbr.Append(" Select value from M_System_Settings where key1 = 'Question_Paper' and key2 = 'Opt_Max_Length' ")
                strquery = strbr.ToString()
                If objconn.connect() Then
                    myCommand = New SqlCommand(strquery, objconn.MyConnection)
                    intopcount = myCommand.ExecuteScalar()
                End If

                Session.Add("noOfOptions", intopcount)

                If Session.Item("normal") = "true" Then
                    gen_textbox(intopcount - 1)
                End If
                '---------Added by Nisha--------
                If Session.Item("listen") = "true" Then
                    gen_textbox(intopcount - 1)
                End If


                If Session.Item("blanks") = "true" Then
                    Genanstextboxes()
                    GenerateforBlanks()
                End If

                Dim strid As String = Request.QueryString("qid")
                Dim strtesttype = sDecodeString(Request.QueryString("test"))
                If strid <> String.Empty And strtesttype <> String.Empty Then

                    chkblanks.Visible = False
                    chknormal.Visible = False
                    chkcho.Visible = False
                    Dim strqueryc = " select Qn_Category_ID from m_question where qnid = " & strid & "and test_type = " & strtesttype
                    If objconn.connect() Then
                        myCommand = New SqlCommand(strqueryc, objconn.MyConnection)
                        int = myCommand.ExecuteScalar()  '{{{
                    End If

                    If int = 3 Then
                        Genanstextboxes()
                        GenerateforBlanks()
                        TABLE1.Visible = True
                        TABLE3.Visible = True
                        TABLE4.Visible = True

                        If chkcho.Checked = True Then

                            Img.Visible = True
                            ImgSection.Visible = True
                            ' fileUpldaudio.Visible = True
                            rowAudio.Visible = True

                        Else
                            Img.Visible = False
                            ImgSection.Visible = False
                            'fileUpldaudio.Visible = False
                            rowAudio.Visible = False
                        End If
                    Else
                        gen_textbox(intopcount - 1)
                        TABLE1.Visible = True
                        TABLE2.Visible = True
                        Img.Visible = False
                        ImgSection.Visible = False
                        'fileUpldaudio.Visible = False
                        rowAudio.Visible = False

                    End If
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                strquery = Nothing
                myCommand = Nothing
                objconn = Nothing
                strbr = Nothing
                strPathDb = Nothing
                intopcount = Nothing
            End Try
        End Sub
#End Region

#Region "img_update_Click"

        Private Sub img_update_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles img_update.Click
            Dim QuesFlag As Boolean
            Dim i As Integer
            Dim RetUpdQues As Integer
            Dim RetOptsValue As OptSame
            'Dim sqlTrans As SqlTransaction
            Dim strbr As New StringBuilder
            Dim strquery As String
            Dim MyCommand As SqlCommand
            Dim intloop, RetInsert As Integer
            Try

                '/**********************Start,Jatin Gangajaliya,2011/04/11*************************/

                Dim intopcount As Integer = CInt(Session.Item("noOfOptions").ToString)
                Dim Txt(intopcount - 1) As System.Web.UI.HtmlControls.HtmlInputText

                If fileUpldaudio.HasFile Then ' Added for Upload Video check(is it selected or not) [Pragnesha 21-5-2019]

                    If fileUpldaudio.PostedFile.ContentType = "audio/wav" OrElse fileUpldaudio.PostedFile.ContentType = "audio/mp3" Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Green")
                        lblMsg.Text = Resources.Resource.QuestionAns_uperr
                        fileUpldaudio.Focus()
                    Else
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_fileerr
                        fileUpldaudio.Focus()
                        Exit Sub
                    End If
                End If

                If fileuldque.HasFile Then ' Added for Upload Video check(is it selected or not) [Pragnesha 21-5-2019]
                    Dim extension As String = System.IO.Path.GetExtension(fileuldque.FileName)


                    'Added by Rahul Shukla on 2019/05/22
                    ' Reason: It was Selecting any file but only Image File should be Select    
                    ' Desc: It was Choose only Image type Format and Show Error message when User not Select Image Format.
                    ' BugID: 1098

                    If extension = ".jpeg" Or extension = ".jpg" Or extension = ".png" Or extension = ".bmp" Or extension = ".tiff" Then
                        fileuldque.Focus()
                    Else
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_imgerr
                        fileuldque.Focus()
                        Exit Sub
                    End If
                End If


                If txt_Question.Text = String.Empty Then
                    lblMsg.Visible = True
                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.QuestionAns_qcb
                    txt_Question.Focus()
                    Exit Sub
                End If


                If int <> 3 Then
                    If optarray.Length >= 1 Then
                        With TABLE1
                            For g As Integer = 0 To intopcount - 1
                                Txt(g) = .FindControl("txt_option" & g + 1)

                                If g = 0 Then
                                    If Txt(0).Value = String.Empty And Not (fileuploader(0).HasFile) Then
                                        lblMsg.Visible = True
                                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                        lblMsg.Text = Resources.Resource.QuestionAns_opterr1
                                        optarray(0).Focus()
                                        Exit Sub
                                    End If
                                End If

                                If g = 1 Then
                                    If Txt(1).Value = String.Empty And Not (fileuploader(1).HasFile) Then
                                        lblMsg.Visible = True
                                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                        lblMsg.Text = Resources.Resource.QuestionAns_opterr2
                                        optarray(1).Focus()
                                        Exit Sub
                                    End If
                                End If
                            Next
                        End With
                    End If
                End If

                If int = 3 Then
                    If optarray(0).Value = String.Empty Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_opterr1
                        optarray(0).Focus()
                        Exit Sub
                    End If
                End If

                RetOptsValue = New OptSame
                RetOptsValue = CompareOptions()
                If RetOptsValue.Status = True Then
                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.QuestionAns_ep1 & (RetOptsValue.Opt1 + 1) & Resources.Resource.QuestionAns_ep2 & (RetOptsValue.Opt2 + 1) & Resources.Resource.QuestionAns_ep3
                    lblMsg.Visible = True
                    optarray(RetOptsValue.Opt2).Focus()
                    Exit Sub
                End If

                If int <> 3 Then
                    Dim bool As Boolean = True
                    For n As Integer = 0 To lstoptions.Items.Count - 1
                        If lstoptions.Items(n).Selected = True Then
                            bool = False
                            Exit For
                        End If
                    Next
                    If bool = True Then
                        lblMsg.Visible = True
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_atone
                        lstoptions.Focus()
                        Exit Sub
                    End If
                End If

                If txtreadonly.Text = String.Empty Then
                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                    lblMsg.Text = Resources.Resource.CourseRegistration_ValTtlMrk
                    lblMsg.Visible = True
                    txtreadonly.Focus()
                    Exit Sub
                Else
                    Dim booltotalmark As Boolean
                    booltotalmark = FieldCheck(txtreadonly.Text)
                    If Not booltotalmark Then
                        lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                        lblMsg.Text = Resources.Resource.QuestionAns_Digerr
                        lblMsg.Visible = True
                        txtreadonly.Focus()
                        Exit Sub
                    End If
                End If


                Dim blankcount As Integer = 0
                Dim boolans As Boolean
                If int = 3 Then
                    blankcount = Regex.Matches(txt_Question.Text, "---").Count
                    If blankcount <> 0 Then
                        For h As Integer = 0 To blankcount - 1
                            If optarrayans(h).Value = String.Empty Then
                                lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                lblMsg.Text = Resources.Resource.QuestionAns_Anserr & h + 1
                                lblMsg.Visible = True
                                optarrayans(h).Focus()
                                Exit Sub
                            Else
                                boolans = FieldCheck(optarrayans(h).Value)
                                If Not boolans Then
                                    lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                    lblMsg.Text = Resources.Resource.QuestionAns_Digerr
                                    lblMsg.Visible = True
                                    optarrayans(h).Focus()
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If

                    For s As Integer = 0 To optarrayans.Length - 1
                        If s < blankcount Then
                        Else
                            If optarrayans(s).Value <> String.Empty Then
                                lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                                lblMsg.Text = Resources.Resource.QuestionAns_onlyans & blankcount & Resources.Resource.QuestionAns_Ans
                                lblMsg.Visible = True
                                optarrayans(s).Focus()
                                Exit Sub
                            End If
                        End If
                    Next

                End If

                If RetOptsValue.Status = False Then
                    If RetInsert <> -1 Then
                        If UCase(Trim(g_OriginalVal)) <> UCase(Trim(txt_Question.Text)) Then
                            QuesFlag = True
                        Else
                            QuesFlag = False
                        End If

                        'If objconn.connect(strPathDb) = True Then

                        '/***********Transaction Start*****************/
                        objconnect.Open()
                        Sqltransc = objconnect.BeginTransaction(IsolationLevel.ReadCommitted)
                        'objconnect.BeginTransaction(IsolationLevel.ReadCommitted)
                        'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                        RetUpdQues = update_question((Request.QueryString("qid")), sDecodeString(Request.QueryString("test")), QuesFlag)
                        'End If

                        If -1 = RetUpdQues Then
                            lblMsg.ForeColor = System.Drawing.Color.FromName("Red")
                            lblMsg.Visible = True
                            lblMsg.Text = Resources.Resource.QuestionAns_errms
                            Exit Sub
                        End If


                        Dim sqlupdate As String = "Delete from m_options where Qnid=" & Request.QueryString("qid") & " AND Test_Type = " & sDecodeString(Request.QueryString("test"))
                        'If objconn.connect(strPathDb) Then
                        'sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                        MyCommand = New SqlCommand(sqlupdate, objconnect, Sqltransc)
                        MyCommand.ExecuteNonQuery()
                        MyCommand.Dispose()
                        'objconn.disconnect()
                        'End If

                        If int <> 3 Then
                            For i = 0 To Session.Item("noOfOptions") - 1
                                Dim k As String = optarray(i).Value
                                If (k <> Nothing) Then
                                    'If objconn.connect(strPathDb) = True Then
                                    update_options(i + 1, CInt(Request.QueryString("qid")), optarray(i).Value, sDecodeString(Request.QueryString("test")))
                                    'End If
                                End If
                            Next
                        End If


                        If int = 3 Then
                            For i = 0 To 19
                                If optarray(i).Value <> "" Then
                                    Dim k As String = optarray(i).Value
                                    If (k <> Nothing) Then
                                        'If objconn.connect(strPathDb) = True Then
                                        update_options(i + 1, CInt(Request.QueryString("qid")), optarray(i).Value, sDecodeString(Request.QueryString("test")))
                                        'End If
                                    End If
                                End If
                            Next
                        End If


                        '/*************************start,Jatin Gangajaliya,2011/04/08**************************/
                        'Desc:Delete Insert into M_Question_Answer table.(Within Transaction)

                        'For normal type questions.
                        If int <> 3 Then
                            strbr = New StringBuilder
                            strbr.Append(" Delete from M_Question_Answer where Qn_ID =  ")
                            strbr.Append(CInt(Request.QueryString("qid")))
                            strbr.Append(" and Test_Type =  ")
                            strbr.Append("'")
                            strbr.Append(sDecodeString(Request.QueryString("test")))
                            strbr.Append("'")
                            strquery = strbr.ToString()

                            'If objconn.connect(strPathDb) = True Then
                            '    sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            MyCommand = New SqlCommand(strquery, objconnect, Sqltransc)
                            MyCommand.ExecuteNonQuery()
                            For n As Integer = 0 To lstoptions.Items.Count - 1
                                If lstoptions.Items(n).Selected = True Then
                                    strbr = New StringBuilder
                                    strbr.Append(" Insert into M_Question_Answer (Qn_ID,Correct_Opt_Id,Test_Type) values ")
                                    strbr.Append(" ( ")
                                    strbr.Append(CInt(Request.QueryString("qid")))
                                    strbr.Append(" , ")
                                    strbr.Append(lstoptions.Items(n).Value)
                                    strbr.Append(" , ")
                                    strbr.Append(cmb_subject.SelectedItem.Value)
                                    strbr.Append(" ) ")
                                    strquery = strbr.ToString()
                                    MyCommand = New SqlCommand(strquery, objconnect, Sqltransc)
                                    MyCommand.ExecuteNonQuery()
                                End If
                            Next
                            'End If
                        End If

                        'For blank type questions.
                        If int = 3 Then
                            strbr = New StringBuilder
                            strbr.Append(" Delete from M_Question_Answer where Qn_ID =  ")
                            strbr.Append(CInt(Request.QueryString("qid")))
                            strbr.Append(" and Test_Type =  ")
                            strbr.Append(sDecodeString(Request.QueryString("test")))
                            strquery = strbr.ToString()

                            'If objconn.connect(strPathDb) = True Then
                            '    sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)
                            MyCommand = New SqlCommand(strquery, objconnect, Sqltransc)
                            MyCommand.ExecuteNonQuery()

                            For n As Integer = 0 To 19
                                If (optarrayans(n).ID = "txtans" & n + 1 And optarrayans(n).Value <> "") Then
                                    strbr = New StringBuilder
                                    strbr.Append(" Insert into M_Question_Answer (Qn_ID,Sub_Id,Correct_Opt_Id,Test_Type) values ")
                                    strbr.Append(" ( ")
                                    strbr.Append(CInt(Request.QueryString("qid")))
                                    strbr.Append(" , ")
                                    strbr.Append(n + 1)
                                    strbr.Append(" , ")
                                    strbr.Append(optarrayans(n).Value)
                                    strbr.Append(" , ")
                                    strbr.Append(cmb_subject.SelectedItem.Value)
                                    strbr.Append(" ) ")
                                    strquery = strbr.ToString()
                                    MyCommand = New SqlCommand(strquery, objconnect, Sqltransc)
                                    MyCommand.ExecuteNonQuery()
                                End If
                            Next

                            'End If
                        End If

                        '/************************end***********************/
                        'sqlTrans.Commit()
                        'objconnect.Close()
                        Sqltransc.Commit()
                        ''Modified By Irfan On 25/02/2015
                        ''Purpose: To hide functionality to add new question after updating the question.
                        'img_addmore.Visible = True
                        'fnc_txtmakeblank()  ''Blank the page
                        'lblMsg.ForeColor = System.Drawing.Color.FromName("Green")
                        'lblMsg.Text = "Record updated Successfully."
                        'lblMsg.Visible = True

                        ''*** back to the Questions view page
                        Session.Remove("blanks")
                        Session.Remove("normal")
                        '-----------Added by Nisha-----------
                        Session.Remove("listen")
                        '-----------Ended by Nisha-----------
                        Response.Redirect("searchQuestion.aspx", False)
                        Session.Add("fromquestionans", "true")
                        ''End Modify by Irfan

                    End If
                End If

            Catch ex As Exception
                Sqltransc.Rollback()
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                i = Nothing
                QuesFlag = Nothing
                RetUpdQues = Nothing
                RetOptsValue = Nothing
                'sqlTrans = Nothing
                MyCommand = Nothing
                strquery = Nothing
                strbr = Nothing
            End Try
        End Sub
#End Region

#Region "img_addmore_Click"

        Private Sub img_addmore_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img_addmore.Click
            Try
                Response.Redirect("question_ans.aspx?qid=", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "btnBack_Click"

        Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles btnBack.Click
            Session.Remove("blanks")
            Session.Remove("normal")
            '--------Added by Nisha------------
            Session.Remove("listen")
            Response.Redirect("searchQuestion.aspx", False)
            Session.Add("fromquestionans", "true")
        End Sub
#End Region

#Region "SetFocus"

        Private Sub SetFocus(ByVal ctrl As System.Web.UI.Control)
            Dim s As String = "<SCRIPT language='javascript'>document.getElementById('" & ctrl.ID & "').focus() </SCRIPT>"
            RegisterStartupScript("focus", s)
        End Sub
#End Region

#Region "btnDel_Click"

        Private Sub btnDel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDel.Click
            Dim MyCommand As SqlCommand
            Dim objconn As New ConnectDb
            Dim strPathDb As String
            Dim strSql As String
            Dim sqlTrans As SqlTransaction
            Try

                strPathDb = ConfigurationSettings.AppSettings("PathDb")
                If objconn.connect() = True Then
                    sqlTrans = objconn.MyConnection.BeginTransaction(IsolationLevel.ReadCommitted)

                    strSql = "Delete  from m_options where m_options.qnid = " & Request.QueryString("qid") &
                        " and m_options.test_type = '" & sDecodeString(Request.QueryString("test")) & "'"
                    MyCommand = New SqlCommand(strSql, objconn.MyConnection, sqlTrans)
                    MyCommand.ExecuteNonQuery()
                    MyCommand.Dispose()


                    strSql = "Delete  from m_question where m_question.qnid = " & Request.QueryString("qid") &
                        " and m_question.test_type = '" & sDecodeString(Request.QueryString("test")) & "'"
                    MyCommand = New SqlCommand(strSql, objconn.MyConnection, sqlTrans)
                    MyCommand.ExecuteNonQuery()
                    MyCommand.Dispose()

                    strSql = "Delete from M_Question_Answer where Qn_ID = " & Request.QueryString("qid") &
                    " and M_Question_Answer.Test_Type = '" & sDecodeString(Request.QueryString("test")) & "'"
                    MyCommand = New SqlCommand(strSql, objconn.MyConnection, sqlTrans)
                    MyCommand.ExecuteNonQuery()
                    MyCommand.Dispose()

                    sqlTrans.Commit()
                End If


                'If objconn.connect(strPathDb) Then
                '    strSql = "Delete  from m_options where m_options.qnid = " & Request.QueryString("qid") & _
                '        " and m_options.test_type = '" & sDecodeString(Request.QueryString("test")) & "'"
                '    MyCommand = New SqlCommand(strSql, objconn.MyConnection)
                '    MyCommand.ExecuteNonQuery()
                '    MyCommand.Dispose()
                'End If
                objconn.disconnect()

                'If objconn.connect(strPathDb) Then
                '    strSql = "Delete  from m_question where m_question.qnid = " & Request.QueryString("qid") & _
                '        " and m_question.test_type = '" & sDecodeString(Request.QueryString("test")) & "'"
                '    MyCommand = New SqlCommand(strSql, objconn.MyConnection)
                '    MyCommand.ExecuteNonQuery()
                '    MyCommand.Dispose()
                'End If
                Response.Redirect("searchQuestion.aspx", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                sqlTrans.Rollback()
                Response.Redirect("error.aspx", False)
            Finally
                objconn = Nothing
                strPathDb = Nothing
                strSql = Nothing
                MyCommand = Nothing
                sqlTrans = Nothing
            End Try
        End Sub
#End Region

        '**************************************************************************
        'Function               :   sDecodeString
        '
        'Return                 :   decoded string
        '
        'Argument               :   None
        '
        'Explanation            :   This will convert the encoded string into 
        '                           normal string
        '                           
        'Note                   :   None
        '**************************************************************************
#Region "sDecodeString"
        Function sDecodeString(ByVal sText As String) As String

            Dim iCnt
            Dim sChar
            Dim sDecode
            Try
                sDecode = ""

                sChar = Split(sText, ENCRYPT_DELIMIT)
                For iCnt = 1 To UBound(sChar)
                    sDecode = sDecode & Chr(CLng("&H" & sChar(iCnt)) - ENCRYPT_KEY)
                Next
                sDecodeString = sDecode
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                iCnt = Nothing
                sChar = Nothing
                sDecode = Nothing
            End Try
        End Function
#End Region

#Region "imgLogoff_Click"

#End Region

#Region "Page_Error"
        Private Sub Page_Error(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Error
            Dim Err As New CreateLog
            Try
                Err.ErrorLog(Server.MapPath("Logs/RMS"), Server.GetLastError().ToString().Trim, "question_ans.aspx.vb")
                Response.Redirect("error.aspx?err=Error On Page", False)
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                Err = Nothing

            End Try
        End Sub
#End Region

#Region "Clear button Click Event"
        'Desc: This is clear button click event.
        'By: Jatin Gangajaliay,2011/3/15

        Protected Sub imgbtnclr_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles imgbtnclr.Click
            Try
                lblMsg.Text = String.Empty
                txt_Question.Text = String.Empty
                lbl.Text = Resources.Resource.QuestionAns_QRD
                img_addmore.Visible = False
                img_update.Visible = False
                cmb_subject.Visible = True
                TxtSubject.Visible = False

                fnc_txtmakeblank()
                cmb_QueLevel.SelectedIndex = 0
                cmb_rgtans.SelectedIndex = 0
                If cmb_subject.Items.Count >= 0 Then
                    cmb_subject.SelectedIndex = 0
                End If
                'fnc_txtmakeblank()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
#End Region

#Region "Reset button click event"
        'Desc: This is reset button click event.
        'By: Jatin Gangajaliya, 2011/3/15

        Protected Sub imgbtnreset_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgbtnreset.Click
            Try
                g_OriginalVal = txt_Question.Text
                If sDecodeString(Request.QueryString("test")) = "" Then
                    populatesubject()
                    cmb_subject.SelectedIndex = Session("testtype") - 1
                Else
                    populatesubject(sDecodeString(Request.QueryString("test")))
                End If
                If Request.QueryString("qid").ToString <> "" Then
                    lbl.Text = Resources.Resource.QuestionAns_QMD
                    img_saveexit.Visible = False
                    img_addmore.Visible = False
                    cmb_subject.Visible = False
                    TxtSubject.Visible = True
                    img_update.Visible = True
                    imgbtnprev.Visible = True
                    btnDel.Visible = True
                    imgbtnreset.Visible = True
                    populatequesans(CInt(Request.QueryString("qid")), sDecodeString(Request.QueryString("test")))
                    playS(CInt(Request.QueryString("qid")), sDecodeString(Request.QueryString("test")))
                End If

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "Question Level Dropdown selected index changed event"
        'By: Jatin Gangajaliya, 2011/04/08
        'Desc: This is Question Level Dropdown selected index changed event.

        'Protected Sub cmb_QueLevel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmb_QueLevel.SelectedIndexChanged
        '    Try
        '        If cmb_QueLevel.SelectedItem.Text = "Low" Then
        '            txtreadonly.Text = "1"
        '        ElseIf cmb_QueLevel.SelectedItem.Text = "Medium" Then
        '            txtreadonly.Text = "2"
        '        ElseIf cmb_QueLevel.SelectedItem.Text = "High" Then
        '            txtreadonly.Text = "3"
        '        End If

        '        If chkblanks.Checked = True Then
        '            GenerateforBlanks()
        '        ElseIf chknormal.Checked = True Then
        '            gen_textbox(Session.Item("noOfOptions") - 1)
        '        End If
        '        cmb_QueLevel.AutoPostBack = True
        '    Catch ex As Exception
        '        If log.IsDebugEnabled Then
        '            log.Debug("Error :" & ex.ToString())
        '        End If
        '        Response.Redirect("error.aspx", False)
        '    End Try
        'End Sub
#End Region

#Region "CheckforSameQuestion function"
        'Desc: This function checks for if same question already exists or not.
        'By: Jatin Gangajaliya, 2011/04/11.

        Public Function CheckforSameQuestion() As Integer
            Dim mydataReader As SqlDataReader
            Dim myCommand As SqlCommand
            Dim OConn As New ConnectDb
            Dim sqlstr, strPathDb, strquestion As String
            Try

                'strPathDb = ConfigurationSettings.AppSettings("PathDb")
                OConn.connect()

                strquestion = Replace(Server.HtmlEncode(txt_Question.Text), "'", ",")
                sqlstr = "select qnid,del_flag from m_question where question=N'" ' like N'%"
                sqlstr = sqlstr & txt_Question.Text
                sqlstr = sqlstr & "' and test_type='" & cmb_subject.SelectedItem.Value & "' "
                ' sqlstr = sqlstr & Replace(Server.HtmlEncode(txt_Question.Text), "'", "''")
                ' sqlstr = sqlstr & "%' and test_type='" & cmb_subject.SelectedItem.Value & "' "

                myCommand = New SqlCommand(sqlstr, OConn.MyConnection)
                mydataReader = myCommand.ExecuteReader()

                If True = mydataReader.HasRows Then
                    While mydataReader.Read
                        If mydataReader.Item("del_flag") = False Then
                            Return -1
                        End If
                    End While
                    'If mydataReader.Item("del_flag") =  Then

                    'End If

                End If
                OConn.disconnect()
                mydataReader.Close()
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                mydataReader = Nothing
                myCommand = Nothing
                OConn = Nothing
                sqlstr = Nothing
                strPathDb = Nothing
                strquestion = Nothing
            End Try
        End Function
#End Region

#Region "Question Image Clear Event"
        'Desc: This is question image clear event.
        'By: Jatin Gangajaliya, 2011/04/13.

        Protected Sub lnkquestion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkquestion.Click
            Try
                quesimage.Text = String.Empty
                lnkquestion.Visible = False
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
        End Sub
#End Region

#Region "Option Image Clear Event"
        'Desc: This is option image clear event.
        'By: Jatin Gangajaliya, 2011/04/13.

        Protected Sub lnkbtnary_click(ByVal sender As Object, ByVal e As EventArgs)
            Dim lnkCast As LinkButton = DirectCast(sender, LinkButton)
            Dim strcmdname As String = lnkCast.CommandName
            Dim strid As Integer = strcmdname.Substring(6)
            Try

                For i As Integer = 0 To imagearray.Length - 1
                    If imagearray(i).ID = "imageoption" & strid Then
                        imagearray(i).Text = String.Empty
                    End If
                    If lnkbtnary(i).ID = "lnkbtn" & strid Then
                        lnkbtnary(i).Visible = False
                    End If
                Next

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            Finally
                lnkCast = Nothing
                strcmdname = Nothing
                strid = Nothing
            End Try
        End Sub
#End Region

        '#Region "Function Returnstring"
        '        'By: Jatin Gangajaliya, 2011/04/13.

        '        Private Function ReturnString() As String
        '            Try

        '                'Note: Generate textboxes with id.
        '                'Dim strqtext As String = txt_Question.Text
        '                'Dim strfinal As String
        '                'If strqtext.Contains("---") Then
        '                '    Dim strary() As String = strqtext.Split("---")
        '                '    For o As Integer = 0 To strary.Length - 1

        '                '        If strary(o) <> "" Then
        '                '            If o < strary.Length - 1 Then
        '                '                strfinal = strfinal & strary(o) & "<input type=text name=lastname id='" & o + 1 & "'/>"
        '                '            Else
        '                '                strfinal = strfinal & strary(o)
        '                '            End If
        '                '        Else
        '                '            strfinal = strfinal & ""
        '                '        End If

        '                '    Next
        '                'Else
        '                '    strfinal = txt_Question.Text
        '                'End If

        '                'Note: Generate textboxes without id.
        '                Dim strqtext As String = txt_Question.Text
        '                Dim strfinal As String
        '                If strqtext.Contains("---") Then
        '                    strfinal = strqtext.Replace("---", "<input type=text name=txt />")
        '                Else
        '                    strfinal = txt_Question.Text
        '                End If

        '                Return strfinal

        '            Catch ex As Exception
        '                If log.IsDebugEnabled Then
        '                    log.Debug("Error :" & ex.ToString())
        '                End If
        '                Throw ex
        '            End Try
        '        End Function
        '#End Region

#Region "Normal Checkchange event"
        'Desc:This is normal checkchange event.
        'By: Jatin Gangajaliya, 2011/04/15.

        Protected Sub chknormal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chknormal.CheckedChanged
            Dim intopcount As Integer = 0
            Session.Remove("blanks")
            Session.Add("normal", "true")
            lblMsg.Text = String.Empty
            txt_Question.Text = String.Empty
            Try
                If chknormal.Checked = True Then
                    TABLE2.Controls.Clear()
                    TABLE3.Controls.Clear()
                    TABLE4.Controls.Clear()
                    chkblanks.Checked = False
                    chkcho.Checked = False
                    If Session.Item("noOfOptions") <> Nothing Then
                        intopcount = CInt(Session.Item("noOfOptions").ToString())
                        gen_textbox(intopcount - 1) 'Commented by rajesh 
                        TABLE2.Visible = True
                        fileuldque.Visible = True
                    End If
                    rowquestion.Visible = True
                    rowsubject.Visible = True

                    btnBack.Visible = True
                    img_saveexit.Visible = True
                    imgbtnprev.Visible = True
                    imgbtnclr.Visible = True
                    'Added by Nisha
                    Img.Visible = True
                    ImgSection.Visible = True
                    ' fileUpldaudio.Visible = False
                    rowAudio.Visible = False

                Else
                    btnBack.Visible = True
                    img_saveexit.Visible = False
                    imgbtnclr.Visible = False
                    imgbtnprev.Visible = False
                    TABLE2.Controls.Clear()
                    rowquestion.Visible = False
                    rowsubject.Visible = False
                    'Added by Nisha
                    Img.Visible = False
                    ImgSection.Visible = False
                    'fileUpldaudio.Visible = False
                    rowAudio.Visible = False

                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
#End Region

#Region "Fill in the blanks checkchange event"
        'Desc:This is normal checkchange event.
        'By: Jatin Gangajaliya, 2011/04/15.

        Protected Sub chkblanks_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkblanks.CheckedChanged
            Dim intopcount As Integer = 0
            Session.Remove("normal")
            Session.Add("blanks", "true")
            lblMsg.Text = String.Empty
            txt_Question.Text = String.Empty
            Try
                If chkblanks.Checked = True Then
                    TABLE2.Controls.Clear()
                    TABLE3.Controls.Clear()
                    TABLE4.Controls.Clear()
                    chknormal.Checked = False
                    chkcho.Checked = False
                    fileuldque.Visible = False
                    lnkquestion.Visible = False
                    GenerateforBlanks()
                    Genanstextboxes()
                    TABLE3.Visible = True
                    TABLE4.Visible = True
                    rowquestion.Visible = True
                    rowsubject.Visible = True

                    btnBack.Visible = True
                    img_saveexit.Visible = True
                    imgbtnclr.Visible = True
                    imgbtnprev.Visible = True
                    'Added by Nisha
                    Img.Visible = False
                    ImgSection.Visible = False
                    'fileUpldaudio.Visible = False
                    rowAudio.Visible = False
                Else

                    btnBack.Visible = True
                    img_saveexit.Visible = False
                    imgbtnprev.Visible = False
                    imgbtnclr.Visible = False
                    TABLE3.Controls.Clear()
                    TABLE4.Controls.Clear()
                    rowquestion.Visible = False
                    rowsubject.Visible = False
                    'Added by Nisha
                    Img.Visible = False
                    ImgSection.Visible = False
                    ' fileUpldaudio.Visible = False
                    rowAudio.Visible = False
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try

        End Sub
#End Region

#Region "Listening check event" 'Nisha'

        Protected Sub chkcho_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkcho.CheckedChanged
            Dim intopcount As Integer = 0
            Session.Remove("normal")
            Session.Remove("blanks")
            Session.Add("listen", "true")
            lblMsg.Text = String.Empty
            txt_Question.Text = String.Empty
            Try
                If chkcho.Checked = True Then
                    TABLE2.Controls.Clear()
                    TABLE3.Controls.Clear()
                    TABLE4.Controls.Clear()
                    chkblanks.Checked = False
                    chknormal.Checked = False
                    If Session.Item("noOfOptions") <> Nothing Then
                        intopcount = CInt(Session.Item("noOfOptions").ToString())
                        gen_textbox(intopcount - 1)
                        TABLE2.Visible = True
                        fileuldque.Visible = True
                    End If
                    rowquestion.Visible = True
                    rowsubject.Visible = True
                    btnBack.Visible = True
                    img_saveexit.Visible = True
                    imgbtnprev.Visible = True
                    imgbtnclr.Visible = True
                    'Added by Nisha
                    Img.Visible = True
                    ImgSection.Visible = True
                    'fileUpldaudio.Visible = True
                    rowAudio.Visible = True
                Else
                    btnBack.Visible = True
                    img_saveexit.Visible = False
                    imgbtnclr.Visible = False
                    imgbtnprev.Visible = False
                    TABLE2.Controls.Clear()
                    rowquestion.Visible = False
                    rowsubject.Visible = False
                    'Added by Nisha
                    Img.Visible = False
                    ImgSection.Visible = False
                    'fileUpldaudio.Visible = False
                    rowAudio.Visible = False
                End If
            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Response.Redirect("error.aspx", False)
            End Try
            '  btnBack.Visible = True

        End Sub


#End Region

#Region "Method for generating textboxes for fill in the blanks"
        'Desc:This is method for generating textboxes for fill in the blanks.
        'By: Jatin Gangajaloya, 2011/04/15.

        Private Sub GenerateforBlanks()
            Dim i As Integer

            Try
                cmb_rgtans = New System.Web.UI.HtmlControls.HtmlSelect
                cmb_QueLevel = New DropDownList
                cmb_QueLevel.AutoPostBack = False
                'lstoptions = New ListBox
                'lstoptions.SelectionMode = ListSelectionMode.Multiple

                ReDim optarray(19)
                tbrow = New HtmlTableRow
                TABLE3.Rows.Insert(TABLE3.Rows.Count - 1, tbrow)
                Dim intindex As Integer = 0
                For i = 0 To 4
                    tbrow = New HtmlTableRow
                    TABLE3.Rows.Insert(TABLE3.Rows.Count - 1, tbrow)

                    tbdat = New HtmlTableCell

                    Dim l As New Literal()
                    If i = 0 Then
                        l.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    Else
                        l.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    End If
                    l.EnableViewState = True
                    tbdat.Controls.Add(l)
                    tbrow.Cells.Add(tbdat)
                    tbdat = New HtmlTableCell
                    optarray(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarray(intindex).ID = "txt_option" & intindex + 1
                    optarray(intindex).Size = 35

                    optarray(intindex).EnableViewState = True
                    optarray(intindex).MaxLength = 50

                    tbdat.Controls.Add(optarray(intindex))
                    tbrow.Cells.Add(tbdat)

                    intindex = intindex + 1

                    '/**************start**************/
                    tbdat = New HtmlTableCell

                    Dim j As New Literal()
                    If i = 0 Or i = 1 Then
                        j.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    Else
                        j.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    End If
                    j.EnableViewState = True
                    tbdat.Controls.Add(j)

                    'tbdat.Attributes.Add("class", "tdcontent_labelforqr")
                    tbrow.Cells.Add(tbdat)
                    tbdat = New HtmlTableCell
                    optarray(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarray(intindex).ID = "txt_option" & intindex + 1
                    optarray(intindex).Size = 35
                    optarray(intindex).MaxLength = 50
                    optarray(intindex).EnableViewState = True
                    'tbdat.Attributes.Add("class", "tdcontent_dataforqr")

                    tbdat.Controls.Add(optarray(intindex))
                    tbrow.Cells.Add(tbdat)
                    '/***************End***************/

                    intindex = intindex + 1

                    tbdat = New HtmlTableCell

                    Dim k As New Literal()
                    If i = 0 Then
                        k.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    Else
                        k.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    End If
                    k.EnableViewState = True
                    tbdat.Controls.Add(k)

                    tbrow.Cells.Add(tbdat)
                    tbdat = New HtmlTableCell
                    optarray(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarray(intindex).ID = "txt_option" & intindex + 1
                    optarray(intindex).Size = 35

                    optarray(intindex).EnableViewState = True
                    optarray(intindex).MaxLength = 50
                    tbdat.Controls.Add(optarray(intindex))
                    tbrow.Cells.Add(tbdat)

                    intindex = intindex + 1


                    tbdat = New HtmlTableCell

                    Dim m As New Literal()
                    If i = 0 Then
                        m.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    Else
                        m.Text = Resources.Resource.QuestionAns_optn & intindex + 1
                    End If
                    m.EnableViewState = True
                    tbdat.Controls.Add(m)

                    'tbdat.Attributes.Add("class", "tdcontent_labelforql")
                    'Rahul
                    'tbdat.Attributes.Add("width", "15%")
                    tbrow.Cells.Add(tbdat)
                    tbdat = New HtmlTableCell
                    optarray(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarray(intindex).ID = "txt_option" & intindex + 1
                    optarray(intindex).Size = 35

                    optarray(intindex).EnableViewState = True
                    optarray(intindex).MaxLength = 50
                    tbdat.Controls.Add(optarray(intindex))
                    tbrow.Cells.Add(tbdat)

                    intindex = intindex + 1

                Next i

                tbrow = New HtmlTableRow

                TABLE3.Rows.Insert(TABLE3.Rows.Count - 1, tbrow)

                'Dim l1 As New ListItem
                'l1.Text = "Basic"
                'l1.Value = "0"
                'cmb_QueLevel.Items.Add(l1)
                'Dim l2 As New ListItem
                'l2.Text = "Intermediate"
                'l2.Value = "1"
                'cmb_QueLevel.Items.Add(l2)

                'Dim l3 As New ListItem
                'l3.Text = "High"
                'l3.Value = "2"
                'cmb_QueLevel.Items.Add(l3)

                'tbrow = New HtmlTableRow
                'cmb_QueLevel.EnableViewState = True
                'TABLE3.Rows.Insert(TABLE3.Rows.Count - 1, tbrow)
                'tbdat = New HtmlTableCell
                'tbdat.InnerText = Resources.Resource.QuestionAns_Quesnlvl
                ''tbdat.Attributes.Add("class", "tdcontent_labelforql")
                'tbrow.Cells.Add(tbdat)
                'tbdat = New HtmlTableCell
                'tbdat.Controls.Add(cmb_QueLevel)
                ''tbdat.Attributes.Add("class", "tdcontent_dataforql")
                'tbrow.Cells.Add(tbdat)


                ''tbrow = New HtmlTableRow
                ''TABLE3.Rows.Insert(TABLE3.Rows.Count - 1, tbrow)
                'tbdat = New HtmlTableCell
                ''tbdat.Attributes.Add("class", "tdcontent_labelforqr")
                'Dim n As New Literal()
                'n.Text = Resources.Resource.WeightMgt_ttlMrk & "<span class='mand'> *</span>"
                'n.EnableViewState = True
                ''tbdat.InnerText = "Total Marks" & "<span class='mand'> *</span>"
                'tbdat.Controls.Add(n)
                'txtreadonly = New TextBox
                ''AddHandler txtreadonly.focus, AddressOf lnkbtnary_click
                'txtreadonly.MaxLength = 2
                'txtreadonly.ID = "TxtReadOnly"
                'txtreadonly.EnableViewState = True
                'tbrow.Cells.Add(tbdat)
                'tbdat = New HtmlTableCell
                'tbdat.Controls.Add(txtreadonly)
                ''tbdat.Attributes.Add("class", "tdcontent_dataforqr")
                'tbrow.Cells.Add(tbdat)

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            Finally
                i = Nothing
            End Try
        End Sub
#End Region

#Region "Generate answer table"
        Private Sub Genanstextboxes()
            Try
                Dim intindex As Integer = 0

                ReDim optarrayans(19)

                tbrow = New HtmlTableRow
                TABLE4.Rows.Insert(TABLE4.Rows.Count - 1, tbrow)

                tbrow = New HtmlTableRow
                TABLE4.Rows.Insert(TABLE4.Rows.Count - 1, tbrow)

                tbdat = New HtmlTableCell
                tbdat.ColSpan = "1"
                tbdat.Attributes.Add("class", "tblhead")
                Dim lbl As Label = New Label
                lbl.Text = Resources.Resource.QuestionAns_Ans
                tbdat.Controls.Add(lbl)
                tbrow.Controls.Add(tbdat)

                'TABLE4.Rows.Insert(TABLE4.Rows.Count - 1, tbrow)

                '/**********************************************/
                tbrow = New HtmlTableRow
                TABLE4.Rows.Insert(TABLE4.Rows.Count - 1, tbrow)

                For i As Integer = 0 To 4
                    tbrow = New HtmlTableRow
                    TABLE4.Rows.Insert(TABLE4.Rows.Count - 1, tbrow)

                    tbdat = New HtmlTableCell
                    'tbdat.Width = "10%"
                    Dim l As New Literal()
                    l.Text = intindex + 1
                    tbdat.Controls.Add(l)
                    'tbdat.Attributes.Add("class", "tdcontent_labelforql")
                    tbrow.Cells.Add(tbdat)

                    tbdat = New HtmlTableCell
                    tbdat.Width = "0%"
                    optarrayans(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarrayans(intindex).ID = "txtans" & intindex + 1
                    optarrayans(intindex).Size = 10
                    'optarrayans(intindex).MaxLength = 2
                    'tbdat.Attributes.Add("class", "tdcontent_dataforql")
                    tbdat.Controls.Add(optarrayans(intindex))
                    tbrow.Cells.Add(tbdat)

                    intindex = intindex + 1

                    '/**************start**************/
                    tbdat = New HtmlTableCell
                    'tbdat.Width = "10%"
                    Dim j As New Literal()
                    j.Text = intindex + 1
                    tbdat.Controls.Add(j)
                    'tbdat.Attributes.Add("class", "tdcontent_labelforqr")
                    tbrow.Cells.Add(tbdat)

                    tbdat = New HtmlTableCell
                    tbdat.Width = "0%"
                    optarrayans(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarrayans(intindex).ID = "txtans" & intindex + 1
                    optarrayans(intindex).Size = 10
                    'optarrayans(intindex).MaxLength = 2
                    'tbdat.Attributes.Add("class", "tdcontent_dataforqr")
                    tbdat.Controls.Add(optarrayans(intindex))
                    tbrow.Cells.Add(tbdat)
                    '/***************End***************/

                    intindex = intindex + 1

                    tbdat = New HtmlTableCell
                    'tbdat.Width = "10%"
                    Dim k As New Literal()
                    k.Text = intindex + 1
                    tbdat.Controls.Add(k)
                    'tbdat.Attributes.Add("class", "tdcontent_labelforql")
                    tbrow.Cells.Add(tbdat)

                    tbdat = New HtmlTableCell
                    tbdat.Width = "0%"
                    optarrayans(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarrayans(intindex).ID = "txtans" & intindex + 1
                    optarrayans(intindex).Size = 10
                    'optarrayans(intindex).MaxLength = 2
                    'tbdat.Attributes.Add("class", "tdcontent_dataforql")
                    tbdat.Controls.Add(optarrayans(intindex))
                    tbrow.Cells.Add(tbdat)

                    intindex = intindex + 1

                    tbdat = New HtmlTableCell
                    'tbdat.Width = "10%"
                    Dim m As New Literal()
                    m.Text = intindex + 1
                    tbdat.Controls.Add(m)
                    'tbdat.Attributes.Add("class", "tdcontent_labelforql")
                    tbrow.Cells.Add(tbdat)

                    tbdat = New HtmlTableCell
                    tbdat.Width = "0%"
                    optarrayans(intindex) = New System.Web.UI.HtmlControls.HtmlInputText
                    optarrayans(intindex).ID = "txtans" & intindex + 1
                    optarrayans(intindex).Size = 10
                    'optarrayans(intindex).MaxLength = 2
                    'tbdat.Attributes.Add("class", "tdcontent_dataforql")
                    tbdat.Controls.Add(optarrayans(intindex))
                    tbrow.Cells.Add(tbdat)

                    intindex = intindex + 1

                Next i

            Catch ex As Exception
                If log.IsDebugEnabled Then
                    log.Debug("Error :" & ex.ToString())
                End If
                Throw ex
            End Try
        End Sub
#End Region

        ' --------------------------------------------
        'Code added by Pragnesha
        'Method Name: playS
        'Purpose: play audio which is saved as per the quid using Audio control 
        '---------------------------------------------

        Private Sub playS(ByVal qid As Integer, ByVal testType As String)
            Dim myCommand As SqlCommand
            Dim myDataReader As SqlDataReader
            Dim objconn As New ConnectDb
            Dim sqlstr As String
            Dim strauarray1() As String
            Dim strA As String

            If objconn.connect() Then
                sqlstr = ""
                sqlstr = sqlstr & "SELECT Audios FROM "
                sqlstr = sqlstr & "m_question WHERE "
                sqlstr = sqlstr & "qnid = " & qid & " AND "
                sqlstr = sqlstr & "test_type = '" & testType & "' order by qnid"
                myCommand = New SqlCommand(sqlstr, objconn.MyConnection)
                myDataReader = myCommand.ExecuteReader

            End If
            While myDataReader.Read
                strA = myDataReader.Item("Audios")
                strauarray1 = CheckAudio(strA) 'ZX
            End While
            Dim mytone As New System.Media.SoundPlayer
            '      Dim rootPath As String = Server.MapPath("~")
            Dim filepath As String = "QuestionAudio\" & strA
            'HiddenField2.Value = Request.Url.Scheme.ToString + "://" + Request.Url.Host.ToString() + ":" + Request.Url.Port.ToString() + "/" + filepath
            'Editted by Rahul Shukla on 16/05/2019
            HiddenField1.Value = Request.Url.Scheme.ToString + "://" + Request.Url.Host.ToString() + ":" + Request.Url.Port.ToString() + "/" + filepath


        End Sub

        '/------------------------------Ended by Pragnesha----------------------------------/


    End Class


End Namespace
