Imports System.Windows.Forms
Imports System.Net

Public Class config
    Public selected_interface As IPAddress = IPAddress.Any
    Public port As Integer = 9786
    Public buffer As Boolean = True
    Public interfaces As Dictionary(Of String, IPAddress)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        combbxif.Enabled = False
        txtbxport.Enabled = False
        selected_interface = interfaces(combbxif.SelectedItem.ToString)
        port = parseInteger(txtbxport.Text)
        buffer = chkbxbf.Checked
        If port > 65535 Then
            MsgBox("The Port you entered was too Big! The Port is now 65535.", MsgBoxStyle.Exclamation, "Information!")
            port = 65535
        End If
        If port < 1 Then
            MsgBox("The Port you entered was too Small! The Port is now 1.", MsgBoxStyle.Exclamation, "Information!")
            port = 1
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        combbxif.Enabled = False
        txtbxport.Enabled = False
        chkbxbf.Enabled = False
        selected_interface = IPAddress.Any
        port = 9786
        buffer = True
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub config_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        combbxif.Enabled = False
        txtbxport.Enabled = False
        chkbxbf.Enabled = False
        interfaces = getNetworkAdapterIPsAndNames()
        interfaces.Add("Listen on All Interfaces : 0.0.0.0", IPAddress.Any)
        txtbxport.Text = 9786
        For Each current As String In interfaces.Keys
            combbxif.Items.Add(current)
        Next
        combbxif.SelectedItem = "Listen on All Interfaces : 0.0.0.0"
        chkbxbf.Checked = True
        combbxif.Enabled = True
        txtbxport.Enabled = True
        chkbxbf.Enabled = True
    End Sub
End Class
