(function () {
    var app = angular.module('task');

    app.controller('taskTestCtrl', ['$scope', function ($scope) {
        $scope.test = 'this is from the test controller';
    }]);
}());