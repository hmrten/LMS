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
                controller: 'groupCtrl'
            })
            //.when('/Shared', {
            //    templateUrl: LMS.rootPath + 'App/Student/Views/studentSharedView.html',
            //    controller: 'studentCtrl'
            //})
            .when('/Task', {
                templateUrl: LMS.rootPath + 'App/Student/Views/studentTaskView.html',
                controller: 'taskCtrl'
            });
            //.when('/Schedule', {
            //    templateUrl: LMS.rootPath + 'App/Student/Views/studentIndexView.html',
            //    controller: 'studentCtrl'
            //});

    });

    app.controller('studentCtrl', ['$scope', function ($scope) {
        $scope.message = 'hello from angular student';
        function toDo () {}; //TO DO
    }]);

    app.controller('groupCtrl', ['$scope', 'dataService', function ($scope, dataService) {
        GetAllStudents();

        function GetAllStudents() {
            dataService.get("User/ListStudents", function (data) {
                //$scope.student.id = data.id;
                $scope.students = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };
    }]);

    app.controller('profileCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
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

    app.controller('taskCtrl', ['$scope', 'dataService', function ($scope, dataService) {
        $scope.message = 'hello from angular task';

        GetAllCurrentTasks();

        function GetAllCurrentTasks() {
            dataService.get("Submission/CurrentAssignments", function (data) {
                $scope.group = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        }; 
    }]);

}());