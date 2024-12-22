using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XNFS_TPKTool_GUI.ViewModels
{
    /// <summary>
    /// A base class for ViewModels to provide property change notifications.
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners about a property value change.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets a property value and raises the PropertyChanged event if the value changes.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">The backing field for the property.</param>
        /// <param name="value">The new value to set.</param>
        /// <param name="propertyName">The name of the property (optional).</param>
        /// <returns>True if the value was changed, false otherwise.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}