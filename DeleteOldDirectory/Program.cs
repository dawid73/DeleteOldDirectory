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
            foreach (System.IO.FileInfo file in directory.GetFiles())
            {

                if (file.CreationTime < DateTime.Now.AddDays(-days))
                {
                    if (file.ToString().StartsWith("@"))
                    {

                        EmptyDelayed(directory, CheckWidthDays(file.ToString()));

                    }
                    else
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
            }
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                if (subDirectory.CreationTime < DateTime.Now.AddDays(-days))
                {
                    if (subDirectory.ToString().StartsWith("@"))
                    {

                        EmptyDelayed(directory, CheckWidthDays(subDirectory.ToString()));

                    }
                    else
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
        }

        public static void EmptyDelayed(this System.IO.DirectoryInfo directory, int days)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                if (file.CreationTime < DateTime.Now.AddDays(-days) && file.ToString().StartsWith("@"))
                {
                    try
                    {
                        file.Delete();
                        wpiszLog("Usunięto przełożony czasowo plik : " + file.ToString());
                    }
                    catch (Exception ex)
                    {
                        wpiszLog("Problem przy usuwaniu przełożonego czasowo pliku: " + file.ToString() + " ||||| " + ex.ToString());
                    }
                }
            }
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                if (subDirectory.CreationTime < DateTime.Now.AddDays(-days) && subDirectory.ToString().StartsWith("@"))
                {
                    try
                    {
                        subDirectory.Delete(true);
                        wpiszLog("Usunięto przełożony czasowo folder: " + subDirectory.ToString());
                    }
                    catch (Exception ex)
                    {
                        wpiszLog("Problem przy usuwaniu przełożonego czasowo foldera: " + subDirectory.ToString() + " ||||| " + ex.ToString());
                    }
                }
            }
        }

        public static int CheckWidthDays(string nameFile)
        {
            string name = nameFile;

            //Match match = Regex.Match(name, @"(@T[0-9]{1})@|@T([0-9]{2})@");

            //Console.WriteLine(match.ToString());
            //Console.ReadLine();

            //string stringmatch = match.Value.ToString().Replace("@", "").Replace("T", "");
            //if (stringmatch.Length > 0) {
            //    int intOutWeek = int.Parse(stringmatch);
            //    int intOutDays = intOutWeek * 7;
            //    return intOutDays;
            //}
            //else
            //{
            //    int weeks = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Weeks"]);
            //    int days = weeks * 7;
            //    return days;
            //}

            char[] tab = name.ToCharArray();
            Console.WriteLine(tab[0]);
            Console.WriteLine(tab[1]);
            Console.WriteLine(tab[2]);
            Console.WriteLine(tab[3]);
            Console.WriteLine(tab[4]);
            Console.WriteLine(tab[5]);
            Console.WriteLine(tab[6]);
            Console.ReadLine();

            if ("@" == tab[3].ToString())
            {
                Console.WriteLine("jest małpa na tab[3]");
                Console.ReadLine();

                int a = int.Parse(tab[2].ToString());
                return a * 7;
            }
            else if ("@" == tab[4].ToString())
            {
                Console.WriteLine("jest małpa na tab[4]");
                Console.ReadLine();
                int a = int.Parse(tab[2].ToString() + tab[3].ToString());
                return a * 7;
            }
            else
            {
                Console.WriteLine("nie ma małpy");
                Console.ReadLine();
                return 14;
            }





        }



        static void Main(string[] args)
        {
            wpiszLog("Uruchomiono program");

            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
            Console.WriteLine(FilePath);

            int weeks = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Weeks"]);
            int days = weeks * 7;

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(FilePath);
            Program.Empty(directory, days);



            //Console.ReadLine();

        }

        public static void wpiszLog(string log)
        {
            string tekstLoga;
            string logFilePatch = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];

            tekstLoga = log;
            try
            {
                File.AppendAllText(logFilePatch + "log.txt", DateTime.Now.ToString() + " :::: " + tekstLoga + Environment.NewLine);

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
