using System;

namespace FluxPrompt.Data
{
    class LaunchHistory
    {
        public string SearchPhrase { get; set; }
        public int CountLaunches { get; set; }
        public DateTime LastLaunched { get; set; }
        public FileLink FileLink { get; set; }
    }
}
