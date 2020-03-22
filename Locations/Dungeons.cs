using RPGTestC.Events;
using System;
using System.Collections.Generic;

namespace RPGTestC.Locations
{
    public class Dungeons
    {
        #region Variables

        static int _questNum;                                   // Номер квеста
        static int _roomIter;                                   // Итерация комнаты
        static int _roomNum;                                    // Номер комнаты

        //static bool completed = false;

        static Point player;                                    // Точка игрока

        static Point[] goals;                                   // Массив точек завершения итерации

        static List<Point> pointList = new List<Point> { };     // Список всех точек

        #endregion

        static public void Init(int questNum, int roomIter, int roomNum = 0)    // Инициализация уровня а.к.а. входная точка
        {
            _roomNum = roomNum;     // Передача
            _questNum = questNum;   // Значений
            _roomIter = roomIter;   // Переменных

            switch (questNum)
            {
                case 0:

                    player = new Point { Index = "@" };

                    goals = new Point[]
                    {
                        new Point { X = 6, Y = 6, Type = Point.PointType.Goal },
                        new Point { X = 4, Y = 6, Index = "%", Type = Point.PointType.Monster },
                        new Point { X = 6, Y = 4, Index = "%", Type = Point.PointType.Monster },
                    };

                    break;

                case 2:

                    if (roomIter == 0)
                    {
                        player = new Point { X = 0, Y = 4 };

                        goals = new Point[] { new Point { X = 20, Y = 3, Type = Point.PointType.Goal } };
                    }

                    else
                    {
                        player = new Point { X = 20, Y = 3, Index = "@" };

                        goals = new Point[] { new Point { X = 0, Y = 3, Index = "%", Type = Point.PointType.Goal } };
                    }
                    break;

                case 3:

                    if (roomIter == 0)
                    {
                        player = new Point { X = 6, Y = 6, Index = "@" };

                        goals = new Point[] { new Point { X = 6, Y = 0, Index = "%", Type = Point.PointType.Goal } };
                    }

                    else if (roomIter == 1)
                    {
                        player = new Point { X = 6, Y = 0, Index = "@" };

                        goals = new Point[] { new Point { X = 6, Y = 6, Type = Point.PointType.Goal } };
                    }

                    else
                    {
                        player = new Point { X = 6, Y = 6, Index = "@" };

                        goals = new Point[] { new Point { X = 0, Y = 1, Type = Point.PointType.Goal } };
                    }
                    break;

                case 4:

                    if (roomNum == 0)
                    {
                        if (roomIter == 0)
                            player = new Point { X = 18, Y = 2, Index = "@" };

                        else
                            player = new Point { X = 2, Y = 2, Index = "@" };

                        goals = new Point[] { new Point { X = 0, Y = 2, Type = Point.PointType.TransitionPos } };
                    }

                    else if (roomNum == 1)
                    {
                        if (roomIter == 0)
                            player = new Point { X = 8, Y = 3, Index = "@" };

                        else
                            player = new Point { X = 2, Y = 3, Index = "@" };

                        goals = new Point[] 
                        {
                            new Point { X = 0, Y = 3, Type = Point.PointType.TransitionPos },
                            new Point { X = 10, Y = 3, Type = Point.PointType.TransitionNeg }
                        };
                    }

                    else
                    {
                        player = new Point { X = 32, Y = 2, Index = "@" };

                        goals = new Point[]
                        {
                            new Point{X = 3, Y = 2, Type = Point.PointType.Goal},
                            new Point{X = 34, Y = 2, Type = Point.PointType.TransitionNeg}
                        };
                    }
                    break;

                default:
                    Console.WriteLine("oops");
                    break;
            }

            pointList.Clear();

            pointList.Add(player);
            foreach(Point goal in goals)
            {
                pointList.Add(goal);
            }

            Dungeon();
        }

        static void Dungeon()                                                   // Логика уровня квеста
        {
            Console.Clear();

            for (int Y = 0; Y < Maps().Length; Y++)             // Пересчитывание каждой координаты Y
            {
                Console.WriteLine(Line(Y));
            }

            foreach(Point goal in goals)
            {
                if (player.IsEqualTo(goal) && goal.IsShown)
                {
                    FinishMapIteration(goal);
                    break;
                }
                else if (goal == goals[goals.Length - 1]) DungeonControls();
                else continue;
            }
        }

        static void FinishMapIteration(Point goal)                              // Завершение итерации карты
        {
            if (goal.Type == Point.PointType.TransitionPos) Init(_questNum, 0, _roomNum + 1);

            else if (goal.Type == Point.PointType.TransitionNeg) Init(_questNum, 1, _roomNum - 1);

            else if (goal.Type == Point.PointType.Monster)
            {
                goal.IsShown = false;
                Fight.Init(false);
                Dungeon();
            }
        }

        static void DungeonControls()                                           // Управление точкой игрока
        {
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    if (player.Y > 0)
                        player.MoveUp();
                    else player.Y = 0;
                    break;

                case ConsoleKey.S:
                    if (player.Y < Maps().Length - 1)
                        player.MoveDown();
                    else player.Y = Maps().Length - 1;
                    break;

                case ConsoleKey.A:
                    if (player.X > 0)
                        player.MoveLeft();
                    else player.X = 0;
                    break;

                case ConsoleKey.D:
                    if (player.X < Maps()[player.Y].Length - 1)
                        player.MoveRight();
                    else player.X = Maps()[player.Y].Length - 1;
                    break;

                default:
                    DungeonControls();
                    break;
            }

            Dungeon();
        }

        static string Line(int Y)                                               // Генерация полосы уровня
        {
            string bufLine = Maps()[Y];

            foreach(Point pnt in pointList)
            {
                if(Y == pnt.Y && pnt.IsShown)
                {
                    bufLine = bufLine.Remove(pnt.X, 1);
                    bufLine = bufLine.Insert(pnt.X, pnt.Index);
                }
            }

            return bufLine;
        }
        
        static string[] Maps()                                                  // Карты
        {
            string[] lines = new string[0];

            switch (_questNum)
            {
                case 0:
                    lines = new string[]
                    {
                        "_______",
                        "_______",
                        "_______",
                        "_______",
                        "_______",
                        "_______",
                        "_______",
                    };

                    return lines;

                case 2:
                    
                    lines = new string[]
                    {
                        "fffffffffFFFFffFFffFfFffFfff",
                        "____________________________",
                        "______________=====OP_______",
                        "_______=======______________",
                        "=======_____________________",
                        "____________________________",
                        "ffFFFfFFFFFFFFFfFFFFFffFffff",
                    };

                    return lines;

                case 3:

                    lines = new string[]
                    {
                        "____*****____",
                        "_____***_____",
                        "#___________#",
                        "#___________#",
                        "#___________#",
                        "_____________",
                        "_____|H|_____",
                    };

                    return lines;

                case 4:

                    switch (_roomNum)
                    {
                        case 0:

                            lines = new string[]
                            {
                                "__________________:",
                                "__________________:",
                                "==================I",
                                "__________________:",
                                "__________________:",
                            };

                            break;

                        case 1:

                            lines = new string[]
                            {
                                "___________",
                                "___________",
                                "___________",
                                "=====T=====",
                                "_____H_____",
                                "_____O_____",
                                "___________"
                            };

                            break;

                        case 2:

                            lines = new string[]
                            {
                                "___________________________________",
                                "___________________________________",
                                "___________________O===============",
                                "___________________________________",
                                "___________________________________"
                            };

                            break;
                    }
                    

                    return lines;

                default:
                    return new string[] { "oops" };
            }
        }
    }
}
