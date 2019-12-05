using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Gebaeude;

namespace Spiel
{
	class Spiellogik
	{
		const int rastergroesse = 32;

		Turm[,] Tuerme;

		Canvas MySpielbrett { get; set; }

		public Spiellogik(Canvas spielbrett)
		{
			MySpielbrett = spielbrett;
			Tuerme = new Turm[(int)(MySpielbrett.ActualWidth / rastergroesse), (int)(MySpielbrett.ActualHeight / rastergroesse)];
		}

		public Point RasterUebersetzung(Point punkt)
		{
			Point koordinate = new Point((int)((punkt.X - 10) / rastergroesse), (int)((punkt.Y - 10) / rastergroesse));

			return koordinate;
		}

		public void Platziere(Point punkt)
		{
			if (Tuerme[(int)punkt.X, (int)punkt.Y] == null)
			{
				Tuerme[(int)punkt.X, (int)punkt.Y] = new Turm(MySpielbrett, rastergroesse, punkt);
			}
		}

		public void Zerstoere(Point punkt)
		{
			if (Tuerme[(int)punkt.X, (int)punkt.Y] != null)
			{
				(Tuerme[(int)punkt.X, (int)punkt.Y] as Turm).ZerstoereTurm();
				Tuerme[(int)punkt.X, (int)punkt.Y] = null;
			}
		}
	}
}
