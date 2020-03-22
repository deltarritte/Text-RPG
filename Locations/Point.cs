namespace RPGTestC.Locations
{
    public class Point
    {
        public int X { get; set; } = 0;

        public int Y { get; set; } = 0;

        public string Index { get; set; } = "!";

        public PointType Type { get; set; } = PointType.Player;

        public bool IsShown { get; set; } = true;

        public enum PointType
        {
            Player,         // Игрок
            Monster,        // Монст
            TransitionPos,  // Положительный переход
            TransitionNeg,  // Отрицательный переход
            Goal            // Цель
        }
        public bool IsEqualTo(Point point) => X == point.X && Y == point.Y;

        public void MoveUp() => Y--;

        public void MoveDown() => Y++;

        public void MoveLeft() => X--;

        public void MoveRight() => X++;
    }
}
