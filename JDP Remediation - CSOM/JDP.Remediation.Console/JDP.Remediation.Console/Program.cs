﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JDP.Remediation.Console
{
    public static class Program
    {
        /// <summary>
        /// Gets or sets the domain provided by user
        /// </summary>
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

        private static void ShowUsage()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Please select an operation to execute:");

            System.Console.WriteLine("1 - Generate Site Collection Report (PPE-Only)");
            System.Console.WriteLine("2 - Generate Non-Default Master Page Usage Report");
            System.Console.WriteLine("3 - Generate Site Column & Content Type Usage Report");
            System.Console.WriteLine("4 - Delete Missing Setup Files");
            System.Console.WriteLine("5 - Delete Missing Features");
            System.Console.WriteLine("6 - Delete Missing Event Receivers");

            System.Console.WriteLine("Q - Quit");
            System.Console.WriteLine();
        }

        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args">Event Arguments</param>
        public static void Main(string[] args)
        {
            string input = String.Empty;

            GetCredentials();

            do
            {
                ShowUsage();

                input = System.Console.ReadLine();
                switch (input.ToUpper(System.Globalization.CultureInfo.CurrentCulture))
                {
                    case "1":
                        System.Console.WriteLine("This operation is intended for use only in PPE; use on PROD at your own risk.");
                        System.Console.WriteLine("For PROD, it is safer to generate the report via the o365 Self-Service Admin Portal.");
                        System.Console.WriteLine("Press \"y\" only if you wish to continue.  Press any other key to abort this operation.");

                        input = System.Console.ReadLine();
                        if (input.ToUpper(System.Globalization.CultureInfo.CurrentCulture) != "Y")
                        {
                            System.Console.WriteLine("Operation aborted by user.");
                            break;
                        }
                        GenerateSiteCollectionReport.DoWork();
                        break;
                    case "2":
                        GenerateNonDefaultMasterPageUsageReport.DoWork();
                        break;
                    case "3":
                        GenerateColumnAndTypeUsageReport.DoWork();
                        break;
                    case "4":
                        DeleteMissingSetupFiles.DoWork();
                        break;
                    case "5":
                        DeleteMissingFeatures.DoWork();
                        break;
                    case "6":
                        DeleteMissingEventReceivers.DoWork();
                        break;

                    case "Q":
                        break;

                    default:
                        break;
                }
            }
            while (input.ToUpper(System.Globalization.CultureInfo.CurrentCulture) != "Q");
        }

        /// <summary>
        /// get credentials
        /// </summary>
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
                    string [] segments = account.Split('\\');
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
