namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        public string? CardName { get; } = default!;
        public string CardNumber { get; } = default!;
        public string Expiration { get; } = default!;
        public string CVV { get; } = default!;
        public string PaymentMethod { get; } = default!;

        protected Payment() { }

        private Payment(string? cardName, string cardNumber, string expiration, string cvv, string paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            CVV = cvv;
            PaymentMethod = paymentMethod;
        }

        public static Payment Of(string? cardName, string cardNumber, string expiration, string cvv, string paymentMethod)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(cardNumber, nameof(cardNumber));
            ArgumentException.ThrowIfNullOrWhiteSpace(expiration, nameof(expiration));
            ArgumentException.ThrowIfNullOrWhiteSpace(cvv, nameof(cvv));
            ArgumentException.ThrowIfNullOrWhiteSpace(paymentMethod, nameof(paymentMethod));
            return new(cardName, cardNumber, expiration, cvv, paymentMethod);
        }
    }
}
