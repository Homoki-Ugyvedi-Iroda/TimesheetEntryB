<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class EntryForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Message = New System.Windows.Forms.Label()
        Me.cbMatterPicker = New System.Windows.Forms.ComboBox()
        Me.Record = New System.Windows.Forms.Button()
        Me.cbRecorder = New System.Windows.Forms.ComboBox()
        Me.DateCompleted = New System.Windows.Forms.DateTimePicker()
        Me.cbPersons = New System.Windows.Forms.ComboBox()
        Me.tbReviewer = New System.Windows.Forms.TextBox()
        Me.Chargeable = New System.Windows.Forms.NumericUpDown()
        Me.RealValue = New System.Windows.Forms.NumericUpDown()
        Me.cbTaskChooser = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.tbDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.Chargeable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RealValue, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Message
        '
        Me.Message.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Message.AutoSize = True
        Me.Message.Location = New System.Drawing.Point(839, 25)
        Me.Message.Name = "Message"
        Me.Message.Size = New System.Drawing.Size(39, 13)
        Me.Message.TabIndex = 8
        Me.Message.Text = "Label1"
        '
        'cbMatterPicker
        '
        Me.cbMatterPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMatterPicker.FormattingEnabled = True
        Me.cbMatterPicker.Location = New System.Drawing.Point(179, 1)
        Me.cbMatterPicker.Name = "cbMatterPicker"
        Me.cbMatterPicker.Size = New System.Drawing.Size(105, 21)
        Me.cbMatterPicker.TabIndex = 0
        '
        'Record
        '
        Me.Record.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Record.Location = New System.Drawing.Point(1316, -1)
        Me.Record.Name = "Record"
        Me.Record.Size = New System.Drawing.Size(50, 23)
        Me.Record.TabIndex = 7
        Me.Record.Text = "Rögzít"
        Me.Record.UseVisualStyleBackColor = True
        '
        'cbRecorder
        '
        Me.cbRecorder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbRecorder.FormattingEnabled = True
        Me.cbRecorder.Location = New System.Drawing.Point(80, 1)
        Me.cbRecorder.Name = "cbRecorder"
        Me.cbRecorder.Size = New System.Drawing.Size(93, 21)
        Me.cbRecorder.TabIndex = 10
        '
        'DateCompleted
        '
        Me.DateCompleted.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.DateCompleted.CustomFormat = ""
        Me.DateCompleted.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DateCompleted.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateCompleted.Location = New System.Drawing.Point(2, 1)
        Me.DateCompleted.Name = "DateCompleted"
        Me.DateCompleted.Size = New System.Drawing.Size(72, 18)
        Me.DateCompleted.TabIndex = 9
        '
        'cbPersons
        '
        Me.cbPersons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbPersons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbPersons.FormattingEnabled = True
        Me.cbPersons.Location = New System.Drawing.Point(602, 1)
        Me.cbPersons.Name = "cbPersons"
        Me.cbPersons.Size = New System.Drawing.Size(234, 21)
        Me.cbPersons.TabIndex = 3
        '
        'tbReviewer
        '
        Me.tbReviewer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.tbReviewer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.tbReviewer.Location = New System.Drawing.Point(291, 1)
        Me.tbReviewer.Name = "tbReviewer"
        Me.tbReviewer.Size = New System.Drawing.Size(134, 20)
        Me.tbReviewer.TabIndex = 1
        '
        'Chargeable
        '
        Me.Chargeable.BackColor = System.Drawing.SystemColors.MenuBar
        Me.Chargeable.DecimalPlaces = 2
        Me.Chargeable.ForeColor = System.Drawing.SystemColors.GrayText
        Me.Chargeable.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.Chargeable.Location = New System.Drawing.Point(1266, 1)
        Me.Chargeable.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.Chargeable.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.Chargeable.Name = "Chargeable"
        Me.Chargeable.Size = New System.Drawing.Size(44, 20)
        Me.Chargeable.TabIndex = 6
        '
        'RealValue
        '
        Me.RealValue.DecimalPlaces = 2
        Me.RealValue.Increment = New Decimal(New Integer() {25, 0, 0, 131072})
        Me.RealValue.Location = New System.Drawing.Point(1204, 1)
        Me.RealValue.Maximum = New Decimal(New Integer() {24, 0, 0, 0})
        Me.RealValue.Name = "RealValue"
        Me.RealValue.Size = New System.Drawing.Size(42, 20)
        Me.RealValue.TabIndex = 5
        Me.RealValue.Value = New Decimal(New Integer() {25, 0, 0, 131072})
        '
        'cbTaskChooser
        '
        Me.cbTaskChooser.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbTaskChooser.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbTaskChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTaskChooser.FormattingEnabled = True
        Me.cbTaskChooser.Location = New System.Drawing.Point(431, 1)
        Me.cbTaskChooser.Name = "cbTaskChooser"
        Me.cbTaskChooser.Size = New System.Drawing.Size(165, 21)
        Me.cbTaskChooser.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(288, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Teljesítésigazolók"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(428, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Feladat (ha volt)"
        '
        'tbDescription
        '
        Me.tbDescription.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.tbDescription.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.tbDescription.Location = New System.Drawing.Point(842, 1)
        Me.tbDescription.Name = "tbDescription"
        Me.tbDescription.Size = New System.Drawing.Size(356, 20)
        Me.tbDescription.TabIndex = 4
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(599, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(135, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Ügyfélpartner, más személy"
        '
        'EntryForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1364, 41)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbTaskChooser)
        Me.Controls.Add(Me.RealValue)
        Me.Controls.Add(Me.Chargeable)
        Me.Controls.Add(Me.tbReviewer)
        Me.Controls.Add(Me.cbPersons)
        Me.Controls.Add(Me.DateCompleted)
        Me.Controls.Add(Me.cbRecorder)
        Me.Controls.Add(Me.Record)
        Me.Controls.Add(Me.tbDescription)
        Me.Controls.Add(Me.cbMatterPicker)
        Me.Controls.Add(Me.Message)
        Me.MinimumSize = New System.Drawing.Size(16, 80)
        Me.Name = "EntryForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        CType(Me.Chargeable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RealValue, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Message As Label
    Friend WithEvents cbMatterPicker As ComboBox
    Friend WithEvents Record As Button
    Friend WithEvents cbRecorder As ComboBox
    Friend WithEvents DateCompleted As DateTimePicker
    Friend WithEvents cbPersons As ComboBox
    Friend WithEvents tbReviewer As TextBox
    Friend WithEvents Chargeable As NumericUpDown
    Friend WithEvents RealValue As NumericUpDown
    Friend WithEvents cbTaskChooser As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents tbDescription As TextBox
    Friend WithEvents Label3 As Label
End Class
