using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dtp8_MUD_0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TBD: Lista istället för array
        Room[] labyrinth = new Room[100];
        int currentRoom;
        public MainWindow()
        {
            InitializeComponent();

            // Init Rooms here:
            //TODO: Ett menyrum vartifrån man kan ladda rumkonfigurationer och sparade spel eller starta nytt spel
            //TBD: Lägg rummen som initieras i separat fil.
            Room R;
            R = new Room(0, "Startrummet");
            R.SetStory("Du står i ett rum med rött tegel. Väggarna fladdrar i facklornas sken. Du ser en hög med tyg nere till vänster. ");
            R.SetImage("ingang-stangd.png");
            R.SetDirections(N: 1, E: Room.NoDoor, S: Room.NoDoor, W: Room.NoDoor);
            labyrinth[0] = R;

            R = new Room(1, "Korsvägen");
            R.SetStory("Du står i korsväg. Det går gångar i alla riktningar. ");
            R.SetImage("vagskal.png");
            R.SetDirections(N: 2, E: 3, S: 0, W: 4);
            labyrinth[1] = R;

            currentRoom = 0;
            DisplayCurrentRoom();
        }

        private void ApplicationKeyPress(object sender, KeyEventArgs e)
        {
            //TODO: Gör KeyPressDisplay till en debug-option vilken skall gå att stänga av från menyrum eller med tangenttryckning.
            string output = "Key pressed: ";
            output += e.Key.ToString();
            // output += ", ";
            // output += AppDomain.CurrentDomain.BaseDirectory;
            KeyPressDisplay.Text = output;
            
            //NYI: Länka majoriteten av tangentkommandon till DoCommand(e.Key) i rum eller DoCommand(e.Key) hos player. Globala behålls här.
            //TBD: Byt till switch-case träd för att fånga flera tangenter till samma metod.
            if(e.Key == Key.Escape)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else if(e.Key == Key.Up)
            {
                 currentRoom = labyrinth[currentRoom].GetNorth();
                DisplayCurrentRoom();
            }
        }
        private void DisplayCurrentRoom()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Room R = labyrinth[currentRoom];
            string bitmapFileName = $"{baseDir}/{R.imageFile}";
            if (File.Exists(bitmapFileName))
            {
                Uri uri = new Uri(bitmapFileName, UriKind.RelativeOrAbsolute);
                RoomImage.Source = BitmapFrame.Create(uri);
            }
            string text = $"Du befinner dig i {R.roomname}. ";
            text += R.story+" ";
            //TBD: Ändra till att visa möjliga dörrar genom att loopa genom rummets lista
            if (R.GetNorth() != Room.NoDoor) text += "Det finns en gång norrut. ";
            if (R.GetEast() != Room.NoDoor) text += "Det finns en gång österut. ";
            if (R.GetSouth() != Room.NoDoor) text += "Det finns en gång söderut. ";
            if (R.GetWest() != Room.NoDoor) text += "Det finns en gång västerut. ";
            RoomText.Text = text;

            /* Andra texter som man kan sätta:
             * KeyAlt1.Text = "upp      gå norrut"; 
             * KeyAlt2.Text = "vänster  gå västerut"; 
             * ... etc. ...
             */
        }
    }
}
