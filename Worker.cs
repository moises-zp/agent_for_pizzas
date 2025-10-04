
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;

using Microsoft.Extensions.Logging;


public class Worker
{
    private readonly ILogger<Worker> _logger;
    private readonly Kernel _kernel;
     
    private readonly OrderPizzaPlugin orderPizzaPlugin;

    private readonly ChatHistory history;

    public Worker(
        ILogger<Worker> logger,
        Kernel kernel,
        OrderPizzaPlugin orderPizzaPlugin,
        ChatHistory chatHistory
    )
    {
        _logger = logger;
        _kernel = kernel;
        this.orderPizzaPlugin = orderPizzaPlugin;
        this.history = chatHistory;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("🚀 Iniciando Worker con Semantic Kernel + Gemini...");


        _kernel.Plugins.AddFromObject(orderPizzaPlugin, "OrderPizza");

        GeminiPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            Temperature = 0.7,
            //
            ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions
        };

        

        var history = new ChatHistory();
        history.AddSystemMessage("Eres un asistente útil y amigable.");
        history.Add(new()
        {
            Role = AuthorRole.User,
            AuthorName = "Moisés Zapata",
            Items = [
                new TextContent {Text="Yo soy una persona que deseo hacer una prueba"}
            ]
        });


        Console.WriteLine("Escribe tus mensajes (o 'salir' para terminar):\n");
        // Initiate a back-and-forth chat
        string? userInput="";

        // Obtener servicio de chat desde el Kernel
        var chat = _kernel.GetRequiredService<IChatCompletionService>();
        
        do
        {
            // Collect user input
            Console.Write("User > ");
            userInput = Console.ReadLine();

            // Verificar si el usuario quiere salir o si la entrada es nula
            if (string.IsNullOrWhiteSpace(userInput) || userInput.Equals("salir", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            // Add user input
            history.AddUserMessage(userInput);

            // Get the response from the AI
            var result = await chat.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: _kernel);

            // Print the results
            Console.WriteLine("Assistant > " + result);

            // Add the message from the agent to the chat history
            history.AddMessage(result.Role, result.Content ?? string.Empty);
        } while (userInput is not null);
    }
}