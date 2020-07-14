using RPGTestC.Achievements;
using RPGTestCBuildA;
using System;

namespace RPGTestC.Events
{
    public class Fight
    {
        #region Variables

        public enum MonsterType                 // Тип перечисления типов монстра
        {
            Standart,
            Poisonous,
            Explosive,
            Thorned,
            Fire,
            Ice,
            Dark,
            Luminous
        }

        public enum Status                      // Тип перечисления статусов игрока
        {
            None,
            Poisoned,
            OnFire,
            Frozen,
            Blind
        }

        public static MonsterType mnstrType;    // Переменная типа монстра

        static bool _boss;              // Местная переменная

        static int Crit;                        // Шанс критического урона
        const float critCoeff = 1.75f;          // Коэффициент критического урона
        static bool shieldOn;                   // Вооружён ли щит
        static public float playerAttack;       // Значение атаки игрока
        static public double playerDefence;     // Значение защиты игрока
        static float playerShield;              // Доп. защита от щита
        public static Status playerStatus;      // Переменная статуса игрока
        static float strikeChance = 0.95f;      // Шанс успешного попадания
        static public int healCount = 3;        // Кол-во зарядов
        static public int maxHealCount = 3;     // Макс. кол-во зарядов

        static public string Name;              // Имя монстра
        static public float MnstrHP;            // Ед. здоровья монстра
        static public float MnstrAttack;        // Ед. урона монстра
        static float bufMnstrAtk;               // Буффер для значения урона монстра
        static bool isInvincible = false;       // Неуязвимость для монстра

        static int count;                       // Счётчик ходов
        static int effectCount;                 // Счётчик действия эффекта (в ходах)

        #endregion

        static public void Init(bool boss)                      // Инициализация схватки
        {
            isInvincible = false;                               // Обновление переменных
            count = 0;                                          // Для новой
            playerShield = (float)Math.Pow(2, RPG.lvl) / 2;     // Схватки
            playerDefence = (2 * RPG.lvl - 2) * RPG.armorDefenceCoeff;
            playerAttack = (float)Math.Round((10 + Math.Pow(2, RPG.lvl)) * RPG.weaponDamageCoeff);

            _boss = boss;                                       // Назначение местной переменной

            if (!_boss)                                         // Если схватка не с боссом, то дать переменным случайные значения
            {
                Name = "Монстр";
                MnstrHP = (float)(30 + RPG.rnd.Next(1, 10) + Math.Pow(2, RPG.lvl) * 1.5);
                MnstrAttack = (float)Math.Round((10 + Math.Pow(2, RPG.lvl)) * RPG.rnd.Next(3, 6) / 4);
                mnstrType = (MonsterType)RPG.rnd.Next(0, 7);
            }

            bufMnstrAtk = MnstrAttack;                          // Назначение буффера значения атаки монстра

            RPG.Dialogue(GetTypeName() + $" {Name}!"
                + "\nУ него: " + MnstrHP + " HP"
                + "\nи " + MnstrAttack + " Урона", 
                false, ConsoleColor.Red);                       // Вывод строки красного цвета о начале схватки

            playerStatus = Status.None;                         // Обновление переменной о статусе игрока

            MonsterFight();                                     // Начать цикл схватки
        }

        static void MonsterFight()                              // Метод-цикл схватки
        {
            RPG.Fancies();                                              // Обновление интерфейса

            Console.Clear();

            if (_boss && MnstrHP > 0) Console.WriteLine(GetTypeName() + " " + Name + " смотрит на вас грозно"); // Вывести данную строку, если схватка с боссом

            Console.WriteLine($"Ход {count}. Характеристики:"
                + $"\n    Монстр:             Игрок: {GetStatusName()}"
                + $"\n HP {MnstrHP}           {RPG.playerHP} [{RPG.HPLine}]"
                + $"\nATK {MnstrAttack}           {playerAttack}");     // Вывести "интерфейс"

            if (MnstrHP > 0)                                            // Продолжение битвы
            {
                Console.WriteLine($"Что будешь делать? \nАтака - 1 ({strikeChance*100}% успех)," +
                    "\nЛечение - 2 (Зарядов: " + healCount + ")" +
                    "\nПоставить щит - 3 (" + playerShield / 2 + " ед. защиты)" +
                    "\nПодсказка по типу - 4");                         // Вывести список возможных действий

                switch (Console.ReadLine())
                {
                    case "1":

                        PlayerAttack();

                        if (isInvincible)
                        {
                            isInvincible = false;
                            MnstrAttack = bufMnstrAtk;
                        }

                        MonsterAttack();
                        break;

                    case "2":

                        if (RPG.playerHP < RPG.playerMaxHP)
                        {
                            if(healCount > 0)
                            {
                                if (shieldOn)
                                {
                                    RPG.playerHP = RPG.playerMaxHP + playerShield;
                                }

                                else
                                {
                                    RPG.playerHP = RPG.playerMaxHP;
                                }

                                healCount--;
                            }
                            else Console.WriteLine("Нет зарядов.");
                        }

                        else
                        {
                            Console.WriteLine("Здоровье полное");
                        }
                        MonsterFight();
                        break;

                    case "3":

                        if (shieldOn)
                        {
                            Console.WriteLine("Щит уже поставлен.");
                            MonsterFight();
                        }

                        else
                        {
                            shieldOn = true;

                            RPG.playerHP += playerShield;
                            playerDefence += playerShield / 2;

                            MonsterAttack();
                        }
                        break;

                    case "4":

                        RPG.Dialogue(GetTypeTip());
                        MonsterFight();

                        break;

                    default:
                        RPG.Dialogue("Нет команды.");
                        MonsterFight();
                        break;
                }
            }

            else // Победа. Получение награды
            {
                playerStatus = Status.None;

                if(mnstrType == MonsterType.Explosive && count == 4)
                {
                    RPG.XP = 0;
                    RPG.Money = 0;
                }

                else
                {
                    if (_boss)
                    {
                        RPG.XP = 100 + (float)Math.Pow(2, RPG.lvl) * 3;
                        RPG.Money = 50 + (int)Math.Round(Math.Pow(2, RPG.lvl / 2)) * 5;
                    }

                    else
                    {
                        RPG.XP = RPG.rnd.Next(1, 25) + 5 * (float)Math.Pow(2, RPG.lvl / 2);
                        RPG.Money = RPG.rnd.Next(1, 5) + 5 * (int)Math.Round(Math.Pow(2, RPG.lvl / 4));
                    }
                }

                RPG.Dialogue("Вы получили " + RPG.XP + " ед. опыта и " + RPG.Money + " ед. монет.", false, ConsoleColor.Yellow);
                RPG.playerXP += RPG.XP;
                RPG.playerMoney += RPG.Money;

                if (RPG.lvl == 15)
                {
                    RPG.Dialogue("Полученный опыт был превращён в " + (int)Math.Round((RPG.playerXP - RPG.lvlUpXP) / 10) + " очков мастерства", false, ConsoleColor.Yellow);
                }

                PlayerlvlUp();
            }
        }

        static void PlayerAttack()                              // Метод для атаки игрока
        {
            Crit = RPG.rnd.Next(1, 10);
            double atksuc = RPG.rnd.NextDouble();

            if (!isInvincible && atksuc <= strikeChance)
            {
                if (Crit >= 8)
                {
                    MnstrHP -= playerAttack * critCoeff;
                    RPG.Dialogue("Критический урон! Вы нанесли " + playerAttack * critCoeff + " урона.", true, ConsoleColor.Yellow);
                }

                else
                {
                    MnstrHP -= playerAttack;
                    RPG.Dialogue("Вы нанесли " + playerAttack + " урона.", true, ConsoleColor.Green);
                }

                Console.WriteLine("У монстра осталось " + MnstrHP + " HP");
            }
            else
            {
                Console.WriteLine("Монстр не получил урона!");
            }
        }

        static void MonsterAttack()                             // Метод для атаки монстра
        {
            if(MnstrHP > 0)
            {
                #region Apply Effect

                GetTypeEffect();

                if (effectCount != 0)
                {
                    effectCount--;

                    switch (playerStatus)
                    {
                        case Status.Poisoned:
                            RPG.playerHP -= 2 * RPG.lvl;
                            RPG.Dialogue($"Было снято {2 * RPG.lvl} ХП от яда.", true, ConsoleColor.DarkGreen);
                            break;

                        case Status.OnFire:
                            RPG.playerHP -= 3 * RPG.lvl;
                            RPG.Dialogue($"Было снято {3 * RPG.lvl} ХП от огня.", true, ConsoleColor.DarkRed);
                            break;

                        default:
                            break;
                    }
                }

                else
                {
                    if (playerStatus == Status.Frozen) playerAttack = (float)Math.Round((10 + Math.Pow(2, RPG.lvl)) * RPG.weaponDamageCoeff);
                    else if (playerStatus == Status.Blind) strikeChance = 0.95f;

                    playerStatus = Status.None;
                }

                #endregion

                #region Attack

                GetTypeSpecAtk();

                if (MnstrAttack != 0) RPG.playerHP -= MnstrAttack - (float)Math.Round(playerDefence / 2);
                RPG.Dialogue("Вам нанесли " + MnstrAttack + " ед. урона (" + Math.Round(playerDefence / 2) + " ед. урона заблокировано)", false, ConsoleColor.Red);

                if (shieldOn)
                {
                    RPG.playerHP -= playerShield;
                    playerDefence -= playerShield / 2;
                    shieldOn = false;
                }

                if (mnstrType == MonsterType.Luminous || mnstrType == MonsterType.Thorned) MnstrAttack = bufMnstrAtk;

                playerDefence = (2 * RPG.lvl - 2) * RPG.armorDefenceCoeff;
                #endregion

                if (RPG.playerHP <= 0)
                {
                    playerStatus = Status.None;

                    Console.WriteLine("Вы умерли." +
                        "\nЗагрузить файл сохранения? 1 - Да, 2 - Выход");

                    var C = Console.ReadLine();

                    switch (C)
                    {
                        case "1":
                            RPG.LoadProgress(false);
                            RPG.GetRandomEvent();
                            break;

                        case "2":
                            Environment.Exit(0);
                            break;
                    }
                }
                
                else count++;
            }

            MonsterFight();
        }
        
        static public void PlayerlvlUp(bool cheat = false)
        {
            while ((RPG.playerXP >= RPG.lvlUpXP || cheat) && RPG.lvl <= 14)
            {
                RPG.lvl++;
                RPG.lvlUpXP = 25 * (float)Math.Pow(2, RPG.lvl - 1);

                RPG.playerMaxHP = 50 + (float)Math.Pow(2, RPG.lvl);

                cheat = false;
            }

            if (RPG.lvl == 15)
            {
                RPG.lvlUpXP = 204800;
                RPG.masteryPoints += (int)Math.Round((RPG.playerXP - RPG.lvlUpXP) / 10);
                RPG.playerXP = RPG.lvlUpXP;
            }
            Console.Clear();
        }
        
        #region Functions
        private static void GetTypeSpecAtk()    // Получить специальную атаку типа монстра
        {
            double a = RPG.rnd.NextDouble();

            switch (mnstrType)
            {
                case MonsterType.Explosive:
                    if (count == 4)
                    {
                        MnstrAttack = (float)Math.Pow(2, RPG.lvl) / 1.5f;
                        MnstrHP = 0;

                        RPG.Dialogue("Монстр подорвался.", true, ConsoleColor.DarkRed);
                    }
                break;

                case MonsterType.Thorned:
                    if (a <= 1f / 4)
                    {
                        if (shieldOn) MnstrAttack += 0.3f * playerShield;
                        playerDefence = 0;

                        RPG.Dialogue("Монстр выстрелил шип!", true, ConsoleColor.DarkRed);
                    }
                    break;

                case MonsterType.Ice:
                    if (a <= 1f / 20)
                    {
                        isInvincible = true;
                        MnstrAttack = 0;

                        RPG.Dialogue("Монстр поставил ледяной щит.", true, ConsoleColor.Cyan);
                    }
                    break;

                case MonsterType.Luminous:
                    if (a <= 3f / 10)
                    {
                        MnstrAttack += MnstrAttack / 5;

                        RPG.Dialogue("Монстр использовал линзу!", true, ConsoleColor.White);
                    }
                    break;
            }
        }

        static string GetStatusName()           // Получить наименование статуса игрока в типе string
        {
            switch (playerStatus)
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

        static string GetTypeName()             // Получить наименование типа монстра в типе string
        {
            switch (mnstrType)
            {
                case MonsterType.Standart:
                    return "Обычный";

                case MonsterType.Poisonous:
                    return "Ядовитый";

                case MonsterType.Explosive:
                    return "Взрывной";

                case MonsterType.Thorned:
                    return "Шипастый";

                case MonsterType.Fire:
                    return "Огненный";

                case MonsterType.Ice:
                    return "Ледяной";

                case MonsterType.Dark:
                    return "Тёмный";

                case MonsterType.Luminous:
                    return "Светящийся";

                default:
                    return null;
            }
        }

        static string GetTypeTip()              // Получить подсказку по типу
        {
            switch (mnstrType)
            {
                case MonsterType.Standart:
                    return "Стандартный тип.\nБез особых атак.\nНет эффектов.";

                case MonsterType.Poisonous:
                    return $"Ядовитый тип.\nБез особых атак.\nОтравление (Шанс 25%): Снимает {2 * RPG.lvl} ХП в ход в течение 2-х ходов.";

                case MonsterType.Explosive:
                    return $"Взрывной тип.\nОсобая атака: Подрыв - активируется на 4-ый ход. Убивает монстра. Наносит {Math.Pow(2, RPG.lvl) / 2} урона.\nНет эффектов.";

                case MonsterType.Thorned:
                    return $"Шипастый тип.\nОсобая атака: Выпускает шип, игнорирующий защиту и щит игрока (в последнем случае наносит +{0.3 * playerShield} урона).\nНет эффектов.";

                case MonsterType.Fire:
                    return $"Огненный тип.\nБез особых атак.\nПоджог (Шанс 33%): Снимает {3 * RPG.lvl} ХП в ход в течение 2-х ходов.";

                case MonsterType.Ice:
                    return $"Ледяной тип.\nЛедяной щит (Шанс 5%): Монстр покрывается ледяной оболочкой, становясь неуязвимым на следующий ход.\nЗаморозка (Шанс 15%): Снижает АТК на {3 * RPG.lvl}%.";

                case MonsterType.Dark:
                    return "Тёмный тип.\nБез особых атак.\nЗабвение (Шанс 15%): Шанс удачно ударить монстра снижается до 33%.";

                case MonsterType.Luminous:
                    return $"Светящийся тип.\nЛинза: Дополнительно наносит {MnstrAttack / 5} ед. урона.\nНет эффектов.";

                default:
                    return null;
            }
        }

        static void GetTypeEffect()             // Получить эффект атаки типа монстра
        {
            double a = RPG.rnd.NextDouble();

            switch (mnstrType)
            {
                case MonsterType.Poisonous:
                    if(a <= 1f / 4)
                    {
                        effectCount = 2;
                        playerStatus = Status.Poisoned;
                        RPG.Dialogue($"Вы отравлены.", true, ConsoleColor.DarkGreen);
                    }
                    break;
                    
                case MonsterType.Fire:
                    if(a <= 1f / 3)
                    {
                        effectCount = 2;
                        playerStatus = Status.OnFire;
                        RPG.Dialogue($"Вы в огне.", true, ConsoleColor.DarkRed);
                    }
                    break;

                case MonsterType.Ice:
                    if(a <= 3f / 20)
                    {
                        playerStatus = Status.Frozen;
                        effectCount = 2;
                        playerAttack *= (1 - 0.03f * RPG.lvl);
                        RPG.Dialogue($"АТК снижена на {3 * RPG.lvl}%");
                    }
                    break;

                case MonsterType.Dark:
                    if(a <= 3f / 20)
                    {
                        effectCount = 1;
                        strikeChance = 1f / 3;
                        playerStatus = Status.Blind;
                        RPG.Dialogue($"Шанс попасть по монстру стал 33%");
                    }
                    break;

                default:
                    break;
            }
        }
        #endregion
    }
}
