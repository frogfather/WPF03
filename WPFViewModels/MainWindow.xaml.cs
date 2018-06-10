using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System;

namespace WpfTreeView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region On Loaded

        /// <summary>
        /// When the application first opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   
            //Get every logical drive on the machine
            foreach (var drive in  Directory.GetLogicalDrives())
            {
                //Create a new item for it
                var item = new TreeViewItem()
                {
                    //Set the header
                    Header = drive,
                    //and the full path - in the case of a drive it's the same thing
                    Tag = drive

                };


                //Add a dummy item
                item.Items.Add(null);

                //Listen out for item being expanded
                item.Expanded += Folder_Expanded;

                //Add it to the tree view
                FolderView.Items.Add(item);                
            }
        }

        #endregion

        #region Expanded
        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            #region initial checks

            //sender is the tree view item that is to be expanded
            var item = (TreeViewItem)sender;

            //if the item contains only dummy data
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;
            //remove the dummy items which are there to make the expand button visible
            item.Items.Clear();

            //get the full path
            var fullPath = (string)item.Tag;

            #endregion

            #region Get directories

            //Create a blank list for the directories
            var directories = new List<string>();

            //do not use this construction! catching errors by ignoring them is v bad practice. 
            //This example is from Angel6 video and he emphasises this. It's here for speed.
            try
            {
                //this populates the directories list with names of all the sub directories
                var dirs = Directory.GetDirectories(fullPath);
                if (dirs.Length > 0)
                    directories.AddRange(dirs);
            }
            catch { }

            //we now have our list of directory names
            directories.ForEach(directoryPath =>
            {
                //create directory item
                var subItem = new TreeViewItem()
                {
                    //set header as folder name
                    Header = GetFileFolderName(directoryPath),
                    //and tag as full path
                    Tag = directoryPath
                };

                //add dummy path so we can expand the folder
                subItem.Items.Add(null);

                //handle expanding
                subItem.Expanded += Folder_Expanded;

                item.Items.Add(subItem);
            });

            #endregion

            #region Get Files
            //Create a blank list for the files
            var files = new List<string>();

            //do not use this construction! catching errors by ignoring them is v bad practice. 
            //This example is from Angel6 video and he emphasises this. It's here for speed.
            try
            {
                //this populates the directories list with names of all the sub directories
                var fs = Directory.GetFiles(fullPath);
                if (fs.Length > 0)
                    files.AddRange(fs);
            }
            catch { }

            //we now have our list of directory names
            files.ForEach(filePath =>
            {
                //create directory item
                var subItem = new TreeViewItem()
                {
                    //set header as file name
                    Header = GetFileFolderName(filePath),
                    //and tag as full path
                    Tag = filePath
                };

                item.Items.Add(subItem);
            });

            #endregion
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Find the file or folder name from a full path
        /// </summary>
        /// <param name="path">The full path</param>
        /// <returns></returns>
        public static string GetFileFolderName(string path)
            {
                //if we have no path return empty
                if (string.IsNullOrEmpty(path))
            { return string.Empty; }

            //make all slashes backslashes
            var normalizedPath = path.Replace('/', '\\');

            //find the last backslash in the path
            var lastIndex = normalizedPath.LastIndexOf('\\');

            //if we don't find and backslash return the path
            if (lastIndex <= 0)
                return path;

            //return name after last backslash
            return path.Substring(lastIndex + 1);

            }
        #endregion

    }



}
