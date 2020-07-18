using RPGTestC.Achievements;
using RPGTestC.Events;
using RPGTestC.Quests;
using RPGTestCBuildA;
using static RPGTestC.Player;
using System;

namespace RPGTestC.Locations
{
    public class City
    {
        #region Variables

        public enum QuestState : Int16
        {
            Unknown,
            Known,
            Accepted,
            Completed
        }
        static public QuestState currentPlotState = QuestState.Unknown;

        static public bool wasInCity;

        static float CityEconomy;
        static float BankTax;
        static float HealTax;
        static float TavernTax;
        static double HealPrice;
        static double ChargePrice;
        static double BankPrice;
        static double TavernPrice;
        static double ValueW;
        static double ValueA;
        static public float cityBank = 10000;

        #endregion

        static void UpdateEconomy()
        {
            CityEconomy = 1000 / cityBank;

            BankTax = 1 + CityEconomy * 2;
            HealTax = 1 + CityEconomy * 4;
            TavernTax = 1 + CityEconomy * 5;

            BankPrice = Math.Round(10 * BankTax, 0);
            HealPrice = Math.Round(10 * HealTax, 0);
            ChargePrice = Math.Round(2 * HealTax, 0);
            TavernPrice = Math.Round(200 * TavernTax);

            ValueW = TavernPrice * DamageCoeff; // Цена на улучшение оружия
            ValueA = TavernPrice * DefenceCoeff; //        ...       брони
        }

        static public void GoToCity()
        {
            UpdateEconomy();
            RPG.Fancies();
            Console.Clear();

            //RPG.Dialogue("     /o[+]o|     _[T]_" +
            //       "\n     |T=n=T|     |=n=|", ConsoleColor.White, true);

            if (!wasInCity)
            {
                wasInCity = true;
                RPG.Dialogue("В городе можно вылечиться или посетить таверну.");
            }

            Console.WriteLine("\nИгрок:" +
                "\nHP: " + HP + "/" + MaxHP + " [" + RPG.HPLine + "] " +
                "\nДеньги: " + Money);

            Console.WriteLine("\nЧто будешь делать? Вылечиться - 1, Таверна - 2, Вернуться в лес - 0");

            switch (Console.ReadLine())
            {
                case "1":
                    GoHeal();
                    break;

                case "2":
                    GoToTavern();
                    break;

                case "0":
                    Console.Clear();
                    break;

                default:
                    GoToCity();
                    break;
            }
        }

        static void GoHeal()
        {
            Console.WriteLine($"Лечение стоит {HealPrice} ({ChargePrice} за 1 заряд)." +
                $"\nУ вас {Money} ({healCount}/{maxHealCount} Зарядов)." +
                $"\nВылечиться? Да - 1, Нет - 2");

            switch (Console.ReadLine())
            {
                case "1":

                    if (HP < MaxHP)
                    {
                        if (Money >= HealPrice)
                        {
                            HP = MaxHP;
                            Money -= Convert.ToInt16(HealPrice);
                            cityBank += Convert.ToInt16(HealPrice/10);
                            UpdateEconomy();

                            Console.WriteLine("Здоровье восстановлено.");
                        }
                        else Console.WriteLine("Недостаточно денег для восстановления здоровья.");
                    }
                    else Console.WriteLine("Здоровье полное.");

                    if (healCount != maxHealCount)
                    {
                        if(Money >= ChargePrice * (maxHealCount - healCount))
                        {
                            healCount = maxHealCount;
                            Money -= Convert.ToInt16(ChargePrice * (maxHealCount - healCount));
                            cityBank += Convert.ToInt16(ChargePrice * (maxHealCount - healCount) / 10);
                            UpdateEconomy();

                            RPG.Dialogue("Заряды восстановлены.");
                        }
                        else RPG.Dialogue("Недостаточно денег для восстановления зарядов.");
                    }
                    else RPG.Dialogue("Заряды полные.");
                    goto case "2";

                case "2":
                    GoToCity();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("Нет команды.\n");
                    GoHeal();
                    break;
            }
        }

        static public void GoToTavern()
        {
            int dialRand = RPG.rnd.Next(1, 100);

            if (dialRand <= 50) RPG.Dialogue("Вы входите в таверну, со всех сторон слышны крики, смех и в целом весёлая атмосфера");
            else if (dialRand > 50 && dialRand <= 75) RPG.Dialogue("В таверне необычайно тихо. \nЛюдей почти нет. \nЛюбопытно.");
            else if (dialRand > 75 && dialRand <= 90) RPG.Dialogue("Когда вы вошли, несколько людей на вас обернулись, но затем вернулись ко своим делам.");
            else if (dialRand > 90)
            {
                RPG.Dialogue("В таверне творится невероятная анархия. Бутылки летают из стороны в сторону, половина столов перевёрнута.", false, ConsoleColor.Yellow);
                RPG.Dialogue("В общем, всё как и полагается.", false, ConsoleColor.Yellow);
                gotAnarchy = true;
            }

            RPG.Dialogue("\nВы садитесь за стол.");
            RPG.Dialogue("Вы тихо сидите за столом.");

            switch (questNum)
            {
                case 0: //Первый вход в таверну

                    RPG.Dialogue("\nК вам подходит владелец заведения.");
                    RPG.Dialogue("Владелец: Стол свободен?", false, ConsoleColor.DarkYellow);

                    RPG.Dialogue("\nНе дожидаясь ответа, он сел напротив вас.");
                    RPG.Dialogue("Владелец: По глазам вижу — Искатель приключений." +
                        "\nМоё имя — Охра, приятно познакомиться.", false, ConsoleColor.DarkYellow);

                    RPG.Dialogue("\nВы пожимаете руки.");

                    RPG.Dialogue("\nОхра", "Я продаю здесь напитки, а ещё занимаюсь совершенствованием инструментов и оружия.", false, ConsoleColor.DarkYellow);
                    RPG.Dialogue("Охра", "Так что если тебе хочется выдерживать больше ударов, или бить больнее, то тебе ко мне.", false, ConsoleColor.DarkYellow);
                    RPG.Dialogue("Охра", "Раз уж ты здесь новенький, то вот, держи выпивку за счёт заведения.", false, ConsoleColor.DarkYellow);

                    RPG.Dialogue("\nНа стол поставили кружку с практически чёрной жидкостью.");

                    RPG.Dialogue("\nОхра", "Называется 'Чёрная Дыра'. Называется так из-за цвета, но говорят, что 'уносит' не хуже чёрной дыры.", false, ConsoleColor.DarkYellow);

                    RPG.Dialogue("\nВы вежливо отказываетесь.");

                    RPG.Dialogue("\nОхра", "Не из тех, кто любит выпить? Что же, тогда тебе здесь практически незачем быть!", false, ConsoleColor.DarkYellow);
                    RPG.Dialogue("Охра", "У меня есть безалкогольные варианты, но бесплатно я их раздавать не собираюсь.", false, ConsoleColor.DarkYellow);
                    RPG.Dialogue("Охра", "Ладно, мне надо руководить этим местом, чтобы снова не возникли анархия и хаос. Увидимся позже.", false, ConsoleColor.DarkYellow);

                    RPG.Dialogue("\nСнова?");

                    RPG.Dialogue("\nОхра уходит.");
                    RPG.Dialogue("Немного посидев, вы решили вернуться в город.");

                    questNum = 1;

                    break;

                case 1: //Вход в таверну, получение первого квеста/награды за него.

                    RPG.Dialogue("Из-за угла выходит Охра.");
                    RPG.Dialogue("Охра садится за ваш стол.");

                    switch (currentPlotState)
                    {
                        case QuestState.Unknown:

                            RPG.Dialogue("Охра", "У меня есть одно дело.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "Недалеко от Синего Источника проживают Стражи.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Мне нужна голова одного из них.", false, ConsoleColor.DarkYellow);

                            if (LVL < 4) RPG.Dialogue("Охра", "Для тебя это может быть сложновато.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("Охра", "Если принесёшь, то в награду я смогу тебе что-нибудь улучшить.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "Идёт?", false, ConsoleColor.DarkYellow);
                            if (AcceptQuest()) RPG.Dialogue("Охра", "Ну и отлично!" + "\nЖду тебя с головой Стража.", false, ConsoleColor.DarkYellow);

                            else
                            {
                                RPG.Dialogue("\nОхра", "Ну нет так нет, но ты заходи ещё, вдруг передумаешь или выпить захочешь.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nОхра уходит.");
                                RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                            }

                            break;

                        case QuestState.Known:

                            RPG.Dialogue("\nОхра", "Да, мне нечем заняться."
                        + "\nИтак, ты согласен с условиями моего договора?", true, ConsoleColor.DarkYellow);

                            if (AcceptQuest()) RPG.Dialogue("Охра", "Ну и отлично!" + "\nЖду тебя с головой Стража.", false, ConsoleColor.DarkYellow);

                            else
                            {
                                RPG.Dialogue("\nОхра", "Ну нет так нет, но ты заходи ещё, вдруг передумаешь или выпить захочешь.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nОхра уходит.");
                                RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                            }

                            break;

                        case QuestState.Accepted:

                            RPG.Dialogue("Вы тихо сидите за столом, ощущая непрерывный взгляд себе в глаза.");

                            RPG.Dialogue("\nВы говорите: Да принесу я тебе голову!");

                            RPG.Dialogue("\nОхра встал и ушёл.");
                            RPG.Dialogue("Немного посидев, вы решили вернуться в город.");

                            break;

                        case QuestState.Completed:

                            RPG.Dialogue("\nОхра", "Ну что, принёс? Можешь не отвечать, вижу, что принёс.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра забирает у вас голову Стража");

                            RPG.Dialogue("\nОхра", "Из их крови выходит замечательный напиток, King Crimson называется.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("Охра", "А теперь к моей части уговора.", false, ConsoleColor.DarkYellow);

                            questNum = 2;
                            currentPlotState = QuestState.Unknown;

                            GearUpgrade(true);

                            break;
                    }

                    break;

                case 2: //Вход в таверну, получение Второго квеста/награды за него.

                    RPG.Dialogue("\nЗа вами кто-то наблюдает.", false, ConsoleColor.DarkMagenta);

                    RPG.Dialogue("\nКто это?");

                    RPG.Dialogue("\nК вам подходит Охра.");

                    RPG.Dialogue("\nОхра", "Рад тебя снова видеть.", false, ConsoleColor.DarkYellow);

                    switch (currentPlotState)
                    {
                        case QuestState.Unknown:

                            RPG.Dialogue("Охра", "У меня для тебя есть ещё одно дело.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Если ты не против, конечно.", false, ConsoleColor.DarkYellow);

                            if (AcceptQuest())
                            {
                                RPG.Dialogue("\nОхра", "Отлично, на тебя всегда можно положиться.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Хочу предупредить, это задание не из лёгких.", false, ConsoleColor.DarkYellow);
                                if (LVL < 6) RPG.Dialogue("Охра", "Тебе придётся нелегко, поэтому рекомендую поптренироваться побольше.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Торговец, который продаёт мне довольно редкие товары, недавно пропал без вести.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Я хочу, чтобы ты его нашёл и разузнал в чём дело.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Его маршрут был доволньно опасен. Я удивлён, что он вообще прожил так долго.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Но думаю, что ты справишься.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Вот карта его маршрута, удачи.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nВы получили карту маршрута торговца.");
                            }

                            else
                            {
                                RPG.Dialogue("\nОхра", "Ладно, твой выбор.", false, ConsoleColor.DarkYellow);
                            }

                            break;

                        case QuestState.Known:

                            RPG.Dialogue("\nОхра", "Ну так что, ты согласен?", false, ConsoleColor.DarkYellow);

                            if (AcceptQuest())
                            {
                                RPG.Dialogue("\nОхра", "Отлично, на тебя всегда можно положиться.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Хочу предупредить, это задание не из лёгких.", false, ConsoleColor.DarkYellow);
                                if (LVL < 6) RPG.Dialogue("Охра", "Тебе придётся нелегко, поэтому рекомендую потренироваться побольше.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Торговец, который продаёт мне довольно редкие товары, недавно пропал без вести.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Я хочу, чтобы ты его нашёл и разузнал в чём дело.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Его маршрут был доволньно опасен. Я удивлён, что он вообще прожил так долго.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Но думаю, что ты справишься.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Вот карта его маршрута, удачи.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nВы получили карту маршрута торговца.");
                            }

                            else RPG.Dialogue("\nОхра", "Ладно, твой выбор.", false, ConsoleColor.DarkYellow);

                            break;

                        case QuestState.Accepted:

                            RPG.Dialogue("Охра", "Есть успехи?", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nВы отрицательно покачали головой.");

                            RPG.Dialogue("\nОхра", "(вздох) Хорошо, не буду тебя напрягать. Но всё же поторопись с выполнением.", false, ConsoleColor.DarkYellow);


                            break;

                        case QuestState.Completed:

                            RPG.Dialogue("Охра", "Как успехи с моим заданием?", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nВы даёте Охре Медальон Торговца.");

                            RPG.Dialogue("\nОхра", "Вот значит как...", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Что же...", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "...", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Придётся искать нового торговца!", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nНа вашем лице выражено удивление.");

                            RPG.Dialogue("\nОхра", "Да, этот торговец был хорош, но это не значит, что он единственный!", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Такие здесь порядки, искатель приключений:", true, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Торговля не стоит на месте!", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "Кстати о торговле...", false, ConsoleColor.DarkYellow);

                            questNum = 3;
                            currentPlotState = QuestState.Unknown;

                            GearUpgrade(true);

                            break;
                    }

                    break;

                case 3:

                    RPG.Dialogue("Чувство наблюдаемости становится сильнее.", false, ConsoleColor.DarkMagenta);

                    RPG.Dialogue("\nДа кто же это?");

                    RPG.Dialogue("\nОборачиваясь по сторонам в поисках наблюдателя, вы видите, что к вашему столу подходит Охра.");

                    if (RPG.rnd.NextDouble() <= 0.1f)
                    {
                        RPG.Dialogue("Ah shit, here we go again.", false, ConsoleColor.Yellow);
                        ahShit = true;
                    }

                    RPG.Dialogue("\nОхра садится за ваш стол.");

                    switch (currentPlotState)
                    {
                        case QuestState.Unknown:

                            RPG.Dialogue("\nОхра", "Все люди здесь абсолютно бесполезны, надеюсь ты меня обрадуешь.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Нужно сходить в подвал Таверны и убить живущее там нечто.", false, ConsoleColor.DarkYellow);
                            if (LVL <= 7) RPG.Dialogue("Охра", "Хотя, тебе поможет разве что удача, чтобы справиться с ним.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("Охра", "Согласен?", true, ConsoleColor.DarkYellow);

                            if (AcceptQuest())
                            {
                                RPG.Dialogue("\nОхра", "Замечательно! Приступай как можно скорее, а то мой бизнес накроется медным тазом!", false, ConsoleColor.DarkYellow);
                            }

                            else
                            {
                                RPG.Dialogue("\nОхра бьёт по столу.");

                                RPG.Dialogue("\nОхра", "Да что же это такое!", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("Охра", "Ладно, я не могу тебя принудить. Найду кого-нибудь.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nОхра ушёл.");
                                RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                            }

                            break;

                        case QuestState.Known:

                            RPG.Dialogue("\nОхра", "Я ещё не нашёл замены.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Надеюсь, теперь ты согласен мне помочь.", false, ConsoleColor.DarkYellow);

                            if (LVL <= 7) RPG.Dialogue("Охра", "Хоть ты и не готов.", false, ConsoleColor.DarkYellow);

                            if (AcceptQuest())
                            {
                                RPG.Dialogue("\nОхра", "Отлично! Приступай как можно скорее, а то мой бизнес накроется медным тазом!", false, ConsoleColor.DarkYellow);
                            }

                            else
                            {
                                RPG.Dialogue("\nОхра бьёт по столу.");

                                RPG.Dialogue("\nОхра", "Да что же это такое!", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("Охра", "Ладно, я не могу тебя принудить. Поищу кого-нибудь ещё.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nОхра ушёл.");
                                RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                            }

                            break;

                        case QuestState.Accepted:

                            Console.WriteLine("Начать задание? 1- Да, 2 - Нет.");
                            if (Console.ReadKey().Key == ConsoleKey.D1)
                            {
                                PlotQuests.Quest();
                            }

                            else
                            {
                                RPG.Dialogue("\nОхра перебирает пальцами по столу пристально смотря вам в глаза.");

                                RPG.Dialogue("\nЭто продолжается в течение минуты.");

                                RPG.Dialogue("\nПосле такой долгой пытки вы сдаётесь, чувствуя нарастающую вину.");

                                RPG.Dialogue("\nОхра встаёт и уходит.");

                                RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                            }

                            break;

                        case QuestState.Completed:

                            if (brokeOut)
                            {

                                RPG.Dialogue("\nОхра", "Что ты наделал?", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Я тебя попросил разобраться только с монстром!", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Что тебе дверь-то сделала?", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Ладно, чёрт с этой дверью.", false, ConsoleColor.DarkYellow);
                            }

                            RPG.Dialogue("\nОхра", "Спасибо.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Теперь моя работа может продолжаться спокойно.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "От этих паразитов обычно легко избавиться.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Но с этим было что-то не то: слишком уж большой.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Что же, раз уж ты спас мой бизнес...", false, ConsoleColor.DarkYellow);

                            questNum = 4;
                            currentPlotState = QuestState.Unknown;

                            GearUpgrade(true);

                            break;
                    }

                    break;

                case 4:

                    RPG.Dialogue("В этот раз никто за вами не наблюдает.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue("По крайней мере, вы так считаете.", false, ConsoleColor.DarkMagenta);

                    RPG.Dialogue("\nНаконец-то.");

                    RPG.Dialogue("\nЧерез некоторе время к вам подходит Охра.");

                    switch (currentPlotState)
                    {
                        case QuestState.Unknown:

                            RPG.Dialogue("\nОхра", "И снова здравствуй.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Как ты понимаешь, у меня снова есть для тебя дело.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Я недавно ходил в северные шахты.", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Там есть ингредиенты для моего нового напитка, хотел набрать достаточно, пока ищу нового торговца.",
                                false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Ингредиенты я набрал, но...", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "Я оставил там свою шляпу.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nВы посмотрели на Охру с немного грустным, разочарованным, но в то же время полным недовольства и умеренной злости взглядом.");

                            RPG.Dialogue("\nОхра", "Не смотри на меня так, эта шляпа очень важна!", false, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "И вообще, тебе сложно, что ли? Заодно новые места увидишь!", false, ConsoleColor.DarkYellow);

                            if (AcceptQuest()) RPG.Dialogue("\nОхра", "Спасибо! Жду тебя с нетерпением!", false, ConsoleColor.DarkYellow);
                            else
                            {
                                RPG.Dialogue("\nОхра посмотрел на вас полным разочароания и печали взглядом.");

                                RPG.Dialogue("\nОхра", "Ладно.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Но просто так я от тебя не отстану.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nОхра ушёл.");
                                RPG.Dialogue("\nПодавив чувство вины, к чему вы уже привыкли, и немного посидев, вы вернулись в город.");
                            }
                            break;

                        case QuestState.Known:

                            RPG.Dialogue("\nОхра", "Ну как там с твоим решением начёт возврата моей шляпы?", true, ConsoleColor.DarkYellow);
                            RPG.Dialogue("Охра", "Я даже скажу пожалуйста.", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "Пожалуйста.", false, ConsoleColor.DarkYellow);

                            if (AcceptQuest()) RPG.Dialogue("\nОхра", "Спасибо!", false, ConsoleColor.DarkYellow);
                            else
                            {
                                RPG.Dialogue("\nОхра посмотрел на вас полным разочароания и печали взглядом.");

                                RPG.Dialogue("\nОхра", "Ладно. Даже 'пожалуйста' не помогло.", false, ConsoleColor.DarkYellow);
                                RPG.Dialogue("Охра", "Что же ты за жестокий человек?.", false, ConsoleColor.DarkYellow);

                                RPG.Dialogue("\nОхра ушёл.");
                                RPG.Dialogue("\nСнова подавив чувство вины, и немного посидев, вы вернулись в город.");
                            }
                            break;

                        case QuestState.Accepted:

                            RPG.Dialogue("\nОхра", "Где шляпа, Искатель Приключений?", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "А?", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра", "Я ведь жду!", false, ConsoleColor.DarkYellow);

                            RPG.Dialogue("\nОхра ушёл, не дав вам шанса высказаться.");

                            RPG.Dialogue("\nНемного посидев, вы вернулись в город.");
                            break;

                        case QuestState.Completed:
                            RPG.Dialogue("\nОхра", "Наконец-то я дождался!", false, ConsoleColor.DarkYellow);

                            questNum = 5;
                            break;
                    }

                    break;

                case 15: //Вход в таверну, получение локации башни/ничего.

                    if (!towerLoc && LVL == 15)
                    {
                        towerLoc = true;

                        RPG.Dialogue("Внезапно из-за спины выходит...");
                        RPG.Dialogue("\nКто это?");
                        RPG.Dialogue("\nНезнакомец", "Аааа, странник, я слышал о тебе!", false, ConsoleColor.DarkMagenta);
                        RPG.Dialogue("Незнакомец", "Более того, я следил за твоими успехами из теней.", false, ConsoleColor.DarkMagenta);
                        RPG.Dialogue("Незнакомец", "У меня для тебя есть дело, но я не могу его обсуждать здесь.", false, ConsoleColor.DarkMagenta);
                        RPG.Dialogue("Незнакомец", "Вот карта с местом нашей встречи. Увидимся там.", false, ConsoleColor.DarkMagenta);

                        RPG.Dialogue("\nВы получили новую локацию: Заброшенная башня");
                        RPG.Dialogue("\nНезнакомец ушёл.");

                        RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                    }

                    else goto default;
                    break;

                default:
                    RPG.Dialogue("Немного посидев, вы решили вернуться в город.");
                    break;
            }

            GoToCity();
        }

        static public void GearUpgrade(bool free)
        {
            Console.Clear();

            if (free) RPG.Dialogue("Охра", "Что тебе нужно улучшить?", true, ConsoleColor.DarkYellow);

            else RPG.Dialogue("Охра", "Моя цена небольшая, " + ValueW + " монет за улучшение оружия, " + ValueA + " за улучшение брони."
                    + "\nС каждым улучшением повышаю эффективность на 5%."
                    + "\n\nУ вас " + Money + " монет.", true, ConsoleColor.DarkYellow);

            Console.WriteLine($"Текущие характеристики: {Damage} АТК ({DamageCoeff * 100}%)" +
                           $"\n                         {Defence} ЗАЩ ({DefenceCoeff * 100}%)");
            Console.WriteLine("Улучшить (+5%) оружие - 1, броню - 2");

            switch (Console.ReadLine())
            {
                case "1":
                    DamageCoeff += 0.05f;
                    Damage = (float)Math.Round((10 + Math.Pow(2, LVL)) * DamageCoeff);

                    if (!free)
                    {
                        Money -= Convert.ToInt16(ValueW);
                        cityBank += (float)ValueW * (TavernTax - 1);
                    }

                    break;

                case "2":
                    DefenceCoeff += 0.05f;
                    Defence = (float)(Math.Pow(LVL, 2) - 1 / (4 * LVL + 1)) * DefenceCoeff;

                    if (!free)
                    {
                        Money -= Convert.ToInt16(ValueA);
                        cityBank += (float)ValueA * (TavernTax - 1);
                    }
                    break;

                default:
                    GearUpgrade(free);
                    break;
            }

            UpdateEconomy();

            RPG.Dialogue("Охра", "Ещё увидимся.", false, ConsoleColor.DarkYellow);
        }

        static bool AcceptQuest()
        {
            Console.WriteLine("\nВзять задание? Да - 1, Нет - 2");

            if (Console.ReadKey().Key == ConsoleKey.D1)
            {
                currentPlotState = QuestState.Accepted;
                return true;
            }

            else
            {
                currentPlotState = QuestState.Known;
                return false;
            }
        }
    }
}
