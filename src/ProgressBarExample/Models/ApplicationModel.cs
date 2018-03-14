using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProgressBarExample.Models
{
	public class ApplicationModel
	{
		public event Action<bool> StatusChanged;

		public event Action<double, double> ProgressChanged;

		public event Action<string> LogRecordCreated;

		public ApplicationModel() { }

		public Task StartProcessAsync(CancellationToken cancellationToken)
		{
			return Task.Run(async () =>
			{
				await ProcessDataAsync(cancellationToken).ConfigureAwait(false);
			});
		}

		private async Task ProcessDataAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			AddLogRecord("Entering to ProcessDataAsync...");

			StatusChanged?.Invoke(true);
			await Task.Delay(3000).ConfigureAwait(false);

			AddLogRecord("Switch to determinant state...");

			StatusChanged?.Invoke(false);

			AddLogRecord("Switching completed...");

			var rnd = new Random();

			double maximum = 0.0, current = 0.0;

			while (!cancellationToken.IsCancellationRequested)
			{
				await Task.Delay(3000).ConfigureAwait(false);

				if (cancellationToken.IsCancellationRequested) break;

				var nextValue = 30.0 * rnd.NextDouble();
				maximum += nextValue;
				current += (2.0 * nextValue * rnd.NextDouble());

				AddLogRecord("Updating progress values...");

				ProgressChanged?.Invoke(maximum, current);
			}
		}

		private void AddLogRecord(string message)
		{
			var currentDate = DateTime.UtcNow;
			var threadId = Thread.CurrentThread.ManagedThreadId;

			string result = string.Format("{0:u} [thread #{1}]: {2}\r\n", currentDate, threadId, message);

			LogRecordCreated?.Invoke(result);
		}
	}
}