namespace Notes.Views;
[QueryProperty(nameof(ItemId), nameof(ItemId))]
public partial class NotePage : ContentPage
{
    public NotePage()
	{
		InitializeComponent();

        //if (File.Exists(_notesTxtFile))
        //{
        //    TextEditor.Text = File.ReadAllText(_notesTxtFile);
        //}

        string filePathname = Path.Combine(FileSystem.AppDataDirectory,  $"{Path.GetRandomFileName()}.notes.txt");
        LoadNote(filePathname);
    }

    public string ItemId { set { LoadNote(value); } }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Note note)
        {
            File.WriteAllText(note.FilePathname, TextEditor.Text);
        }

        await Shell.Current.GoToAsync("..");
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Note note)
        {
            if (File.Exists(note.FilePathname))
            {
                File.Delete(note.FilePathname);
            }
        }

        await Shell.Current.GoToAsync("..");
    }

    private void LoadNote(string filePathname)
    {
        Models.Note note = new Models.Note
        {
            FilePathname = filePathname
        };

        if (File.Exists(filePathname)) 
        {
            note.Text = File.ReadAllText(filePathname);
            note.Date = File.GetCreationTime(filePathname);
        }

        BindingContext = note;
    }
}