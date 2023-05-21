using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// see https://github.com/rvknth043/Global-Low-Level-Key-Board-And-Mouse-Hook
using writing_tools.GlobalLowLevelHook;

namespace writing_tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        // global low level hooks
        KeyboardHook keyboardHook = new KeyboardHook();
        MouseHook mouseHook = new MouseHook();

        // layout
        struct Key
        {
            public string Id { get; set; }
            public int X {get; set;}
            public int Y { get; set; }
            public int W { get; set; }
            public int H { get; set; }
            public Rectangle Rectangle { get; set; }

            public Key(string new_id, int new_x, int new_y, int new_w, int new_h, Rectangle new_rectangle)
            {
                Id = new_id;
                X = new_x;
                Y = new_y;
                W = new_w;
                H = new_h;
                Rectangle = new_rectangle;
            }
        }

        // keyboard layout
        List<Key> keyboardLayout = new List<Key>() 
        {
            new Key("ESCAPE",       10,               10,     50, 50,         new Rectangle()),
            new Key("F1",           70  + 60,         10,     50, 50,         new Rectangle()),
            new Key("F2",           130 + 60,         10,     50, 50,         new Rectangle()),
            new Key("F3",           190 + 60,         10,     50, 50,         new Rectangle()),
            new Key("F4",           250 + 60,         10,     50, 50,         new Rectangle()),
            new Key("F5",           310 + 60 + 12,    10,     50, 50,         new Rectangle()),
            new Key("F6",           370 + 60 + 12,    10,     50, 50,         new Rectangle()),
            new Key("F7",           430 + 60 + 12,    10,     50, 50,         new Rectangle()),
            new Key("F8",           490 + 60 + 12,    10,     50, 50,         new Rectangle()),
            new Key("F9",           550 + 60 + 25,    10,     50, 50,         new Rectangle()),
            new Key("F10",          610 + 60 + 25,    10,     50, 50,         new Rectangle()),
            new Key("F11",          670 + 60 + 25,    10,     50, 50,         new Rectangle()),
            new Key("F12",          730 + 60 + 25,    10,     50, 50,         new Rectangle()),

            new Key("OEM_5",        10,     70,     50, 50,         new Rectangle()),
            new Key("KEY_1",        70,     70,     50, 50,         new Rectangle()),
            new Key("KEY_2",        130,    70,     50, 50,         new Rectangle()),
            new Key("KEY_3",        190,    70,     50, 50,         new Rectangle()),
            new Key("KEY_4",        250,    70,     50, 50,         new Rectangle()),
            new Key("KEY_5",        310,    70,     50, 50,         new Rectangle()),
            new Key("KEY_6",        370,    70,     50, 50,         new Rectangle()),
            new Key("KEY_7",        430,    70,     50, 50,         new Rectangle()),
            new Key("KEY_8",        490,    70,     50, 50,         new Rectangle()),
            new Key("KEY_9",        550,    70,     50, 50,         new Rectangle()),
            new Key("KEY_0",        610,    70,     50, 50,         new Rectangle()),
            new Key("OEM_4",        670,    70,     50, 50,         new Rectangle()),
            new Key("OEM_6",        730,    70,     50, 50,         new Rectangle()),
            new Key("BACK",         790,    70,     75, 50,         new Rectangle()),

            new Key("TAB",          10,          130,    75, 50,    new Rectangle()),
            new Key("KEY_Q",        70  + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_W",        130 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_E",        190 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_R",        250 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_T",        310 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_Z",        370 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_U",        430 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_I",        490 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_O",        550 + 25,    130,    50, 50,    new Rectangle()),
            new Key("KEY_P",        610 + 25,    130,    50, 50,    new Rectangle()),
            new Key("OEM_1",        670 + 25,    130,    50, 50,    new Rectangle()),
            new Key("OEM_PLUS",     730 + 25,    130,    50, 50,    new Rectangle()),
            new Key("RETURN",       790 + 25,    130,    50, 50,    new Rectangle()),

            new Key("CAPITAL",      10,          190,    100, 50,   new Rectangle()),
            new Key("KEY_A",        70  + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_S",        130 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_D",        190 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_F",        250 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_G",        310 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_H",        370 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_J",        430 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_K",        490 + 50,    190,    50, 50,    new Rectangle()),
            new Key("KEY_L",        550 + 50,    190,    50, 50,    new Rectangle()),
            new Key("OEM_3",        610 + 50,    190,    50, 50,    new Rectangle()),
            new Key("OEM_7",        670 + 50,    190,    50, 50,    new Rectangle()),
            new Key("OEM_2",        730 + 50,    190,    50, 50,    new Rectangle()),
            // new Key("RETURN",    790 + 50,    190,    75, 50,    new Rectangle()),

            new Key("LSHIFT",       10,          250,    75, 50,    new Rectangle()),
            new Key("OEM_102",      70  + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_Y",        130 + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_X",        190 + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_C",        250 + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_V",        310 + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_B",        370 + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_N",        430 + 25,    250,    50, 50,    new Rectangle()),
            new Key("KEY_M",        490 + 25,    250,    50, 50,    new Rectangle()),
            new Key("OEM_COMMA",    550 + 25,    250,    50, 50,    new Rectangle()),
            new Key("OEM_PERIOD",   610 + 25,    250,    50, 50,    new Rectangle()),
            new Key("OEM_MINUS",    670 + 25,    250,    50, 50,    new Rectangle()),
            new Key("RSHIFT",       730 + 25,    250,    110, 50,   new Rectangle()),
            
            new Key("LCONTROL",     10,     310,    75, 50,         new Rectangle()),
            new Key("LMENU",        210,    310,    75, 50,         new Rectangle()),
            new Key("SPACE",        295,    310,    250, 50,        new Rectangle()),
            new Key("RMENU",        555,    310,    75, 50,         new Rectangle()),
            new Key("RCONTROL",     790,    310,    75, 50,         new Rectangle())
        };

        // mouse layout
        List<Key> mouseLayout = new List<Key>() 
        {
            new Key("LMB",          1000,   100,    75, 100,         new Rectangle()),
            new Key("MMB",          1085,   125,    25, 50,          new Rectangle()),
            new Key("RMB",          1120,   100,    75, 100,         new Rectangle())
        };

        // brushes - palette
        SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        SolidColorBrush accentBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        // SolidColorBrush accentBrush = (SolidColorBrush)SystemParameters.WindowGlassBrush;

        // list for storing currently pressed keys
        List<string> activeKeys = new List<string>();

        public MainWindow()
        {
            // initialize component
            InitializeComponent();

            // apply brushs
            Background = backgroundBrush;
            activeKeysLabel.Foreground = accentBrush;
            cursorPositionLabel.Foreground = accentBrush;

            practiceButton.Background = backgroundBrush;
            practiceButton.Foreground = accentBrush;
            practiceButton.BorderBrush = accentBrush;

            // generate layout
            List<Key> layout = keyboardLayout.Concat(mouseLayout).ToList();

            foreach (Key key in layout)
            {
                key.Rectangle.Name = key.Id + "_Button";

                key.Rectangle.Width = key.W;
                key.Rectangle.Height = key.H;

                key.Rectangle.RadiusX = 5;
                key.Rectangle.RadiusY = 5;

                key.Rectangle.Fill = backgroundBrush;
                key.Rectangle.Stroke = accentBrush;

                keysButtonsCanvas.Children.Add(key.Rectangle);

                Canvas.SetTop(key.Rectangle, key.Y);
                Canvas.SetLeft(key.Rectangle, key.X);
            }

            // low level keyboard hook
            keyboardHook.KeyDown += new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyDown);
            keyboardHook.KeyUp += new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyUp);

            keyboardHook.Install();

            // low level mouse hook
            mouseHook.MouseMove += new MouseHook.MouseHookCallback(MouseHook_MouseMove);
            mouseHook.LeftButtonDown += new MouseHook.MouseHookCallback(MouseHook_LeftButtonDown);
            mouseHook.LeftButtonUp += new MouseHook.MouseHookCallback(MouseHook_LeftButtonUp);
            mouseHook.RightButtonDown += new MouseHook.MouseHookCallback(MouseHook_RightButtonDown);
            mouseHook.RightButtonUp += new MouseHook.MouseHookCallback(MouseHook_RightButtonUp);
            mouseHook.MiddleButtonDown += new MouseHook.MouseHookCallback(MouseHook_MiddleButtonDown);
            mouseHook.MiddleButtonUp += new MouseHook.MouseHookCallback(MouseHook_MiddleButtonUp);

            mouseHook.Install();

            // for uninstalling the hooks
            Application.Current.Exit += new ExitEventHandler(OnApplicationExit);
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            keyboardHook.KeyDown -= new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyDown);
            keyboardHook.KeyUp -= new KeyboardHook.KeyboardHookCallback(KeyboardHook_KeyUp);

            keyboardHook.Uninstall();

            mouseHook.LeftButtonDown -= new MouseHook.MouseHookCallback(MouseHook_LeftButtonDown);
            mouseHook.LeftButtonUp -= new MouseHook.MouseHookCallback(MouseHook_LeftButtonUp);
            mouseHook.RightButtonDown -= new MouseHook.MouseHookCallback(MouseHook_RightButtonDown);
            mouseHook.RightButtonUp -= new MouseHook.MouseHookCallback(MouseHook_RightButtonUp);
            mouseHook.MiddleButtonDown -= new MouseHook.MouseHookCallback(MouseHook_MiddleButtonDown);
            mouseHook.MiddleButtonUp -= new MouseHook.MouseHookCallback(MouseHook_MiddleButtonUp);

            mouseHook.Uninstall();
        }

        void KeyboardHook_KeyDown(KeyboardHook.VKeys key)
        {
            var already_down = activeKeys.FirstOrDefault(stringToCheck => stringToCheck.Contains(key.ToString().Replace("_", "").Replace("KEY", "")));
            if (already_down != null)
            {
                return;
            }

            activeKeys.Add(key.ToString().Replace("_", "").Replace("KEY", ""));
            activeKeysLabel.Content = string.Join(" + ", activeKeys);
            
            var rectangle = keyboardLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains(key.ToString())).Rectangle;
            if (rectangle != null) {
                rectangle.Fill = accentBrush;
            }
        }

        // event handlers
        void KeyboardHook_KeyUp(KeyboardHook.VKeys key)
        {
            var already_up = activeKeys.FirstOrDefault(stringToCheck => stringToCheck.Contains(key.ToString().Replace("_", "").Replace("KEY", "")));
            if (already_up == null)
            {
                return;
            }

            activeKeys.Remove(key.ToString().Replace("_", "").Replace("KEY", ""));
            activeKeysLabel.Content = string.Join(" + ", activeKeys);

            var rectangle = keyboardLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains(key.ToString())).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = backgroundBrush;
            }
        }

        void MouseHook_MouseMove(MouseHook.MSLLHOOKSTRUCT mouseStruct)
        {
            cursorPositionLabel.Content = "x: " + mouseStruct.pt.x.ToString() + ", y: " + mouseStruct.pt.y.ToString();
        }

        void MouseHook_LeftButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct) 
        {
            var rectangle = mouseLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains("LMB")).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = accentBrush;
            }
        }
        
        void MouseHook_LeftButtonUp(MouseHook.MSLLHOOKSTRUCT mouseStruct) 
        {
            var rectangle = mouseLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains("LMB")).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = backgroundBrush;
            }
        }
        
        void MouseHook_RightButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct) 
        {
            var rectangle = mouseLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains("RMB")).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = accentBrush;
            }
        }
        
        void MouseHook_RightButtonUp(MouseHook.MSLLHOOKSTRUCT mouseStruct) 
        {
            var rectangle = mouseLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains("RMB")).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = backgroundBrush;
            }
        }
        
        void MouseHook_MiddleButtonDown(MouseHook.MSLLHOOKSTRUCT mouseStruct) 
        {
            var rectangle = mouseLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains("MMB")).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = accentBrush;
            }
        }
        
        void MouseHook_MiddleButtonUp(MouseHook.MSLLHOOKSTRUCT mouseStruct) 
        {
            var rectangle = mouseLayout.FirstOrDefault(keyToCheck => keyToCheck.Id.Contains("MMB")).Rectangle;
            if (rectangle != null)
            {
                rectangle.Fill = backgroundBrush;
            }
        }

        private void practiceButton_Click(object sender, RoutedEventArgs e)
        {
            PracticeWindow practiceWindow = new PracticeWindow();
            practiceWindow.Show();
        }
    }
}
