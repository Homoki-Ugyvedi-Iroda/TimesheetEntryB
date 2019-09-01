Imports System.ComponentModel
Imports SPHelper.SPHUI

Public Class EntryForm
    Property Connection As SPHelper.SPHUI
    Public Property Persons As List(Of PersonClass)
    Public Property Matters As List(Of MatterClass)
    Public Property Users As List(Of UserId)
    Public Property Tasks As List(Of TaskClass)
    Public Property TimesheetEntries As List(Of TimesheetEntry)
    Public Shared Logger As New HPHelper.DebugTesting(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name & ".log")

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
        LoadSpLookupValues()
    End Sub
    Public Async Sub LoadSpLookupValues()
        'deserialize-oltokat betölti, és beállítja az async-ot
        'Matter értékek: Id, Value, Active
        'Person értékek: adott matterhöz tartozó personok = Id, Value, Active, Matter
        'Users értékek: Id, loginname
        Dim m As New DataLayer()
        Me.Users = Await m.GetAllUsers(Me.Connection.Context)
        Me.Matters = Await m.GetAllMattersAsync(Me.Connection.Context)
        Me.Persons = Await m.GetAllPersonsAsync(Me.Connection.Context)
        Me.Tasks = Await m.GetAllTasksAsync(Me.Connection.Context)
        Me.TimesheetEntries = Await m.GetAllTsheetAsync(Me.Connection.Context)
        Message.Text = "kapcsolódott SP-hoz"
        DateCompleted.Value = Today

        FillandFindUser

        Dim MatterSource As New BindingList(Of MatterClass)
        For Each item In Me.Matters
            If item.Active = True Then MatterSource.Add(item)
        Next
        cbMatterPicker.DataSource = MatterSource
        cbMatterPicker.DisplayMember = "Value"
        cbMatterPicker.SelectedItem = Nothing

        UpdateValues()
        'Task!
    End Sub
    Private Sub UpdateValues()
        Dim PersonSource As New BindingList(Of PersonClass)
        For Each person As PersonClass In Me.Persons
            For Each matter As MatterClass In person.Matter
                If IsNothing(cbMatterPicker.SelectedItem) Then
                    PersonSource.Add(person)
                Else
                    If matter.Equals(cbMatterPicker.SelectedItem) Then PersonSource.Add(person)
                End If
            Next
        Next
        cbPersons.DataSource = PersonSource
        cbPersons.DisplayMember = "Value"
        cbPersons.SelectedItem = Nothing

        Dim TaskSource As New BindingList(Of TaskClass)
        For Each task As TaskClass In Me.Tasks
            If IsNothing(cbMatterPicker.SelectedItem) Then
                TaskSource.Add(task)
                Continue For
            End If
            If Not IsNothing(task.Matter) AndAlso task.Matter.Equals(cbMatterPicker.SelectedItem) Then
                TaskSource.Add(task)
            End If
        Next
        cbTaskChooser.DataSource = TaskSource
        cbTaskChooser.DisplayMember = "TitleOrTaskName"
        cbTaskChooser.SelectedItem = Nothing
        SetAutoComplete()
    End Sub
    Private Sub SetAutoComplete()
        Dim AllPreviousReviewersInTs As New AutoCompleteStringCollection
        For Each entry As TimesheetEntry In Me.TimesheetEntries
            If IsNothing(cbMatterPicker.SelectedItem) Then
                AllPreviousReviewersInTs.Add(entry.Reviewer)
            Else
                If Not IsNothing(entry.Matter) AndAlso entry.Matter.Equals(cbMatterPicker.SelectedItem) Then _
                    AllPreviousReviewersInTs.Add(entry.Reviewer)
            End If
        Next
        For Each entry As TaskClass In Me.Tasks
            If IsNothing(cbMatterPicker.SelectedItem) Then
                If Not IsNothing(entry.Reviewer) Then AllPreviousReviewersInTs.Add(entry.Reviewer)
            Else
                If Not IsNothing(entry.Matter) AndAlso entry.Matter.Equals(cbMatterPicker.SelectedItem) Then _
                    AllPreviousReviewersInTs.Add(entry.Reviewer)
            End If
        Next
        tbReviewer.AutoCompleteCustomSource = AllPreviousReviewersInTs

        Dim AllPreviousDescriptions As New AutoCompleteStringCollection
        For Each entry As TimesheetEntry In Me.TimesheetEntries
            If IsNothing(cbMatterPicker.SelectedItem) Then
                AllPreviousDescriptions.Add(entry.Description)
            Else
                If Not IsNothing(entry.Matter) AndAlso entry.Matter.Equals(cbMatterPicker.SelectedItem) Then AllPreviousDescriptions.Add(entry.Description)
            End If
        Next
        AllPreviousDescriptions.Add("szerződéstervezet készítése")
        AllPreviousDescriptions.Add("szerződéstervezet véleményezése")
        AllPreviousDescriptions.Add("kiszervezési")
        'Hozzáadni TaskType-okat SP-ből?
        tbDescription.AutoCompleteSource = AutoCompleteSource.CustomSource
        tbDescription.AutoCompleteCustomSource = AllPreviousDescriptions
        tbDescription.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        'cbDescription.AutoCompleteSource = AutoCompleteSource.CustomSource
        'cbDescription.AutoCompleteCustomSource = AllPreviousDescriptions
        'cbDescription.AutoCompleteMode = AutoCompleteMode.SuggestAppend


    End Sub

    Private Sub FillAndFindUser()
        Dim UserSource As New BindingList(Of UserId)
        For Each item In Me.Users
            UserSource.Add(item)
        Next
        cbRecorder.DataSource = UserSource
        cbRecorder.DisplayMember = "DisplayName"
        Dim ItemToSelectForUser As UserId
        Dim CurrentUserEmail = My.Settings.EmailCurrentUser
        If Not String.IsNullOrWhiteSpace(CurrentUserEmail) Then
            ItemToSelectForUser = UserSource.Where(Function(x) x.Email = CurrentUserEmail).FirstOrDefault
        Else
            Dim EmailAddress = InputBox("Adja meg az emailcímét")
            If Not String.IsNullOrWhiteSpace(EmailAddress) Then
                My.Settings.EmailCurrentUser = EmailAddress
                My.Settings.Save()
                ItemToSelectForUser = UserSource.Where(Function(x) x.Email = CurrentUserEmail).FirstOrDefault
            End If
        End If
        If IsNothing(ItemToSelectForUser) Then
            Dim smallest As Integer = 999
            Dim bestId As Integer = 0
            For Each user As UserId In Me.Users
                Dim LevDisCurrent = HPHelper.StringRelated.Levenshtein(user.DisplayName, System.Security.Principal.WindowsIdentity.GetCurrent.Name)
                If LevDisCurrent < smallest Then
                    bestId = user.ID
                    smallest = LevDisCurrent
                End If
            Next
            ItemToSelectForUser = UserSource.Where(Function(x) x.ID = bestId).FirstOrDefault
        End If
        cbRecorder.SelectedItem = ItemToSelectForUser
    End Sub
    Private Sub Record_Click(sender As Object, e As EventArgs) Handles Record.Click
        Dim NewEntry = TimesheetEntryFromGUI()
        Dim mySpHui As New TimesheetEntry
        mySpHui.SaveNewEntryInSp(Me.Connection.Context, NewEntry)
        tbDescription.Text = String.Empty
        cbMatterPicker.SelectedItem = Nothing
        Label1.Text = "rögzítve az SP-ben: " & NewEntry.Description & ", " & cbPersons.Text
        'Törölni meglévő értéket és jelezni, hogy rögzítette!
    End Sub

    Private Function TimesheetEntryFromGUI() As TimesheetEntry
        Dim NewEntry As New TimesheetEntry
        NewEntry.Reviewer = tbReviewer.Text
        NewEntry.DateCompletion = DateCompleted.Value.ToUniversalTime '?
        NewEntry.RealValue = RealValue.Value
        If Chargeable.ForeColor = System.Drawing.SystemColors.GrayText Then NewEntry.ChargeableValue = NewEntry.RealValue Else NewEntry.ChargeableValue = Chargeable.Value
        NewEntry.Recorder = cbRecorder.SelectedItem
        NewEntry.Matter = cbMatterPicker.SelectedItem
        NewEntry.Task = cbTaskChooser.SelectedItem
        NewEntry.Description = tbDescription.Text
        NewEntry.PartnerNames = cbPersons.Text
        Return NewEntry
    End Function


    Private Sub cbMatterPicker_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbMatterPicker.SelectionChangeCommitted
        UpdateValues()
    End Sub

    Private Sub tbReviewer_Validated(sender As Object, e As EventArgs) Handles tbReviewer.Validated
        If tbReviewer.Text.First = " " Then Exit Sub
        Dim PartedText As String() = tbReviewer.Text.Split(" ")
        Dim javitott As New List(Of String)
        For Each _text In PartedText
            javitott.Add(_text.Substring(0, 1).ToUpper & _text.Substring(1, _text.Length - 1))
        Next
        Dim javitottEgybe = String.Join(" ", javitott)
    End Sub

    Private Sub Chargeable_Doubleclick(sender As Object, e As EventArgs) Handles Chargeable.DoubleClick
        If Chargeable.BackColor = System.Drawing.SystemColors.Window Then
            Me.Chargeable.BackColor = System.Drawing.SystemColors.MenuBar
            Me.Chargeable.ForeColor = System.Drawing.SystemColors.GrayText
        Else
            Chargeable.ForeColor = System.Drawing.SystemColors.ControlText
            Chargeable.BackColor = System.Drawing.SystemColors.Window
        End If
    End Sub
End Class
