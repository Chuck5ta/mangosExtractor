Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Text
Imports System.Runtime.InteropServices

Namespace Blizzard
    Public Module Extensions
        Sub New()
        End Sub

        <Extension>
        Public Function FourCc(data As Byte()) As String
            Return Encoding.ASCII.GetString(data)
        End Function

        <Extension>
        Public Function Compare(data As Byte(), otherData As Byte()) As Boolean
            If data.Length <> otherData.Length Then
                Return False
            End If

            For i As Integer = 0 To data.Length - 1
                If data(i) <> otherData(i) Then
                    Return False
                End If
            Next

            Return True
        End Function

        <Extension>
        Public Sub SetBytes(data As Byte(), newData As Byte(), offset As Integer)
            For i As Integer = 0 To newData.Length - 1
                data(offset + i) = newData(i)
            Next
        End Sub

        <Extension>
        Public Function ReadStruct (Of T As Structure)(reader As BinaryReader) As T
            Dim rawData As Byte() = reader.ReadBytes(Marshal.SizeOf(GetType(T)))
            Dim handle As GCHandle = GCHandle.Alloc(rawData, GCHandleType.Pinned)
            Dim returnObject As T = DirectCast(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(T)), T)
            handle.Free()
            Return returnObject
        End Function

        <Extension>
        Public Function Remaining(reader As BinaryReader) As Long
            Return reader.BaseStream.Length - reader.BaseStream.Position
        End Function

        <Extension>
        Public Function ReadRemaining(reader As BinaryReader) As Byte()
            Return reader.ReadBytes(CInt(reader.Remaining()))
        End Function

        <Extension>
        Public Function ToBinaryReader(data As Byte()) As BinaryReader
            Return New BinaryReader(New MemoryStream(data))
        End Function

        <Extension>
        Public Function ToHexString(byteArray As Byte()) As String
            Dim retStr As String = ""
            For Each b As Byte In byteArray
                retStr += b.ToString("X2")
            Next
            Return retStr
        End Function
    End Module
End Namespace
