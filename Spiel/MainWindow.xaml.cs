using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Spiel
{
	public partial class MainWindow : Window
	{
		const int rasterGroesse = 32;

		DispatcherTimer takt = new DispatcherTimer();
		Point zeiger = new Point(-1, -1);
		Point zeigerAlt = new Point(-1, -1);
		bool turmGewechselt = false;
		bool turmAbbruch = false;
		bool turmBauen = false;

		Spiellogik spielLogik;
		int turmAuswahl = 0;
		bool turmVorschau = false;

		public MainWindow()
		{
			InitializeComponent();
			takt.Interval = TimeSpan.FromSeconds(0.02);
			takt.Tick += Update;
			takt.Start();
		}

		void Update(object sender, EventArgs e)
		{
			if (spielLogik != null)
			{
				if (turmVorschau)
				{
					BauMenu();
				}

				spielLogik.AnzeigenErneuern();
				spielLogik.TuermeSchiessen(takt.Interval);
				spielLogik.Animieren(takt.Interval);
				spielLogik.Kollisionen();
				spielLogik.Aufraeumen();
				spielLogik.Generieren(takt.Interval);
			}
			else
			{
				TextBlock[] Anzeigen = new TextBlock[2];
				Anzeigen[0] = geld_anzeige;
				Anzeigen[1] = zeit_anzeige;

				spielLogik = new Spiellogik(spielbrett, Anzeigen);
				KarteBauen();

				zeiger = new Point(0, 0);
				zeigerAlt = new Point(0, 0);
				turmGewechselt = false;
				turmAbbruch = false;
				turmBauen = false;

				turmAuswahl = 0;
				turmVorschau = false;
			}
		}

		void BauMenu()
		{
			if (turmVorschau && !turmAbbruch && !turmBauen)
			{
				zeiger = spielLogik.RasterUebersetzung(Mouse.GetPosition(this));

				if (zeiger != zeigerAlt || turmGewechselt)
				{
					spielLogik.TurmVorschau(zeiger, turmAuswahl);
					zeigerAlt = zeiger;
					turmGewechselt = false;
				}
			}
			else if (turmVorschau && turmAbbruch)
			{
				spielLogik.TurmVorschau(new Point(-1, -1), turmAuswahl);
				turmVorschau = false;
				turmAbbruch = false;
				zeigerAlt = new Point(-1, -1);
			}
			else if (turmVorschau && turmBauen)
			{
				spielLogik.TurmBauen(zeiger, turmAuswahl);
				turmVorschau = false;
				turmBauen = false;
				zeigerAlt = new Point(-1, -1);
			}
		}

		void KarteBauen()
		{
			for (int i = 0; i < (int)(spielbrett.ActualWidth / rasterGroesse); i++)
			{
				for (int j = 0; j < (int)(spielbrett.ActualHeight / rasterGroesse); j++)
				{
					Rectangle box = new Rectangle();
					box.Width = rasterGroesse;
					box.Height = rasterGroesse;
					spielbrett.Children.Add(box);
					Canvas.SetLeft(box, i * rasterGroesse);
					Canvas.SetTop(box, j * rasterGroesse);
					box.Fill = Brushes.GreenYellow;

					if (i == 0 || i == (int)(spielbrett.ActualWidth / rasterGroesse) - 1 || j == 0 || j == (int)(spielbrett.ActualHeight / rasterGroesse) - 1)
					{
						box.Fill = Brushes.DimGray;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (i == 5 && j > 2 && j < 10)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (j == 10 && i > 4 && i < 9)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (i == 8 && j > 5 && j < 10)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (j == 5 && i > 7 && i < 15)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (i == 15 && j > 4 && j < 8)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (j == 8 && i > 10 && i < 16)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (i == 10 && j > 7 && j < 12)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
					else if (j == 11 && i > 10 && i < 24)
					{
						box.Fill = Brushes.SandyBrown;
						spielLogik.TurmBauen(new Point(i, j), turmAuswahl);
					}
				}
			}

			spielLogik.PlatziereGenerator(new Point(5 * rasterGroesse + rasterGroesse / 2, 2 * rasterGroesse + rasterGroesse / 2), new Point(0, 1), 4, 5, 100, 50);
			spielLogik.PlatziereGenerator(new Point(5 * rasterGroesse + rasterGroesse / 2 + 5, 2 * rasterGroesse + rasterGroesse / 2 - 15), new Point(0, 1), 2, 2, 40, 50);
			spielLogik.PlatziereGenerator(new Point(5 * rasterGroesse + rasterGroesse / 2 - 5, 2 * rasterGroesse + rasterGroesse / 2 - 15), new Point(0, 1), 2, 2, 40, 50);
			spielLogik.PlatzierePunkt(new Point(5 * rasterGroesse + rasterGroesse / 2, 10 * rasterGroesse + rasterGroesse), new Point(1, 0));
			spielLogik.PlatzierePunkt(new Point(8 * rasterGroesse + rasterGroesse, 10 * rasterGroesse + rasterGroesse / 2), new Point(0, -1));
			spielLogik.PlatzierePunkt(new Point(8 * rasterGroesse + rasterGroesse / 2, 4 * rasterGroesse + rasterGroesse), new Point(1, 0));
			spielLogik.PlatzierePunkt(new Point(15 * rasterGroesse + rasterGroesse, 5 * rasterGroesse + rasterGroesse / 2), new Point(0, 1));
			spielLogik.PlatzierePunkt(new Point(15 * rasterGroesse + rasterGroesse / 2, 8 * rasterGroesse + rasterGroesse), new Point(-1, 0));
			spielLogik.PlatzierePunkt(new Point(9 * rasterGroesse + rasterGroesse, 8 * rasterGroesse + rasterGroesse / 2), new Point(0, 1));
			spielLogik.PlatzierePunkt(new Point(10 * rasterGroesse + rasterGroesse / 2, 11 * rasterGroesse + rasterGroesse), new Point(1, 0));
			spielLogik.PlatzierePunkt(new Point(23 * rasterGroesse + rasterGroesse, 11 * rasterGroesse + rasterGroesse / 2), new Point(0, 0));
		}

		private void Spielbrett_MouseDown(object sender, MouseButtonEventArgs e)
		{
			switch (e.ChangedButton)
			{
				case MouseButton.Left:
					if (turmVorschau)
					{
						turmBauen = true;
					}
					break;
				case MouseButton.Middle:
					break;
				case MouseButton.Right:
					spielLogik.Zerstoere(spielLogik.RasterUebersetzung(Mouse.GetPosition(this)));
					break;
				case MouseButton.XButton1:
					break;
				case MouseButton.XButton2:
					break;
				default:
					break;
			}
		}

		private void TurmMG_ausw_Click(object sender, RoutedEventArgs e) { turmAuswahl = 1; turmVorschau = true; turmGewechselt = true; }

		private void TurmSniper_ausw_Click(object sender, RoutedEventArgs e) { turmAuswahl = 2; turmVorschau = true; turmGewechselt = true; }

		private void TurmFlammen_ausw_Click(object sender, RoutedEventArgs e) { turmAuswahl = 3; turmVorschau = true; turmGewechselt = true; }

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.D1:
					turmAuswahl = 1;
					turmVorschau = true;
					turmGewechselt = true;
					break;
				case Key.D2:
					turmAuswahl = 2;
					turmVorschau = true;
					turmGewechselt = true;
					break;
				case Key.D3:
					turmAuswahl = 3;
					turmVorschau = true;
					turmGewechselt = true;
					break;
				case Key.D4:
					break;
				case Key.D5:
					break;
				case Key.D6:
					break;
				case Key.D7:
					break;
				case Key.D8:
					break;
				case Key.D9:
					break;
				case Key.Escape:
					if (turmVorschau)
					{
						turmAbbruch = true;
					}
					break;
				default:
					break;
			}
		}
	}
}
