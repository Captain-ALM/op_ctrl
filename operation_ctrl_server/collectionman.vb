Imports captainalm.calmclientandserver

Module collectionman
    Private client_data_list_blocker As New Object()
    Private message_data_list_blocker As New Object()
    Private client_data_store_blocker As New Object()
    Private message_data_store_blocker As New Object()
    Private received_packet_store_blocker As New Object()
    Private thread_c_dat_blocker As New Object()
    Private thread_max_dat_blocker As New Object()
    Private thread_ready_blocker As New Object()

    Sub client_data_list_add(im As String, c As ListViewItem)
        SyncLock client_data_list_blocker
            If client_data_list.ContainsKey(im) Then
                client_data_list(im) = c
            Else
                client_data_list.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub client_data_list_remove(im As String)
        SyncLock client_data_list_blocker
            If client_data_list.ContainsKey(im) Then
                client_data_list.Remove(im)
            End If
        End SyncLock
    End Sub

    Function client_data_list_contains_key(im As String) As Boolean
        Dim toret As Boolean = False
        SyncLock client_data_list_blocker
            toret = client_data_list.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function client_data_list_get(im As String) As ListViewItem
        Dim toret As ListViewItem = Nothing
        SyncLock client_data_list_blocker
            If client_data_list.ContainsKey(im) Then
                toret = client_data_list(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function client_data_list_get_keys() As Dictionary(Of String, ListViewItem).KeyCollection
        Dim toret As Dictionary(Of String, ListViewItem).KeyCollection
        SyncLock client_data_list_blocker
            toret = client_data_list.Keys
        End SyncLock
        Return toret
    End Function

    Function client_data_list_get_values() As Dictionary(Of String, ListViewItem).ValueCollection
        Dim toret As Dictionary(Of String, ListViewItem).ValueCollection
        SyncLock client_data_list_blocker
            toret = client_data_list.Values
        End SyncLock
        Return toret
    End Function

    Sub client_data_list_clear()
        SyncLock client_data_list_blocker
            client_data_list.Clear()
        End SyncLock
    End Sub

    Sub message_data_list_add(im As Integer, c As ListViewItem)
        SyncLock message_data_list_blocker
            If message_data_list.ContainsKey(im) Then
                message_data_list(im) = c
            Else
                message_data_list.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub message_data_list_remove(im As Integer)
        SyncLock message_data_list_blocker
            If message_data_list.ContainsKey(im) Then
                message_data_list.Remove(im)
            End If
        End SyncLock
    End Sub

    Function message_data_list_contains_key(im As Integer) As Boolean
        Dim toret As Boolean = False
        SyncLock message_data_list_blocker
            toret = message_data_list.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function message_data_list_get(im As Integer) As ListViewItem
        Dim toret As ListViewItem = Nothing
        SyncLock message_data_list_blocker
            If message_data_list.ContainsKey(im) Then
                toret = message_data_list(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function message_data_list_get_keys() As Dictionary(Of Integer, ListViewItem).KeyCollection
        Dim toret As Dictionary(Of Integer, ListViewItem).KeyCollection
        SyncLock message_data_list_blocker
            toret = message_data_list.Keys
        End SyncLock
        Return toret
    End Function

    Function message_data_list_get_values() As Dictionary(Of Integer, ListViewItem).ValueCollection
        Dim toret As Dictionary(Of Integer, ListViewItem).ValueCollection
        SyncLock message_data_list_blocker
            toret = message_data_list.Values
        End SyncLock
        Return toret
    End Function

    Sub message_data_list_clear()
        SyncLock message_data_list_blocker
            message_data_list.Clear()
        End SyncLock
    End Sub

    Sub client_data_store_add(im As String, c As client_data_struct)
        SyncLock client_data_store_blocker
            If client_data_store.ContainsKey(im) Then
                client_data_store(im) = c
            Else
                client_data_store.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub client_data_store_remove(im As String)
        SyncLock client_data_store_blocker
            If client_data_store.ContainsKey(im) Then
                client_data_store.Remove(im)
            End If
        End SyncLock
    End Sub

    Function client_data_store_contains_key(im As String) As Boolean
        Dim toret As Boolean = False
        SyncLock client_data_store_blocker
            toret = client_data_store.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function client_data_store_get(im As String) As client_data_struct
        Dim toret As client_data_struct = Nothing
        SyncLock client_data_store_blocker
            If client_data_store.ContainsKey(im) Then
                toret = client_data_store(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function client_data_store_get_keys() As Dictionary(Of String, client_data_struct).KeyCollection
        Dim toret As Dictionary(Of String, client_data_struct).KeyCollection
        SyncLock client_data_store_blocker
            toret = client_data_store.Keys
        End SyncLock
        Return toret
    End Function

    Function client_data_store_get_values() As Dictionary(Of String, client_data_struct).ValueCollection
        Dim toret As Dictionary(Of String, client_data_struct).ValueCollection
        SyncLock client_data_store_blocker
            toret = client_data_store.Values
        End SyncLock
        Return toret
    End Function

    Sub client_data_store_clear()
        SyncLock client_data_store_blocker
            client_data_store.Clear()
        End SyncLock
    End Sub

    Sub message_data_store_add(im As Integer, c As message_struct)
        SyncLock message_data_store_blocker
            If message_data_store.ContainsKey(im) Then
                message_data_store(im) = c
            Else
                message_data_store.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub message_data_store_remove(im As Integer)
        SyncLock message_data_store_blocker
            If message_data_store.ContainsKey(im) Then
                message_data_store.Remove(im)
            End If
        End SyncLock
    End Sub

    Function message_data_store_contains_key(im As Integer) As Boolean
        Dim toret As Boolean = False
        SyncLock message_data_store_blocker
            toret = message_data_store.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function message_data_store_get(im As Integer) As message_struct
        Dim toret As message_struct = Nothing
        SyncLock message_data_store_blocker
            If message_data_store.ContainsKey(im) Then
                toret = message_data_store(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function message_data_store_get_keys() As Dictionary(Of Integer, message_struct).KeyCollection
        Dim toret As Dictionary(Of Integer, message_struct).KeyCollection
        SyncLock message_data_store_blocker
            toret = message_data_store.Keys
        End SyncLock
        Return toret
    End Function

    Function message_data_store_get_values() As Dictionary(Of Integer, message_struct).ValueCollection
        Dim toret As Dictionary(Of Integer, message_struct).ValueCollection
        SyncLock message_data_store_blocker
            toret = message_data_store.Values
        End SyncLock
        Return toret
    End Function

    Sub message_data_store_clear()
        SyncLock message_data_store_blocker
            message_data_store.Clear()
        End SyncLock
    End Sub

    Sub received_packet_store_add(im As Integer, c As packet())
        SyncLock received_packet_store_blocker
            If received_packet_store.ContainsKey(im) Then
                received_packet_store(im) = c
            Else
                received_packet_store.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub received_packet_store_remove(im As Integer)
        SyncLock received_packet_store_blocker
            If received_packet_store.ContainsKey(im) Then
                received_packet_store.Remove(im)
            End If
        End SyncLock
    End Sub

    Function received_packet_store_contains_key(im As Integer) As Boolean
        Dim toret As Boolean = False
        SyncLock received_packet_store_blocker
            toret = received_packet_store.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function received_packet_store_get(im As Integer) As packet()
        Dim toret As packet() = Nothing
        SyncLock received_packet_store_blocker
            If received_packet_store.ContainsKey(im) Then
                toret = received_packet_store(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function received_packet_store_get_keys() As Dictionary(Of Integer, packet()).KeyCollection
        Dim toret As Dictionary(Of Integer, packet()).KeyCollection
        SyncLock received_packet_store_blocker
            toret = received_packet_store.Keys
        End SyncLock
        Return toret
    End Function

    Function received_packet_store_get_values() As Dictionary(Of Integer, packet()).ValueCollection
        Dim toret As Dictionary(Of Integer, packet()).ValueCollection
        SyncLock received_packet_store_blocker
            toret = received_packet_store.Values
        End SyncLock
        Return toret
    End Function

    Sub received_packet_store_clear()
        SyncLock received_packet_store_blocker
            received_packet_store.Clear()
        End SyncLock
    End Sub

    Sub thread_c_dat_add(im As Integer, c As Integer)
        SyncLock thread_c_dat_blocker
            If thread_c_dat.ContainsKey(im) Then
                thread_c_dat(im) = c
            Else
                thread_c_dat.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub thread_c_dat_remove(im As Integer)
        SyncLock thread_c_dat_blocker
            If thread_c_dat.ContainsKey(im) Then
                thread_c_dat.Remove(im)
            End If
        End SyncLock
    End Sub

    Function thread_c_dat_contains_key(im As Integer) As Boolean
        Dim toret As Boolean = False
        SyncLock thread_c_dat_blocker
            toret = thread_c_dat.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function thread_c_dat_get(im As Integer) As Integer
        Dim toret As Integer = Nothing
        SyncLock thread_c_dat_blocker
            If thread_c_dat.ContainsKey(im) Then
                toret = thread_c_dat(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function thread_c_dat_get_keys() As Dictionary(Of Integer, Integer).KeyCollection
        Dim toret As Dictionary(Of Integer, Integer).KeyCollection
        SyncLock thread_c_dat_blocker
            toret = thread_c_dat.Keys
        End SyncLock
        Return toret
    End Function

    Function thread_c_dat_get_values() As Dictionary(Of Integer, Integer).ValueCollection
        Dim toret As Dictionary(Of Integer, Integer).ValueCollection
        SyncLock thread_c_dat_blocker
            toret = thread_c_dat.Values
        End SyncLock
        Return toret
    End Function

    Sub thread_c_dat_clear()
        SyncLock thread_c_dat_blocker
            thread_c_dat.Clear()
        End SyncLock
    End Sub

    Sub thread_max_dat_add(im As Integer, c As Integer)
        SyncLock thread_max_dat_blocker
            If thread_max_dat.ContainsKey(im) Then
                thread_max_dat(im) = c
            Else
                thread_max_dat.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub thread_max_dat_remove(im As Integer)
        SyncLock thread_max_dat_blocker
            If thread_max_dat.ContainsKey(im) Then
                thread_max_dat.Remove(im)
            End If
        End SyncLock
    End Sub

    Function thread_max_dat_contains_key(im As Integer) As Boolean
        Dim toret As Boolean = False
        SyncLock thread_max_dat_blocker
            toret = thread_max_dat.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function thread_max_dat_get(im As Integer) As Integer
        Dim toret As Integer = Nothing
        SyncLock thread_max_dat_blocker
            If thread_max_dat.ContainsKey(im) Then
                toret = thread_max_dat(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function thread_max_dat_get_keys() As Dictionary(Of Integer, Integer).KeyCollection
        Dim toret As Dictionary(Of Integer, Integer).KeyCollection
        SyncLock thread_max_dat_blocker
            toret = thread_max_dat.Keys
        End SyncLock
        Return toret
    End Function

    Function thread_max_dat_get_values() As Dictionary(Of Integer, Integer).ValueCollection
        Dim toret As Dictionary(Of Integer, Integer).ValueCollection
        SyncLock thread_max_dat_blocker
            toret = thread_max_dat.Values
        End SyncLock
        Return toret
    End Function

    Sub thread_max_dat_clear()
        SyncLock thread_max_dat_blocker
            thread_max_dat.Clear()
        End SyncLock
    End Sub

    Sub thread_ready_add(im As Integer, c As Boolean)
        SyncLock thread_ready_blocker
            If thread_ready.ContainsKey(im) Then
                thread_ready(im) = c
            Else
                thread_ready.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub thread_ready_remove(im As Integer)
        SyncLock thread_ready_blocker
            If thread_ready.ContainsKey(im) Then
                thread_ready.Remove(im)
            End If
        End SyncLock
    End Sub

    Function thread_ready_contains_key(im As Integer) As Boolean
        Dim toret As Boolean = False
        SyncLock thread_ready_blocker
            toret = thread_ready.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function thread_ready_get(im As Integer) As Boolean
        Dim toret As Boolean = Nothing
        SyncLock thread_ready_blocker
            If thread_ready.ContainsKey(im) Then
                toret = thread_ready(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function thread_ready_get_keys() As Dictionary(Of Integer, Boolean).KeyCollection
        Dim toret As Dictionary(Of Integer, Boolean).KeyCollection
        SyncLock thread_ready_blocker
            toret = thread_ready.Keys
        End SyncLock
        Return toret
    End Function

    Function thread_ready_get_values() As Dictionary(Of Integer, Boolean).ValueCollection
        Dim toret As Dictionary(Of Integer, Boolean).ValueCollection
        SyncLock thread_ready_blocker
            toret = thread_ready.Values
        End SyncLock
        Return toret
    End Function

    Sub thread_ready_clear()
        SyncLock thread_ready_blocker
            thread_ready.Clear()
        End SyncLock
    End Sub
End Module
