using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole
{
    abstract class CommandBase : ICommand
    {
        protected Context Context { get; private set; }

        protected abstract List<IParameter> ParamDefinitions { get; set; }

        public abstract string Name { get; }

        public abstract string Description { get; }

        protected abstract void ExecuteInternal(ParameterSet parameters);
        
        public void Execute(List<string> words, string line, Context context)
        {
            this.Context = context;
            var p = CreateParameters(ParamDefinitions, words, line);
            this.ExecuteInternal(p);
        }

        public void PrintHelp()
        {
            Console.WriteLine(Name + ": " + Description);
            
            Console.Write("  " + Name);
            foreach (IParameter p in ParamDefinitions)
            {
                Console.Write(" ");
                var optP = p as IOptionalParameter;
                if (optP != null)
                    Console.Write("([" + optP.Name + "]=" + optP.DefaultValue + ")");
                else
                    Console.Write("[" + p.Name + "]");
            }

            Console.WriteLine();
        }

        private static ParameterSet CreateParameters(List<IParameter> paramDefs, List<string> words, string line)
        {
            return new ParameterSet(paramDefs, words, line);
        }
    }
}
