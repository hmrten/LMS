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
                controller: "subjectCtrl"
        });

        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false
        //});
    });

    app.controller('subjectCtrl', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {

        getAllSubjects();

        function getAllSubjects() {
            $http.get(LMS.rootPath + "Subject/List").then(function (resp) {
                $scope.subjects = resp.data;
            }, function (resp) {
                $scope.message = resp.status + resp.statusText;
            });
        };

        getOneSubject();

        function getOneSubject() {
            var id = $routeParams["id"];
     
            $http.get(LMS.rootPath + "Subject/GetSubject/" + id).then(function(resp) {
                $scope.subject = resp.data;
            }, function (resp) {
                $scope.message = resp.status + resp.statusText;
            });
        };

        $scope.create = function () {
            var data = {
                name: $scope.name,
                description: $scope.description
            };
            $http.post(LMS.rootPath + "Subject/Create", data).then(function (resp) {
                $scope.message = resp.statusText;
            });
        };

        $scope.update = function () {
            var data = {
                id: $scope.subject.id,
                name: $scope.subject.name,
                description: $scope.subject.description
            };
            
            $http.post(LMS.rootPath + "Subject/Update", data).then(function (resp) {
                $scope.message = resp.statusText;
                console.log(resp);
            });
        };
    }]);

}());