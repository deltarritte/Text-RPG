using RPGTestC.Items;
using RPGTestC.Items.Armour;
using RPGTestC.Items.Weapons;
using RPGTestC.Locations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RPGTestC
{
    public class Player
    {
        public enum Status                      // Тип перечисления статусов игрока
        {
            None,
            Poisoned,
            OnFire,
            Frozen,
            Blind
        }
        
        public static int LVL = 0;
        public static float XP = 0;
        public static float MaxXP = 25;

        public static float HP = 50;
        public static float MaxHP = 50 + (float)Math.Pow(2,LVL) - 1;

        public static Item[] Inventory = new Item[3]
        {
            //0-Weapon, 1-Armour, 2-Special Item
            new Default_W(),
            new Default_A(),
            new PotionBag()
        };

        public static Item[] Passive_Inventory = new Item[8]
        {
            new None_Item(),
            new None_Item(),
            new None_Item(),
            new None_Item(),
            new None_Item(),
            new None_Item(),
            new None_Item(),
            new None_Item(),
        };
        
        static public Status PStatus;      // Переменная статуса игрока
        static public int healCount = 3;        // Кол-во зарядов
        static public int maxHealCount = 3;     // Макс. кол-во зарядов

        public const float critCoeff = 1.75f;          // Коэффициент критического урона

        public static int questNum = 0; //Номер квеста, который в процессе выполнения (0 - игрок не был в таверне, 1 - Страж в Голубой Долине и т. д., всего 14(пока что))

        public static int Money = 20;
        public static int MasteryPoints = 0;

        static public bool towerLoc = false;    // Доступна ли башня
        static public bool gotAnarchy = false;  // Получено ли секретное достижение 
        static public bool ahShit = false;      // Получено ли секретное достижение
        static public bool brokeOut = false;    // Выбил ли игрок дверь
        static public bool BHDweller = false;   // Выпил ли игрок чёрную дыру

        static public string savename;

        static public float GetMaxXP() => 25 * (float)Math.Round(Math.Pow(1.5, LVL), 1);
        static public float GetMaxHP() => 50 + (float)Math.Pow(2, LVL);

        static public void SaveProgress(bool currentSave)
        {
            List<object> saveList = new List<object>
                {
                City.cityBank,
                LVL,
                XP,
                HP,
                Money,
                City.currentPlotState,
                questNum,
                City.wasInCity,
                MasteryPoints,
                towerLoc,
                Tower.mageName,
                Tower.wasIntroduced,
                gotAnarchy,
                ahShit,
                brokeOut,
                BHDweller,
            };

            foreach (Item it in Inventory)
            {
                saveList.Add(it.ID);
                saveList.Add(it.LVL);
            };

            foreach (Item it in Passive_Inventory)
            {
                saveList.Add(it.ID);
                saveList.Add(it.LVL);
            };

            if (!currentSave)
            {
                if (File.Exists(savename))
                {
                    Console.WriteLine("Данный файл существует. Перезаписать? 1 - Да, 2 - Нет, 3 - Загрузить этот файл");

                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            SaveProgress(true);
                            break;

                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            Console.WriteLine("Введите другое название файла сохранения (Образец: C:\\saveFile.txt)");
                            savename = Console.ReadLine();
                            SaveProgress(false);
                            break;

                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            LoadProgress(true);
                            break;

                        default:
                            Console.WriteLine("Некорректный ввод");
                            SaveProgress(currentSave);
                            break;
                    }
                }
                else SaveProgress(true);
            }
            else
            {
                try
                {
                    string str = "";

                    foreach (object obj in saveList) str += obj + "\n";

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
            int ID;
            int ILVL;

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
                    string line;

                    line = sr.ReadLine();
                    City.cityBank = Convert.ToSingle(line);

                    line = sr.ReadLine();
                    LVL = Convert.ToByte(line);

                    line = sr.ReadLine();
                    XP = Convert.ToInt32(line);
                    
                    MaxXP = GetMaxXP();

                    line = sr.ReadLine();
                    HP = Convert.ToInt32(line);
                    
                    MaxHP = GetMaxHP();

                    line = sr.ReadLine();
                    Money = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    City.currentPlotState = (City.QuestState)Enum.Parse(typeof(City.QuestState), line);

                    line = sr.ReadLine();
                    questNum = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    City.wasInCity = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    MasteryPoints = Convert.ToInt32(line);

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
                    brokeOut = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    BHDweller = Convert.ToBoolean(line);

                    #region Inventory Loading (Leave for last)
                    
                    for (int i = 0; i < Inventory.Length + Passive_Inventory.Length; i++)
                    {
                        line = sr.ReadLine();
                        ID = Convert.ToInt16(line);
                        // This is absolutely fucking horrible and disgusting, but I have no other clue on how to implement this. ffs!
                        /*
                         * Let me explain why I have to resort to such measures: Even though I could have a list of Items from wich I could pull from in
                         * the previous version of loading/saving the inventory, that would lead to something unpleasant: if you have Ying and have
                         * leveled it up, then each new Ying you get will be leveled up as well. See why this is an issue?
                         * 
                         * Basically, all I want to do is make unique examples of Items everytime the player gets one. Which is not ideal anyway.
                         */

                        Item buffer;
                        if (i >= Inventory.Length) buffer = Passive_Inventory[i-Inventory.Length];
                        else buffer = Inventory[i];
                        switch (ID)
                        {
                            case 0:
                                buffer = new None_Item();
                                break;
                            case 1:
                                buffer = new Default_A();
                                break;
                            case 2:
                                buffer = new Ying_A();
                                break;
                            case 3:
                                buffer = new Default_W();
                                break;
                            case 4:
                                buffer = new Yang_W();
                                break;
                            case 5:
                                buffer = new PotionBag();
                                break;
                            case 6:
                                buffer = new None_Item();
                                break;
                        }

                        line = sr.ReadLine();
                        ILVL = Convert.ToInt16(line);

                        buffer.LVL = ILVL;
                        buffer.Upgrade();
                    }
                    #endregion
                }
                catch
                {
                   Console.WriteLine("Загружаемый файл, скорее всего, повреждён.");
                   LoadProgress(false);
                }

                Console.WriteLine("Файл загружен");
            }
        }

        static public void LvlUp(bool cheat = false)
        {
            while ((XP >= MaxXP || cheat) && LVL <= 14)
            {
                LVL++;

                MaxXP = GetMaxXP();
                MaxHP = GetMaxHP();

                cheat = false;
            }

            if (LVL == 15)
            {
                MaxXP = 10950;
                MasteryPoints += (int)Math.Round((XP - MaxXP) / 10);
                XP = MaxXP;
            }
            Console.Clear();
        }
    }
}