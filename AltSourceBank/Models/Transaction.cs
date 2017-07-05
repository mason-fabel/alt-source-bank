using System;

namespace AltSourceBank.Models {
	public class Transaction {
		public double Amount;
		public double Balance;
		public DateTime Date;

		public Transaction() {
			Amount = 0.0;
			Balance = 0.0;
			Date = new DateTime();
		}
	}
}