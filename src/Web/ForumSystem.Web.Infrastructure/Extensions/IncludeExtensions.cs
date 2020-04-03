namespace ForumSystem.Web.Infrastructure.Extensions
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public static class IncludeExtensions
    {
        public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable)
            where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var isVirtual = property.GetGetMethod().IsVirtual;
                if (isVirtual && properties.FirstOrDefault(c => c.Name == property.Name + "Id") != null)
                {
                    queryable = queryable.Include(property.Name);
                }
            }

            return queryable;
        }
    }
}
