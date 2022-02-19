using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;


namespace projektMateuszHandke
{
    class Gra : BazaDanych
    {
        string[] planszaPokazowa = new string[100]; 
        string[] wylosowanaPlansza = new string[100]; 
        string[] planszaGracza = new string[100]; 
        int[] tabStatek5 = new int[6]; 
        int[] tabStatek4 = new int[5]; 
        int[] tabStatek3 = new int[4]; 
        int[] tabStatek2 = new int[3]; 
        int[] tabStatek1 = new int[1]; 
        int punktyRazem;

        public Gra(string nazwaSerwera, string nazwaBazyDanych, string ustawieniaPolaczenia, int punktyBazowe)
            : base(nazwaSerwera, nazwaBazyDanych, ustawieniaPolaczenia)
        {
            this.punktyBazowe = punktyBazowe;
            instrukcja();
            tworzeniePlansz();
            graStart();
            zapis(nick,haslo,punktyBazowe);
        }
        public Gra()
        {
            instrukcja();
            tworzeniePlansz();
            graStart();
        }
        public void instrukcja()
        {
            planszaPokazowa[0] = " ";
            wylosowanaPlansza[0] = " ";
            planszaGracza[0] = " ";
            for (int i = 1; i <= 99; i++)
            {
                if (i <= 9)
                {
                    planszaPokazowa[i] = Convert.ToString(i);
                    wylosowanaPlansza[i] = Convert.ToString(i);
                    planszaGracza[i] = Convert.ToString(i);
                }
                else if (i % 10 == 0)
                {
                    planszaPokazowa[i] = Convert.ToString(i / 10);
                    wylosowanaPlansza[i] = Convert.ToString(i / 10);
                    planszaGracza[i] = Convert.ToString(i / 10);
                }
                else
                {
                    planszaPokazowa[i] = " ";
                    wylosowanaPlansza[i] = " ";
                    planszaGracza[i] = " ";
                }
            }

            obszarStatku(23, 3, 1, planszaPokazowa);
            obszarStatku(58, 4, 2, planszaPokazowa);
            Console.WriteLine();
            Console.WriteLine("Witaj raz jeszcze w grze Statki!");
            Console.WriteLine("Twoim zadaniem jest znalezienie na planszy 5 statków, każdy po 1 o długościach 1,2,3,4 i 5.");
            Console.WriteLine("Kiedy trafisz w statek odkryjesz znak '+', za który od razu dostaniesz 1 punkt. Symbol ':' oznacza pudło, wówczas nie dostaniesz punktu.");
            Console.WriteLine("Za zatopienie statku otrzymasz dodatkowo liczbę punktów odpowiadajacą długości zatopionego statku.");
            Console.WriteLine("Statki mogą być ułożone poziomo lub pionowo, tak jak na przykładowej planszy poniżej.");
            Console.WriteLine("Ich współrzędne to kolejno nr wiersza i kolumny. Przykładowo współrzędne statku o długości 3 na przykładowej planszy to 23, 24 i 25.");
            Console.WriteLine("Masz 30 prób, jednak gdy trafisz '+' lub wpiszesz zły punkt, próba nie jest Ci zabierana. Za niewykorzystane próby otrzymasz dodatkowe punkty.");
            Console.WriteLine("Powodzenia!");
            Console.WriteLine();

            for (int i = 0; i <= 99; i++)
            {
                if ((i > 0) && (i % 10 == 0))
                {
                    Console.WriteLine();
                }
                Console.Write("  " + planszaPokazowa[i]);
            }
            Console.WriteLine();
        }

        public void tworzeniePlansz()
        {
            for (int dlugoscStatku = 5; dlugoscStatku >= 1; dlugoscStatku--)
            {
                Random x = new Random();
                int kierunek = x.Next(1, 3); 
                int pozycjaPoczatkowaStatku = x.Next(11, 100);

                if (sprawdzanie(pozycjaPoczatkowaStatku, kierunek, dlugoscStatku) == 0)
                {

                    while (sprawdzanie(pozycjaPoczatkowaStatku, kierunek, dlugoscStatku) == 0)
                    {
                        pozycjaPoczatkowaStatku = x.Next(11, 100);
                    }
                }
                obszarStatku(pozycjaPoczatkowaStatku, dlugoscStatku, kierunek, wylosowanaPlansza);
            }

            for (int i = 0; i <= 99; i++)
            {
                if (i != 0 && wylosowanaPlansza[i] == " ")
                {
                    wylosowanaPlansza[i] = ":";
                }
            }

            for (int i = 0; i <= 99; i++)
            {
                if ((i > 0) && (i % 10 == 0))
                {
                    Console.WriteLine();
                }
                Console.Write("  " + planszaGracza[i]);
            }
        }
        public void graStart()
        {
            int punkty = 0;
            int iloscProb = 30;
            int kontrolna1 = 0;
            int kontrolna2 = 0;
            int kontrolna3 = 0;
            int kontrolna4 = 0;
            int kontrolna5 = 0;
            Console.WriteLine();

            for (int i = 1; i <= iloscProb; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Podaj położenie: ");

                string strzal1 = Console.ReadLine();

                if (strzal1.Length != 2)
                {
                    Console.WriteLine("Zły punkt.");
                    i--;
                }
                else
                {
                    int znak1 = (int)strzal1[0];
                    int znak2 = (int)strzal1[1];

                    if (znak1 < 49 || znak1 > 57 || znak2 < 48 || znak2 > 57)
                    {
                        Console.WriteLine("Zły punkt.");
                        i--;
                    }
                    else
                    {
                        int strzal = Convert.ToInt32(strzal1);

                        if (strzal < 10 || strzal % 10 == 0 || strzal > 100)
                        {
                            Console.WriteLine("Zły punkt.");
                            i--;
                        }
                        else
                        {
                            if (planszaGracza[strzal] == " ")
                            {
                                planszaGracza[strzal] = wylosowanaPlansza[strzal];
                                Console.WriteLine();
                                if (wylosowanaPlansza[strzal] == "+")
                                {
                                    punkty++;

                                    Console.WriteLine("Trafiony!");

                                    if (strzal == tabStatek1[0])
                                    {
                                        kontrolna1++;
                                        obszarStatku(tabStatek1[0], 1, 1, planszaGracza);
                                        Console.WriteLine("Zatopiony!");
                                        punkty = punkty + 1;
                                    }

                                    if (strzal == tabStatek2[0] || strzal == tabStatek2[1])
                                    {
                                        kontrolna2++;
                                        if (kontrolna2 == 2)
                                        {
                                            obszarStatku(tabStatek2[0], 2, tabStatek2[2], planszaGracza);
                                            Console.WriteLine("Zatopiony!");
                                            punkty = punkty + 2;
                                        }
                                    }

                                    if (strzal == tabStatek3[0] || strzal == tabStatek3[1] || strzal == tabStatek3[2])
                                    {
                                        kontrolna3++;
                                        if (kontrolna3 == 3)
                                        {
                                            obszarStatku(tabStatek3[0], 3, tabStatek3[3], planszaGracza);
                                            Console.WriteLine("Zatopiony!");
                                            punkty = punkty + 3;
                                        }
                                    }

                                    if (strzal == tabStatek4[0] || strzal == tabStatek4[1] || strzal == tabStatek4[2] || strzal == tabStatek4[3])
                                    {
                                        kontrolna4++;
                                        if (kontrolna4 == 4)
                                        {
                                            obszarStatku(tabStatek4[0], 4, tabStatek4[4], planszaGracza);
                                            Console.WriteLine("Zatopiony!");
                                            punkty = punkty + 4;
                                        }
                                    }

                                    if (strzal == tabStatek5[0] || strzal == tabStatek5[1] || strzal == tabStatek5[2] || strzal == tabStatek5[3] || strzal == tabStatek5[4])
                                    {
                                        kontrolna5++;
                                        if (kontrolna5 == 5)
                                        {
                                            obszarStatku(tabStatek5[0], 5, tabStatek5[5], planszaGracza);
                                            Console.WriteLine("Zatopiony!");
                                            punkty = punkty + 5;
                                        }
                                    }

                                    if (kontrolna5 == 5 && kontrolna4 == 4 && kontrolna3 == 3 && kontrolna2 == 2 && kontrolna1 == 1)
                                    {
                                        for (int j = 0; j <= 99; j++)
                                        {
                                            if ((j > 0) && (j % 10 == 0))
                                            {
                                                Console.WriteLine();
                                            }
                                            Console.Write("  " + planszaGracza[j]);
                                        }
                                        Console.WriteLine();
                                        Console.WriteLine();
                                        Console.WriteLine("Gratulacje! Zatopiłeś wszystkie statki!");
                                        punktyRazem = punkty + iloscProb - i + 1;
                                        Console.WriteLine("Twoja ostateczna liczba punktów: {0}", punktyRazem);
                                        
                                        break;
                                    }

                                    i--;
                                }

                                else
                                {
                                    Console.WriteLine("Pudło!");
                                }

                                Console.WriteLine("Ilość punktów: {0}", punkty);
                                Console.WriteLine("Pozostało prób: {0}", iloscProb - i);
                                Console.WriteLine();

                                for (int j = 0; j <= 99; j++)
                                {
                                    if ((j > 0) && (j % 10 == 0))
                                    {
                                        Console.WriteLine();
                                    }
                                    Console.Write("  " + planszaGracza[j]);
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Ten punkt już odkryto, podaj inny");
                                i--;
                            }
                        }
                    }
                }
                if (i == iloscProb)
                {
                    punktyRazem = punkty;
                    Console.WriteLine();
                    Console.WriteLine("Niestety nie udało się zbić wszystkich statków, spróbuj kolejnym razem!");
                    Console.WriteLine("Twoje ugrane punkty wynoszą: {0}", punkty);
                    
                    break;                    
                }
            }
            Console.WriteLine();
            Console.ReadKey();
        }
        private void obszarStatku(int pozycjaPoczatkowaStatku, int dlugoscStatku, int kierunek, string[] nrTablicy)
        {

            if (kierunek == 1)
            {
                if ((pozycjaPoczatkowaStatku - 1) > 10 && (pozycjaPoczatkowaStatku - 1) % 10 != 0)
                {
                    nrTablicy[pozycjaPoczatkowaStatku - 1] = ":";
                }
                if ((pozycjaPoczatkowaStatku + dlugoscStatku) < 100 && (pozycjaPoczatkowaStatku + dlugoscStatku) % 10 != 0)
                {
                    nrTablicy[pozycjaPoczatkowaStatku + dlugoscStatku] = ":";
                }
                if ((pozycjaPoczatkowaStatku + 9) < 100 && (pozycjaPoczatkowaStatku + 9) % 10 != 0)
                {
                    nrTablicy[pozycjaPoczatkowaStatku + 9] = ":";
                }
                if ((pozycjaPoczatkowaStatku - 11) > 10 && (pozycjaPoczatkowaStatku - 11) % 10 != 0)
                {
                    nrTablicy[pozycjaPoczatkowaStatku - 11] = ":";
                }
                if ((pozycjaPoczatkowaStatku + dlugoscStatku + 10) < 100 && (pozycjaPoczatkowaStatku + dlugoscStatku + 10) % 10 != 0)
                {
                    nrTablicy[pozycjaPoczatkowaStatku + dlugoscStatku + 10] = ":";
                }
                if ((pozycjaPoczatkowaStatku - 10 + dlugoscStatku) > 10 && (pozycjaPoczatkowaStatku - 10 + dlugoscStatku) % 10 != 0)
                {
                    nrTablicy[pozycjaPoczatkowaStatku - 10 + dlugoscStatku] = ":";
                }

                int j = 0;

                for (int i = pozycjaPoczatkowaStatku; i < pozycjaPoczatkowaStatku + dlugoscStatku; i++)
                {
                    nrTablicy[i] = "+";

                    if (dlugoscStatku == 5)
                    {
                        tabStatek5[j] = i;
                        tabStatek5[5] = 1;
                    }
                    if (dlugoscStatku == 4)
                    {
                        tabStatek4[j] = i;
                        tabStatek4[4] = 1;
                    }
                    if (dlugoscStatku == 3)
                    {
                        tabStatek3[j] = i;
                        tabStatek3[3] = 1;
                    }
                    if (dlugoscStatku == 2)
                    {
                        tabStatek2[j] = i;
                        tabStatek2[2] = 1;
                    }
                    if (dlugoscStatku == 1)
                    {
                        tabStatek1[j] = i;
                    }
                    j++;

                    if (i > 20)
                    {
                        nrTablicy[i - 10] = ":";
                    }
                    if (i < 90 && i % 10 != 0)
                    {
                        nrTablicy[i + 10] = ":";
                    }
                }
                Console.WriteLine();

            }

            if (kierunek == 2)
            {
                string sprPolozenia = Convert.ToString(pozycjaPoczatkowaStatku);
                char spr = Convert.ToChar(sprPolozenia[1]);

                if (pozycjaPoczatkowaStatku > 20)
                {
                    nrTablicy[pozycjaPoczatkowaStatku - 10] = ":";
                }
                if ((spr != '1') && (pozycjaPoczatkowaStatku > 20))
                {
                    nrTablicy[pozycjaPoczatkowaStatku - 11] = ":";
                }
                if ((spr != '9') && (pozycjaPoczatkowaStatku > 20))
                {
                    nrTablicy[pozycjaPoczatkowaStatku - 9] = ":";
                }
                if ((pozycjaPoczatkowaStatku + dlugoscStatku * 10) < 100)
                {
                    nrTablicy[pozycjaPoczatkowaStatku + dlugoscStatku * 10] = ":";
                }
                if ((pozycjaPoczatkowaStatku + dlugoscStatku * 10) < 100 && (spr != '1'))
                {
                    nrTablicy[(pozycjaPoczatkowaStatku + dlugoscStatku * 10) - 1] = ":";
                }
                if (((pozycjaPoczatkowaStatku + dlugoscStatku * 10) + 1) < 100 && (spr != '9'))
                {
                    nrTablicy[(pozycjaPoczatkowaStatku + dlugoscStatku * 10) + 1] = ":";
                }

                int j = 0;
                for (int i = pozycjaPoczatkowaStatku; i < pozycjaPoczatkowaStatku + dlugoscStatku * 10; i = i + 10)
                {
                    nrTablicy[i] = "+";
                    if (dlugoscStatku == 5)
                    {
                        tabStatek5[j] = i;
                        tabStatek5[5] = 2;
                    }
                    if (dlugoscStatku == 4)
                    {
                        tabStatek4[j] = i;
                        tabStatek4[4] = 2;
                    }
                    if (dlugoscStatku == 3)
                    {
                        tabStatek3[j] = i;
                        tabStatek3[3] = 2;
                    }
                    if (dlugoscStatku == 2)
                    {
                        tabStatek2[j] = i;
                        tabStatek2[2] = 2;
                    }
                    if (dlugoscStatku == 1)
                    {
                        tabStatek1[j] = i;
                    }
                    j++;

                    if (spr != '1')
                    {
                        nrTablicy[i - 1] = ":";
                    }
                    if (spr != '9')
                    {
                        nrTablicy[i + 1] = ":";
                    }
                }
                Console.WriteLine();
            }
        }
        private int sprawdzanie(int polozenie, int kierunek, int dlugosc)
        {
            string sprPolozenia = Convert.ToString(polozenie);
            char spr;
            if (kierunek == 1)
            {
                spr = Convert.ToChar(sprPolozenia[1]);
                int sprawdzajacy = Convert.ToInt32(new string(spr, 1));
                if ((sprawdzajacy >= (11 - dlugosc)) || (sprawdzajacy == 0))
                {
                    return 0;
                }
                if (wylosowanaPlansza[polozenie] == ":" || wylosowanaPlansza[polozenie] == "+")
                {
                    return 0;
                }
                if (dlugosc >= 1 && (wylosowanaPlansza[polozenie + 1] == "+" || wylosowanaPlansza[polozenie + 10] == "+" || wylosowanaPlansza[polozenie - 10] == "+" || wylosowanaPlansza[polozenie - 9] == "+"))
                {
                    return 0;
                }
                if (dlugosc >= 2 && (wylosowanaPlansza[polozenie + 2] == "+" || wylosowanaPlansza[polozenie + 12] == "+" || wylosowanaPlansza[polozenie - 8] == "+"))
                {
                    return 0;
                }
                if (dlugosc >= 3 && (wylosowanaPlansza[polozenie + 3] == "+" || wylosowanaPlansza[polozenie + 13] == "+" || wylosowanaPlansza[polozenie - 7] == "+"))
                {
                    return 0;
                }
                if (dlugosc > 4 && wylosowanaPlansza[polozenie + 4] == "+")
                {
                    return 0;
                }
                if ((polozenie - 1) > 10 && (polozenie - 1) % 10 != 0 && wylosowanaPlansza[polozenie - 1] == "+")
                {
                    return 0;
                }
                if ((polozenie + dlugosc) < 100 && (polozenie + dlugosc) % 10 != 0 && wylosowanaPlansza[polozenie + dlugosc] == "+")
                {
                    return 0;
                }
                if ((polozenie + 9) < 100 && (polozenie + 9) % 10 != 0 && wylosowanaPlansza[polozenie + 9] == "+")
                {
                    return 0;
                }
                if ((polozenie - 11) > 10 && (polozenie - 11) % 10 != 0 && wylosowanaPlansza[polozenie - 11] == "+")
                {
                    return 0;
                }
                if ((polozenie + dlugosc + 10) < 100 && (polozenie + dlugosc + 10) % 10 != 0 && wylosowanaPlansza[polozenie + dlugosc + 10] == "+")
                {
                    return 0;
                }
                if ((polozenie - 10 + dlugosc) > 10 && (polozenie - 10 + dlugosc) % 10 != 0 && wylosowanaPlansza[polozenie - 10 + dlugosc] == "+")
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            if (kierunek == 2)
            {
                spr = Convert.ToChar(sprPolozenia[0]);
                int sprawdzajacy = Convert.ToInt32(new string(spr, 1));
                char spr2 = Convert.ToChar(sprPolozenia[1]);
                int sprawdzajacy2 = Convert.ToInt32(new string(spr2, 1));

                if ((sprawdzajacy >= (11 - dlugosc)) || (sprawdzajacy2 == 0))
                {
                    return 0;
                }
                if (wylosowanaPlansza[polozenie] == ":" || wylosowanaPlansza[polozenie] == "+")
                {
                    return 0;
                }
                if (dlugosc > 1 && wylosowanaPlansza[polozenie + 10] == "+")
                {
                    return 0;
                }
                if (dlugosc > 2 && wylosowanaPlansza[polozenie + 20] == "+")
                {
                    return 0;
                }
                if (dlugosc > 3 && wylosowanaPlansza[polozenie + 30] == "+")
                {
                    return 0;
                }
                if (dlugosc > 4 && wylosowanaPlansza[polozenie + 40] == "+")
                {
                    return 0;
                }
                if (polozenie > 20 && wylosowanaPlansza[polozenie - 10] == "+")
                {
                    return 0;
                }
                if (spr != '1' && polozenie > 20 && wylosowanaPlansza[polozenie - 11] == "+")
                {
                    return 0;
                }
                if ((spr != '9') && (polozenie > 20) && wylosowanaPlansza[polozenie - 9] == "+")
                {
                    return 0;
                }
                if ((polozenie + (dlugosc - 1) * 10) > 100 || ((polozenie + dlugosc * 10) < 100 && (wylosowanaPlansza[polozenie + dlugosc * 10] == "+")))
                {
                    return 0;
                }
                if (((polozenie + (dlugosc - 1) * 10) > 100 && spr != '1') || ((polozenie + dlugosc * 10) < 100 && spr != '1' && wylosowanaPlansza[(polozenie + dlugosc * 10) - 1] == "+"))
                {
                    return 0;
                }
                if (((polozenie + (dlugosc - 1) * 10) > 100 && spr != '9') || ((polozenie + (dlugosc - 1) * 10) > 100 && spr != '9' && wylosowanaPlansza[(polozenie + dlugosc * 10) + 1] == "+"))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            else
            {
                return 0;
            }

        }
        public void zapis(string nick, string haslo, int punktyBazowe)
        {
            Console.WriteLine("Punkty bazowe: {0}",punktyBazowe);
            punktyRazem = punktyRazem + punktyBazowe;

            Console.WriteLine("Twoje punkty na koncie wynoszą teraz: {0}", punktyRazem);

            polaczenie = new SqlConnection(ustawieniaPolaczenia);
            dane.Append("Update tabelkaGraczy Set Punkty= N'" + punktyRazem + "' Where Nick='" + nick + "'and Haslo='" + haslo + "'");
            sqlQuery = dane.ToString();

            using (SqlCommand komenda = new SqlCommand(sqlQuery, polaczenie))
            {
                polaczenie.Open();
                komenda.ExecuteNonQuery();
            }
        }
    }
}
