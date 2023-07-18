using SharedKernel.Core;
using SharedKernel.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.ValueObjects
{
    public record CantidadValue : ValueObject
    {
        public int Value { get; }

        public CantidadValue(int value)
        {
            CheckRule(new NotNullRule(value));
            if (value < 0)
            {
                throw new BussinessRuleValidationException("No puede ser menor a 0");
            }
            Value = value;
        }

        public static implicit operator int(CantidadValue value)
        {
            return value.Value;
        }

        public static implicit operator CantidadValue(int value)
        {
            return new CantidadValue(value);
        }
    }
}
