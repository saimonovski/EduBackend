namespace Domain.Questions;

public abstract class Question(int id)
{
    public int Id { get; } = id;
    public string QuestionContext { get; set; } = "";
    public List<Question> Items { get; set; } = [];
    public QuestionMetadata Metadata { get; set; } = new();

    public abstract bool CheckAnswers(string[] answer);
   
}

