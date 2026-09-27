using System.Text.Json.Serialization;

namespace Domain.Questions;



public abstract class Question()
{
    public int Id { get; set; }
    public QuestionType Type { get; set; }
    public string QuestionContext { get; set; } = "";
    public List<Question> Items { get; set; } = [];
    public QuestionMetadata Metadata { get; set; } = new();

    public abstract bool CheckAnswers(string[] answer);
   
}

