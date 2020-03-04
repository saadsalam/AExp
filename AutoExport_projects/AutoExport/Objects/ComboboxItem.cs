using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport.Objects
{
    public class ComboboxItem
    {
        public string cboText { get; set; }
        public string cboValue { get; set; }

        public ComboboxItem MakeCopy(ComboboxItem cboOriginal)
        {
            ComboboxItem cboCopy = new ComboboxItem();
            cboCopy.cboText = cboOriginal.cboText;
            cboCopy.cboValue = cboOriginal.cboValue;
            return cboCopy;
        }
    }
}
