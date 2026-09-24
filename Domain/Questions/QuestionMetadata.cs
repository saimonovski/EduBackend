namespace Domain.Questions;

public class QuestionMetadata
{
    private string GeneratedBy { get; set; }
    private int Difficulty { get; set; }
    private Dictionary<string,double> SkillsWeight { get; set; }
    private DateTime Timestamp { get; set; }
    private int EstimatedTimeSeconds { get; set; }
}