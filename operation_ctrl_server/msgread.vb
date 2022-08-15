Imports System.Windows.Forms

Public Class msgread
    Private refnumber As Integer = 0

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Public Sub setup(refnum As Integer)
        refnumber = refnum
    End Sub

    Private Sub msgread_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If message_data_store_contains_key(refnumber) Then
            Dim msg As message_struct = message_data_store_get(refnumber)
            txtbxcluuid.Text = msg.client_uuid
            txtbxrefnum.Text = msg.reference_number
            txtbxservermsg.Text = msg.server_msg_header & ControlChars.CrLf & msg.server_message
            txtbxclientmsg.Text = msg.client_msg_header & ControlChars.CrLf & msg.client_message
        Else
            Me.Close()
        End If
    End Sub

    Private Sub txtbxcluuid_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxcluuid.KeyDown
        If (e.KeyCode = Keys.A And e.Control And Not txtbxcluuid.SelectionLength >= txtbxcluuid.Text.Length) Then
            e.SuppressKeyPress = True
            txtbxcluuid.SelectAll()
            e.Handled = True
            'ElseIf e.KeyCode = Keys.A And e.Control Then
            '    e.SuppressKeyPress = True
            '    e.Handled = True
        End If
    End Sub

    Private Sub txtbxrefnum_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxrefnum.KeyDown
        If (e.KeyCode = Keys.A And e.Control And Not txtbxrefnum.SelectionLength >= txtbxrefnum.Text.Length) Then
            e.SuppressKeyPress = True
            txtbxrefnum.SelectAll()
            e.Handled = True
            'ElseIf e.KeyCode = Keys.A And e.Control Then
            '    e.SuppressKeyPress = True
            '    e.Handled = True
        End If
    End Sub

    Private Sub txtbxservermsg_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxservermsg.KeyDown
        If (e.KeyCode = Keys.A And e.Control And Not txtbxservermsg.SelectionLength >= txtbxservermsg.Text.Length) Then
            e.SuppressKeyPress = True
            txtbxservermsg.SelectAll()
            e.Handled = True
            'ElseIf e.KeyCode = Keys.A And e.Control Then
            '    e.SuppressKeyPress = True
            '    e.Handled = True
        End If
    End Sub

    Private Sub txtbxclientmsg_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbxclientmsg.KeyDown
        If (e.KeyCode = Keys.A And e.Control And Not txtbxclientmsg.SelectionLength >= txtbxclientmsg.Text.Length) Then
            e.SuppressKeyPress = True
            txtbxclientmsg.SelectAll()
            e.Handled = True
            'ElseIf e.KeyCode = Keys.A And e.Control Then
            '    e.SuppressKeyPress = True
            '    e.Handled = True
        End If
    End Sub
End Class
