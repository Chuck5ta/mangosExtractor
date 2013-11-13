'Imports System.ComponentModel.Composition
Imports System.Data
Imports System.IO

Namespace Core
    Public Module Export2Md
        Public Property Finished() As Action(Of Integer)
            Get
                Return m_Finished
            End Get
            Set(value As Action(Of Integer))
                m_Finished = value
            End Set
        End Property

        Private m_Finished As Action(Of Integer)


        Public Function ExportMd(ByRef sourceFolder As String, ByRef filename As String, ByRef dbcDataTable As DataTable) As Boolean
            Dim sqlWriter As New StreamWriter(Path.GetDirectoryName(filename) & "\dbc_" & Path.GetFileNameWithoutExtension(filename) & "_" & ReturnMangosCoreVersion & ".md")

            WriteMDStructure(sqlWriter, DBCDataTable, Path.GetFileNameWithoutExtension(filename.Substring(0, filename.Length - 4)), sourceFolder)

            Try
                sqlWriter.Flush()
                sqlWriter.Close()

                Return True
            Catch ex As Exception
                Alert(ex.Message & " - 2", AlertNewLine.ADD_CRLF)
                Return False
            End Try
        End Function

        
        ''' <summary>
        '''     Generates the Wiki MD File header page
        ''' </summary>
        ''' <param name="sqlWriter"></param>
        ''' <param name="data"></param>
        ''' <param name="tablename"></param>
        ''' <remarks></remarks>
        Private Sub WriteMdStructure(sqlWriter As StreamWriter, data As DataTable, tablename As String, ByRef sourceFolder As String)
            sqlWriter.WriteLine("Back to [Known DBC Files](" & MDPageLink(tablename) & ") summary")
            sqlWriter.WriteLine("")
            sqlWriter.WriteLine("----------")
            sqlWriter.Write("Other Versions: ")
            sqlWriter.Write(" [**MaNGOSZero**](dbc_" & tablename & "_MaNGOSZero) ")
            sqlWriter.Write(" [**MaNGOSOne**](dbc_" & tablename & "_MaNGOSOne) ")
            sqlWriter.Write(" [**MaNGOSTwo**](dbc_" & tablename & "_MaNGOSTwo) ")
            sqlWriter.Write(" [**MaNGOSThree**](dbc_" & tablename & "_MaNGOSThree) ")
            sqlWriter.WriteLine(" [**MaNGOSFour**](dbc_" & tablename & "_MaNGOSFour) ")
            sqlWriter.WriteLine("")
            sqlWriter.WriteLine("----------")
            sqlWriter.WriteLine("##### Description of the DBC file " & tablename & " for v" & FullVersion & " (Build " & BuildNo & ")")
            sqlWriter.WriteLine("")
            sqlWriter.WriteLine("<p>The purpose of this file needs to be documented</p>")
            sqlWriter.WriteLine("")

            If data.Rows.Count() > 0 And data.Columns.Count() - 1 > 0 Then
                sqlWriter.WriteLine("##### The Field definitions follow, No. of columns: {0}", data.Columns.Count())
                Dim strDataType As String = ""
                Dim blnOverrideOk As Boolean = False
                Dim columnNameOverride As New Dictionary(Of Integer, String)
                Dim maxCols As Integer = 0
                columnNameOverride = LoadXmlDefinitions(sourceFolder, tablename)
                If columnNameOverride.Count() <= data.Columns.Count() And columnNameOverride.Count() > 0 Then blnOverrideOk = True
                maxCols = columnNameOverride.Count() - 1
                If blnOverrideOk = False Then maxCols = data.Columns.Count() - 1
                sqlWriter.WriteLine("<table border='1' cellpadding='5' cellspacing='0'>")
                sqlWriter.WriteLine("<tr bgcolor='#dedede'>")
                sqlWriter.WriteLine("<th>Name</th>")
                sqlWriter.WriteLine("<th>Type</th>")
                sqlWriter.WriteLine("<th>Include</th>")
                sqlWriter.WriteLine("<th>Comments</th>")
                sqlWriter.WriteLine("</tr>")
                For i As Integer = 0 To maxCols
                    Try
                        If i > maxCols Then Exit For
                        sqlWriter.WriteLine("<tr>")
                        If blnOverrideOk = False Then
                            sqlWriter.WriteLine("<td>{0}</td>", data.Columns(i).ColumnName)
                        Else
                            sqlWriter.WriteLine("<td>{0}</td>", columnNameOverride(i).ToString)
                        End If

                        If Not IsDBNull(data.Rows(data.Rows.Count - 1)(i)) Then
                            strDataType = data.Rows(data.Rows.Count - 1)(i)
                        Else
                            strDataType = "1"
                        End If

                        Select Case strDataType
                            Case "2" '"Int64"
                                sqlWriter.WriteLine("<td align='center'>BIGINT</td>")
                                Exit Select
                            Case "1" '"Int32"
                                sqlWriter.WriteLine("<td align='center'>INT</td>")
                                Exit Select
                            Case "3" '"Single"
                                sqlWriter.WriteLine("<td align='center'>FLOAT</td>")
                                Exit Select
                            Case "0" '"String"
                                sqlWriter.WriteLine("<td align='center'>TEXT</td>")
                                Exit Select
                            Case Else
                                sqlWriter.WriteLine("<td align='center'>INT</td>")
                                Exit Select
                        End Select
                        sqlWriter.WriteLine("<td align='center'>Y</td>")
                        sqlWriter.WriteLine("<td align='center'>&nbsp;</td>")
                        sqlWriter.WriteLine("</tr>")
                    Catch ex As Exception
                        Alert("Error in XML Def for " & tablename & ", column " & i, AlertNewLine.ADD_CRLF)
                    End Try
                Next
                sqlWriter.WriteLine("</table>")
                sqlWriter.WriteLine("###### auto-generated by MaNGOSExtractor")
                sqlWriter.WriteLine("")
                sqlWriter.WriteLine("--------")
                sqlWriter.WriteLine("Provided by [getMaNGOS - The home of MaNGOs and the MaNGOS community](http://www.getmangos.com 'getMaNGOS - The home of MaNGOs and the MaNGOS community')")
            End If
            sqlWriter.WriteLine()
        End Sub


        Private Function MdPageLink(ByVal filename As String) As String
            Dim retString As String = ""
            If InStr("ABCDEFGHabcdefgh", filename.Substring(0, 1)) > 0 Then
                'If filename.Substring(0, 1).Contains("ABCEEFGH") Then
                retString = "Dbc-files"
            ElseIf InStr("IJKLMNOPQRijklmnopqr", filename.Substring(0, 1)) > 0 Then
                retString = "Dbc-files2"
            Else
                retString = "Dbc-files3"
            End If

            Return retString
        End Function
    End Module
End Namespace
