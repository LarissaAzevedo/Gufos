using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


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


namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
