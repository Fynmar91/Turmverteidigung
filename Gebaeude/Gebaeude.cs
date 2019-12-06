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
		Point MyBauKoordinate { get; set; }
		Gegner MyZiel { get; set; }
		double MyAbklingzeit { get; set; }
		protected double MyKadenz { get; set; }
		protected double MySchaden { get; set; }
		protected int MyGeschwindigkeit { get; set; }


		public Turm(Canvas spielbrett, int rasterGroesse, Point bauplatz, int reichweite)
		{
			MySpielbrett = spielbrett;
			MyBauplatz = bauplatz;
			MyBauKoordinate = new Point(bauplatz.X * rasterGroesse + rasterGroesse / 2, bauplatz.Y * rasterGroesse  + rasterGroesse / 2);

			MyZielbereich = new Ellipse();
			MyZielbereich.Fill = new SolidColorBrush(Color.FromArgb(40, 0, 255, 255));
			MyZielbereich.Height = reichweite;
			MyZielbereich.Width = reichweite;
			Canvas.SetLeft(MyZielbereich, MyBauKoordinate.X - reichweite / 2);
			Canvas.SetTop(MyZielbereich, MyBauKoordinate.Y - reichweite / 2);
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

		public bool ZielErfassen(Gegner gegner)
		{
			if (MyZielbereich.RenderedGeometry.FillContains(new Point(gegner.MyPosition.X - (MyBauKoordinate.X - MyZielbereich.ActualWidth / 2), 
																		gegner.MyPosition.Y - (MyBauKoordinate.Y - MyZielbereich.ActualHeight / 2))) && gegner.IstAmLeben())
			{
				MyZiel = gegner;
				return true;
			}

			return false;
		}

		public Projektil Schiessen(Gegner gegner)
		{
			if (MyZiel != null && MyAbklingzeit <= 0)
			{
				MyAbklingzeit = MyKadenz;
				return new Projektil(MySpielbrett, MyBauKoordinate, gegner, MySchaden, MyGeschwindigkeit);
			}

			return null;
		}

		public void Nachladen(double intervall)
		{
			MyAbklingzeit -= intervall;
		}
	}

	public class MGTurm : Turm
	{
		public MGTurm(Canvas spielbrett, int rasterGroesse, Point bauplatz, int reichweite)
						: base(spielbrett, rasterGroesse, bauplatz, reichweite)
		{
			MyKadenz = 0.1;
			MySchaden = 1;
			MyGeschwindigkeit = 400;

			MyBauform = new Polygon();
			MyBauform.Fill = Brushes.SlateGray;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, rasterGroesse - 2));
			MyBauform.Points.Add(new Point(2, rasterGroesse - 2));
			Canvas.SetLeft(MyBauform, bauplatz.X * rasterGroesse);
			Canvas.SetTop(MyBauform, bauplatz.Y * rasterGroesse);
		}
	}

	public class SniperTurm : Turm
	{
		public SniperTurm(Canvas spielbrett, int rasterGroesse, Point bauplatz, int reichweite)
							: base(spielbrett, rasterGroesse, bauplatz, reichweite)
		{
			MyKadenz = 0.5;
			MySchaden = 6;
			MyGeschwindigkeit = 500;

			MyBauform = new Polygon();
			MyBauform.Fill = Brushes.DarkSlateGray;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, 2));
			MyBauform.Points.Add(new Point(rasterGroesse - 2, rasterGroesse - 2));
			MyBauform.Points.Add(new Point(2, rasterGroesse - 2));
			Canvas.SetLeft(MyBauform, bauplatz.X * rasterGroesse);
			Canvas.SetTop(MyBauform, bauplatz.Y * rasterGroesse);
		}
	}
}
