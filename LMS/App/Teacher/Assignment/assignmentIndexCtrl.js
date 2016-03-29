(function () {
    var app = angular.module('assignment');

    app.controller('assignmentIndexCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
        $scope.message = 'hello from angular';

        $scope.navigate = function (path) {
        	//$location.path(LMS.rootPath + path);
        	$window.location.href = LMS.rootPath  + path;
        };
    }]);
}());