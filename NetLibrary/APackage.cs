using System;
using System.Collections.Generic;
using System.Text;

namespace NetLibrary
{
    [Serializable]
    public enum APackageType
    {
        SinglePackage,
        HugePackage
    }

    [Serializable]
    public class APackage
    {

        public int Length { get; }
        public byte[] Data { get; }
        public bool FirstPackage { get; }
        public APackageType PackageType { get; }

        public APackage(int length, byte[] data, APackageType packageType, bool firstPackage)
        {
            Length = length;
            Data = data;
            PackageType = packageType;
            FirstPackage = firstPackage;
        }
        public APackage(int length, byte[] data, APackageType packageType): this(length, data, packageType, false) { }

    }
}
