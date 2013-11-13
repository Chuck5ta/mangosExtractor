Imports System.IO
Imports System.Runtime.InteropServices
Imports MangosExtractor.Core
Imports System.Text

Module ADTReader
    Public NotInheritable Class Extensions
        Private Sub New()
        End Sub

        '<System.Runtime.CompilerServices.Extension> _
        Public Shared Function ReadStruct (Of T As Structure)(s As Stream) As T
            Dim sz = Marshal.SizeOf(GetType(T))
            Dim buf = New Byte(sz - 1) {}
            s.Read(buf, 0, sz)
            Dim handle = GCHandle.Alloc(buf, GCHandleType.Pinned)
            Dim t2 = DirectCast(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), GetType(T)), T)
            handle.Free()
            Return t2
        End Function
    End Class

    Public Class ChunkStream
        Inherits Stream
        Private ReadOnly _start As Long
        Private ReadOnly _length As Long
        Private _position As Long
        Private ReadOnly _parent As Stream

        Public ReadOnly Property Parent() As Stream
            Get
                Return _parent
            End Get
        End Property

        Public Property Identifier() As String
            Get
                Return _mIdentifier
            End Get
            Private Set(value As String)
                _mIdentifier = value
            End Set
        End Property

        Private _mIdentifier As String

        Public Sub New(parent As Stream)
            Dim reader = New BinaryReader(parent)
            SyncLock parent
                _parent = parent

                ' read in header
                Dim id = reader.ReadBytes(4)
                Identifier = "" & ChrW(id(3)) & ChrW(id(2)) & ChrW(id(1)) & ChrW(id(0))
                _length = reader.ReadUInt32()
                _start = _parent.Position
            End SyncLock
        End Sub

        Public Function Ascend() As ChunkStream
            Return TryCast(_parent, ChunkStream)
        End Function

        Public Function Descend() As ChunkStream
            SyncLock _parent
                Dim pos = _start
                _parent.Position = pos
                Return If(pos + 8 > _parent.Length, Nothing, New ChunkStream(_parent))
            End SyncLock
        End Function

        Public Function [Next]() As ChunkStream
            SyncLock _parent
                Dim pos = _start + _length
                _parent.Position = pos
                Return If(pos + 8 > _parent.Length, Nothing, New ChunkStream(_parent))
            End SyncLock
        End Function

#Region "unsupported"

        Public Overrides Sub Flush()
            Throw New NotImplementedException()
        End Sub

        Public Overrides Function Seek(offset As Long, origin As SeekOrigin) As Long
            Throw New NotImplementedException()
        End Function

        Public Overrides Sub SetLength(value As Long)
            Throw New NotImplementedException()
        End Sub

        Public Overrides Sub Write(buffer As Byte(), offset As Integer, count As Integer)
            Throw New NotImplementedException()
        End Sub

#End Region

        Public Overrides Function Read(buffer As Byte(), offset As Integer, count As Integer) As Integer
            SyncLock _parent
                'var oldPos = _parent.Position;
                Try
                    _parent.Position = _position + _start
                    Return _parent.Read(buffer, offset, count)
                Finally
                    '_parent.Position = oldPos + (_parent.Position - _position);
                    _position = _parent.Position - _start
                End Try
            End SyncLock
        End Function

        Public Overrides ReadOnly Property CanRead() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanSeek() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overrides ReadOnly Property CanWrite() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overrides ReadOnly Property Length() As Long
            Get
                Return _length
            End Get
        End Property

        Public Overrides Property Position() As Long
            Get
                Return _position
            End Get
            Set(value As Long)
                If value > _length Then
                    Throw New ArgumentOutOfRangeException()
                End If
                _position = value
                SyncLock _parent
                    _parent.Position = _start + _position
                End SyncLock
            End Set
        End Property
    End Class

    <StructLayout(LayoutKind.Sequential, Size := 4)>
    Public Structure Mhdr
        Public Flags As UInteger
        Public mcin As UInteger
        Public mtex As UInteger
        Public mmdx As UInteger
        Public mmid As UInteger
        Public mwmo As UInteger
        Public mwid As UInteger
        Public mddf As UInteger
        Public modf As UInteger
        Public mfbo As UInteger
        ' this is only set if flags & mhdr_MFBO.
        Public mh2o As UInteger
        Public mtfx As UInteger
    End Structure

    <StructLayout(LayoutKind.Sequential, Size := 4)>
    Public Structure Vec3F
        Public X As Single, Y As Single, Z As Single
    End Structure

    <StructLayout(LayoutKind.Sequential, Size := 4)>
    Public Structure Mcin
        Dim mcnk As UInteger           ' absolute offset.
        Dim size As UInteger           ' the size of the MCNK chunk, this is refering to.
        Dim flags As UInteger          ' these two are always 0. only set in the client.
        Dim asyncId As UInteger
    End Structure

    '<StructLayout(LayoutKind.Sequential, Size:=4)> _
    'Public Structure MCLQ
    '    Dim xCoord As UInt16           ' absolute offset.
    '    Dim yCoord As UInt16           ' the size of the MCNK chunk, this is refering to.
    '    Dim height As Double          ' these two are always 0. only set in the client.
    'End Structure

    Friend Structure LiquidData
        Dim Light As UInt32
        Dim Height As Double
        Dim Data() As SByte
    End Structure

    <StructLayout(LayoutKind.Sequential, Size := 4)>
    Public Structure Mclq
        Dim height1 As Double
        Dim height2 As Double
        Dim liquid(,) As LiquidData
        '            Dim liquid(ADT_CELL_SIZE + 1, ADT_CELL_SIZE + 1) As liquid_data

        '// 1<<0 - ocean
        '// 1<<1 - lava/slime
        '// 1<<2 - water
        '// 1<<6 - all water
        '// 1<<7 - dark water
        '// == 0x0F - do not show liquid
        'Dim flags(ADT_CELL_SIZE, ADT_CELL_SIZE) As SByte
        Dim flags(,) As SByte
        'Dim data(84) As SByte
        Dim prepareLoadedData() As Boolean
    End Structure

    <StructLayout(LayoutKind.Sequential, Size := 4)>
    Public Structure Mcvt
        Dim height As Double          ' 
    End Structure

    'Private m_MH20 As ChunkStream

    <StructLayout(LayoutKind.Sequential, Size := 128)>
    Public Structure Mcnk
        Public flags As UInteger
        Public IndexX As UInteger
        Public IndexY As UInteger
        Public nLayers As UInteger
        ' maximum 4
        Public nDoodadRefs As UInteger
        Public ofsHeight As UInteger
        Public ofsNormal As UInteger
        Public ofsLayer As UInteger
        Public ofsRefs As UInteger
        Public ofsAlpha As UInteger
        Public sizeAlpha As UInteger
        Public ofsShadow As UInteger
        ' only with flags&0x1
        Public sizeShadow As UInteger
        Public areaid As UInteger
        Public nMapObjRefs As UInteger
        Public holes As UInteger

        Public reallyLowQualityTextureingMap0 As UInteger
        Public reallyLowQualityTextureingMap1 As UInteger
        Public reallyLowQualityTextureingMap2 As UInteger
        Public reallyLowQualityTextureingMap3 As UInteger

        Public predTex As UInteger
        ' 03-29-2005 By ObscuR; TODO: Investigate
        Public noEffectDoodad As UInteger
        ' 03-29-2005 By ObscuR; TODO: Investigate
        Public ofsSndEmitters As UInteger
        Public nSndEmitters As UInteger
        'will be set to 0 in the client if ofsSndEmitters doesn&#039;t point to MCSE!
        Public ofsLiquid As UInteger
        Public sizeLiquid As UInteger
        ' 8 when not used; only read if >8.
        Public position As Vec3F
        Public ofsMCCV As UInteger
        ' only with flags&0x40, had UINT32 textureId; in ObscuR&#039;s structure.
        Public ofsMCLV As UInteger
        ' introduced in Cataclysm
    End Structure

    Public Class MapParser
        Public Const Size As Integer = 16*17
        Public Data As Single(,) = New Single(Size - 1, Size - 1) {}

        Public Property MaxV() As Single
            Get
                Return _mMaxV
            End Get
            Private Set(value As Single)
                _mMaxV = value
            End Set
        End Property

        Private _mMaxV As Single

        Public Property MinV() As Single
            Get
                Return _mMinV
            End Get
            Private Set(value As Single)
                _mMinV = value
            End Set
        End Property

        Private _mMinV As Single

        Private m_MCNK As MCNK
        Private m_MCIN As MCIN
        Private m_MH20 As ChunkStream
        Private m_MCVT As MCVT
        Private m_MCLQ As MCLQ

        Public ReadOnly Property Mcnk As MCNK
            Get
                Return m_MCNK
            End Get
        End Property

        Public ReadOnly Property Mcin As MCIN
            Get
                Return m_MCIN
            End Get
        End Property

        Public ReadOnly Property Mh20 As ChunkStream
            Get
                Return m_MH20
            End Get
        End Property

        Public ReadOnly Property Mcvt As MCVT
            Get
                Return m_MCVT
            End Get
        End Property

        Public ReadOnly Property Mclq As MCLQ
            Get
                Return m_MCLQ
            End Get
        End Property

        Public Sub New(adt As Stream)
            MaxV = [Single].MinValue
            MinV = [Single].MaxValue

            Dim chunk = New ChunkStream(adt)
            If chunk.Identifier <> "MVER" Then
                Throw New Exception()
            End If
            chunk = chunk.[Next]()
            ' skip MVER
            If chunk.Identifier <> "MHDR" Then
                Throw New Exception()
            End If
            Dim hdr = Extensions.ReadStruct (Of MHDR)(chunk)

            While chunk IsNot Nothing
                If chunk.Identifier = "MCNK" Then
                    m_MCNK = Extensions.ReadStruct (Of MCNK)(chunk)
                    'Core.Alert("Found: MCNK", Core.AlertNewLine.AddCRLF)
                    'ParseMapPiece(chunk)
                ElseIf chunk.Identifier = "MCIN" Then
                    m_MCIN = Extensions.ReadStruct (Of MCIN)(chunk)
                    'Core.Alert("Found: MCIN", Core.AlertNewLine.AddCRLF)
                    'ParseMapPiece(chunk)
                ElseIf chunk.Identifier = "MH20" Then
                    m_MH20 = chunk
                    'Core.Alert("Found: MH20", Core.AlertNewLine.AddCRLF)
                    'ParseMapPiece(chunk)
                ElseIf chunk.Identifier = "MCVT" Then
                    m_MCVT = Extensions.ReadStruct (Of MCVT)(chunk)
                    Alert("Found: MCVT", MaNgosExtractorCore.AlertNewLine.ADD_CRLF)
                    ' ParseMapPiece(chunk)
                ElseIf chunk.Identifier = "MCLQ" Then
                    m_MCLQ = Extensions.ReadStruct (Of MCLQ)(chunk)
                    'Core.Alert("Found: MCLQ", Core.AlertNewLine.AddCRLF)
                    'ParseMapPiece(chunk)
                Else
                    ' Console.WriteLine(chunk.Identifier)
                End If
                chunk = chunk.[Next]()
            End While
        End Sub

        Private Sub ParseMapPiece(s As ChunkStream)
            Dim hdr = Extensions.ReadStruct (Of MCNK)(s)
            Dim ofs = hdr.ofsHeight - 8
            Dim z = hdr.position.Z

            s.Position = ofs
            Dim mcvt = New ChunkStream(s)

            Dim r = New BinaryReader(mcvt)

            Dim xoff As Long = hdr.IndexX*17
            Dim yoff As Long = hdr.IndexY*17

            ' read the data points
            Dim y = 0
            While True
                For x As Integer = 0 To 8
                    Dim v = r.ReadSingle() + z
                    If v > MaxV Then
                        MaxV = v
                    End If
                    If v < MinV Then
                        MinV = v
                    End If
                    Data(yoff + y, (xoff + x)*2) = v
                Next
                y += 1
                If y = 17 Then
                    Exit While
                End If
                For x As Integer = 0 To 7
                    Dim v = r.ReadSingle() + z
                    If v > MaxV Then
                        MaxV = v
                    End If
                    If v < MinV Then
                        MinV = v
                    End If
                    Data(yoff + y, (xoff + x)*2 + 1) = v
                Next
                y += 1
            End While


            ' interpolate mid values
            ' (might use a different scheme here..)
            For y = 0 To 16 Step 2
                For x As Integer = 0 To 7
                    Dim pl = Data(yoff + y, xoff + x*2)
                    Dim pr = Data(yoff + y, xoff + x*2 + 2)
                    Data(yoff + y, xoff + x*2 + 1) = (pl + pr)*0.5F

                    pl = Data(yoff + x*2, xoff + y)
                    pr = Data(yoff + x*2 + 2, xoff + y)
                    Data(yoff + x*2 + 1, xoff + y) = (pl + pr)*0.5F
                Next
            Next
        End Sub

        'Public Sub Dump(filename As String)
        '    Dim d = MaxV - MinV
        '    Dim scale = 255.0F / d
        '    Dim bmp = New Bitmap(Size, Size)
        '    For y As Integer = 0 To Size - 1
        '        For x As Integer = 0 To Size - 1
        '            Dim v = CInt(Math.Truncate((Data(y, x) - MinV) * scale))
        '            If v < 0 Then
        '                v = 0
        '            End If
        '            If v > 255 Then
        '                v = 255
        '            End If
        '            bmp.SetPixel(x, y, Color.FromArgb(v, v, v))
        '        Next
        '    Next

        '    bmp.Save(filename)
        'End Sub
    End Class

    Class Program
        'Public Shared Sub Dump(data As Single(,), size As Integer, min As Single, max As Single, xoff As Integer, yoff As Integer, _
        '    filename As String)
        '    Dim d = max - min
        '    Dim scale = 255.0F / d
        '    Dim bmp = New Bitmap(size, size)
        '    For y As Integer = 0 To size - 1
        '        For x As Integer = 0 To size - 1
        '            Dim v = CInt(Math.Truncate((data(y + yoff, x + xoff) - min) * scale))
        '            If v < 0 Then
        '                v = 0
        '            End If
        '            If v > 255 Then
        '                v = 255
        '            End If
        '            bmp.SetPixel(x, y, Color.FromArgb(v, v, v))
        '        Next
        '    Next
        '    Try
        '        bmp.Save(filename)
        '    Catch
        '    End Try
        '    bmp.Dispose()
        'End Sub

        Private Structure MapAreaHeader
            Dim Fourcc As UInt32
            Dim Flags As UInt16
            Dim GridArea As UInt16
        End Structure

        Private Structure MapHeightHeader
            Dim Fourcc As UInt32
            Dim Flags As UInt32
            Dim GridHeight As Single
            Dim GridMaxHeight As Single
        End Structure


        Private Shared Function SelectUInt8StepStore(maxDiff As Single) As Single
            If maxDiff = 0 Then maxDiff = 1
            Return 255/maxDiff
        End Function

        Private Shared Function SelectUInt16StepStore(maxDiff As Single) As Single
            If maxDiff = 0 Then maxDiff = 1
            Return 65535/maxDiff
        End Function

        Public Shared Sub ConvertAdt(adtfilename As String, ByRef outputFilename As String, mapx As Integer, mapy As Integer, dictMaps As Dictionary(Of Integer, String), dictAreaTable As Dictionary(Of Integer, String), dictLiquidType As Dictionary(Of Integer, Integer))
            'these need to be options switches
            Dim confAllowHeightLimit As Boolean = True
            Dim confUseMinHeight As Double = - 500.0F


            'Const megaSize As Integer = 64 * MapParser.Size
            'Dim megamap = New Single(megaSize - 1, megaSize - 1) {}

            Dim adtCellsPerGrid As Integer = 16
            Dim adtCellSize As Integer = 8
            Dim adtGridSize As Integer = (adtCellsPerGrid*adtCellSize)

            Dim areaFlags(adtCellsPerGrid, adtCellsPerGrid) As UInt16

            Dim v8(adtGridSize, adtGridSize) As Double
            Dim v9(adtGridSize + 1, adtGridSize + 1) As Double
            Dim uint16V8(adtGridSize, adtGridSize) As UInt16
            Dim uint16V9(adtGridSize + 1, adtGridSize + 1) As UInt16
            Dim uint8V8(adtGridSize, adtGridSize) As SByte
            Dim uint8V9(adtGridSize + 1, adtGridSize + 1) As SByte

            Dim liquidEntry(adtCellsPerGrid, adtCellsPerGrid) As UInt16
            Dim liquidFlags(adtCellsPerGrid, adtCellsPerGrid) As SByte
            Dim liquidShow(adtGridSize, adtGridSize) As Boolean
            Dim liquidHeight(adtGridSize + 1, adtGridSize + 1) As Double

            Dim mapAreaNOArea = &H1

            Dim mapHeightNOHeight = &H1
            Dim mapHeightASInt16 = &H2
            Dim mapHeightASInt8 = &H4


            Dim mapLiquidTypeNoWater = &H0
            Dim mapLiquidTypeMagma = &H1
            Dim mapLiquidTypeOcean = &H2
            Dim mapLiquidTypeSlime = &H4
            Dim mapLiquidTypeWater = &H8

            Dim mapLiquidTypeDarkWater = &H10
            Dim mapLiquidTypeWmoWater = &H20


            Dim mapLiquidNoType = &H1
            Dim mapLiquidNoHeight = &H2

            ' This option allow use float to int conversion
            Dim confAllowFloatToInt As Boolean = True
            Dim confFloatToInt8Limit As Double = 2.0F  '    // Max accuracy = val/256
            Dim confFloatToInt16Limit As Double = 2048.0F '   // Max accuracy = val/65536
            Dim confFlatHeightDeltaLimit As Double = 0.005F ' // If max - min less this value - surface is flat
            Dim confFlatLiquidDeltaLimit As Double = 0.001F ' // If max - min less this value - liquid surface is flat


            Dim maxV = [Single].MinValue
            Dim minV = [Single].MaxValue

            'For y As Integer = 0 To 63
            '    For x As Integer = 0 To 63
            Dim filename = adtfilename '& "_" & y & "_" & x & ".adt"
            'If Not File.Exists(filename) Then
            '    Continue For
            'End If

            Console.WriteLine(filename)
            Dim f = New FileStream(filename, FileMode.Open, FileAccess.Read)
            Dim map = New MapParser(f)

            '' copy to megamap
            'Dim yoff = MapParser.Size * x
            'Dim xoff = MapParser.Size * y
            'For yy As Integer = 0 To MapParser.Size - 1
            '    For xx As Integer = 0 To MapParser.Size - 1
            '        megamap(yoff + yy, xoff + xx) = map.Data(yy, xx)
            '    Next
            'Next

            If map.MaxV > maxV Then
                maxV = map.MaxV
            End If
            If map.MinV < minV Then
                minV = map.MinV
            End If

            'Core.Alert("Length:" & map.Size.ToString(), Core.AlertNewLine.AddCRLF)

#If _MyType <> "Console" Then
            Application.DoEvents()
#Else
            Threading.Thread.Sleep(0)
#End If
            '    Next
            'Next


            'Write out the .Map file
            Dim sqlWriter As New StreamWriter(outputFilename)
            Dim areaHdr As New StringBuilder("AREA")
            Dim mhgtHdr As New StringBuilder("MHGT")
            Dim mliqHdr As New StringBuilder("MLIQ")
            Dim holeHdr As New StringBuilder
            'Dim AREAData As New StringBuilder
            'Dim MHGTData As New StringBuilder
            'Dim MLIQData As New StringBuilder
            'Dim HOLEData As New StringBuilder
            'AREAData.Append(map.Data(mapx, mapy).ToString())

            'Write Identifier
            sqlWriter.Write("MAPS")

            Select Case MajorVersion
                Case "1"
                    'Write version No
                    sqlWriter.Write("z1.3")
                Case "2"
                    'Write version No
                    sqlWriter.Write("s1.3")
                Case "3"
                    'Write version No
                    sqlWriter.Write("v1.3")
                Case "4"
                    'Write version No
                    sqlWriter.Write("v1.2")
                Case "5"
                    'Write version No
                    sqlWriter.Write("v1.2")
                Case Else
                    sqlWriter.Write("????")
            End Select


            ' Get area flags data
            Dim thisMcnk As MCNK = map.Mcnk

            For i As Integer = 0 To adtCellsPerGrid - 1
                For j As Integer = 0 To adtCellsPerGrid - 1
                    thisMcnk.IndexX = i
                    thisMcnk.IndexY = j
                    If thisMcnk.areaid > 0 Then
                        If dictAreaTable(thisMcnk.areaid) <> &HFFFF Then
                            areaFlags(i, j) = dictAreaTable(thisMcnk.areaid)
                            Continue For
                        Else
                            Alert("File: " & filename & " Can't find area flag for areaid " & thisMcnk.areaid & " [" & i & "," & j & "].", AlertNewLine.ADD_CRLF)
                        End If
                    End If

                    areaFlags(i, j) = &HFFFF
                Next
            Next

            '============================================
            ' Try pack area data
            '============================================
            Dim fullAreaData As Boolean = False
            Dim areaflag As UInt32 = areaFlags(0, 0)
            For y As Integer = 0 To adtCellsPerGrid - 1
                For x As Integer = 0 To adtCellsPerGrid - 1
                    If areaFlags(y, x) <> areaflag Then
                        fullAreaData = True
                        Exit For
                    End If
                Next
            Next

            '          map.areaMapOffset = sizeof(map);
            'map.areaMapSize   = sizeof(map_areaHeader);

            Dim areaHeader As MapAreaHeader
            areaHeader.fourcc = 1095910721 'AREA 
            areaHeader.flags = 0
            If (fullAreaData) Then

                areaHeader.gridArea = 0
                'map.areaMapSize += sizeof(area_flags)

            Else

                areaHeader.flags = mapAreaNOArea
                areaHeader.gridArea = CShort(thisMcnk.flags)
            End If


            '
            ' Get Height map from grid
            '
            For i As Integer = 0 To adtCellsPerGrid - 1
                For j As Integer = 0 To adtCellsPerGrid - 1
                    thisMcnk.IndexX = i
                    thisMcnk.IndexY = j

                    'Dim cell As Pointer(Of adt_MCNK) = cells.getMCNK(i, j)
                    'If Not cell Then
                    '    Continue For
                    'End If
                    ' Height values for triangles stored in order:
                    ' 1     2     3     4     5     6     7     8     9
                    '    10    11    12    13    14    15    16    17
                    ' 18    19    20    21    22    23    24    25    26
                    '    27    28    29    30    31    32    33    34
                    ' . . . . . . . .
                    ' For better get height values merge it to V9 and V8 map
                    ' V9 height map:
                    ' 1     2     3     4     5     6     7     8     9
                    ' 18    19    20    21    22    23    24    25    26
                    ' . . . . . . . .
                    ' V8 height map:
                    '    10    11    12    13    14    15    16    17
                    '    27    28    29    30    31    32    33    34
                    ' . . . . . . . .

                    ' Set map height as grid height
                    For y As Integer = 0 To adtCellSize
                        Dim cy As Integer = i*adtCellSize + y
                        For x As Integer = 0 To adtCellSize
                            Dim cx As Integer = j*adtCellSize + x
                            v9(cy, cx) = thisMcnk.position.Y ' cell.ypos
                        Next
                    Next
                    For y As Integer = 0 To adtCellSize - 1
                        Dim cy As Integer = i*adtCellSize + y
                        For x As Integer = 0 To adtCellSize - 1
                            Dim cx As Integer = j*adtCellSize + x
                            v8(cy, cx) = thisMcnk.position.Y ' cell.ypos
                        Next
                    Next
                    ' Get custom height
                    Dim thisMCVT As MCVT = map.Mcvt


                    'Dim v As Double = thisMCVT.height '  thisMCNK.ofsHeight ' cell.getMCVT()
                    If Not IsNothing(thisMCVT) Then
                        Continue For
                    End If
                    ' get V9 height map
                    For y As Integer = 0 To adtCellSize
                        Dim cy As Integer = i*adtCellSize + y
                        For x As Integer = 0 To adtCellSize
                            Dim cx As Integer = j*adtCellSize + x
                            v9(cy, cx) += thisMCVT.height '.height_map(y * (ADT_CELL_SIZE * 2 + 1) + x)
                        Next
                    Next
                    ' get V8 height map
                    For y As Integer = 0 To adtCellSize - 1
                        Dim cy As Integer = i*adtCellSize + y
                        For x As Integer = 0 To adtCellSize - 1
                            Dim cx As Integer = j*adtCellSize + x
                            v8(cy, cx) += thisMCVT.height 'v.height_map(y * (ADT_CELL_SIZE * 2 + 1) + ADT_CELL_SIZE + 1 + x)
                        Next
                    Next
                Next
            Next

            '============================================
            ' Try pack height data
            '============================================
            Dim maxHeight As Single = - 20000
            Dim minHeight As Single = 20000
            For y As Integer = 0 To adtGridSize - 1
                For x As Integer = 0 To adtGridSize - 1
                    Dim h As Single = v8(y, x)
                    If maxHeight < h Then
                        maxHeight = h
                    End If
                    If minHeight > h Then
                        minHeight = h
                    End If
                Next
            Next
            For y As Integer = 0 To adtGridSize
                For x As Integer = 0 To adtGridSize
                    Dim h As Single = v9(y, x)
                    If maxHeight < h Then
                        maxHeight = h
                    End If
                    If minHeight > h Then
                        minHeight = h
                    End If
                Next
            Next

            ' Check for allow limit minimum height (not store height in deep ochean - allow save some memory)
            If confAllowHeightLimit = True AndAlso minHeight < confUseMinHeight Then
                For y As Integer = 0 To adtGridSize - 1
                    For x As Integer = 0 To adtGridSize - 1
                        If v8(y, x) < confUseMinHeight Then
                            v8(y, x) = confUseMinHeight
                        End If
                    Next
                Next
                For y As Integer = 0 To adtGridSize
                    For x As Integer = 0 To adtGridSize
                        If v9(y, x) < confUseMinHeight Then
                            v9(y, x) = confUseMinHeight
                        End If
                    Next
                Next
                If minHeight < confUseMinHeight Then
                    minHeight = confUseMinHeight
                End If
                If maxHeight < confUseMinHeight Then
                    maxHeight = confUseMinHeight
                End If
            End If


            '        map.heightMapOffset = map.areaMapOffset + map.areaMapSize;
            'map.heightMapSize = sizeof(map_heightHeader);

            Dim heightHeader As MapHeightHeader
            heightHeader.fourcc = 1296582484 'MGHT
            heightHeader.flags = 0
            heightHeader.gridHeight = minHeight
            heightHeader.gridMaxHeight = maxHeight

            If (maxHeight = minHeight) Then
                heightHeader.flags = mapHeightNOHeight
            End If

            ' Not need store if flat surface
            If (confAllowFloatToInt And (maxHeight - minHeight) < confFlatHeightDeltaLimit) Then
                heightHeader.flags = mapHeightNOHeight
            End If

            ' Try store as packed in uint16 or uint8 values
            If Not (heightHeader.flags And mapHeightNOHeight) Then
                Dim [step] As Single
                ' Try Store as uint values
                If confAllowFloatToInt Then
                    Dim diff As Single = maxHeight - minHeight
                    If diff < confFloatToInt8Limit Then
                        ' As uint8 (max accuracy = CONF_float_to_int8_limit/256)
                        heightHeader.flags = heightHeader.flags Or mapHeightASInt8
                        [step] = SelectUInt8StepStore(diff)
                    ElseIf diff < confFloatToInt16Limit Then
                        ' As uint16 (max accuracy = CONF_float_to_int16_limit/65536)
                        heightHeader.flags = heightHeader.flags Or mapHeightASInt16
                        [step] = SelectUInt16StepStore(diff)
                    End If
                End If

                ' Pack it to int values if need
                If heightHeader.flags And mapHeightASInt8 Then
                    For y As Integer = 0 To adtGridSize - 1
                        For x As Integer = 0 To adtGridSize - 1
                            uint8V8(y, x) = Convert.ToSingle((v8(y, x) - minHeight)*[step] + 0.5F)
                        Next
                    Next
                    For y As Integer = 0 To adtGridSize
                        For x As Integer = 0 To adtGridSize
                            uint8V9(y, x) = Convert.ToSingle((v9(y, x) - minHeight)*[step] + 0.5F)
                        Next
                    Next
                    'map.heightMapSize += sizeof(uint8_V9) + sizeof(uint8_V8)
                ElseIf heightHeader.flags And mapHeightASInt16 Then
                    For y As Integer = 0 To adtGridSize - 1
                        For x As Integer = 0 To adtGridSize - 1
                            uint16V8(y, x) = Convert.ToSingle((v8(y, x) - minHeight)*[step] + 0.5F)
                        Next
                    Next
                    For y As Integer = 0 To adtGridSize
                        For x As Integer = 0 To adtGridSize
                            uint16V9(y, x) = Convert.ToSingle((v9(y, x) - minHeight)*[step] + 0.5F)
                        Next
                    Next
                    'map.heightMapSize += sizeof(uint16_V9) + sizeof(uint16_V8)
                Else
                    'map.heightMapSize += sizeof(V9) + sizeof(V8)
                End If
            End If

            ' Get from MCLQ chunk (old)
            For i As Integer = 0 To adtCellsPerGrid - 1
                For j As Integer = 0 To adtCellsPerGrid - 1
                    'Dim cell As Pointer(Of adt_MCNK) = cells.getMCNK(i, j)
                    'If Not cell Then
                    '    Continue For
                    'End If
                    Dim thisMCLQ As MCLQ = map.Mclq

                    Dim liquid(,) As LiquidData = thisMCLQ.liquid ' Double = thisMCLQ.liquid(i, j).height
                    Dim count As Integer = 0

                    If IsNothing(thisMCLQ.liquid) = True Then 'OrElse cell.sizeMCLQ <= 8 Then
                        Continue For
                    End If

                    For y As Integer = 0 To adtCellSize - 1
                        Dim cy As Integer = i*adtCellSize + y
                        For x As Integer = 0 To adtCellSize - 1
                            Dim cx As Integer = j*adtCellSize + x
                            If thisMCLQ.flags(y, x) <> &HF Then
                                liquidShow(cy, cx) = True
                                If thisMCLQ.flags(y, x) And (1 << 7) Then
                                    liquidFlags(i, j) = liquidFlags(i, j) Or mapLiquidTypeDarkWater
                                End If
                                count += 1
                            End If
                        Next
                    Next

                    Dim cFlag As UInt32 = map.Mcnk.flags
                    If cFlag And (1 << 2) Then
                        liquidEntry(i, j) = 1
                        ' water
                        liquidFlags(i, j) = liquidFlags(i, j) Or mapLiquidTypeWater
                    End If
                    If cFlag And (1 << 3) Then
                        liquidEntry(i, j) = 2
                        ' ocean
                        liquidFlags(i, j) = liquidFlags(i, j) Or mapLiquidTypeOcean
                    End If
                    If cFlag And (1 << 4) Then
                        liquidEntry(i, j) = 3
                        ' magma/slime
                        liquidFlags(i, j) = liquidFlags(i, j) Or mapLiquidTypeMagma
                    End If

                    If Not count AndAlso liquidFlags(i, j) Then
                        MsgBox("Wrong liquid detect in MCLQ chunk")
                    End If

                    For y As Integer = 0 To adtCellSize
                        Dim cy As Integer = i*adtCellSize + y
                        For x As Integer = 0 To adtCellSize
                            Dim cx As Integer = j*adtCellSize + x
                            liquidHeight(cy, cx) = map.Mclq.liquid(y, x).height
                            'MLIQData.Append(map.MCLQ.liquid(y, x).height)
                        Next
                    Next
                Next
            Next


            'Write Area Offset (always 40)
            sqlWriter.Write(Chr(40) & Chr(0) & Chr(0) & Chr(0))

            'Write Areasize                                 s
            'Dim test1 As String = Hex(MCNK)
            'test1 = Strings.StrDup(8 - test1.Length(), "0") & test1

            'Dim byte1 As Integer = Convert.ToInt32(test1.Substring(0, 2), 16)
            'Dim byte2 As Integer = Convert.ToInt32(test1.Substring(2, 2), 16)
            'Dim byte3 As Integer = Convert.ToInt32(test1.Substring(4, 2), 16)
            'Dim byte4 As Integer = Convert.ToInt32(test1.Substring(6, 2), 16)
            'MHGTHdr.Append(Chr(byte4.ToString()) & Chr(byte3.ToString()) & Chr(byte2.ToString()) & Chr(byte1.ToString()))
            mhgtHdr.Append(Chr(8) & Chr(0) & Chr(0) & Chr(0))

            'Write MHGT Offset
            sqlWriter.Write(Chr(48) & Chr(0) & Chr(0) & Chr(0))

            'MHGT Size:	4 Bytes		=	10 00 00 00 (16 Bytes)
            sqlWriter.Write(Chr(16) & Chr(0) & Chr(0) & Chr(0))

            'MLIQ Offset:	4 Bytes		=	40 02 00 00 (576 Bytes)
            sqlWriter.Write(Chr(64) & Chr(0) & Chr(0) & Chr(0))

            'MLIQ Size:	4 Bytes		=	10 00 00 00 (16 Bytes)
            sqlWriter.Write(Chr(16) & Chr(0) & Chr(0) & Chr(0))

            'Hole Offset:	4 Bytes		=	50 02 00 00 (591 Bytes)
            sqlWriter.Write(Chr(80) & Chr(0) & Chr(0) & Chr(0))

            'Hole Size:	4 Bytes		=	00 02 00 00 (512 Bytes)
            sqlWriter.Write(Chr(0) & Chr(2) & Chr(0) & Chr(0))

            'Write Identifier
            sqlWriter.Write(areaHdr.ToString())
            sqlWriter.Write(mhgtHdr.ToString())
            sqlWriter.Write(mliqHdr.ToString())
            sqlWriter.Write(holeHdr.ToString())

            'Now Need to write out the data chunks
            'sqlWriter.Write(AREAData.ToString())
            'sqlWriter.Write(MHGTData.ToString())
            'sqlWriter.Write(MLIQData.ToString())
            'sqlWriter.Write(HOLEData.ToString())

            sqlWriter.Flush()
            sqlWriter.Close()

            'Dump(megamap, 16 * MapParser.Size, minV, maxV,
            '                         16 * MapParser.Size * x, 16 * MapParser.Size * y,
            'adtfilename.Replace(".adt", "") + y.ToString() + "_" + x.ToString() + ".bmp")

            'Dump(megamap, 16 * MapParser.Size, minV, maxV,
            '                         16 * MapParser.Size * x, 16 * MapParser.Size * y,
            'outputFilename.Replace("map", "bmp"))

            'Console.WriteLine("Dumping...")
            'Dim fs = New FileStream(adtfilename & "_map.dat", FileMode.Create, FileAccess.Write)
            'Dim w = New BinaryWriter(fs)
            'w.Write(megaSize)
            'w.Write(megaSize)
            'w.Write(minV)
            'w.Write(maxV)
            'For y As Integer = 0 To megaSize - 1
            '    For x As Integer = 0 To megaSize - 1
            '        w.Write(megamap(y, x))
            '    Next
            'Next
            'w.Close()


            ''
            '            For y As Integer = 0 To 3
            '                For x As Integer = 0 To 3
            '                    Dump(megamap, 16 * MapParser.Size, minV, maxV,
            '                         16 * MapParser.Size * x, 16 * MapParser.Size * y,
            'adtfilename.Replace(".adt", "") + y.ToString() + "_" + x.ToString() + ".bmp")
            '                Next
            '            Next


            'Console.WriteLine("Press any key")
            'Console.ReadKey()
        End Sub
    End Class
End Module

