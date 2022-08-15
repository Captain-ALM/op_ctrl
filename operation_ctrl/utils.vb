Imports System.Net
Imports System.IO
Imports captainalm.calmclientandserver
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Security.Principal
Imports System.Reflection

Friend Module utils
    Public Function check_if_ip_address(ip2chk As String) As Boolean
        Try
            IPAddress.Parse(ip2chk)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function convert_string_to_integer(str As String) As Integer
        Dim toret As Integer = 0
        Try
            toret = Integer.Parse(str)
        Catch ex As Exception
            toret = 0
        End Try
        Return toret
    End Function

    Public Function check_if_dir_exists(str As String) As Boolean
        Dim toret As Boolean = False

        If Directory.Exists(str) Then
            toret = True
        Else
            toret = False
        End If

        Return toret
    End Function

    Public Function check_server_up(address As String, port As Integer) As Boolean
        Return Client.CheckServer(IPAddress.Parse(address), port)
    End Function

    Public Function getNetworkAdapterIP(Optional adapternumber As Integer = 1) As IPAddress
        Dim list As List(Of IPAddress) = New List(Of IPAddress)()
        Dim allNetworkInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        For i As Integer = 0 To allNetworkInterfaces.Length - 1
            Dim networkInterface As NetworkInterface = allNetworkInterfaces(i)
            If networkInterface.OperationalStatus = OperationalStatus.Up AndAlso networkInterface.NetworkInterfaceType <> NetworkInterfaceType.Loopback Then
                For Each current As UnicastIPAddressInformation In networkInterface.GetIPProperties().UnicastAddresses
                    If current.Address.AddressFamily = AddressFamily.InterNetwork Then
                        list.Add(current.Address)
                    End If
                Next
            End If
        Next
        If list.Count = 0 Then
            Return IPAddress.None
        Else
            If adapternumber > list.Count Or adapternumber < 1 Then
                Return IPAddress.None
            Else
                Return list(adapternumber - 1)
            End If
        End If
    End Function

    Public Function user_data() As WindowsIdentity
        Return WindowsIdentity.GetCurrent
    End Function

    Public Function add_dll(name As String, bytes As Byte()) As Boolean
        Try
            Dim ass As Assembly = Assembly.Load(bytes)
            If Not dlls_loaded.ContainsKey(name) Then
                dlls_loaded.Add(name, ass)
                Return True
            End If
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Function add_dll(name As String, assem As Assembly) As Boolean
        Try
            Dim ass As Assembly = assem
            If Not dlls_loaded.ContainsKey(name) Then
                dlls_loaded.Add(name, ass)
                Return True
            End If
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Function split_packets(ByVal packet As packet, ByVal split_size As Integer) As packet()
        'Dim packet_str As String = captainalm.calmclientandserver.utils.packet2string(packet)
        Dim packet_str As String = ""
        If packet.hasobject Then
            packet_str = packet.referencenumber & ";" & convertobjecttostring(packet.sender) & ";" & convertobjecttostring(packet.receivers) & ";" & convertobjecttostring(packet.header) & ";" & convertobjecttostring(packet.hasobject) & ";" & convertobjecttostring(packet.objectdata(pass))
        Else
            packet_str = packet.referencenumber & ";" & convertobjecttostring(packet.sender) & ";" & convertobjecttostring(packet.receivers) & ";" & convertobjecttostring(packet.header) & ";" & convertobjecttostring(packet.hasobject) & ";" & convertobjecttostring(packet.stringdata(pass))
        End If
        Dim arrsize As Double = packet_str.Length / split_size
        Dim arrsiz As Integer = Int(Math.Ceiling(arrsize))
        Dim packet_data(arrsiz - 1) As String
        Dim cindex As Integer = 0
        For i As Integer = 0 To arrsiz - 1 Step 1
            If cindex + split_size > packet_str.Length Then
                packet_data(i) = packet_str.Substring(cindex)
            Else
                packet_data(i) = packet_str.Substring(cindex, split_size)
            End If
            cindex += split_size
        Next
        Dim packet_frame(arrsiz) As packet
        packet_frame(0) = New packet(packet.referencenumber, packet.sender, packet.receivers, "0;" & packet.header, arrsiz, New EncryptionParameter(EncryptionMethod.unicodease, pass))
        For i As Integer = 1 To arrsiz Step 1
            packet_frame(i) = New packet(packet.referencenumber, packet.sender, packet.receivers, i & ";" & packet.header, packet_data(i - 1), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        Next
        Return packet_frame
    End Function

    Public Function join_packets(ByVal packet_frame As packet()) As packet
        Dim count As Integer = packet_frame(0).stringdata(pass)
        Dim packet_txt As String = ""
        For i As Integer = 1 To count Step 1
            packet_txt = packet_txt + packet_frame(i).stringdata(pass)
        Next
        'Dim packet As packet = captainalm.calmclientandserver.utils.string2packet(packet_txt)
        Dim packet As New packet
        Dim split As String = ";"
        Dim pdat As String() = packet_txt.Split(split)
        Dim refnum As Integer = 0
        Dim s As String = ""
        Dim rec As New List(Of String)
        Dim h As String = ""
        Dim hasobj As Boolean = False
        Dim obj As encapsulation = Nothing
        Dim str As String = ""
        refnum = pdat(0)
        s = convertstringtoobject(pdat(1))
        rec = convertstringtoobject(pdat(2))
        h = convertstringtoobject(pdat(3))
        hasobj = convertstringtoobject(pdat(4))
        If hasobj Then
            obj = New encapsulation(convertstringtoobject(pdat(5)))
            packet = New packet(refnum, s, rec, h, obj, New EncryptionParameter(EncryptionMethod.unicodease, pass))
        Else
            str = convertstringtoobject(pdat(5))
            packet = New packet(refnum, s, rec, h, str, New EncryptionParameter(EncryptionMethod.unicodease, pass))
        End If
        Return packet
    End Function

    Public Function get_imports_from_comment(ByVal data As String) As List(Of String)
        Dim im As New List(Of String)
        Dim splitter As String = "|"
        Dim dat As String() = data.Substring(1, data.IndexOf(ControlChars.CrLf)).Split(splitter)
        For Each current As String In dat
            im.Add(current)
        Next
        Return im
    End Function
End Module

Friend Structure compiled_assembly
    Dim assembly As Assembly
    Dim stats As String
    Dim success As Boolean

    Sub New(ByVal assembl As Assembly)
        assembly = assembl
        stats = "[SUCCESS]"
        success = True
    End Sub

    Sub New(ByVal sta As String)
        stats = sta
        success = False
        assembly = Nothing
    End Sub
End Structure
