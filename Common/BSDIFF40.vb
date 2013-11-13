Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Namespace Blizzard
    Public Class Patch
        Implements IDisposable
        Private _mPtch As PTCH
        Private _mMd5 As MD5
        Private m_XFRM As XFRM
        Private _mBsdiff40 As BSDIFF40
        '        Private m_WDBC As WDBC

        ' BSD0
        Private m_unpackedSize As UInteger
        Private m_compressedDiff As Byte()

        ' BSDIFF40
        Private _mCtrlBlock As Byte(), _mDiffBlock As Byte(), _mExtraBlock As Byte()

        Private m_type As String

        Public Sub New(patchFile As String)
            Using fs As New FileStream(patchFile, FileMode.Open, FileAccess.Read)
                Using br As New BinaryReader(fs)
                    _mPtch = br.ReadStruct (Of Ptch)()
                    Debug.Assert(_mPtch.m_magic.FourCc() = "PTCH")

                    _mMd5 = br.ReadStruct (Of MD5)()
                    Debug.Assert(_mMd5.m_magic.FourCc() = "MD5_")

                    m_XFRM = br.ReadStruct (Of XFRM)()
                    Debug.Assert(m_XFRM.m_magic.FourCc() = "XFRM")

                    m_type = m_XFRM.m_type.FourCc()

                    Select Case m_type
                        Case "BSD0"
                            m_unpackedSize = br.ReadUInt32()
                            m_compressedDiff = br.ReadRemaining()
                            BsdiffParse()
                            Exit Select
                        Case "COPY"
                            m_compressedDiff = br.ReadRemaining()
                            Return
                        Case Else
                            Debug.Assert(False, [String].Format("Unknown patch type: {0}", m_type))
                            Exit Select
                    End Select
                End Using
            End Using
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' TODO
        End Sub

        Public Sub PrintHeaders(ByRef filename As String)
            Core.Alert("        Patching: " & filename, Core.AlertNewLine.ADD_CRLF) '& " Size=" & m_PTCH.m_patchSize & ", Before=" & m_PTCH.m_sizeBefore & ", After=" & m_PTCH.m_sizeAfter, Core.runningAsGui)
        End Sub

        Private Sub BsdiffParseHeader(br As BinaryReader)
            _mBsdiff40 = br.ReadStruct (Of BSDIFF40)()

            Debug.Assert(_mBsdiff40.m_magic.FourCC() = "BSDIFF40")

            Debug.Assert(_mBsdiff40.m_ctrlBlockSize > 0 AndAlso _mBsdiff40.m_diffBlockSize > 0)

            Debug.Assert(_mBsdiff40.m_sizeAfter = _mPtch.m_sizeAfter)
        End Sub

        Private Sub BsdiffParse()
            Dim diff() As Byte = RLEUnpack()

            Using ms As New MemoryStream(diff)
                Using br As New BinaryReader(ms)
                    BSDIFFParseHeader(br)

                    _mCtrlBlock = br.ReadBytes(CInt(_mBsdiff40.m_ctrlBlockSize))
                    _mDiffBlock = br.ReadBytes(CInt(_mBsdiff40.m_diffBlockSize))
                    _mExtraBlock = br.ReadRemaining()
                End Using
            End Using
        End Sub

        Private Function RLEUnpack() As Byte()
            Dim ret As New List(Of Byte)()

            Using ms As New MemoryStream(m_compressedDiff)
                Using br As New BinaryReader(ms, Encoding.ASCII)
                    While br.PeekChar() >= 0
                        Dim b As Byte = br.ReadByte()
                        If (b And &H80) <> 0 Then
                            ret.AddRange(br.ReadBytes((b And &H7F) + 1))
                        Else
                            ret.AddRange(New Byte(b) {})
                        End If
                    End While
                End Using
            End Using

            Debug.Assert(ret.Count = m_unpackedSize)

            Return ret.ToArray()
        End Function

        Public Sub Apply(oldFileName As String, newFileName As String, validate As Boolean)
            If m_type = "COPY" Then
                File.WriteAllBytes(newFileName, m_compressedDiff)
                Return
            End If

            Dim oldFile As Byte() = File.ReadAllBytes(oldFileName)

            If validate Then
                ' pre-validate
                Debug.Assert(oldFile.Length = _mPtch.m_sizeBefore)
                Dim md5 As New MD5CryptoServiceProvider()
                Dim hash() As Byte = md5.ComputeHash(oldFile)

                Try
                    hash.Compare(_mMd5.m_md5Before)
                Catch
                    Core.Alert("Input MD5 mismatch!: " & hash.Compare(_mMd5.m_md5Before), Core.AlertNewLine.ADD_CRLF)
                End Try
            End If

            Dim ctrlBlock As BinaryReader = _mCtrlBlock.ToBinaryReader()
            Dim diffBlock As BinaryReader = _mDiffBlock.ToBinaryReader()
            Dim extraBlock As BinaryReader = _mExtraBlock.ToBinaryReader()

            Dim newFile As Byte() = New Byte(_mPtch.m_sizeAfter - 1) {}

            Dim newFileOffset As Integer = 0, oldFileOffset As Integer = 0

            While newFileOffset < _mPtch.m_sizeAfter
                Dim diffChunkSize As Integer = ctrlBlock.ReadInt32()

                Dim extraChunkSize As Integer = ctrlBlock.ReadInt32()
                Dim extraOffset As UInteger = ctrlBlock.ReadUInt32()

                Debug.Assert(newFileOffset + diffChunkSize <= _mPtch.m_sizeAfter)

                newFile.SetBytes(diffBlock.ReadBytes(diffChunkSize), newFileOffset)

                For i As Integer = 0 To diffChunkSize - 1
                    If (oldFileOffset + i >= 0) AndAlso (oldFileOffset + i < _mPtch.m_sizeBefore) Then
                        Dim nb As UInt32 = newFile(newFileOffset + i)
                        Dim ob As UInt32 = oldFile(oldFileOffset + i)
                        newFile(newFileOffset + i) = CByte((nb + ob) Mod 256)
                    End If
                Next

                newFileOffset += diffChunkSize
                oldFileOffset += diffChunkSize

                Debug.Assert(newFileOffset + extraChunkSize <= _mPtch.m_sizeAfter)

                newFile.SetBytes(extraBlock.ReadBytes(extraChunkSize), newFileOffset)

                newFileOffset += extraChunkSize
                oldFileOffset += CInt(xsign(extraOffset))
            End While

            ctrlBlock.Close()
            diffBlock.Close()
            extraBlock.Close()

            If validate Then
                ' post-validate
                Debug.Assert(newFile.Length = _mPtch.m_sizeAfter)
                Dim md5 As New MD5CryptoServiceProvider()
                Dim hash() As Byte = md5.ComputeHash(newFile)

                Try
                    hash.Compare(_mMd5.m_md5After)
                Catch
                    Core.Alert("Output MD5 mismatch!: " & hash.Compare(_mMd5.m_md5After), Core.AlertNewLine.ADD_CRLF)
                End Try
            End If


            File.WriteAllBytes(newFileName, newFile)
        End Sub

        Private Shared Function Xsign(i As UInteger) As Long
            If (i And &H80000000UI) <> 0 Then
                Return (CLng(&H80000000UI) - i)
            End If
            Return i
        End Function
    End Class
End Namespace