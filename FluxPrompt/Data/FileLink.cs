using System;
using System.Collections.Generic;
using System.Text;

namespace FluxPrompt.Data
{
    /// <summary>
    /// A representation of file shortcuts.
    /// </summary>
    class FileLink
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
