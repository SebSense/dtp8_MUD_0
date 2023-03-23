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
    /// 
    public partial class MainWindow : Window
    {
        //TBD: Gör detta till en klass som lagrar globala variablar.
        //TODO: Bryt ut till ett spara fil format och gör det möjligt att ladda olika moduler och sparade spel från filer.
        static List<Room> currentMap = new();
        static int[] findRoom = new int[1000000];
        static Room currentRoom = new Room(-1, "placeholder");
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
            R.SetDirections(N: 500500, E: Room.NoDoor, S: Room.NoDoor, W: Room.NoDoor);
            currentMap.Add(R);
            findRoom[R.number] = currentMap.Count - 1;

            R = new Room(500500, "Korsvägen");
            R.SetStory("Du står i korsväg. Det går gångar i alla riktningar. ");
            R.SetImage("vagskal.png");
            R.SetDirections(N: 500501, E: 501500, S: 0, W: 499500);
            currentMap.Add(R);
            findRoom[R.number] = currentMap.Count - 1;

            R = new Room(499500, "Baren");
            R.SetStory("Du står i en bar. Inte tid för detta nu.. ");
            R.SetImage("vagskal.png");
            R.SetDirections(N: Room.NoDoor, E: 500500, S: Room.NoDoor, W: Room.NoDoor);
            currentMap.Add(R);
            findRoom[R.number] = currentMap.Count - 1;

            R = new Room(500501, "Korsvägen");
            R.SetStory("Du står i en annan korsväg. Det går gångar i alla riktningar. ");
            R.SetImage("vagskal.png");
            R.SetDirections(N: 500501, E: 499501, S: 500500, W: 501501);
            currentMap.Add(R);
            findRoom[R.number] = currentMap.Count - 1;

            R = new Room(501500, "Gränden");
            R.SetStory("Du står i en gränd.");
            R.SetImage("vagskal.png");
            R.SetDirections(N: Room.NoDoor, E: Room.NoDoor, S: Room.NoDoor, W: 500500);
            currentMap.Add(R);
            findRoom[R.number] = currentMap.Count - 1;


            ChangeRoom(0);
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
            if (e.Key == Key.Escape)
            {
              //  System.Windows.Application.Current.Shutdown();
            }
            else if (e.Key == Key.Up || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Down)
            {
                ChangeRoom(currentRoom.Move(e.Key));
                DisplayCurrentRoom();
            }
        }
        private void DisplayCurrentRoom()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Room R = currentRoom;
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
        static void ChangeRoom(int room)
        {
            try { currentRoom = currentMap[findRoom[room]]; }
            catch (Exception e) { Console.WriteLine("Change room error: " + e.Message); }
        }
    }
}
