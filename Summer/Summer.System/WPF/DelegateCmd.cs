using System;
using System.Windows.Input;

namespace Summer.System.WPF
{
	public class DelegateCmd : ICommand
	{
		public DelegateCmd ( )
		{

		}

		public DelegateCmd ( Action<object> action )
		{
			this.action = action;
		}

		public Action<object> action;
		public void Execute ( object parameter )
		{
			if ( action != null )
				action.Invoke ( parameter );
		}

		public bool CanExecute ( object parameter )
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;
	}
}