Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Form1

    Private BinaryStr As String = ""
    Private Bits(7) As Boolean ' 8 bits for an 8-bit binary number.
    Private BitRects(7) As Rectangle ' Rectangles for each bit box.
    Private BitBoxSize As Integer = 0
    Private BitSpacing As Integer = 0
    Private BitBoxesLeft As Integer = 0
    Private BitBoxesTop As Integer = 0

    Private BitOnBrush = Brushes.Chartreuse
    Private BitOffBrush = New SolidBrush(Color.FromArgb(255, 195, 195, 195))

    Private TextOnBrush = Brushes.Black
    Private TextOffBrush = New SolidBrush(Color.FromArgb(255, 75, 75, 75))

    Private BitBoxFont As New Font("Consolas", 12)

    Private DecimalVal As Integer = 0
    Private DecimalBrush = Brushes.White
    Private DecimalFont = New Font("Consolas", 12)

    Private PlaceValue As String = ""
    Private PlaceValueBrush = Brushes.DarkGray
    Private PlaceValueFont = New Font("Consolas", 12)

    Private ActiveValues As New List(Of Integer)
    Private Breakdown As String = ""
    Private BreakdownBrush = Brushes.DarkGray
    Private BreakdownFont = New Font("Consolas", 12)

    Private Rect As Rectangle

    Private Player As AudioPlayer

    Private AlineCenter As New StringFormat With {.Alignment = StringAlignment.Center}

    Private Graph As Graphics

    Private HoveredBitIndex As Integer = -1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        InitializeApp()

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Graph = e.Graphics

        Graph.CompositingMode = Drawing2D.CompositingMode.SourceOver

        Graph.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        DrawBitBoxes()

        DrawDecimalValue()

        DrawActiveValuesBreakdown()

    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        For i = 0 To 7

            If BitRects(i).Contains(e.Location) Then

                Player.PlayOverlapping("CashCollected")

                Bits(i) = Not Bits(i)

                Me.Invalidate()

                Exit For

            End If

        Next

    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)

        Dim newHoverIndex As Integer = -1

        For i = 0 To 7
            If BitRects(i).Contains(e.Location) Then
                newHoverIndex = i
                Exit For
            End If
        Next

        If newHoverIndex <> HoveredBitIndex Then
            HoveredBitIndex = newHoverIndex
            Me.Invalidate()
        End If

        Cursor = If(HoveredBitIndex <> -1, Cursors.Hand, Cursors.Default)

    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        Dim currentValue = Convert.ToInt32(String.Join("", Bits.Select(Function(b) If(b, "1", "0"))), 2)

        If e.KeyCode = Keys.Up Then

            If currentValue < 255 Then

                currentValue += 1

                Player.PlayOverlapping("CashCollected")

            End If

        ElseIf e.KeyCode = Keys.Down Then

            If currentValue > 0 Then

                currentValue -= 1

                Player.PlayOverlapping("CashCollected")

            End If

        Else

            Return

        End If

        ' Update bits array
        Dim binaryStr = Convert.ToString(currentValue, 2).PadLeft(8, "0"c)

        For i = 0 To 7

            Bits(i) = binaryStr(i) = "1"

        Next

        Me.Invalidate()

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        UpdateSizes()

        UpdateStartPositions()

        UpdateBitRects()

        UpdateFonts()

        Me.Invalidate()

    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Player.CloseSounds()

    End Sub

    Private Sub InitializeApp()

        Player = New AudioPlayer

        CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "CashCollected.mp3")

        Player.AddOverlapping("CashCollected", FilePath)

        Player.SetVolumeOverlapping("CashCollected", 1000)

        FilePath = Path.Combine(Application.StartupPath, "ComputerPulsation.mp3")

        Player.AddSound("ComputerPulsation", FilePath)

        Player.SetVolume("ComputerPulsation", 40)

        MinimumSize = New Size(720, 480)

        BackColor = Color.Black
        KeyPreview = True
        DoubleBuffered = True
        WindowState = FormWindowState.Maximized

        Text = "8-Bit Binary Display - Code with Joe"

        For i = 0 To 7
            BitRects(i) = New Rectangle(BitBoxesLeft + i * (BitBoxSize + BitSpacing),
                                        BitBoxesTop,
                                        BitBoxSize,
                                        BitBoxSize)
        Next

        Player.LoopSound("ComputerPulsation")

    End Sub

    Private Sub UpdateBitRects()

        For i = 0 To 7

            BitRects(i) = New Rectangle(BitBoxesLeft + i * (BitBoxSize + BitSpacing),
                                        BitBoxesTop,
                                        BitBoxSize,
                                        BitBoxSize)

        Next

    End Sub

    Private Sub UpdateFonts()

        Dim scaleFactor As Single = Me.DeviceDpi / 96.0F ' 96 DPI is the default for 100% scaling

        Select Case scaleFactor
            Case = 1.0F
                BitBoxFont = New Font("Consolas", Math.Max(20, Me.ClientSize.Height \ 12))
                PlaceValueFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 18))
                BreakdownFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 20))
                DecimalFont = New Font("Consolas", Math.Max(12, Me.ClientSize.Height \ 9))
            Case = 1.25F
                BitBoxFont = New Font("Consolas", Math.Max(20, Me.ClientSize.Height \ 15))
                PlaceValueFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 23))
                BreakdownFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 25))
                DecimalFont = New Font("Consolas", Math.Max(12, Me.ClientSize.Height \ 10))
            Case = 1.5F
                BitBoxFont = New Font("Consolas", Math.Max(20, Me.ClientSize.Height \ 18))
                PlaceValueFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 28))
                BreakdownFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 30))
                DecimalFont = New Font("Consolas", Math.Max(12, Me.ClientSize.Height \ 12))
        End Select

    End Sub







    Private Sub UpdateStartPositions()
        BitBoxesLeft = (Me.ClientSize.Width - (8 * (BitBoxSize + BitSpacing) - BitSpacing)) \ 2
        BitBoxesTop = (Me.ClientSize.Height) \ 2 - (BitBoxSize \ 2)
    End Sub

    Private Sub UpdateSizes()
        BitBoxSize = Math.Max(32, Me.ClientSize.Height \ 6)
        BitSpacing = Math.Max(5, Me.ClientSize.Height \ 42)
    End Sub

    Private Sub DrawDecimalValue()
        ' Draw decimal value.

        BinaryStr = String.Join("", Bits.Select(Function(b) If(b, "1", "0")))

        DecimalVal = Convert.ToInt32(BinaryStr, 2)

        Graph.DrawString($"{DecimalVal}",
                         DecimalFont,
                         DecimalBrush,
                         ClientSize.Width \ 2,
                         BitBoxesTop - Me.ClientSize.Height \ 3,
                         AlineCenter)

    End Sub

    Private Sub DrawBitBoxes()

        ' Draw bit boxes.
        For i = 0 To 7

            Rect = BitRects(i)

            Graph.FillRectangle(If(Bits(i), BitOnBrush, BitOffBrush),
                                Rect)

            ' Draw the binary digit centered horizontally and vertically inside of the bit boxes.
            Graph.DrawString(If(Bits(i), "1", "0"),
                             BitBoxFont,
                             If(Bits(i), TextOnBrush, TextOffBrush),
                             Rect.X + (BitBoxSize - Graph.MeasureString(If(Bits(i), "1", "0"), BitBoxFont).Width) / 2,
                             Rect.Y + (BitBoxSize - Graph.MeasureString(If(Bits(i), "1", "0"), BitBoxFont).Height) / 2)




            ' Draw place value above each bit box.
            PlaceValue = CStr(2 ^ (7 - i))

            Dim currentPlaceValueBrush As Brush = If(i = HoveredBitIndex, Brushes.Orange, PlaceValueBrush)

            'Graph.DrawString(PlaceValue,
            '     PlaceValueFont,
            '     currentPlaceValueBrush,
            '     Rect.X + BitBoxSize \ 2,
            '     Rect.Y - Me.ClientSize.Height \ 12,
            '     AlineCenter)
            Graph.DrawString(PlaceValue,
                 PlaceValueFont,
                 currentPlaceValueBrush,
                 Rect.X + BitBoxSize \ 2,
                 Rect.Y - Graph.MeasureString(PlaceValue, PlaceValueFont).Height,
                 AlineCenter)


            ' Draw border if hovered.
            If i = HoveredBitIndex Then

                Using borderPen As New Pen(If(Bits(i), Color.DodgerBlue, Color.DodgerBlue), BitSpacing / 3)

                    borderPen.Alignment = Drawing2D.PenAlignment.Outset

                    Graph.DrawRectangle(borderPen, Rect)

                End Using

            End If

        Next

    End Sub

    Private Sub DrawActiveValuesBreakdown()
        ' Draw a breakdown showing the active place values adding up to the decimal value.

        ActiveValues = New List(Of Integer)

        For i = 0 To 7

            If Bits(i) Then ActiveValues.Add(2 ^ (7 - i))

        Next

        If ActiveValues.Count > 1 Then

            Breakdown = String.Join(" + ", ActiveValues)

            'Graph.DrawString($"{Breakdown} = {DecimalVal}",
            '                 BreakdownFont, BreakdownBrush,
            '                 ClientSize.Width \ 2,
            '                 BitBoxesTop + Me.ClientSize.Height \ 4,
            '                 AlineCenter)
            Graph.DrawString($"{Breakdown} = {DecimalVal}",
                             BreakdownFont, BreakdownBrush,
                             ClientSize.Width \ 2,
                             BitBoxesTop + BitBoxSize + BitSpacing,
                             AlineCenter)

        End If

    End Sub

    Private Sub CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "CashCollected.mp3")

        CreateFileFromResource(FilePath, My.Resources.Resource1.CashCollected)

        FilePath = Path.Combine(Application.StartupPath, "ComputerPulsation.mp3")

        CreateFileFromResource(FilePath, My.Resources.Resource1.computer_pulsation)

    End Sub

    Private Sub CreateFileFromResource(filepath As String, resource As Byte())

        Try

            If Not IO.File.Exists(filepath) Then

                IO.File.WriteAllBytes(filepath, resource)

            End If

        Catch ex As Exception

            Debug.Print($"Error creating file: {ex.Message}")

        End Try

    End Sub

End Class

Public Structure AudioPlayer

    <DllImport("winmm.dll", EntryPoint:="mciSendStringW")>
    Private Shared Function mciSendStringW(<MarshalAs(UnmanagedType.LPWStr)> ByVal lpszCommand As String,
                                           <MarshalAs(UnmanagedType.LPWStr)> ByVal lpszReturnString As StringBuilder,
                                           ByVal cchReturn As UInteger, ByVal hwndCallback As IntPtr) As Integer
    End Function

    Private Sounds() As String

    Public Function AddSound(SoundName As String, FilePath As String) As Boolean

        ' Do we have a name and does the file exist?
        If Not String.IsNullOrWhiteSpace(SoundName) AndAlso IO.File.Exists(FilePath) Then
            ' Yes, we have a name and the file exists.

            Dim CommandOpen As String = $"open ""{FilePath}"" alias {SoundName}"

            ' Do we have sounds?
            If Sounds Is Nothing Then
                ' No we do not have sounds.

                ' Did the sound file open?
                If SendMciCommand(CommandOpen, IntPtr.Zero) Then
                    ' Yes, the sound file did open.

                    ' Start the Sounds array with the sound.
                    ReDim Sounds(0)

                    Sounds(0) = SoundName

                    Return True ' The sound was added.

                End If

                ' Is the sound in the array already?
            ElseIf Not Sounds.Contains(SoundName) Then
                ' Yes we have sounds and no the sound is not in the array.

                ' Did the sound file open?
                If SendMciCommand(CommandOpen, IntPtr.Zero) Then
                    ' Yes, the sound file did open.

                    ' Add the sound to the Sounds array.
                    Array.Resize(Sounds, Sounds.Length + 1)

                    Sounds(Sounds.Length - 1) = SoundName

                    Return True ' The sound was added.

                End If

            End If

        End If

        Debug.Print($"{SoundName} not added to sounds.")

        Return False ' The sound was not added.

    End Function

    Public Function SetVolume(SoundName As String, Level As Integer) As Boolean

        ' Do we have sounds and is the sound in the array and is the level in the valid range?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) AndAlso Level >= 0 AndAlso Level <= 1000 Then
            ' We have sounds and the sound is in the array and the level is in range.

            Dim CommandVolume As String = $"setaudio {SoundName} volume to {Level}"

            Return SendMciCommand(CommandVolume, IntPtr.Zero) ' The volume was set.

        End If

        Debug.Print($"{SoundName} volume not set.")

        Return False ' The volume was not set.

    End Function

    Public Function LoopSound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            Dim CommandPlayRepeat As String = $"play {SoundName} repeat"

            Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
                   SendMciCommand(CommandPlayRepeat, IntPtr.Zero) ' The sound is looping.

        End If

        Debug.Print($"{SoundName} not looping.")

        Return False ' The sound is not looping.

    End Function

    Public Function PlaySound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandSeekToStart As String = $"seek {SoundName} to start"

            Dim CommandPlay As String = $"play {SoundName} notify"

            Return SendMciCommand(CommandSeekToStart, IntPtr.Zero) AndAlso
                   SendMciCommand(CommandPlay, IntPtr.Zero) ' The sound is playing.

        End If

        Debug.Print($"{SoundName} not playing.")

        Return False ' The sound is not playing.

    End Function

    Public Function PauseSound(SoundName As String) As Boolean

        ' Do we have sounds and is the sound in the array?
        If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
            ' We have sounds and the sound is in the array.

            Dim CommandPause As String = $"pause {SoundName} notify"

            Return SendMciCommand(CommandPause, IntPtr.Zero) ' The sound is paused.

        End If

        Debug.Print($"{SoundName} not paused.")

        Return False ' The sound is not paused.

    End Function

    Public Function IsPlaying(SoundName As String) As Boolean

        Return GetStatus(SoundName, "mode") = "playing"

    End Function

    Public Sub AddOverlapping(SoundName As String, FilePath As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H",
                                      "I", "J", "K", "L", "M", "N", "O", "P",
                                      "Q", "R", "S", "T", "U", "V", "W", "X",
                                      "Y", "Z", "AA", "AB", "AC", "AD", "AE"}

            AddSound(SoundName & Suffix, FilePath)

        Next

    End Sub

    Public Sub PlayOverlapping(SoundName As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H",
                                      "I", "J", "K", "L", "M", "N", "O", "P",
                                      "Q", "R", "S", "T", "U", "V", "W", "X",
                                      "Y", "Z", "AA", "AB", "AC", "AD", "AE"}

            If Not IsPlaying(SoundName & Suffix) Then

                PlaySound(SoundName & Suffix)

                Exit Sub

            End If

        Next

    End Sub

    Public Sub SetVolumeOverlapping(SoundName As String, Level As Integer)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H",
                                      "I", "J", "K", "L", "M", "N", "O", "P",
                                      "Q", "R", "S", "T", "U", "V", "W", "X",
                                      "Y", "Z", "AA", "AB", "AC", "AD", "AE"}

            SetVolume(SoundName & Suffix, Level)

        Next

    End Sub

    Private Function SendMciCommand(command As String, hwndCallback As IntPtr) As Boolean

        Dim ReturnString As New StringBuilder(128)

        Try

            Return mciSendStringW(command, ReturnString, 0, hwndCallback) = 0

        Catch ex As Exception

            Debug.Print($"Error sending MCI command: {command} | {ex.Message}")

            Return False

        End Try

    End Function

    Private Function GetStatus(SoundName As String, StatusType As String) As String

        Try

            ' Do we have sounds and is the sound in the array?
            If Sounds IsNot Nothing AndAlso Sounds.Contains(SoundName) Then
                ' We have sounds and the sound is in the array.

                Dim CommandStatus As String = $"status {SoundName} {StatusType}"

                Dim StatusReturn As New StringBuilder(128)

                mciSendStringW(CommandStatus, StatusReturn, 128, IntPtr.Zero)

                Return StatusReturn.ToString.Trim.ToLower

            End If

        Catch ex As Exception

            Debug.Print($"Error getting status: {SoundName} | {ex.Message}")

        End Try

        Return String.Empty

    End Function

    Public Sub CloseSounds()

        If Sounds IsNot Nothing Then

            For Each Sound In Sounds

                Dim CommandClose As String = $"close {Sound}"

                SendMciCommand(CommandClose, IntPtr.Zero)

            Next

            Sounds = Nothing

        End If

    End Sub

End Structure
