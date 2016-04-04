(function () {
    var app = angular.module('student', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentIndexView.html',
                controller: 'studentCtrl'
            });
            //.when('/Create', {
            //    templateUrl: LMS.rootPath + 'App/Teacher/Views/assignmentCreateView.html',
            //    controller: 'studentCtrl'
            //});

    });

    app.controller('studentstCtrl', ['$scope', function ($scope) {
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