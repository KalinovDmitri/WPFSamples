using System;

namespace System.Windows.Input
{
	public class DelegateCommand : CommandBase
	{
		#region Fields

		private Func<bool> Resolver;

		private Action Executor;
		#endregion

		#region Constructors

		public DelegateCommand(Action executor, Func<bool> resolver = null)
		{
			if (executor == null)
			{
				throw new ArgumentNullException(nameof(executor), "Executor delegate can't be null.");
			}

			Executor = executor;
			Resolver = resolver ?? AlwaysTrue;
		}
		#endregion

		#region CommandBase methods overriding

		public override bool CanExecute(object parameter)
		{
			return Resolver.Invoke();
		}

		public override void Execute(object parameter)
		{
			Executor.Invoke();
		}
		#endregion

		#region Private class methods

		private static bool AlwaysTrue() => true;
		#endregion
	}
}