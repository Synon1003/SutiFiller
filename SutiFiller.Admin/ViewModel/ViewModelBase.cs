using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SutiFiller.Admin.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Üzenet küldésének eseménye.
        public event EventHandler<MessageEventArgs> MessageApplication;

        // Nézetmodell ősosztály példányosítása.
        protected ViewModelBase() { }

        // Tulajdonság változása ellenőrzéssel.
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        // Üzenet küldésének eseménykiváltása.
        protected void OnMessageApplication(String message)
        {
            if (MessageApplication != null)
                MessageApplication(this, new MessageEventArgs(message));
        }
    }
}
