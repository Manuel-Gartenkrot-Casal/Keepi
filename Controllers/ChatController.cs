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
    var session = HttpContext.Session.GetString("usuario");
    if (string.IsNullOrEmpty(session))
    {
        return RedirectToAction("Login", "Auth");
    }
    
    var chatHistory = GetChatHistory(null);
    return View(chatHistory);
}

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                string user = HttpContext.Session.GetString("usuario");
                if (string.IsNullOrEmpty(user))
                {
                    Console.WriteLine("[ChatController] Usuario no autenticado");
                    return Json(new ChatResponse
                    {
                        Message = "Tu sesión ha expirado. Por favor, inicia sesión nuevamente.",
                        Success = false
                    });
                }

                Usuario usuario = Objeto.StringToObject<Usuario>(user);
                if (usuario == null || usuario.ID <= 0)
                {
                    Console.WriteLine("[ChatController] Error al deserializar usuario o ID inválido");
                    return Json(new ChatResponse
                    {
                        Message = "Error de sesión. Por favor, inicia sesión nuevamente.",
                        Success = false
                    });
                }

                if (request == null || string.IsNullOrWhiteSpace(request.Message))
                {
                    return Json(new ChatResponse
                    {
                        Message = "El mensaje no puede estar vacío",
                        Success = false
                    });
                }

                if (request.Message.Length > 1000)
                {
                    return Json(new ChatResponse
                    {
                        Message = "El mensaje es demasiado largo. Máximo 1000 caracteres.",
                        Success = false
                    });
                }

                int idUsuario = usuario.ID;
                string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");

                Console.WriteLine($"[ChatController] Usuario autenticado: {usuario.Username} (ID: {idUsuario})");
                Console.WriteLine($"[ChatController] nombreHeladera: {nombreHeladera ?? "NULL"}");

                List<ProductoXHeladera> heladeraJson = new List<ProductoXHeladera>();

                if (string.IsNullOrEmpty(nombreHeladera))
                {
                    Console.WriteLine("[ChatController] nombreHeladera es null o vacío - inicializando heladera");
                    List<string> nombresHeladeras = BD.traerNombresHeladerasById(idUsuario);
                    
                    if (nombresHeladeras != null && nombresHeladeras.Count > 0)
                    {
                        nombreHeladera = nombresHeladeras[0];
                        HttpContext.Session.SetString("nombreHeladera", nombreHeladera);
                        Console.WriteLine($"[ChatController] Heladera inicializada: {nombreHeladera}");
                    }
                    else
                    {
                        Console.WriteLine("[ChatController] Usuario no tiene heladeras configuradas");
                        return Json(new ChatResponse
                        {
                            Message = "No tienes heladeras configuradas. Por favor, configura una heladera primero.",
                            Success = false
                        });
                    }
                }

                List<string> heladerasDelUsuario = BD.traerNombresHeladerasById(idUsuario);
                if (!heladerasDelUsuario.Contains(nombreHeladera))
                {
                    Console.WriteLine($"[ChatController] Usuario {idUsuario} intentó acceder a heladera no autorizada: {nombreHeladera}");
                    return Json(new ChatResponse
                    {
                        Message = "No tienes permiso para acceder a esta heladera.",
                        Success = false
                    });
                }

                // Load products from refrigerator
                heladeraJson = BD.getProductosByNombreHeladeraAndIdUsuario(nombreHeladera, idUsuario);
                Console.WriteLine($"[ChatController] Productos cargados: {heladeraJson?.Count ?? 0}");

                var userMessage = new ChatMessage
                {
                    Content = request.Message,
                    IsUser = true,
                    Timestamp = DateTime.Now
                };

                List<ChatMessage> chatHistory = GetChatHistory(userMessage);
                SaveChatHistory(chatHistory);

                // Get AI response
                var botResponse = await _chatService.GetChatResponseAsync(request.Message, chatHistory, heladeraJson ?? new List<ProductoXHeladera>());

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
                Console.WriteLine($"[ChatController] Excepción: {ex.Message}");
                Console.WriteLine($"[ChatController] Stack Trace: {ex.StackTrace}");
                return Json(new ChatResponse
                {
                    Message = "Ocurrió un error al procesar tu mensaje. Por favor, intenta nuevamente.",
                    Success = false
                });
            }
        }

        [HttpPost]
        public IActionResult ClearHistory()
        {
            var session = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(session))
            {
                return Json(new { success = false, message = "Sesión no válida" });
            }

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
