using Application.Dto;
using Domain.Questions;

namespace Domain.Model;

public abstract class Model(string modelName)
{
    public string ModelName { get; } = modelName;
    
   public abstract  Task<List<Question>> GenerateQuestions(QuestionType questionType, int count, int difficulty, string topic, List<string> subtopics);
   public abstract Task<bool> CheckQuestionAnswers(Dictionary<int, string[]> answers);
   
   
}