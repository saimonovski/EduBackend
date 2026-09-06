using Application.Dto;

namespace Domain.Questions;

public class OpenQuestion(int id) : Question(id, QuestionType.Open)
{
    //todo
    public override bool CheckAnswers(string[] answer)
    {
        throw new NotImplementedException();
    }
}