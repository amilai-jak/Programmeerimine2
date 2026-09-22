using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KooliProjekt.WpfApplication
{
    // 10.04.2026 - opetaja naitest voetud NotifyPropertyChanged baasklass
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
