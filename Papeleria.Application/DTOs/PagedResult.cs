using Papeleria.WebApi.DTOs;

namespace Papeleria.Application.DTOs
{
    public class PagedResult<T> : ResultDto<IEnumerable<T>>
    {
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public static PagedResult<T> SuccessPagedResult(IEnumerable<T> items, int totalItems, int page, int pageSize, string message = "Consulta paginada exitosa.")
        {
            return new PagedResult<T>
            {
                Data = items,
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                StatusCode = 200,
                Success = true,
                Message = message
            };
        }

        public static PagedResult<T> FailurePagedResult(string message = "Error en la consulta paginada.", int statusCode = 500)
        {
            return new PagedResult<T>
            {
                Data = Enumerable.Empty<T>(),
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Page = 0,
                PageSize = 0,
                TotalItems = 0
            };
        }
    }
}
