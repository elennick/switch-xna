using System.IO;

namespace EasyStorage
{
	/// <summary>
	/// A method for loading or saving a file.
	/// </summary>
	/// <param name="stream">A Stream to use for accessing the file data.</param>
	public delegate void FileAction(Stream stream);

	/// <summary>
	/// Defines the interface for an object that can perform file operations.
	/// </summary>
	public interface ISaveDevice
	{
		/// <summary>
		/// Saves a file.
		/// </summary>
		/// <param name="containerName">The name of the container in which to save the file.</param>
		/// <param name="fileName">The file to save.</param>
		/// <param name="saveAction">The save action to perform.</param>
		/// <returns>True if the save completed without errors, false otherwise.</returns>
		bool Save(string containerName, string fileName, FileAction saveAction);

		/// <summary>
		/// Loads a file.
		/// </summary>
		/// <param name="containerName">The name of the container from which to load the file.</param>
		/// <param name="fileName">The file to load.</param>
		/// <param name="loadAction">The load action to perform.</param>
		/// <returns>True if the load completed without error, false otherwise.</returns>
		bool Load(string containerName, string fileName, FileAction loadAction);

		/// <summary>
		/// Deletes a file.
		/// </summary>
		/// <param name="containerName">The name of the container from which to delete the file.</param>
		/// <param name="fileName">The file to delete.</param>
		/// <returns>True if the file either doesn't exist or was deleted succesfully, false if the file exists but failed to be deleted.</returns>
		bool Delete(string containerName, string fileName);

		/// <summary>
		/// Determines if a given file exists.
		/// </summary>
		/// <param name="containerName">The name of the container in which to check for the file.</param>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>True if the file exists, false otherwise.</returns>
		bool FileExists(string containerName, string fileName);
		
		/// <summary>
		/// Gets an array of all files available in a container.
		/// </summary>
		/// <param name="containerName">The name of the container in which to search for files.</param>
		/// <returns>An array of file names of the files in the container.</returns>
		string[] GetFiles(string containerName);
        
		/// <summary>
		/// Gets an array of all files available in a container.
		/// </summary>
		/// <param name="containerName">The name of the container in which to search for files.</param>
		/// <param name="directory">A subdirectory to search in the container.</param>
		/// <returns>An array of file names of the files in the container.</returns>
		string[] GetFiles(string containerName, string directory);
        
		/// <summary>
		/// Gets an array of all files available in a container.
		/// </summary>
		/// <param name="containerName">The name of the container in which to search for files.</param>
		/// <param name="directory">A subdirectory to search in the container.</param>
		/// <param name="pattern">A search pattern to use to find files.</param>
		/// <returns>An array of file names of the files in the container.</returns>
		string[] GetFiles(string containerName, string directory, string pattern);
	}
}
