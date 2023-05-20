using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShowStopper.Models
{
    public abstract class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _id;
        public string Id
        {
            get => _id;
            set
            {
                if (_id == value) return;   
                _id = value;
                HandlePropertyChanged();
            }
        }

        protected void HandlePropertyChanged([CallerMemberName] string propertyName = "")
        {
            var eventArgs = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, eventArgs);
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity other) return Id == other.Id;
            return false;
        }
    }
}
