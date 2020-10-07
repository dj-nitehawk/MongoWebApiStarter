using MongoDB.Entities;
using System;

namespace Dom
{
    public class Image : FileEntity, ICreatedOn
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsLinked { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}