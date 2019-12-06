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
		const double invervall = 0.02;

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
			takt.Interval = TimeSpan.FromSeconds(invervall);
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

				spielLogik.TuermeSchiessen();
				spielLogik.Animieren();
				spielLogik.Kollisionen();
				spielLogik.Aufraeumen();
				spielLogik.Generieren();
			}
			else
			{
				TextBlock[] Anzeigen = new TextBlock[1];

				Anzeigen[0] = geld_anzeige;

				spielLogik = new Spiellogik(spielbrett, Anzeigen);
				GitterBauen();

				takt = new DispatcherTimer();
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

		void GitterBauen()
		{
			for (int i = 0; i < (int)(spielbrett.ActualWidth / rasterGroesse); i++)
			{
				for (int j = 0; j < (int)(spielbrett.ActualHeight / rasterGroesse); j++)
				{
					Rectangle box = new Rectangle();
					box.Fill = Brushes.Gray;
					box.Width = rasterGroesse - 2;
					box.Height = rasterGroesse - 2;
					spielbrett.Children.Add(box);
					Canvas.SetLeft(box, i * rasterGroesse + 1);
					Canvas.SetTop(box, j * rasterGroesse + 1);
				}
			}
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
					spielLogik.PlatziereGenerator(Mouse.GetPosition(this));
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
