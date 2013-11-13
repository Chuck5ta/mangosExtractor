Imports System.IO
Imports System.Text
Imports System.Globalization
Imports MpqLib.Mpq
Imports System.Xml
Imports System.Reflection

Namespace Core
    Module MaNgosExtractorCore
        Private _mBuildNo As Integer
        Private _mMajorVersion As Integer
        Private _mFullVersion As String
        Public Const MaNgosExtractorVersion As String = "v1.47"

        Property BuildNo As Integer
            Get
                Return _mBuildNo
            End Get
            Private Set(value As Integer)
                _mBuildNo = value
            End Set
        End Property

        Property MajorVersion As Integer
            Get
                Return _mMajorVersion
            End Get
            Private Set(value As Integer)
                _mMajorVersion = value
            End Set
        End Property

        Property FullVersion As String
            Get
                Return _mFullVersion
            End Get
            Private Set(value As String)
                _mFullVersion = value
            End Set
        End Property

        
        ''' <summary>
        '''     Returns the version number as a string which is pulled from the application properties
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Version As String
            Get

                Return " v" & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Revision   'm_Version
            End Get
            'Private Set(value As Integer)
            '    m_Version = value
            'End Set
        End Property

        
        ''' <summary>
        '''     A boolean value which indicates whether the app is running as a gui or console (false=console)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RunAsGui As Boolean
            Get
                Return _mRunningAsGui
            End Get
            Set(value As Boolean)
                _mRunningAsGui = value
            End Set
        End Property

        Private _mRunningAsGui As Boolean = False

        
        ''' <summary>
        '''     Defines the Listbox which messages are sent to in gui mode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Alertlist As ListBox
            Private Get
                Return _mAlertlist
            End Get
            Set(value As ListBox)
                _mAlertlist = value
            End Set
        End Property

        ''' <summary>
        '''     Defines the Listbox which messages are sent to in gui mode
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AlertTitle As Listbox
            Private Get
                Return _mAlertTitle
            End Get
            Set(value As Listbox)
                _mAlertTitle = value
            End Set
        End Property

        Private _mAlertlist As ListBox
        Private _mAlertTitle As Listbox

        Public Sub ReadWarcraftExe(ByRef filename As String)
            Try
                _mFullVersion = FileVersionInfo.GetVersionInfo(filename).FileVersion
                BuildNo = _mFullVersion.Substring(_mFullVersion.LastIndexOf(", ") + 2)
                FullVersion = _mFullVersion.Substring(0, _mFullVersion.LastIndexOf(", ")).Replace(" ", "").Replace(",", ".")
                MajorVersion = _mFullVersion.Substring(0, 1)
            Catch ex As Exception

            End Try
        End Sub

        
        ''' <summary>
        '''     Recursively reads the directory structure from the StartFolder down
        ''' </summary>
        ''' <param name="startFolder"></param>
        ''' <param name="folderList"></param>
        ''' <remarks></remarks>
        Public Function ReadFolders(ByRef startFolder As DirectoryInfo, ByRef folderList As Collection) As String
            Dim sbOutput As New StringBuilder
            If Directory.Exists(startFolder.FullName) = True Then
                Try
                    For Each thisFolder As DirectoryInfo In startFolder.GetDirectories()
                        Try
                            'Skip the cache and updates folders if they exist
                            If thisFolder.FullName.ToLower.Contains("cache") = False And thisFolder.FullName.ToLower.Contains("updates") = False Then
                                FolderList.Add(thisFolder, thisFolder.FullName)
#If _MyType <> "Console" Then
                                Application.DoEvents()
#Else
                                Threading.Thread.Sleep(0)
#End If
                                ReadFolders(thisFolder, FolderList)
                            End If
                        Catch ex As Exception
                            sbOutput.AppendLine("Error reading folder '" & thisFolder.FullName & "'")
                        End Try
                    Next
                Catch ex As Exception
                    sbOutput.AppendLine("Error reading folder '" & startFolder.FullName & "'")
                End Try
            Else
                sbOutput.AppendLine("Warcraft folder '" & startFolder.FullName & "' can not be located")
            End If
            Return sbOutput.ToString()
        End Function

        
        ''' <summary>
        '''     Extracts DBC Files including Patch files (MPQLib Version)
        ''' </summary>
        ''' <param name="mpqFilename"></param>
        ''' <param name="fileFilter"></param>
        ''' <param name="destinationFolder"></param>
        ''' <remarks></remarks>
        Public Function ExtractDbcFiles(ByVal mpqFilename As String, ByVal fileFilter As String, ByVal destinationFolder As String) As String
            Dim archive As CArchive
            Dim fileList As IEnumerable(Of CFileInfo)
            Dim sbOutput As New StringBuilder

            Try
                'Open the Archive Folder
                archive = New CArchive(mpqFilename)

                'Get a list of all files matching FileFilter
                fileList = archive.FindFiles(fileFilter)

                'Process each file found
                For Each thisFile As CFileInfo In fileList
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If
                    Dim inbyteData(thisFile.Size - 1) As Byte
                    Dim intFileType As Integer = 0
                    'intFileType = 0  = Unknown
                    'intFileType = 1  = WDBC
                    'intFileType = 2  = WDB2
                    'intFileType = 3  = PTCH
                    'Create the output directory tree, allowing for additional paths contained within the filename

                    Dim strSubFolder As String
                    If thisFile.FileName.Contains("\") = True Then
                        strSubFolder = thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))
                        If My.Computer.FileSystem.DirectoryExists(destinationFolder & strSubFolder) = False Then
                            Directory.CreateDirectory(destinationFolder & strSubFolder)
                        End If
                    Else
                        strSubFolder = ""
                        If My.Computer.FileSystem.DirectoryExists(destinationFolder) = False Then
                            Directory.CreateDirectory(destinationFolder)
                        End If
                    End If

                    Dim strOriginalName As String = thisFile.FileName.Substring(thisFile.FileName.LastIndexOf("\") + 1, thisFile.FileName.Length - (thisFile.FileName.LastIndexOf("\") + 1))
                    Dim strPatchName As String = strOriginalName & "_" & mpqFilename.Substring(mpqFilename.LastIndexOf("\") + 1, mpqFilename.Length - (mpqFilename.LastIndexOf("\") + 1) - 4) & ".patch"
                    Dim strNewName As String = strOriginalName & ".New"
                    If destinationFolder.EndsWith("\") = False Then destinationFolder = destinationFolder & "\"

                    'Skip corrupt files (Length < 21)
                    If inbyteData.Length > 20 Then

                        'We perform this export so that we can get the header bytes
                        archive.ExportFile(thisFile.FileName, inbyteData)
                        If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 67) Then intFileType = 1 'WDBC HEader
                        If (inbyteData(0) = 87 And inbyteData(1) = 68 And inbyteData(2) = 66 And inbyteData(3) = 50) Then intFileType = 2 'WDB2 Header
                        If (inbyteData(0) = 80 And inbyteData(1) = 84 And inbyteData(2) = 67 And inbyteData(3) = 72) Then intFileType = 3 'PTCH File

                        If intFileType = 1 Or intFileType = 2 Then 'Is a WDBC/WDB2 File

                            'Create the output directory tree, allowing for additional paths contained within the filename
                            If thisFile.FileName.Contains("\") = True Then
                                If My.Computer.FileSystem.DirectoryExists(destinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
                                    Directory.CreateDirectory(destinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
                                End If
                            Else
                                If My.Computer.FileSystem.DirectoryExists(destinationFolder) = False Then
                                    Directory.CreateDirectory(destinationFolder)
                                End If
                            End If

                            'If the file already exists, delete it and recreate it
                            If My.Computer.FileSystem.FileExists(destinationFolder & "\" & thisFile.FileName) = True Then
                                My.Computer.FileSystem.DeleteFile(destinationFolder & "\" & thisFile.FileName)
                            End If
                            archive.ExportFile(thisFile.FileName, destinationFolder & "\" & thisFile.FileName)
                        ElseIf intFileType = 3 Then 'PTCH File

                            '###############################################################################
                            '## Patch Files are a special case and are only present in Cata and Mop       ##
                            '## - The current Implementation has been split into two stages               ##
                            '###############################################################################
                            '## Stage 1 - Saves the files out with a .patch extension                     ##
                            '###############################################################################
                            '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                            '##           original file                                                   ##
                            '###############################################################################

                            '###############################################################################
                            '## Stage 1 - Saves the files out with a .patch extension                     ##
                            '###############################################################################

                            'If the file already exists, delete it and recreate it
                            If My.Computer.FileSystem.FileExists(destinationFolder & strSubFolder & "\" & strPatchName) = True Then
                                My.Computer.FileSystem.DeleteFile(destinationFolder & strSubFolder & "\" & strPatchName)
                            End If
                            archive.ExportFile(thisFile.FileName, destinationFolder & strSubFolder & "\" & strPatchName)

                            'Copy the patch to .new
                            If My.Computer.FileSystem.FileExists(destinationFolder & strSubFolder & "\" & strNewName) = False Then
                                File.Copy(destinationFolder & strSubFolder & "\" & strPatchName, destinationFolder & strSubFolder & "\" & strNewName)
                            End If


                            '###############################################################################
                            '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                            '##           original file                                                   ##
                            '###############################################################################
                            Using p As New Blizzard.Patch(destinationFolder & strSubFolder & "\" & strPatchName)
                                p.PrintHeaders(strOriginalName)
                                p.Apply(destinationFolder & strSubFolder & "\" & strOriginalName, destinationFolder & strSubFolder & "\" & strNewName, True)
                            End Using

                            'Move the original and the patch
                            My.Computer.FileSystem.DeleteFile(destinationFolder & strSubFolder & "\" & strOriginalName)
                            My.Computer.FileSystem.DeleteFile(destinationFolder & strSubFolder & "\" & strPatchName)

                            'Rename the .new as the Original Name
                            My.Computer.FileSystem.RenameFile(destinationFolder & strSubFolder & "\" & strNewName, strOriginalName)


                            'Else    'File is something else
                            '    'As I am matching on *.db* rather than *.dbc or *.db2, one .db file is found as well - so this check ignores it
                            '    If thisFile.FileName.EndsWith(".db") = False Then
                            '        sbOutput.AppendLine("Strange File Type: " & thisFile.FileName)
                            'End If
                        End If
                    End If
                    '                    exportSQL(DestinationFolder & strSubFolder & "\" & strOriginalName)
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If
                Next
            Catch ex As Exception
                sbOutput.AppendLine(ex.Message)
            End Try
            Return sbOutput.ToString()
        End Function

        
        ''' <summary>
        '''     Extracts ADT Files including Patch files (MPQLib Version)
        ''' </summary>
        ''' <param name="mpqFilename"></param>
        ''' <param name="fileFilter"></param>
        ''' <param name="destinationFolder"></param>
        ''' <remarks></remarks>
        Public Function ExtractAdtFiles(ByVal mpqFilename As String, ByVal fileFilter As String, ByVal destinationFolder As String) As String
            Dim archive As CArchive
            Dim fileList As IEnumerable(Of CFileInfo)
            Dim sbOutput As New StringBuilder

            Try
                'Open the Archive Folder
                archive = New CArchive(mpqFilename)

                'Get a list of all files matching FileFilter
                fileList = archive.FindFiles(fileFilter)

                'Process each file found
                Dim blnProcessFile As Boolean = True
                For Each thisFile As CFileInfo In fileList
                    blnProcessFile = True
                    If thisFile.FileName.EndsWith("_obj0.adt") = True Then blnProcessFile = False
                    If thisFile.FileName.EndsWith("_obj1.adt") = True Then blnProcessFile = False
                    If thisFile.FileName.EndsWith("_tex0.adt") = True Then blnProcessFile = False
                    If thisFile.FileName.EndsWith("_tex1.adt") = True Then blnProcessFile = False

                    If blnProcessFile = True Then
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                        Dim inbyteData(thisFile.Size - 1) As Byte
                        Dim intFileType As Integer = 0
                        'intFileType = 0  = Unknown
                        'intFileType = 1  = WDBC
                        'intFileType = 2  = WDB2
                        'intFileType = 3  = PTCH
                        'Create the output directory tree, allowing for additional paths contained within the filename

                        Dim strSubFolder As String
                        If thisFile.FileName.Contains("\") = True Then
                            strSubFolder = thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))
                            If My.Computer.FileSystem.DirectoryExists(destinationFolder & strSubFolder) = False Then
                                Directory.CreateDirectory(destinationFolder & strSubFolder)
                            End If
                        Else
                            strSubFolder = ""
                            If My.Computer.FileSystem.DirectoryExists(destinationFolder) = False Then
                                Directory.CreateDirectory(destinationFolder)
                            End If
                        End If

                        Dim strOriginalName As String = thisFile.FileName.Substring(thisFile.FileName.LastIndexOf("\") + 1, thisFile.FileName.Length - (thisFile.FileName.LastIndexOf("\") + 1))
                        Dim strPatchName As String = strOriginalName & "_" & mpqFilename.Substring(mpqFilename.LastIndexOf("\") + 1, mpqFilename.Length - (mpqFilename.LastIndexOf("\") + 1) - 4) & ".patch"
                        Dim strNewName As String = strOriginalName & ".New"
                        If destinationFolder.EndsWith("\") = False Then destinationFolder = destinationFolder & "\"

                        'Skip corrupt files (Length < 21)
                        If inbyteData.Length > 20 Then

                            'We perform this export so that we can get the header bytes
                            Alert("Processing: " & thisFile.FileName, AlertNewLine.ADD_CRLF)
                            archive.ExportFile(thisFile.FileName, inbyteData)
                            If (inbyteData(0) = 82 And inbyteData(1) = 69 And inbyteData(2) = 86 And inbyteData(3) = 77) Then intFileType = 1 'REVM HEader
                            If (inbyteData(0) = 80 And inbyteData(1) = 84 And inbyteData(2) = 67 And inbyteData(3) = 72) Then intFileType = 3 'PTCH File

                            If intFileType <> 3 Then 'Is a not a patch file

                                'Create the output directory tree, allowing for additional paths contained within the filename
                                If thisFile.FileName.Contains("\") = True Then
                                    If My.Computer.FileSystem.DirectoryExists(destinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
                                        Directory.CreateDirectory(destinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
                                    End If
                                Else
                                    If My.Computer.FileSystem.DirectoryExists(destinationFolder) = False Then
                                        Directory.CreateDirectory(destinationFolder)
                                    End If
                                End If

                                'If the file already exists, delete it and recreate it
                                If My.Computer.FileSystem.FileExists(destinationFolder & "\" & thisFile.FileName) = True Then
                                    My.Computer.FileSystem.DeleteFile(destinationFolder & "\" & thisFile.FileName)
                                End If
                                archive.ExportFile(thisFile.FileName, destinationFolder & "\" & thisFile.FileName)
                            ElseIf intFileType = 3 Then 'PTCH File

                                '###############################################################################
                                '## Patch Files are a special case and are only present in Cata and Mop       ##
                                '## - The current Implementation has been split into two stages               ##
                                '###############################################################################
                                '## Stage 1 - Saves the files out with a .patch extension                     ##
                                '###############################################################################
                                '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                                '##           original file                                                   ##
                                '###############################################################################

                                '###############################################################################
                                '## Stage 1 - Saves the files out with a .patch extension                     ##
                                '###############################################################################

                                'If the file already exists, delete it and recreate it
                                If My.Computer.FileSystem.FileExists(destinationFolder & strSubFolder & "\" & strPatchName) = True Then
                                    My.Computer.FileSystem.DeleteFile(destinationFolder & strSubFolder & "\" & strPatchName)
                                End If
                                archive.ExportFile(thisFile.FileName, destinationFolder & strSubFolder & "\" & strPatchName)

                                'Copy the patch to .new
                                If My.Computer.FileSystem.FileExists(destinationFolder & strSubFolder & "\" & strNewName) = False Then
                                    File.Copy(destinationFolder & strSubFolder & "\" & strPatchName, destinationFolder & strSubFolder & "\" & strNewName)
                                End If


                                '###############################################################################
                                '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                                '##           original file                                                   ##
                                '###############################################################################
                                Using p As New Blizzard.Patch(destinationFolder & strSubFolder & "\" & strPatchName)
                                    p.PrintHeaders(strOriginalName)
                                    p.Apply(destinationFolder & strSubFolder & "\" & strOriginalName, destinationFolder & strSubFolder & "\" & strNewName, True)
                                End Using

                                'Move the original and the patch
                                My.Computer.FileSystem.DeleteFile(destinationFolder & strSubFolder & "\" & strOriginalName)
                                My.Computer.FileSystem.DeleteFile(destinationFolder & strSubFolder & "\" & strPatchName)

                                'Rename the .new as the Original Name
                                My.Computer.FileSystem.RenameFile(destinationFolder & strSubFolder & "\" & strNewName, strOriginalName)

                            End If
                        End If
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                    End If
                Next
            Catch ex As Exception
                sbOutput.AppendLine(ex.Message)
            End Try
            Return sbOutput.ToString()
        End Function

        
        ''' <summary>
        '''     Extracts WDT Files including Patch files (MPQLib Version)
        ''' </summary>
        ''' <param name="mpqFilename"></param>
        ''' <param name="fileFilter"></param>
        ''' <param name="destinationFolder"></param>
        ''' <remarks></remarks>
        Public Function ExtractWdtFiles(ByVal mpqFilename As String, ByVal fileFilter As String, ByVal destinationFolder As String) As String
            Dim archive As CArchive
            Dim fileList As IEnumerable(Of CFileInfo)
            Dim sbOutput As New StringBuilder

            Try
                'Open the Archive Folder
                archive = New CArchive(mpqFilename)

                'Get a list of all files matching FileFilter
                fileList = archive.FindFiles(fileFilter)

                'Process each file found
                Dim blnProcessFile As Boolean = True
                For Each thisFile As CFileInfo In fileList
                    blnProcessFile = True

                    If blnProcessFile = True Then
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                        Dim inbyteData(thisFile.Size - 1) As Byte
                        Dim intFileType As Integer = 0

                        'Create the output directory tree, allowing for additional paths contained within the filename
                        Dim strSubFolder As String
                        If thisFile.FileName.Contains("\") = True Then
                            strSubFolder = thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))
                            If My.Computer.FileSystem.DirectoryExists(DestinationFolder & strSubFolder) = False Then
                                Directory.CreateDirectory(DestinationFolder & strSubFolder)
                            End If
                        Else
                            strSubFolder = ""
                            If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
                                Directory.CreateDirectory(DestinationFolder)
                            End If
                        End If

                        Dim strOriginalName As String = thisFile.FileName.Substring(thisFile.FileName.LastIndexOf("\") + 1, thisFile.FileName.Length - (thisFile.FileName.LastIndexOf("\") + 1))
                        Dim strPatchName As String = strOriginalName & "_" & mpqFilename.Substring(mpqFilename.LastIndexOf("\") + 1, mpqFilename.Length - (mpqFilename.LastIndexOf("\") + 1) - 4) & ".patch"
                        Dim strNewName As String = strOriginalName & ".New"
                        If DestinationFolder.EndsWith("\") = False Then DestinationFolder = DestinationFolder & "\"

                        'Skip corrupt files (Length < 21)
                        If inbyteData.Length > 20 Then

                            'We perform this export so that we can get the header bytes
                            Alert("Processing: " & thisFile.FileName, AlertNewLine.ADD_CRLF)
                            archive.ExportFile(thisFile.FileName, inbyteData)
                            If (inbyteData(0) = 82 And inbyteData(1) = 69 And inbyteData(2) = 86 And inbyteData(3) = 77) Then intFileType = 1 'REVM HEader
                            If (inbyteData(0) = 80 And inbyteData(1) = 84 And inbyteData(2) = 67 And inbyteData(3) = 72) Then intFileType = 3 'PTCH File

                            If intFileType <> 3 Then 'Is a not a patch file

                                'Create the output directory tree, allowing for additional paths contained within the filename
                                If thisFile.FileName.Contains("\") = True Then
                                    If My.Computer.FileSystem.DirectoryExists(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
                                        Directory.CreateDirectory(DestinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
                                    End If
                                Else
                                    If My.Computer.FileSystem.DirectoryExists(DestinationFolder) = False Then
                                        Directory.CreateDirectory(DestinationFolder)
                                    End If
                                End If

                                'If the file already exists, delete it and recreate it
                                If My.Computer.FileSystem.FileExists(DestinationFolder & "\" & thisFile.FileName) = True Then
                                    My.Computer.FileSystem.DeleteFile(DestinationFolder & "\" & thisFile.FileName)
                                End If
                                archive.ExportFile(thisFile.FileName, DestinationFolder & "\" & thisFile.FileName)
                            ElseIf intFileType = 3 Then 'PTCH File

                                '###############################################################################
                                '## Patch Files are a special case and are only present in Cata and Mop       ##
                                '## - The current Implementation has been split into two stages               ##
                                '###############################################################################
                                '## Stage 1 - Saves the files out with a .patch extension                     ##
                                '###############################################################################
                                '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                                '##           original file                                                   ##
                                '###############################################################################

                                '###############################################################################
                                '## Stage 1 - Saves the files out with a .patch extension                     ##
                                '###############################################################################

                                'If the file already exists, delete it and recreate it
                                If My.Computer.FileSystem.FileExists(DestinationFolder & strSubFolder & "\" & strPatchName) = True Then
                                    My.Computer.FileSystem.DeleteFile(DestinationFolder & strSubFolder & "\" & strPatchName)
                                End If
                                archive.ExportFile(thisFile.FileName, DestinationFolder & strSubFolder & "\" & strPatchName)

                                'Copy the patch to .new
                                If My.Computer.FileSystem.FileExists(DestinationFolder & strSubFolder & "\" & strNewName) = False Then
                                    File.Copy(DestinationFolder & strSubFolder & "\" & strPatchName, DestinationFolder & strSubFolder & "\" & strNewName)
                                End If


                                '###############################################################################
                                '## Stage 2 - will attempt to process the patch files and apply them to the   ##
                                '##           original file                                                   ##
                                '###############################################################################
                                Using p As New Blizzard.Patch(destinationFolder & strSubFolder & "\" & strPatchName)
                                    p.PrintHeaders(strOriginalName)
                                    p.Apply(destinationFolder & strSubFolder & "\" & strOriginalName, destinationFolder & strSubFolder & "\" & strNewName, True)
                                End Using

                                'Move the original and the patch
                                My.Computer.FileSystem.DeleteFile(DestinationFolder & strSubFolder & "\" & strOriginalName)
                                My.Computer.FileSystem.DeleteFile(DestinationFolder & strSubFolder & "\" & strPatchName)

                                'Rename the .new as the Original Name
                                My.Computer.FileSystem.RenameFile(DestinationFolder & strSubFolder & "\" & strNewName, strOriginalName)

                            End If
                        End If
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                    End If
                Next
            Catch ex As Exception
                sbOutput.AppendLine(ex.Message)
            End Try
            Return sbOutput.ToString()
        End Function

        
        ''' <summary>
        '''     Generic Extraction Routine
        ''' </summary>
        ''' <param name="MPQFilename"></param>
        ''' <param name="FileFilter"></param>
        ''' <param name="DestinationFolder"></param>
        ''' <remarks></remarks>
        Public Function ExtractFilesGeneric(ByVal mpqFilename As String, ByVal fileFilter As String, ByVal destinationFolder As String) As String
            Dim archive As CArchive
            Dim fileList As IEnumerable(Of CFileInfo)
            Dim sbOutput As New StringBuilder

            Try
                archive = New CArchive(mpqFilename)

                fileList = archive.FindFiles(fileFilter)

                For Each thisFile As CFileInfo In fileList
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If
                    If thisFile.FileName.Contains("\") = True Then
                        If My.Computer.FileSystem.DirectoryExists(destinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\")))) = False Then
                            Directory.CreateDirectory(destinationFolder & "\" & thisFile.FileName.Substring(0, (thisFile.FileName.LastIndexOf("\"))))
                        End If
                    Else
                        If My.Computer.FileSystem.DirectoryExists(destinationFolder) = False Then
                            Directory.CreateDirectory(destinationFolder)
                        End If
                    End If

                    If My.Computer.FileSystem.FileExists(destinationFolder & "\" & thisFile.FileName) = True Then
                        My.Computer.FileSystem.DeleteFile(destinationFolder & "\" & thisFile.FileName)
                    End If

                    Alert(thisFile.FileName, AlertNewLine.ADD_CRLF)

                    archive.ExportFile(thisFile.FileName, destinationFolder & "\" & thisFile.FileName)
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If
                Next
            Catch ex As Exception
                sbOutput.AppendLine(ex.Message)
            End Try
            Return sbOutput.ToString()
        End Function

        ''' <summary>
        '''     Loads a DBC file data into a datatable
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadDbCtoDataTable(ByRef filename As String) As Data.DataTable
            Dim dbcDataTable As New Data.DataTable
            Return LoadDbCtoDataTable(filename, dbcDataTable)
        End Function

        
        ''' <summary>
        '''     Loads a DBC file data into a datatable
        ''' </summary>
        ''' <param name="filename"></param>
        ''' <param name="dbcDataTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadDbCtoDataTable(ByRef filename As String, ByRef dbcDataTable As Data.DataTable) As Data.DataTable
            Dim mReader As FileReader.IWowClientDbReader
            'Dim myOutputTable As New DataTable()

            Dim entireRow() As Byte
            'Dim thisRow As DataRow

            mReader = FileReader.DBReaderFactory.GetReader(filename)
            Try
                entireRow = mReader.GetRowAsByteArray(0)
            Catch ex As Exception
                entireRow = Nothing
            End Try
            Dim intMaxcols As Integer
            If IsNothing(entireRow) = True Then
                intMaxcols = 0
            Else
                intMaxcols = entireRow.Length - 1
            End If
            '           Dim colType(intMaxcols / 4) As String

            '###################################################################
            '### If we have columns, start looping through them
            '###################################################################
            If intMaxcols > 0 Then
                Dim intMaxRows As Integer = 0
                Try
                    intMaxRows = mReader.RecordsCount() - 1
                Catch
                    intMaxRows = 0
                End Try

                '###################################################################
                '### If we have rows, start looping through them
                '###################################################################
                If intMaxRows >= 0 Then
                    'Create the entire blank table
                    Dim tempCols As Integer = (intMaxcols / 4) - 1
                    For cols As Integer = 0 To tempCols 'intMaxcols Step 4
                        dbcDataTable.Columns.Add("U_c" & (cols / 4).ToString(), GetType(String))
                    Next
                    For intRows As Integer = 0 To intMaxRows + 1
                        Try
                            dbcDataTable.Rows.Add()
                        Catch
                            Stop
                        End Try
                    Next

                    Dim consoleLine As String = filename.Substring(filename.LastIndexOf("\") + 1) & " Rows: " & intMaxRows & " Cols: " & tempCols + 1
                    consoleLine = consoleLine.PadRight(57)
                    '                    Alert(consoleLine, AlertNewLine.ADD_CRLF)
                    Alert(consoleLine, AlertNewLine.NO_CRLF)

                    'Populate Raw Data into the table
                    Dim colTypes(tempCols) As String
                    Dim Divisor As Integer

                    If intMaxRows > 25 Then
                        Divisor = intMaxRows / 25
                    Else
                        Divisor = 1
                    End If

                    Dim dotcount As Integer = 0
                    For intRows As Integer = 0 To intMaxRows
                        If intRows Mod Divisor = 0 Then
                            If intRows > 0 And dotcount < 24 Then
                                Alert("x", AlertNewLine.NO_CRLF)
                                dotcount = dotcount + 1
#If _MyType <> "Console" Then
                                Application.DoEvents()
#Else
                                Threading.Thread.Sleep(0)
#End If
                            End If
                        End If


                        entireRow = mReader.GetRowAsByteArray(intRows)
                        For intcols = 0 To tempCols 'intMaxcols Step 4
                            Dim rowCols As Integer
                            rowCols = 4 * intcols
                            Try
                                'Load the HEX Bytes into the Cell as a string
                                dbcDataTable.Rows(intRows)(intcols) = CStr(MyHex(entireRow(rowCols + 3)) & MyHex(entireRow(rowCols + 2)) & MyHex(entireRow(rowCols + 1)) & MyHex(entireRow(rowCols + 0)))

                                'Not use ReturnValueType to determine the datatype
                                Dim retType As String = ReturnValueType(dbcDataTable.Rows(intRows)(intcols))
                                If colTypes(intcols) <> "" Then
                                    'If the returnedtype is different to the current type
                                    If colTypes(intcols) <> retType Then
                                        Select Case colTypes(intcols)
                                            Case "Int32" 'Everything can replace Ints
                                                colTypes(intcols) = retType
                                            Case "Long" 'Only Floats can replace Longs
                                                If retType = "Float" Then colTypes(intcols) = retType
                                            Case "Float"
                                                'Nothing replaces Floats
                                            Case "String" 'Floats and long replace the string flaf
                                                If retType = "Float" Then colTypes(intcols) = retType
                                                If retType = "Long" Then colTypes(intcols) = retType
                                        End Select
                                    End If
                                Else
                                    colTypes(intcols) = retType
                                End If

                                Try
                                    dbcDataTable.Rows(intRows)(intcols) = ReturnValue(dbcDataTable.Rows(intRows)(intcols))
                                Catch ex As Exception
                                    Stop
                                End Try

                                'Attempt to clean up the values
                                If IsDBNull(dbcDataTable.Rows(intRows)(intcols)) = True Then
                                    If colTypes(intcols) = "Int32" Then
                                        dbcDataTable.Rows(intRows)(intcols) = 0
                                    End If
                                    If colTypes(intcols) = "Long" Then
                                        dbcDataTable.Rows(intRows)(intcols) = 0
                                    End If
                                    If colTypes(intcols) = "Float" Then
                                        dbcDataTable.Rows(intRows)(intcols) = 0
                                    End If
                                End If
                            Catch ex As Exception
                                Stop
                            End Try

                        Next

                    Next
                    If dotcount < 24 Then
                        Dim tempstr As String
                        tempstr = New String("x", 24 - dotcount)
                        Alert(tempstr, AlertNewLine.NO_CRLF)

                    End If

                    'Alert("", AlertNewLine.ADD_CRLF)

                    ' ''#############################################################################################
                    ' ''### Try and find all the string columns
                    ' ''#############################################################################################

                    For intCols As Integer = 0 To dbcDataTable.Columns.Count() - 1
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                        Dim isString As Boolean = True
                        Try
                            For introws As Integer = 0 To dbcDataTable.Rows.Count() - 1 'intMaxcols Step 4
                                Try
                                    If colTypes(intCols) = "Int32" Then
                                        If IsDBNull(dbcDataTable.Rows(introws)(intCols)) = False Then
                                            If CStr(dbcDataTable.Rows(introws)(intCols)).Contains(".") = False Then
                                                Dim tempInt As Integer
                                                If Integer.TryParse(dbcDataTable.Rows(introws)(intCols), tempInt) = False Then
                                                    isString = False
                                                Else
                                                    tempInt = dbcDataTable.Rows(introws)(intCols)
                                                    If tempInt > 1 Then
                                                        If mReader.StringTable.ContainsKey((dbcDataTable.Rows(introws)(intCols))) = False Then
                                                            isString = False
                                                            Exit For
                                                        End If
                                                    ElseIf tempInt < 0 Then
                                                        isString = False
                                                        Exit For
                                                    End If
                                                End If
                                            Else
                                                isString = False
                                                Exit For
                                            End If
                                        Else 'Its false, populate with 0
                                            dbcDataTable.Rows(introws)(intCols) = 0
                                        End If
                                    Else
                                        isString = False
                                        Exit For
                                    End If
                                Catch ex As Exception
                                    Stop
                                    '                isString = False
                                End Try
                                If IsDBNull(dbcDataTable.Rows(introws)(intCols)) = False Then
                                    If CStr(dbcDataTable.Rows(introws)(intCols)) = "NaN" Then
                                        isString = False
                                    Else
                                        Dim tempInt As Integer
                                        If Integer.TryParse(dbcDataTable.Rows(introws)(intCols), tempInt) = False Then
                                            isString = False
                                        End If
                                    End If
                                End If
                            Next
                        Catch ex As Exception
                            Stop
                        End Try
                        If isString = True Then
                            colTypes(intCols) = "String"
                        End If
                    Next

                    'Populate the String
                    For introws As Integer = 0 To dbcDataTable.Rows.Count() - 1 'intMaxcols Step 4
                        If introws Mod 100 = 0 Then
#If _MyType <> "Console" Then
                            Application.DoEvents()
#Else
                            Threading.Thread.Sleep(0)
#End If
                        End If
                        Try
                            For intCols As Integer = 0 To dbcDataTable.Columns.Count() - 1
                                'Need to place the string overrides here !
                                'Some tables have columns which are not string fields but have data which matches the string table
                                If MajorVersion = 1 Then 'Classic
                                    If filename.Contains("BankBagSlotPrices.dbc") = True Then
                                        If intCols = 1 Then colTypes(intCols) = "Int32"
                                    ElseIf filename.Contains("CreatureDisplayInfoExtra.dbc") = True Then
                                        If intCols = 2 Then colTypes(intCols) = "Int32"
                                    End If
                                End If

                                Try
                                    If colTypes(intCols) = "String" Then
                                        If IsDBNull(dbcDataTable.Rows(introws)(intCols)) = False Then
                                            If CStr(dbcDataTable.Rows(introws)(intCols)).Contains(".") = False Then
                                                If mReader.StringTable.ContainsKey((dbcDataTable.Rows(introws)(intCols))) = True Then
                                                    dbcDataTable.Rows(introws)(intCols) = mReader.StringTable((dbcDataTable.Rows(introws)(intCols)))
                                                End If
                                            End If
                                        End If
                                    Else
                                        If IsDBNull(dbcDataTable.Rows(introws)(intCols)) = True Then
                                            If colTypes(intCols) = "Int32" Then
                                                dbcDataTable.Rows(introws)(intCols) = 0
                                            End If
                                            If colTypes(intCols) = "Long" Then
                                                dbcDataTable.Rows(introws)(intCols) = 0
                                            End If
                                            If colTypes(intCols) = "Float" Then
                                                dbcDataTable.Rows(introws)(intCols) = 0
                                            End If
                                        End If

                                        If CStr(dbcDataTable.Rows(introws)(intCols)) = "NaN" Then
                                            dbcDataTable.Rows(introws)(intCols) = 0
                                        End If

                                        If colTypes(intCols) = "Int32" And CStr(dbcDataTable.Rows(introws)(intCols)).Length = 0 Then
                                            dbcDataTable.Rows(introws)(intCols) = 0
                                        End If
                                        If colTypes(intCols) = "Long" And CStr(dbcDataTable.Rows(introws)(intCols)).Length = 0 Then
                                            dbcDataTable.Rows(introws)(intCols) = 0
                                        End If
                                        If CStr(dbcDataTable.Rows(introws)(intCols)).Trim = "" Then
                                            dbcDataTable.Rows(introws)(intCols) = 0
                                        End If
                                    End If
                                Catch ex As Exception
                                    Stop
                                End Try
                            Next
                        Catch ex As Exception
                            Stop
                        End Try
                    Next

                    '#######################################################################
                    '## Last row contains column types
                    '#######################################################################
                    dbcDataTable.NewRow()
                    For intCols As Integer = 0 To dbcDataTable.Columns.Count() - 1
                        Dim strDataType = colTypes(intCols)

                        If strDataType = "Int32" Then 'Integer
                            dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(intCols)) = 1
                        ElseIf strDataType = "Float" Then 'Float
                            dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(intCols)) = 3
                        ElseIf strDataType = "String" Then 'Float
                            dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(intCols)) = 0
                        Else 'Long
                            dbcDataTable.Rows(dbcDataTable.Rows.Count() - 1)(CInt(intCols)) = 2
                        End If

                    Next

                End If

            End If

            Return dbcDataTable
        End Function

        
        ''' <summary>
        '''     Remove characters that mess with MySQL by escaping them with a leading \
        ''' </summary>
        ''' <param name="input"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function StripBadCharacters(input As String) As String
            input = input.Replace("\", "\\")
            input = input.Replace("'", "\'")
            input = input.Replace("_", "\_")
            input = input.Replace("%", "\%")
            input = input.Replace(Chr(34), "\" & Chr(34))
            Return input
        End Function

        Public Enum AlertNewLine
            NO_CRLF = 1
            ADD_CRLF = 2
        End Enum

        
        ''' <summary>
        ''' Sends a message to either a gui listbox or console
        ''' </summary>
        ''' <param name="alertMessage"></param>
        ''' <param name="lineEnding"></param>
        ''' <remarks></remarks>
        Public Sub Alert(ByRef alertMessage As String, ByRef lineEnding As AlertNewLine)
            Alert(alertMessage, lineEnding, Alertlist)
        End Sub

        ''' <summary>
        ''' Sends a message to either a gui listbox or console (alertbox control can be overridden).
        ''' </summary>
        ''' <param name="alertMessage">The alert message.</param>
        ''' <param name="lineEnding">The line ending.</param>
        ''' <param name="thisAlertList">The this alert list.</param>
        ''' <returns></returns>
        Public Sub Alert(ByRef alertMessage As String, ByRef lineEnding As AlertNewLine, ByRef thisAlertList As Listbox)
            If _mRunningAsGui = True Then 'running as a Gui App

                If Not IsNothing(thisAlertList) Then

                    If lineEnding = AlertNewLine.ADD_CRLF Then
#If _MyType <> "Console" Then
                        thisAlertList.Items.Add(alertMessage)
#Else
                        thisAlertList.Items.Add(alertMessage, alertMessage)
#End If
                        thisAlertList.SelectedIndex = thisAlertList.Items.Count() - 1
                        thisAlertList.SelectedIndex = -1
                    Else
                        Dim temp As String = thisAlertList.Items(thisAlertList.Items.Count() - 1)
                        alertMessage = temp & alertMessage


                        thisAlertList.Items.RemoveAt(thisAlertList.Items.Count() - 1)
#If _MyType <> "Console" Then
                        thisAlertList.Items.Add(alertMessage)
#Else
                        thisAlertList.Items.Add(alertMessage, alertMessage)
#End If
                        thisAlertList.SelectedIndex = thisAlertList.Items.Count() - 1
                        thisAlertList.SelectedIndex = -1
                    End If
                End If
            Else 'Running as console
                If lineEnding = AlertNewLine.ADD_CRLF Then
                    Console.WriteLine(alertMessage)
                Else
                    Console.Write(alertMessage)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Main Export routine, this reads all the dbc's from the output folder and calls the selected export routines on each
        ''' one
        ''' </summary>
        ''' <param name="baseFolder">The base folder.</param>
        ''' <param name="outputFolder">The output folder.</param>
        ''' <param name="exportCsv">The export CSV.</param>
        ''' <param name="exportSql">The export SQL.</param>
        ''' <param name="exportXml">The export XML.</param>
        ''' <param name="exportMd">The export md.</param>
        ''' <param name="exportH">The export H.</param>
        ''' <remarks></remarks>
        Public Sub ExportDbcFiles(ByRef baseFolder As String, ByRef outputFolder As String, ByRef exportCsv As Boolean, ByRef exportSql As Boolean, ByRef exportXml As Boolean, ByRef exportMd As Boolean, ByRef exportH As Boolean)
            'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
            If outputFolder.EndsWith("\") = False Then outputFolder = outputFolder & "\"
            If My.Computer.FileSystem.DirectoryExists(outputFolder & "DBFilesClient\") = False Then
                Directory.CreateDirectory(outputFolder & "DBFilesClient\")
            End If
            Dim myFolders As DirectoryInfo
            myFolders = New DirectoryInfo(outputFolder & "\DBFilesClient")

            Dim files() As FileInfo = myFolders.GetFiles("*.DB?")
            Dim filelistSorted As New SortedList()

            For Each thisFile As FileInfo In files
                filelistSorted.Add(thisFile.Name, thisFile.Name)
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            'Display the progressbar header
            If exportCsv = True Or exportSql = True Or exportXml = True Or exportMd = True Or exportH = True Then
                Dim consoleLine As String = "0----+----50----+----100"
                Dim consoleLineTitle As String = "Warcraft Version v" & FullVersion & " Build " & BuildNo
                consoleLineTitle = consoleLineTitle.PadRight(57) & consoleLine
                consoleLine = consoleLine.PadLeft(81)
                Alert(consoleLineTitle, AlertNewLine.ADD_CRLF, AlertTitle)   'Place this in the Title section so that it remains onscreen
                Alert(consoleLine, AlertNewLine.ADD_CRLF)               'Place this at the start of the export lines
                Alert("", AlertNewLine.ADD_CRLF)
            End If

            ' For Each file As FileInfo In Files 'myFolders.GetFiles("*.DB?")
            For Each fileItem As DictionaryEntry In filelistSorted 'myFolders.GetFiles("*.DB?")
                Dim dbcDataTable As New Data.DataTable
#If _MyType <> "Console" Then
                Application.DoEvents()
#End If
                'Load the entire DBC into a DataTable to be processed by all exports
                If exportCsv = True Or exportSql = True Or exportXml = True Or exportMd = True Or exportH = True Then
                    'Alert("", AlertNewLine.ADD_CRLF)
                    'Alert(fileItem.Value, AlertNewLine.NO_CRLF)
                    dbcDataTable = LoadDbCtoDataTable(outputFolder & "\DBFilesClient" & "\" & fileItem.Value)
                    If dbcDataTable.Rows.Count() > 0 Then
                        Alert("  Saving: ", AlertNewLine.NO_CRLF)


                        'Export to SQL Files
                        If exportSql = True Then
                            Alert("SQL ", AlertNewLine.NO_CRLF)
                            Core.ExportSql(outputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable, baseFolder)
                        End If

                        'Export to h Files
                        If exportH = True Then
                            Alert("H ", AlertNewLine.NO_CRLF)
                            Core.ExportH(outputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable, baseFolder)
                        End If


                        'Export to CSV
                        If exportCsv = True Then
                            Alert("CSV ", AlertNewLine.NO_CRLF)
                            Core.ExportCsv(outputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable, baseFolder)
                        End If

                        'Export to XML
                        If exportXml = True Then
                            Alert("XML ", AlertNewLine.NO_CRLF)
                            Core.ExportXml(baseFolder, outputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable)
                        End If

                        'Export to git MD Files
                        If exportMd = True Then
                            Alert("MD ", AlertNewLine.NO_CRLF)
                            Core.ExportMd(baseFolder, outputFolder & "\DBFilesClient" & "\" & fileItem.Value, dbcDataTable)
                        End If

                        If exportCsv = True Or exportSql = True Or exportXml = True Or exportMd = True Or exportH = True Then
                            Alert("", AlertNewLine.ADD_CRLF)
                        End If
                    End If
                End If
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
                dbcDataTable = Nothing
            Next
        End Sub

        ''' <summary>
        '''     This function returns the dbc fieldnames xml config file
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReturnDbcxmLfilename() As String
            Dim xmlFilename As String = ""
            Select Case MajorVersion
                Case "1"
                    xmlFilename = "dbc_classic.xml"
                Case "2"
                    xmlFilename = "dbc_tbc.xml"
                Case "3"
                    xmlFilename = "dbc_wotlk.xml"
                Case "4"
                    xmlFilename = "dbc_cata.xml"
                Case "5"
                    xmlFilename = "dbc_mop.xml"
            End Select
            Return xmlFilename
        End Function


        ''' <summary>
        '''     This function returns the mangosCoreVersion based on the exe
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ReturnMangosCoreVersion() As String
            Dim xmlFilename As String = ""
            Select Case MajorVersion
                Case "1"
                    xmlFilename = "MaNGOSZero"
                Case "2"
                    xmlFilename = "MaNGOSOne"
                Case "3"
                    xmlFilename = "MaNGOSTwo"
                Case "4"
                    xmlFilename = "MaNGOSThree"
                Case "5"
                    xmlFilename = "MaNGOSFour"
            End Select
            Return xmlFilename
        End Function


        ''' <summary>
        '''     Loads the fieldnames from the config xml for the specified file and returns a Dictionary of all field names
        ''' </summary>
        ''' <param name="sourceFolder"></param>
        ''' <param name="tablename"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadXmlDefinitions(ByRef sourceFolder As String, ByRef tablename As String) As Dictionary(Of Integer, String)
            Dim thisCollection As New Dictionary(Of Integer, String) ' Collection
            Dim myXmlDoc As New XmlDocument()
            Dim xmlFilename As String = ""
            xmlFilename = ReturnDbcxmLfilename()

            If My.Computer.FileSystem.FileExists(sourceFolder & "\" & xmlFilename) = True Then
                myXmlDoc.Load(sourceFolder & "\" & xmlFilename)
                ' Alert(" External XML Definitions used", AlertNewLine.AddCRLF)
            Else
                Dim textStreamReader As StreamReader
                Dim assembly As Assembly
                Try
                    assembly = [assembly].GetExecutingAssembly()
                    textStreamReader = New StreamReader(assembly.GetManifestResourceStream(assembly.GetName.Name.ToString() & "." & xmlFilename))
                    myXmlDoc.Load(textStreamReader)
                    'Alert(" Internal XML Definitions used", AlertNewLine.AddCRLF)
                Catch ex As Exception
                End Try
            End If

            Dim maxCols As Integer = 0
            Dim thisNode As XmlNode
            Dim thisFieldCountNode As XmlNode
            Dim thisFieldNode As XmlNode
            '<root>
            '    <Files>
            '        <Achievement>
            thisNode = myXmlDoc.SelectSingleNode("root")
            If Not IsNothing(thisNode) Then thisNode = thisNode.SelectSingleNode("Files")
            If Not IsNothing(thisNode) Then thisNode = thisNode.SelectSingleNode(tablename)

            If Not IsNothing(thisNode) Then 'found, time to read it
                thisFieldCountNode = thisNode.SelectSingleNode("fieldcount") '<fieldcount>15</fieldcount>
                If Not IsNothing(thisFieldCountNode) Then
                    maxCols = thisFieldCountNode.InnerText '<field type="bigint" name="id" include="y" />
                    thisFieldNode = thisFieldCountNode.NextSibling

                    For thisCol As Integer = 0 To maxCols - 1
                        If Not IsNothing(thisFieldNode) Then
                            thisCollection.Add(thisCol, thisFieldNode.Attributes.GetNamedItem("name").InnerText)
                        End If
                        Try
                            thisFieldNode = thisFieldNode.NextSibling
                        Catch ex As Exception
                        End Try

#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                    Next
                End If

            End If
            Return thisCollection
        End Function


        ''' <summary>
        '''     Converts a hex string to a single.
        ''' </summary>
        ''' <param name="hexValue">The hex value.</param>
        ''' <returns></returns>
        Private Function ConvertHexToSingle(ByVal hexValue As String) As Single
            Try
                Dim iInputIndex As Integer = 0
                Dim iOutputIndex As Integer = 0
                Dim bArray(3) As Byte

                For iInputIndex = 0 To hexValue.Length - 1 Step 2
                    bArray(iOutputIndex) = Byte.Parse(hexValue.Chars(iInputIndex) & hexValue.Chars(iInputIndex + 1), NumberStyles.HexNumber)
                    iOutputIndex += 1
                Next

                Array.Reverse(bArray)

                Return BitConverter.ToSingle(bArray, 0)
            Catch ex As Exception
                Throw New FormatException("The supplied hex value is either empty or in an incorrect format. Use the following format: 00000000", ex)
            End Try
        End Function


        ''' <summary>
        '''     Returns a 2 digit hex string.
        ''' </summary>
        ''' <param name="value">The value.</param>
        ''' <returns>
        '''     <c>String></c>
        ''' </returns>
        Private Function MyHex(ByVal value As Integer) As String
            Dim retString As String
            retString = Hex(value)
            If retString.Length = 1 Then
                retString = "0" & retString
            End If

            Return retString
        End Function

        Private Function ReturnValueType(ByVal hexString As String) As String
            Dim blnFloat As Boolean = False
            Dim blnLong As Boolean = False
            Dim blnInteger As Boolean = False

            If hexString.Length < 8 Then
                hexString = hexString.PadLeft(8, "0") '& hexString
            End If

            Dim textFloat As String = ConvertHexToSingle(hexString)
            If textFloat.Contains("E-") = True Or textFloat.Contains("E+") = True Then
                blnFloat = False
            Else
                blnFloat = True
            End If

            Dim thisTemp As Long
            Try
                thisTemp = "&h" & hexString
            Catch ex As Exception
                thisTemp = Nothing
            End Try

            Dim thisInt As Integer
            If Integer.TryParse(thisTemp, thisInt) = True Then
                blnInteger = True
            End If

            Dim thisLong As Long
            If Long.TryParse(thisTemp, thisLong) = True Then
                blnLong = True
            End If

            'Add some overrides here
            ' &H00000000 is not a float !
            If thisTemp = "&h00000000" Then
                '             TextBox3.Text = "0"
                Return "Int32"
            End If

            If blnFloat = True Then
                '              TextBox3.Text = textFloat
                Return "Float"
            ElseIf blnInteger = True Then
                '               TextBox3.Text = CInt(thisInt)
                Return "Int32"
            ElseIf blnLong = True Then
                '                TextBox3.Text = CLng(thisLong)
                Return "Long"
            Else
                Return "<BANG>"
            End If
        End Function

        Private Function ReturnValue(ByVal hexString As String) As String
            Dim blnFloat As Boolean = False
            Dim blnLong As Boolean = False
            Dim blnInteger As Boolean = False

            If hexString.Length < 8 Then
                hexString = hexString.PadLeft(8, "0") '& hexString
            End If
            Dim textFloat As String = ConvertHexToSingle(hexString)
            If textFloat.Contains("E-") = True Or textFloat.Contains("E+") = True Then
                blnFloat = False
            Else
                blnFloat = True
            End If

            Dim thisTemp As Long
            Try
                thisTemp = "&h" & hexString
            Catch ex As Exception
                thisTemp = Nothing
            End Try

            Dim thisInt As Integer
            If Integer.TryParse(thisTemp, thisInt) = True Then
                blnInteger = True
            End If

            Dim thisLong As Long
            If Long.TryParse(thisTemp, thisLong) = True Then
                blnLong = True
            End If

            'Add some overrides here
            ' &H00000000 is not a float !
            If thisTemp = "&h00000000" Then
                '             TextBox3.Text = "0"
                Return 0
            End If

            If blnFloat = True Then
                Return textFloat
                '                Return "Float"
            ElseIf blnInteger = True Then
                Return CInt(thisInt)
                '                Return "Int32"
            ElseIf blnLong = True Then
                Return CLng(thisLong)
                '                Return "Long"
            Else
                Return 0
            End If
        End Function
    End Module
End Namespace