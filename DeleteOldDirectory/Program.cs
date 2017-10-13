using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace DeleteOldDirectory
{
    static class Program
    {
        

        public static void Empty(this System.IO.DirectoryInfo directory, int days)
        {
            //delete file:  
            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                int d;
                if (file.ToString().StartsWith("@"))
                {
                    d = CheckWidthDays(file.ToString());
                    if (d > days) {
                        wpiszLog("Nie usunięto, pomimo przeterminowania: " + file.ToString());
                    }
                }
                else
                {
                    d = days;
                }

                if (file.CreationTime < DateTime.Now.AddDays(-d))
                {
                    try
                    {
                        file.Delete();
                        wpiszLog("Usunięto plik : " + file.ToString());
                    }
                    catch (Exception ex)
                    {
                        wpiszLog("Problem przy usuwaniu pliku: " + file.ToString() + " ||||| " + ex.ToString());
                    }
                }
            }

            //delete directory:
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                int d;
                if (subDirectory.ToString().StartsWith("@"))
                {
                    d = CheckWidthDays(subDirectory.ToString());
                    if (d > days)
                    {
                        wpiszLog("Nie usunięto, pomimo przeterminowania: " + subDirectory.ToString());
                    }
                }
                else
                {
                    d = days;
                   
                }

                if (subDirectory.CreationTime < DateTime.Now.AddDays(-d))
                {
                    try
                    {
                        subDirectory.Delete(true);
                        wpiszLog("Usunięto folder: " + subDirectory.ToString());
                    }
                    catch (Exception ex)
                    {
                        wpiszLog("Problem przy usuwaniu foldera: " + subDirectory.ToString() + " ||||| " + ex.ToString());
                    }
                }
            }
        }


        public static int CheckWidthDays(string nameFile)
        {
            string name = nameFile;
            Regex regex = new Regex(@"\d+");
            Match match = regex.Match(name);


            if (match.Length > 0)
            {
                int intOutWeek = int.Parse(match.ToString());
                int intOutDays = intOutWeek * 7;
                return intOutDays;
            }
            else
            {
                int weeks = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Weeks"]);
                int days = weeks * 7;
                return days;
            }
        }



        static void Main(string[] args)
        {
            wpiszLog("Uruchomiono program");

            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
            
            int weeks = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Weeks"]);
            int days = weeks * 7;

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(FilePath);
            Program.Empty(directory, days);
     
        }


        public static void wpiszLog(string log)
        {
            string tekstLoga;
            string logFilePatch = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];

            tekstLoga = log;
            try
            {
                string filename = DateTime.Now.ToString("yyyy-MM-dd") + "_LOG_DeleteOldDirectory.txt";
                File.AppendAllText(logFilePatch + filename, DateTime.Now.ToString() + " :::: " + tekstLoga + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
                throw;
            }


        }


    }
}
