using RPGTestC.Events;

namespace RPGTestC.Items.Weapons
{
    public class Default_W : Item
    {
        public Default_W()
        {
            IType = Type.Weapon;
            MaxLVL = 3;
            Prefix = "Default";
            Name = "Sword";
            Description = "Default Sword";

            baseDamage = 11;
        }

        public override void OnUse(Monster monster)
        {
            Damage = baseDamage;
        }

        public override void Upgrade() => baseDamage = 11 * (LVL + 1);
    }
}
