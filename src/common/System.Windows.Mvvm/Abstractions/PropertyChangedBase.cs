using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace System.Windows.Mvvm
{
	public abstract class PropertyChangedBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected internal PropertyChangedBase() { }

		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}