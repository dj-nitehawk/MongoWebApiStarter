using MongoWebApiStarter;
using ServiceStack;

namespace Account.Get
{
    [Route("/account/{ID}")]
    public class Request : IRequest<Response>
    {
        public string ID { get; set; }

        public string AccountID; // public fields are auto populated from user claim if names match
        public string EmployeeID;
    }
}
