using System;
using System.IO;

namespace FolderWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\C#"; // Remove C# and add your Path to watch

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path); // Create folder if not exists

            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = path,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                Filter = "*.*", // Watch all types files
                EnableRaisingEvents = true,
                IncludeSubdirectories = false // Only the target directory
            };

            // Event handlers
            watcher.Created += (s, e) => LogEvent("CREATED", e);
            watcher.Changed += (s, e) => LogEvent("CHANGED", e);
            watcher.Deleted += (s, e) => LogEvent("DELETED", e);
            watcher.Renamed += (s, e) => LogRenamedEvent(e);

            Console.WriteLine($"Watching folder: {path}");
            Console.WriteLine("Try creating/editing/deleting files in File Explorer.");
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        // Method to handle standard events (Created, Changed, Deleted)
        static void LogEvent(string eventType, FileSystemEventArgs e)
        {
            string fileType = Path.GetExtension(e.FullPath); // Get file type
            DateTime timestamp = File.GetCreationTime(e.FullPath); // Get file creation time
            Console.WriteLine($"{eventType}: {e.Name} (Type: {fileType}, Time: {timestamp})");
        }

        // Method to handle renamed event
        static void LogRenamedEvent(RenamedEventArgs e)
        {
            string fileTypeOld = Path.GetExtension(e.OldFullPath);
            string fileTypeNew = Path.GetExtension(e.FullPath);
            DateTime timestamp = File.GetCreationTime(e.FullPath);
            Console.WriteLine($"RENAMED: {e.OldName} ➜ {e.Name} (Old Type: {fileTypeOld}, New Type: {fileTypeNew}, Time: {timestamp})");
        }
    }
}
