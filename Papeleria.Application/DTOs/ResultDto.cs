namespace Papeleria.WebApi.DTOs
{
    public class ResultDto<T>
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ResultDto<T> SuccessResult(T data, string message = "Operación exitosa.", int statusCode = 200)
        {
            return new ResultDto<T>
            {
                Data = data,
                Success = true,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ResultDto<T> FailureResult(string message = "Ocurrió un error.", int statusCode = 500)
        {
            return new ResultDto<T>
            {
                Data = default,
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }
    }

}
