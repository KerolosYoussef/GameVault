﻿namespace Discount.Grpc.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"ProductName: {ProductName}, Description: {Description}, Amount: {Amount}";
        }
    }
}
