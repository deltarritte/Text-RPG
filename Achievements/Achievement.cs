using RPGTestCBuildA;
using System;
using System.Collections.Generic;

namespace RPGTestC.Achievements
{
    public class Achievement
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public ConsoleColor Color { get; set; } = ConsoleColor.Green;

        public CheckCondition Checker;

        public delegate bool CheckCondition();
    }
}
