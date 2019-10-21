using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

//Instalamos o Entity Framework
//dotnet tool install --global dotnet-ef
//instala a ferramenta para todos os futuros projetos   //para parar de rodar o projeto: ctrl + c

//baixamos o pacote SQLServer do Entity Framework
//dotnet add package Microsoft.EntityFrameworkCore.SqlServer

//baixamos o pacote que irá escrever nossos códigos - models
//dotnet add package Microsoft.EntityFrameworkCore.Design

//confirmar se foi restaurado
//dotnet restore

//testamos a instalação do EF
//dotnet ef

//código que criará o nosso Contexto da Base de Dados e nossos models *mágica*
//tudo relacionado ao banco(conexão) será chamado de contexto //faz funcionar a geração de códigos e banco
//Parâmetros:
//1º servidor. 2º base de dados. 3º e 4º Conexão com banco em si, Usuário e Senha //String de conexão

//-o diretório -d data anotation anotação foreign key, se é not null 
//dotnet ef dbcontext scaffold "Server=N-1S-DEV-02\SQLEXPRESS; Database=Gufos; User Id=sa; Password=132" Microsoft.EntityFrameworkCore.SqlServer -o Models -d

//SWAGGER - documentação

//instalação do pacote
//dotnet add backend.csproj package Swashbuckle.AspNetCore -v 5.0.0-rc4

//JWT - JSON WEB TOKEN

//adicionado o pacote jwt
//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 3.0.0

namespace backend {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public Startup (IConfiguration configuration) {
            this.Configuration = configuration;

        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddControllersWithViews ().AddNewtonsoftJson (
                opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //configuração de como os objetos relacionados aparecerão nos retornos
            //configura o Swagger
            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("V1", new OpenApiInfo { Title = "API", Version = "V1" });

                //definindo o caminho e arquivo temporário de documentação
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments (xmlPath);
            });

            //configurando o JWT
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer (options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Configuration["Jwt:Issuer"]))

                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            //usamos efetivamente o Swagger
            app.UseSwagger ();

            //especificamos o endpoint na aplicação
            //basicamente uma url ou uma rota
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/V1/swagger.json", "API V1");
            });

            app.UseHttpsRedirection ();

            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}