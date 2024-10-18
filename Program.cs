using SudokuFullGame.Components;
using SudokuFullGame.Models;
using SudokuFullGame.Services;
using MudBlazor;
using MudBlazor.Services;
using Microsoft.AspNetCore.SignalR;
using SudokuFullGame.Hubs;
using SudokuFullGame.Components.Pages;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<RoomService>();
builder.Services.AddMudServices();
builder.Services.AddSignalR();
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<SudokuGeneratorService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
   
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.MapHub<RoomHub>("/roomHub");
app.MapHub<SudokuHub>("/sudokuHub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
