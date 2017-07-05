using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltSourceBank.Models {
	public class User {
		public string Name;
		public string Email;
		public string Password;
		public string Key;
		public List<Transaction> Transactions;

		public User() {
			Name = "";
			Email = "";
			Password = "";
			Key = "";
			Transactions = new List<Transaction>();
		}

		public static bool Create(string name, string email, string password) {
			bool result;
			User newUser;
			List<User> users;

			result = false;

			newUser = new User();
			newUser.Name = name;
			newUser.Email = email;
			newUser.Password = password;

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			if (!users.Any(u => u.Email == email)) {
				((List<User>) HttpContext.Current.Application["Users"]).Add(newUser);
				result = true;
			}
			HttpContext.Current.Application.UnLock();

			return result;
		}

		public static User Login(string email, string password) {
			int i;
			string key;
			User user;
			Random rng;
			List<User> users;

			rng = new Random();

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			try {
				user = users.Single(u => u.Email == email && u.Password == password);
				do {
					key = "";
					for (i = 0; i < 10; i++)
					{
						key += rng.Next(0, 9).ToString();
					}
				} while (users.Any(u => u.Key == key));
				user.Key = key;
			} catch {
				user = null;
			}
			HttpContext.Current.Application.UnLock();

			return user;
		}

		public static void Deposit(string key, double amount) {
			User user;
			Transaction transaction;
			List<User> users;

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			try {
				user = users.Single(u => u.Key == key);
			} catch {
				user = null;
			}
			HttpContext.Current.Application.UnLock();

			if (user != null) {
				transaction = new Transaction();
				transaction.Amount = amount;
				transaction.Date = DateTime.Now;
				transaction.Balance = User.Balance(key) + amount;
				user.Transactions.Add(transaction);
			}

			return;
		}

		public static void Withdrawal(string key, double amount) {
			User user;
			Transaction transaction;
			List<User> users;

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			try {
				user = users.Single(u => u.Key == key);
			} catch {
				user = null;
			}
			HttpContext.Current.Application.UnLock();

			if (user != null) {
				transaction = new Transaction();
				transaction.Amount = -1 * amount;
				transaction.Date = DateTime.Now;
				transaction.Balance = User.Balance(key) - amount;
				user.Transactions.Add(transaction);
			}

			return;
		}

		public static double Balance(string key) {
			double balance;
			User user;
			List<User> users;

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			try {
				user = users.Single(u => u.Key == key);
			} catch {
				user = null;
			}
			HttpContext.Current.Application.UnLock();

			balance = 0;

			if (user != null) {
				foreach (Transaction t in user.Transactions) {
					balance += t.Amount;
				}
			}

			return balance;
		}

		public static List<Transaction> History(string key) {
			User user;
			List<Transaction> history;
			List<User> users;

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			try {
				user = users.Single(u => u.Key == key);
			} catch {
				user = null;
			}
			HttpContext.Current.Application.UnLock();

			if (user != null) {
				history = user.Transactions;
			} else {
				history = new List<Transaction>();
			}

			return history;
		}

		public static void Logout(string key) {
			List<User> users;
			User user;

			HttpContext.Current.Application.Lock();
			users = (List<User>) HttpContext.Current.Application["Users"];
			try {
				user = users.Single(u => u.Key == key);
				user.Key = "";
			} catch {
				user = null;
			}
			HttpContext.Current.Application.UnLock();

			return;
		}
	}
}