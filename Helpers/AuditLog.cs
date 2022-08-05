using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublishAPI.Helpers
{
    public class AuditLog
    {
        #region Local
        public string Path { get; set; }
        public string RollingInterval { get; set; }
        public bool Shared { get; set; }
        public int RetainedFileCountLimit { get; set; }
        #endregion



    }
}
