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
        int comNumber;
        public string[]? args;

        public int ComNumber
        {
            get { return comNumber; }
        }

        public Command(int commandNumber, string[]? args = null)
        {
            comNumber = commandNumber;
            if (args == null) this.args = null;
            else this.args = args;
        }

        public string Encrypt()
        {
            string command = comNumber + ";";
            if (args == null) return command;
            return command + string.Join(";", args);
        }

        public static Command? Decrypt(string str)
        {
            int Number;
            string[] commands = str.Split(';', StringSplitOptions.RemoveEmptyEntries);
            if (commands == null || commands.Length == 0)
                return null;
            if (commands.Length == 1 & !int.TryParse(commands[0], out Number))
                return null;

            string[] args = commands[1..commands.Length];
            Debug.WriteLine("Decrypter: Command = " + Number + ". args count = " + args.Length+ " | "+ string.Join(" ",args));

            if (args.Length > 0)
                return new Command(Number, args);
            return new Command(Number);

        }
    }
}
