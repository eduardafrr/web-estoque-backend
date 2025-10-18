using estoque.core.Categoria.Repositorio;
using estoque.core.Repositorio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Adiciona suporte a controladores

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Google OAuth Example API", Version = "v1" });

    // Incluir coment�rios XML para documenta��o (opcional, mas recomendado)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configurar autentica��o
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/api/auth/login"; // Caminho para iniciar o login
    options.LogoutPath = "/api/auth/logout"; // Caminho para logout
    options.AccessDeniedPath = "/api/auth/access-denied"; // Caminho para acesso negado

    // Configura��es de seguran�a para produ��o (ajuste conforme necess�rio)
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Use Always em produ��o
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]
        ?? throw new InvalidOperationException("Google ClientId n�o configurado");
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
        ?? throw new InvalidOperationException("Google ClientSecret n�o configurado");

    // Configurar escopos (opcional)
    googleOptions.Scope.Add("profile");
    googleOptions.Scope.Add("email");

    // Salvar tokens para uso posterior (opcional)
    googleOptions.SaveTokens = true;

    // Configurar callback path - DEVE CORRESPONDER AO URI DE REDIRECIONAMENTO NO GOOGLE CLOUD CONSOLE
    googleOptions.CallbackPath = "/signin-google";
});

// Adicionar autoriza��o
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Google OAuth Example API V1");
    });
}

//builder.Services.AddDbContext<estoque.core.Categoria.Model.CategoriaContext>();

app.UseHttpsRedirection();

builder.Services.AddScoped<ProdutoRepositorio>();
builder.Services.AddScoped<CategoriaRepositorio>();

// Adicionar middleware de autentica��o e autoriza��o
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
