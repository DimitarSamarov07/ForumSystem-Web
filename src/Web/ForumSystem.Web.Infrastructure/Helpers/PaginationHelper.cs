using System;
using System.Collections.Generic;
using System.Text;

namespace ForumSystem.Web.Infrastructure.Helpers
{
    using System.Linq;

    public static class PaginationHelper
    {
        public static IQueryable<T> CreateForPage<T>(IQueryable<T> inputModel, int perPage, int currentPage)
        {
            var obj = inputModel
                .Skip(perPage * (currentPage - 1))
                .Take(perPage);

            return obj;
        }
    }
}
