using System;
using System.Collections.Generic;

namespace RPGTestC.UI
{
    public class Menu
    {
        protected string DefaultControls = "W,S - Выбрать элемент меню, E - Активация элемента меню, Q - Выход";

        static int index = 0;
        // Basically - index0 is where the first set of buttons begins (if there are more than 2
        // index1 is where the second set of buttons (or, in retrospect, of any selectable components) begins
        // But I have no idea where would you need a menu with buttons separated by some text or whatever
        protected int index0 = -1;
        protected int index1 = -1;

        List<Component> Components = new List<Component> { };

        public Menu() { }
        public Menu(int _idx0, int _idx1 = -1)
        {
            index0 = _idx0;
            index1 = _idx1;
            index = index0;
            Components[index].Selected = true;
        }
        public virtual void ShowMenu()
        {
            foreach (Component comp in Components)
            {
                comp.Display();
            }

            Console.WriteLine(DefaultControls);

            MenuControls();
        }
        public virtual void MenuControls()
        {
            ConsoleKey key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.E:
                    Console.Clear();
                    Components[index].keyAction?.Invoke();
                    goto default;

                case ConsoleKey.Q:
                    Console.Clear();
                    break;

                case ConsoleKey.S:
                    Console.Clear();
                    Components[index].Selected = false;
                    if (index < Components.Count - 1)
                    {
                        // if you haven't reached the end of the list and the next component can be selected then increment index
                        if (Components[index + 1].Selectable) index++;
                        // if the next component can't be selected, but there is a second set of selectables, then move on to them
                        else if (index1 != -1) index = index1;
                    }
                    goto default;

                case ConsoleKey.W:
                    Console.Clear();
                    Components[index].Selected = false;
                    // if you haven't reached the beginning of the list and the previous component is selectable then decrement index
                    if (index > 0 && Components[index - 1].Selectable) index--;
                    // if the previous component can't be selected and there is (and there is) a first set of selectables then get back to them
                    if (!Components[index - 1].Selectable && index > index0) index = index0;
                    goto default;

                default:
                    Components[index].Selected = true;
                    ShowMenu();
                    break;
            }
        }

        protected void AddComponent(Component component) => Components.Add(component);
    }
}
