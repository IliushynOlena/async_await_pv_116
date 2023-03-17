﻿using IOExtensions;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
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
using Path = System.IO.Path;

namespace _02_copy_file
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel model;
      
        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel()
            {
                Source = "D:\\OneDrive - ITSTEP\\MyMaterials\\2 семестр\\ООП\\C++ -" +
                " Exceptions-20210623_094011-Meeting Recording.mp4",
                Destination = "C:\\Users\\helen\\Desktop\\Test",
                Progress = 0
            };
            this.DataContext = model;
        }
        private async void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            // type 1 - using File class
            string filename = Path.GetFileName(model.Source);
            string destFilePath = Path.Combine(model.Destination , filename);
            //File.Copy(Source, destFilePath, true);
            await CopyFileAsync(model.Source, destFilePath);    
            MessageBox.Show("Complited!!!");
        }
        private Task CopyFileAsync(string src, string dest)
        {
            //return Task.Run(() =>
            //{
            //    // type 2- using FileStream class
            //    using FileStream streamSource = new FileStream(src, FileMode.Open, FileAccess.Read);
            //    using FileStream streamDest = new FileStream(dest, FileMode.Create, FileAccess.Write);

            //    byte[] buffer = new byte[1024 * 8];//8KB   //12
            //    int bytes = 0;
            //    do
            //    {
            //        bytes = streamSource.Read(buffer, 0, buffer.Length);//0.5

            //        streamDest.Write(buffer, 0, bytes);//8
            //        //% = total  received 
            //        float procent = streamDest.Length / (streamSource.Length / 100);
            //        model.Progress = procent;

            //    } while (bytes > 0);

            //});
            return FileTransferManager.CopyWithProgressAsync(src, dest, (pr) =>
            {
                model.Progress = pr.Percentage;

            }, false);
        }
        private void OpenSourceClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == true)
            {
                model.Source = dialog.FileName;
            }
        }
        private void OpenDestClick(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog  dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = dialog.FileName;
            }
        }
    }
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public double Progress { get; set; }
        public bool IsWaiting => Progress == 0;
    }
}
