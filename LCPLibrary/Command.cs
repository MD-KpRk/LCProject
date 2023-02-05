using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCPLibrary
{
    public class Command
    {
        public int com;
        public string[]? args;

        public Command(int commandNumber, string[]? args = null)
        {
            com = commandNumber;
            if (args == null) this.args = null;
            else this.args = args;
        }

        public string Encrypt()
        {
            StringBuilder str = new StringBuilder();

            str.Append(this.com + ";");
            if (args == null) return str.ToString();
            for (int i = 0; i < args.Length; i++)
                str.Append(args[i] + ";");
            return str.ToString();
        }

        public static Command? Decrypt(string str)
        {
            int com;
            string[] commands = str.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (commands == null || commands.Length == 0)
                return null;
            if (commands.Length == 1 & !int.TryParse(commands[0], out com))
                return null;
            Debug.WriteLine("Decrypter: Command = " + com + ". args count = " + commands.Length);

            if (commands.Length > 1)
            {
                return new Command(com, commands[1..commands.Length]);
            }
            else return new Command(com);

        }
    }
}
