using System;
using System.Windows.Controls;

namespace ESMLoop.Windows
{
    /// <summary>
    /// Interaction logic for Question3Control.xaml
    /// </summary>
    public partial class Question3Control : UserControl
    {
        private const string _OTHER = "Sonstige (Bitte im Freitextfeld unten beschreiben)";
        private readonly String[] _dropdownItems =
            new[]
            {
                "Korrekturlesen",
                "Kreative Tätigkeit",
                "(Literatur)Recherche",
                "Mathematische Berechnungen",
                "Präsentationserstellung",
                "Programmierung",
                "Statistische Auswertung",
                "Studiendesignerstellung",
                "(Text)formatierung",
                "Textproduktion",
                "Visualisierungs-/Tabellenerstellung",
                _OTHER
            };

        public Question3Control()
        {
            InitializeComponent();
            InitializeDropdown();
        }

        private void InitializeDropdown()
        {
            DropDown.Items.Clear();
            foreach (string dropdownItem in _dropdownItems)
            {
                DropDown.Items.Add(dropdownItem);
            }
        }

        public bool Answered()
        {
            bool other = DropDown.Text.Equals(_OTHER);
            bool changed = DropDown.SelectedIndex != -1;
            bool empty = TextBoxDescription.Text.Equals(String.Empty);
            //return (changed && !other) || (changed && other && !empty);
            //Vereinfachung:
            return changed && (!other || !empty);
        }

        public Question3Answer Answer()
        {
            return new Question3Answer(DropDown.Text, TextBoxDescription.Text);
        }
    }
    public class Question3Answer
    {
        public string DropDownAnswer;
        public string TextBoxDescription;

        public Question3Answer(string DropDownAnswer, string TextBoxDescription)
        {
            this.DropDownAnswer = DropDownAnswer;
            this.TextBoxDescription = TextBoxDescription;
        }
    }
}

