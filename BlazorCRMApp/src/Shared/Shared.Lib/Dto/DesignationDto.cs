using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Lib.Dto
{
    public class DesignationRequestDto
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Designation Name")]
        public string Name { get; set; } = null!;

        [Required]
        [DisplayName("Designation Colour")]
        public string Colour { get; set; } = null!;

        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }

    public class DesignationResponseDto
    {
        public int Id { get; set; }


        [DisplayName("Designation Name")]
        public string Name { get; set; }

        public string Colour { get; set; } = null!;

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }
    }

    public class PriorityRequestDto
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Priority Title")]
        public string Title { get; set; } = null!;

        [Required]
        [DisplayName("Priority Color")]
        public string Color { get; set; } = null!;

        [DisplayName("Active")]
        public bool IsActive { get; set; }
    }

    public class PriorityResponseDto
    {
        public int Id { get; set; }


        [DisplayName("Priority Title")]
        public string Title { get; set; }

        public string Color { get; set; } = null!;

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        public string CreatedDate { get; set; }

        public string ModifiedDate { get; set; }
    }

}
