Imports System.Threading

Public Class LoginFrm
    Public trydomain As String = ""
    Public tryname As String = ""
    Public trypass As String = ""
    Public login_succeded As Boolean = False
    Public set_overide As Boolean = False

    Private Declare Auto Function LogonUser Lib "advapi32.dll" (ByVal lpszUsername As String, _
        ByVal lpszDomain As String, ByVal lpszPassword As String, ByVal dwLogonType As Integer, _
        ByVal dwLogonProvider As Integer, ByRef phToken As IntPtr) As Integer
    Private Declare Auto Function CloseHandle Lib "kernel32.dll" (ByVal handle As IntPtr) As Boolean

    Private Const LOGON32_LOGON_INTERACTIVE = 2
    Private Const LOGON32_PROVIDER_DEFAULT = 0

    Private Delegate Sub SetTextCallback(ByVal [text] As String)

    Public Function returnlogindetails() As Object
        Return New Object() {trydomain, tryname, trypass, login_succeded}
    End Function

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If UsernameTextBox.Text.Contains("\") Then
            trydomain = UsernameTextBox.Text.Substring(0, UsernameTextBox.Text.IndexOf("\"))
            tryname = UsernameTextBox.Text.Substring(UsernameTextBox.Text.IndexOf("\") + 1)
        Else
            tryname = Me.UsernameTextBox.Text
        End If
        trypass = Me.PasswordTextBox.Text
        Dim threadvr As New Thread(New ThreadStart(AddressOf hidethread))
        threadvr.IsBackground = True
        contrvis(False)
        Try
            If String.IsNullOrEmpty(trydomain) Then trydomain = Environment.UserDomainName
            Dim hToken As IntPtr
            If Not set_overide Then
                If LogonUser(tryname, trydomain, trypass, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, hToken) Then
                    If Not hToken.Equals(IntPtr.Zero) Then
                        login_succeded = True
                    Else
                        Throw New Exception("Login Failed!")
                    End If
                Else
                    Throw New Exception("Login Failed!")
                End If
            End If
            login_succeded = True
            Me.Close()
        Catch ex As Exception
            LabelPassRetry.Text = "Login Attempt Failed"
            threadvr.Start()
            login_succeded = False
        End Try
        contrvis(True)
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        contrvis(False)
        Me.Close()
    End Sub

    Private Sub hidethread()
        Try
            Thread.Sleep(2500)
            Try
                settext("")
            Catch ex As Exception
            End Try
        Catch ex As Exception
            If ex.GetType.ToString.ToLower <> "system.threading.threadabortexception" Then
                Try
                    Me.Invoke(Sub() MsgBox("An error has occured and the thread will terminate: '" & ex.GetType.ToString & "'" & vbCrLf & "Source: " & ex.Source & "." & vbCrLf & "Description: " & vbCrLf & ex.Message & vbCrLf & "Stack Trace: " & vbCrLf & ex.StackTrace, MsgBoxStyle.Critical, "Flybird Launcher - Thread Error"))
                Catch ex2 As Exception
                End Try
            End If
        End Try
    End Sub

    Private Sub LoginFrm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        login_succeded = False
        tryname = ""
        trypass = ""
        contrvis(True)
    End Sub

    Private Sub LoginFrm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        LabelPassRetry.Text = ""
    End Sub

    Private Sub settext(ByVal txt As String)
        If Me.LabelPassRetry.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf settext)
            Me.Invoke(d, New Object() {txt})
        Else
            Me.LabelPassRetry.Text = txt
        End If
    End Sub

    Private Sub contrvis(vis As Boolean)
        OK.Enabled = vis
        Cancel.Enabled = vis
        UsernameTextBox.Enabled = vis
        PasswordTextBox.Enabled = vis
    End Sub

    Private Sub LoginFrm_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Control And e.Alt Then
            set_overide = True
        Else
            set_overide = False
        End If
    End Sub
End Class
