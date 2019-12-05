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
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const int rastergroesse = 32;

		DispatcherTimer takt = new DispatcherTimer();
		Spiellogik spiellogik;

		public MainWindow()
		{
			InitializeComponent();
			takt.Interval = TimeSpan.FromSeconds(0.02);
			takt.Tick += Update;
			takt.Start();
		}

		void Update(object sender, EventArgs e)
		{
			if (spiellogik != null)
			{
				
			}
			else
			{
				spiellogik = new Spiellogik(spielbrett);
				GitterBauen();
			}
		}

		void GitterBauen()
		{
			for (int i = 0; i < (int)(spielbrett.ActualWidth / rastergroesse); i++)
			{
				for (int j = 0; j < (int)(spielbrett.ActualHeight / rastergroesse); j++)
				{
					Rectangle box = new Rectangle();
					box.Fill = Brushes.Gray;
					box.Width = rastergroesse - 2;
					box.Height = rastergroesse - 2;
					spielbrett.Children.Add(box);
					Canvas.SetLeft(box, i * rastergroesse + 1);
					Canvas.SetTop(box, j * rastergroesse + 1);
				}
			}
		}

		private void Spielbrett_MouseDown(object sender, MouseButtonEventArgs e)
		{
			switch (e.ChangedButton)
			{
				case MouseButton.Left:
					spiellogik.Platziere(spiellogik.RasterUebersetzung(Mouse.GetPosition(this)));
					break;
				case MouseButton.Middle:
					break;
				case MouseButton.Right:
					spiellogik.Zerstoere(spiellogik.RasterUebersetzung(Mouse.GetPosition(this)));
					break;
				case MouseButton.XButton1:
					break;
				case MouseButton.XButton2:
					break;
				default:
					break;
			}
		}

		private void Spielbrett_MouseUp(object sender, MouseButtonEventArgs e)
		{
		}
	}
}
