(function () {
    var app = angular.module('assignment', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
            	templateUrl: LMS.rootPath + 'App/Teacher/Assignment/Views/assignmentIndexView.html',
            	controller: 'assignmentIndexCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Assignment/Views/assignmentCreateView.html',
                controller: 'assignmentCreateCtrl'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });
}());