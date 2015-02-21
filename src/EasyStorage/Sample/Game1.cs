using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using EasyStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Sample
{
	// our test game. there's nothing visual to it. we prompt for a device and upon a successful
	// selection we run a few tests to make sure we can save, load, delete, and check for files.
	public class Game1 : Game
	{
		private readonly ISaveDevice saveDevice;

		// one object that we'll save and one for loading so we can compare
		// that not only did the file I/O work, but that we actually got
		// correct data back when we load the file
		private Foo fooSaved;
		private Foo fooLoaded;

		// a serializer for our Foo type
		private readonly XmlSerializer serializer = new XmlSerializer(typeof(Foo));

		public Game1()
		{
			new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			// we can set our supported languages explicitly or we can allow the
			// game to support all the languages. the first language given will
			// be the default if the current language is not one of the supported
			// languages. this only affects the text found in message boxes shown
			// by EasyStorage and does not have any affect on the rest of the game.
			EasyStorageSettings.SetSupportedLanguages(Language.French, Language.Spanish);

			// on PC we use a save that saves to the user's "Saved Games" folder instead
			// of using the Xna.Storage APIs

			// on Xbox, we use a save device that gets a shared StorageDevice to
			// handle our file IO.
#if WINDOWS
			saveDevice = new PCSaveDevice("EasyStorageTestGame");
			TestDevice();
#else
			// add the GamerServicesComponent
			Components.Add(new GamerServicesComponent(this));

			// create and add our SaveDevice
			SharedSaveDevice sharedSaveDevice = new SharedSaveDevice();
			Components.Add(sharedSaveDevice);

			// hook an event for when the device is selected to run our test
			sharedSaveDevice.DeviceSelected += (s, e) => TestDevice();
			
			// hook two event handlers to force the user to choose a new device if they cancel the
			// device selector or if they disconnect the storage device after selecting it
			sharedSaveDevice.DeviceSelectorCanceled += (s, e) => e.Response = SaveDeviceEventResponse.Force;
			sharedSaveDevice.DeviceDisconnected += (s, e) => e.Response = SaveDeviceEventResponse.Force;

			// prompt for a device on the first Update we can
			sharedSaveDevice.PromptForDevice();

			// make sure we hold on to the device
			saveDevice = sharedSaveDevice;
#endif
		}

		// runs a small set of tests to validate the methods of an ISaveDevice
		private void TestDevice()
		{
			const string containerName = "Test Container";

			const string fileName1 = "Test1.xml";
			const string fileName2 = "Test2.xml";
			const string fileName3 = "Test3.xml";

			// serialize out some XML data
			if (!saveDevice.Save(containerName, fileName1, SerializeTest))
				Trace.WriteLine("Failed to save file.");

			// make sure we can see the file
			if (!saveDevice.FileExists(containerName, fileName1))
				Trace.WriteLine("Failed to find file.");

			// load it back in
			if (!saveDevice.Load(containerName, fileName1, DeserializeTest))
				Trace.WriteLine("Failed to load file.");

			// make sure our two foo objects actually are the same
			if (!fooSaved.Equals(fooLoaded))
				Trace.WriteLine("Loaded object not the same as saved object.");

			// and delete it
			if (!saveDevice.Delete(containerName, fileName1))
				Trace.WriteLine("Failed to delete file");

			// save a few more files to test out
			if (!saveDevice.Save(containerName, fileName2, SerializeTest))
				Trace.WriteLine("Failed to save file.");

			// sleep a few ms because SerializeTest uses Random to generate
			// values and we want different values
			Thread.Sleep(5);

			if (!saveDevice.Save(containerName, fileName3, SerializeTest))
				Trace.WriteLine("Failed to save file.");

			// get the list of files on disk and verify them
			string[] files = saveDevice.GetFiles(containerName);
			if (files.Length != 2)
				Trace.WriteLine("Didn't find all files.");
			foreach (var f in files)
				Trace.WriteLine(f + " found");
		}

		private void SerializeTest(Stream stream)
		{
			fooSaved = new Foo();
			Trace.WriteLine("Foo Saved: " + fooSaved);
			serializer.Serialize(stream, fooSaved);
		}

		private void DeserializeTest(Stream stream)
		{
			fooLoaded = serializer.Deserialize(stream) as Foo;
			Trace.WriteLine("Foo Loaded: " + fooLoaded);
		}

		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(gameTime);
		}
	}
}
