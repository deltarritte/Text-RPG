using RPGTestCBuildA;
using System;

namespace RPGTestC.Achievements
{
    public class AchievementMenu
    {
        public static void ShowList()
        {
            Console.Clear();

            Console.WriteLine(Achievement.achList.Count + " достижений всего.");

            foreach (Achievement Achievement in Achievement.achList)
            {
                if (Achievement.Condition)
                {
                    RPG.Dialogue("\n" + Achievement.Title, true, Achievement.Color);
                    Console.WriteLine(Achievement.Description);
                }

                else
                {
                    RPG.Dialogue("\n" + Achievement.Title, true, ConsoleColor.Red);
                }
            }

            Console.ReadKey();
        }
    }
}
