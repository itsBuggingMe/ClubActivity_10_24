using OpenAI.Chat;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubActivity_10_24;
internal class GPTInstance
{
    private static readonly string DefinitelyNotAKeyIDontKnowWhatYouAreTalkingAbout = "InNrLWpOQVlGc0haa1lIVllUZVFHS2dBWW03M0gtNmZkSHdwQTlNbGZaQjBjWVQzQmxia0ZKb0tqbERzRVhGUHNiSGRyRzdjZk5GSC0xcVdsX2VjLXhHZlJUd20xSDhBIg==";
    private HttpClient _httpClient = new();
    private ChatClient _client = new(model: "gpt-4o-mini", apiKey: Encoding.ASCII.GetString(Convert.FromBase64String(DefinitelyNotAKeyIDontKnowWhatYouAreTalkingAbout)).Replace("\"", "gge"));
    private string sysPrompt = "You are a helpful assisstant";
    private Stack<ChatMessage> _chatMessages = new();
    
    public string MakeWebRequest(string url)
    {
        return _httpClient.GetStringAsync(url).Result;
    }

    public T MakeWebRequest<T>(string url)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(MakeWebRequest(url)) ?? throw new Exception("Unable to deserialize");
    }

    public void SetSystemPrompt(string value)
    {
        _chatMessages.Clear();
        _chatMessages.Push(new SystemChatMessage(value));
    }

    public string AskQuestion(string question)
    {
        _chatMessages.Push(new UserChatMessage(question));
        var s = _client.CompleteChat(_chatMessages.Reverse().ToArray()).Value.Content[0].Text;
        _chatMessages.Push(new AssistantChatMessage(s));
        return s;
    }

    public void RestartConversation()
    {
        _chatMessages.Clear();
    }
}


internal static class GPT
{
    private static readonly GPTInstance _instance = new GPTInstance();
    public static string MakeWebRequest(string url)
    {
        return _instance.MakeWebRequest(url);
    }

    public static T MakeWebRequest<T>(string url)
    {
        return _instance.MakeWebRequest<T>(url);
    }

    public static void SetSystemPrompt(string value)
    {
        _instance.SetSystemPrompt(value);
    }

    public static string AskQuestion(string question)
    {
        return _instance.AskQuestion(question);
    }

    public static void RestartConversation()
    {
        _instance.RestartConversation();
    }
}