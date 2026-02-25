namespace PD411_Shop.Models
{
    public interface IBaseModel
    {
        public int Id { get; set; }
    }

    public class BaseModel : IBaseModel
    {
        public int Id { get; set; }
    }
}
