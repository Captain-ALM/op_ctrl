Imports System.Windows.Forms

Public Class cmd_dll
    Public dll_command As String = ""
    Public dll_args As New Object

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        txtbxc.Enabled = False
        txtbxargs.Enabled = False
        dll_command = txtbxc.Text
        dll_args = txtbxargs.Lines
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        txtbxc.Enabled = False
        txtbxargs.Enabled = False
        dll_command = ""
        dll_args = Nothing
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmd_dll_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtbxc.Enabled = True
        txtbxargs.Enabled = True
        txtbxc.Text = ""
        txtbxargs.Text = ""
    End Sub

    Private Sub txtbxargs_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxargs.KeyDown
        If (e.KeyCode = Keys.A And e.Control And Not txtbxargs.SelectionLength >= txtbxargs.Text.Length) Then
            e.SuppressKeyPress = True
            txtbxargs.SelectAll()
            e.Handled = True
            'ElseIf e.KeyCode = Keys.A And e.Control Then
            '    e.SuppressKeyPress = True
            '    e.Handled = True
        End If
    End Sub

    Private Sub txtbxc_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxc.KeyDown
        If (e.KeyCode = Keys.A And e.Control And Not txtbxc.SelectionLength >= txtbxc.Text.Length) Then
            e.SuppressKeyPress = True
            txtbxc.SelectAll()
            e.Handled = True
            'ElseIf e.KeyCode = Keys.A And e.Control Then
            '    e.SuppressKeyPress = True
            '    e.Handled = True
        End If
    End Sub
End Class
