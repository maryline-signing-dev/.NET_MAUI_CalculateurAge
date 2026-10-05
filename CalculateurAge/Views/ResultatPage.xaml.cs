

namespace CalculateurAge.Views;

[QueryProperty(nameof(Nom), "nom")]
[QueryProperty(nameof(Age), "age")]
[QueryProperty(nameof(Majorite), "majorite")]
[QueryProperty(nameof(AgeDetaille), "ageDetaille")]
[QueryProperty(nameof(JourNaissance), "jourNaissance")]
[QueryProperty(nameof(ProchainAnniv), "prochainAnniv")]
[QueryProperty(nameof(ErreurDate), "erreur")]
public partial class ResultatPage : ContentPage
{
    public string? Nom { get; set; }
    public string? Age { get; set; }
    public string? Majorite { get; set; }
    public string? AgeDetaille { get; set; }
    public string? JourNaissance { get; set; }
    public string? ProchainAnniv { get; set; }
    public string? ErreurDate { get; set; }

    public ResultatPage() => InitializeComponent();

    protected override void OnAppearing()
    {
        base.OnAppearing();

        lblMessage.Text = $"{Nom}, vous avez {Age} ans";
        lblAgeDetaille.Text = $" Exactement, {AgeDetaille}";
        lblJourNaissance.Text = JourNaissance;
        lblProchainAnniv.Text = ProchainAnniv;
        lblMessageMajorite.Text = $"Vous êtes {Majorite}";
 
    }

    private async void OnRetourClicked(object s, EventArgs e)
        => await Shell.Current.GoToAsync("..");
}