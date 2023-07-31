using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Models
{
    internal class AllNotes
    {
        public ObservableCollection<Note> Notes { get; } = new ObservableCollection<Note>();

        public void LoadNotes()
        {
            IEnumerable<Note> notes =
                Directory.EnumerateFiles(FileSystem.AppDataDirectory, "*.notes.txt")
                .Select(f => new Note()
                {
                    FilePathname = f,
                    Text = File.ReadAllText(f),
                    Date = File.GetCreationTime(f)
                })
                .OrderBy(n => n.Date);

            Notes.Clear();
            foreach (var n in notes)
            {
                Notes.Add(n);
            }
        }
    }
}
