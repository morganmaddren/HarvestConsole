using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestConsole
{
    interface ICommand
    {
        string Name { get; }

        string Description { get; }

        void PrintHelp();

        void Execute(List<string> words, string line, Context context);
    }
}
