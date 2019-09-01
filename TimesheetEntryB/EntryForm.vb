Public Class EntryForm
    Property Connection As SPHelper.SPHUI
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Me.SetDesktopLocation(0, Screen.PrimaryScreen.WorkingArea.Size.Height - Me.Size.Height)
        Me.Icon = My.Resources.tsheet
        Dim MySP As New SPHelper.SPHUI()
        MySP.Connect(My.Settings.SpUrl)
        If MySP.Connected = True Then
            Connection = MySP
            Me.Connection = MySP
        Else
            MsgBox("Nem érhető el a Sharepoint szerver, offline.")
        End If

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class
