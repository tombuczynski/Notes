using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    internal class Note
    {
        public Note()
        {
            Filename = Path.GetRandomFileName() + FileExtension;
            Date = DateTime.Now;
            Text = "";
        }

        private static string FileRootDirname => FileSystem.AppDataDirectory;
        private static string GetFilePathname(string filename) => Path.Combine(FileRootDirname, filename);
        private string FilePathname => GetFilePathname(Filename);

        private const string FileExtension = ".notes.txt";

        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public void Save() => File.WriteAllText(FilePathname, Text);

        public void Delete()
        {
            if (File.Exists(FilePathname))
            {
                File.Delete(FilePathname);
            }
        }

        public static Note Load(string filename)
        {
            string filepathname = GetFilePathname(filename);

            if (!File.Exists(filepathname))
            {
                throw new FileNotFoundException($"Unable to find file {filename} on local storage.");
            }

            return new Note
            {
                Text = File.ReadAllText(filepathname),
                Date = File.GetLastWriteTime(filepathname),
                Filename = filename
            };
        }

        public static IEnumerable<Note> LoadAll()
        {
            return 
            Directory.EnumerateFiles(FileRootDirname, "*" + FileExtension)
                .Select(f => Load(Path.GetFileName(f)));

        }


    }
}
