Imports System.Net.NetworkInformation
Imports System.Net
Imports System.Net.Sockets
Imports captainalm.calmclientandserver

Module utils
    Public Function getNetworkAdapterIP(Optional adapternumber As Integer = 1) As IPAddress
        Dim list As List(Of IPAddress) = New List(Of IPAddress)()
        Dim allNetworkInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        For i As Integer = 0 To allNetworkInterfaces.Length - 1
            Dim networkInterface As NetworkInterface = allNetworkInterfaces(i)
            'If networkInterface.OperationalStatus = OperationalStatus.Up AndAlso networkInterface.NetworkInterfaceType <> NetworkInterfaceType.Loopback Then
            'the above is if we want no loopback
            If networkInterface.OperationalStatus = OperationalStatus.Up Then
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

    Public Function getNetworkAdapterIPsAndNames() As Dictionary(Of String, IPAddress)
        Dim list As New Dictionary(Of String, IPAddress)
        Dim allNetworkInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        For i As Integer = 0 To allNetworkInterfaces.Length - 1
            Dim networkInterface As NetworkInterface = allNetworkInterfaces(i)
            'If networkInterface.OperationalStatus = OperationalStatus.Up AndAlso networkInterface.NetworkInterfaceType <> NetworkInterfaceType.Loopback Then
            'the above is if we want no loopback
            If networkInterface.OperationalStatus = OperationalStatus.Up Then
                For Each current As UnicastIPAddressInformation In networkInterface.GetIPProperties().UnicastAddresses
                    If current.Address.AddressFamily = AddressFamily.InterNetwork Then
                        list.Add(networkInterface.Name & " : " & current.Address.ToString, current.Address)
                    End If
                Next
            End If
        Next
        Return list
    End Function

    Public Function parseInteger(str As String) As Integer
        Dim toret As Integer = 0
        Try
            toret = Integer.Parse(str)
        Catch ex As Exception
            toret = 0
        End Try
        Return toret
    End Function

    Public Function convert_int_cmd_int_to_name(cmd As internal_command_name) As String
        If cmd = internal_command_name.Stop Then
            Return "stop"
        ElseIf cmd = internal_command_name.Restart Then
            Return "restart"
        ElseIf cmd = internal_command_name.Reset Then
            Return "reset"
        ElseIf cmd = internal_command_name.Exit Then
            Return "exit"
        ElseIf cmd = internal_command_name.End Then
            Return "end"
        ElseIf cmd = internal_command_name.dll Then
            Return "dll"
        ElseIf cmd = internal_command_name.dlls Then
            Return "dlls"
        ElseIf cmd = internal_command_name.rdll Then
            Return "rdll"
        ElseIf cmd = internal_command_name.cdlls Then
            Return "cdlls"
        ElseIf cmd = internal_command_name.servstp Then
            Return "servstp"
        End If
        Return ""
    End Function
End Module

Module packetmanipulator
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
End Module

Public Structure message_struct
    Public client_uuid As String
    Public reference_number As Integer
    Public client_message As String
    Public server_message As String
    Public pending_reception As Boolean
    Public has_obj As Boolean
    Public server_msg_header As String
    Public client_msg_header As String

    Public Sub New(ByVal refi As Integer, ByVal clientuuid As String, ByVal smsg As String, ByVal hobj As Boolean, ByVal smh As String)
        reference_number = refi
        client_uuid = clientuuid
        server_message = smsg
        client_message = "Pending Reception..."
        client_msg_header = ""
        has_obj = hobj
        server_msg_header = smh
        pending_reception = True
    End Sub

    Public Sub New(ByVal refi As Integer, ByVal clientuuid As String, ByVal smsg As String, ByVal cmsg As String, ByVal hobj As Boolean, ByVal smh As String, ByVal cmh As String)
        reference_number = refi
        client_uuid = clientuuid
        server_message = smsg
        client_message = cmsg
        client_msg_header = cmh
        has_obj = hobj
        server_msg_header = smh
        pending_reception = False
    End Sub

    Public Sub New(ByVal msgstr As message_struct, ByVal cmsg As String, ByVal cmh As String)
        reference_number = msgstr.reference_number
        client_uuid = msgstr.client_uuid
        server_message = msgstr.server_message
        client_message = cmsg
        client_msg_header = cmh
        has_obj = msgstr.has_obj
        server_msg_header = msgstr.server_msg_header
        pending_reception = False
    End Sub
End Structure

Public Structure client_data_struct
    Public name As String
    Public user_name As String
    Public sid As String
    Public isadmin As String
    Public ip As String
    Public valid As Boolean

    Public Sub New(ByVal clnom As String)
        name = clnom
        ip = ""
        user_name = ""
        sid = ""
        isadmin = ""
        valid = False
    End Sub

    Public Sub New(ByVal clnom As String, ByVal data As String)
        name = clnom
        Dim seperator As String = ";"
        Dim dataload As String() = data.Split(seperator.ToCharArray)
        For Each current As String In dataload
            If current.StartsWith("ip:") Then
                ip = current.Substring(3)
            ElseIf current.StartsWith("user:") Then
                user_name = current.Substring(5)
            ElseIf current.StartsWith("sid:") Then
                sid = current.Substring(4)
            ElseIf current.StartsWith("admin:") Then
                isadmin = current.Substring(6)
            End If
        Next
        valid = True
    End Sub
End Structure

Public Interface Ipacket_data
    Function make_packet(pdp As packet_data_param) As packet
    Function make_packet_with_refnum_0(pdp As packet_data_param) As packet
    Function get_receivers() As List(Of String)
    Function get_data() As Object
    Function is_string() As Boolean
    Function get_header() As String
End Interface

Public Structure packet_data
    Implements Ipacket_data

    Public receivers As List(Of String)
    Public data As Object
    Public header As String
    Public is_string_data As Boolean

    Public Sub New(ByVal rec As List(Of String), ByVal head As String, ByVal dat As String)
        receivers = rec
        header = head
        data = dat
        is_string_data = True
    End Sub

    Public Sub New(ByVal rec As List(Of String), ByVal head As String, ByVal dat As encapsulation)
        receivers = rec
        header = head
        data = dat.GetObject()
        is_string_data = False
    End Sub

    Public Function make_packet(pdp As packet_data_param) As packet Implements Ipacket_data.make_packet
        If is_string_data Then
            Return New packet(generate_ref_number(), "", receivers, header, CType(data, String), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        Else
            Return New packet(generate_ref_number(), "", receivers, header, New encapsulation(data), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        End If
    End Function

    Public Function make_packet_with_refnum_0(pdp As packet_data_param) As packet Implements Ipacket_data.make_packet_with_refnum_0
        If is_string_data Then
            Return New packet(0, "", receivers, header, CType(data, String), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        Else
            Return New packet(0, "", receivers, header, New encapsulation(data), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        End If
    End Function

    Public Function get_receivers() As List(Of String) Implements Ipacket_data.get_receivers
        Return receivers
    End Function

    Public Function is_string() As Boolean Implements Ipacket_data.is_string
        Return is_string_data
    End Function

    Public Function get_data() As Object Implements Ipacket_data.get_data
        Return data
    End Function

    Public Function get_header() As String Implements Ipacket_data.get_header
        Return header
    End Function
End Structure

Public Structure packet_data_rsnd
    Implements Ipacket_data

    Public receivers As List(Of String)
    Public data As Object
    Public header As String
    Public is_string_data As Boolean
    Public refnumber As Integer

    Public Sub New(ByVal rn As Integer, ByVal rec As List(Of String), ByVal head As String, ByVal dat As String)
        refnumber = rn
        receivers = rec
        header = head
        data = dat
        is_string_data = True
    End Sub

    Public Sub New(ByVal rn As Integer, ByVal rec As List(Of String), ByVal head As String, ByVal dat As encapsulation)
        refnumber = rn
        receivers = rec
        header = head
        data = dat.GetObject()
        is_string_data = False
    End Sub

    Public Function make_packet(pdp As packet_data_param) As packet Implements Ipacket_data.make_packet
        If is_string_data Then
            Return New packet(refnumber, "", receivers, header, CType(data, String), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        Else
            Return New packet(refnumber, "", receivers, header, New encapsulation(data), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        End If
    End Function

    Public Function make_packet_with_refnum_0(pdp As packet_data_param) As packet Implements Ipacket_data.make_packet_with_refnum_0
        If is_string_data Then
            Return New packet(0, "", receivers, header, CType(data, String), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        Else
            Return New packet(0, "", receivers, header, New encapsulation(data), New EncryptionParameter(EncryptionMethod.unicodease, pass))
        End If
    End Function

    Public Function get_receivers() As List(Of String) Implements Ipacket_data.get_receivers
        Return receivers
    End Function

    Public Function is_string() As Boolean Implements Ipacket_data.is_string
        Return is_string_data
    End Function

    Public Function get_data() As Object Implements Ipacket_data.get_data
        Return data
    End Function

    Public Function get_header() As String Implements Ipacket_data.get_header
        Return header
    End Function
End Structure

Public Structure packet_data_req
    Implements Ipacket_data

    Public receivers As List(Of String)
    Public data As Object
    Public header As String
    Public is_string_data As Boolean

    Sub New(rec As List(Of String))
        data = ""
        is_string_data = True
        header = "req"
        receivers = rec
    End Sub

    Public Function make_packet(client_name As packet_data_param) As packet Implements Ipacket_data.make_packet
        Return New packet(generate_ref_number(), "", receivers, header, client_name.heldparam, New EncryptionParameter(EncryptionMethod.unicodease, pass))
    End Function

    Public Function make_packet_with_refnum_0(client_name As packet_data_param) As packet Implements Ipacket_data.make_packet_with_refnum_0
        Return New packet(0, "", receivers, header, client_name.heldparam, New EncryptionParameter(EncryptionMethod.unicodease, pass))
    End Function

    Public Function get_receivers() As List(Of String) Implements Ipacket_data.get_receivers
        Return receivers
    End Function

    Public Function is_string() As Boolean Implements Ipacket_data.is_string
        Return True
    End Function

    Public Function get_data() As Object Implements Ipacket_data.get_data
        Return data
    End Function

    Public Function get_header() As String Implements Ipacket_data.get_header
        Return header
    End Function
End Structure

Public Structure packet_data_param
    Dim heldparam As String
    Sub New(clientname As String)
        heldparam = clientname
    End Sub
End Structure

Public Enum internal_command_name As Integer
    [Stop] = 0
    Restart = 1
    Reset = 2
    [Exit] = 3
    [End] = 4
    dll = 5
    dlls = 6
    rdll = 7
    cdlls = 8
    servstp = 9
    none = -1
End Enum
