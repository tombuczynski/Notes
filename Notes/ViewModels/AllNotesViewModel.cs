using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notes.ViewModels
{
    internal class AllNotesViewModel : IQueryAttributable
    {
        public AllNotesViewModel()
        {
            AllNotes = new ObservableCollection<NoteViewModel>(Models.Note.LoadAll()
                .Select(n => new NoteViewModel(n))
                .OrderByDescending(nvm => nvm.Date));
            NewNoteCmd = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCmd = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
        }

        #region Properties
        public ObservableCollection<NoteViewModel> AllNotes { get; }
        public ICommand NewNoteCmd { get; }
        public ICommand SelectNoteCmd { get; }
        #endregion

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(NoteViewModel.KeySaved))
            {
                string noteId = query[NoteViewModel.KeySaved].ToString();
                NoteViewModel matchedViewModel = AllNotes.Where(nvm => nvm.Id == noteId).FirstOrDefault();

                if (matchedViewModel != null)
                {
                    matchedViewModel.Reload();
                }
                else
                {
                    AllNotes.Add(new NoteViewModel(Models.Note.Load(noteId)));
                }

            }
            else if (query.ContainsKey(NoteViewModel.KeyDeleted))
            {
                string noteId = query[NoteViewModel.KeyDeleted].ToString();
                NoteViewModel matchedViewModel = AllNotes.Where(nvm => nvm.Id == noteId).FirstOrDefault();

                if (matchedViewModel != null)
                {
                    AllNotes.Remove(matchedViewModel);
                }
            }
        }

        private async Task NewNoteAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.NotePage));
        }
        private async Task SelectNoteAsync(NoteViewModel n)
        {
            if (n != null)
                await Shell.Current.GoToAsync(nameof(Views.NotePage) + $"?{NoteViewModel.KeyLoad}={n.Id}");
        }
    }
}
