Imports System.Net
Imports System.Threading
Imports System.Reflection
Imports System.IO
Imports captainalm.calmclientandserver

Public Class main
    Public configurator As New config
    Public ip_address As IPAddress = IPAddress.Any
    Public port As Integer = 9786
    Public ui_process_thread As New Thread(New ThreadStart(AddressOf ui_sub))
    Public ui_process_exit As Boolean = False
    Private block_lview_r As New Object()
    Private block_stats_r As New Object()

    Private Sub main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        contrvis(False)
        main_instance = Me
    End Sub

    Private Sub main_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Dim ret As DialogResult = configurator.ShowDialog()
        If ret = Windows.Forms.DialogResult.OK Then
            ip_address = configurator.selected_interface
            port = configurator.port
            delay = Not configurator.buffer
        Else
            Me.Close()
        End If

        op_ip_address = ip_address
        op_port = port
        Dim chk As Boolean = start()
        If Not chk Then
            End
        End If

        ui_process_thread.IsBackground = True
        ui_process_thread.Start()

        contrvis(True)

        'If ListViewMessages.SelectedItems.Count = 0 Then
        '    contrvis(False, visop.openmsg)
        'Else
        '    contrvis(True, visop.openmsg)
        'End If

        'If ListViewClients.SelectedItems.Count = 0 Then
        'contrvis(False, visop.clman)
        'Else
        'contrvis(True, visop.clman)
        'End If
    End Sub

    Private Sub contrvis(vis As Boolean, Optional ops As visop = visop.all)
        If ops = visop.all Then
            butrfs.Enabled = vis
            butreset.Enabled = vis
            butexit.Enabled = vis
            SplitContainerPanel.Enabled = vis
        End If
        If ops = visop.msglview Or ops = visop.all Then
            ListViewMessages.Enabled = vis
        End If
        If ops = visop.cllview Or ops = visop.all Then
            ListViewClients.Enabled = vis
        End If
        If ops = visop.clrmsg Or ops = visop.all Then
            butmsgcls.Enabled = vis
        End If
        If ops = visop.openmsg Or ops = visop.all Then
            butopenmsg.Enabled = vis
        End If
        If ops = visop.clman Or ops = visop.all Then
            butadddll.Enabled = vis
            butdiscon.Enabled = vis
            butsenddllmsg.Enabled = vis
            butsendintmsg.Enabled = vis
        End If
    End Sub

    Private Enum visop As Integer
        all = 0
        openmsg = 2
        clrmsg = 3
        clman = 1
        cllview = 4
        msglview = 5
    End Enum

    Private Sub ui_sub()
        While Not ui_process_exit
            Try
                While ui_process_queue.Count > 0
                    Dim del As [Delegate] = ui_process_queue.Dequeue()
                    Me.Invoke(del)
                    Thread.Sleep(100)
                End While
            Catch ex As Exception
            End Try
            Thread.Sleep(100)
        End While
        Thread.CurrentThread.Abort()
    End Sub

    Public Sub refresh_l_views()
        SyncLock block_lview_r
            contrvis(False)
            Try
                ListViewClients.Items.Clear()
                For Each current As ListViewItem In client_data_list_get_values()
                    ListViewClients.Items.Add(current)
                Next
            Catch ex As Exception
            End Try
            Try
                ListViewMessages.Items.Clear()
                For Each current As ListViewItem In message_data_list_get_values()
                    ListViewMessages.Items.Add(current)
                Next
            Catch ex As Exception
            End Try
            contrvis(True)
        End SyncLock
    End Sub

    Private Sub butrfs_Click(sender As Object, e As EventArgs) Handles butrfs.Click
        send_msg_cl_req(server_obj.ConnectedClients)
    End Sub

    Private Sub butmsgcls_Click(sender As Object, e As EventArgs) Handles butmsgcls.Click
        message_data_store_clear_queued = True
    End Sub

    Private Sub butreset_Click(sender As Object, e As EventArgs) Handles butreset.Click
        contrvis(False)
        stop_server()
        Process.Start(Assembly.GetEntryAssembly.Location)
        Me.Close()
    End Sub

    Private Sub butexit_Click(sender As Object, e As EventArgs) Handles butexit.Click
        contrvis(False)
        stop_server()
        Me.Close()
    End Sub

    Private Sub butopenmsg_Click(sender As Object, e As EventArgs) Handles butopenmsg.Click
        If ListViewMessages.SelectedItems.Count = 1 Then
            contrvis(False)
            Dim selected_item As ListViewItem = ListViewMessages.SelectedItems(0)
            Dim item_ref As Integer = CInt(selected_item.Text)
            Dim form As New msgread
            form.setup(item_ref)
            form.ShowDialog()
            If Not form.Disposing AndAlso form.IsDisposed Then
                form.Dispose()
            End If
            contrvis(True)
        End If
    End Sub

    'Private Sub ListViewMessages_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListViewMessages.SelectedIndexChanged
    '    If ListViewMessages.SelectedItems.Count = 0 Then
    '        contrvis(False, visop.openmsg)
    '    Else
    '        contrvis(True, visop.openmsg)
    '    End If
    'End Sub

    'Private Sub ListViewClients_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListViewClients.SelectedIndexChanged
    '    If ListViewClients.SelectedItems.Count = 0 Then
    '        contrvis(False, visop.clman)
    '    Else
    '        contrvis(True, visop.clman)
    '    End If
    'End Sub

    Private Sub butadddll_Click(sender As Object, e As EventArgs) Handles butadddll.Click
        If Not ListViewClients.SelectedItems.Count = 0 Then
            contrvis(False)
            If OpenDLLDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim dlls_to_load As String() = OpenDLLDialog.FileNames
                For Each current_dll As String In dlls_to_load
                    If File.Exists(current_dll) Then
                        If Path.GetExtension(current_dll) = ".dll" Then
                            Dim dll_loaded As Byte() = File.ReadAllBytes(current_dll)
                            Dim rec As New List(Of String)
                            For Each current_item As ListViewItem In ListViewClients.SelectedItems
                                Dim c_clnom As String = current_item.Text
                                rec.Add(c_clnom)
                            Next
                            send_msg(rec, "dll:" & Path.GetFileNameWithoutExtension(current_dll), New encapsulation(dll_loaded))
                        ElseIf Path.GetExtension(current_dll) = ".vb" Then
                            Dim dll_loaded As String = File.ReadAllText(current_dll)
                            Dim rec As New List(Of String)
                            For Each current_item As ListViewItem In ListViewClients.SelectedItems
                                Dim c_clnom As String = current_item.Text
                                rec.Add(c_clnom)
                            Next
                            send_msg(rec, "compvb:" & Path.GetFileNameWithoutExtension(current_dll), dll_loaded)
                        ElseIf Path.GetExtension(current_dll) = ".cs" Then
                            Dim dll_loaded As String = File.ReadAllText(current_dll)
                            Dim rec As New List(Of String)
                            For Each current_item As ListViewItem In ListViewClients.SelectedItems
                                Dim c_clnom As String = current_item.Text
                                rec.Add(c_clnom)
                            Next
                            send_msg(rec, "compcs:" & Path.GetFileNameWithoutExtension(current_dll), dll_loaded)
                        End If
                    End If
                Next
            End If
            OpenDLLDialog.Reset()
            OpenDLLDialog.Multiselect = True
            OpenDLLDialog.Title = "Choose DLL to load:"
            OpenDLLDialog.Filter = "Loadable Files|*.dll;*.vb;*.cs|Source Code Files|*.vb;*.cs|VB Source Files|*.vb|C# Source Files|*.cs|Dynamic Link Libraries|*.dll|All Files|*.*"
            contrvis(True)
        End If
    End Sub

    Private Sub butsendintmsg_Click(sender As Object, e As EventArgs) Handles butsendintmsg.Click
        If Not ListViewClients.SelectedItems.Count = 0 Then
            contrvis(False)
            Dim form As New cmd_int
            Dim chk As DialogResult = form.ShowDialog()
            If chk = Windows.Forms.DialogResult.OK Then
                Dim selected_command As internal_command_name = form.internal_command
                Dim args_fsc As String = form.internal_cmd_args
                Dim rec As New List(Of String)
                For Each current_item As ListViewItem In ListViewClients.SelectedItems
                    Dim c_clnom As String = current_item.Text
                    rec.Add(c_clnom)
                Next
                send_msg(rec, "int:" & convert_int_cmd_int_to_name(selected_command), args_fsc)
            End If
            If Not form.Disposing AndAlso form.IsDisposed Then
                form.Dispose()
            End If
            contrvis(True)
        End If
    End Sub

    Private Sub butsenddllmsg_Click(sender As Object, e As EventArgs) Handles butsenddllmsg.Click
        If Not ListViewClients.SelectedItems.Count = 0 Then
            contrvis(False)
            Dim form As New cmd_dll
            Dim chk As DialogResult = form.ShowDialog()
            If chk = Windows.Forms.DialogResult.OK Then
                Dim selected_command As String = form.dll_command
                Dim args_fsc2 As Array = form.dll_args
                Dim args_fsc As Object = args_fsc2
                Dim rec As New List(Of String)
                For Each current_item As ListViewItem In ListViewClients.SelectedItems
                    Dim c_clnom As String = current_item.Text
                    rec.Add(c_clnom)
                Next
                send_msg(rec, "cmd:" & selected_command, New encapsulation(args_fsc))
            End If
            If Not form.Disposing AndAlso form.IsDisposed Then
                form.Dispose()
            End If
            contrvis(True)
        End If
    End Sub

    Private Sub butdiscon_Click(sender As Object, e As EventArgs) Handles butdiscon.Click
        If Not ListViewClients.SelectedItems.Count = 0 Then
            contrvis(False)
            For Each current_item As ListViewItem In ListViewClients.SelectedItems
                Dim c_clnom As String = current_item.Text
                server_obj.Disconnect(c_clnom)
                client_data_store_remove(c_clnom)
                client_data_list_remove(c_clnom)
            Next
            contrvis(True)
            update_lv = True
        End If
    End Sub

    Public Sub refresh_stats()
        SyncLock block_stats_r
            If progressbar_on Then
                ProgressBarstats.Style = ProgressBarStyle.Marquee
                ProgressBarstats.Value = 50
            Else
                ProgressBarstats.Value = 0
                ProgressBarstats.Style = ProgressBarStyle.Blocks
            End If

            If stat_text <> "" Then
                lblstats.Text = stat_text
            Else
                lblstats.Text = "Ready to send Packets."
            End If
        End SyncLock
    End Sub

    Private Sub chkbxst_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxst.CheckedChanged
        multi_thread_send = chkbxst.Checked
    End Sub

    Private Sub ListViewClients_KeyDown(sender As Object, e As KeyEventArgs) Handles ListViewClients.KeyDown
        e.SuppressKeyPress = True
        If (e.KeyCode = Keys.A And e.Control) Then
            If ListViewClients.SelectedItems.Count >= ListViewClients.Items.Count Then
                ListViewClients.SelectedIndices.Clear()
            Else
                For i As Integer = 0 To ListViewClients.Items.Count - 1 Step 1
                    ListViewClients.SelectedIndices.Add(i)
                Next
            End If
            e.Handled = True
        ElseIf e.KeyCode = Keys.Delete Then
            If Not ListViewClients.SelectedItems.Count = 0 Then
                contrvis(False)
                For Each current_item As ListViewItem In ListViewClients.SelectedItems
                    Dim c_clnom As String = current_item.Text
                    server_obj.Disconnect(c_clnom)
                    client_data_store_remove(c_clnom)
                    client_data_list_remove(c_clnom)
                Next
                contrvis(True)
                update_lv = True
            End If
            e.Handled = True
        ElseIf e.KeyCode = Keys.End Then
            If Not ListViewClients.SelectedItems.Count = 0 Then
                contrvis(False)
                Dim rec As New List(Of String)
                For Each current_item As ListViewItem In ListViewClients.SelectedItems
                    Dim c_clnom As String = current_item.Text
                    rec.Add(c_clnom)
                Next
                send_msg(rec, "int:" & convert_int_cmd_int_to_name(internal_command_name.End), "")
                contrvis(True)
                update_lv = True
            End If
            e.Handled = True
        ElseIf e.KeyCode = Keys.F5 Then
            send_msg_cl_req(server_obj.ConnectedClients)
            e.Handled = True
        End If
    End Sub

    Private Sub butabout_Click(sender As Object, e As EventArgs) Handles butabout.Click
        Dim form As New AboutBx()
        contrvis(False)
        form.ShowDialog(Me)
        contrvis(True)
        If Not form.Disposing AndAlso form.IsDisposed Then
            form.Dispose()
        End If
    End Sub

    Private Sub butrsndmsg_Click(sender As Object, e As EventArgs) Handles butrsndmsg.Click
        If ListViewMessages.SelectedItems.Count = 1 Then
            contrvis(False)
            Try
                Dim selected_item As ListViewItem = ListViewMessages.SelectedItems(0)
                Dim item_ref As Integer = CInt(selected_item.Text)
                Dim msg As message_struct = message_data_store_get(item_ref)
                'If msg.pending_reception And client_data_store_contains_key(msg.client_uuid) Then
                If client_data_store_contains_key(msg.client_uuid) Then
                    Dim rec As New List(Of String)
                    rec.Add(msg.client_uuid)
                    If msg.has_obj Then
                        send_msg(msg.reference_number, rec, msg.server_msg_header, New encapsulation(convertstringtoobject(msg.server_message)))
                    Else
                        send_msg(rec, msg.server_msg_header, msg.server_message)
                    End If
                End If
            Catch ex As Exception
            End Try
            contrvis(True)
        End If
    End Sub

    Private Sub ListViewMessages_KeyDown(sender As Object, e As KeyEventArgs) Handles ListViewMessages.KeyDown
        e.SuppressKeyPress = True
        If e.KeyCode = Keys.Delete Then
            message_data_store_clear_queued = True
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter Then
            If ListViewMessages.SelectedItems.Count = 1 Then
                contrvis(False)
                Dim selected_item As ListViewItem = ListViewMessages.SelectedItems(0)
                Dim item_ref As Integer = CInt(selected_item.Text)
                Dim form As New msgread
                form.setup(item_ref)
                form.ShowDialog()
                If Not form.Disposing AndAlso form.IsDisposed Then
                    form.Dispose()
                End If
                contrvis(True)
            End If
            e.Handled = True
        ElseIf e.KeyCode = Keys.Insert Then
            If ListViewMessages.SelectedItems.Count = 1 Then
                contrvis(False)
                Try
                    Dim selected_item As ListViewItem = ListViewMessages.SelectedItems(0)
                    Dim item_ref As Integer = CInt(selected_item.Text)
                    Dim msg As message_struct = message_data_store_get(item_ref)
                    'If msg.pending_reception And client_data_store_contains_key(msg.client_uuid) Then
                    If client_data_store_contains_key(msg.client_uuid) Then
                        Dim rec As New List(Of String)
                        rec.Add(msg.client_uuid)
                        If msg.has_obj Then
                            send_msg(msg.reference_number, rec, msg.server_msg_header, New encapsulation(convertstringtoobject(msg.server_message)))
                        Else
                            send_msg(rec, msg.server_msg_header, msg.server_message)
                        End If
                    End If
                Catch ex As Exception
                End Try
                contrvis(True)
            End If
            e.Handled = True
        End If
    End Sub
End Class
