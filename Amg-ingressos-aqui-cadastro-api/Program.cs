using Amg_ingressos_aqui_cadastro_api.Infra;
using Amg_ingressos_aqui_cadastro_api.Model;
using Amg_ingressos_aqui_cadastro_api.Repository;
using Amg_ingressos_aqui_cadastro_api.Repository.Interfaces;
using Amg_ingressos_aqui_cadastro_api.Services;
using Amg_ingressos_aqui_cadastro_api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.Configure<TransactionDatabaseSettings>(
    builder.Configuration.GetSection("CadastroDatabase"));
    
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// injecao de dependencia
//services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReceiptAccountService, ReceiptAccountService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
//repository
builder.Services.AddScoped<IUserRepository, UserRepository<object>>();
builder.Services.AddScoped<IReceiptAccountRepository, ReceiptAccountRepository<object>>();
builder.Services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository<object>>();
//infra
builder.Services.AddScoped<IDbConnection<User>, DbConnection<User>>();
builder.Services.AddScoped<IDbConnection<ReceiptAccount>, DbConnection<ReceiptAccount>>();
builder.Services.AddScoped<IDbConnection<PaymentMethod>, DbConnection<PaymentMethod>>();

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

app.Run();
