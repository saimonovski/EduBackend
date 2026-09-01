using Domain.Questions;

namespace Application.Dto;

public record QuestionDto(
    int QuestionId,
    QuestionType QuestionType,
    string QuestionContext,
    string SerializedItemsId,
    QuestionMetadata QuestionMetadata
);



public enum QuestionType
{
    Open,
    Closed
}