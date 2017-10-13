// autor: Dawid Honkisz


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
            //usuwanie pliku :  

            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                // zmienna pomocnicza. Jeżeli plik zaczyna się od @
                // wówczas w zmienna d wspiuje się wartość znajdująca się
                // w nazwie pliku. jeżeli nie ma w nazwie pliku @ wówczas
                // w wartość d wspiuje sie wartość z app.conf
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

                //sprawdzenie czy dany plik jest 
                //utworzny wcześniej niż aktualna data - wartość d
                if (file.CreationTime < DateTime.Now.AddDays(-d))
                {
                    try
                    {   //usunięcie pliku
                        file.Delete();
                        wpiszLog("Usunięto plik : " + file.ToString());
                    }
                    catch (Exception ex)
                    {
                        wpiszLog("Problem przy usuwaniu pliku: " + file.ToString() + " ||||| " + ex.ToString());
                    }
                }
            }

            //usuwanie katalogu:
            //zasada działania taka sama jak w przypadku 
            //pliku, czyli pętli powyżej ^^
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

        // funkcja przyjmuje nazwę pliku,
        // i dzięki fukncji Regex wyciąga
        // z nazwy pierwsze wystąpienie cyfy
        // nie ma znaczenia z ilu cyfer skłąda się liczba.
        // wyciągane jest pierwsza liczba w nazwie pliku, 
        // czyli w tym przypadku piersza liczba po '@'
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
            
            // w app.config podawana jest wartość
            // tygodni, ponieważ nie da się odjąć
            // od daty tygodni, wartość 'week' 
            // pomnożamy przez 7
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
