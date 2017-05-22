using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tutorial1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private bool isClicked = false;
		private bool isChecked = false;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			if(isClicked)
			{
				label.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 255));
				label.Foreground.Opacity = slider.Value/255;
				isClicked = false;
			}
			else
			{
				label.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 0, 255));
				label.Foreground.Opacity = slider.Value/255;
				isClicked = true;
			}
		}

		private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			label.Foreground.Opacity = slider.Value/255;
			if(isChecked)
			{
				checkBox.Content = slider.Value;
			}
		}

		private void checkBox_Checked(object sender, RoutedEventArgs e)
		{
			isChecked = true;
			checkBox.Content = slider.Value;
		}

		private void checkBox_Unchecked(object sender, RoutedEventArgs e)
		{
			isChecked = false;
			checkBox.Content = "Show Opacity";
		}
	}
}
