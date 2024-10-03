namespace AzureTranslatorWeb.Models
{
    // Clase para representar el idioma detectado en el texto de entrada.
    public class DetectedLanguage
    {
        public string Language { get; set; } // Código del idioma detectado (e.g., "es" para español).
        public float Score { get; set; } // Confianza de la detección (valor entre 0 y 1).
    }
}
