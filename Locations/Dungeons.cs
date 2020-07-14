using RPGTestC.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTestC.Locations
{
    public class Dungeons
    {
        static int i;
        static bool isActive;
        static Room[] roomSet;
        static Point player;

        static public void Init(int questNum, int iteration = 0)
        {
            i = iteration;
            isActive = true;

            switch (questNum)
            {
                case 0:
                    Room.test_room_0 = new Room
                    {
                        Player = new Point(22, 8) { Type = Point.PointType.Player },
                        PointList = new Point[3]
                        {
                            new Point(23, 9) { Type = Point.PointType.Goal},
                            new Point(10, 8) { Type = Point.PointType.Monster},
                            new Point(12, 8) { Type = Point.PointType.Transition, TransitRoomIndex = 1 }
                        },
                        Map = new string[]
                        {
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________",
                            "_________________________"
                        }
                    };
                    Room.test_room_1 = new Room
                    {
                        Player = new Point(22, 8) { },
                        PointList = new Point[3]
                        {
                            new Point(23, 9) { Type = Point.PointType.Goal},
                            new Point(10, 8) { Type = Point.PointType.Monster},
                            new Point(12, 8) { Type = Point.PointType.Transition, TransitRoomIndex = 0}
                        },
                        Map = Room.test_room_0.Map
                    };

                    roomSet = new Room[2]
                    {
                        Room.test_room_0,
                        Room.test_room_1
                    };
                    break;

                case 2:
                    Room.quest_2_0_0 = new Room
                    {
                        Player = new Point(0, 4) { Type = Point.PointType.Player },
                        PointList = new Point[1]
                        {
                            new Point(20, 3) { Type = Point.PointType.Goal }
                        },
                        Map = new string[]
                        {
                            "fffffffffFFFFffFFffFfFffFfff",
                            "____________________________",
                            "______________=====OP_______",
                            "_______=======______________",
                            "=======_____________________",
                            "____________________________",
                            "ffFFFfFFFFFFFFFfFFFFFffFffff",
                        }
                    };
                    Room.quest_2_0_1 = new Room
                    {
                        Player = new Point(20, 3) { Type = Point.PointType.Player },
                        PointList = new Point[1]
                        {
                            new Point(0, 3) { Type = Point.PointType.Goal }
                        },
                        Map = Room.quest_2_0_0.Map
                    };

                    roomSet = new Room[2]
                    {
                        new Room(Room.quest_2_0_0),
                        new Room(Room.quest_2_0_1)
                    };
                    break;

                case 3:
                    roomSet = new Room[3]
                    {
                        new Room(Room.quest_3_0_0),
                        new Room(Room.quest_3_0_1),
                        new Room(Room.quest_3_0_2)
                    };
                    break;

                case 4:
                    roomSet = new Room[5]
                    {
                        new Room(Room.quest_4_0_0),
                        new Room(Room.quest_4_0_1),
                        new Room(Room.quest_4_1_0),
                        new Room(Room.quest_4_1_1),
                        new Room(Room.quest_4_2_0),
                    };
                    break;
            }

            player = new Point(roomSet[i].Player);

            Dungeon();
        }

        static void Dungeon()
        {
            Console.Clear();
            Console.WriteLine($"player: {player.X}, {player.Y}; Player: {roomSet[i].Player.X}, {roomSet[i].Player.Y}.");
            //Draw the map line by line from top to bottom
            for(int Y = 0; Y < roomSet[i].Map.Length; Y++) Console.WriteLine(Line(Y));

            DungeonControls();

            //Check if player has stepped on a point
            foreach (Point pnt in roomSet[i].PointList)
            {
                if(player.IsEqualTo(pnt) && pnt.IsShown)
                {
                    GetEvent(pnt);
                    break;
                }
            }

            if (isActive) Dungeon();
        }

        static string Line(int Y)
        {
            string bufLine = roomSet[i].Map[Y];

            foreach(Point pnt in roomSet[i].PointList)
            {
                if (Y == pnt.Y && pnt.IsShown)
                {
                    bufLine = bufLine.Remove(pnt.X, 1);
                    bufLine = bufLine.Insert(pnt.X, pnt.Index());
                }
            }

            if (Y == player.Y)
            {
                bufLine = bufLine.Remove(player.X, 1);
                bufLine = bufLine.Insert(player.X, player.Index());
            }

            return bufLine;
        }

        static void GetEvent(Point pnt)
        {
            switch (pnt.Type)
            {
                case Point.PointType.Monster:
                    pnt.IsShown = false;
                    Fight.Init(false);
                    goto default;

                case Point.PointType.Transition:
                    i = pnt.TransitRoomIndex;
                    player = new Point(roomSet[i].Player);
                    goto default;

                case Point.PointType.Goal:
                    isActive = false;
                    break;

                default:
                    Dungeon();
                    break;
            }
        }

        static void DungeonControls()
        {
            int mapHeight = roomSet[i].Map.Length - 1;
            int mapWidth = roomSet[i].Map[player.Y].Length - 1;

            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    if (player.Y > 0)
                        player.MoveUp();
                    else player.Y = 0;
                    break;

                case ConsoleKey.S:
                    if (player.Y < mapHeight)
                        player.MoveDown();
                    else player.Y = mapHeight;
                    break;

                case ConsoleKey.A:
                    if (player.X > 0)
                        player.MoveLeft();
                    else player.X = 0;
                    break;

                case ConsoleKey.D:
                    if (player.X < mapWidth)
                        player.MoveRight();
                    else player.X = mapWidth;
                    break;

                default:
                    DungeonControls();
                    break;
            }
        }
    }
}
