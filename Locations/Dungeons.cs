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

        static bool completed = false;

        static Point player;                                    // Точка игрока

        static Point monster_1;

        static Point monster_2;

        static List<Point> goals;                                   // Список всех точек

        #endregion

        static public void Init(int questNum, int roomIter, int roomNum = 0, bool isFirst = true)    // Инициализация уровня а.к.а. входная точка
        {
            completed = false;
            _roomNum = roomNum;     // Передача
            _questNum = questNum;   // Значений
            _roomIter = roomIter;   // Переменных

            switch (questNum)
            {
                case 0:

                    if (roomNum == 0)
                    {
                        if (isFirst)
                        {
                            monster_1 = new Point { X = 4, Y = 6, Type = Point.PointType.Monster };
                            monster_2 = new Point { X = 6, Y = 4, Type = Point.PointType.Monster };
                        }

                        player = new Point { Type = Point.PointType.Player };

                        goals = new List<Point>
                        {
                            player,
                            new Point { X = 6, Y = 6, Type = Point.PointType.Goal },
                            new Point { X = 2, Y = 2, Type = Point.PointType.TransitionPos },
                            monster_1,
                            monster_2
                        };
                    }

                    else
                    {
                        player = new Point { Type = Point.PointType.Player };

                        goals = new List<Point>
                        {
                            player,
                            new Point { X = 6, Y = 6, Type = Point.PointType.Goal },
                            new Point { X = 2, Y = 2, Type = Point.PointType.TransitionNeg },
                        };
                    }
                    

                    break;

                case 2:

                    if (roomIter == 0)
                    {
                        player = new Point { X = 0, Y = 4, Type = Point.PointType.Player };

                        goals = new List<Point> { new Point { X = 20, Y = 3, Type = Point.PointType.Goal } };
                    }

                    else
                    {
                        player = new Point { X = 20, Y = 3, Type = Point.PointType.Player };

                        goals = new List<Point> { new Point { X = 0, Y = 3, Type = Point.PointType.Goal } };
                    }
                    break;

                case 3:

                    if (roomIter == 0)
                    {
                        player = new Point { X = 6, Y = 6, Type = Point.PointType.Player };

                        goals = new List<Point> { new Point { X = 6, Y = 0, Type = Point.PointType.Goal } };
                    }

                    else if (roomIter == 1)
                    {
                        player = new Point { X = 6, Y = 0, Type = Point.PointType.Player };

                        goals = new List<Point> { new Point { X = 6, Y = 6, Type = Point.PointType.Goal } };
                    }

                    else
                    {
                        player = new Point { X = 6, Y = 6, Type = Point.PointType.Player };

                        goals = new List<Point> { new Point { X = 0, Y = 1, Type = Point.PointType.Goal } };
                    }
                    break;

                case 4:

                    if (roomNum == 0)
                    {
                        if (roomIter == 0)
                            player = new Point { X = 18, Y = 2, Type = Point.PointType.Player};

                        else
                            player = new Point { X = 2, Y = 2, Type = Point.PointType.Player };

                        goals = new List<Point> { new Point { X = 0, Y = 2, Type = Point.PointType.TransitionPos } };
                    }

                    else if (roomNum == 1)
                    {
                        if (roomIter == 0)
                            player = new Point { X = 8, Y = 3, Type = Point.PointType.Player };

                        else
                            player = new Point { X = 2, Y = 3, Type = Point.PointType.Player };

                        goals = new List<Point>
                        {
                            new Point { X = 0, Y = 3, Type = Point.PointType.TransitionPos },
                            new Point { X = 10, Y = 3, Type = Point.PointType.TransitionNeg }
                        };
                    }

                    else
                    {
                        player = new Point { X = 32, Y = 2, Type = Point.PointType.Player };

                        goals = new List<Point>
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

            Dungeon();
        }

        static void Dungeon()                                                   // Логика уровня квеста
        {
            Console.Clear();

            if (!completed)
            {
                Console.WriteLine(_roomNum + " " + player.X + " " + player.Y);

                for (int Y = 0; Y < Maps().Length; Y++)             // Пересчитывание каждой координаты Y
                {
                    Console.WriteLine(Line(Y));
                }

                foreach (Point goal in goals)
                {
                    if (player.IsEqualTo(goal) && goal.IsShown && goal.Type != Point.PointType.Player)
                    {
                        FinishMapIteration(goal);
                        break;
                    }
                    else if (goal == goals[goals.Count - 1]) DungeonControls();
                    else continue;
                }
            }
        }

        static void FinishMapIteration(Point goal)                              // Завершение итерации карты
        {
            switch (goal.Type)
            {
                case Point.PointType.Monster:
                    goal.IsShown = false;
                    Fight.Init(false);
                    Dungeon();
                    break;

                case Point.PointType.TransitionPos:
                    Init(_questNum, 0, _roomNum + 1, false);
                    break;

                case Point.PointType.TransitionNeg:
                    Init(_questNum, 1, _roomNum - 1, false);
                    break;

                case Point.PointType.Goal:
                    completed = true;
                    break;

                default:
                    break;
            }
        }

        static void DungeonControls()                                           // Управление точкой игрока
        {
            int mapSizeY = Maps().Length - 1;
            int mapSizeX = Maps()[player.Y].Length - 1;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    if (player.Y > 0)
                        player.MoveUp();
                    else player.Y = 0;
                    break;

                case ConsoleKey.S:
                    if (player.Y < mapSizeY)
                        player.MoveDown();
                    else player.Y = mapSizeY;
                    break;

                case ConsoleKey.A:
                    if (player.X > 0)
                        player.MoveLeft();
                    else player.X = 0;
                    break;

                case ConsoleKey.D:
                    if (player.X < mapSizeX)
                        player.MoveRight();
                    else player.X = mapSizeX;
                    break;

                case ConsoleKey.C:
                    string line = Console.ReadLine();
                    char[] a = new char[1] { ' ' };

                    string[] lineArr = line.Split(a, 4);

                    if (lineArr[1] == "tp")
                    {
                        player.X = Convert.ToInt16(lineArr[2]);
                        player.Y = Convert.ToInt16(lineArr[3]);

                        if (player.Y > mapSizeY) player.Y = mapSizeY;

                        else player.Y = 0;

                        if (player.X > mapSizeX) player.X = mapSizeX;

                        else player.X = 0;
                    }

                    else if (lineArr[1] == "end")
                    {
                        completed = true;
                    }

                    else goto default;

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

            foreach(Point pnt in goals)
            {
                if(Y == pnt.Y && pnt.IsShown)
                {
                    bufLine = bufLine.Remove(pnt.X, 1);
                    bufLine = bufLine.Insert(pnt.X, pnt.Index());
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
