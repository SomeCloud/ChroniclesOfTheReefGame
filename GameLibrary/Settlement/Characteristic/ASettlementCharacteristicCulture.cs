using System;

namespace GameLibrary.Settlement.Characteristic
{
    [Serializable]
    public class ASettlementCharacteristicCulture : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Culture; }

        public ASettlementCharacteristicCulture(int value) : base(value) { }
    }
}
