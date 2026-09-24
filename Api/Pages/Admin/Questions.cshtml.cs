using Application.Dto;
using Application.Interfaces;
using Application.Mappings;
using Domain.Questions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Api.Pages.Admin;


[Authorize(Roles = "Admin")]

public class Questions (IQuestionService questionService): PageModel 
{

        [BindProperty]
        public string NewQuestionContent { get; set; } = string.Empty;

        public int NewQuestionId { get; set; } = -1;

        public List<QuestionDto> QuestionList { get; set; } = [];

    
        
        public async Task  OnGet()
        {
            var result = await questionService.GetAllQuestionsAsync();
            Console.WriteLine("Checking: "+result.IsSuccess+"\n"+result.ErrorMessage);
            if (!result.IsSuccess) return;
            var questionDto = QuestionMapper.CreateQuestionDto(result.Value!.ToList());
            QuestionList = questionDto;
            Console.WriteLine(questionDto.Count);
        }
    

        public IActionResult OnPostAdd()
        {
            if (!string.IsNullOrWhiteSpace(NewQuestionContent))
            {
                QuestionList.Add(new QuestionDto(NewQuestionId, QuestionType.Closed, NewQuestionContent, []));
            }


            return RedirectToPage(); 
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            var itemResult = await questionService.GetQuestionAsync(id);
            if (itemResult.IsSuccess)
            {
                QuestionList.Remove(QuestionMapper.CreateQuestionDto(itemResult.Value!));
            }

            return RedirectToPage();
        }
    }
