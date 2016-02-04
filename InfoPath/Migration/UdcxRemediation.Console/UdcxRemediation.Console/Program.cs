﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace UdcxRemediation.Console
{
    public static class Program
    {
        /// <summary>
        /// Gets or sets the domain provided by user
        /// </summary>
        ///
        public static string AdminDomain
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the username provided by user
        /// </summary>
        public static string AdminUsername
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets the password provided by user
        /// </summary>
        public static SecureString AdminPassword
        {
            get;
            set;
        }
        public static void Main(string[] args)
        {
            GetCredentials();
            string inputFilePath = GetInputFilePath();
            CommentUDCXFileNodes.DoWork(inputFilePath);

            System.Console.WriteLine("Completed");
            System.Console.ReadLine();
        }

        private static string GetInputFilePath()
        {
            bool retryFilePathInput;
            string inputFilePath;

            do
            {
                retryFilePathInput = false;
                System.Console.WriteLine("\n\n" + @"Please enter the Path conataining the UDCX Report: ");
                System.Console.WriteLine(@"- Give the path in the following format [Folder path containing the UDCX Report]\[CSV File Name]");
                System.Console.WriteLine(@"- Example: E:\UdcxReport\Report.csv" + "\n");

                inputFilePath = System.Console.ReadLine();

                if (inputFilePath == "")
                {
                    retryFilePathInput = true;
                    System.Console.WriteLine("\n" + @"Please make sure the File Path is not empty" + "\n");
                }
                else
                {
                    if (!File.Exists(inputFilePath))
                    {
                        retryFilePathInput = true;
                        System.Console.WriteLine("\n" + @"Please make sure the File Path is in the valid format" + "\n");
                    }
                }
            }
            while (retryFilePathInput);

            return inputFilePath;
        }

        private static void GetCredentials()
        {
            ConsoleKeyInfo key;
            bool retryUserNameInput = false;
            string account = String.Empty;
            string password = String.Empty;

            do
            {
                System.Console.WriteLine(@"Please enter the Admin account: ");
                System.Console.WriteLine(@"- Use [domain\alias] for SPO-D & On-Prem farms");
                System.Console.WriteLine(@"- Use [alias@domain.com] for SPO-MT & vNext farms");

                account = System.Console.ReadLine();

                if (account.Contains('\\'))
                {
                    string[] segments = account.Split('\\');
                    AdminDomain = segments[0];
                    AdminUsername = segments[1];
                    break;
                }
                if (account.Contains("@"))
                {
                    AdminUsername = account;
                    break;
                }
            }
            while (retryUserNameInput);

            System.Console.WriteLine("Please enter the Admin password: ");

            do
            {
                key = System.Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace)
                {
                    password += key.KeyChar;
                    System.Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        System.Console.CursorLeft--;
                        System.Console.Write('\0');
                        System.Console.CursorLeft--;
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);

            AdminPassword = Helper.CreateSecureString(password.TrimEnd('\r'));
        }
    }
}
