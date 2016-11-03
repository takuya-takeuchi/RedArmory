﻿using System;
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

namespace Logo
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var args = new BitmapMakerArgs
                {
                    Element = MainCanvas,
                    FileName = FileNameText.Text,
                    CreateBlackVersion = CreateBlackCheck.IsChecked == true,
                    ImageSizes = ImageSizeList.Items.OfType<Size>().ToArray(),
                };
                BitmapMaker.Start(args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "XAML to Image");
                Close();
            }
        }
    }
}
