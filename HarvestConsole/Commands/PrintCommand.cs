using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Commands
{
    class PrintOptions
    {
        public PrintOptions(bool debug, bool nocount, bool nopic)
        {
            this.Debug = debug;
            this.NoCount = nocount;
            this.NoPic = nopic;
        }

        public bool Debug { get; private set; }
        public bool NoCount { get; private set; }
        public bool NoPic { get; private set; }
    }

    class PrintCommand : CommandBase
    {
        public override string Name { get { return "print"; } }

        public override string Description { get { return "Print PDF for a sheet"; } }

        static readonly Parameter<string> Sheet = new Parameter<string>("sheet");
        static readonly OptionalParameter<bool> Debug = new OptionalParameter<bool>("debug", "False");
        static readonly OptionalParameter<bool> NoCount = new OptionalParameter<bool>("nocount", "False");
        static readonly OptionalParameter<bool> NoPic = new OptionalParameter<bool>("nopic", "False");
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
            Sheet,
            Debug,
            NoCount,
            NoPic,
        };

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var sheetName = parameters.Get(Sheet);
            var debug = parameters.Get(Debug);
            CardDataSpreadsheet sheet = this.Context.SpreadsheetManager.Load(sheetName);
            SheetPrinter printer = new SheetPrinter(this.Context, new PrintOptions(debug, parameters.Get(NoCount), parameters.Get(NoPic)));
            printer.CreatePDF(sheet);

            Console.WriteLine("Done!");
            Console.WriteLine();
        }
    }
}
