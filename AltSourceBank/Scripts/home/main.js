var altsourceBank = angular.module("altsourceBank", []);

altsourceBank.controller("altsourceBankController", function altsourceBankController($scope, $http) {
	$scope.user = {
		email: "",
		password: "",
		key: "",
		balance: 0,
		transactions: []
	};
	$scope.alert = {
		failure: false
	};

	$scope.update = function () {
		var balance = 0;

		$http({
			method: "POST",
			url: "/User/Balance/",
			data: {
				key: $scope.user.key
			}
		}).then(function success(response) {
			$scope.user.balance = response.data;
		}, function failure(response) {
			console.log("Unable to retrieve balance");
		});

		$http({
			method: "POST",
			url: "/User/History/",
			data: {
				key: $scope.user.key
			}
		}).then(function success(response) {
			$scope.user.transactions = [];
			response.data.forEach(function (transaction) {
				balance += transaction;

				$scope.user.transactions.push({
					date: new Date(parseInt(transaction.Date.substr(6))),
					amount: transaction.Amount,
					balance: transaction.Balance
				});
			});
		}, function failure(response) {
			console.log("Unable to retrieve balance");
		});

		return;
	};

	$scope.clear = function () {
		$scope.user.balance = 0;
		$scope.user.transactions = [];
	}

	$scope.log_in = function () {
		var user = {
			email: $scope.user.email,
			password: $scope.user.password
		};

		console.log("Attemtping to log in as " + user.email);

		$http({
			method: "POST",
			url: "/User/Login/",
			data: user
		}).then(function success(response) {
			console.log("Logged in as " + user.email);
			$scope.user.name = response.data.Name;
			$scope.user.email = response.data.Email;
			$scope.user.password = "";
			$scope.user.key = response.data.Key;
			$scope.update();
			$scope.alert.failure = false;
		}, function failure(response) {
			console.log("Unable to log in as " + user.email);
			$scope.user.password = "";
			$scope.alert.failure = true;
		});

		return;
	};

	$scope.deposit = function () {
		$http({
			method: "POST",
			url: "/User/Deposit/",
			data: {
				key: $scope.user.key,
				amount: $scope.deposit_amount
			}
		}).then(function success(response) {
			console.log("Deposit: " + $scope.deposit_amount);
			$scope.user.balance += $scope.deposit_amount;
			$scope.user.transactions.push({
				amount: $scope.deposit_amount,
				date: new Date(),
				balance: $scope.user.balance
			});
		}, function failure(response) {
			console.log("Unable to deposit");
		});

		return;
	};

	$scope.withdrawal = function () {
		$http({
			method: "POST",
			url: "/User/Withdrawal/",
			data: {
				key: $scope.user.key,
				amount: $scope.withdrawal_amount
			}
		}).then(function success(response) {
			console.log("Withdrawal: " + $scope.withdrawal_amount);
			$scope.user.balance -= $scope.withdrawal_amount;
			$scope.user.transactions.push({
				amount: -1 * $scope.withdrawal_amount,
				date: new Date(),
				balance: $scope.user.balance
			});
		}, function failure(response) {
			console.log("Unable to withdraw");
		});

		return;
	};

	$scope.log_out = function () {
		var user = {
			key: $scope.user.key
		};

		$http({
			method: "POST",
			url: "/User/Logout/",
			data: user
		}).then(function success(response) {
			console.log("Logged out");
		}, function failure(response) {
			console.log("Logged out with failure");
		});

		$scope.user.key = "";

		return;
	};
});