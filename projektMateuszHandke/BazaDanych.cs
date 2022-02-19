using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace projektMateuszHandke
{
    class BazaDanych
    {
        public int punktyBazowe;
        public static string nick = " ", haslo = " ";
        public StringBuilder dane = new StringBuilder();
        public string nazwaSerwera = "";
        public string nazwaBazyDanych = "";
        public string ustawieniaPolaczenia = "";
        public string sqlQuery;
        public SqlConnection polaczenie = new SqlConnection();

        public BazaDanych(string nazwaSerwera, string nazwaBazyDanych, string ustawieniaPolaczenia)
        {
            this.nazwaSerwera = nazwaSerwera;
            this.nazwaBazyDanych = nazwaBazyDanych;
            this.ustawieniaPolaczenia = ustawieniaPolaczenia;
        }
        public BazaDanych()
        {

        }
        public void konfiguracja(int opcja)
        {
            polaczenie = new SqlConnection(ustawieniaPolaczenia);
            polaczenie.Open();

            Console.WriteLine("Podaj nick");
            nick = Console.ReadLine();
            Console.WriteLine("Podaj swoje hasło");
            haslo = Console.ReadLine();

            Console.WriteLine();

            if (opcja == 1)
            {
                while (czyNickIstnieje(nick, polaczenie) == 1)
                {
                    Console.WriteLine("Ten nick jest zajęty, podaj nowy.");
                    nick = Console.ReadLine();

                    Console.WriteLine("Podaj jeszcze raz swoje hasło");
                    haslo = Console.ReadLine();
                }

                dane.Append("Insert Into tabelkaGraczy (Nick,Haslo,Punkty) Values");
                dane.Append("(N'" + nick + "',N'" + haslo + "',N'0')");
                Console.WriteLine("Twój nick jest dostępny! Zarejestrowano Cię! Wyłącz i uruchom ponownie grę w celu aktywacji konta i następnie zaloguj się");
            }

            if (opcja == 2 || opcja == 3)
            {

                string select = "SELECT  *  FROM[Statki].[dbo].[tabelkaGraczy] WHERE Nick='" + nick + "' and Haslo='" + haslo + "'";
                SqlCommand komenda = new SqlCommand(select, polaczenie);
                SqlDataReader dataReader = komenda.ExecuteReader();
                string stanPunktow = "a";
                while (dataReader.Read())
                {
                    stanPunktow = dataReader.GetValue(2).ToString();
                    if (opcja == 2)
                    {
                        Console.WriteLine("Zostałeś zalogowany, twoja aktualna liczba punktów to: " + stanPunktow);
                    }
                    if (opcja == 3)
                    {
                        Console.WriteLine("Podaj nowe hasło");
                        string noweHaslo = Console.ReadLine();

                        Console.WriteLine();
                        dane.Append("Update tabelkaGraczy Set Haslo= N'" + noweHaslo + "' Where Nick='" + nick + "'and Haslo='" + haslo + "'");
                        Console.WriteLine("Hasło zmienione! Wyłącz i uruchom ponownie grę w celu zaktualizowania zmian na koncie i następnie zaloguj się");
                    }
                }
                dataReader.Close();
                punktyBazowe = Convert.ToInt32(stanPunktow);

                if (opcja == 2)
                {
                    new Gra(nazwaSerwera, nazwaBazyDanych, ustawieniaPolaczenia, punktyBazowe);
                }
            }           

            if (opcja == 1 || opcja == 3)
            {
                sqlQuery = dane.ToString();

                using (SqlCommand komenda = new SqlCommand(sqlQuery, polaczenie))
                {
                    komenda.ExecuteNonQuery();
                }
            }

            dane.Clear();
            Console.Read();
        }
        private int czyNickIstnieje(string nick, SqlConnection polaczenie)
        {
            int kontrolna = 0;
            string select = "SELECT  *  FROM[Statki].[dbo].[tabelkaGraczy]";
            SqlCommand komenda = new SqlCommand(select, polaczenie);
            SqlDataReader dataReader = komenda.ExecuteReader();

            while (dataReader.Read())
            {
                if (nick == dataReader.GetValue(0).ToString())
                {
                    kontrolna = 1;
                }
            }
            dataReader.Close();
            return kontrolna;
        }
    }
}
