using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateurAge.ViewModels;

//Contient l ETAT de l ecran et les ACTIONS possibles
public partial class CalculateurViewModel : BaseViewModel
{
    //champs prives : la vraie donnee
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private string _majorite = "";
    private bool _resultatVisible;
    private string _erreurDate = "";
    private string _ageDetaille = "";
    private string _jourNaissance = "";
    private string _prochainAnniversaire = "";

    //proprietes publiques: ce que le XML voit
    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value))
                CalculerCommand.Rafraichir();
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set
        {
            // Verification de la date de naissance
            if (SetField(ref _dateNaissance, value))
            {
                // Refus date future : on recalcule l'erreur à chaque changement
                ErreurDate = value.Date > DateTime.Today
                    ? "La date de naissance ne peut pas être dans le futur."
                    : "";
                CalculerCommand.Rafraichir();
            }
        }
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set => SetField(ref _resultatVisible, value);
    }

    public string Majorite
    {
        get => _majorite;
        set => SetField(ref _majorite, value);
    }

    public string ErreurDate
    {
        get => _erreurDate;
        set
        {
            if (SetField(ref _erreurDate, value))
                OnPropertyChanged(nameof(ErreurVisible));
        }
    }

    public string AgeDetaille
    {
        get => _ageDetaille;
        set => SetField(ref _ageDetaille, value);
    }

    public string JourNaissance
    {
        get => _jourNaissance;
        set => SetField(ref _jourNaissance, value);
    }

    public string ProchainAnniversaire
    {
        get => _prochainAnniversaire;
        set => SetField(ref _prochainAnniversaire, value);
    }
    public bool ErreurVisible => !string.IsNullOrEmpty(ErreurDate);

    // Liee a Button.Command dans le XAML
    public RelayCommand CalculerCommand
    {
        get;
    }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
           // Bloque si nom vide OU date future
            () => !string.IsNullOrWhiteSpace(Nom)
            && DateNaissance.Date < DateTime.Today);
    }

    // Demande à la vue de naviguer — le ViewModel ne navigue pas lui-même
    public event Action<string, string, string, string, string, string, string>? NavigationDemandee;

    // La logique metier: aucun controle d interface ici
    private void Calculer()
    {
        DateTime naissance = DateNaissance.Date;
        DateTime aujourd = DateTime.Today;

        // CanExecute garantit qu'on n'arrive jamais ici avec une date future
        int annees = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-annees)) annees--;

        //age detaille : annee, mois, jour
        int mois = aujourd.Month - naissance.Month;
        int jours = aujourd.Day - naissance.Day;

        if (jours < 0)
        {
            mois--;
            // Nombre de jours du mois précédent
            int moisPrecedent = aujourd.Month == 1 ? 12 : aujourd.Month - 1;
            int anneePrecedente = aujourd.Month == 1 ? aujourd.Year - 1 : aujourd.Year;
            jours += DateTime.DaysInMonth(anneePrecedente, moisPrecedent);
        }
        if (mois < 0)
            annees--; mois += 12;

        AgeDetaille = $" {annees} an{(annees > 1 ? "s" : "")}, " +
              $"{mois} mois et " +
              $"{jours} jour{(jours > 1 ? "s" : "")}";

        //jour de naissance exacte
        string[] joursFr = { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
        JourNaissance = $"Vous etes né(e) un {joursFr[(int)naissance.DayOfWeek]}";

        // Prochain anniversaire
        DateTime prochainAnniv = new DateTime(aujourd.Year, naissance.Month, naissance.Day);
        if (prochainAnniv <= aujourd)
            prochainAnniv = prochainAnniv.AddYears(1);

        int joursRestants = (prochainAnniv - aujourd).Days;
        string moisFr = new[] { "", "janvier", "février", "mars", "avril", "mai", "juin",
                                "juillet", "août", "septembre", "octobre", "novembre", "décembre" }
                            [prochainAnniv.Month];
        ProchainAnniversaire = $"Votre prochain anniversaire est le {prochainAnniv.Day} {moisFr} {prochainAnniv.Year}\n" +
                               $" Il reste donc {joursRestants} jour{(joursRestants > 1 ? "s" : "")}";

        string majorite = annees >= 18 ? "Majeur(e)" : "Mineur(e)";

        Resultat = $"{Nom}, vous avez {annees} ans";
        ResultatVisible = true;
        Majorite = majorite;
        ErreurDate = "";

        NavigationDemandee?.Invoke(
            (Nom),
            (annees.ToString()),
            (majorite),
            (AgeDetaille),
            (JourNaissance),
            (ProchainAnniversaire),
            "");
    }


}



