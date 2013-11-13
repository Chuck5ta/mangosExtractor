Imports System.Data
Imports System.IO
Imports System.Text
Imports System.Globalization

Namespace Core
    Public Module Export2SQL
        'Public Property Finished() As Action(Of Integer)
        '    Get
        '        Return _mFinished
        '    End Get
        '    Set(value As Action(Of Integer))
        '        _mFinished = value
        '    End Set
        'End Property

        'Private _mFinished As Action(Of Integer)

        Public Function ExportSql(ByRef filename As String, ByRef dbcDataTable As DataTable, ByRef sourceFolder As String) As Boolean
            Dim intMaxRows As Integer = DBCDataTable.Rows.Count() - 1
            Dim intMaxCols As Integer '= DBCDataTable.Columns.Count() - 1
            If DBCDataTable.Columns.Count() > 0 Then
                '            Dim sqlWriter As New StreamWriter(Filename.Substring(0, Filename.Length - 4) & ".dbc.sql")
                Dim sqlWriter As New StreamWriter(filename & ".sql")

                intMaxCols = WriteSqlStructure(sqlWriter, DBCDataTable, Path.GetFileNameWithoutExtension(filename.Substring(0, filename.Length - 4)), sourceFolder)

                Try
                    ' Dim intCounterRows As Integer = (intMaxRows - 1)

                    For rows = 0 To intMaxRows - 1
                        Dim result As New StringBuilder()
                        result.AppendFormat("INSERT INTO `dbc_{0}` VALUES (", Path.GetFileNameWithoutExtension(filename))

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

                                'Last row contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
                                Select Case thisLastColData
                                    Case "2" '"Long"
                                        result.Append(thisColData)
                                        Exit Select
                                    Case "1" '"Int32"
                                        result.Append(thisColData)
                                        Exit Select
                                    Case "3" '"Single", "Float"
                                        result.Append(CSng(thisColData).ToString(CultureInfo.InvariantCulture))
                                        Exit Select
                                    Case "0" '"String"
                                        result.Append("""" & StripBadCharacters(thisColData.ToString) & """")
                                        Exit Select
                                    Case Else
                                        Alert([String].Format("Unknown field type {0}!", thisColData), AlertNewLine.ADD_CRLF)
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

                        result.Append(");")
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
        '''     Generates the SQL File header to drop the existing table and recreate it
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Function WriteSqlStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String, ByRef sourceFolder As String) As Integer
            sqlWriter.WriteLine("DROP TABLE IF EXISTS `dbc_{0}`;", tablename)

            If data.Rows.Count() > 0 And data.Columns.Count() - 1 > 0 Then
                sqlWriter.WriteLine("CREATE TABLE `dbc_{0}` (", tablename)
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
                        If blnOverrideOk = False Then
                            sqlWriter.Write(vbTab & [String].Format("`{0}`", data.Columns(i).ColumnName))
                        Else
                            sqlWriter.Write(vbTab & [String].Format("`{0}`", columnNameOverride(i).ToString))
                        End If
                    Catch ex As Exception
                        Alert("Error in XML Def for " & tablename & ", column " & i, AlertNewLine.ADD_CRLF)
                    End Try

                    If Not IsDBNull(data.Rows(data.Rows.Count - 1)(i)) Then
                        strDataType = data.Rows(data.Rows.Count - 1)(i)
                    Else
                        strDataType = "1"
                    End If

                    Select Case strDataType
                        Case "2" '"Int64"
                            sqlWriter.Write(" BIGINT NOT NULL DEFAULT '0'")
                            Exit Select
                        Case "1" '"Int32"
                            sqlWriter.Write(" INT NOT NULL DEFAULT '0'")
                            Exit Select
                        Case "3" '"Single"
                            sqlWriter.Write(" FLOAT NOT NULL DEFAULT '0'")
                            Exit Select
                        Case "0" '"String"
                            sqlWriter.Write(" TEXT NOT NULL")
                            Exit Select
                        Case Else
                            sqlWriter.Write(" INT NOT NULL DEFAULT '0'")
                            Exit Select
                            'Alert("Unknown field type " & data.Columns(i).DataType.Name & "!", Core.AlertNewLine.NoCRLF)
                    End Select

                    If i < maxCols Then sqlWriter.WriteLine(",")
                Next

                For Each index As DataColumn In data.PrimaryKey
                    sqlWriter.WriteLine(vbTab & "PRIMARY KEY (`{0}`)", index.ColumnName)
                Next

                sqlWriter.WriteLine(")")
                sqlWriter.WriteLine(" ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci COMMENT='Export of {0}';", tablename)
                sqlWriter.WriteLine(" SET NAMES UTF8;")

                sqlWriter.WriteLine()

                Return maxCols
            End If
            Return 0
        End Function
    End Module
End Namespace
