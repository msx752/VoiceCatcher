using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vave
{
    #region Genel Komut Tanımlamaları
    public class Command : IDisposable
    {
        public event EventHandler<CommandEventArgs> CommandEventHandler;
        public Command()
        {
            State = CommandState.Stopped;
        }
        public Thread thrCommand
        {
            get { return thrCommand; }
            set
            {
                if (thrCommand != null)
                {
                    thrCommand.Abort();
                    Debug.WriteLine(string.Format("[{0}] thrCommand Sonlanmamış", this.COMMAND));
                }
                thrCommand = value;
            }
        }

        public CommandState State { get; set; }
        public List<string> Description { get; set; }
        public string COMMAND { get; set; }
        public void Exec(params string[] values)
        {
            thrCommand = new Thread(doit);
            thrCommand.Start(values);
        }
        void doit(object values)
        {
            if (this.State == CommandState.Stopped)
            {
                this.State = CommandState.Running;
                CommandEventHandler(this, new CommandEventArgs(values as string[]));
                this.State = CommandState.Stopped;
            }
            else if (this.State == CommandState.Busy)
            {
                Debug.WriteLine(string.Format("[{0}] bu komut şuan meşgul durumda halen işleme devam ediyor.", this.COMMAND));
            }
        }
        public void Dispose()
        {

        }
    }

    public enum CommandState
    {
        Running = 1,
        Stopped = 2,
        Busy = 3
    }

    public class CommandEventArgs : EventArgs
    {
        [DebuggerStepThrough]
        public CommandEventArgs(params string[] _param)
        {
            this.param = _param;

        }
        public string[] param { get; set; }
    }

    #endregion

}
