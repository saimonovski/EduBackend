namespace Domain.Questions;

public class OpenQuestion : Question
{
    public OpenQuestion()
    {
        Type =  QuestionType.Closed;
    }
    public override bool CheckAnswers(string[] answer)
    {
        throw new NotImplementedException();
    }
}