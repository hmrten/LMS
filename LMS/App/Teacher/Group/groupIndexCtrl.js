(function () {
    var app = angular.module('group');

    app.controller('groupIndexCtrl', ['$scope', function ($scope) {
        $scope.message = 'hello from angular';
    }]);
}());