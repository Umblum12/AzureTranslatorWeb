using AzureTranslatorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureTranslatorWeb.Controllers
{
    public class TranslationController : Controller
    {
        // Clave de suscripción de Azure y URL del punto de conexión.
        private readonly string subscriptionKey = "d00b2d1f779e4c06b07d4d88b4ce5968"; // Clave de suscripción del servicio de traducción de Azure.
        private readonly string endpoint = "https://api.cognitive.microsofttranslator.com"; // Endpoint de la API de Azure Translator.
        private readonly string location = "eastus"; // Ubicación de la región del recurso en Azure.

        private static List<ChatMessage> chatHistory = new List<ChatMessage>(); // Historial de mensajes del chat.

        [HttpGet]
        public IActionResult Index()
        {
            // Se pasa el historial de chat a la vista para poder mostrarlo.
            ViewBag.ChatHistory = chatHistory;
            return View(); // Se devuelve la vista para que el usuario pueda interactuar con ella.
        }

        [HttpPost]
        public async Task<IActionResult> Index(string textToTranslate, string targetLanguage)
        {
            // Verifica si el usuario ingresó un texto para traducir.
            if (string.IsNullOrEmpty(textToTranslate))
            {
                // Si el texto está vacío, se muestra un mensaje de error.
                ViewBag.Error = "Please enter text to translate.";
                ViewBag.ChatHistory = chatHistory;
                return View(); // Se devuelve la vista con el mensaje de error.
            }

            // Construcción de la ruta para la solicitud a la API con la versión y el idioma de destino.
            string route = $"/translate?api-version=3.0&to={targetLanguage}";
            // Crea el cuerpo de la solicitud con el texto que se desea traducir.
            object[] body = new object[] { new { Text = textToTranslate } };
            // Serializa el cuerpo a formato JSON.
            var requestBody = JsonConvert.SerializeObject(body);

            // Se utiliza `using` para garantizar la correcta liberación de recursos.
            using (var client = new HttpClient()) // Cliente HTTP que se usará para enviar la solicitud.
            using (var request = new HttpRequestMessage()) // Solicitud HTTP que se enviará.
            {
                request.Method = HttpMethod.Post; // Define el método HTTP como POST.
                request.RequestUri = new Uri(endpoint + route); // Establece la URL completa de la solicitud (endpoint + ruta).
                // Define el contenido de la solicitud (el JSON con el texto a traducir) y la codificación.
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                // Añade la clave de suscripción al encabezado de la solicitud.
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                // Añade la región del recurso al encabezado de la solicitud.
                request.Headers.Add("Ocp-Apim-Subscription-Region", location);

                try
                {
                    // Envía la solicitud y espera la respuesta de forma asíncrona.
                    HttpResponseMessage response = await client.SendAsync(request);
                    // Verifica si la respuesta indica éxito (código 200 OK).
                    response.EnsureSuccessStatusCode();

                    // Lee el contenido de la respuesta como una cadena de texto.
                    string result = await response.Content.ReadAsStringAsync();
                    // Deserializa el resultado JSON a un objeto TranslationResult[].
                    var translationResult = JsonConvert.DeserializeObject<TranslationResult[]>(result);

                    // Verifica si la respuesta tiene datos válidos de traducción.
                    if (translationResult != null && translationResult.Length > 0 &&
                        translationResult[0]?.Translations != null && translationResult[0].Translations.Length > 0)
                    {
                        // Extrae el texto traducido.
                        string translatedText = translationResult[0].Translations[0].Text;
                        // Agrega el mensaje del usuario y la traducción al historial de chat.
                        chatHistory.Add(new ChatMessage { UserMessage = textToTranslate, TranslatedMessage = translatedText });
                    }
                    else
                    {
                        // Si la respuesta es inválida o vacía, muestra un mensaje de error.
                        ViewBag.Error = "Translation result is empty or invalid.";
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Manejo de errores de solicitudes HTTP.
                    ViewBag.Error = $"Request failed: {ex.Message}";
                }
                catch (Exception ex)
                {
                    // Manejo de cualquier otro error.
                    ViewBag.Error = $"Deserialization failed: {ex.Message}";
                }
            }

            // Actualiza el historial del chat y retorna la vista.
            ViewBag.ChatHistory = chatHistory;
            return View();
        }
    }
}
