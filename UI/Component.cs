using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGTestC.UI
{
    public class Component
    {
        string Text;
        public bool Selectable = false;
        public bool Selected = false;

        public Component(string _text, bool _selectable = false)
        {
            Text = _text;
            Selectable = _selectable;
        }

        public virtual void Display() 
        {
            if (!Selected)
                Console.WriteLine(Text);
            else
                Console.WriteLine("["+Text+"]");
        }
        public delegate void KeyAction();
        public KeyAction keyAction;
    }
}
