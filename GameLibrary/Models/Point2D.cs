using GameLibrary.Interfaces;

namespace GameLibrary.Models
{
    public class Point2D : IPoint
    {
        public float X { get; set; }
        public float Y { get; set; }


        public Point2D(IPoint point)
        {
            X = point.X;
            Y = point.Y;
        }
        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"\r\n\tX:{X}\r\n\tY:{Y}";
        }

        public bool Equals(IPoint other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Point2D)) return false;
            return Equals((Point2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (int)X;
                result = (result * 397) ^ (int)Y;
                return result;
            }
        }
    }
}
