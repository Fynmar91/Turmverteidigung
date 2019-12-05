using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Spiel
{
	class Spiellogik
	{
		static public Point Rasteruebersetzung(Canvas spielbrett, Point punkt)
		{
			double breite = spielbrett.ActualWidth / 16;
			double hoehe = spielbrett.ActualHeight / 16;
			Point koordinate = new Point((int)(punkt.X / 16), (int)(punkt.Y / 16));

			return koordinate;
		}
	}
}
