using NotesProxy.Manager;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

        builder.Services.AddSingleton(NoteManager.Instance.Notes);
        builder.Services.AddSingleton(NoteManager.Instance.Files);
        builder.Services.AddSingleton(NoteManager.Instance.Config);

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        // app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}