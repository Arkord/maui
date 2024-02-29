using Microsoft.Maui.Controls.Shapes;

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

	private async void OnFromResult(object sender, EventArgs e)
	{
		PBar.Progress = 0;

		Progress<int> progress = new Progress<int>();
        progress.ProgressChanged += (sender, e) =>
        {
            Console.WriteLine($"Progress: {e}%");
			
			PBar.Progress = Convert.ToDouble(e) / 1+00;
        };

        await ProcessData(progress);
        Console.WriteLine("Process completed.");
	}

	
	static async Task ProcessData(IProgress<int> progress)
    {
        for (int i = 0; i <= 100; i++)
        {
            // Simulate some long-running operation
            await Task.Delay(100);

            // Report progress
            progress.Report(i);
        }
    }

	protected void RotateMatrices(IEnumerable<Matrix> matrices, float degrees)
	{
		Parallel.ForEach(matrices, matrix => matrix.Rotate(degrees));
	}

	protected void ProcessData()
    {
        // Create a list of items to process
        List<int> items = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Process each item in the list concurrently
        Parallel.ForEach(items, item =>
        {
            // Simulate processing time
            Task.Delay(1000).Wait();
            Console.WriteLine($"Processed item {item}");
        });

        Console.WriteLine("All items processed");
    }

	protected void InvertMatrices(IEnumerable<Matrix> matrices)
	{
		Parallel.ForEach(matrices, (matrix, state) =>
		{
			// if (!matrix)
			// 	state.Stop();
			// else
			// 	matrix.Invert();
		});
	}

	private void OnBtnAggregation(object sender, EventArgs e)
	{
		IEnumerable<int> numbers = new List<int> { 1, 2, 3, 4, 5 };

		int result = ParallelSum(numbers);
		Console.WriteLine($"the result is {result}");
	}

	private int ParallelSum(IEnumerable<int> values)
	{
		object mutex = new object();
		int result = 0;
		Parallel.ForEach(
			source: values,
			localInit: () => 0,
			body: (
					item, 
					state, 
					localValue
				) => localValue + item,
				localFinally: localValue =>
				{
					lock (mutex)
					result += localValue;
				}
			);
		return result;
	}	

	void ProcessArray(double[] array)
	{
		Parallel.Invoke(
			() => ProcessPartialArray(array, 0, array.Length / 2),
			() => ProcessPartialArray(array, array.Length / 2, array.Length)
		);
	}

	void ProcessPartialArray(double[] array, int begin, int end)
	{
		// CPU-intensive processing...
	}

}

