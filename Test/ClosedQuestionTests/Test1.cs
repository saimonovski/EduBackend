using Application.Interfaces;
using Domain.Questions;
using Infrastructure.Services;

namespace EduBackend_Test.ClosedQuestionTests;


public class Test1
{
    private  IQuestionRepository _questionRepository;
    private  IQuestionService _questionService;

    [SetUp]
    public void Setup()
    {
        _questionRepository = new MockQuestionRepository();
        _questionService = new QuestionService(_questionRepository);
    }
    
    [Test]
   public async Task TestCorrectAnswerChecking()
    {
        var mainQuestion = TestClosedQuestion.Create();

        var subQuestion = TestClosedQuestion.Create();
        subQuestion.Answers = ["incorrect", "correct", "incorrect2"];
        subQuestion.CorrectAnswers.Add("correct");


        mainQuestion.Items.Add(subQuestion);
        mainQuestion.Answers = ["incorrect", "correct", "incorrect2", "incorrect3", "correct2"];
        mainQuestion.CorrectAnswers.Add("correct");
        mainQuestion.CorrectAnswers.Add("correct2");
        
       await _questionRepository.Update(mainQuestion);
       await  _questionRepository.Update(subQuestion);

       var answer1 = await _questionService.CheckAnswers(mainQuestion.Id, "cor rect");
       var answer2 = await _questionService.CheckAnswers(mainQuestion.Id, "Correct2");
       var answer3 = await _questionService.CheckAnswers(mainQuestion.Id, "correct");
       var answer4 = await _questionService.CheckAnswers(mainQuestion.Id, "correct");
       var answer5 = await _questionService.CheckAnswers(subQuestion.Id, "correct ");
      
        Assert.Multiple( () =>
        {
            Assert.That(answer1);
            Assert.That(answer2);
            Assert.That(answer3);
            Assert.That(answer4);
            Assert.That(answer5);
        });
    }
   
    [Test]
    public async  Task TestInCorrectAnswerChecking()
    {
            var mainQuestion = TestClosedQuestion.Create();
        
            var subQuestion = TestClosedQuestion.Create();
            subQuestion.Answers = ["incorrect", "correct", "incorrect2"];
            subQuestion.CorrectAnswers.Add("correct");
        
            mainQuestion.Items.Add(subQuestion);
            mainQuestion.Answers = ["incorrect", "correct", "incorrect2", "incorrect3", "correct2"];
            mainQuestion.CorrectAnswers.Add("correct");
            mainQuestion.CorrectAnswers.Add("correct2");
            
          await  _questionRepository.Update(mainQuestion);
          await  _questionRepository.Update(subQuestion);
            
            Assert.Multiple(async () =>
            {
                Assert.That(await _questionService.CheckAnswers(mainQuestion.Id,"incorrect"), Is.False);
                Assert.That(await _questionService.CheckAnswers(mainQuestion.Id, "incorrect2"), Is.False);
                Assert.That(await _questionService.CheckAnswers(subQuestion.Id, "incorrect"), Is.False);
            });
        }
        
    }
