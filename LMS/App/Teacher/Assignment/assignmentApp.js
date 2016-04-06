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

    function parseMSDate(s) {
        if (!s) return null;
        return new Date(parseInt(s.substr(6)));
    };

    app.filter('msDate', function () {
        return function (s) {
            return parseMSDate(s);
        };
    });
    app.filter('dayDiff', function () {
        return function (detail) {
            if (!detail) return 0;
            var date_start = parseMSDate(detail.date_start);
            var date_end = parseMSDate(detail.date_end);
            var dt = date_end - date_start;
            return Math.floor(dt / (1000 * 3600 * 24));
        };
    });

    app.controller('indexCtrl', function ($scope, $http, $animate, $timeout) {
        function getData() {
            $http.get(LMS.rootPath + 'Assignment/List').then(function (resp) {
                $scope.groups = resp.data;
            });
        };

        function refreshSubmissions() {
            var id = $scope.details.id;
            $http.get(LMS.rootPath + 'Assignment/Submissions/' + id).then(function (resp) {
                $scope.details.submissions = resp.data;
            });
        };

        $scope.toggle = function (subject) {
            subject.expanded = !subject.expanded;
        };

        $scope.showDetails = function (a) {
            var el = angular.element('.details');
            $animate.removeClass(el, 'show');
            $scope.details = a;
            $timeout(function () { $animate.addClass(el, 'show') }, 150);
        };

        $scope.showSubmissions = function () {
            $scope.selectedSub = null;
            $('#submissions').modal('show');
        };

        $scope.showGrading = function (sub) {
            var tr = angular.element('#sub-' + sub.id);
            tr.addClass('selected-row');
            $scope.form = {};
            $scope.selectedSub = sub;
        };

        $scope.gradeClass = function (sub) {
            if (sub.grading.id == null) return '';
            return sub.grading.grade ? 'glyphicon-star' : 'glyphicon-star-empty';
        };
        $scope.gradeString = function (sub) {
            if (sub.grading.id == null) return '';
            return sub.grading.grade ? 'Godkänd' : 'Ej godkänd';
        };

        $scope.grade = function () {
            $scope.msg = null;
            var sub = $scope.selectedSub;
            var form = $scope.form;
            form.sub_id = sub.id;
            var data = angular.toJson(form);
            $http.post(LMS.rootPath + 'Assignment/Grade', data).then(function (resp) {
                $scope.msg = {
                    type: 'success',
                    strong: 'Betygsättning lyckades!',
                    text: resp.statusText
                };
                refreshSubmissions();
            }, function (resp) {
                $scope.msg = {
                    type: 'danger',
                    strong: 'Betygsättning misslyckades!',
                    text: resp.status + ': ' + resp.statusText
                };
            });
        };

        getData();
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