Imports System.Threading
Imports System.Globalization

Public Class BasePage
    Inherits System.Web.UI.Page
    Protected Overrides Sub InitializeCulture()
        Dim language As String = "en-us"

        If HttpContext.Current.Request.Cookies("Language") IsNot Nothing Then
            Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies("Language")
            If cookie IsNot Nothing Then
                language = cookie.Value.Split("="c)(1)

            End If
        End If

        'Check if PostBack is caused by Language DropDownList.
        If Request.Form("__EVENTTARGET") IsNot Nothing AndAlso Request.Form("__EVENTTARGET").Contains("ddlLanguages") Then
            'Set the Language.
            'language = Request.Form(Request.Form("__EVENTTARGET"))
            'language = "en-Us"
            'If MasterPage.UserName.ToString() = "en-us" Then
            '    language = "en-Us"
            'Else
            '    If MasterPage.UserName.ToString() = "ja-JP" Then
            '        language = "ja-JP"
            '    End If
            'End If


        End If

            'Set the Culture.
            Thread.CurrentThread.CurrentCulture = New CultureInfo(language)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(language)

        Dim lang As HttpCookie = New HttpCookie("Lang")
        lang.Value = language.ToString
        Response.Cookies.Add(lang)


    End Sub
End Class


'Public Class BasePage
'    Inherits System.Web.UI.Page
'    Protected Overrides Sub InitializeCulture()
'        Dim language As String = "en-us"

'        'Detect User's Language.
'        If Request.UserLanguages IsNot Nothing Then
'            'Set the Language.
'            'language = Request.UserLanguages(0)
'            ' language = Request.UserLanguages(0)
'            If MasterPage.UserName Is Nothing Then
'                language = Request.UserLanguages(0)
'            Else

'                language = MasterPage.UserName.ToString
'            End If
'        End If

'        'Check if PostBack is caused by Language DropDownList.
'        If Request.Form("__EVENTTARGET") IsNot Nothing AndAlso Request.Form("__EVENTTARGET").Contains("ddlLanguages") Then
'            'Set the Language.
'            ' language = Request.Form(Request.Form("__EVENTTARGET"))
'            If MasterPage.UserName.ToString() = "en-us" Then
'                language = "en-Us"
'            Else
'                If MasterPage.UserName.ToString() = "ja-JP" Then
'                    language = "ja-JP"
'                End If
'            End If
'        End If

'        'Set the Culture.
'        Thread.CurrentThread.CurrentCulture = New CultureInfo(language)
'        Thread.CurrentThread.CurrentUICulture = New CultureInfo(language)
'    End Sub
'End Class
