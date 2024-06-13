namespace Option1.Models
{
    public class Products
    {
        // = null! - нельзя допустить попадание товара в базу данных без имени
        // описания или категории
        public int Id { get; set; }
        
        public string Name { get; set; } = null!; 
        public string Description { get; set; } = null!; 
        public int Price { get; set; }  
        public string Category { get; set; } = null!; 
    }
}
