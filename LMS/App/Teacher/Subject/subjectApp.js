(function () {
    var app = angular.module('subject', ['base']);

    app.config(function ($routeProvider, $locationProvider) {
    	$routeProvider
            .when('/', {
                templateUrl: LMS.rootPath + 'App/Teacher/Subject/Views/subjectIndexView.html',
                controller: "subjectCtrl"
            })
            .when('/Create', {
                templateUrl: LMS.rootPath + 'App/Teacher/Subject/Views/subjectCreateView.html',
                controller: "subjectCtrl"
            })
            .when('/Edit/:id', {
                templateUrl: LMS.rootPath + 'App/Teacher/Subject/Views/subjectEditView.html',
                controller: "editCtrl"
        });
    });

    app.controller('subjectCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
        getAllSubjects();

        function getAllSubjects() {
            dataService.get("Subject/List", function (data) {
                $scope.subjects = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };

        $scope.create = function () {
            var data = {
                name: $scope.name,
                description: $scope.description
            };
            function onResponse(resp) {
                $scope.message = resp.statusText;
            };
            dataService.post("Subject/Create", data, onResponse, onResponse);
        };

    }]);
    app.controller('editCtrl', ['$scope', '$routeParams', 'dataService', function ($scope, $routeParams, dataService) {
        getOneSubject();

        function getOneSubject() {
            var id = $routeParams["id"];
            dataService.get("Subject/GetSubject/" + id, function (data) {
                $scope.subject = data;
            }, function (resp) { // Om det blir fel
                $scope.message = resp.statusText;
            });
        };

        $scope.update = function () {
            var data = {
                id: $scope.subject.id,
                name: $scope.subject.name,
                description: $scope.subject.description
            };
            function onResponse(resp) {
                $scope.message = resp.statusText;
            };
            dataService.post("Subject/Update", data, onResponse, onResponse);
        };
    }]);

}());