using MongoDB.Entities.Common;
using System;

namespace MongoWebApiStarter.Data.Entities
{
    public class Image : Entity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime AccessedOn { get; set; }
        public byte[] Bytes { get; set; }
    }
}
