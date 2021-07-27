using System;

namespace Database.Helper
{
    public class Test
    {
        
        [PrimaryKey]
        public string ID { get; set; }

        public string Filename { get; set; }
        public string PostedAt { get; set; }
        public string Caption { get; set; }
        public int Likes { get; set; }
        public int CommentsCount { get; set; }
        public int share { get; set; }

        [ForeignKey]
        public string UserId { get; set; }

        [ForeignKey]
        public int TestID { get; set; }


    }
}