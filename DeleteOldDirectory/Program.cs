using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories())
            {
                if (subDirectory.CreationTime < DateTime.Now.AddDays(-days))
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

        static void Main(string[] args)
        {
            wpiszLog("Uruchomiono program");

            string FilePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
            Console.WriteLine(FilePath);

            int weeks = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["Weeks"]);
            int days = weeks * 7; 

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(FilePath);
            Program.Empty(directory, days);

            //Zatrzymanie programu
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
