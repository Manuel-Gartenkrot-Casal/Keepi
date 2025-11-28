using Microsoft.AspNetCore.Mvc;
using semantic_kernel.Models;
using semantic_kernel.Services;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Keepi.Models;

namespace semantic_kernel.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatController(IChatService chatService, IHttpContextAccessor httpContextAccessor)
        {
            _chatService = chatService;
            _httpContextAccessor = httpContextAccessor;
        }

       public IActionResult ChatBot()
{
    var chatHistory = GetChatHistory(null); // No agregar mensaje nuevo
    return View(chatHistory);
}

        [HttpGet]
        public IActionResult GetHistory()
        {
            var history = GetChatHistory(null);
            return Json(new { success = true, history = history });
        }



        // Aceptamos el JSON del body explícitamente para requests AJAX
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                // Debug info: content type and model state pueden ayudar a diagnosticar problemas de binding
                var contentType = Request.ContentType;
                Console.WriteLine($"[ChatController] Content-Type: {contentType}");
                try
                {
                    // Intentar leer el body raw para depuración (solo si el binding falla)
                    Request.Body.Position = 0;
                }
                catch { }
                Console.WriteLine($"[ChatController] ModelState.IsValid: {ModelState.IsValid}");
                Console.WriteLine($"[ChatController] Request bound: {JsonConvert.SerializeObject(request)}");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ChatResponse
                    {
                        Message = "Request model is invalid",
                        Success = false
                    });
                }

                if (request == null)
                {
                    return BadRequest(new ChatResponse
                    {
                        Message = "Request body is missing or could not be bound",
                        Success = false
                    });
                }
                var userMessage = new ChatMessage
                {
                    Content = request.Message,
                    IsUser = true,
                    Timestamp = DateTime.Now
                };

                List<ChatMessage> chatHistory = GetChatHistory(userMessage);
                SaveChatHistory(chatHistory); // Guarda el mensaje del usuario


                string user = HttpContext.Session.GetString("usuario");
                if (user == null)
                {
                    Console.WriteLine("[ChatController] Usuario no autenticado");
                    return Json(new ChatResponse
                    {
                        Message = "Inicia sesión para usar el ChatBot",
                        Success = false
                    });
                }

                Usuario usuario = Objeto.StringToObject<Usuario>(user);
                if (usuario == null)
                {
                    Console.WriteLine("[ChatController] Error al deserializar usuario");
                    return Json(new ChatResponse
                    {
                        Message = "Error de sesión. Por favor, inicia sesión nuevamente.",
                        Success = false
                    });
                }

                int idUsuario = usuario.ID;
                string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");

                Console.WriteLine($"[ChatController] nombreHeladera: {nombreHeladera ?? "NULL"}");
                Console.WriteLine($"[ChatController] idUsuario: {idUsuario}");

                List<ProductoXHeladera> heladeraJson = new List<ProductoXHeladera>();

                if (string.IsNullOrEmpty(nombreHeladera))
                {
                    Console.WriteLine("[ChatController] nombreHeladera es null o vacío - usuario no tiene heladera inicializada");
                    // Intentar inicializar la heladera automáticamente
                    List<string> nombresHeladeras = BD.traerNombresHeladerasById(idUsuario);
                    if (nombresHeladeras != null && nombresHeladeras.Count > 0)
                    {
                        nombreHeladera = nombresHeladeras[0];
                        HttpContext.Session.SetString("nombreHeladera", nombreHeladera);
                        Console.WriteLine($"[ChatController] Heladera inicializada automáticamente: {nombreHeladera}");
                    }
                    else
                    {
                        Console.WriteLine("[ChatController] Usuario no tiene heladeras");
                    }
                }

                if (!string.IsNullOrEmpty(nombreHeladera))
                {
                    heladeraJson = BD.getProductosByNombreHeladeraAndIdUsuario(nombreHeladera, idUsuario);
                    Console.WriteLine($"[ChatController] heladeraJson count: {heladeraJson?.Count ?? 0}");
                }

                // Validar que heladeraJson no sea null (si es null, inicializar como lista vacía)
                if (heladeraJson == null)
                {
                    heladeraJson = new List<ProductoXHeladera>();
                }

                var botResponse = await _chatService.GetChatResponseAsync(request.Message, chatHistory, heladeraJson);

                var botMessage = new ChatMessage
                {
                    Content = botResponse,
                    IsUser = false,
                    Timestamp = DateTime.Now
                };
                chatHistory.Add(botMessage);

                SaveChatHistory(chatHistory);

                return Json(new ChatResponse
                {
                    Message = botResponse,
                    Success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new ChatResponse
                {
                    Message = $"Error: {ex.Message}",
                    Success = false
                });
            }
        }

        [HttpPost]
        public IActionResult ClearHistory()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("ChatHistory");
            return Json(new { success = true });
        }
       private List<ChatMessage> GetChatHistory(ChatMessage chatMessage)
{
    var session = _httpContextAccessor.HttpContext?.Session;
    List<ChatMessage> historyJson = new List<ChatMessage>();

    if (session != null)
    {
        var json = session.GetString("ChatHistory");
        if (!string.IsNullOrEmpty(json))
        {
            historyJson = Objeto.StringToObject<List<ChatMessage>>(json);
        }

        // Asegurar que la lista esté inicializada
        if (historyJson == null)
        {
            historyJson = new List<ChatMessage>();
        }

        if (chatMessage != null)
        {
            historyJson.Add(chatMessage);
        }
    }

    return historyJson;
}



        private void SaveChatHistory(List<ChatMessage> chatHistory)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var historyJson = Objeto.ObjectToString(chatHistory);
                session.SetString("ChatHistory", historyJson);
            }
        }
    }
}
