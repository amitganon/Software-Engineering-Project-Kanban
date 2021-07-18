using Frontend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fronted.ViewModel
{
    class UpgradeToProViewModel : NotifiableObject
    {
        private bool isClicked = false;
        public bool IsClicked { get => isClicked; set { isClicked = value; RaisePropertyChanged("IsClicked"); RaisePropertyChanged("TextBlockColor"); } }
        public SolidColorBrush TextBlockColor { get { return isClicked ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.Black); } }
        public UpgradeToProViewModel()
        {
        }
    }
}
