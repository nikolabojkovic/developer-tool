using System.Net;

namespace Core.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string entity, string key) 
            : base($"Entity \"{entity}\" ({key}) was not found.", HttpStatusCode.NotFound)
        {
        }
    }
}