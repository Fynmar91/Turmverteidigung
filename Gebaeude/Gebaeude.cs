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


		public Turm(Canvas spielbrett, int rastergroesse, Point bauplatz, int reichweite)
		{
			MySpielbrett = spielbrett;
			MyBauplatz = bauplatz;

			MyZielbereich = new Ellipse();
			MyZielbereich.Fill = new SolidColorBrush(Color.FromArgb(40, 0, 255, 255));
			MyZielbereich.Height = reichweite;
			MyZielbereich.Width = reichweite;
			Canvas.SetLeft(MyZielbereich, bauplatz.X * rastergroesse - reichweite / 2 + rastergroesse / 2);
			Canvas.SetTop(MyZielbereich, bauplatz.Y * rastergroesse - reichweite / 2 + rastergroesse / 2);
		}

		public void BaueTurm()
		{
			MySpielbrett.Children.Add(MyBauform);
			MySpielbrett.Children.Add(MyZielbereich);
		}

		public void ZerstoereTurm()
		{
			MySpielbrett.Children.Remove(MyBauform);
			MySpielbrett.Children.Remove(MyZielbereich);
		}
	}

	public class MGTurm : Turm
	{
		public MGTurm(Canvas spielbrett, int rastergroesse, Point bauplatz, int reichweite)
						: base(spielbrett, rastergroesse, bauplatz, reichweite)
		{
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
