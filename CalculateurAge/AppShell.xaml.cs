using CalculateurAge.Views;

namespace CalculateurAge
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Declaration de la roue: sans cette sihnoe, GoToAsync lève une exception "route inconnu"
            Routing.RegisterRoute(nameof(ResultatPage), typeof(ResultatPage));
        }
    }
}
