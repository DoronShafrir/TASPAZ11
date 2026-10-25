using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public class AIService
{
    private readonly IChatCompletionService _chatService;

    public AIService(string model, string apiKey)
    {
        var kernelBuilder = Kernel.CreateBuilder();

        kernelBuilder.AddOpenAIChatCompletion(model, apiKey);

        var kernel = kernelBuilder.Build();

        _chatService =
            kernel.GetRequiredService<IChatCompletionService>();
    }

    public async Task<string> AskAsync(string question)
    {
        var result =
            await _chatService.GetChatMessageContentAsync(question);

        return result.Content ?? "";
    }
}