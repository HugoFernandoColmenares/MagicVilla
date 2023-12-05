namespace MagicVilla_API.Modelos.Especificaciones
{
    public class PageList<T> : List<T>
    {
        public Metadata MetaData { get; set; }
        public PageList(List<T> items, int count, int pageNumber, int pageSize)
        {
            MetaData = new Metadata 
            {
                TotalCount = count,
                PagesSize = pageSize,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };
            AddRange(items);
        }
        public static PageList<T> ToPageList(IEnumerable<T> entidad, int pageNumber, int pageSize)
        {
            var count = entidad.Count();
            var items = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PageList<T>(items, count, pageNumber, pageSize);
        }
    }
}
