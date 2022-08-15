Imports captainalm.calmclientandserver
Imports System.Security.Principal
Imports System.Reflection
Imports Microsoft.CSharp
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler
Imports System.Threading

Friend Module controller
    Friend queue_reset As Boolean = False

    Friend thread_registry As New Dictionary(Of String, Thread)

    Private thread_registry_blocker As New Object()

    Private thread_management_blocker As New Object()

    Private dlls_loaded_blocker As New Object()

    Sub exec()

        'While receive_packet_queue.Count > 0
        '    Dim p As packet = receive_packet_queue.Dequeue()
        '    If p.header.StartsWith("0;") Then
        '        Dim packet_frame(Int(p.stringdata(pass))) As packet
        '        packet_frame(0) = p
        '        If received_packet_store.ContainsKey(p.referencenumber) Then
        '            received_packet_store(p.referencenumber) = packet_frame
        '        Else
        '            received_packet_store.Add(p.referencenumber, packet_frame)
        '        End If
        '    Else
        '        If received_packet_store.ContainsKey(p.referencenumber) Then
        '            Dim index_str As String = p.header.Substring(0, p.header.IndexOf(";"))
        '            Dim index As Integer = Int(index_str)
        '            Dim packet_frame As packet() = received_packet_store(p.referencenumber)
        '            packet_frame(index) = p
        '            received_packet_store(p.referencenumber) = packet_frame
        '            If index = Int(packet_frame(0).stringdata(pass)) Then
        '                addpacket(join_packets(received_packet_store(p.referencenumber)))
        '                received_packet_store.Remove(p.referencenumber)
        '            End If
        '        End If
        '    End If
        'End While

        While getcount() > 0
            Dim cpacket As packet = getpacket(0)
            reg_thread(cpacket.referencenumber & ":" & cpacket.header, cpacket)
            removepacket(0)
        End While

        Try
            For Each str As String In thread_registry_get_keys()
                Dim thr As Thread = thread_registry_get(str)
                If Not thr.IsAlive Then
                    unreg_thread(str)
                End If
                Thread.Sleep(10)
            Next
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try

        If queue_reset Then
            disconnect()
            queue_reset = False
        End If
    End Sub

    Sub reg_thread(ByVal name As String, ByVal p As packet)
        SyncLock thread_management_blocker
            Dim th As Thread = Nothing
            If thread_registry_contains_key(name) Then
                If thread_registry_get(name).IsAlive Then
                    Dim ct As Thread = thread_registry_get(name)
                    Dim cname As String = name
                    Dim i As Integer = 0
                    Dim bool As Boolean = ct.IsAlive
                    While bool
                        If thread_registry_contains_key(cname) Then
                            ct = thread_registry_get(cname)
                            If Not ct.IsAlive Then
                                bool = False
                                Exit While
                            Else
                                bool = True
                            End If
                            cname = name & i
                            i += 1
                        Else
                            bool = False
                            Exit While
                        End If
                        th = New Thread(New ThreadStart(Sub() threadparser(cname, p)))
                        th.IsBackground = True
                        thread_registry_add(cname, th)
                        th.Start()
                    End While
                Else
                    th = New Thread(New ThreadStart(Sub() threadparser(name, p)))
                    th.IsBackground = True
                    thread_registry_add(name, th)
                    th.Start()
                End If
            Else
                th = New Thread(New ThreadStart(Sub() threadparser(name, p)))
                th.IsBackground = True
                thread_registry_add(name, th)
                th.Start()
            End If
        End SyncLock
    End Sub

    Sub unreg_thread(ByVal name As String)
        SyncLock thread_management_blocker
            thread_registry_remove(name)
        End SyncLock
    End Sub

    Sub threadparser(ByVal name As String, ByVal cpacket As packet)
        Dim msg_sent As Boolean = False
        Try
            If cpacket.header.StartsWith("dll:") Then
                Dim nom As String = cpacket.header.Substring(cpacket.header.IndexOf(":") + 1)
                If add_dll(nom, cpacket.objectdata(pass)) Then
                    send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "DLL Added [OK]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                Else
                    send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "DLL Not Added [FAIL]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                End If
            ElseIf cpacket.header.StartsWith("int:") Then
                Dim cmd As String = cpacket.header.Substring(cpacket.header.IndexOf(":") + 1)
                send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", internal_cmd(cmd, cpacket.stringdata(pass), name), New EncryptionParameter(EncryptionMethod.unicodease, pass)))
            ElseIf cpacket.header.StartsWith("cmd:") Then
                send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", dll_cmd(cpacket.header.Substring(cpacket.header.IndexOf(":") + 1), cpacket.objectdata(pass)), New EncryptionParameter(EncryptionMethod.unicodease, pass)))
            ElseIf cpacket.header = "req" Then
                client_name = cpacket.stringdata(pass)
                send_intal_data(cpacket.referencenumber)
            ElseIf cpacket.header.StartsWith("compcs:") Then
                Dim nom As String = cpacket.header.Substring(cpacket.header.IndexOf(":") + 1)
                Dim cc As compiled_assembly = compile_cs(cpacket.stringdata(pass))
                If cc.success Then
                    If add_dll(nom, cc.assembly) Then
                        send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "C# Added [OK]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                    Else
                        send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "C# Not Added [FAIL]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                    End If
                Else
                    send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", cc.stats, New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                End If
            ElseIf cpacket.header.StartsWith("compvb:") Then
                Dim nom As String = cpacket.header.Substring(cpacket.header.IndexOf(":") + 1)
                Dim cc As compiled_assembly = compile_vb(cpacket.stringdata(pass))
                If cc.success Then
                    If add_dll(nom, cc.assembly) Then
                        send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "VB Added [OK]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                    Else
                        send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "VB Not Added [FAIL]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                    End If
                Else
                    send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", cc.stats, New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                End If
            Else
                send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
            End If
            msg_sent = True
        Catch ex As ThreadAbortException
            If Not msg_sent Then
                send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "Thread Aborted [OK]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
            End If
            Thread.CurrentThread.Abort()
        Catch ex As Exception
        End Try
        Thread.CurrentThread.Abort()
    End Sub

    Function compile_cs(ByVal data As String) As compiled_assembly
        Try
            Dim cc As CodeDomProvider = New CSharpCodeProvider()
            Dim cp As New CompilerParameters()
            cp.GenerateInMemory = True
            For Each c As String In get_imports_from_comment(data)
                cp.ReferencedAssemblies.Add(c)
            Next
            Dim cr As CompilerResults = cc.CompileAssemblyFromSource(cp, data)
            If cr.Errors.HasErrors OrElse cr.Errors.HasWarnings Then
                Return New compiled_assembly("Compilation Failed! [FAIL]")
            Else
                Return New compiled_assembly(cr.CompiledAssembly)
            End If
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
            Return New compiled_assembly("Thread Aborted! [FAIL]")
        Catch ex As Exception
            Return New compiled_assembly("Exception: " & ex.GetType().ToString & " : " & ex.Message & " [FAIL]")
        End Try
    End Function

    Function compile_vb(ByVal data As String) As compiled_assembly
        Try
            Dim cc As CodeDomProvider = New VBCodeProvider()
            Dim cp As New CompilerParameters()
            cp.GenerateInMemory = True
            For Each c As String In get_imports_from_comment(data)
                cp.ReferencedAssemblies.Add(c)
            Next
            Dim cr As CompilerResults = cc.CompileAssemblyFromSource(cp, data)
            If cr.Errors.HasErrors OrElse cr.Errors.HasWarnings Then
                Return New compiled_assembly("Compilation Failed! [FAIL]")
            Else
                Return New compiled_assembly(cr.CompiledAssembly)
            End If
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
            Return New compiled_assembly("Thread Aborted! [FAIL]")
        Catch ex As Exception
            Return New compiled_assembly("Exception: " & ex.GetType().ToString & " : " & ex.Message & " [FAIL]")
        End Try
    End Function

    Function dll_cmd(cmd As String, args As Object) As String
        Try
            Dim dll_nom As String = cmd.Substring(0, cmd.IndexOf("."))
            Dim dll_instance_nom As String = cmd.Substring(cmd.IndexOf(".") + 1, (cmd.LastIndexOf(".") - 1) - cmd.IndexOf("."))
            Dim func_name As String = cmd.Substring(cmd.LastIndexOf(".") + 1)
            If dlls_loaded_contains_key(dll_nom) Then
                Dim dll_instance As Object = dlls_loaded_get(dll_nom).CreateInstance(dll_instance_nom)
                Dim dll_instance_type As Type = dll_instance.GetType()
                Dim func_info As MethodInfo = dll_instance_type.GetMethod(func_name, BindingFlags.Public Or BindingFlags.Instance)
                Try
                    Return func_info.Invoke(dll_instance, New Object() {args}).ToString & " [OK]"
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                    Return "Thread Aborted! [FAIL]"
                Catch ex As Exception
                    func_info.Invoke(dll_instance, New Object() {args})
                End Try
                Return "[OK]"
            Else
                Return "Requested DLL Not Loaded [FAIL]"
            End If
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
            Return "Thread Aborted! [FAIL]"
        Catch ex As Exception
            Return "Exception: " & ex.GetType().ToString & " : " & ex.Message & " [FAIL]"
        End Try
    End Function

    Function internal_cmd(cmd As String, args As String, name As String) As String
        If cmd = "stop" Or cmd = "exit" Or cmd = "end" Then
            queue_reset = True
            to_restart = False
            to_exit = True
        ElseIf cmd = "restart" Then
            queue_reset = True
            restart_args = args
            to_restart = True
            to_exit = True
        ElseIf cmd = "reset" Then
            queue_reset = True
        ElseIf cmd = "dlls" Then
            Dim toret As String = "[DLLS]" & ControlChars.CrLf
            For Each Val As String In dlls_loaded_get_keys()
                toret = toret & Val & ControlChars.CrLf
            Next
            toret = toret & "Internal Message Received [DONE]"
            Return toret
        ElseIf cmd = "dll" Then
            Dim toret As String = "[DLL]" & ControlChars.CrLf
            If dlls_loaded_contains_key(args) Then
                toret = toret & "Dynamic Link Library" & args & " does exist."
            Else
                toret = toret & "Dynamic Link Library" & args & " does not exist."
            End If
            toret = toret & ControlChars.CrLf & "Internal Message Received [DONE]"
            Return toret
        ElseIf cmd = "cdlls" Then
            dlls_loaded_clear()
        ElseIf cmd = "rdll" Then
            If dlls_loaded_contains_key(args) Then
                dlls_loaded_remove(args)
            End If
        ElseIf cmd = "servstp" Then
            Return "Internal Message Invalid for Non Service [FAIL]"
        ElseIf cmd = "threads" Then
            Dim toret As String = "[THREADS]" & ControlChars.CrLf
            Try
                For Each Val As String In thread_registry_get_keys()
                    If thread_registry_get(Val).IsAlive Then
                        toret = toret & Val & ControlChars.CrLf
                    End If
                Next
            Catch ex As ThreadAbortException
                Thread.CurrentThread.Abort()
            Catch ex As Exception
            End Try
            toret = toret & "Internal Message Received [DONE]"
            Return toret
        ElseIf cmd = "sthread" Then
            If thread_registry_contains_key(args) Then
                Dim tmp_t As Thread = thread_registry_get(args)
                If tmp_t.IsAlive Then
                    tmp_t.Abort()
                Else
                    unreg_thread(args)
                End If
            End If
        ElseIf cmd = "sthreads" Then
            Try
                For Each Val As String In thread_registry_get_keys()
                    If Not Val = name Then
                        Dim tmp_t As Thread = thread_registry_get(Val)
                        If tmp_t.IsAlive Then
                            tmp_t.Abort()
                        Else
                            unreg_thread(Val)
                        End If
                    End If
                Next
            Catch ex As ThreadAbortException
                Thread.CurrentThread.Abort()
            Catch ex As Exception
            End Try
        End If
        Return "Internal Message Received [DONE]"
    End Function

    Sub send_intal_data(Optional ref As Integer = 0)
        send_packet(New packet(ref, client_name, New List(Of String), "client_data", "ip:" & ip_address & ";user:" & user_data().Name & ";sid:" & user_data().User.Value & ";admin:" & New WindowsPrincipal(user_data()).IsInRole(WindowsBuiltInRole.Administrator).ToString, New EncryptionParameter(EncryptionMethod.unicodease, pass)))
    End Sub

    Sub thread_registry_add(im As String, c As Thread)
        SyncLock thread_registry_blocker
            If thread_registry.ContainsKey(im) Then
                thread_registry(im) = c
            Else
                thread_registry.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub thread_registry_remove(im As String)
        SyncLock thread_registry_blocker
            If thread_registry.ContainsKey(im) Then
                thread_registry.Remove(im)
            End If
        End SyncLock
    End Sub

    Function thread_registry_contains_key(im As String) As Boolean
        Dim toret As Boolean = False
        SyncLock thread_registry_blocker
            toret = thread_registry.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function thread_registry_get(im As String) As Thread
        Dim toret As Thread = Nothing
        SyncLock thread_registry_blocker
            If thread_registry.ContainsKey(im) Then
                toret = thread_registry(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function thread_registry_get_keys() As Dictionary(Of String, Thread).KeyCollection
        Dim toret As Dictionary(Of String, Thread).KeyCollection
        SyncLock thread_registry_blocker
            toret = thread_registry.Keys
        End SyncLock
        Return toret
    End Function

    Function thread_registry_get_values() As Dictionary(Of String, Thread).ValueCollection
        Dim toret As Dictionary(Of String, Thread).ValueCollection
        SyncLock thread_registry_blocker
            toret = thread_registry.Values
        End SyncLock
        Return toret
    End Function

    Sub thread_registry_clear()
        SyncLock thread_registry_blocker
            thread_registry.Clear()
        End SyncLock
    End Sub

    Sub dlls_loaded_add(im As String, c As Assembly)
        SyncLock dlls_loaded_blocker
            If dlls_loaded.ContainsKey(im) Then
                dlls_loaded(im) = c
            Else
                dlls_loaded.Add(im, c)
            End If
        End SyncLock
    End Sub

    Sub dlls_loaded_remove(im As String)
        SyncLock dlls_loaded_blocker
            If dlls_loaded.ContainsKey(im) Then
                dlls_loaded.Remove(im)
            End If
        End SyncLock
    End Sub

    Function dlls_loaded_contains_key(im As String) As Boolean
        Dim toret As Boolean = False
        SyncLock dlls_loaded_blocker
            toret = dlls_loaded.ContainsKey(im)
        End SyncLock
        Return toret
    End Function

    Function dlls_loaded_get(im As String) As Assembly
        Dim toret As Assembly = Nothing
        SyncLock dlls_loaded_blocker
            If dlls_loaded.ContainsKey(im) Then
                toret = dlls_loaded(im)
            End If
        End SyncLock
        Return toret
    End Function

    Function dlls_loaded_get_keys() As Dictionary(Of String, Assembly).KeyCollection
        Dim toret As Dictionary(Of String, Assembly).KeyCollection
        SyncLock dlls_loaded_blocker
            toret = dlls_loaded.Keys
        End SyncLock
        Return toret
    End Function

    Function dlls_loaded_get_values() As Dictionary(Of String, Assembly).ValueCollection
        Dim toret As Dictionary(Of String, Assembly).ValueCollection
        SyncLock dlls_loaded_blocker
            toret = dlls_loaded.Values
        End SyncLock
        Return toret
    End Function

    Sub dlls_loaded_clear()
        SyncLock dlls_loaded_blocker
            dlls_loaded.Clear()
        End SyncLock
    End Sub
End Module
