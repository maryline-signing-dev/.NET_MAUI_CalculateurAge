using CalculateurAge.Views;
using System.Threading.Tasks;

namespace CalculateurAge
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        // Gestionnaire appele au clic du bouton Calculer
        // sender=le controle clique; e=donnees de l evenement

        //en B5 on passe de private async Task (A2) à private async void
        private async void  OnCalculerClicked(object sender, EventArgs e)
        {
            //Validation : on refuse un nom vide.
            if (string.IsNullOrWhiteSpace(entryNom.Text))
            {
                DisplayAlert("Erreur", "Entrez un nom", "OK");
                return; //on sort sans rien calculer
            }
            DateTime d = pickerDate.Date;
            int age = DateTime.Today.Year - d.Year;
            //Si l'anniversaire n'est pas encore passe cette annee, on retire une annee
            if (d.Date > DateTime.Today.AddYears(-age))
                age--;

            //on ecrit directement en A2dans les controles: c est precisement ce que le MVVM va supprimer
            //lblResultat.Text = $"{entryNom.Text}, vous avez {age} ans";
            //lblResultat.IsVisible = true;

            //en B5 les deux lignes ci-dessus sont remplacés
            await Shell.Current.GoToAsync($"{nameof(ResultatPage)}?nom={entryNom.Text}&age={age}");

        }
    }
}
