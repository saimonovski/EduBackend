namespace Domain.Questions;

public abstract class Question
{
    private int Id { get; }
    private string QuestionContext { get; }
    private List<Question> Items { get; }
    private QuestionMetadata Metadata { get; set; }

   public abstract Task<bool> CheckAnswer(string answer);
   public abstract Task<bool> CheckAnswers(string[] answer);
   
   
}