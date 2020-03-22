using RPGTestC.Achievements;
using RPGTestC.Events;
using RPGTestC.Locations;
using RPGTestC.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RPGTestCBuildA
{
    public class RPG
    {
        #region Variables
        static public Random rnd = new Random();

        static public float playerXP = 0;
        static public int masteryPoints = 0;
        static public int playerMoney = 20;
        static public byte lvl = 1;
        static public float weaponDamageCoeff = 1f;
        static public float armorDefenceCoeff = 1f;
        static public float playerHP = 52;
        static public float playerMaxHP = 50 + (float)Math.Pow(2, lvl);
        static public float lvlUpXP = 25;

        static public bool towerLoc = false;  //Доступна ли башня
        static public int questNum = 0; //Номер квеста, который в процессе выполнения (0 - игрок не был в таверне, 1 - Страж в Голубой Долине и т. д., всего 14(пока что))
        static public bool gotAnarchy = false;
        static public bool ahShit = false;
        static public bool brokeIn = false;
        
        static public float XP;
        static public int Money;
        static string savename;
        static public string XPLine;
        static public string HPLine;
        static string Spaces;
        static int Perc;

        static public List<object> saveList = new List<object>
            {
                City.cityBank,
                lvl,
                playerXP,
                lvlUpXP,
                playerHP,
                playerMaxHP,
                playerMoney,
                City.currentPlotState,
                questNum,
                City.wasInCity,
                weaponDamageCoeff,
                armorDefenceCoeff,
                masteryPoints,
                towerLoc,
                Tower.mageName,
                Tower.wasIntroduced,
                gotAnarchy,
                ahShit,
                brokeIn
            };
        #endregion

        public static void Fancies()
        {
            char a = '='; // Символ наполнения
            char b = ' '; // Пропуск
            char c = 'F'; // Символ поджога
            char d = 'P'; // Символ отравления

            Perc = Convert.ToInt16(Math.Round(playerHP / playerMaxHP, 1) * 20);

            if (Perc > 20) Perc = 20;

            switch (Fight.playerStatus)
            {
                case Fight.Status.OnFire:

                    HPLine = new string(c, Perc);

                    break;

                case Fight.Status.Poisoned:

                    HPLine = new string(d, Perc);

                    break;

                case Fight.Status.None:
                    
                    HPLine = new string(a, Perc);

                    break;
            }

            Spaces = new string (b, 20 - Perc);
            HPLine += Spaces;

            Perc = Convert.ToInt32(Math.Round(playerXP / lvlUpXP, 0) * 20);
            XPLine = new string (a, Perc);
            Spaces = new string (b, 20 - Perc);
            XPLine += Spaces;
        }

        static void SaveProgress(bool currentSave)
        {
            if (!currentSave)
            {
                if (File.Exists(savename))
                {
                    Console.WriteLine("Данный файл существует. Перезаписать? 1 - Да, 2 - Нет, 3 - Загрузить этот файл");
           
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1:
                            SaveProgress(true);
                            break;

                        case ConsoleKey.D2:
                            Console.WriteLine("Введите другое название файла сохранения (Образец: C:\\saveFile.txt)");
                            savename = Console.ReadLine();
                            SaveProgress(false);
                            break;

                        case ConsoleKey.D3:
                            LoadProgress(true);
                            break;

                        default:
                            Console.WriteLine("Некорректный ввод");
                            SaveProgress(currentSave);
                            break;
                    }
                }

                else
                {
                    SaveProgress(true);
                }
            }

            else
            {
                try
                {
                    string str = "";

                    foreach (object obj in saveList)
                    {
                        str += obj + "\n";
                    }

                    using (StreamWriter sw = new StreamWriter(savename, false, Encoding.Default)) sw.Write(str);

                    Console.WriteLine("Файл сохранён.");
                }

                catch
                {
                    Console.WriteLine("Некорректный ввод");
                    Console.WriteLine("Выберите название для файла сохранения (Образец: C:saveFile.txt)");
                    savename = Console.ReadLine();
                    SaveProgress(false);
                }
            }
        }

        static public void LoadProgress(bool currentSave)
        {
            if (!currentSave)
            {
                Console.WriteLine("Введите название загружаемого файла (Образец: C:\\saveFile.txt)");
                savename = Console.ReadLine();

                if (!File.Exists(savename))
                {
                    Console.WriteLine("Данный файл не существует. Попробуйте ещё раз.");
                    LoadProgress(false);
                }                   
                else
                    LoadProgress(true);
            }

            using (StreamReader sr = new StreamReader(savename))
            {
                try
                {
                    string line = sr.ReadLine();
                    
                    City.cityBank = Convert.ToSingle(line);

                    line = sr.ReadLine();
                    lvl = Convert.ToByte(line);

                    line = sr.ReadLine();
                    playerXP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    lvlUpXP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    playerHP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    playerMaxHP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    playerMoney = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    City.currentPlotState = (City.QuestState)Enum.Parse(typeof(City.QuestState), line);

                    line = sr.ReadLine();
                    questNum = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    City.wasInCity = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    weaponDamageCoeff = Convert.ToSingle(line);

                    line = sr.ReadLine();
                    armorDefenceCoeff = Convert.ToSingle(line);

                    line = sr.ReadLine();
                    masteryPoints = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    towerLoc = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    Tower.mageName = line;

                    line = sr.ReadLine();
                    Tower.wasIntroduced = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    gotAnarchy = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    ahShit = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    brokeIn = Convert.ToBoolean(line);
                }
                catch
                {
                    Console.WriteLine("Загружаемый файл, скорее всего, повреждён.");
                    LoadProgress(false);
                }

                Console.WriteLine("Файл загружен");
            }
        }

        static public void Main()
        {
            Console.WriteLine("Выберите название для файла сохранения (Образец: C:saveFile)");
            savename = Console.ReadLine();
            SaveProgress(false);

            Console.Title = "RPG C# Ver. 0.5 Build C";
            Dialogue("Ну привет, искатель приключений.");
            GetRandomEvent();
        }

        static public void GetRandomEvent()
        {
            Fancies();
            
            Console.WriteLine("\nИгрок:" +
            $"\nHP: {playerHP}/{playerMaxHP} [{HPLine}]" +
            $"\nУР {lvl}: {playerXP}/{lvlUpXP} [{XPLine}]");

            if (lvl == 15) Dialogue("lvl " + lvl + ": " + playerXP + "/" + lvlUpXP + " [" + XPLine + "] " +
                  "\nОчки мастерства: " + masteryPoints, true, ConsoleColor.Yellow);

            Console.WriteLine($"Деньги: {playerMoney}");

            Console.WriteLine("\nЛес - 1, Задание - 2, Город - 3");
            if (towerLoc) Console.WriteLine("Заброшенная башня - 9");
            Console.WriteLine("Сохранить игру - s, Загрузить игру - l, Достижения - a");

            switch (Console.ReadLine())
            {
                case "1":

                    Console.Clear();
                    int Event = rnd.Next(0, 10);

                    if (Event < 5)
                    {
                        Console.WriteLine("Тут ничего");
                    }

                    else if (Event == 5)
                    {
                        Money = rnd.Next(0, 10) + 2 * lvl;
                        playerMoney += Money;
                        Dialogue("Ого, сундук" +
                            "\nВы получили " + Money + " монет", true, ConsoleColor.Yellow);
                    }

                    else
                    {
                        Fight.Init(false);
                    }
                    break;

                case "2":

                    Console.Clear();

                    if (City.currentPlotState == City.QuestState.Accepted)
                    {
                        Console.WriteLine("Доступно сюжетное задание.");

                        switch (questNum)
                        {
                            case 1:
                                Console.WriteLine("Рекомендуемый уровень: 4");
                                break;

                            case 2:
                                Console.WriteLine("Рекомендуемый уровень: 7");
                                break;

                            case 3:
                                Console.WriteLine("Рекомендуемый уровень: 8");
                                break;
                        }

                        Console.WriteLine("Вы хотите начать это задание? Да - 1, Нет - 2");

                        if (Console.ReadLine() == "1")
                        {
                            PlotQuests.Quest();
                        }
                    }

                    else
                    {
                        Console.WriteLine("Нет доступных заданий.");
                    }
                    break;

                case "3":
                    City.GoToCity();
                    break;

                case "4":
                    Dungeons.Init(0, 0);
                    Console.Clear();
                    break;

                case "4 4":
                    Dungeons.Init(4, 0);
                    Console.Clear();
                    break;

                case "5":
                    int bufQuestNum = questNum;                                 // Буфферные
                    City.QuestState bufQuestState = City.currentPlotState;      // Переменные
                    questNum = 3;
                    PlotQuests.Quest();
                    Console.Clear();
                    City.currentPlotState = bufQuestState;
                    questNum = bufQuestNum;
                    break;

                case "9":
                    if (towerLoc)
                    {
                        Tower.Entry();
                        Console.Clear();
                    }
                    else goto default;
                    break;

                case "a":
                    AchievementMenu.ShowList();
                    Console.Clear();
                    break;

                case "l":
                    LoadProgress(false);
                    Console.Clear();
                    break;

                case "s":
                    SaveProgress(true);
                    Console.Clear();
                    break;

                case "fight poison":
                    Fight.mnstrType = Fight.MonsterType.Poisonous;
                    goto case "fight";

                case "fight explosive":
                    Fight.mnstrType = Fight.MonsterType.Explosive;
                    goto case "fight";

                case "fight throned":
                    Fight.mnstrType = Fight.MonsterType.Thorned;
                    goto case "fight";

                case "fight fire":
                    Fight.mnstrType = Fight.MonsterType.Fire;
                    goto case "fight";

                case "fight ice":
                    Fight.mnstrType = Fight.MonsterType.Ice;
                    goto case "fight";

                case "fight dark":
                    Fight.mnstrType = Fight.MonsterType.Dark;
                    goto case "fight";

                case "fight light":
                    Fight.mnstrType = Fight.MonsterType.Luminous;
                    goto case "fight";

                case "fight":
                    Fight.MnstrHP = 262260;
                    Fight.MnstrAttack = 0;
                    Fight.Name = "тестовый монстр-босс.";
                    Fight.Init(true);
                    break;

                case "scrooge":
                    Console.Clear();
                    playerMoney += 1000000;
                    break;

                case "gototavern":
                    if (questNum == 0) questNum = 1;
                    City.GoToTavern();
                    break;

                case "lvlup":
                    Fight.PlayerlvlUp(true);
                    break;

                case "gearupg":
                    City.GearUpgrade(true);
                    break;

                case "allthexp": //Дать максимальный уровень. 
                    //Кол-во опыта считается так: a*(1 + 2^0 + 2^1 + 2^2 + 2^3 + 2^4 + 2^5 ... + 2^n-3), где a - начальное кол-во опыта, n - необходимый уровень
                    playerXP = 204800;
                    Fight.PlayerlvlUp(false);
                    break;

                case "questscomplete": //"Выполнить" квесты
                    Console.Clear();
                    questNum = 15;
                    break;

                case "cheatsheet"://Список отладочных команд
                    Console.Clear();
                    Console.WriteLine("scrooge - +1.000.000 монет\n" +
                        "gototavern - пойти в таверну\n" +
                        "lvlup - повысить уровень\n" +
                        "gearupg - улучшить экипировку\n" +
                        "allthexp - получить двадцатый уровень\n" +
                        "questscomplete - выполнить все квесты (без награды)");
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Нет команды.");
                    break;
            }

            GetRandomEvent();
        }

        static public void Dialogue(object text, bool skip = false, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
            if (!skip)
            {
                Console.ReadKey();
            }
        }

        static public void Dialogue(string name, string text, bool skip = false, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(name + ": " + text);
            Console.ResetColor();
            if (!skip)
            {
                Console.ReadKey();
            }
        }
    }
}
