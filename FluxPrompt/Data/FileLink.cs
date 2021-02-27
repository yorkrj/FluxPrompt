using System;
using System.Collections.Generic;
using System.Text;

namespace FluxPrompt.Data
{
    /// <summary>
    /// A stripped down representation of a shortcut.
    /// </summary>
    class FileLink
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string WorkingDirectory { get; set; }
        public string Arguments { get; set; }
    }
}
