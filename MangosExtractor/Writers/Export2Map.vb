Imports System.IO
Imports MangosExtractor.Core

Public Class Export2Map
    '
    ' Adt file convertor function and data
    '

    ' Map file format data
    Public MapMagic As String = "MAPS"
    Public MapVersionMagic As String = "z1.3"
    Public MapAreaMagic As String = "AREA"
    Public MapHeightMagic As String = "MHGT"
    Public MapLiquidMagic As String = "MLIQ"

    Structure MapFileheader
        Private mapMagic As UInt32
        Private versionMagic As UInt32
        Private areaMapOffset As UInt32
        Private areaMapSize As UInt32
        Private heightMapOffset As UInt32
        Private heightMapSize As UInt32
        Private liquidMapOffset As UInt32
        Private liquidMapSize As UInt32
        Private holesOffset As UInt32
        Private holesSize As UInt32
    End Structure

    Const MapAreaNoArea As Integer = &H1

    Private Structure MapAreaHeader
        Dim Fourcc As UInt32
        Dim Flags As UInt16
        Dim GridArea As UInt16
    End Structure

    Const MapHeightNoHeight As Integer = &H1
    Const MapHeightAsInt16 As Integer = &H2
    Const MapHeightAsInt8 As Integer = &H4

    Private Structure MapHeightHeader
        Dim Fourcc As UInt32
        Dim Flags As UInt32
        Dim GridHeight As Double
        Dim GridMaxHeight As Double
    End Structure

    Const MapLiquidTypeNoWater As Integer = &H0
    Const MapLiquidTypeMagma As Integer = &H1
    Const MapLiquidTypeOcean As Integer = &H2
    Const MapLiquidTypeSlime As Integer = &H4
    Const MapLiquidTypeWater As Integer = &H8

    Const MapLiquidTypeDarkWater As Integer = &H10
    Const MapLiquidTypeWmoWater As Integer = &H20

    Const MapLiquidNoType As Integer = &H1
    Const MapLiquidNoHeight As Integer = &H2

    Structure MapLiquidHeader
        Private _fourcc As UInt32
        Private _flags As UInt16
        Private _liquidType As UInt16
        Private _offsetX As SByte
        Private _offsetY As SByte
        Private _width As SByte
        Private _height As SByte
        Private _liquidLevel As Double
    End Structure

    Private Shared Function SelectUInt8StepStore(maxDiff As Single) As Single
        Return 255/maxDiff
    End Function

    Private Shared Function SelectUInt16StepStore(maxDiff As Single) As Single
        Return 65535/maxDiff
    End Function

    Const AdtCellsPerGrid As Integer = 16
    Const AdtCellSize As Integer = 8
    Const AdtGridSize As Integer = (AdtCellsPerGrid*AdtCellSize)

    ' Temporary grid data store
    Public AreaFlags(AdtCellsPerGrid, AdtCellsPerGrid) As UInt16

    Public V8(AdtGridSize, AdtGridSize) As Double
    Public V9(AdtGridSize + 1, AdtGridSize + 1) As Double
    Public Uint16V8(AdtGridSize, AdtGridSize) As UInt16
    Public Uint16V9(AdtGridSize + 1, AdtGridSize + 1) As UInt16
    Public Uint8V8(AdtGridSize, AdtGridSize) As SByte
    Public Uint8V9(AdtGridSize + 1, AdtGridSize + 1) As SByte

    Public LiquidEntry(AdtCellsPerGrid, AdtCellsPerGrid) As UInt16
    Public LiquidFlags(AdtCellsPerGrid, AdtCellsPerGrid) As SByte
    Public LiquidShow(AdtGridSize, AdtGridSize) As Boolean
    Public LiquidHeight(AdtGridSize + 1, AdtGridSize + 1) As Double

    ' This option allow use float to int conversion
    Public ConfAllowFloatToInt As Boolean = True
    Public ConfFloatToInt8Limit As Double = 2.0F  '    // Max accuracy = val/256
    Public ConfFloatToInt16Limit As Double = 2048.0F '   // Max accuracy = val/65536
    Public ConfFlatHeightDeltaLimit As Double = 0.005F ' // If max - min less this value - surface is flat
    Public ConfFlatLiquidDeltaLimit As Double = 0.001F ' // If max - min less this value - liquid surface is flat

    Public Sub ConvertAdt(adtfilename As String, ByRef outputFilename As String, mapx As Integer, mapy As Integer, dictMaps As Dictionary(Of Integer, String), dictAreaTable As Dictionary(Of Integer, String), dictLiquidType As Dictionary(Of Integer, Integer))
        'these need to be options switches
        Dim CONF_allow_height_limit As Boolean = True
        Dim CONF_use_minHeight As Double = - 500.0F

        Dim filename = adtfilename
        Console.WriteLine(filename)
        Dim f = New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim map = New MapParser(f)

        Dim maxV = [Single].MinValue
        Dim minV = [Single].MaxValue

        Dim areaFlags(AdtCellsPerGrid, AdtCellsPerGrid) As UInt16

        Dim v8(AdtGridSize, AdtGridSize) As Double
        Dim v9(AdtGridSize + 1, AdtGridSize + 1) As Double
        Dim uint16V8(AdtGridSize, AdtGridSize) As UInt16
        Dim uint16V9(AdtGridSize + 1, AdtGridSize + 1) As UInt16
        Dim uint8V8(AdtGridSize, AdtGridSize) As SByte
        Dim uint8V9(AdtGridSize + 1, AdtGridSize + 1) As SByte

        Dim liquidEntry(AdtCellsPerGrid, AdtCellsPerGrid) As UInt16
        Dim liquidFlags(AdtCellsPerGrid, AdtCellsPerGrid) As SByte
        Dim liquidShow(AdtGridSize, AdtGridSize) As Boolean
        Dim liquidHeight(AdtGridSize + 1, AdtGridSize + 1) As Double

        If map.MaxV > maxV Then
            maxV = map.MaxV
        End If
        If map.MinV < minV Then
            minV = map.MinV
        End If


        ' Get area flags data
        Dim thisMcnk As Mcnk = map.Mcnk

        For i As Integer = 0 To AdtCellsPerGrid - 1
            For j As Integer = 0 To AdtCellsPerGrid - 1
                thisMcnk.IndexX = i
                thisMcnk.IndexY = j
                If thisMcnk.areaid > 0 Then
                    If dictAreaTable(thisMcnk.areaid) <> &HFFFF Then
                        areaFlags(i, j) = dictAreaTable(thisMcnk.areaid)
                        Continue For
                    Else
                        Alert("File: " & filename & " Can't find area flag for areaid " & thisMcnk.areaid & " [" & i & "," & j & "].", MaNgosExtractorCore.AlertNewLine.ADD_CRLF)
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
        For y As Integer = 0 To AdtCellsPerGrid - 1
            For x As Integer = 0 To AdtCellsPerGrid - 1
                If areaFlags(y, x) <> areaflag Then
                    fullAreaData = True
                    Exit For
                End If
            Next
        Next

        '          map.areaMapOffset = sizeof(map);
        'map.areaMapSize   = sizeof(map_areaHeader);

        Dim areaHeader As MapAreaHeader
        areaHeader.Fourcc = 1095910721 'AREA 
        areaHeader.Flags = 0
        If (fullAreaData) Then

            areaHeader.GridArea = 0
            'map.areaMapSize += sizeof(area_flags)

        Else

            areaHeader.Flags = MapAreaNoArea
            areaHeader.GridArea = CShort(thisMcnk.flags)
        End If


        '
        ' Get Height map from grid
        '
        For i As Integer = 0 To AdtCellsPerGrid - 1
            For j As Integer = 0 To AdtCellsPerGrid - 1
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
                For y As Integer = 0 To AdtCellSize
                    Dim cy As Integer = i*AdtCellSize + y
                    For x As Integer = 0 To AdtCellSize
                        Dim cx As Integer = j*AdtCellSize + x
                        v9(cy, cx) = thisMcnk.position.Y ' cell.ypos
                    Next
                Next
                For y As Integer = 0 To AdtCellSize - 1
                    Dim cy As Integer = i*AdtCellSize + y
                    For x As Integer = 0 To AdtCellSize - 1
                        Dim cx As Integer = j*AdtCellSize + x
                        v8(cy, cx) = thisMcnk.position.Y ' cell.ypos
                    Next
                Next
                ' Get custom height
                Dim thisMcvt As Mcvt = map.Mcvt


                'Dim v As Double = thisMCVT.height '  thisMCNK.ofsHeight ' cell.getMCVT()
                If Not IsNothing(thisMcvt) Then
                    Continue For
                End If
                ' get V9 height map
                For y As Integer = 0 To AdtCellSize
                    Dim cy As Integer = i*AdtCellSize + y
                    For x As Integer = 0 To AdtCellSize
                        Dim cx As Integer = j*AdtCellSize + x
                        v9(cy, cx) += thisMcvt.height '.height_map(y * (ADT_CELL_SIZE * 2 + 1) + x)
                    Next
                Next
                ' get V8 height map
                For y As Integer = 0 To AdtCellSize - 1
                    Dim cy As Integer = i*AdtCellSize + y
                    For x As Integer = 0 To AdtCellSize - 1
                        Dim cx As Integer = j*AdtCellSize + x
                        v8(cy, cx) += thisMcvt.height 'v.height_map(y * (ADT_CELL_SIZE * 2 + 1) + ADT_CELL_SIZE + 1 + x)
                    Next
                Next
            Next
        Next

        '============================================
        ' Try pack height data
        '============================================
        Dim maxHeight As Single = - 20000
        Dim minHeight As Single = 20000
        For y As Integer = 0 To AdtGridSize - 1
            For x As Integer = 0 To AdtGridSize - 1
                Dim h As Single = v8(y, x)
                If maxHeight < h Then
                    maxHeight = h
                End If
                If minHeight > h Then
                    minHeight = h
                End If
            Next
        Next
        For y As Integer = 0 To AdtGridSize
            For x As Integer = 0 To AdtGridSize
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
        If CONF_allow_height_limit = True AndAlso minHeight < CONF_use_minHeight Then
            For y As Integer = 0 To AdtGridSize - 1
                For x As Integer = 0 To AdtGridSize - 1
                    If v8(y, x) < CONF_use_minHeight Then
                        v8(y, x) = CONF_use_minHeight
                    End If
                Next
            Next
            For y As Integer = 0 To AdtGridSize
                For x As Integer = 0 To AdtGridSize
                    If v9(y, x) < CONF_use_minHeight Then
                        v9(y, x) = CONF_use_minHeight
                    End If
                Next
            Next
            If minHeight < CONF_use_minHeight Then
                minHeight = CONF_use_minHeight
            End If
            If maxHeight < CONF_use_minHeight Then
                maxHeight = CONF_use_minHeight
            End If
        End If


        '        map.heightMapOffset = map.areaMapOffset + map.areaMapSize;
        'map.heightMapSize = sizeof(map_heightHeader);

        Dim heightHeader As MapHeightHeader
        heightHeader.Fourcc = 1296582484 'MGHT
        heightHeader.Flags = 0
        heightHeader.GridHeight = minHeight
        heightHeader.GridMaxHeight = maxHeight

        If (maxHeight = minHeight) Then
            heightHeader.Flags = MapHeightNoHeight
        End If

        ' Not need store if flat surface
        If (ConfAllowFloatToInt And (maxHeight - minHeight) < ConfFlatHeightDeltaLimit) Then
            heightHeader.Flags = MapHeightNoHeight
        End If

        ' Try store as packed in uint16 or uint8 values
        If Not (heightHeader.Flags And MapHeightNoHeight) Then
            Dim [step] As Single
            ' Try Store as uint values
            If ConfAllowFloatToInt Then
                Dim diff As Single = maxHeight - minHeight
                If diff < ConfFloatToInt8Limit Then
                    ' As uint8 (max accuracy = CONF_float_to_int8_limit/256)
                    heightHeader.Flags = heightHeader.Flags Or MapHeightAsInt8
                    [step] = SelectUInt8StepStore(diff)
                ElseIf diff < ConfFloatToInt16Limit Then
                    ' As uint16 (max accuracy = CONF_float_to_int16_limit/65536)
                    heightHeader.Flags = heightHeader.Flags Or MapHeightAsInt16
                    [step] = SelectUInt16StepStore(diff)
                End If
            End If

            ' Pack it to int values if need
            If heightHeader.Flags And MapHeightAsInt8 Then
                For y As Integer = 0 To AdtGridSize - 1
                    For x As Integer = 0 To AdtGridSize - 1
                        uint8V8(y, x) = CSByte((v8(y, x) - minHeight)*[step] + 0.5F)
                    Next
                Next
                For y As Integer = 0 To AdtGridSize
                    For x As Integer = 0 To AdtGridSize
                        uint8V9(y, x) = CSByte((v9(y, x) - minHeight)*[step] + 0.5F)
                    Next
                Next
                'map.heightMapSize += sizeof(uint8_V9) + sizeof(uint8_V8)
            ElseIf heightHeader.Flags And MapHeightAsInt16 Then
                For y As Integer = 0 To AdtGridSize - 1
                    For x As Integer = 0 To AdtGridSize - 1
                        uint16V8(y, x) = CUInt((v8(y, x) - minHeight)*[step] + 0.5F)
                    Next
                Next
                For y As Integer = 0 To AdtGridSize
                    For x As Integer = 0 To AdtGridSize
                        uint16V9(y, x) = CUInt((v9(y, x) - minHeight)*[step] + 0.5F)
                    Next
                Next
                'map.heightMapSize += sizeof(uint16_V9) + sizeof(uint16_V8)
            Else
                'map.heightMapSize += sizeof(V9) + sizeof(V8)
            End If
        End If

        ' Get from MCLQ chunk (old)
        For i As Integer = 0 To AdtCellsPerGrid - 1
            For j As Integer = 0 To AdtCellsPerGrid - 1
                'Dim cell As Pointer(Of adt_MCNK) = cells.getMCNK(i, j)
                'If Not cell Then
                '    Continue For
                'End If
                Dim thisMclq As Mclq = map.Mclq

                Dim liquid(,) As LiquidData = thisMclq.liquid ' Double = thisMCLQ.liquid(i, j).height
                Dim count As Integer = 0

                'If Not liquid OrElse cell.sizeMCLQ <= 8 Then
                '    Continue For
                'End If

                For y As Integer = 0 To AdtCellSize - 1
                    Dim cy As Integer = i*AdtCellSize + y
                    For x As Integer = 0 To AdtCellSize - 1
                        Dim cx As Integer = j*AdtCellSize + x
                        If thisMclq.flags(y, x) <> &HF Then
                            liquidShow(cy, cx) = True
                            If thisMclq.flags(y, x) And (1 << 7) Then
                                liquidFlags(i, j) = liquidFlags(i, j) Or MapLiquidTypeDarkWater
                            End If
                            count += 1
                        End If
                    Next
                Next

                Dim cFlag As UInt32 = thisMclq.flags(i, j)
                If cFlag And (1 << 2) Then
                    liquidEntry(i, j) = 1
                    ' water
                    liquidFlags(i, j) = liquidFlags(i, j) Or MapLiquidTypeWater
                End If
                If cFlag And (1 << 3) Then
                    liquidEntry(i, j) = 2
                    ' ocean
                    liquidFlags(i, j) = liquidFlags(i, j) Or MapLiquidTypeOcean
                End If
                If cFlag And (1 << 4) Then
                    liquidEntry(i, j) = 3
                    ' magma/slime
                    liquidFlags(i, j) = liquidFlags(i, j) Or MapLiquidTypeMagma
                End If

                If Not count AndAlso liquidFlags(i, j) Then
                    Alert("Wrong liquid detect in MCLQ chunk", AlertNewLine.ADD_CRLF)
                End If

                For y As Integer = 0 To AdtCellSize
                    Dim cy As Integer = i*AdtCellSize + y
                    For x As Integer = 0 To AdtCellSize
                        Dim cx As Integer = j*AdtCellSize + x
                        liquidHeight(cy, cx) = thisMclq.liquid(y, x).Height
                    Next
                Next
            Next
        Next
    End Sub
End Class
