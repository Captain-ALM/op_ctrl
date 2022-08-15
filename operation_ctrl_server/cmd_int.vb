Imports System.Windows.Forms

Public Class cmd_int
    Public internal_command As internal_command_name = internal_command_name.none
    Public internal_cmd_args As String = ""

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        combbxic.Enabled = False
        txtbxargs.Enabled = False
        internal_command = combbxic.SelectedIndex
        If internal_command = internal_command_name.Restart Or internal_command = internal_command_name.dll Or internal_command = internal_command_name.rdll Then
            internal_cmd_args = txtbxargs.Text
        Else
            internal_cmd_args = ""
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        combbxic.Enabled = False
        txtbxargs.Enabled = False
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cmd_int_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        combbxic.SelectedIndex = 0
        combbxic.Enabled = True
        txtbxargs.Enabled = False
        txtbxargs.Text = ""
        internal_command = internal_command_name.none
        internal_cmd_args = ""
    End Sub

    Private Sub combbxic_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combbxic.SelectedIndexChanged
        If combbxic.SelectedIndex = internal_command_name.Restart Or combbxic.SelectedIndex = internal_command_name.dll Or combbxic.SelectedIndex = internal_command_name.rdll Or combbxic.SelectedIndex = internal_command_name.sthread Then
            txtbxargs.Enabled = True
        Else
            txtbxargs.Enabled = False
        End If
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
End Class
