﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Top_Down_Shooter
{
    public sealed class Character1 : Character
    {
        public Character1()
        {
            MoveSpeed = new Vector2(3, 3);

            Health = 100;

            //PickupGun(new MachineGun(ObjectPos, ObjectDir, 100, 100, 200));
            //PickupGun(new MachineGun(ObjectPos, ObjectDir, 200, 200, 500));

            //BackupGun = (weak gun with infinite ammo here);
            
            hurtbox = new Hurtbox(this, Helper.CreateRect(ObjectPos, ObjectTexture.Width, ObjectTexture.Height), 0);

            //PlayerGun.GunOwner = this;
        }


    }
}
