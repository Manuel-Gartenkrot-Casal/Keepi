using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using semantic_kernel.Models;
using Keepi.Models;

namespace semantic_kernel.Services
{
    public interface IChatService
    {
        Task<string> GetChatResponseAsync(string userMessage, List<ChatMessage> chatHistory, List<ProductoXHeladera> heladeraJson);
    }

    public class ChatService : IChatService
    {
        private readonly Kernel _kernel;
        private readonly GeminiPromptExecutionSettings _promptSettings;

        public ChatService(IConfiguration configuration)
        {
            var apiKey = configuration["GoogleAI:ApiKey"]
                ?? throw new InvalidOperationException("API Key not found in appsettings.json");
            var modelId = configuration["GoogleAI:ModelId"] ?? "gemini-2.5-flash";

            // Crea el builder y agrega el conector de Gemini
            var builder = Kernel.CreateBuilder();
            builder.AddGoogleAIGeminiChatCompletion(modelId: modelId, apiKey: apiKey);
            _kernel = builder.Build();

            // Ajustes de ejecución del prompt
            _promptSettings = new GeminiPromptExecutionSettings
            {
                Temperature = 0.7,
                TopP = 0.95
            };
        }

        public async Task<string> GetChatResponseAsync(string userMessage, List<ChatMessage> chatHistory, List<ProductoXHeladera> heladeraJson)
        {
            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();

            // Prompt de sistema
            var systemPromptBase = """
                Eres un asistente conversacional amigable y servicial de Keepi, tu nombre es "Mr. Keepi".
                Responde siempre en español y mantén el contexto de la conversación.
                No menciones el nombre del usuario.
                Tu misión es ayudar al usuario a resolver sus dudas y problemas acerca de alimentos, principalmente ideas para hacer recetas con los productos que tiene en la heladera, fechas de vencimiento, por ejemplo: "Hice X receta con el producto Y, ¿hasta cuando puedo consumirlo si lo guardo en la heladera?"
                Cuando el usuario te pase informacion incompleta, hazle preguntas muy simples y cortas para que complete la información, lo importante es que el usuario no se sienta abrumado.
                Cuando el usuario te pida una receta, responde con una receta simple y fácil de hacer, con pocos ingredientes y pasos.
                Si el usuario te pregunta sobre algo que no sabes, responde que no lo sabes y ofrece ayuda para encontrar la información.
                Tus respuestas deben ser cortas y directas, el usuario esta hablando contigo porque le falta tiempo.
                """;
                string systemPrompt = systemPromptBase + Environment.NewLine
    + "heladeraJson = \"" + heladeraJson + "\"";
            history.AddSystemMessage(systemPrompt);

            // Agrega el historial de conversación
            foreach (var message in chatHistory.TakeLast(10)) // Mantener solo los últimos 10 mensajes
            {
                if (message.IsUser)
                {
                    history.AddUserMessage(message.Content);
                }
                else
                {
                    history.AddAssistantMessage(message.Content);
                }
            }

            // Agrega el mensaje actual del usuario
            history.AddUserMessage(userMessage);

            // Obtiene la respuesta del modelo
            var response = await chat.GetChatMessageContentAsync(history, _promptSettings);
            return response.Content?.Trim() ?? "(sin respuesta)";
        }
    }
}
