using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toronto.Concerts.MAUI.ViewModels
{
    public partial class ViewModelBase : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [ObservableProperty]
        bool isBusy;

        [ObservableProperty]
        string title;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
