
namespace GameLibrary.Settlement.Characteristic
{
    public class ASettlementCharacteristicReligion : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Religion; }

        public ASettlementCharacteristicReligion(int value) : base(value) { }
    }
}
