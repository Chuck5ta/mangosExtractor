Imports System.Data
Imports System.IO
Imports System.Text

Namespace Core
    Public Module Export2Csv
        'Public Property Finished() As Action(Of Integer)
        '    Get
        '        Return _mFinished
        '    End Get
        '    Set(value As Action(Of Integer))
        '        _mFinished = value
        '    End Set
        'End Property

        'Private _mFinished As Action(Of Integer)

        Public Function ExportCsv(ByRef filename As String, ByRef dbcDataTable As DataTable, ByRef sourceFolder As String) As Boolean
            Dim intMaxRows As Integer = dbcDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer '= DBCDataTable.Columns.Count() - 1
            If dbcDataTable.Columns.Count() > 0 Then
                '            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")
                Dim sqlWriter As New StreamWriter(filename & ".csv")

                intMaxCols = WriteCsvStructure(sqlWriter, dbcDataTable, Path.GetFileNameWithoutExtension(filename.Substring(0, filename.Length - 4)), sourceFolder)

                Try
                    ' Dim intCounterRows As Integer = (intMaxRows - 1)

                    For rows = 0 To intMaxRows - 1
                        Dim result As New StringBuilder()
                        Dim flds As Integer = 0

                        Try
                            For cols As Integer = 0 To intMaxCols
                                Dim thisColData As String
                                If Not IsDBNull(dbcDataTable.Rows(rows)(cols)) Then
                                    thisColData = dbcDataTable.Rows(rows)(cols)
                                Else
                                    thisColData = "1"
                                End If

                                Dim thisLastColData As String
                                If Not IsDBNull(dbcDataTable.Rows(intMaxRows)(cols)) Then
                                    thisLastColData = dbcDataTable.Rows(intMaxRows)(cols)
                                Else
                                    thisLastColData = "1"
                                End If

                                'if the data contains a fullstop . or comma , when wrap the field in quotes
                                If thisColData.Contains(".") = True Or thisColData.Contains(",") = True Then thisLastColData = "0"

                                Select Case thisLastColData
                                    Case "0" '"String"
                                        result.Append("""" & thisColData.ToString & """")
                                        Exit Select
                                    Case Else
                                        result.Append(thisColData)
                                End Select

                                If flds < intMaxCols Then
                                    result.Append(",")
                                End If

                                flds += 1
#If _MyType <> "Console" Then
                                Application.DoEvents()
#Else
                                Threading.Thread.Sleep(0)
#End If

                            Next
                        Catch ex As Exception
                            Alert(ex.Message & " - 1", AlertNewLine.ADD_CRLF)
                        End Try
                        sqlWriter.WriteLine(result)
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                    Next

                    sqlWriter.Flush()
                    sqlWriter.Close()

                    Return True
                Catch ex As Exception
                    Alert(ex.Message & " - 2", AlertNewLine.ADD_CRLF)
                    Return False
                End Try
            End If
            Return False
        End Function

        
        ''' <summary>
        '''     Generates the CSV header row
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Function WriteCsvStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String, ByRef sourceFolder As String) As Integer
            If data.Rows.Count() > 0 And data.Columns.Count() - 1 > 0 Then
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
                        If blnOverrideOk = False Then
                            sqlWriter.Write("""" & data.Columns(i).ColumnName & """")
                        Else
                            sqlWriter.Write("""" & columnNameOverride(i).ToString & """")
                        End If
                    Catch ex As Exception
                        Alert("Error in XML Def for " & tablename & ", column " & i, AlertNewLine.ADD_CRLF)
                    End Try

                    If i < maxCols Then sqlWriter.Write(",")
                Next
                sqlWriter.WriteLine(" ")
                Return maxCols
            End If
            Return 0
        End Function
    End Module
End Namespace
