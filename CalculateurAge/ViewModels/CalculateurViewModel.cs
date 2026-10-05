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
    public event Action<string, string, string, string>? NavigationDemandee;

    // La logique metier: aucun controle d interface ici
    private void Calculer()
    {
        // CanExecute garantit qu'on n'arrive jamais ici avec une date future
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age)) age--;

        string majorite = age >= 18 ? "Majeur(e)" : "Mineur(e)";

        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
        Majorite = majorite;
        ErreurDate = "";

        NavigationDemandee?.Invoke(
            (Nom),
            (age.ToString()),
            (majorite),
            "");
    }


}



