using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ESMLoop.Windows
{
    /// <summary>
    /// Interaction logic for Question2Control.xaml
    /// </summary>
    public partial class Question2Control : UserControl
    {
        private readonly Dictionary<int, LikertControl> _likerts;

        public Question2Control()
        {
            InitializeComponent();

            _likerts = new()
            {
                { 0, Nasa0 },
                { 1, Nasa1 },
                { 2, Nasa2 },
                { 3, Nasa3 },
                { 4, Nasa4 },
                { 5, Nasa5 }
            };
        }
        public bool Answered()
        {
            return !_likerts.Values.Select(x => x.answered).Contains(false);
        }

        public int[] Answer()
        {
            return _likerts.Values.Select(x => x._ticked).ToArray();
        }
    }
}
