using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Map
{
    public class APeople
    {
        public int Age;
        public int Count;

        public APeople(int age, int count) => (Age, Count) = (age, count);

        public override string ToString() => "[ Возраст: " + Age + ", Количество: " + Count + " ]";

    }
}
