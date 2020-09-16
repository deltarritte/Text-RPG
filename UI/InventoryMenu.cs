using RPGTestC.Items;
using System;

namespace RPGTestC.UI
{
    public class InventoryMenu
    {
        static int index = 0;

        public static void ShowMenu()
        {
            Console.Clear();

            Console.WriteLine($"Текущее оружие: {Player.Inventory[0].GetName()} ({Player.Inventory[0].LVL} УР)" +
                $"\n{Player.Inventory[0].Description}" +
                $"\nУрон: {Player.Inventory[0].baseDamage} АТК");

            Console.WriteLine($"\nТекущая броня: {Player.Inventory[1].GetName()} ({Player.Inventory[1].LVL} УР)" +
                $"\n{Player.Inventory[1].Description}" +
                $"\nУрон: {Player.Inventory[1].Defence} ЗАЩ");

            Console.WriteLine($"\nТекущий предмет: {Player.Inventory[2].GetName()}" +
                $"\n{Player.Inventory[2].Description}");

            Console.WriteLine("\n");

            for(int i = 0; i < Player.Passive_Inventory.Length; i++)
            {
                if (i == index) Console.WriteLine($"[{Player.Passive_Inventory[i].GetName()}] - {Player.Passive_Inventory[i].Description}");
                else Console.WriteLine($" {Player.Passive_Inventory[i].GetName()} ");
            }

            Console.WriteLine("W,S - Выбрать предмет, E - Экипировать предмет, Q - Выйти");

            MenuControls();
        }

        static void MenuControls()
        {
            Item buffer = new Item();

            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.E:
                    buffer = Player.Passive_Inventory[index];

                    switch (Player.Passive_Inventory[index].IType)
                    {
                        case Item.Type.Armour:
                            Player.Passive_Inventory[index] = Player.Inventory[1];
                            Player.Inventory[1] = buffer;
                            break;

                        case Item.Type.Weapon:
                            Player.Passive_Inventory[index] = Player.Inventory[0];
                            Player.Inventory[0] = buffer;
                            break;

                        case Item.Type.Item:
                            Player.Passive_Inventory[index] = Player.Inventory[2];
                            Player.Inventory[2] = buffer;
                            break;

                        default:
                            RPG.Dialogue("Нельзя экипировать.");
                            break;
                    }
                    goto default;

                case ConsoleKey.Q:
                    break;

                case ConsoleKey.S:
                    if (index < Player.Passive_Inventory.Length - 1) index++;
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
