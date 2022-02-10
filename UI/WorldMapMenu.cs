using RPGTestC.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTestC.UI
{
    class MapComponent : Component
    {
        public MapComponent(string _text, bool _selectable = false) : base(_text, _selectable)
        {
        }

        public override void Display()
        {
            base.Display();
        }
    }
    public class WorldMapMenu : Menu
    {
        MapComponent Map = new MapComponent();
        public WorldMapMenu()
        {
            DefaultControls = "W,A,S,D - Перемещение по карте, Enter - Взаимодействие с объектами";
        }
        public override void MenuControls()
        {
            base.MenuControls();
        }
        public override void ShowMenu()
        {
            base.ShowMenu();
        }
    }
}
