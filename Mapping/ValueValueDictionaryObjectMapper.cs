﻿namespace Internals.Mapping
{
    using System.Collections.Generic;
    using Reflection;


    class ValueValueDictionaryObjectMapper<T, TKey, TValue> :
        ObjectMapper<T>
    {
        readonly ReadWriteProperty<T> _property;

        public ValueValueDictionaryObjectMapper(ReadWriteProperty<T> property)
        {
            _property = property;
        }

        public void ApplyTo(T obj, IDictionary<string, object> dictionary)
        {
            object value;
            if (!dictionary.TryGetValue(_property.Property.Name, out value))
                return;

            if (value == null)
                return;

            var values = value as object[];
            if (values == null)
                return;

            var elements = new Dictionary<TKey, TValue>();

            for (int i = 0; i < values.Length; i++)
            {
                var elementArray = values[i] as object[];
                if (elementArray != null)
                {
                    if (elementArray.Length >= 1)
                    {
                        var elementKey = (TKey)elementArray[0];

                        TValue elementValue = default(TValue);
                        if (elementArray.Length >= 2)
                            elementValue = (TValue)elementArray[1];

                        elements[elementKey] = elementValue;
                    }
                }
            }

            _property.Set(obj, elements);
        }
    }
}