using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SpielObjekte;

namespace Spiel
{
	class Spiellogik
	{
		const int rasterGroesse = 32;

		TextBlock[] Anzeigen;

		Canvas MySpielbrett { get; set; }
		Turm[,] MyTuerme { get; set; }
		List<Gegner> MyGegner { get; set; }
		int MyGeld { get; set; }

		public Spiellogik(Canvas spielbrett, TextBlock[] Anzeigen)
		{
			MySpielbrett = spielbrett;
			MyTuerme = new Turm[(int)(MySpielbrett.ActualWidth / rasterGroesse), (int)(MySpielbrett.ActualHeight / rasterGroesse)];
			MyGegner = new List<Gegner>();
			MyGeld = 500;
			this.Anzeigen = Anzeigen;
		}

		public Point RasterUebersetzung(Point punkt)
		{
			return new Point((int)((punkt.X - 10) / rasterGroesse), (int)((punkt.Y - 10) / rasterGroesse));
		}

		public void PlatziereTurm(Point punkt, int turmTyp)
		{
			if (MyTuerme[(int)punkt.X, (int)punkt.Y] == null)
			{
				switch (turmTyp)
				{
					case 1:
						if (MyGeld >= 100)
						{
							MyTuerme[(int)punkt.X, (int)punkt.Y] = new MGTurm(MySpielbrett, rasterGroesse, punkt);
							MyTuerme[(int)punkt.X, (int)punkt.Y].BaueTurm();
							MyGeld -= 100;
							AnzeigenErneuern();
						}
						break;
					case 2:
						if (MyGeld >= 200)
						{
							MyTuerme[(int)punkt.X, (int)punkt.Y] = new SniperTurm(MySpielbrett, rasterGroesse, punkt);
							MyTuerme[(int)punkt.X, (int)punkt.Y].BaueTurm();
							MyGeld -= 200;
							AnzeigenErneuern();
						}
						break;
					default:
						break;
				}
			}
		}

		public void Zerstoere(Point punkt)
		{
			if (MyTuerme[(int)punkt.X, (int)punkt.Y] != null)
			{
				(MyTuerme[(int)punkt.X, (int)punkt.Y] as Turm).ZerstoereTurm();
				MyTuerme[(int)punkt.X, (int)punkt.Y] = null;
			}
		}

		void AnzeigenErneuern()
		{
			Anzeigen[0].Text = MyGeld.ToString();
		}

		public void PlatziereGegner(Point punkt)
		{
			MyGegner.Add(new Gegner(MySpielbrett, punkt));
			MyGegner.Last().ErzeugeGegner();
		}
	}
}
