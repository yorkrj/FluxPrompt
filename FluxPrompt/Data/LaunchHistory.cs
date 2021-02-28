using System;

namespace FluxPrompt.Data
{
    class LaunchHistory : FileLink
    {
        public string SearchPhrase { get; set; }
        public int CountLaunches { get; set; }
        public DateTime LastLaunched { get; set; }
    }
}
