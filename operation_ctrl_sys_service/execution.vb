Imports captainalm.calmclientandserver
Imports System.Security.Principal
Imports System.Threading
Imports System.Net

Friend Module execution
    Friend client_obj As New client(New client_constructor)
    Friend client_name As String = getNetworkAdapterIP().ToString & "-" & WindowsIdentity.GetCurrent.User.ToString
    Private packets As New List(Of packet)
    Friend receive_packet_queue As New Queue(Of packet)
    Friend received_packet_store As New Dictionary(Of Integer, packet())
    Private connect_was_called As Boolean = False
    Private packet_access_blocker As New Object()
    Private packet_send_blocker As New Object()

    Public Function connect() As Boolean
        Try
            If Not connected And (client_obj.IsConnected = False Or IsNothing(client_obj)) Then
                If IsNothing(client_obj) Then
                    client_obj = New client(New client_constructor())
                End If
                reg_events()
                client_obj.ClientRefreshDelay = Timeout.Infinite 'stops client refresh
                client_obj.DisconnectOnInvalidPacket = False
                Dim toret As Boolean = client_obj.Connect(New ClientStart(IPAddress.Parse(ip_address), port, client_name, New EncryptionParameter(EncryptionMethod.unicodease, pass), 8192, True, nodelay))
                If toret Then
                    connect_was_called = True
                End If
                Return toret
            Else
                Return False
            End If
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function disconnect() As Boolean
        Try
            If connect_was_called Or (connected And (client_obj.IsConnected Or Not IsNothing(client_obj))) Then
                connected = False
                disconnected_so_timeout = True
                Try
                    For Each Val As String In thread_registry_get_keys()
                        Dim tmp_t As Thread = thread_registry_get(Val)
                        If tmp_t.IsAlive Then
                            tmp_t.Abort()
                        Else
                            unreg_thread(Val)
                        End If
                    Next
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                End Try
                Try
                    client_obj.Disconnect()
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                End Try
                Try
                    unreg_events()
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                End Try
                Try
                    client_obj.KillThreads()
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                End Try
                Try
                    client_obj.Flush()
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                End Try
                client_obj = Nothing
                client_obj = New client(New client_constructor)
                connect_was_called = False
                Return True
            End If
            Return False
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Function send_packet(p As packet)
        If connected Then
            If client_obj.IsConnected Then
                Dim toret As Boolean = False
                SyncLock packet_send_blocker
                    'Dim packetts As packet() = split_packets(p, 2048)
                    'Dim delay_it_count As Integer = 0
                    'For i As Integer = 0 To packetts.Length - 1 Step 1
                    '    If delay_it_count > 3 Then
                    '        Thread.Sleep(1000)
                    '        delay_it_count = 0
                    '    End If
                    '    client_obj.Send(packetts(i))
                    '    delay_it_count += 1
                    '    Thread.Sleep(500)
                    'Next
                    'delay_it_count = 0
                    client_obj.Send(p)
                    toret = True
                End SyncLock
                Return toret
            End If
        End If
        Return False
    End Function

    Sub reg_events()
        AddHandler client_obj.ServerConnectSuccess, AddressOf server_con
        AddHandler client_obj.ServerDisconnect, AddressOf server_dis
        AddHandler client_obj.ServerMessage, AddressOf server_msg
        AddHandler client_obj.ServerConnectFailed, AddressOf server_dis2
    End Sub

    Sub unreg_events()
        RemoveHandler client_obj.ServerConnectSuccess, AddressOf server_con
        RemoveHandler client_obj.ServerDisconnect, AddressOf server_dis
        RemoveHandler client_obj.ServerMessage, AddressOf server_msg
        RemoveHandler client_obj.ServerConnectFailed, AddressOf server_dis2
    End Sub

    Sub server_con()
        connected = True
    End Sub

    Sub server_dis()
        disconnected_so_timeout = True
        connected = False
        clearpackets()
        Try
            For Each Val As String In thread_registry_get_keys()
                Dim tmp_t As Thread = thread_registry_get(Val)
                If tmp_t.IsAlive Then
                    tmp_t.Abort()
                Else
                    unreg_thread(Val)
                End If
            Next
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Try
            unreg_events()
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Try
            client_obj.KillThreads()
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Try
            client_obj.Flush()
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        client_obj = Nothing
        client_obj = New client(New client_constructor)
    End Sub

    Sub server_dis2(ByVal r As failed_connection_reason)
        disconnected_so_timeout = True
        connected = False
        clearpackets()
        Try
            Try
                For Each Val As String In thread_registry_get_keys()
                    Dim tmp_t As Thread = thread_registry_get(Val)
                    If tmp_t.IsAlive Then
                        tmp_t.Abort()
                    Else
                        unreg_thread(Val)
                    End If
                Next
            Catch ex As ThreadAbortException
                Thread.CurrentThread.Abort()
            Catch ex As Exception
            End Try
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Try
            unreg_events()
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Try
            client_obj.KillThreads()
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Try
            client_obj.Flush()
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        client_obj = Nothing
        client_obj = New client(New client_constructor)
    End Sub

    Sub server_msg(p As packet)
        addpacket(p)
    End Sub

    Sub addpacket(p As packet)
        SyncLock packet_access_blocker
            packets.Add(p)
        End SyncLock
    End Sub

    Function getpacket(index As Integer) As packet
        SyncLock packet_access_blocker
            Return packets(index)
        End SyncLock
    End Function

    Function getpackets() As List(Of packet)
        SyncLock packet_access_blocker
            Dim newpacket As New List(Of packet)
            For i As Integer = 1 To packets.Count - 1 Step 1
                newpacket.Add(packets(i))
            Next
            Return newpacket
        End SyncLock
    End Function

    Function getcount() As Integer
        SyncLock packet_access_blocker
            Return packets.Count
        End SyncLock
    End Function

    Sub removepacket(index As Integer)
        SyncLock packet_access_blocker
            packets.RemoveAt(index)
        End SyncLock
    End Sub

    Sub clearpackets()
        SyncLock packet_access_blocker
            packets.Clear()
        End SyncLock
    End Sub
End Module
