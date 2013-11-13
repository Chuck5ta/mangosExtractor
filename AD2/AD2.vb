Imports AD2.Core
Imports System.IO
Imports System.Threading

Module Ad2
    Sub Main()
        Console.WriteLine(" ")
        Console.WriteLine("MaNGOSExtractor{0} CommandLine", MaNGOSExtractorVersion)
        Console.WriteLine("================================")
        Console.WriteLine(" ")
        If My.Application.CommandLineArgs.Count = 0 Then
            Console.WriteLine("Usage:")
            Console.WriteLine("ad2 -[opt] [value]")
            Console.WriteLine("-i set input path")
            Console.WriteLine("-o set output path")
            Console.WriteLine("-e {1/2/3} 1 Maps Only, 2 DBC Only, 3 both DBC and Maps   Default is 3")
            Console.WriteLine("-s create .SQL file for each .DBC, requires -o switch")
            Console.WriteLine("-c create .CSV file for each .DBC, requires -o switch")
            Console.WriteLine("-x create .XML file for each .DBC, requires -o switch")
            Console.WriteLine("-m create .MD file for each .DBC, requires -o switch")
            Console.WriteLine("-h create .H files for each .DBC, requires -o switch")
            End
        Else
            Dim strExtractionLevel As String
            Dim strInputFolder As String = ""
            Dim strOutputFolder As String = ""
            Dim intMaxCommands As Integer = My.Application.CommandLineArgs.Count - 1
            Dim blnCmdError As Boolean = False
            Dim blnExtract As Boolean = False
            Dim blnExportToSql As Boolean = False
            Dim blnExportToCsv As Boolean = False
            Dim blnExportToXml As Boolean = False
            Dim blnExportToMd As Boolean = False
            Dim blnExportToH As Boolean = False

            For commands As Integer = 0 To intMaxCommands
                Select Case My.Application.CommandLineArgs(commands)
                    Case "-e"
                        If commands < intMaxCommands Then
                            strExtractionLevel = My.Application.CommandLineArgs(commands + 1)
                        Else
                            strExtractionLevel = ""
                        End If

                        Select Case strExtractionLevel
                            Case "1"
                                Console.WriteLine("Extraction Selected: Maps Only")
                                blnExtract = True
                            Case "2"
                                Console.WriteLine("Extraction Selected: DBC Only")
                                blnExtract = True
                            Case "3"
                                Console.WriteLine("Extraction Selected: Both DBC and Maps")
                                blnExtract = True
                            Case Else
                                Console.WriteLine("Extraction Selected: *ERROR* - Invalid Option '" & strExtractionLevel & "'")
                                blnCmdError = True
                        End Select
                        commands = commands + 1
                    Case "-i"
                        If commands < intMaxCommands Then
                            strInputFolder = My.Application.CommandLineArgs(commands + 1)
                        Else
                            strInputFolder = ""
                        End If

                        If Directory.Exists(strInputFolder) = True Then
                            Console.WriteLine("Source Folder: " & strInputFolder)
                        Else
                            Console.WriteLine("Source Folder: *ERROR* - Folder '" & strInputFolder & "' was not found (or accessible)")
                            blnCmdError = True

                        End If
                        commands = commands + 1

                    Case "-o"
                        If commands < intMaxCommands Then
                            strOutputFolder = My.Application.CommandLineArgs(commands + 1)
                        Else
                            strOutputFolder = ""
                        End If

                        If Directory.Exists(strOutputFolder) = True Then
                            Console.WriteLine("Output Folder: " & strOutputFolder)
                        Else
                            Try
                                Directory.CreateDirectory(strOutputFolder)
                                Console.WriteLine("Output Folder: " & strOutputFolder)
                            Catch ex As Exception
                                Console.WriteLine("Output Folder: *ERROR* - Folder '" & strOutputFolder & "' could not be created")
                                blnCmdError = True
                            End Try
                        End If
                        commands = commands + 1
                    Case "-s"
                        blnExportToSql = True
                        If Directory.Exists(strOutputFolder) = False Then
                            Console.WriteLine("Output Folder: *ERROR* - Folder '" & strOutputFolder & "' could not be found")
                            blnCmdError = True
                        End If
                    Case "-c"
                        blnExportToCsv = True
                        If Directory.Exists(strOutputFolder) = False Then
                            Console.WriteLine("Output Folder: *ERROR* - Folder '" & strOutputFolder & "' could not be found")
                            blnCmdError = True
                        End If
                    Case "-x"
                        blnExportToXml = True
                        If Directory.Exists(strOutputFolder) = False Then
                            Console.WriteLine("Output Folder: *ERROR* - Folder '" & strOutputFolder & "' could not be found")
                            blnCmdError = True
                        End If
                    Case "-m"
                        blnExportToMd = True
                        If Directory.Exists(strOutputFolder) = False Then
                            Console.WriteLine("Output Folder: *ERROR* - Folder '" & strOutputFolder & "' could not be found")
                            blnCmdError = True
                        End If
                    Case "-h"
                        blnExportToH = True
                        If Directory.Exists(strOutputFolder) = False Then
                            Console.WriteLine("Output Folder: *ERROR* - Folder '" & strOutputFolder & "' could not be found")
                            blnCmdError = True
                        End If
                    Case Else
                        Console.WriteLine("Command={0}", My.Application.CommandLineArgs(commands))
                End Select
            Next

            'If any parameters have been flagged as having an error, bail out
            If blnExportToSql = True And strOutputFolder = "" And blnExtract = False Then
                Console.WriteLine("*ERROR* -o {output folder} is required")
                blnCmdError = True
            ElseIf blnExportToCsv = True And strOutputFolder = "" And blnExtract = False Then
                Console.WriteLine("*ERROR* -o {output folder} is required")
                blnCmdError = True
            ElseIf blnExportToH = True And strOutputFolder = "" Then
                Console.WriteLine("*ERROR* -o {output folder} is required")
                blnCmdError = True
            ElseIf blnExtract = True And strOutputFolder = "" Then
                Console.WriteLine("*ERROR* -o {output folder} is required")
                blnCmdError = True
            End If


            If blnCmdError = True Then End

            'At this stage we should have the options we need plus parameters and paths
            Dim colBaseFiles As New SortedList()     'Collection containing all the base files
            Dim colMainFiles As New SortedList()    'Collection containing all the main files
            Dim colUpdateFiles As New SortedList()   'Collection containing any update or patch files

            Dim colFolders As New Collection                'Collection to hold for the folders to be processed
            Dim myFolders As DirectoryInfo

            If Directory.Exists(strInputFolder) = False Then
                Alert("Warcraft folder '" & strInputFolder & "' can not be located", AlertNewLine.ADD_CRLF)
                Exit Sub
            End If

            ReadWarcraftExe(strInputFolder & "/Wow.exe")
            If FullVersion <> "" Then
                Alert("Warcraft Version v" & FullVersion & " Build " & BuildNo, AlertNewLine.ADD_CRLF)
            End If

            If blnExtract = True Then
                'Set the Top level as {Wow Folder}\data
                myFolders = New DirectoryInfo(strInputFolder & "/Data")

                'Add the Data folder to the collection before we start walking down the tree
                colFolders.Add(myFolders, myFolders.FullName)

                'Build a list of all the subfolders under data
                ReadFolders(myFolders, colFolders)

                'Now we need to walk through the folders, getting the MPQ files along the way
                For t As Integer = 1 To colFolders.Count()
                    myFolders = colFolders.Item(t)
                    For Each file As FileInfo In myFolders.GetFiles("*.MPQ")
                        If file.FullName.ToLower.Contains("update") = True Or file.FullName.ToLower.Contains("patch") = True Then
                            colUpdateFiles.Add(file.FullName, file.FullName)
                        ElseIf file.FullName.ToLower.Contains("base") = True Then
                            colBaseFiles.Add(file.FullName, file.FullName)
                        Else
                            colMainFiles.Add(file.FullName, file.FullName)
                        End If
                    Next
                Next

                If strOutputFolder.EndsWith("\") = False And strOutputFolder.EndsWith("/") = False Then
                    strOutputFolder = strOutputFolder & "/"
                End If

                If My.Computer.FileSystem.DirectoryExists(strOutputFolder) = False Then
                    Directory.CreateDirectory(strOutputFolder)
                End If


                For Each strItem As DictionaryEntry In colMainFiles
                    Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                    Try
                        ExtractDBCFiles(strItem.Value, "*.db?", strOutputFolder)
                    Catch ex As Exception
                        Alert(ex.Message, AlertNewLine.ADD_CRLF)
                    End Try
                Next

                For Each strItem As DictionaryEntry In colMainFiles
                    Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                    Try
                        ExtractDBCFiles(strItem.Value, "*.db?", strOutputFolder)
                    Catch ex As Exception
                        Alert(ex.Message, AlertNewLine.ADD_CRLF)
                    End Try
                Next

                For Each strItem As DictionaryEntry In colUpdateFiles
                    Alert("Reading: " & strItem.Value, False)

                    Try
                        ExtractDBCFiles(strItem.Value, "*.db?", strOutputFolder)
                    Catch ex As Exception
                        Alert(ex.Message, AlertNewLine.ADD_CRLF)
                    End Try
                    Thread.Sleep(0)
                Next
                Alert("Extraction Finished", AlertNewLine.ADD_CRLF)
            End If


            'Pass the Parameters to the export routine
            If blnExportToSql = True Or blnExportToCsv = True Or blnExportToXml = True Or blnExportToMd = True Or blnExportToH = True Then
                ExportDBCFiles(strInputFolder, strOutputFolder, blnExportToCsv, blnExportToSql, blnExportToXml, blnExportToMd, blnExportToH)
                Alert("Export Finished", AlertNewLine.ADD_CRLF)
            End If

            End
        End If
    End Sub

    Class Listbox
        Function Items() As SortedList
            Return Nothing
        End Function

        Function StartIndex() As Integer
            Return 0
        End Function

        Public Sub RemoveAt(index As Integer)
            'Me.owner.CheckNoDataSource()
            'If (index < 0) OrElse (index >= Me.InnerArray.GetCount(0)) Then
            '    Throw New ArgumentOutOfRangeException("index", SR.GetString("InvalidArgument", New Object() {"index", index.ToString(CultureInfo.CurrentCulture)}))
            'End If
            'Me.owner.UpdateMaxItemWidth(Me.InnerArray.GetItem(index, 0), True)
            'Me.InnerArray.RemoveAt(index)
            'If Me.owner.IsHandleCreated Then
            '    Me.owner.NativeRemoveAt(index)
            'End If
            'Me.owner.UpdateHorizontalExtent()
        End Sub


        Property SelectedIndex As Integer
            Get
                Return 0
            End Get
            Set(value As Integer)
            End Set
        End Property

        ReadOnly Property Count() As Integer
            Get
                Return - 1
            End Get
        End Property
    End Class
End Module
