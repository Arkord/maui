namespace MauiApp1;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}


    private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private void DoSomethingAsync(object sender, EventArgs e)
	{
		// int value = 13;
		// // Asynchronously wait 1 second.
		// await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
		// value *= 2;
		// // Asynchronously wait 1 second.
		// await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
		// Console.WriteLine(value);

		var bynderContents =  DoCurlAsync();
	}
	
	public async Task<string> DoCurlAsync()
	{
		using (var httpClient = new HttpClient())
		using (var httpResonse = await httpClient.GetAsync("https://www.bynder.com"))
		{
			return await httpResonse.Content.ReadAsStringAsync();
		}
	}

	private void OnTestRequest(object sender, EventArgs e)
	{
		HttpClient client = new HttpClient();
        _ = DownloadStringWithRetries(client, "https://jsonplaceholder.typicode.com/posts");
	}


	async Task<string> DownloadStringWithRetries(HttpClient client, string uri)
	{
		// Retry after 1 second, then after 2 seconds, then 4.
		TimeSpan nextDelay = TimeSpan.FromSeconds(1);
		for (int i = 0; i != 3; ++i)
		{
			try
			{
				return await client.GetStringAsync(uri);
			}
			catch
			{

			}
			await Task.Delay(nextDelay);
			nextDelay = nextDelay + nextDelay;
		}
		// Try one last time, allowing the error to propagate.
		return await client.GetStringAsync(uri);
	}

}

