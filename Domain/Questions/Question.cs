using System.Text.Json.Serialization;

namespace Domain.Questions;



public abstract class Question(int id, QuestionType type)
{
    public int Id { get; } = id;
    public QuestionType Type { get; } = type;
    public string QuestionContext { get; set; } = "";
    public List<Question> Items { get; set; } = [];
    public QuestionMetadata Metadata { get; set; } = new();

    public abstract bool CheckAnswers(string[] answer);
   
}

