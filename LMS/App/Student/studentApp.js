(function () {
    var app = angular.module('student', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentIndexView.html',
                controller: 'studentCtrl'
            })
            .when('/Profile', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentProfileView.html',
                controller: 'profileCtrl'
            })
            .when('/Group', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentGroupView.html',
                controller: 'profileCtrl'
            })
            .when('/Shared', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentSharedView.html',
                controller: 'profileCtrl'
            })
            .when('/Task', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentTaskView.html',
                controller: 'profileCtrl'
            })
            .when('/Schedule', {
                templateUrl: LMS.rootPath + 'App/Teacher/Views/assignmentCreateView.html',
                controller: 'studentCtrl'
            });

    });

    app.controller('studentCtrl', ['$scope', function ($scope) {
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
    app.controller('profileCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
        $scope.message = 'hello from angular profile';
        $scope.user = {};
        $scope.formReady = false;

        getIdentity();

        function getIdentity() {
            dataService.get("User/StudentIdentity", function (data) {
                $scope.user.id = data.id;
                $scope.formReady = true;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };

        $scope.update = function () {
            var data = angular.toJson($scope.user);
            function onSuccess(resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Uppdateringen lyckades!',
                    text: resp.statusText
                };
            };
            function onError(resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Uppdateringen misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
            };
            dataService.post("User/UpdatePassword", data, onSuccess, onError);
        };
    }]);

}());