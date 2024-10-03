namespace AzureTranslatorWeb.Models
{
    // Clase para representar una traducción realizada por la API.
    public class Translation
    {
        public string Text { get; set; } // Texto traducido.
        public string To { get; set; } // Idioma de destino al que se tradujo.
    }
}
