using System.IdentityModel.Tokens.Jwt;
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
builder.Services.AddAuthorization(options => options.FallbackPolicy = null);
builder.Services.AddTransient<JwtSecurityTokenHandler>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddDbContext<Context>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<AlocacaoUseCases>();
builder.Services.AddScoped<DisciplinaUseCases>();
builder.Services.AddScoped<LaboratorioUseCases>();
builder.Services.AddScoped<AplicativoUseCases>();
builder.Services.AddScoped<UsuarioUseCases>();
builder.Services.Configure<SecuritySettings>(options =>
    {
        options.passwordSalt = Environment.GetEnvironmentVariable("PASSWORD_SALT");
        options.jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
    }
); 


// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    var context = services.GetRequiredService<Context>();
        
    context.Database.Migrate(); 
        
    
    
}
    //app.UseHttpsRedirection();
app.MapStaticAssets();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}




app.MapControllers();





app.Run();