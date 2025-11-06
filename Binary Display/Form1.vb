Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Form1

    Private Player As AudioPlayer

    Private bits(7) As Boolean
    Private bitRects(7) As Rectangle
    Private BitBoxSize As Integer = 200
    Private BitSpacing As Integer = 10
    Private StartX As Integer = 200
    Private StartY As Integer = 200

    Dim BitBoxFont As New Font("Consolas", 128)
    Dim placeValueFont = New Font("Consolas", 55)
    Dim breakdownFont = New Font("Consolas", 55)
    Dim DecimalFont = New Font("Consolas", 72)

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CreateSoundFiles()

        Dim FilePath As String = Path.Combine(Application.StartupPath, "CashCollected.mp3")

        Player.AddOverlapping("CashCollected", FilePath)

        Player.SetVolumeOverlapping("CashCollected", 1000)

        FilePath = Path.Combine(Application.StartupPath, "ComputerPulsation.mp3")

        Player.AddSound("ComputerPulsation", FilePath)

        Player.SetVolume("ComputerPulsation", 100)

        MinimumSize = New Size(720, 480)

        BackColor = Color.Black
        KeyPreview = True
        DoubleBuffered = True
        WindowState = FormWindowState.Maximized

        Text = "8-Bit Binary Display - Code with Joe"

        For i = 0 To 7
            bitRects(i) = New Rectangle(StartX + i * (BitBoxSize + BitSpacing), StartY, BitBoxSize, BitBoxSize)
        Next

        Player.LoopSound("ComputerPulsation")

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim g = e.Graphics

        Dim brushOn = Brushes.Lime
        Dim brushOff = Brushes.DarkGray

        Dim TextbrushOn = Brushes.Black
        Dim TextbrushOff = Brushes.Gray
        Dim AlineCenter As New StringFormat With {
            .Alignment = StringAlignment.Center
        }

        Dim pen = Pens.Black

        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        ' Draw bit boxes
        For i = 0 To 7
            Dim rect = bitRects(i)

            g.FillRectangle(If(bits(i), brushOn, brushOff), rect)
            g.DrawRectangle(pen, rect)

            'Centered horizontally and vertically inside of the bit boxes
            g.DrawString(If(bits(i), "1", "0"), BitBoxFont, If(bits(i), TextbrushOn, TextbrushOff), rect.X + (BitBoxSize - g.MeasureString(If(bits(i), "1", "0"), BitBoxFont).Width) / 2, rect.Y + (BitBoxSize - g.MeasureString(If(bits(i), "1", "0"), BitBoxFont).Height) / 2)

            Dim placeValueBrush = Brushes.LightGray

            Dim placeValue = CStr(2 ^ (7 - i))
            g.DrawString(placeValue, placeValueFont, placeValueBrush, rect.X + BitBoxSize \ 2, rect.Y - Me.ClientSize.Height \ 12, AlineCenter)

        Next

        ' Draw decimal value
        Dim binaryStr = String.Join("", bits.Select(Function(b) If(b, "1", "0")))
        Dim decimalVal = Convert.ToInt32(binaryStr, 2)
        g.DrawString($"{decimalVal}", DecimalFont, Brushes.White, ClientSize.Width \ 2, StartY - Me.ClientSize.Height \ 3, AlineCenter)

        ' Show the place values adding up to the decimal value.
        Dim activeValues = New List(Of Integer)
        For i = 0 To 7
            If bits(i) Then activeValues.Add(2 ^ (7 - i))
        Next

        If activeValues.Count > 1 Then
            Dim breakdown = String.Join(" + ", activeValues)
            Dim breakdownBrush = Brushes.DarkGray
            g.DrawString($"{breakdown} = {decimalVal}", breakdownFont, breakdownBrush, ClientSize.Width \ 2, StartY + Me.ClientSize.Height \ 4, AlineCenter)
        End If

    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)
        For i = 0 To 7
            If bitRects(i).Contains(e.Location) Then

                Player.PlayOverlapping("CashCollected")

                bits(i) = Not bits(i)

                Me.Invalidate()

                Exit For
            End If
        Next
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        MyBase.OnKeyDown(e)

        Dim currentValue = Convert.ToInt32(String.Join("", bits.Select(Function(b) If(b, "1", "0"))), 2)

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
            bits(i) = binaryStr(i) = "1"
        Next

        Me.Invalidate()

    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        BitBoxSize = Math.Max(32, Me.ClientSize.Height \ 6)

        BitSpacing = Math.Max(5, Me.ClientSize.Height \ 42)

        StartX = (Me.ClientSize.Width - (8 * (BitBoxSize + BitSpacing) - BitSpacing)) \ 2
        StartY = (Me.ClientSize.Height) \ 2 - (BitBoxSize \ 2)

        For i = 0 To 7
            bitRects(i) = New Rectangle(StartX + i * (BitBoxSize + BitSpacing), StartY, BitBoxSize, BitBoxSize)
        Next

        BitBoxFont = New Font("Consolas", Math.Max(20, Me.ClientSize.Height \ 11))
        placeValueFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 21))
        breakdownFont = New Font("Consolas", Math.Max(6, Me.ClientSize.Height \ 21))
        DecimalFont = New Font("Consolas", Math.Max(12, Me.ClientSize.Height \ 8))

        Me.Invalidate()

    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Player.CloseSounds()

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
                                      "Q", "R", "S", "T", "U", "V", "W", "X"}

            AddSound(SoundName & Suffix, FilePath)

        Next

    End Sub

    Public Sub PlayOverlapping(SoundName As String)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H",
                                      "I", "J", "K", "L", "M", "N", "O", "P",
                                      "Q", "R", "S", "T", "U", "V", "W", "X"}

            If Not IsPlaying(SoundName & Suffix) Then

                PlaySound(SoundName & Suffix)

                Exit Sub

            End If

        Next

    End Sub

    Public Sub SetVolumeOverlapping(SoundName As String, Level As Integer)

        For Each Suffix As String In {"A", "B", "C", "D", "E", "F", "G", "H",
                                      "I", "J", "K", "L", "M", "N", "O", "P",
                                      "Q", "R", "S", "T", "U", "V", "W", "X"}

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
