using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfTreeView
{
    /// <summary>
    /// convert full path to a specific image type of a drive, folder or file
    /// </summary>
    /// 
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConvertor : IValueConverter
    {
        public static HeaderToImageConvertor Instance = new HeaderToImageConvertor();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //convert string to image
            var path = (string)value;

            //if the path is null, ignore
            if (path == null) { return null; }

            //get the name of the file folder
            var name = MainWindow.GetFileFolderName(path);

            //by default assume a file
            var image = "Images/File.png";

            //if the name is blank assume it's a drive as we cannot have a blank file/folder name
            if (String.IsNullOrEmpty(name))
            {
                image = "Images/Drive.png";
            }
            else if (new FileInfo(path).Attributes.HasFlag(FileAttributes.Directory))
            {
                image = "Images/Folder.png";
            }


            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
