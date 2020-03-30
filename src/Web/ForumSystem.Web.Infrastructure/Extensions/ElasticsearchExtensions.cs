using System;

namespace ForumSystem.Web.Infrastructure.Extensions
{
    using Data.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Nest;

    public static class ElasticSearchExtensions
    {
        public static void AddElasticSearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = configuration["elasticsearch:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<Post>(m => m
                    .PropertyName(p => p.Id, "id"))
                .DefaultMappingFor<PostReply>(m => m
                    .PropertyName(c => c.Id, "id"))
                .DefaultMappingFor<Category>(x => x
                    .PropertyName(p => p.Id, "id"));

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);
        }
    }
}