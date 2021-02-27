using Shell32;
using System;
using System.Collections.Generic;
using System.IO;

namespace FluxPrompt.Data
{
    class FileLinksModel
    {
        public FileLinksModel()
        {
            ScanShortcuts();
        }

        public List<FileLink> FileLinks { get; set; }
        public List<LaunchHistory> LaunchHistories { get; set; }

        /// <summary>
        /// Given a search phrase, return an ordered list of FileLink with previously searched items and closest matches at the top.
        /// </summary>
        public List<FileLink> GetFileLinks(string SearchPhrase)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scan user's Start Menu and load details into this model.
        /// </summary>
        private void ScanShortcuts()
        {
            //TODO Improve launch times and avoid needing administrative permissions on every launch.
            // Instead of scanning for shortcuts on every launch, persist and load scanned shortcut results on subsequent loads.
            // Consider BinaryWriter/Reader over Json Serialization for performance.
            // Provide mechanism to refresh persisted shortcuts.

            Stack<string> paths = new Stack<string>();

            paths.Push(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu));
            paths.Push(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu));

            ScanShortcuts(paths);
        }

        /// <summary>
        /// Scan the given paths for shortcut files. Parse these shortcuts and add details to the FileLinks collection of this model.
        /// </summary>
        /// <param name="Paths">A stack of strings containing folder paths that contain shortcut (.lnk) files.</param>
        private void ScanShortcuts(Stack<string> Paths)
        {
            FileLinks = new List<FileLink>();
            Shell32.Shell shell = new Shell32.Shell();

            while (Paths.Count > 0)
            {
                string path = Paths.Pop();
                string[] files = System.IO.Directory.GetFiles(path, "*.lnk", System.IO.SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    path = Path.GetDirectoryName(file);
                    string filenameOnly = Path.GetFileName(file);
                    Folder folder = shell.NameSpace(path);
                    FolderItem folderItem = folder.ParseName(filenameOnly);
                    try
                    {
                        ShellLinkObject link = (ShellLinkObject)folderItem.GetLink;

                        FileLinks.Add(new FileLink
                        {
                            Key = Guid.NewGuid(),
                            Name = folderItem.Name,
                            Path = link.Path,
                            WorkingDirectory = link.WorkingDirectory,
                            Arguments = link.Arguments
                        });
                    }
                    catch
                    {
                        // Do nothing for now.
                        //TODO Eventually report on exceptions scanning shortcuts after this is not running on every launch.
                    }
                }
            }
        }
    }
}
