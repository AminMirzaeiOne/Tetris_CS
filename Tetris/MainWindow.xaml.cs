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
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GAMESPEED = 700;// millisecond
        List<System.Media.SoundPlayer> soundList = new List<System.Media.SoundPlayer>();
        DispatcherTimer timer;
        Random shapeRandom;


        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
