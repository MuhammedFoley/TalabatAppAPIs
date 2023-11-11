namespace TalabatAppAPIs.Erorrs
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode, string? message=null)
        {
            StatusCode=statusCode;
            Message=message??GetDefaultMessageFprStatusCode(statusCode);
        }

        private string? GetDefaultMessageFprStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "a bad requset you have made",
                401 => "Authorized,you are not",
                404 => "Resource was not found",
                500 => "Erorrs are the path to the dark side,Erorr lead to Anger,Anget leads to hate,Hate lead to career change  ",
                _ => null
            };
        }
    }
}
