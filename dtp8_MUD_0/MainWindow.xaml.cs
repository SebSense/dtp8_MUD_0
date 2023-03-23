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
        static List<Door> doors = new();
        static int[] findByID = new int[1000000];
        static Room currentRoom = new Room(-1, "placeholder");
        static Player player = new();
        public MainWindow()
        {
            InitializeComponent();
            
            // Init Rooms here:
            //TODO: Ett menyrum vartifrån man kan ladda rumkonfigurationer och sparade spel eller starta nytt spel
            //TBD: Lägg rummen som initieras i separat fil.
            Room R;
            Door D;
            R = new Room(0, "Startrummet");
            R.SetStory("Du står i ett rum med rött tegel. Väggarna fladdrar i facklornas sken. Du ser en hög med tyg nere till vänster. ");
            R.SetImage("ingang-stangd.png");
            R.SetDirections(N: 5, E: Room.NoDoor, S: Room.NoDoor, W: Room.NoDoor);
            currentMap.Add(R);
            findByID[R.number] = currentMap.Count - 1;

            R = new Room(500500, "Korsvägen");
            R.SetStory("Du står i korsväg. Det går gångar i alla riktningar. ");
            R.SetImage("vagskal.png");
            R.SetDirections(N: 500505, E: 505500, S: 5, W: 495500);
            currentMap.Add(R);
            findByID[R.number] = currentMap.Count - 1;

            D = new Door(5, currentMap[findByID[0]], currentMap[findByID[500500]]);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

            R = new Room(490500, "Bron");
            R.SetStory("Du står vid avsatsen till en bro... ");
            R.SetImage("bro.png");
            R.SetDirections(N: 490505, E: 495500, S: Room.NoDoor, W: Room.NoDoor);
            currentMap.Add(R);
            findByID[R.number] = currentMap.Count - 1;

            D = new Door(495500, currentMap[findByID[490500]], currentMap[findByID[500500]]);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

            R = new Room(500510, "Skattkammaren");
            R.SetStory("Du står i en skattkammare. ");
            R.SetImage("kista.png");
            R.SetDirections(N: Room.NoDoor, E: Room.NoDoor, S: 500505, W: 495510);
            R.keys.Add("röd nyckel");
            currentMap.Add(R);
            findByID[R.number] = currentMap.Count - 1;

            D = new Door(500505, currentMap[findByID[500510]], currentMap[findByID[500500]], "röd nyckel", false);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

            R = new Room(510500, "Gränden");
            R.SetStory("Du står i en gränd.");
            R.SetImage("galler.png");
            R.keys.Add("silvernyckel");
            R.SetDirections(N: Room.NoDoor, E: Room.NoDoor, S: Room.NoDoor, W: 505500);
            currentMap.Add(R);
            findByID[R.number] = currentMap.Count - 1;

            D = new Door(505500, currentMap[findByID[510500]], currentMap[findByID[500500]]);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

            R = new Room(490510, "Trevägskorset");
            R.SetStory("Du står i ett trevägskors.");
            R.SetImage("vagskal.png");
            R.SetDirections(N: 490515, E: 495510, S: 490505, W: Room.NoDoor);

            currentMap.Add(R);
            findByID[R.number] = currentMap.Count - 1;

            D = new Door(490515, currentMap[findByID[490510]], currentMap[findByID[490520]], "guldnyckel", false);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

            D = new Door(495510, currentMap[findByID[490510]], currentMap[findByID[500510]], "silvernyckel", false);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

            D = new Door(490505, currentMap[findByID[490500]], currentMap[findByID[490510]]);
            doors.Add(D);
            findByID[D.id] = doors.Count - 1;

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
                System.Windows.Application.Current.Shutdown();
            }
            else if (e.Key == Key.Up || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Down)
            {
                DisplayCurrentRoom();
                try
                {
                    int doorID = currentRoom.Move(e.Key);
                    Door D = doors[findByID[doorID]];
                    int newRoomID = D.MoveFrom(currentRoom);
                    if (newRoomID == -1)
                    {
                        RoomText.Text = "Dörren är låst. ";
                    }
                    else 
                    { 
                        ChangeRoom(newRoomID);
                        DisplayCurrentRoom();
                    }
                }
                catch (Exception ex) { RoomText.Text = "Du gick in i en vägg. "; }
            }
            else if (e.Key == Key.NumPad0 || e.Key == Key.D0)
            {
                ChangeRoom(0);
            }
            else if (e.Key == Key.L)
            {
                RoomText.Text = "Du letar efter nycklar...\n";
                if (currentRoom.keys.Any())
                {
                    while (currentRoom.keys.Any())
                    {
                        RoomText.Text += "Du hittar en " + currentRoom.keys.Last() + "! Du fäster nyckeln i din nyckelknippa.\n";
                        player.keys.Add(currentRoom.keys.Last());
                        currentRoom.keys.Remove(currentRoom.keys.Last());
                    }
                }
                else RoomText.Text += "Du hittar ingen nyckel!";
               // Console.ReadKey(true);
               // DisplayCurrentRoom();
            }
            else if (e.Key == Key.N)
            {
                RoomText.Text = "";
                string[] riktningar = { "norr", "öster", "söder", "väster" };
                for(int i = 0; i < 4; i++) 
                {
                    try
                    {
                        int doorID = currentRoom.adjacent[i];
                        Door D = doors[findByID[doorID]];
                        RoomText.Text += "Du försöker låsa upp dörren till " + riktningar[i] + ". " + D.Unlock(player) + "\n";
                    }
                    catch { }
                }
            }
            else if (e.Key == Key.R)
            {
                DisplayCurrentRoom();
            }
            else if (e.Key == Key.S)
            {
                RoomText.Text = "Du är " + player.name + ". Du är på äventyr i labyrinten.";
                if (player.keys.Any())
                {
                    RoomText.Text += "\nI din nyckelknippa har du en " + player.keys[0];
                    for (int i = 1; i < player.keys.Count - 1; i++)
                        RoomText.Text += ", en " + player.keys[i];
                    if (player.keys.Count > 1) RoomText.Text += " och en " + player.keys.Last();
                    RoomText.Text += ".";
                }
                else RoomText.Text += "\nDu har inga nycklar i din nyckelknippa.";
            }
        }
        private void DisplayCurrentRoom()
        {
            Room R = currentRoom;
            string bitmapFileName = $"./MJU23v-bilder/{R.imageFile}";
            if (File.Exists(bitmapFileName))
            {
                Uri uri = new Uri(bitmapFileName, UriKind.RelativeOrAbsolute);
                RoomImage.Source = BitmapFrame.Create(uri);
            }
            string text = R.story+" ";
            //TBD: Ändra till att visa möjliga dörrar genom att loopa genom rummets lista
            text += "\nDu ser utgångar åt ";
            if (R.GetNorth() != Room.NoDoor) text += "norr ";
            if (R.GetEast() != Room.NoDoor) text += "öster ";
            if (R.GetSouth() != Room.NoDoor) text += "söder ";
            if (R.GetWest() != Room.NoDoor) text += "väster ";
            RoomText.Text = text;
            TitleText.Text = R.roomname;
            /* Andra texter som man kan sätta:
             * KeyAlt1.Text = "upp      gå norrut"; 
             * KeyAlt2.Text = "vänster  gå västerut"; 
             * ... etc. ...
             */
        }
        static void ChangeRoom(int room)
        {
            currentRoom = currentMap[findByID[room]];
        }
    }
}
