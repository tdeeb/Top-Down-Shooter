﻿using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SharpDX;
using SharpDX.XAudio2;
using SharpDX.Windows;
using Windows.Storage;

namespace Top_Down_Shooter
{
    //A sound manager that contains methods for playing sounds as well as containing global sound and music volumes
    public static class SoundManager
    {
        public static float SoundVolume;
        public static float MusicVolume;
        
        static SoundManager()
        {
            //Initialize them to the middle value of .5f
            SoundVolume = MusicVolume = MediaPlayer.Volume = .5f;
        }

        // Sets the sound volume
        public static void SetSoundVolume(float volume)
        {
            SoundVolume = volume;
        }

        // Sets the music volume
        public static void SetMusicVolume(float volume)
        {
            MusicVolume = MediaPlayer.Volume = volume;
        }

        //Plays a sound with the default pitch and pan values
        public static void PlaySound(SoundEffect sound)
        {
            if (sound != null)
            {
                sound.Play(SoundVolume, 0f, 0f);
            }
        }

        //Plays a song, stopping the previous song first - you can choose to not loop the song if you wish, but the default will be set to loop
        public static void PlaySong(Song song, bool loop = true)
        {
            if (song != null)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = loop;
            }
        }

        public static void SaveVolumeSettings()
        {
            // Put the two volume settings in an array
            float[] Volumes = new float[2] { MusicVolume, SoundVolume };

            // Store the music and sound volume settings in a HighPriority roaming setting for instant syncing across devices
            ApplicationData.Current.RoamingSettings.Values["HighPriority"] = Volumes;
        }

        public static void LoadVolumeSettings()
        {
            // Check if the player's volume settings were previously saved
            if (ApplicationData.Current.RoamingSettings.Values["HighPriority"] != null)
            {
                // Retrieve the volume values
                float[] Volumes = (float[])ApplicationData.Current.RoamingSettings.Values["HighPriority"];

                // Set the Music and Sound volumes
                SetMusicVolume(Volumes[0]);
                SetSoundVolume(Volumes[1]);
            }
        }


    }
}