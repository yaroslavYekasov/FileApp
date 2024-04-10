namespace FileApp;

public partial class FileMain : ContentPage
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    public FileMain()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateFileslist();
    }

    private void UpdateFileslist()
    {
        fileList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));
        fileList.SelectedItem = null;
    }

    private void Delete_Clicked(object sender, EventArgs e)
    {
        string fileName = (string)((MenuItem)sender).BindingContext;
        File.Delete(Path.Combine(folderPath, fileName));
        UpdateFileslist();
    }

    private void ToList_Clicked(object sender, EventArgs e)
    {
        string fileName = (string)((MenuItem)sender).BindingContext;
        List<string> list = File.ReadLines(Path.Combine(folderPath, fileName)).ToList();
        listFailist.ItemsSource = list;
    }

    private void fileList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) { return; }
        string fileName = e.SelectedItem.ToString();
        textEditor.Text = File.ReadAllText(Path.Combine(folderPath, fileName));
        fileNameEntry.Text = fileName;
        fileList.SelectedItem = null;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        string fileName = fileNameEntry.Text;
        if (string.IsNullOrEmpty(fileName)) { return; }
        if (File.Exists(Path.Combine(folderPath, fileName)))
        {
            bool isRewrited = await DisplayAlert("Kinnitus", "Fail on juba olemas. kas tahas ümber kirjutada?", "jah", "ei");
            if (isRewrited == false) { return; }
        }
        File.WriteAllText(Path.Combine(folderPath, fileName), textEditor.Text);
    }
}