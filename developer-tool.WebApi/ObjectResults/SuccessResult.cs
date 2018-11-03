using Microsoft.AspNetCore.Mvc;

namespace WebApi.Results 
{
    public class SuccessObjectResult : ObjectResult
    {
        private SuccessObjectResult(object value) : base(value)
        {
            Value = new { data = value };
        }

        public static SuccessObjectResult Data(object value) {
            return new SuccessObjectResult(value);
        }
    }
}