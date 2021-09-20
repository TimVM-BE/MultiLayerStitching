using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiLayerStitching.First;
using MultiLayerStitching.Second;
using MultiLayerStitching.Third;

namespace MultiLayerStitching
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddHttpContextAccessor();

            services.AddGraphQLServer("First")
                .AddQueryType(x => x.Name("Query"))
                    .AddTypeExtension<FirstQueries>()
                ;

            services.AddGraphQLServer("Second")
                .AddQueryType(x => x.Name("Query"))
                    .AddTypeExtension<SecondQueries>()
                ;

            services.AddGraphQLServer("Third")
                .AddQueryType(x => x.Name("Query"))
                    .AddTypeExtension<ThirdQueries>()
                ;

            // Imagine one team creating first and second service and adding a gateway called 'Merge'.
            // ** A great example would be this beeing an internal gateway
            services.AddGraphQLServer("Merge")
                .AddLocalSchema("First")
                .AddLocalSchema("Second")
                .AddTypeExtensionsFromString(@"extend type Query { bar2: String @delegate(schema: ""Second"", path: ""bar"") }");

            // Imagine another team having ownership over a third service and wanting to integrate some of the code exposed in 'Merge'
            // ** And this beeing the public facing gateway
            services.AddGraphQLServer()
                .AddLocalSchema("Merge")
                .AddLocalSchema("Third")
                .IgnoreField("Query", "Foo", "Merge")
                .AddTypeExtensionsFromString(@"extend type Query { bar3: String @delegate(schema: ""Third"", path: ""hello('you')"") }");
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
                endpoints.MapGraphQL("/merge/graphql", "Merge");
            });
        }
    }
}
