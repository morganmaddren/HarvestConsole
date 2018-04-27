using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole.Commands
{
    class HelpCommand : CommandBase
    {
        public override string Name { get { return "help"; } }

        public override string Description { get { return "help"; } }

        static readonly OptionalParameter<string> Command = new OptionalParameter<string>("command", null);
        protected override List<IParameter> ParamDefinitions { get; set; } = new List<IParameter>()
        {
            Command
        };

        protected override void ExecuteInternal(ParameterSet parameters)
        {
            var cmdName = parameters.Get(Command);
            if (cmdName != null)
            {
                var cmd = Program.Commands.FirstOrDefault(x => x.Name == cmdName);
                if (cmd != null)
                {
                    cmd.PrintHelp();
                }
                else
                {
                    Console.WriteLine("Unrecognized command: " + cmdName);
                }
            }
            else
            {
                foreach (var cmd in Program.Commands)
                {
                    Console.WriteLine(cmd.Name + ": " + cmd.Description);
                }
            }

            Console.WriteLine();
        }
    }
}
