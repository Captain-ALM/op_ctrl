Imports captainalm.calmclientandserver
Imports System.Net
Imports System.Threading

Module op_server
    Friend pass As String = "tor stinks"
    Public op_ip_address As IPAddress = IPAddress.Any
    Public op_port As Integer = 9786
    Public server_obj As New server(New server_constructor())
    Public running As Boolean = False
    Public msg_man_thread As Thread = Nothing
    Public msg_snd_thread As Thread = Nothing
    Public misc_thread As Thread = Nothing
    Public msg_pr_queue As New Queue(Of packet)
    Public msg_s_queue As New Queue(Of Ipacket_data)
    Public ui_process_queue As New Queue(Of [Delegate])
    Public main_instance As main = Nothing
    Public client_data_list As New Dictionary(Of String, ListViewItem)
    Public message_data_list As New Dictionary(Of Integer, ListViewItem)
    Public client_data_store As New Dictionary(Of String, client_data_struct)
    Public message_data_store As New Dictionary(Of Integer, message_struct)
    Public message_data_store_clear_queued As Boolean = False
    Public client_data_queue As New Queue(Of String)
    Public received_packet_store As New Dictionary(Of Integer, packet())
    Public received_pr_queue As New Queue(Of packet)
    Public update_stats As Boolean = False
    Public update_lv As Boolean = False
    Public stat_text As String = ""
    Public progressbar_on As Boolean = False
    Private current_ref_number As Integer = 1
    Private ref_num_block As New Object()
    Public send_threads As Thread()
    Public multi_thread_send As Boolean = False
    Public message_send_on As Boolean = False
    Public thread_c_dat As New Dictionary(Of Integer, Integer)
    Public thread_max_dat As New Dictionary(Of Integer, Integer)
    Public thread_ready As New Dictionary(Of Integer, Boolean)
    Public begin_threaded_msg_send As Boolean = False
    Public delay As Boolean = False

    Public Function start() As Boolean
        Try
            If Not running Then
                server_obj.flush()
                server_obj = New server(New server_constructor(op_ip_address, op_port))
                reg_events()
                Dim toret As Boolean = False
                server_obj.DisconnectOnInvalidPacket = False
                toret = server_obj.Start(New ServerStart(New EncryptionParameter(EncryptionMethod.unicodease, pass), 8192, True, delay, True))
                msg_man_thread = New Thread(New ThreadStart(AddressOf msg_man_sub))
                msg_snd_thread = New Thread(New ThreadStart(AddressOf msg_snd_sub))
                misc_thread = New Thread(New ThreadStart(AddressOf misc_sub))
                msg_man_thread.IsBackground = True
                msg_snd_thread.IsBackground = True
                misc_thread.IsBackground = True
                If toret Then
                    running = True
                    misc_thread.Start()
                    msg_man_thread.Start()
                    msg_snd_thread.Start()
                End If
                Return toret
            End If
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Function stop_server() As Boolean
        Try
            If running Then
                server_obj.Stop()
                Do While running
                    Thread.Sleep(100)
                Loop
                unreg_events()
                server_obj.flush()
            End If
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Sub reg_events()
        AddHandler server_obj.ClientConnectSuccess, AddressOf clcon
        AddHandler server_obj.ClientDisconnect, AddressOf cldis
        AddHandler server_obj.ClientMessage, AddressOf clmsg
        AddHandler server_obj.ServerStopped, AddressOf srvs
    End Sub

    Public Sub unreg_events()
        RemoveHandler server_obj.ClientConnectSuccess, AddressOf clcon
        RemoveHandler server_obj.ClientDisconnect, AddressOf cldis
        RemoveHandler server_obj.ClientMessage, AddressOf clmsg
        RemoveHandler server_obj.ServerStopped, AddressOf srvs
    End Sub

    Public Sub clcon(clnom As String) 'on client connect
        client_data_store_add(clnom, New client_data_struct(clnom))
        Dim litem As ListViewItem = New ListViewItem(clnom)
        client_data_list_add(clnom, litem)
        update_lv = True
        client_data_queue.Enqueue(clnom) 'add client to the data request list
    End Sub

    Public Sub cldis(clnom As String) 'on client disconnect
        client_data_store_remove(clnom)
        client_data_list_remove(clnom)
        update_lv = True
    End Sub

    Public Sub clmsg(clnom As String, p As packet) 'on client message
        'msg_pr_queue.Enqueue(p)
        If p.isvalidpacket Then
            received_pr_queue.Enqueue(p)
        End If
    End Sub

    Public Sub srvs() 'on server stop
        running = False
    End Sub

    Public Sub send_msg(recievers As List(Of String), header As String, message As String)
        Dim p As New packet_data(recievers, header, message)
        msg_s_queue.Enqueue(p)
    End Sub

    Public Sub send_msg(rn As Integer, recievers As List(Of String), header As String, message As String)
        Dim p As New packet_data_rsnd(rn, recievers, header, message)
        msg_s_queue.Enqueue(p)
    End Sub

    Public Sub send_msg_cl_req(recievers As List(Of String))
        Dim p As New packet_data_req(recievers)
        msg_s_queue.Enqueue(p)
    End Sub

    Public Sub send_msg(recievers As List(Of String), header As String, message As encapsulation)
        Dim p As New packet_data(recievers, header, message)
        msg_s_queue.Enqueue(p)
    End Sub

    Public Sub send_msg(rn As Integer, recievers As List(Of String), header As String, message As encapsulation)
        Dim p As New packet_data_rsnd(rn, recievers, header, message)
        msg_s_queue.Enqueue(p)
    End Sub

    Public Sub msg_man_sub()
        While running
            Try
                If received_pr_queue.Count > 0 Then
                    Dim p As packet = received_pr_queue.Dequeue()
                    msg_pr_queue.Enqueue(p)
                    '    If p.header.StartsWith("0;") Then
                    '        Dim packet_frame(Int(p.stringdata(pass))) As packet
                    '        packet_frame(0) = p
                    '        received_packet_store_add(p.referencenumber, packet_frame)
                    'Else
                    '    If received_packet_store_contains_key(p.referencenumber) Then
                    '        Dim index_str As String = p.header.Substring(0, p.header.IndexOf(";"))
                    '        Dim index As Integer = Int(index_str)
                    '        Dim packet_frame As packet() = received_packet_store_get(p.referencenumber)
                    '        packet_frame(index) = p
                    '        received_packet_store_add(p.referencenumber, packet_frame)
                    '        If index = Int(packet_frame(0).stringdata(pass)) Then
                    '                msg_pr_queue.Enqueue(join_packets(received_packet_store_get(p.referencenumber)))
                    '            received_packet_store_remove(p.referencenumber)
                    '        End If
                    '    End If
                    'End If
                End If

                If msg_pr_queue.Count > 0 Then
                    Dim p As packet = msg_pr_queue.Dequeue()
                    If p.header = "client_data" Then
                        client_data_store_add(p.sender, New client_data_struct(p.sender, p.stringdata(pass)))
                    Dim litem As ListViewItem = New ListViewItem(p.sender)
                        litem.SubItems.Add(client_data_store_get(p.sender).user_name)
                        litem.SubItems.Add(client_data_store_get(p.sender).ip)
                        litem.SubItems.Add(client_data_store_get(p.sender).isadmin)
                        litem.SubItems.Add(client_data_store_get(p.sender).sid)
                        client_data_list_add(p.sender, litem)
                    update_lv = True
                ElseIf p.header = "return" Then
                        If message_data_store_contains_key(p.referencenumber) Then
                            message_data_store_add(p.referencenumber, New message_struct(message_data_store_get(p.referencenumber), p.stringdata(pass), p.header))
                            Dim litem As ListViewItem = New ListViewItem(p.referencenumber)
                            litem.SubItems.Add(message_data_store_get(p.referencenumber).server_msg_header & ControlChars.CrLf & message_data_store_get(p.referencenumber).server_message)
                            litem.SubItems.Add(message_data_store_get(p.referencenumber).client_msg_header & ControlChars.CrLf & message_data_store_get(p.referencenumber).client_message)
                            litem.SubItems.Add(message_data_store_get(p.referencenumber).client_uuid)
                            message_data_list_add(p.referencenumber, litem)
                        update_lv = True
                    End If
                End If
                End If
            Catch ex As Exception
            End Try
            Thread.Sleep(100)
        End While
        Thread.CurrentThread.Abort()
    End Sub

    Public Sub msg_snd_sub()
        While running
            Try
                If msg_s_queue.Count > 0 Then
                    message_send_on = True
                    Dim p As Ipacket_data = msg_s_queue.Dequeue()
                    If multi_thread_send Then
                        progressbar_on = True
                        stat_text = "Preparing to send Packets..."
                        update_stats = True
                        send_threads = Nothing
                        ReDim send_threads(p.get_receivers.Count - 1)
                        thread_ready_clear()
                        Dim ct As Integer = 0
                        For Each current As String In p.get_receivers
                            If p.get_header <> "req" And (p.get_header.StartsWith("dll:") Or p.get_header.StartsWith("cmd:") Or p.get_header.StartsWith("int:") Or p.get_header.StartsWith("compcs:") Or p.get_header.StartsWith("compvb:")) Then
                                Dim p_to_send As packet = p.make_packet(New packet_data_param)
                                message_data_store_add(p_to_send.referencenumber, New message_struct(p_to_send.referencenumber, current, p_to_send.stringdata(pass), p_to_send.hasobject, p_to_send.header))
                                Dim litem As ListViewItem = New ListViewItem(p_to_send.referencenumber)
                                litem.SubItems.Add(message_data_store_get(p_to_send.referencenumber).server_msg_header & ControlChars.CrLf & message_data_store_get(p_to_send.referencenumber).server_message)
                                litem.SubItems.Add(message_data_store_get(p_to_send.referencenumber).client_message)
                                litem.SubItems.Add(message_data_store_get(p_to_send.referencenumber).client_uuid)
                                message_data_list_add(p_to_send.referencenumber, litem)
                                update_lv = True
                                send_threads(ct) = New Thread(New ThreadStart(Sub() send_forked_execute(p_to_send.referencenumber, current, p_to_send)))
                                send_threads(ct).IsBackground = True
                                thread_ready_add(p_to_send.referencenumber, False)
                            Else
                                Dim p_to_send As packet = p.make_packet(New packet_data_param(current))
                                send_threads(ct) = New Thread(New ThreadStart(Sub() send_forked_execute(p_to_send.referencenumber, current, p_to_send)))
                                send_threads(ct).IsBackground = True
                                thread_ready_add(p_to_send.referencenumber, False)
                            End If
                            ct += 1
                        Next
                        begin_threaded_msg_send = False
                        thread_c_dat_clear()
                        thread_max_dat_clear()
                        For Each t As Thread In send_threads
                            t.Start()
                        Next
                        Dim expression As Boolean = False
                        While Not expression
                            expression = True
                            For Each bool As Boolean In thread_ready_get_values()
                                expression = expression And bool
                            Next
                            Thread.Sleep(100)
                        End While
                        Dim total_packets As Integer = 0
                        For Each Int As Integer In thread_max_dat_get_values()
                            total_packets += Int
                        Next
                        begin_threaded_msg_send = True
                        Dim intval As Integer = 0
                        While intval < total_packets
                            intval = 0
                            For Each Int As Integer In thread_c_dat_get_values()
                                intval += Int
                            Next
                            stat_text = "Sending Packets: " & intval & "/" & total_packets & "."
                            update_stats = True
                            Thread.Sleep(100)
                        End While
                        stat_text = "Finishing Up..."
                        update_stats = True
                        For Each t As Thread In send_threads
                            If Not IsNothing(t) Then
                                If t.IsAlive Then
                                    t.Join(10000)
                                    If t.IsAlive Then
                                        t.Abort()
                                    End If
                                End If
                            End If
                        Next
                        begin_threaded_msg_send = False

                        progressbar_on = False
                        stat_text = ""
                        update_stats = True
                    Else
                        For Each current As String In p.get_receivers
                            If p.get_header <> "req" And (p.get_header.StartsWith("dll:") Or p.get_header.StartsWith("cmd:") Or p.get_header.StartsWith("int:") Or p.get_header.StartsWith("compcs:") Or p.get_header.StartsWith("compvb:")) Then
                                progressbar_on = True
                                stat_text = "Preparing to send Packet..."
                                update_stats = True
                                Dim p_to_send As packet = p.make_packet(New packet_data_param())
                                message_data_store_add(p_to_send.referencenumber, New message_struct(p_to_send.referencenumber, current, p_to_send.stringdata(pass), p_to_send.hasobject, p_to_send.header))
                                Dim litem As ListViewItem = New ListViewItem(p_to_send.referencenumber)
                                litem.SubItems.Add(message_data_store_get(p_to_send.referencenumber).server_msg_header & ControlChars.CrLf & message_data_store_get(p_to_send.referencenumber).server_message)
                                litem.SubItems.Add(message_data_store_get(p_to_send.referencenumber).client_message)
                                litem.SubItems.Add(message_data_store_get(p_to_send.referencenumber).client_uuid)
                                message_data_list_add(p_to_send.referencenumber, litem)
                                update_lv = True
                                'Dim packetts As packet() = split_packets(p_to_send, 2048)
                                'Dim delay_it_count As Integer = 0
                                'For i As Integer = 0 To packetts.Length - 1 Step 1
                                '    If delay_it_count > 3 Then
                                '        Thread.Sleep(1000)
                                '        delay_it_count = 0
                                '    End If
                                '    stat_text = "Sending Packets: " & i + 1 & "/" & packetts.Length & "."
                                '    update_stats = True
                                '    server_obj.Send(current, packetts(i))
                                '    delay_it_count += 1
                                '    Thread.Sleep(500)
                                'Next
                                'delay_it_count = 0
                                stat_text = "Sending Packet..."
                                update_stats = True
                                server_obj.Send(current, p_to_send)
                                progressbar_on = False
                                stat_text = ""
                                update_stats = True
                            Else
                                progressbar_on = True
                                stat_text = "Preparing to send Packet..."
                                update_stats = True
                                Dim p_to_send As packet = p.make_packet(New packet_data_param(current))
                                'Dim packetts As packet() = split_packets(p_to_send, 2048)
                                'Dim delay_it_count As Integer = 0
                                'For i As Integer = 0 To packetts.Length - 1 Step 1
                                '    If delay_it_count > 3 Then
                                '        Thread.Sleep(1000)
                                '        delay_it_count = 0
                                '    End If
                                '    stat_text = "Sending Packets: " & i + 1 & "/" & packetts.Length & "."
                                '    update_stats = True
                                '    server_obj.Send(current, packetts(i))
                                '    delay_it_count += 1
                                '    Thread.Sleep(500)
                                'Next
                                'delay_it_count = 0
                                stat_text = "Sending Packet... "
                                update_stats = True
                                server_obj.Send(current, p_to_send)
                                progressbar_on = False
                                stat_text = ""
                                update_stats = True
                                'server_obj.Send(current, p.make_packet_with_refnum_0())
                            End If
                        Next
                    End If
                    progressbar_on = False
                    stat_text = ""
                    update_stats = True
                    message_send_on = False
                End If
            Catch ex As Exception
                progressbar_on = False
                stat_text = ""
                update_stats = True
            End Try
            Thread.Sleep(100)
        End While
        Thread.CurrentThread.Abort()
    End Sub

    Public Sub misc_sub()
        While running
            Try
                If message_send_on And main_instance.chkbxst.Enabled Then
                    ui_process_queue.Enqueue(Sub() main_instance.chkbxst.Enabled = False)
                Else
                    If Not main_instance.chkbxst.Enabled And Not message_send_on Then
                        ui_process_queue.Enqueue(Sub() main_instance.chkbxst.Enabled = True)
                    End If
                End If

                If message_data_store_clear_queued Then
                    For Each current As Integer In message_data_store_get_keys()
                        If Not message_data_store_get(current).pending_reception Or Not client_data_store_contains_key(message_data_store_get(current).client_uuid) Then
                            message_data_store_remove(current)
                            If message_data_list_contains_key(current) Then message_data_list_remove(current)
                        End If
                    Next
                    update_lv = True
                    message_data_store_clear_queued = False
                End If

                If client_data_queue.Count > 0 Then
                    Dim cclnt As String = client_data_queue.Dequeue()
                    Dim rec As New List(Of String)
                    rec.Add(cclnt)
                    send_msg_cl_req(rec)
                End If

                For Each current As String In client_data_store_get_keys()
                    Dim cl_exists As Boolean = False
                    For Each current2 As String In server_obj.ConnectedClients
                        If current = current2 Then
                            cl_exists = True
                        End If
                    Next
                    If Not cl_exists Then
                        If client_data_store_contains_key(current) Then
                            client_data_store_remove(current)
                        End If
                        If client_data_list_contains_key(current) Then
                            client_data_list_remove(current)
                        End If
                        update_lv = True
                    End If
                Next

                If update_lv Then
                    update_lv = False
                    ui_process_queue.Enqueue(Sub() main_instance.refresh_l_views())
                End If

                If update_stats Then
                    update_stats = False
                    ui_process_queue.Enqueue(Sub() main_instance.refresh_stats())
                End If
            Catch ex As Exception
            End Try
            Thread.Sleep(100)
        End While
        Thread.CurrentThread.Abort()
    End Sub

    Public Function generate_ref_number() As Integer
        Dim toret As Integer = 0
        SyncLock ref_num_block
            If current_ref_number = Integer.MaxValue Then
                current_ref_number = Integer.MinValue
                toret = current_ref_number
            Else
                current_ref_number += 1
                toret = current_ref_number
            End If
        End SyncLock
        Return toret
    End Function

    Public Sub send_forked_execute(ByVal refnum As Integer, ByVal rec As String, ByVal p As packet)
        'Dim packetts As packet() = split_packets(p, 2048)
        'Dim delay_it_count As Integer = 0
        'thread_max_dat_add(refnum, packetts.Length)
        thread_max_dat_add(refnum, 1)
        thread_c_dat_add(refnum, 0)
        thread_ready_add(refnum, True)
        While Not begin_threaded_msg_send
            Thread.Sleep(100)
        End While
        server_obj.Send(rec, p)
        thread_c_dat_add(refnum, 1)
        'For i As Integer = 0 To packetts.Length - 1 Step 1
        '    If delay_it_count > 3 Then
        '        Thread.Sleep(1000)
        '        delay_it_count = 0
        '    End If
        '    server_obj.Send(rec, packetts(i))
        '    delay_it_count += 1
        '    thread_c_dat_add(refnum, i + 1)
        '    Thread.Sleep(500)
        'Next
        'delay_it_count = 0
    End Sub
End Module