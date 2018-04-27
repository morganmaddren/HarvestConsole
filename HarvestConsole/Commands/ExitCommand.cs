using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Commands
{
    class ExitCommand : CommandBase
    {
        public override string Name { get { return "exit"; } }

        public override string Description { get { return "exit the terminal"; } }

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
            this.Context.IsExiting = true;
        }
    }
}
