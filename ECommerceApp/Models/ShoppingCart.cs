﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}