using Domain.Questions;
using Domain.Users;

namespace Domain.LearningModel;

public class LearningSession
{
    public Guid Id { get; init; }
    public string UserId { get; set; }
    public int SessionTime { get; set; } //In minutes
    /*
     * Zapisujemy tylko te zadania 
     */
    
    public List<Milestone> Milestones { get; set; } = [];
}

public class Milestone
{
    public Guid MilestoneId { get; set; }
    public Dictionary<LearningMaterial,List<Question>> LearningMaterials { get; set; } = [];
    public List<Question> Questions { get; set; } = [];
    
}

