using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace WPFSeleniumWebScrapper
{
    //Utilizaremos propertychange para esta actualizando los dalores entre el modelo y la interfaz
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        //Variables
        private readonly CollectionView _years;
        private int _MyYear;
        private ObservableCollection<Registro> _Rows { get; set; }
        //Constructor
        public ConnectionViewModel()
        {
            IList<int> l = new List<int>();
            foreach(var i in Enumerable.Range(2000, 81).ToList()) l.Add(i);
            _years = new CollectionView(l);
            _MyYear = DateTime.Now.Year;
        }
        //Objetos
        public CollectionView years
        {
            get { return _years; }
        }
        public int MyYear
        {
            get { return _MyYear; }
            set {
                _MyYear = value;
                OnPropertyChanged("MyYear");
            }
        }
        public ObservableCollection<Registro> Rows
        {
            get { return _Rows; }
            set {
                _Rows = value;
                OnPropertyChanged("Rows");
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (propertyName != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
    public class Registro
    {
        public String? fecha { get; set; }
        public String? hora { get; set; }
        public String? origen { get; set; }
        public String? evento { get; set; }
    }
}