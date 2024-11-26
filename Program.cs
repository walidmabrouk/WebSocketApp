using WebSocketApp.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWebSocketMiddleware();

var app = builder.Build();


var connectionManager = app.Services.GetRequiredService<WebSocketServerConnectionManager>();
connectionManager.StartPeriodicNotifications();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseWebSockets();
app.UseWebSocketMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();