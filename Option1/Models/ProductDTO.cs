namespace Option1.Models
{
    /* объект, используемый 
     * для инкапсуляции данных и  их отправки из одной подсистемы приложения в другую
    */
    public class ProductDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Price { get; set; }
    }
}
