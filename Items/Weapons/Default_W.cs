using RPGTestC.Events;

namespace RPGTestC.Items.Weapons
{
    public class Default_W : Item
    {
        public Default_W(int id)
        {
            ID = id;
            IType = Type.Weapon;
            MaxLVL = 3;
            Prefix = "Default";
            Name = "Sword";
            Description = "Default Sword";

            baseDamage = 11;
        }

        public override void OnUse(Monster[] monsters, int index = 0)
        {
            Damage = baseDamage;
        }

        public override void Upgrade() => baseDamage = 11 * (LVL + 1);
    }
}
