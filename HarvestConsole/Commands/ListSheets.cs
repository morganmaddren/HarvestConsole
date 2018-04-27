using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Commands
{
    class ListSheets : CommandBase
    {
        public override string Name { get { return "sheets"; } }

        public override string Description { get { return "Lists the releases of card sets present in the spreadsheets directory"; } }
        
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
        };
        
        protected override void ExecuteInternal(ParameterSet parameters)
        {
            Console.WriteLine();
            Console.WriteLine("Available sheets:");
            foreach (var name in Context.SpreadsheetManager.SpreadsheetNames)
            {
                Console.WriteLine("  " + name);
            }
            Console.WriteLine();
        }
    }
}
