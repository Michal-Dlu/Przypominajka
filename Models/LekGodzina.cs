

namespace Przypominajka.Models
{
    public class LekGodzina
    {
        public string? nazwa { get; set; }
        public List<string>? godziny { get; set; }

        // Właściwość, która łączy godziny w jeden ciąg
       // public string GodzinyText => $"{nazwa} - Godziny podania: " + string.Join(", ", godziny.Select(g => g.ToString()));
        public string GodzinyText =>
            $"{nazwa ?? "Brak nazwy"} - Godziny podania: " +
            (godziny != null && godziny.Any()
                ? string.Join(", ", godziny)
                : "Brak godzin");
    }
}
