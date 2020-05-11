using MongoDB.Entities.Core;
using MongoWebApiStarter.Biz.Interfaces;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class TemplateModel : IMapper<Entity>
    {
        public void LoadFrom(Entity entity)
        {
            throw new System.NotImplementedException();
        }

        public Entity ToEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}
