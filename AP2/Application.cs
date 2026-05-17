using System;
using System.Collections.Generic;
using System.IO;

public class Vehicule
{
    public string Marque { get; set; }
    public string Modele { get; set; }
    public int Prix { get; set; }
    public string Immatriculation { get; set; }
    public string Etat { get; set; }
    public int Km { get; set; }

    public Vehicule(string marque, string modele, int prix, string immatriculation, string etat, int km)
    {
        Marque = marque;
        Modele = modele;
        Prix = prix;
        Immatriculation = immatriculation;
        Etat = etat;
        Km = km;
    }

    public virtual void AfficherDetails()
    {
        Console.WriteLine($"Marque : {Marque} ");
        Console.WriteLine($"Modele : {Modele} ");
        Console.WriteLine($"Prix : {Prix}€ ");
        Console.WriteLine($"Immatriculation : {Immatriculation} ");
        Console.WriteLine($"Kilométrage : {Km}");
        Console.WriteLine($"Etat : {Etat} ");
    }
}

public class Voiture : Vehicule
{
    public int Nbr_places { get; set; }
    public bool Climatisation { get; set; }

    public Voiture(string marque, string modele, int prix, string immatriculation, string etat, int km, int nbr_places, bool climatisation) 
        : base(marque, modele, prix, immatriculation, etat, km)
    {
        Nbr_places = nbr_places;
        Climatisation = climatisation;
    }

    public override void AfficherDetails()
    {
        Console.WriteLine("🚗 : BIP BIP !!");
        base.AfficherDetails();
        Console.WriteLine(Climatisation ? "Il y a la climatisation" : "Il n'y a pas de climatisation");
        Console.WriteLine($"Le nombre de places est de: {Nbr_places}");
    }
}

public class Moto : Vehicule
{
    public int Cylindrée { get; set; }
    public string Permis { get; set; }

    public Moto(string marque, string modele, int prix, string immatriculation, string etat, int km, int cylindrée, string permis) 
        : base(marque, modele, prix, immatriculation, etat, km)
    {
        Cylindrée = cylindrée;
        Permis = permis;
    }

    public override void AfficherDetails()
    {
        Console.WriteLine("🛵 : TUT TUT !!");
        base.AfficherDetails();
        Console.WriteLine($"La cylindrée est de : {Cylindrée}");
        Console.WriteLine($"Permis requis : {Permis}");
    }
}

public class CampingCar : Vehicule
{
    public double Longueur { get; set; }
    public int Capacité_couchage { get; set; }

    public CampingCar(string marque, string modele, int prix, string immatriculation, string etat, int km, double longueur, int capacité_couchage) 
        : base(marque, modele, prix, immatriculation, etat, km)
    {
        Longueur = longueur;
        Capacité_couchage = capacité_couchage;
    }

    public override void AfficherDetails()
    {
        Console.WriteLine("🚙: VROUM BRRRR !!");
        base.AfficherDetails();
        Console.WriteLine($"La longueur est de : {Longueur}m");
        Console.WriteLine($"Capacité de couchage : {Capacité_couchage}");
    }
}

public class Concessionnaire
{
    public string Nom;
    private List<Vehicule> _vehicule { get; set; }

    public Concessionnaire(string nom)
    {
        Nom = nom;
        _vehicule = new List<Vehicule>();
    }

    public void AjouterVehicule(Vehicule v)
    {
        _vehicule.Add(v);
        Console.WriteLine($"Véhicule {v.Immatriculation} ajouté à {Nom}.");
    }

    public void ChargerDepuisCSV(string chemin)
    {
        if (!File.Exists(chemin)) return;
        var lignes = File.ReadAllLines(chemin);

        for (int i = 1; i < lignes.Length; i++)
        {
            var inf = lignes[i].Split(';');
            if (inf.Length < 7) continue;

            string type = inf[0];
            string immat = inf[1];
            string marque = inf[2];
            string modele = inf[3];
            int prix = int.Parse(inf[4]);
            int km = int.Parse(inf[5]);
            string etat = inf[6];

            Vehicule v = null;
            switch (type)
            {
                case "Voiture":
                    int places = int.Parse(inf[7]);
                    // Correction "Oui/Non"
                    bool clim = inf[8].Trim().Equals("Oui", StringComparison.OrdinalIgnoreCase);
                    v = new Voiture(marque, modele, prix, immat, etat, km, places, clim);
                    break;
                case "Moto":
                    int cyl = int.Parse(inf[7]);
                    v = new Moto(marque, modele, prix, immat, etat, km, cyl, inf[8]);
                    break;
                case "CampingCar":
                    double longu = double.Parse(inf[7]);
                    int couch = int.Parse(inf[8]);
                    v = new CampingCar(marque, modele, prix, immat, etat, km, longu, couch);
                    break;
            }
            if (v != null) _vehicule.Add(v);
        }
    }

    public void Listertoutlesvéhicules()
    {
        foreach (Vehicule v in _vehicule)
        {
            v.AfficherDetails();
            Console.WriteLine("==========================================");
        }
    }

    public List<Vehicule> FiltrerParEtat(string etatRecherche)
    {
        return _vehicule.FindAll(v => v.Etat.Equals(etatRecherche, StringComparison.OrdinalIgnoreCase));
    }

    public List<Vehicule> FiltrerParType(string typeRecherche)
    {
        return _vehicule.FindAll(v => v.GetType().Name.Equals(typeRecherche, StringComparison.OrdinalIgnoreCase));
    }

    public List<Vehicule> FiltrerParEtatType(string etatRecherche, string typeRecherche)
    {
        return _vehicule.FindAll(v => v.Etat.Equals(etatRecherche, StringComparison.OrdinalIgnoreCase) 
                                   && v.GetType().Name.Equals(typeRecherche, StringComparison.OrdinalIgnoreCase));
    }

    public int indicateur_financier()
    {
        int total = 0;
        foreach (Vehicule v in _vehicule)
        {
            if (v.Etat.Equals("Disponible", StringComparison.OrdinalIgnoreCase))
                total += v.Prix;
        }
        return total;
    }

    public double CalculerDevis(Vehicule v, int jours)
    {
        double devis = v.Prix * jours;
        if (jours > 7) devis *= 0.9; 
        return devis;
    }

    public void ExportCSV(string chemin)
    {
        List<string> lignes = new List<string> { "Type;Immatriculation;Marque;Modele;Prix;Km;Etat;Divers1;Divers2" };
        foreach (Vehicule v in _vehicule)
        {
            if (v.Etat.Equals("Disponible", StringComparison.OrdinalIgnoreCase))
            {
                string ligne = v switch
                {
                    Voiture vt => $"Voiture;{v.Immatriculation};{v.Marque};{v.Modele};{v.Prix};{v.Km};{v.Etat};{vt.Nbr_places};{(vt.Climatisation ? "Oui" : "Non")}",
                    Moto m => $"Moto;{v.Immatriculation};{v.Marque};{v.Modele};{v.Prix};{v.Km};{v.Etat};{m.Cylindrée};{m.Permis}",
                    CampingCar c => $"CampingCar;{v.Immatriculation};{v.Marque};{v.Modele};{v.Prix};{v.Km};{v.Etat};{c.Longueur};{c.Capacité_couchage}",
                    _ => ""
                };
                if (ligne != "") lignes.Add(ligne);
            }
        }
        File.WriteAllLines(chemin, lignes);
        Console.WriteLine($"Export réussi : {chemin}");
    }
}

public class Program
{
    public static void Main()
    {
        Concessionnaire concessionnaire = new Concessionnaire("LL&IBN");
        concessionnaire.ChargerDepuisCSV("flotte.csv");

        bool continuer = true;
        while(continuer)
        {
            Console.WriteLine("\n--- MENU ---");
            Console.WriteLine("1 - Afficher tous les véhicules");
            Console.WriteLine("2 - Afficher les disponibles");
            Console.WriteLine("3 - Filtrer par type");
            Console.WriteLine("4 - Indicateur financier");
            Console.WriteLine("5 - Export CSV");
            Console.WriteLine("6 - Quitter");
            Console.Write("Votre choix : ");
            
            string choix = Console.ReadLine() ?? "";
            
            switch (choix)
            {
                case "1":
                    concessionnaire.Listertoutlesvéhicules();
                    break;
                case "2":
                    var dispos = concessionnaire.FiltrerParEtat("Disponible");
                    foreach (var v in dispos) { v.AfficherDetails(); Console.WriteLine("---------"); }
                    break;
                case "3":
                    Console.Write("Type (Voiture/Moto/CampingCar) : ");
                    string t = Console.ReadLine() ?? "";
                    var filtre = concessionnaire.FiltrerParType(t);
                    foreach (var v in filtre) { v.AfficherDetails(); Console.WriteLine("----------"); }
                    break;
                case "4":
                    Console.WriteLine($"Valeur du parc disponible : {concessionnaire.indicateur_financier()} €");
                    break;
                case "5":
                    concessionnaire.ExportCSV("disponibles.csv");
                    break;
                case "6":
                    continuer = false;
                    Console.WriteLine("BYE BYE");
                    break;
                default:
                    Console.WriteLine("Choix invalide !");
                    break;
            }
        }
    }
}