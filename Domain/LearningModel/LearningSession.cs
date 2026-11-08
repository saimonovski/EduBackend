using Api.Endpoints.Learn;
using Domain.Questions;
using Domain.Users;

namespace Domain.LearningModel;

public class LearningSession()
{
    public string Id  { get; set; }
    public string UserId { get; set; }
    public string Category { get; set; }
    
    public List<TaskParserUtil.SimplifiedTask> Tasks { get; set; } = [];
    public string PresentationId { get; set; }
}

public class Milestone
{
    public Guid MilestoneId { get; set; }
    public Dictionary<LearningMaterial,List<Question>> LearningMaterials { get; set; } = [];
    public List<Question> Questions { get; set; } = [];
    
}

