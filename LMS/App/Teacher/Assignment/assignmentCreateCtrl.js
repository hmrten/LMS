(function () {
    var app = angular.module('assignment');

    app.controller('assignmentCreateCtrl', ['$scope', 'fileUpload', function ($scope, fileUpload) {
        $scope.test = 'this is from the Create controller';

        $scope.create = function () {
            var data = {
                title: $scope.title,
                description: $scope.description,
                file: $scope.file
            };

            fileUpload.uploadFile(data, 'FileManager/Upload').then(
                function (resp) {
                    $scope.msg = 'success: ' + resp.status + ' - ' + resp.statusText;
                },
                function (resp) {
                    $scope.msg = 'error: ' + resp.status + ' - ' + resp.statusText;
                });
        };
    }]);
}());