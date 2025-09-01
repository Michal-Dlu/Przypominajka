using Przypominajka.Models;
using Przypominajka.Services;

namespace Przypominajka;

public partial class Form : ContentPage
{
    private int _pierwszaGodzinaPodania = 0;
    private int _wybraneDawkowanie = 0;
    private string _lekGodzina;

    public Form()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await WypiszLeki();
        // Teraz metoda asynchroniczna jest wywo�ywana po za�adowaniu strony
    }

    public object LekDoUsuniecia { get; private set; }

    private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedIndex = picker.SelectedIndex;
        _wybraneDawkowanie = selectedIndex;

        if (picker.SelectedIndex != -1)
        {
            var selectedDawkowanie = picker.SelectedItem.ToString();  // Pobierz wybran� opcj�
            Console.WriteLine($"Wybrana godzina: {selectedDawkowanie}:00");
        }
        else
        {
            // Gdy nie wybrano �adnej opcji, mo�esz zaktualizowa� UI lub wy�wietli� komunikat
            Console.WriteLine("Prosz� wybra� godzin�");
        }
    }

    private void GodzinaPodania_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        GodzinaPodaniaLabel.Text = $"Pierwsza Godzina Podania: {e.NewValue}:00";
        _pierwszaGodzinaPodania = (int)(e.NewValue);
    }

    private string GetWybranyLek()
    {
        var _wybranyLek = Lek.Text;
        if (string.IsNullOrWhiteSpace(_wybranyLek))
        {
            _wybranyLek = "Nie wpisano nazwy leku";
            return "";
        }
        return _wybranyLek;
    }
    private async void OnButtonClicked(object sender, EventArgs e)
    {
        var pierwszaGodzinaPodania = _pierwszaGodzinaPodania;
        var wybraneDawkowanie = _wybraneDawkowanie;
        int[] mnoznik = new int[] { 1, 2, 3, 4, 6, 8, 12, 24 };
        int wybranyMnoznik = mnoznik[_wybraneDawkowanie];
        int liczbaDawkNaDobe = wybranyMnoznik;
        int[] godziny_podania = new int[wybranyMnoznik];
        godziny_podania[0] = _pierwszaGodzinaPodania;

        for (int i = 0; i < liczbaDawkNaDobe; i++)
        {
            godziny_podania[i] = (_pierwszaGodzinaPodania + i * (24 / wybranyMnoznik)) % 24;
        }

        // Tworzymy instancj� bazy danych
        var databaseService = new DatabaseService("leki.db");
        // Tworzymy nowy obiekt Lek
        var nowylek = new Lek()
        {
            nazwa = GetWybranyLek(),
            dawkowanie = wybraneDawkowanie.ToString()
        };
        databaseService.DodajLek(nowylek);
        // Dodajemy godziny podania do bazy danych
        foreach (var godzina in godziny_podania)
        {

            var godzinaObj = new GodzinyPodania()
            {
                godzina = godzina,
                LekId = nowylek.Id // Ustawiamy Id leku, kt�ry zosta� dodany do bazy danych
            };

            databaseService.DodajGodzinePodania(godzinaObj, nowylek);
            var page = ((App)Application.Current).Services.GetService<MainPage>();
            await Navigation.PushAsync(page);
            //Navigation.PushAsync(new MainPage());
        }

        // Wy�wietlamy komunikat o klikni�ciu przycisku
        Console.WriteLine("Przycisk zosta� klikni�ty!");
        await WypiszLeki();
    }

    private async Task WypiszLeki()
    {
        DatabaseService databaseService = new DatabaseService("leki.db");
        List<Lek> leki = await databaseService.PobierzLek();  // Pobranie danych z bazy
        List<GodzinyPodania> godzinyPodania = await databaseService.PobierzGodzinyPodania();  // Pobranie godzin podania
        List<LekGodzina> lekiGodziny = new List<LekGodzina>();
        // Tworzymy instancj� serwisu bazy danych
        Console.WriteLine("Aktualna lista lek�w w bazie:");
        foreach (var lek in leki)
        {
            Console.WriteLine($"Id: {lek.Id}, nazwa: {lek.nazwa}, dawkowanie: {lek.dawkowanie}");
        }
        foreach (var lek in leki)
        {
            var godzinyDlaLeku = godzinyPodania
                .Where(g => g.LekId == lek.Id)
                .Select(g => g.godzina)  // Pobieramy tylko godziny
                .OrderBy(g => g)
                .Select(g => g.ToString() + ":00")
                .ToList();
            lekiGodziny.Add(new LekGodzina
            {
                nazwa = lek.nazwa,
                godziny = godzinyDlaLeku
            });// Ka�dy lek ma swoj� unikaln� list� godzin
        }
        LekiPicker.ItemsSource = lekiGodziny.Select(l => l.nazwa).ToList();

    }

    private async void LekDoUsuniecia_SelectedIndexChanged(object sender, EventArgs e)
    {
        DatabaseService databaseService = new DatabaseService("leki.db");
        List<Lek> leki = await databaseService.PobierzLek();  // Pobranie danych z bazy
        List<GodzinyPodania> godzinyPodania = await databaseService.PobierzGodzinyPodania();  // Pobranie godzin podania
        List<LekGodzina> lekiGodziny = new List<LekGodzina>();

        var picker = (Picker)sender;

        var selectedLek = picker.SelectedItem as string;
        if (selectedLek != null)
        {
            _lekGodzina = selectedLek;
        }
        else
        {
            Console.WriteLine("Prosz� wybra� lek do usuni�cia");
        }
    }
    private async void DeleteButtonClicked(object sender, EventArgs e)
    {

        var wybranyLek = _lekGodzina;

        // Tworzymy instancj� bazy danych
        var databaseService = new DatabaseService("leki.db");
        List<Lek> leki = await databaseService.PobierzLek();  // Pobranie danych z bazy
        List<GodzinyPodania> godzinyPodania = await databaseService.PobierzGodzinyPodania();  // Pobranie godzin podania
        List<LekGodzina> lekiGodziny = new List<LekGodzina>();


        var Id = leki.Where(l => l.nazwa == wybranyLek).Select(l => l.Id).FirstOrDefault();
        var LekId = Id;
        databaseService.UsunLek_i_GodzinePodania(LekId);

        // Aktualizujemy list� lek�w
        await WypiszLeki();

        Console.WriteLine($"Lek {wybranyLek} zosta� usuni�ty.");
        var page = ((App)Application.Current).Services.GetService<MainPage>();
        await Navigation.PushAsync(page);
    }

}





