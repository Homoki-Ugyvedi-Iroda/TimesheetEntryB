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
    Private Const SpTimesheetNotInvoicedView = "https://homoki.sharepoint.com/Lists/Time%20Entry/AllItems.aspx?viewid=e1638d1e%2Df79d%2D4a65%2Da394%2D95a32243b7e6"
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
        TimerForRefresh.Start()
    End Sub
    Public Async Sub LoadSpLookupValues()
        'deserialize-oltokat betölti, és beállítja az async-ot
        'Matter értékek: Id, Value, Active
        'Person értékek: adott matterhöz tartozó personok = Id, Value, Active, Matter
        'Users értékek: Id, loginname
        Logger.WriteInfo("Public Async Sub LoadSpLookupValues")
        Message.Text = "kapcsolódott SP-hoz"
        Await RefreshSpLookupValues()
        DateCompleted.Value = Today
        FillAndFindUser()

        'Esetleg az alábbi Matterrel kapcsolatos részt külön Sub-ba?
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
    Public Async Function RefreshSpLookupValues() As Task
        Logger.WriteInfo("RefreshSpLookupValues")
        If Not IsNothing(Me.Tasks) Then Logger.WriteInfo("Taskcount before: " & Me.Tasks.Count)
        Dim m As New DataLayer()
        Me.Users = Await m.GetAllUsers(Me.Connection.Context)
        Me.Matters = Await m.GetAllMattersAsync(Me.Connection.Context)
        Me.Persons = Await m.GetAllPersonsAsync(Me.Connection.Context)
        Me.Tasks = Await m.GetAllTasksAsync(Me.Connection.Context)
        Me.TimesheetEntries = Await m.GetAllTsheetAsync(Me.Connection.Context)
    End Function
    Private Sub UpdateValues()
        Logger.WriteInfo("UpdateValues")

        Dim PersonSource As New BindingList(Of PersonClass)
        For Each person As PersonClass In Me.Persons
            PersonSource.Add(person)
            '#Ha le kell szűkíteni csak az adott Matter partnereire, akkor az alábbival tehetjük meg, de egyelőre nem fontos
            'For Each matter As MatterClass In person.Matter
            'If IsNothing(cbMatterPicker.SelectedItem) Then
            '    PersonSource.Add(person)
            'Else
            '    If matter.Equals(cbMatterPicker.SelectedItem) Then PersonSource.Add(person)
            'End If
            'Next
        Next
        cbPersons.DataSource = PersonSource
        cbPersons.DisplayMember = "Value"
        cbPersons.SelectedItem = Nothing
        Logger.WriteInfo("cbPersonSource numbers: " & Me.cbPersons.Items.Count)

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
        Logger.WriteInfo("cbTask numbers: " & Me.cbTaskChooser.Items.Count)

        SetAutoComplete()
    End Sub
    Private Sub SetAutoComplete()
        Logger.WriteInfo("SetAutoComplete")

        Dim AllPreviousReviewersInTs As New AutoCompleteStringCollection
        tbReviewer.AutoCompleteCustomSource.Clear()
        For Each entry As TimesheetEntry In Me.TimesheetEntries
            If String.IsNullOrEmpty(entry.Reviewer) Then Continue For
            If IsNothing(cbMatterPicker.SelectedItem) Then
                If Not AllPreviousReviewersInTs.Contains(entry.Reviewer) Then AllPreviousReviewersInTs.Add(entry.Reviewer)
            Else
                If Not IsNothing(entry.Matter) AndAlso Not AllPreviousReviewersInTs.Contains(entry.Reviewer) AndAlso entry.Matter.Equals(cbMatterPicker.SelectedItem) Then _
                    AllPreviousReviewersInTs.Add(entry.Reviewer)
            End If
        Next
        Logger.WriteInfo("SetAutoComplete_tbReviewer:" & AllPreviousReviewersInTs.Count)

        For Each entry As TaskClass In Me.Tasks
            If String.IsNullOrEmpty(entry.Reviewer) Then Continue For
            If IsNothing(cbMatterPicker.SelectedItem) Then
                If Not AllPreviousReviewersInTs.Contains(entry.Reviewer) Then AllPreviousReviewersInTs.Add(entry.Reviewer)
            Else
                If Not IsNothing(entry.Matter) AndAlso Not AllPreviousReviewersInTs.Contains(entry.Reviewer) AndAlso entry.Matter.ID = cbMatterPicker.SelectedItem.ID Then _
                    AllPreviousReviewersInTs.Add(entry.Reviewer)
            End If
        Next
        tbReviewer.AutoCompleteCustomSource = AllPreviousReviewersInTs

        Dim AllPreviousDescriptions As New AutoCompleteStringCollection
        tbDescription.AutoCompleteCustomSource.Clear()
        For Each entry As TimesheetEntry In Me.TimesheetEntries
            If String.IsNullOrEmpty(entry.Description) Then Continue For
            If IsNothing(cbMatterPicker.SelectedItem) AndAlso Not AllPreviousDescriptions.Contains(entry.Description) Then
                Dim KettosPontUtaniResz As String = entry.Description
                If entry.Description.Contains(":") Then
                    KettosPontUtaniResz = KettosPontUtaniResz.Substring(InStr(entry.Description, ":")).Trim
                End If
                AllPreviousDescriptions.Add(KettosPontUtaniResz)
            Else
                If Not IsNothing(entry.Matter) AndAlso entry.Matter.ID = cbMatterPicker.SelectedItem.ID AndAlso Not AllPreviousDescriptions.Contains(entry.Description) Then AllPreviousDescriptions.Add(entry.Description)
            End If
        Next
        AllPreviousDescriptions.Add("szerződéstervezet készítése")
        AllPreviousDescriptions.Add("szerződéstervezet véleményezése")
        AllPreviousDescriptions.Add("kiszervezési")
        tbDescription.AutoCompleteCustomSource = AllPreviousDescriptions
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
        EntryFormDeleteFields
        Label1.Text = "rögzítve az SP-ben: " & NewEntry.Description & ", " & cbPersons.Text
        'Törölni meglévő értéket és jelezni, hogy rögzítette!
    End Sub
    Private Sub EntryFormDeleteFields()
        tbDescription.Text = String.Empty
        cbMatterPicker.SelectedItem = Nothing
        cbTaskChooser.SelectedItem = Nothing
        tbReviewer.Text = String.Empty
        cbPersons.Text = String.Empty
        RealValue.Value = 0.25
        Chargeable.Value = 0
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
        tbReviewer.Text = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbReviewer.Text)
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

    Private Sub TimerForRefresh_Tick(sender As Object, e As EventArgs) Handles TimerForRefresh.Tick
        RefreshSpLookupValues()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        EntryFormDeleteFields()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnSpTs.Click
        Process.Start(SpTimesheetNotInvoicedView)
    End Sub

End Class
