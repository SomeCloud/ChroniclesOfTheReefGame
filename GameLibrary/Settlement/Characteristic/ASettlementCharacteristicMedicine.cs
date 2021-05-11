
namespace GameLibrary.Settlement.Characteristic
{
    class ASettlementCharacteristicMedicine : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Medicine; }

        public ASettlementCharacteristicMedicine(int value) : base(value) { }
    }
}
