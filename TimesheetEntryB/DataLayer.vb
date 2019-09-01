Imports System.IO
Imports Microsoft.SharePoint.Client
Imports Newtonsoft.Json
Imports SPHelper.SPHUI

Public Class DataLayer

    Private CacheDirectory = Path.Combine(Path.GetTempPath, My.Application.Info.AssemblyName, "Cache")
    Private CTimesheetPath = Path.Combine(CacheDirectory, "Timesheet.json")
    'kikapcsoláskor is serialize-oljon, ha szinkronban tartjuk a TS-sel?
    Private CMattersPath = Path.Combine(CacheDirectory, "Matters.json")
    Private CPersonsPath = Path.Combine(CacheDirectory, "Persons.json")
    Private CTasksPath = Path.Combine(CacheDirectory, "Tasks.json")

    Public Property AllMatters As List(Of MatterClass)
    Public Property AllPersons As List(Of PersonClass) 'Name, matterdata
    Public Property AllUsers As List(Of UserId)
    Public Property AllTsheetEntries As List(Of TimesheetEntry)
    Public Property AllTasks As List(Of SPHelper.SPHUI.TaskClass)

    Public Function DeserializeFromFiles() As Integer
        Dim success As Byte = 0
        If IO.File.Exists(CTimesheetPath) Then
            Me.AllTsheetEntries = DeserializeTsheetEntries(CTimesheetPath)
            If Me.AllTsheetEntries.Count > 0 Then success += 1
        End If
        If IO.File.Exists(CMattersPath) Then
            Me.AllMatters = SPHelper.SPHUI.DeserializeMatterClass(CMattersPath)
            If Me.AllMatters.Count > 0 Then success += 1
        End If
        If IO.File.Exists(CPersonsPath) Then
            Me.AllPersons = SPHelper.SPHUI.DeserializePersonClass(CPersonsPath)
            If Me.AllPersons.Count > 0 Then success += 1
        End If
        If IO.File.Exists(CTasksPath) Then
            Me.AllTasks = SPHelper.SPHUI.DeserializeTaskClass(CTasksPath)
        End If
        HPHelper.DebugTesting.SetBasicDebuggerandTrace()
        Debug.WriteLine("Items data deserialized from file to memory - " & success)
        Return success
    End Function
    Private Function DeserializeTsheetEntries(fileName As String) As List(Of TimesheetEntry)
        Dim inputString As String = String.Empty
        Using Reader As New StreamReader(fileName)
            inputString = Reader.ReadLine()
        End Using
        Return JsonConvert.DeserializeObject(Of List(Of TimesheetEntry))(inputString)
    End Function
    Private Function SerializeToFiles() As Integer
        If Not Directory.Exists(CacheDirectory) Then
            Directory.CreateDirectory(CacheDirectory)
            Debug.WriteLine("Cache directory created at " & CacheDirectory)
        End If
        Dim success As Byte = 0
        For Each obj As Object In {Me.AllMatters, Me.AllMatters, Me.AllTasks, Me.AllTsheetEntries}
            If Not IsNothing(obj) Then
                Dim succeeded = HPHelper.Serialize.Serialize(CMattersPath, obj)
                If succeeded Then success += 1
            End If
        Next
        'If Not IsNothing(Me.AllMatters) Then
        '    Dim succeeded = HPHelper.Serialize.Serialize(CMattersPath, Me.AllMatters)
        '    If succeeded Then success += 1
        'End If
        'If Not IsNothing(Me.AllPersons) Then
        '    Dim succeeded = HPHelper.Serialize.Serialize(CPersonsPath, Me.AllPersons)
        '    If succeeded Then success += 1
        'End If
        'If Not IsNothing(Me.AllTasks) Then
        '    Dim succeeded = HPHelper.Serialize.Serialize(CPersonsPath, Me.AllTasks)
        '    If succeeded Then success += 1
        'End If
        'If Not IsNothing(Me.AllTsheetEntries) Then
        '    Dim succeeded = HPHelper.Serialize.Serialize(CPersonsPath, Me.AllTsheetEntries)
        '    If succeeded Then success += 1
        'End If
        Debug.WriteLine("Matters, persons, tasks and timesheet deserialized from file to memory: " & success)
        Return success
    End Function
    Public Async Function GetAllUsers(context As ClientContext) As Task(Of List(Of UserId))
        Dim mySP As New SPHelper.SPHUI
        Me.AllUsers = Await mySP.GetAllUsersAsync(context, True, "Intranet HUI Members")
        Return Me.AllUsers
    End Function
    Public Async Function GetAllMattersAsync(context As ClientContext) As Task(Of List(Of MatterClass))
        Dim mySP As New SPHelper.SPHUI
        Me.AllMatters = Await mySP.GetAllMattersAsync(context, Me.AllUsers)
        Return Me.AllMatters
    End Function
    Public Async Function GetAllPersonsAsync(context As ClientContext) As Task(Of List(Of PersonClass))
        Dim mySP As New SPHelper.SPHUI
        Me.AllPersons = Await mySP.GetAllPersonsAsyncTimesheet(context, Me.AllMatters)
        Return Me.AllPersons
    End Function
    Public Async Function GetAllTasksAsync(context As ClientContext) As Task(Of List(Of TaskClass))
        Dim mySP As New SPHelper.SPHUI
        Me.AllTasks = Await mySP.GetAllTasksForTsheet(context, Me.AllUsers, Me.AllMatters, Me.AllPersons)
        Return Me.AllTasks
    End Function
    Public Async Function GetAllTsheetAsync(context As ClientContext) As Task(Of List(Of TimesheetEntry))
        Dim mySP As New SPHelper.SPHUI
        Me.AllTsheetEntries = Await mySP.GetAllTimesheetEntries(context, Me.AllUsers, Me.AllMatters, Me.AllTasks)
        Return Me.AllTsheetEntries
    End Function

End Class
