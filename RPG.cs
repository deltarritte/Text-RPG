using RPGTestC.Achievements;
using RPGTestC.Events;
using RPGTestC.Items;
using RPGTestC.Items.Armour;
using RPGTestC.Items.Weapons;
using RPGTestC.Locations;
using RPGTestC.Quests;
using RPGTestC.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static RPGTestC.Player;

/*/
 * Made by deltarritte
 * for no reason whatsoever
 * 
 * plz do knot steel
/*/

namespace RPGTestC
{
    public class RPG
    {
        #region Variables
        public static string VERSION = "05";

        static public Random rnd = new Random();
        
        static int Money;
        static public string XPLine;
        static public string HPLine;
        static string Spaces;
        static int Perc;

        public struct ChainAction
        {
            public int num;
            public char name;
            public int cost;
            public float weight;
        }
        #endregion

        static public void Fancies()
        {
            char a = '='; // Символ наполнения
            char b = ' '; // Пропуск
            char c = 'F'; // Символ поджога
            char d = 'P'; // Символ отравления

            HPLine = "";
            XPLine = "";

            Perc = Convert.ToInt16(Math.Round(Player.HP / Player.MaxHP, 1) * 20);

            if (Perc > 20) Perc = 20;

            switch (PStatus)
            {
                case Status.OnFire:

                    HPLine = new string(c, Perc);

                    break;

                case Status.Poisoned:

                    HPLine = new string(d, Perc);

                    break;

                default:
                    
                    HPLine = new string(a, Perc);

                    break;
            }

            Spaces = new string (b, 20 - Perc);
            HPLine += Spaces;

            Perc = Convert.ToInt32(Math.Round(Player.XP / Player.MaxXP, 0) * 20);
            XPLine = new string (a, Perc);
            Spaces = new string (b, 20 - Perc);
            XPLine += Spaces;
        }

        static public void Main()
        {
            Console.Title = "RPG C# Ver."+VERSION;

            if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");
            SaveFileMenu.isLoading = true;
            SaveFileHandler();
            Console.Clear();

            Dialogue("Ну привет, искатель приключений.",true);

            GetRandomEvent();
        }

        static public void GetRandomEvent()
        {
            Fancies();

            Monster test = new Monster();

            Console.WriteLine("\nИгрок:" +
            $"\nHP: {HP}/{MaxHP} [{HPLine}]");

            if (LVL != 15) Console.WriteLine($"УР {LVL}: {XP}/{MaxXP} [{XPLine}]");
            else Dialogue($"LVL {LVL}: {XP}/{MaxXP} [{XPLine}] " +
                        $"\nОчки мастерства: {MasteryPoints}", true, ConsoleColor.Yellow);

            Console.WriteLine($"Деньги: {Player.Money}");

            Console.WriteLine("\nЛес - 1, Задание - 2, Город - 3");
            if (towerLoc) Console.WriteLine("Заброшенная башня - 9");
            Console.WriteLine("Сохранить игру - s, Загрузить игру - l, Достижения - a, Инвентарь - i");

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
                        Money = rnd.Next(0, 10) + 2 * LVL;
                        Player.Money += Money;
                        Dialogue("Ого, сундук" +
                            "\nВы получили " + Money + " монет", true, ConsoleColor.Yellow);
                    }
                    else Fight.Init();
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

                    else Console.WriteLine("Нет доступных заданий.");
                    break;

                case "3":
                    City.GoToCity();
                    break;

                case "4":
                    Dungeons.Init(0);
                    Console.Clear();
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

                case "i":
                    InventoryMenu.ShowMenu();
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
                    test.Type = Monster.MType.Poisonous;
                    goto case "fight";

                case "fight explosive":
                    test.Type = Monster.MType.Explosive;
                    goto case "fight";

                case "fight thorned":
                    test.Type = Monster.MType.Thorned;
                    goto case "fight";

                case "fight fire":
                    test.Type = Monster.MType.Fire;
                    goto case "fight";

                case "fight ice":
                    test.Type = Monster.MType.Ice;
                    goto case "fight";

                case "fight dark":
                    test.Type = Monster.MType.Dark;
                    goto case "fight";

                case "fight light":
                    test.Type = Monster.MType.Luminous;
                    goto case "fight";

                case "fight":
                    test.HP = 262260;
                    test.Damage = 0;
                    test.Name = "тестовый монстр-босс";
                    Fight.Init(new Monster[1] { test }, true);
                    break;

                case "fight 2":
                    Fight.Init(2);
                    break;

                case "bring balance":
                    for (int i = 0; i < Passive_Inventory.Length; i++)
                    {
                        if (Passive_Inventory[i].ID == 0)
                        {
                            Passive_Inventory[i] = new Yang_W();
                            break;
                        }
                    }
                    for (int i = 0; i < Passive_Inventory.Length; i++)
                    {
                        if (Passive_Inventory[i].ID == 0)
                        {
                            Passive_Inventory[1] = new Ying_A();
                            break;
                        }
                    }
                    break;

                case "scrooge":
                    Console.Clear();
                    Player.Money += 1000000;
                    break;

                case "gototavern":
                    if (questNum == 0) questNum = 1;
                    City.GoToTavern();
                    break;

                case "lvlup":
                    LvlUp(true);
                    break;

                case "gearupg":
                    City.GearUpgrade(true);
                    break;

                case "allthexp": //Дать максимальный уровень. 
                    //Кол-во опыта считается так: a*(1 + 2^0 + 2^1 + 2^2 + 2^3 + 2^4 + 2^5 ... + 2^n-3), где a - начальное кол-во опыта, n - необходимый уровень
                    XP = 204800;
                    LvlUp();
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