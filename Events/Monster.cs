using RPGTestCBuildA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTestC.Events
{
    public class Monster
    {
        public enum MType
        {
            Standart,
            Poisonous,
            Explosive,
            Thorned,
            Fire,
            Ice,
            Dark,
            Luminous
        }

        public string Name = "Монстр";
        public int LVL = Player.LVL;
        public int effectCooldown = 0;
        public float HP;
        public float Damage;
        public bool isInvincible = false;
        public MType Type = MType.Standart;
        
        public Monster()
        {
            HP = (float)(20 * RPG.rnd.Next(110, 120) / 100f * Math.Pow(1.63d, LVL));
            Damage = (float)Math.Round((10 + Math.Pow(2, LVL - 2)) * RPG.rnd.Next(2, 4));
        }

        public Monster(int lvl)
        {
            LVL = lvl;
            HP = (float)(20 * RPG.rnd.Next(110, 120) / 100f * Math.Pow(1.63d, lvl));
            Damage = (float)Math.Round((10 + Math.Pow(2, lvl - 2)) * RPG.rnd.Next(2, 4));
        }

    }
}
