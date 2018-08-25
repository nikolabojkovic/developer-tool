//using System.ComponentModel.DataAnnotations;
using WebApi.Interfaces;

namespace WebApi.Models
{
    public class Test : IEntity
    {
       // [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
    }
}
