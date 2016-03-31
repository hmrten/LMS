(function () {
    var app = angular.module('group', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
            	templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupIndexView.html'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupCreateView.html'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('groupCtrl', ['$scope', function ($scope) {
        $scope.message = 'hello from angular';
    }]);
}());