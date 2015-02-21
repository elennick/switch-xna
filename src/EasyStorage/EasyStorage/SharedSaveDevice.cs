using System;
using Microsoft.Xna.Framework.GamerServices;

namespace EasyStorage
{
	/// <summary>
	/// A SaveDevice used for non player-specific saving of data.
	/// </summary>
	public sealed class SharedSaveDevice : SaveDevice
	{
		/// <summary>
		/// Derived classes should implement this method to call the Guide.BeginShowStorageDeviceSelector
		/// method with the desired parameters, using the given callback.
		/// </summary>
		/// <param name="callback">The callback to pass to Guide.BeginShowStorageDeviceSelector.</param>
		protected override void GetStorageDevice(AsyncCallback callback)
		{
			Guide.BeginShowStorageDeviceSelector(callback, null);
		}
	}
}