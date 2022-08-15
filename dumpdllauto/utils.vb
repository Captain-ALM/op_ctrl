Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.DirectoryServices
Imports System.DirectoryServices.AccountManagement
Imports System.DirectoryServices.ActiveDirectory

Friend Module utils
    Public WTS_CURRENT_SERVER_HANDLE As IntPtr = IntPtr.Zero

    Public Function convertobjectargstostringargs(args As Object, Optional limit As Integer = 0) As String()
        Dim toret(0) As String
        If IsNothing(args) Then
            Return Nothing
        End If
        Try
            Dim numofi As Integer = numberofindexes(args, limit)
            ReDim toret(numofi - 1)
            For i As Integer = 0 To numofi - 1 Step 1
                toret(i) = args(i).ToString
            Next
        Catch ex As Exception
            Return Nothing
        End Try
        Return toret
    End Function

    Public Function numberofindexes(args As Object, Optional limit As Integer = 0) As Integer
        If IsNothing(args) Then
            Return 0
        End If
        Try
            Dim i As Integer = 0
            Try
                If limit = 0 Then
                    While True
                        Dim tmp As Object = args(i)
                        tmp = Nothing
                        i = i + 1
                    End While
                Else
                    While i <= limit
                        Dim tmp As Object = args(i)
                        tmp = Nothing
                        i = i + 1
                    End While
                End If
                Return i
            Catch ex As Exception
                Return i
            End Try
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Function castInt(str As String) As Integer
        Try
            Return Integer.Parse(str)
        Catch ex As InvalidCastException
            Return 0
        Catch ex As OverflowException
            Return 0
        Catch ex As ArithmeticException
            Return 0
        Catch ex As ArgumentException
            Return 0
        End Try
    End Function

    Function getdatetime() As String
        Return Date.Now.Year & "-" & Date.Now.Month & "-" & Date.Now.Day & "-" & Date.Now.Hour & "-" & Date.Now.Minute & "-" & Date.Now.Second & "-" & Date.Now.Millisecond
    End Function

    Function getlsass(Optional indx As Integer = 0) As Integer
        Return Process.GetProcessesByName("lsass")(indx).Id
    End Function

    <DllImport("dbghelp.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Public Function MiniDumpWriteDump(ByVal hProcess As IntPtr, _
                                             ByVal ProcessId As Int32, _
                                             ByVal hFile As IntPtr, _
                                             ByVal DumpType As MINIDUMP_TYPE, _
                                             ByVal ExceptionParam As IntPtr, _
                                             ByVal UserStreamParam As IntPtr, _
                                             ByVal CallackParam As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("wtsapi32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Public Sub WTSFreeMemory(ByVal pMemory As IntPtr)
    End Sub

    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Public Function CloseHandle(ByVal hMemory As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("wtsapi32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Public Function WTSEnumerateSessionsA(ByVal hServer As IntPtr, _
                                             <MarshalAs(UnmanagedType.U4)>
                                             ByVal Reserved As Int32, _
                                             <MarshalAs(UnmanagedType.U4)>
                                             ByVal Version As Int32, _
                                             ByRef ppSessionInfo As IntPtr, _
                                             <MarshalAs(UnmanagedType.U4)>
                                             ByRef pCount As Int32) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("wtsapi32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Public Function WTSQueryUserToken(<MarshalAs(UnmanagedType.U4)> ByVal SessionId As UInt32, _
                                            ByRef phToken As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("advapi32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)> _
    Public Function GetTokenInformation(ByVal TokenHandle As IntPtr, _
                                        ByVal TokenInformationClass As _TOKEN_INFORMATION_CLASS, _
                                        ByVal TokenInformation As IntPtr, _
                                        <MarshalAs(UnmanagedType.U4)> ByVal TokenInformationLength As System.UInt32, _
                                        <MarshalAs(UnmanagedType.U4)> ByRef ReturnLength As System.UInt32) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    Public Function getSessions() As WTS_SESSION_INFOA()
        Dim ppsi As IntPtr = IntPtr.Zero
        Dim cnt As Int32 = 0
        If Not WTSEnumerateSessionsA(WTS_CURRENT_SERVER_HANDLE, 0, 1, ppsi, cnt) Then
            main.adderr("WTSEnumerateSessionsA", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Dim sessionInfo() As WTS_SESSION_INFOA = New WTS_SESSION_INFOA(cnt - 1) {}
        Dim session_ptr As System.IntPtr = IntPtr.Zero
        Dim wsi As New WTS_SESSION_INFOA
        For i As Integer = 0 To cnt - 1
            session_ptr = New IntPtr(ppsi.ToInt32() + (i * Marshal.SizeOf(wsi)))
            sessionInfo(i) = CType(Marshal.PtrToStructure(session_ptr, GetType(WTS_SESSION_INFOA)), WTS_SESSION_INFOA)
        Next
        WTSFreeMemory(ppsi)
        Return sessionInfo
    End Function

    Public Function getUtokenFSession(session As WTS_SESSION_INFOA) As IntPtr
        Dim ut As IntPtr = IntPtr.Zero
        If Not WTSQueryUserToken(session.SessionID, ut) Then
            main.adderr("WTSQueryUserToken", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return ut
    End Function

    Public Function isAdministratorSID(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isAdministratorSID(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isAdministratorSID(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim wi As New WindowsIdentity(ut)
        Return New WindowsPrincipal(wi).IsInRole(WindowsBuiltInRole.Administrator) And Not wi.IsSystem
    End Function

    Public Function isLocalSID(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isLocalSID(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isLocalSID(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim wi As New WindowsIdentity(ut)
        Dim wp As WindowsPrincipal = New WindowsPrincipal(wi)
        Return wp.IsInRole(New SecurityIdentifier(Security.Principal.WellKnownSidType.BuiltinUsersSid, Nothing)) And wp.IsInRole(WindowsBuiltInRole.User)
    End Function

    Public Function isTokenAdministratorSID(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isTokenAdministratorSID(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isTokenAdministratorSID(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        If Environment.OSVersion.Platform <> PlatformID.Win32NT Then Return False
        If Environment.OSVersion.Version.Major < 6 Then Return False
        Dim tokenInfLength As Int32 = Marshal.SizeOf(GetType(Int32))
        Dim tokenInformation As IntPtr = Marshal.AllocHGlobal(tokenInfLength)
        If Not GetTokenInformation(ut, _TOKEN_INFORMATION_CLASS.TokenElevationType, tokenInformation, tokenInfLength, tokenInfLength) Then
            main.adderr("GetTokenInformation", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Dim toret As Boolean = False
        If tokenInformation <> IntPtr.Zero Then
            Dim elevationType As TokenElevationType = CType(Marshal.ReadInt32(tokenInformation), TokenElevationType)
            toret = (elevationType = TokenElevationType.TokenElevationTypeLimited Or elevationType = TokenElevationType.TokenElevationTypeFull)
            Marshal.FreeHGlobal(tokenInformation)
        End If
        Return toret
    End Function

    Public Function isDomainSID(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isDomainSID(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isDomainSID(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim wi As New WindowsIdentity(ut)
        If wi.User.AccountDomainSid Is Nothing Then Return False
        Return New WindowsPrincipal(wi).IsInRole(New SecurityIdentifier(WellKnownSidType.AccountDomainUsersSid, wi.User.AccountDomainSid))
    End Function

    Public Function isDomainAdministratorSID(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isDomainAdministratorSID(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isDomainAdministratorSID(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim wi As New WindowsIdentity(ut)
        If wi.User.AccountDomainSid Is Nothing Then Return False
        Dim wp As WindowsPrincipal = New WindowsPrincipal(wi)
        Return wp.IsInRole(New SecurityIdentifier(WellKnownSidType.AccountDomainAdminsSid, wi.User.AccountDomainSid)) Or wp.IsInRole(New SecurityIdentifier(WellKnownSidType.AccountAdministratorSid, wi.User.AccountDomainSid)) Or wp.IsInRole(New SecurityIdentifier(WellKnownSidType.AccountEnterpriseAdminsSid, wi.User.AccountDomainSid))
    End Function

    Public Function isLocalAD(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isLocalAD(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isLocalAD(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim ctx As PrincipalContext = New PrincipalContext(ContextType.Machine)
        Dim wi As New WindowsIdentity(ut)
        Dim toret As Boolean = False
        Try
            Dim prnc As UserPrincipal = UserPrincipal.FindByIdentity(ctx, wi.Name)
            If prnc Is Nothing Then toret = False Else toret = True
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
            toret = False
            main.adderr(ex)
        End Try
        Return toret
    End Function

    Public Function isDomainAD(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isDomainAD(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isDomainAD(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim ctx As PrincipalContext = getPCDomain()
        Dim wi As New WindowsIdentity(ut)
        Dim toret As Boolean = False
        Try
            If ctx Is Nothing Then Throw New NullReferenceException("PrincipalContext For Domain is Null.")
            Dim prnc As UserPrincipal = UserPrincipal.FindByIdentity(ctx, wi.Name)
            If prnc Is Nothing Then toret = False Else toret = True
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
            toret = False
            main.adderr(ex)
        End Try
        Return toret
    End Function

    Public Function isAdministratorAD(session As WTS_SESSION_INFOA) As Boolean
        Dim ut As IntPtr = getUtokenFSession(session)
        If ut = IntPtr.Zero Then Return False
        Dim toret As Boolean = isAdministratorAD(ut)
        If Not CloseHandle(ut) Then
            main.adderr("CloseHandle", Marshal.GetLastWin32Error, Marshal.GetHRForLastWin32Error, New StackTrace().ToString())
        End If
        Return toret
    End Function

    Public Function isAdministratorAD(ut As IntPtr) As Boolean
        If ut = IntPtr.Zero Then Return False
        Dim ctx As PrincipalContext = getPCDomain()
        Dim wi As New WindowsIdentity(ut)
        If ctx Is Nothing Then ctx = New PrincipalContext(ContextType.Machine)
        Dim prnc As UserPrincipal = UserPrincipal.FindByIdentity(ctx, wi.Name)
        Dim toret As Boolean = False
        For Each sr As Principal In prnc.GetAuthorizationGroups()
            If sr.Sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) Then toret = True
            If sr.Sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid) Then toret = True
            If sr.Sid.IsWellKnown(WellKnownSidType.AccountDomainAdminsSid) Then toret = True
            If sr.Sid.IsWellKnown(WellKnownSidType.AccountEnterpriseAdminsSid) Then toret = True
            If toret = True Then Exit For
        Next
        Return toret
    End Function

    Public Function getPCDomain() As PrincipalContext
        Dim ctx As PrincipalContext = Nothing
        Try
            Domain.GetComputerDomain()
            ctx = New PrincipalContext(ContextType.Domain, System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties.DomainName)
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
            ctx = Nothing
            main.adderr(ex)
        End Try
        Return ctx
    End Function

    Public Enum MINIDUMP_TYPE
        DumpNormal = 0
        DumpWithDataSegs = 1
        DumpWithFullMemory = 2
        DumpWithHandleData = 4
        DumpFilterMemory = 8
        DumpScanMemory = 10
        DumpWithUnloadedModules = 20
        DumpWithIndirectlyReferencedMemory = 40
        DumpFilterModulePaths = 80
        DumpWithProcessThreadData = 100
        DumpWithPrivateReadWriteMemory = 200
        DumpWithoutOptionalData = 400
        DumpWithFullMemoryInfo = 800
        DumpWithThreadInfo = 1000
        DumpWithCodeSegs = 2000
    End Enum

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)> _
    Public Structure WTS_SESSION_INFOA
        Dim SessionID As UInt32
        Dim pWinStationName As String
        Dim State As WTS_CONNECTSTATE_CLASS
    End Structure

    Public Enum WTS_CONNECTSTATE_CLASS
        WTSActive
        WTSConnected
        WTSConnectQuery
        WTSShadow
        WTSDisconnected
        WTSIdle
        WTSListen
        WTSReset
        WTSDown
        WTSInit
    End Enum

    Public Enum _TOKEN_INFORMATION_CLASS
        TokenUser = 1
        TokenGroups
        TokenPrivileges
        TokenOwner
        TokenPrimaryGroup
        TokenDefaultDacl
        TokenSource
        TokenType
        TokenImpersonationLevel
        TokenStatistics
        TokenRestrictedSids
        TokenSessionId
        TokenGroupsAndPrivileges
        TokenSessionReference
        TokenSandBoxInert
        TokenAuditPolicy
        TokenOrigin
        TokenElevationType
        TokenLinkedToken
        TokenElevation
        TokenHasRestrictions
        TokenAccessInformation
        TokenVirtualizationAllowed
        TokenVirtualizationEnabled
        TokenIntegrityLevel
        TokenUIAccess
        TokenMandatoryPolicy
        TokenLogonSid
        TokenIsAppContainer
        TokenCapabilities
        TokenAppContainerSid
        TokenAppContainerNumber
        TokenUserClaimAttributes
        TokenDeviceClaimAttributes
        TokenRestrictedUserClaimAttributes
        TokenRestrictedDeviceClaimAttributes
        TokenDeviceGroups
        TokenRestrictedDeviceGroups
        TokenSecurityAttributes
        TokenIsRestricted
        TokenProcessTrustLevel
        TokenPrivateNameSpace
        TokenSingletonAttributes
        TokenBnoIsolation
        TokenChildProcessFlags
        MaxTokenInfoClass
        TokenIsLessPrivilegedAppContainer
    End Enum

    Public Enum TokenElevationType
        TokenElevationTypeDefault = 1
        TokenElevationTypeFull
        TokenElevationTypeLimited
    End Enum
End Module
