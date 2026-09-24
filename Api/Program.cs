using System.Text.Json.Serialization;
using Application.Interfaces;
using EduBackend_Test;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IQuestionRepository,  MockQuestionRepository>();

builder.Services.AddScoped<IQuestionService, QuestionService>();

builder.Services.AddControllers()
   .AddJsonOptions(options =>
   {
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
   });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
   
   .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
   {
      options.LoginPath = "/admin/login";  
      options.AccessDeniedPath = "/admin/denied"; 
      options.ExpireTimeSpan = TimeSpan.FromHours(8);
   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   app.UseSwaggerGen();
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseFastEndpoints();


app.MapRazorPages();

app.Run();

