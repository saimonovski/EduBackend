using Application.Interfaces;
using EduBackend_Test;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument();

builder.Services.AddSingleton<IQuestionRepository,  MockQuestionRepository>();

builder.Services.AddScoped<IQuestionService, QuestionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwaggerGen();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.Run();

