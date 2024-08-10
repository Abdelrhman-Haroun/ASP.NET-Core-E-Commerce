﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Image")]
        [ValidateNever]
        public string Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Categories")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }


    }
}