using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Mvvm;

using ProgressBarExample.Models;

namespace ProgressBarExample.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private readonly INavigationService _navigationService;
		private readonly ApplicationModel _applicationModel;

		private bool _prepareProcessing;
		private bool _processingStarted = false;
		private double _maxProgress;
		private double _currentProgress;
		private string _runButtonText = "Run";
		private string _log;

		private CancellationTokenSource _cancellationSource;
		private Task _processingTask;

		public bool PrepareProcessing
		{
			get => _prepareProcessing;
			private set
			{
				if (_prepareProcessing != value)
				{
					_prepareProcessing = value;
					RaisePropertyChanged();
				}
			}
		}

		public bool ProcessingStarted
		{
			get => _processingStarted;
			private set
			{
				_processingStarted = value;
				RaisePropertyChanged();
			}
		}

		public double MaxProgress
		{
			get => _maxProgress;
			private set
			{
				_maxProgress = value;
				RaisePropertyChanged();
			}
		}

		public double CurrentProgress
		{
			get => _currentProgress;
			private set
			{
				_currentProgress = value;
				RaisePropertyChanged();
			}
		}

		public string RunButtonText
		{
			get => _runButtonText;
			private set
			{
				_runButtonText = value;
				RaisePropertyChanged();
			}
		}

		public string Log
		{
			get => _log;
			private set
			{
				_log = value;
				RaisePropertyChanged();
			}
		}

		private ICommand _startCommand;
		public ICommand StartProcessingCommand => _startCommand ?? (_startCommand = new DelegateCommand(StartStopProcessing));

		public MainViewModel(INavigationService navigationService, ApplicationModel applicationModel)
		{
			_navigationService = navigationService;
			_applicationModel = applicationModel;
		}

		protected override void OnViewLoaded(IView view)
		{
			base.OnViewLoaded(view);

			_applicationModel.StatusChanged += ApplicationModelOnStatusChanged;
			_applicationModel.ProgressChanged += ApplicationModelOnProgressChanged;
			_applicationModel.LogRecordCreated += ApplicationModelOnLogRecordCreated;
		}

		protected override void OnViewUnloaded()
		{
			base.OnViewUnloaded();

			_applicationModel.StatusChanged -= ApplicationModelOnStatusChanged;
			_applicationModel.ProgressChanged -= ApplicationModelOnProgressChanged;
			_applicationModel.LogRecordCreated -= ApplicationModelOnLogRecordCreated;
		}

		private void ApplicationModelOnLogRecordCreated(string logRecord)
		{
			Log += logRecord;
		}
		
		private void ApplicationModelOnStatusChanged(bool isIndeterminate)
		{
			PrepareProcessing = isIndeterminate;
			if (!isIndeterminate)
			{
				MaxProgress = 1;
				CurrentProgress = 0;
			}
		}

		private void ApplicationModelOnProgressChanged(double maximum, double current)
		{
			MaxProgress = maximum;
			CurrentProgress = current;
		}

		private async void StartStopProcessing()
		{
			AddLogRecord("Enter to StartStopProcessing...");

			if (_processingTask != null && _cancellationSource != null && !_cancellationSource.IsCancellationRequested)
			{
				_cancellationSource.Cancel();
				await _processingTask;

				ProcessingStarted = false;
				RunButtonText = "Run";
			}
			else
			{
				_cancellationSource = new CancellationTokenSource();
				_processingTask = _applicationModel.StartProcessAsync(_cancellationSource.Token);

				ProcessingStarted = true;
				RunButtonText = "Stop";
			}
		}

		private void AddLogRecord(string message)
		{
			var currentDate = DateTime.UtcNow;
			var threadId = Thread.CurrentThread.ManagedThreadId;

			string record = string.Format("{0:u} [thread #{1}]: {2}\r\n", currentDate, threadId, message);

			Log += record;
		}
	}
}