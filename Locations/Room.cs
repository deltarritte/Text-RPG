using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTestC.Locations
{
    public class Room
    {
        public Point[] PointList { get; set; }

        public bool Completed { get; set; } = false;

        public Point Player { get; set; } = new Point(0, 0) { Type = Point.PointType.Player };

        public string[] Map;

        public Room() { }

        public Room(Room room)
        {
            PointList = room.PointList;
            Player = room.Player;
            Map = room.Map;
        }
        //0
        public static Room test_room_0;
        //1
        public static Room test_room_1;
        //0
        public static Room quest_2_0_0; // "quest_(quest#)_(room#)_(iteration#)"
        //1
        public static Room quest_2_0_1;
        //0
        public static Room quest_3_0_0 = new Room
        {
            Player = new Point(6, 6) { Type = Point.PointType.Player },
            PointList = new Point[1]
            {
                new Point(6, 0) { Type = Point.PointType.Goal }
            },
            Map = new string[]
            {
                "____*****____",
                "_____***_____",
                "#___________#",
                "#___________#",
                "#___________#",
                "_____________",
                "_____|H|_____",
            }
        };
        //1
        public static Room quest_3_0_1 = new Room
        {
            Player = new Point(6, 0) { Type = Point.PointType.Player },
            PointList = new Point[1]
            {
                new Point(6, 6) { Type = Point.PointType.Goal }
            },
            Map = quest_3_0_0.Map
        };
        //2
        public static Room quest_3_0_2 = new Room
        {
            Player = new Point(6, 6) { Type = Point.PointType.Player },
            PointList = new Point[1]
            {
                new Point(0, 1) { Type = Point.PointType.Goal }
            },
            Map = quest_3_0_0.Map
        };
        //0
        public static Room quest_4_0_0 = new Room
        {
            Player = new Point(18, 2) { Type = Point.PointType.Player },
            PointList = new Point[1]
            {
                new Point(0, 2) { Type = Point.PointType.Transition, TransitRoomIndex = 2 }
            },
            Map = new string[]
            {
                "__________________:",
                "__________________:",
                "==================I",
                "__________________:",
                "__________________:",
            }
        };
        //1
        public static Room quest_4_0_1 = new Room
        {
            Player = new Point(2, 2) { Type = Point.PointType.Player },
            PointList = quest_4_0_0.PointList,
            Map = quest_4_0_0.Map,
        };
        //2
        public static Room quest_4_1_0 = new Room
        {
            Player = new Point(8, 3) { Type = Point.PointType.Player },
            PointList = new Point[2]
            {
                new Point (0, 3) { Type = Point.PointType.Transition, TransitRoomIndex = 4 },
                new Point (10, 3) { Type = Point.PointType.Transition, TransitRoomIndex = 1 }
            },
            Map = new string[]
            {
                "___________",
                "___________",
                "___________",
                "=====T=====",
                "_____H_____",
                "_____O_____",
                "___________"
            }
        };
        //3
        public static Room quest_4_1_1 = new Room
        {
            Player = new Point(2, 3) { Type = Point.PointType.Player },
            PointList = quest_4_1_0.PointList,
            Map = quest_4_1_0.Map,
        };
        //4
        public static Room quest_4_2_0 = new Room
        {
            Player = new Point(32, 2) { Type = Point.PointType.Player },
            PointList = new Point[2]
            {
                new Point(3, 2) { Type = Point.PointType.Goal },
                new Point(34, 2) { Type = Point.PointType.Transition, TransitRoomIndex = 3 }
            },
            Map = new string[]
            {
                "___________________________________",
                "___________________________________",
                "___________________O===============",
                "___________________________________",
                "___________________________________"
            }
        };
    }
}
