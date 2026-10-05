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
        set => SetField (ref _dateNaissance, value);
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

    // Liee a Button.Command dans le XAML
    public RelayCommand CalculerCommand
    {
        get;
    }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom));
    }

    // Demande à la vue de naviguer — le ViewModel ne navigue pas lui-même
    public event Action<string, string, string>? NavigationDemandee;

    // La logique metier: aucun controle d interface ici
    private void Calculer()
    {
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age))
            age--;

        //Ajout de la fonctionnalité Majorite
        string majorite = age >= 21 ? "Majeur " : "Mineur ";

        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
        Majorite = $"Vous etes {majorite}";

        // Demande à la vue de naviguer (le ViewModel ne navigue pas lui-même)
        NavigationDemandee?.Invoke(Nom, age.ToString(), majorite);

    }


}


