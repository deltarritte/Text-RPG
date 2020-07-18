using RPGTestC.Achievements;
using RPGTestCBuildA;
using System;
using static RPGTestC.Player;

namespace RPGTestC.Events
{
    public class Fight
    {
        #region Variables
        static bool boss;
        static int amount;
        static float bufDamage;                 // Буффер для значения урона монстра
        static int count;                       // Счётчик ходов
        static int effectCount;                 // Счётчик действия эффекта (в ходах)
        static float strikeChance = 0.95f;      // Шанс успешного попадания
        static int Crit;                        // Шанс критического урона
        static float RewardXP;
        static int RewardMoney;

        static Monster[] monsters;
        #endregion
        static void Reset()
        {
            count = 0;
            Defence = GetDefence();
            Damage = GetDamage();
            PStatus = Status.None;
        }

        static public void Init(int _amount = 1)                      // Инициализация схватки
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
                for (int i = 0; i < amount; i++)
                {
                    monsters[i].HP *= (1 - 1 / (2 * amount));
                    monsters[i].Damage *= (1 - 1 / (2 * amount));

                    RPG.Dialogue("Монстры!");
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

        static void MonsterFight()                              // Метод-цикл схватки
        {
            RPG.Fancies();                                              // Обновление элементов интерфейса
            Console.Clear();
            Console.WriteLine($"Ход {count}. Характеристики:");

            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine($"\n    Монстр {i+1} (УР: {monsters[i].LVL}):"
                                + $"\n HP {monsters[i].HP}"
                                + $"\nATK {monsters[i].Damage}");     // Вывести "интерфейс"
            }
            Console.WriteLine($"\nЗдоровье игрока: [{RPG.HPLine}] {HP}/{MaxHP} ({Defence} ЗАЩ). Статус: {GetStatusName()}");

            if (Remaining() != 0)                                            // Продолжение битвы
            {
                Console.WriteLine($"Что будешь делать? \nАтака - 1 ({Damage} АТК, {strikeChance * 100}% успех),"
                                + $"\nЛечение - 2 (Зарядов: {healCount})"
                                +  "\nПодсказка по типу - 3");                         // Вывести список возможных действий

                switch (Console.ReadLine())
                {
                    case "1":
                        
                        int i = 0;
                        string A;

                        if (amount != 1 && Remaining() > 1)
                        {
                            Console.WriteLine("Кому нанести удар?");
                            for (int j = 0; j < amount; j++) Console.WriteLine($"{j + 1} - {GetTypeName(monsters[j].Type)} {monsters[j].Name}");
                            A = Console.ReadLine();

                            if (!Int32.TryParse(A, out i)) goto default;
                            if (i < 1 || i > amount) goto default;

                            i -= 1;
                        }

                        else if (Remaining() == 1) for (int j = 0; j < amount; j++) if (monsters[i].HP > 0) i = j;

                        PlayerAttack(i);

                        if (monsters[i].isInvincible)
                        {
                            monsters[i].isInvincible = false;
                            monsters[i].Damage = bufDamage;
                        }

                        MonsterAttack();
                        break;

                    case "2":

                        if (HP < MaxHP)
                        {
                            if(healCount > 0)
                            {
                                HP = MaxHP;
                                healCount--;
                            }
                            else RPG.Dialogue("Нет зарядов.");
                        }
                        else RPG.Dialogue("Здоровье полное");

                        MonsterFight();
                        break;

                    case "3":
                        foreach(Monster mnstr in monsters) RPG.Dialogue("\n" + GetTypeTip(mnstr));
                        MonsterFight();
                        break;

                    default:
                        RPG.Dialogue("Некорректный ввод.");
                        MonsterFight();
                        break;
                }
            }
            else // Победа. Получение награды
            {
                if(amount == 1 && monsters[0].Type == Monster.MType.Explosive && count == 4)
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
                Reset();
                LvlUp();
            }
        }

        static void PlayerAttack(int index)                              // Метод для атаки игрока
        {
            Crit = RPG.rnd.Next(1, 10);
            double atksuc = RPG.rnd.NextDouble();

            if (!monsters[index].isInvincible && atksuc <= strikeChance)
            {
                if (Crit >= 8)
                {
                    monsters[index].HP -= Damage * critCoeff;
                    RPG.Dialogue($"Критический урон! Вы нанесли {Damage * critCoeff} урона.", true, ConsoleColor.Yellow);
                }

                else
                {
                    monsters[index].HP -= Damage;
                    RPG.Dialogue($"Вы нанесли {Damage} урона.", true, ConsoleColor.Green);
                }

                Console.WriteLine("У монстра осталось " + monsters[index].HP + " HP");
            }
            else Console.WriteLine("Монстр не получил урона!");
        }

        static void MonsterAttack()                             // Метод для атаки монстра
        {
            #region Apply Effect
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
            else
            {
                foreach(Monster mnstr in monsters) if (mnstr.effectCooldown != 0) mnstr.effectCooldown--;

                if (PStatus == Status.Frozen) Damage = GetDamage();
                else if (PStatus == Status.Blind) strikeChance = 0.95f;

                PStatus = Status.None;
            }
            #endregion

            for(int i = 0; i < amount; i++)
            {
                if (monsters[i].HP > 0)
                {
                    if (monsters[i].effectCooldown == 0) GetTypeEffect(monsters[i]);

                    #region Attack

                    GetTypeSpecAtk(monsters[i]);

                    if (monsters[i].Damage != 0) HP -= monsters[i].Damage - (float)Math.Round(Defence);
                    RPG.Dialogue($"{monsters[i].Name} {i+1} нанёс {monsters[i].Damage} ед. урона ({Math.Round(Defence)} ед. урона заблокировано)", true, ConsoleColor.Red);
                    
                    if (monsters[i].Type == Monster.MType.Luminous) monsters[i].Damage = bufDamage;

                    if (monsters[i].Type == Monster.MType.Thorned) Defence = GetDefence();
                    #endregion

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
                }
            }
            count++;
            Console.ReadKey();
            MonsterFight();
        }
        
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
                        Defence = 0;
                        RPG.Dialogue("Монстр выстрелил шип!", true, ConsoleColor.DarkRed);
                    }
                    break;

                case Monster.MType.Ice:
                    if (a <= 1f / 20)
                    {
                        monster.isInvincible = true;
                        bufDamage = monster.Damage;
                        monster.Damage = 0;

                        RPG.Dialogue("Монстр поставил ледяной щит.", true, ConsoleColor.Cyan);
                    }
                    break;

                case Monster.MType.Luminous:
                    if (a <= 3f / 10)
                    {
                        bufDamage = monster.Damage;
                        monster.Damage += monster.Damage / 5;

                        RPG.Dialogue("Монстр использовал линзу!", true, ConsoleColor.White);
                    }
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
                    if(a <= 1f / 4)
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
                    break;
                    
                case Monster.MType.Fire:
                    if(a <= 1f / 3)
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
                                Damage = (float)Math.Round((10 + Math.Pow(2, LVL)) * DamageCoeff);
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
                                Damage *= (1 - 0.03f * LVL);
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
                                Damage *= (1 - 0.03f * LVL);
                                RPG.Dialogue($"АТК снижена на {3 * LVL}%", true, ConsoleColor.Cyan);
                                break;
                        }
                    }
                    break;

                case Monster.MType.Dark:
                    if(a <= 3f / 20)
                    {
                        effectCount = 1;
                        monster.effectCooldown = 5;
                        strikeChance = 1f / 3;
                        PStatus = Status.Blind;
                        RPG.Dialogue($"Шанс попасть по монстру стал 33%");
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
