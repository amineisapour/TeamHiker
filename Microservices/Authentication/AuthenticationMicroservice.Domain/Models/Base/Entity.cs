using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationMicroservice.Domain.Models.Base
{
    public abstract class Entity : GiliX.Domain.IEntity
    {
        protected Entity() : base()
        {
            Id = System.Guid.NewGuid();
            IsActive = true;
            RegisterDateTime = DateTime.Now;
        }

        [Required, Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public System.Guid Id { get; set; }
        
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime RegisterDateTime { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
