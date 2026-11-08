using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Endpoints.Learn
{
    public static class TaskParserUtil
    {
        /// <summary>
        /// Asynchronicznie parsuje strumień odpowiedzi (NDJSON) linia po linii.
        /// </summary>
        public static async IAsyncEnumerable<SimplifiedTask> ParseTasksAsync(
            Stream stream, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using var reader = new StreamReader(stream);
            string? line;
            
            while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                SimplifiedTask? parsedTask = null;
                try
                {
                    var wrapper = JsonSerializer.Deserialize<ApiResponse>(line);

                    if (wrapper?.Event == "task" && wrapper.Data != null)
                    {
                        parsedTask = new SimplifiedTask
                        {
                            Id = wrapper.Data.TaskId,
                            PresentationId = wrapper.Data.PresentationId,
                            TaskType = wrapper.Data.TaskType,
                            Options = wrapper.Data.Options ?? new List<OptionItem>()
                        };
                    }
                    else if (wrapper?.Event == "error")
                    {
                        // Opcjonalnie: możesz zalogować lub obsłużyć błąd ze strumienia
                        throw new System.Exception($"Task generation error from API: {wrapper.Message}");
                    }
                }
                catch (JsonException)
                {
                    // Ignorujemy linie, które nie pasują do schematu (np. {"event": "done", ...})
                }

                if (parsedTask != null)
                {
                    yield return parsedTask;
                }
            }
        }

        #region Modele DTO

        public class SimplifiedTask
        {
            public string Id { get; set; }
            public string PresentationId { get; set; }
            public string TaskType { get; set; }
            public List<OptionItem> Options { get; set; }
        }

        public class OptionItem
        {
            [JsonPropertyName("id")] public string Id { get; set; }
            [JsonPropertyName("text")] public string Text { get; set; }
        }

        internal class ApiResponse
        {
            [JsonPropertyName("event")] public string Event { get; set; }
            [JsonPropertyName("data")] public TaskData Data { get; set; }
            [JsonPropertyName("message")] public string Message { get; set; } // Obsługa komunikatu błędu
        }

        internal class TaskData
        {
            [JsonPropertyName("task_id")] public string TaskId { get; set; }
            [JsonPropertyName("presentation_id")] public string PresentationId { get; set; }
            [JsonPropertyName("task_type")] public string TaskType { get; set; }
            [JsonPropertyName("options")] public List<OptionItem> Options { get; set; }
        }

        #endregion
    }
}