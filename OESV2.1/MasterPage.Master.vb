Partial Public Class MasterPage
    'Inherits BasePage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Added By Sumit Tawade [16/10/2019]
        lbluser.Text = " " + Session("loginname")
        If Not Me.IsPostBack Then
            Dim language As String = "en-us"
            If HttpContext.Current.Request.Cookies("Language") IsNot Nothing Then
                Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies("Language")
                If cookie IsNot Nothing Then
                    language = cookie.Value.Split("="c)(1)
                End If
            End If
            Me.ddlLanguages.Items.FindByValue(language.ToLower()).Selected = True
            'UserName = language

            'Added by Rahul Shukla on 10/10/2019
            'Reason : when the Page is not Postback and Logoff button clicked or Session Timeout it will 
            'Destroy session and Prevent user from going back to page after logging out


            'If Not IsPostBack Then

            '    If Session("loginname") Is Nothing Then
            '        Response.Redirect("login.aspx")
            '    End If
            'End If
            If Session("loginname") Is Nothing Then
                Response.Redirect("login.aspx")
            End If
            Response.ClearHeaders()
            Response.AddHeader("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate")
            Response.AddHeader("Pragma", "no-cache")

            'Ended By Sumit Tawade

        End If



    End Sub

    'Added By Sumit Tawade [16/10/2019]
    Protected Sub ChangeLanguage(sender As Object, e As EventArgs)
        Dim languageCookie As New HttpCookie("Language")
        languageCookie.Values("LanguageCode") = Me.ddlLanguages.SelectedValue
        languageCookie.Expires = DateTime.Now.AddDays(30)
        HttpContext.Current.Response.Cookies.Add(languageCookie)
        Me.Response.Redirect(Request.Url.AbsoluteUri)
    End Sub
    'Ended By Sumit Tawade
End Class