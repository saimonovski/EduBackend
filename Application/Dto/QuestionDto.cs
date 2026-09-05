using Domain.Questions;

namespace Application.Dto;

public record QuestionDto(
    int QuestionId,
    QuestionType QuestionType,
    string QuestionContext,
    string SerializedItemsId,
    QuestionMetadata QuestionMetadata //todo add answers, todo change this to dao and category dto also to dao. Implement proper DTO objects
);