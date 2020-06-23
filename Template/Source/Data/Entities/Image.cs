using MongoDB.Entities;
using System;

namespace Data
{
    public class Image : FileEntity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime AccessedOn { get; set; }
    }
}
