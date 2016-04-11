/*
    Copyright (C) 2016 Ryukuo

    This file is part of Organizer.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Organizer.Components;
using System.IO;
using System.Windows.Controls;

namespace Organizer.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DirectorySelectionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFolderDialog openFolderDialog = new OpenFolderDialog()
            {
                Title = "organizer - select a directory...",
            };
            if (openFolderDialog.ShowDialog() == true)
            {
                if (Directory.Exists(openFolderDialog.FileName))
                {
                    DirectoryTextBox.Text = openFolderDialog.FileName.ToLower();
                }
            }
        }

        private async void AddMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string extension = await this.ShowInputAsync("organizer", "enter an extension: (*.*)");
            if (extension.Contains("(") || extension.Contains(")"))
            {
                extension = extension.Replace("(", "").Replace(")", "");
            }
            if (!extension.Contains("."))
            {
                extension = "." + extension;
            }
            if (!extension.Contains("*"))
            {
                extension = "*" + extension;
            }
            ExtensionListBox.Items.Add(new ListBoxItem { Content = extension });
        }

        private void RemoveMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExtensionListBox.Items.Remove(ExtensionListBox.SelectedItem);
        }

        private void ClearMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExtensionListBox.Items.Clear();
        }

        private void OrganizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string prefix = (bool)AddPrefixCheckBox.IsChecked ? PrefixTextBox.Text : "";
            string suffix = (bool)AddSuffixCheckBox.IsChecked ? SuffixTextBox.Text : "";
            DirectoryInfo directoryInfo = new DirectoryInfo(DirectoryTextBox.Text);
            int i = (int)StartingIndexNumericUpDown.Value;
            if (FileExtensionsCheckBox.IsChecked == true && ExtensionListBox.Items.Count > 0)
            {
                foreach (ListBoxItem lbi in ExtensionListBox.Items)
                {
                    FileInfo[] files = directoryInfo.GetFiles(lbi.Content.ToString());
                    foreach (FileInfo file in files)
                    {
                        file.MoveTo(Path.Combine(DirectoryTextBox.Text, prefix + i++ + suffix + file.Extension));
                    }

                    if (ResetIndexCheckBox.IsChecked == true && ResetIndexCheckBox.IsEnabled == true)
                    {
                        i = (int)StartingIndexNumericUpDown.Value;
                    }
                }
                
            }
            else
            {
                FileInfo[] files = directoryInfo.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.MoveTo(Path.Combine(DirectoryTextBox.Text, prefix + i++ + suffix + file.Extension));
                }
            }
        }
    }
}
