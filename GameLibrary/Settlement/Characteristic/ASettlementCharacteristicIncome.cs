
namespace GameLibrary.Settlement.Characteristic
{
    public class ASettlementCharacteristicIncome : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Income; }

        public ASettlementCharacteristicIncome(int value) : base(value) { }
    }
}
