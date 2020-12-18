using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGTestC.Events;
using static RPGTestC.Player;

namespace RPGTestC.Items.Armour
{
    public class Ying_A : Item
    {
        const float Absorb = 0.3f;

        public Ying_A()
        {
            ID = 2;
            IType = Type.Armour;
            MaxLVL = 20;
            LVL = 15;
            Name = "Броня Инь";
            Description = $"Защита сил тьмы. Поглощает {(1-Absorb)*100}% урона." +
                $"Снимает статусные эффекты.";
            
            Defence = 77;
        }

        public override void OnUse(Monster[] monsters, int index = 0)
        {
            if (PStatus != 0)
            {
                Fight.effectCount = 0;
                PStatus = 0;
            }
            monsters[index].Damage *= Absorb;
        }

        public override void Upgrade() => Defence = 77 * LVL / 15f;
    }
}
