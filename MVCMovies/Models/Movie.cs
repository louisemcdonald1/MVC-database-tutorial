//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

//created by the ADO.NET Entity Data Model and matches the database table
//data annotations added by developer
namespace MVCMovies.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Movie
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]   //max length 60 chars, min length 2
        public string Title { get; set; }

        [Display(Name = "Release Date")]        //used to make column names more readable
        [DataType(DataType.Date)]               //removes time from the date display
                                                //formats date 
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ReleaseDate { get; set; }

        [Required]
        [StringLength(30)]
        //regular expression used to check input - this one insists on input
        //starting with a capital letter and having letters, hyphens or spaces to follow
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s\-]*$", ErrorMessage = "Genre must begin with a capital letter and can only contain letters, hyphens and spaces")]
        public string Genre { get; set; }

        [Range(0, 1000)]                        //sets range of numerical value
        [DataType(DataType.Currency)]           //adds currency symbol and separators   
        public Nullable<decimal> Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
