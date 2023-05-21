using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.Net.Http;

namespace writing_tools
{
    /// <summary>
    /// Interaction logic for PracticeWindow.xaml
    /// </summary>
    public partial class PracticeWindow : Window
    {
        // palette
        SolidColorBrush backgroundBrush = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        SolidColorBrush accentBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
        // SolidColorBrush accentBrush = (SolidColorBrush)SystemParameters.WindowGlassBrush;

        string currentCharacter;

        public PracticeWindow()
        {
            // initialize component
            InitializeComponent();

            // apply brushs
            Background = backgroundBrush;

            displayTextBlock.Foreground = accentBrush;

            // load text
            displayTextBlock.Inlines.Remove(displayTextBlock.Inlines.ElementAt(0));  // remove "LOADING"

            displayTextBlock.Text = FetchJoke();
            currentCharacter = displayTextBlock.Text.ElementAt(0).ToString();

            // get input chars
            TextInput += new TextCompositionEventHandler(Input_TextInput);
        }

        void Input_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == currentCharacter)
            {
                displayTextBlock.Text = displayTextBlock.Text.Substring(1);

                if (displayTextBlock.Text == "")
                {
                    displayTextBlock.Text = FetchJoke();
                }

                this.currentCharacter = displayTextBlock.Text.ElementAt(0).ToString();
            }

            while (currentCharacter == "\n" || currentCharacter == "\r\n")
            {
                displayTextBlock.Text = displayTextBlock.Text.Substring(1);
                currentCharacter = displayTextBlock.Text.ElementAt(0).ToString();
            }
        }

        // fetch jokes

        HttpClient client = new HttpClient();

        private string FetchJoke()
        {

            HttpResponseMessage response = client.GetAsync("https://v2.jokeapi.dev/joke/Any?lang=de&format=txt").Result;
            response.EnsureSuccessStatusCode();
            
            var joke = response.Content.ReadAsStringAsync().Result;

            return joke;
        }
    }
}
