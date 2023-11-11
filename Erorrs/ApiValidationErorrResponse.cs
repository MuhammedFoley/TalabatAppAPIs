namespace TalabatAppAPIs.Erorrs
{
    public class ApiValidationErorrResponse:ApiResponse
    {
        public IEnumerable<String> Erorrs { get; set; }

        public ApiValidationErorrResponse() : base(400) 
        {
            Erorrs = new List<String>();
        }
    }
}
