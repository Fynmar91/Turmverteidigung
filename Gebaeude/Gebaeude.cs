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
	public class Turm
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


		public Turm(Canvas spielbrett, int rastergroesse, Point bauplatz, int reichweite)
		{
			MySpielbrett = spielbrett;
			MyBauplatz = bauplatz;
			MyBauKoordinate = new Point(bauplatz.X * rastergroesse + rastergroesse / 2, bauplatz.Y * rastergroesse  + rastergroesse / 2);

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
				return new Projektil(MySpielbrett, MyBauKoordinate, gegner, MySchaden);
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
		public MGTurm(Canvas spielbrett, int rastergroesse, Point bauplatz, int reichweite)
						: base(spielbrett, rastergroesse, bauplatz, reichweite)
		{
			MyKadenz = 0.2;
			MySchaden = 1;

			MyBauform = new Polygon();
			MyBauform.Fill = Brushes.Beige;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rastergroesse - 2, 2));
			MyBauform.Points.Add(new Point(rastergroesse - 2, rastergroesse - 2));
			MyBauform.Points.Add(new Point(2, rastergroesse - 2));
			Canvas.SetLeft(MyBauform, bauplatz.X * rastergroesse);
			Canvas.SetTop(MyBauform, bauplatz.Y * rastergroesse);
		}
	}

	public class SniperTurm : Turm
	{
		public SniperTurm(Canvas spielbrett, int rastergroesse, Point bauplatz, int reichweite)
							: base(spielbrett, rastergroesse, bauplatz, reichweite)
		{
			MyKadenz = 2;
			MySchaden = 8;

			MyBauform = new Polygon();
			MyBauform.Fill = Brushes.DarkRed;
			MyBauform.Points.Add(new Point(2, 2));
			MyBauform.Points.Add(new Point(rastergroesse - 2, 2));
			MyBauform.Points.Add(new Point(rastergroesse - 2, rastergroesse - 2));
			MyBauform.Points.Add(new Point(2, rastergroesse - 2));
			Canvas.SetLeft(MyBauform, bauplatz.X * rastergroesse);
			Canvas.SetTop(MyBauform, bauplatz.Y * rastergroesse);
		}
	}
}
