/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:PlantafelNAV"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PlantafelNAV.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<MitarbeiterVm>(true);
            SimpleIoc.Default.Register<PlantafelVm>(true);
            SimpleIoc.Default.Register<ArbeitsplatzVm>(true);
            SimpleIoc.Default.Register<ArbeitsplanVm>(true);
            SimpleIoc.Default.Register<APAuslastungVm>(true);
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public MitarbeiterVm Mitarbeiter
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MitarbeiterVm>();
            }
        }

        public PlantafelVm Plantafel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlantafelVm>();
            }
        }

        public ArbeitsplatzVm Arbeitsplatz
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ArbeitsplatzVm>();
            }
        }
        public ArbeitsplanVm Arbeitsplan
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ArbeitsplanVm>();
            }
        }


        public APAuslastungVm APAuslastung
        {
            get
            {
                return ServiceLocator.Current.GetInstance<APAuslastungVm>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}