# Binary Display

A interactive tool for learning binary numbers through visual feedback and sound.

[Code Walkthrough](#code-walkthrough)


---

### Binary Basics

**Binary is a numbering system using only two digits 0 and 1.**

---

<img width="1920" height="1080" alt="003" src="https://github.com/user-attachments/assets/23ac32f7-21b8-40a3-9be6-eaf5d428fb8e" />

---

### 🧮 Place Values in Binary

**Each bit has a place value, just like decimal — but based on powers of 2.**

---

### ✨ Features

- **Interactive Bit Toggling**: Click any of the 8 bit boxes to toggle between 0 and 1. Each change updates the binary string and decimal value instantly.
- **Keyboard Controls**: Use ↑ and ↓ keys to increment or decrement the binary value. Smooth transitions with auditory feedback.
- **Visual Breakdown**: Displays the decimal value and its binary decomposition (e.g., `32 + 8 + 2 = 42`) for deeper understanding.
- **Place Value Labels**: Each bit box is annotated with its corresponding power-of-two value (e.g., 128, 64, ..., 1).
- **Responsive Layout**: Automatically scales fonts and layout for HD/UHD resolutions and window resizing.
- **Ambient Sound Loop**: Background "ComputerPulsation" audio loops continuously to create an immersive experience.
- **Overlapping Sound Effects**: Instant playback of "CashCollected" sound on interaction, with overlapping enabled for responsiveness.


---

### 🔹 What’s a Bit?

**Each binary digit is called a _bit_.**

---


### 🎯 Educational Goals

This app is ideal for:
- Teaching binary concepts in a visually engaging way
- Reinforcing place value and bit significance
- Creating emotionally affirming learning environments with sound and color
- Demonstrating how binary maps to decimal in real time


---

### 🧱 What’s a Byte?

**A group of 8 bits is called a _byte_.**

---


### 🛠️ Technologies Used

- **VB.NET WinForms**
- **System.Drawing** for custom rendering
- **Custom AudioPlayer** for sound layering and looping
- **Dynamic font scaling** for accessibility


---

### ⚡ Powers of 2

**Each place is a power of 2, from right to left.**

---

### 🚀 Getting Started

1. Clone the repo
2. Build and run the project in Visual Studio
3. Click or use arrow keys to explore binary values
4. Enjoy the sound-enhanced feedback loop!
---
### 🔢 Range of 8 Bits

**With 8 bits, we can represent numbers from 0 to 255.**







---

# Code Walkthrough

---

## 📦 Imports

```vbnet
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
```

- These bring in useful .NET libraries for file handling, UI components, and text encoding.

---

## 🎮 Form1 Class Overview

```vbnet
Public Class Form1
```

- This is the main window of your app. Everything visual and interactive happens here.

---

## 🧮 Core Variables

```vbnet
Private BinaryStr As String
Private Bits(7) As Boolean
Private BitRects(7) As Rectangle
```

- `BinaryStr`: Holds the current binary string (e.g., `"01010101"`).
- `Bits`: Boolean array representing each bit (True = 1, False = 0).
- `BitRects`: Rectangles for drawing clickable bit boxes.

---

## 📐 Layout and Styling

```vbnet
Private BitBoxSize As Integer = 0
Private BitSpacing As Integer = 0
Private BitBoxesLeft As Integer = 0
Private BitBoxesTop As Integer = 0
```

- These control the size and position of the bit boxes.

```vbnet
Private BitOnBrush = Brushes.Lime
Private BitOffBrush = Brushes.DarkGray
Private TextOnBrush = Brushes.Black
Private TextOffBrush = Brushes.Gray
```

- Brushes define the colors for bits and text.

```vbnet
Private BitBoxFont As New Font("Consolas", 12)
```

- Font used to draw the binary digits inside boxes.

---

## 🔢 Decimal and Place Value Display

```vbnet
Private DecimalVal As Integer
Private DecimalBrush = Brushes.White
Private DecimalFont = New Font("Consolas", 12)
```

- Displays the decimal equivalent of the binary value.

```vbnet
Private PlaceValue As String
Private PlaceValueBrush = Brushes.LightGray
Private PlaceValueFont = New Font("Consolas", 12)
```

- Shows the place value (128, 64, ..., 1) above each bit box.

---

## ➕ Breakdown Display

```vbnet
Private ActiveValues As New List(Of Integer)
Private Breakdown As String
Private BreakdownBrush = Brushes.DarkGray
Private BreakdownFont = New Font("Consolas", 12)
```

- Shows which place values are active and how they add up to the decimal value.

---

## 🎧 Audio Setup

```vbnet
Private Player As AudioPlayer
```

- Handles sound playback for feedback.

---

## 🖼️ Drawing Helpers

```vbnet
Private AlineCenter As New StringFormat With {.Alignment = StringAlignment.Center}
Private Graph As Graphics
```

- Used to center text and draw graphics on the form.

---

## 🚀 Form Load

```vbnet
Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    InitializeApp()
End Sub
```

- Initializes the app when the form loads.

---

## 🎨 Paint Event

```vbnet
Protected Overrides Sub OnPaint(e As PaintEventArgs)
```

- Called whenever the form needs to redraw.

```vbnet
Graph = e.Graphics
Graph.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
```

- Sets up smooth text rendering.

```vbnet
DrawBitBoxes()
DrawDecimalValue()
DrawActiveValuesBreakdown()
```

- Draws the bit boxes, decimal value, and breakdown.

---

## 🖱️ Mouse Click

```vbnet
Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
```

- Toggles a bit when its box is clicked.

```vbnet
If BitRects(i).Contains(e.Location) Then
    Player.PlayOverlapping("CashCollected")
    Bits(i) = Not Bits(i)
    Me.Invalidate()
```

- Plays sound, flips the bit, and redraws.

---

## ⌨️ Keyboard Input

```vbnet
Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
```

- Handles Up/Down arrow keys to increment/decrement the binary value.

```vbnet
Dim currentValue = Convert.ToInt32(String.Join("", Bits.Select(Function(b) If(b, "1", "0"))), 2)
```

- Converts the bit array to an integer.

```vbnet
If e.KeyCode = Keys.Up Then ...
ElseIf e.KeyCode = Keys.Down Then ...
```

- Adjusts the value and updates the bits.

---

## 📏 Resize Event

```vbnet
Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
```

- Updates layout when the window size changes.

```vbnet
UpdateSizes()
UpdateStartPositions()
UpdateBitRects()
UpdateFonts()
Me.Invalidate()
```

- Recalculates everything and redraws.

---

## ❌ Closing Event

```vbnet
Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
    Player.CloseSounds()
End Sub
```

- Cleans up audio resources.

---

## 🛠️ Initialization

```vbnet
Private Sub InitializeApp()
```

- Sets up sound files, window properties, and bit box layout.

---

## 🔄 Layout Helpers

```vbnet
Private Sub UpdateBitRects()
Private Sub UpdateFonts()
Private Sub UpdateStartPositions()
Private Sub UpdateSizes()
```

- Modular functions to calculate layout and font sizes based on window dimensions.

---

## 🧾 Drawing Functions

### Decimal Value

```vbnet
Private Sub DrawDecimalValue()
```

- Converts bits to decimal and draws it centered above the boxes.

### Bit Boxes

```vbnet
Private Sub DrawBitBoxes()
```

- Draws each box, binary digit, and place value.

### Breakdown

```vbnet
Private Sub DrawActiveValuesBreakdown()
```

- Shows which place values are active and their sum.

---

## 🔊 Sound File Setup

```vbnet
Private Sub CreateSoundFiles()
Private Sub CreateFileFromResource(filepath As String, resource As Byte())
```

- Extracts embedded sound resources and saves them as `.mp3` files.

---


























<img width="1920" height="1080" alt="005" src="https://github.com/user-attachments/assets/715f8259-cfb2-4b72-95eb-895418321963" />














