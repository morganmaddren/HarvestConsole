using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole
{
    class ParameterSet
    {
        Dictionary<string, string> bindings;
        public string FullLine { get; private set; }

        public ParameterSet(List<IParameter> definitions, List<string> parameters, string line)
        {
            FullLine = line;
            bindings = new Dictionary<string, string>();

            int curDef = 0;
            for (int curParam = 1; curParam < parameters.Count; curParam++)
            {
                var p = parameters[curParam];

                if (p.StartsWith("-")) // named param
                {
                    string pName = p.Substring(1);

                    if (bindings.ContainsKey(pName))
                        throw new InvalidOperationException("Same parameter twice: " + pName);

                    var d = definitions.First(x => x.Name == pName);
                    //if (d as IOptionalParameter == null)
                    //    throw new InvalidOperationException("Only optional parameters may be named");
                    
                    bindings[d.Name] = parameters[curParam + 1];
                    curParam++;
                }
                else // positional param
                {
                    if (definitions.Count > 0)
                    {
                        var d = definitions[curDef];
                        //if (d as IOptionalParameter != null)
                        //    throw new InvalidOperationException("too many non optional params");

                        bindings[d.Name] = p;
                        curDef++;
                    }
                }
            }

            foreach (var d in definitions)
            {
                var optD = d as IOptionalParameter;
                if (optD == null)
                {
                    if (!bindings.ContainsKey(d.Name))
                        throw new InvalidOperationException("missing param: " + d.Name);
                }
                else
                {
                    if (!bindings.ContainsKey(optD.Name))
                        bindings[optD.Name] = optD.DefaultValue;
                }
            }
        }

        public T Get<T>(Parameter<T> p)
        {
            if (this.bindings.ContainsKey(p.Name))
            {
                return (T)ConvertObj(typeof(T), bindings[p.Name]);
            }

            throw new InvalidOperationException("parameter not found: " + p.Name);
        }

        private object ConvertObj(Type type, string input)
        {
            if (type == typeof(string))
                return input;
            if (type == typeof(int))
                return int.Parse(input);
            if (type == typeof(bool))
                return bool.Parse(input);

            throw new InvalidOperationException("invalid type: " + type);
        }
    }
}
