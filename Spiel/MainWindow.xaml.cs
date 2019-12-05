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
		DispatcherTimer takt = new DispatcherTimer();

		public MainWindow()
		{
			InitializeComponent();
			takt.Interval = TimeSpan.FromSeconds(0.02);
			takt.Tick += Update;
		}

		void Update(object sender, EventArgs e)
		{

		}

		private void Spielbrett_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				Point koordinate = Spiellogik.RasterUebersetzung(spielbrett, Mouse.GetPosition(this));
				rasterX.Text = koordinate.X.ToString();
				rasterY.Text = koordinate.Y.ToString();
				Spiellogik.PlatziereFeld(spielbrett, koordinate);
			}
		}

		private void Spielbrett_MouseUp(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{

			}
		}
	}
}
