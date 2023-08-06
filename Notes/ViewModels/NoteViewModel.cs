using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notes.ViewModels
{
    internal class NoteViewModel : ObservableObject, IQueryAttributable
    {
        public const string KeyLoad = "load";
        public const string KeySaved = "saved";
        public const string KeyDeleted = "deleted";

        private Models.Note _note;

        public NoteViewModel(Models.Note note)
        {
            _note = note;
            SetupCommands();
        }

        public NoteViewModel()
        {
            _note = new Models.Note();

            SetupCommands();
        }

        #region Properties
        public string Text
		{
			get { return _note.Text; }
			set 
			{ 
				if (_note.Text != value)
				{
                    _note.Text = value;
					OnPropertyChanged();
                }
            }
		}
        public DateTime Date => _note.Date;
        public string Id => _note.Filename;
        public ICommand SaveCmd { get; private set; }
        public ICommand DeleteCmd { get; private set; }
        #endregion

        public void Reload()
        {
            _note = Models.Note.Load(_note.Filename);

            RefreshProperties();
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey(KeyLoad))
            {
                _note = Models.Note.Load(query[KeyLoad].ToString());

                RefreshProperties();
            }
        }

        private void RefreshProperties()
        {
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(Date));
        }

        private async Task Save()
        {
            _note.Date = DateTime.Now;
            _note.Save();

            await AppShell.Current.GoToAsync($"..?{KeySaved}={_note.Filename}");
        }

        private async Task Delete()
        {
            _note.Delete();

            await AppShell.Current.GoToAsync($"..?{KeyDeleted}={_note.Filename}");
        }

        private void SetupCommands()
        {
            SaveCmd = new AsyncRelayCommand(Save);
            DeleteCmd = new AsyncRelayCommand(Delete);
        }
    }
}
