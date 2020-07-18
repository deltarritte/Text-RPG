using RPGTestC.Events;

namespace RPGTestC.Locations
{
    public class Point
    {
        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public PointType Type { get; set; }

        public bool IsShown { get; set; } = true;

        public enum PointType
        {
            Player,         // Игрок
            Monster,        // Монстр
            Transition,     // Переход
            Goal            // Цель
        }

        public string Index()
        {
            switch (Type)
            {
                case PointType.Player:
                    return "@";

                case PointType.Monster:
                    return "%";

                case PointType.Transition:
                    return ">";

                case PointType.Goal:
                    return "!";

                default:
                    return "?";
            }
        }

        public int TransitRoomIndex;

        public Monster Monster = new Monster();

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public bool IsEqualTo(Point point) => X == point.X && Y == point.Y;

        public void MoveUp() => Y--;

        public void MoveDown() => Y++;

        public void MoveLeft() => X--;

        public void MoveRight() => X++;
    }
}
