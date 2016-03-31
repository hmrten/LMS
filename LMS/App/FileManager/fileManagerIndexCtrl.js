(function () {
    var app = angular.module('fileManager');

    app.controller('fileManagerIndexCtrl', ['$scope', function ($scope) {
        $scope.dirs = ['shared', 'private'];
    }]);
}());