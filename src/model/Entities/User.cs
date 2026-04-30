using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using src.model.Entities.EntityConfiguration;

namespace src.model.Entities
{
    [EntityTypeConfiguration(typeof(UserConfiguration))]
    [Table("Users")]
    public class User
    {
        public long Id {get; set;}
        public String Name {get;set;}
        public String Token{get;set;}

        public User(long id,String name,String token)
        {
            this.Id = id;
            this.Name = name;
            this.Token = token;
        }
    }
}