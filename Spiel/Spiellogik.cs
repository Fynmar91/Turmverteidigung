using SpielObjekte;
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
		const int rasterGroesse = 32;
		const int turmPreisMG = 100;
		const int turmPreisSniper = 200;
		const double intervall = 0.02;


		int MyGeld { get; set; }

		Canvas MySpielbrett { get; set; }
		Turm[,] MyTuerme { get; set; }
		Turm TempTurm { get; set; }
		TextBlock[] MyAnzeigen { get; set; }
		List<Gegner> MyGegner { get; set; }
		List<Projektil> MyProjektile { get; set; }
		List<GegnerGenerator> MyGegnerGeneratoren { get; set; }

		public Spiellogik(Canvas spielbrett, TextBlock[] Anzeigen)
		{
			MyGeld = 5000;

			MySpielbrett = spielbrett;
			MyTuerme = new Turm[(int)(MySpielbrett.ActualWidth / rasterGroesse), (int)(MySpielbrett.ActualHeight / rasterGroesse)];
			MyGegner = new List<Gegner>();
			MyProjektile = new List<Projektil>();
			MyGegnerGeneratoren = new List<GegnerGenerator>();
			this.MyAnzeigen = Anzeigen;
		}

		public Point RasterUebersetzung(Point punkt)
		{
			return new Point((int)((punkt.X - 10) / rasterGroesse), (int)((punkt.Y - 10) / rasterGroesse));
		}

		public void TurmVorschau(Point punkt, int turmTyp)
		{
			if (punkt.X >= 0 && punkt.X < MySpielbrett.ActualWidth / rasterGroesse &&
				punkt.Y >= 0 && punkt.Y < MySpielbrett.ActualHeight / rasterGroesse)
			{
				if (MyTuerme[(int)punkt.X, (int)punkt.Y] == null)
				{
					if (TempTurm != null)
					{
						TempTurm.ZerstoereTurm();
					}

					switch (turmTyp)
					{
						case 1:
							if (MyGeld >= turmPreisMG)
							{
								TempTurm = new MGTurm(MySpielbrett, rasterGroesse, punkt, rasterGroesse + rasterGroesse * 4);
								TempTurm.ZeigeTurm();
							}
							break;
						case 2:
							if (MyGeld >= turmPreisSniper)
							{
								TempTurm = new SniperTurm(MySpielbrett, rasterGroesse, punkt, rasterGroesse + rasterGroesse * 6);
								TempTurm.ZeigeTurm();
							}
							break;
						default:
							break;
					}
				}
			}
			else if (TempTurm != null)
			{
				TempTurm.ZerstoereTurm();
			}

		}

		public void TurmBauen(Point punkt, int turmTyp)
		{
			if (punkt.X >= 0 && punkt.X < MySpielbrett.ActualWidth / rasterGroesse &&
				punkt.Y >= 0 && punkt.Y < MySpielbrett.ActualHeight / rasterGroesse)
			{
				if (MyTuerme[(int)punkt.X, (int)punkt.Y] == null)
				{
					if (TempTurm != null)
					{
						TempTurm.ZerstoereTurm();
					}

					switch (turmTyp)
					{
						case 1:
							if (MyGeld >= turmPreisMG)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = new MGTurm(MySpielbrett, rasterGroesse, punkt, rasterGroesse + rasterGroesse * 4);
								MyTuerme[(int)punkt.X, (int)punkt.Y].BaueTurm();
								MyGeld -= turmPreisMG;
								AnzeigenErneuern();
							}
							break;
						case 2:
							if (MyGeld >= turmPreisSniper)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = new SniperTurm(MySpielbrett, rasterGroesse, punkt, rasterGroesse + rasterGroesse * 6);
								MyTuerme[(int)punkt.X, (int)punkt.Y].BaueTurm();
								MyGeld -= turmPreisSniper;
								AnzeigenErneuern();
							}
							break;
						default:
							break;
					}
				}
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

		public void PlatziereGenerator(Point punkt)
		{
			MyGegnerGeneratoren.Add(new GegnerGenerator(MySpielbrett, punkt, MyGegner));
		}

		public void TuermeSchiessen()
		{
			foreach (var turm in MyTuerme)
			{
				if (turm != null)
				{
					turm.Nachladen(intervall);

					foreach (var gegner in MyGegner)
					{
						if (turm.ZielErfassen(gegner))
						{
							Projektil projektil = turm.Schiessen(gegner);

							if (projektil != null)
							{
								MyProjektile.Add(projektil);
							}

							break;
						}
					}
				}
			}
		}

		public void Animieren()
		{
			foreach (var item in MyGegner)
			{
				item.Bewegen(intervall);
			}

			foreach (var item in MyProjektile)
			{
				item.Bewegen(intervall);
			}
		}

		public void Kollisionen()
		{
			List<Projektil> ProjektilAbfall = new List<Projektil>();

			foreach (var projektil in MyProjektile)
			{
				foreach (var gegner in MyGegner)
				{
					if (projektil.Getroffen(gegner))
					{
						ProjektilAbfall.Add(projektil);
					}
				}
			}

			foreach (var item in ProjektilAbfall)
			{
				MyProjektile.Remove(item);
				MySpielbrett.Children.Remove(item.MyForm);
				MySpielbrett.Children.Remove(item.MyKollisionsbox);
			}
		}

		public void Aufraeumen()
		{
			List<Gegner> GegnerAbfall = new List<Gegner>();
			List<Projektil> ProjektilAbfall = new List<Projektil>();

			foreach (var item in MyGegner)
			{
				if (!item.IstAmLeben())
				{
					GegnerAbfall.Add(item);
					MyGeld += 100;
				}
			}

			foreach (var item in MyProjektile)
			{
				if (!item.MyZiel.IstAmLeben())
				{
					ProjektilAbfall.Add(item);
				}
			}

			foreach (var item in GegnerAbfall)
			{
				MyGegner.Remove(item);
				MySpielbrett.Children.Remove(item.MyForm);
			}

			foreach (var item in ProjektilAbfall)
			{
				MyProjektile.Remove(item);
				MySpielbrett.Children.Remove(item.MyForm);
				MySpielbrett.Children.Remove(item.MyKollisionsbox);
			}
		}

		public void Generieren()
		{
			foreach (var item in MyGegnerGeneratoren)
			{
				item.Generiere(intervall);
			}
		}
	}

	public class GegnerGenerator
	{
		public Point MyPosition { get; set; }
		Canvas MySpielbrett { get; set; }
		List<Gegner> MyGegner { get; set; }
		double MyGenerationsAbklingzeit { get; set; }
		double MyGenerationsRate { get; set; }

		public GegnerGenerator(Canvas spielbrett, Point punkt, List<Gegner> gegner)
		{
			MySpielbrett = spielbrett;
			MyPosition = punkt;
			MyGegner = gegner;
			MyGenerationsRate = 2;
		}

		public void Generiere(double intervall)
		{
			MyGenerationsAbklingzeit -= intervall;

			if (MyGenerationsAbklingzeit <= 0)
			{
				MyGenerationsAbklingzeit = MyGenerationsRate;

				MyGegner.Add(new Gegner(MySpielbrett, MyPosition));
			}
		}
	}
}


