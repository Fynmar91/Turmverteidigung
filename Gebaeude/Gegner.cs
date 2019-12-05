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
	public class Gegner
	{
		public double MyX { get; set; }
		public double MyY { get; set; }
		static Canvas MySpielbrett { get; set; }
		protected Polygon MyForm { get; set; }

		public Gegner(Canvas spielbrett, Point punkt)
		{
			MyX = punkt.X;
			MyY = punkt.Y;
			MySpielbrett = spielbrett;

			MyForm = new Polygon();
			MyForm.Fill = Brushes.Blue;
			MyForm.Points.Add(new Point(0, 10));
			MyForm.Points.Add(new Point(5, -5));
			MyForm.Points.Add(new Point(-5, -5));
			Canvas.SetLeft(MyForm, MyX);
			Canvas.SetTop(MyForm, MyY);
		}

		public void ErzeugeGegner()
		{
			MySpielbrett.Children.Add(MyForm);
		}
	}
}
