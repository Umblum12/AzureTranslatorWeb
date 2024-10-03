namespace AzureTranslatorWeb.Models
{
    // Clase para almacenar los mensajes del chat.
    public class ChatMessage
    {
        public string UserMessage { get; set; } // Mensaje original del usuario.
        public string TranslatedMessage { get; set; } // Mensaje traducido por la API.
    }
}
