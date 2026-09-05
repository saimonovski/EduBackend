using System.Runtime.Serialization;
using Application.Dto;
using Application.Entity;
using Application.Interfaces;
using Domain.Questions;

namespace Application.Mappings;

public class QuestionMapper(IQuestionRepository questionRepository)
{

    public async Task<Result<Question>> ToDomain(QuestionDto dto)
    {
       var result = await questionRepository.GetAllByIds(DeserializeItemsId(dto.SerializedItemsId).ToArray());
       if (!result.IsSuccess)
       {
           return Result<Question>.Failure(result.ErrorMessage);
       }
       var enumerable = result.Value!;
       
       var id = dto.QuestionId;
       
       var question = QuestionFactory(id, dto.QuestionType);
       
       question.QuestionContext = dto.QuestionContext;
       question.Metadata = dto.QuestionMetadata;
       question.Items = enumerable.ToList();
    
       return Result<Question>.Success(question);
    }

    private List<int> DeserializeItemsId(string serializedItems)
    {
        var deserializedItems = serializedItems.Split(',');
        var items = new List<int>();
        foreach (var item in deserializedItems)
        {
                if (int.TryParse(item, out int itemId))
                {
                    items.Add(itemId);
                }
            
        }

        return items;
    }

    private Question QuestionFactory(int id, QuestionType type)
    {
        return type switch
        {
            QuestionType.Closed => new ClosedQuestion(id),
            QuestionType.Open => new OpenQuestion(id),
            _ => throw new ArgumentOutOfRangeException("Logic for that option is not defined" + type)
        };
    }
    
}