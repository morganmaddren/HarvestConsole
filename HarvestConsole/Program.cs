using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using HarvestConsole.Commands;

namespace HarvestConsole
{
    class Program
    {
        public static readonly List<ICommand> Commands = new List<ICommand>()
        {
            new HelpCommand(),
            new ListSheets(),
            new ExitCommand(),
            new PrintCommand(),
            new LexCommand(),
            new ManaCurveCommand(),
            new BalanceCommand(),
            new BalanceEvalCommand(),
            new ReinvestmentSimCommand(),
        };
        
        static Context context;

        static void Main(string[] args)
        {
            context = new Context();

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                var words = line.SplitAndTrim(' ', StringSplitOptions.RemoveEmptyEntries);
                Evaluate(words, line, context);

                if (context.IsExiting)
                    break;
            }
        }

        static void Evaluate(List<string> words, string line, Context context)
        {
            if (words.Count == 0)
                return;

            var cmd = Commands.FirstOrDefault(x => x.Name == words[0]);
            if (cmd != null)
            {
                //try
                //{
                    cmd.Execute(words, line, context);
                /*}
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    //Console.WriteLine(e.StackTrace);
                }*/
            }
            else
            {
                Console.WriteLine("Unrecognized command: '{0}'", words[0]);
            }
        }
    }
}
