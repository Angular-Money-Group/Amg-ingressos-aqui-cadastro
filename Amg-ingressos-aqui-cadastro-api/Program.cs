using System.Reflection;
using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Repository;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Api de usuarios",
        Description = "api de gerenciamentod de usuarios",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.Configure<CadastroDatabaseSettings>(
    builder.Configuration.GetSection("CadastroDatabase"));

// injecao de dependencia
//services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReceiptAccountService, ReceiptAccountService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAssociateService, AssociateService>();
builder.Services.AddScoped<ISupportService, SupportService>();
builder.Services.AddScoped<IAssociateService, AssociateService>();
builder.Services.AddScoped<ICollaboratorService, CollaboratorService>();
builder.Services.AddScoped<IEventService, EventService>();
//repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReceiptAccountRepository, ReceiptAccountRepository>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
builder.Services.AddScoped<ISupportRepository, SupportRepository>();
builder.Services.AddScoped<ISequenceRepository, SequenceRepository>();
builder.Services.AddScoped<IAssociateColabOrganizerRepository, AssociateColabOrganizerRepository>();
builder.Services.AddScoped<IAssociateColabEventRepository, AssociateColabEventRepository>();
builder.Services.AddScoped<IAssociateUserApiDataEventRepository, AssociateUserApiDataEventRepository>();


//infra
builder.Services.AddScoped<IDbConnection, DbConnection>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Certifique-se de ter essa linha
app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<CadastroExceptionHandlerMiddleaware>();
app.Run();
