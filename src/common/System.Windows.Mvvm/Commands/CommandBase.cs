using System;

namespace System.Windows.Input
{
	public abstract class CommandBase : ICommand
	{
		#region Events

		public event EventHandler CanExecuteChanged
		{
			add
			{
				CommandManager.RequerySuggested += value;
			}
			remove
			{
				CommandManager.RequerySuggested -= value;
			}
		}
		#endregion

		#region Constructors

		protected internal CommandBase() { }
		#endregion

		#region Public class methods

		public abstract bool CanExecute(object parameter);

		public abstract void Execute(object parameter);
		#endregion
	}
}