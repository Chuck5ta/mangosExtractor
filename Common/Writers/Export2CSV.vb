Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Data
Imports System.Linq.Expressions

Namespace Core
    Public Module Export2CSV
        Public Property Finished() As Action(Of Integer)
            Get
                Return m_Finished
            End Get
            Set(value As Action(Of Integer))
                m_Finished = value
            End Set
        End Property

        Private m_Finished As Action(Of Integer)


        Public Function exportCSV(ByRef Filename As String, ByRef DBCDataTable As DataTable,
                                  ByRef sourceFolder As String) As Boolean
            Dim intMaxRows As Integer = DBCDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer '= DBCDataTable.Columns.Count() - 1

            '            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")
            Dim sqlWriter As New StreamWriter(Filename & ".csv")
            intMaxCols = WriteCsvStructure(sqlWriter, DBCDataTable,
                              Path.GetFileNameWithoutExtension(Filename.Substring(0, Filename.Length - 4)), sourceFolder) - 1

            Try
                Dim intCounterRows As Integer = (intMaxRows - 1)

                For rows = 0 To intMaxRows - 1
                    Dim result As New StringBuilder()
                    Dim flds As Integer = 0

                    Try
                        For cols As Integer = 0 To intMaxCols
                            Dim thisColData As String
                            If Not IsDBNull(DBCDataTable.Rows(rows)(cols)) Then
                                thisColData = DBCDataTable.Rows(rows)(cols)
                            Else
                                thisColData = "1"
                            End If

                            Dim thisLastColData As String
                            If Not IsDBNull(DBCDataTable.Rows(intMaxRows)(cols)) Then
                                thisLastColData = DBCDataTable.Rows(intMaxRows)(cols)
                            Else
                                thisLastColData = "1"
                            End If

                            'if the data contains a fullstop . or comma , when wrap the field in quotes
                            If thisColData.Contains(".") = True Or thisColData.Contains(",") = True Then _
                                thisLastColData = "0"

                            Select Case thisLastColData
                                Case "0" '"String"
                                    result.Append("""" & DirectCast(thisColData.ToString, String) & """")
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
                            Thread.Sleep(0)
#End If

                        Next
                    Catch ex As Exception
                        Alert(ex.Message & " - 1", AlertNewLine.AddCRLF)
                    End Try
                    sqlWriter.WriteLine(result)
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                    Thread.Sleep(0)
#End If

                Next

                sqlWriter.Flush()
                sqlWriter.Close()

                Return True
            Catch ex As Exception
                Alert(ex.Message & " - 2", AlertNewLine.AddCRLF)
                Return False
            End Try
        End Function


        ''' <summary>
        '''     Generates the SQL File header to drop the existing table and recreate it
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Function WriteCsvStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String,
                                      ByRef sourceFolder As String) As Integer
            Dim retColCount As Integer
            Dim blnOverrideOk As Boolean = False
            Dim ColumnNameOverride As New Dictionary(Of Integer, String)
            ColumnNameOverride = LoadXMLDefinitions(sourceFolder, tablename)
            If ColumnNameOverride.Count() <= data.Columns.Count() Then blnOverrideOk = True
            For i As Integer = 0 To ColumnNameOverride.Count() - 1
                Try
                    If i > data.Columns.Count() Then Exit For
                    If blnOverrideOk = False Then
                        retColCount = data.Columns.Count()
                        sqlWriter.Write("""" & data.Columns(i).ColumnName & """")
                    Else
                        retColCount = ColumnNameOverride.Count()
                        sqlWriter.Write("""" & ColumnNameOverride(i).ToString & """")
                    End If
                Catch ex As Exception
                    Alert("Error in XML Def for " & tablename & ", column " & i, AlertNewLine.AddCRLF)
                End Try
                If i < data.Columns.Count - 1 Then sqlWriter.Write(",")
            Next
            sqlWriter.WriteLine(" ")
            Return retColCount
        End Function
    End Module
End Namespace
