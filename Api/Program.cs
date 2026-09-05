using Application.Interfaces;
using EduBackend_Test;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument();

builder.Services.AddSingleton<IQuestionService, QuestionService>();
builder.Services.AddScoped<IQuestionRepository,  MockQuestionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwaggerGen();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.Run();

