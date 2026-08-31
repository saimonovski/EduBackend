namespace Domain.Questions;

public class ClosedQuestion : Question
{
    public List<string> Answers { get; set; }
    public List<string> CorrectAnswers { get; set; }

    public override Task<bool> CheckAnswer(string answer)
    {
        //todo zwaliduj stringa (usun wszystkie znaki biale i doprowadz do lowercase) i zwaliduj CorrectAnswers
        // todo sprawdz czy podana odpowiedz to jedna z CorrectAnswers
        throw new NotImplementedException();
    }

    public override Task<bool> CheckAnswers(string[] answer)
    {
        //todo zwaliduj stringa (usun wszystkie znaki biale i doprowadz do lowercase) i zwaliduj CorrectAnswers
        // todo sprawdz czy podana odpowiedz to jedna z CorrectAnswers
        throw new NotImplementedException();
    }


    
}