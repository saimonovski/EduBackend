using Application.Dto;

namespace Domain.Questions;

public class ClosedQuestion(int id) : Question(id, QuestionType.Closed)
{
    public List<string> Answers { get; set; } = [];
    public List<string> CorrectAnswers { get; set; } = [];

   

    public override bool CheckAnswers(string[] answer)
    {
        if (answer.Length == 0 || CorrectAnswers.Count == 0)
            return false;
        
        var normalizedUserAnswers = answer
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .Select(NormalizeString)
            .ToHashSet(); 

 
        var normalizedCorrectAnswers = CorrectAnswers
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .Select(NormalizeString)
            .ToHashSet();

     
        return normalizedUserAnswers.Any(normalizedCorrectAnswers.Contains);    }

    private static string NormalizeString(string input)
    {
        var cleaned = new string(input.Where(c => !char.IsWhiteSpace(c)).ToArray());
        return cleaned.ToLowerInvariant();
    }

    
}