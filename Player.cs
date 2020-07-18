using RPGTestC.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static float MaxHP = 50 + (float)Math.Pow(2,LVL);

        static public float Damage = (float)Math.Round((10 + Math.Pow(2, LVL))) * DamageCoeff;       // Значение атаки игрока
        static public float Defence = (float)(Math.Pow(LVL, 2) - 1 / (4 * LVL + 1)) * DefenceCoeff;     // Значение защиты игрока
        static public Status PStatus;      // Переменная статуса игрока
        static public int healCount = 3;        // Кол-во зарядов
        static public int maxHealCount = 3;     // Макс. кол-во зарядов

        public const float critCoeff = 1.75f;          // Коэффициент критического урона
        public static float DamageCoeff = 1f;
        public static float DefenceCoeff = 1f;

        public static int questNum = 0; //Номер квеста, который в процессе выполнения (0 - игрок не был в таверне, 1 - Страж в Голубой Долине и т. д., всего 14(пока что))

        public static int Money = 20;
        public static int MasteryPoints = 0;

        static public bool towerLoc = false;    // Доступна ли башня
        static public bool gotAnarchy = false;  // Получено ли секретное достижение 
        static public bool ahShit = false;      // Получено ли секретное достижение
        static public bool brokeOut = false;    // Выбил ли игрок дверь

        static public string savename;

        static public float GetMaxXP() => 25 * (float)Math.Round(Math.Pow(1.5, LVL), 1);
        static public float GetMaxHP() => 50 + (float)Math.Pow(2, LVL);
        static public float GetDamage() => (float)Math.Round((10 + Math.Pow(2, LVL))) * DamageCoeff;
        static public float GetDefence() => (float)(Math.Pow(LVL, 2) - 1 / (4 * LVL + 1)) * DefenceCoeff;

        static public void SaveProgress(bool currentSave)
        {
            List<object> saveList = new List<object>
                {
                City.cityBank,
                LVL,
                XP,
                MaxXP,
                HP,
                MaxHP,
                Money,
                City.currentPlotState,
                questNum,
                City.wasInCity,
                DamageCoeff,
                DefenceCoeff,
                MasteryPoints,
                towerLoc,
                Tower.mageName,
                Tower.wasIntroduced,
                gotAnarchy,
                ahShit,
                brokeOut
            };

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

                    line = sr.ReadLine();
                    MaxXP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    HP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    MaxHP = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    Money = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    City.currentPlotState = (City.QuestState)Enum.Parse(typeof(City.QuestState), line);

                    line = sr.ReadLine();
                    questNum = Convert.ToInt32(line);

                    line = sr.ReadLine();
                    City.wasInCity = Convert.ToBoolean(line);

                    line = sr.ReadLine();
                    DamageCoeff = Convert.ToSingle(line);

                    line = sr.ReadLine();
                    DefenceCoeff = Convert.ToSingle(line);

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
                Damage = GetDamage();
                Defence = GetDefence();

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
