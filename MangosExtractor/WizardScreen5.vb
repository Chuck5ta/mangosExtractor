Imports System.IO
Imports System.Data
Imports MangosExtractor.Core

Public Class WizardScreen5
''' <summary>
    ''' Exits the Application
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
''' <remarks></remarks>
    Private Sub BtnQuit_Click(sender As Object, e As EventArgs) Handles BtnQuit.Click
        End
    End Sub

    Private Sub WizardScreen5_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = Text & " " & MaNgosExtractorVersion
    End Sub

    ''' <summary>
    ''' Loads the summary.
    ''' </summary>
    ''' <returns></returns>
    Public Sub LoadSummary()
        lstMainLog.Items.Add("Summary Information")
        lstMainLog.Items.Add("===================")
        lstMainLog.Items.Add("")
        lstMainLog.Items.Add("Source WOW Folder: " & WizardScreen1.txtBaseFolder.Text)
        lstMainLog.Items.Add("    Output Folder: " & WizardScreen1.txtOutputFolder.Text)
        lstMainLog.Items.Add("")
        lstMainLog.Items.Add("Extraction Options:")
        lstMainLog.Items.Add("- Extract DBC files from client data: " & WizardScreen2.chkDBC.Checked)
        lstMainLog.Items.Add("- Extract ADT files from client data: " & WizardScreen2.chkExtractADT.Checked)
        lstMainLog.Items.Add("- Extract WDT files from client data: " & WizardScreen2.chkExtractWDT.Checked)
        lstMainLog.Items.Add("- Extract WMO files from client data: " & WizardScreen2.chkExtractWMO.Checked)
        lstMainLog.Items.Add("")
        lstMainLog.Items.Add("Data Export Options:")
        lstMainLog.Items.Add(" - Create a CSV file containing all the DBC Data: " & WizardScreen3.chkCSV.Checked)
        lstMainLog.Items.Add(" - Create a SQL file containing all the DBC Data: " & WizardScreen3.chkSQL.Checked)
        lstMainLog.Items.Add(" - Create an XML File containing the field defs: " & WizardScreen3.chkExportXML.Checked)
        lstMainLog.Items.Add(" - Create some C++ .H headers for the DBC Structures: " & WizardScreen3.chkExportH.Checked)
        lstMainLog.Items.Add(" - Save Default XML column defs: " & WizardScreen3.chkXMLDefs.Checked)
        lstMainLog.Items.Add(" - Create a Markdown page for each DBC: " & WizardScreen3.chkExportMD.Checked)
        lstMainLog.Items.Add(" - Create MaNGOS .Map files from Extracted data: " & WizardScreen3.chkExtractMaps.Checked)
        lstMainLog.Items.Add(" - Create MaNGOS .VMAP files from Extracted data: " & WizardScreen3.chkExtractVmap.Checked)
        lstMainLog.Items.Add(" - Create MaNGOS .MMAP files from Extracted data: " & WizardScreen3.chkExtractMMAPS.Checked)
        lstMainLog.Items.Add("")
    End Sub

    '    Private Sub BrnWdbClick(sender As Object, e As EventArgs)
    '        lstMainLog.Items.Clear()
    '        alertlist = lstMainLog
    '        Cursor = Windows.Forms.Cursors.WaitCursor
    '        brnWDB.Enabled = False
    '        'Dim colBaseFiles As New SortedList()    'Collection containing all the base files
    '        'Dim colMainFiles As New SortedList()    'Collection containing all the main files
    '        'Dim colUpdateFiles As New SortedList()  'Collection containing any update or patch files

    '        'Dim colFolders As New Collection                'Collection to hold for the folders to be processed
    '        Dim myFolders As DirectoryInfo

    '        If Directory.Exists(txtBaseFolder.Text) = False Then
    '            Alert("Warcraft folder '" & txtBaseFolder.Text & "' can not be located", AlertNewLine.AddCRLF)
    '            Exit Sub
    '        End If

    '        'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
    '        txtOutputFolder.Text = txtOutputFolder.Text & "/WDBFiles"
    '        If txtOutputFolder.Text.EndsWith("\") = False Then txtOutputFolder.Text = txtOutputFolder.Text & "\"
    '        If My.Computer.FileSystem.DirectoryExists(txtOutputFolder.Text) = False Then
    '            Directory.CreateDirectory(txtOutputFolder.Text)
    '        End If


    '        'If chkCSV.Checked = True Or chkSQL.Checked = True Then
    '        'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
    '        myFolders = New DirectoryInfo(txtBaseFolder.Text & "/CACHE/WDB/engb")
    '        For Each file As FileInfo In myFolders.GetFiles("*.WDB")
    '            Dim dbcDataTable As New DataTable

    '            'Load the entire DBC into a DataTable to be processed by both exports
    '            '                If chkCSV.Checked = True Or chkSQL.Checked = True Then
    '            Alert("Loading WBC " & file.Name & " into memory", AlertNewLine.AddCRLF)
    '            loadDBCtoDataTable(txtBaseFolder.Text & "/CACHE/WDB/engb" & "/" & file.Name, dbcDataTable)
    '#If _MyType <> "Console" Then
    '            Application.DoEvents()
    '#Else
    '            Threading.Thread.Sleep(0)
    '#End If

    '            'End If

    '            ' If chkSQL.Checked = True Then
    '            Alert("Creating SQL for " & file.Name, AlertNewLine.AddCRLF)
    '            exportSQL(txtOutputFolder.Text & "\" & file.Name, dbcDataTable, txtBaseFolder.Text)
    '#If _MyType <> "Console" Then
    '            Application.DoEvents()
    '#Else
    '            Threading.Thread.Sleep(0)
    '#End If
    '        Next
    '        Alert("Finished Exporting", AlertNewLine.AddCRLF)
    '        'End If
    '        Me.Cursor = Windows.Forms.Cursors.Default
    '        brnWDB.Enabled = True
    '    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        WizardScreen3.Show()
        Hide()
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        lstMainLog.Items.Clear()
        alertlist = lstMainLog
        lstHeader.Items.Clear()
        AlertTitle = lstHeader
        btnBack.Enabled = False

        Cursor = Cursors.WaitCursor
        runAsGui() = True

        Dim colBaseFiles As New SortedList()    'Collection containing all the base files
        Dim colMainFiles As New SortedList()    'Collection containing all the main files
        Dim colUpdateFiles As New SortedList()  'Collection containing any update or patch files

        Dim colFolders As New Collection        'Collection to hold for the folders to be processed
        Dim myFolders As DirectoryInfo

        If Directory.Exists(WizardScreen1.txtBaseFolder.Text) = False Then
            Alert("Warcraft folder '" & WizardScreen1.txtBaseFolder.Text & "' can not be located", AlertNewLine.ADD_CRLF)
            Cursor = Cursors.Default
            btnBack.Enabled = True
            Exit Sub
        End If

        ReadWarcraftExe(WizardScreen1.txtBaseFolder.Text & "/Wow.exe")
        If FullVersion <> "" Then
            Alert("Warcraft Version v" & FullVersion & " Build " & BuildNo, AlertNewLine.ADD_CRLF, lstHeader)
        End If

        If WizardScreen3.chkXMLDefs.Checked = True Then
            SaveXml(WizardScreen1.txtBaseFolder.Text)
        End If

        If WizardScreen2.chkDBC.Checked = True Or WizardScreen3.chkExtractMaps.Checked = True Or WizardScreen2.chkExtractWMO.Checked = True Or WizardScreen2.chkExtractWDT.Checked = True Or WizardScreen2.chkExtractADT.Checked = True Then
            'Set the Top level as {Wow Folder}\data then load all the MPQ files
            myFolders = New DirectoryInfo(WizardScreen1.txtBaseFolder.Text & "/data")

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
#If _MyType <> "Console" Then
                    Application.DoEvents()
#Else
                    Threading.Thread.Sleep(0)
#End If
                Next
            Next

            If WizardScreen1.txtOutputFolder.Text.EndsWith("\") = False And WizardScreen1.txtOutputFolder.Text.EndsWith("/") = False Then
                WizardScreen1.txtOutputFolder.Text = WizardScreen1.txtOutputFolder.Text & "/"
            End If

            If My.Computer.FileSystem.DirectoryExists(WizardScreen1.txtOutputFolder.Text) = False Then
                Directory.CreateDirectory(WizardScreen1.txtOutputFolder.Text)
            End If
        End If

        If WizardScreen2.chkDBC.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractDBCFiles(strItem.Value, "*.db?", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractDBCFiles(strItem.Value, "*.db?", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)

                Try
                    ExtractDBCFiles(strItem.Value, "*.db?", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
            Alert("Extraction Finished", AlertNewLine.ADD_CRLF)
        End If

        If WizardScreen2.chkExtractADT.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractADTFiles(strItem.Value, "*.adt", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractADTFiles(strItem.Value, "*.adt", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)

                Try
                    ExtractADTFiles(strItem.Value, "*.adt", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
        End If

        If WizardScreen3.chkExtractMaps.Checked = True Then
            'Load the DBC Data into the Dictionary dictMaps
            Dim dtMaps As New DataTable
            Dim dictMaps As New Dictionary(Of Integer, String)

            Alert("Loading Maps: ", AlertNewLine.ADD_CRLF)
            dtMaps = LoadDbCtoDataTable(WizardScreen1.txtOutputFolder.Text & "/DBFilesClient" & "/map.dbc")

            For counter As Integer = 0 To dtMaps.Rows.Count() - 2
                dictMaps.Add(dtMaps.Rows(counter)(0), dtMaps.Rows(counter)(1))
            Next

            'Load the DBC Data into the Dictionary dictAreaTable
            Dim dtAreaTable As New DataTable
            Dim dictAreaTable As New Dictionary(Of Integer, String)

            Alert("Loading Areas: ", AlertNewLine.ADD_CRLF)

            dtAreaTable = loadDBCtoDataTable(WizardScreen1.txtOutputFolder.Text & "/DBFilesClient" & "/AreaTable.dbc")

            For counter As Integer = 0 To dtAreaTable.Rows.Count() - 2
                dictAreaTable.Add(dtAreaTable.Rows(counter)(0), dtAreaTable.Rows(counter)(3))
            Next

            'Load the DBC Data into the Dictionary dictLiquidType
            Dim dtLiquidType As New DataTable
            Dim dictLiquidType As New Dictionary(Of Integer, Integer)

            Alert("Loading Liquid Types: ", AlertNewLine.ADD_CRLF)
            dtLiquidType = loadDBCtoDataTable(WizardScreen1.txtOutputFolder.Text & "/DBFilesClient" & "/LiquidType.dbc")


            For counter As Integer = 0 To dtLiquidType.Rows.Count() - 2
                dictLiquidType.Add(dtLiquidType.Rows(counter)(0), dtLiquidType.Rows(counter)(3))
            Next

            Alert("Extraction Finished", AlertNewLine.ADD_CRLF)


            Alert(".... Converting Maps", AlertNewLine.ADD_CRLF)
            Const adtRes As Integer = 64
            Dim mapKey As String = "000"
            Dim mapX As String = "00"
            Dim mapY As String = "00"
            Dim adTfilename As String = ""
            Dim mapFilename As String = ""

            If My.Computer.FileSystem.DirectoryExists(WizardScreen1.txtOutputFolder.Text & "maps/") = False Then
                Directory.CreateDirectory(WizardScreen1.txtOutputFolder.Text & "maps/")
            End If
            For Each thisMap As KeyValuePair(Of Integer, String) In dictMaps
                Alert(" Extracting..." & thisMap.Value, AlertNewLine.ADD_CRLF)
                For x As Integer = 0 To adtRes
                    For y As Integer = 0 To adtRes
                        adTfilename = WizardScreen1.txtOutputFolder.Text & "World/maps/" & thisMap.Value & "/" & thisMap.Value & "_" & x & "_" & y & ".adt"
                        If My.Computer.FileSystem.FileExists(adTfilename) = True Then
                            Alert("Reading from: " & adTfilename, AlertNewLine.ADD_CRLF)
                            mapKey = thisMap.Key.ToString() '"000"
                            If mapKey.Length() = 1 Then mapKey = "00" & mapKey
                            If mapKey.Length() = 2 Then mapKey = "0" & mapKey
                            mapX = x.ToString() '"00"
                            If mapX.Length() = 1 Then mapX = "0" & mapX
                            mapY = y.ToString() '"00"
                            If mapY.Length() = 1 Then mapY = "0" & mapY


                            'ADTReader.Program.Dump(ADTfilename)
                            mapFilename = WizardScreen1.txtOutputFolder.Text & "maps/" & mapKey.Substring(0, 3) & mapY.Substring(0, 2) & mapX.Substring(0, 2) & ".map"
                            Program.ConvertADT(adTfilename, mapFilename, x, y, dictMaps, dictAreaTable, dictLiquidType)
                            'Alert(" Writing to: " & MapFilename, AlertNewLine.AddCRLF)
                            'ConvertADT(ADTfilename, MapFilename, dictMaps, dictAreaTable, dictLiquidType)
                        End If
#If _MyType <> "Console" Then
                        Application.DoEvents()
#Else
                        Threading.Thread.Sleep(0)
#End If
                    Next
                Next
            Next
        End If


        If WizardScreen2.chkExtractWMO.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractFilesGeneric(strItem.Value, "*.wmo", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractFilesGeneric(strItem.Value, "*.wmo", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)

                Try
                    ExtractFilesGeneric(strItem.Value, "*.wmo", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
        End If

        If WizardScreen2.chkExtractWDT.Checked = True Then
            For Each strItem As DictionaryEntry In colBaseFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractWDTFiles(strItem.Value, "*.wdt", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colMainFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)
                Try
                    ExtractWDTFiles(strItem.Value, "*.wdt", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next

            For Each strItem As DictionaryEntry In colUpdateFiles
                Alert("Reading: " & strItem.Value, AlertNewLine.ADD_CRLF)

                Try
                    ExtractWDTFiles(strItem.Value, "*.wdt", WizardScreen1.txtOutputFolder.Text)
                Catch ex As Exception
                    Alert(ex.Message, AlertNewLine.ADD_CRLF)
                End Try
#If _MyType <> "Console" Then
                Application.DoEvents()
#Else
                Threading.Thread.Sleep(0)
#End If
            Next
        End If

        If WizardScreen3.chkCSV.Checked = True Or WizardScreen3.chkSQL.Checked = True Or WizardScreen3.chkExportXML.Checked = True Or WizardScreen3.chkExportMD.Checked = True Or WizardScreen3.chkExportH.Checked = True Then
            'Now that we have all the DBC's extracted and patched, we need to check the export options and export data
            ExportDbcFiles(WizardScreen1.txtBaseFolder.Text, WizardScreen1.txtOutputFolder.Text, WizardScreen3.chkCSV.Checked, WizardScreen3.chkSQL.Checked, WizardScreen3.chkExportXML.Checked, WizardScreen3.chkExportMD.Checked, WizardScreen3.chkExportH.Checked)
            Alert("Finished Exporting", AlertNewLine.ADD_CRLF)
        End If

        Cursor = Cursors.Default

        Alert("Finished", AlertNewLine.ADD_CRLF)
        btnBack.Enabled = True
    End Sub
End Class