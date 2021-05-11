using System;
using System.Collections.Generic;
using System.Text;

namespace GameLibrary.Settlement.Characteristic
{
    public interface ISettlementCharacteristic
    {

        public ASettlementCharacteristicType SettlementCharacteristicType { get; }
        public int Value { get; }
        public void SetValue(int value);
    }
}
