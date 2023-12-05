// <copyright file="AutomationExecutor.cs" company="EPAMS Systems">
// Copyright (c) EPAMS Systems. All rights reserved.
// </copyright>

namespace BinanceBotNuGetPackage.BotExecutor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    //using Newtonsoft.Json;

    /// <summary>
    /// Executor of UiPath processes.
    /// </summary>
    public class AutomationExecutor
    {
        /// <summary>
        /// Template for the command to run UiPath flow file.
        /// </summary>
        private const string _uipathProcessExecutionTemplate = @"/C uirobot.exe -file ""{0}\{1}""";

        /// <summary>
        /// Template for the command to run UiPath flow file.
        /// </summary>
        private const string _uipathProgramFilesFolder = @"C:\Program Files\UiPath\";

        /// <summary>
        /// Path to the UiPath execution file.
        /// </summary>
        private readonly string _workingDirectory;

        /// <summary>
        /// Path to the UiPath flow to be executed.
        /// </summary>
        private readonly string _uipathFlowFolderPath;
        
        

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationExecutor"/> class.
        /// </summary>
        /// <param name="flowFolder">Flow containing the targeting UiPath process.</param>
        public AutomationExecutor(string flowFolder)
        {
            string currentUserFolder = this.getCurrentUserFolder();
            string uipathAppdataFolder = Path.Combine(currentUserFolder, @"AppData\Local\Programs\UiPath\");
            if (Directory.Exists(uipathAppdataFolder))
            {
                this._workingDirectory = this.findUiPathExecutionFileFolder(uipathAppdataFolder);
            }
            else if (Directory.Exists(_uipathProgramFilesFolder))
            {
                this._workingDirectory = this.findUiPathExecutionFileFolder(_uipathProgramFilesFolder);
            }
            else
            {
                throw new Exception("Unable to find UiPath folder. Please check the folder correct location or possible UiPath is not installed");
            }

            this._uipathFlowFolderPath = this.findUiPathFlowPath(flowFolder);
        }
        
        public string ExecuteProcessWithNotParsedOutput(
            string workflowName,
            string cryptoName,
            string startDate,
            string endDate,
            string periodSelector,
            string connectionString)
        {
            Console.WriteLine($"Executing of the {workflowName} flow");
            Console.WriteLine();
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WorkingDirectory = this._workingDirectory,
                FileName = "cmd.exe",
                Arguments = string.Format(
                    _uipathProcessExecutionTemplate, 
                    _uipathFlowFolderPath,
                    workflowName
                    ) + @" --input {" +
                            "'in_Data':"             + "'" + cryptoName       + "'," +
                            "'in_StartDate':"        + "'" + startDate        + "'," +
                            "'in_EndDate':"          + "'" + endDate          + "'," +
                            "'in_PeriodSelector':"   + "'" + periodSelector   + "'," +
                            "'in_ConnectionString':" + "'" + connectionString + "'" +
                            "}"
            };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        #region Path Management

        private string getCurrentUserFolder()
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                return Directory.GetParent(path).ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string findUiPathExecutionFileFolder(string uipathAppdataFolder)
        {
            var uipathAppdataDirectoryInfo = new DirectoryInfo(uipathAppdataFolder);

            var directories = uipathAppdataDirectoryInfo.GetDirectories();
            foreach (var directory in directories)
            {
                var files = directory.GetFiles();

                foreach (var file in files)
                {
                    if (file.Name.Equals("UiRobot.exe"))
                    {
                        return file.DirectoryName;
                    }
                }
            }

            return string.Empty;
        }

        private string findUiPathFlowPath(string flowFolder)
        {
            var uipathFlowFolder = Directory.GetCurrentDirectory().Split("\\").Last();
            string currentDir = Directory.GetCurrentDirectory();
            string parentDir = new string(currentDir.ToCharArray());
            DirectoryInfo dirInfo;
            bool found = false;
            try
            {
                do
                {
                    parentDir = Directory.GetParent(parentDir).FullName;
                    dirInfo = new DirectoryInfo(parentDir);
                    found = dirInfo.GetDirectories()
                        .Select(info => info.Name)
                        .Contains(uipathFlowFolder);
                }
                while (!found);

                dirInfo = dirInfo.GetDirectories().First(dir => dir.Name == uipathFlowFolder);
                
                return dirInfo.FullName;
            }
            catch (NullReferenceException ex)
            {
                throw new DirectoryNotFoundException("Path hierarchy has been totally processed, which means that there is no parent directory in the end: target folder '" + uipathFlowFolder + "' was not found.", ex);
            }
        }
        #endregion
    }
}