using RPGTestC.Items;
using RPGTestC.Items.Weapons;
using System;
using static RPGTestC.Player;

namespace RPGTestC.Events
{
    public class Fight
    {
        #region Variables
        static bool boss;
        //static bool retrig = false;
        static int amount;
        static int count;                       // Счётчик ходов
        static public int effectCount;                 // Счётчик действия эффекта (в ходах)
        static float strikeChance = 0.95f;      // Шанс успешного попадания
        static float strikeMultiplier = 1f;
        static float defenceMultiplier = 1f;
        static int Crit;                        // Шанс критического урона
        static float RewardXP;
        static int RewardMoney;
        static int ActionPoints = 100;
        static int ChainCost = 0;
        static RPG.ChainAction[] ActionChain;
        static int index = 0;

        static RPG.ChainAction Wait = new RPG.ChainAction();
        static RPG.ChainAction Attack = new RPG.ChainAction();
        static RPG.ChainAction Defend = new RPG.ChainAction();
        static RPG.ChainAction Spellcast = new RPG.ChainAction();
        static void SetupActions()
        {
            Wait.name = 'W';
            Wait.cost = 0;
            Wait.weight = 0;
            Wait.num = 0;

            Attack.name = 'A';
            Attack.cost = 25;
            //Attack.weight = 0.5f;
            Attack.num = 1;

            Spellcast.name = 'S';
            Spellcast.cost = 30;
            //Spellcast.weight = 0.4f;
            Spellcast.num = 2;

            Defend.name = 'D';
            Defend.cost = 20;
            //Defend.weight = 0.1f;
            Defend.num = 3;
        }

        static Monster[] monsters;
        #endregion
        static void Reset()
        {
            count = 0;
            //Defence = GetDefence();
            //Damage = GetDamage();
            PStatus = Status.None;
            SetupActions();
            index = 0;
            ChainCost = 0;
            ActionChain = new RPG.ChainAction[5] { Wait, Wait, Wait, Wait, Wait };
        }

        // Инициализация схватки
        static public void Init(int _amount = 1)                      
        {
            Reset();

            amount = _amount;
            monsters = new Monster[amount];
            for (int i = 0; i < amount; i++)
            {
                if (i == 0) monsters[i] = new Monster
                {
                    Type = (Monster.MType)RPG.rnd.Next(0, 7),
                };

                else monsters[i] = new Monster(LVL - 1)
                {
                    Type = (Monster.MType)RPG.rnd.Next(0, 7),
                };
            }

            if (amount != 1)
            {
                RPG.Dialogue("Монстры!");
                for (int i = 0; i < amount; i++)
                {
                    monsters[i].HP *= (1 - 1 / (2 * amount));
                    monsters[i].Damage *= (1 - 1 / (2 * amount));

                    RPG.Dialogue($"\n{i}: {GetTypeName(monsters[i].Type)} {monsters[i].Name} (УР: {monsters[i].LVL})!"
                                + "\nУ него: " + monsters[i].HP + " HP"
                                + "\nи " + monsters[i].Damage + " Урона",
                                false, ConsoleColor.Red);
                }

            }
            else RPG.Dialogue(GetTypeName(monsters[0].Type) + $" {monsters[0].Name}!"
                        + "\nУ него: " + monsters[0].HP + " HP"
                        + "\nи " + monsters[0].Damage + " Урона",
                 false, ConsoleColor.Red);                       // Вывод строки красного цвета о начале схватки
                
                
            MonsterFight();                                     // Начать цикл схватки
        }
        static public void Init(Monster[] _monsters, bool _boss = false)
        {
            Reset();

            boss = _boss;
            monsters = _monsters;
            amount = monsters.Length;

            if (amount == 1)
                RPG.Dialogue($"{GetTypeName(monsters[0].Type)} {monsters[0].Name}!"
                        + $"\nУ него: {monsters[0].HP} HP"
                        + $"\nи {monsters[0].Damage} Урона.",
                 false, ConsoleColor.Red);                       // Вывод строки красного цвета о начале схватки
            else
            {
                RPG.Dialogue("Монстры!");
                for (int i = 0; i < amount; i++)
                {
                    RPG.Dialogue($"\n{i}: {GetTypeName(monsters[i].Type)} {monsters[i].Name}!"
                                + $"\nУ него: {monsters[i].HP} HP"
                                + $"\nи {monsters[i].Damage} Урона.",
                                false, ConsoleColor.Red);
                }
            }
            MonsterFight();
        }
        static void MonsterFight()
        {
            if(Remaining() == 0)
            {
                if (amount == 1 && monsters[0].Type == Monster.MType.Explosive && count == 4)
                {
                    RewardXP = 0;
                    RewardMoney = 0;
                }
                else
                {
                    if (boss)
                    {
                        RewardXP = 100 + (float)Math.Pow(2, LVL) * 3;
                        RewardMoney = 50 + (int)Math.Round(Math.Pow(2, LVL / 2)) * 5;
                    }
                    else
                    {
                        RewardXP = (RPG.rnd.Next(1, 25) + 5 * (float)Math.Pow(2, LVL / 2)) * amount;
                        RewardMoney = (RPG.rnd.Next(1, 5) + 5 * (int)Math.Round(Math.Pow(2, LVL / 4))) * amount;
                    }
                }

                RPG.Dialogue("Вы получили " + RewardXP + " ед. опыта и " + RewardMoney + " ед. монет.", false, ConsoleColor.Yellow);
                XP += RewardXP;
                Money += RewardMoney;

                if (LVL == 15) RPG.Dialogue("Полученный опыт был превращён в " + (int)Math.Round((XP - MaxXP) / 10) + " очков мастерства", false, ConsoleColor.Yellow);
                LvlUp();
                SaveProgress(true);
                Console.ReadKey();
            }
            else
            {
                // Снятие дебаффов с игрока
                foreach (Monster mnstr in monsters) if (mnstr.effectCooldown != 0) mnstr.effectCooldown--;

                if (effectCount == 0)
                {
                    if (PStatus == Status.Frozen) Weapon.Damage = Weapon.baseDamage;
                    else if (PStatus == Status.Blind) strikeChance = 0.95f;

                    PStatus = Status.None;
                }

                for (int i = 0; i < amount; i++) monsters[i].AI();

                index = 0;
                PlayerTurn();
            }
        }
        static void PlayerTurn()
        {
            Console.Clear();
            RPG.Fancies();                                              // Обновление элементов интерфейса
            Console.WriteLine($"Ход {count}. Характеристики:");
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine($"\n    {monsters[i].Name} {i + 1} (УР: {monsters[i].LVL}, {monsters[i].GetAState()}):"
                                + $"\n HP {monsters[i].HP}"
                                + $"\nATK {monsters[i].Damage}"
                                + $"\nЦепь: {monsters[i].GetActionChain()}");     // Вывести "интерфейс"
            }

            Console.WriteLine($"\nЗдоровье игрока: [{RPG.HPLine}] {HP}/{MaxHP} ({Inventory[1].Defence} ЗАЩ). Статус: {GetStatusName()}");

            // Вывод списка возможных действий
            Console.Write($"\nТекущая цепь: -");

            for (int i = 0; i < ActionChain.Length; i++)
            {
                if(i == index) Console.Write($"[{ActionChain[i].name}]-");
                else Console.Write($"{ActionChain[i].name}-");
            }

            Console.WriteLine($"\nОчки действий: {ChainCost}/{ActionPoints}"
                            + $"\nАтаковать ({Attack.cost}): A"
                            + $"\nИсп. предмет ({Spellcast.cost}): S"
                            + $"\nЗащититься и/или вылечиться ({Defend.cost}, Зарядов: {healCount}/{maxHealCount}): D"
                            + $"\nИспользовать текущую цепь: 1"
                            + $"\nСменить звено: 2, 3"
                            + $"\nОчистить цепь: 4");

            // Приём ввода
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    ChainClash();
                    break;
                case ConsoleKey.NumPad2:
                case ConsoleKey.D2:
                    if (index > 0) index--;
                    goto default;
                case ConsoleKey.NumPad3:
                case ConsoleKey.D3:
                    if (index < ActionChain.Length - 1) index++;
                    goto default;
                case ConsoleKey.NumPad4:
                case ConsoleKey.D4:
                    index = 0;
                    ChainCost = 0;
                    ActionChain = new RPG.ChainAction[5] { Wait, Wait, Wait, Wait, Wait };
                    goto default;

                case ConsoleKey.W:
                    ChainCost -= ActionChain[index].cost;
                    ActionChain[index] = Wait;
                    ChainCost += ActionChain[index].cost;
                    if (index < ActionChain.Length - 1) index++;
                    goto default;
                case ConsoleKey.A:
                    if (ChainCost - ActionChain[index].cost + Attack.cost <= ActionPoints)
                    {
                        ChainCost -= ActionChain[index].cost;
                        ActionChain[index] = Attack;
                        ChainCost += ActionChain[index].cost;
                        if (index < ActionChain.Length - 1) index++;
                    }
                    goto default;
                case ConsoleKey.S:
                    if (ChainCost - ActionChain[index].cost + Spellcast.cost <= ActionPoints)
                    {
                        ChainCost -= ActionChain[index].cost;
                        ActionChain[index] = Spellcast;
                        ChainCost += ActionChain[index].cost;
                        if (index < ActionChain.Length - 1) index++;
                    }
                    goto default;
                case ConsoleKey.D:
                    if (ChainCost - ActionChain[index].cost + Defend.cost <= ActionPoints)
                    {
                        ChainCost -= ActionChain[index].cost;
                        ActionChain[index] = Defend;
                        ChainCost += ActionChain[index].cost;
                        if (index < ActionChain.Length - 1) index++;
                    }
                    goto default;

                default:
                    PlayerTurn();
                    break;
            }
        }
        static void ChainClash()
        {
            bool defending = false;
            bool success = false;
            for (int i = 0; i < monsters.Length; i++)
            {
                for (int j = 0; j < ActionChain.Length; j++)
                {
                    // 0 - W, 1 - A, 2 - S, 3 - D
                    switch (ActionChain[j].num)
                    {
                        case 1:
                            Crit = RPG.rnd.Next(1, 10);
                            double atksuc = RPG.rnd.NextDouble();

                            if (monsters[i].ActionChain[j].num == 3) atksuc -= 0.1f;

                            if (!monsters[0].isInvincible && atksuc <= strikeChance)
                            {
                                success = true;
                                Inventory[0].OnUse(monsters, 0);
                                if (Crit >= 8)
                                {
                                    monsters[0].HP -= Weapon.Damage * critCoeff * strikeMultiplier;
                                    RPG.Dialogue($"Критический урон! Вы нанесли {Weapon.Damage * critCoeff * strikeMultiplier} урона.", true, ConsoleColor.Yellow);
                                }
                                else
                                {
                                    monsters[0].HP -= Weapon.Damage * strikeMultiplier;
                                    RPG.Dialogue($"Вы нанесли {Weapon.Damage * strikeMultiplier} урона.", true, ConsoleColor.Green);
                                }

                                if (monsters[0].HP < 0) monsters[0].HP = 0;
                                Console.WriteLine("У монстра осталось " + monsters[0].HP + " HP");
                            }
                            else Console.WriteLine("Монстр не получил урона!");

                            break;

                        case 2:
                            Special.OnUse(monsters);
                            break;

                        case 3:
                            defenceMultiplier += 2f;
                            if (healCount > 0) HP = MaxHP;
                            defending = true;
                            break;

                        default:
                            break;
                    }
                    switch (monsters[i].ActionChain[j].num)
                    {
                        case 1:
                            if (monsters[i].HP <= 0) break;

                            Inventory[1].OnUse(monsters, i);

                            if (monsters[i].Damage - Inventory[1].Defence * defenceMultiplier > 0)
                            {
                                HP -= monsters[i].Damage - Inventory[1].Defence * defenceMultiplier;
                                RPG.Dialogue($"{monsters[i].Name} {i + 1} нанёс {monsters[i].Damage} ед. урона ({Inventory[1].Defence * defenceMultiplier} ед. урона заблокировано)", true, ConsoleColor.Red);
                            }
                            else RPG.Dialogue($"{monsters[i].Name} {i + 1} не нанёс  урона.", true, ConsoleColor.Red);

                            if (HP <= 0)
                            {
                                PStatus = Status.None;

                                Console.WriteLine("Вы умерли." +
                                    "\nЗагрузить файл сохранения? 1 - Да; Любая клавиша - Выход");

                                var C = Console.ReadLine();

                                switch (C)
                                {
                                    case "1":
                                        LoadProgress(false);
                                        RPG.GetRandomEvent();
                                        break;

                                    default:
                                        Environment.Exit(0);
                                        break;
                                }
                            }
                            break;

                        case 2:
                            GetTypeEffect(monsters[i]);
                            GetTypeSpecAtk(monsters[i]);
                            break;

                        case 3:
                            if (!success) RPG.Dialogue($"Противник увернулся от атаки.", true);
                            else RPG.Dialogue($"Противник попытался увернуться от атаки.", true);
                            break;
                        default:
                            break;
                    } // Switch
                } // for ActionChain

                if (effectCount != 0)
                {
                    effectCount--;
                    switch (PStatus)
                    {
                        case Status.Poisoned:
                            HP -= 2 * (LVL + 1);
                            RPG.Dialogue($"Было снято {2 * (LVL + 1)} ХП от яда.", true, ConsoleColor.DarkGreen);
                            break;

                        case Status.OnFire:
                            HP -= 3 * (LVL + 1);
                            RPG.Dialogue($"Было снято {3 * (LVL + 1)} ХП от огня.", true, ConsoleColor.DarkRed);
                            break;

                        default:
                            break;
                    }
                }

                if (monsters[i].Type == Monster.MType.Luminous || Inventory[1].GetType().Equals(new Yang_W())) monsters[i].ResetDamage();

                else if (monsters[i].Type == Monster.MType.Thorned || defending) defenceMultiplier = 1f;

                else if (monsters[i].Type == Monster.MType.Ice && monsters[i].isInvincible)
                {
                    monsters[i].isInvincible = false;
                    monsters[i].ResetDamage();
                }
            } // for monsters
            count++;
            Console.ReadKey();
            MonsterFight();
        } // ChainClash
        
        #region Functions
        private static void GetTypeSpecAtk(Monster monster)    // Получить специальную атаку типа монстра
        {
            double a = RPG.rnd.NextDouble();

            switch (monster.Type)
            {
                case Monster.MType.Explosive:
                    if (count == 4)
                    {
                        monster.Damage = (float)Math.Pow(2, LVL) / 1.5f;
                        monster.HP = 0;

                        RPG.Dialogue("Монстр подорвался.", true, ConsoleColor.DarkRed);
                    }
                break;

                case Monster.MType.Thorned:
                    if (a <= 1f / 4)
                    {
                        defenceMultiplier = 0f;
                        RPG.Dialogue("Монстр выстрелил шип!", true, ConsoleColor.DarkRed);
                    }
                    break;

                case Monster.MType.Ice:
                    if (a <= 1f / 20)
                    {
                        monster.isInvincible = true;
                        monster.Damage = 0;

                        RPG.Dialogue("Монстр поставил ледяной щит.", true, ConsoleColor.Cyan);
                    }
                    break;

                case Monster.MType.Luminous:
                    if (a <= 3f / 10)
                    {
                        monster.Damage += monster.Damage / 5;

                        RPG.Dialogue("Монстр использовал линзу!", true, ConsoleColor.White);
                    }
                    break;
                default:
                    break;
            }
        }

        static string GetStatusName()           // Получить наименование статуса игрока в типе string
        {
            switch (PStatus)
            {
                case Status.None:
                    return "Нет Эффектов";

                case Status.Poisoned:
                    return "Отравлен";

                case Status.OnFire:
                    return "В Огне";

                case Status.Frozen:
                    return "Заморожен";

                case Status.Blind:
                    return "Ослеплён";

                default:
                    return "???";
            }
        }

        static string GetTypeName(Monster.MType type)             // Получить наименование типа монстра в типе string
        {
            switch (type)
            {
                case Monster.MType.Standart:
                    return "Обычный";

                case Monster.MType.Poisonous:
                    return "Ядовитый";

                case Monster.MType.Explosive:
                    return "Взрывной";

                case Monster.MType.Thorned:
                    return "Шипастый";

                case Monster.MType.Fire:
                    return "Огненный";

                case Monster.MType.Ice:
                    return "Ледяной";

                case Monster.MType.Dark:
                    return "Тёмный";

                case Monster.MType.Luminous:
                    return "Светящийся";

                default:
                    return null;
            }
        }

        static string GetTypeTip(Monster monster)              // Получить подсказку по типу
        {
            switch (monster.Type)
            {
                case Monster.MType.Standart:
                    return "Стандартный тип.\nБез особых атак.\nНет эффектов.";

                case Monster.MType.Poisonous:
                    return $"Ядовитый тип.\nБез особых атак.\nОтравление (Шанс 25%): Снимает {2 * (LVL + 1)} ХП в ход в течение 2-х ходов.";

                case Monster.MType.Explosive:
                    return $"Взрывной тип.\nОсобая атака: Подрыв - активируется на 4-ый ход. Убивает монстра. Наносит {Math.Pow(2, LVL) / 2} урона.\nНет эффектов.";

                case Monster.MType.Thorned:
                    return $"Шипастый тип.\nОсобая атака: Выпускает шип, игнорирующий защиту игрока.\nНет эффектов.";

                case Monster.MType.Fire:
                    return $"Огненный тип.\nБез особых атак.\nПоджог (Шанс 33%): Снимает {3 * (LVL + 1)} ХП в ход в течение 2-х ходов.";

                case Monster.MType.Ice:
                    return $"Ледяной тип.\nЛедяной щит (Шанс 5%): Монстр покрывается ледяной оболочкой, становясь неуязвимым на следующий ход.\nЗаморозка (Шанс 15%): Снижает АТК на {3 * (LVL + 1)}%.";

                case Monster.MType.Dark:
                    return "Тёмный тип.\nБез особых атак.\nЗабвение (Шанс 15%): Шанс удачно ударить монстра снижается до 33%.";

                case Monster.MType.Luminous:
                    return $"Светящийся тип.\nЛинза: Дополнительно наносит {monster.Damage / 5} ед. урона.\nНет эффектов.";

                default:
                    return null;
            }
        }

        static void GetTypeEffect(Monster monster)             // Получить эффект атаки типа монстра
        {
            double a = RPG.rnd.NextDouble();

            switch (monster.Type)
            {
                case Monster.MType.Poisonous:
                    if (a <= 1f / 4)
                    {
                        switch (PStatus)
                        {
                            case Status.OnFire:
                                RPG.Dialogue("Монстр попытался вас отравить, но яд разложился от высокой температуры.", true, ConsoleColor.DarkGreen);
                                break;

                            case Status.Frozen:
                                RPG.Dialogue("Монстр попытался вас отравить, но яд замёрз и стал неэффективен.", true, ConsoleColor.DarkGreen);
                                break;

                            default:
                                effectCount = 2;
                                monster.effectCooldown = 3;
                                PStatus = Status.Poisoned;
                                RPG.Dialogue($"Вы отравлены.", true, ConsoleColor.DarkGreen);
                                break;
                        }
                    }
                    else RPG.Dialogue("Монстр попытался вас отравить.", true, ConsoleColor.DarkGreen);
                    break;
                    
                case Monster.MType.Fire:
                    if (a <= 1f / 3)
                    {
                        switch (PStatus)
                        {
                            case Status.Poisoned:
                                effectCount = 2;
                                monster.effectCooldown = 3;
                                PStatus = Status.OnFire;
                                RPG.Dialogue("Монстр вас поджог и разложил яд. Но теперь вы в огне.", true, ConsoleColor.DarkRed);
                                break;

                            case Status.Frozen:
                                effectCount = 0;
                                PStatus = 0;
                                strikeMultiplier = 1f;
                                Console.WriteLine("В попытке поджога, монстр вас разморозил.", true, ConsoleColor.DarkRed);
                                break;

                            default:
                                effectCount = 2;
                                monster.effectCooldown = 3;
                                PStatus = Status.OnFire;
                                RPG.Dialogue($"Вы в огне.", true, ConsoleColor.DarkRed);
                                break;
                        }
                    }
                    else RPG.Dialogue($"Монстр попытался вас поджечь.", true, ConsoleColor.DarkRed);
                    break;

                case Monster.MType.Ice:
                    if (a <= 3f / 20)
                    {
                        switch (PStatus)
                        {
                            case Status.Poisoned:
                                PStatus = Status.Frozen;
                                effectCount = 2;
                                monster.effectCooldown = 4;
                                strikeMultiplier -= 0.03f * LVL;
                                RPG.Dialogue($"Монстр вас заморозил, сделав яд неэффективным. Но АТК снижена на {3 * LVL}%", true, ConsoleColor.Cyan);
                                break;

                            case Status.OnFire:
                                effectCount = 0;
                                PStatus = 0;
                                RPG.Dialogue("В попытке вас заморозить, монстр вас потушил.", true, ConsoleColor.Cyan);
                                break;

                            default:
                                PStatus = Status.Frozen;
                                effectCount = 2;
                                monster.effectCooldown = 4;
                                strikeMultiplier -= 0.03f * LVL;
                                RPG.Dialogue($"АТК снижена на {3 * LVL}%", true, ConsoleColor.Cyan);
                                break;
                        }
                    }
                    else RPG.Dialogue($"Монстр попытался вас заморозить.", true, ConsoleColor.Cyan);
                    break;

                case Monster.MType.Dark:
                    if(a <= 3f / 20)
                    {
                        effectCount = 1;
                        monster.effectCooldown = 5;
                        strikeChance = 1f / 3;
                        PStatus = Status.Blind;
                        RPG.Dialogue($"Монстр скрылся в тенях. Шанс попасть по монстру стал 33%");
                    }
                    break;

                default:
                    break;
            }
        }

        static int Remaining()
        {
            int Alive = 0;
            foreach(Monster mnstr in monsters)
            {
                if (mnstr.HP > 0) Alive++;
            }
            return Alive;
        }
        #endregion
    }
}
