

namespace CalculateurAge.Views;

[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
[QueryProperty(nameof(Majorite), "majorite")]
[QueryProperty(nameof(ErreurDate), "erreur")]
public partial class ResultatPage : ContentPage
{
    public string? Nom { get; set; }
    public string? Age { get; set; }
    public string? Majorite { get; set; }
    public string? ErreurDate { get; set; }

    public ResultatPage() => InitializeComponent();

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Si une erreur existe, on affiche uniquement l'erreur
        if (!string.IsNullOrEmpty(ErreurDate))
        {
            lblMessage.Text = "";
            lblMessageMajorite.Text = "";
            erreurMessage.Text = ErreurDate;

            return;
        }
        else
        {
            // Sinon, on affiche le résultat normal
            erreurMessage.Text = "";
            lblMessage.Text = $"{Nom}, vous avez {Age} ans";
            lblMessageMajorite.Text = $"Vous êtes {Majorite}";
        }

    }

    private async void OnRetourClicked(object s, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}