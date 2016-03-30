(function () {
    var app = angular.module('group', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
            	templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupIndexView.html',
            	controller: 'groupIndexCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Group/Views/groupCreateView.html',
                controller: 'groupCreateCtrl'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });
}());