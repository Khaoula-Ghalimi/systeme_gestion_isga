using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using systeme_gestion_isga.Domain.Entities;

namespace systeme_gestion_isga.Features.Subject.ViewModels
{
    public class SubjectVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name must be at most 100 characters.")]
        public string Name { get; set; }

        [StringLength(10, ErrorMessage = "Code must be at most 10 characters.")]
        public string Code { get; set; }

    }
}
