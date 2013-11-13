Imports System.Data
Imports System.IO

Namespace Core
    Public Module Export2H
        'Public Property Finished() As Action(Of Integer)
        '    Get
        '        Return _mFinished
        '    End Get
        '    Set(value As Action(Of Integer))
        '        _mFinished = value
        '    End Set
        'End Property

        'Private _mFinished As Action(Of Integer)

        Public Function ExportH(ByRef filename As String, ByRef dbcDataTable As DataTable, ByRef sourceFolder As String) As Boolean
            If dbcDataTable.Columns.Count() > 0 Then
                Dim pathname As String = filename.Substring(0, filename.Length - 4).Replace("\\", "\")
                Dim sqlWriter As New StreamWriter(pathname.Replace("_", "") & "Entryfmt.h")
                Dim sqlWriter2 As New StreamWriter(pathname & "Entry.h")
                WriteHStructure(sqlWriter, sqlWriter2, dbcDataTable, Path.GetFileNameWithoutExtension(pathname), sourceFolder)

                Try
                    sqlWriter.Flush()
                    sqlWriter.Close()

                    sqlWriter2.Flush()
                    sqlWriter2.Close()

                    Return True
                Catch ex As Exception
                    Alert(ex.Message & " - 2", AlertNewLine.ADD_CRLF)
                    Return False
                End Try
            End If
            Return False
        End Function


        ''' <summary>
        '''     Generates the SQL File header to drop the existing table and recreate it
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Function WriteHStructure(sqlWriter As StreamWriter, sqlWriter2 As StreamWriter, data As DataTable, tablename As String, ByRef sourceFolder As String) As Integer
            sqlWriter.Write("const char {0}[]= " & Chr(34), tablename & "Entryfmt")
            sqlWriter2.WriteLine("struct " & tablename & "Entry")
            sqlWriter2.WriteLine("{")

            'Needs to write out files in this format:
            'const char Achievementfmt[]="niiissiiiiisii";

            If data.Rows.Count() > 0 And data.Columns.Count() - 1 > 0 Then
                Dim strDataType As String = ""
                Dim blnOverrideOk As Boolean = False
                Dim columnNameOverride As New Dictionary(Of Integer, String)
                Dim maxCols As Integer = 0
                columnNameOverride = LoadXmlDefinitions(sourceFolder, tablename)
                If columnNameOverride.Count() <= data.Columns.Count() And columnNameOverride.Count() > 0 Then blnOverrideOk = True
                maxCols = columnNameOverride.Count() - 1
                If blnOverrideOk = False Then maxCols = data.Columns.Count() - 1
                For i As Integer = 0 To maxCols
                    Try
                        If i > maxCols Then Exit For
                        If Not IsDBNull(data.Rows(data.Rows.Count - 1)(i)) Then
                            strDataType = data.Rows(data.Rows.Count - 1)(i)
                        Else
                            strDataType = "1"
                        End If

                        Select Case strDataType
                            Case "2" '"Int64"
                                sqlWriter.Write("i")
                                sqlWriter2.Write(vbTab & "uint64" & vbTab)
                                Exit Select
                            Case "1" '"Int32"
                                sqlWriter.Write("i")
                                sqlWriter2.Write(vbTab & "uint32" & vbTab)
                                Exit Select
                            Case "3" '"Single"
                                sqlWriter.Write("f")
                                sqlWriter2.Write(vbTab & "float" & vbTab)
                                Exit Select
                            Case "0" '"String"
                                sqlWriter.Write("s")
                                sqlWriter2.Write(vbTab & "DBCString" & vbTab)
                                Exit Select
                            Case Else
                                sqlWriter.Write("x")
                                Exit Select
                                'Alert("Unknown field type " & data.Columns(i).DataType.Name & "!", Core.AlertNewLine.NoCRLF)
                        End Select

                        If blnOverrideOk = False Then
                            sqlWriter2.Write([String].Format("{0}", data.Columns(i).ColumnName))
                            sqlWriter2.WriteLine(";")
                        Else
                            sqlWriter2.Write([String].Format("{0}", columnNameOverride(i).ToString))
                            sqlWriter2.WriteLine(";")
                        End If
                    Catch ex As Exception
                        Alert("Error in XML Def for " & tablename & ", column " & i, AlertNewLine.ADD_CRLF)
                    End Try

                Next
                sqlWriter.WriteLine(Chr(34) & ";")
                sqlWriter2.WriteLine("}")
                Return maxCols
            End If
            sqlWriter.WriteLine()
            Return 0
        End Function
    End Module
End Namespace
