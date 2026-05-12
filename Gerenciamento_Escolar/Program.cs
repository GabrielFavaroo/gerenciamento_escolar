using System.Text;
using Gerenciamento_Escolar.Infrastructure;
using Gerenciamento_Escolar.Persistence;
using Gerenciamento_Escolar.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(auth =>{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;}
    ).AddJwtBearer(
    be =>
    {
        be.RequireHttpsMetadata = false;
    
    be.SaveToken = true;
    be.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.encodingKey)),
        ValidateIssuer = false,
        ValidateAudience = false
        
    };

    });
builder.Services.AddAuthorization();

builder.Services.AddTransient<TokenService>();
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AlocacaoUseCases>();
builder.Services.AddScoped<DisciplinaUseCases>();
builder.Services.AddScoped<LaboratorioUseCases>();
builder.Services.AddScoped<AplicativoUseCases>();
builder.Services.AddScoped<UsuarioUseCases>();


// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();