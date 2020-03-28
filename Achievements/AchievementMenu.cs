using RPGTestCBuildA;
using System;

namespace RPGTestC.Achievements
{
    public class AchievementMenu
    {
        public static void ShowList()
        {
            Console.Clear();

            int count = 0;

            foreach (Achievement ach in Achievement.achList)
            {
                if (ach.Condition)
                {
                    count++;
                    RPG.Dialogue("\n" + ach.Title, true, ach.Color);
                    Console.WriteLine(ach.Description);
                }

                else
                {
                    RPG.Dialogue("\n" + ach.Title, true, ConsoleColor.Red);
                }
            }

            Console.WriteLine("\n" + count + "/" + Achievement.achList.Count + " достижений получено.");

            Console.ReadKey();
        }
    }
}
