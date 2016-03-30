(function () {
    var app = angular.module('group');

    app.controller('groupCreateCtrl', ['$scope', function ($scope) {
        $scope.test = 'this is from the Create controller';
    }]);
}());