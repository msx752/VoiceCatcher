using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vave
{
    public class Engine
    {
        List<Command> cmd = new List<Command>();
        public Engine()
        {
            ////////////////////////////////////////////////
            Command c = new Command();//komut tanımlama
            c.COMMAND = "hesap makinesi";
            c.Description = new List<string>() { "hesap", "makinesi" };
            c.CommandEventHandler += c_CommandEventHandler;
            cmd.Add(c);
            ////////////////////////////////////////////////
            c = new Command();
            c.COMMAND = "not defteri";
            c.Description = new List<string>() { "not", "defteri" };
            c.CommandEventHandler += c_CommandEventHandler;
            cmd.Add(c);
            ////////////////////////////////////////////////
        }

        void c_CommandEventHandler(object sender, CommandEventArgs e)
        {
            Command _sender = sender as Command;
            switch (_sender.COMMAND)
            {
                case "hesap makinesi":

                    break;
                case "not defteri":

                    break;
            }
        }


        /// <summary>
        /// CallCoommand
        /// </summary>
        /// <param name="CallCoommand">The element type of the array</param>
        public List<Command> CallCoommand(string _FindCommandWithText)
        {
            List<Command> SONUC = cmd.FindAll(x => x.COMMAND == _FindCommandWithText || x.Description.IndexOf(_FindCommandWithText) != -1);
            return SONUC;
        }
    }
}
