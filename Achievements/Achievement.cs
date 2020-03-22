using RPGTestCBuildA;
using System;
using System.Collections.Generic;

namespace RPGTestC.Achievements
{
    public class Achievement
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool Condition { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.Green;

        #region Progress Achievements
        static public Achievement prog1 = new Achievement
        {
            Title = "Прогресс 1.",
            Description = "Получите второй уровень.",
            Condition = RPG.lvl >= 2,
        };

        static public Achievement prog2 = new Achievement
        {
            Title = "Прогресс 2.",
            Description = "Получите третий уровень.",
            Condition = RPG.lvl >= 3,
        };

        static public Achievement prog3 = new Achievement
        {
            Title = "Прогресс 3.",
            Description = "Получите четвёртый уровень.",
            Condition = RPG.lvl >= 4,
        };

        static public Achievement prog4 = new Achievement
        {
            Title = "Прогресс 4.",
            Description = "Получите пятый уровень.",
            Condition = RPG.lvl >= 5,
        };

        static public Achievement prog5 = new Achievement
        {
            Title = "Прогресс 5.",
            Description = "Получите шестой уровень.",
            Condition = RPG.lvl >= 6,
        };

        static public Achievement prog6 = new Achievement
        {
            Title = "Прогресс 6.",
            Description = "Получите седьмой уровень.",
            Condition = RPG.lvl >= 7,
        };

        static public Achievement prog7 = new Achievement
        {
            Title = "Прогресс 7.",
            Description = "Получите восьмой уровень.",
            Condition = RPG.lvl >= 8,
        };
        static public Achievement prog8 = new Achievement
        {
            Title = "Прогресс 8.",
            Description = "Получите девятый уровень.",
            Condition = RPG.lvl >= 9,
        };
        static public Achievement prog9 = new Achievement
        {
            Title = "Прогресс 9.",
            Description = "Получите десятый уровень.",
            Condition = RPG.lvl >= 10,
        };
        static public Achievement prog10 = new Achievement
        {
            Title = "Прогресс 10.",
            Description = "Получите одиннадцатый уровень.",
            Condition = RPG.lvl >= 11,
        };
        static public Achievement prog11 = new Achievement
        {
            Title = "Прогресс 11.",
            Description = "Получите двенадцатый уровень.",
            Condition = RPG.lvl >= 12,
        };
        static public Achievement prog12 = new Achievement
        {
            Title = "Прогресс 12.",
            Description = "Получите тринадцатый уровень.",
            Condition = RPG.lvl >= 13,
        };
        static public Achievement prog13 = new Achievement
        {
            Title = "Прогресс 13.",
            Description = "Получите четырнадцатый уровень.",
            Condition = RPG.lvl >= 14,
        };
        static public Achievement prog14 = new Achievement
        {
            Title = "Прогресс 14.",
            Description = "Получите пятнадцатый уровень.",
            Condition = RPG.lvl >= 15,
            Color = ConsoleColor.Yellow
        };
        #endregion

        #region Quest Achievements
        public static Achievement quest1 = new Achievement
        {
            Title = "Синее Смещение.",
            Description = "Выполнить квест по убийству Стража.",
            Condition = RPG.questNum >= 2,
        };

        public static Achievement quest2 = new Achievement
        {
            Title = "Расследование Века.",
            Description = "Обнаружить тело торговца.",
            Condition = RPG.questNum >= 3,
        };

        public static Achievement quest3 = new Achievement
        {
            Title = "Неугодный Житель.",
            Description = "Выгнать нечто из подвала таверны.",
            Condition = RPG.questNum >= 4
        };
        #endregion

        public static Achievement anarchy = new Achievement
        {
            Title = "Анархия и Хаос.",
            Description = "Застать момент анархии в Таверне.",
            Condition = RPG.gotAnarchy,
            Color = ConsoleColor.Yellow
        };

        public static Achievement memeref = new Achievement
        {
            Title = "Отсылка к старому мему.",
            Description = "Сделать отсылку к тупому мему.",
            Condition = RPG.ahShit,
            Color = ConsoleColor.Yellow
        };

        public static Achievement brokenDoor = new Achievement
        {
            Title = "Прямой подход.",
            Description = "Выломать дверь, ведущую в подвал.",
            Condition = RPG.brokeIn,
            Color = ConsoleColor.Yellow
        };

        public static Achievement nonBrute = new Achievement
        {
            Title = "Дверной пацифист.",
            Description = "Найти мирное решение проблемы.",
            Condition = !RPG.brokeIn && RPG.questNum > 3,
            Color = ConsoleColor.Yellow
        };

        public static Achievement tower = new Achievement
        {
            Title = "Загадочная встреча.",
            Description = "Узнать о существовании Башни.",
            Condition = RPG.towerLoc
        };
        
        public static List<Achievement> achList = new List<Achievement>
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
}
