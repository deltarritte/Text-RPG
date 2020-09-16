using RPGTestC.Events;
using RPGTestC.Items.Armour;
using RPGTestC.Items.Weapons;

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

        public int LVL = 0;
        public int MaxLVL = 0;
        public string Prefix;
        public string Name;
        public string Description = "Нет описания";
        
        public float baseDamage = 0;
        public float Defence = 0;
        public float Damage;

        public bool Usable = true;

        public string GetName()
        {
            if (Prefix != "") return $"{Prefix} " + Name;
            else return Name;
        }

        public bool Upgradeable() => LVL < MaxLVL;

        // Для эффектов (обычно действующих на игрока)
        public virtual void OnUse() { }
        // Для эффектов, действующих на нападающего (обычно используется для оружия)
        public virtual void OnUse(Monster monster) { }
        // Для эффектов, действующих на нападающих/всех(не исключая игрока) (обычно используется для предметов)
        public virtual void OnUse(Monster[] monsters) { }
        // У каждого предмета статы улучшаются индивидуально, поэтому приходится так выкручиваться
        public virtual void Upgrade() { }

        public static Item[] ItemList = new Item[7]
        {
            new None_Item(),    // 0 - Ничего
            new Default_A(),    // 1 - Стандартная броня
            new Ying_A(),       // 2 - Броня Инь
            new Default_W(),    // 3 - Стандартное оружие
            new Yang_W(),       // 4 - Меч Ян
            new PotionBag(),    // 5 - Сумка с Зельями
            new None_Item(),    // 6 - Зеркало Баланса
        };
    }
}
