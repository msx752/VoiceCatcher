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
            //hangi nesneye işlem uygulayacağımızı buradan seçtiriyoruz
            //daha sonra önceden belirlenmiş olan işlemi yaptırıyoruz 
            //örn: silme,açma,kapatma vb.
            switch (_sender.COMMAND)
            {
                case "hesap makinesi":

                    break;
                case "not defteri":

                    break;
            }

            _sender.State = CommandState.Stopped;//en mantıklı sonlandırma burada tanımlamak
        }

        /// <summary>
        /// sistemde kayıtlı olan komutlarla karşılaştırma yapılıyor
        /// </summary>
        /// <param name="_FindCommandWithText">The element type of the array</param>
        public List<Command> CallCommand(string _FindCommandWithText)
        {
            List<Command> SONUC = cmd.FindAll(x => x.COMMAND == _FindCommandWithText);
            //if (SONUC != null)
            //{
            //    if (SONUC.Count == 1)
            //    {
            //        return SONUC;
            //    }
            //}
            return SONUC;
        }
    }
}
