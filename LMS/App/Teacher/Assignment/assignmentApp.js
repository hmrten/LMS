(function () {
    var app = angular.module('assignment', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Assignment/Views/assignmentIndexView.html',
                controller: 'assignmentCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Assignment/Views/assignmentCreateView.html',
                controller: 'assignmentCtrl'
            });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('assignmentCtrl', ['$scope', function ($scope) {
        $scope.message = 'hello from angular';

        $scope.create = function () {
            var data = {
                title: $scope.title,
                description: $scope.description,
                file: $scope.file
            };

            fileUpload.uploadFile(data, 'FileManager/Upload').then(
                function (resp) {
                    $scope.msg = 'success: ' + resp.status + ' - ' + resp.statusText;
                },
                function (resp) {
                    $scope.msg = 'error: ' + resp.status + ' - ' + resp.statusText;
                });
        };
    }]);
}());