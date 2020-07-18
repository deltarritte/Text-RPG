using RPGTestC.Achievements;
using RPGTestC.Events;
using RPGTestC.Locations;
using RPGTestCBuildA;
using static RPGTestC.Player;
using System;

namespace RPGTestC.Quests
{
    public class PlotQuests
    {
        static Monster[] monster;

        public static void Quest()
        {
            City.currentPlotState = City.QuestState.Completed;
            switch (questNum)
            {
                case 1:
                    Console.Clear();
                    RPG.Dialogue("Вы дошли до синего источника");
                    RPG.Dialogue("Как и ожидалось, всё здесь синее.");
                    RPG.Dialogue("Источник представлял из себя небольшую речку, начало которой виднелось на горизонте.");
                    RPG.Dialogue("Вы проходите вглубь, ощущая свежесть окружающего воздуха.");
                    RPG.Dialogue("Вы чувствуете расслабление и решаете присесть.");
                    RPG.Dialogue("\n...");
                    RPG.Dialogue("\nИз-за деревьев выходит Страж.");
                    RPG.Dialogue("\nЧёрт, подумали вы.");

                    monster = new Monster[1]
                    {
                        new Monster
                        {
                            HP = 100 + 20 * LVL,
                            Damage = 20 + 2 * LVL,
                            Name = "Страж",
                            Type = Monster.MType.Standart,
                        }
                    };
                    Fight.Init(monster, true);
                    break;

                case 2:
                    Console.Clear();
                    RPG.Dialogue("Вы следуете по маршруту торговца, уходя глубоко в лес.");
                    RPG.Dialogue("С каждым шагом окружающая тишина становится всё подозрительнее.");
                    RPG.Dialogue("Туман густеет.");
                    RPG.Dialogue("Давление обстановки растёт.");
                    RPG.Dialogue("Вы наткнулись на следы колёс.");
                    RPG.Dialogue("Последовать по ним кажется хорошей идеей.");
                    RPG.Dialogue("\n...");
                    RPG.Dialogue("\nЭто будет долгий поход.");

                    Dungeons.Init(questNum);

                    RPG.Dialogue("\nВы подошли к разрушенной повозке.");
                    RPG.Dialogue("\nА рядом с ней...");
                    RPG.Dialogue("\nМёртвый торговец.");
                    RPG.Dialogue("\nСледов нападения не видно.");
                    RPG.Dialogue("Исследовав повозку, вы не нашли ни одного товара.");
                    RPG.Dialogue("Только много гари.");
                    RPG.Dialogue("Возможно, повозка загорелась, и торговец не смог спастись?");
                    RPG.Dialogue("Возможно, торговец вёз взрывчатку.");
                    RPG.Dialogue("\nСтранное дело.");
                    RPG.Dialogue("\n...");
                    RPG.Dialogue("\nВы заметили медальон на торговце.");
                    RPG.Dialogue("Вы его взяли, без особой цели. Он не выглядит дорогостоящим.");
                    RPG.Dialogue("Кроме красного камня посередине. Но его достать оттуда пркатически невозможно.");
                    RPG.Dialogue("\nНа обратной стороне медальона написано '963'");
                    RPG.Dialogue("\nЧто бы это не значило.");
                    RPG.Dialogue("\nВы начали возвращаться.");

                    Dungeons.Init(questNum, 1);

                    RPG.Dialogue("\nПеред вами стоит монстр.");
                    RPG.Dialogue("Что не удивительно.");

                    monster = new Monster[1]
                    {
                        new Monster
                        {
                            HP = 400 + 30 * LVL,
                            Damage = 30 + 3 * LVL,
                            Name = "Заградитель",
                            Type = Monster.MType.Fire
                        }
                    };
                    Fight.Init(monster, true);
                    break;

                case 3:
                    Console.Clear();
                    RPG.Dialogue("Вход в подвал находится снаружи.");
                    RPG.Dialogue("Видимо, так удобнее?");
                    RPG.Dialogue("Вы спустились в подвал таверны.");
                    RPG.Dialogue("Здесь довольно темно.");
                    RPG.Dialogue("Единственное освещение это свет из открытой двери.");
                    RPG.Dialogue("В конце подвала видно только чёрный туман.");
                    RPG.Dialogue("\nВы начинаете движение в его сторону.");

                    Dungeons.Init(questNum);

                    RPG.Dialogue("\nКак и ожидалось:");
                    RPG.Dialogue("Внутри тумана монстр.");

                    monster = new Monster[1]
                    {
                        new Monster
                        {
                            HP = 600 + 40 * LVL,
                            Damage = 40 + 8 * LVL,
                            Name = "Туманный зверь",
                            Type = Monster.MType.Dark
                        }
                    };
                    Fight.Init(monster, true);

                    RPG.Dialogue("Тело монстра рассеялось в тумане.");
                    RPG.Dialogue("Время возвращаться.");

                    Dungeons.Init(questNum, 1);

                    RPG.Dialogue("\nПохоже, что дверь заклинило.");
                    RPG.Dialogue("Можно попробовать найти другой выход.");
                    RPG.Dialogue("Можно выломать дверь.");

                    Console.WriteLine("Что делать?" +
                        "\n1 - Выломать" +
                        "\n2 - Найти другой выход");

                    if(Console.ReadLine() == "1")
                    {
                        brokeOut = true;

                        RPG.Dialogue("\nВы успешно выломали дверь.");
                        RPG.Dialogue("Будет очень неприятно снова встретить Охру.");
                    }

                    else
                    {
                        RPG.Dialogue("\nНет, ломать дверь действительно не стоит.");
                        RPG.Dialogue("Ведь из подвала есть ещё один выход в город.");

                        Dungeons.Init(questNum, 2);

                        RPG.Dialogue("\nНу вот.");
                        RPG.Dialogue("И не нужно никакой излишней жестокости!");
                    }

                    break;

                default:
                    Console.WriteLine("Хз чёт");
                    break;
            }
        }
    }
}
