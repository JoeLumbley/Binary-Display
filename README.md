# Binary Display

A interactive app for learning binary numbers through visual feedback and sound.



<img width="1920" height="1080" alt="006" src="https://github.com/user-attachments/assets/694a3165-eef6-475d-9dca-89ea89a8ff06" />



---

[Code Walkthrough](#code-walkthrough) | [Binary Basics](#binary-basics)



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

### 🎯 Educational Goals

This app is ideal for:
- Teaching binary concepts in a visually engaging way
- Reinforcing place value and bit significance
- Creating emotionally affirming learning environments with sound and color
- Demonstrating how binary maps to decimal in real time

---

### 🛠️ Technologies Used

- **VB.NET WinForms**
- **System.Drawing** for custom rendering
- **Custom AudioPlayer** for sound layering and looping
- **Dynamic font scaling** for accessibility

---

### 🚀 Getting Started

1. Clone the repo
2. Build and run the project in Visual Studio
3. Click or use arrow keys to explore binary values
4. Enjoy the sound-enhanced feedback loop!

---

### Binary Basics

**Binary is a numbering system using only two digits 0 and 1.**

**Each binary digit is called a _bit_.**

**Each bit has a place value, just like decimal — but based on powers of 2.**

**A group of 8 bits is called a _byte_.**

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
- **Method Declaration**: This line defines the `OnPaint` method, which is protected and overrides the base class's method. It takes a parameter `e` of type `PaintEventArgs`, which contains information about the paint event, including the graphics object used for drawing.

```vbnet
    MyBase.OnPaint(e)
```
- **Call Base Method**: This line calls the base class implementation of `OnPaint`, ensuring that any default behavior defined in the parent class is executed before any custom painting occurs.

```vbnet
    Graph = e.Graphics
```
- **Assign Graphics Object**: This line assigns the `Graphics` object from the `PaintEventArgs` (`e.Graphics`) to a class-level variable `Graph`. This allows for easy access to the graphics context for subsequent drawing operations.

```vbnet
    Graph.CompositingMode = Drawing2D.CompositingMode.SourceOver
```
- **Set Compositing Mode**: This line sets the compositing mode of the graphics object to `SourceOver`. This mode ensures that the source (the drawn elements) is blended with the destination (the existing content) based on their alpha values, allowing for proper transparency handling.

```vbnet
    Graph.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
```
- **Set Text Rendering Hint**: This line sets the text rendering hint to `AntiAlias`, which improves the quality of rendered text by smoothing the edges. This results in a more visually appealing appearance for any text drawn on the control.

```vbnet
    DrawBitBoxes()
```
- **Draw Bit Boxes**: This line calls a method named `DrawBitBoxes()`, which is responsible for rendering the graphical representation of the bit boxes. This method likely iterates through the `Bits` array and draws each corresponding rectangle.

```vbnet
    DrawDecimalValue()
```
- **Draw Decimal Value**: This line calls a method named `DrawDecimalValue()`, which is responsible for rendering the current decimal value represented by the bits. This could involve converting the binary representation to a decimal and displaying it on the control.

```vbnet
    DrawActiveValuesBreakdown()
```
- **Draw Active Values Breakdown**: This line calls a method named `DrawActiveValuesBreakdown()`, which likely renders additional information about the active values or the state of the bits. This could include visual indicators or textual descriptions of the bits that are currently set to `True`.

```vbnet
End Sub
```
- **End Subroutine**: This line indicates the end of the `OnPaint` subroutine.


The `OnPaint` subroutine is responsible for rendering the visual elements of the control. It initializes the graphics context, sets rendering properties for better visual quality, and calls specific methods to draw the bit boxes, the decimal value, and any additional information about the active values. This method is crucial for ensuring that the graphical representation of the data is updated and displayed correctly whenever the control needs to be repainted.

---

## 🖱️ Mouse Click


```vbnet
Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
```
- **Method Declaration**: This line defines the `OnMouseClick` method, which is protected and overrides the base class's method. It takes a parameter `e` of type `MouseEventArgs`, which contains information about the mouse click event.

```vbnet
    MyBase.OnMouseClick(e)
```
- **Call Base Method**: This line calls the base class implementation of `OnMouseClick`, ensuring that any default behavior defined in the parent class is executed.

```vbnet
    For i = 0 To 7
```
- **Loop Initialization**: This line starts a loop that will iterate from 0 to 7, allowing the code to check each of the 8 bit rectangles.

```vbnet
        If BitRects(i).Contains(e.Location) Then
```
- **Check Click Location**: This line checks if the location of the mouse click (`e.Location`) is within the rectangle corresponding to the current bit (`BitRects(i)`). If it is, the following code will execute.

```vbnet
            Player.PlayOverlapping("CashCollected")
```
- **Play Sound**: This line plays a sound effect named "CashCollected" using a `Player` object. This sound provides feedback to the user when they interact with a bit box.

```vbnet
            Bits(i) = Not Bits(i)
```
- **Toggle Bit State**: This line toggles the state of the bit at index `i`. If the bit was `True` (or `1`), it changes to `False` (or `0`), and vice versa. The `Not` operator inverts the boolean value.

```vbnet
            Me.Invalidate()
```
- **Invalidate Control**: This line calls `Invalidate()` on the current control (`Me`), which marks the control for redrawing. This will trigger a repaint, updating the visual representation of the bit boxes to reflect the new state.

```vbnet
            Exit For
```
- **Exit Loop**: This line exits the `For` loop immediately after toggling the bit state for the clicked rectangle, preventing further checks since only one bit can be toggled per click.

```vbnet
        Next
```
- **End Loop**: This line marks the end of the `For` loop, which will continue to the next index until all bits are processed, but will not execute further if a bit has already been toggled.

```vbnet
    End Sub
```
- **End Subroutine**: This line indicates the end of the `OnMouseClick` subroutine.


The `OnMouseClick` subroutine allows users to interact with the graphical representation of bits by clicking on them. When a bit box is clicked, the corresponding bit's state is toggled, a sound is played for feedback, and the display is updated to reflect the change. This creates an interactive experience where users can manipulate binary values visually.

---


## 🖱️ Mouse Move




```vbnet
Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
```
- **Method Declaration**: This line defines the `OnMouseMove` method, which is protected and overrides the base class's method. It takes a parameter `e` of type `MouseEventArgs`, which contains information about the mouse movement event.

```vbnet
    MyBase.OnMouseMove(e)
```
- **Call Base Method**: This line calls the base class implementation of `OnMouseMove`, ensuring that any default behavior defined in the parent class is executed.

```vbnet
    Dim newHoverIndex As Integer = -1
```
- **Initialize Hover Index**: This line initializes a variable `newHoverIndex` to -1. This variable will be used to track the index of the bit box currently being hovered over.

```vbnet
    For i = 0 To 7
```
- **Loop Initialization**: This line starts a loop that will iterate from 0 to 7, allowing the code to check each of the 8 bit rectangles.

```vbnet
        If BitRects(i).Contains(e.Location) Then
```
- **Check Hover Location**: This line checks if the location of the mouse (`e.Location`) is within the rectangle corresponding to the current bit (`BitRects(i)`). If it is, the following code will execute.

```vbnet
            newHoverIndex = i
```
- **Update Hover Index**: If the mouse is over the current bit rectangle, this line updates `newHoverIndex` to the current index `i`.

```vbnet
            Exit For
```
- **Exit Loop**: This line exits the `For` loop immediately after finding the hovered bit box, preventing further checks since only one bit box can be hovered at a time.

```vbnet
        Next
```
- **End Loop**: This line marks the end of the `For` loop, which processes all 8 bit rectangles.

```vbnet
    If newHoverIndex <> HoveredBitIndex Then
```
- **Check for Hover Change**: This line checks if the newly detected hover index (`newHoverIndex`) is different from the previously stored hover index (`HoveredBitIndex`).

```vbnet
        HoveredBitIndex = newHoverIndex
```
- **Update Hovered Index**: If the hover index has changed, this line updates `HoveredBitIndex` to the new value.

```vbnet
        Me.Invalidate()
```
- **Invalidate Control**: This line calls `Invalidate()` on the current control (`Me`), marking it for redrawing. This will trigger a repaint, updating the visual representation of the bit boxes to reflect the new hover state.

```vbnet
    Cursor = If(HoveredBitIndex <> -1, Cursors.Hand, Cursors.Default)
```
- **Change Cursor**: This line sets the cursor based on whether a bit box is being hovered over:
  - If `HoveredBitIndex` is not -1 (indicating a bit box is hovered), the cursor changes to a hand icon (`Cursors.Hand`).
  - If no bit box is hovered (i.e., `HoveredBitIndex` is -1), the cursor defaults to the standard arrow icon (`Cursors.Default`).

```vbnet
End Sub
```
- **End Subroutine**: This line indicates the end of the `OnMouseMove` subroutine.


The `OnMouseMove` subroutine updates the hover state of the bit boxes in response to mouse movement. It determines which bit box, if any, is currently being hovered over, updates the hover index, triggers a repaint of the control to reflect any changes, and adjusts the cursor to provide visual feedback to the user. This enhances the interactivity of the graphical user interface by indicating which bit box can be clicked.



---

## ⌨️ Keyboard Input



```vbnet
Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
```
- **Method Declaration**: This line defines the `OnKeyDown` method, which is protected and overrides the base class's method. It takes a parameter `e` of type `KeyEventArgs`, which contains information about the key event.

```vbnet
    MyBase.OnKeyDown(e)
```
- **Call Base Method**: This line calls the base class implementation of `OnKeyDown`, ensuring that any default behavior defined in the parent class is executed.

```vbnet
    Dim currentValue = Convert.ToInt32(String.Join("", Bits.Select(Function(b) If(b, "1", "0"))), 2)
```
- **Current Value Calculation**: This line calculates the current integer value represented by the `Bits` array:
  - `Bits.Select(Function(b) If(b, "1", "0"))` converts each boolean bit to a string ("1" for `True`, "0" for `False`).
  - `String.Join("", ...)` concatenates these strings into a single binary string.
  - `Convert.ToInt32(..., 2)` converts the binary string to an integer (base 2).

```vbnet
    If e.KeyCode = Keys.Up Then
```
- **Check for Up Arrow Key**: This line checks if the up arrow key was pressed.

```vbnet
        If currentValue < 255 Then
```
- **Upper Limit Check**: This line checks if the current value is less than 255 (the maximum value for an 8-bit binary number).

```vbnet
            currentValue += 1
```
- **Increment Value**: If the current value is less than 255, this line increments it by 1.

```vbnet
            Player.PlayOverlapping("CashCollected")
```
- **Play Sound**: This line plays a sound effect named "CashCollected" to provide feedback to the user when the value is incremented.

```vbnet
        End If
```
- **End If Statement**: This line marks the end of the conditional check for the up arrow key.

```vbnet
    ElseIf e.KeyCode = Keys.Down Then
```
- **Check for Down Arrow Key**: This line checks if the down arrow key was pressed.

```vbnet
        If currentValue > 0 Then
```
- **Lower Limit Check**: This line checks if the current value is greater than 0.

```vbnet
            currentValue -= 1
```
- **Decrement Value**: If the current value is greater than 0, this line decrements it by 1.

```vbnet
            Player.PlayOverlapping("CashCollected")
```
- **Play Sound**: This line plays the same sound effect "CashCollected" to provide feedback to the user when the value is decremented.

```vbnet
        End If
```
- **End If Statement**: This line marks the end of the conditional check for the down arrow key.

```vbnet
    Else
```
- **Else Condition**: This line begins the else block, which handles cases where neither the up nor down key was pressed.

```vbnet
        Return
```
- **Exit Method**: This line exits the subroutine early if neither the up nor down key is pressed, preventing further execution.

```vbnet
    End If
```
- **End If Statement**: This line closes the outer if-else structure.

```vbnet
    ' Update bits array
```
- **Comment**: This comment indicates that the following code will update the `Bits` array based on the new value.

```vbnet
    Dim binaryStr = Convert.ToString(currentValue, 2).PadLeft(8, "0"c)
```
- **Binary String Conversion**: This line converts the `currentValue` back to a binary string:
  - `Convert.ToString(currentValue, 2)` converts the integer to a binary string.
  - `.PadLeft(8, "0"c)` ensures the string is 8 characters long, padding with leading zeros if necessary.

```vbnet
    For i = 0 To 7
```
- **Loop Initialization**: This line starts a loop that will iterate from 0 to 7, allowing the code to update each bit in the `Bits` array.

```vbnet
        Bits(i) = binaryStr(i) = "1"
```
- **Update Bits Array**: This line updates the `Bits` array:
  - `binaryStr(i) = "1"` checks if the `i`-th character of the binary string is "1".
  - The result (a boolean value) is assigned to `Bits(i)`, setting the bit to `True` if the character is "1" and `False` otherwise.

```vbnet
    Next
```
- **End Loop**: This line marks the end of the `For` loop, which processes all 8 bits.

```vbnet
    Me.Invalidate()
```
- **Invalidate Control**: This line calls `Invalidate()` on the current control (`Me`), marking it for redrawing. This will trigger a repaint, updating the visual representation of the bit boxes to reflect the new state.

```vbnet
End Sub
```
- **End Subroutine**: This line indicates the end of the `OnKeyDown` subroutine.

The `OnKeyDown` subroutine allows users to increment or decrement a binary value using the up and down arrow keys. When the up arrow is pressed, the value increases (up to a maximum of 255), and when the down arrow is pressed, the value decreases (down to a minimum of 0). The corresponding sound is played for feedback, and the visual representation of the bits is updated to reflect the new value.

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


---


### Bit Boxes

```vbnet
Private Sub DrawBitBoxes()
```
- **Purpose**: This line defines a new subroutine named `DrawBitBoxes`. The `Private` keyword indicates that this subroutine can only be accessed within the same class or module.

```vbnet
    ' Draw bit boxes.
```
- **Comment**: This is a comment that describes what the following code will do. Comments are ignored by the compiler but are useful for developers to understand the code.

```vbnet
    For i = 0 To 7
```
- **Loop Initialization**: This line starts a loop that will iterate from 0 to 7, which means it will run 8 times (for each bit in a byte).

```vbnet
        Rect = BitRects(i)
```
- **Rectangle Assignment**: This line assigns the rectangle corresponding to the current bit (indexed by `i`) from an array called `BitRects`. Each rectangle represents the area where the bit will be drawn.

```vbnet
        Graph.FillRectangle(If(Bits(i), BitOnBrush, BitOffBrush), Rect)
```
- **Fill Rectangle**: This line fills the rectangle with a color based on the state of the bit (`Bits(i)`). If the bit is `True` (or `1`), it uses `BitOnBrush`; if `False` (or `0`), it uses `BitOffBrush`.

```vbnet
        ' Draw the binary digit centered horizontally and vertically inside of the bit boxes.
```
- **Comment**: Another comment indicating that the next lines will handle drawing the binary digit.

```vbnet
        Graph.DrawString(If(Bits(i), "1", "0"), BitBoxFont, If(Bits(i), TextOnBrush, TextOffBrush), 
                          Rect.X + (BitBoxSize - Graph.MeasureString(If(Bits(i), "1", "0"), BitBoxFont).Width) / 2,
                          Rect.Y + (BitBoxSize - Graph.MeasureString(If(Bits(i), "1", "0"), BitBoxFont).Height) / 2)
```
- **Draw String**: This complex line draws the binary digit (either "1" or "0") inside the rectangle:
  - The first part `If(Bits(i), "1", "0")` checks the bit value.
  - `BitBoxFont` specifies the font used for the text.
  - `If(Bits(i), TextOnBrush, TextOffBrush)` chooses the text color based on the bit state.
  - The `X` and `Y` coordinates are calculated to center the text within the box by subtracting half the width and height of the string from the rectangle's coordinates.

```vbnet
        ' Draw place value above each bit box.
```
- **Comment**: Indicates that the next lines will draw the place value for each bit.

```vbnet
        PlaceValue = CStr(2 ^ (7 - i))
```
- **Place Value Calculation**: This line calculates the place value of the bit. For a binary representation, the place value is determined as \(2^{(7 - i)}\). For example, when `i` is 0, the place value is 128 (or \(2^7\)).

```vbnet
        Graph.DrawString(PlaceValue, PlaceValueFont, PlaceValueBrush, 
                          Rect.X + BitBoxSize \ 2, 
                          Rect.Y - Me.ClientSize.Height \ 12, 
                          AlineCenter)
```
- **Draw Place Value**: This line draws the calculated place value above the corresponding bit box:
  - `PlaceValueFont` specifies the font for the place value text.
  - `PlaceValueBrush` is the color used for the text.
  - The coordinates position the place value above the box, adjusted by half the box size and a fraction of the client area height.

```vbnet
        ' Draw border if hovered.
```
- **Comment**: Indicates that the next lines will check if the bit box is hovered over and draw a border if it is.

```vbnet
        If i = HoveredBitIndex Then
```
- **Hover Check**: This line checks if the current index `i` matches the index of the hovered bit.

```vbnet
            Using borderPen As New Pen(If(Bits(i), Color.DeepPink, Color.OrangeRed), BitSpacing / 3)
```
- **Border Pen Creation**: This line creates a new pen for drawing borders. The color depends on the state of the bit (`Bits(i)`), and the pen width is set based on `BitSpacing`.

```vbnet
                borderPen.Alignment = Drawing2D.PenAlignment.Outset
```
- **Pen Alignment**: This line sets the alignment of the pen to be outside the rectangle, ensuring the border appears correctly.

```vbnet
                Graph.DrawRectangle(borderPen, Rect)
```
- **Draw Rectangle**: This line draws the border around the rectangle using the created pen.

```vbnet
            End Using
```
- **End Using Block**: This line indicates the end of the `Using` block for the `borderPen`, ensuring proper resource management.

```vbnet
        End If
```
- **End If Statement**: This line marks the end of the conditional check for hover.

```vbnet
    Next
```
- **End Loop**: This line marks the end of the `For` loop, which will continue to the next index until all bits are processed.

```vbnet
End Sub
```
- **End Subroutine**: This line indicates the end of the `DrawBitBoxes` subroutine.


The `DrawBitBoxes` subroutine visually represents the bits of a byte as boxes filled with colors, displays the binary digits inside the boxes, shows their place values above, and highlights the box when hovered over. This code is part of a graphical user interface that helps users visualize binary data in a clear and interactive way.


---



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



















<img width="1920" height="1080" alt="007" src="https://github.com/user-attachments/assets/fb979af1-196f-41cf-985d-c344796152f0" />





















