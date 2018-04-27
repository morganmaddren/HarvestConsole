using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarvestConsole.Typesetting;

namespace HarvestConsole.Commands
{
    class LexCommand : CommandBase
    {
        public override string Name { get { return "tok"; } }

        public override string Description { get { return "tokenize a string"; } }

        protected override List<IParameter> ParamDefinitions
        {
            get
            {
                return new List<IParameter>();
            }

            set
            {
            }
        }

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var lexed = StringTokenizer.Tokenize(parameters.FullLine.Substring(4));
            foreach (string s in lexed)
            {
                Console.WriteLine("'" + s + "'");
            }
        }
    }
}
