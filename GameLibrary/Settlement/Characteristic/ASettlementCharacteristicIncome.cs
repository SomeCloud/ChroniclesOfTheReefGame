using System;

namespace GameLibrary.Settlement.Characteristic
{
    [Serializable]
    public class ASettlementCharacteristicIncome : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Income; }

        public ASettlementCharacteristicIncome(int value) : base(value) { }
    }
}
