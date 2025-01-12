using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ESMLoop
{
    /// <summary>
    /// Interaction logic for LikertControl.xaml
    /// </summary>
    public partial class LikertControl : UserControl
    {
        internal readonly Dictionary<int, CheckBox> _checkBoxes;
        internal int _ticked;
        internal bool answered;

        public string Question
        {
            get { return (string)GetValue(QuestionProperty); }
            set { SetValue(QuestionProperty, value); }
        }
        public static readonly DependencyProperty QuestionProperty =
        DependencyProperty.Register("Question", typeof(string), typeof(LikertControl), new PropertyMetadata(null));

        public string Agree
        {
            get { return (string)GetValue(AgreeProperty); }
            set { SetValue(AgreeProperty, value); }
        }
        public static readonly DependencyProperty AgreeProperty =
        DependencyProperty.Register("Agree", typeof(string), typeof(LikertControl), new PropertyMetadata(null));

        public string Disagree
        {
            get { return (string)GetValue(DisagreeProperty); }
            set { SetValue(DisagreeProperty, value); }
        }
        public static readonly DependencyProperty DisagreeProperty =
        DependencyProperty.Register("Disagree", typeof(string), typeof(LikertControl), new PropertyMetadata(null));

        public LikertControl()
        {
            InitializeComponent();
            //Use shorthand constructor
            _checkBoxes = new Dictionary<int, CheckBox>()
            {
                { 1, CheckBox0 },
                { 2, CheckBox1 },
                { 3, CheckBox2 },
                { 4, CheckBox3 },
                { 5, CheckBox4 },
                { 6, CheckBox5 },
                { 7, CheckBox6 }
            };
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int ownID = _checkBoxes.First(c => c.Value.Equals((CheckBox)sender)).Key;
            UntickAllExceptSelf(ownID);
            _ticked = ownID;
            answered = true;
        }

        private void UntickAllExceptSelf(int id)
        {
            List<CheckBox> allExceptSelf = _checkBoxes.Where(c => c.Key != id).Select(c => c.Value).ToList();
            allExceptSelf.ForEach(checkBox => checkBox.IsChecked = false);
        }
    }
}
