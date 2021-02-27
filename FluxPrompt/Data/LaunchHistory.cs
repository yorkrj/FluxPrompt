using System;
using System.Collections.Generic;
using System.Text;

namespace FluxPrompt.Data
{
    class LaunchHistory
    {
        public string SearchPhrase { get; set; }
        public FileLink FileLink { get; set; }
        public int CountLaunches { get; set; }
        public DateTime LastLaunched { get; set; }
    }
}
