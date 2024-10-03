namespace AzureTranslatorWeb.Models
{
    // Clase para modelar la estructura de la respuesta de la API de Azure Translator.
    public class TranslationResult
    {
        public DetectedLanguage DetectedLanguage { get; set; } // Información sobre el idioma detectado.
        public Translation[] Translations { get; set; } // Lista de traducciones.
    }
}
