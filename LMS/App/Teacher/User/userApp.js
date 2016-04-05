(function () {
    var app = angular.module('user', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/User/Views/userIndexView.html',
                controller: "userCtrl"
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/User/Views/userCreateView.html',
                controller: "userCtrl"
            })
            .when('/Edit/:id', {
                templateUrl: LMS.rootPath + 'App/Teacher/User/Views/userEditView.html',
                controller: "editCtrl"
        });

    });

    app.controller('userCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
        getAllUsers();

        function getAllUsers() {
            dataService.get("User/List", function (data) {
                $scope.groups = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };

        getAllRoles();
        function getAllRoles() {
            dataService.get("User/GetRoles", function (data) {
                $scope.roles = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };

        $scope.create = function () {
            //var data = {
            //    id: $scope.id,
            //    name: $scope.name
            //};
            var data = angular.toJson($scope.user);
            function onSuccess(resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Skapa lyckades!',
                    text: resp.statusText
                };
            };
            function onError(resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Skapa misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
            };
            dataService.post("User/Create", data, onSuccess, onError);
        };

    }]);
    app.controller('editCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
        getOneUser();

        function getOneUser() {
            var id = $routeParams["id"];
            dataService.get("User/GetUser/" + id, function (data) {
                $scope.user = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };

        $scope.update = function () {
            var data = angular.toJson($scope.user);
            function onResponse(resp) {
                $scope.message = resp.statusText;
            };
            dataService.post("User/Update", data, onResponse, onResponse);
        };
    }]);

}());