using System;

namespace GameLibrary.Settlement.Characteristic
{
    [Serializable]
    public class ASettlementCharacteristicScience : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Science; }

        public ASettlementCharacteristicScience(int value) : base(value) { }
    }
}
