(function () {
    var app = angular.module('task');

    app.controller('taskIndexCtrl', ['$scope', '$location', '$window', function ($scope, $location, $window) {
        $scope.message = 'hello from angular';

        $scope.navigate = function (path) {
        	//$location.path(LMS.rootPath + path);
        	$window.location.href = LMS.rootPath  + path;
        };
    }]);
}());