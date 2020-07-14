using RPGTestCBuildA;
using System;
using System.Collections.Generic;

namespace RPGTestC.Achievements
{
    public class AchievementMenu
    {
        static List<Achievement> achList;

        #region Progress Achievements
        public static Achievement prog1 = new Achievement
        {
            Title = "Прогресс 1.",
            Description = "Получите второй уровень.",
            Checker = Prog1_Check
        };
        public static Achievement prog2 = new Achievement
        {
            Title = "Прогресс 2.",
            Description = "Получите третий уровень.",
            Checker = Prog2_Check
        };
        public static Achievement prog3 = new Achievement
        {
            Title = "Прогресс 3.",
            Description = "Получите четвёртый уровень.",
            Checker = Prog3_Check
        };
        public static Achievement prog4 = new Achievement
        {
            Title = "Прогресс 4.",
            Description = "Получите пятый уровень.",
            Checker = Prog4_Check
        };
        public static Achievement prog5 = new Achievement
        {
            Title = "Прогресс 5.",
            Description = "Получите шестой уровень.",
            Checker = Prog5_Check
        };
        public static Achievement prog6 = new Achievement
        {
            Title = "Прогресс 6.",
            Description = "Получите седьмой уровень.",
            Checker = Prog6_Check
        };
        public static Achievement prog7 = new Achievement
        {
            Title = "Прогресс 7.",
            Description = "Получите восьмой уровень.",
            Checker = Prog7_Check
        };
        public static Achievement prog8 = new Achievement
        {
            Title = "Прогресс 8.",
            Description = "Получите девятый уровень.",
            Checker = Prog8_Check
        };
        public static Achievement prog9 = new Achievement
        {
            Title = "Прогресс 9.",
            Description = "Получите десятый уровень.",
            Checker = Prog9_Check
        };
        public static Achievement prog10 = new Achievement
        {
            Title = "Прогресс 10.",
            Description = "Получите одиннадцатый уровень.",
            Checker = Prog10_Check
        };
        public static Achievement prog11 = new Achievement
        {
            Title = "Прогресс 11.",
            Description = "Получите двенадцатый уровень.",
            Checker = Prog11_Check
        };
        public static Achievement prog12 = new Achievement
        {
            Title = "Прогресс 12.",
            Description = "Получите тринадцатый уровень.",
            Checker = Prog12_Check
        };
        public static Achievement prog13 = new Achievement
        {
            Title = "Прогресс 13.",
            Description = "Получите четырнадцатый уровень.",
            Checker = Prog13_Check
        };
        public static Achievement prog14 = new Achievement
        {
            Title = "Прогресс 14.",
            Description = "Получите пятнадцатый уровень.",
            Checker = Prog14_Check,
            Color = ConsoleColor.Yellow
        };
        #endregion

        #region Quest Achievements
        public static Achievement quest1 = new Achievement
        {
            Title = "Синее Смещение.",
            Description = "Выполнить квест по убийству Стража.",
            Checker = quest1_Check,
        };

        public static Achievement quest2 = new Achievement
        {
            Title = "Расследование Века.",
            Description = "Обнаружить тело торговца.",
            Checker = quest2_Check,
        };

        public static Achievement quest3 = new Achievement
        {
            Title = "Неугодный Житель.",
            Description = "Выгнать нечто из подвала таверны.",
            Checker = quest3_Check,
        };
        #endregion

        public static Achievement anarchy = new Achievement
        {
            Title = "Анархия и Хаос.",
            Description = "Застать момент анархии в Таверне.",
            Checker = anarchy_Check,
            Color = ConsoleColor.Yellow
        };
        public static Achievement memeref = new Achievement
        {
            Title = "Отсылка к старому мему.",
            Description = "Сделать отсылку к тупому мему.",
            Checker = anarchy_Check,
            Color = ConsoleColor.Yellow
        };
        public static Achievement brokenDoor = new Achievement
        {
            Title = "Прямой подход.",
            Description = "Выломать дверь, ведущую в подвал.",
            Checker = brokenDoor_Check,
            Color = ConsoleColor.Yellow
        };
        public static Achievement nonBrute = new Achievement
        {
            Title = "Дверной пацифист.",
            Description = "Найти мирное решение проблемы.",
            Checker = nonBrute_Check,
            Color = ConsoleColor.Yellow
        };
        public static Achievement tower = new Achievement
        {
            Title = "Загадочная встреча.",
            Description = "Узнать о существовании Башни.",
            Checker = tower_Check,
        };

        public static void UpdateList()
        {
            achList = new List<Achievement>
            {
                prog1,
                prog2,
                prog3,
                prog4,
                prog5,
                prog6,
                prog7,
                prog8,
                prog9,
                prog10,
                prog11,
                prog12,
                prog13,
                prog14,

                quest1,
                quest2,
                quest3,

                anarchy,
                memeref,
                brokenDoor,
                nonBrute,
                tower
            };
        }

        #region Checks
        private static bool Prog1_Check() => RPG.lvl >= 2;
        private static bool Prog2_Check() => RPG.lvl >= 3;
        private static bool Prog3_Check() => RPG.lvl >= 4;
        private static bool Prog4_Check() => RPG.lvl >= 5;
        private static bool Prog5_Check() => RPG.lvl >= 6;
        private static bool Prog6_Check() => RPG.lvl >= 7;
        private static bool Prog7_Check() => RPG.lvl >= 8;
        private static bool Prog8_Check() => RPG.lvl >= 9;
        private static bool Prog9_Check() => RPG.lvl >= 10;
        private static bool Prog10_Check() => RPG.lvl >= 11;
        private static bool Prog11_Check() => RPG.lvl >= 12;
        private static bool Prog12_Check() => RPG.lvl >= 13;
        private static bool Prog13_Check() => RPG.lvl >= 14;
        private static bool Prog14_Check() => RPG.lvl >= 15;
        private static bool quest1_Check() => RPG.questNum >= 2;
        private static bool quest2_Check() => RPG.questNum >= 3;
        private static bool quest3_Check() => RPG.questNum >= 4;
        private static bool anarchy_Check() => RPG.gotAnarchy;
        private static bool memeref_Check() => RPG.ahShit;
        private static bool brokenDoor_Check() => RPG.brokeIn;
        private static bool nonBrute_Check() => !RPG.brokeIn && RPG.questNum > 3;
        private static bool tower_Check() => RPG.towerLoc;
        #endregion

        public static void ShowList()
        {
            Console.Clear();

            UpdateList();

            int count = 0;

            foreach (Achievement ach in achList)
            {
                if (ach.Checker())
                {
                    count++;
                    RPG.Dialogue("\n" + ach.Title, true, ach.Color);
                    Console.WriteLine(ach.Description);
                }
                else RPG.Dialogue("\n" + ach.Title, true, ConsoleColor.Red);
            }

            Console.WriteLine("\n" + count + "/" + achList.Count + " достижений получено.");

            Console.ReadKey();
        }
    }
}
