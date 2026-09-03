using System.Text.Json.Serialization;

namespace Domain.Questions;


[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ClosedQuestion), typeDiscriminator: "closed")]
[JsonDerivedType(typeof(OpenQuestion), typeDiscriminator: "open")]
public abstract class Question(int id)
{
    public int Id { get; } = id;
    public string QuestionContext { get; set; } = "";
    public List<Question> Items { get; set; } = [];
    public QuestionMetadata Metadata { get; set; } = new();

    public abstract bool CheckAnswers(string[] answer);
   
}

