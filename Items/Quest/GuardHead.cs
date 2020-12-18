using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTestC.Items.Quest
{
    public class GuardHead : Item
    {
        public GuardHead()
        {
            IType = Type.QuestItem;
            Name = "Голова Стража";
            Description = "Голова Стража Синего Источника. Тяжеловата, да и кровь ещё течёт.";
            Colour = ConsoleColor.Yellow;
        }
    }
}
