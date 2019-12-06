namespace StrumentiMusicali.Library.Core.Events.Image
{
    public class ImmaginiFile<T>
        where T : Entity.Base.BaseEntity
    {
        public ImmaginiFile(string file, string nome, T entity)
        {

            File = file;
            Name = nome;
            Entity = entity;
        }

        public string File { get; private set; }
        public string Name { get; private set; }

        public T Entity { get; private set; }
    }
}
