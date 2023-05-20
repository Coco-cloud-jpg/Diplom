namespace DAL.DTOS
{
    public class ShortEntityModelGuid: ShortEntityModelBase<Guid>
    {
    }
    public class ShortEntityModelShort : ShortEntityModelBase<short>
    {
    }
    public class ShortEntityModelBase<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
    }
}
