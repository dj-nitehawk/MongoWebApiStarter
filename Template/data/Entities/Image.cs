using MongoDB.Entities;
using System;

namespace MongoWebApiStarter.Data.Entities
{
    public class Image : FileEntity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime AccessedOn { get; set; }
    }
}
