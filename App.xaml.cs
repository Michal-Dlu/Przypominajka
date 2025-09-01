namespace Przypominajka
{
    public partial class App : Application
    {
       public IServiceProvider Services { get; }
            public App(IServiceProvider serviceProvider)
            {
                InitializeComponent();
                Services = serviceProvider;
                
                MainPage = new NavigationPage(serviceProvider.GetRequiredService<MainPage>());
            }
        }
    }
