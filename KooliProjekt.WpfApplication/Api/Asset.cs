namespace KooliProjekt.WpfApplication
{
    // 09.04.2026 - API mudel kopeeritud Windows Formsi projektist
    // 10.04.2026 - mudel kasutab NotifyPropertyChanged baasklassi
    public class Asset : NotifyPropertyChangedBase
    {
        private int _id;
        private int _assetClassID;
        private string _name;
        private string _ticker;
        private bool _isRealEstate;
        private string _assetClassName;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public int AssetClassID
        {
            get
            {
                return _assetClassID;
            }
            set
            {
                _assetClassID = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public string Ticker
        {
            get
            {
                return _ticker;
            }
            set
            {
                _ticker = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsRealEstate
        {
            get
            {
                return _isRealEstate;
            }
            set
            {
                _isRealEstate = value;
                NotifyPropertyChanged();
            }
        }

        public string AssetClassName
        {
            get
            {
                return _assetClassName;
            }
            set
            {
                _assetClassName = value;
                NotifyPropertyChanged();
            }
        }
    }
}
