using System;

namespace CommonPrimitivesLibrary
{
    public struct ASize
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Count { get => Width * Height; }

        public ASize(int width = -1, int height = -1) => (Width, Height) = (width, height);

        public override bool Equals(object obj)
        {
            if (obj is ASize point)
                return Width == point.Width && Height == point.Height;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool Equals(ASize size) => Width == size.Width && Height == size.Height ? true : false;

        public static ASize operator +(ASize A, ASize B) => new ASize(A.Width + B.Width, A.Height + B.Height);
        public static ASize operator +(ASize A, int n) => new ASize(A.Width + n, A.Height + n);
        public static ASize operator -(ASize A, ASize B) => new ASize(A.Width - B.Width, A.Height - B.Height);
        public static ASize operator -(ASize A, int n) => new ASize(A.Width - n, A.Height - n);
        public static ASize operator *(ASize A, ASize B) => new ASize(A.Width * B.Width, A.Height * B.Height);
        public static ASize operator *(ASize A, int n) => new ASize(A.Width * n, A.Height * n);
        public static ASize operator /(ASize A, ASize B) => new ASize(A.Width / B.Width, A.Height / B.Height);
        public static ASize operator /(ASize A, int n) => new ASize(A.Width / n, A.Height / n);
        public static bool operator ==(ASize A, ASize B) => A.Equals(B);
        public static bool operator !=(ASize A, ASize B) => !A.Equals(B);

        public APoint ToAPoint() => new APoint(Width, Height);
        public override string ToString() => "( Width: " + Width + ", Height: " + Height + ") ";

        public bool IsValid() => Width != -1 && Height != -1;
    }
}
