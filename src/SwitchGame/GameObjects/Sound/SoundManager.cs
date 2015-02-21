using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Switch.GameObjects.Sound
{
    class SoundManager
    {
        private static SoundManager instance;
        private Dictionary<String, SoundEffect> sounds;
        private Dictionary<String, Song> songs;
        private ContentManager contentManager;
        private bool songStarted;
        private bool contentLoaded;
        private bool musicEnabled;
        private String currentSong;
        private bool musicPaused;

        private SoundManager()
        {
            sounds = new Dictionary<String, SoundEffect>();
            songs = new Dictionary<String, Song>();

            songStarted = false;
            contentLoaded = false;
            musicEnabled = !SwitchGame.DEBUG_MODE;
            musicPaused = false;
            currentSong = "";
        }

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        public void setContentManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public void loadSoundMedia(ContentManager contentManager)
        {
            if (!contentLoaded)
            {
                setContentManager(contentManager);

                sounds.Add("flip", contentManager.Load<SoundEffect>("Sound\\Effects\\flip"));
                sounds.Add("explode-tile", contentManager.Load<SoundEffect>("Sound\\Effects\\explode-tile"));
                sounds.Add("laser", contentManager.Load<SoundEffect>("Sound\\Effects\\laser"));
                sounds.Add("nuke-explode", contentManager.Load<SoundEffect>("Sound\\Effects\\nuke-explode"));
                sounds.Add("bullettime", contentManager.Load<SoundEffect>("Sound\\Effects\\bullettime"));
                sounds.Add("levelup", contentManager.Load<SoundEffect>("Sound\\Effects\\levelup"));
                sounds.Add("menu-select", contentManager.Load<SoundEffect>("Sound\\Effects\\menu-select"));
                sounds.Add("menu-select2", contentManager.Load<SoundEffect>("Sound\\Effects\\menu-select2"));
                sounds.Add("wombat-growl", contentManager.Load<SoundEffect>("Sound\\Effects\\wombat-growl"));
                sounds.Add("readySet", contentManager.Load<SoundEffect>("Sound\\Effects\\readySet"));
                sounds.Add("go", contentManager.Load<SoundEffect>("Sound\\Effects\\go"));
                sounds.Add("2p-alarm", contentManager.Load<SoundEffect>("Sound\\Effects\\2p-alarm"));
                sounds.Add("game-over", contentManager.Load<SoundEffect>("Sound\\Effects\\game-over"));
                sounds.Add("player-select", contentManager.Load<SoundEffect>("Sound\\Effects\\player-select"));

                songs.Add("gameplay-song", contentManager.Load<Song>("Sound\\Music\\HouseMoFo"));
                songs.Add("menu-song", contentManager.Load<Song>("Sound\\Music\\Religions"));

                contentLoaded = true;
            }
        }
         
        public void playSound(String soundName) 
        {
            if(sounds.ContainsKey(soundName)) {
                sounds[soundName].Play();
            }
        }

        public void playSong(String songName)
        {
            if ((!songStarted || songName != currentSong) && 
                songs.ContainsKey(songName) && 
                musicEnabled &&
                !musicPaused)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.Play(songs[songName]);
                songStarted = true;
                currentSong = songName;
            }
        }

        public void stopSong()
        {
            MediaPlayer.Stop();
            songStarted = false;
        }

        public bool isSongPlaying()
        {
            return songStarted;
        }

        public bool isMusicPaused()
        {
            return musicPaused;
        }

        public void setMusicEnabled(bool musicEnabled)
        {
            this.musicEnabled = musicEnabled;
            if (!this.musicEnabled && isSongPlaying())
            {
                stopSong();
            }
        }

        public void setMusicPaused(bool musicPaused)
        {
            this.musicPaused = musicPaused;
            if (this.musicPaused && isSongPlaying())
            {
                stopSong();
            }
        }
    }
}
