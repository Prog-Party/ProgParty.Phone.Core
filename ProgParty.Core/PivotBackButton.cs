using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ProgParty.Core
{
    public class PivotBackButton
    {
        Stack<int> _searchPivotStack = new Stack<int>();

        private Pivot _pivot { get; set; }
        private Page _pivotPage { get; set; }

        public static PivotBackButton Instance { get; } = new PivotBackButton();

        private PivotBackButton()
        {
            _searchPivotStack = new Stack<int>();
        }
        internal void Register(Pivot pivot, Page page)
        {
            _pivot = pivot;
            _pivotPage = page;
            pivot.SelectionChanged += searchPivot_SelectionChanged;
        }


        public void PivotChanged(SelectionChangedEventArgs e, int index)
        {
            var original = e.OriginalSource;
            _searchPivotStack.Push(index);
        }

        public void BackButtonPressed(Windows.Phone.UI.Input.BackPressedEventArgs e, Frame frame, Pivot pivot)
        {
            if (_searchPivotStack.Count == 0)
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                    e.Handled = true;
                }
            }
            else
            {
                _searchPivotStack.Pop();
                if (_searchPivotStack.Count != 0)
                {
                    pivot.SelectedIndex = _searchPivotStack.Pop();
                    e.Handled = true;
                }
                else if (frame.CanGoBack)
                {
                    frame.GoBack();
                    e.Handled = true;

                }
            }
        }


        public void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            BackButtonPressed(e, _pivotPage.Frame, _pivot);
        }

        private void searchPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var searchPivot = (Pivot)sender;
            PivotChanged(e, searchPivot.SelectedIndex);
        }
    }
}
