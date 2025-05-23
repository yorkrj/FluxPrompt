﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FluxPrompt.Data
{
    class FileLinksModel
    {
        public List<FileLink> FileLinks { get; set; }
        public List<LaunchHistory> LaunchHistories { get; set; }

        private const string FileLinkFileName = "Links.dat";
        private const string HistoryFileName = "History.dat";
        private readonly string AppDataPath;

        public FileLinksModel()
        {
            // Create AppData directory for FluxPrompt
            AppDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FluxPrompt"
            );
            Directory.CreateDirectory(AppDataPath);

            string linksPath = Path.Combine(AppDataPath, FileLinkFileName);
            string historyPath = Path.Combine(AppDataPath, HistoryFileName);

            if (File.Exists(linksPath))
            {
                LoadFileLinks(linksPath);
            }
            else
            {
                ScanShortcuts();
                SaveFileLinks(linksPath);
            }

            LoadHistory(historyPath);
        }

        public FileLink GetFileLink(Guid Key)
        {
            return FileLinks.FirstOrDefault(t => t.Key == Key);
        }

        public void SetLaunchHistory(string SearchPhrase, Guid Key)
        {
            string searchPhrase = SearchPhrase.Trim().ToLowerInvariant();

            LaunchHistory foundHistory = LaunchHistories.FirstOrDefault(t => 
                t.FileLink.Key == Key 
                && t.SearchPhrase.Trim().ToLowerInvariant() == searchPhrase);

            if (foundHistory == null)
            {
                LaunchHistory newHistory = new LaunchHistory
                {
                    SearchPhrase = searchPhrase,
                    CountLaunches = 1,
                    LastLaunched = DateTime.Now,
                    FileLink = FileLinks.FirstOrDefault(t => t.Key == Key)
                };
                LaunchHistories.Add(newHistory);
            }
            else
            {
                foundHistory.CountLaunches++;
                foundHistory.LastLaunched = DateTime.Now;
            }
        }

        /// <summary>
        /// Given a search phrase, return an ordered list of FileLink with previously searched items and closest matches at the top.
        /// </summary>
        public List<FileLink> GetFileLinks(string SearchPhrase)
        {
            List<Tuple<int, FileLink>> rankedResults = new List<Tuple<int, FileLink>>();
            string searchPhrase = SearchPhrase.Trim().ToLowerInvariant();

           foreach (LaunchHistory link in LaunchHistories)
            {
                if (searchPhrase == link.SearchPhrase)
                {
                    rankedResults.Add(new Tuple<int, FileLink>(
                        -link.CountLaunches,
                        link.FileLink));
                }
            }

            foreach (FileLink link in FileLinks)
            {
                if (!rankedResults.Any(t => t.Item2.Key == link.Key))
                {
                    int match = 0,
                        newMatch = 0;

                    foreach (char item in searchPhrase)
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
                        int firstMatch = link.Name.ToLowerInvariant().IndexOf(searchPhrase.First());

                        rankedResults.Add(new Tuple<int, FileLink>(
                            firstMatch + match,
                            link));
                    }
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

        private void SaveFileLinks(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (FileStream stream = new FileStream(filePath, FileMode.CreateNew))
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                foreach (FileLink link in FileLinks)
                {
                    writer.Write(link.Key.ToString());
                    writer.Write(link.Name);
                    writer.Write(link.Path);
                    writer.Write(link.WorkingDirectory);
                    writer.Write(link.Arguments);
                }
            }
        }

        private void LoadFileLinks(string filePath)
        {
            FileLinks = new List<FileLink>();

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                while (reader.PeekChar() != -1)
                {
                    FileLink link = new FileLink()
                    {
                        Key = Guid.Parse(reader.ReadString()),
                        Name = reader.ReadString(),
                        Path = reader.ReadString(),
                        WorkingDirectory = reader.ReadString(),
                        Arguments = reader.ReadString()
                    };
                    FileLinks.Add(link);
                }
            }
        }

        public void SaveHistory()
        {
            string historyPath = Path.Combine(AppDataPath, HistoryFileName);
            if (File.Exists(historyPath))
            {
                File.Delete(historyPath);
            }

            using (FileStream stream = new FileStream(historyPath, FileMode.CreateNew))
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false))
            {
                foreach (LaunchHistory history in LaunchHistories)
                {
                    writer.Write(history.SearchPhrase);
                    writer.Write(history.CountLaunches);
                    writer.Write(history.LastLaunched.ToBinary());
                    writer.Write(history.FileLink.Key.ToString());
                }
            }
        }

        private void LoadHistory(string filePath)
        {
            LaunchHistories = new List<LaunchHistory>();

            if (!File.Exists(filePath))
            {
                return; // TODO: Handle missing history file.
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false))
            {
                while (reader.PeekChar() != -1)
                {
                    string searchPhrase = reader.ReadString();
                    int countLaunches = reader.ReadInt32();
                    long lastLaunchedBinary = reader.ReadInt64();
                    Guid fileLinkKey = Guid.Parse(reader.ReadString());

                    FileLink fileLink = FileLinks.FirstOrDefault(t => t.Key == fileLinkKey);
                    if (fileLink != null)
                    {
                        LaunchHistory history = new LaunchHistory
                        {
                            SearchPhrase = searchPhrase,
                            CountLaunches = countLaunches,
                            LastLaunched = DateTime.FromBinary(lastLaunchedBinary),
                            FileLink = fileLink
                        };
                        LaunchHistories.Add(history);
                    }
                }
            }
        }

        /// <summary>
        /// Scan the given paths for shortcut files. Parse these shortcuts and add details to the FileLinks collection of this model.
        /// </summary>
        /// <param name="Paths">A stack of strings containing folder paths that contain shortcut (.lnk) files.</param>
        private void ScanShortcuts(Stack<string> Paths)
        {
            FileLinks = new List<FileLink>();
            LaunchHistories = new List<LaunchHistory>();

            while (Paths.Count > 0)
            {
                string path = Paths.Pop();
                string[] files = Directory.GetFiles(path, "*.lnk", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    try
                    {
                        var startInfo = new ProcessStartInfo(file);
                        if (!string.IsNullOrWhiteSpace(startInfo.FileName))
                        {
                            FileLinks.Add(new FileLink
                            {
                                Key = Guid.NewGuid(),
                                Name = Path.GetFileNameWithoutExtension(file),
                                Path = startInfo.FileName,
                                WorkingDirectory = startInfo.WorkingDirectory,
                                Arguments = startInfo.Arguments
                            });
                        }
                    }
                    catch
                    {
                        // Do nothing for now. Permission exceptions are to be expected here.
                        //TODO Eventually report on exceptions scanning shortcuts after this is not running on every launch.
                    }
                }
            }
        }
    }
}

