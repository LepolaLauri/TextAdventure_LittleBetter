/// <summary>
///  Text Adventure Litte Better version
///  Raaka versio 
/// </summary>

#region Defines 
const int PAIKKOJEN_MAARA = 6;
const int ESINEIDEN_MAARA = 2;
const int ILMANSUUNTIEN_MAARA = 6;


// Alkumääritykset
Paikka[] paikkaData = HaePaikat();
Esine[] esineData = HaeEsine();

// Käyttäjän komentotila

// Mistä paikasta aloitetaan
int nykyinen_paikka = 1;

// Määritetään poistumismuuttuja
bool exit = true;

// Määritetään paikan 1 ovi -> onko auki vai ei
bool paikka1Ovi = false;

// Määritetään paikan 4 salanappi -> onko löydetty
bool paikka4nappi = false;

// Määritetään paikan 4 salanappi -> onko painettu
bool paikka4nappipainettu = false;

#endregion

#region Main
do
{
    // Hae paikan kuvaus
    Console.WriteLine(paikkaData[nykyinen_paikka].paikanKuvaus);

    // Hae ilmansuunnat mihin voi kulkee
    Console.WriteLine(HaeKulkuSuunnat(paikkaData[nykyinen_paikka].paikanIlmansuunnat));

    // Hae paikan esineet, jos on
    HaePaikanEsineet(esineData, nykyinen_paikka);

    // Tee satunnaisten tapahtumien tarkistus paikalle
    int ticks = System.Environment.TickCount;
    Random rnd = new Random(ticks);
    int nix = rnd.Next(1, 100);

    // Satunnainen kissa liikkuu
    bool kissa = false;
    if (nix < 20)
    {
        Console.WriteLine("Täällä liikkuu kissa.");
        kissa = true;
    }

    // Näytetään "komentotulkki" ja odotetaan käyttäjän komentoa
    Console.Write("> ");
    string komentoRivi = Console.ReadLine();
    while (komentoRivi.Length == 0)
    {
        Console.Write("> ");
        komentoRivi = Console.ReadLine();
    }
    komentoRivi = komentoRivi.ToUpper(); // Kaikki teksti isoilla kirjaimilla
     
    // Jaetaan käyttäjän komento yksi sanaisiin ja kaksi sanaisiin (erotusmerkkinä välilyönti)
    string[] komennot = komentoRivi.Split(' ');
    if (komennot.Length == 1 && komennot[0].Length > 2)
    {
        string ekaKomento = komennot[0].Substring(0, 3);
        // Exit komento
        if (ekaKomento == "EXI") exit = false;


        else if (ekaKomento == "POH")
            Liikkuminen(Ilmansuunnat.POHJOINEN, ref nykyinen_paikka, paikkaData);
        else if (ekaKomento == "ITÄ")
            Liikkuminen(Ilmansuunnat.ITÄ, ref nykyinen_paikka, paikkaData);
        else if (ekaKomento == "ETE")
            Liikkuminen(Ilmansuunnat.ETELÄ, ref nykyinen_paikka, paikkaData);
        else if (ekaKomento == "LÄN")
            Liikkuminen(Ilmansuunnat.LÄNSI, ref nykyinen_paikka, paikkaData);
        else if (ekaKomento == "YLÖ")
            Liikkuminen(Ilmansuunnat.YLÖS, ref nykyinen_paikka, paikkaData);
        else if (ekaKomento == "ALA")
            Liikkuminen(Ilmansuunnat.ALAS, ref nykyinen_paikka, paikkaData);

        // Inventaario komento
        else if (ekaKomento == "INV")
        {
            Console.WriteLine("Sinulla on tavarat: ");
            for (int i = 0; i < esineData.Length; i++)
            {
                if (esineData[i].esineenSijainti == 0)
                {
                    Console.WriteLine(esineData[i].esineenKuvaus);
                }
                else { }
            }
        }

        else Console.WriteLine("MITÄ?"); 


    }
    else if (komennot.Length == 2 && komennot[0].Length > 2 && komennot[1].Length > 2)
    {
        string ekaKomento = komennot[0].Substring(0, 3);
        string tokaKomento = komennot[1].Substring(0, 3);

        // Ota komento ja sen käsittely
        if (ekaKomento == "OTA")
        {
            // Komento OTA AVAIN ja esine (avain) on tässä sijainnissa
            if (tokaKomento == "AVA" && esineData[(int)Esineet.AVAIN].esineenSijainti == nykyinen_paikka)
            {
                Console.WriteLine("Otit avaimen.");
                esineData[(int)Esineet.AVAIN].esineenSijainti = 0; // Määritetään avain on itsellään (pistetään taskuun)
            }
            else if (tokaKomento == "AVA") { Console.WriteLine("Täällä ei ole avainta."); } // Ei ole avainta mitä ottaa
            // Komento OTA Veitsi ja esine (veitsi) on tässä sijainnissa
            else if (tokaKomento == "VEI" && esineData[(int)Esineet.VEITSI].esineenSijainti == nykyinen_paikka)
            {
                Console.WriteLine("Otit veitsen.");
                esineData[(int)Esineet.VEITSI].esineenSijainti = 0; // Määritetään avain on itsellään (pistetään taskuun)
            }
            else if (tokaKomento == "KIS" || kissa == true) { Console.WriteLine("Yritit ottaa kissan kiinni mut se on niin nopea. Ei onnistu!"); } // Hassu kissa tapahtuma
            else Console.WriteLine("Haluat ottaa MITÄ?");

        }

        // Pudota komento ja sen käsittely
        else if (ekaKomento == "PUD")
        {
            // Komento PUDOTA AVAIN ja esine 1 (avain) pudotetaan nykyiseen sijaintiin
            if (tokaKomento == "AVA" && esineData[(int)Esineet.AVAIN].esineenSijainti == 0)
            {
                Console.WriteLine("Pudotit avaimen.");
                esineData[(int)Esineet.AVAIN].esineenSijainti = nykyinen_paikka; // Esineen sijainti määritellään nykyiseen paikkaan
            }
            // Komento PUDOTA VEITSI ja esine 2 (veitsi) pudotetaan nykyiseen sijaintiin
            if (tokaKomento == "VEI" && esineData[(int)Esineet.VEITSI].esineenSijainti == 0)
            {
                Console.WriteLine("Pudotit veitsen.");
                esineData[(int)Esineet.VEITSI].esineenSijainti = nykyinen_paikka; // Esineen sijainti määritellään nykyiseen paikkaan
            }
            else if (tokaKomento == "AVA" && esineData[(int)Esineet.AVAIN].esineenSijainti != 0) { Console.WriteLine("Sinulla ei ole avainta jota pudottaa."); } // Pudotetaan esinettä jota ei ole
            else if (tokaKomento == "VEI" && esineData[(int)Esineet.VEITSI].esineenSijainti != 0) { Console.WriteLine("Sinulla ei ole veistä jota pudottaa."); } // Pudotetaan esinettä jota ei ole

            else if (tokaKomento == "KIS" || kissa == true) { Console.WriteLine("Yrität siis pudottaa kissan. Kunhan nyt saisit sen edes kiini, ei onnistu!"); } // Hassu kissa tapahtuma
            else Console.WriteLine("Pudotat MITÄ?"); // Pudotetaan jotain ihmeellistä
        }

        // Avaa komento ja sen käsittely
        else if (ekaKomento == "AVA")
        {
            // Komento AVAA OVI ja paikan sijainti = 1 sekä esine  (avain) on otettu taskuun : Ovea ei ole avattu aiemmin
            if (tokaKomento == "OVI" && nykyinen_paikka == 1 && esineData[(int)Esineet.AVAIN].esineenSijainti == 0 && paikka1Ovi == false)
            {
                Console.WriteLine("Avain kävi oveen ja sait sen auki.");
                paikka1Ovi = true; // Ovi avattu paikassa 1
                paikkaData[nykyinen_paikka].paikanIlmansuunnat[(int)Ilmansuunnat.POHJOINEN] = 2; // Muutetaan paikan 1 kulkeminen pohjoiseen tästä lähtien "auki" eli suoraan paikkaan 2
                paikkaData[nykyinen_paikka].paikanKuvaus = @"Olet omassa huoneessasi Draculan linnassa. On aamu ja näit hänet viimeksi eilen, saatettuaan sinut tänne kammioon. Pohjoiseen oleva ovi on nyt auki.";
            }
            // Komento AVAA OVI ja paikan sijainti = 1 sekä esine (avain) on otettu taskuun : Ovea on avattu aiemmin
            else if (tokaKomento == "OVI" && nykyinen_paikka == 1 && paikka1Ovi == true)
            {
                Console.WriteLine("Olet jo avannut oven ja se nyt pysyvästi auki.");
            }
            // Komento AVAA OVI ja paikan sijainti = 1 sekä esine (avain) ei ole itsellä
            else if (tokaKomento == "OVI" && nykyinen_paikka == 1 && paikka1Ovi == false && esineData[(int)Esineet.AVAIN].esineenSijainti != 0)
            {
                Console.WriteLine("Sinulla ei ole avainta.");
            }
            else if (tokaKomento == "OVI" && nykyinen_paikka != 1) { Console.WriteLine("Täällä ei ole ovea mitä avata."); } // Avataan ovi jota ei ole
            else if (tokaKomento == "KIS" || kissa == true) { Console.WriteLine("Yrität siis avata kissan. Liian raakaa, ei onnistu!"); } // Hassu kissa tapahtuma
            else Console.WriteLine("Haluat avata MITÄ?"); // Avataan jotain ihmeellistä
        }

        // Tutki komento ja sen käsittely
        else if (ekaKomento == "TUT")
        {
            // 
            if (tokaKomento == "HUO" && nykyinen_paikka == 4)
            {
                Console.WriteLine("Tutkit pientä outoa huonetta ja löydät seinästä pienen napin.");
                paikka4nappi = true;
            }
            else if (tokaKomento == "HUO" && nykyinen_paikka != 4) { Console.WriteLine("Tutkit huonetta mutta et näe mitään epätavallista."); }
            else if (tokaKomento == "AVA" && esineData[(int)Esineet.AVAIN].esineenSijainti == 0) { Console.WriteLine("Avain on ihan normaali ja käy omaan huoneesi oveen."); }
            else if (tokaKomento == "KIS" || kissa == true) { Console.WriteLine("Tutkit kissaa ja se on musta."); } // Hassu kissa tapahtuma
            else Console.WriteLine("Tutkit MITÄ?"); // Tutkitaan jotain ihmeellistä
        }

        // Paina komento ja sen käsittely
        else if (ekaKomento == "PAI")
        {
            if (tokaKomento == "NAP" && nykyinen_paikka == 4 && paikka4nappi == true && paikka4nappipainettu == false)
            {
                Console.WriteLine("Painat nappia seinästä ja yht'äkkiä kuuluu kova ääni. Pian lattiasta lähtee salaportaat alaspäin.");
                paikka4nappipainettu = true;
                paikkaData[nykyinen_paikka].paikanIlmansuunnat[(int)Ilmansuunnat.ALAS] = 5;
                paikkaData[nykyinen_paikka].paikanKuvaus = "Olet oudossa pienessä huoneessa. Täällä on salaiset portaat alaspäin";
            }
            else if (tokaKomento == "NAP" && nykyinen_paikka == 4 && paikka4nappi == true && paikka4nappipainettu == true)
            {
                Console.WriteLine("Olet jo painanut nappia. Näyttäisi siltä mitään ei tapahdu enää.");
            }
            else if (tokaKomento == "NAP") { Console.WriteLine("Täällä ei ole nappia mitä painaa."); }
            else Console.WriteLine("Painat MITÄ?"); // Tutkitaan jotain ihmeellistä
        }

        else Console.WriteLine("MITÄ?");
    }
    else { Console.WriteLine("MITÄ?"); } // Käyttäjä antanut enemmän kuin kaksi sanaa MITÄS NYT !



} while (exit) ;

#endregion

#region Functions

static void Liikkuminen(Ilmansuunnat _ilmansuunta, ref int _nykyinenPaikka, Paikka[] _paikka)
{
    if (_paikka[_nykyinenPaikka].paikanIlmansuunnat[(int)_ilmansuunta] == 0)
        Console.WriteLine("Et voi kulkea siihen suuntaan.");
    else if (_paikka[_nykyinenPaikka].paikanIlmansuunnat[(int)_ilmansuunta] == -1)
    {
        if (_nykyinenPaikka == 1)
        {
            Console.WriteLine("Ovi on lukossa.");
        }
    }
    else if (_paikka[_nykyinenPaikka].paikanIlmansuunnat[(int)_ilmansuunta] > 0)
    {
        _nykyinenPaikka = _paikka[_nykyinenPaikka].paikanIlmansuunnat[(int)_ilmansuunta];
    }
    else
    { }
}


static string HaeKulkuSuunnat(int[] ilm)
{
    string kulkuStr = "Voit kulkea: ";

    if (ilm[(int)Ilmansuunnat.POHJOINEN] > 0 ) kulkuStr += "POHJOISEEN ";
    if (ilm[(int)Ilmansuunnat.ITÄ] > 0) kulkuStr += "ITÄÄN ";
    if (ilm[(int)Ilmansuunnat.ETELÄ] > 0) kulkuStr += "ETELÄÄN ";
    if (ilm[(int)Ilmansuunnat.LÄNSI] > 0) kulkuStr += "LÄNTEEN ";
    if (ilm[(int)Ilmansuunnat.YLÖS] > 0) kulkuStr += "YLÖS ";
    if (ilm[(int)Ilmansuunnat.ALAS] > 0) kulkuStr += "ALAS ";

    return kulkuStr;
}

static Paikka[] HaePaikat()
{
    Paikka[] nix = new Paikka[PAIKKOJEN_MAARA];
    nix[0] = new Paikka(0, "", new int[6] { 0, 0, 0, 0, 0, 0 }); // Paikkaa ei ole
    nix[1] = new Paikka(1, @"Olet omassa huoneessasi Draculan linnassa. On aamu ja näit hänet viimeksi eilen, saatettuaan sinut tänne kammioon. Pohjoiseen on ovi.", new int[6] { -1, 0, 0, 0, 0, 0 });
    nix[2] = new Paikka(2, @"Olet käytävällä, jossa muutamien taulujen lisäksi ei näytäisi olevan mitään muuta.", new int[6] { 0, 4, 1, 3, 0, 0 });
    nix[3] = new Paikka(3, @"Olet vieras huoneessa, joka näyttää samalta kuin sinun oma huoneesi.", new int[6] { 0, 2, 0, 0, 0, 0 });
    nix[4] = new Paikka(4, @"Olet oudossa pienessä huoneessa. Se näyttäisi olevan täysin tyhjä.", new int[6] { 0, 0, 0, 2, 0, -1 });
    nix[5] = new Paikka(5, @"ALAKERTA.", new int[6] { 0, 0, 0, 0, 4, 0 });

    return nix;
}

static void HaePaikanEsineet(Esine[] _esine, int _nykyinenPaikka)
{
    string esineet = "Täällä on esineet: ";
    for (int i = 0; i < ESINEIDEN_MAARA; i++)
    {
        if (_esine[i].esineenSijainti == _nykyinenPaikka) esineet += _esine[i].esineenKuvaus + " ";
        else { };
    }
    Console.WriteLine(esineet);
}


static Esine[] HaeEsine()
{
    Esine[] nix = new Esine[ESINEIDEN_MAARA];
    nix[0] = new Esine("Avain", 1);
    nix[1] = new Esine("Veitsi", 3);

    return nix;
}

#endregion

#region Enums
public enum Esineet
{
    AVAIN = 0,
    VEITSI = 1
};

public enum Ilmansuunnat
{
    POHJOINEN = 0,
    ITÄ = 1,
    ETELÄ = 2,
    LÄNSI = 3,
    YLÖS = 4,
    ALAS = 5
};

#endregion

#region Structures
public struct Paikka
{
    public Paikka(int n, string k, int[] i)
    {
        paikanNumero = n;
        paikanKuvaus = k;
        paikanIlmansuunnat = i;
    }
    public int paikanNumero { get; set; }
    public string paikanKuvaus { get; set; }
    public int[] paikanIlmansuunnat { get; set; }

}

public struct Esine
{
    public Esine(string k, int s)
    {
        esineenKuvaus = k;
        esineenSijainti = s;
    }

    public string esineenKuvaus { get; set; }
    public int esineenSijainti { get; set; }    
}

#endregion