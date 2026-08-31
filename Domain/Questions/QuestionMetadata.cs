namespace Domain.Questions;

public class QuestionMetadata
{
    private string GeneratedBy { get; set; }
    private int Difficulty { get; set; }
    private int TopicId { get; set; }
    private List<int> SubtopicsId { get; set; }
    private DateTime Timestamp { get; set; }
    private int EstimatedTimeSeconds { get; set; }
}