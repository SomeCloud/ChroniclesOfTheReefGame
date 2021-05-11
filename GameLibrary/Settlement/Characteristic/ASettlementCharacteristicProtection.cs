
namespace GameLibrary.Settlement.Characteristic
{
    public class ASettlementCharacteristicProtection: ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Protection; }

        public ASettlementCharacteristicProtection(int value) : base(value) { }
    }
}
