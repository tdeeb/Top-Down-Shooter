﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Top_Down_Shooter
{
    //The base class for a level; it contains all the level objects
    //This class may be instantiated instead of used purely as a base class
    public class BaseLevel
    {
        private KeyboardState LevelKeyboard;
        public List<LevelObject> levelObjects;
        
        //The player reference
        public Character Player;

        //The camera for the level
        public Camera LevelCam;

        //The level's tile engine
        public TileEngine TileEngine;

        public BaseLevel()
        {
            levelObjects = new List<LevelObject>();

            LevelKeyboard = new KeyboardState(Keys.Enter);
            TileEngine = new TileEngine(Main.ScreenSize * 2);
            LevelCam = new Camera(this);
        }

        public void AddObject(LevelObject levelobject)
        {
            if (levelobject != null)
            {
                levelobject.Level = this;
                levelObjects.Add(levelobject);
            }
        }

        public void AddPlayer(Character player)
        {
            Player = player;
            AddObject(Player);
        }

        private void HitboxCollision(int index)
        {
            LevelObject victim = levelObjects[index];

            //Check for collisions
            for (int i = 0; i < levelObjects.Count; i++)
            {
                LevelObject attacker = levelObjects[i];

                //An object can't hit itself, so don't bother checking for that
                if (i != index && attacker.CollisionOwner != victim)
                {
                    //If an object has a hitbox and touched the hurtbox of the object we're checking for, make that object take damage
                    if (attacker.hitbox != null && attacker.hitbox.CanHitObject(victim.hurtbox) == true)
                    {
                        victim.TakeDamage(attacker.hitbox);

                        //Do something extra to either object after the damage has been done (projectile exploding, etc.)
                        attacker.DamagedObject(victim);

                        //If the object gained invincibility after taking damage, break out of the loop since there's no way it can get hurt now
                        if (victim.hurtbox.IsInvincible == true) break;
                    }
                }
            }
        }

        public void Update(Main main)
        {
            //Check for pausing/unpausing the game
            if (Input.IsKeyDown(LevelKeyboard, Keys.Enter) == true)
            {
                if (main.GameState == GameState.InGame)
                    main.ChangeGameState(GameState.Paused);
                else main.ChangeGameState(GameState.InGame);
            }

            if (main.GameState == GameState.InGame)
            {
                //Update all level objects
                for (int i = 0; i < levelObjects.Count; i++)
                {
                    //Check if the object has a hurtbox and can get hurt
                    if (levelObjects[i].hurtbox != null && levelObjects[i].hurtbox.CanTakeDamage() == true)
                    {
                        //Check if any object's hitbox hit this one's hurtbox and take any necessary actions
                        HitboxCollision(i);
                    }

                    //Check if the object is dead and remove it if so
                    if (levelObjects[i].IsDead == true)
                    {
                        levelObjects.RemoveAt(i);
                        i--;
                    }
                    //Otherwise update the object
                    else
                    {
                        //Store the number of level objects
                        int oldcount = levelObjects.Count;

                        //Update the object
                        levelObjects[i].Update();

                        //Check if any objects were removed (Ex. collecting a Powerup) and decrease the index by that amount if so
                        int countdifference = oldcount - levelObjects.Count;
                        if (countdifference > 0) i -= countdifference;
                    }
                }
            }

            LevelCam.Update();
            LevelKeyboard = Keyboard.GetState();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < levelObjects.Count; i++)
                levelObjects[i].Draw(spriteBatch);
            
            if (Debug.TileDraw == true)
                TileEngine.Draw(spriteBatch);
        }
    }
}
