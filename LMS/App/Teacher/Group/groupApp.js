(function () {
    var app = angular.module('group', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupIndexView.html',
                controller: 'groupCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupCreateView.html',
                controller: 'groupCtrl'
            })
            .when('/Edit/:id', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupEditView.html',
                controller: 'editCtrl'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('groupCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
        $scope.msg = 'hello from angular';

        getGroups();

        function getGroups() {
            $http.get(LMS.rootPath + 'Group/List').then(function (resp) {
                $scope.groups = resp.data;
            });
        };
    }]);

    app.controller('editCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
        var id = parseInt($routeParams['id']);
        $scope.test = 'testing from getDetails';
        $http.get(LMS.rootPath + 'Group/Details/' + id).then(function (resp) {
            $scope.details = resp.data;
        });
    }]);
}());