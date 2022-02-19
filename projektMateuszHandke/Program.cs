using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security;

namespace projektMateuszHandke
{        
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Witaj w grze Statki!");
            Console.WriteLine("Wybierz co chcesz zrobić i wciśnij enter.");
            Console.WriteLine("1 - zarejestruj się");
            Console.WriteLine("2 - zaloguj się");
            Console.WriteLine("3 - zmień hasło");
            Console.WriteLine("4 - graj bez logowania");
            Console.WriteLine();
            Console.WriteLine("Podaj opcję:");
            int wybor=0;
            opcja();

            void opcja()
            {
                try
                {
                    wybor = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();

                    if (wybor == 1 || wybor == 2 || wybor == 3)
                    {
                        string nazwaSerwera = @"DESKTOP-5D7M6PO\SQLEXPRESS"; //tu zamieniamy nazwę serwera
                        string nazwaBazyDanych = "Statki";
                        string ustawieniaPolaczenia = @"Data Source=" + nazwaSerwera + ";Initial Catalog=" + nazwaBazyDanych + ";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                        BazaDanych baza = new BazaDanych(nazwaSerwera, nazwaBazyDanych, ustawieniaPolaczenia);
                        baza.konfiguracja(wybor);
                    }
                    if (wybor == 4)
                    {
                        new Gra();
                    }
                    else if (wybor < 1 || wybor > 4)
                    {
                        Console.WriteLine("Błędnie wprowadzone dane. Podaj ponownie opcję:");
                        opcja();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine();
                    Console.WriteLine("Błędnie wprowadzone dane. Podaj ponownie opcję:");
                    opcja();
                }
            }
        }
    }
}
