
namespace GameLibrary.Settlement.Characteristic
{
    public class ASettlementCharacteristicScience : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Science; }

        public ASettlementCharacteristicScience(int value) : base(value) { }
    }
}
