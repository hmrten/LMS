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

    app.controller('groupCtrl', ['$scope', 'dataService', function ($scope, dataService) {
        function getGroups() {
            dataService.getData('Group/List', function (data) {
                $scope.groups = data;
            });
        };

        $scope.msg = 'hello from angular';
        getGroups();
    }]);

    app.controller('editCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
        function getDetails() {
            var id = parseInt($routeParams['id']);
            dataService.getData('Group/Details/' + id, function (data) {
                $scope.details = data;
            });
        }

        $scope.clearList = function (id) {
            var opts = angular.element(id)[0];
            for (var i = 0; i < opts.length; ++i) {
                opts[i].selected = false;
            }
            $scope.msg = 'clearing: ' + id;
        };

        $scope.test = 'testing from getDetails';
        getDetails();
    }]);
}());