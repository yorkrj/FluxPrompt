using Shell32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FluxPrompt.Data
{
    class FileLinksModel
    {
        public List<FileLink> FileLinks { get; set; }
        public List<LaunchHistory> LaunchHistories { get; set; }

        public FileLinksModel()
        {
            ScanShortcuts();
        }

        public FileLink GetFileLink(Guid Key)
        {
            return FileLinks.FirstOrDefault(t => t.Key == Key);
        }

        /// <summary>
        /// Given a search phrase, return an ordered list of FileLink with previously searched items and closest matches at the top.
        /// </summary>
        public List<FileLink> GetFileLinks(string SearchPhrase)
        {
            List<Tuple<int, FileLink>> rankedResults = new List<Tuple<int, FileLink>>();
            string keyPhrase = SearchPhrase.ToLower();

            //TODO search LaunchHistories first

            foreach (FileLink link in FileLinks)
            {
                int match = 0,
                    newMatch = 0;

                foreach (char item in keyPhrase)
                {
                    newMatch = link.Name.ToLowerInvariant().IndexOf(item, match);

                    if (newMatch >= match)
                    {
                        match = newMatch;
                    }
                    else
                    {
                        newMatch = int.MaxValue;
                        break;
                    }
                }

                if (newMatch == match)
                {
                    int firstMatch = link.Name.ToLowerInvariant().IndexOf(keyPhrase.First());

                    rankedResults.Add(new Tuple<int, FileLink>(
                        firstMatch + match,
                        link));
                }
            }

            List<FileLink> results = (from c in rankedResults
                                      orderby c.Item1, c.Item2.Name.Length, c.Item2.Name
                                      select c.Item2
                                      ).ToList();

            return results;
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
            Shell shell = new Shell();

            while (Paths.Count > 0)
            {
                string path = Paths.Pop();
                string[] files = Directory.GetFiles(path, "*.lnk", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    path = Path.GetDirectoryName(file);
                    string filenameOnly = Path.GetFileName(file);
                    Folder folder = shell.NameSpace(path);
                    FolderItem folderItem = folder.ParseName(filenameOnly);
                    try
                    {
                        ShellLinkObject link = (ShellLinkObject)folderItem.GetLink;

                        if (!string.IsNullOrWhiteSpace(link.Path))
                        {
                            FileLinks.Add(new FileLink
                            {
                                Key = Guid.NewGuid(),
                                Name = folderItem.Name,
                                Path = link.Path,
                                WorkingDirectory = link.WorkingDirectory,
                                Arguments = link.Arguments
                            });
                        }
                    }
                    catch
                    {
                        // Do nothing. Permission exceptions are to be expected here.
                        //TODO Eventually report on exceptions scanning shortcuts after this is not running on every launch.
                    }
                }
            }
        }
    }
}
