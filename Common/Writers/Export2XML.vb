Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Reflection

Namespace Core
    Public Module Export2Xml
        'Public Property Finished() As Action(Of Integer)
        '    Get
        '        Return _mFinished
        '    End Get
        '    Set(value As Action(Of Integer))
        '        _mFinished = value
        '    End Set
        'End Property

        'Private _mFinished As Action(Of Integer)

        Public Function SaveXml(ByRef sourceFolder As String) As Boolean
            Dim myXmlDoc As New XmlDocument()
            Dim xmlFilename As String = ""
            xmlFilename = ReturnDbcxmLfilename()

            If My.Computer.FileSystem.FileExists(sourceFolder & "\" & xmlFilename) = True Then
                myXmlDoc.Load(sourceFolder & "\" & xmlFilename)
                'Alert(" External XML Definitions used", Core.AlertNewLine.AddCRLF)
            Else
                Dim textStreamReader As StreamReader
                Dim assembly As Assembly
                Try
                    assembly = [assembly].GetExecutingAssembly()
                    textStreamReader = New StreamReader(assembly.GetManifestResourceStream(assembly.GetName.Name.ToString() & "." & xmlFilename))
                    myXmlDoc.Load(textStreamReader)
                    myXmlDoc.Save(sourceFolder & "\" & xmlFilename) '"D:\wtt\TEST.XML")
                Catch ex As Exception
                End Try
                'Else
                '    Stop
                '    'TODO: Need to create base structure here
            End If
            Return True
        End Function


        ''' <summary>
        '''     Export data to the XML file
        ''' </summary>
        ''' <param name="sourceFolder"></param>
        ''' <param name="tableName"></param>
        ''' <param name="dbcDataTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExportXml(ByRef sourceFolder As String, ByRef tableName As String, ByRef dbcDataTable As DataTable) As Boolean
            Dim myXmlDoc As New XmlDocument()
            Dim xmlFilename As String = ""
            xmlFilename = ReturnDbcxmLfilename()

            If My.Computer.FileSystem.FileExists(sourceFolder & "\" & xmlFilename) = True Then
                myXmlDoc.Load(sourceFolder & "\" & xmlFilename)
                'Alert(" External XML Definitions used", Core.AlertNewLine.AddCRLF)
            Else
                Dim textStreamReader As StreamReader
                Dim assembly As Assembly
                Try
                    assembly = [assembly].GetExecutingAssembly()
                    textStreamReader = New StreamReader(assembly.GetManifestResourceStream(assembly.GetName.Name.ToString() & "." & xmlFilename))
                    myXmlDoc.Load(textStreamReader)
                    'Alert(" Internal XML Definitions used", Core.AlertNewLine.AddCRLF)
                Catch ex As Exception
                End Try
                'Else
                '    Stop
                '    'TODO: Need to create base structure here
            End If

            WriteXmlStructure(myXmlDoc, dbcDataTable, Path.GetFileNameWithoutExtension(tableName.Substring(0, tableName.Length - 4)), sourceFolder & "\" & xmlFilename)

            Return True
        End Function


        ''' <summary>
        '''     Generates the SQL File header to drop the existing table and recreate it
        ''' </summary>
        ''' <param name="myXml">My XML.</param>
        ''' <param name="data">The data.</param>
        ''' <param name="tablename">The tablename.</param>
        ''' <param name="xmlFilename">The XML filename.</param>
        ''' <returns></returns>
        Private Function WriteXmlStructure(myXml As XmlDocument, data As DataTable, tablename As String, ByRef xmlFilename As String) As Integer
            'Check whether the tablename exists in the XML
            'If it does, bail out and dont do anything

            'If it doesnt exist, need to create node and add it
            Dim thisNode As XmlNode
            Dim thisSubNode As XmlNode
            Dim maxCols As Integer = data.Columns.Count - 1

            thisNode = myXml.SelectSingleNode("root")
            '            Alert("", AlertNewLine.ADD_CRLF)
            If Not IsNothing(thisNode) Then
                thisNode = thisNode.SelectSingleNode("Files")
                If Not IsNothing(thisNode) Then
                    thisSubNode = thisNode.SelectSingleNode(tablename)

                    If Not IsNothing(thisSubNode) Then
                        'Stop
                        'Delete the existing node and recreate it

                        thisNode.RemoveChild(thisSubNode)
                        '    thisSubNode = thisNode.PreviousSibling
                        'Else
                        '    thisSubNode = thisNode.LastChild
                    End If 'Not found, time to create it

                    Dim newNode As XmlElement
                    Dim newIncludeNode As XmlElement
                    Dim newtableNode As XmlElement
                    Dim newfieldNode As XmlElement

                    Dim newtableNameNode As XmlElement
                    Dim intMaxRows As Integer = data.Rows.Count() - 1
                    If intMaxRows = -1 Then intMaxRows = 0

                    If maxCols > 0 And intMaxRows >= 0 Then
                        newtableNode = myXML.CreateNode(XmlNodeType.Element, tablename, "")

                        newIncludeNode = myXML.CreateNode(XmlNodeType.Element, "include", "")
                        newIncludeNode.InnerText = "Y"
                        newtableNode.AppendChild(newIncludeNode)

                        newtableNameNode = myXML.CreateNode(XmlNodeType.Element, "tablename", "")
                        newtableNameNode.InnerText = "dbc_" & tablename

                        newtableNode.AppendChild(newtableNameNode)

                        newNode = myXML.CreateNode(XmlNodeType.Element, "fieldcount", "")
                        newNode.InnerText = maxCols + 1
                        newtableNode.AppendChild(newNode)

                        Dim blnOverrideOk As Boolean = False
                        Dim columnNameOverride As New Dictionary(Of Integer, String)
                        columnNameOverride = LoadXmlDefinitions(xmlFilename, tablename)
                        If columnNameOverride.Count() <= data.Columns.Count() And columnNameOverride.Count() > 0 Then blnOverrideOk = True
                        maxCols = columnNameOverride.Count() - 1
                        If blnOverrideOk = False Then maxCols = data.Columns.Count() - 1
                        For allColumns As Integer = 0 To maxCols
                            Try
                                If allColumns > maxCols Then Exit For
                                Dim thisLastColData As String
                                If Not IsDBNull(data.Rows(intMaxRows)(allColumns)) Then
                                    thisLastColData = data.Rows(intMaxRows)(allColumns)
                                Else
                                    thisLastColData = "1"
                                End If

                                Dim outfieldType As String = ""
                                'Last Row contains the field type.. 0 = String, 1 = Int32, 2 = Long, 3 = Float
                                Select Case thisLastColData
                                    Case "2" '"Long"
                                        outfieldType = "bigint"
                                        Exit Select
                                    Case "1" '"Int32"
                                        outfieldType = "int"
                                        Exit Select
                                    Case "3" '"Single", "Float"
                                        outfieldType = "float"
                                        Exit Select
                                    Case "0" '"String"
                                        outfieldType = "text"
                                        Exit Select
                                    Case Else
                                        outfieldType = "int"
                                        Alert([String].Format("Unknown field type {0}!, int assumed", thisLastColData), AlertNewLine.ADD_CRLF)
                                End Select


                                newfieldNode = myXML.CreateNode(XmlNodeType.Element, "field", "")
                                newfieldNode.SetAttribute("type", outfieldType)

                                If blnOverrideOk = False Then
                                    newfieldNode.SetAttribute("name", "col" & allColumns.ToString())
                                Else
                                    newfieldNode.SetAttribute("name", columnNameOverride(allColumns).ToString())
                                End If
                            Catch ex As Exception
                                Alert("Error in XML Def for " & tablename & ", column " & allColumns, AlertNewLine.ADD_CRLF)
                            End Try

                            newfieldNode.SetAttribute("include", "y")
                            newtableNode.AppendChild(newfieldNode)
                        Next

                        thisNode.InsertAfter(newtableNode, thisNode.LastChild)

                        myXML.Save(xmlFilename) '"D:\wtt\TEST.XML")
                        '    End If
                    End If
                End If
                Return maxCols
            End If
            Return 0
        End Function
    End Module
End Namespace
