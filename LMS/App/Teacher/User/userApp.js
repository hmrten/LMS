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
                controller: "userCtrl"
        });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('userCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {

        getAllusers();

        function getAllusers() {
            $http.get(LMS.rootPath + "user/List").then(function (resp) {
                $scope.users = resp.data;
            }, function (resp) {
                $scope.message = resp.status + resp.statusText;
            });
        };

        getOneuser();

        function getOneuser() {
            var id = $routeParams["id"];
     
            $http.get(LMS.rootPath + "user/Getuser/" + id).then(function(resp) {
                $scope.user = resp.data;
            }, function (resp) {
                $scope.message = resp.status + resp.statusText;
            });
        };

        $scope.create = function () {
            var data = {
                name: $scope.name,
                description: $scope.description
            };
            $http.post(LMS.rootPath + "user/Create", data).then(function (resp) {
                $scope.message = resp.statusText;
            });
        };

        $scope.update = function () {
            var data = {
                id: $scope.user.id,
                name: $scope.user.name,
                description: $scope.user.description
            };
            
            $http.post(LMS.rootPath + "user/Update", data).then(function (resp) {
                $scope.message = resp.statusText;
                console.log(resp);
            });
        };
    }]);

}());