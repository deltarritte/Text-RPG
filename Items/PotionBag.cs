using System;
using RPGTestC.Events;

namespace RPGTestC.Items
{
    public class PotionBag : Item
    {
        public PotionBag()
        {
            ID = 0x010000;
            ItemType = Type.Item;
            Name = "Сумка с зельями";
            Description = "Сумка с 4-мя видами зелий. Зелье, взятое из сумки - случайное. Будьте осторожны.";
        }

        public override void OnUse(Monster[] monsters, int index = 0)
        {
            Random rnd = new Random();
            int potion = rnd.Next(1, 5);
            
            switch (potion)
            {
                // heal the player
                case 1:
                    float recovHP = 10;
                    if (Player.MaxHP - Player.HP > 10) Player.HP += 10;
                    else
                    {
                        recovHP = Player.MaxHP - Player.HP;
                        Player.HP = Player.MaxHP;
                    }
                    RPG.Dialogue($"Лечебное Зелье! (Восстановлено {recovHP})", true, ConsoleColor.Green);
                    break;

                // damage all monsters
                case 2:
                    foreach(Monster mnstr in monsters) mnstr.HP -= 12;
                    RPG.Dialogue("Взрывное Зелье! (Всем монстрам было нанесено 12 урона)", true, ConsoleColor.Green);
                    break;

                // damage a random monster
                case 3:
                    int idx = rnd.Next(0, monsters.Length - 1);
                    monsters[idx].HP -= 31 - 16 / (monsters[idx].LVL + 1);
                    RPG.Dialogue($"Необычно Конкретное Зелье! (Монстру {idx+1} было нанесено {31 - 16 / (monsters[idx].LVL + 1)})", true, ConsoleColor.Green);
                    break;

                // damage the player
                case 4:
                    Player.HP -= 62 - 60 / (Player.LVL + 1);
                    RPG.Dialogue($"Зелье Саботажа! (Вам было нанесено {62 - 60 / (Player.LVL + 1)} урона)", true, ConsoleColor.Red);
                    break;
            }
        }
    }
}
