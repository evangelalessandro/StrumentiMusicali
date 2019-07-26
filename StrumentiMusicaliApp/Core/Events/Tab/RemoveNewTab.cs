namespace StrumentiMusicali.App.Core.Events.Tab
{
    public class RemoveNewTab
    {
        public RemoveNewTab(DevExpress.XtraTab.XtraTabPage tab)
        {
            Tab = tab;
        }

        public DevExpress.XtraTab.XtraTabPage Tab { get; set; }
    }
}
