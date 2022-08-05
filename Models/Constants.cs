using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishAPI.Models
{
    public class Constants
    {
        public const string JSON_CONTENT = "application/json";
        public const string GET_VOTERID = "getVoterid";
        public const string Post = "Post";
        public const string Path = "api/mongodb/getVoterid";
        public const string DatabaseSettings = "DatabaseSettings";
        public const string UserSettings = "UserSettings";

        public const string testChanges = "testChanges";
        public static string Health { get; internal set; }
        public static string HealthCommand { get; internal set; }
    }
}
