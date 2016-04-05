(function () {
    var app = angular.module('assignment', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Assignment/Views/assignmentIndexView.html',
                controller: 'indexCtrl'
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Assignment/Views/assignmentCreateView.html',
                controller: 'createCtrl'
            });
    });

    app.controller('createCtrl', ['$scope',  '$http', 'fileUpload', function ($scope, $http, fileUpload) {
        function init() {
            $http.get(LMS.rootPath + 'Data/Subjects').then(function (resp) {
                $scope.subjects = resp.data;
            });
        };

        $scope.create = function () {
            var data = {
                model: angular.toJson($scope.model),
                file: $scope.file
            };

            fileUpload.uploadFile(data, 'Assignment/Create').then(
                function (resp) {
                    $scope.msg = 'success: ' + resp.status + ' - ' + resp.statusText;
                },
                function (resp) {
                    $scope.msg = 'error: ' + resp.status + ' - ' + resp.statusText;
                });
        };

        init();
    }]);
}());