(function () {
    var app = angular.module('task');

    app.controller('taskIndexCtrl', ['$scope', function ($scope) {
        $scope.menu = LMS.rootPath + 'App/Teacher/Task/Views/taskMenuView.html';
        $scope.message = 'hello from angular';
    }]);
}());