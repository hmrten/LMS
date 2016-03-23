(function () {
    var app = angular.module('task', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Task/Views/taskIndexView.html',
                controller: 'taskIndexCtrl'
            })
            .when('/test', {
                templateUrl: LMS.rootPath + 'App/Teacher/Task/Views/taskTestView.html',
                controller: 'taskTestCtrl'
            })
            .otherwise({
                redirectTo: '/'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });
}());