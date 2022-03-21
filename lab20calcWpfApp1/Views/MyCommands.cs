using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace lab20calcWpfApp1
{
    class MyCommands
    {
        public static RoutedUICommand Exit { get; set; }
        public static RoutedUICommand TopDark { get; set; }
        public static RoutedUICommand TopLight { get; set; }

        static MyCommands()
        {
            InputGestureCollection inputsExit = new InputGestureCollection();
            inputsExit.Add(new KeyGesture(Key.Q, ModifierKeys.Control, "Ctrl+Q"));
            Exit = new RoutedUICommand("_Exit", "Exit", typeof(MyCommands), inputsExit);

            TopLight = new RoutedUICommand("_LightTheme", "TopLight", typeof(MyCommands));

            TopDark = new RoutedUICommand("_DarkTheme", "TopDark", typeof(MyCommands));
        }
    }
}
