﻿using BashSoft.Judge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BashSoft.IO
{
    class CommandInterpreter
    {
        public static void InterpredCommand(string input)
        {
            string[] data = input.Split(' ');

            string command = data[0];

            switch (command)
            {
                case "open":
                    TryOpenFile(input, data);
                    break;

                case "mkdir":
                    TryCreateDirectory(input, data);
                    break;

                case "ls":
                    TryTraverseFolders(input, data);
                    break;

                case "cmp":
                    TryCompareFiles(input, data);
                    break;

                case "cdRel":
                    TryChangePathRelatively(input, data);
                    break;

                case "cdAbs":
                    TryChangePathAbsolute(input, data);
                    break;

                case "readDb":
                    TryReadDatabaseFromFile(input, data);
                    break;

                case "help":
                    TryGetHelp(input, data);
                    break;

                case "filter":
                    TryFilterAndTake(input, data);
                    break;

                case "order"://decOrder
                              TryOrderAndTake(input, data);
                    break;

                case "download":
                    //TODO
                    break;

                case "downloadAsynch":
                    //TODO
                    break;

                case "show":
                    TryShowWantedData(input, data);
                    break;

                default:
                    DisplayInvalidCommandMessage(input);
                    break;
            }
        }

        private static void TryOrderAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                string coursename = data[1];
                string comparison = data[2].ToLower();
                string takeCommand = data[3].ToLower();
                string takeQuantity = data[4].ToLower();

                TryParseParametersForOrderAndTake(takeCommand, takeQuantity, coursename, comparison);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryParseParametersForOrderAndTake(string takeCommand, string takeQuantity, string coursename, string comparison)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    StudentsRepository.OrderAndTake(coursename, comparison);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = Int32.TryParse(takeQuantity, out studentsToTake);

                    if (hasParsed)
                    {
                        StudentsRepository.OrderAndTake(coursename, comparison, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);

                    }
                }
            }
        }
        private static void TryFilterAndTake(string input, string[] data)
        {
            if (data.Length == 5)
            {
                string coursename = data[1];
                string filter = data[2].ToLower();
                string takeCommand = data[3].ToLower();
                string takeQuantity = data[4].ToLower();

                TryParseParametersForFilterAndTake(takeCommand, takeQuantity, coursename, filter);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryParseParametersForFilterAndTake(string takeCommand, string takeQuantity, string coursename, string filter)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity=="all")
                {
                    StudentsRepository.FilerAndTake(coursename, filter);
                }
                else
                {
                    int studentsToTake;
                    bool hasParsed = Int32.TryParse(takeQuantity,out studentsToTake);

                    if (hasParsed)
                    {
                        StudentsRepository.FilerAndTake(coursename, filter, studentsToTake);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);

                    }
                }

            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeCommand);
            }
        }

        private static void TryShowWantedData(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string courseName = data[1];

                StudentsRepository.GetAllStudentsFromTheCourse(courseName);
            }
            else if (data.Length == 3)
            {
                string courseName = data[1];
                string userName = data[2];
                StudentsRepository.GetStudentScoresFromTheCourse(courseName, userName);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryGetHelp(string input, string[] data)
        {
            OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "make directory - mkdir: path "));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "traverse directory - ls: depth "));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "comparing files - cmp: path1 path2"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "change directory - cdRel:relative path"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "change directory - cdAbs:absolute path"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "read students data base - readDb: path"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "filter {courseName} excelent/average/poor  take 2/5/all students - filterExcelent (the output is written on the console)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "order increasing students - order {courseName} ascending/descending take 20/10/all (the output is written on the console)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "download file - download: path of file (saved in current directory)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "download file asinchronously - downloadAsynch: path of file (save in the current directory)"));
            OutputWriter.WriteMessageOnNewLine(string.Format("|{0, -98}|", "get help – help"));
            OutputWriter.WriteMessageOnNewLine($"{new string('_', 100)}");
            OutputWriter.WriteEmptyLine();
        }

        private static void TryReadDatabaseFromFile(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string fileName = data[1];
                StudentsRepository.InitializeData(fileName);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryChangePathAbsolute(string input, string[] data)
        {

            if (data.Length == 2)
            {
                string absolutePath = data[1];
                IOManager.ChangeCurrentDirectoryAbsolute(absolutePath);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryChangePathRelatively(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string relPath = data[1];
                IOManager.ChangeCurrentDirectoryRelative(relPath);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryCompareFiles(string input, string[] data)
        {
            if (data.Length == 3)
            {
                string firstPath = data[1];
                string secondPath = data[2];

                Tester.CompareContent(firstPath, secondPath);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryTraverseFolders(string input, string[] data)
        {
            if (data.Length == 1)
            {
                IOManager.TraverseDirectory(0);
            }
            else if (data.Length == 2)
            {
                int depth;
                bool hasParsed = int.TryParse(data[1], out depth);
                if (hasParsed)
                {
                    IOManager.TraverseDirectory(depth);
                }
                else
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
                }
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryCreateDirectory(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string folderName = data[1];

                IOManager.CreateDirectoryInCurrentFolder(folderName);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }

        private static void TryOpenFile(string input, string[] data)
        {
            if (data.Length == 2)
            {
                string fileName = data[1];

                Process.Start(SessionData.CurrentPath + "\\" + fileName);
            }
            else
            {
                DisplayInvalidCommandMessage(input);
            }
        }
        private static void DisplayInvalidCommandMessage(string input)
        {
            OutputWriter.DisplayException($"The command '{input}' is invalid");
        }
    }
}
