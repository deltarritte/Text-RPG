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

            Console.WriteLine($"Текущее оружие: {Player.Weapon.GetName()} ({Player.Weapon.LVL} УР)" +
                $"\n{Player.Weapon.Description}" +
                $"\nУрон: {Player.Weapon.baseDamage} АТК");

            Console.WriteLine($"\nТекущая броня: {Player.Armour.GetName()} ({Player.Armour.LVL} УР)" +
                $"\n{Player.Armour.Description}" +
                $"\nЗащита: {Player.Armour.Defence} ЗАЩ");

            Console.WriteLine($"\nТекущий предмет: {Player.Special.GetName()}" +
                $"\n{Player.Special.Description}");

            Console.WriteLine("\n");

            for(int i = 0; i < Player.Passive_Inventory.Length; i++)
            {
                if (i == index) RPG.Dialogue($"[{Player.Passive_Inventory[i].GetName()}] - {Player.Passive_Inventory[i].Description} (УР: {Player.Passive_Inventory[i].LVL})", 
                                            true, Player.Passive_Inventory[i].Colour);
                else RPG.Dialogue($" {Player.Passive_Inventory[i].GetName()} ", true, Player.Passive_Inventory[i].Colour);
            }

            Console.WriteLine("W,S - Выбрать предмет, E - Экипировать предмет, Q - Выйти");

            MenuControls();
        }

        static void MenuControls()
        {
            Item buffer;

            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.E:
                    buffer = Player.Passive_Inventory[index];

                    switch (Player.Passive_Inventory[index].ItemType)
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
