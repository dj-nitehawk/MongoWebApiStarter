using MongoWebApiStarter.Biz.Base;
using MongoWebApiStarter.Data.Repos;
using System;
using System.Text;

namespace MongoWebApiStarter.Biz.Models
{
    public partial class EmailModel : ModelBase<EmailRepo>
    {
        public override void Save()
        {
            if (MergeFields.Count == 0) throw new InvalidOperationException("Cannot proceed without any MergeFields!");

            var sb = new StringBuilder(template);

            foreach (var f in MergeFields)
            {
                sb.Replace("{" + f.Key + "}", f.Value);
            }

            var msg = ToEntity(this);
            msg.BodyHTML = sb.ToString();

            Repo.Save(msg);
        }

        public override void Load()
        {
            throw new NotImplementedException();
        }

        public void AddToSendingQueue()
        {
            Save();
        }
    }
}
