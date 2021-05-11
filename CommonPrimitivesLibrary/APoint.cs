using System;
using System.Reflection;
using System.Linq;

namespace CommonPrimitivesLibrary
{
    public struct APoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public APoint(int x = 0, int y = 0) => (X, Y) = (x, y);

        public override bool Equals(object obj)
        {
            if (obj is APoint point)
                return X == point.X && Y == point.Y;
            return false;
        }

        public static APoint operator +(APoint A, APoint B) => new APoint(A.X + B.X, A.Y + B.Y);
        public static APoint operator +(APoint A, int n) => new APoint(A.X + n, A.Y + n);
        public static APoint operator -(APoint A, APoint B) => new APoint(A.X - B.X, A.Y - B.Y);
        public static APoint operator -(APoint A, int n) => new APoint(A.X - n, A.Y - n);
        public static APoint operator *(APoint A, APoint B) => new APoint(A.X * B.X, A.Y * B.Y);
        public static APoint operator *(APoint A, int n) => new APoint(A.X * n, A.Y * n);
        public static APoint operator *(APoint A, double n) => new APoint(Convert.ToInt32(A.X * n), Convert.ToInt32(A.Y * n));
        public static APoint operator /(APoint A, APoint B) => new APoint(A.X / B.X, A.Y / B.Y);
        public static APoint operator /(APoint A, int n) => new APoint(A.X / n, A.Y / n);
        public static bool operator ==(APoint A, APoint B) => A.Equals(B);
        public static bool operator !=(APoint A, APoint B) => !A.Equals(B);

        public ASize ToASize() => new ASize(X, Y);
        
        public override string ToString() => "X: " + X + ", Y: " + Y;

        public APoint Abs() => new APoint(Math.Abs(X), Math.Abs(Y));

        public bool IsZero() => X == 0 && Y == 0;
    }
}
