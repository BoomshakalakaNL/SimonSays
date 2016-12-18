using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using SimonSays.Classes;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimonSays
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Led ledBlauw;
        private Led ledOrange;
        private Led ledRood;
        private Led ledGroen;

        private Pushbutton btnBlauw;
        private Pushbutton btnOrange;
        private Pushbutton btnRood;
        private Pushbutton btnGroen;

        private Buzzer buzzer;

        private DispatcherTimer timer;
        private Stopwatch stopwatch;
        private Random randomColor;

        private List<int> PlayerSequence = new List<int>();
        private List<int> ComputerSequence = new List<int>();

        private bool Play = false;
        private bool PlayerIdle = true;
        private int tijdLimiet = 1500; //in miliseconden

        public MainPage()
        {
            this.InitializeComponent();
            InitSimon();
            Run_Simon();
        }

        /// <summary>
        /// Een functie die alle onderdelen in het project initialiseerd
        /// </summary>
        private void InitSimon()
        {
            this.ledBlauw = new Led(19, 1, GpioPinDriveMode.Output);
            this.ledOrange = new Led(20, 2, GpioPinDriveMode.Output);
            this.ledRood = new Led(13, 3, GpioPinDriveMode.Output);
            this.ledGroen = new Led(5, 4, GpioPinDriveMode.Output);

            this.btnBlauw = new Pushbutton(26, 1, GpioPinDriveMode.InputPullUp);
            this.btnOrange = new Pushbutton(21, 2, GpioPinDriveMode.InputPullUp);
            this.btnRood = new Pushbutton(16, 3, GpioPinDriveMode.InputPullUp);
            this.btnGroen = new Pushbutton(6, 4, GpioPinDriveMode.InputPullUp);

            this.buzzer = new Buzzer(true, 4, 0, GpioPinDriveMode.Output);

            this.randomColor = new Random();
            this.stopwatch = new Stopwatch();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(10);
            this.timer.Tick += TimerTick;
            this.timer.Start();
        }

        /// <summary>
        /// Iedere keer dat de timer tikt, controleer of de speler actief is, of de speler kan mag 
        /// spelen en of de speler een knop indrukt.
        /// </summary>
        private void TimerTick(object sender, object e)
        {
            if (!PlayerIdle)
            {
                if (Play)
                {
                    if (btnBlauw.IsPushbuttonPressed())
                    {

                        PlayerSequence.Add(1);
                        ledBlauw.knipperLed(1, 250, 0, 100);
                        stopwatch.Restart();
                    }
                    else if (btnOrange.IsPushbuttonPressed())
                    {
                        PlayerSequence.Add(2);
                        ledOrange.knipperLed(1, 250, 0, 100);
                        stopwatch.Restart();
                    }
                    else if (btnRood.IsPushbuttonPressed())
                    {
                        PlayerSequence.Add(3);
                        ledRood.knipperLed(1, 250, 0, 100);
                        stopwatch.Restart();
                    }
                    else if (btnGroen.IsPushbuttonPressed())
                    {
                        PlayerSequence.Add(4);
                        ledGroen.knipperLed(1, 250, 0, 100);
                        stopwatch.Restart();
                    }
                    Controleren(ComputerSequence, PlayerSequence);
                }
                else
                {
                    Run_Simon();
                }
            }
            else
            {
                if (btnBlauw.IsPushbuttonPressed() || btnGroen.IsPushbuttonPressed() || btnOrange.IsPushbuttonPressed() || btnRood.IsPushbuttonPressed())
                {
                    PlayerIdle = false;
                }
                else
                {
                    ToonIdleSignaal(500);
                }
            }
        }

        /// <summary>
        /// Functie die een Signaal laat zien wanneer de speler Idle is.
        /// </summary>
        /// <param name="delay">Tijd tussen aan en uit gaan.</param>
        private void ToonIdleSignaal(int delay)
        {
            this.ledBlauw.zetLedAan();
            this.ledGroen.zetLedAan();
            this.ledOrange.zetLedAan();
            this.ledRood.zetLedAan();
            Task.Delay(delay).Wait();
            this.ledRood.zetLedUit();
            this.ledOrange.zetLedUit();
            this.ledGroen.zetLedUit();
            this.ledBlauw.zetLedUit();
            Task.Delay(delay).Wait();
        }

        /// <summary>
        /// Functie die een Signaal laat zien de een nieuwe ronde start.
        /// </summary>
        private void ToonStartSignaal()
        {
            for (int i = 0; i < 2; i++)
            {
                this.ledBlauw.knipperLed(1, 25, 25, 0);
                this.ledOrange.knipperLed(1, 25, 25, 0);
                this.ledRood.knipperLed(1, 25, 25, 0);
                this.ledGroen.knipperLed(1, 25, 25, 0);
                this.ledRood.knipperLed(1, 25, 25, 0);
                this.ledOrange.knipperLed(1, 25, 25, 0);
                this.ledBlauw.knipperLed(1, 25, 25, 0);
            }
            Task.Delay(100).Wait();
        }

        /// <summary>
        /// Functie die een Signaal laat zien dat de speler een ronde gewonnen heeft.
        /// </summary>
        private void RondeWinSignaal()
        {
            this.buzzer.piepBuzzer(2, 40, 10, 0);
            for (int i = 10; i < 70; i += 3)
            {
                this.ledGroen.knipperLed(1, i, 0, i);
            }
        }

        /// <summary>
        /// Functie die een Signaal laat zien dat de speler een ronde verloren heeft.
        /// </summary>
        private void RondeVerliesSignaal()
        {
            this.buzzer.piepBuzzer(1, 250, 0, 0);
            for (int i = 10; i < 100; i += 3)
            {
                this.ledRood.knipperLed(1, i, 0, i);
            }
        }

        /// <summary>
        /// Functie die een random kleur toevoegd aan een lijst die leeg is of al bestaat, waarbij
        /// 1 blauw is, 2 orange, 3 rood en 4 groen.
        /// </summary>
        private void CreateColorSequence()
        {
            ComputerSequence.Add(randomColor.Next(1, 5));
        }

        /// <summary>
        /// Functie die de kleuren van de een sequence laat zien, waarbij 1 blauw is, 2 orange, 3 
        /// rood en 4 groen.
        /// </summary>
        /// <param name="Sequence">Welke lijst moet worden getoont.</param>
        private void ShowColorSequence(List<int> Sequence)
        {
            for (int i = 0; i < Sequence.Count(); i++)
            {
                if (Sequence[i] == 1)
                {
                    ledBlauw.knipperLed(1, 250, 0, 250);
                }
                else if (Sequence[i] == 2)
                {
                    ledOrange.knipperLed(1, 250, 0, 250);
                }
                else if (Sequence[i] == 3)
                {
                    ledRood.knipperLed(1, 250, 0, 250);
                }
                else if (Sequence[i] == 4)
                {
                    ledGroen.knipperLed(1, 250, 0, 250);
                }
            }
            Task.Delay(250).Wait();
        }

        /// <summary>
        /// Functie die controleerd of twee lijsten met elkaar overeenkomen en controleerd of de 
        /// speler nog tijd heeft om te spelen.
        /// </summary>
        /// <param name="CorrectSequence">Welke lijst is correct.</param>
        /// <param name="ToBeCheckedSequence">Welke lijst moet worden gecontoleerd.</param>
        private void Controleren(List<int> CorrectSequence, List<int> ToBeCheckedSequence)
        {
            if (ToBeCheckedSequence.Count() <= CorrectSequence.Count())
            {
                for (int i = 0; i < ToBeCheckedSequence.Count(); i++)
                {
                    if (CorrectSequence[i] != ToBeCheckedSequence[i])
                    {
                        GameOver(CorrectSequence, ToBeCheckedSequence);
                    }
                }
                if (!speelTijd(tijdLimiet) && ToBeCheckedSequence.Count() == CorrectSequence.Count())
                {
                    RondeWinSignaal();
                    Play = false;
                    ToBeCheckedSequence.Clear();
                }
                else if (!speelTijd(tijdLimiet) && ToBeCheckedSequence.Count() < CorrectSequence.Count())
                {
                    GameOver(CorrectSequence, ToBeCheckedSequence);
                }
            }
            else
            {
                GameOver(CorrectSequence, ToBeCheckedSequence);
            }
        }

        /// <summary>
        /// Functie die ervoor zorgt dat het spel opnieuw kan starten.
        /// </summary>
        /// <param name="CorrectSequence">Lijst die correct is</param>
        /// <param name="ToBeCheckedSequence">Lijst die incorrect is.</param>
        private void GameOver(List<int> CorrectSequence, List<int> IncorrectSequence)
        {
            Play = false;
            PlayerIdle = true;
            RondeVerliesSignaal();
            CorrectSequence.Clear();
            IncorrectSequence.Clear();
            Run_Simon();
        }

        /// <summary>
        /// Functie die ervoor zorgt dat het spel blijft lopen.
        /// </summary>
        private void Run_Simon()
        {
            if (!PlayerIdle)
            {
                CreateColorSequence();
                ToonStartSignaal();
                ShowColorSequence(ComputerSequence);
                PlayerSequence.Clear();
                Play = true;
                stopwatch.Restart();
            }
        }

        /// <summary>
        /// Functie die controleerd of er nog speeltijd is.
        /// </summary>
        /// <param name="tijdlimiet">Hoeveel speeltijd is er.</param>
        /// <returns>True wanneer speler nog tijd heeft en False wanneer dit niet zo is.</returns>
        private bool speelTijd(int tijdlimiet)
        {
            return (stopwatch.ElapsedMilliseconds < tijdlimiet);
        }
    }
}
