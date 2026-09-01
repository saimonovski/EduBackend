using System.Runtime.Serialization;
using Application.Dto;
using Application.Interfaces;
using Domain.Questions;

namespace Application.Mappings;

public class QuestionMapper(IQuestionRepository questionRepository)
{

    public async Task<Question> ToDomain(QuestionDto dto)
    {
       var enumerable = await questionRepository.GetAllById(DeserializeItemsId(dto.SerializedItemsId).ToArray());

       var id = dto.QuestionId;
       var question = QuestionFactory(id, dto.QuestionType);
       
       question.QuestionContext = dto.QuestionContext;
       question.Metadata = dto.QuestionMetadata;
       question.Items = enumerable.ToList();
    
       return question;
    }

    private List<int> DeserializeItemsId(string serializedItems)
    {
        string[] deserializedItems = serializedItems.Split(',');
        List<int> items = new List<int>();
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
        
        switch (type)
        {
            case QuestionType.Closed: return new ClosedQuestion(id);
            case QuestionType.Open: return new OpenQuestion(id);
            default:
                throw new ArgumentOutOfRangeException("Logic for that option is not defined"+type);
        }
        
    }
    
}