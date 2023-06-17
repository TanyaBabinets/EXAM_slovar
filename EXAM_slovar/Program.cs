using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EXAM_slovar
{
    [Serializable]
    [DataContract]

    public enum TypeOfDict
    {
        Eng_to_Rus, Rus_to_Eng
    }


    public class Words
    {

        private Dictionary<string, List<string>> dict;

        private TypeOfDict _typeOfDict;
        public Words(TypeOfDict typeOfDict)
        {
            _typeOfDict = typeOfDict;
            dict = new Dictionary<string, List<string>>();
        }//выделяем память на словарь  

        public void AddWord1(string one, List<string> two)
        {
            // Проверка наличия в словаре
            if (dict.ContainsKey(one))
            {
                Console.WriteLine($"Такое слово '{one}' уже существует.");

            }
            else
                // Добавление 
                dict.Add(one, two);
            Console.Write($" Слово '{one}' в переводе ");
            foreach (string word in two)
            {
                Console.Write(word + " , ");
            }
            Console.WriteLine("добавленo");
        }

        public void AddWord()
        {
            Console.WriteLine("Введите новое слово: ");
            string newW = Console.ReadLine();

            List<string> translations = new List<string>();
            // Проверка наличия в словаре
            if (dict.ContainsKey(newW))
            {
                Console.WriteLine($"Такое слово '{newW}' уже существует.");

            }
            else
            {
                Console.WriteLine("Введите новый перевод: ");
                // Добавление 
                string text = Console.ReadLine();
                translations.AddRange(text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                dict.Add(newW, translations);
                Console.Write($" Слово '{newW}' в переводе ");
                foreach (string word in translations)
                {
                    Console.Write(word + ", ");
                }
                Console.WriteLine("добавленo");
            }
        }
        //  удаление
        public void RemoveWord()
        {
            Console.WriteLine("Введите слово для удаления");
            string wordRemove = Console.ReadLine();
            if (dict.ContainsKey(wordRemove))
            {
                dict.Remove(wordRemove);
                Console.WriteLine($"Слово {wordRemove} удалено");
            }
        }
        public void DeleteTranslation() // delete вариант перевода
        {
            Console.WriteLine("Введите слово для удаления перевода");
            string w = Console.ReadLine();
            //показать все переводЫ!!!!!!!
            Console.WriteLine("Какой вариант перевода удалить?");
            string perevod = Console.ReadLine();

            if (dict.ContainsKey(w))
            {
                dict[w].Remove(perevod);
            }
            Console.WriteLine($"Вариант перевода слова {w} удален");
        }
        public void EditTranslation()
        {

            Console.WriteLine("Введите слово: ");
            string w = Console.ReadLine();
            Console.WriteLine($"Изменяем вариант перевода слова {w}");
            Console.WriteLine("Введите текущий перевод, которые надо изменить: ");
                 string currentWord = Console.ReadLine();
            Console.WriteLine("Введите слово, на которое надо заменить перевод: ");
                 string zamena = Console.ReadLine();

            if (dict.ContainsKey(w))
            {
                for (int i = 0; i < dict[w].Count; i++)
                {
                    if (dict[w][i] == currentWord)
                        dict[w][i] = zamena;
                }
                Console.WriteLine($"Вариант перевода слова {w} изменен");
            }
        }
        public void ChangeWord()
        {
            Console.WriteLine("Введите слово для замены в словаре: ");
            string wordChange = Console.ReadLine();
            Console.WriteLine("Введите замену: ");
            string b = Console.ReadLine();
            if (dict.ContainsKey(wordChange))
            {
               List<string> words = dict[wordChange];
                dict.Remove(wordChange);
                dict.Add(b, words);
                Console.WriteLine($"  словo {b} измененo");

            }
        }
        //   узнать perevod
        public void Translate() // поиск перевода слова
        {
            Console.WriteLine("Введите слово для перевода: ");
            string w = Console.ReadLine();

            List<string> value;
            Console.WriteLine($"Perevod slova {w}:");
            if (dict.TryGetValue(w, out value))// value=list translates
            {
                foreach (string word in value)
                {
                    Console.WriteLine(word);
                }
            }
        }
        public void SaveDict(TypeOfDict type)   
        {
            //записываем данные в файл
            string filename;
            if (type == TypeOfDict.Eng_to_Rus)
                filename = "slovaEng.txt";
            else
                filename = "slovaRus.txt";
            var newDict = dict.OrderBy(elem => elem.Key);//сортируем по алфавиту

                using (StreamWriter writer = new StreamWriter(filename))
                {
                    foreach (var d in newDict)
                    {                      
                        writer.WriteLine(d.Key);
                        writer.Write("translation: ");
                        foreach (var transl in d.Value)
                        {
                            writer.Write(transl + ", ");

                        }
                        writer.WriteLine();
                    }    
                     
                }
                Console.WriteLine("Данные загружены в файл");
                      
        }
        public void SaveWord()
        {
            //записываем данные в файл
            Console.WriteLine("Введите слово для сохранения в файл");
            string a=Console.ReadLine();   
            
            using (StreamWriter writer = new StreamWriter("Slovo.txt"))
            {
                if (dict.ContainsKey(a))
                {
                   
                    writer.Write("slovo: ");
                    writer.WriteLine(a);
                    writer.Write("translation: ");
                    foreach (var transl in dict[a])//проходим по переводу слова а
                    {
                        writer.Write(transl + ", ");

                    }
                    writer.WriteLine();
                }

            }
            Console.WriteLine("Слово загружено в файл");
        }
        
        public void LoadDict(TypeOfDict type)
        {
            string filename;
            if (type == TypeOfDict.Eng_to_Rus)
                filename = "slovaEng.txt";
            else
                filename = "slovaRus.txt";
            dict.Clear();
            using (StreamReader reader = new StreamReader(filename))
            {
                string word;
                while ((word = reader.ReadLine()) != null)
                {
                    string tr = reader.ReadLine();
                    string[] words = tr.Split(new char[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
                 
                    string[] Words = new string[words.Length - 1];
                    for (int i = 0; i < Words.Length; i++)
                        Words[i] = words[i + 1];//убираем слово translation

                    
                    dict.Add(word, Words.ToList());

                }
            }

            Console.WriteLine(ToString());
        }

        public override string ToString()
        {

            // Вывод информации 
            string res = "Весь список\n";
            foreach (var word in dict)
            {
                res += $"{word.Key,-15}  ";
                foreach (string word1 in word.Value)
                {
                    res += $"{word1}, ";
                }
                res += "\n";
            }
            return res;
        }
        public void Serial()
        {
            //   сериализация
            List<SerialDictionary> serializableDict = new List<SerialDictionary>();

            foreach (var kvp in dict)
            {
                SerialDictionary item = new SerialDictionary();
                item.Key = kvp.Key;
                item.Values = kvp.Value;
                serializableDict.Add(item);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<SerialDictionary>));

            using (FileStream stream = new FileStream("Eng-Rus.xml", FileMode.Create))
            {
                serializer.Serialize(stream, serializableDict);
                Console.WriteLine("Сериализация XML успешно выполнена!!");
            }
          
        }
        public void SerialJson()
        {
            FileStream stream3 = new FileStream("Task5.json", FileMode.Create);
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Dictionary<string, List<string>>));
            jsonFormatter.WriteObject(stream3, dict);
            stream3.Close();
            Console.WriteLine("Сериализация JSON успешно выполнена!");
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {

            Words wordsEng = new Words(TypeOfDict.Eng_to_Rus);
            Words wordsRus = new Words(TypeOfDict.Rus_to_Eng);

            wordsEng.AddWord1("cloud", new List<string>() { "облако", "туча", "туман", "затмить" });
            wordsEng.AddWord1("snippet", new List<string>() { "отрывок", "фрагмент" });
            wordsEng.AddWord1("ravage", new List<string>() { "разрушение", "разрушительные последствия" });
            wordsEng.AddWord1("complicit", new List<string>() { "замешан", "причастен", "соучастник" });
            wordsEng.AddWord1("confinement", new List<string>() { "заключение", "заточение" });
            wordsEng.AddWord1("expell", new List<string>() { "исключить", "выгнать" });
            wordsEng.AddWord1("disdain", new List<string>() { "презрение", "пренебрежение", "неуважение" });
            wordsEng.AddWord1("liability", new List<string>() { "ответственность", "обязанность", "обуза", "долг" });
            wordsEng.AddWord1("vouch", new List<string>() { "ручаться", "поручительство", "подтвердить" });
            wordsEng.AddWord1("hubris", new List<string>() { "высокомерие", "спесь", "надменность" });
            wordsEng.AddWord1("peers", new List<string>() { "ровесники", "сверстники", "коллеги" });
            wordsEng.AddWord1("doom", new List<string>() { "обреченность", "рок" });
            Console.WriteLine(new string ('-',60) );

            wordsRus.AddWord1("облако", new List<string>() { "cloud", "mist" });
            wordsRus.AddWord1("заманить", new List<string>() { "lure", "trap", "trick", "attract" });
            wordsRus.AddWord1("совпадение", new List<string>() { "match", "overlap", "coincidence" });
            wordsRus.AddWord1("ровесники", new List<string>() { "peers", "same age" });
            wordsRus.AddWord1("отрывок", new List<string>() { "snippet", "passage", "part", "extract" });
            wordsRus.AddWord1("мошенник", new List<string>() { "fraud", "scam", "cheater", "crook" });
            wordsRus.AddWord1("лицемер", new List<string>() { "hypocrite" });
            wordsRus.AddWord1("судьба", new List<string>() { "fate", "destiny", "fatum", "fortune" });
            wordsRus.AddWord1("провал", new List<string>() { "failure", "dip", "disaster", "collapse" });
            wordsRus.AddWord1("последний", new List<string>() { "last", "fatal", "latest" });
            wordsRus.AddWord1("утро", new List<string>() { "morning" });



            Words wordsChoice = null;  //выбранный словарь
            int choice = 1;
           bool flag = true;
            TypeOfDict typeOfDict = TypeOfDict.Eng_to_Rus;// ставим по умолчанию его
            do

            {
                bool isDictionary = true;//верно выбранный словарь
                if (flag)
                {
                    Console.WriteLine("\nChoose dict 1- eng, 2-rus:");
                    int choiceD = int.Parse(Console.ReadLine());
                    

                    switch (choiceD) //выбираем тип словаря - Анло-рус или Рус-англ
                    {
                        case 1:
                            wordsChoice = wordsEng;
                            break;
                        case 2:
                            wordsChoice = wordsRus;
                            typeOfDict = TypeOfDict.Rus_to_Eng;
                            break;
                        default:
                            Console.WriteLine("Error");
                            isDictionary = false;
                            break;
                    }
                }
                flag = false;   
                if (isDictionary) //если выбор словаря сделан верно
                {
                    Console.WriteLine("\n1.Перевести слово");
                    Console.WriteLine("2.Добавить слово");
                    Console.WriteLine("3.Показать весь список слов");
                    Console.WriteLine("4.Заменить слово");
                    Console.WriteLine("5.Удалить слово");
                    Console.WriteLine("6.Изменить вариант перевода");
                    Console.WriteLine("7.Удалить перевод");
                    Console.WriteLine("8.Поиск слова");
                    Console.WriteLine("9.Сохранить слово в файл");                    
                    Console.WriteLine("10.Соxранить  словарь в файл");                  
                    Console.WriteLine("11.Загрузить словарь из файла");                   
                    Console.WriteLine("12.Вернуться в предыдущее меню");
                    Console.WriteLine("13.Выполнить сериализацию ");
                    Console.WriteLine("0.EXIT ");
                    choice = int.Parse(Console.ReadLine());

                    switch (choice)
                    {
                        case 1:
                            wordsChoice.Translate();
                            break;
                        case 2:

                            wordsChoice.AddWord();
                            break;
                        case 3:
                            Console.WriteLine(wordsChoice);
                            break;
                        case 4:
                            wordsChoice.ChangeWord();
                            break;
                        case 5:
                            wordsChoice.RemoveWord();
                            break;
                        case 6:
                            wordsChoice.EditTranslation(); // слово, текущий перевод, новый перевод
                            break;
                        case 7:
                            wordsChoice.DeleteTranslation(); //слово и вариант перевода для удаления
                            break;
                        case 8:
                            wordsChoice.Translate();
                            break;
                        case 9:
                            wordsChoice.SaveWord();
                            break;
                        case 10:
                            wordsChoice.SaveDict(typeOfDict);
                            break;                     
                        case 11:
                            wordsChoice.LoadDict(typeOfDict);
                            break;
                        case 12:
                            flag= true; 
                            break;
                        case 13:
                            wordsChoice.Serial();
                            wordsChoice.SerialJson();
                            break;
                        case 0:
                            choice = 0;
                            break;

                    }
                }
            }
            while (choice != 0);

            Console.WriteLine(wordsEng);
            Console.WriteLine(wordsRus);
            Console.ReadKey();
        }
    }

}






