var altsourceBank = angular.module("altsourceBank", []);

altsourceBank.controller("altsourceBankUserController", function altsourceBankUserController($scope, $http, $timeout, $window) {
	$scope.user = {
		name: "",
		email: "",
		password: "",
		password_confirm: "",
	};
	$scope.alert = {
		success: false,
		failure: false,
	};

	$scope.create_user = function () {
		var new_user;

		if ($scope.user && $scope.user.password == $scope.user.password_confirm) {
			new_user = {
				Name: $scope.user.name,
				Email: $scope.user.email,
				Password: $scope.user.password,
			};

			console.log("Creating user: " + new_user.Name);

			$http({
				method: "POST",
				url: "/User/Create/",
				data: new_user,
			}).then(function success(response) {
				console.log("User created successfully!");
				$scope.alert.success = true;
				$scope.alert.failure = false;
				$timeout(function () { $window.location.href = "/"; }, 1000);
			}, function failure(response) {
				console.log("Unable to create user: " + new_user.Name);
				$scope.alert.success = false;
				$scope.alert.failure = true;
			});
		}

		return;
	};
});