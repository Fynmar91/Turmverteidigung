using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpielObjekte
{
	public abstract class Gebaeude
	{
		protected const int rasterGroesse = 32;
	}

	public class Strasse : Gebaeude
	{
		protected Polygon MyBauform { get; set; }

		public Strasse(Canvas spielbrett, int rasterGroesse, Point bauplatz)
		{
			MyBauform = new Polygon();
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, rasterGroesse - 2));
			MyBauform.Points.Add(new Point(2, rasterGroesse - 2));
			Canvas.SetLeft(MyBauform, bauplatz.X * rasterGroesse);
			Canvas.SetTop(MyBauform, bauplatz.Y * rasterGroesse);
			spielbrett.Children.Add(MyBauform);
		}
	}

	public class Turm : Gebaeude
	{
		protected Polygon MyBauform { get; set; }
		protected Ellipse MyZielbereich { get; set; }
		Canvas MySpielbrett { get; set; }
		Point MyBauplatz { get; set; }
		protected Point MyBauKoordinate { get; set; }
		List<Gegner> MyZiele { get; set; }
		int MyReichweite { get; set; }
		double MyAbklingzeit { get; set; }
		protected double MyKadenz { get; set; }
		protected double MySchaden { get; set; }
		protected int MyProjektilGeschwindigkeit { get; set; }
		protected int MyFeuerstoss { get; set; }
		protected Brush MyGeschossFarbe { get; set; }



		public Turm(Canvas spielbrett, Point bauplatz, int reichweite)
		{
			MySpielbrett = spielbrett;
			MyBauplatz = bauplatz;
			MyReichweite = reichweite;
			MyBauKoordinate = new Point(MyBauplatz.X * rasterGroesse + rasterGroesse / 2, MyBauplatz.Y * rasterGroesse + rasterGroesse / 2);

			TurmForm();
		}

		void TurmForm()
		{
			MyBauform = new Polygon();
			MyZielbereich = new Ellipse();
			MyZielbereich.Fill = new SolidColorBrush(Color.FromArgb(40, 0, 255, 255));
			MyZielbereich.Height = MyReichweite;
			MyZielbereich.Width = MyReichweite;
			Canvas.SetLeft(MyZielbereich, MyBauKoordinate.X - MyReichweite / 2);
			Canvas.SetTop(MyZielbereich, MyBauKoordinate.Y - MyReichweite / 2);
			Canvas.SetLeft(MyBauform, MyBauKoordinate.X - rasterGroesse / 2);
			Canvas.SetTop(MyBauform, MyBauKoordinate.Y - rasterGroesse / 2);
		}

		public void ZeigeTurm()
		{
			MySpielbrett.Children.Add(MyBauform);
			MySpielbrett.Children.Add(MyZielbereich);
		}

		public void BaueTurm()
		{
			MySpielbrett.Children.Add(MyBauform);
			MySpielbrett.Children.Add(MyZielbereich);
			MyZielbereich.Fill.Opacity = 0;
		}

		public void ZerstoereTurm()
		{
			MySpielbrett.Children.Remove(MyBauform);
			MySpielbrett.Children.Remove(MyZielbereich);
		}

		void ZielErfassen(List<Gegner> MyGegner)
		{
			MyZiele = new List<Gegner>();

			foreach (var gegner in MyGegner)
			{
				if (MyZielbereich.RenderedGeometry.FillContains(new Point(gegner.MyPosition.X - (MyBauKoordinate.X - MyZielbereich.ActualWidth / 2),
																		gegner.MyPosition.Y - (MyBauKoordinate.Y - MyZielbereich.ActualHeight / 2))) && gegner.IstAmLeben())
				{
					MyZiele.Add(gegner);
				}
			}
		}

		public void Schiessen(List<Gegner> MyGegner, List<Projektil> MyProjektile, double intervall)
		{
			Nachladen(intervall);
			ZielErfassen(MyGegner);


			if (MyZiele != null && MyAbklingzeit <= 0)
			{
				MyAbklingzeit = MyKadenz;

				for (int i = 0; i < MyZiele.Count(); i++)
				{
					MyProjektile.Add(new Projektil(MySpielbrett, MyBauKoordinate, MyZiele[i], MySchaden, MyProjektilGeschwindigkeit, MyGeschossFarbe));

					if (i >= MyFeuerstoss - 1) { break; }
				}
			}
		}

		void Nachladen(double intervall)
		{
			MyAbklingzeit -= intervall;
		}
	}

	public class MGTurm : Turm
	{
		public MGTurm(Canvas spielbrett, Point bauplatz, int reichweite)
						: base(spielbrett, bauplatz, reichweite)
		{
			MyKadenz = 0.1;
			MySchaden = 4;
			MyProjektilGeschwindigkeit = 600;
			MyFeuerstoss = 1;
			MyGeschossFarbe = Brushes.Black;
			
			MyBauform.Fill = Brushes.SlateGray;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, rasterGroesse - 2));
			MyBauform.Points.Add(new Point(2, rasterGroesse - 2));

		}
	}

	public class SniperTurm : Turm
	{
		public SniperTurm(Canvas spielbrett, Point bauplatz, int reichweite)
							: base(spielbrett, bauplatz, reichweite)
		{
			MyKadenz = 0.5;
			MySchaden = 20;
			MyProjektilGeschwindigkeit = 600;
			MyFeuerstoss = 1;
			MyGeschossFarbe = Brushes.Black;

			MyBauform.Fill = Brushes.DarkSlateGray;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, rasterGroesse - 2));
			MyBauform.Points.Add(new Point(2, rasterGroesse - 2));
		}
	}

	public class FlammenTurm : Turm
	{
		public FlammenTurm(Canvas spielbrett, Point bauplatz, int reichweite)
							: base(spielbrett, bauplatz, reichweite)
		{
			MyKadenz = 0.1;
			MySchaden = 1;
			MyProjektilGeschwindigkeit = 400;
			MyFeuerstoss = 5;
			MyGeschossFarbe = Brushes.Red;

			MyBauform.Fill = Brushes.DarkRed;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, rasterGroesse - 2));
			MyBauform.Points.Add(new Point(2, rasterGroesse - 2));
		}
	}
}
