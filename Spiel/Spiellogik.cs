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
		const int turmPreisMG = 100;
		const int turmPreisSniper = 200;


		int MyGeld { get; set; }

		Canvas MySpielbrett { get; set; }
		Turm[,] MyTuerme { get; set; }
		List<Gegner> MyGegner { get; set; }
		TextBlock[] MyAnzeigen { get; set; }

		public Spiellogik(Canvas spielbrett, TextBlock[] Anzeigen)
		{
			MyGeld = 5000;

			MySpielbrett = spielbrett;
			MyTuerme = new Turm[(int)(MySpielbrett.ActualWidth / rasterGroesse), (int)(MySpielbrett.ActualHeight / rasterGroesse)];
			MyGegner = new List<Gegner>();
			this.MyAnzeigen = Anzeigen;
		}

		public Point RasterUebersetzung(Point punkt)
		{
			return new Point((int)((punkt.X - 10) / rasterGroesse), (int)((punkt.Y - 10) / rasterGroesse));
		}

		public void TurmPlatzieren(Point punkt, int turmTyp)
		{
			if (punkt.X >= 0 && punkt.X < MySpielbrett.ActualWidth / rasterGroesse && 
				punkt.Y >= 0 && punkt.Y < MySpielbrett.ActualHeight / rasterGroesse)
			{
				if (MyTuerme[(int)punkt.X, (int)punkt.Y] == null)
				{
					switch (turmTyp)
					{
						case 1:
							if (MyGeld >= turmPreisMG)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = 
									new MGTurm(MySpielbrett, rasterGroesse, punkt, rasterGroesse + rasterGroesse * 3 );
								MyTuerme[(int)punkt.X, (int)punkt.Y].BaueTurm();
								AnzeigenErneuern();
							}
							break;
						case 2:
							if (MyGeld >= turmPreisSniper)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = 
									new SniperTurm(MySpielbrett, rasterGroesse, punkt, rasterGroesse + rasterGroesse * 4 );
								MyTuerme[(int)punkt.X, (int)punkt.Y].BaueTurm();
								AnzeigenErneuern();
							}
							break;
						default:
							break;
					}
				}
			}
		}

		public void Baue(int turmTyp)
		{
			switch (turmTyp)
			{
				case 1:
					MyGeld -= turmPreisMG;
					break;
				case 2:
					MyGeld -= turmPreisSniper;
					break;
				default:
					break;
			}
		}

		public void Zerstoere(Point punkt)
		{
			if (punkt.X >= 0 && punkt.X < MySpielbrett.ActualWidth / rasterGroesse &&
				punkt.Y >= 0 && punkt.Y < MySpielbrett.ActualHeight / rasterGroesse)
			{
				if (MyTuerme[(int)punkt.X, (int)punkt.Y] != null)
				{
					(MyTuerme[(int)punkt.X, (int)punkt.Y] as Turm).ZerstoereTurm();
					MyTuerme[(int)punkt.X, (int)punkt.Y] = null;
				}
			}
		}

		void AnzeigenErneuern()
		{
			MyAnzeigen[0].Text = MyGeld.ToString();
		}

		public void PlatziereGegner(Point punkt)
		{
			MyGegner.Add(new Gegner(MySpielbrett, punkt));
			MyGegner.Last().ErzeugeGegner();
		}
	}
}
