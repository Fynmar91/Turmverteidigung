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
		const int turmPreisFlammen = 200;

		const int turmReichweiteMG = 4;
		const int turmReichweiteSniper = 6;
		const int turmReichweiteFlammen = 3;

		double MyLaufzeit { get; set; }
		int MyGeld { get; set; }

		Canvas MySpielbrett { get; set; }
		Gebaeude[,] MyTuerme { get; set; }
		Gebaeude TempTurm { get; set; }
		TextBlock[] MyAnzeigen { get; set; }
		List<Gegner> MyGegner { get; set; }
		List<Projektil> MyProjektile { get; set; }
		List<GegnerGenerator> MyGegnerGeneratoren { get; set; }
		List<GegnerPunkt> MyGegnerPunkte { get; set; }

		public Spiellogik(Canvas spielbrett, TextBlock[] Anzeigen)
		{
			MyGeld = 200;

			MySpielbrett = spielbrett;
			MyTuerme = new Gebaeude[(int)(MySpielbrett.ActualWidth / rasterGroesse), (int)(MySpielbrett.ActualHeight / rasterGroesse)];
			MyGegner = new List<Gegner>();
			MyProjektile = new List<Projektil>();
			MyGegnerGeneratoren = new List<GegnerGenerator>();
			MyGegnerPunkte = new List<GegnerPunkt>();
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
						(TempTurm as Turm).ZerstoereTurm();
					}

					switch (turmTyp)
					{
						case 1:
							if (MyGeld >= turmPreisMG)
							{
								TempTurm = new MGTurm(MySpielbrett, punkt, rasterGroesse + rasterGroesse * turmReichweiteMG);
								(TempTurm as Turm).ZeigeTurm();
							}
							break;
						case 2:
							if (MyGeld >= turmPreisSniper)
							{
								TempTurm = new SniperTurm(MySpielbrett, punkt, rasterGroesse + rasterGroesse * turmReichweiteSniper);
								(TempTurm as Turm).ZeigeTurm();
							}
							break;
						case 3:
							if (MyGeld >= turmPreisFlammen)
							{
								TempTurm = new FlammenTurm(MySpielbrett, punkt, rasterGroesse + rasterGroesse * turmReichweiteFlammen);
								(TempTurm as Turm).ZeigeTurm();
							}
							break;
						default:
							break;
					}
				}
			}
			else if (TempTurm != null)
			{
				(TempTurm as Turm).ZerstoereTurm();
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
						(TempTurm as Turm).ZerstoereTurm();
					}

					switch (turmTyp)
					{
						case 0:
							MyTuerme[(int)punkt.X, (int)punkt.Y] = new Strasse(MySpielbrett, rasterGroesse, punkt);
							break;
						case 1:
							if (MyGeld >= turmPreisMG)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = new MGTurm(MySpielbrett, punkt, rasterGroesse + rasterGroesse * turmReichweiteMG);
								(MyTuerme[(int)punkt.X, (int)punkt.Y] as Turm).BaueTurm();
								MyGeld -= turmPreisMG;
							}
							break;
						case 2:
							if (MyGeld >= turmPreisSniper)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = new SniperTurm(MySpielbrett, punkt, rasterGroesse + rasterGroesse * turmReichweiteSniper);
								(MyTuerme[(int)punkt.X, (int)punkt.Y] as Turm).BaueTurm();
								MyGeld -= turmPreisSniper;
							}
							break;
						case 3:
							if (MyGeld >= turmPreisFlammen)
							{
								MyTuerme[(int)punkt.X, (int)punkt.Y] = new FlammenTurm(MySpielbrett, punkt, rasterGroesse + rasterGroesse * turmReichweiteFlammen);
								(MyTuerme[(int)punkt.X, (int)punkt.Y] as Turm).BaueTurm();
								MyGeld -= turmPreisSniper;
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
				if (MyTuerme[(int)punkt.X, (int)punkt.Y] != null && MyTuerme[(int)punkt.X, (int)punkt.Y] is Turm)
				{
					(MyTuerme[(int)punkt.X, (int)punkt.Y] as Turm).ZerstoereTurm();
					MyTuerme[(int)punkt.X, (int)punkt.Y] = null;
				}
			}
		}

		public void PlatziereGenerator(Point punkt, Point richtung, double generationsRate, int groesse, int leben, int geschwindigkeit)
		{
			MyGegnerGeneratoren.Add(new GegnerGenerator(MySpielbrett, punkt, MyGegner, richtung, generationsRate, groesse, leben, geschwindigkeit));
		}

		public void PlatzierePunkt(Point punkt, Point richtung)
		{
			MyGegnerPunkte.Add(new GegnerPunkt(MySpielbrett, punkt, richtung));
		}

		public void TuermeSchiessen(TimeSpan intervall)
		{
			foreach (var turm in MyTuerme.OfType<Turm>())
			{
				(turm as Turm).Schiessen(MyGegner, MyProjektile, intervall.TotalSeconds);
			}
		}
		/// <summary>
		/// //////////////////////////////////////////////////
		/// </summary>
		public void AnzeigenErneuern()
		{
			MyAnzeigen[0].Text = MyGeld.ToString();
			MyAnzeigen[1].Text = MyLaufzeit.ToString("#.0");
		}
		/// <summary>
		/// //////////////////////////////////////////////////
		/// </summary>
		public void Animieren(TimeSpan intervall)
		{
			foreach (var item in MyGegner)
			{
				item.Bewegen(intervall.TotalSeconds);
			}

			foreach (var item in MyProjektile)
			{
				item.Bewegen(intervall.TotalSeconds);
			}
		}
		/// <summary>
		/// //////////////////////////////////////////////////
		/// </summary>
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

			List<Gegner> GegnerAbfall = new List<Gegner>();

			foreach (var punkt in MyGegnerPunkte)
			{
				foreach (var gegner in MyGegner)
				{
					if (punkt.Umleiten(gegner) && punkt == MyGegnerPunkte.Last())
					{
						GegnerAbfall.Add(gegner);
					}
				}
			}

			foreach (var item in GegnerAbfall)
			{
				MyGegner.Remove(item);
				MySpielbrett.Children.Remove(item.MyForm);
			}
		}
		/// <summary>
		/// //////////////////////////////////////////////////
		/// </summary>
		public void Aufraeumen()
		{
			List<Gegner> GegnerAbfall = new List<Gegner>();

			foreach (var item in MyGegner)
			{
				if (!item.IstAmLeben())
				{
					GegnerAbfall.Add(item);
					MyGeld += 10;
				}
			}

			foreach (var item in GegnerAbfall)
			{
				MyGegner.Remove(item);
				MySpielbrett.Children.Remove(item.MyForm);
			}

			List<Projektil> ProjektilAbfall = new List<Projektil>();

			foreach (var item in MyProjektile)
			{
				if (!item.MyZiel.IstAmLeben())
				{
					ProjektilAbfall.Add(item);
				}
			}

			foreach (var item in ProjektilAbfall)
			{
				MyProjektile.Remove(item);
				MySpielbrett.Children.Remove(item.MyForm);
				MySpielbrett.Children.Remove(item.MyKollisionsbox);
			}
		}
		/// <summary>
		/// //////////////////////////////////////////////////
		/// </summary>
		public void Generieren(TimeSpan intervall)
		{
			MyLaufzeit += intervall.TotalSeconds;

			if (MyLaufzeit > 10)
			{
				foreach (var item in MyGegnerGeneratoren)
				{
					item.Schneller();
				}

				MyLaufzeit = 0;
			}

			foreach (var item in MyGegnerGeneratoren)
			{
				item.Generiere(intervall.TotalSeconds);
			}
		}
	}

	public class GegnerGenerator
	{
		public Point MyPosition { get; set; }
		Canvas MySpielbrett { get; set; }
		List<Gegner> MyGegner { get; set; }
		double MyAbklingzeit { get; set; }
		double MyGenerationsRate { get; set; }
		Point MyRichtung { get; set; }
		int MyGroesse { get; set; }
		int MyLeben { get; set; }
		int MyGeschwindigkeit { get; set; }

		public GegnerGenerator(Canvas spielbrett, Point punkt, List<Gegner> gegner, Point richtung, double generationsRate, int groesse, int leben, int geschwindigkeit)
		{
			MySpielbrett = spielbrett;
			MyPosition = punkt;
			MyGegner = gegner;
			MyGenerationsRate = generationsRate;
			MyRichtung = richtung;
			MyGroesse = groesse;
			MyLeben = leben;
			MyGeschwindigkeit = geschwindigkeit;
		}

		public void Generiere(double intervall)
		{
			MyAbklingzeit -= intervall;

			if (MyAbklingzeit <= 0)
			{
				MyAbklingzeit = MyGenerationsRate;

				MyGegner.Add(new Gegner(MySpielbrett, MyPosition, MyGroesse, MyRichtung, MyLeben, MyGeschwindigkeit));
			}
		}

		public void Schneller()
		{
			MyGenerationsRate = MyGenerationsRate * 0.7;
		}
	}

	public class GegnerPunkt
	{
		Point MyPosition { get; set; }
		Ellipse MyZielbereich { get; set; }
		Point MyRichtung { get; set; }

		public GegnerPunkt(Canvas spielbrett, Point bauplatz, Point richtung)
		{
			MyPosition = bauplatz;
			MyRichtung = richtung;

			MyZielbereich = new Ellipse();
			//MyZielbereich.Fill = new SolidColorBrush(Color.FromArgb(60, 0, 255, 255));
			MyZielbereich.Height = 32;
			MyZielbereich.Width = 32;
			Canvas.SetLeft(MyZielbereich, bauplatz.X - 32 / 2);
			Canvas.SetTop(MyZielbereich, bauplatz.Y - 32 / 2);

			spielbrett.Children.Add(MyZielbereich);
		}

		public bool Umleiten(Gegner gegner)
		{
			if (MyZielbereich.RenderedGeometry.FillContains(new Point(gegner.MyPosition.X - (MyPosition.X - MyZielbereich.ActualWidth / 2),
																		gegner.MyPosition.Y - (MyPosition.Y - MyZielbereich.ActualHeight / 2))))
			{
				gegner.MyRichtung = MyRichtung;
				return true;
			}

			return false;
		}
	}
}


