using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Playfair
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите ключ: ");
            string key = Console.ReadLine();
            Console.WriteLine("\nВведите строку: ");
            string text = Console.ReadLine();
            text = text.ToLower();

            List<string> arr = new List<string>() { };
            text = text.Replace(" ","") ;
            string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя-._";
            string mat_alph = key + alphabet;
            mat_alph = new string(mat_alph.Distinct().ToArray());
           
            string[,] matrix = new string[6, 6];
            int sub = 0;
            int string_order = 0;
            string bigram = "";
            int e = 0;
            int r = 1;

            //добавление букв я, если встречаются одинаковые символы
            for(int d = 0; d<text.Length; d++)
            {
                if (text[e] == text[r])
                {
                    text = text.Insert(e+1, "я");
                  
                }
                if (e == text.Length && text[e] == text[r])
                {
                    text = text.Insert(e + 1, "я");
                    break;
                }
                else if(e == text.Length && text[e] != text[r])
                {
                    break;
                }

                e += 2;
                r += 2;
                if (r >= text.Length)
                {
                    break;
                }
            }

            //добавление букв я в конец 
            if (text.Length % 2 != 0 && Convert.ToString(text[text.Length - 1]) != "я")
            {
                text += "я";
            }
            else if (text.Length % 2 != 0 && text[text.Length - 1] == Convert.ToChar("я"))
            {
                text += "а";
            }
            

            //создание биграмм 
            for (int k = 0; k< text.Length/2; k++)
            {
                string enc = text.Substring(sub, 2);
                sub += 2;
                arr.Add(enc);
                bigram += enc + " " ;
                
            }
            Console.WriteLine("\n" + bigram + "\n");
            bigram = bigram.Replace(" ", "");

            //Создание матрицы
            for (int i=0; i<6; i++) 
            {
                for(int j = 0; j<6; j++)
                {
                    matrix[i, j] = Convert.ToString(mat_alph[string_order]);
                    string_order++;
                    Console.Write("{0,6}", matrix[i, j]);
                }
                Console.WriteLine();
            }

            //сохранение координат 
            List<int> cols = new List<int>() { };
            List<int> rows = new List<int>() { };
            int x = 0;
            bool ok = true;
            while (ok)
            {
                for (int i = 0; i < 6; i++) //строка
                {
                    for (int j = 0; j < 6; j++)//столбец
                    {
                        if (matrix[i, j] == Convert.ToString(bigram[x]))
                        {
                            if (x == bigram.Length-1)
                            {
                                ok = false;
                                rows.Add(i);
                                cols.Add(j);
                             
                                break;

                            }
                                rows.Add(i);
                                cols.Add(j);
                                x++;
                        }
                    }

                }
            }

            //шифрование
            string encrypt = "";
            int row_1;
            int col_1;
            int row_2;
            int col_2;
            int enc_row_1;
            int enc_col_1;
            int enc_row_2=0;
            int enc_col_2;

            for (int p=0; p<text.Length;)
            {
                row_1 = rows[p];
                col_1 = cols[p];

                row_2 = rows[p + 1];
                col_2 = cols[p + 1];

                //на одной строке
                if(row_1 == row_2)
                {
                    //если крайний
                    if (col_1 == 5)
                    {
                        enc_col_1 = 0;
                        enc_row_1 = row_1;
                        enc_row_2 = row_1;
                        enc_col_2 = col_2 + 1;
                        encrypt += matrix[enc_row_1, enc_col_1];
                        encrypt += matrix[enc_row_2, enc_col_2] + " ";
                        p += 2;
                    }

                    else if (col_2 == 5)
                    {
                        enc_col_1 = col_1+1;
                        enc_row_1 = row_1;
                        enc_row_2 = row_1;
                        enc_col_2 = 0;
                        encrypt += matrix[enc_row_1, enc_col_1];
                        encrypt += matrix[enc_row_2, enc_col_2] + " ";
                        p += 2;
                    }

                    else
                    {
                        enc_col_1 = col_1 + 1;
                        enc_row_1 = row_1;
                        enc_row_2 = row_1;
                        enc_col_2 = col_2 + 1;
                        encrypt += matrix[enc_row_1, enc_col_1];
                        encrypt += matrix[enc_row_2, enc_col_2] + " ";
                        p += 2;
                    }
                }

                //на одном столбце
                if (col_1 == col_2)
                {
                    if (row_1 == 5)
                    {
                        enc_col_1 = col_1;
                        enc_row_1 = 0;
                        enc_row_2 = row_2 + 1;
                        enc_col_2 = col_1;
                        encrypt += matrix[enc_row_1, enc_col_1];
                        encrypt += matrix[enc_row_2, enc_col_2] + " ";
                        p += 2;
                    }

                    else if (row_2 == 5)
                    {
                        enc_col_1 = col_1;
                        enc_row_1 = row_1 + 1;
                        enc_row_2 = 0;
                        enc_col_2 = col_1;
                        encrypt += matrix[enc_row_1, enc_col_1];
                        encrypt += matrix[enc_row_2, enc_col_2] + " ";
                        p += 2;
                    }
                    else
                    {
                        enc_col_1 = col_1;
                        enc_row_1 = row_1 + 1;
                        enc_row_2 = row_2 + 1;
                        enc_col_2 = col_1;
                        encrypt += matrix[enc_row_1, enc_col_1];
                        encrypt += matrix[enc_row_2, enc_col_2] + " ";
                        p += 2;
                    }
                }

                else if (row_1 != row_2 && col_1 != col_2)
                {
                    enc_row_1 = rows[p];
                    enc_col_1 = cols[p + 1];
                    enc_row_2 = rows[p + 1];
                    enc_col_2 = cols[p];

                    encrypt += matrix[enc_row_1, enc_col_1];
                    encrypt += matrix[enc_row_2, enc_col_2] + " ";
                    p += 2;
                }
            }
            Console.WriteLine("\nЗашифрованные биграммы: " +encrypt );
            encrypt = encrypt.Replace(" ", "");
            Console.WriteLine("Зашифрованный текст: " + encrypt);

            //сохранение координат зашифрованного текста
            int y = 0;
            List<int> dec_rows = new List<int>() { };
            List<int> dec_cols = new List<int>() { };
            bool check = true;

            while (check)
            {
                for (int i = 0; i < 6; i++) //строка
                {
                    for (int j = 0; j < 6; j++)//столбец
                    {
                        if (matrix[i, j] == Convert.ToString(encrypt[y]))
                        {
                            if (y == encrypt.Length - 1)
                            {
                                check = false;
                                dec_rows.Add(i);
                                dec_cols.Add(j);

                                break;

                            }
                            dec_rows.Add(i);
                            dec_cols.Add(j);
                            y++;
                              
                        }
                       
                    }
                }
            }

      
            //дешифрование
            string decrypt = "";
            int _row_1;
            int _col_1;
            int _row_2;
            int _col_2;
            int dec_row_1;
            int dec_col_1;
            int dec_row_2 = 0;
            int dec_col_2;

            for (int d = 0; d < text.Length;)
            {
                _row_1 = dec_rows[d];
                _col_1 = dec_cols[d];

                _row_2 = dec_rows[d + 1];
                _col_2 = dec_cols[d + 1];

                //на одной строке
                if (_row_1 == _row_2)
                {
                    //если крайний
                    if (_col_1 == 0)
                    {
                        dec_col_1 = 5;
                        dec_row_1 = _row_1;
                        dec_row_2 = _row_1;
                        dec_col_2 = _col_2 - 1;
                        decrypt += matrix[dec_row_1, dec_col_1];
                        decrypt += matrix[dec_row_2, dec_col_2] + " ";
                        d += 2;
                    }

                    else if (_col_2 == 0)
                    {
                        dec_col_1 = _col_1 - 1;
                        dec_row_1 = _row_1;
                        dec_row_2 = _row_1;
                        dec_col_2 = 5;
                        decrypt += matrix[dec_row_1, dec_col_1];
                        decrypt += matrix[dec_row_2, dec_col_2] + " ";
                        d += 2;
                    }

                    else
                    {
                        dec_col_1 = _col_1 - 1;
                        dec_row_1 = _row_1;
                        dec_row_2 = _row_1;
                        dec_col_2 = _col_2 - 1;
                        decrypt += matrix[dec_row_1, dec_col_1];
                        decrypt += matrix[dec_row_2, dec_col_2] + " ";
                        d += 2;
                    }
                }

                //на одном столбце
                if (_col_1 == _col_2)
                {
                    if (_row_1 == 0)
                    {
                        dec_col_1 = _col_1;
                        dec_row_1 = 5;
                        dec_row_2 = _row_2 - 1;
                        dec_col_2 = _col_1;
                        decrypt += matrix[dec_row_1, dec_col_1];
                        decrypt += matrix[dec_row_2, dec_col_2] + " ";
                        d += 2;
                    }

                    else if (_row_2 == 0)
                    {
                        dec_col_1 = _col_1;
                        dec_row_1 = _row_1 - 1;
                        dec_row_2 = 5;
                        dec_col_2 = _col_1;
                        decrypt += matrix[dec_row_1, dec_col_1];
                        decrypt += matrix[dec_row_2, dec_col_2] + " ";
                        d += 2;
                    }
                    else
                    {
                        dec_col_1 = _col_1;
                        dec_row_1 = _row_1 - 1;
                        dec_row_2 = _row_2 - 1;
                        dec_col_2 = _col_1;
                        decrypt += matrix[dec_row_1, dec_col_1];
                        decrypt += matrix[dec_row_2, dec_col_2] + " ";
                        d += 2;
                    }
                }

                else if (_row_1 != _row_2 && _col_1 != _col_2)
                {
                    dec_row_1 = dec_rows[d];
                    dec_col_1 = dec_cols[d + 1];
                    dec_row_2 = dec_rows[d + 1];
                    dec_col_2 = dec_cols[d];

                    decrypt += matrix[dec_row_1, dec_col_1];
                    decrypt += matrix[dec_row_2, dec_col_2] + " ";
                    d += 2;
                }
            }
            Console.WriteLine("\nРасшифрованные биграммы: " + decrypt);
            decrypt = decrypt.Replace(" ", "");
            

            //удаление букв я
            int q = 0;
            int n = 2;
            for (int i = 0; i < decrypt.Length; i++)
            {
                if (decrypt[q] == decrypt[n] && Convert.ToString(decrypt[q + 1]) == "я")
                {
                    decrypt = decrypt.Remove(q + 1,1);
                    if (decrypt.Length % 2 != 0)
                    {
                        decrypt = decrypt.Remove(decrypt.Length - 1, 1);
                    }
                }

                if(decrypt.Length % 2 == 0 && Convert.ToString(decrypt[decrypt.Length-1]) == "я")
                {
                    decrypt = decrypt.Remove(decrypt.Length - 1, 1);
                }
                q += 2;
                n += 2;

                if (q + 2 >= decrypt.Length)
                {
                    break;
                }
            }
            Console.WriteLine("Расшифрованный текст: " + decrypt);

        }
    }
}
