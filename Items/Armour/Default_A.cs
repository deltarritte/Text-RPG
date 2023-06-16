using RPGTestC.Events;

namespace RPGTestC.Items.Armour
{
    public class Default_A : Item
    {
        public Default_A()
        {
            ID = 0x000001;
            ItemType = Type.Armour;
            MaxLVL = 3;
            Prefix = "Default";
            Name = "Armour";
            Description = "Default Armour";

            Defence = 1;
        }

        public override void OnUse(Monster[] monsters, int index = 0) { }

        public override void Upgrade() => Defence = LVL + 1;
    }
}
