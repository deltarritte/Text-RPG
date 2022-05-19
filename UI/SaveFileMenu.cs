using System;

namespace RPGTestC.UI
{
    public class SaveFileMenu
    {
        public static bool isLoading = false;
        static int index = 0;
        static readonly string newfile = "Создать новый файл";
        public static void ShowMenu()
        {
            Console.Clear();

            if (index == 0) RPG.Dialogue($"[{newfile}]", true);
            else RPG.Dialogue($"{newfile}", true);

            for (int i = 1; i < Player.subDirs.Length + 1; i++)
            {
                if (i == index) RPG.Dialogue($"[{Player.subDirs[i - 1]}]", true);
                else RPG.Dialogue($" {Player.subDirs[i - 1]} ", true);
            }

            Console.WriteLine("W,S - Переместить выделение, E - Выбрать пункт меню, Q - Выйти");

            MenuControls();
        }

        static void MenuControls()
        {
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.E:
                    if(index == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Выберите название для файла сохранения (Образец: saveFile)");
                        Player.savename = "Saves\\" + Console.ReadLine() + ".RPGSF" + RPG.VERSION;
                        Player.SaveProgress(false);
                    }
                    else
                    {
                        Player.savename = Player.subDirs[index - 1];
                        if (isLoading)
                        {
                            Player.LoadProgress(true);
                            isLoading = false;
                        }
                        else Player.SaveProgress(true);
                    }
                    goto case ConsoleKey.Q;

                case ConsoleKey.Q:
                    break;

                case ConsoleKey.S:
                    if (index < Player.subDirs.Length) index++;
                    goto default;

                case ConsoleKey.W:
                    if (index > 0) index--;
                    goto default;

                default:
                    ShowMenu();
                    break;
            }
        }
    }
}
