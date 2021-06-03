using System;

namespace GameLibrary.Settlement.Characteristic
{
    [Serializable]
    class ASettlementCharacteristicMedicine : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Medicine; }

        public ASettlementCharacteristicMedicine(int value) : base(value) { }
    }
}
