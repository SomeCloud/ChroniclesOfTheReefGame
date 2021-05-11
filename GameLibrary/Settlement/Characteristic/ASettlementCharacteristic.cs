using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Settlement.Characteristic
{
    public abstract class ASettlementCharacteristic: ISettlementCharacteristic
    {

        public int _Value;

        public abstract ASettlementCharacteristicType SettlementCharacteristicType { get; }
        public int Value { get => _Value; }

        public ASettlementCharacteristic(int value) => _Value = value;
        public void SetValue(int value) => _Value = value;

    }
}
