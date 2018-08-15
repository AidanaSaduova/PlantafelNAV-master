using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

namespace PlantafelNAV.ViewModel
{

    

    public class MainViewModel : ViewModelBase
    {
        private static readonly MainViewModel instance = new MainViewModel();
        public static MainViewModel Instance
        {
            get { return instance; }
        }

        private ViewModelBase currentView;
       


        //

        public ViewModelBase CurrentView
        {
            get { return currentView; }
            set { currentView = value;
                RaisePropertyChanged();
            }
        }


        public RelayCommand BtnPlantafel { get; set; }
        public RelayCommand BtnMitarbeiter { get; set; }
   

        public MainViewModel()
        {
         //Buttons to switch between Plantafel and Mitarbeiter views
          BtnPlantafel = new RelayCommand(()=> {CurrentView = SimpleIoc.Default.GetInstance<PlantafelVm>(); });
          BtnMitarbeiter = new RelayCommand(()=> {CurrentView = SimpleIoc.Default.GetInstance<MitarbeiterVm>(); });
            
        }

    }
}