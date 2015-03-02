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

        //gelen yazı değerinin içerisindeki yükleme(yapılacak işe) göre
        //buradan bir seçim yaptırıp önce işlemi belirleriz daha sonra 
        //ney üzerinde çalışacağımızıseçtirirz.
        public List<string> CommandType 
        { get { return new List<string>() 
        { "aç", "kapat", "sil", "taşı", "değiştir" }; } }
        //////////////////////////////////////////////////////
       
        public Thread thrCommand { get; set; }
        public CommandState State { get; set; }

        //burada açıklama yazabiliriz veya yapılacak 
        //işlemi çağırmak için özel isim tanımlaması yapılabilir 
        //şimdilik sadece fikir.
        public List<string> Description { get; set; }

        //burada komutun ismi tanımlanır örn: hesap makinesi
        public string COMMAND { get; set; }
        public void Exec(params string[] values)
        {
            if (this.State == CommandState.Stopped)
            {
                thrCommand = new Thread(doit);
                thrCommand.Start(values);
            }
            else if (this.State == CommandState.Running)
                Debug.WriteLine(string.Format("[{0}] bu komut şuan meşgul durumda halen işleme devam ediyor.", this.COMMAND));
        }
        void doit(object values)
        {
            this.State = CommandState.Running;//işlemin başlangıcı burası
            CommandEventHandler(this, new CommandEventArgs(values as string[]));

            this.thrCommand.Abort();
        }
        public void Dispose()
        {

        }
    }

    public enum CommandState
    {
        Running,
        Stopped
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
