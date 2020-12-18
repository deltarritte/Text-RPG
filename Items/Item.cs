using RPGTestC.Events;
using RPGTestC.Items.Armour;
using RPGTestC.Items.Weapons;
using System;

namespace RPGTestC.Items
{
    public class Item
    {
        public enum Type
        {
            None,
            Armour,
            Weapon,
            Item,
            QuestItem
        }
        public Type IType = Type.None;

        public int ID = 0;
        public int LVL = 0;
        public int MaxLVL = 0;
        public string Prefix;
        public string Name;
        public string Description = "Нет описания";
        public ConsoleColor Colour = ConsoleColor.Gray;
        
        public float baseDamage = 0;
        public float Defence = 0;
        public float Damage;

        public bool Usable = true;

        public Item() { }

        public Item(Item item)
        {
            IType = item.IType;
            LVL = item.LVL;
            MaxLVL = item.MaxLVL;
            Prefix = item.Prefix;
            Name = item.Name;
            Description = item.Description;
            baseDamage = item.baseDamage;
            Defence = item.Defence;
            Usable = item.Usable;
        }

        public string GetName()
        {
            if (Prefix != "") return $"{Prefix} " + Name;
            else return Name;
        }

        public bool Upgradeable() => LVL < MaxLVL;

        // Для эффектов (обычно действующих на игрока)
        public virtual void OnUse() { }
        // Для эффектов, действующих на нападающих/всех(не исключая игрока)
        public virtual void OnUse(Monster[] monsters, int index = 0) { }
        // У каждого предмета статы улучшаются индивидуально, поэтому приходится так выкручиваться
        public virtual void Upgrade() { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
