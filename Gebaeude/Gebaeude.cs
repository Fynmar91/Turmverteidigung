using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Gebaeude
{
	public class Turm
	{
		Rectangle MyForm { get; set; }
		static Canvas MySpielbrett { get; set; }
		Point MyBauplatz { get; set; }


		public Turm(Canvas spielbrett, int rastergroesse, Point bauplatz)
		{
			MyForm = new Rectangle();
			MySpielbrett = spielbrett;
			MyBauplatz = bauplatz;

			BaueTurm(rastergroesse);
		}

		void BaueTurm(int rastergroesse)
		{
			MyForm.Fill = Brushes.Beige;
			MyForm.Width = rastergroesse - 2;
			MyForm.Height = rastergroesse - 2;
			MySpielbrett.Children.Add(MyForm);
			Canvas.SetLeft(MyForm, MyBauplatz.X * rastergroesse + 1);
			Canvas.SetTop(MyForm, MyBauplatz.Y * rastergroesse + 1);
		}

		public void ZerstoereTurm()
		{
			MySpielbrett.Children.Remove(MyForm);
		}
	}
}
