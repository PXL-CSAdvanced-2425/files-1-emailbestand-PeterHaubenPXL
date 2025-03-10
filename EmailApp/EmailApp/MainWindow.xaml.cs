using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmailApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    string path = @"../../../../../email.txt";
    Dictionary<string, string> dictGeg = new Dictionary<string, string>();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void readFileButton_Click(object sender, RoutedEventArgs e)
    {
        InlezenBestand();
    }


    private void InlezenBestand()
    {
        StringBuilder sb = new StringBuilder();

        string[] strArray;
        string str;

        using (StreamReader sr = new StreamReader(path))
        {
            try
            {
                if (File.Exists(path)) // controleer of bestand bestaat
                {
                    while (!sr.EndOfStream)
                    {
                        strArray = sr.ReadLine().Split(',');
                        strArray[0] = strArray[0].Substring(1, strArray[0].Length - 2);

                        while (strArray[0].Length < 20)
                        {
                            strArray[0] += " ";
                        }

                        //strArray[1] = strArray[1].Substring(1);
                        strArray[1] = strArray[1].Substring(1, strArray[1].Length - 2);
                        sb.AppendLine(strArray[0] + ": " + strArray[1]);
                    }
                    resultTextBox.Text = sb.ToString();
                }
                else
                {
                    MessageBox.Show("File bestaat niet!", "File bestaat niet!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }

    private void readDialogButton_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();

        if (ofd.ShowDialog() == true)
        {
            path = ofd.FileName;

            InlezenBestand();
        }
    }

    private void readDictionaryButton_Click(object sender, RoutedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();

        string[] strArray = null; 

        using (StreamReader sr = new StreamReader(path))
        {
            try
            {
                if (File.Exists(path)) // controleer of bestand bestaat
                {
                    while (!sr.EndOfStream)
                    {
                        strArray = sr.ReadLine().Split(',');

                        strArray[0] = strArray[0].Substring(1, strArray[0].Length - 2);
                        strArray[1] = strArray[1].Substring(1, strArray[1].Length - 2);

                        dictGeg.Add(strArray[0], strArray[1]);
                    }

                    foreach(var item in dictGeg)
                    {
                        sb.AppendLine(item.Key + ": " + item.Value);
                    }
                    resultTextBox.Text = sb.ToString();
                }
                else
                {
                    MessageBox.Show("File bestaat niet!", "File bestaat niet!");
                }
                writeButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }


    }

    private void writeButton_Click(object sender, RoutedEventArgs e)
    {
        if(dictGeg.Count >= 1)
        {
            using(StreamWriter sw = new StreamWriter("Adressen.txt"))
            {
                foreach(var item in dictGeg)
                {
                    sw.WriteLine(item.Value);
                }
            }
        }
        else
        {
            MessageBox.Show("Er is nog niets opgeslagen in dictonairy!", "Geen data");
        }
    }

    private void addButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameTextBox.Text))
        {
            MessageBox.Show("Naam mag niet leeg zijn","Ontbrekend data", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(emailTextBox.Text))
        {
            MessageBox.Show("E-mail mag niet leeg zijn", "Ontbrekend data", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if(!File.Exists(path))
        {
            MessageBox.Show("File bestaat niet", "File ontbreekt", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        SaveFileDialog sfd = new SaveFileDialog();
        
        if (sfd.ShowDialog() == true)
        {
            path = sfd.FileName;

            if (!File.Exists(path))
            {
                MessageBox.Show("File bestaat niet", "File ontbreekt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("\"" + nameTextBox.Text + "\",\"" + emailTextBox.Text + "\"");
                }
            }
        }
        
    }

}