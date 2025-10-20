using Microsoft.AspNetCore.Mvc;
using semantic_kernel.Models;
using semantic_kernel.Services;
using System.Text.Json;

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
            var chatHistory = GetChatHistory();
            return View(chatHistory);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            try
            {
                var chatHistory = GetChatHistory();
                
                // Agrega el mensaje del usuario al historial
                var userMessage = new ChatMessage
                {
                    Content = request.Message,
                    IsUser = true,
                    Timestamp = DateTime.Now
                };
                chatHistory.Add(userMessage);

                // Obtiene la respuesta del chatbot
                var botResponse = await _chatService.GetChatResponseAsync(request.Message, chatHistory);
                
                // Agrega la respuesta del bot al historial
                var botMessage = new ChatMessage
                {
                    Content = botResponse,
                    IsUser = false,
                    Timestamp = DateTime.Now
                };
                chatHistory.Add(botMessage);

                // Guarda el historial actualizado en la sesión
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

        private List<ChatMessage> GetChatHistory()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var historyJson = session.GetString("ChatHistory");
                if (!string.IsNullOrEmpty(historyJson))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<List<ChatMessage>>(historyJson) ?? new List<ChatMessage>();
                    }
                    catch
                    {
                        // Si hay error al deserializar, retorna una lista vacía
                    }
                }
            }
            return new List<ChatMessage>();
        }

        private void SaveChatHistory(List<ChatMessage> chatHistory)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var historyJson = JsonSerializer.Serialize(chatHistory);
                session.SetString("ChatHistory", historyJson);
            }
        }
    }
}
