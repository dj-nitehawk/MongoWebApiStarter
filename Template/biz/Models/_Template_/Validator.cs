using FluentValidation;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class TemplateModel
    {
        public class Validator : AbstractValidator<TemplateModel>
        {
            public Validator()
            {

            }
        }
    }
}
