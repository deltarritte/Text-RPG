using System;

namespace RPGTestC.Locations
{
    public class Tower
    {
        static string[] names = new string[] { "Олег", "Инорен", "Враног", "Уръян", "Многослов" };
        static public string mageName;
        static public bool wasIntroduced = false;

        public static void Entry()
        {
            if (!wasIntroduced)
            {
                wasIntroduced = true;

                mageName = names[RPG.rnd.Next(0,names.Length)]; 
                Texts(1);

                Console.WriteLine("Пропустить рассказ незнакомца? 1 - Да, 2 - Нет");
                var A = Console.ReadLine();

                if (A == "1") AltarRoom();
                else Texts(2);
            }

            AltarRoom();
        }

        static void AltarRoom()
        {
            //TODO
        }

        static void Texts(int num)
        {
            switch (num)
            {
                case 1:
                    RPG.Dialogue("Следуя указаниям незнакомца, вы подходите к Заброшеной Башне.");
                    RPG.Dialogue("Судя по внешнему виду, башне несколько сотен лет.");
                    RPG.Dialogue("Вокруг только лес, нет никакого замка или другого сооружения, к которому могла быть пристроена башня");
                    RPG.Dialogue("Может крикнуть что-нибудь?");
                    RPG.Dialogue("\nНезнакомец", "Не стоит кричать, я ощущаю твоё присутствие и без этого.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue("\nНезнакомец появился у вас за спиной. Как и все.");
                    RPG.Dialogue("\nНезнакомец", "Я поведаю все тайны башни пока мы идём до нужного места. Следуй за мной, странник, отступать уже поздно.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue("\nВы следуете за незнакомцем в башню.");
                    break;

                case 2:
                    RPG.Dialogue("\nНезнакомец", "Для начала хотелось бы представиться: Моё имя " + mageName, false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Эта башня была построена ещё 312 лет назад для особой касты людей.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Да, отдельно построенная башня, ничего в этом нет.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Как ты понимаешь, странник, теперь ты принадлежишь этой касте.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "А значит твоё место здесь.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Немногие могут достигнуть твоего уровня, на моей памяти есть только 10 человек, включая тебя.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Первые трое входили в круг Основателей.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Другие шестеро - их последователи и наследники Башни.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Последний наследник погиб ещё 180 лет назад, поэтому Башня находилась без присмотра.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Надеюсь, ты сможешь вернуть Её в былое величие.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "(тихий смех) Понимаю, ты впервые слышишь об этом месте, тебе Она ни к чему.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue(mageName, "Надеюсь ты передумаешь, когда мы придём в Алтарную Комнату.", false, ConsoleColor.DarkMagenta);
                    RPG.Dialogue("\nВо время рассказа вы поднимались по очень высокой круговой лестнице. Время пролетело незаметно, и вот вы стоите перед дверью.");
                    RPG.Dialogue("Вы вошли в Алтарную Комнату.");
                    AltarRoom();
                    break;

                default:
                    RPG.Dialogue("Что-то пошло не так. Хм.");
                    break;
            }
        }
    }
}