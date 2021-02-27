using System;
using System.Collections.Generic;

namespace FluxPrompt.Data
{
    class FileLinksModel
    {
        public FileLinksModel()
        {
            //TODO Improve launch times and avoid needing administrative permissions on every launch.
            // Instead of scanning for shortcuts on every launch, persist and load scanned shortcut results on subsequent loads.
            // Consider BinaryWriter/Reader over Json Serialization for performance.
            // Provide mechanism to refresh persisted shortcuts.

            ScanShortcuts();
        }

        public List<FileLink> FileLinks { get; set; }
        public List<LaunchHistory> LaunchHistories { get; set; }

        public List<FileLink> GetFileLinks(string SearchPhrase)
        {
            throw new NotImplementedException();
        }

        private void ScanShortcuts()
        {
            //TODO Implement ScanShortcuts() method.
        }
    }
}
