using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole
{
    interface IParameter
    {
        string Name { get; }
    }

    interface IOptionalParameter : IParameter
    {
        string DefaultValue { get; }
    }

    class Parameter<T> : IParameter
    {
        public Parameter(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }

    class OptionalParameter<T> : Parameter<T>, IOptionalParameter
    {
        public OptionalParameter(string name, string defaultValue)
            : base(name)
        {
            this.DefaultValue = defaultValue;
        }

        public string DefaultValue { get; private set; }
    }
}
