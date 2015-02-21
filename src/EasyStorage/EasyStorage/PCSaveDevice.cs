using System;
using System.IO;

namespace EasyStorage
{
#if WINDOWS
	/// <summary>
	/// A SaveDevice exclusive to Windows that does not use the XNA Storage APIs or
	/// rely on any of the GamerServices. Since we implement ISaveDevice, all the
	/// methods must take a containerName for compatibility, but this implementation
	/// does not use that value for anything; all files are stored directly in the
	/// RootDirectory of the device.
	/// </summary>
	public class PCSaveDevice : ISaveDevice
	{
		/// <summary>
		/// Gets or sets the root directory for where file operations
		/// will occur.
		/// </summary>
		/// <remarks>
		/// The default value is a folder in the current user's 
		/// folder under %USER%/Saved Games/%gameName%.
		/// </remarks>
		public string RootDirectory { get; set; }

		/// <summary>
		/// Creates a new PCSaveDevice.
		/// </summary>
		/// <param name="gameName">The name of the game in use. Used to initialize the RootDirectory.</param>
		public PCSaveDevice(string gameName)
		{
			// we save in %USER%/Saved Games/%gameName%
			RootDirectory = Path.Combine(Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "Saved Games"), gameName);
		}

		/// <summary>
		/// Saves a file.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <param name="fileName">The file to save.</param>
		/// <param name="saveAction">The save action to perform.</param>
		/// <returns>True if the save completed without errors, false otherwise.</returns>
		public bool Save(string containerName, string fileName, FileAction saveAction)
		{
			if (!Directory.Exists(RootDirectory))
				Directory.CreateDirectory(RootDirectory);

			string path = Path.Combine(RootDirectory, fileName);
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
					saveAction(stream);
				return true;
			}
			catch 
			{
				return false; 
			}
		}

		/// <summary>
		/// Loads a file.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <param name="fileName">The file to load.</param>
		/// <param name="loadAction">The load action to perform.</param>
		/// <returns>True if the load completed without error, false otherwise.</returns>
		public bool Load(string containerName, string fileName, FileAction loadAction)
		{
			if (!Directory.Exists(RootDirectory))
				Directory.CreateDirectory(RootDirectory);

			string path = Path.Combine(RootDirectory, fileName);
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
					loadAction(stream);
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Deletes a file.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <param name="fileName">The file to delete.</param>
		/// <returns>True if the file either doesn't exist or was deleted succesfully, false if the file exists but failed to be deleted.</returns>
		public bool Delete(string containerName, string fileName)
		{
			if (!Directory.Exists(RootDirectory))
				Directory.CreateDirectory(RootDirectory);

			string path = Path.Combine(RootDirectory, fileName);
			if (File.Exists(path))
			{
				File.Delete(path);
				return !File.Exists(path);
			}
			return true;
		}

		/// <summary>
		/// Determines if a given file exists.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>True if the file exists, false otherwise.</returns>
		public bool FileExists(string containerName, string fileName)
		{
			if (!Directory.Exists(RootDirectory))
				Directory.CreateDirectory(RootDirectory);

			return File.Exists(Path.Combine(RootDirectory, fileName));
		}
		
		/// <summary>
		/// Gets an array of all files available in the SaveDevice.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <returns>An array of file names of the files in the SaveDevice.</returns>
		public string[] GetFiles(string containerName)
		{
			return GetFiles(containerName, null, null);
		}

		/// <summary>
		/// Gets an array of all files available in the SaveDevice.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <param name="directory">A subdirectory to search in the SaveDevice.</param>
		/// <returns>An array of file names of the files in the SaveDevice.</returns>
		public string[] GetFiles(string containerName, string directory)
		{
			return GetFiles(containerName, directory, null);
		}

		/// <summary>
		/// Gets an array of all files available in the SaveDevice.
		/// </summary>
		/// <param name="containerName">Used to match the ISaveDevice interface; ignored by the implementation.</param>
		/// <param name="directory">A subdirectory to search in the SaveDevice.</param>
		/// <param name="pattern">A search pattern to use to find files.</param>
		/// <returns>An array of file names of the files in the SaveDevice.</returns>
		public string[] GetFiles(string containerName, string directory, string pattern)
		{
			if (!Directory.Exists(RootDirectory))
				Directory.CreateDirectory(RootDirectory);

			string path = RootDirectory;
			if (!string.IsNullOrEmpty(directory))
				path = Path.Combine(path, directory);

			string[] files = string.IsNullOrEmpty(pattern) ? Directory.GetFiles(path) : Directory.GetFiles(path, pattern);

			for (int i = 0; i < files.Length; i++)
				files[i] = files[i].Substring(RootDirectory.Length + 1);

			return files;
		}
	}
#endif
}
