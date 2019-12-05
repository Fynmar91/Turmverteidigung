using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Spiel
{
	class Spiellogik
	{
		static public Point RasterUebersetzung(Canvas spielbrett, Point punkt)
		{
			const int rastergroesse = 32;
			Point koordinate = new Point((int)(punkt.X / rastergroesse), (int)(punkt.Y / rastergroesse));

			return koordinate;
		}

		static public void PlatziereFeld(Canvas spielbrett, Point punkt)
		{
			const int rastergroesse = 32;
			Rectangle feld = new Rectangle();
			feld.Fill = Brushes.Beige;
			feld.Width = rastergroesse;
			feld.Height = rastergroesse;
			spielbrett.Children.Add(feld);
			Canvas.SetLeft(feld, punkt.X * rastergroesse);
			Canvas.SetTop(feld, punkt.Y * rastergroesse);
		}
	}
}
