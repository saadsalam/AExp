using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoExport
{
    //Delegate types for RecordButtons usercontrol
    public delegate void DeleteRecordDelegate();
    public delegate void ModifyRecordDelegate();
    public delegate void MovePrevDelegate();
    public delegate void MoveNextDelegate();
    public delegate void NewRecordDelegate();
    public delegate void SaveRecordDelegate();
    public delegate void CancelRecordDelegate();
}
