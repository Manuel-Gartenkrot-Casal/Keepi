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
            ChatMessage vacio = null;
            var chatHistory = GetChatHistory(vacio);
            return View(chatHistory);
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
                try {
                    // Intentar leer el body raw para depuración (solo si el binding falla)
                    Request.Body.Position = 0;
                } catch { }
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

                
                HttpContext.Session.GetString("nombreHeladera");
                string user = HttpContext.Session.GetString("usuario");
                    if (user == null)
                    {
                    
                    return RedirectToAction("Login", "Auth");
                    }
                    Usuario usuario = Objeto.StringToObject<Usuario>(user);
                    int idUsuario = usuario.ID;
                
                string nombreHeladera = HttpContext.Session.GetString("nombreHeladera");

                List<ProductoXHeladera> heladeraJson = BD.getProductosByNombreHeladeraAndIdUsuario(nombreHeladera, idUsuario);
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

        private List<ChatMessage> GetChatHistory(ChatMessage chatHistory)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var Json = session.GetString("ChatHistory");
                List<ChatMessage> historyJson = Objeto.StringToObject<List<ChatMessage>>(Json);

                if (historyJson != null && historyJson.Any())
                {
                    try
                    {   
                        historyJson.Add(chatHistory);
                        return (historyJson);
                    }
                    catch
                    {   }
                }
            }
            return new List<ChatMessage>();
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
