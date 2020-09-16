using System;

namespace RPGTestC.Events
{
    public class Monster
    {
        public enum MType : Int32
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
        float bufDamage;
        public float Damage;
        public bool isInvincible = false;
        public MType Type = MType.Standart;
        
        public Monster(bool blank = false)
        {
            if (!blank)
            {
                HP = (float)(20 * RPG.rnd.Next(110, 120) / 100f * Math.Pow(1.63d, LVL));
                Damage = (float)Math.Round((10 + Math.Pow(2, LVL - 2)) * RPG.rnd.Next(2, 4));
            }
            SetDefaults();
        }
        public Monster(int lvl)
        {
            if (lvl < 0) LVL = 0;
            else LVL = lvl;
            HP = (float)(20 * RPG.rnd.Next(110, 120) / 100f * Math.Pow(1.63d, lvl));
            Damage = (float)Math.Round((10 + Math.Pow(2, lvl - 2)) * RPG.rnd.Next(2, 4));
            SetDefaults();
        }
        void SetDefaults() => bufDamage = Damage;
        public void ResetDamage() => Damage = bufDamage;
    }
}
