using StrumentiMusicali.Library.Core.interfaces;
using StrumentiMusicali.Library.Core.Item.Base;
using StrumentiMusicali.Library.Entity.Base;

namespace StrumentiMusicali.Library.Core.Events.Generics
{
    public class ItemSelected<TBaseItem, TEntity> : CurrentItem<TBaseItem, TEntity>
        where TBaseItem : BaseItem<TEntity>
        where TEntity : BaseEntity
    {
        public ItemSelected(TBaseItem baseItem, IKeyController filterController)

            : base(baseItem, filterController)
        {
        }
    }
}