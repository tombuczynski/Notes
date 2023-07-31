using Notes.Models;

namespace Notes.Views;

public partial class AllNotesPage : ContentPage
{
    private AllNotes _notes;
	public AllNotesPage()
	{
		InitializeComponent();

        _notes = new AllNotes();

        BindingContext = _notes;
    }

    private void NotesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            Note note = (Note)e.CurrentSelection[0];

            Shell.Current.GoToAsync($"{nameof(NotePage)}?{nameof(NotePage.ItemId)}={note.FilePathname}");

            NotesCollection.SelectedItem = null;
        }
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NotePage));
    }

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        _notes.LoadNotes();
    }
}