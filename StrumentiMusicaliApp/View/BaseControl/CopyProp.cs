using DevExpress.XtraVerticalGrid.Rows;
using System;

namespace StrumentiMusicali.App.View.BaseControl
{

    public class CopyProp
    {
        public System.Reflection.PropertyInfo Prop { get; set; }
        public object ObjectItem { get; set; }

        public EditorRow EditRow { get; set; }

        public void SetValue(object val)
        {
            Prop.SetValue(ObjectItem, val);
        }
        public Object GetValue()
        {
            return Prop.GetValue(ObjectItem);
        }
    }
}
