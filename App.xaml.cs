using Microsoft.Extensions.DependencyInjection;

namespace Minhas_Compras
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new View.ListaProduto());
        }

      
    }
}