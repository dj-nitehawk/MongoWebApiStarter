using MongoDB.Entities;
using System;

namespace Data
{
    [Database(Constants.FileBucketDB)]
    public class Image : FileEntity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime AccessedOn { get; set; }
        public bool IsLinked { get; set; }
    }
}
