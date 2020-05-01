Option Infer On

Imports System
Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.VisualBasic.FileIO

Module Program

    'Path to openmw.cfg
    Public cfgPath As String
    'Path to Morrowind.ini
    Public iniPath As String
    'Path to config for this program (BBCAutoPatcher.cfg) which stores the cfgPath.
    Public localCfgPath As String
    'Stores name of executable
    Public fileTitle As String

    Sub Main(args As String())
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
        Console.WriteLine("Load Order Converter by SilentNightxxx and johnnyhostile")
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")

        fileTitle = Process.GetCurrentProcess().MainModule.ModuleName
        Directory.SetCurrentDirectory(Process.GetCurrentProcess().MainModule.FileName.Replace(fileTitle, ""))

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            localCfgPath = FileSystem.CurrentDirectory + "\LoadOrderConverter.cfg"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            localCfgPath = FileSystem.CurrentDirectory + "/LoadOrderConverter.cfg"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.OSX) Then
            localCfgPath = FileSystem.CurrentDirectory + "/LoadOrderConverter.cfg"
        End If

        If System.IO.File.Exists(localCfgPath) Then
            cfgPath = FileSystem.ReadAllText(localCfgPath)
        Else
            FirstRun()
        End If

        ConvertLoadOrder()

    End Sub

    Sub ConvertLoadOrder()

        Console.WriteLine("Converting Morrowind to OpenMW...")

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            iniPath = FileSystem.CurrentDirectory + "\Morrowind.ini"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            iniPath = FileSystem.CurrentDirectory + "/Morrowind.ini"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.OSX) Then
            iniPath = FileSystem.CurrentDirectory + "/Morrowind.ini"
        End If

        If System.IO.File.Exists(iniPath) And System.IO.File.Exists(cfgPath) Then

            If File.ReadAllText(iniPath).Length = 0 Then

                Console.WriteLine("Error, Morrowind.ini is empty. Please run the game once.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)

            ElseIf File.ReadAllText(cfgPath).Length = 0 Then

                Console.WriteLine("Error, openmw.cfg is empty. Please run OpenMW once.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)


            Else

                Dim newLines = From line In File.ReadAllLines(cfgPath) Where Not line.StartsWith("content=")
                File.WriteAllLines(cfgPath, newLines)

                Dim lines = File.ReadAllLines(iniPath)
                For Each line In lines
                    If line.StartsWith("GameFile") Then
                        Dim splitLine As String() = line.Split("=")
                        Dim formattedLine = "content=" + splitLine(1)

                        Using sw As New StreamWriter(File.Open(cfgPath, FileMode.Append))
                            sw.WriteLine(formattedLine)
                        End Using

                    End If
                Next

                Console.WriteLine("Load order converted.")

            End If

        ElseIf System.IO.File.Exists(cfgPath) Then

            Console.WriteLine("Error, Morrowind.ini doesn't exist. Please run the game once and make sure you placed this program in the")
            Console.WriteLine("game directory.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey(True)
            Environment.Exit(-1)

        ElseIf System.IO.File.Exists(iniPath) Then

            Console.WriteLine("Error, saved openmw.cfg doesn't exist. Please delete LoadOrderConverter.cfg and rerun to reconfigure the program.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey(True)
            Environment.Exit(-1)

        Else

            Console.WriteLine("Error, Morrowind.ini and openmw.cfg don't exist. Please run the game once and make sure you placed this program")
            Console.WriteLine("in the game directory. After that, delete LoadOrderConverter.cfg and rerun to reconfigure the program.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey(True)
            Environment.Exit(-1)

        End If

    End Sub

    Sub FirstRun()

        Console.WriteLine("Preforming initial setup...")

        If RuntimeInformation.IsOSPlatform(OSPlatform.Windows) Then
            cfgPath = SpecialDirectories.MyDocuments + "\My Games\OpenMW\openmw.cfg"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.Linux) Then
            cfgPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/openmw/openmw.cfg"
        End If

        If RuntimeInformation.IsOSPlatform(OSPlatform.OSX) Then
            cfgPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/Library/Preferences/openmw/openmw.cfg"
        End If

        If System.IO.File.Exists(cfgPath) Then
            Console.WriteLine("OpenMW configuration file detected.")
            File.Create(localCfgPath).Dispose()
            FileSystem.WriteAllText(localCfgPath, cfgPath, True)
        Else
            Console.WriteLine("Enter the path to your openmw.cfg file:")
            cfgPath = Console.ReadLine()
            If System.IO.File.Exists(cfgPath) Then
                Console.WriteLine("OpenMW configuration file detected.")
                File.Create(localCfgPath).Dispose()
                FileSystem.WriteAllText(localCfgPath, cfgPath, True)
            Else
                Console.WriteLine("Error, no openmw.cfg detected.")
                Console.WriteLine("Press any key to continue...")
                Console.ReadKey(True)
                Environment.Exit(-1)
            End If
        End If

        Console.WriteLine("Configuration finished, from now on you can convert your load order from Morrowind to OpenMW by running")
        Console.WriteLine("ConvertToOMW and you can convert from OpenMW to Morrowind by running ConvertToMW. If you have any issues you can")
        Console.WriteLine("go through configuration again by deleting LoadOrderConverter.cfg and rerunning one of the two programs.")
        Console.WriteLine("-------------------------------------------------------------------------------------------------------------------")
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey(True)
        Environment.Exit(-1)

    End Sub

End Module

