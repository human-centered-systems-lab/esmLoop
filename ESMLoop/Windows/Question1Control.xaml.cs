using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace ESMLoop.Windows
{
    /// <summary>
    /// Interaction logic for Question1Control.xaml
    /// </summary>
    public partial class Question1Control : UserControl
    {
        private readonly Dictionary<int, LikertControl> _likerts;

        public Question1Control()
        {
            InitializeComponent();
            _likerts = new()
            {
                { 0, Flow0 },
                { 1, Flow1 },
                { 2, Flow2 },
                { 3, Flow3 },
                { 4, Flow4 },
                { 5, Flow5 },
                { 6, Flow6 },
                { 7, Flow7 },
                { 8, Flow8 },
                { 9, Flow9 },
                { 10, Flow10 },
                { 11, Flow11 },
                { 12, Flow12 }
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
