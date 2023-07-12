
using System.ComponentModel.DataAnnotations;

namespace WeatherForecast.Domain
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? ModifiedBy { get; set; }
        public virtual void Update()
        {
            this.ModifiedDate = DateTime.Now;
        }
    }
    
}