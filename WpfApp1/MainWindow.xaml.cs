using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Xml;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath;
        private List<string> names =
            new List<string>() {
                "Microsoft",
                "Apple",
                "Oracle",
                "IBM",
                "Яндекс",
                "ТОВ \"ТЕХМЕТМАШ\"",
                "Без имени"
            };

        public MainWindow()
        {
            InitializeComponent();
        }
        private void MenuItem_ClickOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "XML files (*.xml)|*.xml";

            if (!(fileDialog.ShowDialog() ?? false))
                return;
            filePath = fileDialog.FileName;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            // получим корневой элемент
            XmlElement xRoot = xmlDocument.DocumentElement;
            if (xRoot == null)
                return;

            int count = 1;
            intersectionsListBox.Items.Add($"В данном списке всего - {xRoot.ChildNodes.Count} элементов.");
            foreach (XmlElement childNode in xRoot.ChildNodes)
            {
                intersectionsListBox.Items.Add($"{count}:");
                count++;
                switch (childNode?.NodeType)
                {
                    case XmlNodeType.Element:
                        if (childNode.HasChildNodes)
                        {
                            foreach (XmlNode childNode2 in childNode.ChildNodes)
                            {
                                switch (childNode2?.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        intersectionsListBox.Items.Add($"{childNode2.Name}: {childNode2.InnerText}");
                                        break;
                                    case XmlNodeType.Text:
                                        intersectionsListBox.Items.Add(" - " + childNode2.Value);
                                        break;
                                    case XmlNodeType.EndElement:
                                        intersectionsListBox.Items.Add(" ");
                                        break;
                                }
                            }
                        }
                        else
                            intersectionsListBox.Items.Add(childNode.Name + childNode.OuterXml);
                        break;
                    case XmlNodeType.Text:
                        intersectionsListBox.Items.Add(" - " + childNode.Value);
                        break;
                    case XmlNodeType.EndElement:
                        intersectionsListBox.Items.Add(" ");
                        break;
                }
                intersectionsListBox.Items.Add(string.Empty);
            }
        }

        private void MenuItem_ClickEnter(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            MessageBox.Show("вводится строка для сравнения");
        }

        private void MenuItem_ClickObject(object sender, RoutedEventArgs e)
        {

            if (filePath == null)
                return;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);

            XmlNodeList recordNodes =
                xmlDocument.GetElementsByTagName("DATA")[0].ChildNodes;

            var intersectionsList =
                recordNodes.Cast<XmlNode>()
                    .Select(record => ((XmlElement)record).GetElementsByTagName("NAME")[0].InnerText)
                    .Intersect(names);

            foreach (var item in intersectionsList)
            {
                ResultListBox.Items.Add(item);
            }

        }
        private void MenuItem_ClickClose(object sender, RoutedEventArgs e)
        {
            intersectionsListBox.Items.Clear();
            MenuItem menuItem = (MenuItem)sender;
            MessageBox.Show("Document is closed");
            //MessageBox.Show(menuItem.Header.ToString());
        }
    }
}
