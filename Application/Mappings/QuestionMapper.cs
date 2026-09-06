using System.Runtime.Serialization;
using Application.Dto;
using Application.Entity;
using Application.Interfaces;
using Domain.Questions;

namespace Application.Mappings;

public record QuestionDao(
    int QuestionId,
    QuestionType QuestionType,
    string QuestionContext,
    List<int> ItemsId,
    QuestionMetadata QuestionMetadata //todo add answers, 
);

public record QuestionDto(
    int Id,
    QuestionType Type,
    string QuestionContext,
    List<QuestionDto> Items,
    QuestionMetadata QuestionMetadata
);

public record ClosedQuestionDao(int Id, QuestionType QuestionType, string QuestionContext, List<int> ItemsId, QuestionMetadata QuestionMetadata, List<string> Answers, List<string> CorrectAnswers): QuestionDao(Id, QuestionType,QuestionContext, ItemsId, QuestionMetadata);
public record ClosedQuestionDto(int Id, QuestionType QuestionType, string QuestionContext, List<QuestionDto> Items, QuestionMetadata QuestionMetadata, List<string> Answers): QuestionDto(Id, QuestionType,QuestionContext,Items, QuestionMetadata);


public static class QuestionMapper
{
    public static QuestionDto CreateQuestionDto(Question question)
    {
        return QuestionDtoFactory(question);
    }
    public static List<QuestionDto> CreateQuestionDto(List<Question> questions)
    {
        return questions.Select(CreateQuestionDto).ToList();
    }
    public static QuestionDao ToDao(Question question)
    {
       return QuestionDaoFactory(question);
    }
    
    
    public static Question ToDomain(QuestionDao dao, List<Question> subquestions)
    {
        ArgumentNullException.ThrowIfNull(subquestions);
        
       return QuestionFactory( dao, subquestions);
    }

    private static QuestionDao QuestionDaoFactory(Question question)
    {
        var convertedItems = question.Items.Select(x => x.Id).ToList();

        return question switch
        {
            ClosedQuestion closed => new ClosedQuestionDao(closed.Id, closed.Type, closed.QuestionContext,
                convertedItems, closed.Metadata, closed.Answers, closed.CorrectAnswers),
            _ => new QuestionDao(question.Id, question.Type, question.QuestionContext,
                convertedItems, question.Metadata)
        };
    }
    
    private static QuestionDto QuestionDtoFactory(Question question)
    {
        var convertedItems = question.Items.Select(CreateQuestionDto).ToList();
        return question switch
        {
            ClosedQuestion closed => new ClosedQuestionDto(closed.Id, closed.Type, closed.QuestionContext,
                convertedItems, closed.Metadata, closed.Answers),
            _ => new QuestionDto(question.Id, question.Type, question.QuestionContext,
                convertedItems, question.Metadata)
        };
    }

    private static  Question QuestionFactory(QuestionDao dao, List<Question> subquestions)
    {
        return dao switch
        {
            ClosedQuestionDao closed =>

                new ClosedQuestion(closed.Id)
                {
                    Answers = closed.Answers,
                    CorrectAnswers = closed.CorrectAnswers,
                    Items = subquestions
                },
            _ => throw new ArgumentOutOfRangeException("Not implemented functionality for this type: " + dao.QuestionType)
        };


    }
    
    
}