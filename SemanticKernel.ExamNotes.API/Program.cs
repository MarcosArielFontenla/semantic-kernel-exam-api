using Microsoft.SemanticKernel;
using SemanticKernel.ExamNotes.Business.Services;
using SemanticKernel.ExamNotes.Business.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var modelId = "gemini-1.5-pro";
var apiKey = builder.Configuration["AI:GeminiAI:ApiKey"];

if (string.IsNullOrWhiteSpace(apiKey))
    throw new InvalidOperationException("API key is required to use the Gemini AI service");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());
        
});

builder.Services.AddScoped<IExamService, ExamService>();
builder.Services.AddScoped<IGeminiService, GeminiService>();

#pragma warning disable SKEXP0070
builder.Services.AddKernel();

builder.Services.AddSingleton(sp =>
{
    return Kernel.CreateBuilder()
                 .AddGoogleAIGeminiChatCompletion(modelId, apiKey)
                 .Build();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();