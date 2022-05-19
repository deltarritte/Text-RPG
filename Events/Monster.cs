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
        enum AttackState : Int16
        {
            Aggressive,
            Balanced,
            Passive
        }

        int ActionPoints = 100;

        public string Name = "Монстр";
        public int LVL = Player.LVL;
        public int effectCooldown = 0;
        float MaxHP;
        public float HP;
        float bufDamage;
        public float Damage;
        public bool isInvincible = false;
        public MType Type = MType.Standart;
        AttackState AState = AttackState.Aggressive;

        public RPG.ChainAction[] ActionChain = new RPG.ChainAction[5];

        RPG.ChainAction Wait = new RPG.ChainAction();
        RPG.ChainAction Attack = new RPG.ChainAction();
        RPG.ChainAction Defend = new RPG.ChainAction();
        RPG.ChainAction Spellcast = new RPG.ChainAction();
        void SetupActions()
        {
            Wait.name = 'W';
            Wait.cost = 0;
            Wait.weight = 0;
            Wait.num = 0;

            Attack.name = 'A';
            Attack.cost = 25;
            //Attack.weight = 0.5f;
            Attack.num = 1;

            Spellcast.name = 'S';
            Spellcast.cost = 30;
            //Spellcast.weight = 0.4f;
            Spellcast.num = 2;

            Defend.name = 'D';
            Defend.cost = 20;
            //Defend.weight = 0
            Defend.num = 3;
        }

        public Monster(bool blank = false)
        {

            if (!blank)
            {
                HP = (float)(20 * RPG.rnd.Next(110, 120) / 100f * Math.Pow(1.63d, LVL));
                MaxHP = HP;
                Damage = (float)Math.Round((10 + Math.Pow(2, LVL - 2)) * RPG.rnd.Next(2, 4));
            }

            SetupActions();
            SetDefaults();
        }
        public Monster(int lvl)
        {
            if (lvl < 0) LVL = 0;
            else LVL = lvl;
            HP = (float)(20 * RPG.rnd.Next(110, 120) / 100f * Math.Pow(1.63d, lvl));
            MaxHP = HP;
            Damage = (float)Math.Round((10 + Math.Pow(2, lvl - 2)) * RPG.rnd.Next(2, 4));

            SetupActions();
            SetDefaults();
        }
        public void AI()
        {
            int ChainCost = 0;
            if (effectCooldown == 0) Spellcast.weight = 0.3f;

            if (HP < MaxHP / 3) AState = AttackState.Passive;
            else if (HP < 2 * MaxHP / 3) AState = AttackState.Balanced;
            else AState = AttackState.Aggressive;

            switch (AState)
            {
                case AttackState.Aggressive:
                    Attack.weight = 0.6f;
                    Defend.weight = 1 - Spellcast.weight - Attack.weight;
                    break;
                case AttackState.Balanced:
                    Attack.weight = (1 - Spellcast.weight) / 2;
                    Defend.weight = Attack.weight;
                    break;
                case AttackState.Passive:
                    Defend.weight = 0.6f;
                    Attack.weight = 1 - Spellcast.weight - Defend.weight;
                    break;
                default:
                    break;
            }

            for(int i = 0; i < ActionChain.Length; i++)
            {
                float FatePoint = (float)RPG.rnd.NextDouble();

                if (ChainCost + Attack.cost <= ActionPoints && FatePoint <= Attack.weight)
                {
                    ActionChain[i] = Attack;
                    ChainCost += Attack.cost;
                }
                else if (ChainCost + Spellcast.cost <= ActionPoints && FatePoint <= Attack.weight + Spellcast.weight)
                {
                    ActionChain[i] = Spellcast;
                    ChainCost += Spellcast.cost;
                    Spellcast.weight = 0f;
                }
                else if (ChainCost + Defend.cost <= ActionPoints)
                {
                    ActionChain[i] = Defend;
                    ChainCost += Defend.cost;
                }
                else ActionChain[i] = Wait;
            }
        }
        void SetDefaults() => bufDamage = Damage;
        public void ResetDamage() => Damage = bufDamage;
        public string GetActionChain()
        {
            string chain = "-";

            for (int i = 0; i < ActionChain.Length; i++)
                chain += ActionChain[i].name + "-";

            return chain;
        }
        public string GetAState()
        {
            switch (AState)
            {
                case AttackState.Aggressive:
                    return "Aggressive";

                case AttackState.Balanced:
                    return "Balance";

                case AttackState.Passive:
                    return "Passive";

                default:
                    return "What?";
            }
        }
    }
}
