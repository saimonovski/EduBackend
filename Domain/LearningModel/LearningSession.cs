using Domain.Questions;
using Domain.Users;

namespace Domain.LearningModel;

public class LearningSession
{
    public string UserId { get; set; }
    public List<Question> Questions { get; set; } = [];
    public List<LearningMaterial> LearningMaterials { get; set; } = [];
}