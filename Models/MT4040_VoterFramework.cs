using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace PublishAPI.Models
{
    [BsonIgnoreExtraElements]
    public class MT4040_VoterFramework
    {
        public string rollno_flag { get; set; }
        public int pb_code { get; set; }
        public string guardianname { get; set; }

        public string gender { get; set; }

        public int rollno { get; set; }
        public int pagenum { get; set; }

        public int version { get; set; }
        public int sectiono { get; set; }
        public string guardiantype { get; set; }
        public string ecino { get; set; }
        public int stac { get; set; }
        public int stacpb { get; set; }
        public string name { get; set; }
        public string voterid { get; set; }
        public string houseno { get; set; }
        public string rollno_text { get; set; }
       
        public int age { get; set; }
        public int ac_code { get; set; }



    }
}
