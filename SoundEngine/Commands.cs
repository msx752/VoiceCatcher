using System;
using System.Collections.Generic;
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
            Command c = new Command();
            c.Description = new List<string>() { "hesap", "makinesi" };
            //c.Exec()= new Exec() burası method :D:D:D:D:D:D::D
            //{
                //misal kod buraya
            //}
            cmd.Add(c);
        }

        /// <summary>
        /// CallCoommand
        /// </summary>
        /// <param name="CallCoommand">The element type of the array</param>
        public bool CallCoommand(string _FindCommandWithText)
        {

            return true;
            return false;//sonuca göre
        }

        #region Genel Komut Tanımlamaları
        public class Command : IDisposable
        {
            public CommandState State { get; set; }
            public List<string> Description { get; set; }

            public async void Exec(params string[] values)
            {

                int intResult = await Execute();
                this.State = CommandState.Stopped;
            }

            async Task<int> Execute()
            {
                do
                {

                } 
                while (this.State == CommandState.Waiting);
                this.State = CommandState.Running;


                //işlem burada yapılacak burası her command sınıfı için farklı olacak
                //fakat List<> içerisinde eklenen her değişken için bu methoda nasıl değer atayaceğız sıkıntı ??
                int hours = 0;
                return hours;
            }

            public void Dispose()
            {

            }
        }

        public enum CommandState
        {
            Running = 1,
            Stopped = 2,
            Waiting = 3
        }

        #endregion

    }
}
