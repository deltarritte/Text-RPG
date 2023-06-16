using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGTestC.Events;

namespace RPGTestC.Items.Weapons
{
    public class Yang_W : Item
    {
        const float Multiplier = 1.5f;
        const float DarkMultiplier = 2f;

        public Yang_W()
        {
            ID = 0x000200;
            ItemType = Type.Weapon;
            MaxLVL = 20;
            LVL = 15;
            Name = "Меч Ян";
            Description = $"Ярость сил света. Наносит {DarkMultiplier * 100}% урона тёмным типам, и {Multiplier * 100}% остальным типам, кроме светящегося.";

            baseDamage = 77;
        }

        public override void OnUse(Monster[] monsters, int index = 0)
        {
            switch (monsters[index].Type)
            {
                case Monster.MType.Standart:
                case Monster.MType.Poisonous:
                case Monster.MType.Explosive:
                case Monster.MType.Thorned:
                case Monster.MType.Fire:
                case Monster.MType.Ice:
                    Damage = baseDamage * Multiplier;
                    break;

                case Monster.MType.Dark:
                    Damage = baseDamage * DarkMultiplier;
                    break;

                default:
                    Damage = baseDamage;
                    break;
            }
        }

        public override void Upgrade() => baseDamage = 77 * LVL / 15f;
    }
}
