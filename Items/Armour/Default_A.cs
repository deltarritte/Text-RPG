using RPGTestC.Events;

namespace RPGTestC.Items.Armour
{
    public class Default_A : Item
    {
        public Default_A()
        {
            IType = Type.Armour;
            MaxLVL = 3;
            Prefix = "Default";
            Name = "Armour";
            Description = "Default Armour";

            Defence = 1;
        }

        public override void OnUse(Monster monster) { }

        public override void Upgrade() => Defence = LVL + 1;
    }
}
