using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Settlement.Characteristic
{
    public class ASettlementCharacteristicFood : ASettlementCharacteristic, ISettlementCharacteristic
    {
        public override ASettlementCharacteristicType SettlementCharacteristicType { get => ASettlementCharacteristicType.Food; }

        public ASettlementCharacteristicFood(int value) : base(value) { }
    }
}
