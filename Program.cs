using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;


using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticTest;



Console.WriteLine("Hello, World!");

// Populate values from your OpenAI deployment
//var modelId = "gemini-2.5-flash";
//var apiKey = "AIzaSyAPfm4lzbhchT4PKYp04rjF0fVTwTqt1xk";


var builderHosting = Host.CreateApplicationBuilder();


// 1. Obtener la interfaz de configuración (ya inyectada por el builder)
var configuration = builderHosting.Configuration;

// 2. Leer los valores de la sección "GeminiConfiguration"
var modelId = configuration["GeminiConfiguration:ModelId"];
var apiKey = configuration["GeminiConfiguration:ApiKey"];

// 3. Verificación básica (opcional pero recomendado)
if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(modelId))
{
    Console.WriteLine("Error: La API Key o el Model ID no están configurados en appsettings.json.");
    return; // Termina la aplicación si faltan credenciales
}


// Configuración de logging
builderHosting.Logging.ClearProviders();
builderHosting.Logging.AddConsole();
builderHosting.Logging.AddDebug();
builderHosting.Logging.SetMinimumLevel(LogLevel.Trace);


builderHosting.Services.AddSingleton<IPizzaService, PizzaService>();
builderHosting.Services.AddSingleton<IUserContext, UserContext>();
builderHosting.Services.AddSingleton<IPaymentService, PaymentService>();


builderHosting.Services.AddScoped(sp => new ChatHistory());

// 3. Registrar el Plugin (contenedor de KernelFunctions) 
// Lo registramos como Transient o Singleton para que el contenedor lo resuelva. 
// Registro del plugin
builderHosting.Services.AddTransient<OrderPizzaPlugin>();


// 4. Registrar la instancia del Kernel
// Registro del Kernel
builderHosting.Services.AddTransient(sp => new Kernel(sp));




// 5. Registrar el servicio de LLM (IchatCompletionService)
// Configuración del proveedor LLM (Google Gemini en este caso)
builderHosting.Services.AddGoogleAIGeminiChatCompletion(modelId, apiKey);


// Registrar Worker que usará Kernel + Logger
builderHosting.Services.AddTransient<Worker>();

var host = builderHosting.Build();
var worker = host.Services.GetRequiredService<Worker>();
await worker.RunAsync();
