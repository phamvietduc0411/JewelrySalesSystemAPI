using Microsoft.EntityFrameworkCore;

namespace JewelrySalesSystem.DAL.Common
{
    public class PaginatedList<T>
    {
        public PaginatedList(List<T> items, int currentPage, int pageSize, int totalRecords)
        {
            Items = items;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }

        public List<T> Items { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public bool HasNextPage => CurrentPage * PageSize < TotalRecords;

        public bool HasPreviousPage => CurrentPage > 1;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
        {
            var totalRecords = await query.CountAsync();

            var totalPages = ((double)totalRecords / (double)pageSize);

            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var paginatedList = new PaginatedList<T>(items, page, pageSize, totalRecords)
            {
                TotalPages = roundedTotalPages
            };

            return paginatedList;
        }
    }
}
