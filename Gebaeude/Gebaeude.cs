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
		static Canvas MySpielbrett { get; set; }
		Point MyBauplatz { get; set; }


		public Turm(Canvas spielbrett, int rastergroesse, Point bauplatz)
		{
			MySpielbrett = spielbrett;
			MyBauplatz = bauplatz;
		}

		public void BaueTurm()
		{
			MySpielbrett.Children.Add(MyBauform);
		}

		public void ZerstoereTurm()
		{
			MySpielbrett.Children.Remove(MyBauform);
		}
	}

	public class MGTurm : Turm
	{
		public MGTurm(Canvas spielbrett, int rastergroesse, Point bauplatz) : base(spielbrett, rastergroesse, bauplatz)
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
		public SniperTurm(Canvas spielbrett, int rastergroesse, Point bauplatz) : base(spielbrett, rastergroesse, bauplatz)
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
