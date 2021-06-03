using System;

namespace GameLibrary.Settlement.Characteristic
{
    [Serializable]
    public class ASettlementCharacteristicReligion : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Religion; }

        public ASettlementCharacteristicReligion(int value) : base(value) { }
    }
}
